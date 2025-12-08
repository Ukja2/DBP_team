using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Models;
using System.Text;
using System.ComponentModel;
using System.Data;
using DBP_team.UI;

namespace DBP_team
{
    public partial class AdminForm : Form
    {
        private readonly User _me;
        private readonly int _companyId;
        private int? _selectedPermissionUserId = null;
        private bool _tvUpdating = false; // prevent recursive AfterCheck loops

        public AdminForm() : this(new User { Id = 0, CompanyId = 0, FullName = "관리자" }) { }

        public AdminForm(User me)
        {
            _me = me ?? new User { Id = 0, CompanyId = 0, FullName = "관리자" };
            _companyId = _me.CompanyId ?? 0;

            InitializeComponent();

            bool isDesignTime = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (!isDesignTime)
            {
                if (!AdminGuard.IsAdmin(_me))
                {
                    MessageBox.Show("관리자만 접근 가능합니다.", "권한 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Load += (s, e) => this.Close();
                    return;
                }
                this.Load -= AdminForm_Load;
                this.Load += AdminForm_Load;
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                if (_dtFrom != null) _dtFrom.Value = DateTime.Now.Date.AddDays(-7);
                if (_dtTo != null) _dtTo.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

                try
                {
                    EnsureUserViewPermissionTableExists();

                    LoadDeptGrid();
                    LoadDeptComboForUser();
                    LoadUsersGrid();
                    LoadUserFilterCombo();
                    SearchAccessLogs();
                    InitializePermissionTree();
                    InitializeChatBanUI();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("초기화 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EnsureUserViewPermissionTableExists()
        {
            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS user_view_permission (" +
                    " id INT AUTO_INCREMENT PRIMARY KEY, " +
                    " viewer_user_id INT NOT NULL, " +
                    " dept_id INT NULL, " +
                    " group_code VARCHAR(50) NULL, " +
                    " created_at DATETIME DEFAULT CURRENT_TIMESTAMP, " +
                    " INDEX idx_viewer(viewer_user_id) " +
                    ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");

                try
                {
                    DBManager.Instance.ExecuteNonQuery(
                        "ALTER TABLE user_view_permission ADD UNIQUE KEY uq_viewer_target (viewer_user_id, dept_id, group_code)");
                }
                catch { }

                try
                {
                    DBManager.Instance.ExecuteNonQuery(
                        "ALTER TABLE user_view_permission MODIFY COLUMN group_code VARCHAR(100) NULL");
                }
                catch { }
            }
            catch
            {
            }
        }

        private void DeptAdd_Click(object sender, EventArgs e) => AddDepartment();
        private void DeptUpdate_Click(object sender, EventArgs e) => UpdateDepartment();
        private void DeptSearch_Click(object sender, EventArgs e) => LoadDeptGrid(_txtDeptSearch.Text?.Trim());
        private void DeptGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) _txtDeptName.Text = Convert.ToString(_gridDept.Rows[e.RowIndex].Cells["name"].Value);
        }
        private void UserSearch_Click(object sender, EventArgs e) => LoadUsersGrid(_txtUserSearch.Text?.Trim());
        private void ApplyDept_Click(object sender, EventArgs e) => ApplyUserDepartment();
        private void ChatSearch_Click(object sender, EventArgs e) => LoadChatGrid();
        private void SearchLog_Click(object sender, EventArgs e) => SearchAccessLogs();
        private void GridChat_CellClick(object sender, DataGridViewCellEventArgs e) { }
        private void GridLogs_CellClick(object sender, DataGridViewCellEventArgs e) { }

        private void LoadDeptGrid(string keyword = null)
        {
            var sql = "SELECT id, name FROM departments WHERE company_id = @cid " +
                      (string.IsNullOrWhiteSpace(keyword) ? string.Empty : "AND name LIKE @kw ") +
                      "ORDER BY name";
            MySqlParameter[] pars = string.IsNullOrWhiteSpace(keyword)
                ? new[] { new MySqlParameter("@cid", _companyId) }
                : new[] { new MySqlParameter("@cid", _companyId), new MySqlParameter("@kw", "%" + keyword + "%") };
            var dt = DBManager.Instance.ExecuteDataTable(sql, pars);
            _gridDept.DataSource = dt;
            if (_gridDept.Columns.Contains("id")) _gridDept.Columns["id"].Visible = false;
            if (_gridDept.Columns.Contains("name")) _gridDept.Columns["name"].HeaderText = "부서명";
        }

        private void AddDepartment()
        {
            try
            {
                using (var dlg = new InputDialog("부서 추가", string.Empty))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                    var name = dlg.ResultText?.Trim();
                    if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("부서명을 입력하세요."); return; }
                    DBManager.Instance.ExecuteNonQuery(
                        "INSERT INTO departments (company_id, name) VALUES (@cid, @name)",
                        new MySqlParameter("@cid", _companyId), new MySqlParameter("@name", name));
                    LoadDeptGrid();
                    LoadDeptComboForUser();
                    InitializePermissionTree();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("부서 추가 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDepartment()
        {
            if (_gridDept.CurrentRow == null) { MessageBox.Show("수정할 부서를 선택하세요."); return; }
            var drv = _gridDept.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) { MessageBox.Show("선택 행 데이터를 읽을 수 없습니다."); return; }
            var id = Convert.ToInt32(drv["id"]);
            var name = _txtDeptName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("부서명을 입력하세요."); return; }
            DBManager.Instance.ExecuteNonQuery(
                "UPDATE departments SET name = @name WHERE id = @id AND company_id = @cid",
                new MySqlParameter("@name", name), new MySqlParameter("@id", id), new MySqlParameter("@cid", _companyId));
            LoadDeptGrid();
            LoadDeptComboForUser();
        }

        private void LoadDeptComboForUser()
        {
            var dt = DBManager.Instance.ExecuteDataTable(
                "SELECT id, name FROM departments WHERE company_id = @cid ORDER BY name",
                new MySqlParameter("@cid", _companyId));
            _cboDeptForUser.DataSource = dt;
            _cboDeptForUser.DisplayMember = "name";
            _cboDeptForUser.ValueMember = "id";
        }

        private void LoadUsersGrid(string keyword = null)
        {
            DataTable dt;

            if (AdminGuard.IsAdmin(_me))
            {
                var sql = "SELECT u.id, COALESCE(u.full_name, u.email) AS name, u.email, " +
                          "u.department_id, d.name AS department " +
                          "FROM users u " +
                          "LEFT JOIN departments d ON d.id = u.department_id " +
                          "WHERE u.company_id = @cid";

                var pars = new System.Collections.Generic.List<MySqlParameter>
        {
            new MySqlParameter("@cid", _companyId)
        };

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    sql += " AND (u.full_name LIKE @kw OR u.email LIKE @kw)";
                    pars.Add(new MySqlParameter("@kw", "%" + keyword + "%"));
                }

                sql += " ORDER BY name";

                dt = DBManager.Instance.ExecuteDataTable(sql, pars.ToArray());
            }
            else
            {
                dt = Models.EmployeePermissionService.LoadVisibleEmployees(
                    viewerId: _me.Id,
                    companyId: _companyId,
                    keyword: keyword
                );
            }

            _gridUsers.DataSource = dt;

            if (_gridUsers.Columns.Contains("id"))
                _gridUsers.Columns["id"].Visible = false;

            if (_gridUsers.Columns.Contains("department_id"))
                _gridUsers.Columns["department_id"].Visible = false;

            if (_gridUsers.Columns.Contains("name"))
                _gridUsers.Columns["name"].HeaderText = "이름";

            if (_gridUsers.Columns.Contains("email"))
                _gridUsers.Columns["email"].HeaderText = "이메일";

            if (_gridUsers.Columns.Contains("department"))
                _gridUsers.Columns["department"].HeaderText = "부서";
        }

        private void ApplyUserDepartment()
        {
            if (_gridUsers.SelectedRows.Count == 0) { MessageBox.Show("사용자를 선택하세요."); return; }
            if (!int.TryParse(Convert.ToString(_cboDeptForUser.SelectedValue), out int deptId)) { MessageBox.Show("부서를 선택하세요."); return; }

            foreach (DataGridViewRow row in _gridUsers.SelectedRows)
            {
                var drv = row.DataBoundItem as DataRowView;
                if (drv == null) continue;
                int uid = Convert.ToInt32(drv["id"]);
                DBManager.Instance.ExecuteNonQuery(
                    "UPDATE users SET department_id = @did WHERE id = @uid AND company_id = @cid",
                    new MySqlParameter("@did", deptId), new MySqlParameter("@uid", uid), new MySqlParameter("@cid", _companyId));
            }

            LoadUsersGrid(_txtUserSearch.Text?.Trim());
        }

        private void LoadUserFilterCombo()
        {
            var dt = DBManager.Instance.ExecuteDataTable(
                "SELECT id, COALESCE(full_name,email) AS name FROM users WHERE company_id=@cid ORDER BY name",
                new MySqlParameter("@cid", _companyId));
            _cboUserFilter.DataSource = dt;
            _cboUserFilter.DisplayMember = "name";
            _cboUserFilter.ValueMember = "id";
            _cboUserFilter.SelectedIndex = -1;
        }

        private void LoadChatGrid()
        {
            var from = _dtFrom.Value;
            var to = _dtTo.Value;
            string kw = _txtKeyword.Text?.Trim();
            int? userId = _cboUserFilter.SelectedIndex >= 0 ? (int?)Convert.ToInt32(_cboUserFilter.SelectedValue) : null;

            var sql = new StringBuilder();
            sql.Append("SELECT s.full_name AS sender, r.full_name AS receiver, c.message AS message, c.created_at AS created_at ");
            sql.Append("FROM chat c ");
            sql.Append("LEFT JOIN users s ON s.id = c.sender_id ");
            sql.Append("LEFT JOIN users r ON r.id = c.receiver_id ");
            sql.Append("WHERE c.created_at BETWEEN @from AND @to ");
            sql.Append("AND (s.company_id = @cid OR r.company_id = @cid) ");
            var pars = new System.Collections.Generic.List<MySqlParameter> {
                new MySqlParameter("@from", from), new MySqlParameter("@to", to), new MySqlParameter("@cid", _companyId)
            };
            if (!string.IsNullOrWhiteSpace(kw)) { sql.Append("AND c.message LIKE @kw "); pars.Add(new MySqlParameter("@kw", "%" + kw + "%")); }
            if (userId.HasValue) { sql.Append("AND (c.sender_id = @uid OR c.receiver_id = @uid) "); pars.Add(new MySqlParameter("@uid", userId.Value)); }
            sql.Append("ORDER BY c.created_at DESC");

            var dt = DBManager.Instance.ExecuteDataTable(sql.ToString(), pars.ToArray());
            _gridChat.DataSource = dt;
            if (_gridChat.Columns.Contains("sender")) _gridChat.Columns["sender"].HeaderText = "보낸사람";
            if (_gridChat.Columns.Contains("receiver")) _gridChat.Columns["receiver"].HeaderText = "받는사람";
            if (_gridChat.Columns.Contains("message")) _gridChat.Columns["message"].HeaderText = "내용";
            if (_gridChat.Columns.Contains("created_at")) _gridChat.Columns["created_at"].HeaderText = "시간";
        }

        private void SearchAccessLogs()
        {
            if (_gridLogs == null) return;

            DataTable GetAccessLogs(DateTime start, DateTime end, string keyword)
            {
                string sql = @"
                    SELECT l.created_at, u.full_name, u.email, l.activity_type, l.user_id
                    FROM user_activity_logs l
                    JOIN users u ON l.user_id = u.id
                    WHERE l.created_at >= @start_date AND l.created_at < @end_date
                      AND u.company_id = @company_id";

                var parameters = new System.Collections.Generic.List<MySqlParameter>
                {
                    new MySqlParameter("@start_date", start),
                    new MySqlParameter("@end_date", end),
                    new MySqlParameter("@company_id", _companyId)
                };

                if (!string.IsNullOrEmpty(keyword))
                {
                    sql += " AND (u.full_name LIKE @keyword OR u.email LIKE @keyword OR CAST(u.id AS CHAR) LIKE @keyword)";
                    parameters.Add(new MySqlParameter("@keyword", $"%{keyword}%"));
                }

                sql += " ORDER BY l.created_at DESC";

                return DBManager.Instance.ExecuteDataTable(sql, parameters.ToArray());
            }

            try
            {
                DateTime startDate = dtpStart.Value.Date;
                DateTime endDate = dtpEnd.Value.Date.AddDays(1);
                string searchKeyword = txtSearchUser.Text.Trim();

                var dt = GetAccessLogs(startDate, endDate, searchKeyword);

                _gridLogs.DataSource = dt;

                if (_gridLogs.Columns.Contains("created_at"))
                {
                    _gridLogs.Columns["created_at"].HeaderText = "시간";
                    _gridLogs.Columns["created_at"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                }
                if (_gridLogs.Columns.Contains("full_name")) _gridLogs.Columns["full_name"].HeaderText = "사용자명";
                if (_gridLogs.Columns.Contains("activity_type")) _gridLogs.Columns["activity_type"].HeaderText = "활동";
                if (_gridLogs.Columns.Contains("user_id")) _gridLogs.Columns["user_id"].HeaderText = "사용자 ID";

                if (_gridLogs.Columns.Contains("email")) _gridLogs.Columns["email"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그 검색 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TeamAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new TeamAddDialog(_companyId))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                    int deptId = dlg.SelectedDepartmentId;
                    var teamName = dlg.TeamName?.Trim();
                    if (deptId <= 0 || string.IsNullOrWhiteSpace(teamName))
                    {
                        MessageBox.Show("부서와 팀 이름을 입력하세요.");
                        return;
                    }

                    DBManager.Instance.ExecuteNonQuery(
                        "INSERT INTO teams (department_id, name) VALUES (@did, @name)",
                        new MySqlParameter("@did", deptId),
                        new MySqlParameter("@name", teamName));

                    LoadDeptGrid();
                    LoadUsersGrid(_txtUserSearch.Text?.Trim());
                    InitializePermissionTree();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("팀 추가 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializePermissionTree()
        {
            try
            {
                // 사용자 목록: team_id 포함
                var usersDt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, COALESCE(full_name, email) AS name, department_id, team_id FROM users WHERE company_id=@cid ORDER BY name",
                    new MySqlParameter("@cid", _companyId));

                _cboViewer.DataSource = usersDt.Copy();
                _cboViewer.DisplayMember = "name";
                _cboViewer.ValueMember = "id";
                _cboViewer.SelectedIndex = -1;

                // 부서/팀 로드
                var deptDt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM departments WHERE company_id=@cid ORDER BY name",
                    new MySqlParameter("@cid", _companyId));

                var teamDt = DBManager.Instance.ExecuteDataTable(
                    "SELECT t.id, t.name, t.department_id FROM teams t JOIN departments d ON d.id=t.department_id WHERE d.company_id=@cid ORDER BY t.name",
                    new MySqlParameter("@cid", _companyId));

                var teamsByDept = teamDt?.AsEnumerable()
                    .GroupBy(r => r.Field<int>("department_id"))
                    .ToDictionary(g => g.Key, g => g.ToList())
                    ?? new Dictionary<int, List<DataRow>>();

                // Use Tuple<int,int> as key to avoid anonymous type mismatches
                var usersByDeptTeam = usersDt?.AsEnumerable()
                    .GroupBy(r => Tuple.Create(r.Field<int?>("department_id") ?? 0, r.Field<int?>("team_id") ?? 0))
                    .ToDictionary(g => g.Key, g => g.ToList())
                    ?? new Dictionary<Tuple<int, int>, List<DataRow>>();

                _tvVisibility.BeginUpdate();
                _tvVisibility.Nodes.Clear();

                // (부서 없음) 루트에 (팀 없음) 노드 + 직원
                var rootNoDept = new TreeNode("(부서 없음)") { Tag = new PermissionTag { DeptId = 0 } };
                var noDeptNoTeamKey = Tuple.Create(0, 0);
                if (usersByDeptTeam.TryGetValue(noDeptNoTeamKey, out var usersNoDept))
                {
                    var teamNoneNode = new TreeNode("(팀 없음)") { Tag = new PermissionTag { DeptId = 0, TeamId = 0 } };
                    foreach (var ur in usersNoDept)
                    {
                        var uid = Convert.ToInt32(ur["id"]);
                        var uname = Convert.ToString(ur["name"]);
                        var userNode = new TreeNode(uname) { Tag = new PermissionTag { DeptId = 0, TeamId = 0, UserId = uid } };
                        teamNoneNode.Nodes.Add(userNode);
                    }
                    rootNoDept.Nodes.Add(teamNoneNode);
                }
                _tvVisibility.Nodes.Add(rootNoDept);

                // 각 부서
                if (deptDt != null)
                {
                    foreach (DataRow d in deptDt.Rows)
                    {
                        var deptId = Convert.ToInt32(d["id"]);
                        var deptName = Convert.ToString(d["name"]);
                        var deptNode = new TreeNode(deptName) { Tag = new PermissionTag { DeptId = deptId } };

                        // 팀 없음 그룹
                        var noTeamKey = Tuple.Create(deptId, 0);
                        if (usersByDeptTeam.TryGetValue(noTeamKey, out var usersNoTeam))
                        {
                            var teamNoneNode = new TreeNode("(팀 없음)") { Tag = new PermissionTag { DeptId = deptId, TeamId = 0 } };
                            foreach (var ur in usersNoTeam)
                            {
                                var uid = Convert.ToInt32(ur["id"]);
                                var uname = Convert.ToString(ur["name"]);
                                var userNode = new TreeNode(uname) { Tag = new PermissionTag { DeptId = deptId, TeamId = 0, UserId = uid } };
                                teamNoneNode.Nodes.Add(userNode);
                            }
                            deptNode.Nodes.Add(teamNoneNode);
                        }

                        // 팀 노드 및 팀별 직원
                        if (teamsByDept.TryGetValue(deptId, out var trows))
                        {
                            foreach (var tr in trows)
                            {
                                var teamId = Convert.ToInt32(tr["id"]);
                                var teamName = Convert.ToString(tr["name"]);
                                var teamNode = new TreeNode(teamName) { Tag = new PermissionTag { DeptId = deptId, TeamId = teamId } };

                                var key = Tuple.Create(deptId, teamId);
                                if (usersByDeptTeam.TryGetValue(key, out var usersInTeam))
                                {
                                    foreach (var ur in usersInTeam)
                                    {
                                        var uid = Convert.ToInt32(ur["id"]);
                                        var uname = Convert.ToString(ur["name"]);
                                        var userNode = new TreeNode(uname) { Tag = new PermissionTag { DeptId = deptId, TeamId = teamId, UserId = uid } };
                                        teamNode.Nodes.Add(userNode);
                                    }
                                }

                                deptNode.Nodes.Add(teamNode);
                            }
                        }

                        _tvVisibility.Nodes.Add(deptNode);
                    }
                }

                _tvVisibility.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("권한 설정 초기화 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class PermissionTag
        {
            public int DeptId { get; set; }
            public int? TeamId { get; set; }
            public int? UserId { get; set; }
        }

        private void cboViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_cboViewer.SelectedIndex < 0) return;
                object sel = _cboViewer.SelectedValue;
                if (sel == null) return;
                int viewerId;
                if (sel is DataRowView drv && drv.Row.Table.Columns.Contains("id"))
                    viewerId = Convert.ToInt32(drv["id"]);
                else if (sel is IConvertible)
                    viewerId = Convert.ToInt32(sel);
                else
                {
                    int.TryParse(Convert.ToString(sel), out viewerId);
                }
                if (viewerId <= 0) return;
                _selectedPermissionUserId = viewerId;

                var permDt = DBManager.Instance.ExecuteDataTable(
                    "SELECT dept_id, group_code FROM user_view_permission WHERE viewer_user_id=@uid",
                    new MySqlParameter("@uid", _selectedPermissionUserId));

                var allowedDepts = new HashSet<int>();
                var allowedTeams = new HashSet<string>(); // deptId:teamId
                var allowedUsers = new HashSet<string>(); // deptId:teamId:userId
                foreach (DataRow r in permDt.Rows)
                {
                    var did = r["dept_id"] != DBNull.Value ? Convert.ToInt32(r["dept_id"]) : 0;
                    var code = Convert.ToString(r["group_code"]);
                    if (string.IsNullOrEmpty(code))
                    {
                        allowedDepts.Add(did);
                    }
                    else if (code.StartsWith("TEAM:"))
                    {
                        if (int.TryParse(code.Substring(5), out int tid))
                            allowedTeams.Add(did + ":" + tid);
                    }
                    else if (code.StartsWith("USER:"))
                    {
                        if (int.TryParse(code.Substring(5), out int uid))
                            allowedUsers.Add(did + ":" + 0 + ":" + uid); // 팀 없음은 0, 실제 팀은 후처리에서 모두 비교
                    }
                }

                _tvUpdating = true;
                foreach (TreeNode deptNode in _tvVisibility.Nodes)
                {
                    var dtag = deptNode.Tag as PermissionTag;
                    if (dtag == null) continue;
                    bool deptAllowed = allowedDepts.Contains(dtag.DeptId);
                    deptNode.Checked = deptAllowed;

                    foreach (TreeNode teamNode in deptNode.Nodes)
                    {
                        var ttag = teamNode.Tag as PermissionTag;
                        if (ttag == null) continue;

                        // 팀 노드 또는 (팀 없음) 노드
                        bool teamAllowed = ttag.TeamId.HasValue && allowedTeams.Contains(ttag.DeptId + ":" + (ttag.TeamId ?? 0));
                        teamNode.Checked = teamAllowed || deptAllowed;

                        foreach (TreeNode userNode in teamNode.Nodes)
                        {
                            var utag = userNode.Tag as PermissionTag;
                            if (utag == null) continue;
                            string keyUser = utag.DeptId + ":" + (utag.TeamId ?? 0) + ":" + (utag.UserId ?? 0);
                            bool userAllowed = allowedUsers.Contains(keyUser);
                            userNode.Checked = userAllowed || teamAllowed || deptAllowed;
                        }
                    }
                }
                _tvUpdating = false;
            }
            catch (Exception ex)
            {
                _tvUpdating = false;
                MessageBox.Show("사용자 권한 로드 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tvVisibility_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_tvUpdating) return;
            if (_selectedPermissionUserId == null) return;

            try
            {
                var tag = e.Node.Tag as PermissionTag;
                if (tag == null) return;

                if (e.Node.Parent == null)
                {
                    // 부서 노드 저장
                    if (e.Node.Checked)
                    {
                        DBManager.Instance.ExecuteNonQuery(
                            "INSERT IGNORE INTO user_view_permission (viewer_user_id, dept_id) VALUES (@u, @d)",
                            new MySqlParameter("@u", _selectedPermissionUserId),
                            new MySqlParameter("@d", tag.DeptId));
                    }
                    else
                    {
                        DBManager.Instance.ExecuteNonQuery(
                            "DELETE FROM user_view_permission WHERE viewer_user_id=@u AND dept_id=@d",
                            new MySqlParameter("@u", _selectedPermissionUserId),
                            new MySqlParameter("@d", tag.DeptId));
                    }
                }
                else
                {
                    var parentTag = e.Node.Parent.Tag as PermissionTag;
                    int deptId = parentTag?.DeptId ?? tag.DeptId;

                    if (tag.UserId.HasValue)
                    {
                        var code = "USER:" + tag.UserId.Value;
                        if (e.Node.Checked)
                        {
                            DBManager.Instance.ExecuteNonQuery(
                                "INSERT IGNORE INTO user_view_permission (viewer_user_id, dept_id, group_code) VALUES (@u, @d, @g)",
                                new MySqlParameter("@u", _selectedPermissionUserId),
                                new MySqlParameter("@d", deptId),
                                new MySqlParameter("@g", code));
                        }
                        else
                        {
                            DBManager.Instance.ExecuteNonQuery(
                                "DELETE FROM user_view_permission WHERE viewer_user_id=@u AND dept_id=@d AND group_code=@g",
                                new MySqlParameter("@u", _selectedPermissionUserId),
                                new MySqlParameter("@d", deptId),
                                new MySqlParameter("@g", code));
                        }
                    }
                    else if (tag.TeamId.HasValue)
                    {
                        var code = "TEAM:" + (tag.TeamId ?? 0);
                        if (e.Node.Checked)
                        {
                            DBManager.Instance.ExecuteNonQuery(
                                "INSERT IGNORE INTO user_view_permission (viewer_user_id, dept_id, group_code) VALUES (@u, @d, @g)",
                                new MySqlParameter("@u", _selectedPermissionUserId),
                                new MySqlParameter("@d", deptId),
                                new MySqlParameter("@g", code));
                        }
                        else
                        {
                            DBManager.Instance.ExecuteNonQuery(
                                "DELETE FROM user_view_permission WHERE viewer_user_id=@u AND dept_id=@d AND group_code=@g",
                                new MySqlParameter("@u", _selectedPermissionUserId),
                                new MySqlParameter("@d", deptId),
                                new MySqlParameter("@g", code));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("권한 저장 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPermSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("변경사항은 즉시 저장됩니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPermReset_Click(object sender, EventArgs e)
        {
            try
            {
                _cboViewer.SelectedIndex = -1;
                _tvVisibility.Nodes.Clear();
                InitializePermissionTree();
            }
            catch { }
        }

        // Ensure stub exists to satisfy designer or callers
        private void InitializeChatBanUI()
        {
            try
            {
                // 간단한 초기화: 사용자 콤보 로드 및 리스트뷰 컬럼 준비
                var usersDt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, COALESCE(full_name, email) AS name FROM users WHERE company_id=@cid ORDER BY name",
                    new MySqlParameter("@cid", _companyId));

                _cbUser1.DataSource = usersDt.Copy();
                _cbUser1.DisplayMember = "name";
                _cbUser1.ValueMember = "id";

                _cbUser2.DataSource = usersDt;
                _cbUser2.DisplayMember = "name";
                _cbUser2.ValueMember = "id";

                // bans 목록은 필요 시 DB에서 로드하도록 남겨둠
            }
            catch { }
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            try
            {
                if (_cbUser1.SelectedValue == null || _cbUser2.SelectedValue == null)
                {
                    MessageBox.Show("사용자 A/B를 선택하세요.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var a = Convert.ToInt32(_cbUser1.SelectedValue);
                var b = Convert.ToInt32(_cbUser2.SelectedValue);
                if (a == b)
                {
                    MessageBox.Show("동일 사용자끼리는 차단할 수 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // 차단 테이블이 있다면 여기에 반영. 없으면 UI에만 표시.
                var item = new ListViewItem(new[] { _cbUser1.Text, _cbUser2.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                _lvBans.Items.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show("차단 처리 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUnblock_Click(object sender, EventArgs e)
        {
            try
            {
                if (_lvBans.SelectedItems.Count == 0)
                {
                    MessageBox.Show("해제할 항목을 선택하세요.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (ListViewItem it in _lvBans.SelectedItems)
                {
                    _lvBans.Items.Remove(it);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("차단 해제 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}