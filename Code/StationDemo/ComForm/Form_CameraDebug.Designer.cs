namespace StationDemo
{
    partial class Form_VisionDebug
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
            this.tabControl_VisionStepPages = new System.Windows.Forms.TabControl();
            this.dataGridViewProcessItem = new System.Windows.Forms.DataGridView();
            this.SelEdit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.VisionItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcessItem = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Sel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ExposureTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GainVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Test = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox_GainVal = new System.Windows.Forms.TextBox();
            this.textBox_exposureTimeVal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_SelCam = new System.Windows.Forms.ComboBox();
            this.panel_VisionCtrls = new System.Windows.Forms.Panel();
            this.vision_FindCircleCtr1 = new VisionProcess.VisionFindCircleCtr();
            this.vision_1BarCodeSetCtr1 = new VisionProcess.Vision1BarCodeSetCtr();
            this.roundButton_ShapeROIAdd = new AutoFrameUI.RoundButton();
            this.roundButton_ShapeROISub = new AutoFrameUI.RoundButton();
            this.vision_2dCodeSetCtr1 = new VisionProcess.Vision2dCodeSetCtr();
            this.roundButton_Test = new AutoFrameUI.RoundButton();
            this.Vision_MatchSetCtr1 = new VisionProcess.VisionMatchSetCtr();
            this.roundButton_DrawRect1 = new AutoFrameUI.RoundButton();
            this.roundButton_SeachArea = new AutoFrameUI.RoundButton();
            this.roundButton_ReadImg = new AutoFrameUI.RoundButton();
            this.roundButton_SnapSave = new AutoFrameUI.RoundButton();
            this.roundButton_AddItem = new AutoFrameUI.RoundButton();
            this.roundButton_Save = new AutoFrameUI.RoundButton();
            this.roundButton_DelItem = new AutoFrameUI.RoundButton();
            this.roundButton1 = new AutoFrameUI.RoundButton();
            this.roundButton_ContinuousSnap = new AutoFrameUI.RoundButton();
            this.visionControl1 = new UserCtrl.VisionControl();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcessItem)).BeginInit();
            this.panel_VisionCtrls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl_VisionStepPages
            // 
            this.tabControl_VisionStepPages.Location = new System.Drawing.Point(994, 12);
            this.tabControl_VisionStepPages.Name = "tabControl_VisionStepPages";
            this.tabControl_VisionStepPages.SelectedIndex = 0;
            this.tabControl_VisionStepPages.Size = new System.Drawing.Size(10, 14);
            this.tabControl_VisionStepPages.TabIndex = 0;
            // 
            // dataGridViewProcessItem
            // 
            this.dataGridViewProcessItem.AllowUserToAddRows = false;
            this.dataGridViewProcessItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProcessItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelEdit,
            this.VisionItem,
            this.ProcessItem,
            this.Sel,
            this.ExposureTime,
            this.GainVal,
            this.Test});
            this.dataGridViewProcessItem.Location = new System.Drawing.Point(-23, -5);
            this.dataGridViewProcessItem.Name = "dataGridViewProcessItem";
            this.dataGridViewProcessItem.RowHeadersVisible = false;
            this.dataGridViewProcessItem.RowTemplate.Height = 23;
            this.dataGridViewProcessItem.Size = new System.Drawing.Size(473, 396);
            this.dataGridViewProcessItem.TabIndex = 1;
            this.dataGridViewProcessItem.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProcessItem_CellContentClick);
            // 
            // SelEdit
            // 
            this.SelEdit.HeaderText = "";
            this.SelEdit.Name = "SelEdit";
            this.SelEdit.Width = 40;
            // 
            // VisionItem
            // 
            this.VisionItem.HeaderText = "名称";
            this.VisionItem.Name = "VisionItem";
            this.VisionItem.ReadOnly = true;
            // 
            // ProcessItem
            // 
            this.ProcessItem.HeaderText = "处理类型";
            this.ProcessItem.Items.AddRange(new object[] {
            "模板匹配",
            "二维码",
            "找边",
            "找圆",
            "特征抽取",
            "一维码"});
            this.ProcessItem.Name = "ProcessItem";
            this.ProcessItem.Width = 70;
            // 
            // Sel
            // 
            this.Sel.HeaderText = "相机";
            this.Sel.Name = "Sel";
            this.Sel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Sel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Sel.Width = 60;
            // 
            // ExposureTime
            // 
            this.ExposureTime.HeaderText = "曝光值";
            this.ExposureTime.Name = "ExposureTime";
            this.ExposureTime.Width = 60;
            // 
            // GainVal
            // 
            this.GainVal.HeaderText = "增益值";
            this.GainVal.Name = "GainVal";
            this.GainVal.Width = 60;
            // 
            // Test
            // 
            this.Test.HeaderText = "光源亮度";
            this.Test.Name = "Test";
            this.Test.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Test.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Test.Width = 80;
            // 
            // textBox_GainVal
            // 
            this.textBox_GainVal.Location = new System.Drawing.Point(452, 134);
            this.textBox_GainVal.Name = "textBox_GainVal";
            this.textBox_GainVal.Size = new System.Drawing.Size(80, 21);
            this.textBox_GainVal.TabIndex = 5;
            // 
            // textBox_exposureTimeVal
            // 
            this.textBox_exposureTimeVal.Location = new System.Drawing.Point(453, 76);
            this.textBox_exposureTimeVal.Name = "textBox_exposureTimeVal";
            this.textBox_exposureTimeVal.Size = new System.Drawing.Size(79, 21);
            this.textBox_exposureTimeVal.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(454, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "曝光值";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(454, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "增益值";
            // 
            // comboBox_SelCam
            // 
            this.comboBox_SelCam.FormattingEnabled = true;
            this.comboBox_SelCam.Location = new System.Drawing.Point(450, 12);
            this.comboBox_SelCam.Name = "comboBox_SelCam";
            this.comboBox_SelCam.Size = new System.Drawing.Size(82, 20);
            this.comboBox_SelCam.TabIndex = 9;
            this.comboBox_SelCam.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelCam_SelectedIndexChanged);
            // 
            // panel_VisionCtrls
            // 
            this.panel_VisionCtrls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_VisionCtrls.Controls.Add(this.vision_FindCircleCtr1);
            this.panel_VisionCtrls.Controls.Add(this.vision_1BarCodeSetCtr1);
            this.panel_VisionCtrls.Controls.Add(this.roundButton_ShapeROIAdd);
            this.panel_VisionCtrls.Controls.Add(this.roundButton_ShapeROISub);
            this.panel_VisionCtrls.Controls.Add(this.vision_2dCodeSetCtr1);
            this.panel_VisionCtrls.Controls.Add(this.roundButton_Test);
            this.panel_VisionCtrls.Controls.Add(this.Vision_MatchSetCtr1);
            this.panel_VisionCtrls.Controls.Add(this.roundButton_DrawRect1);
            this.panel_VisionCtrls.Controls.Add(this.roundButton_SeachArea);
            this.panel_VisionCtrls.Location = new System.Drawing.Point(12, 460);
            this.panel_VisionCtrls.Name = "panel_VisionCtrls";
            this.panel_VisionCtrls.Size = new System.Drawing.Size(1247, 204);
            this.panel_VisionCtrls.TabIndex = 14;
            // 
            // vision_FindCircleCtr1
            // 
            this.vision_FindCircleCtr1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.vision_FindCircleCtr1.Location = new System.Drawing.Point(1, -1);
            this.vision_FindCircleCtr1.Margin = new System.Windows.Forms.Padding(1);
            this.vision_FindCircleCtr1.Name = "vision_FindCircleCtr1";
            this.vision_FindCircleCtr1.Size = new System.Drawing.Size(858, 195);
            this.vision_FindCircleCtr1.strPath = "";
            this.vision_FindCircleCtr1.TabIndex = 17;
            // 
            // vision_1BarCodeSetCtr1
            // 
            this.vision_1BarCodeSetCtr1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.vision_1BarCodeSetCtr1.Location = new System.Drawing.Point(-2, -1);
            this.vision_1BarCodeSetCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.vision_1BarCodeSetCtr1.Name = "vision_1BarCodeSetCtr1";
            this.vision_1BarCodeSetCtr1.Size = new System.Drawing.Size(762, 199);
            this.vision_1BarCodeSetCtr1.strPath = "";
            this.vision_1BarCodeSetCtr1.TabIndex = 16;
            // 
            // roundButton_ShapeROIAdd
            // 
            this.roundButton_ShapeROIAdd.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_ShapeROIAdd.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_ShapeROIAdd.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_ShapeROIAdd.FlatAppearance.BorderSize = 0;
            this.roundButton_ShapeROIAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_ShapeROIAdd.ImageHeight = 80;
            this.roundButton_ShapeROIAdd.ImageWidth = 80;
            this.roundButton_ShapeROIAdd.Location = new System.Drawing.Point(1045, 18);
            this.roundButton_ShapeROIAdd.Name = "roundButton_ShapeROIAdd";
            this.roundButton_ShapeROIAdd.Radius = 24;
            this.roundButton_ShapeROIAdd.Size = new System.Drawing.Size(75, 28);
            this.roundButton_ShapeROIAdd.SpliteButtonWidth = 18;
            this.roundButton_ShapeROIAdd.TabIndex = 15;
            this.roundButton_ShapeROIAdd.Text = "任意+ROI";
            this.roundButton_ShapeROIAdd.UseVisualStyleBackColor = false;
            this.roundButton_ShapeROIAdd.Visible = false;
            this.roundButton_ShapeROIAdd.Click += new System.EventHandler(this.roundButton_ShapeROIAdd_Click);
            // 
            // roundButton_ShapeROISub
            // 
            this.roundButton_ShapeROISub.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_ShapeROISub.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_ShapeROISub.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_ShapeROISub.FlatAppearance.BorderSize = 0;
            this.roundButton_ShapeROISub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_ShapeROISub.ImageHeight = 80;
            this.roundButton_ShapeROISub.ImageWidth = 80;
            this.roundButton_ShapeROISub.Location = new System.Drawing.Point(1045, 69);
            this.roundButton_ShapeROISub.Name = "roundButton_ShapeROISub";
            this.roundButton_ShapeROISub.Radius = 24;
            this.roundButton_ShapeROISub.Size = new System.Drawing.Size(75, 28);
            this.roundButton_ShapeROISub.SpliteButtonWidth = 18;
            this.roundButton_ShapeROISub.TabIndex = 14;
            this.roundButton_ShapeROISub.Text = "任意-ROI";
            this.roundButton_ShapeROISub.UseVisualStyleBackColor = false;
            this.roundButton_ShapeROISub.Visible = false;
            this.roundButton_ShapeROISub.Click += new System.EventHandler(this.roundButton_ShapeROISub_Click);
            // 
            // vision_2dCodeSetCtr1
            // 
            this.vision_2dCodeSetCtr1.BackColor = System.Drawing.SystemColors.Info;
            this.vision_2dCodeSetCtr1.Location = new System.Drawing.Point(-21, 89);
            this.vision_2dCodeSetCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.vision_2dCodeSetCtr1.Name = "vision_2dCodeSetCtr1";
            this.vision_2dCodeSetCtr1.Size = new System.Drawing.Size(768, 195);
            this.vision_2dCodeSetCtr1.strPath = "";
            this.vision_2dCodeSetCtr1.TabIndex = 13;
            this.vision_2dCodeSetCtr1.Visible = false;
            // 
            // roundButton_Test
            // 
            this.roundButton_Test.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_Test.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Test.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Test.FlatAppearance.BorderSize = 0;
            this.roundButton_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_Test.ImageHeight = 80;
            this.roundButton_Test.ImageWidth = 80;
            this.roundButton_Test.Location = new System.Drawing.Point(1126, 69);
            this.roundButton_Test.Name = "roundButton_Test";
            this.roundButton_Test.Radius = 24;
            this.roundButton_Test.Size = new System.Drawing.Size(75, 28);
            this.roundButton_Test.SpliteButtonWidth = 18;
            this.roundButton_Test.TabIndex = 12;
            this.roundButton_Test.Text = "测试";
            this.roundButton_Test.UseVisualStyleBackColor = false;
            this.roundButton_Test.Visible = false;
            this.roundButton_Test.Click += new System.EventHandler(this.roundButton_Test_Click);
            // 
            // Vision_MatchSetCtr1
            // 
            this.Vision_MatchSetCtr1.Location = new System.Drawing.Point(-2, -1);
            this.Vision_MatchSetCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.Vision_MatchSetCtr1.Name = "Vision_MatchSetCtr1";
            this.Vision_MatchSetCtr1.Size = new System.Drawing.Size(876, 234);
            this.Vision_MatchSetCtr1.strPath = "";
            this.Vision_MatchSetCtr1.TabIndex = 0;
            this.Vision_MatchSetCtr1.Visible = false;
            // 
            // roundButton_DrawRect1
            // 
            this.roundButton_DrawRect1.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_DrawRect1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_DrawRect1.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_DrawRect1.FlatAppearance.BorderSize = 0;
            this.roundButton_DrawRect1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_DrawRect1.ImageHeight = 80;
            this.roundButton_DrawRect1.ImageWidth = 80;
            this.roundButton_DrawRect1.Location = new System.Drawing.Point(953, 18);
            this.roundButton_DrawRect1.Name = "roundButton_DrawRect1";
            this.roundButton_DrawRect1.Radius = 24;
            this.roundButton_DrawRect1.Size = new System.Drawing.Size(75, 28);
            this.roundButton_DrawRect1.SpliteButtonWidth = 18;
            this.roundButton_DrawRect1.TabIndex = 10;
            this.roundButton_DrawRect1.Text = "矩形1ROI";
            this.roundButton_DrawRect1.UseVisualStyleBackColor = false;
            this.roundButton_DrawRect1.Visible = false;
            this.roundButton_DrawRect1.Click += new System.EventHandler(this.roundButton_DrawRect1_Click);
            // 
            // roundButton_SeachArea
            // 
            this.roundButton_SeachArea.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_SeachArea.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_SeachArea.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_SeachArea.FlatAppearance.BorderSize = 0;
            this.roundButton_SeachArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_SeachArea.ImageHeight = 80;
            this.roundButton_SeachArea.ImageWidth = 80;
            this.roundButton_SeachArea.Location = new System.Drawing.Point(1126, 18);
            this.roundButton_SeachArea.Name = "roundButton_SeachArea";
            this.roundButton_SeachArea.Radius = 24;
            this.roundButton_SeachArea.Size = new System.Drawing.Size(75, 28);
            this.roundButton_SeachArea.SpliteButtonWidth = 18;
            this.roundButton_SeachArea.TabIndex = 11;
            this.roundButton_SeachArea.Text = "搜索区域";
            this.roundButton_SeachArea.UseVisualStyleBackColor = false;
            this.roundButton_SeachArea.Visible = false;
            this.roundButton_SeachArea.Click += new System.EventHandler(this.roundButton_SeachArea_Click);
            // 
            // roundButton_ReadImg
            // 
            this.roundButton_ReadImg.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_ReadImg.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_ReadImg.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_ReadImg.FlatAppearance.BorderSize = 0;
            this.roundButton_ReadImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_ReadImg.ImageHeight = 80;
            this.roundButton_ReadImg.ImageWidth = 80;
            this.roundButton_ReadImg.Location = new System.Drawing.Point(456, 300);
            this.roundButton_ReadImg.Name = "roundButton_ReadImg";
            this.roundButton_ReadImg.Radius = 24;
            this.roundButton_ReadImg.Size = new System.Drawing.Size(80, 29);
            this.roundButton_ReadImg.SpliteButtonWidth = 18;
            this.roundButton_ReadImg.TabIndex = 17;
            this.roundButton_ReadImg.Text = "读取图片";
            this.roundButton_ReadImg.UseVisualStyleBackColor = false;
            this.roundButton_ReadImg.Click += new System.EventHandler(this.roundButton_ReadImg_Click);
            // 
            // roundButton_SnapSave
            // 
            this.roundButton_SnapSave.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_SnapSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_SnapSave.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_SnapSave.FlatAppearance.BorderSize = 0;
            this.roundButton_SnapSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_SnapSave.ImageHeight = 80;
            this.roundButton_SnapSave.ImageWidth = 80;
            this.roundButton_SnapSave.Location = new System.Drawing.Point(456, 257);
            this.roundButton_SnapSave.Name = "roundButton_SnapSave";
            this.roundButton_SnapSave.Radius = 24;
            this.roundButton_SnapSave.Size = new System.Drawing.Size(80, 29);
            this.roundButton_SnapSave.SpliteButtonWidth = 18;
            this.roundButton_SnapSave.TabIndex = 16;
            this.roundButton_SnapSave.Text = "拍照保存";
            this.roundButton_SnapSave.UseVisualStyleBackColor = false;
            this.roundButton_SnapSave.Click += new System.EventHandler(this.roundButton_SnapSave_Click);
            // 
            // roundButton_AddItem
            // 
            this.roundButton_AddItem.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_AddItem.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_AddItem.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_AddItem.FlatAppearance.BorderSize = 0;
            this.roundButton_AddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_AddItem.ImageHeight = 80;
            this.roundButton_AddItem.ImageWidth = 80;
            this.roundButton_AddItem.Location = new System.Drawing.Point(11, 411);
            this.roundButton_AddItem.Name = "roundButton_AddItem";
            this.roundButton_AddItem.Radius = 24;
            this.roundButton_AddItem.Size = new System.Drawing.Size(75, 28);
            this.roundButton_AddItem.SpliteButtonWidth = 18;
            this.roundButton_AddItem.TabIndex = 15;
            this.roundButton_AddItem.Text = "添加";
            this.roundButton_AddItem.UseVisualStyleBackColor = false;
            this.roundButton_AddItem.Click += new System.EventHandler(this.roundButton_AddItem_Click);
            // 
            // roundButton_Save
            // 
            this.roundButton_Save.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_Save.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Save.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Save.FlatAppearance.BorderSize = 0;
            this.roundButton_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_Save.ImageHeight = 80;
            this.roundButton_Save.ImageWidth = 80;
            this.roundButton_Save.Location = new System.Drawing.Point(215, 411);
            this.roundButton_Save.Name = "roundButton_Save";
            this.roundButton_Save.Radius = 24;
            this.roundButton_Save.Size = new System.Drawing.Size(75, 28);
            this.roundButton_Save.SpliteButtonWidth = 18;
            this.roundButton_Save.TabIndex = 13;
            this.roundButton_Save.Text = "保存";
            this.roundButton_Save.UseVisualStyleBackColor = false;
            this.roundButton_Save.Click += new System.EventHandler(this.roundButton_Save_Click);
            // 
            // roundButton_DelItem
            // 
            this.roundButton_DelItem.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_DelItem.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_DelItem.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_DelItem.FlatAppearance.BorderSize = 0;
            this.roundButton_DelItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_DelItem.ImageHeight = 80;
            this.roundButton_DelItem.ImageWidth = 80;
            this.roundButton_DelItem.Location = new System.Drawing.Point(109, 411);
            this.roundButton_DelItem.Name = "roundButton_DelItem";
            this.roundButton_DelItem.Radius = 24;
            this.roundButton_DelItem.Size = new System.Drawing.Size(75, 28);
            this.roundButton_DelItem.SpliteButtonWidth = 18;
            this.roundButton_DelItem.TabIndex = 12;
            this.roundButton_DelItem.Text = "删除";
            this.roundButton_DelItem.UseVisualStyleBackColor = false;
            this.roundButton_DelItem.Click += new System.EventHandler(this.roundButton_DelItem_Click);
            // 
            // roundButton1
            // 
            this.roundButton1.BackColor = System.Drawing.Color.Transparent;
            this.roundButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton1.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton1.FlatAppearance.BorderSize = 0;
            this.roundButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton1.ImageHeight = 80;
            this.roundButton1.ImageWidth = 80;
            this.roundButton1.Location = new System.Drawing.Point(456, 214);
            this.roundButton1.Name = "roundButton1";
            this.roundButton1.Radius = 24;
            this.roundButton1.Size = new System.Drawing.Size(80, 29);
            this.roundButton1.SpliteButtonWidth = 18;
            this.roundButton1.TabIndex = 4;
            this.roundButton1.Text = "单次拍照";
            this.roundButton1.UseVisualStyleBackColor = false;
            this.roundButton1.Click += new System.EventHandler(this.roundButton1_Click);
            // 
            // roundButton_ContinuousSnap
            // 
            this.roundButton_ContinuousSnap.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_ContinuousSnap.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_ContinuousSnap.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_ContinuousSnap.FlatAppearance.BorderSize = 0;
            this.roundButton_ContinuousSnap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_ContinuousSnap.ImageHeight = 80;
            this.roundButton_ContinuousSnap.ImageWidth = 80;
            this.roundButton_ContinuousSnap.Location = new System.Drawing.Point(456, 171);
            this.roundButton_ContinuousSnap.Name = "roundButton_ContinuousSnap";
            this.roundButton_ContinuousSnap.Radius = 24;
            this.roundButton_ContinuousSnap.Size = new System.Drawing.Size(80, 29);
            this.roundButton_ContinuousSnap.SpliteButtonWidth = 18;
            this.roundButton_ContinuousSnap.TabIndex = 3;
            this.roundButton_ContinuousSnap.Text = "连续拍照";
            this.roundButton_ContinuousSnap.UseVisualStyleBackColor = false;
            this.roundButton_ContinuousSnap.Click += new System.EventHandler(this.roundButton_ContinuousSnap_Click);
            // 
            // visionControl1
            // 
            this.visionControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.visionControl1.ImgHight = 1944;
            this.visionControl1.ImgWidth = 2592;
            this.visionControl1.Location = new System.Drawing.Point(538, -5);
            this.visionControl1.Name = "visionControl1";
            this.visionControl1.Size = new System.Drawing.Size(721, 444);
            this.visionControl1.TabIndex = 2;
            this.visionControl1.TabStop = false;
            // 
            // Form_VisionDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 676);
            this.Controls.Add(this.roundButton_ReadImg);
            this.Controls.Add(this.roundButton_SnapSave);
            this.Controls.Add(this.roundButton_AddItem);
            this.Controls.Add(this.panel_VisionCtrls);
            this.Controls.Add(this.roundButton_Save);
            this.Controls.Add(this.roundButton_DelItem);
            this.Controls.Add(this.comboBox_SelCam);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_exposureTimeVal);
            this.Controls.Add(this.textBox_GainVal);
            this.Controls.Add(this.roundButton1);
            this.Controls.Add(this.roundButton_ContinuousSnap);
            this.Controls.Add(this.visionControl1);
            this.Controls.Add(this.dataGridViewProcessItem);
            this.Controls.Add(this.tabControl_VisionStepPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_VisionDebug";
            this.Load += new System.EventHandler(this.Form_CameraDebug_Load);
            this.VisibleChanged += new System.EventHandler(this.OnShowChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcessItem)).EndInit();
            this.panel_VisionCtrls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_VisionStepPages;
        private System.Windows.Forms.DataGridView dataGridViewProcessItem;
        private UserCtrl.VisionControl visionControl1;
        private AutoFrameUI.RoundButton roundButton_ContinuousSnap;
        private AutoFrameUI.RoundButton roundButton1;
        private System.Windows.Forms.TextBox textBox_GainVal;
        private System.Windows.Forms.TextBox textBox_exposureTimeVal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_SelCam;
        private AutoFrameUI.RoundButton roundButton_DrawRect1;
        private AutoFrameUI.RoundButton roundButton_SeachArea;
        private AutoFrameUI.RoundButton roundButton_DelItem;
        private AutoFrameUI.RoundButton roundButton_Save;
        private System.Windows.Forms.Panel panel_VisionCtrls;
        private VisionProcess.VisionMatchSetCtr Vision_MatchSetCtr1;
        private AutoFrameUI.RoundButton roundButton_AddItem;
        private AutoFrameUI.RoundButton roundButton_SnapSave;
        private AutoFrameUI.RoundButton roundButton_ReadImg;
        private AutoFrameUI.RoundButton roundButton_Test;
        private VisionProcess.Vision2dCodeSetCtr vision_2dCodeSetCtr1;
        private AutoFrameUI.RoundButton roundButton_ShapeROIAdd;
        private AutoFrameUI.RoundButton roundButton_ShapeROISub;
        private VisionProcess.Vision1BarCodeSetCtr vision_1BarCodeSetCtr1;
        private VisionProcess.VisionFindCircleCtr vision_FindCircleCtr1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn VisionItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn ProcessItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn Sel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExposureTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn GainVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Test;
    }
}