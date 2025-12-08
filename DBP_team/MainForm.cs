using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Models;

namespace DBP_team
{
    public partial class MainForm : Form
    {
        private int _companyId;
        private string _companyName;
        private string _userName;
        private bool _initializedFromCtor;
        private int _userId; // 로그인된 사용자 id

        // Polling fields
        private System.Windows.Forms.Timer _pollTimer;
        private DateTime _lastPollTime;
        private NotifyIcon _notifyIcon;

        // status image list for online/offline
        private ImageList _statusImages;
        private const int IMG_OFFLINE = 0;
        private const int IMG_ONLINE = 1;
        private const int IMG_WHITE = 2;

        public MainForm()
        {
            InitializeComponent();
            HookTreeEvents();

            // initialize status images
            InitStatusImages();

            // Self chat 버튼 이벤트 연결 (디자이너에 btnSelfChat이 있어야 합니다)
            try
            {
                this.btnSelfChat.Click -= btnSelfChat_Click;
                this.btnSelfChat.Click += btnSelfChat_Click;
            }
            catch
            {
                // btnSelfChat이 없으면 무시 (디자이너 이름 확인)
            }

            // 프로필 버튼 이벤트 이미 디자이너에 연결되어 있을 수 있으므로 안전하게 연결
            try
            {
                this.btnProfile.Click -= button1_Click;
                this.btnProfile.Click += button1_Click;
            }
            catch
            {
                // 무시
            }

            // ensure cleanup
            this.FormClosed -= MainForm_FormClosed;
            this.FormClosed += MainForm_FormClosed;
        }

        private void InitStatusImages()
        {
            try
            {
                _statusImages = new ImageList();
                _statusImages.ColorDepth = ColorDepth.Depth32Bit;
                _statusImages.ImageSize = new Size(12, 12);
                // gray circle
                _statusImages.Images.Add(CreateCircleBitmap(Color.Gray, 12)); // index 0
                // green circle
                _statusImages.Images.Add(CreateCircleBitmap(Color.Green, 12)); // index 1
                // white circle for non-user nodes
                _statusImages.Images.Add(CreateCircleBitmap(Color.White, 12)); // index 2
                treeViewUser.ImageList = _statusImages;
            }
            catch { /* ignore */ }
        }

        private static Bitmap CreateCircleBitmap(Color color, int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (var b = new SolidBrush(color))
                {
                    g.FillEllipse(b, 0, 0, size - 1, size - 1);
                }
                using (var p = new Pen(Color.FromArgb(100, 0, 0, 0)))
                {
                    g.DrawEllipse(p, 0, 0, size - 1, size - 1);
                }
            }
            return bmp;
        }

        // User 객체로 초기화
        public MainForm(User user) : this()
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            _userId = user.Id;
            _companyId = user.CompanyId ?? 0;
            _userName = string.IsNullOrWhiteSpace(user.FullName) ? user.Email : user.FullName;
            _companyName = user.CompanyName; // 로그인 시 전달된 companyName 우선 사용

            // DB에서 회사명을 가져와야 하면 여기서 보완: 전달된 값이 비어있으면 DB에서 조회
            if (string.IsNullOrWhiteSpace(_companyName) && _companyId > 0)
            {
                try
                {
                    var obj = DBManager.Instance.ExecuteScalar(
                        "SELECT name FROM companies WHERE id = @id",
                        new MySqlParameter("@id", _companyId));
                    if (obj != null && obj != DBNull.Value)
                        _companyName = obj.ToString();
                }
                catch
                {
                    // 실패하면 _companyName은 null로 두고 아래에서 폴백 텍스트 사용
                }
            }

            _initializedFromCtor = true;

            UpdateSelfHeaderDisplay();

            LoadCompanyTree();
            LoadRecentChats();
            LoadFavorites(); // 즐겨찾기 로드 호출 추가

            // start polling for new incoming messages
            StartPolling();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_initializedFromCtor) return;

            if (_companyId > 0)
            {
                // 런타임 상황에 따라 회사명을 DB에서 보완 조회
                if (string.IsNullOrWhiteSpace(_companyName))
                {
                    try
                    {
                        var obj = DBManager.Instance.ExecuteScalar(
                            "SELECT name FROM companies WHERE id = @id",
                            new MySqlParameter("@id", _companyId));
                        if (obj != null && obj != DBNull.Value)
                            _companyName = obj.ToString();
                    }
                    catch
                    {
                        // 무시
                    }
                }

                UpdateSelfHeaderDisplay();
                LoadCompanyTree();
                LoadRecentChats();
                LoadFavorites(); // 즐겨찾기 로드 호출 추가

                // start polling for new messages
                StartPolling();
            }
            else
            {
                labelCompany.Text = "회사 정보 없음";
                labelName.Text = "사용자 정보 없음";
            }
        }

        private void UpdateSelfHeaderDisplay()
        {
            // 내가 보는 내 이름은 기본 full_name/email 유지 (원하면 MultiProfileService 사용 가능) -> 현재 유지
            labelCompany.Text = string.IsNullOrWhiteSpace(_companyName) ? $"회사 ID:{_companyId}" : _companyName;
            labelName.Text = string.IsNullOrWhiteSpace(_userName) ? "사용자: (알 수 없음)" : $"사용자: {_userName}";
        }

        // 트리뷰 더블클릭 이벤트 훅
        private void HookTreeEvents()
        {
            treeViewUser.NodeMouseDoubleClick -= TreeViewUser_NodeMouseDoubleClick;
            treeViewUser.NodeMouseDoubleClick += TreeViewUser_NodeMouseDoubleClick;
        }

        private bool IsUserOnline(int userId)
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT activity_type FROM user_activity_logs WHERE user_id = @uid ORDER BY created_at DESC LIMIT 1",
                    new MySqlParameter("@uid", userId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    var act = dt.Rows[0]["activity_type"]?.ToString();
                    return string.Equals(act, "LOGIN", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch { }
            return false;
        }

        private int GetStatusImageIndex(bool isOnline)
        {
            return isOnline ? IMG_ONLINE : IMG_OFFLINE;
        }

        // 트리 노드 더블클릭 핸들러: user:ID 태그를 파싱해 ChatForm 열기
        private void TreeViewUser_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;
            var tag = e.Node.Tag as string;
            if (string.IsNullOrWhiteSpace(tag)) return;

            // tag 형식: "user:123"
            if (tag.StartsWith("user:", StringComparison.OrdinalIgnoreCase))
            {
                var parts = tag.Split(':');
                if (parts.Length == 2 && int.TryParse(parts[1], out int otherId))
                {
                    // 상대가 나에게 어떻게 보일지를 가져온다 (owner=other, viewer=me)
                    var otherDisplay = MultiProfileService.GetDisplayNameForViewer(otherId, _userId);
                    if (string.IsNullOrWhiteSpace(otherDisplay)) otherDisplay = e.Node.Text;

                    // 로그인된 사용자 id(_userId) 가 있어야 함
                    if (_userId <= 0)
                    {
                        MessageBox.Show("로그인 사용자 정보가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // ban check
                    if (ChatBanDAO.IsChatBanned(_userId, otherId))
                    {
                        MessageBox.Show("관리자 정책에 의해 대화가 제한된 사용자입니다.");
                        return;
                    }

                    var chat = new ChatForm(_userId, otherId, otherDisplay);
                    chat.StartPosition = FormStartPosition.CenterParent;
                    chat.Show(this);
                }
            }
        }

        // btnSelfChat 클릭 핸들러: 본인과의 채팅창 열기
        private void btnSelfChat_Click(object sender, EventArgs e)
        {
            if (_userId <= 0)
            {
                MessageBox.Show("로그인 사용자 정보가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var displayName = string.IsNullOrWhiteSpace(_userName) ? "나" : _userName;
            var chat = new ChatForm(_userId, _userId, displayName);
            chat.StartPosition = FormStartPosition.CenterParent;
            chat.Show(this);
        }

        // 프로필 버튼 클릭: 프로필 폼 열기
        private void button1_Click(object sender, EventArgs e)
        {
            if (_userId <= 0)
            {
                MessageBox.Show("로그인 사용자 정보가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var pf = new ProfileForm(_userId);
            pf.StartPosition = FormStartPosition.CenterParent;
            pf.ShowDialog(this);
            // 프로필 변경 후 트리/최근 목록 새로고침하여 반영
            LoadCompanyTree();
            LoadRecentChats();
            LoadFavorites(); // 즐겨찾기 목록도 새로고침
        }

        // 기존 LoadCompanyTree() 메서드 수정: 로그인 사용자는 노드에 추가하지 않음
        private void LoadCompanyTree()
        {
            treeViewUser.BeginUpdate();
            treeViewUser.Nodes.Clear();

            try
            {
                if (_companyId <= 0)
                {
                    treeViewUser.Nodes.Add(new TreeNode("회사 정보가 없습니다.") { ImageIndex = IMG_WHITE, SelectedImageIndex = IMG_WHITE });
                    return;
                }

                var dtDeps = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM departments WHERE company_id = @cid ORDER BY name",
                    new MySqlParameter("@cid", _companyId));

                if (dtDeps == null || dtDeps.Rows.Count == 0)
                {
                    treeViewUser.Nodes.Add(new TreeNode("등록된 부서가 없습니다.") { ImageIndex = IMG_WHITE, SelectedImageIndex = IMG_WHITE });
                    return;
                }

                // 권한 서비스로 현재 로그인 사용자가 볼 수 있는 직원 전체 목록을 한 번만 조회
                var dtVisible = Models.EmployeePermissionService.LoadVisibleEmployees(_userId, _companyId);

                foreach (DataRow dep in dtDeps.Rows)
                {
                    int depId = Convert.ToInt32(dep["id"]);
                    string depName = dep["name"]?.ToString() ?? $"부서 {depId}";
                    var depNode = new TreeNode(depName) { Tag = $"department:{depId}", ImageIndex = IMG_WHITE, SelectedImageIndex = IMG_WHITE };

                    // 팀 목록
                    var dtTeams = DBManager.Instance.ExecuteDataTable(
                        "SELECT id, name FROM teams WHERE department_id = @did ORDER BY name",
                        new MySqlParameter("@did", depId));

                    if (dtTeams != null && dtTeams.Rows.Count > 0)
                    {
                        foreach (DataRow team in dtTeams.Rows)
                        {
                            int teamId = Convert.ToInt32(team["id"]);
                            string teamName = team["name"]?.ToString() ?? $"팀 {teamId}";
                            var teamNode = new TreeNode(teamName) { Tag = $"team:{teamId}", ImageIndex = IMG_WHITE, SelectedImageIndex = IMG_WHITE };

                            // dtVisible에서 해당 부서+팀 사용자만 추가
                            if (dtVisible != null && dtVisible.Rows.Count > 0)
                            {
                                string expr = $"department_id = {depId} AND team_id = {teamId}";
                                DataRow[] rows = dtVisible.Select(expr);
                                foreach (var u in rows)
                                {
                                    int uid = Convert.ToInt32(u["id"]);
                                    if (uid == _userId) continue;
                                    var baseDisplay = u["name"]?.ToString();
                                    if (string.IsNullOrWhiteSpace(baseDisplay)) baseDisplay = u["email"]?.ToString() ?? "이름 없음";
                                    var mpDisplay = MultiProfileService.GetDisplayNameForViewer(uid, _userId);
                                    if (!string.IsNullOrWhiteSpace(mpDisplay)) baseDisplay = mpDisplay;
                                    bool online = IsUserOnline(uid);
                                    var userNode = new TreeNode(baseDisplay)
                                    {
                                        Tag = $"user:{uid}",
                                        ImageIndex = GetStatusImageIndex(online),
                                        SelectedImageIndex = GetStatusImageIndex(online)
                                    };
                                    teamNode.Nodes.Add(userNode);
                                }
                            }

                            depNode.Nodes.Add(teamNode);
                        }
                    }

                    // 팀에 속하지 않은 사용자들 (team_id IS NULL OR 0)
                    if (dtVisible != null && dtVisible.Rows.Count > 0)
                    {
                        string exprNoTeam = $"department_id = {depId} AND (team_id IS NULL OR team_id = 0)";
                        DataRow[] noTeamRows = dtVisible.Select(exprNoTeam);
                        foreach (var u in noTeamRows)
                        {
                            int uid = Convert.ToInt32(u["id"]);
                            if (uid == _userId) continue;
                            var baseDisplay = u["name"]?.ToString();
                            if (string.IsNullOrWhiteSpace(baseDisplay)) baseDisplay = u["email"]?.ToString() ?? "이름 없음";
                            var mpDisplay = MultiProfileService.GetDisplayNameForViewer(uid, _userId);
                            if (!string.IsNullOrWhiteSpace(mpDisplay)) baseDisplay = mpDisplay;
                            bool online = IsUserOnline(uid);
                            var userNode = new TreeNode(baseDisplay)
                            {
                                Tag = $"user:{uid}",
                                ImageIndex = GetStatusImageIndex(online),
                                SelectedImageIndex = GetStatusImageIndex(online)
                            };
                            depNode.Nodes.Add(userNode);
                        }
                    }

                    treeViewUser.Nodes.Add(depNode);
                }

                treeViewUser.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("트리 로드 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                treeViewUser.EndUpdate();
            }
        }

        // Load recent chats (only users that have chat rows with current user), ordered by last message desc
        public void LoadRecentChats()
        {
            try
            {
                listViewRecent.Items.Clear();
                if (_userId <= 0) return;

                // Subquery: get other user id and last message time per conversation
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT u.id AS user_id, COALESCE(u.full_name, u.email) AS name, c.message, c.created_at " +
                    "FROM users u " +
                    "INNER JOIN (" +
                    "  SELECT CASE WHEN sender_id = @me THEN receiver_id ELSE sender_id END AS other_id, MAX(created_at) AS last_at " +
                    "  FROM chat WHERE sender_id = @me OR receiver_id = @me GROUP BY other_id" +
                    ") c2 ON u.id = c2.other_id " +
                    "LEFT JOIN chat c ON ((c.sender_id = @me && c.receiver_id = u.id) OR (c.sender_id = u.id && c.receiver_id = @me)) AND c.created_at = c2.last_at " +
                    "ORDER BY c2.last_at DESC",
                    new MySqlParameter("@me", _userId));

                if (dt == null || dt.Rows.Count == 0) return;

                foreach (DataRow r in dt.Rows)
                {
                    var uid = Convert.ToInt32(r["user_id"]);
                    var nameBase = r["name"]?.ToString() ?? "(이름 없음)";
                    // 멀티프로필 적용 (상대가 나에게 보여줄 이름)
                    var mpName = MultiProfileService.GetDisplayNameForViewer(uid, _userId);
                    if (!string.IsNullOrWhiteSpace(mpName)) nameBase = mpName;
                    var msg = r.Table.Columns.Contains("message") ? r["message"]?.ToString() : string.Empty;
                    var time = r.Table.Columns.Contains("created_at") && r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : (DateTime?)null;

                    var timeText = time.HasValue ? FormatRelativeTime(time.Value) : string.Empty;
                    var lvi = new ListViewItem(new[] { nameBase, msg ?? string.Empty, timeText }) { Tag = uid };
                    listViewRecent.Items.Add(lvi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("최근 대화 불러오기 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewRecent_DoubleClick(object sender, EventArgs e)
        {
            if (listViewRecent.SelectedItems.Count == 0) return;
            var lvi = listViewRecent.SelectedItems[0];
            if (!(lvi.Tag is int otherId)) return;

            var otherDisplay = MultiProfileService.GetDisplayNameForViewer(otherId, _userId);
            if (string.IsNullOrWhiteSpace(otherDisplay)) otherDisplay = lvi.Text;
            // ban check
            if (ChatBanDAO.IsChatBanned(_userId, otherId))
            {
                MessageBox.Show("관리자 정책에 의해 대화가 제한된 사용자입니다.");
                return;
            }

            var chat = new ChatForm(_userId, otherId, otherDisplay);
            chat.StartPosition = FormStartPosition.CenterParent;
            chat.Show(this);
        }

        // Polling: start timer and notify icon
        private void StartPolling()
        {
            try
            {
                // initialize last poll time to now so we only get new messages
                _lastPollTime = DateTime.Now;

                if (_pollTimer == null)
                {
                    _pollTimer = new System.Windows.Forms.Timer();
                    _pollTimer.Interval = 3000; // 3초
                    _pollTimer.Tick += PollTimer_Tick;
                }

                if (_notifyIcon == null)
                {
                    _notifyIcon = new NotifyIcon();
                    _notifyIcon.Icon = SystemIcons.Application;
                    _notifyIcon.Visible = true;
                    _notifyIcon.BalloonTipTitle = "새 메시지";
                    _notifyIcon.BalloonTipClicked += (s, e) =>
                    {
                        // 사용자가 풍선을 클릭하면 최근 리스트 갱신
                        LoadRecentChats();
                    };
                }

                _pollTimer.Start();
            }
            catch
            {
                // 폴링 초기화 실패 무시
            }
        }

        private async void PollTimer_Tick(object sender, EventArgs e)
        {
            // 비동기 DB 호출: 새로 온 나에게 향한 메시지(created_at > _lastPollTime)
            try
            {
                if (_userId <= 0) return;

                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT c.id, c.sender_id, COALESCE(u.full_name,u.email) AS sender_name, c.message, c.created_at " +
                    "FROM chat c JOIN users u ON u.id = c.sender_id " +
                    "WHERE c.receiver_id = @me AND c.created_at > @last ORDER BY c.created_at ASC",
                    new MySqlParameter("@me", _userId),
                    new MySqlParameter("@last", _lastPollTime));

                if (dt != null && dt.Rows.Count > 0)
                {
                    DateTime newest = _lastPollTime;
                    foreach (DataRow r in dt.Rows)
                    {
                        var senderId = Convert.ToInt32(r["sender_id"]);
                        var baseName = r["sender_name"]?.ToString() ?? "(알수없음)";
                        var mpName = MultiProfileService.GetDisplayNameForViewer(senderId, _userId);
                        if (!string.IsNullOrWhiteSpace(mpName)) baseName = mpName;

                        var message = r["message"]?.ToString() ?? "";
                        var created = r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now;
                        if (created > newest) newest = created;

                        // 짧게 메시지 표시 (최대 80자)
                        var shortMsg = message.Length > 80 ? message.Substring(0, 77) + "..." : message;
                        // Show balloon (non-modal)
                        try
                        {
                            _notifyIcon.BalloonTipTitle = $"새 메시지: {baseName}";
                            _notifyIcon.BalloonTipText = shortMsg;
                            _notifyIcon.ShowBalloonTip(4000);
                        }
                        catch { }
                    }

                    // 최신 시간 갱신
                    _lastPollTime = newest;

                    // 새 메시지 탐지 시 최근 리스트 갱신 (UI thread)
                    try
                    {
                        if (!this.IsDisposed && this.IsHandleCreated)
                            this.BeginInvoke((Action)(() => LoadRecentChats()));
                    }
                    catch { }
                }
            }
            catch
            {
                // 폴링 중 오류는 무시(로그 필요 시 추가)
            }
        }

        // Stop polling and dispose notify icon on close
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _pollTimer?.Stop();
                _pollTimer?.Dispose();
            }
            catch { }

            try
            {
                if (_notifyIcon != null)
                {
                    _notifyIcon.Visible = false;
                    _notifyIcon.Dispose();
                }
            }
            catch { }
        }

        // returns relative time string in Korean like "방금 전", "5분 전", "2시간 전"
        private static string FormatRelativeTime(DateTime dt)
        {
            var span = DateTime.Now - dt;
            if (span.TotalSeconds < 60)
                return "방금 전";
            if (span.TotalMinutes < 60)
            {
                var m = (int)Math.Floor(span.TotalMinutes);
                return $"{m}분 전";
            }
            if (span.TotalHours < 24)
            {
                var h = (int)Math.Floor(span.TotalHours);
                return $"{h}시간 전";
            }
            if (span.TotalDays < 30)
            {
                var d = (int)Math.Floor(span.TotalDays);
                return $"{d}일 전";
            }
            if (span.TotalDays < 365)
            {
                var months = (int)Math.Floor(span.TotalDays / 30);
                return $"{months}개월 전";
            }
            var years = (int)Math.Floor(span.TotalDays / 365);
            return $"{years}년 전";
        }

        // Open an additional Login form instance (allow multiple logins)
        private void btnOpenLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var login = new Loginform();
                // Allow multiple independent login windows
                login.StartPosition = FormStartPosition.CenterParent;
                login.Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그인 창을 열 수 없습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewRecent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // --- 로그아웃 로그 기록 추가 ---
            DBManager.LogUserActivity(_userId, "LOGOUT");
            // --------------------------

            AppSession.CurrentUser = null;

            Properties.Settings.Default.AutoLogin = false;
            Properties.Settings.Default.Save();

            var login = new Loginform();
            login.StartPosition = FormStartPosition.CenterScreen;
            login.Show();

            this.Close();
        }

        private void LoadFavorites()
        {
            if (lvFavorites == null) return;
            lvFavorites.Items.Clear();
            if (_userId <= 0) return;

            const string sql = @"
                SELECT u.id AS user_id, COALESCE(u.full_name, u.email) AS display_name, d.name AS department_name
                FROM users u
                INNER JOIN favorites f ON f.target_id = u.id
                LEFT JOIN departments d ON d.id = u.department_id
                WHERE f.user_id = @userId
                ORDER BY display_name;";

            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@userId", _userId));
                if (dt == null) return;
                foreach (DataRow row in dt.Rows)
                {
                    int userId = Convert.ToInt32(row["user_id"]);
                    string name = row["display_name"]?.ToString() ?? string.Empty;
                    string dept = row["department_name"]?.ToString() ?? string.Empty;

                    // Apply MultiProfile display if available
                    var mpName = MultiProfileService.GetDisplayNameForViewer(userId, _userId);
                    if (!string.IsNullOrWhiteSpace(mpName)) name = mpName;

                    var item = new ListViewItem(name) { Tag = userId };
                    item.ToolTipText = dept;
                    lvFavorites.Items.Add(item);
                }

                // 즐겨찾기가 비어있을 때 안내 텍스트 추가
                if (lvFavorites.Items.Count == 0)
                {
                    var emptyItem = new ListViewItem("즐겨찾기가 비어있습니다")
                    {
                        ForeColor = Color.FromArgb(150, 150, 150),
                        Font = new Font("맑은 고딕", 9F, FontStyle.Italic),
                        Tag = null // 클릭 방지용
                    };
                    lvFavorites.Items.Add(emptyItem);

                    var hintItem = new ListViewItem("부서 목록에서 직원을 우클릭하여 추가하세요")
                    {
                        ForeColor = Color.FromArgb(150, 150, 150),
                        Font = new Font("맑은 고딕", 8F, FontStyle.Italic),
                        Tag = null
                    };
                    lvFavorites.Items.Add(hintItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("즐겨찾기 로드 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FavoriteSelectedDepartmentNode(object sender, EventArgs e)
        {
            var node = treeViewUser.SelectedNode;
            if (node == null || node.Tag == null) return;

            // Expect tag like "user:123" or an int id. Only users can be favorited.
            if (!TryGetUserIdFromNodeTag(node.Tag, out int targetUserId))
            {
                MessageBox.Show("사용자만 즐겨찾기에 추가할 수 있습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_userId <= 0) return;

            const string sql = @"
                INSERT INTO favorites (user_id, target_id)
                VALUES (@userId, @targetId)
                ON DUPLICATE KEY UPDATE target_id = target_id;";

            try
            {
                DBManager.Instance.ExecuteNonQuery(sql,
                    new MySqlParameter("@userId", _userId),
                    new MySqlParameter("@targetId", targetUserId));
                LoadFavorites();
            }
            catch (Exception ex)
            {
                MessageBox.Show("즐겨찾기 추가 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UnfavoriteSelected(object sender, EventArgs e)
        {
            if (lvFavorites == null || lvFavorites.SelectedItems.Count == 0) return;
            var item = lvFavorites.SelectedItems[0];
            if (!(item.Tag is int targetUserId)) return;
            if (_userId <= 0) return;

            const string sql = @"DELETE FROM favorites WHERE user_id = @userId AND target_id = @targetId";
            try
            {
                DBManager.Instance.ExecuteNonQuery(sql,
                    new MySqlParameter("@userId", _userId),
                    new MySqlParameter("@targetId", targetUserId));
                LoadFavorites();
            }
            catch (Exception ex)
            {
                MessageBox.Show("즐겨찾기 해제 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenChatForSelectedFavorite(object sender, EventArgs e)
        {
            if (lvFavorites == null || lvFavorites.SelectedItems.Count == 0) return;
            var item = lvFavorites.SelectedItems[0];
            if (!(item.Tag is int targetUserId)) return;

            var displayName = MultiProfileService.GetDisplayNameForViewer(targetUserId, _userId);
            if (string.IsNullOrWhiteSpace(displayName)) displayName = item.Text;

            // ban check
            if (ChatBanDAO.IsChatBanned(_userId, targetUserId))
            {
                MessageBox.Show("관리자 정책에 의해 대화가 제한된 사용자입니다.");
                return;
            }

            var chat = new ChatForm(_userId, targetUserId, displayName);
            chat.StartPosition = FormStartPosition.CenterParent;
            chat.Show(this);
        }

        private bool TryGetUserIdFromNodeTag(object tag, out int userId)
        {
            // Accept formats: int, "user:123", "123"
            userId = 0;
            if (tag is int i)
            {
                userId = i; return true;
            }
            if (tag is string s)
            {
                // designer code sets like "user:123"; support plain numeric too
                if (s.StartsWith("user:", StringComparison.OrdinalIgnoreCase))
                {
                    var parts = s.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out userId)) return true;
                }
                else
                {
                    return int.TryParse(s, out userId);
                }
            }
            return false;
        }

        // --- 검색 로직 및 이벤트 핸들러 추가 ---

        private void SearchUsers()
        {
            // txtSearch 컨트롤이 디자이너에 추가되어 있어야 합니다.
            if (this.Controls.Find("txtSearch", true).FirstOrDefault() is TextBox txtSearch)
            {
                string keyword = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    LoadCompanyTree(); // 키워드가 없으면 전체 조직도 로드
                    return;
                }

                treeViewUser.BeginUpdate();
                treeViewUser.Nodes.Clear();

                var searchRootNode = new TreeNode($"'{keyword}' 검색 결과");
                treeViewUser.Nodes.Add(searchRootNode);

                // --- SQL 쿼리 수정 ---
                const string sql = @"
                    SELECT u.id, u.full_name, u.email, d.name AS department_name
                    FROM users u
                    LEFT JOIN departments d ON u.department_id = d.id
                    WHERE CAST(u.id AS CHAR) LIKE @keyword_wildcard
                       OR u.full_name LIKE @keyword_wildcard
                       OR u.email LIKE @keyword_wildcard
                       OR d.name LIKE @keyword_wildcard
                    ORDER BY d.name, u.full_name;";

                try
                {
                    var dt = DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@keyword_wildcard", $"%{keyword}%"));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            int uid = Convert.ToInt32(row["id"]);
                            if (uid == _userId) continue;

                            string baseDisplay = row["full_name"]?.ToString() ?? row["email"]?.ToString() ?? "이름 없음";
                            var mpDisplay = MultiProfileService.GetDisplayNameForViewer(uid, _userId);
                            if (!string.IsNullOrWhiteSpace(mpDisplay)) baseDisplay = mpDisplay;

                            string deptName = row["department_name"]?.ToString();
                            string nodeText = string.IsNullOrEmpty(deptName) ? baseDisplay : $"{baseDisplay} ({deptName})";

                            bool online = IsUserOnline(uid);
                            var userNode = new TreeNode(nodeText)
                            {
                                Tag = $"user:{uid}",
                                ImageIndex = GetStatusImageIndex(online),
                                SelectedImageIndex = GetStatusImageIndex(online)
                            };
                            searchRootNode.Nodes.Add(userNode);
                        }
                    }
                    else
                    {
                        searchRootNode.Nodes.Add(new TreeNode("검색 결과가 없습니다."));
                    }
                    searchRootNode.Expand();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("검색 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    treeViewUser.EndUpdate();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchUsers();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // 클릭 소리 방지
                SearchUsers();
            }
        }

        private void lvFavorites_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
