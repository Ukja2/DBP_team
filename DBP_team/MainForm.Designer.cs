namespace DBP_team
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.splitDepartmentsFavorites = new System.Windows.Forms.SplitContainer();
            this.treeViewUser = new System.Windows.Forms.TreeView();
            this.cmsTreeUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.lvFavorites = new System.Windows.Forms.ListView();
            this.cmsFavorites = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenChat = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenLogin = new System.Windows.Forms.Button();
            this.listViewRecent = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLastMsg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.btnSelfChat = new System.Windows.Forms.Button();
            this.btnProfile = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDepartmentsFavorites)).BeginInit();
            this.splitDepartmentsFavorites.Panel1.SuspendLayout();
            this.splitDepartmentsFavorites.Panel2.SuspendLayout();
            this.splitDepartmentsFavorites.SuspendLayout();
            this.cmsTreeUser.SuspendLayout();
            this.cmsFavorites.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(5);
            this.pnlSearch.Size = new System.Drawing.Size(453, 40);
            this.pnlSearch.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(8, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(165, 21);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(179, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(67, 25);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // splitDepartmentsFavorites
            // 
            this.splitDepartmentsFavorites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitDepartmentsFavorites.Location = new System.Drawing.Point(12, 278);
            this.splitDepartmentsFavorites.Name = "splitDepartmentsFavorites";
            this.splitDepartmentsFavorites.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitDepartmentsFavorites.Panel1
            // 
            this.splitDepartmentsFavorites.Panel1.Controls.Add(this.treeViewUser);
            this.splitDepartmentsFavorites.Panel1.Controls.Add(this.pnlSearch);
            // 
            // splitDepartmentsFavorites.Panel2
            // 
            this.splitDepartmentsFavorites.Panel2.Controls.Add(this.lvFavorites);
            this.splitDepartmentsFavorites.Size = new System.Drawing.Size(453, 375);
            this.splitDepartmentsFavorites.SplitterDistance = 220;
            this.splitDepartmentsFavorites.TabIndex = 7;
            // 
            // treeViewUser
            // 
            this.treeViewUser.ContextMenuStrip = this.cmsTreeUser;
            this.treeViewUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewUser.Location = new System.Drawing.Point(0, 40);
            this.treeViewUser.Name = "treeViewUser";
            this.treeViewUser.Size = new System.Drawing.Size(453, 180);
            this.treeViewUser.TabIndex = 0;
            // 
            // cmsTreeUser
            // 
            this.cmsTreeUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddFavorite});
            this.cmsTreeUser.Name = "cmsTreeUser";
            this.cmsTreeUser.Size = new System.Drawing.Size(160, 26);
            // 
            // miAddFavorite
            // 
            this.miAddFavorite.Name = "miAddFavorite";
            this.miAddFavorite.Size = new System.Drawing.Size(159, 22);
            this.miAddFavorite.Text = "즐겨찾기 추가";
            this.miAddFavorite.Click += new System.EventHandler(this.FavoriteSelectedDepartmentNode);
            // 
            // lvFavorites
            // 
            this.lvFavorites.ContextMenuStrip = this.cmsFavorites;
            this.lvFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFavorites.HideSelection = false;
            this.lvFavorites.Location = new System.Drawing.Point(0, 0);
            this.lvFavorites.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvFavorites.Name = "lvFavorites";
            this.lvFavorites.Size = new System.Drawing.Size(453, 151);
            this.lvFavorites.TabIndex = 0;
            this.lvFavorites.UseCompatibleStateImageBehavior = false;
            this.lvFavorites.View = System.Windows.Forms.View.List;
            this.lvFavorites.DoubleClick += new System.EventHandler(this.OpenChatForSelectedFavorite);
            // 
            // cmsFavorites
            // 
            this.cmsFavorites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRemoveFavorite,
            this.miOpenChat});
            this.cmsFavorites.Name = "cmsFavorites";
            this.cmsFavorites.Size = new System.Drawing.Size(160, 48);
            // 
            // miRemoveFavorite
            // 
            this.miRemoveFavorite.Name = "miRemoveFavorite";
            this.miRemoveFavorite.Size = new System.Drawing.Size(159, 22);
            this.miRemoveFavorite.Text = "즐겨찾기 해제";
            this.miRemoveFavorite.Click += new System.EventHandler(this.UnfavoriteSelected);
            // 
            // miOpenChat
            // 
            this.miOpenChat.Name = "miOpenChat";
            this.miOpenChat.Size = new System.Drawing.Size(159, 22);
            this.miOpenChat.Text = "채팅 열기";
            this.miOpenChat.Click += new System.EventHandler(this.OpenChatForSelectedFavorite);
            // 
            // btnOpenLogin
            // 
            this.btnOpenLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenLogin.Location = new System.Drawing.Point(299, 10);
            this.btnOpenLogin.Name = "btnOpenLogin";
            this.btnOpenLogin.Size = new System.Drawing.Size(70, 34);
            this.btnOpenLogin.TabIndex = 5;
            this.btnOpenLogin.Text = "로그인";
            this.btnOpenLogin.UseVisualStyleBackColor = true;
            this.btnOpenLogin.Click += new System.EventHandler(this.btnOpenLogin_Click);
            // 
            // listViewRecent
            // 
            this.listViewRecent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRecent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colLastMsg,
            this.colTime});
            this.listViewRecent.FullRowSelect = true;
            this.listViewRecent.HideSelection = false;
            this.listViewRecent.Location = new System.Drawing.Point(12, 112);
            this.listViewRecent.Name = "listViewRecent";
            this.listViewRecent.Size = new System.Drawing.Size(453, 160);
            this.listViewRecent.TabIndex = 3;
            this.listViewRecent.UseCompatibleStateImageBehavior = false;
            this.listViewRecent.View = System.Windows.Forms.View.Details;
            this.listViewRecent.DoubleClick += new System.EventHandler(this.listViewRecent_DoubleClick);
            // 
            // colName
            // 
            this.colName.Text = "이름";
            this.colName.Width = 120;
            // 
            // colLastMsg
            // 
            this.colLastMsg.Text = "최근메시지";
            this.colLastMsg.Width = 180;
            // 
            // colTime
            // 
            this.colTime.Text = "시간";
            this.colTime.Width = 120;
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(10, 13);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(58, 12);
            this.labelCompany.TabIndex = 1;
            this.labelCompany.Text = "company";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(10, 35);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(30, 12);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "user";
            // 
            // btnSelfChat
            // 
            this.btnSelfChat.Location = new System.Drawing.Point(12, 58);
            this.btnSelfChat.Name = "btnSelfChat";
            this.btnSelfChat.Size = new System.Drawing.Size(83, 23);
            this.btnSelfChat.TabIndex = 3;
            this.btnSelfChat.Text = "나와의 채팅";
            this.btnSelfChat.UseVisualStyleBackColor = true;
            // 
            // btnProfile
            // 
            this.btnProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProfile.Location = new System.Drawing.Point(375, 10);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(66, 34);
            this.btnProfile.TabIndex = 4;
            this.btnProfile.Text = "프로필";
            this.btnProfile.UseVisualStyleBackColor = true;
            this.btnProfile.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.Location = new System.Drawing.Point(299, 51);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(70, 34);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "로그아웃";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.Controls.Add(this.labelCompany);
            this.pnlTop.Controls.Add(this.btnLogout);
            this.pnlTop.Controls.Add(this.labelName);
            this.pnlTop.Controls.Add(this.btnProfile);
            this.pnlTop.Controls.Add(this.btnSelfChat);
            this.pnlTop.Controls.Add(this.btnOpenLogin);
            this.pnlTop.Location = new System.Drawing.Point(12, 12);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(453, 95);
            this.pnlTop.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 665);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.splitDepartmentsFavorites);
            this.Controls.Add(this.listViewRecent);
            this.MinimumSize = new System.Drawing.Size(400, 600);
            this.Name = "MainForm";
            this.Text = "메신저";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.splitDepartmentsFavorites.Panel1.ResumeLayout(false);
            this.splitDepartmentsFavorites.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDepartmentsFavorites)).EndInit();
            this.splitDepartmentsFavorites.ResumeLayout(false);
            this.cmsTreeUser.ResumeLayout(false);
            this.cmsFavorites.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewUser;
        private System.Windows.Forms.ListView listViewRecent;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colLastMsg;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button btnSelfChat;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Button btnOpenLogin;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.SplitContainer splitDepartmentsFavorites;
        private System.Windows.Forms.ListView lvFavorites;
        private System.Windows.Forms.ContextMenuStrip cmsTreeUser;
        private System.Windows.Forms.ToolStripMenuItem miAddFavorite;
        private System.Windows.Forms.ContextMenuStrip cmsFavorites;
        private System.Windows.Forms.ToolStripMenuItem miRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem miOpenChat;
        // --- 새 컨트롤 변수 추가 ---
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel pnlTop;
    }
}