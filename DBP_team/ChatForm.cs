using System;
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

        public ChatForm(int myUserId, int otherUserId, string otherName)
        {
            InitializeComponent();

            _myUserId = myUserId;
            _otherUserId = otherUserId;
            _otherName = string.IsNullOrWhiteSpace(otherName) ? "상대" : otherName;

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
        }

        private void ConnectToChatServer()
        {
            try
            {
                _tcp = new TcpClient();
                _tcp.Connect("127.0.0.1", 9000);
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
                MessageBox.Show("채팅 서버 연결 실패: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private ChatBubbleControl CreateBubble(string message, DateTime time, bool isMine, int id)
        {
            var bubble = new ChatBubbleControl();
            bubble.Width = Math.Max(200, _flow.ClientSize.Width);
            bubble.Tag = Tuple.Create(message, time, isMine, id);
            bubble.SetData(message, time, isMine, _flow.ClientSize.Width);
            bubble.Margin = new Padding(0, 6, 0, 6);
            return bubble;
        }

        private void AddBubbleImmediate(string message, DateTime time, bool isMine, int id)
        {
            var bubble = CreateBubble(message, time, isMine, id);
            if (isMine) bubble.SetRead(false); // until READ received
            _flow.Controls.Add(bubble);
            _flow.ScrollControlIntoView(bubble);
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
    }
}
