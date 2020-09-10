namespace StationDemo
{
    partial class UserManger
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
            this.dataGridView_UserList = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserOwnRight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox_AddItem = new System.Windows.Forms.GroupBox();
            this.button_AddUser = new AutoFrameUI.RoundButton();
            this.comboBox_SelRight = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_PassWord = new System.Windows.Forms.TextBox();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ProductDir = new System.Windows.Forms.TextBox();
            this.BtnScarnProductDir = new AutoFrameUI.RoundButton();
            this.groupBox_SystemSet = new System.Windows.Forms.GroupBox();
            this.groupBox_ProgramMode = new System.Windows.Forms.GroupBox();
            this.checkBox_ModeRun = new System.Windows.Forms.CheckBox();
            this.checkBox_ModeAirRun = new System.Windows.Forms.CheckBox();
            this.groupBoxSafeSingal = new System.Windows.Forms.GroupBox();
            this.checkBox_UseSafeDoor = new System.Windows.Forms.CheckBox();
            this.checkBox_UseSafeGate = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_DelUser = new AutoFrameUI.RoundButton();
            this.check_CloseSafeProtect = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UserList)).BeginInit();
            this.groupBox_AddItem.SuspendLayout();
            this.groupBox_SystemSet.SuspendLayout();
            this.groupBox_ProgramMode.SuspendLayout();
            this.groupBoxSafeSingal.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_UserList
            // 
            this.dataGridView_UserList.AllowUserToAddRows = false;
            this.dataGridView_UserList.AllowUserToDeleteRows = false;
            this.dataGridView_UserList.BackgroundColor = System.Drawing.Color.MediumSpringGreen;
            this.dataGridView_UserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_UserList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.UserOwnRight});
            this.dataGridView_UserList.Location = new System.Drawing.Point(0, 8);
            this.dataGridView_UserList.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_UserList.Name = "dataGridView_UserList";
            this.dataGridView_UserList.ReadOnly = true;
            this.dataGridView_UserList.RowTemplate.Height = 30;
            this.dataGridView_UserList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_UserList.Size = new System.Drawing.Size(276, 329);
            this.dataGridView_UserList.TabIndex = 0;
            // 
            // UserName
            // 
            this.UserName.HeaderText = "用户名";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            // 
            // UserOwnRight
            // 
            this.UserOwnRight.HeaderText = "权限";
            this.UserOwnRight.Name = "UserOwnRight";
            this.UserOwnRight.ReadOnly = true;
            // 
            // groupBox_AddItem
            // 
            this.groupBox_AddItem.Controls.Add(this.button_AddUser);
            this.groupBox_AddItem.Controls.Add(this.comboBox_SelRight);
            this.groupBox_AddItem.Controls.Add(this.label3);
            this.groupBox_AddItem.Controls.Add(this.textBox_PassWord);
            this.groupBox_AddItem.Controls.Add(this.textBox_UserName);
            this.groupBox_AddItem.Controls.Add(this.label2);
            this.groupBox_AddItem.Controls.Add(this.label1);
            this.groupBox_AddItem.Location = new System.Drawing.Point(280, 8);
            this.groupBox_AddItem.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_AddItem.Name = "groupBox_AddItem";
            this.groupBox_AddItem.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_AddItem.Size = new System.Drawing.Size(191, 153);
            this.groupBox_AddItem.TabIndex = 3;
            this.groupBox_AddItem.TabStop = false;
            this.groupBox_AddItem.Text = "增加用户";
            // 
            // button_AddUser
            // 
            this.button_AddUser.BackColor = System.Drawing.Color.Transparent;
            this.button_AddUser.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.button_AddUser.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.button_AddUser.FlatAppearance.BorderSize = 0;
            this.button_AddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_AddUser.ImageHeight = 80;
            this.button_AddUser.ImageWidth = 80;
            this.button_AddUser.Location = new System.Drawing.Point(4, 116);
            this.button_AddUser.Margin = new System.Windows.Forms.Padding(2);
            this.button_AddUser.Name = "button_AddUser";
            this.button_AddUser.Radius = 24;
            this.button_AddUser.Size = new System.Drawing.Size(87, 33);
            this.button_AddUser.SpliteButtonWidth = 18;
            this.button_AddUser.TabIndex = 9;
            this.button_AddUser.Text = "添加用户";
            this.button_AddUser.UseVisualStyleBackColor = false;
            this.button_AddUser.Click += new System.EventHandler(this.button_AddUser_Click);
            // 
            // comboBox_SelRight
            // 
            this.comboBox_SelRight.FormattingEnabled = true;
            this.comboBox_SelRight.Location = new System.Drawing.Point(69, 87);
            this.comboBox_SelRight.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_SelRight.Name = "comboBox_SelRight";
            this.comboBox_SelRight.Size = new System.Drawing.Size(110, 20);
            this.comboBox_SelRight.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 93);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "权 限";
            // 
            // textBox_PassWord
            // 
            this.textBox_PassWord.Location = new System.Drawing.Point(69, 59);
            this.textBox_PassWord.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_PassWord.Name = "textBox_PassWord";
            this.textBox_PassWord.Size = new System.Drawing.Size(110, 21);
            this.textBox_PassWord.TabIndex = 5;
            // 
            // textBox_UserName
            // 
            this.textBox_UserName.Location = new System.Drawing.Point(69, 25);
            this.textBox_UserName.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_UserName.Name = "textBox_UserName";
            this.textBox_UserName.Size = new System.Drawing.Size(110, 21);
            this.textBox_UserName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密 码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "用户名";
            // 
            // textBox_ProductDir
            // 
            this.textBox_ProductDir.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_ProductDir.Location = new System.Drawing.Point(4, 50);
            this.textBox_ProductDir.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_ProductDir.Name = "textBox_ProductDir";
            this.textBox_ProductDir.ReadOnly = true;
            this.textBox_ProductDir.Size = new System.Drawing.Size(345, 32);
            this.textBox_ProductDir.TabIndex = 5;
            // 
            // BtnScarnProductDir
            // 
            this.BtnScarnProductDir.BackColor = System.Drawing.Color.Transparent;
            this.BtnScarnProductDir.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnScarnProductDir.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnScarnProductDir.FlatAppearance.BorderSize = 0;
            this.BtnScarnProductDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnScarnProductDir.ImageHeight = 80;
            this.BtnScarnProductDir.ImageWidth = 80;
            this.BtnScarnProductDir.Location = new System.Drawing.Point(352, 53);
            this.BtnScarnProductDir.Margin = new System.Windows.Forms.Padding(2);
            this.BtnScarnProductDir.Name = "BtnScarnProductDir";
            this.BtnScarnProductDir.Radius = 24;
            this.BtnScarnProductDir.Size = new System.Drawing.Size(66, 28);
            this.BtnScarnProductDir.SpliteButtonWidth = 18;
            this.BtnScarnProductDir.TabIndex = 6;
            this.BtnScarnProductDir.Text = "浏览";
            this.BtnScarnProductDir.UseVisualStyleBackColor = false;
            this.BtnScarnProductDir.Click += new System.EventHandler(this.BtnScarnProductDir_Click);
            // 
            // groupBox_SystemSet
            // 
            this.groupBox_SystemSet.Controls.Add(this.groupBox_ProgramMode);
            this.groupBox_SystemSet.Controls.Add(this.groupBoxSafeSingal);
            this.groupBox_SystemSet.Controls.Add(this.label4);
            this.groupBox_SystemSet.Controls.Add(this.textBox_ProductDir);
            this.groupBox_SystemSet.Controls.Add(this.BtnScarnProductDir);
            this.groupBox_SystemSet.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_SystemSet.Location = new System.Drawing.Point(483, 1);
            this.groupBox_SystemSet.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_SystemSet.Name = "groupBox_SystemSet";
            this.groupBox_SystemSet.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_SystemSet.Size = new System.Drawing.Size(422, 357);
            this.groupBox_SystemSet.TabIndex = 7;
            this.groupBox_SystemSet.TabStop = false;
            this.groupBox_SystemSet.Text = "系统设置";
            // 
            // groupBox_ProgramMode
            // 
            this.groupBox_ProgramMode.Controls.Add(this.checkBox_ModeRun);
            this.groupBox_ProgramMode.Controls.Add(this.checkBox_ModeAirRun);
            this.groupBox_ProgramMode.Location = new System.Drawing.Point(193, 94);
            this.groupBox_ProgramMode.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_ProgramMode.Name = "groupBox_ProgramMode";
            this.groupBox_ProgramMode.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_ProgramMode.Size = new System.Drawing.Size(180, 129);
            this.groupBox_ProgramMode.TabIndex = 9;
            this.groupBox_ProgramMode.TabStop = false;
            this.groupBox_ProgramMode.Text = "安全型号";
            // 
            // checkBox_ModeRun
            // 
            this.checkBox_ModeRun.AutoSize = true;
            this.checkBox_ModeRun.Location = new System.Drawing.Point(12, 64);
            this.checkBox_ModeRun.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_ModeRun.Name = "checkBox_ModeRun";
            this.checkBox_ModeRun.Size = new System.Drawing.Size(75, 26);
            this.checkBox_ModeRun.TabIndex = 1;
            this.checkBox_ModeRun.Text = "运行";
            this.checkBox_ModeRun.UseVisualStyleBackColor = true;
            this.checkBox_ModeRun.CheckedChanged += new System.EventHandler(this.checkBox_ModeRun_CheckedChanged);
            // 
            // checkBox_ModeAirRun
            // 
            this.checkBox_ModeAirRun.AutoSize = true;
            this.checkBox_ModeAirRun.Location = new System.Drawing.Point(13, 34);
            this.checkBox_ModeAirRun.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_ModeAirRun.Name = "checkBox_ModeAirRun";
            this.checkBox_ModeAirRun.Size = new System.Drawing.Size(75, 26);
            this.checkBox_ModeAirRun.TabIndex = 0;
            this.checkBox_ModeAirRun.Text = "空跑";
            this.checkBox_ModeAirRun.UseVisualStyleBackColor = true;
            this.checkBox_ModeAirRun.CheckedChanged += new System.EventHandler(this.checkBox_ModeAirRun_CheckedChanged);
            // 
            // groupBoxSafeSingal
            // 
            this.groupBoxSafeSingal.Controls.Add(this.check_CloseSafeProtect);
            this.groupBoxSafeSingal.Controls.Add(this.checkBox_UseSafeDoor);
            this.groupBoxSafeSingal.Controls.Add(this.checkBox_UseSafeGate);
            this.groupBoxSafeSingal.Location = new System.Drawing.Point(9, 93);
            this.groupBoxSafeSingal.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxSafeSingal.Name = "groupBoxSafeSingal";
            this.groupBoxSafeSingal.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxSafeSingal.Size = new System.Drawing.Size(180, 130);
            this.groupBoxSafeSingal.TabIndex = 8;
            this.groupBoxSafeSingal.TabStop = false;
            this.groupBoxSafeSingal.Text = "安全型号";
            // 
            // checkBox_UseSafeDoor
            // 
            this.checkBox_UseSafeDoor.AutoSize = true;
            this.checkBox_UseSafeDoor.Location = new System.Drawing.Point(4, 67);
            this.checkBox_UseSafeDoor.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_UseSafeDoor.Name = "checkBox_UseSafeDoor";
            this.checkBox_UseSafeDoor.Size = new System.Drawing.Size(144, 26);
            this.checkBox_UseSafeDoor.TabIndex = 1;
            this.checkBox_UseSafeDoor.Text = "启用安全门";
            this.checkBox_UseSafeDoor.UseVisualStyleBackColor = true;
            this.checkBox_UseSafeDoor.CheckedChanged += new System.EventHandler(this.checkBox_UseSafeDoor_CheckedChanged);
            // 
            // checkBox_UseSafeGate
            // 
            this.checkBox_UseSafeGate.AutoSize = true;
            this.checkBox_UseSafeGate.Location = new System.Drawing.Point(4, 37);
            this.checkBox_UseSafeGate.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_UseSafeGate.Name = "checkBox_UseSafeGate";
            this.checkBox_UseSafeGate.Size = new System.Drawing.Size(167, 26);
            this.checkBox_UseSafeGate.TabIndex = 0;
            this.checkBox_UseSafeGate.Text = "启用安全光栅";
            this.checkBox_UseSafeGate.UseVisualStyleBackColor = true;
            this.checkBox_UseSafeGate.CheckedChanged += new System.EventHandler(this.checkBox_UseSafeGate_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "产品文件夹";
            // 
            // button_DelUser
            // 
            this.button_DelUser.BackColor = System.Drawing.Color.Transparent;
            this.button_DelUser.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.button_DelUser.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.button_DelUser.FlatAppearance.BorderSize = 0;
            this.button_DelUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_DelUser.ImageHeight = 80;
            this.button_DelUser.ImageWidth = 80;
            this.button_DelUser.Location = new System.Drawing.Point(281, 243);
            this.button_DelUser.Margin = new System.Windows.Forms.Padding(2);
            this.button_DelUser.Name = "button_DelUser";
            this.button_DelUser.Radius = 24;
            this.button_DelUser.Size = new System.Drawing.Size(87, 33);
            this.button_DelUser.SpliteButtonWidth = 18;
            this.button_DelUser.TabIndex = 8;
            this.button_DelUser.Text = "删除用户";
            this.button_DelUser.UseVisualStyleBackColor = false;
            this.button_DelUser.Click += new System.EventHandler(this.button_DelUser_Click);
            // 
            // check_CloseSafeProtect
            // 
            this.check_CloseSafeProtect.AutoSize = true;
            this.check_CloseSafeProtect.Location = new System.Drawing.Point(4, 97);
            this.check_CloseSafeProtect.Margin = new System.Windows.Forms.Padding(2);
            this.check_CloseSafeProtect.Name = "check_CloseSafeProtect";
            this.check_CloseSafeProtect.Size = new System.Drawing.Size(167, 26);
            this.check_CloseSafeProtect.TabIndex = 2;
            this.check_CloseSafeProtect.Text = "屏蔽安全保护";
            this.check_CloseSafeProtect.UseVisualStyleBackColor = true;
            this.check_CloseSafeProtect.CheckedChanged += new System.EventHandler(this.check_CloseSafeProtect_CheckedChanged);
            // 
            // UserManger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 515);
            this.Controls.Add(this.button_DelUser);
            this.Controls.Add(this.groupBox_SystemSet);
            this.Controls.Add(this.groupBox_AddItem);
            this.Controls.Add(this.dataGridView_UserList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserManger";
            this.Text = "UserManger";
            this.Load += new System.EventHandler(this.UserManger_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UserList)).EndInit();
            this.groupBox_AddItem.ResumeLayout(false);
            this.groupBox_AddItem.PerformLayout();
            this.groupBox_SystemSet.ResumeLayout(false);
            this.groupBox_SystemSet.PerformLayout();
            this.groupBox_ProgramMode.ResumeLayout(false);
            this.groupBox_ProgramMode.PerformLayout();
            this.groupBoxSafeSingal.ResumeLayout(false);
            this.groupBoxSafeSingal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_UserList;
        private System.Windows.Forms.GroupBox groupBox_AddItem;
        private System.Windows.Forms.ComboBox comboBox_SelRight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_PassWord;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ProductDir;
        private AutoFrameUI.RoundButton BtnScarnProductDir;
        private System.Windows.Forms.GroupBox groupBox_SystemSet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserOwnRight;
        private AutoFrameUI.RoundButton button_AddUser;
        private AutoFrameUI.RoundButton button_DelUser;
        private System.Windows.Forms.GroupBox groupBoxSafeSingal;
        private System.Windows.Forms.CheckBox checkBox_UseSafeDoor;
        private System.Windows.Forms.CheckBox checkBox_UseSafeGate;
        private System.Windows.Forms.GroupBox groupBox_ProgramMode;
        private System.Windows.Forms.CheckBox checkBox_ModeRun;
        private System.Windows.Forms.CheckBox checkBox_ModeAirRun;
        private System.Windows.Forms.CheckBox check_CloseSafeProtect;
    }
}