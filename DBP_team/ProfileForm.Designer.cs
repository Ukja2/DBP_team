namespace DBP_team
{
    partial class ProfileForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureProfile = new System.Windows.Forms.PictureBox();
            this.labelFullName = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelDepartment = new System.Windows.Forms.Label();
            this.labelTeam = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnChangeImage = new System.Windows.Forms.Button();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtNickname = new System.Windows.Forms.TextBox();
            this.labelNickname = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelPostalCode = new System.Windows.Forms.Label();
            this.txtAddressMain = new System.Windows.Forms.TextBox();
            this.txtAddressDetail = new System.Windows.Forms.TextBox();
            this.btnAddressSearch = new System.Windows.Forms.Button();
            this.btnMultiProfiles = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.ChangePWbtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureProfile
            // 
            this.pictureProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pictureProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureProfile.Location = new System.Drawing.Point(15, 15);
            this.pictureProfile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureProfile.Name = "pictureProfile";
            this.pictureProfile.Size = new System.Drawing.Size(120, 120);
            this.pictureProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureProfile.TabIndex = 0;
            this.pictureProfile.TabStop = false;
            // 
            // labelFullName
            // 
            this.labelFullName.AutoSize = true;
            this.labelFullName.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.labelFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelFullName.Location = new System.Drawing.Point(150, 18);
            this.labelFullName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFullName.Name = "labelFullName";
            this.labelFullName.Size = new System.Drawing.Size(31, 15);
            this.labelFullName.TabIndex = 1;
            this.labelFullName.Text = "이름";
            this.labelFullName.Click += new System.EventHandler(this.labelFullName_Click);
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.labelEmail.Location = new System.Drawing.Point(150, 90);
            this.labelEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(82, 15);
            this.labelEmail.TabIndex = 2;
            this.labelEmail.Text = "이메일: (없음)";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelCompany.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.labelCompany.Location = new System.Drawing.Point(150, 110);
            this.labelCompany.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(70, 15);
            this.labelCompany.TabIndex = 3;
            this.labelCompany.Text = "회사: (없음)";
            // 
            // labelDepartment
            // 
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelDepartment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.labelDepartment.Location = new System.Drawing.Point(280, 110);
            this.labelDepartment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(70, 15);
            this.labelDepartment.TabIndex = 4;
            this.labelDepartment.Text = "부서: (없음)";
            // 
            // labelTeam
            // 
            this.labelTeam.AutoSize = true;
            this.labelTeam.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelTeam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.labelTeam.Location = new System.Drawing.Point(410, 110);
            this.labelTeam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(58, 15);
            this.labelTeam.TabIndex = 5;
            this.labelTeam.Text = "팀: (없음)";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnClose.Location = new System.Drawing.Point(430, 420);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 35);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnChangeImage
            // 
            this.btnChangeImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnChangeImage.FlatAppearance.BorderSize = 0;
            this.btnChangeImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeImage.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnChangeImage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnChangeImage.Location = new System.Drawing.Point(27, 143);
            this.btnChangeImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnChangeImage.Name = "btnChangeImage";
            this.btnChangeImage.Size = new System.Drawing.Size(96, 28);
            this.btnChangeImage.TabIndex = 9;
            this.btnChangeImage.Text = "이미지 변경";
            this.btnChangeImage.UseVisualStyleBackColor = false;
            this.btnChangeImage.Click += new System.EventHandler(this.btnChangeImage_Click);
            // 
            // txtFullName
            // 
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtFullName.Location = new System.Drawing.Point(205, 15);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(300, 25);
            this.txtFullName.TabIndex = 11;
            // 
            // txtNickname
            // 
            this.txtNickname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNickname.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtNickname.Location = new System.Drawing.Point(205, 50);
            this.txtNickname.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(300, 25);
            this.txtNickname.TabIndex = 12;
            // 
            // labelNickname
            // 
            this.labelNickname.AutoSize = true;
            this.labelNickname.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.labelNickname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelNickname.Location = new System.Drawing.Point(150, 53);
            this.labelNickname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNickname.Name = "labelNickname";
            this.labelNickname.Size = new System.Drawing.Size(31, 15);
            this.labelNickname.TabIndex = 13;
            this.labelNickname.Text = "별명";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(330, 420);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 35);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelPostalCode
            // 
            this.labelPostalCode.AutoSize = true;
            this.labelPostalCode.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.labelPostalCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelPostalCode.Location = new System.Drawing.Point(20, 230);
            this.labelPostalCode.Name = "labelPostalCode";
            this.labelPostalCode.Size = new System.Drawing.Size(67, 15);
            this.labelPostalCode.TabIndex = 15;
            this.labelPostalCode.Text = "우편번호: -";
            // 
            // txtAddressMain
            // 
            this.txtAddressMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.txtAddressMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddressMain.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.txtAddressMain.Location = new System.Drawing.Point(20, 255);
            this.txtAddressMain.Multiline = true;
            this.txtAddressMain.Name = "txtAddressMain";
            this.txtAddressMain.ReadOnly = true;
            this.txtAddressMain.Size = new System.Drawing.Size(380, 50);
            this.txtAddressMain.TabIndex = 16;
            // 
            // txtAddressDetail
            // 
            this.txtAddressDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddressDetail.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.txtAddressDetail.Location = new System.Drawing.Point(20, 315);
            this.txtAddressDetail.Name = "txtAddressDetail";
            this.txtAddressDetail.Size = new System.Drawing.Size(380, 23);
            this.txtAddressDetail.TabIndex = 17;
            this.txtAddressDetail.Visible = false;
            this.txtAddressDetail.TextChanged += new System.EventHandler(this.txtAddressDetail_TextChanged);
            // 
            // btnAddressSearch
            // 
            this.btnAddressSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnAddressSearch.FlatAppearance.BorderSize = 0;
            this.btnAddressSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddressSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnAddressSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnAddressSearch.Location = new System.Drawing.Point(410, 255);
            this.btnAddressSearch.Name = "btnAddressSearch";
            this.btnAddressSearch.Size = new System.Drawing.Size(110, 50);
            this.btnAddressSearch.TabIndex = 18;
            this.btnAddressSearch.Text = "주소 변경";
            this.btnAddressSearch.UseVisualStyleBackColor = false;
            this.btnAddressSearch.Click += new System.EventHandler(this.btnAddressSearch_Click);
            // 
            // btnMultiProfiles
            // 
            this.btnMultiProfiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnMultiProfiles.FlatAppearance.BorderSize = 0;
            this.btnMultiProfiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMultiProfiles.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnMultiProfiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnMultiProfiles.Location = new System.Drawing.Point(20, 360);
            this.btnMultiProfiles.Name = "btnMultiProfiles";
            this.btnMultiProfiles.Size = new System.Drawing.Size(150, 32);
            this.btnMultiProfiles.TabIndex = 19;
            this.btnMultiProfiles.Text = "멀티프로필 관리";
            this.btnMultiProfiles.UseVisualStyleBackColor = false;
            this.btnMultiProfiles.Click += new System.EventHandler(this.btnMultiProfiles_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.ChangePWbtn);
            this.pnlTop.Controls.Add(this.pictureProfile);
            this.pnlTop.Controls.Add(this.labelFullName);
            this.pnlTop.Controls.Add(this.txtFullName);
            this.pnlTop.Controls.Add(this.labelNickname);
            this.pnlTop.Controls.Add(this.txtNickname);
            this.pnlTop.Controls.Add(this.labelEmail);
            this.pnlTop.Controls.Add(this.labelCompany);
            this.pnlTop.Controls.Add(this.labelDepartment);
            this.pnlTop.Controls.Add(this.labelTeam);
            this.pnlTop.Controls.Add(this.btnChangeImage);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(540, 190);
            this.pnlTop.TabIndex = 20;
            // 
            // ChangePWbtn
            // 
            this.ChangePWbtn.Location = new System.Drawing.Point(413, 143);
            this.ChangePWbtn.Name = "ChangePWbtn";
            this.ChangePWbtn.Size = new System.Drawing.Size(107, 28);
            this.ChangePWbtn.TabIndex = 14;
            this.ChangePWbtn.Text = "비밀번호 변경";
            this.ChangePWbtn.UseVisualStyleBackColor = true;
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(540, 470);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btnMultiProfiles);
            this.Controls.Add(this.btnAddressSearch);
            this.Controls.Add(this.txtAddressDetail);
            this.Controls.Add(this.txtAddressMain);
            this.Controls.Add(this.labelPostalCode);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DBP Talk - 프로필";
            this.Load += new System.EventHandler(this.ProfileForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureProfile;
        private System.Windows.Forms.Label labelFullName;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelDepartment;
        private System.Windows.Forms.Label labelTeam;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnChangeImage;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.TextBox txtNickname;
        private System.Windows.Forms.Label labelNickname;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelPostalCode;
        private System.Windows.Forms.TextBox txtAddressMain;
        private System.Windows.Forms.TextBox txtAddressDetail;
        private System.Windows.Forms.Button btnAddressSearch;
        private System.Windows.Forms.Button btnMultiProfiles;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button ChangePWbtn;
    }
}