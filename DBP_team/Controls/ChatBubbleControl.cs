using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DBP_team.Controls
{
    // 단일 파일 구현: 디자이너 유무와 무관하게 동작
    public partial class ChatBubbleControl : UserControl
    {
        private Panel panelBubble;
        private Label lblMessage;
        private Label lblTime;
        private Label lblRead; // add read indicator
        private Button btnDownload; // added
        private ContextMenuStrip _cms;
        private ToolStripMenuItem _miEdit;
        private ToolStripMenuItem _miDelete;

        private const int SIDE_MARGIN = 12;
        private const int DEFAULT_PADDING_H = 8;
        private const int DEFAULT_PADDING_V = 5;
        private const int TIME_HEIGHT = 16;
        private const int MAX_BUBBLE_PERCENT = 60; // 부모 폭의 %를 최대 폭으로 사용
        private const int MAX_BUBBLE_PIXELS = 420; // 절대 최대 픽셀 폭(원하면 조정)

        private int _fileId = 0;
        private string _fileName = null;
        private int _messageId = 0; // chat table id (0 if not persisted)
        private bool _isMine = false;

        public event Action<int> OnEditRequested; // messageId
        public event Action<int> OnDeleteRequested; // messageId
        public event Action<int, string> OnDownloadRequested; // fileId, filename

        public ChatBubbleControl()
        {
            // 디자이너가 있으면 호출 시도(안전)
            try { InitializeComponent(); } catch { /* ignore */ }

            InitializeComponents();
            this.BackColor = Color.Transparent;
        }

        // 런타임에서 위쪽 패딩 조절 가능
        public int InnerTopPadding
        {
            get => panelBubble?.Padding.Top ?? DEFAULT_PADDING_V;
            set
            {
                if (panelBubble != null)
                    panelBubble.Padding = new Padding(panelBubble.Padding.Left, Math.Max(0, value), panelBubble.Padding.Right, Math.Max(0, value + 2));
            }
        }

        // 런타임에서 좌우 패딩 조절 가능
        public int InnerHorizontalPadding
        {
            get => panelBubble?.Padding.Left ?? DEFAULT_PADDING_H;
            set
            {
                if (panelBubble != null)
                    panelBubble.Padding = new Padding(Math.Max(0, value), panelBubble.Padding.Top, Math.Max(0, value), panelBubble.Padding.Bottom);
            }
        }

        private void InitializeComponents()
        {
            if (panelBubble != null) return; // 이미 초기화됨

            this.panelBubble = new Panel();
            this.lblMessage = new Label();
            this.lblTime = new Label();
            this.lblRead = new Label();
            this.btnDownload = new Button();
            _cms = new ContextMenuStrip();
            _miEdit = new ToolStripMenuItem("수정");
            _miDelete = new ToolStripMenuItem("삭제");
            _cms.Items.AddRange(new ToolStripItem[] { _miEdit, _miDelete });
            _miEdit.Click += (s, e) => { if (_isMine && _messageId > 0) OnEditRequested?.Invoke(_messageId); };
            _miDelete.Click += (s, e) => { if (_isMine && _messageId > 0) OnDeleteRequested?.Invoke(_messageId); };

            // panelBubble 기본 설정
            this.panelBubble.BackColor = Color.FromArgb(240, 240, 240);
            this.panelBubble.Padding = new Padding(DEFAULT_PADDING_H, DEFAULT_PADDING_V, DEFAULT_PADDING_H, DEFAULT_PADDING_V + 2);
            this.panelBubble.AutoSize = true;
            this.panelBubble.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.panelBubble.ContextMenuStrip = _cms;

            // lblMessage
            this.lblMessage.AutoSize = true;
            this.lblMessage.MaximumSize = new Size(300, 0); // 런타임에서 조정
            this.lblMessage.Font = new Font("맑은 고딕", 10F, FontStyle.Regular);
            this.lblMessage.ForeColor = Color.FromArgb(50, 50, 50);

            // lblTime: 오른쪽 정렬을 위해 AutoSize=false, 폭을 패널 폭에 맞춤
            this.lblTime.AutoSize = false;
            this.lblTime.Height = 14;
            this.lblTime.Font = new Font("맑은 고딕", 8F);
            this.lblTime.TextAlign = ContentAlignment.MiddleRight;
            this.lblTime.ForeColor = Color.FromArgb(120, 120, 120);

            // lblRead: 왼쪽 아래 작은 표시
            this.lblRead.AutoSize = true;
            this.lblRead.Font = new Font("맑은 고딕", 8F);
            this.lblRead.Text = string.Empty;
            this.lblRead.ForeColor = Color.FromArgb(120, 120, 120);

            // btnDownload
            this.btnDownload.AutoSize = true;
            this.btnDownload.Text = "다운로드";
            this.btnDownload.Font = new Font("맑은 고딕", 8F);
            this.btnDownload.Visible = false;
            this.btnDownload.Click += BtnDownload_Click;
            this.btnDownload.FlatStyle = FlatStyle.Flat;
            this.btnDownload.FlatAppearance.BorderSize = 1;
            this.btnDownload.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            this.btnDownload.BackColor = Color.White;
            this.btnDownload.ForeColor = Color.FromArgb(80, 80, 80);

            // 조립
            this.panelBubble.Controls.Add(lblMessage);
            this.panelBubble.Controls.Add(lblTime);
            this.panelBubble.Controls.Add(lblRead);
            this.panelBubble.Controls.Add(btnDownload);
            this.Controls.Add(panelBubble);

            // 기본 크기
            this.Width = 360;
            this.Height = panelBubble.Height + 6;

            // 레이아웃 업데이트
            this.Layout += ChatBubbleControl_Layout;
            this.Resize += ChatBubbleControl_Resize;
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (_fileId > 0 && OnDownloadRequested != null)
            {
                OnDownloadRequested.Invoke(_fileId, _fileName ?? "file");
            }
        }

        private void ChatBubbleControl_Resize(object sender, EventArgs e)
        {
            UpdateBubbleRegion();
        }

        private void ChatBubbleControl_Layout(object sender, LayoutEventArgs e)
        {
            // 반드시 패널 패딩을 텍스트의 시작 좌표로 사용하도록 명시
            lblMessage.Location = new Point(panelBubble.Padding.Left, panelBubble.Padding.Top);

            // lblTime 위치 및 폭: 말풍선 내부 아래, 폭은 panelBubble 내부폭으로 맞추고 오른쪽 정렬
            int innerWidth = Math.Max(1, panelBubble.ClientSize.Width - panelBubble.Padding.Horizontal);
            int timeTop = panelBubble.Padding.Top + lblMessage.Height + 4;

            // place read label to the right of time, on the same line, right aligned
            int readWidth = lblRead.Visible ? lblRead.PreferredWidth : 0;
            lblRead.Location = new Point(panelBubble.Padding.Left + innerWidth - readWidth, timeTop);

            int timeWidth = Math.Max(0, innerWidth - (readWidth > 0 ? (readWidth + 6) : 0));
            lblTime.Size = new Size(timeWidth, lblTime.Height);
            lblTime.Location = new Point(panelBubble.Padding.Left, timeTop);

            // btnDownload 위치: 시간 왼쪽에 배치
            btnDownload.Location = new Point(panelBubble.Padding.Left, lblTime.Top + lblTime.Height + 4);

            // panelBubble 높이 자동 확대
            int extra = btnDownload.Visible ? (btnDownload.Height + 6) : 4;
            panelBubble.Height = panelBubble.Padding.Vertical + lblMessage.Height + lblTime.Height + extra;

            // 컨트롤 높이
            this.Height = panelBubble.Height + 6;

            UpdateBubbleRegion();
        }

        private void UpdateBubbleRegion()
        {
            // 둥근 모서리 적용
            int radius = Math.Max(0, 8); // 필요 시 기본 반지름 변경
            var rect = new Rectangle(0, 0, Math.Max(1, panelBubble.Width), Math.Max(1, panelBubble.Height));
            using (var gp = RoundedRect(rect, radius))
            {
                if (panelBubble.Region != null) panelBubble.Region.Dispose();
                panelBubble.Region = new Region(gp);
            }
        }

        private GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var gp = new GraphicsPath();
            if (radius <= 0)
            {
                gp.AddRectangle(r);
                return gp;
            }

            var d = radius * 2;
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        /// <summary>
        /// 메시지/시간/발신자 정보 설정.
        /// caller는 bubble.Width = flow.ClientSize.Width 로 설정한 뒤 호출하세요.
        /// </summary>
        public void SetData(string message, DateTime time, bool isMine, int containerWidth)
        {
            EnsureInit();

            lblMessage.Text = message ?? string.Empty;
            lblTime.Text = time.ToString("yyyy-MM-dd HH:mm");
            _isMine = isMine;

            // 1) 계산: 퍼센트 기반 폭 + 픽셀 캡 적용 → 절대 최대 폭 결정
            int pctWidth = Math.Max(60, containerWidth * MAX_BUBBLE_PERCENT / 100);
            int maxBubbleWidth = Math.Min(pctWidth, MAX_BUBBLE_PIXELS);

            // 2) lblMessage 줄바꿈을 위해 최대 텍스트 폭 설정 (패딩 고려)
            int textMaxWidth = Math.Max(10, maxBubbleWidth - panelBubble.Padding.Horizontal);
            lblMessage.MaximumSize = new Size(textMaxWidth, 0);

            // 3) 강제 레이아웃 갱신 → panelBubble.PreferredSize 계산
            lblMessage.AutoSize = true;
            lblMessage.PerformLayout();

            lblTime.AutoSize = false;
            lblTime.Height = 14;

            panelBubble.PerformLayout();

            // 4) panelBubble 실제 폭을 제한: preferredWidth + padding 또는 maxBubbleWidth 중 작은 값
            int preferredWidth = panelBubble.PreferredSize.Width;
            int finalBubbleWidth = Math.Min(maxBubbleWidth, Math.Max(preferredWidth, 40));
            panelBubble.Width = finalBubbleWidth; // 고정 폭(더 이상 부모보다 넘치지 않음)

            // 5) lblTime 폭을 panel 내부폭으로 맞춤(오른쪽 정렬 유지)
            int innerWidth = Math.Max(1, panelBubble.ClientSize.Width - panelBubble.Padding.Horizontal);
            lblTime.Size = new Size(innerWidth, lblTime.Height);
            // read label stays small

            // 6) 컨트롤 전체 너비 보장(호출자에서 bubble.Width는 flow.ClientSize.Width로 설정되어야 함)
            if (this.Width < containerWidth) this.Width = containerWidth;

            // 7) 좌/우 정렬 및 스타일
            if (isMine)
            {
                panelBubble.Left = Math.Max(SIDE_MARGIN, this.ClientSize.Width - SIDE_MARGIN - panelBubble.Width);
                panelBubble.BackColor = Color.FromArgb(0, 132, 255);
                lblMessage.ForeColor = Color.White;
                lblTime.ForeColor = Color.FromArgb(200, 255, 255, 255);
                lblRead.ForeColor = Color.FromArgb(200, 255, 255, 255);
            }
            else
            {
                panelBubble.Left = SIDE_MARGIN;
                panelBubble.BackColor = Color.FromArgb(240, 240, 240);
                lblMessage.ForeColor = Color.FromArgb(50, 50, 50);
                lblTime.ForeColor = Color.FromArgb(120, 120, 120);
                lblRead.ForeColor = Color.FromArgb(120, 120, 120);
            }

            // 8) 높이 재조정
            panelBubble.Height = panelBubble.Padding.Vertical + lblMessage.Height + lblTime.Height + 4 + (btnDownload.Visible ? btnDownload.Height + 6 : 0);
            this.Height = panelBubble.Height + 6;

            UpdateBubbleRegion();
            Invalidate();

            // context menu: enable only for own text messages (not file token)
            bool isFile = !string.IsNullOrEmpty(message) && message.StartsWith("FILE:", StringComparison.OrdinalIgnoreCase);
            _miEdit.Enabled = _isMine && !isFile && _messageId > 0;
            _miDelete.Enabled = _isMine && _messageId > 0;
        }

        public void SetMessageId(int id)
        {
            _messageId = id;
            // update menu enabled state
            _miEdit.Enabled = _isMine && _messageId > 0;
            _miDelete.Enabled = _isMine && _messageId > 0;
        }

        private void EnsureInit()
        {
            if (panelBubble == null) InitializeComponents();
        }

        private void ChatBubbleControl_Load(object sender, EventArgs e)
        {
            // 빈 핸들러(디자이너가 연결할 수 있음)
        }

        public void SetRead(bool isRead)
        {
            lblRead.Text = isRead ? "읽음" : string.Empty;
            lblRead.Visible = isRead;
            this.PerformLayout();
        }

        public void SetFile(int fileId, string fileName)
        {
            _fileId = fileId;
            _fileName = fileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                lblMessage.Text = fileName;
            }
            btnDownload.Visible = true;
            this.PerformLayout();
        }
    }
}