using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public partial class ProfileForm : Form
    {
        private readonly int _userId;
        private readonly AuthService _auth = new AuthService();

        // 새로 추가: 프로필을 보는 사용자(뷰어) id와 읽기전용 플래그
        private int _viewerId;
        private bool _readOnly;

        public ProfileForm(int userId)
        {
            InitializeComponent();
            UI.IconHelper.ApplyAppIcon(this);
            _userId = userId;

            if (pictureProfile == null || labelFullName == null)
            {
                MessageBox.Show("디자이너 컨트롤이 초기화되지 않았습니다. Designer 파일과 namespace를 확인하세요.", "디버그", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                pictureProfile.BackColor = Color.LightGray;
            }

            try { this.btnClose.Click -= btnClose_Click; this.btnClose.Click += btnClose_Click; } catch { }
            try { this.btnChangeImage.Click -= btnChangeImage_Click; this.btnChangeImage.Click += btnChangeImage_Click; } catch { }
            try { this.btnSave.Click -= btnSave_Click; this.btnSave.Click += btnSave_Click; } catch { }
            try { this.btnAddressSearch.Click -= btnAddressSearch_Click; this.btnAddressSearch.Click += btnAddressSearch_Click; } catch { }
            try { this.btnMultiProfiles.Click -= btnMultiProfiles_Click; this.btnMultiProfiles.Click += btnMultiProfiles_Click; } catch { }
            // ChangePWbtn 이벤트 연결 추가
            try { this.ChangePWbtn.Click -= ChangePWbtn_Click; this.ChangePWbtn.Click += ChangePWbtn_Click; } catch { }
        }

        // 추가된 오버로드: 뷰어(id)로 다른 사용자의 프로필을 읽기전용으로 열 때 사용
        public ProfileForm(int viewerId, int targetUserId, bool readOnly) : this(targetUserId)
        {
            _viewerId = viewerId;
            _readOnly = readOnly;

            if (_readOnly)
            {
                // 읽기전용 모드: 수정 가능한 컨트롤 비활성화/숨김 (안전하게 try/catch)
                try { if (txtFullName != null) txtFullName.ReadOnly = true; } catch { }
                try { if (txtNickname != null) txtNickname.ReadOnly = true; } catch { }
                try { if (txtAddressMain != null) txtAddressMain.ReadOnly = true; } catch { }
                try { if (txtAddressDetail != null) txtAddressDetail.ReadOnly = true; } catch { }
                try { if (btnChangeImage != null) btnChangeImage.Enabled = false; } catch { }
                try { if (btnSave != null) btnSave.Visible = false; } catch { }
                try { if (btnAddressSearch != null) btnAddressSearch.Enabled = false; } catch { }
                try { if (ChangePWbtn != null) ChangePWbtn.Visible = false; } catch { }
            }
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void btnMultiProfiles_Click(object sender, EventArgs e)
        {
            using (var f = new UI.MultiProfilesForm(_userId))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }

        private void LoadProfile()
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT u.id, u.full_name, u.nickname, u.email, u.phone, u.profile_image, u.address, u.zipNo, " +
                    "c.name AS company_name, d.name AS department_name, t.name AS team_name " +
                    "FROM users u " +
                    "LEFT JOIN companies c ON u.company_id = c.id " +
                    "LEFT JOIN departments d ON u.department_id = d.id " +
                    "LEFT JOIN teams t ON u.team_id = t.id " +
                    "WHERE u.id = @id",
                    new MySqlParameter("@id", _userId));

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("사용자 정보를 불러오지 못했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                var row = dt.Rows[0];

                var fullName = row.Table.Columns.Contains("full_name") ? row["full_name"].ToString() : string.Empty;
                if (txtFullName != null)
                    txtFullName.Text = fullName ?? string.Empty;

                labelEmail.Text = "이메일: " + (row["email"]?.ToString() ?? "(없음)");
                labelCompany.Text = "회사: " + (row["company_name"]?.ToString() ?? "(없음)");
                labelDepartment.Text = "부서: " + (row["department_name"]?.ToString() ?? "(없음)");
                labelTeam.Text = "팀: " + (row["team_name"]?.ToString() ?? "(없음)");

                try
                {
                    var nick = row.Table.Columns.Contains("nickname") ? row["nickname"]?.ToString() : null;
                    if (txtNickname != null)
                        txtNickname.Text = !string.IsNullOrWhiteSpace(nick) ? nick : string.Empty;
                }
                catch { }

                // Address fields: try several possible column names and show as up to two lines.
                try
                {
                    string address = null;
                    string zip = null;

                    // common address column names fallback
                    var addrCols = new[] { "address", "addr", "address_main", "address1", "full_address" };
                    foreach (var c in addrCols)
                    {
                        if (row.Table.Columns.Contains(c) && row[c] != DBNull.Value)
                        {
                            address = row[c]?.ToString();
                            break;
                        }
                    }

                    // detail column fallback (not shown until edit)
                    var detailCols = new[] { "address_detail", "addr_detail", "address2", "detail_address" };
                    string detail = null;
                    foreach (var c in detailCols)
                    {
                        if (row.Table.Columns.Contains(c) && row[c] != DBNull.Value)
                        {
                            detail = row[c]?.ToString();
                            break;
                        }
                    }

                    // zip fallback
                    var zipCols = new[] { "zipNo", "zipcode", "postal", "postal_code" };
                    foreach (var c in zipCols)
                    {
                        if (row.Table.Columns.Contains(c) && row[c] != DBNull.Value)
                        {
                            zip = row[c]?.ToString();
                            break;
                        }
                    }

                    // if address still null, try to compose from components (si/sgg/emd/rn)
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        var parts = new List<string>();
                        if (row.Table.Columns.Contains("siNm") && row["siNm"] != DBNull.Value) parts.Add(row["siNm"].ToString());
                        if (row.Table.Columns.Contains("sggNm") && row["sggNm"] != DBNull.Value) parts.Add(row["sggNm"].ToString());
                        if (row.Table.Columns.Contains("emdNm") && row["emdNm"] != DBNull.Value) parts.Add(row["emdNm"].ToString());
                        if (row.Table.Columns.Contains("rn") && row["rn"] != DBNull.Value) parts.Add(row["rn"].ToString());
                        if (parts.Count > 0) address = string.Join(" ", parts);
                    }

                    // display
                    txtAddressMain.Text = FormatAddressForDisplay(address);
                    // if detail column exists, show it (user can edit)
                    if (!string.IsNullOrWhiteSpace(detail))
                    {
                        txtAddressDetail.Text = detail;
                        txtAddressDetail.Visible = true;
                    }
                    else
                    {
                        txtAddressDetail.Text = string.Empty;
                        txtAddressDetail.Visible = false;
                    }

                    labelPostalCode.Text = "우편번호: " + (zip ?? "-");
                }
                catch { }

                if (row.Table.Columns.Contains("profile_image") && row["profile_image"] != DBNull.Value && row["profile_image"] is byte[])
                {
                    var bytes = (byte[])row["profile_image"];
                    using (var ms = new MemoryStream(bytes))
                    {
                        try
                        {
                            var img = Image.FromStream(ms);
                            pictureProfile.Image = new Bitmap(img);
                        }
                        catch
                        {
                            pictureProfile.Image = null;
                        }
                    }
                }
                else
                {
                    pictureProfile.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("프로필 로드 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper: split a long address into up to two lines for display
        private string FormatAddressForDisplay(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return string.Empty;
            // If address already contains newline, return as-is
            if (address.Contains("\n")) return address;

            // Try to split near the middle at a space
            var maxFirstLine = 40; // chars
            if (address.Length <= maxFirstLine) return address;

            // find last space before maxFirstLine
            var idx = address.LastIndexOf(' ', Math.Min(address.Length - 1, maxFirstLine));
            if (idx <= 0) idx = Math.Min(maxFirstLine, address.Length / 2);

            var first = address.Substring(0, idx).Trim();
            var second = address.Substring(idx).Trim();
            return first + "\r\n" + second;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChangeImage_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일|*.*";
                ofd.Title = "프로필 이미지 선택";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    Image loadedImage;
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    using (var tmp = Image.FromStream(fs))
                    {
                        loadedImage = new Bitmap(tmp);
                    }

                    try { pictureProfile.Image?.Dispose(); } catch { }
                    pictureProfile.Image = loadedImage;

                    byte[] imgBytes;
                    using (var ms = new MemoryStream())
                    {
                        loadedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imgBytes = ms.ToArray();
                    }

                    DBManager.Instance.ExecuteNonQuery("UPDATE users SET profile_image = @img WHERE id = @id",
                        new MySqlParameter("@img", MySqlDbType.Blob) { Value = imgBytes },
                        new MySqlParameter("@id", _userId));

                    MessageBox.Show("프로필 이미지가 변경되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("이미지 적용 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddressSearch_Click(object sender, EventArgs e)
        {
            using (var f = new AddressSearchForm(null))
            {
                var dr = f.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(f.SelectedPostalCode)) labelPostalCode.Text = "우편번호: " + f.SelectedPostalCode;
                    if (!string.IsNullOrEmpty(f.SelectedAddress)) txtAddressMain.Text = FormatAddressForDisplay(f.SelectedAddress);

                    // show detail input for user to enter building/apt etc.
                    txtAddressDetail.Visible = true;
                    txtAddressDetail.Focus();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var newName = txtFullName?.Text?.Trim() ?? string.Empty;
                var newNick = txtNickname?.Text?.Trim();

                // combine main + detail into single address
                var main = (txtAddressMain.Text ?? string.Empty).Replace("\r\n", " ").Trim();
                var detail = txtAddressDetail.Visible ? txtAddressDetail.Text?.Trim() : null;
                var combined = string.IsNullOrWhiteSpace(detail) ? main : (main + " " + detail).Trim();

                // ensure address and zipNo columns exist
                try { DBManager.Instance.ExecuteNonQuery("ALTER TABLE users ADD COLUMN address TEXT NULL"); } catch { }
                try { DBManager.Instance.ExecuteNonQuery("ALTER TABLE users ADD COLUMN zipNo VARCHAR(20) NULL"); } catch { }

                var sql = "UPDATE users SET full_name = @name, nickname = @nick, address = @addr, zipNo = @zip WHERE id = @id";
                var pName = new MySqlParameter("@name", string.IsNullOrWhiteSpace(newName) ? (object)DBNull.Value : newName);
                var pNick = new MySqlParameter("@nick", string.IsNullOrWhiteSpace(newNick) ? (object)DBNull.Value : newNick);
                var pAddr = new MySqlParameter("@addr", string.IsNullOrWhiteSpace(combined) ? (object)DBNull.Value : combined);
                var pZip = new MySqlParameter("@zip", labelPostalCode.Text.Replace("우편번호:", "").Trim() ?? (object)DBNull.Value);
                var pId = new MySqlParameter("@id", _userId);

                var rows = DBManager.Instance.ExecuteNonQuery(sql, pName, pNick, pAddr, pZip, pId);
                if (rows > 0)
                {
                    MessageBox.Show("프로필 정보가 저장되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProfile();
                }
                else
                {
                    MessageBox.Show("저장에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void labelFullName_Click(object sender, EventArgs e)
        {

        }

        private void txtAddressDetail_TextChanged(object sender, EventArgs e)
        {

        }

        private void ChangePWbtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new Form())
                {
                    dlg.Text = "비밀번호 변경";
                    dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ClientSize = new Size(380, 170);
                    dlg.MaximizeBox = false;
                    dlg.MinimizeBox = false;
                    dlg.ShowIcon = false;

                    var lblCurrent = new Label { Text = "현재 비밀번호", Location = new Point(10, 12), AutoSize = true };
                    var txtCurrent = new TextBox { Location = new Point(140, 10), Width = 220, UseSystemPasswordChar = true };

                    var lblNew = new Label { Text = "새 비밀번호", Location = new Point(10, 48), AutoSize = true };
                    var txtNew = new TextBox { Location = new Point(140, 46), Width = 220, UseSystemPasswordChar = true };

                    var lblConfirm = new Label { Text = "새 비밀번호 확인", Location = new Point(10, 84), AutoSize = true };
                    var txtConfirm = new TextBox { Location = new Point(140, 82), Width = 220, UseSystemPasswordChar = true };

                    var btnOk = new Button { Text = "변경", Location = new Point(200, 120), DialogResult = DialogResult.OK, Size = new Size(75, 28) };
                    var btnCancel = new Button { Text = "취소", Location = new Point(285, 120), DialogResult = DialogResult.Cancel, Size = new Size(75, 28) };

                    dlg.Controls.AddRange(new Control[] { lblCurrent, txtCurrent, lblNew, txtNew, lblConfirm, txtConfirm, btnOk, btnCancel });
                    dlg.AcceptButton = btnOk;
                    dlg.CancelButton = btnCancel;

                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    var current = txtCurrent.Text;
                    var newPw = txtNew.Text;
                    var confirm = txtConfirm.Text;

                    if (string.IsNullOrWhiteSpace(current) || string.IsNullOrWhiteSpace(newPw) || string.IsNullOrWhiteSpace(confirm))
                    {
                        MessageBox.Show("모든 필드를 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (newPw != confirm)
                    {
                        MessageBox.Show("새 비밀번호가 일치하지 않습니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (newPw.Length < 6)
                    {
                        MessageBox.Show("새 비밀번호는 최소 6자 이상이어야 합니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 저장된 해시 확인
                    var dt = DBManager.Instance.ExecuteDataTable("SELECT password_hash FROM users WHERE id = @id",
                        new MySqlParameter("@id", _userId));

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("사용자 정보를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var storedHash = dt.Rows[0]["password_hash"]?.ToString();

                    if (!_auth.VerifyPassword(current, storedHash))
                    {
                        MessageBox.Show("현재 비밀번호가 일치하지 않습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var newHash = _auth.CreateHash(newPw);

                    var rows = DBManager.Instance.ExecuteNonQuery("UPDATE users SET password_hash = @hash WHERE id = @id",
                        new MySqlParameter("@hash", newHash),
                        new MySqlParameter("@id", _userId));

                    if (rows > 0)
                    {
                        MessageBox.Show("비밀번호가 변경되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("비밀번호 변경에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("비밀번호 변경 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
