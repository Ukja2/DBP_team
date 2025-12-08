using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Models;
using System.Text;
using System.ComponentModel;

namespace DBP_team
{
    public partial class AdminForm : Form
    {
        private readonly User _me;
        private readonly int _companyId;
        private int? _selectedPermissionUserId = null;

        // Chat ban controls are declared in Designer.cs to be designer-friendly

        // Designer-friendly parameterless constructor
        public AdminForm() : this(new User { Id = 0, CompanyId = 0, FullName = "관리자" })
        {
        }

        public AdminForm(User me)
        {
            _me = me ?? new User { Id = 0, CompanyId = 0, FullName = "관리자" };
            _companyId = _me.CompanyId ?? 0;

            InitializeComponent();

            // Detect design-time reliably
            bool isDesignTime = LicenseManager.UsageMode == LicenseUsageMode.Designtime;

            if (!isDesignTime)
            {
                if (!AdminGuard.IsAdmin(_me))
                {
                    MessageBox.Show("관리자만 접근 가능합니다.", "권한 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Load += (s, e) => this.Close(); // 폼 로드 후 바로 닫기
                    return;
                }

                // Runtime UI injection for chat ban management will be done on Load
                this.Load -= AdminForm_Load;
                this.Load += AdminForm_Load;
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                // DateTimePicker 초기값 설정 (디자인 타임 접근 방지)
                if (_dtFrom != null) _dtFrom.Value = DateTime.Now.Date.AddDays(-7);
                if (_dtTo != null) _dtTo.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

                LoadDeptGrid();
                LoadDeptComboForUser();
                LoadUsersGrid();
                LoadUserFilterCombo();
                SearchAccessLogs();
                InitPermissionTab();
                SearchAccessLogs(); // 초기 접속 로그 로드

                // Runtime UI injection for chat ban management
                InitializeChatBanUI();
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
        private void GridChat_CellClick(object sender, DataGridViewCellEventArgs e) { /* 필요한 경우 구현 */ }
        private void GridLogs_CellClick(object sender, DataGridViewCellEventArgs e) { /* 필요한 경우 구현 */ }

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
            var name = _txtDeptName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("부서명을 입력하세요."); return; }
            DBManager.Instance.ExecuteNonQuery(
                "INSERT INTO departments (company_id, name) VALUES (@cid, @name)",
                new MySqlParameter("@cid", _companyId), new MySqlParameter("@name", name));
            LoadDeptGrid();
            LoadDeptComboForUser();
        }

        private void UpdateDepartment()
        {
            if (_gridDept.CurrentRow == null) { MessageBox.Show("수정할 부서를 선택하세요."); return; }
            var id = Convert.ToInt32((_gridDept.CurrentRow.DataBoundItem as DataRowView)["id"]);
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

        // 직원(유저) 목록 로드 + G 기능 권한 적용
        private void LoadUsersGrid(string keyword = null)
        {
            DataTable dt;

            // 1. 관리자(Admin)는 항상 회사 전체 직원 보이게 (권한 제한 없음)
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
                // 2. 일반 사용자: G 기능 권한 로직 적용
                // EmployeePermissionService에서 user_view_permission 기반으로 필터링됨
                dt = EmployeePermissionService.LoadVisibleEmployees(
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
        private void InitPermissionTab()
        {
            // 디자이너에서 권한 탭 관련 컨트롤이 추가되어 있지 않으면 아무 작업하지 않음
            if (dgvUsers == null || clbDepartments == null || lblSelectedUser == null
                || chkAllEmployees == null || btnSavePermission == null || btnResetPermission == null)
                return;

            // 이벤트 중복 등록 방지: 먼저 제거 후 등록
            chkAllEmployees.CheckedChanged -= chkAllEmployees_CheckedChanged;
            chkAllEmployees.CheckedChanged += chkAllEmployees_CheckedChanged;

            btnSavePermission.Click -= btnSavePermission_Click;
            btnSavePermission.Click += btnSavePermission_Click;

            btnResetPermission.Click -= btnResetPermission_Click;
            btnResetPermission.Click += btnResetPermission_Click;

            // 그리드 동작 설정
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.AutoGenerateColumns = true;
            dgvUsers.MultiSelect = false;

            // 초기 데이터 로드
            LoadPermissionUserGrid();
            LoadPermissionDepartments();

            // UI 초기화
            ResetPermissionUI();
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0) return;

            var row = dgvUsers.Rows[e.RowIndex];
            var drv = row.DataBoundItem as DataRowView;
            if (drv == null) return;

            _selectedPermissionUserId = Convert.ToInt32(drv["id"]);

            if (lblSelectedUser != null)
                lblSelectedUser.Text = $"선택된 사용자: {drv["name"]} (ID={_selectedPermissionUserId})";

            LoadUserPermission(_selectedPermissionUserId.Value);
        }
        //왼
        private void LoadPermissionUserGrid(string keyword = null)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT u.id, COALESCE(u.full_name, u.email) AS name, u.email, ");
            sql.Append("u.department_id, d.name AS department ");
            sql.Append("FROM users u ");
            sql.Append("LEFT JOIN departments d ON d.id = u.department_id ");
            sql.Append("WHERE u.company_id = @cid ");

            var pars = new System.Collections.Generic.List<MySqlParameter>
    {
        new MySqlParameter("@cid", _companyId)
    };

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql.Append("AND (u.full_name LIKE @kw OR u.email LIKE @kw) ");
                pars.Add(new MySqlParameter("@kw", "%" + keyword + "%"));
            }

            sql.Append("ORDER BY name");

            var dt = DBManager.Instance.ExecuteDataTable(sql.ToString(), pars.ToArray());
            dgvUsers.DataSource = dt;

            if (dgvUsers.Columns.Contains("id")) dgvUsers.Columns["id"].Visible = false;
            if (dgvUsers.Columns.Contains("department_id")) dgvUsers.Columns["department_id"].Visible = false;
            if (dgvUsers.Columns.Contains("name")) dgvUsers.Columns["name"].HeaderText = "이름";
            if (dgvUsers.Columns.Contains("email")) dgvUsers.Columns["email"].HeaderText = "이메일";
            if (dgvUsers.Columns.Contains("department")) dgvUsers.Columns["department"].HeaderText = "부서";
        }
        //오
        private void LoadPermissionDepartments()
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM departments WHERE company_id = @cid ORDER BY name",
                    new MySqlParameter("@cid", _companyId));

                clbDepartments.Items.Clear();

                // CheckedListBox는 DataSource 쓰지 말고 Items에 직접 넣기
                foreach (DataRow row in dt.Rows)
                {
                    clbDepartments.Items.Add(
                        new ComboItem
                        {
                            Id = Convert.ToInt32(row["id"]),
                            Name = row["name"].ToString()
                        },
                        false  // 초기 체크상태 false
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("부서 목록을 불러올 때 오류 발생: " + ex.Message);
            }
        }

        // CheckedListBox에 넣을 객체 (표시용)
        private class ComboItem
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString() => Name; // 화면에 표시될 이름
        }

        private void ResetPermissionUI()
        {
            _selectedPermissionUserId = null;

            chkAllEmployees.Checked = true;
            clbDepartments.Enabled = false;

            for (int i = 0; i < clbDepartments.Items.Count; i++)
                clbDepartments.SetItemChecked(i, false);
        }
        private void LoadUserPermission(int userId)
        {
            // 먼저 전체 체크 해제
            for (int i = 0; i < clbDepartments.Items.Count; i++)
                clbDepartments.SetItemChecked(i, false);

            var dt = DBManager.Instance.ExecuteDataTable(
                "SELECT dept_id FROM user_view_permission WHERE viewer_user_id = @uid",
                new MySqlParameter("@uid", userId));

            // 권한 레코드가 없으면 = 모든 직원 보기
            if (dt.Rows.Count == 0)
            {
                chkAllEmployees.Checked = true;
                clbDepartments.Enabled = false;
                return;
            }

            chkAllEmployees.Checked = false;
            clbDepartments.Enabled = true;

            // 이 사용자가 가진 dept_id들을 HashSet으로
            var allowedDeptIds = dt.AsEnumerable()
                                   .Where(r => r["dept_id"] != DBNull.Value)
                                   .Select(r => Convert.ToInt32(r["dept_id"]))
                                   .ToHashSet();

            for (int i = 0; i < clbDepartments.Items.Count; i++)
            {
                var item = clbDepartments.Items[i] as ComboItem;
                if (item == null) continue;

                if (allowedDeptIds.Contains(item.Id))
                    clbDepartments.SetItemChecked(i, true);
            }
        }

        private void chkAllEmployees_CheckedChanged(object sender, EventArgs e)
        {
            clbDepartments.Enabled = !chkAllEmployees.Checked;
        }

        private void btnSavePermission_Click(object sender, EventArgs e)
        {
            if (_selectedPermissionUserId == null)
            {
                MessageBox.Show("사용자를 선택하세요.");
                return;
            }

            int uid = _selectedPermissionUserId.Value;

            DBManager.Instance.ExecuteNonQuery(
                "DELETE FROM user_view_permission WHERE viewer_user_id = @uid",
                new MySqlParameter("@uid", uid));

            if (chkAllEmployees.Checked)
            {
                MessageBox.Show("저장되었습니다.");
                return;
            }

            foreach (var obj in clbDepartments.CheckedItems)
            {
                var dept = obj as ComboItem;
                if (dept == null) continue;

                int deptId = dept.Id;

                DBManager.Instance.ExecuteNonQuery(
                    @"INSERT INTO user_view_permission (viewer_user_id, dept_id, group_code)
              VALUES (@uid, @deptId, NULL)",
                    new MySqlParameter("@uid", uid),
                    new MySqlParameter("@deptId", deptId));
            }
            MessageBox.Show("저장되었습니다.");
        }

        private void btnResetPermission_Click(object sender, EventArgs e)
        {
            ResetPermissionUI();

        // --- Chat Ban Management ---
        private void InitializeChatBanUI()
        {
            // Designer provides pageChatBan and controls; just bind data
            if (pageChatBan == null) return;
            LoadUserList();
            LoadBanList();
        }

        private void LoadUserList()
        {
            var dt = DBManager.Instance.ExecuteDataTable(
                "SELECT id, COALESCE(full_name,email) AS name FROM users WHERE company_id=@cid ORDER BY name",
                new MySqlParameter("@cid", _companyId));

            _cbUser1.DataSource = dt.Copy();
            _cbUser1.DisplayMember = "name";
            _cbUser1.ValueMember = "id";
            _cbUser1.SelectedIndex = -1;

            _cbUser2.DataSource = dt;
            _cbUser2.DisplayMember = "name";
            _cbUser2.ValueMember = "id";
            _cbUser2.SelectedIndex = -1;
        }

        private void LoadBanList()
        {
            string sql = @"
                SELECT b.id, b.user_id_1, b.user_id_2, b.created_at,
                       u1.full_name AS name1, u2.full_name AS name2
                FROM chat_bans b
                JOIN users u1 ON u1.id = b.user_id_1
                JOIN users u2 ON u2.id = b.user_id_2
                WHERE u1.company_id = @cid OR u2.company_id = @cid
                ORDER BY b.created_at DESC";

            var dt = DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@cid", _companyId));

            _lvBans.BeginUpdate();
            _lvBans.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                var item = new ListViewItem(Convert.ToString(row["name1"]))
                {
                    Tag = row["id"]
                };
                item.SubItems.Add(Convert.ToString(row["name2"]));
                item.SubItems.Add(Convert.ToDateTime(row["created_at"]).ToString("yyyy-MM-dd HH:mm:ss"));
                _lvBans.Items.Add(item);
            }
            _lvBans.EndUpdate();
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            if (_cbUser1.SelectedIndex < 0 || _cbUser2.SelectedIndex < 0)
            {
                MessageBox.Show("두 사용자를 선택하세요.");
                return;
            }

            int uid1 = Convert.ToInt32(_cbUser1.SelectedValue);
            int uid2 = Convert.ToInt32(_cbUser2.SelectedValue);
            if (uid1 == uid2)
            {
                MessageBox.Show("같은 사용자끼리는 차단할 수 없습니다.");
                return;
            }

            int a = Math.Min(uid1, uid2);
            int b = Math.Max(uid1, uid2);

            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "INSERT INTO chat_bans (user_id_1, user_id_2, created_at) VALUES (@u1, @u2, @created)",
                    new MySqlParameter("@u1", a),
                    new MySqlParameter("@u2", b),
                    new MySqlParameter("@created", DateTime.Now));

                LoadBanList();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("차단 추가 중 오류: " + ex.Message);
            }
        }

        private void btnUnblock_Click(object sender, EventArgs e)
        {
            if (_lvBans.SelectedItems.Count == 0)
            {
                MessageBox.Show("해제할 항목을 선택하세요.");
                return;
            }

            var idObj = _lvBans.SelectedItems[0].Tag;
            if (idObj == null) return;

            int banId = Convert.ToInt32(idObj);
            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "DELETE FROM chat_bans WHERE id=@id",
                    new MySqlParameter("@id", banId));
                LoadBanList();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("차단 해제 중 오류: " + ex.Message);
            }
        }
    }
}