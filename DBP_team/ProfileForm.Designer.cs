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
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureProfile
            // 
            this.pictureProfile.Location = new System.Drawing.Point(14, 11);
            this.pictureProfile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureProfile.Name = "pictureProfile";
            this.pictureProfile.Size = new System.Drawing.Size(140, 111);
            this.pictureProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureProfile.TabIndex = 0;
            this.pictureProfile.TabStop = false;
            // 
            // labelFullName
            // 
            this.labelFullName.AutoSize = true;
            this.labelFullName.Location = new System.Drawing.Point(162, 15);
            this.labelFullName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFullName.Name = "labelFullName";
            this.labelFullName.Size = new System.Drawing.Size(37, 12);
            this.labelFullName.TabIndex = 1;
            this.labelFullName.Text = "이름 :";
            this.labelFullName.Click += new System.EventHandler(this.labelFullName_Click);
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(175, 74);
            this.labelEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(83, 12);
            this.labelEmail.TabIndex = 2;
            this.labelEmail.Text = "이메일: (없음)";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(175, 99);
            this.labelCompany.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(71, 12);
            this.labelCompany.TabIndex = 3;
            this.labelCompany.Text = "회사: (없음)";
            // 
            // labelDepartment
            // 
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Location = new System.Drawing.Point(175, 126);
            this.labelDepartment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(71, 12);
            this.labelDepartment.TabIndex = 4;
            this.labelDepartment.Text = "부서: (없음)";
            // 
            // labelTeam
            // 
            this.labelTeam.AutoSize = true;
            this.labelTeam.Location = new System.Drawing.Point(175, 151);
            this.labelTeam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(59, 12);
            this.labelTeam.TabIndex = 5;
            this.labelTeam.Text = "팀: (없음)";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(392, 312);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 24);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnChangeImage
            // 
            this.btnChangeImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeImage.Location = new System.Drawing.Point(25, 128);
            this.btnChangeImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnChangeImage.Name = "btnChangeImage";
            this.btnChangeImage.Size = new System.Drawing.Size(110, 24);
            this.btnChangeImage.TabIndex = 9;
            this.btnChangeImage.Text = "이미지 변경";
            this.btnChangeImage.UseVisualStyleBackColor = true;
            this.btnChangeImage.Click += new System.EventHandler(this.btnChangeImage_Click);
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(213, 12);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(267, 21);
            this.txtFullName.TabIndex = 11;
            // 
            // txtNickname
            // 
            this.txtNickname.Location = new System.Drawing.Point(213, 38);
            this.txtNickname.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(267, 21);
            this.txtNickname.TabIndex = 12;
            // 
            // labelNickname
            // 
            this.labelNickname.AutoSize = true;
            this.labelNickname.Location = new System.Drawing.Point(162, 41);
            this.labelNickname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNickname.Name = "labelNickname";
            this.labelNickname.Size = new System.Drawing.Size(37, 12);
            this.labelNickname.TabIndex = 13;
            this.labelNickname.Text = "별명 :";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(296, 312);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 24);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelPostalCode
            // 
            this.labelPostalCode.AutoSize = true;
            this.labelPostalCode.Location = new System.Drawing.Point(12, 171);
            this.labelPostalCode.Name = "labelPostalCode";
            this.labelPostalCode.Size = new System.Drawing.Size(67, 12);
            this.labelPostalCode.TabIndex = 15;
            this.labelPostalCode.Text = "우편번호: -";
            // 
            // txtAddressMain
            // 
            this.txtAddressMain.Location = new System.Drawing.Point(12, 186);
            this.txtAddressMain.Multiline = true;
            this.txtAddressMain.Name = "txtAddressMain";
            this.txtAddressMain.ReadOnly = true;
            this.txtAddressMain.Size = new System.Drawing.Size(350, 42);
            this.txtAddressMain.TabIndex = 16;
            // 
            // txtAddressDetail
            // 
            this.txtAddressDetail.Location = new System.Drawing.Point(14, 234);
            this.txtAddressDetail.Name = "txtAddressDetail";
            this.txtAddressDetail.Size = new System.Drawing.Size(350, 21);
            this.txtAddressDetail.TabIndex = 17;
            this.txtAddressDetail.Visible = false;
            this.txtAddressDetail.TextChanged += new System.EventHandler(this.txtAddressDetail_TextChanged);
            // 
            // btnAddressSearch
            // 
            this.btnAddressSearch.Location = new System.Drawing.Point(373, 186);
            this.btnAddressSearch.Name = "btnAddressSearch";
            this.btnAddressSearch.Size = new System.Drawing.Size(108, 49);
            this.btnAddressSearch.TabIndex = 18;
            this.btnAddressSearch.Text = "주소 변경";
            this.btnAddressSearch.UseVisualStyleBackColor = true;
            this.btnAddressSearch.Click += new System.EventHandler(this.btnAddressSearch_Click);
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 349);
            this.Controls.Add(this.btnAddressSearch);
            this.Controls.Add(this.txtAddressDetail);
            this.Controls.Add(this.txtAddressMain);
            this.Controls.Add(this.labelPostalCode);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnChangeImage);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelTeam);
            this.Controls.Add(this.labelDepartment);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelNickname);
            this.Controls.Add(this.txtNickname);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.labelFullName);
            this.Controls.Add(this.pictureProfile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "프로필";
            this.Load += new System.EventHandler(this.ProfileForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).EndInit();
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
    }
}