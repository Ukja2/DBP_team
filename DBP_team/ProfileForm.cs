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
        private readonly int _viewerId; // who is viewing
        private readonly bool _readOnly;

        public ProfileForm(int userId)
            : this(viewerId: userId, targetUserId: userId, readOnly: false)
        {
        }

        // New ctor to support viewing others in read-only
        public ProfileForm(int viewerId, int targetUserId, bool readOnly)
        {
            InitializeComponent();
            UI.IconHelper.ApplyAppIcon(this);
            _userId = targetUserId;
            _viewerId = viewerId;
            _readOnly = readOnly;

            if (pictureProfile == null || labelFullName == null)
            {
                MessageBox.Show("디자이너 컨트롤이 초기화되지 않았습니다. Designer 파일과 namespace를 확인하세요.", "디버그", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                pictureProfile.BackColor = Color.LightGray;
            }

            // Buttons: only hook when not read-only
            try { this.btnClose.Click -= btnClose_Click; this.btnClose.Click += btnClose_Click; } catch { }
            if (!_readOnly)
            {
                try { this.btnChangeImage.Click -= btnChangeImage_Click; this.btnChangeImage.Click += btnChangeImage_Click; } catch { }
                try { this.btnSave.Click -= btnSave_Click; this.btnSave.Click += btnSave_Click; } catch { }
                try { this.btnAddressSearch.Click -= btnAddressSearch_Click; this.btnAddressSearch.Click += btnAddressSearch_Click; } catch { }
                try { this.btnMultiProfiles.Click -= btnMultiProfiles_Click; this.btnMultiProfiles.Click += btnMultiProfiles_Click; } catch { }
            }
            else
            {
                // hide edit-related buttons in read-only view
                try { if (btnChangeImage != null) btnChangeImage.Visible = false; } catch { }
                try { if (btnSave != null) btnSave.Visible = false; } catch { }
                try { if (btnAddressSearch != null) btnAddressSearch.Visible = false; } catch { }
                try { if (btnMultiProfiles != null) btnMultiProfiles.Visible = false; } catch { }

                // disable editable fields
                try { if (txtFullName != null) { txtFullName.ReadOnly = true; txtFullName.BackColor = SystemColors.Control; } } catch { }
                try { if (txtNickname != null) { txtNickname.ReadOnly = true; txtNickname.BackColor = SystemColors.Control; } } catch { }
                try { if (txtAddressMain != null) { txtAddressMain.ReadOnly = true; txtAddressMain.BackColor = SystemColors.Control; } } catch { }
                try { if (txtAddressDetail != null) { txtAddressDetail.ReadOnly = true; txtAddressDetail.BackColor = SystemColors.Control; } } catch { }
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

                // Full name: apply multi-profile display name if viewer is not the owner and read-only
                var fullName = row.Table.Columns.Contains("full_name") ? row["full_name"].ToString() : string.Empty;
                if (_readOnly && _viewerId > 0 && _viewerId != _userId)
                {
                    var mpName = MultiProfileService.GetDisplayNameForViewer(_userId, _viewerId);
                    if (!string.IsNullOrWhiteSpace(mpName)) fullName = mpName;
                }
                if (txtFullName != null) txtFullName.Text = fullName ?? string.Empty;

                // Email/company/department/team labels
                labelEmail.Text = "이메일: " + (row["email"]?.ToString() ?? "(없음)");
                labelCompany.Text = "회사: " + (row["company_name"]?.ToString() ?? "(없음)");
                labelDepartment.Text = "부서: " + (row["department_name"]?.ToString() ?? "(없음)");
                labelTeam.Text = "팀: " + (row["team_name"]?.ToString() ?? "(없음)");

                // Nickname: always show from nickname column if present
                try
                {
                    string nick = null;
                    if (row.Table.Columns.Contains("nickname") && row["nickname"] != DBNull.Value)
                        nick = row["nickname"].ToString();
                    if (txtNickname != null)
                        txtNickname.Text = string.IsNullOrWhiteSpace(nick) ? string.Empty : nick;
                }
                catch { }

                // Address/zip
                try
                {
                    string address = null;
                    string zip = null;
                    var addrCols = new[] { "address", "addr", "address_main", "address1", "full_address" };
                    foreach (var c in addrCols)
                    {
                        if (row.Table.Columns.Contains(c) && row[c] != DBNull.Value)
                        {
                            address = row[c]?.ToString();
                            break;
                        }
                    }
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
                    var zipCols = new[] { "zipNo", "zipcode", "postal", "postal_code" };
                    foreach (var c in zipCols)
                    {
                        if (row.Table.Columns.Contains(c) && row[c] != DBNull.Value)
                        {
                            zip = row[c]?.ToString();
                            break;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        var parts = new List<string>();
                        if (row.Table.Columns.Contains("siNm") && row["siNm"] != DBNull.Value) parts.Add(row["siNm"].ToString());
                        if (row.Table.Columns.Contains("sggNm") && row["sggNm"] != DBNull.Value) parts.Add(row["sggNm"].ToString());
                        if (row.Table.Columns.Contains("emdNm") && row["emdNm"] != DBNull.Value) parts.Add(row["emdNm"].ToString());
                        if (row.Table.Columns.Contains("rn") && row["rn"] != DBNull.Value) parts.Add(row["rn"].ToString());
                        if (parts.Count > 0) address = string.Join(" ", parts);
                    }

                    txtAddressMain.Text = FormatAddressForDisplay(address);
                    if (!string.IsNullOrWhiteSpace(detail))
                    {
                        txtAddressDetail.Text = detail;
                        txtAddressDetail.Visible = !_readOnly; // hide detail input when read-only
                    }
                    else
                    {
                        txtAddressDetail.Text = string.Empty;
                        txtAddressDetail.Visible = !_readOnly ? false : false;
                    }

                    labelPostalCode.Text = "우편번호: " + (zip ?? "-");
                }
                catch { }

                // Profile image with multi-profile override
                try
                {
                    byte[] imgBytes = null;
                    if (_readOnly && _viewerId > 0 && _viewerId != _userId)
                        imgBytes = MultiProfileService.GetProfileImageForViewer(_userId, _viewerId);
                    if (imgBytes == null && row.Table.Columns.Contains("profile_image") && row["profile_image"] != DBNull.Value)
                        imgBytes = row["profile_image"] as byte[];

                    if (imgBytes != null)
                    {
                        using (var ms = new MemoryStream(imgBytes))
                        {
                            try { pictureProfile.Image = Image.FromStream(ms); }
                            catch { pictureProfile.Image = null; }
                        }
                    }
                    else
                    {
                        pictureProfile.Image = null;
                    }
                }
                catch { }
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
    }
}
