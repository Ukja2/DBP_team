using System;
using System.Data;
using System.Drawing;
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

        public MainForm()
        {
            InitializeComponent();
            HookTreeEvents();

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

            labelCompany.Text = string.IsNullOrWhiteSpace(_companyName) ? $"회사 ID:{_companyId}" : _companyName;
            labelName.Text = string.IsNullOrWhiteSpace(_userName) ? "사용자: (알 수 없음)" : $"사용자: {_userName}";

            LoadCompanyTree();
            LoadRecentChats();

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

                labelCompany.Text = string.IsNullOrWhiteSpace(_companyName) ? $"회사 ID:{_companyId}" : _companyName;
                labelName.Text = string.IsNullOrWhiteSpace(_userName) ? "사용자: (알 수 없음)" : $"사용자: {_userName}";
                LoadCompanyTree();
                LoadRecentChats();

                // start polling for new messages
                StartPolling();
            }
            else
            {
                labelCompany.Text = "회사 정보 없음";
                labelName.Text = "사용자 정보 없음";
            }
        }

        // 트리뷰 더블클릭 이벤트 훅
        private void HookTreeEvents()
        {
            treeViewUser.NodeMouseDoubleClick -= TreeViewUser_NodeMouseDoubleClick;
            treeViewUser.NodeMouseDoubleClick += TreeViewUser_NodeMouseDoubleClick;
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
                    var otherName = e.Node.Text;
                    // 로그인된 사용자 id(_userId) 가 있어야 함
                    if (_userId <= 0)
                    {
                        MessageBox.Show("로그인 사용자 정보가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var chat = new ChatForm(_userId, otherId, otherName);
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
                    treeViewUser.Nodes.Add(new TreeNode("회사 정보가 없습니다."));
                    return;
                }

                var dtDeps = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM departments WHERE company_id = @cid ORDER BY name",
                    new MySqlParameter("@cid", _companyId));

                if (dtDeps == null || dtDeps.Rows.Count == 0)
                {
                    treeViewUser.Nodes.Add(new TreeNode("등록된 부서가 없습니다."));
                    return;
                }

                foreach (DataRow dep in dtDeps.Rows)
                {
                    int depId = Convert.ToInt32(dep["id"]);
                    string depName = dep["name"]?.ToString() ?? $"부서 {depId}";
                    var depNode = new TreeNode(depName) { Tag = $"department:{depId}" };

                    var dtTeams = DBManager.Instance.ExecuteDataTable(
                        "SELECT id, name FROM teams WHERE department_id = @did ORDER BY name",
                        new MySqlParameter("@did", depId));

                    if (dtTeams != null && dtTeams.Rows.Count > 0)
                    {
                        foreach (DataRow team in dtTeams.Rows)
                        {
                            int teamId = Convert.ToInt32(team["id"]);
                            string teamName = team["name"]?.ToString() ?? $"팀 {teamId}";
                            var teamNode = new TreeNode(teamName) { Tag = $"team:{teamId}" };

                            var dtUsersInTeam = DBManager.Instance.ExecuteDataTable(
                                "SELECT id, full_name, email, role FROM users WHERE company_id = @cid AND department_id = @did AND team_id = @tid ORDER BY full_name",
                                new MySqlParameter("@cid", _companyId),
                                new MySqlParameter("@did", depId),
                                new MySqlParameter("@tid", teamId));

                            if (dtUsersInTeam != null && dtUsersInTeam.Rows.Count > 0)
                            {
                                foreach (DataRow u in dtUsersInTeam.Rows)
                                {
                                    int uid = Convert.ToInt32(u["id"]);
                                    // 로그인 사용자면 건너뜀
                                    if (uid == _userId) continue;

                                    var display = u["full_name"]?.ToString();
                                    if (string.IsNullOrWhiteSpace(display)) display = u["email"]?.ToString() ?? "이름 없음";
                                    var userNode = new TreeNode(display) { Tag = $"user:{uid}" };
                                    teamNode.Nodes.Add(userNode);
                                }
                            }

                            depNode.Nodes.Add(teamNode);
                        }
                    }

                    var dtUsersNoTeam = DBManager.Instance.ExecuteDataTable(
                        "SELECT id, full_name, email, role FROM users WHERE company_id = @cid AND department_id = @did AND (team_id IS NULL OR team_id = 0) ORDER BY full_name",
                        new MySqlParameter("@cid", _companyId),
                        new MySqlParameter("@did", depId));

                    if (dtUsersNoTeam != null && dtUsersNoTeam.Rows.Count > 0)
                    {
                        foreach (DataRow u in dtUsersNoTeam.Rows)
                        {
                            int uid = Convert.ToInt32(u["id"]);
                            // 로그인 사용자면 건너뜀
                            if (uid == _userId) continue;

                            var display = u["full_name"]?.ToString();
                            if (string.IsNullOrWhiteSpace(display)) display = u["email"]?.ToString() ?? "이름 없음";
                            var userNode = new TreeNode(display) { Tag = $"user:{uid}" };
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
        private void LoadRecentChats()
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
                    "LEFT JOIN chat c ON ((c.sender_id = @me AND c.receiver_id = u.id) OR (c.sender_id = u.id && c.receiver_id = @me)) AND c.created_at = c2.last_at " +
                    "ORDER BY c2.last_at DESC",
                    new MySqlParameter("@me", _userId));

                if (dt == null || dt.Rows.Count == 0) return;

                foreach (DataRow r in dt.Rows)
                {
                    var uid = Convert.ToInt32(r["user_id"]);
                    var name = r["name"]?.ToString() ?? "(이름 없음)";
                    var msg = r.Table.Columns.Contains("message") ? r["message"]?.ToString() : string.Empty;
                    var time = r.Table.Columns.Contains("created_at") && r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : (DateTime?)null;

                    var timeText = time.HasValue ? FormatRelativeTime(time.Value) : string.Empty;
                    var lvi = new ListViewItem(new[] { name, msg ?? string.Empty, timeText }) { Tag = uid };
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

            var otherName = lvi.Text;
            if (_userId <= 0) { MessageBox.Show("로그인 사용자 정보가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            var chat = new ChatForm(_userId, otherId, otherName);
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
                        var senderName = r["sender_name"]?.ToString() ?? "(알수없음)";
                        var message = r["message"]?.ToString() ?? "";
                        var created = r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now;
                        if (created > newest) newest = created;

                        // 짧게 메시지 표시 (최대 80자)
                        var shortMsg = message.Length > 80 ? message.Substring(0, 77) + "..." : message;
                        // Show balloon (non-modal)
                        try
                        {
                            _notifyIcon.BalloonTipTitle = $"새 메시지: {senderName}";
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
    }
}
