using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DBP_team.UI
{
    public class TeamAddDialog : Form
    {
        private ComboBox _cbDept;
        private TextBox _txtName;
        private Button _ok;
        private Button _cancel;
        private readonly int _companyId;

        public int SelectedDepartmentId
        {
            get
            {
                int id; if (_cbDept.SelectedValue != null && int.TryParse(_cbDept.SelectedValue.ToString(), out id)) return id; return 0;
            }
        }
        public string TeamName => _txtName.Text;

        public TeamAddDialog(int companyId)
        {
            _companyId = companyId;
            Text = "팀 추가";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            ClientSize = new Size(420, 180);

            var lblDept = new Label { Left = 12, Top = 18, Width = 80, Text = "부서 선택" };
            _cbDept = new ComboBox { Left = 100, Top = 14, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblName = new Label { Left = 12, Top = 60, Width = 80, Text = "팀 이름" };
            _txtName = new TextBox { Left = 100, Top = 56, Width = 300 };

            _ok = new Button { Text = "추가", DialogResult = DialogResult.OK, Left = 244, Top = 120, Width = 75, Height = 28 };
            _cancel = new Button { Text = "취소", DialogResult = DialogResult.Cancel, Left = 325, Top = 120, Width = 75, Height = 28 };

            Controls.Add(lblDept);
            Controls.Add(_cbDept);
            Controls.Add(lblName);
            Controls.Add(_txtName);
            Controls.Add(_ok);
            Controls.Add(_cancel);

            AcceptButton = _ok;
            CancelButton = _cancel;

            Load += TeamAddDialog_Load;
        }

        private void TeamAddDialog_Load(object sender, EventArgs e)
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM departments WHERE company_id = @cid ORDER BY name",
                    new MySqlParameter("@cid", _companyId));
                _cbDept.DataSource = dt;
                _cbDept.DisplayMember = "name";
                _cbDept.ValueMember = "id";
                _cbDept.SelectedIndex = dt != null && dt.Rows.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("부서 목록을 불러오지 못했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
