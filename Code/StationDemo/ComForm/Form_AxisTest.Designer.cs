namespace StationDemo
{
    partial class Form_AxisTest
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
            this.dataGridView_AxisParamSet = new System.Windows.Forms.DataGridView();
            this.AxisNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.HighSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Acc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MidSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MDec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LDcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlusePerRote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisLeadRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaveMotorParam = new System.Windows.Forms.Button();
            this.Test = new System.Windows.Forms.Button();
            this.radioButton_HighSpeed = new System.Windows.Forms.RadioButton();
            this.radioButton_MidSpeed = new System.Windows.Forms.RadioButton();
            this.radioButton_LowSpeed = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_MoveDistance = new System.Windows.Forms.TextBox();
            this.textBox_Count = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_MotorTest = new System.Windows.Forms.GroupBox();
            this.Btn_TestStop = new System.Windows.Forms.Button();
            this.textBox_InPosDelay = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_AxisNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView_HomeSet = new System.Windows.Forms.DataGridView();
            this.HomeAxisNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeAxisName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.HomeDir = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.HomeHighVel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeAccH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeDecH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeLowVel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeAccL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeDecL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomeOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRlsDistance = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.BtnXDLaserTest = new System.Windows.Forms.Button();
            this.txtStepDistance = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEndPos = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtStartPos = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_AxisParamSet)).BeginInit();
            this.groupBox_MotorTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_HomeSet)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_AxisParamSet
            // 
            this.dataGridView_AxisParamSet.AllowUserToAddRows = false;
            this.dataGridView_AxisParamSet.AllowUserToDeleteRows = false;
            this.dataGridView_AxisParamSet.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_AxisParamSet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_AxisParamSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_AxisParamSet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AxisNo,
            this.AxisName,
            this.AxisType,
            this.HighSpeed,
            this.Acc,
            this.Dec,
            this.MidSpeed,
            this.MAcc,
            this.MDec,
            this.LowSpeed,
            this.LAcc,
            this.LDcc,
            this.PlusePerRote,
            this.AxisLeadRange});
            this.dataGridView_AxisParamSet.Location = new System.Drawing.Point(10, 11);
            this.dataGridView_AxisParamSet.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_AxisParamSet.Name = "dataGridView_AxisParamSet";
            this.dataGridView_AxisParamSet.RowHeadersVisible = false;
            this.dataGridView_AxisParamSet.RowHeadersWidth = 20;
            this.dataGridView_AxisParamSet.RowTemplate.Height = 18;
            this.dataGridView_AxisParamSet.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_AxisParamSet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_AxisParamSet.Size = new System.Drawing.Size(1057, 292);
            this.dataGridView_AxisParamSet.TabIndex = 0;
            // 
            // AxisNo
            // 
            this.AxisNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AxisNo.Frozen = true;
            this.AxisNo.HeaderText = "轴号";
            this.AxisNo.Name = "AxisNo";
            this.AxisNo.ReadOnly = true;
            this.AxisNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AxisNo.Width = 50;
            // 
            // AxisName
            // 
            this.AxisName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AxisName.Frozen = true;
            this.AxisName.HeaderText = "轴名";
            this.AxisName.Name = "AxisName";
            this.AxisName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AxisType
            // 
            this.AxisType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AxisType.Frozen = true;
            this.AxisType.HeaderText = "轴类型";
            this.AxisType.Items.AddRange(new object[] {
            "SEVER",
            "STEP"});
            this.AxisType.Name = "AxisType";
            this.AxisType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AxisType.Width = 60;
            // 
            // HighSpeed
            // 
            this.HighSpeed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HighSpeed.Frozen = true;
            this.HighSpeed.HeaderText = "高速";
            this.HighSpeed.Name = "HighSpeed";
            this.HighSpeed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HighSpeed.Width = 80;
            // 
            // Acc
            // 
            this.Acc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Acc.Frozen = true;
            this.Acc.HeaderText = "高加速度";
            this.Acc.Name = "Acc";
            this.Acc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Acc.Width = 80;
            // 
            // Dec
            // 
            this.Dec.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Dec.Frozen = true;
            this.Dec.HeaderText = "高减速度";
            this.Dec.Name = "Dec";
            this.Dec.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Dec.Width = 80;
            // 
            // MidSpeed
            // 
            this.MidSpeed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MidSpeed.Frozen = true;
            this.MidSpeed.HeaderText = "中速";
            this.MidSpeed.Name = "MidSpeed";
            this.MidSpeed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MidSpeed.Width = 80;
            // 
            // MAcc
            // 
            this.MAcc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MAcc.Frozen = true;
            this.MAcc.HeaderText = "中加速度";
            this.MAcc.Name = "MAcc";
            this.MAcc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MAcc.Width = 80;
            // 
            // MDec
            // 
            this.MDec.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MDec.Frozen = true;
            this.MDec.HeaderText = "中减速度";
            this.MDec.Name = "MDec";
            this.MDec.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MDec.Width = 80;
            // 
            // LowSpeed
            // 
            this.LowSpeed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LowSpeed.Frozen = true;
            this.LowSpeed.HeaderText = "低速";
            this.LowSpeed.Name = "LowSpeed";
            this.LowSpeed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LowSpeed.Width = 80;
            // 
            // LAcc
            // 
            this.LAcc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LAcc.Frozen = true;
            this.LAcc.HeaderText = "低加速度";
            this.LAcc.Name = "LAcc";
            this.LAcc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LAcc.Width = 80;
            // 
            // LDcc
            // 
            this.LDcc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LDcc.Frozen = true;
            this.LDcc.HeaderText = "低减速度";
            this.LDcc.Name = "LDcc";
            this.LDcc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LDcc.Width = 80;
            // 
            // PlusePerRote
            // 
            this.PlusePerRote.HeaderText = "每转脉冲";
            this.PlusePerRote.Name = "PlusePerRote";
            this.PlusePerRote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PlusePerRote.Width = 50;
            // 
            // AxisLeadRange
            // 
            this.AxisLeadRange.HeaderText = "导程";
            this.AxisLeadRange.Name = "AxisLeadRange";
            this.AxisLeadRange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AxisLeadRange.Width = 50;
            // 
            // SaveMotorParam
            // 
            this.SaveMotorParam.Location = new System.Drawing.Point(760, 330);
            this.SaveMotorParam.Margin = new System.Windows.Forms.Padding(2);
            this.SaveMotorParam.Name = "SaveMotorParam";
            this.SaveMotorParam.Size = new System.Drawing.Size(72, 35);
            this.SaveMotorParam.TabIndex = 1;
            this.SaveMotorParam.Text = "保存";
            this.SaveMotorParam.UseVisualStyleBackColor = true;
            this.SaveMotorParam.Click += new System.EventHandler(this.SaveMotorParam_Click);
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(40, 49);
            this.Test.Margin = new System.Windows.Forms.Padding(2);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(73, 32);
            this.Test.TabIndex = 2;
            this.Test.Text = "测试";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // radioButton_HighSpeed
            // 
            this.radioButton_HighSpeed.AutoSize = true;
            this.radioButton_HighSpeed.Location = new System.Drawing.Point(1, 18);
            this.radioButton_HighSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_HighSpeed.Name = "radioButton_HighSpeed";
            this.radioButton_HighSpeed.Size = new System.Drawing.Size(47, 16);
            this.radioButton_HighSpeed.TabIndex = 3;
            this.radioButton_HighSpeed.TabStop = true;
            this.radioButton_HighSpeed.Text = "高速";
            this.radioButton_HighSpeed.UseVisualStyleBackColor = true;
            // 
            // radioButton_MidSpeed
            // 
            this.radioButton_MidSpeed.AutoSize = true;
            this.radioButton_MidSpeed.Location = new System.Drawing.Point(1, 45);
            this.radioButton_MidSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_MidSpeed.Name = "radioButton_MidSpeed";
            this.radioButton_MidSpeed.Size = new System.Drawing.Size(47, 16);
            this.radioButton_MidSpeed.TabIndex = 4;
            this.radioButton_MidSpeed.TabStop = true;
            this.radioButton_MidSpeed.Text = "中速";
            this.radioButton_MidSpeed.UseVisualStyleBackColor = true;
            // 
            // radioButton_LowSpeed
            // 
            this.radioButton_LowSpeed.AutoSize = true;
            this.radioButton_LowSpeed.Location = new System.Drawing.Point(1, 73);
            this.radioButton_LowSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_LowSpeed.Name = "radioButton_LowSpeed";
            this.radioButton_LowSpeed.Size = new System.Drawing.Size(47, 16);
            this.radioButton_LowSpeed.TabIndex = 5;
            this.radioButton_LowSpeed.TabStop = true;
            this.radioButton_LowSpeed.Text = "低速";
            this.radioButton_LowSpeed.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "移动距离";
            // 
            // textBox_MoveDistance
            // 
            this.textBox_MoveDistance.Location = new System.Drawing.Point(60, 22);
            this.textBox_MoveDistance.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_MoveDistance.Name = "textBox_MoveDistance";
            this.textBox_MoveDistance.Size = new System.Drawing.Size(81, 21);
            this.textBox_MoveDistance.TabIndex = 7;
            // 
            // textBox_Count
            // 
            this.textBox_Count.Location = new System.Drawing.Point(115, 68);
            this.textBox_Count.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Count.Name = "textBox_Count";
            this.textBox_Count.Size = new System.Drawing.Size(81, 21);
            this.textBox_Count.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "往返次数";
            // 
            // groupBox_MotorTest
            // 
            this.groupBox_MotorTest.Controls.Add(this.Test);
            this.groupBox_MotorTest.Controls.Add(this.textBox_MoveDistance);
            this.groupBox_MotorTest.Controls.Add(this.label1);
            this.groupBox_MotorTest.Location = new System.Drawing.Point(239, 311);
            this.groupBox_MotorTest.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_MotorTest.Name = "groupBox_MotorTest";
            this.groupBox_MotorTest.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_MotorTest.Size = new System.Drawing.Size(146, 105);
            this.groupBox_MotorTest.TabIndex = 10;
            this.groupBox_MotorTest.TabStop = false;
            this.groupBox_MotorTest.Text = "千分表测试";
            // 
            // Btn_TestStop
            // 
            this.Btn_TestStop.Location = new System.Drawing.Point(760, 383);
            this.Btn_TestStop.Margin = new System.Windows.Forms.Padding(2);
            this.Btn_TestStop.Name = "Btn_TestStop";
            this.Btn_TestStop.Size = new System.Drawing.Size(72, 37);
            this.Btn_TestStop.TabIndex = 14;
            this.Btn_TestStop.Text = "停止";
            this.Btn_TestStop.UseVisualStyleBackColor = true;
            this.Btn_TestStop.Click += new System.EventHandler(this.Btn_TestStop_Click);
            // 
            // textBox_InPosDelay
            // 
            this.textBox_InPosDelay.Location = new System.Drawing.Point(115, 44);
            this.textBox_InPosDelay.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_InPosDelay.Name = "textBox_InPosDelay";
            this.textBox_InPosDelay.Size = new System.Drawing.Size(81, 21);
            this.textBox_InPosDelay.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 47);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "到位延时";
            // 
            // textBox_AxisNo
            // 
            this.textBox_AxisNo.Location = new System.Drawing.Point(115, 18);
            this.textBox_AxisNo.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_AxisNo.Name = "textBox_AxisNo";
            this.textBox_AxisNo.Size = new System.Drawing.Size(81, 21);
            this.textBox_AxisNo.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 24);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "轴号";
            // 
            // dataGridView_HomeSet
            // 
            this.dataGridView_HomeSet.AllowUserToAddRows = false;
            this.dataGridView_HomeSet.AllowUserToDeleteRows = false;
            this.dataGridView_HomeSet.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_HomeSet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_HomeSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_HomeSet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HomeAxisNo,
            this.HomeAxisName,
            this.HomeMode,
            this.HomeDir,
            this.HomeHighVel,
            this.HomeAccH,
            this.HomeDecH,
            this.HomeLowVel,
            this.HomeAccL,
            this.HomeDecL,
            this.HomeOffset});
            this.dataGridView_HomeSet.Location = new System.Drawing.Point(0, 441);
            this.dataGridView_HomeSet.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_HomeSet.Name = "dataGridView_HomeSet";
            this.dataGridView_HomeSet.RowHeadersVisible = false;
            this.dataGridView_HomeSet.RowHeadersWidth = 20;
            this.dataGridView_HomeSet.RowTemplate.Height = 18;
            this.dataGridView_HomeSet.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_HomeSet.Size = new System.Drawing.Size(1057, 257);
            this.dataGridView_HomeSet.TabIndex = 11;
            // 
            // HomeAxisNo
            // 
            this.HomeAxisNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeAxisNo.Frozen = true;
            this.HomeAxisNo.HeaderText = "轴号";
            this.HomeAxisNo.Name = "HomeAxisNo";
            this.HomeAxisNo.ReadOnly = true;
            this.HomeAxisNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HomeAxisNo.Width = 50;
            // 
            // HomeAxisName
            // 
            this.HomeAxisName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeAxisName.Frozen = true;
            this.HomeAxisName.HeaderText = "轴名";
            this.HomeAxisName.Name = "HomeAxisName";
            this.HomeAxisName.ReadOnly = true;
            this.HomeAxisName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // HomeMode
            // 
            this.HomeMode.Frozen = true;
            this.HomeMode.HeaderText = "方式";
            this.HomeMode.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35"});
            this.HomeMode.Name = "HomeMode";
            this.HomeMode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // HomeDir
            // 
            this.HomeDir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeDir.HeaderText = "方向(true:正）";
            this.HomeDir.Items.AddRange(new object[] {
            "False",
            "True"});
            this.HomeDir.Name = "HomeDir";
            this.HomeDir.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.HomeDir.Width = 130;
            // 
            // HomeHighVel
            // 
            this.HomeHighVel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeHighVel.HeaderText = "高速";
            this.HomeHighVel.Name = "HomeHighVel";
            this.HomeHighVel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HomeHighVel.Width = 80;
            // 
            // HomeAccH
            // 
            this.HomeAccH.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeAccH.HeaderText = "加速度";
            this.HomeAccH.Name = "HomeAccH";
            this.HomeAccH.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HomeAccH.Width = 80;
            // 
            // HomeDecH
            // 
            this.HomeDecH.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeDecH.HeaderText = "减速度";
            this.HomeDecH.Name = "HomeDecH";
            this.HomeDecH.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HomeDecH.Width = 80;
            // 
            // HomeLowVel
            // 
            this.HomeLowVel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeLowVel.HeaderText = "低速";
            this.HomeLowVel.Name = "HomeLowVel";
            this.HomeLowVel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HomeLowVel.Width = 80;
            // 
            // HomeAccL
            // 
            this.HomeAccL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeAccL.HeaderText = "低加速度";
            this.HomeAccL.Name = "HomeAccL";
            this.HomeAccL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // HomeDecL
            // 
            this.HomeDecL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeDecL.HeaderText = "低加速度";
            this.HomeDecL.Name = "HomeDecL";
            this.HomeDecL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // HomeOffset
            // 
            this.HomeOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HomeOffset.HeaderText = "原点偏移";
            this.HomeOffset.Name = "HomeOffset";
            this.HomeOffset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 424);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "原点设置";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRlsDistance);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.BtnXDLaserTest);
            this.groupBox1.Controls.Add(this.txtStepDistance);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtEndPos);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtStartPos);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(390, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 101);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "干涉仪测试";
            // 
            // txtRlsDistance
            // 
            this.txtRlsDistance.Location = new System.Drawing.Point(203, 56);
            this.txtRlsDistance.Margin = new System.Windows.Forms.Padding(2);
            this.txtRlsDistance.Name = "txtRlsDistance";
            this.txtRlsDistance.Size = new System.Drawing.Size(81, 21);
            this.txtRlsDistance.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(143, 60);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "反向距离：";
            // 
            // BtnXDLaserTest
            // 
            this.BtnXDLaserTest.Location = new System.Drawing.Point(287, 50);
            this.BtnXDLaserTest.Margin = new System.Windows.Forms.Padding(2);
            this.BtnXDLaserTest.Name = "BtnXDLaserTest";
            this.BtnXDLaserTest.Size = new System.Drawing.Size(73, 32);
            this.BtnXDLaserTest.TabIndex = 8;
            this.BtnXDLaserTest.Text = "测试";
            this.BtnXDLaserTest.UseVisualStyleBackColor = true;
            this.BtnXDLaserTest.Click += new System.EventHandler(this.BtnXDLaserTest_Click);
            // 
            // txtStepDistance
            // 
            this.txtStepDistance.Location = new System.Drawing.Point(203, 25);
            this.txtStepDistance.Margin = new System.Windows.Forms.Padding(2);
            this.txtStepDistance.Name = "txtStepDistance";
            this.txtStepDistance.Size = new System.Drawing.Size(81, 21);
            this.txtStepDistance.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(143, 29);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "间隔步长：";
            // 
            // txtEndPos
            // 
            this.txtEndPos.Location = new System.Drawing.Point(60, 57);
            this.txtEndPos.Margin = new System.Windows.Forms.Padding(2);
            this.txtEndPos.Name = "txtEndPos";
            this.txtEndPos.Size = new System.Drawing.Size(81, 21);
            this.txtEndPos.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 61);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "终止坐标";
            // 
            // txtStartPos
            // 
            this.txtStartPos.Location = new System.Drawing.Point(60, 23);
            this.txtStartPos.Margin = new System.Windows.Forms.Padding(2);
            this.txtStartPos.Name = "txtStartPos";
            this.txtStartPos.Size = new System.Drawing.Size(81, 21);
            this.txtStartPos.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "起始坐标";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_HighSpeed);
            this.groupBox2.Controls.Add(this.textBox_InPosDelay);
            this.groupBox2.Controls.Add(this.radioButton_LowSpeed);
            this.groupBox2.Controls.Add(this.textBox_Count);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.radioButton_MidSpeed);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_AxisNo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(10, 315);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(224, 100);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "电机选择";
            // 
            // Form_AxisTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1134, 718);
            this.Controls.Add(this.Btn_TestStop);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.SaveMotorParam);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridView_HomeSet);
            this.Controls.Add(this.groupBox_MotorTest);
            this.Controls.Add(this.dataGridView_AxisParamSet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form_AxisTest";
            this.Text = "Form_AxisTest";
            this.Load += new System.EventHandler(this.Form_AxisTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_AxisParamSet)).EndInit();
            this.groupBox_MotorTest.ResumeLayout(false);
            this.groupBox_MotorTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_HomeSet)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_AxisParamSet;
        private System.Windows.Forms.Button SaveMotorParam;
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.RadioButton radioButton_HighSpeed;
        private System.Windows.Forms.RadioButton radioButton_MidSpeed;
        private System.Windows.Forms.RadioButton radioButton_LowSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_MoveDistance;
        private System.Windows.Forms.TextBox textBox_Count;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox_MotorTest;
        private System.Windows.Forms.DataGridView dataGridView_HomeSet;
        private System.Windows.Forms.TextBox textBox_AxisNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_InPosDelay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Btn_TestStop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRlsDistance;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BtnXDLaserTest;
        private System.Windows.Forms.TextBox txtStepDistance;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEndPos;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtStartPos;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisName;
        private System.Windows.Forms.DataGridViewComboBoxColumn AxisType;
        private System.Windows.Forms.DataGridViewTextBoxColumn HighSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Acc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dec;
        private System.Windows.Forms.DataGridViewTextBoxColumn MidSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn MDec;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn LDcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlusePerRote;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisLeadRange;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeAxisNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeAxisName;
        private System.Windows.Forms.DataGridViewComboBoxColumn HomeMode;
        private System.Windows.Forms.DataGridViewComboBoxColumn HomeDir;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeHighVel;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeAccH;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeDecH;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeLowVel;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeAccL;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeDecL;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomeOffset;
    }
}