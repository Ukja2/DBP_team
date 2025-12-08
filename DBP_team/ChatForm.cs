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
using DBP_team.UI;

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
            UI.IconHelper.ApplyAppIcon(this);

            _myUserId = myUserId;
            _otherUserId = otherUserId;
            var nameForHeader = MultiProfileService.GetDisplayNameForViewer(_myUserId, _otherUserId);
            _otherName = string.IsNullOrWhiteSpace(otherName) ? (string.IsNullOrWhiteSpace(nameForHeader) ? "상대" : nameForHeader) : otherName;

            labelChat.Text = _otherName;

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

            lblSearchCount.Text = "0/0";

            // 시간 검색용 DateTimePicker 초기화
            dtpStartTime.Value = DateTime.Now.AddDays(-7); // 기본값: 7일 전
            dtpEndTime.Value = DateTime.Now; // 기본값: 현재 시간

            txtChat.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && !e.Shift)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    btnSend.PerformClick();
                }
            };

            // 입력창 자동 높이 조절
            txtChat.TextChanged += (s, e) =>
            {
                AdjustTextBoxHeight();
            };

            // 초기 높이 설정
            AdjustTextBoxHeight();
        }

        private void ConnectToChatServer()
        {
            try
            {
                // 자기 자신과의 채팅은 서버 연결이 불필요하므로 바로 반환
                if (_myUserId == _otherUserId) return;

                var host = ResolveHost();

                _tcp = new TcpClient();
                if (string.IsNullOrEmpty(host)) host = ExternalHost;
                _tcp.Connect(host, ChatServerPort);
                var ns = _tcp.GetStream();
                _reader = new StreamReader(ns, Encoding.UTF8);
                _writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };
                _cts = new CancellationTokenSource();

                _writer.WriteLine("AUTH|" + _myUserId);
                var resp = _reader.ReadLine();

                // mark existing messages from other user as read (server side)
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
                    if (parts.Length >= 5 && parts[0] == "MSG")
                    {
                        int from = 0, to = 0, mid = 0;
                        int.TryParse(parts[1], out from);
                        int.TryParse(parts[2], out to);
                        var msg = DecodeBase64(parts[3]);
                        int.TryParse(parts[4], out mid);

                        // 자기 자신과의 채팅은 서버 echo를 무시 (중복 방지)
                        if (_myUserId == _otherUserId && from == _myUserId && to == _myUserId)
                            continue;

                        if (from == _otherUserId && to == _myUserId)
                        {
                            var time = DateTime.Now;
                            this.BeginInvoke((Action)(() =>
                            {
                                // add bubble with real message id so edits/deletes apply while open
                                var bubble = CreateBubble(msg, time, false, mid);
                                _flow.Controls.Add(bubble);
                                _flow.ScrollControlIntoView(bubble);
                                UpdateRecentList();
                                _writer?.WriteLine("READ|" + _myUserId + "|" + _otherUserId);
                            }));
                        }
                    }
                    else if (parts.Length >= 3 && parts[0] == "READ")
                    {
                        int readerId = 0, senderId = 0;
                        int.TryParse(parts[1], out readerId);
                        int.TryParse(parts[2], out senderId);
                        if (readerId == _otherUserId && senderId == _myUserId)
                        {
                            // DB에 읽음 표시 반영
                            try
                            {
                                DBManager.Instance.ExecuteNonQuery(
                                    "UPDATE chat SET is_read = 1 WHERE sender_id = @me AND receiver_id = @other AND is_read <> 1",
                                    new MySqlParameter("@me", _myUserId),
                                    new MySqlParameter("@other", _otherUserId));
                            }
                            catch { }

                            this.BeginInvoke((Action)(ApplyReadToMyMessages));
                        }
                    }
                    else if (parts.Length >= 4 && parts[0] == "SENT")
                    {
                        // SENT|from|to|messageId -> update last mine bubble id
                        int from = 0, to = 0, mid = 0;
                        int.TryParse(parts[1], out from);
                        int.TryParse(parts[2], out to);
                        int.TryParse(parts[3], out mid);
                        if (from == _myUserId && to == _otherUserId && mid > 0)
                        {
                            this.BeginInvoke((Action)(() => ApplySentMessageId(mid)));
                        }
                    }
                    else if (parts.Length >= 3 && parts[0] == "EDIT")
                    {
                        // Format: EDIT|messageId|base64(newText)
                        int msgId = 0;
                        int.TryParse(parts[1], out msgId);
                        var newText = DecodeBase64(parts[2]);
                        this.BeginInvoke((Action)(() => ApplyRemoteEdit(msgId, newText)));
                    }
                    else if (parts.Length >= 2 && parts[0] == "DEL")
                    {
                        // Format: DEL|messageId
                        int msgId = 0;
                        int.TryParse(parts[1], out msgId);
                        this.BeginInvoke((Action)(() => ApplyRemoteDelete(msgId)));
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        // New helper: mark my outgoing bubbles as read
        private void ApplyReadToMyMessages()
        {
            foreach (Control c in _flow.Controls)
            {
                var bubble = c as ChatBubbleControl;
                if (bubble == null) continue;
                var t = bubble.Tag as Tuple<string, DateTime, bool, int>;
                if (t == null) continue;
                if (t.Item3)
                {
                    bubble.SetRead(true);
                }
            }
        }

        private void ApplyRemoteEdit(int messageId, string newText)
        {
            try
            {
                foreach (Control c in _flow.Controls)
                {
                    var b = c as ChatBubbleControl;
                    if (b == null) continue;
                    var t = b.Tag as Tuple<string, DateTime, bool, int>;
                    if (t != null && t.Item4 == messageId)
                    {
                        var newTag = Tuple.Create(newText, t.Item2, t.Item3, t.Item4);
                        b.Tag = newTag;
                        b.SetData(newText, t.Item2, t.Item3, _flow.ClientSize.Width);
                        break;
                    }
                }
            }
            catch { }
        }

        private void ApplyRemoteDelete(int messageId)
        {
            try
            {
                for (int i = 0; i < _flow.Controls.Count; i++)
                {
                    var b = _flow.Controls[i] as ChatBubbleControl;
                    if (b == null) continue;
                    var t = b.Tag as Tuple<string, DateTime, bool, int>;
                    if (t != null && t.Item4 == messageId)
                    {
                        _flow.Controls.RemoveAt(i);
                        b.Dispose();
                        break;
                    }
                }
            }
            catch { }
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
                    bubble.SetMessageId(id);
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
            catch { }
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
                    sfd.FileName = fname;
                    var ext = Path.GetExtension(fname);
                    if (!string.IsNullOrEmpty(ext))
                    {
                        sfd.DefaultExt = ext.StartsWith(".") ? ext.Substring(1) : ext;
                    }
                    sfd.Filter = "All files (*.*)|*.*";
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    var outPath = sfd.FileName;

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
            bubble.Width = Math.Max(200, _flow.ClientSize.Width);
            bubble.Tag = Tuple.Create(message, time, isMine, id);
            bubble.SetData(message, time, isMine, _flow.ClientSize.Width);
            bubble.SetMessageId(id);
            bubble.Margin = new Padding(0, 6, 0, 6);

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

            bubble.OnEditRequested += Bubble_OnEditRequested;
            bubble.OnDeleteRequested += Bubble_OnDeleteRequested;

            return bubble;
        }

        private void Bubble_OnEditRequested(int messageId)
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable("SELECT message FROM chat WHERE id = @id AND sender_id = @me",
                    new MySqlParameter("@id", messageId), new MySqlParameter("@me", _myUserId));
                if (dt == null || dt.Rows.Count == 0) return;
                var oldText = dt.Rows[0]["message"]?.ToString() ?? string.Empty;

                using (var dlg = new InputDialog("메시지 수정", oldText))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                    var newText = dlg.ResultText?.Trim();
                    if (string.IsNullOrEmpty(newText)) return;

                    DBManager.Instance.ExecuteNonQuery("UPDATE chat SET message = @msg WHERE id = @id AND sender_id = @me",
                        new MySqlParameter("@msg", newText),
                        new MySqlParameter("@id", messageId),
                        new MySqlParameter("@me", _myUserId));

                    // Update my UI immediately
                    foreach (Control c in _flow.Controls)
                    {
                        var b = c as ChatBubbleControl;
                        if (b == null) continue;
                        var t = b.Tag as Tuple<string, DateTime, bool, int>;
                        if (t != null && t.Item4 == messageId)
                        {
                            var newTag = Tuple.Create(newText, t.Item2, t.Item3, t.Item4);
                            b.Tag = newTag;
                            b.SetData(newText, t.Item2, t.Item3, _flow.ClientSize.Width);
                            break;
                        }
                    }

                    // Notify peer in realtime via chat server
                    if (_myUserId != _otherUserId)
                    {
                        _writer?.WriteLine("EDIT|" + messageId + "|" + EncodeBase64(newText));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("메시지 수정 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Bubble_OnDeleteRequested(int messageId)
        {
            try
            {
                var confirm = MessageBox.Show("이 메시지를 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                DBManager.Instance.ExecuteNonQuery("DELETE FROM chat WHERE id = @id AND sender_id = @me",
                    new MySqlParameter("@id", messageId), new MySqlParameter("@me", _myUserId));

                // Update my UI immediately
                for (int i = 0; i < _flow.Controls.Count; i++)
                {
                    var b = _flow.Controls[i] as ChatBubbleControl;
                    if (b == null) continue;
                    var t = b.Tag as Tuple<string, DateTime, bool, int>;
                    if (t != null && t.Item4 == messageId)
                    {
                        _flow.Controls.RemoveAt(i);
                        b.Dispose();
                        break;
                    }
                }

                // Notify peer in realtime via chat server
                if (_myUserId != _otherUserId)
                {
                    _writer?.WriteLine("DEL|" + messageId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("메시지 삭제 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            foreach (var b in _searchResults)
            {
                b.BackColor = Color.FromArgb(255, 255, 220);
            }
            var cur = _searchResults[_searchIndex];
            cur.BackColor = Color.FromArgb(255, 220, 120);
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

        private void btnSearchTime_Click(object sender, EventArgs e)
        {
            var startTime = dtpStartTime.Value;
            var endTime = dtpEndTime.Value;

            if (startTime > endTime)
            {
                MessageBox.Show("시작 시간이 종료 시간보다 늦을 수 없습니다.", "검색 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ClearPreviousHighlights();

            // 종료 시간을 하루 끝으로 설정 (23:59:59)
            var endTimeWithTime = endTime.Date.AddDays(1).AddSeconds(-1);

            foreach (Control c in _flow.Controls)
            {
                if (c is ChatBubbleControl bubble)
                {
                    var t = bubble.Tag as Tuple<string, DateTime, bool, int>;
                    if (t == null) continue;
                    var msgTime = t.Item2;

                    // 시간 범위 내에 있는 메시지 찾기
                    if (msgTime >= startTime && msgTime <= endTimeWithTime)
                    {
                        _searchResults.Add(bubble);
                    }
                }
            }

            if (_searchResults.Count == 0)
            {
                MessageBox.Show("해당 시간 범위에 일치하는 메시지가 없습니다.", "검색", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblSearchCount.Text = "0/0";
                return;
            }

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
                var sentTime = DateTime.Now;

                if (_myUserId == _otherUserId)
                {
                    try
                    {
                        DBManager.Instance.ExecuteNonQuery(
                            "INSERT INTO chat (sender_id, receiver_id, message, created_at, is_read) VALUES (@s,@r,@m,NOW(),1)",
                            new MySqlParameter("@s", _myUserId),
                            new MySqlParameter("@r", _otherUserId),
                            new MySqlParameter("@m", text));
                        var idObj = DBManager.Instance.ExecuteScalar("SELECT LAST_INSERT_ID()");
                        int msgId = (idObj != null && idObj != DBNull.Value) ? Convert.ToInt32(idObj) : 0;
                        AddBubbleImmediate(text, sentTime, true, msgId);
                    }
                    catch { AddBubbleImmediate(text, sentTime, true, 0); }

                    txtChat.Clear();
                    txtChat.Focus();
                    UpdateRecentList();
                    return;
                }

                // 일반 상대에게는 서버로 전송
                var line = "MSG|" + _myUserId + "|" + _otherUserId + "|" + EncodeBase64(text);
                _writer?.WriteLine(line);

                txtChat.Clear();
                txtChat.Focus();
                // Temporary bubble with id 0; will be updated on SENT
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
        }

        private void txtChat_TextChanged(object sender, EventArgs e)
        {
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
        }

        private void btnEmoji_Click(object sender, EventArgs e)
        {
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

                    if (Path.GetExtension(path).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        if (bytes.Length < 4 || bytes[0] != (byte)'P' || bytes[1] != (byte)'K')
                        {
                            MessageBox.Show("선택한 파일은 ZIP 형식이 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    var fileId = SaveFileToDb(_myUserId, _otherUserId, Path.GetFileName(path), bytes);
                    if (fileId <= 0)
                    {
                        MessageBox.Show("파일을 DB에 저장하지 못했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var token = "FILE:" + fileId + ":" + Path.GetFileName(path);

                    if (_myUserId != _otherUserId)
                    {
                        var line = "MSG|" + _myUserId + "|" + _otherUserId + "|" + EncodeBase64(token);
                        _writer?.WriteLine(line);
                    }

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
                var parts = message.Split(new[] { ':' }, 3);
                if (parts.Length == 3 && int.TryParse(parts[1], out int fid))
                {
                    var fname = parts[2];
                    AddBubbleImmediateFile(fid, fname, time, isMine);
                    return;
                }
            }

            var bubble = CreateBubble(message, time, isMine, id);
            if (isMine) bubble.SetRead(false);
            _flow.Controls.Add(bubble);
            _flow.ScrollControlIntoView(bubble);
        }

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
<<<<<<< HEAD

        private void ApplySentMessageId(int messageId)
        {
            // find last my bubble with id 0 and set real id to enable edit/delete context menu
            for (int i = _flow.Controls.Count - 1; i >= 0; i--)
            {
                var b = _flow.Controls[i] as ChatBubbleControl;
                if (b == null) continue;
                var t = b.Tag as Tuple<string, DateTime, bool, int>;
                if (t != null && t.Item3 && t.Item4 == 0)
                {
                    b.Tag = Tuple.Create(t.Item1, t.Item2, t.Item3, messageId);
                    b.SetMessageId(messageId);
                    b.SetData(t.Item1, t.Item2, t.Item3, _flow.ClientSize.Width);
                    break;
                }
=======
        private void AdjustTextBoxHeight()
        {
            if (txtChat == null || pnlBottom == null) return;

            const int minHeight = 25;
            const int maxHeight = 105;
            const int topPadding = 12;
            const int bottomPadding = 13;

            // 현재 텍스트 높이 측정
            int textHeight = minHeight;

            if (!string.IsNullOrEmpty(txtChat.Text))
            {
                Size size = TextRenderer.MeasureText(
                    txtChat.Text + "W",  // 여유 공간
                    txtChat.Font,
                    new Size(txtChat.Width - 20, 0),
                    TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl
                );

                textHeight = Math.Max(minHeight, Math.Min(size.Height, maxHeight));
            }

            // TextBox 높이 변경
            if (txtChat.Height != textHeight)
            {
                txtChat.Height = textHeight;
                pnlBottom.Height = topPadding + textHeight + bottomPadding;
>>>>>>> 3d35fdb5b24dc34abddd7222ec3b71d74001855f
            }
        }
    }
}

