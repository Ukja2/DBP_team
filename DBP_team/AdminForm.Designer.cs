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
            this._btnApplyDept = new System.Windows.Forms.Button();
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
            this.label1 = new System.Windows.Forms.Label();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAllEmployees = new System.Windows.Forms.CheckBox();
            this.clbDepartments = new System.Windows.Forms.CheckedListBox();
            this.btnSavePermission = new System.Windows.Forms.Button();
            this.btnResetPermission = new System.Windows.Forms.Button();
            this.lblSelectedUser = new System.Windows.Forms.Label();
            this._tabs.SuspendLayout();
            this.pageDept.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridDept)).BeginInit();
            this.pnlDeptTop.SuspendLayout();
            this.pageUserDept.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridUsers)).BeginInit();
            this.pnlUserTop.SuspendLayout();
            this.pageChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridChat)).BeginInit();
            this.pnlChatTop.SuspendLayout();
            this.pageAccessLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridLogs)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pagePermission.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // _tabs
            // 
            this._tabs.Controls.Add(this.pageDept);
            this._tabs.Controls.Add(this.pageUserDept);
            this._tabs.Controls.Add(this.pageChat);
            this._tabs.Controls.Add(this.pageAccessLogs);
            this._tabs.Controls.Add(this.pagePermission);
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
            this.pageDept.Location = new System.Drawing.Point(4, 29);
            this.pageDept.Name = "pageDept";
            this.pageDept.Padding = new System.Windows.Forms.Padding(3);
            this.pageDept.Size = new System.Drawing.Size(1016, 687);
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
            this._gridDept.Size = new System.Drawing.Size(1010, 621);
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
            this.lblDeptName.Size = new System.Drawing.Size(54, 20);
            this.lblDeptName.TabIndex = 0;
            this.lblDeptName.Text = "부서명";
            // 
            // _txtDeptName
            // 
            this._txtDeptName.Location = new System.Drawing.Point(60, 8);
            this._txtDeptName.Name = "_txtDeptName";
            this._txtDeptName.Size = new System.Drawing.Size(220, 27);
            this._txtDeptName.TabIndex = 1;
            // 
            // _btnDeptAdd
            // 
            this._btnDeptAdd.Location = new System.Drawing.Point(290, 7);
            this._btnDeptAdd.Name = "_btnDeptAdd";
            this._btnDeptAdd.Size = new System.Drawing.Size(80, 25);
            this._btnDeptAdd.TabIndex = 2;
            this._btnDeptAdd.Text = "추가";
            this._btnDeptAdd.Click += new System.EventHandler(this.DeptAdd_Click);
            // 
            // _btnDeptUpdate
            // 
            this._btnDeptUpdate.Location = new System.Drawing.Point(375, 7);
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
            this.lblDeptSearch.Location = new System.Drawing.Point(470, 12);
            this.lblDeptSearch.Name = "lblDeptSearch";
            this.lblDeptSearch.Size = new System.Drawing.Size(94, 20);
            this.lblDeptSearch.TabIndex = 4;
            this.lblDeptSearch.Text = "검색(부서명)";
            // 
            // _txtDeptSearch
            // 
            this._txtDeptSearch.Location = new System.Drawing.Point(560, 8);
            this._txtDeptSearch.Name = "_txtDeptSearch";
            this._txtDeptSearch.Size = new System.Drawing.Size(220, 27);
            this._txtDeptSearch.TabIndex = 5;
            // 
            // _btnDeptSearch
            // 
            this._btnDeptSearch.Location = new System.Drawing.Point(785, 7);
            this._btnDeptSearch.Name = "_btnDeptSearch";
            this._btnDeptSearch.Size = new System.Drawing.Size(80, 25);
            this._btnDeptSearch.TabIndex = 6;
            this._btnDeptSearch.Text = "검색";
            this._btnDeptSearch.Click += new System.EventHandler(this.DeptSearch_Click);
            // 
            // pageUserDept
            // 
            this.pageUserDept.Controls.Add(this._gridUsers);
            this.pageUserDept.Controls.Add(this.pnlUserTop);
            this.pageUserDept.Location = new System.Drawing.Point(4, 29);
            this.pageUserDept.Name = "pageUserDept";
            this.pageUserDept.Padding = new System.Windows.Forms.Padding(3);
            this.pageUserDept.Size = new System.Drawing.Size(1016, 687);
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
            this._gridUsers.Size = new System.Drawing.Size(1010, 621);
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
            this.lblUserSearch.Size = new System.Drawing.Size(89, 20);
            this.lblUserSearch.TabIndex = 0;
            this.lblUserSearch.Text = "사용자 검색";
            // 
            // _txtUserSearch
            // 
            this._txtUserSearch.Location = new System.Drawing.Point(80, 8);
            this._txtUserSearch.Name = "_txtUserSearch";
            this._txtUserSearch.Size = new System.Drawing.Size(240, 27);
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
            this.lblDeptSelect.Size = new System.Drawing.Size(74, 20);
            this.lblDeptSelect.TabIndex = 3;
            this.lblDeptSelect.Text = "부서 선택";
            // 
            // _cboDeptForUser
            // 
            this._cboDeptForUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboDeptForUser.Location = new System.Drawing.Point(480, 8);
            this._cboDeptForUser.Name = "_cboDeptForUser";
            this._cboDeptForUser.Size = new System.Drawing.Size(240, 28);
            this._cboDeptForUser.TabIndex = 4;
            // 
            // _btnApplyDept
            // 
            this._btnApplyDept.Location = new System.Drawing.Point(725, 7);
            this._btnApplyDept.Name = "_btnApplyDept";
            this._btnApplyDept.Size = new System.Drawing.Size(180, 25);
            this._btnApplyDept.TabIndex = 5;
            this._btnApplyDept.Text = "선택된 사용자 소속 변경";
            this._btnApplyDept.Click += new System.EventHandler(this.ApplyDept_Click);
            // 
            // pageChat
            // 
            this.pageChat.Controls.Add(this._gridChat);
            this.pageChat.Controls.Add(this.pnlChatTop);
            this.pageChat.Location = new System.Drawing.Point(4, 29);
            this.pageChat.Name = "pageChat";
            this.pageChat.Padding = new System.Windows.Forms.Padding(3);
            this.pageChat.Size = new System.Drawing.Size(1016, 687);
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
            this._gridChat.Size = new System.Drawing.Size(1010, 611);
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
            this.lblFrom.Size = new System.Drawing.Size(54, 20);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "시작일";
            // 
            // _dtFrom
            // 
            this._dtFrom.Location = new System.Drawing.Point(60, 8);
            this._dtFrom.Name = "_dtFrom";
            this._dtFrom.Size = new System.Drawing.Size(140, 27);
            this._dtFrom.TabIndex = 1;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblTo.Location = new System.Drawing.Point(210, 12);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(54, 20);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "종료일";
            // 
            // _dtTo
            // 
            this._dtTo.Location = new System.Drawing.Point(260, 8);
            this._dtTo.Name = "_dtTo";
            this._dtTo.Size = new System.Drawing.Size(140, 27);
            this._dtTo.TabIndex = 3;
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblKeyword.Location = new System.Drawing.Point(410, 12);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(54, 20);
            this.lblKeyword.TabIndex = 4;
            this.lblKeyword.Text = "키워드";
            // 
            // _txtKeyword
            // 
            this._txtKeyword.Location = new System.Drawing.Point(455, 8);
            this._txtKeyword.Name = "_txtKeyword";
            this._txtKeyword.Size = new System.Drawing.Size(220, 27);
            this._txtKeyword.TabIndex = 5;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblUser.Location = new System.Drawing.Point(685, 12);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(54, 20);
            this.lblUser.TabIndex = 6;
            this.lblUser.Text = "사용자";
            // 
            // _cboUserFilter
            // 
            this._cboUserFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboUserFilter.Location = new System.Drawing.Point(730, 8);
            this._cboUserFilter.Name = "_cboUserFilter";
            this._cboUserFilter.Size = new System.Drawing.Size(200, 28);
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
            this.pageAccessLogs.Location = new System.Drawing.Point(4, 29);
            this.pageAccessLogs.Name = "pageAccessLogs";
            this.pageAccessLogs.Padding = new System.Windows.Forms.Padding(3);
            this.pageAccessLogs.Size = new System.Drawing.Size(1016, 687);
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
            this._gridLogs.Size = new System.Drawing.Size(1010, 621);
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
            this.lblDate.Size = new System.Drawing.Size(42, 20);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "기간:";
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(56, 11);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(100, 27);
            this.dtpStart.TabIndex = 1;
            // 
            // lblDateSeparator
            // 
            this.lblDateSeparator.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDateSeparator.AutoSize = true;
            this.lblDateSeparator.Location = new System.Drawing.Point(162, 16);
            this.lblDateSeparator.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.lblDateSeparator.Name = "lblDateSeparator";
            this.lblDateSeparator.Size = new System.Drawing.Size(20, 20);
            this.lblDateSeparator.TabIndex = 2;
            this.lblDateSeparator.Text = "~";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(185, 11);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(100, 27);
            this.dtpEnd.TabIndex = 3;
            // 
            // lblUser_log
            // 
            this.lblUser_log.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUser_log.AutoSize = true;
            this.lblUser_log.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblUser_log.Location = new System.Drawing.Point(303, 16);
            this.lblUser_log.Margin = new System.Windows.Forms.Padding(15, 6, 0, 3);
            this.lblUser_log.Name = "lblUser_log";
            this.lblUser_log.Size = new System.Drawing.Size(57, 20);
            this.lblUser_log.TabIndex = 4;
            this.lblUser_log.Text = "사용자:";
            // 
            // txtSearchUser
            // 
            this.txtSearchUser.Location = new System.Drawing.Point(363, 11);
            this.txtSearchUser.Name = "txtSearchUser";
            this.txtSearchUser.Size = new System.Drawing.Size(150, 27);
            this.txtSearchUser.TabIndex = 5;
            // 
            // btnSearchLog
            // 
            this.btnSearchLog.Location = new System.Drawing.Point(526, 8);
            this.btnSearchLog.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.btnSearchLog.Name = "btnSearchLog";
            this.btnSearchLog.Size = new System.Drawing.Size(80, 25);
            this.btnSearchLog.TabIndex = 6;
            this.btnSearchLog.Text = "검색";
            this.btnSearchLog.Click += new System.EventHandler(this.SearchLog_Click);
            // 
            // pagePermission
            // 
            this.pagePermission.Controls.Add(this.lblSelectedUser);
            this.pagePermission.Controls.Add(this.btnResetPermission);
            this.pagePermission.Controls.Add(this.btnSavePermission);
            this.pagePermission.Controls.Add(this.clbDepartments);
            this.pagePermission.Controls.Add(this.chkAllEmployees);
            this.pagePermission.Controls.Add(this.label2);
            this.pagePermission.Controls.Add(this.dgvUsers);
            this.pagePermission.Controls.Add(this.label1);
            this.pagePermission.Location = new System.Drawing.Point(4, 29);
            this.pagePermission.Name = "pagePermission";
            this.pagePermission.Padding = new System.Windows.Forms.Padding(3);
            this.pagePermission.Size = new System.Drawing.Size(1016, 687);
            this.pagePermission.TabIndex = 4;
            this.pagePermission.Text = "권한 설정";
            this.pagePermission.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(181, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "사용자 선택";
            // 
            // dgvUsers
            // 
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(27, 43);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.RowTemplate.Height = 27;
            this.dgvUsers.Size = new System.Drawing.Size(431, 194);
            this.dgvUsers.TabIndex = 1;
            this.dgvUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
            this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(733, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "권한 설정";
            // 
            // chkAllEmployees
            // 
            this.chkAllEmployees.AutoSize = true;
            this.chkAllEmployees.Location = new System.Drawing.Point(830, 56);
            this.chkAllEmployees.Name = "chkAllEmployees";
            this.chkAllEmployees.Size = new System.Drawing.Size(131, 24);
            this.chkAllEmployees.TabIndex = 0;
            this.chkAllEmployees.Text = "모든 직원 보기";
            this.chkAllEmployees.UseVisualStyleBackColor = true;
            this.chkAllEmployees.CheckedChanged += new System.EventHandler(this.chkAllEmployees_CheckedChanged);
            // 
            // clbDepartments
            // 
            this.clbDepartments.FormattingEnabled = true;
            this.clbDepartments.Location = new System.Drawing.Point(574, 79);
            this.clbDepartments.Name = "clbDepartments";
            this.clbDepartments.Size = new System.Drawing.Size(387, 158);
            this.clbDepartments.TabIndex = 4;
            // 
            // btnSavePermission
            // 
            this.btnSavePermission.Location = new System.Drawing.Point(657, 284);
            this.btnSavePermission.Name = "btnSavePermission";
            this.btnSavePermission.Size = new System.Drawing.Size(82, 33);
            this.btnSavePermission.TabIndex = 5;
            this.btnSavePermission.Text = "저장";
            this.btnSavePermission.UseVisualStyleBackColor = true;
            this.btnSavePermission.Click += new System.EventHandler(this.btnSavePermission_Click);
            // 
            // btnResetPermission
            // 
            this.btnResetPermission.Location = new System.Drawing.Point(806, 284);
            this.btnResetPermission.Name = "btnResetPermission";
            this.btnResetPermission.Size = new System.Drawing.Size(82, 33);
            this.btnResetPermission.TabIndex = 6;
            this.btnResetPermission.Text = "초기화";
            this.btnResetPermission.UseVisualStyleBackColor = true;
            this.btnResetPermission.Click += new System.EventHandler(this.btnResetPermission_Click);
            // 
            // lblSelectedUser
            // 
            this.lblSelectedUser.AutoSize = true;
            this.lblSelectedUser.Location = new System.Drawing.Point(570, 240);
            this.lblSelectedUser.Name = "lblSelectedUser";
            this.lblSelectedUser.Size = new System.Drawing.Size(142, 20);
            this.lblSelectedUser.TabIndex = 7;
            this.lblSelectedUser.Text = "선택된 사용자: 없음";
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
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
            this.pageChat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridChat)).EndInit();
            this.pnlChatTop.ResumeLayout(false);
            this.pnlChatTop.PerformLayout();
            this.pageAccessLogs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridLogs)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pagePermission.ResumeLayout(false);
            this.pagePermission.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
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
        private System.Windows.Forms.Button _btnApplyDept;
        private System.Windows.Forms.DataGridView _gridUsers;
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
        private TabPage pagePermission;
        private Label label2;
        private DataGridView dgvUsers;
        private Label label1;
        private CheckBox chkAllEmployees;
        private Button btnResetPermission;
        private Button btnSavePermission;
        private CheckedListBox clbDepartments;
        private Label lblSelectedUser;
    }
}
