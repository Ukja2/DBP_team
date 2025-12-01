using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Controls;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DBP_team
{
    public partial class ChatForm : Form
    {
        private readonly int _myUserId;
        private readonly int _otherUserId;
        private readonly string _otherName;
        private FlowLayoutPanel _flow;

        // TCP client
        private TcpClient _tcp;
        private StreamReader _reader;
        private StreamWriter _writer;
        private CancellationTokenSource _cts;
        private Thread _recvThread;

        // Prefer internal host; fallback to external for users outside the LAN
        private const string InternalHost = "192.168.55.207";
        private const string ExternalHost = "39.112.89.182";
        private const int ChatServerPort = 9000;
        private static string _resolvedHost; // cached for process lifetime

        private List<Control> _highlighted = new List<Control>();
        private List<ChatBubbleControl> _searchResults = new List<ChatBubbleControl>();
        private int _searchIndex = -1;

        public ChatForm(int myUserId, int otherUserId, string otherName)
        {
            InitializeComponent();

            _myUserId = myUserId;
            _otherUserId = otherUserId;
            // 대화 상단에 표시할 이름은 otherId 관점에서의 내 표시명으로 교체 (상대에게 내가 어떻게 보이는지)
            var nameForHeader = MultiProfileService.GetDisplayNameForViewer(_myUserId, _otherUserId);
            _otherName = string.IsNullOrWhiteSpace(otherName) ? (string.IsNullOrWhiteSpace(nameForHeader) ? "상대" : nameForHeader) : otherName;

            labelChat.Text = _otherName;

            // 기존 listChat(디자이너)에 의존하므로 숨기고 FlowLayoutPanel을 추가
            if (this.Controls.Contains(listChat)) listChat.Visible = false;

            _flow = new FlowLayoutPanel
            {
                Location = listChat.Location,
                Size = listChat.Size,
                Anchor = listChat.Anchor,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                BackColor = listChat.BackColor
            };

            this.Controls.Add(_flow);
            _flow.BringToFront();

            _flow.Resize += (s, e) =>
            {
                foreach (Control c in _flow.Controls)
                {
                    if (c is ChatBubbleControl bubble)
                    {
                        var t = bubble.Tag as Tuple<string, DateTime, bool, int>;
                        if (t != null)
                        {
                            bubble.Width = _flow.ClientSize.Width;
                            bubble.SetData(t.Item1, t.Item2, t.Item3, _flow.ClientSize.Width);
                        }
                    }
                }
            };

            btnSend.Text = "전송";

            LoadMessages();
            ConnectToChatServer();

            // ensure search UI is initialized label
            lblSearchCount.Text = "0/0";
        }

        private void ConnectToChatServer()
        {
            try
            {
                var host = ResolveHost();

                _tcp = new TcpClient();
                // If host was not resolvable via quick probe (edge case), fallback to external
                if (string.IsNullOrEmpty(host)) host = ExternalHost;
                _tcp.Connect(host, ChatServerPort);
                var ns = _tcp.GetStream();
                _reader = new StreamReader(ns, Encoding.UTF8);
                _writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };
                _cts = new CancellationTokenSource();

                _writer.WriteLine("AUTH|" + _myUserId);
                var resp = _reader.ReadLine();

                // mark existing messages from other user as read
                _writer.WriteLine("READ|" + _myUserId + "|" + _otherUserId);

                _recvThread = new Thread(RecvLoop) { IsBackground = true };
                _recvThread.Start();
            }
            catch (Exception ex)
            {
                var hostInfo = string.IsNullOrEmpty(_resolvedHost) ? "(host 미결정)" : _resolvedHost;
                MessageBox.Show("채팅 서버 연결 실패: " + ex.Message + " [" + hostInfo + "]", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Decide once per process: try internal first with short timeout, else external
        private static string ResolveHost()
        {
            if (!string.IsNullOrEmpty(_resolvedHost)) return _resolvedHost;
            if (CanConnectQuick(InternalHost, ChatServerPort, 700))
            {
                _resolvedHost = InternalHost;
            }
            else
            {
                _resolvedHost = ExternalHost;
            }
            return _resolvedHost;
        }

        private static bool CanConnectQuick(string host, int port, int timeoutMs)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var ar = client.BeginConnect(host, port, null, null);
                    bool ok = ar.AsyncWaitHandle.WaitOne(timeoutMs);
                    if (!ok) { try { client.Close(); } catch { } return false; }
                    client.EndConnect(ar);
                    try { client.Close(); } catch { }
                    return true;
                }
            }
            catch { return false; }
        }

        private void RecvLoop()
        {
            try
            {
                while (_tcp != null && _tcp.Connected && !_cts.IsCancellationRequested)
                {
                    var line = _reader.ReadLine();
                    if (line == null) break;
                    var parts = line.Split('|');
                    if (parts.Length >= 4 && parts[0] == "MSG")
                    {
                        int from = 0, to = 0;
                        int.TryParse(parts[1], out from);
                        int.TryParse(parts[2], out to);
                        var msg = DecodeBase64(parts[3]);
                        if (from == _otherUserId && to == _myUserId)
                        {
                            var time = DateTime.Now;
                            this.BeginInvoke((Action)(() =>
                            {
                                AddBubbleImmediate(msg, time, false, 0);
                                UpdateRecentList();
                                // after displaying, send READ ack to server to mark them read
                                _writer?.WriteLine("READ|" + _myUserId + "|" + _otherUserId);
                            }));
                        }
                    }
                    else if (parts.Length >= 3 && parts[0] == "READ")
                    {
                        // other user read my messages; update my bubbles to show '읽음'
                        int readerId = 0, senderId = 0;
                        int.TryParse(parts[1], out readerId);
                        int.TryParse(parts[2], out senderId);
                        if (readerId == _otherUserId && senderId == _myUserId)
                        {
                            this.BeginInvoke((Action)(MarkMyMessagesRead));
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void MarkMyMessagesRead()
        {
            foreach (Control c in _flow.Controls)
            {
                var bubble = c as ChatBubbleControl;
                if (bubble == null) continue;
                var t = bubble.Tag as Tuple<string, DateTime, bool, int>;
                if (t == null) continue;
                if (t.Item3) // my message
                {
                    bubble.SetRead(true);
                }
            }
        }

        private static string EncodeBase64(string s) => Convert.ToBase64String(Encoding.UTF8.GetBytes(s ?? ""));
        private static string DecodeBase64(string b)
        {
            try { return Encoding.UTF8.GetString(Convert.FromBase64String(b)); } catch { return ""; }
        }

        private void LoadMessages()
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, sender_id, receiver_id, message, created_at, is_read " +
                    "FROM chat " +
                    "WHERE (sender_id = @me AND receiver_id = @other) OR (sender_id = @other AND receiver_id = @me) " +
                    "ORDER BY created_at ASC",
                    new MySqlParameter("@me", _myUserId),
                    new MySqlParameter("@other", _otherUserId));

                _flow.Controls.Clear();

                if (dt == null || dt.Rows.Count == 0)
                {
                    var lbl = new Label { Text = "대화가 없습니다.", AutoSize = true, ForeColor = Color.Gray, Margin = new Padding(10) };
                    _flow.Controls.Add(lbl);
                    return;
                }

                foreach (DataRow r in dt.Rows)
                {
                    var id = Convert.ToInt32(r["id"]);
                    var time = r["created_at"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(r["created_at"]);
                    var senderId = Convert.ToInt32(r["sender_id"]);
                    var whoMine = senderId == _myUserId;
                    var message = r["message"]?.ToString() ?? "";

                    var bubble = CreateBubble(message, time, whoMine, id);
                    if (whoMine)
                    {
                        bool isRead = r["is_read"] != DBNull.Value && Convert.ToInt32(r["is_read"]) == 1;
                        bubble.SetRead(isRead);
                    }
                    _flow.Controls.Add(bubble);
                }

                if (_flow.Controls.Count > 0)
                    _flow.ScrollControlIntoView(_flow.Controls[_flow.Controls.Count - 1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("채팅 불러오는 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Ensure chat_files table exists
        private void EnsureChatFilesTableExists()
        {
            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS chat_files (" +
                    "id INT AUTO_INCREMENT PRIMARY KEY, " +
                    "sender_id INT NOT NULL, " +
                    "receiver_id INT NOT NULL, " +
                    "filename VARCHAR(255), " +
                    "content LONGBLOB, " +
                    "created_at DATETIME DEFAULT CURRENT_TIMESTAMP" +
                    ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");
            }
            catch { /* ignore creation errors for now */ }
        }

        private int SaveFileToDb(int senderId, int receiverId, string filename, byte[] content)
        {
            EnsureChatFilesTableExists();
            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "INSERT INTO chat_files (sender_id, receiver_id, filename, content, created_at) VALUES (@s,@r,@f,@c,NOW())",
                    new MySqlParameter("@s", senderId),
                    new MySqlParameter("@r", receiverId),
                    new MySqlParameter("@f", filename),
                    new MySqlParameter("@c", content));

                var idObj = DBManager.Instance.ExecuteScalar("SELECT LAST_INSERT_ID()");
                if (idObj != null && idObj != DBNull.Value)
                    return Convert.ToInt32(idObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일 DB 저장 실패: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }

        private void OnBubbleDownloadRequestedHandler(int fileId, string fileName)
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT filename, content FROM chat_files WHERE id = @id LIMIT 1",
                    new MySqlParameter("@id", fileId));
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("파일을 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var row = dt.Rows[0];
                var fname = row["filename"]?.ToString() ?? fileName ?? "file";
                // If DB filename lacks extension but the caller provided a filename with extension, append it.
                try
                {
                    var dbExt = Path.GetExtension(fname);
                    if (string.IsNullOrEmpty(dbExt) && !string.IsNullOrEmpty(fileName))
                    {
                        var pExt = Path.GetExtension(fileName);
                        if (!string.IsNullOrEmpty(pExt)) fname = fname + pExt;
                    }
                }
                catch { }
                var contentObj = row["content"];
                byte[] data = null;
                if (contentObj is byte[] b) data = b;
                else if (contentObj != DBNull.Value) data = (byte[])contentObj;

                if (data == null)
                {
                    MessageBox.Show("파일 데이터가 비어있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var sfd = new SaveFileDialog())
                {
                    // ensure extension preserved
                    sfd.FileName = fname;
                    // DefaultExt should not contain leading dot
                    var ext = Path.GetExtension(fname);
                    if (!string.IsNullOrEmpty(ext))
                    {
                        sfd.DefaultExt = ext.StartsWith(".") ? ext.Substring(1) : ext;
                    }
                    sfd.Filter = "All files (*.*)|*.*";
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    var outPath = sfd.FileName;

                    // if user did not include extension, append original extension
                    if (string.IsNullOrEmpty(Path.GetExtension(outPath)) && !string.IsNullOrEmpty(Path.GetExtension(fname)))
                    {
                        outPath = outPath + Path.GetExtension(fname);
                    }

                    File.WriteAllBytes(outPath, data);
                    MessageBox.Show("파일이 저장되었습니다: " + outPath, "다운로드", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try
                    {
                        Process.Start("explorer.exe", "/select,\"" + outPath + "\"");
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일 다운로드 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ChatBubbleControl CreateBubble(string message, DateTime time, bool isMine, int id)
        {
            var bubble = new ChatBubbleControl();
            // set control width to container width so SetData aligns correctly
            bubble.Width = Math.Max(200, _flow.ClientSize.Width);
            bubble.Tag = Tuple.Create(message, time, isMine, id);
            // call SetData after width is set
            bubble.SetData(message, time, isMine, _flow.ClientSize.Width);
            bubble.Margin = new Padding(0, 6, 0, 6);

            // detect file token format: FILE:{fileId}:{filename}
            if (!string.IsNullOrEmpty(message) && message.StartsWith("FILE:", StringComparison.OrdinalIgnoreCase))
            {
                var parts = message.Split(new[] { ':' }, 3);
                if (parts.Length == 3 && int.TryParse(parts[1], out int fid))
                {
                    var fname = parts[2];
                    bubble.SetFile(fid, fname);
                    bubble.OnDownloadRequested += OnBubbleDownloadRequestedHandler;
                }
            }

            return bubble;
        }

        private void AddBubbleImmediateFile(int fileId, string filename, DateTime time, bool isMine)
        {
            var bubble = new ChatBubbleControl();
            bubble.Width = Math.Max(200, _flow.ClientSize.Width);
            bubble.Tag = Tuple.Create("FILE:" + fileId + ":" + filename, time, isMine, 0);
            bubble.SetData(string.Empty, time, isMine, _flow.ClientSize.Width);
            bubble.SetFile(fileId, filename);
            bubble.Margin = new Padding(0, 6, 0, 6);
            bubble.OnDownloadRequested += OnBubbleDownloadRequestedHandler;
            if (isMine) bubble.SetRead(false);
            _flow.Controls.Add(bubble);
            _flow.ScrollControlIntoView(bubble);
        }

        private void ClearPreviousHighlights()
        {
            foreach (var c in _highlighted)
            {
                if (c is ChatBubbleControl cb)
                {
                    var t = cb.Tag as Tuple<string, DateTime, bool, int>;
                    if (t != null)
                    {
                        var isMine = t.Item3;
                        // reset by reapplying SetData which restores panel colors
                        cb.SetData(t.Item1, t.Item2, isMine, _flow.ClientSize.Width);
                        cb.BackColor = Color.Transparent;
                    }
                }
                else
                {
                    try { c.BackColor = Color.Transparent; } catch { }
                }
            }
            _highlighted.Clear();
            _searchResults.Clear();
            _searchIndex = -1;
            lblSearchCount.Text = "0/0";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var q = txtSearch.Text?.Trim();
            if (string.IsNullOrEmpty(q))
            {
                MessageBox.Show("검색어를 입력하세요.", "검색", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ClearPreviousHighlights();

            // find all bubbles that contain the query
            foreach (Control c in _flow.Controls)
            {
                if (c is ChatBubbleControl bubble)
                {
                    var t = bubble.Tag as Tuple<string, DateTime, bool, int>;
                    if (t == null) continue;
                    var msg = t.Item1 ?? string.Empty;
                    if (msg.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        _searchResults.Add(bubble);
                    }
                }
            }

            if (_searchResults.Count == 0)
            {
                MessageBox.Show("일치하는 메시지가 없습니다.", "검색", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblSearchCount.Text = "0/0";
                return;
            }

            // highlight all results and focus on first
            for (int i = 0; i < _searchResults.Count; i++)
            {
                var bubble = _searchResults[i];
                bubble.BackColor = Color.FromArgb(255, 255, 220);
                _highlighted.Add(bubble);
            }

            _searchIndex = 0;
            lblSearchCount.Text = ($"{_searchIndex + 1}/{_searchResults.Count}");
            ScrollToSearchIndex();
        }

        private void ScrollToSearchIndex()
        {
            if (_searchIndex < 0 || _searchIndex >= _searchResults.Count) return;
            var bubble = _searchResults[_searchIndex];
            _flow.ScrollControlIntoView(bubble);
            // emphasize current one (darker highlight)
            foreach (var b in _searchResults)
            {
                b.BackColor = Color.FromArgb(255, 255, 220);
            }
            var cur = _searchResults[_searchIndex];
            cur.BackColor = Color.FromArgb(255, 220, 120);
            // ensure it's in highlighted list
            if (!_highlighted.Contains(cur)) _highlighted.Add(cur);
        }

        private void btnSearchNext_Click(object sender, EventArgs e)
        {
            if (_searchResults.Count == 0) return;
            _searchIndex = (_searchIndex + 1) % _searchResults.Count;
            lblSearchCount.Text = ($"{_searchIndex + 1}/{_searchResults.Count}");
            ScrollToSearchIndex();
        }

        private void btnSearchPrev_Click(object sender, EventArgs e)
        {
            if (_searchResults.Count == 0) return;
            _searchIndex = (_searchIndex - 1 + _searchResults.Count) % _searchResults.Count;
            lblSearchCount.Text = ($"{_searchIndex + 1}/{_searchResults.Count}");
            ScrollToSearchIndex();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = txtChat.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("메시지를 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // send via TCP to server
                var line = "MSG|" + _myUserId + "|" + _otherUserId + "|" + EncodeBase64(text);
                _writer?.WriteLine(line);

                var sentTime = DateTime.Now;
                txtChat.Clear();
                txtChat.Focus();
                AddBubbleImmediate(text, sentTime, true, 0);
                UpdateRecentList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("메시지 전송 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try { _cts?.Cancel(); } catch { }
            try { _writer?.WriteLine("QUIT|"); } catch { }
            try { _tcp?.Close(); } catch { }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 현재 사용 안 함
        }

        private void txtChat_TextChanged(object sender, EventArgs e)
        {
            // 필요 시 입력 검증 등 추가
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            // 폼 로드시 추가 초기화가 필요하면 작성
        }

        // --- New handlers for search, emoji, file ---
        private void btnEmoji_Click(object sender, EventArgs e)
        {
            // 간단한 팝업으로 몇 가지 이모티콘을 선택하게 함
            var menu = new ContextMenuStrip();
            var emojis = new[] { "😀", "😁", "😂", "😍", "😢", "😮", "👍", "👎", "🎉" };
            foreach (var em in emojis)
            {
                var item = new ToolStripMenuItem(em);
                item.Click += (s, ev) => { InsertEmoji(em); };
                menu.Items.Add(item);
            }
            menu.Show(btnEmoji, new Point(0, -menu.Size.Height));
        }

        private void InsertEmoji(string emoji)
        {
            var pos = txtChat.SelectionStart;
            txtChat.Text = txtChat.Text.Insert(pos, emoji);
            txtChat.SelectionStart = pos + emoji.Length;
            txtChat.Focus();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Multiselect = false;
                dlg.Title = "파일 전송";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var path = dlg.FileName;

                try
                {
                    var bytes = File.ReadAllBytes(path);

                    // If zip, validate header quickly
                    if (Path.GetExtension(path).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        if (bytes.Length < 4 || bytes[0] != (byte)'P' || bytes[1] != (byte)'K')
                        {
                            MessageBox.Show("선택한 파일은 ZIP 형식이 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // save to DB
                    var fileId = SaveFileToDb(_myUserId, _otherUserId, Path.GetFileName(path), bytes);
                    if (fileId <= 0)
                    {
                        MessageBox.Show("파일을 DB에 저장하지 못했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // send file token via chat server so other side gets notified and message persisted
                    var token = "FILE:" + fileId + ":" + Path.GetFileName(path);
                    var line = "MSG|" + _myUserId + "|" + _otherUserId + "|" + EncodeBase64(token);
                    _writer?.WriteLine(line);

                    // locally show file bubble immediately
                    AddBubbleImmediateFile(fileId, Path.GetFileName(path), DateTime.Now, true);
                    UpdateRecentList();

                    MessageBox.Show("파일 전송 요청이 전송되었습니다.", "파일 전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("파일 처리 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddBubbleImmediate(string message, DateTime time, bool isMine, int id)
        {
            if (!string.IsNullOrEmpty(message) && message.StartsWith("FILE:", StringComparison.OrdinalIgnoreCase))
            {
                // message format: FILE:{fileId}:{filename}
                var parts = message.Split(new[] { ':' }, 3);
                if (parts.Length == 3 && int.TryParse(parts[1], out int fid))
                {
                    var fname = parts[2];
                    AddBubbleImmediateFile(fid, fname, time, isMine);
                    return;
                }
            }

            var bubble = CreateBubble(message, time, isMine, id);
            if (isMine) bubble.SetRead(false); // until READ received
            _flow.Controls.Add(bubble);
            _flow.ScrollControlIntoView(bubble);
        }

        // Try to refresh recent list in main form so "내가 마지막으로 보낸" 대화가 바로 갱신되도록 요청
        private void UpdateRecentList()
        {
            try
            {
                MainForm main = null;
                if (this.Owner is MainForm) main = (MainForm)this.Owner;
                if (main == null)
                {
                    main = System.Windows.Forms.Application.OpenForms.OfType<MainForm>().FirstOrDefault();
                }

                if (main != null)
                {
                    try { main.LoadRecentChats(); } catch { }
                }
            }
            catch { }
        }
    }
}
