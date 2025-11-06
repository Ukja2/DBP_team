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
            this.btnOpenLogin = new System.Windows.Forms.Button();
            this.treeViewUser = new System.Windows.Forms.TreeView();
            this.listViewRecent = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLastMsg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.btnSelfChat = new System.Windows.Forms.Button();
            this.btnProfile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeViewUser
            // 
            this.treeViewUser.Location = new System.Drawing.Point(34, 301);
            this.treeViewUser.Name = "treeViewUser";
            this.treeViewUser.Size = new System.Drawing.Size(394, 253);
            this.treeViewUser.TabIndex = 0;
            // 
            // listViewRecent
            // 
            this.listViewRecent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colLastMsg,
            this.colTime});
            this.listViewRecent.FullRowSelect = true;
            this.listViewRecent.HideSelection = false;
            this.listViewRecent.Location = new System.Drawing.Point(34, 115);
            this.listViewRecent.Name = "listViewRecent";
            this.listViewRecent.Size = new System.Drawing.Size(394, 180);
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
            this.colLastMsg.Width = 140;
            // 
            // colTime
            // 
            this.colTime.Text = "시간";
            this.colTime.Width = 120;
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(32, 18);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(58, 12);
            this.labelCompany.TabIndex = 1;
            this.labelCompany.Text = "company";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(32, 40);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(30, 12);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "user";
            // 
            // btnSelfChat
            // 
            this.btnSelfChat.Location = new System.Drawing.Point(34, 65);
            this.btnSelfChat.Name = "btnSelfChat";
            this.btnSelfChat.Size = new System.Drawing.Size(83, 23);
            this.btnSelfChat.TabIndex = 3;
            this.btnSelfChat.Text = "나와의 채팅";
            this.btnSelfChat.UseVisualStyleBackColor = true;
            // 
            // btnProfile
            // 
            this.btnProfile.Location = new System.Drawing.Point(292, 18);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(50, 34);
            this.btnProfile.TabIndex = 4;
            this.btnProfile.Text = "프로필";
            this.btnProfile.UseVisualStyleBackColor = true;
            this.btnProfile.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOpenLogin
            // 
            this.btnOpenLogin.Location = new System.Drawing.Point(216, 18);
            this.btnOpenLogin.Name = "btnOpenLogin";
            this.btnOpenLogin.Size = new System.Drawing.Size(70, 34);
            this.btnOpenLogin.TabIndex = 5;
            this.btnOpenLogin.Text = "로그인";
            this.btnOpenLogin.UseVisualStyleBackColor = true;
            this.btnOpenLogin.Click += new System.EventHandler(this.btnOpenLogin_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 665);
            this.Controls.Add(this.btnProfile);
            this.Controls.Add(this.btnOpenLogin);
            this.Controls.Add(this.btnSelfChat);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.listViewRecent);
            this.Controls.Add(this.treeViewUser);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}