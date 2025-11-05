using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public partial class Loginform : Form
    {
        private readonly AuthService _auth = new AuthService();

        public Loginform()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var reg = new Registerform();

            // 새 폼이 현재 폼과 같은 크기/위치로 뜨도록 설정
            reg.StartPosition = FormStartPosition.Manual;
            reg.Size = this.Size;

            var desired = this.Location;
            var screen = Screen.FromControl(this);
            var bounds = screen.WorkingArea;

            // X 보정
            if (desired.X < bounds.Left) desired.X = bounds.Left;
            if (desired.X + reg.Width > bounds.Right) desired.X = Math.Max(bounds.Left, bounds.Right - reg.Width);

            // Y 보정
            if (desired.Y < bounds.Top) desired.Y = bounds.Top;
            if (desired.Y + reg.Height > bounds.Bottom) desired.Y = Math.Max(bounds.Top, bounds.Bottom - reg.Height);

            reg.Location = desired;

            // 닫힐 때 로그인 폼을 reg과 동일한 위치/크기로 맞춘 뒤 다시 보이게 처리
            reg.FormClosed += (s, args) =>
            {
                // 위치/크기 복사
                this.StartPosition = FormStartPosition.Manual;
                this.Size = reg.Size;

                var desiredBack = reg.Location;
                var screenBack = Screen.FromControl(this);
                var boundsBack = screenBack.WorkingArea;

                if (desiredBack.X < boundsBack.Left) desiredBack.X = boundsBack.Left;
                if (desiredBack.X + this.Width > boundsBack.Right) desiredBack.X = Math.Max(boundsBack.Left, boundsBack.Right - this.Width);

                if (desiredBack.Y < boundsBack.Top) desiredBack.Y = boundsBack.Top;
                if (desiredBack.Y + this.Height > boundsBack.Bottom) desiredBack.Y = Math.Max(boundsBack.Top, boundsBack.Bottom - this.Height);

                this.Location = desiredBack;

                this.Show();
                reg.Dispose();
            };

            this.Hide();
            reg.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var email = txtId.Text?.Trim();
            var pwd = txtPwd.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pwd))
            {
                MessageBox.Show("이메일과 비밀번호를 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, password_hash, full_name, role FROM users WHERE email = @email",
                    new MySqlParameter("@email", email));

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("존재하지 않는 계정입니다.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var row = dt.Rows[0];
                var storedHash = row["password_hash"]?.ToString();

                if (!_auth.VerifyPassword(pwd, storedHash))
                {
                    MessageBox.Show("비밀번호가 일치하지 않습니다.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 로그인 성공: last_login 업데이트
                DBManager.Instance.ExecuteNonQuery("UPDATE users SET last_login = NOW() WHERE id = @id",
                    new MySqlParameter("@id", Convert.ToInt32(row["id"])));

                // 성공 메시지 명확히 표시
                var fullName = row["full_name"]?.ToString() ?? email;
                MessageBox.Show($"{fullName}님, 로그인 성공했습니다.", "로그인 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 필요하면 여기서 메인 폼으로 전환
                // this.Hide();
                // var main = new MainForm(...);
                // main.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그인 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
