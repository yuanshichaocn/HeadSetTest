namespace StationDemo
{
    partial class Form_Auto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Auto));
            this.tabControl_SelCammer = new System.Windows.Forms.TabControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl_Log = new System.Windows.Forms.TabControl();
            this.dataGridView_Sum = new System.Windows.Forms.DataGridView();
            this.CountItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Val = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClearItem = new System.Windows.Forms.DataGridViewButtonColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label_CurrentFile = new System.Windows.Forms.Label();
            this.userPanel_Flag = new UserCtrl.UserPanel();
            this.MachineStatePause = new UserCtrl.UserLabel();
            this.MachineStateStop = new UserCtrl.UserLabel();
            this.MachineStateAuto = new UserCtrl.UserLabel();
            this.MachineStateEmg = new UserCtrl.UserLabel();
            this.BtnReset = new AutoFrameUI.RoundButton();
            this.VerNo = new System.Windows.Forms.Label();
            this.BtnClearAllProduct = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Sum)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl_SelCammer
            // 
            this.tabControl_SelCammer.Location = new System.Drawing.Point(2, 12);
            this.tabControl_SelCammer.Name = "tabControl_SelCammer";
            this.tabControl_SelCammer.SelectedIndex = 0;
            this.tabControl_SelCammer.Size = new System.Drawing.Size(9, 16);
            this.tabControl_SelCammer.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.ShowImg);
            // 
            // tabControl_Log
            // 
            this.tabControl_Log.Location = new System.Drawing.Point(2, 18);
            this.tabControl_Log.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl_Log.Name = "tabControl_Log";
            this.tabControl_Log.SelectedIndex = 0;
            this.tabControl_Log.Size = new System.Drawing.Size(945, 537);
            this.tabControl_Log.TabIndex = 1;
            // 
            // dataGridView_Sum
            // 
            this.dataGridView_Sum.AllowUserToAddRows = false;
            this.dataGridView_Sum.AllowUserToDeleteRows = false;
            this.dataGridView_Sum.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dataGridView_Sum.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_Sum.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CountItem,
            this.Val,
            this.ClearItem});
            this.dataGridView_Sum.Location = new System.Drawing.Point(954, 281);
            this.dataGridView_Sum.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_Sum.Name = "dataGridView_Sum";
            this.dataGridView_Sum.ReadOnly = true;
            this.dataGridView_Sum.RowHeadersVisible = false;
            this.dataGridView_Sum.RowTemplate.Height = 30;
            this.dataGridView_Sum.RowTemplate.ReadOnly = true;
            this.dataGridView_Sum.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Sum.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_Sum.Size = new System.Drawing.Size(335, 245);
            this.dataGridView_Sum.TabIndex = 2;
            // 
            // CountItem
            // 
            this.CountItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CountItem.HeaderText = "统计项目";
            this.CountItem.Name = "CountItem";
            this.CountItem.ReadOnly = true;
            // 
            // Val
            // 
            this.Val.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Val.HeaderText = "计数";
            this.Val.Name = "Val";
            this.Val.ReadOnly = true;
            this.Val.Width = 60;
            // 
            // ClearItem
            // 
            this.ClearItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ClearItem.HeaderText = "清零";
            this.ClearItem.Name = "ClearItem";
            this.ClearItem.ReadOnly = true;
            this.ClearItem.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ClearItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ClearItem.Width = 60;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "light_gray.png");
            this.imageList1.Images.SetKeyName(1, "light_green.png");
            this.imageList1.Images.SetKeyName(2, "light_red.png");
            // 
            // label_CurrentFile
            // 
            this.label_CurrentFile.AutoSize = true;
            this.label_CurrentFile.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CurrentFile.Location = new System.Drawing.Point(954, 59);
            this.label_CurrentFile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CurrentFile.Name = "label_CurrentFile";
            this.label_CurrentFile.Size = new System.Drawing.Size(93, 16);
            this.label_CurrentFile.TabIndex = 6;
            this.label_CurrentFile.Text = "当前产品：";
            // 
            // userPanel_Flag
            // 
            this.userPanel_Flag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userPanel_Flag.Location = new System.Drawing.Point(954, 532);
            this.userPanel_Flag.m_nNumPerPage = 10;
            this.userPanel_Flag.m_nNumPerRow = 4;
            this.userPanel_Flag.m_page = 0;
            this.userPanel_Flag.m_splitHigh = 30;
            this.userPanel_Flag.m_splitWidth = 170;
            this.userPanel_Flag.Margin = new System.Windows.Forms.Padding(4);
            this.userPanel_Flag.Name = "userPanel_Flag";
            this.userPanel_Flag.Size = new System.Drawing.Size(333, 245);
            this.userPanel_Flag.TabIndex = 17;
            // 
            // MachineStatePause
            // 
            this.MachineStatePause.Location = new System.Drawing.Point(957, 232);
            this.MachineStatePause.Margin = new System.Windows.Forms.Padding(1);
            this.MachineStatePause.Name = "MachineStatePause";
            this.MachineStatePause.Size = new System.Drawing.Size(73, 28);
            this.MachineStatePause.State = false;
            this.MachineStatePause.TabIndex = 11;
            // 
            // MachineStateStop
            // 
            this.MachineStateStop.Location = new System.Drawing.Point(957, 166);
            this.MachineStateStop.Margin = new System.Windows.Forms.Padding(1);
            this.MachineStateStop.Name = "MachineStateStop";
            this.MachineStateStop.Size = new System.Drawing.Size(73, 28);
            this.MachineStateStop.State = false;
            this.MachineStateStop.TabIndex = 10;
            // 
            // MachineStateAuto
            // 
            this.MachineStateAuto.Location = new System.Drawing.Point(957, 198);
            this.MachineStateAuto.Margin = new System.Windows.Forms.Padding(1);
            this.MachineStateAuto.Name = "MachineStateAuto";
            this.MachineStateAuto.Size = new System.Drawing.Size(73, 28);
            this.MachineStateAuto.State = false;
            this.MachineStateAuto.TabIndex = 9;
            // 
            // MachineStateEmg
            // 
            this.MachineStateEmg.Location = new System.Drawing.Point(957, 134);
            this.MachineStateEmg.Margin = new System.Windows.Forms.Padding(1);
            this.MachineStateEmg.Name = "MachineStateEmg";
            this.MachineStateEmg.Size = new System.Drawing.Size(73, 28);
            this.MachineStateEmg.State = false;
            this.MachineStateEmg.TabIndex = 8;
            // 
            // BtnReset
            // 
            this.BtnReset.BackColor = System.Drawing.Color.Transparent;
            this.BtnReset.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnReset.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnReset.FlatAppearance.BorderSize = 0;
            this.BtnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnReset.ImageHeight = 80;
            this.BtnReset.ImageWidth = 80;
            this.BtnReset.Location = new System.Drawing.Point(954, 88);
            this.BtnReset.Margin = new System.Windows.Forms.Padding(2);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Radius = 24;
            this.BtnReset.Size = new System.Drawing.Size(105, 43);
            this.BtnReset.SpliteButtonWidth = 18;
            this.BtnReset.TabIndex = 7;
            this.BtnReset.Text = "复位";
            this.BtnReset.UseVisualStyleBackColor = false;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // VerNo
            // 
            this.VerNo.AutoSize = true;
            this.VerNo.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VerNo.Location = new System.Drawing.Point(1323, 12);
            this.VerNo.Name = "VerNo";
            this.VerNo.Size = new System.Drawing.Size(213, 21);
            this.VerNo.TabIndex = 24;
            this.VerNo.Text = "软件版本号：Ver1.00";
            // 
            // BtnClearAllProduct
            // 
            this.BtnClearAllProduct.Location = new System.Drawing.Point(1188, 212);
            this.BtnClearAllProduct.Name = "BtnClearAllProduct";
            this.BtnClearAllProduct.Size = new System.Drawing.Size(101, 48);
            this.BtnClearAllProduct.TabIndex = 25;
            this.BtnClearAllProduct.Text = "清料";
            this.BtnClearAllProduct.UseVisualStyleBackColor = true;
            this.BtnClearAllProduct.Click += new System.EventHandler(this.BtnClearAllProduct_Click);
            // 
            // Form_Auto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 788);
            this.Controls.Add(this.BtnClearAllProduct);
            this.Controls.Add(this.VerNo);
            this.Controls.Add(this.userPanel_Flag);
            this.Controls.Add(this.MachineStatePause);
            this.Controls.Add(this.MachineStateStop);
            this.Controls.Add(this.MachineStateAuto);
            this.Controls.Add(this.MachineStateEmg);
            this.Controls.Add(this.BtnReset);
            this.Controls.Add(this.label_CurrentFile);
            this.Controls.Add(this.dataGridView_Sum);
            this.Controls.Add(this.tabControl_Log);
            this.Controls.Add(this.tabControl_SelCammer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Auto";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseMainForm);
            this.Load += new System.EventHandler(this.Form_Auto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Sum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_SelCammer;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl_Log;
        private System.Windows.Forms.DataGridView dataGridView_Sum;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Val;
        private System.Windows.Forms.DataGridViewButtonColumn ClearItem;
        private System.Windows.Forms.Label label_CurrentFile;
        private AutoFrameUI.RoundButton BtnReset;
        private UserCtrl.UserLabel MachineStateEmg;
        private UserCtrl.UserLabel MachineStateAuto;
        private UserCtrl.UserLabel MachineStateStop;
        private UserCtrl.UserLabel MachineStatePause;
        private UserCtrl.UserPanel userPanel_Flag;
        private System.Windows.Forms.Label VerNo;
        private System.Windows.Forms.Button BtnClearAllProduct;
    }
}