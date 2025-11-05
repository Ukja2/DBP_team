namespace DBP_team
{
    partial class Registerform
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(118, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 40);
            this.label1.TabIndex = 4;
            this.label1.Text = "DBP Talk";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.Color.White;
            this.txtId.Font = new System.Drawing.Font("나눔고딕", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtId.Location = new System.Drawing.Point(75, 198);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(230, 30);
            this.txtId.TabIndex = 5;
            this.txtId.Text = "아이디";
            // 
            // txtPwd
            // 
            this.txtPwd.BackColor = System.Drawing.Color.White;
            this.txtPwd.Font = new System.Drawing.Font("나눔고딕", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPwd.Location = new System.Drawing.Point(75, 234);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(230, 30);
            this.txtPwd.TabIndex = 6;
            this.txtPwd.Text = "비밀번호";
            this.txtPwd.TextChanged += new System.EventHandler(this.txtPwd_TextChanged);
            // 
            // txtPwdCheck
            // 
            this.txtPwdCheck.BackColor = System.Drawing.Color.White;
            this.txtPwdCheck.Font = new System.Drawing.Font("나눔고딕", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPwdCheck.Location = new System.Drawing.Point(75, 270);
            this.txtPwdCheck.Name = "txtPwdCheck";
            this.txtPwdCheck.Size = new System.Drawing.Size(230, 30);
            this.txtPwdCheck.TabIndex = 7;
            this.txtPwdCheck.Text = "비밀번호 확인";
            // 
            // comboCompany
            // 
            this.comboCompany.Font = new System.Drawing.Font("굴림", 15F);
            this.comboCompany.FormattingEnabled = true;
            this.comboCompany.Location = new System.Drawing.Point(75, 339);
            this.comboCompany.Name = "comboCompany";
            this.comboCompany.Size = new System.Drawing.Size(230, 28);
            this.comboCompany.TabIndex = 8;
            this.comboCompany.Text = "회사";
            // 
            // comboTeam
            // 
            this.comboTeam.Font = new System.Drawing.Font("굴림", 15F);
            this.comboTeam.FormattingEnabled = true;
            this.comboTeam.Location = new System.Drawing.Point(75, 373);
            this.comboTeam.Name = "comboTeam";
            this.comboTeam.Size = new System.Drawing.Size(230, 28);
            this.comboTeam.TabIndex = 9;
            this.comboTeam.Text = "부서";
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(75, 440);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(230, 35);
            this.btnRegister.TabIndex = 10;
            this.btnRegister.Text = "회원가입";
            this.btnRegister.UseVisualStyleBackColor = true;
            // 이벤트 연결: 기존 버튼 클릭 핸들러 재사용
            this.btnRegister.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkRegister
            // 
            this.linkRegister.AutoSize = true;
            this.linkRegister.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.linkRegister.Location = new System.Drawing.Point(168, 494);
            this.linkRegister.Name = "linkRegister";
            this.linkRegister.Size = new System.Drawing.Size(41, 12);
            this.linkRegister.TabIndex = 11;
            this.linkRegister.TabStop = true;
            this.linkRegister.Text = "로그인";
            this.linkRegister.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.linkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRegister_LinkClicked);
            // 
            // comboTeam2
            // 
            this.comboTeam2.Font = new System.Drawing.Font("굴림", 15F);
            this.comboTeam2.FormattingEnabled = true;
            this.comboTeam2.Location = new System.Drawing.Point(75, 406);
            this.comboTeam2.Name = "comboTeam2";
            this.comboTeam2.Size = new System.Drawing.Size(230, 28);
            this.comboTeam2.TabIndex = 12;
            this.comboTeam2.Text = "팀";
            this.comboTeam2.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Font = new System.Drawing.Font("나눔고딕", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtName.Location = new System.Drawing.Point(75, 162);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(230, 30);
            this.txtName.TabIndex = 13;
            this.txtName.Text = "이름";
            this.txtName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Registerform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(379, 616);
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
            this.Name = "Registerform";
            this.Text = "Registerform";
            this.Load += new System.EventHandler(this.formRegister_Load);
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
    }
}