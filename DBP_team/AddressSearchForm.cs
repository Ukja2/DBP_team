using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBP_team
{
    public class AddressSearchForm : Form
    {
        private TextBox txtQuery;
        private Button btnSearch;
        private ListBox listResults;
        private Label lblStatus;
        private Button btnOk;
        private Button btnCancel;

        public string SelectedPostalCode { get; private set; }
        public string SelectedAddress { get; private set; }

        // Juso API confmKey provided by user
        private const string JusoConfmKey = "devU01TX0FVVEgyMDI1MTEwNjE1MzQwOTExNjQxMTc=";

        public AddressSearchForm(string initialQuery = null)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(initialQuery)) txtQuery.Text = initialQuery;
        }

        private void InitializeComponent()
        {
            this.Text = "주소 검색";
            this.ClientSize = new Size(560, 360);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            txtQuery = new TextBox { Location = new Point(12, 12), Size = new Size(436, 24) };
            btnSearch = new Button { Location = new Point(456, 10), Size = new Size(92, 26), Text = "검색" };
            listResults = new ListBox { Location = new Point(12, 44), Size = new Size(536, 260) };
            lblStatus = new Label { Location = new Point(12, 308), Size = new Size(536, 20), Text = "" };
            btnOk = new Button { Location = new Point(372, 332), Size = new Size(80, 28), Text = "확인" };
            btnCancel = new Button { Location = new Point(468, 332), Size = new Size(80, 28), Text = "취소" };

            // --- 이벤트 핸들러를 별도 메서드로 연결 ---
            btnSearch.Click += BtnSearch_Click;
            listResults.DoubleClick += ListResults_DoubleClick;
            btnOk.Click += BtnOk_Click;
            btnCancel.Click += BtnCancel_Click;
            // -----------------------------------------

            this.Controls.Add(txtQuery);
            this.Controls.Add(btnSearch);
            this.Controls.Add(listResults);
            this.Controls.Add(lblStatus);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
        }

        // --- 이벤트 핸들러 메서드 구현 ---

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            await DoSearchAsync();
        }

        private void ListResults_DoubleClick(object sender, EventArgs e)
        {
            AcceptSelection();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            AcceptSelection();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // --- 로직 메서드 ---
        private void AcceptSelection()
        {
            if (listResults.SelectedItem == null) return;
            var item = listResults.SelectedItem as AddressResult;
            if (item == null) return;
            SelectedPostalCode = item.PostalCode;
            SelectedAddress = item.Address;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private async Task DoSearchAsync()
        {
            var q = txtQuery.Text?.Trim();
            if (string.IsNullOrWhiteSpace(q))
            {
                MessageBox.Show("검색어를 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                lblStatus.Text = "검색 중...";
                btnSearch.Enabled = false;
                listResults.Items.Clear();

                var results = await SearchAddressAsync(q);
                foreach (var r in results)
                {
                    listResults.Items.Add(r);
                }

                lblStatus.Text = results.Count == 0 ? "결과가 없습니다." : $"결과 {results.Count}건";
            }
            catch (Exception ex)
            {
                MessageBox.Show("주소 검색 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "검색 실패";
            }
            finally
            {
                btnSearch.Enabled = true;
            }
        }

        private async Task<List<AddressResult>> SearchAddressAsync(string query)
        {
            var list = new List<AddressResult>();

            using (var http = new HttpClient())
            {
                var url = "https://www.juso.go.kr/addrlink/addrLinkApi.do?confmKey=" + Uri.EscapeDataString(JusoConfmKey)
                          + "&currentPage=1&countPerPage=20&keyword=" + Uri.EscapeDataString(query) + "&resultType=json";

                var resp = await http.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    var txt = await resp.Content.ReadAsStringAsync();
                    throw new Exception($"API error: {resp.StatusCode} - {txt}");
                }

                var json = await resp.Content.ReadAsStringAsync();

                var jusoArrayPos = json.IndexOf("\"juso\":", StringComparison.OrdinalIgnoreCase);
                if (jusoArrayPos >= 0)
                {
                    var objPattern = new Regex("\\{[^}]*\\}", RegexOptions.Multiline);
                    var roadPattern = new Regex("\"roadAddr\"\\s*:\\s*\"([^\"]*)\"", RegexOptions.Compiled);
                    var jibunPattern = new Regex("\"jibunAddr\"\\s*:\\s*\"([^\"]*)\"", RegexOptions.Compiled);
                    var zipPattern = new Regex("\"zipNo\"\\s*:\\s*\"([^\"]*)\"", RegexOptions.Compiled);

                    var arrStart = json.IndexOf('[', jusoArrayPos);
                    if (arrStart >= 0)
                    {
                        var arrEnd = json.IndexOf(']', arrStart);
                        if (arrEnd > arrStart)
                        {
                            var arrContent = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
                            foreach (Match m in objPattern.Matches(arrContent))
                            {
                                var objText = m.Value;
                                string addr = null;
                                string zip = null;
                                var rm = roadPattern.Match(objText);
                                if (rm.Success) addr = rm.Groups[1].Value;
                                var jm = jibunPattern.Match(objText);
                                if (jm.Success && string.IsNullOrWhiteSpace(addr)) addr = jm.Groups[1].Value;
                                var zm = zipPattern.Match(objText);
                                if (zm.Success) zip = zm.Groups[1].Value;

                                if (!string.IsNullOrWhiteSpace(addr))
                                {
                                    list.Add(new AddressResult { PostalCode = zip ?? string.Empty, Address = addr });
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        private class AddressResult
        {
            public string PostalCode { get; set; }
            public string Address { get; set; }
            public override string ToString() => $"[{PostalCode}] {Address}";
        }
    }
}
