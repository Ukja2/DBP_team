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
            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSearch.Size = new System.Drawing.Size(484, 50);
            this.pnlSearch.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtSearch.Location = new System.Drawing.Point(13, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(370, 25);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.Text = "직원 검색...";
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(393, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(78, 25);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // splitDepartmentsFavorites
            // 
            this.splitDepartmentsFavorites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitDepartmentsFavorites.Location = new System.Drawing.Point(12, 305);
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
            this.splitDepartmentsFavorites.Size = new System.Drawing.Size(484, 390);
            this.splitDepartmentsFavorites.SplitterDistance = 240;
            this.splitDepartmentsFavorites.TabIndex = 7;
            // 
            // treeViewUser
            // 
            this.treeViewUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewUser.ContextMenuStrip = this.cmsTreeUser;
            this.treeViewUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewUser.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.treeViewUser.Location = new System.Drawing.Point(0, 50);
            this.treeViewUser.Name = "treeViewUser";
            this.treeViewUser.Size = new System.Drawing.Size(484, 190);
            this.treeViewUser.TabIndex = 0;
            // 
            // cmsTreeUser
            // 
            this.cmsTreeUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddFavorite});
            this.cmsTreeUser.Name = "cmsTreeUser";
            this.cmsTreeUser.Size = new System.Drawing.Size(151, 26);
            // 
            // miAddFavorite
            // 
            this.miAddFavorite.Name = "miAddFavorite";
            this.miAddFavorite.Size = new System.Drawing.Size(150, 22);
            this.miAddFavorite.Text = "즐겨찾기 추가";
            this.miAddFavorite.Click += new System.EventHandler(this.FavoriteSelectedDepartmentNode);
            // 
            // lvFavorites
            // 
            this.lvFavorites.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvFavorites.ContextMenuStrip = this.cmsFavorites;
            this.lvFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFavorites.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lvFavorites.HideSelection = false;
            this.lvFavorites.Location = new System.Drawing.Point(0, 0);
            this.lvFavorites.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvFavorites.Name = "lvFavorites";
            this.lvFavorites.Size = new System.Drawing.Size(484, 146);
            this.lvFavorites.TabIndex = 0;
            this.lvFavorites.UseCompatibleStateImageBehavior = false;
            this.lvFavorites.View = System.Windows.Forms.View.List;
            this.lvFavorites.SelectedIndexChanged += new System.EventHandler(this.lvFavorites_SelectedIndexChanged);
            this.lvFavorites.DoubleClick += new System.EventHandler(this.OpenChatForSelectedFavorite);
            // 
            // cmsFavorites
            // 
            this.cmsFavorites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRemoveFavorite,
            this.miOpenChat});
            this.cmsFavorites.Name = "cmsFavorites";
            this.cmsFavorites.Size = new System.Drawing.Size(151, 48);
            // 
            // miRemoveFavorite
            // 
            this.miRemoveFavorite.Name = "miRemoveFavorite";
            this.miRemoveFavorite.Size = new System.Drawing.Size(150, 22);
            this.miRemoveFavorite.Text = "즐겨찾기 해제";
            this.miRemoveFavorite.Click += new System.EventHandler(this.UnfavoriteSelected);
            // 
            // miOpenChat
            // 
            this.miOpenChat.Name = "miOpenChat";
            this.miOpenChat.Size = new System.Drawing.Size(150, 22);
            this.miOpenChat.Text = "채팅 열기";
            this.miOpenChat.Click += new System.EventHandler(this.OpenChatForSelectedFavorite);
            // 
            // btnOpenLogin
            // 
            this.btnOpenLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnOpenLogin.FlatAppearance.BorderSize = 0;
            this.btnOpenLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenLogin.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnOpenLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnOpenLogin.Location = new System.Drawing.Point(375, 51);
            this.btnOpenLogin.Name = "btnOpenLogin";
            this.btnOpenLogin.Size = new System.Drawing.Size(96, 34);
            this.btnOpenLogin.TabIndex = 5;
            this.btnOpenLogin.Text = "관리자 모드";
            this.btnOpenLogin.UseVisualStyleBackColor = false;
            this.btnOpenLogin.Click += new System.EventHandler(this.btnOpenLogin_Click);
            // 
            // listViewRecent
            // 
            this.listViewRecent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRecent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewRecent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colLastMsg,
            this.colTime});
            this.listViewRecent.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.listViewRecent.FullRowSelect = true;
            this.listViewRecent.HideSelection = false;
            this.listViewRecent.Location = new System.Drawing.Point(12, 130);
            this.listViewRecent.Name = "listViewRecent";
            this.listViewRecent.Size = new System.Drawing.Size(484, 174);
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
            this.colLastMsg.Width = 230;
            // 
            // colTime
            // 
            this.colTime.Text = "시간";
            this.colTime.Width = 110;
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelCompany.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.labelCompany.Location = new System.Drawing.Point(10, 13);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(71, 19);
            this.labelCompany.TabIndex = 1;
            this.labelCompany.Text = "company";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.labelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.labelName.Location = new System.Drawing.Point(10, 37);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(42, 21);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "user";
            // 
            // btnSelfChat
            // 
            this.btnSelfChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnSelfChat.FlatAppearance.BorderSize = 0;
            this.btnSelfChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelfChat.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnSelfChat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnSelfChat.Location = new System.Drawing.Point(12, 67);
            this.btnSelfChat.Name = "btnSelfChat";
            this.btnSelfChat.Size = new System.Drawing.Size(95, 30);
            this.btnSelfChat.TabIndex = 3;
            this.btnSelfChat.Text = "나와의 채팅";
            this.btnSelfChat.UseVisualStyleBackColor = false;
            // 
            // btnProfile
            // 
            this.btnProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.btnProfile.FlatAppearance.BorderSize = 0;
            this.btnProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfile.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnProfile.ForeColor = System.Drawing.Color.White;
            this.btnProfile.Location = new System.Drawing.Point(399, 10);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(72, 35);
            this.btnProfile.TabIndex = 4;
            this.btnProfile.Text = "프로필";
            this.btnProfile.UseVisualStyleBackColor = false;
            this.btnProfile.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnLogout.Location = new System.Drawing.Point(273, 51);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(96, 34);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "로그아웃";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.labelCompany);
            this.pnlTop.Controls.Add(this.btnLogout);
            this.pnlTop.Controls.Add(this.labelName);
            this.pnlTop.Controls.Add(this.btnProfile);
            this.pnlTop.Controls.Add(this.btnSelfChat);
            this.pnlTop.Controls.Add(this.btnOpenLogin);
            this.pnlTop.Location = new System.Drawing.Point(12, 12);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(484, 105);
            this.pnlTop.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(508, 712);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.splitDepartmentsFavorites);
            this.Controls.Add(this.listViewRecent);
            this.MinimumSize = new System.Drawing.Size(450, 650);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DBP Talk";
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
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel pnlTop;
    }
}