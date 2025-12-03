using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Models;

namespace DBP_team
{
    public partial class Loginform : Form
    {
        private readonly AuthService _auth = new AuthService();

        public Loginform()
        {
            InitializeComponent();

            txtPwd.UseSystemPasswordChar = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtId.Text = Properties.Settings.Default.SavedId;
            txtPwd.Text = Properties.Settings.Default.SavedPwd;
            chkRemember.Checked = Properties.Settings.Default.RememberMe;
            chkAutoLogin.Checked = Properties.Settings.Default.AutoLogin;

            // 자동 로그인
            if (chkAutoLogin.Checked &&
                !string.IsNullOrWhiteSpace(txtId.Text) &&
                !string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                var result = MessageBox.Show(
                    "저장된 계정으로 자동 로그인 하시겠습니까?",
                    "자동 로그인",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        btnLogin.PerformClick();
                    }));
                }
                else
                {
                    chkAutoLogin.Checked = false;
                    SaveLoginSettings();
                }
            }
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

            // 기존 위치/크기 유지
            reg.StartPosition = FormStartPosition.Manual;
            reg.Size = this.Size;
            var desired = this.Location;
            var screen = Screen.FromControl(this);
            var bounds = screen.WorkingArea;

            if (desired.X < bounds.Left) desired.X = bounds.Left;
            if (desired.X + reg.Width > bounds.Right) desired.X = Math.Max(bounds.Left, bounds.Right - reg.Width);
            if (desired.Y < bounds.Top) desired.Y = bounds.Top;
            if (desired.Y + reg.Height > bounds.Bottom) desired.Y = Math.Max(bounds.Top, bounds.Bottom - reg.Height);

            reg.Location = desired;

            reg.FormClosed += (s, args) =>
            {
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
                    "SELECT u.id, u.password_hash, u.full_name, u.role, u.company_id, c.name AS company_name, u.email " +
                    "FROM users u LEFT JOIN companies c ON u.company_id = c.id WHERE u.email = @email",
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

                // 마지막 로그인 시간 업데이트
                DBManager.Instance.ExecuteNonQuery("UPDATE users SET last_login = NOW() WHERE id = @id",
                    new MySqlParameter("@id", Convert.ToInt32(row["id"])));

                // User 객체 생성
                var user = new User
                {
                    Id = Convert.ToInt32(row["id"]),
                    FullName = row["full_name"]?.ToString(),
                    Role = row["role"]?.ToString(),
                    Email = row["email"]?.ToString(),
                    CompanyId = row["company_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["company_id"]),
                    CompanyName = row["company_name"] == DBNull.Value ? null : row["company_name"].ToString()
                };

                AppSession.CurrentUser = user;

                SaveLoginSettings();

                Form next;
                if (AdminGuard.IsAdmin(user))
                {
                    next = new AdminForm(user);
                }
                else
                {
                    next = new MainForm(user);
                }

                next.StartPosition = FormStartPosition.CenterScreen;
                this.Hide();
                next.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그인 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkRemember_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkRemember.Checked)
            {
                chkAutoLogin.Checked = false;
            }
        }

        private void chkAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoLogin.Checked && !chkRemember.Checked)
            {
                chkRemember.Checked = true;
            }
        }

        private void SaveLoginSettings()
        {
            if (chkRemember.Checked)
            {
                Properties.Settings.Default.SavedId = txtId.Text.Trim();
                Properties.Settings.Default.SavedPwd = txtPwd.Text;
                Properties.Settings.Default.RememberMe = true;
            }
            else
            {
                Properties.Settings.Default.SavedId = "";
                Properties.Settings.Default.SavedPwd = "";
                Properties.Settings.Default.RememberMe = false;
            }

            // 자동로그인 여부는 체크박스 상태 그대로 저장
            Properties.Settings.Default.AutoLogin = chkAutoLogin.Checked;

            Properties.Settings.Default.Save();
        }
        private bool _passwordVisible = false;
        private void btnTogglePwd_Click(object sender, EventArgs e)
        {
            _passwordVisible = !_passwordVisible;

            if (_passwordVisible)
            {
                txtPwd.UseSystemPasswordChar = false;
            }
            else
            {
                txtPwd.UseSystemPasswordChar = true;
            }
        }
    }
}
