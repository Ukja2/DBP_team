namespace DBP_team
{
    partial class Registerform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being being used.
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtPwdCheck = new System.Windows.Forms.TextBox();
            this.comboCompany = new System.Windows.Forms.ComboBox();
            this.comboTeam = new System.Windows.Forms.ComboBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.linkRegister = new System.Windows.Forms.LinkLabel();
            this.comboTeam2 = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtNickname = new System.Windows.Forms.TextBox();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.txtAddressSearch = new System.Windows.Forms.TextBox();
            this.btnAddressSearch = new System.Windows.Forms.Button();
            this.txtAddressDetail = new System.Windows.Forms.TextBox();
            this.pictureProfile = new System.Windows.Forms.PictureBox();
            this.btnChooseImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.label1.Location = new System.Drawing.Point(140, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 45);
            this.label1.TabIndex = 4;
            this.label1.Text = "회원가입";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.Color.White;
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtId.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.txtId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtId.Location = new System.Drawing.Point(50, 140);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(340, 27);
            this.txtId.TabIndex = 5;
            this.txtId.Text = "아이디";
            // 
            // txtPwd
            // 
            this.txtPwd.BackColor = System.Drawing.Color.White;
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwd.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.txtPwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtPwd.Location = new System.Drawing.Point(50, 180);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(340, 27);
            this.txtPwd.TabIndex = 6;
            this.txtPwd.Text = "비밀번호";
            this.txtPwd.TextChanged += new System.EventHandler(this.txtPwd_TextChanged);
            // 
            // txtPwdCheck
            // 
            this.txtPwdCheck.BackColor = System.Drawing.Color.White;
            this.txtPwdCheck.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwdCheck.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.txtPwdCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtPwdCheck.Location = new System.Drawing.Point(50, 220);
            this.txtPwdCheck.Name = "txtPwdCheck";
            this.txtPwdCheck.Size = new System.Drawing.Size(340, 27);
            this.txtPwdCheck.TabIndex = 7;
            this.txtPwdCheck.Text = "비밀번호 확인";
            // 
            // comboCompany
            // 
            this.comboCompany.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboCompany.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.comboCompany.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.comboCompany.FormattingEnabled = true;
            this.comboCompany.Location = new System.Drawing.Point(50, 480);
            this.comboCompany.Name = "comboCompany";
            this.comboCompany.Size = new System.Drawing.Size(340, 25);
            this.comboCompany.TabIndex = 8;
            this.comboCompany.Text = "회사";
            // 
            // comboTeam
            // 
            this.comboTeam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboTeam.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.comboTeam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.comboTeam.FormattingEnabled = true;
            this.comboTeam.Location = new System.Drawing.Point(50, 515);
            this.comboTeam.Name = "comboTeam";
            this.comboTeam.Size = new System.Drawing.Size(340, 25);
            this.comboTeam.TabIndex = 9;
            this.comboTeam.Text = "부서";
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(50, 590);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(340, 45);
            this.btnRegister.TabIndex = 10;
            this.btnRegister.Text = "회원가입";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkRegister
            // 
            this.linkRegister.AutoSize = true;
            this.linkRegister.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.linkRegister.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.linkRegister.Location = new System.Drawing.Point(195, 650);
            this.linkRegister.Name = "linkRegister";
            this.linkRegister.Size = new System.Drawing.Size(43, 15);
            this.linkRegister.TabIndex = 11;
            this.linkRegister.TabStop = true;
            this.linkRegister.Text = "로그인";
            this.linkRegister.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.linkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRegister_LinkClicked);
            // 
            // comboTeam2
            // 
            this.comboTeam2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboTeam2.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.comboTeam2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.comboTeam2.FormattingEnabled = true;
            this.comboTeam2.Location = new System.Drawing.Point(50, 550);
            this.comboTeam2.Name = "comboTeam2";
            this.comboTeam2.Size = new System.Drawing.Size(340, 25);
            this.comboTeam2.TabIndex = 12;
            this.comboTeam2.Text = "팀";
            this.comboTeam2.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtName.Location = new System.Drawing.Point(50, 100);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(200, 27);
            this.txtName.TabIndex = 13;
            this.txtName.Text = "이름";
            this.txtName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtNickname
            // 
            this.txtNickname.BackColor = System.Drawing.Color.White;
            this.txtNickname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNickname.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtNickname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.txtNickname.Location = new System.Drawing.Point(260, 100);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(130, 25);
            this.txtNickname.TabIndex = 14;
            this.txtNickname.Text = "별명 (선택)";
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.BackColor = System.Drawing.Color.White;
            this.txtPostalCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPostalCode.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtPostalCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtPostalCode.Location = new System.Drawing.Point(50, 390);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.ReadOnly = true;
            this.txtPostalCode.Size = new System.Drawing.Size(100, 25);
            this.txtPostalCode.TabIndex = 15;
            this.txtPostalCode.Text = "우편번호";
            // 
            // txtAddressSearch
            // 
            this.txtAddressSearch.BackColor = System.Drawing.Color.White;
            this.txtAddressSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddressSearch.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtAddressSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtAddressSearch.Location = new System.Drawing.Point(160, 390);
            this.txtAddressSearch.Name = "txtAddressSearch";
            this.txtAddressSearch.ReadOnly = true;
            this.txtAddressSearch.Size = new System.Drawing.Size(150, 25);
            this.txtAddressSearch.TabIndex = 16;
            this.txtAddressSearch.Text = "주소";
            // 
            // btnAddressSearch
            // 
            this.btnAddressSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnAddressSearch.FlatAppearance.BorderSize = 0;
            this.btnAddressSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddressSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnAddressSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnAddressSearch.Location = new System.Drawing.Point(320, 390);
            this.btnAddressSearch.Name = "btnAddressSearch";
            this.btnAddressSearch.Size = new System.Drawing.Size(70, 25);
            this.btnAddressSearch.TabIndex = 17;
            this.btnAddressSearch.Text = "주소검색";
            this.btnAddressSearch.UseVisualStyleBackColor = false;
            this.btnAddressSearch.Click += new System.EventHandler(this.btnAddressSearch_Click);
            // 
            // txtAddressDetail
            // 
            this.txtAddressDetail.BackColor = System.Drawing.Color.White;
            this.txtAddressDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddressDetail.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtAddressDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtAddressDetail.Location = new System.Drawing.Point(50, 425);
            this.txtAddressDetail.Name = "txtAddressDetail";
            this.txtAddressDetail.Size = new System.Drawing.Size(340, 25);
            this.txtAddressDetail.TabIndex = 18;
            this.txtAddressDetail.Text = "상세 주소";
            // 
            // pictureProfile
            // 
            this.pictureProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pictureProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureProfile.Location = new System.Drawing.Point(50, 270);
            this.pictureProfile.Name = "pictureProfile";
            this.pictureProfile.Size = new System.Drawing.Size(100, 100);
            this.pictureProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureProfile.TabIndex = 19;
            this.pictureProfile.TabStop = false;
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnChooseImage.FlatAppearance.BorderSize = 0;
            this.btnChooseImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseImage.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnChooseImage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnChooseImage.Location = new System.Drawing.Point(160, 310);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(80, 30);
            this.btnChooseImage.TabIndex = 20;
            this.btnChooseImage.Text = "사진 선택";
            this.btnChooseImage.UseVisualStyleBackColor = false;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);
            // 
            // Registerform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(40, 711);
            this.Controls.Add(this.btnChooseImage);
            this.Controls.Add(this.pictureProfile);
            this.Controls.Add(this.txtAddressDetail);
            this.Controls.Add(this.btnAddressSearch);
            this.Controls.Add(this.txtAddressSearch);
            this.Controls.Add(this.txtPostalCode);
            this.Controls.Add(this.txtNickname);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.comboTeam2);
            this.Controls.Add(this.linkRegister);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.comboTeam);
            this.Controls.Add(this.comboCompany);
            this.Controls.Add(this.txtPwdCheck);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(455, 750);
            this.Name = "Registerform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DBP Talk - 회원가입";
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtPwdCheck;
        private System.Windows.Forms.ComboBox comboCompany;
        private System.Windows.Forms.ComboBox comboTeam;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.LinkLabel linkRegister;
        private System.Windows.Forms.ComboBox comboTeam2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtNickname;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.TextBox txtAddressSearch;
        private System.Windows.Forms.Button btnAddressSearch;
        private System.Windows.Forms.TextBox txtAddressDetail;
        private System.Windows.Forms.PictureBox pictureProfile;
        private System.Windows.Forms.Button btnChooseImage;
    }
}