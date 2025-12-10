using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace DBP_team
{
    partial class AdminForm
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
            this._tabs = new System.Windows.Forms.TabControl();
            this.pageDept = new System.Windows.Forms.TabPage();
            this._gridDept = new System.Windows.Forms.DataGridView();
            this.pnlDeptTop = new System.Windows.Forms.Panel();
            this.lblDeptName = new System.Windows.Forms.Label();
            this._txtDeptName = new System.Windows.Forms.TextBox();
            this._btnDeptAdd = new System.Windows.Forms.Button();
            this._btnDeptUpdate = new System.Windows.Forms.Button();
            this.lblDeptSearch = new System.Windows.Forms.Label();
            this._txtDeptSearch = new System.Windows.Forms.TextBox();
            this._btnDeptSearch = new System.Windows.Forms.Button();
            this.pageUserDept = new System.Windows.Forms.TabPage();
            this._gridUsers = new System.Windows.Forms.DataGridView();
            this.pnlUserTop = new System.Windows.Forms.Panel();
            this.lblUserSearch = new System.Windows.Forms.Label();
            this._txtUserSearch = new System.Windows.Forms.TextBox();
            this._btnUserSearch = new System.Windows.Forms.Button();
            this.lblDeptSelect = new System.Windows.Forms.Label();
            this._cboDeptForUser = new System.Windows.Forms.ComboBox();
            this.lblTeamSelect = new System.Windows.Forms.Label();
            this._cboTeamForUser = new System.Windows.Forms.ComboBox();
            this._btnApplyDept = new System.Windows.Forms.Button();
            this.pageTeam = new System.Windows.Forms.TabPage();
            this._gridTeam = new System.Windows.Forms.DataGridView();
            this.pnlTeamTop = new System.Windows.Forms.Panel();
            this.lblTeamDeptSelect = new System.Windows.Forms.Label();
            this._cboDeptForTeam = new System.Windows.Forms.ComboBox();
            this.lblTeamName = new System.Windows.Forms.Label();
            this._txtTeamName = new System.Windows.Forms.TextBox();
            this._btnTeamAdd2 = new System.Windows.Forms.Button();
            this._btnTeamUpdate = new System.Windows.Forms.Button();
            this.lblTeamSearch = new System.Windows.Forms.Label();
            this._txtTeamSearch = new System.Windows.Forms.TextBox();
            this._btnTeamSearch = new System.Windows.Forms.Button();
            this.pageChat = new System.Windows.Forms.TabPage();
            this._gridChat = new System.Windows.Forms.DataGridView();
            this.pnlChatTop = new System.Windows.Forms.Panel();
            this.lblFrom = new System.Windows.Forms.Label();
            this._dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this._dtTo = new System.Windows.Forms.DateTimePicker();
            this.lblKeyword = new System.Windows.Forms.Label();
            this._txtKeyword = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this._cboUserFilter = new System.Windows.Forms.ComboBox();
            this._btnChatSearch = new System.Windows.Forms.Button();
            this.pageAccessLogs = new System.Windows.Forms.TabPage();
            this._gridLogs = new System.Windows.Forms.DataGridView();
            this.pnlSearch = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.lblDateSeparator = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.lblUser_log = new System.Windows.Forms.Label();
            this.txtSearchUser = new System.Windows.Forms.TextBox();
            this.btnSearchLog = new System.Windows.Forms.Button();
            this.pagePermission = new System.Windows.Forms.TabPage();
            this._tvVisibility = new System.Windows.Forms.TreeView();
            this._pnlPermissionTop = new System.Windows.Forms.FlowLayoutPanel();
            this.labelViewer = new System.Windows.Forms.Label();
            this._cboViewer = new System.Windows.Forms.ComboBox();
            this._btnPermSave = new System.Windows.Forms.Button();
            this._btnPermReset = new System.Windows.Forms.Button();
            this.pageChatBan = new System.Windows.Forms.TabPage();
            this._lvBans = new System.Windows.Forms.ListView();
            this._pnlBanTop = new System.Windows.Forms.FlowLayoutPanel();
            this.labelUserA = new System.Windows.Forms.Label();
            this._cbUser1 = new System.Windows.Forms.ComboBox();
            this.labelUserB = new System.Windows.Forms.Label();
            this._cbUser2 = new System.Windows.Forms.ComboBox();
            this._btnBlock = new System.Windows.Forms.Button();
            this._btnUnblock = new System.Windows.Forms.Button();
            this._tabs.SuspendLayout();
            this.pageDept.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridDept)).BeginInit();
            this.pnlDeptTop.SuspendLayout();
            this.pageUserDept.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridUsers)).BeginInit();
            this.pnlUserTop.SuspendLayout();
            this.pageTeam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridTeam)).BeginInit();
            this.pnlTeamTop.SuspendLayout();
            this.pageChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridChat)).BeginInit();
            this.pnlChatTop.SuspendLayout();
            this.pageAccessLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridLogs)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pagePermission.SuspendLayout();
            this._pnlPermissionTop.SuspendLayout();
            this.pageChatBan.SuspendLayout();
            this._pnlBanTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabs
            // 
            this._tabs.Controls.Add(this.pageDept);
            this._tabs.Controls.Add(this.pageTeam);
            this._tabs.Controls.Add(this.pageUserDept);
            this._tabs.Controls.Add(this.pageChat);
            this._tabs.Controls.Add(this.pageAccessLogs);
            this._tabs.Controls.Add(this.pagePermission);
            this._tabs.Controls.Add(this.pageChatBan);
            this._tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabs.Location = new System.Drawing.Point(0, 0);
            this._tabs.Name = "_tabs";
            this._tabs.SelectedIndex = 0;
            this._tabs.Size = new System.Drawing.Size(1024, 720);
            this._tabs.TabIndex = 0;
            // 
            // pageDept
            // 
            this.pageDept.Controls.Add(this._gridDept);
            this.pageDept.Controls.Add(this.pnlDeptTop);
            this.pageDept.Location = new System.Drawing.Point(4, 24);
            this.pageDept.Name = "pageDept";
            this.pageDept.Padding = new System.Windows.Forms.Padding(3);
            this.pageDept.Size = new System.Drawing.Size(1016, 692);
            this.pageDept.TabIndex = 0;
            this.pageDept.Text = "부서관리";
            this.pageDept.UseVisualStyleBackColor = true;
            // 
            // _gridDept
            // 
            this._gridDept.AllowUserToAddRows = false;
            this._gridDept.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridDept.BackgroundColor = System.Drawing.Color.White;
            this._gridDept.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._gridDept.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridDept.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridDept.Location = new System.Drawing.Point(3, 63);
            this._gridDept.MultiSelect = false;
            this._gridDept.Name = "_gridDept";
            this._gridDept.ReadOnly = true;
            this._gridDept.RowHeadersWidth = 51;
            this._gridDept.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridDept.Size = new System.Drawing.Size(1010, 626);
            this._gridDept.TabIndex = 1;
            this._gridDept.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DeptGrid_CellClick);
            // 
            // pnlDeptTop
            // 
            this.pnlDeptTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlDeptTop.Controls.Add(this.lblDeptName);
            this.pnlDeptTop.Controls.Add(this._txtDeptName);
            this.pnlDeptTop.Controls.Add(this._btnDeptAdd);
            this.pnlDeptTop.Controls.Add(this._btnDeptUpdate);
            this.pnlDeptTop.Controls.Add(this.lblDeptSearch);
            this.pnlDeptTop.Controls.Add(this._txtDeptSearch);
            this.pnlDeptTop.Controls.Add(this._btnDeptSearch);
            this.pnlDeptTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDeptTop.Location = new System.Drawing.Point(3, 3);
            this.pnlDeptTop.Name = "pnlDeptTop";
            this.pnlDeptTop.Padding = new System.Windows.Forms.Padding(8);
            this.pnlDeptTop.Size = new System.Drawing.Size(1010, 60);
            this.pnlDeptTop.TabIndex = 0;
            // 
            // lblDeptName
            // 
            this.lblDeptName.AutoSize = true;
            this.lblDeptName.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblDeptName.Location = new System.Drawing.Point(8, 12);
            this.lblDeptName.Name = "lblDeptName";
            this.lblDeptName.Size = new System.Drawing.Size(43, 15);
            this.lblDeptName.TabIndex = 0;
            this.lblDeptName.Text = "부서명";
            // 
            // _txtDeptName
            // 
            this._txtDeptName.Location = new System.Drawing.Point(60, 8);
            this._txtDeptName.Name = "_txtDeptName";
            this._txtDeptName.Size = new System.Drawing.Size(220, 23);
            this._txtDeptName.TabIndex = 1;
            // 
            // _btnDeptAdd
            // 
            this._btnDeptAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnDeptAdd.Location = new System.Drawing.Point(845, 6);
            this._btnDeptAdd.Name = "_btnDeptAdd";
            this._btnDeptAdd.Size = new System.Drawing.Size(154, 25);
            this._btnDeptAdd.TabIndex = 2;
            this._btnDeptAdd.Text = "부서 추가";
            this._btnDeptAdd.Click += new System.EventHandler(this.DeptAdd_Click);
            // 
            // _btnDeptUpdate
            // 
            this._btnDeptUpdate.Location = new System.Drawing.Point(306, 6);
            this._btnDeptUpdate.Name = "_btnDeptUpdate";
            this._btnDeptUpdate.Size = new System.Drawing.Size(80, 25);
            this._btnDeptUpdate.TabIndex = 3;
            this._btnDeptUpdate.Text = "수정";
            this._btnDeptUpdate.Click += new System.EventHandler(this.DeptUpdate_Click);
            // 
            // lblDeptSearch
            // 
            this.lblDeptSearch.AutoSize = true;
            this.lblDeptSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblDeptSearch.Location = new System.Drawing.Point(423, 12);
            this.lblDeptSearch.Name = "lblDeptSearch";
            this.lblDeptSearch.Size = new System.Drawing.Size(75, 15);
            this.lblDeptSearch.TabIndex = 4;
            this.lblDeptSearch.Text = "검색(부서명)";
            // 
            // _txtDeptSearch
            // 
            this._txtDeptSearch.Location = new System.Drawing.Point(504, 7);
            this._txtDeptSearch.Name = "_txtDeptSearch";
            this._txtDeptSearch.Size = new System.Drawing.Size(220, 23);
            this._txtDeptSearch.TabIndex = 5;
            // 
            // _btnDeptSearch
            // 
            this._btnDeptSearch.Location = new System.Drawing.Point(730, 6);
            this._btnDeptSearch.Name = "_btnDeptSearch";
            this._btnDeptSearch.Size = new System.Drawing.Size(99, 25);
            this._btnDeptSearch.TabIndex = 6;
            this._btnDeptSearch.Text = "검색";
            this._btnDeptSearch.Click += new System.EventHandler(this.DeptSearch_Click);
            // 
            // pageUserDept
            // 
            this.pageUserDept.Controls.Add(this._gridUsers);
            this.pageUserDept.Controls.Add(this.pnlUserTop);
            this.pageUserDept.Location = new System.Drawing.Point(4, 24);
            this.pageUserDept.Name = "pageUserDept";
            this.pageUserDept.Padding = new System.Windows.Forms.Padding(3);
            this.pageUserDept.Size = new System.Drawing.Size(1016, 692);
            this.pageUserDept.TabIndex = 1;
            this.pageUserDept.Text = "사용자 소속 관리";
            this.pageUserDept.UseVisualStyleBackColor = true;
            // 
            // _gridUsers
            // 
            this._gridUsers.AllowUserToAddRows = false;
            this._gridUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridUsers.BackgroundColor = System.Drawing.Color.White;
            this._gridUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._gridUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridUsers.Location = new System.Drawing.Point(3, 63);
            this._gridUsers.Name = "_gridUsers";
            this._gridUsers.ReadOnly = true;
            this._gridUsers.RowHeadersWidth = 51;
            this._gridUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridUsers.Size = new System.Drawing.Size(1010, 626);
            this._gridUsers.TabIndex = 1;
            // 
            // pnlUserTop
            // 
            this.pnlUserTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pnlUserTop.Controls.Add(this.lblUserSearch);
            this.pnlUserTop.Controls.Add(this._txtUserSearch);
            this.pnlUserTop.Controls.Add(this._btnUserSearch);
            this.pnlUserTop.Controls.Add(this.lblDeptSelect);
            this.pnlUserTop.Controls.Add(this._cboDeptForUser);
            this.pnlUserTop.Controls.Add(this.lblTeamSelect);
            this.pnlUserTop.Controls.Add(this._cboTeamForUser);
            this.pnlUserTop.Controls.Add(this._btnApplyDept);
            this.pnlUserTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUserTop.Location = new System.Drawing.Point(3, 3);
            this.pnlUserTop.Name = "pnlUserTop";
            this.pnlUserTop.Padding = new System.Windows.Forms.Padding(8);
            this.pnlUserTop.Size = new System.Drawing.Size(1010, 60);
            this.pnlUserTop.TabIndex = 0;
            // 
            // lblUserSearch
            // 
            this.lblUserSearch.AutoSize = true;
            this.lblUserSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblUserSearch.Location = new System.Drawing.Point(8, 12);
            this.lblUserSearch.Name = "lblUserSearch";
            this.lblUserSearch.Size = new System.Drawing.Size(71, 15);
            this.lblUserSearch.TabIndex = 0;
            this.lblUserSearch.Text = "사용자 검색";
            // 
            // _txtUserSearch
            // 
            this._txtUserSearch.Location = new System.Drawing.Point(80, 8);
            this._txtUserSearch.Name = "_txtUserSearch";
            this._txtUserSearch.Size = new System.Drawing.Size(240, 23);
            this._txtUserSearch.TabIndex = 1;
            // 
            // _btnUserSearch
            // 
            this._btnUserSearch.Location = new System.Drawing.Point(325, 7);
            this._btnUserSearch.Name = "_btnUserSearch";
            this._btnUserSearch.Size = new System.Drawing.Size(80, 25);
            this._btnUserSearch.TabIndex = 2;
            this._btnUserSearch.Text = "검색";
            this._btnUserSearch.Click += new System.EventHandler(this.UserSearch_Click);
            // 
            // lblDeptSelect
            // 
            this.lblDeptSelect.AutoSize = true;
            this.lblDeptSelect.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblDeptSelect.Location = new System.Drawing.Point(420, 12);
            this.lblDeptSelect.Name = "lblDeptSelect";
            this.lblDeptSelect.Size = new System.Drawing.Size(59, 15);
            this.lblDeptSelect.TabIndex = 3;
            this.lblDeptSelect.Text = "부서 선택";
            // 
            // _cboDeptForUser
            // 
            this._cboDeptForUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboDeptForUser.Location = new System.Drawing.Point(480, 8);
            this._cboDeptForUser.Name = "_cboDeptForUser";
            this._cboDeptForUser.Size = new System.Drawing.Size(125, 23);
            this._cboDeptForUser.TabIndex = 4;
            // 
            // lblTeamSelect
            // 
            this.lblTeamSelect.AutoSize = true;
            this.lblTeamSelect.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblTeamSelect.Location = new System.Drawing.Point(611, 11);
            this.lblTeamSelect.Name = "lblTeamSelect";
            this.lblTeamSelect.Size = new System.Drawing.Size(47, 15);
            this.lblTeamSelect.TabIndex = 6;
            this.lblTeamSelect.Text = "팀 선택";
            this.lblTeamSelect.Click += new System.EventHandler(this.lblTeamSelect_Click);
            // 
            // _cboTeamForUser
            // 
            this._cboTeamForUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboTeamForUser.Location = new System.Drawing.Point(664, 7);
            this._cboTeamForUser.Name = "_cboTeamForUser";
            this._cboTeamForUser.Size = new System.Drawing.Size(141, 23);
            this._cboTeamForUser.TabIndex = 7;
            // 
            // _btnApplyDept
            // 
            this._btnApplyDept.Location = new System.Drawing.Point(829, 6);
            this._btnApplyDept.Name = "_btnApplyDept";
            this._btnApplyDept.Size = new System.Drawing.Size(170, 25);
            this._btnApplyDept.TabIndex = 5;
            this._btnApplyDept.Text = "선택된 사용자 소속 변경";
            this._btnApplyDept.Click += new System.EventHandler(this.ApplyDept_Click);
            // 
            // pageTeam
            // 
            this.pageTeam.Controls.Add(this._gridTeam);
            this.pageTeam.Controls.Add(this.pnlTeamTop);
            this.pageTeam.Location = new System.Drawing.Point(4, 24);
            this.pageTeam.Name = "pageTeam";
            this.pageTeam.Padding = new System.Windows.Forms.Padding(3);
            this.pageTeam.Size = new System.Drawing.Size(1016, 692);
            this.pageTeam.TabIndex = 6;
            this.pageTeam.Text = "팀관리";
            this.pageTeam.UseVisualStyleBackColor = true;
            // 
            // _gridTeam
            // 
            this._gridTeam.AllowUserToAddRows = false;
            this._gridTeam.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridTeam.BackgroundColor = System.Drawing.Color.White;
            this._gridTeam.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._gridTeam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridTeam.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridTeam.Location = new System.Drawing.Point(3, 63);
            this._gridTeam.MultiSelect = false;
            this._gridTeam.Name = "_gridTeam";
            this._gridTeam.ReadOnly = true;
            this._gridTeam.RowHeadersWidth = 51;
            this._gridTeam.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridTeam.Size = new System.Drawing.Size(1010, 626);
            this._gridTeam.TabIndex = 1;
            this._gridTeam.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TeamGrid_CellClick);
            // 
            // pnlTeamTop
            // 
            this.pnlTeamTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlTeamTop.Controls.Add(this.lblTeamDeptSelect);
            this.pnlTeamTop.Controls.Add(this._cboDeptForTeam);
            this.pnlTeamTop.Controls.Add(this.lblTeamName);
            this.pnlTeamTop.Controls.Add(this._txtTeamName);
            this.pnlTeamTop.Controls.Add(this._btnTeamAdd2);
            this.pnlTeamTop.Controls.Add(this._btnTeamUpdate);
            this.pnlTeamTop.Controls.Add(this.lblTeamSearch);
            this.pnlTeamTop.Controls.Add(this._txtTeamSearch);
            this.pnlTeamTop.Controls.Add(this._btnTeamSearch);
            this.pnlTeamTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTeamTop.Location = new System.Drawing.Point(3, 3);
            this.pnlTeamTop.Name = "pnlTeamTop";
            this.pnlTeamTop.Padding = new System.Windows.Forms.Padding(8);
            this.pnlTeamTop.Size = new System.Drawing.Size(1010, 60);
            this.pnlTeamTop.TabIndex = 0;
            // 
            // lblTeamDeptSelect
            // 
            this.lblTeamDeptSelect.AutoSize = true;
            this.lblTeamDeptSelect.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblTeamDeptSelect.Location = new System.Drawing.Point(8, 12);
            this.lblTeamDeptSelect.Name = "lblTeamDeptSelect";
            this.lblTeamDeptSelect.Size = new System.Drawing.Size(59, 15);
            this.lblTeamDeptSelect.TabIndex = 0;
            this.lblTeamDeptSelect.Text = "부서 선택";
            // 
            // _cboDeptForTeam
            // 
            this._cboDeptForTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboDeptForTeam.Location = new System.Drawing.Point(80, 8);
            this._cboDeptForTeam.Name = "_cboDeptForTeam";
            this._cboDeptForTeam.Size = new System.Drawing.Size(200, 23);
            this._cboDeptForTeam.TabIndex = 1;
            this._cboDeptForTeam.SelectedIndexChanged += new System.EventHandler(this.TeamDept_SelectedIndexChanged);
            // 
            // lblTeamName
            // 
            this.lblTeamName.AutoSize = true;
            this.lblTeamName.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblTeamName.Location = new System.Drawing.Point(290, 12);
            this.lblTeamName.Name = "lblTeamName";
            this.lblTeamName.Size = new System.Drawing.Size(31, 15);
            this.lblTeamName.TabIndex = 2;
            this.lblTeamName.Text = "팀명";
            // 
            // _txtTeamName
            // 
            this._txtTeamName.Location = new System.Drawing.Point(330, 8);
            this._txtTeamName.Name = "_txtTeamName";
            this._txtTeamName.Size = new System.Drawing.Size(180, 23);
            this._txtTeamName.TabIndex = 3;
            // 
            // _btnTeamAdd2
            // 
            this._btnTeamAdd2.Location = new System.Drawing.Point(515, 7);
            this._btnTeamAdd2.Name = "_btnTeamAdd2";
            this._btnTeamAdd2.Size = new System.Drawing.Size(80, 25);
            this._btnTeamAdd2.TabIndex = 4;
            this._btnTeamAdd2.Text = "추가";
            this._btnTeamAdd2.Click += new System.EventHandler(this.TeamAdd2_Click);
            // 
            // _btnTeamUpdate
            // 
            this._btnTeamUpdate.Location = new System.Drawing.Point(600, 7);
            this._btnTeamUpdate.Name = "_btnTeamUpdate";
            this._btnTeamUpdate.Size = new System.Drawing.Size(80, 25);
            this._btnTeamUpdate.TabIndex = 5;
            this._btnTeamUpdate.Text = "수정";
            this._btnTeamUpdate.Click += new System.EventHandler(this.TeamUpdate_Click);
            // 
            // lblTeamSearch
            // 
            this.lblTeamSearch.AutoSize = true;
            this.lblTeamSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblTeamSearch.Location = new System.Drawing.Point(770, 12);
            this.lblTeamSearch.Name = "lblTeamSearch";
            this.lblTeamSearch.Size = new System.Drawing.Size(63, 15);
            this.lblTeamSearch.TabIndex = 7;
            this.lblTeamSearch.Text = "검색(팀명)";
            // 
            // _txtTeamSearch
            // 
            this._txtTeamSearch.Location = new System.Drawing.Point(845, 8);
            this._txtTeamSearch.Name = "_txtTeamSearch";
            this._txtTeamSearch.Size = new System.Drawing.Size(120, 23);
            this._txtTeamSearch.TabIndex = 8;
            // 
            // _btnTeamSearch
            // 
            this._btnTeamSearch.Location = new System.Drawing.Point(970, 7);
            this._btnTeamSearch.Name = "_btnTeamSearch";
            this._btnTeamSearch.Size = new System.Drawing.Size(40, 25);
            this._btnTeamSearch.TabIndex = 9;
            this._btnTeamSearch.Text = "검색";
            this._btnTeamSearch.Click += new System.EventHandler(this.TeamSearch_Click);
            // 
            // pageChat
            // 
            this.pageChat.Controls.Add(this._gridChat);
            this.pageChat.Controls.Add(this.pnlChatTop);
            this.pageChat.Location = new System.Drawing.Point(4, 24);
            this.pageChat.Name = "pageChat";
            this.pageChat.Padding = new System.Windows.Forms.Padding(3);
            this.pageChat.Size = new System.Drawing.Size(1016, 692);
            this.pageChat.TabIndex = 2;
            this.pageChat.Text = "대화 검색";
            this.pageChat.UseVisualStyleBackColor = true;
            // 
            // _gridChat
            // 
            this._gridChat.AllowUserToAddRows = false;
            this._gridChat.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridChat.BackgroundColor = System.Drawing.Color.White;
            this._gridChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._gridChat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridChat.Location = new System.Drawing.Point(3, 73);
            this._gridChat.MultiSelect = false;
            this._gridChat.Name = "_gridChat";
            this._gridChat.ReadOnly = true;
            this._gridChat.RowHeadersWidth = 51;
            this._gridChat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridChat.Size = new System.Drawing.Size(1010, 616);
            this._gridChat.TabIndex = 1;
            // 
            // pnlChatTop
            // 
            this.pnlChatTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlChatTop.Controls.Add(this.lblFrom);
            this.pnlChatTop.Controls.Add(this._dtFrom);
            this.pnlChatTop.Controls.Add(this.lblTo);
            this.pnlChatTop.Controls.Add(this._dtTo);
            this.pnlChatTop.Controls.Add(this.lblKeyword);
            this.pnlChatTop.Controls.Add(this._txtKeyword);
            this.pnlChatTop.Controls.Add(this.lblUser);
            this.pnlChatTop.Controls.Add(this._cboUserFilter);
            this.pnlChatTop.Controls.Add(this._btnChatSearch);
            this.pnlChatTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChatTop.Location = new System.Drawing.Point(3, 3);
            this.pnlChatTop.Name = "pnlChatTop";
            this.pnlChatTop.Padding = new System.Windows.Forms.Padding(8);
            this.pnlChatTop.Size = new System.Drawing.Size(1010, 70);
            this.pnlChatTop.TabIndex = 0;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblFrom.Location = new System.Drawing.Point(8, 12);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(43, 15);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "시작일";
            // 
            // _dtFrom
            // 
            this._dtFrom.Location = new System.Drawing.Point(60, 8);
            this._dtFrom.Name = "_dtFrom";
            this._dtFrom.Size = new System.Drawing.Size(140, 23);
            this._dtFrom.TabIndex = 1;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblTo.Location = new System.Drawing.Point(210, 12);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(43, 15);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "종료일";
            // 
            // _dtTo
            // 
            this._dtTo.Location = new System.Drawing.Point(260, 8);
            this._dtTo.Name = "_dtTo";
            this._dtTo.Size = new System.Drawing.Size(140, 23);
            this._dtTo.TabIndex = 3;
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblKeyword.Location = new System.Drawing.Point(410, 12);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(43, 15);
            this.lblKeyword.TabIndex = 4;
            this.lblKeyword.Text = "키워드";
            // 
            // _txtKeyword
            // 
            this._txtKeyword.Location = new System.Drawing.Point(455, 8);
            this._txtKeyword.Name = "_txtKeyword";
            this._txtKeyword.Size = new System.Drawing.Size(220, 23);
            this._txtKeyword.TabIndex = 5;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblUser.Location = new System.Drawing.Point(685, 12);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(43, 15);
            this.lblUser.TabIndex = 6;
            this.lblUser.Text = "사용자";
            // 
            // _cboUserFilter
            // 
            this._cboUserFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboUserFilter.Location = new System.Drawing.Point(730, 8);
            this._cboUserFilter.Name = "_cboUserFilter";
            this._cboUserFilter.Size = new System.Drawing.Size(200, 23);
            this._cboUserFilter.TabIndex = 7;
            // 
            // _btnChatSearch
            // 
            this._btnChatSearch.Location = new System.Drawing.Point(940, 7);
            this._btnChatSearch.Name = "_btnChatSearch";
            this._btnChatSearch.Size = new System.Drawing.Size(60, 25);
            this._btnChatSearch.TabIndex = 8;
            this._btnChatSearch.Text = "검색";
            this._btnChatSearch.Click += new System.EventHandler(this.ChatSearch_Click);
            // 
            // pageAccessLogs
            // 
            this.pageAccessLogs.Controls.Add(this._gridLogs);
            this.pageAccessLogs.Controls.Add(this.pnlSearch);
            this.pageAccessLogs.Location = new System.Drawing.Point(4, 24);
            this.pageAccessLogs.Name = "pageAccessLogs";
            this.pageAccessLogs.Padding = new System.Windows.Forms.Padding(3);
            this.pageAccessLogs.Size = new System.Drawing.Size(1016, 692);
            this.pageAccessLogs.TabIndex = 3;
            this.pageAccessLogs.Text = "접속 이력";
            this.pageAccessLogs.UseVisualStyleBackColor = true;
            // 
            // _gridLogs
            // 
            this._gridLogs.AllowUserToAddRows = false;
            this._gridLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridLogs.BackgroundColor = System.Drawing.Color.White;
            this._gridLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._gridLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridLogs.Location = new System.Drawing.Point(3, 63);
            this._gridLogs.MultiSelect = false;
            this._gridLogs.Name = "_gridLogs";
            this._gridLogs.ReadOnly = true;
            this._gridLogs.RowHeadersWidth = 51;
            this._gridLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridLogs.Size = new System.Drawing.Size(1010, 626);
            this._gridLogs.TabIndex = 1;
            this._gridLogs.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridLogs_CellClick);
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlSearch.Controls.Add(this.lblDate);
            this.pnlSearch.Controls.Add(this.dtpStart);
            this.pnlSearch.Controls.Add(this.lblDateSeparator);
            this.pnlSearch.Controls.Add(this.dtpEnd);
            this.pnlSearch.Controls.Add(this.lblUser_log);
            this.pnlSearch.Controls.Add(this.txtSearchUser);
            this.pnlSearch.Controls.Add(this.btnSearchLog);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(3, 3);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(8);
            this.pnlSearch.Size = new System.Drawing.Size(1010, 60);
            this.pnlSearch.TabIndex = 0;
            // 
            // lblDate
            // 
            this.lblDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblDate.Location = new System.Drawing.Point(11, 16);
            this.lblDate.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(34, 15);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "기간:";
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(48, 11);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(100, 23);
            this.dtpStart.TabIndex = 1;
            // 
            // lblDateSeparator
            // 
            this.lblDateSeparator.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDateSeparator.AutoSize = true;
            this.lblDateSeparator.Location = new System.Drawing.Point(154, 16);
            this.lblDateSeparator.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.lblDateSeparator.Name = "lblDateSeparator";
            this.lblDateSeparator.Size = new System.Drawing.Size(15, 15);
            this.lblDateSeparator.TabIndex = 2;
            this.lblDateSeparator.Text = "~";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(172, 11);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(100, 23);
            this.dtpEnd.TabIndex = 3;
            // 
            // lblUser_log
            // 
            this.lblUser_log.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUser_log.AutoSize = true;
            this.lblUser_log.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblUser_log.Location = new System.Drawing.Point(290, 16);
            this.lblUser_log.Margin = new System.Windows.Forms.Padding(15, 6, 0, 3);
            this.lblUser_log.Name = "lblUser_log";
            this.lblUser_log.Size = new System.Drawing.Size(46, 15);
            this.lblUser_log.TabIndex = 4;
            this.lblUser_log.Text = "사용자:";
            // 
            // txtSearchUser
            // 
            this.txtSearchUser.Location = new System.Drawing.Point(339, 11);
            this.txtSearchUser.Name = "txtSearchUser";
            this.txtSearchUser.Size = new System.Drawing.Size(150, 23);
            this.txtSearchUser.TabIndex = 5;
            // 
            // btnSearchLog
            // 
            this.btnSearchLog.Location = new System.Drawing.Point(502, 8);
            this.btnSearchLog.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.btnSearchLog.Name = "btnSearchLog";
            this.btnSearchLog.Size = new System.Drawing.Size(80, 25);
            this.btnSearchLog.TabIndex = 6;
            this.btnSearchLog.Text = "검색";
            this.btnSearchLog.Click += new System.EventHandler(this.SearchLog_Click);
            // 
            // pagePermission
            // 
            this.pagePermission.Controls.Add(this._tvVisibility);
            this.pagePermission.Controls.Add(this._pnlPermissionTop);
            this.pagePermission.Location = new System.Drawing.Point(4, 24);
            this.pagePermission.Name = "pagePermission";
            this.pagePermission.Padding = new System.Windows.Forms.Padding(3);
            this.pagePermission.Size = new System.Drawing.Size(1016, 692);
            this.pagePermission.TabIndex = 4;
            this.pagePermission.Text = "권한 설정";
            this.pagePermission.UseVisualStyleBackColor = true;
            // 
            // _tvVisibility
            // 
            this._tvVisibility.CheckBoxes = true;
            this._tvVisibility.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tvVisibility.Location = new System.Drawing.Point(3, 63);
            this._tvVisibility.Name = "_tvVisibility";
            this._tvVisibility.Size = new System.Drawing.Size(1010, 626);
            this._tvVisibility.TabIndex = 1;
            this._tvVisibility.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvVisibility_AfterCheck);
            // 
            // _pnlPermissionTop
            // 
            this._pnlPermissionTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._pnlPermissionTop.Controls.Add(this.labelViewer);
            this._pnlPermissionTop.Controls.Add(this._cboViewer);
            this._pnlPermissionTop.Controls.Add(this._btnPermSave);
            this._pnlPermissionTop.Controls.Add(this._btnPermReset);
            this._pnlPermissionTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlPermissionTop.Location = new System.Drawing.Point(3, 3);
            this._pnlPermissionTop.Name = "_pnlPermissionTop";
            this._pnlPermissionTop.Padding = new System.Windows.Forms.Padding(8);
            this._pnlPermissionTop.Size = new System.Drawing.Size(1010, 60);
            this._pnlPermissionTop.TabIndex = 2;
            // 
            // labelViewer
            // 
            this.labelViewer.AutoSize = true;
            this.labelViewer.Location = new System.Drawing.Point(11, 16);
            this.labelViewer.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.labelViewer.Name = "labelViewer";
            this.labelViewer.Size = new System.Drawing.Size(43, 15);
            this.labelViewer.TabIndex = 0;
            this.labelViewer.Text = "사용자";
            // 
            // _cboViewer
            // 
            this._cboViewer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboViewer.Location = new System.Drawing.Point(60, 11);
            this._cboViewer.Name = "_cboViewer";
            this._cboViewer.Size = new System.Drawing.Size(240, 23);
            this._cboViewer.TabIndex = 1;
            this._cboViewer.SelectedIndexChanged += new System.EventHandler(this.cboViewer_SelectedIndexChanged);
            // 
            // _btnPermSave
            // 
            this._btnPermSave.Location = new System.Drawing.Point(318, 14);
            this._btnPermSave.Margin = new System.Windows.Forms.Padding(15, 6, 3, 3);
            this._btnPermSave.Name = "_btnPermSave";
            this._btnPermSave.Size = new System.Drawing.Size(100, 25);
            this._btnPermSave.TabIndex = 2;
            this._btnPermSave.Text = "저장";
            this._btnPermSave.Click += new System.EventHandler(this.btnPermSave_Click);
            // 
            // _btnPermReset
            // 
            this._btnPermReset.Location = new System.Drawing.Point(429, 14);
            this._btnPermReset.Margin = new System.Windows.Forms.Padding(8, 6, 3, 3);
            this._btnPermReset.Name = "_btnPermReset";
            this._btnPermReset.Size = new System.Drawing.Size(100, 25);
            this._btnPermReset.TabIndex = 3;
            this._btnPermReset.Text = "초기화";
            this._btnPermReset.Click += new System.EventHandler(this.btnPermReset_Click);
            // 
            // pageChatBan
            // 
            this.pageChatBan.Controls.Add(this._lvBans);
            this.pageChatBan.Controls.Add(this._pnlBanTop);
            this.pageChatBan.Location = new System.Drawing.Point(4, 24);
            this.pageChatBan.Name = "pageChatBan";
            this.pageChatBan.Padding = new System.Windows.Forms.Padding(3);
            this.pageChatBan.Size = new System.Drawing.Size(1016, 692);
            this.pageChatBan.TabIndex = 4;
            this.pageChatBan.Text = "대화 권한 관리";
            this.pageChatBan.UseVisualStyleBackColor = true;
            // 
            // _lvBans
            // 
            this._lvBans.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lvBans.FullRowSelect = true;
            this._lvBans.HideSelection = false;
            this._lvBans.Location = new System.Drawing.Point(3, 63);
            this._lvBans.Name = "_lvBans";
            this._lvBans.Size = new System.Drawing.Size(1010, 626);
            this._lvBans.TabIndex = 0;
            this._lvBans.UseCompatibleStateImageBehavior = false;
            this._lvBans.View = System.Windows.Forms.View.Details;
            // 
            // _pnlBanTop
            // 
            this._pnlBanTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._pnlBanTop.Controls.Add(this.labelUserA);
            this._pnlBanTop.Controls.Add(this._cbUser1);
            this._pnlBanTop.Controls.Add(this.labelUserB);
            this._pnlBanTop.Controls.Add(this._cbUser2);
            this._pnlBanTop.Controls.Add(this._btnBlock);
            this._pnlBanTop.Controls.Add(this._btnUnblock);
            this._pnlBanTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlBanTop.Location = new System.Drawing.Point(3, 3);
            this._pnlBanTop.Name = "_pnlBanTop";
            this._pnlBanTop.Padding = new System.Windows.Forms.Padding(8);
            this._pnlBanTop.Size = new System.Drawing.Size(1010, 60);
            this._pnlBanTop.TabIndex = 1;
            // 
            // labelUserA
            // 
            this.labelUserA.AutoSize = true;
            this.labelUserA.Location = new System.Drawing.Point(11, 16);
            this.labelUserA.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.labelUserA.Name = "labelUserA";
            this.labelUserA.Size = new System.Drawing.Size(55, 15);
            this.labelUserA.TabIndex = 0;
            this.labelUserA.Text = "사용자 A";
            // 
            // _cbUser1
            // 
            this._cbUser1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbUser1.Location = new System.Drawing.Point(72, 11);
            this._cbUser1.Name = "_cbUser1";
            this._cbUser1.Size = new System.Drawing.Size(220, 23);
            this._cbUser1.TabIndex = 1;
            // 
            // labelUserB
            // 
            this.labelUserB.AutoSize = true;
            this.labelUserB.Location = new System.Drawing.Point(310, 16);
            this.labelUserB.Margin = new System.Windows.Forms.Padding(15, 8, 3, 3);
            this.labelUserB.Name = "labelUserB";
            this.labelUserB.Size = new System.Drawing.Size(54, 15);
            this.labelUserB.TabIndex = 2;
            this.labelUserB.Text = "사용자 B";
            // 
            // _cbUser2
            // 
            this._cbUser2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbUser2.Location = new System.Drawing.Point(370, 11);
            this._cbUser2.Name = "_cbUser2";
            this._cbUser2.Size = new System.Drawing.Size(220, 23);
            this._cbUser2.TabIndex = 3;
            // 
            // _btnBlock
            // 
            this._btnBlock.Location = new System.Drawing.Point(608, 14);
            this._btnBlock.Margin = new System.Windows.Forms.Padding(15, 6, 3, 3);
            this._btnBlock.Name = "_btnBlock";
            this._btnBlock.Size = new System.Drawing.Size(100, 25);
            this._btnBlock.TabIndex = 4;
            this._btnBlock.Text = "차단하기";
            this._btnBlock.Click += new System.EventHandler(this.btnBlock_Click);
            // 
            // _btnUnblock
            // 
            this._btnUnblock.Location = new System.Drawing.Point(719, 14);
            this._btnUnblock.Margin = new System.Windows.Forms.Padding(8, 6, 3, 3);
            this._btnUnblock.Name = "_btnUnblock";
            this._btnUnblock.Size = new System.Drawing.Size(100, 25);
            this._btnUnblock.TabIndex = 5;
            this._btnUnblock.Text = "차단 해제";
            this._btnUnblock.Click += new System.EventHandler(this.btnUnblock_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1024, 720);
            this.Controls.Add(this._tabs);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DBP Talk - 관리자 콘솔";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this._tabs.ResumeLayout(false);
            this.pageDept.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridDept)).EndInit();
            this.pnlDeptTop.ResumeLayout(false);
            this.pnlDeptTop.PerformLayout();
            this.pageUserDept.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridUsers)).EndInit();
            this.pnlUserTop.ResumeLayout(false);
            this.pnlUserTop.PerformLayout();
            this.pageTeam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridTeam)).EndInit();
            this.pnlTeamTop.ResumeLayout(false);
            this.pnlTeamTop.PerformLayout();
            this.pageChat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridChat)).EndInit();
            this.pnlChatTop.ResumeLayout(false);
            this.pnlChatTop.PerformLayout();
            this.pageAccessLogs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridLogs)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pagePermission.ResumeLayout(false);
            this._pnlPermissionTop.ResumeLayout(false);
            this._pnlPermissionTop.PerformLayout();
            this.pageChatBan.ResumeLayout(false);
            this._pnlBanTop.ResumeLayout(false);
            this._pnlBanTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tabs;
        private System.Windows.Forms.TabPage pageDept;
        private System.Windows.Forms.Panel pnlDeptTop;
        private System.Windows.Forms.Label lblDeptName;
        private System.Windows.Forms.TextBox _txtDeptName;
        private System.Windows.Forms.Button _btnDeptAdd;
        private System.Windows.Forms.Button _btnDeptUpdate;
        private System.Windows.Forms.Label lblDeptSearch;
        private System.Windows.Forms.TextBox _txtDeptSearch;
        private System.Windows.Forms.Button _btnDeptSearch;
        private System.Windows.Forms.DataGridView _gridDept;
        private System.Windows.Forms.TabPage pageUserDept;
        private System.Windows.Forms.Panel pnlUserTop;
        private System.Windows.Forms.Label lblUserSearch;
        private System.Windows.Forms.TextBox _txtUserSearch;
        private System.Windows.Forms.Button _btnUserSearch;
        private System.Windows.Forms.Label lblDeptSelect;
        private System.Windows.Forms.ComboBox _cboDeptForUser;
        private System.Windows.Forms.Label lblTeamSelect;
        private System.Windows.Forms.ComboBox _cboTeamForUser;
        private System.Windows.Forms.Button _btnApplyDept;
        private System.Windows.Forms.DataGridView _gridUsers;
        private System.Windows.Forms.TabPage pageTeam;
        private System.Windows.Forms.DataGridView _gridTeam;
        private System.Windows.Forms.Panel pnlTeamTop;
        private System.Windows.Forms.Label lblTeamDeptSelect;
        private System.Windows.Forms.ComboBox _cboDeptForTeam;
        private System.Windows.Forms.Label lblTeamName;
        private System.Windows.Forms.TextBox _txtTeamName;
        private System.Windows.Forms.Button _btnTeamAdd2;
        private System.Windows.Forms.Button _btnTeamUpdate;
        private System.Windows.Forms.Label lblTeamSearch;
        private System.Windows.Forms.TextBox _txtTeamSearch;
        private System.Windows.Forms.Button _btnTeamSearch;
        private System.Windows.Forms.TabPage pageChat;
        private System.Windows.Forms.Panel pnlChatTop;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker _dtFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker _dtTo;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.TextBox _txtKeyword;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox _cboUserFilter;
        private System.Windows.Forms.Button _btnChatSearch;
        private System.Windows.Forms.DataGridView _gridChat;
        private System.Windows.Forms.TabPage pageAccessLogs;
        private System.Windows.Forms.FlowLayoutPanel pnlSearch;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label lblDateSeparator;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label lblUser_log;
        private System.Windows.Forms.TextBox txtSearchUser;
        private System.Windows.Forms.Button btnSearchLog;
        private System.Windows.Forms.DataGridView _gridLogs;
        private System.Windows.Forms.TabPage pagePermission;
        private System.Windows.Forms.TreeView _tvVisibility;
        private System.Windows.Forms.FlowLayoutPanel _pnlPermissionTop;
        private System.Windows.Forms.Label labelViewer;
        private System.Windows.Forms.ComboBox _cboViewer;
        private System.Windows.Forms.Button _btnPermSave;
        private System.Windows.Forms.Button _btnPermReset;
        private System.Windows.Forms.TabPage pageChatBan;
        private System.Windows.Forms.FlowLayoutPanel _pnlBanTop;
        private System.Windows.Forms.ComboBox _cbUser1;
        private System.Windows.Forms.ComboBox _cbUser2;
        private System.Windows.Forms.Button _btnBlock;
        private System.Windows.Forms.Button _btnUnblock;
        private System.Windows.Forms.ListView _lvBans;
        private System.Windows.Forms.Label labelUserA;
        private System.Windows.Forms.Label labelUserB;
    }
}
