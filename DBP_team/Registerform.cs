using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public partial class Registerform : Form
    {
        private readonly AuthService _auth = new AuthService();

        public Registerform()
        {
            InitializeComponent();
        }

        private void formRegister_Load(object sender, EventArgs e)
        {
            // DB 연결 확인
            string connErr;
            if (!DBManager.Instance.TestConnection(out connErr))
            {
                MessageBox.Show("데이터베이스 연결 실패:\n" + connErr + "\n연결정보와 네트워크를 확인하세요.", "DB 연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 연결이 안되면 더 이상 콤보를 로드하지 않음
                return;
            }

            // 기존 초기화 로직
            comboCompany.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTeam.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTeam2.DropDownStyle = ComboBoxStyle.DropDownList;

            comboCompany.Text = "회사";
            comboTeam.Text = "부서";
            comboTeam2.Text = "팀";

            LoadCompanies();

            comboCompany.SelectedIndexChanged -= comboCompany_SelectedIndexChanged;
            comboTeam.SelectedIndexChanged -= comboTeam_SelectedIndexChanged;
            comboTeam2.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;

            comboCompany.SelectedIndexChanged += comboCompany_SelectedIndexChanged;
            comboTeam.SelectedIndexChanged += comboTeam_SelectedIndexChanged;
            comboTeam2.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }
   

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var email = txtId.Text?.Trim();
            var pwd = txtPwd.Text;
            var pwdCheck = txtPwdCheck.Text;
            var name = txtName.Text?.Trim();

            // 기본 유효성 검사
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pwd) || string.IsNullOrWhiteSpace(pwdCheck) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("이메일, 비밀번호(확인), 이름을 모두 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pwd != pwdCheck)
            {
                MessageBox.Show("비밀번호와 비밀번호 확인이 일치하지 않습니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 회사/부서 선택 필수화 (원하면 팀도 필수로 변경 가능)
            int companyId = GetSelectedId(comboCompany);
            int departmentId = GetSelectedId(comboTeam);
            int teamId = GetSelectedId(comboTeam2);

            if (companyId <= 0)
            {
                MessageBox.Show("회사 선택은 필수입니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (departmentId <= 0)
            {
                MessageBox.Show("부서 선택은 필수입니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string err;
            var ok = _auth.Register(email, pwd, name, companyId, departmentId, teamId, out err);
            if (!ok)
            {
                MessageBox.Show(err ?? "회원가입 실패", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("회원가입이 완료되었습니다. 로그인 화면으로 이동합니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        // companies 로드
        private void LoadCompanies()
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable("SELECT id, name FROM companies ORDER BY name");
                if (dt == null)
                {
                    ClearCompanies();
                    return;
                }

                var dtWithDefault = CreateTableWithDefault(dt, 0, "회사 선택");

                // 바인딩 중 이벤트 발생 방지: DataSource 설정 전 이벤트 제거(안전)
                comboCompany.SelectedIndexChanged -= comboCompany_SelectedIndexChanged;
                comboCompany.DataSource = null;
                comboCompany.DisplayMember = "name";
                comboCompany.ValueMember = "id";
                comboCompany.DataSource = dtWithDefault;
                comboCompany.SelectedIndex = 0;
                comboCompany.SelectedIndexChanged += comboCompany_SelectedIndexChanged;

                // 하위 콤보 초기화
                ClearDepartments();
                ClearTeams();
            }
            catch (Exception ex)
            {
                MessageBox.Show("회사 목록 로드 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearCompanies();
            }
        }

        private void ClearCompanies()
        {
            comboCompany.SelectedIndexChanged -= comboCompany_SelectedIndexChanged;
            comboCompany.DataSource = null;
            comboCompany.Text = "회사";
            comboCompany.SelectedIndexChanged += comboCompany_SelectedIndexChanged;
        }

        // departments 로드 (companyId 에 따라)
        private void LoadDepartments(int companyId)
        {
            try
            {
                // 0 또는 기본값이면 초기화만 수행
                if (companyId <= 0)
                {
                    ClearDepartments();
                    ClearTeams();
                    return;
                }

                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM departments WHERE company_id = @cid ORDER BY name",
                    new MySqlParameter("@cid", companyId));

                if (dt == null)
                {
                    ClearDepartments();
                    ClearTeams();
                    return;
                }

                var dtWithDefault = CreateTableWithDefault(dt, 0, "부서 선택");

                comboTeam.SelectedIndexChanged -= comboTeam_SelectedIndexChanged;
                comboTeam.DataSource = null;
                comboTeam.DisplayMember = "name";
                comboTeam.ValueMember = "id";
                comboTeam.DataSource = dtWithDefault;
                comboTeam.SelectedIndex = 0;
                comboTeam.SelectedIndexChanged += comboTeam_SelectedIndexChanged;

                ClearTeams();
            }
            catch (Exception ex)
            {
                MessageBox.Show("부서 목록 로드 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearDepartments();
                ClearTeams();
            }
        }

        // teams 로드 (departmentId 에 따라)
        private void LoadTeams(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                {
                    ClearTeams();
                    return;
                }

                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM teams WHERE department_id = @did ORDER BY name",
                    new MySqlParameter("@did", departmentId));

                if (dt == null)
                {
                    ClearTeams();
                    return;
                }

                var dtWithDefault = CreateTableWithDefault(dt, 0, "팀 선택");

                comboTeam2.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                comboTeam2.DataSource = null;
                comboTeam2.DisplayMember = "name";
                comboTeam2.ValueMember = "id";
                comboTeam2.DataSource = dtWithDefault;
                comboTeam2.SelectedIndex = 0;
                comboTeam2.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("팀 목록 로드 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearTeams();
            }
        }

        // 콤보박스 선택 값 불러오는 안전한 유틸
        private int GetSelectedId(ComboBox cb)
        {
            if (cb == null) return 0;

            // SelectedIndex 기준으로 기본(0) 선택 여부를 먼저 검사
            if (cb.SelectedIndex <= 0) return 0;

            // SelectedValue가 정상적이면 사용
            if (cb.SelectedValue != null)
            {
                int id;
                if (int.TryParse(cb.SelectedValue.ToString(), out id))
                    return id;
            }

            // SelectedItem이 DataRowView인 경우 예비 처리
            var drv = cb.SelectedItem as DataRowView;
            if (drv != null && drv.Row.Table.Columns.Contains("id"))
            {
                int val;
                if (int.TryParse(drv["id"].ToString(), out val)) return val;
            }

            return 0;
        }

        // departments 초기화
        private void ClearDepartments()
        {
            comboTeam.SelectedIndexChanged -= comboTeam_SelectedIndexChanged;
            comboTeam.DataSource = null;
            comboTeam.Text = "부서";
            comboTeam.SelectedIndexChanged += comboTeam_SelectedIndexChanged;
        }

        // teams 초기화
        private void ClearTeams()
        {
            comboTeam2.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboTeam2.DataSource = null;
            comboTeam2.Text = "팀";
            comboTeam2.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        // 안전하게 새 DataTable을 만들고 기본 행을 추가
        private DataTable CreateTableWithDefault(DataTable source, int defaultId, string defaultName)
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));

            var defRow = dt.NewRow();
            defRow["id"] = defaultId;
            defRow["name"] = defaultName;
            dt.Rows.Add(defRow);

            if (source == null) return dt;

            foreach (DataRow r in source.Rows)
            {
                var nr = dt.NewRow();
                nr["id"] = Convert.ToInt32(r["id"]);
                nr["name"] = r["name"].ToString();
                dt.Rows.Add(nr);
            }

            return dt;
        }

        // 콤보 선택 이벤트 핸들러들
        private void comboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            // SelectedIndex 가 0(기본) 이면 부서/팀 초기화
            if (comboCompany.SelectedIndex <= 0)
            {
                ClearDepartments();
                ClearTeams();
                return;
            }

            var id = GetSelectedId(comboCompany);
            LoadDepartments(id);
        }

        private void comboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTeam.SelectedIndex <= 0)
            {
                ClearTeams();
                return;
            }

            var id = GetSelectedId(comboTeam);
            LoadTeams(id);
        }

        // 디자이너에서 comboTeam2에 연결된 핸들러 이름 유지
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 팀 선택 변경 시 추가 동작 필요하면 여기서 처리
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
