using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Models;
using System.Text;

namespace DBP_team
{
    public partial class AdminForm : Form
    {
        // --- 필드 선언 ---
        private readonly User _me;
        private readonly int _companyId;

        private TabControl _tabs, mainTabControl;

        // Dept
        private TextBox _txtDeptName, _txtDeptSearch;
        private Button _btnDeptAdd, _btnDeptUpdate, _btnDeptSearch; // removed seed button
        private DataGridView _gridDept;

        // User-Dept
        private TextBox _txtUserSearch;
        private ComboBox _cboDeptForUser;
        private Button _btnUserSearch, _btnApplyDept;
        private DataGridView _gridUsers;

        // Chat search
        private DateTimePicker _dtFrom, _dtTo;
        private TextBox _txtKeyword;
        private ComboBox _cboUserFilter;
        private Button _btnChatSearch;
        private DataGridView _gridChat;

        // 접속 이력 탭용 컨트롤 필드
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private TextBox txtSearchUser;
        private Button btnSearchLog;
        private DataGridView _gridLogs; // ListView를 DataGridView로 변경

        public AdminForm(User me)
        {
            _me = me ?? throw new ArgumentNullException(nameof(me));
            _companyId = me.CompanyId ?? 0;

            if (!AdminGuard.IsAdmin(me))
            {
                MessageBox.Show("관리자 권한이 없습니다.", "권한 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            Text = "관리자 콘솔";
            Width = 1024;
            Height = 720;
            StartPosition = FormStartPosition.CenterScreen;

            BuildUi();
            LoadDeptGrid();
            LoadDeptComboForUser();
            LoadUsersGrid();
            LoadUserFilterCombo();
        }

        private void BuildUi()
        {
            _tabs = new TabControl { Dock = DockStyle.Fill };

            // Tab 1: 부서관리
            var pageDept = new TabPage("부서관리");
            var pnlDeptTop = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(8) };

            var lblDeptName = new Label { Text = "부서명", AutoSize = true, Top = 12, Left = 8 };
            _txtDeptName = new TextBox { Width = 220, Left = 60, Top = 8 };
            _btnDeptAdd = new Button { Text = "등록", Left = 290, Top = 7, Width = 80 };
            _btnDeptUpdate = new Button { Text = "변경", Left = 375, Top = 7, Width = 80 };

            var lblDeptSearch = new Label { Text = "검색(부서명)", AutoSize = true, Top = 12, Left = 470 };
            _txtDeptSearch = new TextBox { Left = 560, Top = 8, Width = 220 };
            _btnDeptSearch = new Button { Text = "검색", Left = 785, Top = 7, Width = 80 };

            _btnDeptAdd.Click += (s, e) => AddDepartment();
            _btnDeptUpdate.Click += (s, e) => UpdateDepartment();
            _btnDeptSearch.Click += (s, e) => LoadDeptGrid(_txtDeptSearch.Text?.Trim());

            pnlDeptTop.Controls.Add(lblDeptName);
            pnlDeptTop.Controls.Add(_txtDeptName);
            pnlDeptTop.Controls.Add(_btnDeptAdd);
            pnlDeptTop.Controls.Add(_btnDeptUpdate);
            pnlDeptTop.Controls.Add(lblDeptSearch);
            pnlDeptTop.Controls.Add(_txtDeptSearch);
            pnlDeptTop.Controls.Add(_btnDeptSearch);

            _gridDept = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            _gridDept.MultiSelect = false;
            _gridDept.CellClick += (s, e) => { if (e.RowIndex >= 0) _txtDeptName.Text = Convert.ToString(_gridDept.Rows[e.RowIndex].Cells["name"].Value); };
            pageDept.Controls.Add(_gridDept);
            pageDept.Controls.Add(pnlDeptTop);

            // Tab 2: 사용자 소속 변경
            var pageUserDept = new TabPage("사용자 소속 변경");
            var pnlUserTop = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(8) };

            var lblUserSearch = new Label { Text = "사용자 검색", AutoSize = true, Top = 12, Left = 8 };
            _txtUserSearch = new TextBox { Width = 240, Left = 80, Top = 8 };
            _btnUserSearch = new Button { Text = "검색", Left = 325, Top = 7, Width = 80 };
            _btnUserSearch.Click += (s, e) => LoadUsersGrid(_txtUserSearch.Text?.Trim());

            var lblDeptSelect = new Label { Text = "부서 선택", AutoSize = true, Top = 12, Left = 420 };
            _cboDeptForUser = new ComboBox { Left = 480, Top = 8, Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
            _btnApplyDept = new Button { Text = "선택 사용자 소속 변경", Left = 725, Top = 7, Width = 180 };
            _btnApplyDept.Click += (s, e) => ApplyUserDepartment();

            pnlUserTop.Controls.Add(lblUserSearch);
            pnlUserTop.Controls.Add(_txtUserSearch);
            pnlUserTop.Controls.Add(_btnUserSearch);
            pnlUserTop.Controls.Add(lblDeptSelect);
            pnlUserTop.Controls.Add(_cboDeptForUser);
            pnlUserTop.Controls.Add(_btnApplyDept);

            _gridUsers = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            _gridUsers.MultiSelect = true;
            pageUserDept.Controls.Add(_gridUsers);
            pageUserDept.Controls.Add(pnlUserTop);

            // Tab 3: 대화 검색
            var pageChat = new TabPage("대화 검색");
            var pnlChatTop = new Panel { Dock = DockStyle.Top, Height = 70, Padding = new Padding(8) };

            var lblFrom = new Label { Text = "시작일", AutoSize = true, Top = 12, Left = 8 };
            _dtFrom = new DateTimePicker { Width = 140, Left = 60, Top = 8, Value = DateTime.Now.Date.AddDays(-7) };

            var lblTo = new Label { Text = "종료일", AutoSize = true, Top = 12, Left = 210 };
            _dtTo = new DateTimePicker { Left = 260, Top = 8, Width = 140, Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1) };

            var lblKeyword = new Label { Text = "키워드", AutoSize = true, Top = 12, Left = 410 };
            _txtKeyword = new TextBox { Left = 455, Top = 8, Width = 220 };

            var lblUser = new Label { Text = "사용자", AutoSize = true, Top = 12, Left = 685 };
            _cboUserFilter = new ComboBox { Left = 730, Top = 8, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            _btnChatSearch = new Button { Text = "검색", Left = 940, Top = 7, Width = 60 };
            _btnChatSearch.Click += (s, e) => LoadChatGrid();

            pnlChatTop.Controls.Add(lblFrom);
            pnlChatTop.Controls.Add(_dtFrom);
            pnlChatTop.Controls.Add(lblTo);
            pnlChatTop.Controls.Add(_dtTo);
            pnlChatTop.Controls.Add(lblKeyword);
            pnlChatTop.Controls.Add(_txtKeyword);
            pnlChatTop.Controls.Add(lblUser);
            pnlChatTop.Controls.Add(_cboUserFilter);
            pnlChatTop.Controls.Add(_btnChatSearch);

            _gridChat = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            pageChat.Controls.Add(_gridChat);
            pageChat.Controls.Add(pnlChatTop);

            // --- "접속 이력" 탭 추가 ---
            var pageAccessLogs = CreateAccessLogsTab(); // 접속 이력 탭 생성

            _tabs.TabPages.Add(pageDept);
            _tabs.TabPages.Add(pageUserDept);
            _tabs.TabPages.Add(pageChat);
            _tabs.TabPages.Add(pageAccessLogs); // 생성된 탭을 TabControl에 추가
            Controls.Add(_tabs);
        }

        // 부서 목록 로드 - hide IDs from view
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
            if (_gridDept.CurrentRow == null) { MessageBox.Show("행을 선택하세요."); return; }
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

        // 사용자 목록 - show department name; hide ids
        private void LoadUsersGrid(string keyword = null)
        {
            var sql = "SELECT u.id, COALESCE(u.full_name,u.email) AS name, u.email, u.department_id, d.name AS department " +
                      "FROM users u LEFT JOIN departments d ON d.id = u.department_id " +
                      "WHERE u.company_id=@cid";
            var pars = new System.Collections.Generic.List<MySqlParameter> { new MySqlParameter("@cid", _companyId) };
            if (!string.IsNullOrWhiteSpace(keyword)) { sql += " AND (u.full_name LIKE @kw OR u.email LIKE @kw)"; pars.Add(new MySqlParameter("@kw", "%" + keyword + "%")); }
            sql += " ORDER BY name";
            var dt = DBManager.Instance.ExecuteDataTable(sql, pars.ToArray());
            _gridUsers.DataSource = dt;
            if (_gridUsers.Columns.Contains("id")) _gridUsers.Columns["id"].Visible = false;
            if (_gridUsers.Columns.Contains("department_id")) _gridUsers.Columns["department_id"].Visible = false;
            if (_gridUsers.Columns.Contains("name")) _gridUsers.Columns["name"].HeaderText = "이름";
            if (_gridUsers.Columns.Contains("email")) _gridUsers.Columns["email"].HeaderText = "이메일";
            if (_gridUsers.Columns.Contains("department")) _gridUsers.Columns["department"].HeaderText = "부서";
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

        // 채팅 검색 - show names only
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

        private TabPage CreateAccessLogsTab()
        {
            var tabPage = new TabPage("접속 이력");

            // 검색 패널 (기존과 동일)
            var pnlSearch = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(8),
                WrapContents = false
            };
            var lblDate = new Label { Text = "기간:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(3, 6, 0, 3) };
            dtpStart = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 100, Margin = new Padding(3) };
            var lblDateSeparator = new Label { Text = "~", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(3, 6, 0, 3) };
            dtpEnd = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 100, Margin = new Padding(3) };
            var lblUser = new Label { Text = "사용자:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(15, 6, 0, 3) };
            txtSearchUser = new TextBox { Width = 150, Margin = new Padding(3) };
            btnSearchLog = new Button { Text = "검색", Width = 80, Margin = new Padding(10, 0, 3, 0) };
            btnSearchLog.Click += (s, e) => SearchAccessLogs();
            pnlSearch.Controls.AddRange(new Control[] { lblDate, dtpStart, lblDateSeparator, dtpEnd, lblUser, txtSearchUser, btnSearchLog });

            // --- 로그 표시 컨트롤을 DataGridView로 변경 ---
            _gridLogs = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tabPage.Controls.Add(_gridLogs); // ListView 대신 DataGridView 추가
            tabPage.Controls.Add(pnlSearch);

            // --- 초기 데이터 로드 이벤트 (기존과 동일) ---
            this.Shown += (s, e) =>
            {
                if (_gridLogs != null && !this.IsDisposed && _tabs.SelectedTab == tabPage)
                {
                    SearchAccessLogs();
                }
            };
            _tabs.SelectedIndexChanged += (s, e) =>
            {
                if (_tabs.SelectedTab == tabPage)
                {
                    SearchAccessLogs();
                }
            };

            return tabPage;
        }

        private void SearchAccessLogs()
        {
            if (_gridLogs == null) return; // 컨트롤이 생성되지 않았으면 종료

            // DAO 역할을 하는 검색 로직
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
                
                // --- 데이터 바인딩 방식을 DataGridView에 맞게 수정 ---
                _gridLogs.DataSource = dt;

                // 컬럼 헤더 텍스트 및 숨김 처리
                if (_gridLogs.Columns.Contains("created_at"))
                {
                    _gridLogs.Columns["created_at"].HeaderText = "시간";
                    _gridLogs.Columns["created_at"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                }
                if (_gridLogs.Columns.Contains("full_name")) _gridLogs.Columns["full_name"].HeaderText = "사용자명";
                if (_gridLogs.Columns.Contains("activity_type")) _gridLogs.Columns["activity_type"].HeaderText = "활동";
                if (_gridLogs.Columns.Contains("user_id")) _gridLogs.Columns["user_id"].HeaderText = "사용자 ID";
                
                // email 컬럼은 결과에 표시하지 않음 (사용자명으로 충분)
                if (_gridLogs.Columns.Contains("email")) _gridLogs.Columns["email"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그 검색 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
