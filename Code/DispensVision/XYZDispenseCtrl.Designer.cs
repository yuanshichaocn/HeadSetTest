namespace XYZDispensVision
{
    partial class DispenseCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnPinHeight = new System.Windows.Forms.Button();
            this.BtnLaserCalib = new System.Windows.Forms.Button();
            this.BtnCalibMotion = new System.Windows.Forms.Button();
            this.BtnLaserTest = new System.Windows.Forms.Button();
            this.BtnPinTest = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_DispCalbLightVal = new System.Windows.Forms.TextBox();
            this.labelLightDispCalib = new System.Windows.Forms.Label();
            this.textExpsoure = new System.Windows.Forms.TextBox();
            this.textGain = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textLaserYOffset = new System.Windows.Forms.TextBox();
            this.textLaserXOffset = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textPinYOffset = new System.Windows.Forms.TextBox();
            this.textPinXOffset = new System.Windows.Forms.TextBox();
            this.textYStep = new System.Windows.Forms.TextBox();
            this.textXStep = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_RecordPoint = new System.Windows.Forms.Button();
            this.button_AllAxisMove = new System.Windows.Forms.Button();
            this.button_SingleAxisMove = new System.Windows.Forms.Button();
            this.dataGridView_PointInfo = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Xpos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ypos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VisionDstSet = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_RecordPoint = new System.Windows.Forms.Button();
            this.btn_SingleAxisMove = new System.Windows.Forms.Button();
            this.dataGridView_DispPosList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_AllAxisMove = new System.Windows.Forms.Button();
            this.visionMatchSetCtr2 = new VisionProcess.VisionMatchSetCtr();
            this.textBox_DispDstLightVal = new System.Windows.Forms.TextBox();
            this.labelLightDispDst = new System.Windows.Forms.Label();
            this.textDstExpsoure = new System.Windows.Forms.TextBox();
            this.textDstGain = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnPrTest = new System.Windows.Forms.Button();
            this.BtnRoiplus = new System.Windows.Forms.Button();
            this.RoiSub = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.BtnSanp = new System.Windows.Forms.Button();
            this.btnSnapSave = new System.Windows.Forms.Button();
            this.ContinuousSnap = new System.Windows.Forms.Button();
            this.textExpsoureTimeVal = new System.Windows.Forms.TextBox();
            this.textGainVal = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.rightTab1 = new System.Windows.Forms.TabControl();
            this.VisionCalibSet = new System.Windows.Forms.TabPage();
            this.visionMatchSetCtr1 = new VisionProcess.VisionMatchSetCtr();
            this.lbldispensepressure = new System.Windows.Forms.Label();
            this.txtZLatchData = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnLaserReadData = new System.Windows.Forms.Button();
            this.txtLaserReadData = new System.Windows.Forms.TextBox();
            this.DispTrace = new System.Windows.Forms.TabPage();
            this.BtnDel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnDelay = new System.Windows.Forms.Button();
            this.BtnIO = new System.Windows.Forms.Button();
            this.BtnAddArc = new System.Windows.Forms.Button();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.BtnAddPoint = new System.Windows.Forms.Button();
            this.dataGridViewDispTrace = new System.Windows.Forms.DataGridView();
            this.DispName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.set = new System.Windows.Forms.DataGridViewButtonColumn();
            this.timerScan = new System.Windows.Forms.Timer(this.components);
            this.btnDispenseOutManual = new System.Windows.Forms.Button();
            this.txtManualDispenseOutTime = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnLightONOFF = new System.Windows.Forms.Button();
            this.button_Unegtive = new System.Windows.Forms.Button();
            this.button_Ynegtive = new System.Windows.Forms.Button();
            this.button_Znegtive = new System.Windows.Forms.Button();
            this.button_Zpositive = new System.Windows.Forms.Button();
            this.button_Xnegtive = new System.Windows.Forms.Button();
            this.button_Xpositive = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_Upositive = new System.Windows.Forms.Button();
            this.button_Ypositive = new System.Windows.Forms.Button();
            this.comboBox_SelMotionType = new System.Windows.Forms.ComboBox();
            this.button_ServoOnZ = new System.Windows.Forms.Button();
            this.button_ServoOnY = new System.Windows.Forms.Button();
            this.button_ServoOnX = new System.Windows.Forms.Button();
            this.label_CmdPosU = new System.Windows.Forms.Label();
            this.label_ActPosU = new System.Windows.Forms.Label();
            this.label_CmdPosZ = new System.Windows.Forms.Label();
            this.label_ActPosZ = new System.Windows.Forms.Label();
            this.label_CmdPosY = new System.Windows.Forms.Label();
            this.label_ActPosY = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label_CmdPosX = new System.Windows.Forms.Label();
            this.label_ActPosX = new System.Windows.Forms.Label();
            this.button_ServoOnU = new System.Windows.Forms.Button();
            this.button_homeU = new System.Windows.Forms.Button();
            this.button_homeZ = new System.Windows.Forms.Button();
            this.button_homeY = new System.Windows.Forms.Button();
            this.button_homeX = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.labelControl_EMGU = new System.Windows.Forms.Label();
            this.labelControl_ORIU = new System.Windows.Forms.Label();
            this.labelControl_LimtNU = new System.Windows.Forms.Label();
            this.labelControl_LimtPU = new System.Windows.Forms.Label();
            this.labelControl_AlarmU = new System.Windows.Forms.Label();
            this.labelControl_EMGZ = new System.Windows.Forms.Label();
            this.labelControl_ORIZ = new System.Windows.Forms.Label();
            this.labelControl_LimtNZ = new System.Windows.Forms.Label();
            this.labelControl_LimtPZ = new System.Windows.Forms.Label();
            this.labelControl_AlarmZ = new System.Windows.Forms.Label();
            this.labelControl_EMGY = new System.Windows.Forms.Label();
            this.labelControl_ORIY = new System.Windows.Forms.Label();
            this.labelControl_LimtNY = new System.Windows.Forms.Label();
            this.labelControl_LimtPY = new System.Windows.Forms.Label();
            this.labelControl_AlarmY = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.labelControl_EMGX = new System.Windows.Forms.Label();
            this.labelControl_ORIX = new System.Windows.Forms.Label();
            this.labelControl_LimtNX = new System.Windows.Forms.Label();
            this.labelControl_LimtPX = new System.Windows.Forms.Label();
            this.labelControl_AlarmX = new System.Windows.Forms.Label();
            this.MoveOperate = new System.Windows.Forms.GroupBox();
            this.visionControl1 = new UserCtrl.VisionControl();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PointInfo)).BeginInit();
            this.VisionDstSet.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DispPosList)).BeginInit();
            this.rightTab1.SuspendLayout();
            this.VisionCalibSet.SuspendLayout();
            this.DispTrace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDispTrace)).BeginInit();
            this.MoveOperate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPinHeight
            // 
            this.btnPinHeight.Location = new System.Drawing.Point(253, 400);
            this.btnPinHeight.Name = "btnPinHeight";
            this.btnPinHeight.Size = new System.Drawing.Size(100, 47);
            this.btnPinHeight.TabIndex = 10;
            this.btnPinHeight.Text = "针高标定";
            this.btnPinHeight.UseVisualStyleBackColor = true;
            this.btnPinHeight.Click += new System.EventHandler(this.btnPinHeight_Click);
            // 
            // BtnLaserCalib
            // 
            this.BtnLaserCalib.Location = new System.Drawing.Point(492, 392);
            this.BtnLaserCalib.Name = "BtnLaserCalib";
            this.BtnLaserCalib.Size = new System.Drawing.Size(100, 47);
            this.BtnLaserCalib.TabIndex = 9;
            this.BtnLaserCalib.Text = "相机镭射标定";
            this.BtnLaserCalib.UseVisualStyleBackColor = true;
            this.BtnLaserCalib.Click += new System.EventHandler(this.BtnLaserCalib_Click);
            // 
            // BtnCalibMotion
            // 
            this.BtnCalibMotion.Location = new System.Drawing.Point(6, 400);
            this.BtnCalibMotion.Name = "BtnCalibMotion";
            this.BtnCalibMotion.Size = new System.Drawing.Size(100, 47);
            this.BtnCalibMotion.TabIndex = 7;
            this.BtnCalibMotion.Text = "视觉标定";
            this.BtnCalibMotion.UseVisualStyleBackColor = true;
            this.BtnCalibMotion.Click += new System.EventHandler(this.BtnCalibMotion_Click);
            // 
            // BtnLaserTest
            // 
            this.BtnLaserTest.Location = new System.Drawing.Point(598, 393);
            this.BtnLaserTest.Name = "BtnLaserTest";
            this.BtnLaserTest.Size = new System.Drawing.Size(100, 47);
            this.BtnLaserTest.TabIndex = 6;
            this.BtnLaserTest.Text = "Laser跟踪测试";
            this.BtnLaserTest.UseVisualStyleBackColor = true;
            this.BtnLaserTest.Click += new System.EventHandler(this.BtnLaserTest_Click);
            // 
            // BtnPinTest
            // 
            this.BtnPinTest.Location = new System.Drawing.Point(133, 400);
            this.BtnPinTest.Name = "BtnPinTest";
            this.BtnPinTest.Size = new System.Drawing.Size(100, 47);
            this.BtnPinTest.TabIndex = 5;
            this.BtnPinTest.Text = "Pin跟踪测试";
            this.BtnPinTest.UseVisualStyleBackColor = true;
            this.BtnPinTest.Click += new System.EventHandler(this.BtnPinTest_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_DispCalbLightVal);
            this.groupBox2.Controls.Add(this.labelLightDispCalib);
            this.groupBox2.Controls.Add(this.textExpsoure);
            this.groupBox2.Controls.Add(this.textGain);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textLaserYOffset);
            this.groupBox2.Controls.Add(this.textLaserXOffset);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textPinYOffset);
            this.groupBox2.Controls.Add(this.textPinXOffset);
            this.groupBox2.Controls.Add(this.textYStep);
            this.groupBox2.Controls.Add(this.textXStep);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(419, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 191);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "参数设定";
            // 
            // textBox_DispCalbLightVal
            // 
            this.textBox_DispCalbLightVal.Location = new System.Drawing.Point(58, 163);
            this.textBox_DispCalbLightVal.Name = "textBox_DispCalbLightVal";
            this.textBox_DispCalbLightVal.Size = new System.Drawing.Size(81, 21);
            this.textBox_DispCalbLightVal.TabIndex = 18;
            // 
            // labelLightDispCalib
            // 
            this.labelLightDispCalib.AutoSize = true;
            this.labelLightDispCalib.Location = new System.Drawing.Point(6, 166);
            this.labelLightDispCalib.Name = "labelLightDispCalib";
            this.labelLightDispCalib.Size = new System.Drawing.Size(53, 12);
            this.labelLightDispCalib.TabIndex = 17;
            this.labelLightDispCalib.Text = "标定光源";
            // 
            // textExpsoure
            // 
            this.textExpsoure.Location = new System.Drawing.Point(211, 54);
            this.textExpsoure.Name = "textExpsoure";
            this.textExpsoure.Size = new System.Drawing.Size(77, 21);
            this.textExpsoure.TabIndex = 16;
            // 
            // textGain
            // 
            this.textGain.Location = new System.Drawing.Point(211, 21);
            this.textGain.Name = "textGain";
            this.textGain.Size = new System.Drawing.Size(74, 21);
            this.textGain.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(144, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "标定曝光";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(144, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "标定增益";
            // 
            // textLaserYOffset
            // 
            this.textLaserYOffset.Location = new System.Drawing.Point(211, 128);
            this.textLaserYOffset.Name = "textLaserYOffset";
            this.textLaserYOffset.Size = new System.Drawing.Size(77, 21);
            this.textLaserYOffset.TabIndex = 12;
            // 
            // textLaserXOffset
            // 
            this.textLaserXOffset.Location = new System.Drawing.Point(212, 91);
            this.textLaserXOffset.Name = "textLaserXOffset";
            this.textLaserXOffset.Size = new System.Drawing.Size(77, 21);
            this.textLaserXOffset.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(144, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "LaserY偏移";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(145, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "LaserX偏移";
            // 
            // textPinYOffset
            // 
            this.textPinYOffset.Location = new System.Drawing.Point(58, 125);
            this.textPinYOffset.Name = "textPinYOffset";
            this.textPinYOffset.Size = new System.Drawing.Size(81, 21);
            this.textPinYOffset.TabIndex = 7;
            // 
            // textPinXOffset
            // 
            this.textPinXOffset.Location = new System.Drawing.Point(58, 91);
            this.textPinXOffset.Name = "textPinXOffset";
            this.textPinXOffset.Size = new System.Drawing.Size(81, 21);
            this.textPinXOffset.TabIndex = 6;
            // 
            // textYStep
            // 
            this.textYStep.Location = new System.Drawing.Point(58, 54);
            this.textYStep.Name = "textYStep";
            this.textYStep.Size = new System.Drawing.Size(81, 21);
            this.textYStep.TabIndex = 5;
            // 
            // textXStep
            // 
            this.textXStep.Location = new System.Drawing.Point(58, 21);
            this.textXStep.Name = "textXStep";
            this.textXStep.Size = new System.Drawing.Size(81, 21);
            this.textXStep.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "PinY偏移";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "PinX偏移";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y步长";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "X步长";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_RecordPoint);
            this.groupBox1.Controls.Add(this.button_AllAxisMove);
            this.groupBox1.Controls.Add(this.button_SingleAxisMove);
            this.groupBox1.Controls.Add(this.dataGridView_PointInfo);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 191);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "点位设定";
            // 
            // button_RecordPoint
            // 
            this.button_RecordPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_RecordPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_RecordPoint.Location = new System.Drawing.Point(343, 12);
            this.button_RecordPoint.Name = "button_RecordPoint";
            this.button_RecordPoint.Size = new System.Drawing.Size(66, 40);
            this.button_RecordPoint.TabIndex = 21;
            this.button_RecordPoint.Text = "记录点位";
            this.button_RecordPoint.UseVisualStyleBackColor = false;
            this.button_RecordPoint.Click += new System.EventHandler(this.button_RecordPoint_Click);
            // 
            // button_AllAxisMove
            // 
            this.button_AllAxisMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_AllAxisMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_AllAxisMove.Location = new System.Drawing.Point(343, 125);
            this.button_AllAxisMove.Name = "button_AllAxisMove";
            this.button_AllAxisMove.Size = new System.Drawing.Size(66, 40);
            this.button_AllAxisMove.TabIndex = 19;
            this.button_AllAxisMove.Text = "联动";
            this.button_AllAxisMove.UseVisualStyleBackColor = false;
            this.button_AllAxisMove.Click += new System.EventHandler(this.button_AllAxisMove_Click);
            // 
            // button_SingleAxisMove
            // 
            this.button_SingleAxisMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_SingleAxisMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_SingleAxisMove.Location = new System.Drawing.Point(343, 66);
            this.button_SingleAxisMove.Name = "button_SingleAxisMove";
            this.button_SingleAxisMove.Size = new System.Drawing.Size(66, 40);
            this.button_SingleAxisMove.TabIndex = 18;
            this.button_SingleAxisMove.Text = "点动";
            this.button_SingleAxisMove.UseVisualStyleBackColor = false;
            this.button_SingleAxisMove.Click += new System.EventHandler(this.button_SingleAxisMove_Click);
            // 
            // dataGridView_PointInfo
            // 
            this.dataGridView_PointInfo.AllowUserToAddRows = false;
            this.dataGridView_PointInfo.AllowUserToDeleteRows = false;
            this.dataGridView_PointInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_PointInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Xpos,
            this.Ypos,
            this.ZPos});
            this.dataGridView_PointInfo.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView_PointInfo.Location = new System.Drawing.Point(6, 12);
            this.dataGridView_PointInfo.Name = "dataGridView_PointInfo";
            this.dataGridView_PointInfo.RowHeadersVisible = false;
            this.dataGridView_PointInfo.RowTemplate.Height = 20;
            this.dataGridView_PointInfo.Size = new System.Drawing.Size(331, 165);
            this.dataGridView_PointInfo.TabIndex = 10;
            // 
            // Column1
            // 
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "点位名称";
            this.Column1.MinimumWidth = 50;
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 150;
            // 
            // Xpos
            // 
            this.Xpos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Xpos.HeaderText = "X";
            this.Xpos.Name = "Xpos";
            this.Xpos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Xpos.Width = 60;
            // 
            // Ypos
            // 
            this.Ypos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Ypos.HeaderText = "Y";
            this.Ypos.Name = "Ypos";
            this.Ypos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Ypos.Width = 60;
            // 
            // ZPos
            // 
            this.ZPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ZPos.HeaderText = "Z";
            this.ZPos.Name = "ZPos";
            this.ZPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ZPos.Width = 60;
            // 
            // VisionDstSet
            // 
            this.VisionDstSet.Controls.Add(this.textBox_DispDstLightVal);
            this.VisionDstSet.Controls.Add(this.textDstGain);
            this.VisionDstSet.Controls.Add(this.label12);
            this.VisionDstSet.Controls.Add(this.groupBox3);
            this.VisionDstSet.Controls.Add(this.visionMatchSetCtr2);
            this.VisionDstSet.Controls.Add(this.labelLightDispDst);
            this.VisionDstSet.Controls.Add(this.label11);
            this.VisionDstSet.Controls.Add(this.textDstExpsoure);
            this.VisionDstSet.Location = new System.Drawing.Point(4, 22);
            this.VisionDstSet.Name = "VisionDstSet";
            this.VisionDstSet.Padding = new System.Windows.Forms.Padding(3);
            this.VisionDstSet.Size = new System.Drawing.Size(714, 516);
            this.VisionDstSet.TabIndex = 1;
            this.VisionDstSet.Text = "产品视觉设定";
            this.VisionDstSet.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_RecordPoint);
            this.groupBox3.Controls.Add(this.btn_SingleAxisMove);
            this.groupBox3.Controls.Add(this.dataGridView_DispPosList);
            this.groupBox3.Controls.Add(this.btn_AllAxisMove);
            this.groupBox3.Location = new System.Drawing.Point(7, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(314, 310);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "点位设定";
            // 
            // btn_RecordPoint
            // 
            this.btn_RecordPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_RecordPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_RecordPoint.Location = new System.Drawing.Point(6, 268);
            this.btn_RecordPoint.Name = "btn_RecordPoint";
            this.btn_RecordPoint.Size = new System.Drawing.Size(66, 40);
            this.btn_RecordPoint.TabIndex = 21;
            this.btn_RecordPoint.Text = "记录点位";
            this.btn_RecordPoint.UseVisualStyleBackColor = false;
            this.btn_RecordPoint.Click += new System.EventHandler(this.button_RecordPoint_Click);
            // 
            // btn_SingleAxisMove
            // 
            this.btn_SingleAxisMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_SingleAxisMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_SingleAxisMove.Location = new System.Drawing.Point(112, 268);
            this.btn_SingleAxisMove.Name = "btn_SingleAxisMove";
            this.btn_SingleAxisMove.Size = new System.Drawing.Size(66, 40);
            this.btn_SingleAxisMove.TabIndex = 18;
            this.btn_SingleAxisMove.Text = "点动";
            this.btn_SingleAxisMove.UseVisualStyleBackColor = false;
            this.btn_SingleAxisMove.Click += new System.EventHandler(this.button_SingleAxisMove_Click);
            // 
            // dataGridView_DispPosList
            // 
            this.dataGridView_DispPosList.AllowUserToAddRows = false;
            this.dataGridView_DispPosList.AllowUserToDeleteRows = false;
            this.dataGridView_DispPosList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DispPosList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dataGridView_DispPosList.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView_DispPosList.Location = new System.Drawing.Point(1, 14);
            this.dataGridView_DispPosList.Name = "dataGridView_DispPosList";
            this.dataGridView_DispPosList.RowHeadersVisible = false;
            this.dataGridView_DispPosList.RowTemplate.Height = 20;
            this.dataGridView_DispPosList.Size = new System.Drawing.Size(307, 249);
            this.dataGridView_DispPosList.TabIndex = 10;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "点位名称";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "机械X";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "机械Y";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.HeaderText = "机械Z";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // btn_AllAxisMove
            // 
            this.btn_AllAxisMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_AllAxisMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_AllAxisMove.Location = new System.Drawing.Point(233, 268);
            this.btn_AllAxisMove.Name = "btn_AllAxisMove";
            this.btn_AllAxisMove.Size = new System.Drawing.Size(66, 40);
            this.btn_AllAxisMove.TabIndex = 19;
            this.btn_AllAxisMove.Text = "联动";
            this.btn_AllAxisMove.UseVisualStyleBackColor = false;
            this.btn_AllAxisMove.Click += new System.EventHandler(this.button_AllAxisMove_Click);
            // 
            // visionMatchSetCtr2
            // 
            this.visionMatchSetCtr2.Location = new System.Drawing.Point(0, 3);
            this.visionMatchSetCtr2.Name = "visionMatchSetCtr2";
            this.visionMatchSetCtr2.Size = new System.Drawing.Size(710, 191);
            this.visionMatchSetCtr2.strPath = "";
            this.visionMatchSetCtr2.TabIndex = 0;
            // 
            // textBox_DispDstLightVal
            // 
            this.textBox_DispDstLightVal.Location = new System.Drawing.Point(597, 285);
            this.textBox_DispDstLightVal.Name = "textBox_DispDstLightVal";
            this.textBox_DispDstLightVal.Size = new System.Drawing.Size(93, 21);
            this.textBox_DispDstLightVal.TabIndex = 28;
            // 
            // labelLightDispDst
            // 
            this.labelLightDispDst.AutoSize = true;
            this.labelLightDispDst.Location = new System.Drawing.Point(538, 285);
            this.labelLightDispDst.Name = "labelLightDispDst";
            this.labelLightDispDst.Size = new System.Drawing.Size(53, 12);
            this.labelLightDispDst.TabIndex = 27;
            this.labelLightDispDst.Text = "点胶光源";
            this.labelLightDispDst.DoubleClick += new System.EventHandler(this.labelLightDispDst_Click);
            // 
            // textDstExpsoure
            // 
            this.textDstExpsoure.Location = new System.Drawing.Point(597, 214);
            this.textDstExpsoure.Name = "textDstExpsoure";
            this.textDstExpsoure.Size = new System.Drawing.Size(93, 21);
            this.textDstExpsoure.TabIndex = 20;
            // 
            // textDstGain
            // 
            this.textDstGain.Location = new System.Drawing.Point(597, 249);
            this.textDstGain.Name = "textDstGain";
            this.textDstGain.Size = new System.Drawing.Size(93, 21);
            this.textDstGain.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(538, 217);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 18;
            this.label11.Text = "点胶曝光";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(538, 252);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 17;
            this.label12.Text = "点胶增益";
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.DarkOrchid;
            this.BtnSave.Location = new System.Drawing.Point(988, 541);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(107, 45);
            this.BtnSave.TabIndex = 8;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnPrTest
            // 
            this.BtnPrTest.Location = new System.Drawing.Point(8, 527);
            this.BtnPrTest.Name = "BtnPrTest";
            this.BtnPrTest.Size = new System.Drawing.Size(82, 44);
            this.BtnPrTest.TabIndex = 4;
            this.BtnPrTest.Text = "匹配测试";
            this.BtnPrTest.UseVisualStyleBackColor = true;
            this.BtnPrTest.Click += new System.EventHandler(this.button2_Click);
            // 
            // BtnRoiplus
            // 
            this.BtnRoiplus.Location = new System.Drawing.Point(8, 472);
            this.BtnRoiplus.Name = "BtnRoiplus";
            this.BtnRoiplus.Size = new System.Drawing.Size(82, 44);
            this.BtnRoiplus.TabIndex = 2;
            this.BtnRoiplus.Text = "任意Roi+";
            this.BtnRoiplus.UseVisualStyleBackColor = true;
            this.BtnRoiplus.Click += new System.EventHandler(this.BtnRoiplus_Click);
            // 
            // RoiSub
            // 
            this.RoiSub.Location = new System.Drawing.Point(99, 472);
            this.RoiSub.Name = "RoiSub";
            this.RoiSub.Size = new System.Drawing.Size(82, 44);
            this.RoiSub.TabIndex = 3;
            this.RoiSub.Text = "任意Roi-";
            this.RoiSub.UseVisualStyleBackColor = true;
            this.RoiSub.Click += new System.EventHandler(this.RoiSub_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(8, 330);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(82, 47);
            this.btnRead.TabIndex = 4;
            this.btnRead.Text = "读取图片";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // BtnSanp
            // 
            this.BtnSanp.Location = new System.Drawing.Point(96, 331);
            this.BtnSanp.Name = "BtnSanp";
            this.BtnSanp.Size = new System.Drawing.Size(82, 47);
            this.BtnSanp.TabIndex = 5;
            this.BtnSanp.Text = "采集图片";
            this.BtnSanp.UseVisualStyleBackColor = true;
            this.BtnSanp.Click += new System.EventHandler(this.BtnSanp_Click);
            // 
            // btnSnapSave
            // 
            this.btnSnapSave.Location = new System.Drawing.Point(184, 331);
            this.btnSnapSave.Name = "btnSnapSave";
            this.btnSnapSave.Size = new System.Drawing.Size(82, 47);
            this.btnSnapSave.TabIndex = 10;
            this.btnSnapSave.Text = "采集保存";
            this.btnSnapSave.UseVisualStyleBackColor = true;
            this.btnSnapSave.Click += new System.EventHandler(this.btnSnapSave_Click);
            // 
            // ContinuousSnap
            // 
            this.ContinuousSnap.Location = new System.Drawing.Point(280, 331);
            this.ContinuousSnap.Name = "ContinuousSnap";
            this.ContinuousSnap.Size = new System.Drawing.Size(82, 47);
            this.ContinuousSnap.TabIndex = 11;
            this.ContinuousSnap.Text = "连续采集";
            this.ContinuousSnap.UseVisualStyleBackColor = true;
            this.ContinuousSnap.Click += new System.EventHandler(this.ContinuousSnap_Click);
            // 
            // textExpsoureTimeVal
            // 
            this.textExpsoureTimeVal.Location = new System.Drawing.Point(78, 429);
            this.textExpsoureTimeVal.Name = "textExpsoureTimeVal";
            this.textExpsoureTimeVal.Size = new System.Drawing.Size(100, 21);
            this.textExpsoureTimeVal.TabIndex = 20;
            // 
            // textGainVal
            // 
            this.textGainVal.Location = new System.Drawing.Point(78, 396);
            this.textGainVal.Name = "textGainVal";
            this.textGainVal.Size = new System.Drawing.Size(100, 21);
            this.textGainVal.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 431);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 18;
            this.label9.Text = "曝光";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 396);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "增益";
            // 
            // rightTab1
            // 
            this.rightTab1.Controls.Add(this.VisionCalibSet);
            this.rightTab1.Controls.Add(this.VisionDstSet);
            this.rightTab1.Controls.Add(this.DispTrace);
            this.rightTab1.Location = new System.Drawing.Point(383, 0);
            this.rightTab1.Name = "rightTab1";
            this.rightTab1.SelectedIndex = 0;
            this.rightTab1.Size = new System.Drawing.Size(722, 542);
            this.rightTab1.TabIndex = 11;
            // 
            // VisionCalibSet
            // 
            this.VisionCalibSet.Controls.Add(this.btnPinHeight);
            this.VisionCalibSet.Controls.Add(this.groupBox1);
            this.VisionCalibSet.Controls.Add(this.BtnLaserCalib);
            this.VisionCalibSet.Controls.Add(this.groupBox2);
            this.VisionCalibSet.Controls.Add(this.visionMatchSetCtr1);
            this.VisionCalibSet.Controls.Add(this.BtnLaserTest);
            this.VisionCalibSet.Controls.Add(this.lbldispensepressure);
            this.VisionCalibSet.Controls.Add(this.BtnPinTest);
            this.VisionCalibSet.Controls.Add(this.txtZLatchData);
            this.VisionCalibSet.Controls.Add(this.BtnCalibMotion);
            this.VisionCalibSet.Controls.Add(this.label13);
            this.VisionCalibSet.Controls.Add(this.btnLaserReadData);
            this.VisionCalibSet.Controls.Add(this.txtLaserReadData);
            this.VisionCalibSet.Location = new System.Drawing.Point(4, 22);
            this.VisionCalibSet.Name = "VisionCalibSet";
            this.VisionCalibSet.Padding = new System.Windows.Forms.Padding(3);
            this.VisionCalibSet.Size = new System.Drawing.Size(714, 516);
            this.VisionCalibSet.TabIndex = 0;
            this.VisionCalibSet.Text = "视觉标定";
            this.VisionCalibSet.UseVisualStyleBackColor = true;
            // 
            // visionMatchSetCtr1
            // 
            this.visionMatchSetCtr1.Location = new System.Drawing.Point(3, 203);
            this.visionMatchSetCtr1.Name = "visionMatchSetCtr1";
            this.visionMatchSetCtr1.Size = new System.Drawing.Size(713, 191);
            this.visionMatchSetCtr1.strPath = "";
            this.visionMatchSetCtr1.TabIndex = 1;
            // 
            // lbldispensepressure
            // 
            this.lbldispensepressure.AutoSize = true;
            this.lbldispensepressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbldispensepressure.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldispensepressure.Location = new System.Drawing.Point(250, 461);
            this.lbldispensepressure.Name = "lbldispensepressure";
            this.lbldispensepressure.Size = new System.Drawing.Size(133, 14);
            this.lbldispensepressure.TabIndex = 26;
            this.lbldispensepressure.Text = "针头压力传感器状态";
            // 
            // txtZLatchData
            // 
            this.txtZLatchData.Location = new System.Drawing.Point(153, 458);
            this.txtZLatchData.Name = "txtZLatchData";
            this.txtZLatchData.Size = new System.Drawing.Size(80, 21);
            this.txtZLatchData.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label13.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(3, 464);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(126, 14);
            this.label13.TabIndex = 24;
            this.label13.Text = "针头触碰锁存高度:";
            // 
            // btnLaserReadData
            // 
            this.btnLaserReadData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnLaserReadData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLaserReadData.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLaserReadData.Location = new System.Drawing.Point(492, 445);
            this.btnLaserReadData.Name = "btnLaserReadData";
            this.btnLaserReadData.Size = new System.Drawing.Size(100, 43);
            this.btnLaserReadData.TabIndex = 22;
            this.btnLaserReadData.Text = "激光测高读数";
            this.btnLaserReadData.UseVisualStyleBackColor = false;
            // 
            // txtLaserReadData
            // 
            this.txtLaserReadData.Location = new System.Drawing.Point(598, 454);
            this.txtLaserReadData.Name = "txtLaserReadData";
            this.txtLaserReadData.Size = new System.Drawing.Size(100, 21);
            this.txtLaserReadData.TabIndex = 23;
            // 
            // DispTrace
            // 
            this.DispTrace.Controls.Add(this.BtnDel);
            this.DispTrace.Controls.Add(this.button1);
            this.DispTrace.Controls.Add(this.BtnDelay);
            this.DispTrace.Controls.Add(this.BtnIO);
            this.DispTrace.Controls.Add(this.BtnAddArc);
            this.DispTrace.Controls.Add(this.btnAddLine);
            this.DispTrace.Controls.Add(this.BtnAddPoint);
            this.DispTrace.Controls.Add(this.dataGridViewDispTrace);
            this.DispTrace.Location = new System.Drawing.Point(4, 22);
            this.DispTrace.Name = "DispTrace";
            this.DispTrace.Size = new System.Drawing.Size(714, 516);
            this.DispTrace.TabIndex = 2;
            this.DispTrace.Text = "点胶轨迹";
            this.DispTrace.UseVisualStyleBackColor = true;
            this.DispTrace.Click += new System.EventHandler(this.DispTrace_Click);
            // 
            // BtnDel
            // 
            this.BtnDel.Location = new System.Drawing.Point(111, 290);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Size = new System.Drawing.Size(83, 29);
            this.BtnDel.TabIndex = 7;
            this.BtnDel.Text = "删除";
            this.BtnDel.UseVisualStyleBackColor = true;
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(274, 326);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 29);
            this.button1.TabIndex = 6;
            this.button1.Text = "读取路径";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnDelay
            // 
            this.BtnDelay.Location = new System.Drawing.Point(7, 478);
            this.BtnDelay.Name = "BtnDelay";
            this.BtnDelay.Size = new System.Drawing.Size(83, 29);
            this.BtnDelay.TabIndex = 5;
            this.BtnDelay.Text = "添加延时";
            this.BtnDelay.UseVisualStyleBackColor = true;
            this.BtnDelay.Click += new System.EventHandler(this.BtnDelay_Click);
            // 
            // BtnIO
            // 
            this.BtnIO.Location = new System.Drawing.Point(7, 431);
            this.BtnIO.Name = "BtnIO";
            this.BtnIO.Size = new System.Drawing.Size(83, 29);
            this.BtnIO.TabIndex = 4;
            this.BtnIO.Text = "添加IO";
            this.BtnIO.UseVisualStyleBackColor = true;
            this.BtnIO.Click += new System.EventHandler(this.BtnIO_Click);
            // 
            // BtnAddArc
            // 
            this.BtnAddArc.Location = new System.Drawing.Point(7, 384);
            this.BtnAddArc.Name = "BtnAddArc";
            this.BtnAddArc.Size = new System.Drawing.Size(83, 29);
            this.BtnAddArc.TabIndex = 3;
            this.BtnAddArc.Text = "添加圆弧";
            this.BtnAddArc.UseVisualStyleBackColor = true;
            this.BtnAddArc.Click += new System.EventHandler(this.BtnAddArc_Click);
            // 
            // btnAddLine
            // 
            this.btnAddLine.Location = new System.Drawing.Point(7, 337);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(83, 29);
            this.btnAddLine.TabIndex = 2;
            this.btnAddLine.Text = "添加直线";
            this.btnAddLine.UseVisualStyleBackColor = true;
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            // 
            // BtnAddPoint
            // 
            this.BtnAddPoint.Location = new System.Drawing.Point(7, 290);
            this.BtnAddPoint.Name = "BtnAddPoint";
            this.BtnAddPoint.Size = new System.Drawing.Size(83, 29);
            this.BtnAddPoint.TabIndex = 1;
            this.BtnAddPoint.Text = "添加点";
            this.BtnAddPoint.UseVisualStyleBackColor = true;
            this.BtnAddPoint.Click += new System.EventHandler(this.BtnAddPoint_Click);
            // 
            // dataGridViewDispTrace
            // 
            this.dataGridViewDispTrace.AllowUserToAddRows = false;
            this.dataGridViewDispTrace.AllowUserToDeleteRows = false;
            this.dataGridViewDispTrace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDispTrace.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DispName,
            this.Type,
            this.set});
            this.dataGridViewDispTrace.Location = new System.Drawing.Point(2, 3);
            this.dataGridViewDispTrace.Name = "dataGridViewDispTrace";
            this.dataGridViewDispTrace.RowHeadersVisible = false;
            this.dataGridViewDispTrace.RowTemplate.Height = 23;
            this.dataGridViewDispTrace.Size = new System.Drawing.Size(705, 268);
            this.dataGridViewDispTrace.TabIndex = 0;
            this.dataGridViewDispTrace.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDispTrace_CellContentClick);
            // 
            // DispName
            // 
            this.DispName.HeaderText = "名称";
            this.DispName.Name = "DispName";
            // 
            // Type
            // 
            this.Type.HeaderText = "类型";
            this.Type.Items.AddRange(new object[] {
            "点",
            "线段",
            "圆弧"});
            this.Type.Name = "Type";
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // set
            // 
            this.set.HeaderText = "设置";
            this.set.Name = "set";
            // 
            // btnDispenseOutManual
            // 
            this.btnDispenseOutManual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnDispenseOutManual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDispenseOutManual.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDispenseOutManual.Location = new System.Drawing.Point(797, 547);
            this.btnDispenseOutManual.Name = "btnDispenseOutManual";
            this.btnDispenseOutManual.Size = new System.Drawing.Size(100, 40);
            this.btnDispenseOutManual.TabIndex = 27;
            this.btnDispenseOutManual.Text = "手动按时吐胶";
            this.btnDispenseOutManual.UseVisualStyleBackColor = false;
            this.btnDispenseOutManual.Click += new System.EventHandler(this.btnDispenseOutManual_Click);
            // 
            // txtManualDispenseOutTime
            // 
            this.txtManualDispenseOutTime.Location = new System.Drawing.Point(903, 560);
            this.txtManualDispenseOutTime.Name = "txtManualDispenseOutTime";
            this.txtManualDispenseOutTime.Size = new System.Drawing.Size(52, 21);
            this.txtManualDispenseOutTime.TabIndex = 28;
            this.txtManualDispenseOutTime.Text = "100";
            this.txtManualDispenseOutTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(961, 565);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 29;
            this.label14.Text = "ms";
            // 
            // btnLightONOFF
            // 
            this.btnLightONOFF.Location = new System.Drawing.Point(99, 526);
            this.btnLightONOFF.Name = "btnLightONOFF";
            this.btnLightONOFF.Size = new System.Drawing.Size(82, 44);
            this.btnLightONOFF.TabIndex = 30;
            this.btnLightONOFF.Text = "光源开关";
            this.btnLightONOFF.UseVisualStyleBackColor = true;
            this.btnLightONOFF.Click += new System.EventHandler(this.btnLightONOFF_Click);
            // 
            // button_Unegtive
            // 
            this.button_Unegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Unegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Unegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Unegtive.Location = new System.Drawing.Point(187, 539);
            this.button_Unegtive.Name = "button_Unegtive";
            this.button_Unegtive.Size = new System.Drawing.Size(53, 44);
            this.button_Unegtive.TabIndex = 41;
            this.button_Unegtive.Text = "U-";
            this.button_Unegtive.UseVisualStyleBackColor = true;
            this.button_Unegtive.Click += new System.EventHandler(this.button_Unegtive_Click);
            this.button_Unegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Unegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Ynegtive
            // 
            this.button_Ynegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Ynegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Ynegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Ynegtive.Location = new System.Drawing.Point(244, 539);
            this.button_Ynegtive.Name = "button_Ynegtive";
            this.button_Ynegtive.Size = new System.Drawing.Size(53, 44);
            this.button_Ynegtive.TabIndex = 40;
            this.button_Ynegtive.Text = "Y-";
            this.button_Ynegtive.UseVisualStyleBackColor = true;
            this.button_Ynegtive.Click += new System.EventHandler(this.button_Ynegtive_Click);
            this.button_Ynegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Ynegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Znegtive
            // 
            this.button_Znegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Znegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Znegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Znegtive.Location = new System.Drawing.Point(305, 539);
            this.button_Znegtive.Name = "button_Znegtive";
            this.button_Znegtive.Size = new System.Drawing.Size(53, 44);
            this.button_Znegtive.TabIndex = 39;
            this.button_Znegtive.Text = "Z-";
            this.button_Znegtive.UseVisualStyleBackColor = true;
            this.button_Znegtive.Click += new System.EventHandler(this.button_Znegtive_Click);
            this.button_Znegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Znegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Zpositive
            // 
            this.button_Zpositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Zpositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Zpositive.ForeColor = System.Drawing.Color.Red;
            this.button_Zpositive.Location = new System.Drawing.Point(305, 433);
            this.button_Zpositive.Name = "button_Zpositive";
            this.button_Zpositive.Size = new System.Drawing.Size(53, 44);
            this.button_Zpositive.TabIndex = 38;
            this.button_Zpositive.Text = "Z+";
            this.button_Zpositive.UseVisualStyleBackColor = true;
            this.button_Zpositive.Click += new System.EventHandler(this.button_Zpositive_Click);
            this.button_Zpositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Zpositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Xnegtive
            // 
            this.button_Xnegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Xnegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Xnegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Xnegtive.Location = new System.Drawing.Point(187, 486);
            this.button_Xnegtive.Name = "button_Xnegtive";
            this.button_Xnegtive.Size = new System.Drawing.Size(53, 44);
            this.button_Xnegtive.TabIndex = 37;
            this.button_Xnegtive.Text = "X-";
            this.button_Xnegtive.UseVisualStyleBackColor = true;
            this.button_Xnegtive.Click += new System.EventHandler(this.button_Xnegtive_Click);
            this.button_Xnegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Xnegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Xpositive
            // 
            this.button_Xpositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Xpositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Xpositive.ForeColor = System.Drawing.Color.Red;
            this.button_Xpositive.Location = new System.Drawing.Point(305, 486);
            this.button_Xpositive.Name = "button_Xpositive";
            this.button_Xpositive.Size = new System.Drawing.Size(53, 44);
            this.button_Xpositive.TabIndex = 36;
            this.button_Xpositive.Text = "X+";
            this.button_Xpositive.UseVisualStyleBackColor = true;
            this.button_Xpositive.Click += new System.EventHandler(this.button_Xpositive_Click);
            this.button_Xpositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Xpositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_stop
            // 
            this.button_stop.BackColor = System.Drawing.Color.Fuchsia;
            this.button_stop.Location = new System.Drawing.Point(244, 486);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(53, 44);
            this.button_stop.TabIndex = 35;
            this.button_stop.Text = "stop";
            this.button_stop.UseVisualStyleBackColor = false;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_Upositive
            // 
            this.button_Upositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Upositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Upositive.ForeColor = System.Drawing.Color.Red;
            this.button_Upositive.Location = new System.Drawing.Point(187, 433);
            this.button_Upositive.Name = "button_Upositive";
            this.button_Upositive.Size = new System.Drawing.Size(53, 44);
            this.button_Upositive.TabIndex = 34;
            this.button_Upositive.Text = "U+";
            this.button_Upositive.UseVisualStyleBackColor = true;
            this.button_Upositive.Click += new System.EventHandler(this.button_Upositive_Click);
            this.button_Upositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Upositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Ypositive
            // 
            this.button_Ypositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Ypositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Ypositive.ForeColor = System.Drawing.Color.Red;
            this.button_Ypositive.Location = new System.Drawing.Point(244, 433);
            this.button_Ypositive.Name = "button_Ypositive";
            this.button_Ypositive.Size = new System.Drawing.Size(53, 44);
            this.button_Ypositive.TabIndex = 33;
            this.button_Ypositive.Text = "Y+";
            this.button_Ypositive.UseVisualStyleBackColor = true;
            this.button_Ypositive.Click += new System.EventHandler(this.button_Ypositive_Click);
            this.button_Ypositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Ypositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // comboBox_SelMotionType
            // 
            this.comboBox_SelMotionType.FormattingEnabled = true;
            this.comboBox_SelMotionType.Items.AddRange(new object[] {
            "Jog",
            "0.01",
            "0.05",
            "0.1",
            "0.5",
            "1",
            "5",
            "10",
            "50",
            "100"});
            this.comboBox_SelMotionType.Location = new System.Drawing.Point(191, 396);
            this.comboBox_SelMotionType.Name = "comboBox_SelMotionType";
            this.comboBox_SelMotionType.Size = new System.Drawing.Size(167, 20);
            this.comboBox_SelMotionType.TabIndex = 42;
            // 
            // button_ServoOnZ
            // 
            this.button_ServoOnZ.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnZ.Location = new System.Drawing.Point(20, 115);
            this.button_ServoOnZ.Name = "button_ServoOnZ";
            this.button_ServoOnZ.Size = new System.Drawing.Size(55, 30);
            this.button_ServoOnZ.TabIndex = 91;
            this.button_ServoOnZ.Text = "伺服On";
            this.button_ServoOnZ.UseVisualStyleBackColor = false;
            this.button_ServoOnZ.Click += new System.EventHandler(this.button_ServoOnZ_Click);
            // 
            // button_ServoOnY
            // 
            this.button_ServoOnY.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnY.Location = new System.Drawing.Point(20, 78);
            this.button_ServoOnY.Name = "button_ServoOnY";
            this.button_ServoOnY.Size = new System.Drawing.Size(55, 30);
            this.button_ServoOnY.TabIndex = 90;
            this.button_ServoOnY.Text = "伺服On";
            this.button_ServoOnY.UseVisualStyleBackColor = false;
            this.button_ServoOnY.Click += new System.EventHandler(this.button_ServoOnY_Click);
            // 
            // button_ServoOnX
            // 
            this.button_ServoOnX.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnX.Location = new System.Drawing.Point(20, 35);
            this.button_ServoOnX.Name = "button_ServoOnX";
            this.button_ServoOnX.Size = new System.Drawing.Size(55, 30);
            this.button_ServoOnX.TabIndex = 89;
            this.button_ServoOnX.Text = "伺服On";
            this.button_ServoOnX.UseVisualStyleBackColor = false;
            this.button_ServoOnX.Click += new System.EventHandler(this.button_ServoOnX_Click);
            // 
            // label_CmdPosU
            // 
            this.label_CmdPosU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosU.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosU.Location = new System.Drawing.Point(227, 163);
            this.label_CmdPosU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosU.Name = "label_CmdPosU";
            this.label_CmdPosU.Size = new System.Drawing.Size(62, 16);
            this.label_CmdPosU.TabIndex = 84;
            this.label_CmdPosU.Text = "00000000";
            // 
            // label_ActPosU
            // 
            this.label_ActPosU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosU.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosU.Location = new System.Drawing.Point(142, 160);
            this.label_ActPosU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosU.Name = "label_ActPosU";
            this.label_ActPosU.Size = new System.Drawing.Size(62, 16);
            this.label_ActPosU.TabIndex = 83;
            this.label_ActPosU.Text = "00000000";
            // 
            // label_CmdPosZ
            // 
            this.label_CmdPosZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosZ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosZ.Location = new System.Drawing.Point(227, 120);
            this.label_CmdPosZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosZ.Name = "label_CmdPosZ";
            this.label_CmdPosZ.Size = new System.Drawing.Size(62, 16);
            this.label_CmdPosZ.TabIndex = 82;
            this.label_CmdPosZ.Text = "00000000";
            // 
            // label_ActPosZ
            // 
            this.label_ActPosZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosZ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosZ.Location = new System.Drawing.Point(142, 119);
            this.label_ActPosZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosZ.Name = "label_ActPosZ";
            this.label_ActPosZ.Size = new System.Drawing.Size(62, 16);
            this.label_ActPosZ.TabIndex = 81;
            this.label_ActPosZ.Text = "00000000";
            // 
            // label_CmdPosY
            // 
            this.label_CmdPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosY.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosY.Location = new System.Drawing.Point(227, 84);
            this.label_CmdPosY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosY.Name = "label_CmdPosY";
            this.label_CmdPosY.Size = new System.Drawing.Size(62, 16);
            this.label_CmdPosY.TabIndex = 80;
            this.label_CmdPosY.Text = "00000000";
            // 
            // label_ActPosY
            // 
            this.label_ActPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosY.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosY.Location = new System.Drawing.Point(142, 84);
            this.label_ActPosY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosY.Name = "label_ActPosY";
            this.label_ActPosY.Size = new System.Drawing.Size(62, 16);
            this.label_ActPosY.TabIndex = 79;
            this.label_ActPosY.Text = "00000000";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(218, 17);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(72, 16);
            this.label16.TabIndex = 78;
            this.label16.Text = "命令位置";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(138, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 16);
            this.label15.TabIndex = 77;
            this.label15.Text = "实际位置";
            // 
            // label_CmdPosX
            // 
            this.label_CmdPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosX.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosX.Location = new System.Drawing.Point(227, 43);
            this.label_CmdPosX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosX.Name = "label_CmdPosX";
            this.label_CmdPosX.Size = new System.Drawing.Size(62, 16);
            this.label_CmdPosX.TabIndex = 76;
            this.label_CmdPosX.Text = "00000000";
            // 
            // label_ActPosX
            // 
            this.label_ActPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosX.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosX.Location = new System.Drawing.Point(142, 44);
            this.label_ActPosX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosX.Name = "label_ActPosX";
            this.label_ActPosX.Size = new System.Drawing.Size(62, 16);
            this.label_ActPosX.TabIndex = 75;
            this.label_ActPosX.Text = "00000000";
            // 
            // button_ServoOnU
            // 
            this.button_ServoOnU.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnU.Location = new System.Drawing.Point(20, 153);
            this.button_ServoOnU.Name = "button_ServoOnU";
            this.button_ServoOnU.Size = new System.Drawing.Size(55, 30);
            this.button_ServoOnU.TabIndex = 92;
            this.button_ServoOnU.Text = "伺服On";
            this.button_ServoOnU.UseVisualStyleBackColor = false;
            this.button_ServoOnU.Click += new System.EventHandler(this.button_ServoOnU_Click);
            // 
            // button_homeU
            // 
            this.button_homeU.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeU.Location = new System.Drawing.Point(82, 150);
            this.button_homeU.Name = "button_homeU";
            this.button_homeU.Size = new System.Drawing.Size(55, 30);
            this.button_homeU.TabIndex = 47;
            this.button_homeU.Text = "回原点";
            this.button_homeU.UseVisualStyleBackColor = false;
            this.button_homeU.Click += new System.EventHandler(this.button_homeU_Click);
            // 
            // button_homeZ
            // 
            this.button_homeZ.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeZ.Location = new System.Drawing.Point(82, 115);
            this.button_homeZ.Name = "button_homeZ";
            this.button_homeZ.Size = new System.Drawing.Size(55, 30);
            this.button_homeZ.TabIndex = 46;
            this.button_homeZ.Text = "回原点";
            this.button_homeZ.UseVisualStyleBackColor = false;
            this.button_homeZ.Click += new System.EventHandler(this.button_homeZ_Click);
            // 
            // button_homeY
            // 
            this.button_homeY.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeY.Location = new System.Drawing.Point(82, 75);
            this.button_homeY.Name = "button_homeY";
            this.button_homeY.Size = new System.Drawing.Size(55, 30);
            this.button_homeY.TabIndex = 45;
            this.button_homeY.Text = "回原点";
            this.button_homeY.UseVisualStyleBackColor = false;
            this.button_homeY.Click += new System.EventHandler(this.button_homeY_Click);
            // 
            // button_homeX
            // 
            this.button_homeX.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeX.Location = new System.Drawing.Point(82, 35);
            this.button_homeX.Name = "button_homeX";
            this.button_homeX.Size = new System.Drawing.Size(55, 30);
            this.button_homeX.TabIndex = 44;
            this.button_homeX.Text = "回原点";
            this.button_homeX.UseVisualStyleBackColor = false;
            this.button_homeX.Click += new System.EventHandler(this.button_homeX_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(3, 163);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(11, 12);
            this.label21.TabIndex = 96;
            this.label21.Text = "U";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(3, 124);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(11, 12);
            this.label22.TabIndex = 95;
            this.label22.Text = "Z";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(3, 84);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(11, 12);
            this.label23.TabIndex = 94;
            this.label23.Text = "Y";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(3, 44);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(11, 12);
            this.label24.TabIndex = 93;
            this.label24.Text = "X";
            // 
            // labelControl_EMGU
            // 
            this.labelControl_EMGU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGU.Location = new System.Drawing.Point(482, 157);
            this.labelControl_EMGU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGU.Name = "labelControl_EMGU";
            this.labelControl_EMGU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGU.TabIndex = 121;
            this.labelControl_EMGU.Text = "OFF";
            // 
            // labelControl_ORIU
            // 
            this.labelControl_ORIU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORIU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORIU.Location = new System.Drawing.Point(391, 157);
            this.labelControl_ORIU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORIU.Name = "labelControl_ORIU";
            this.labelControl_ORIU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORIU.TabIndex = 120;
            this.labelControl_ORIU.Text = "OFF";
            // 
            // labelControl_LimtNU
            // 
            this.labelControl_LimtNU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNU.Location = new System.Drawing.Point(434, 157);
            this.labelControl_LimtNU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNU.Name = "labelControl_LimtNU";
            this.labelControl_LimtNU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNU.TabIndex = 119;
            this.labelControl_LimtNU.Text = "OFF";
            // 
            // labelControl_LimtPU
            // 
            this.labelControl_LimtPU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPU.Location = new System.Drawing.Point(345, 157);
            this.labelControl_LimtPU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPU.Name = "labelControl_LimtPU";
            this.labelControl_LimtPU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPU.TabIndex = 118;
            this.labelControl_LimtPU.Text = "OFF";
            // 
            // labelControl_AlarmU
            // 
            this.labelControl_AlarmU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmU.Location = new System.Drawing.Point(304, 157);
            this.labelControl_AlarmU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmU.Name = "labelControl_AlarmU";
            this.labelControl_AlarmU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmU.TabIndex = 117;
            this.labelControl_AlarmU.Text = "OFF";
            // 
            // labelControl_EMGZ
            // 
            this.labelControl_EMGZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGZ.Location = new System.Drawing.Point(482, 119);
            this.labelControl_EMGZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGZ.Name = "labelControl_EMGZ";
            this.labelControl_EMGZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGZ.TabIndex = 116;
            this.labelControl_EMGZ.Text = "OFF";
            // 
            // labelControl_ORIZ
            // 
            this.labelControl_ORIZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORIZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORIZ.Location = new System.Drawing.Point(391, 119);
            this.labelControl_ORIZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORIZ.Name = "labelControl_ORIZ";
            this.labelControl_ORIZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORIZ.TabIndex = 115;
            this.labelControl_ORIZ.Text = "OFF";
            // 
            // labelControl_LimtNZ
            // 
            this.labelControl_LimtNZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNZ.Location = new System.Drawing.Point(434, 119);
            this.labelControl_LimtNZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNZ.Name = "labelControl_LimtNZ";
            this.labelControl_LimtNZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNZ.TabIndex = 114;
            this.labelControl_LimtNZ.Text = "OFF";
            // 
            // labelControl_LimtPZ
            // 
            this.labelControl_LimtPZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPZ.Location = new System.Drawing.Point(345, 119);
            this.labelControl_LimtPZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPZ.Name = "labelControl_LimtPZ";
            this.labelControl_LimtPZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPZ.TabIndex = 113;
            this.labelControl_LimtPZ.Text = "OFF";
            // 
            // labelControl_AlarmZ
            // 
            this.labelControl_AlarmZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmZ.Location = new System.Drawing.Point(304, 119);
            this.labelControl_AlarmZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmZ.Name = "labelControl_AlarmZ";
            this.labelControl_AlarmZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmZ.TabIndex = 112;
            this.labelControl_AlarmZ.Text = "OFF";
            // 
            // labelControl_EMGY
            // 
            this.labelControl_EMGY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGY.Location = new System.Drawing.Point(482, 81);
            this.labelControl_EMGY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGY.Name = "labelControl_EMGY";
            this.labelControl_EMGY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGY.TabIndex = 111;
            this.labelControl_EMGY.Text = "OFF";
            // 
            // labelControl_ORIY
            // 
            this.labelControl_ORIY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORIY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORIY.Location = new System.Drawing.Point(391, 81);
            this.labelControl_ORIY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORIY.Name = "labelControl_ORIY";
            this.labelControl_ORIY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORIY.TabIndex = 110;
            this.labelControl_ORIY.Text = "OFF";
            // 
            // labelControl_LimtNY
            // 
            this.labelControl_LimtNY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNY.Location = new System.Drawing.Point(434, 81);
            this.labelControl_LimtNY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNY.Name = "labelControl_LimtNY";
            this.labelControl_LimtNY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNY.TabIndex = 109;
            this.labelControl_LimtNY.Text = "OFF";
            // 
            // labelControl_LimtPY
            // 
            this.labelControl_LimtPY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPY.Location = new System.Drawing.Point(345, 81);
            this.labelControl_LimtPY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPY.Name = "labelControl_LimtPY";
            this.labelControl_LimtPY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPY.TabIndex = 108;
            this.labelControl_LimtPY.Text = "OFF";
            // 
            // labelControl_AlarmY
            // 
            this.labelControl_AlarmY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmY.Location = new System.Drawing.Point(304, 81);
            this.labelControl_AlarmY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmY.Name = "labelControl_AlarmY";
            this.labelControl_AlarmY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmY.TabIndex = 107;
            this.labelControl_AlarmY.Text = "OFF";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label25.Location = new System.Drawing.Point(477, 17);
            this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(40, 16);
            this.label25.TabIndex = 106;
            this.label25.Text = "急停";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label26.Location = new System.Drawing.Point(386, 17);
            this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(40, 16);
            this.label26.TabIndex = 105;
            this.label26.Text = "原点";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.Location = new System.Drawing.Point(421, 17);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(56, 16);
            this.label27.TabIndex = 104;
            this.label27.Text = "负极限";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label28.Location = new System.Drawing.Point(332, 17);
            this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(56, 16);
            this.label28.TabIndex = 103;
            this.label28.Text = "正极限";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label29.Location = new System.Drawing.Point(295, 17);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(40, 16);
            this.label29.TabIndex = 102;
            this.label29.Text = "报警";
            // 
            // labelControl_EMGX
            // 
            this.labelControl_EMGX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGX.Location = new System.Drawing.Point(482, 43);
            this.labelControl_EMGX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGX.Name = "labelControl_EMGX";
            this.labelControl_EMGX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGX.TabIndex = 101;
            this.labelControl_EMGX.Text = "OFF";
            // 
            // labelControl_ORIX
            // 
            this.labelControl_ORIX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORIX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORIX.Location = new System.Drawing.Point(391, 43);
            this.labelControl_ORIX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORIX.Name = "labelControl_ORIX";
            this.labelControl_ORIX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORIX.TabIndex = 100;
            this.labelControl_ORIX.Text = "OFF";
            // 
            // labelControl_LimtNX
            // 
            this.labelControl_LimtNX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNX.Location = new System.Drawing.Point(434, 43);
            this.labelControl_LimtNX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNX.Name = "labelControl_LimtNX";
            this.labelControl_LimtNX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNX.TabIndex = 99;
            this.labelControl_LimtNX.Text = "OFF";
            // 
            // labelControl_LimtPX
            // 
            this.labelControl_LimtPX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPX.Location = new System.Drawing.Point(345, 43);
            this.labelControl_LimtPX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPX.Name = "labelControl_LimtPX";
            this.labelControl_LimtPX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPX.TabIndex = 98;
            this.labelControl_LimtPX.Text = "OFF";
            // 
            // labelControl_AlarmX
            // 
            this.labelControl_AlarmX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmX.Location = new System.Drawing.Point(304, 43);
            this.labelControl_AlarmX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmX.Name = "labelControl_AlarmX";
            this.labelControl_AlarmX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmX.TabIndex = 97;
            this.labelControl_AlarmX.Text = "OFF";
            // 
            // MoveOperate
            // 
            this.MoveOperate.Controls.Add(this.button_homeX);
            this.MoveOperate.Controls.Add(this.button_homeY);
            this.MoveOperate.Controls.Add(this.label16);
            this.MoveOperate.Controls.Add(this.label15);
            this.MoveOperate.Controls.Add(this.button_ServoOnU);
            this.MoveOperate.Controls.Add(this.label25);
            this.MoveOperate.Controls.Add(this.label24);
            this.MoveOperate.Controls.Add(this.label26);
            this.MoveOperate.Controls.Add(this.labelControl_EMGU);
            this.MoveOperate.Controls.Add(this.label27);
            this.MoveOperate.Controls.Add(this.label23);
            this.MoveOperate.Controls.Add(this.label28);
            this.MoveOperate.Controls.Add(this.button_ServoOnZ);
            this.MoveOperate.Controls.Add(this.label29);
            this.MoveOperate.Controls.Add(this.button_homeZ);
            this.MoveOperate.Controls.Add(this.labelControl_ORIU);
            this.MoveOperate.Controls.Add(this.label22);
            this.MoveOperate.Controls.Add(this.button_ServoOnY);
            this.MoveOperate.Controls.Add(this.button_homeU);
            this.MoveOperate.Controls.Add(this.labelControl_LimtNU);
            this.MoveOperate.Controls.Add(this.label21);
            this.MoveOperate.Controls.Add(this.button_ServoOnX);
            this.MoveOperate.Controls.Add(this.labelControl_AlarmX);
            this.MoveOperate.Controls.Add(this.labelControl_LimtPU);
            this.MoveOperate.Controls.Add(this.labelControl_LimtPX);
            this.MoveOperate.Controls.Add(this.labelControl_AlarmU);
            this.MoveOperate.Controls.Add(this.labelControl_LimtNX);
            this.MoveOperate.Controls.Add(this.labelControl_EMGZ);
            this.MoveOperate.Controls.Add(this.labelControl_ORIX);
            this.MoveOperate.Controls.Add(this.labelControl_ORIZ);
            this.MoveOperate.Controls.Add(this.labelControl_EMGX);
            this.MoveOperate.Controls.Add(this.labelControl_LimtNZ);
            this.MoveOperate.Controls.Add(this.label_ActPosX);
            this.MoveOperate.Controls.Add(this.label_CmdPosX);
            this.MoveOperate.Controls.Add(this.label_CmdPosU);
            this.MoveOperate.Controls.Add(this.labelControl_AlarmY);
            this.MoveOperate.Controls.Add(this.labelControl_LimtPY);
            this.MoveOperate.Controls.Add(this.labelControl_LimtPZ);
            this.MoveOperate.Controls.Add(this.label_ActPosY);
            this.MoveOperate.Controls.Add(this.label_ActPosU);
            this.MoveOperate.Controls.Add(this.labelControl_LimtNY);
            this.MoveOperate.Controls.Add(this.labelControl_AlarmZ);
            this.MoveOperate.Controls.Add(this.label_CmdPosY);
            this.MoveOperate.Controls.Add(this.label_CmdPosZ);
            this.MoveOperate.Controls.Add(this.labelControl_ORIY);
            this.MoveOperate.Controls.Add(this.labelControl_EMGY);
            this.MoveOperate.Controls.Add(this.label_ActPosZ);
            this.MoveOperate.Location = new System.Drawing.Point(0, 589);
            this.MoveOperate.Name = "MoveOperate";
            this.MoveOperate.Size = new System.Drawing.Size(520, 191);
            this.MoveOperate.TabIndex = 123;
            this.MoveOperate.TabStop = false;
            this.MoveOperate.Text = "运动调试";
            // 
            // visionControl1
            // 
            this.visionControl1.ImgHight = 1944;
            this.visionControl1.ImgWidth = 2592;
            this.visionControl1.Location = new System.Drawing.Point(3, 3);
            this.visionControl1.Name = "visionControl1";
            this.visionControl1.Size = new System.Drawing.Size(374, 311);
            this.visionControl1.TabIndex = 122;
            this.visionControl1.TabStop = false;
            // 
            // DispenseCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtManualDispenseOutTime);
            this.Controls.Add(this.btnDispenseOutManual);
            this.Controls.Add(this.MoveOperate);
            this.Controls.Add(this.visionControl1);
            this.Controls.Add(this.comboBox_SelMotionType);
            this.Controls.Add(this.button_Unegtive);
            this.Controls.Add(this.button_Ynegtive);
            this.Controls.Add(this.button_Znegtive);
            this.Controls.Add(this.button_Zpositive);
            this.Controls.Add(this.button_Xnegtive);
            this.Controls.Add(this.button_Xpositive);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_Upositive);
            this.Controls.Add(this.button_Ypositive);
            this.Controls.Add(this.btnLightONOFF);
            this.Controls.Add(this.rightTab1);
            this.Controls.Add(this.textExpsoureTimeVal);
            this.Controls.Add(this.ContinuousSnap);
            this.Controls.Add(this.textGainVal);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnSnapSave);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.BtnSanp);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.RoiSub);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnRoiplus);
            this.Controls.Add(this.BtnPrTest);
            this.Name = "DispenseCtrl";
            this.Size = new System.Drawing.Size(1106, 786);
            this.Load += new System.EventHandler(this.DispenseCtrl_Load);
            this.VisibleChanged += new System.EventHandler(this.DispenseCtrl_VisibleChanged);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PointInfo)).EndInit();
            this.VisionDstSet.ResumeLayout(false);
            this.VisionDstSet.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DispPosList)).EndInit();
            this.rightTab1.ResumeLayout(false);
            this.VisionCalibSet.ResumeLayout(false);
            this.VisionCalibSet.PerformLayout();
            this.DispTrace.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDispTrace)).EndInit();
            this.MoveOperate.ResumeLayout(false);
            this.MoveOperate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private VisionProcess.VisionMatchSetCtr visionMatchSetCtr1;
        private VisionProcess.VisionMatchSetCtr visionMatchSetCtr2;
        private System.Windows.Forms.Button RoiSub;
        private System.Windows.Forms.Button BtnRoiplus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_RecordPoint;
        private System.Windows.Forms.Button button_AllAxisMove;
        private System.Windows.Forms.Button button_SingleAxisMove;
        private System.Windows.Forms.DataGridView dataGridView_PointInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textPinYOffset;
        private System.Windows.Forms.TextBox textPinXOffset;
        private System.Windows.Forms.TextBox textYStep;
        private System.Windows.Forms.TextBox textXStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnPinTest;
        private System.Windows.Forms.Button BtnPrTest;
        private System.Windows.Forms.TextBox textLaserYOffset;
        private System.Windows.Forms.TextBox textLaserXOffset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnLaserTest;
        private System.Windows.Forms.Button BtnLaserCalib;
        private System.Windows.Forms.Button BtnCalibMotion;
        private System.Windows.Forms.Button btnPinHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Xpos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ypos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZPos;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_RecordPoint;
        private System.Windows.Forms.Button btn_AllAxisMove;
        private System.Windows.Forms.Button btn_SingleAxisMove;
        private System.Windows.Forms.DataGridView dataGridView_DispPosList;
        private System.Windows.Forms.Button BtnSanp;
        private System.Windows.Forms.TextBox textExpsoure;
        private System.Windows.Forms.TextBox textGain;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSnapSave;
        private System.Windows.Forms.Button ContinuousSnap;
        private System.Windows.Forms.TextBox textExpsoureTimeVal;
        private System.Windows.Forms.TextBox textGainVal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textDstExpsoure;
        private System.Windows.Forms.TextBox textDstGain;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabControl rightTab1;
        private System.Windows.Forms.TabPage VisionCalibSet;
        private System.Windows.Forms.TabPage VisionDstSet;
        private System.Windows.Forms.Button btnLaserReadData;
        private System.Windows.Forms.TextBox txtLaserReadData;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtZLatchData;
        private System.Windows.Forms.Label lbldispensepressure;
        private System.Windows.Forms.Timer timerScan;
        private System.Windows.Forms.Button btnDispenseOutManual;
        private System.Windows.Forms.TextBox txtManualDispenseOutTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnLightONOFF;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.TextBox textBox_DispDstLightVal;
        private System.Windows.Forms.Label labelLightDispDst;
        private System.Windows.Forms.TextBox textBox_DispCalbLightVal;
        private System.Windows.Forms.Label labelLightDispCalib;
        private System.Windows.Forms.Button button_Unegtive;
        private System.Windows.Forms.Button button_Ynegtive;
        private System.Windows.Forms.Button button_Znegtive;
        private System.Windows.Forms.Button button_Zpositive;
        private System.Windows.Forms.Button button_Xnegtive;
        private System.Windows.Forms.Button button_Xpositive;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_Upositive;
        private System.Windows.Forms.Button button_Ypositive;
        private System.Windows.Forms.ComboBox comboBox_SelMotionType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label_CmdPosX;
        private System.Windows.Forms.Label label_ActPosX;
        private System.Windows.Forms.Label label_CmdPosY;
        private System.Windows.Forms.Label label_ActPosY;
        private System.Windows.Forms.Label label_CmdPosZ;
        private System.Windows.Forms.Label label_ActPosZ;
        private System.Windows.Forms.Label label_CmdPosU;
        private System.Windows.Forms.Label label_ActPosU;
      
        private System.Windows.Forms.Button button_ServoOnZ;
        private System.Windows.Forms.Button button_ServoOnY;
        private System.Windows.Forms.Button button_ServoOnX;
        private System.Windows.Forms.Button button_ServoOnU;
        private System.Windows.Forms.Button button_homeU;
        private System.Windows.Forms.Button button_homeZ;
        private System.Windows.Forms.Button button_homeY;
        private System.Windows.Forms.Button button_homeX;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label labelControl_EMGU;
        private System.Windows.Forms.Label labelControl_ORIU;
        private System.Windows.Forms.Label labelControl_LimtNU;
        private System.Windows.Forms.Label labelControl_LimtPU;
        private System.Windows.Forms.Label labelControl_AlarmU;
        private System.Windows.Forms.Label labelControl_EMGZ;
        private System.Windows.Forms.Label labelControl_ORIZ;
        private System.Windows.Forms.Label labelControl_LimtNZ;
        private System.Windows.Forms.Label labelControl_LimtPZ;
        private System.Windows.Forms.Label labelControl_AlarmZ;
        private System.Windows.Forms.Label labelControl_EMGY;
        private System.Windows.Forms.Label labelControl_ORIY;
        private System.Windows.Forms.Label labelControl_LimtNY;
        private System.Windows.Forms.Label labelControl_LimtPY;
        private System.Windows.Forms.Label labelControl_AlarmY;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label labelControl_EMGX;
        private System.Windows.Forms.Label labelControl_ORIX;
        private System.Windows.Forms.Label labelControl_LimtNX;
        private System.Windows.Forms.Label labelControl_LimtPX;
        private System.Windows.Forms.Label labelControl_AlarmX;
        private UserCtrl.VisionControl visionControl1;
        private System.Windows.Forms.GroupBox MoveOperate;
        private System.Windows.Forms.TabPage DispTrace;
        private System.Windows.Forms.DataGridView dataGridViewDispTrace;
        private System.Windows.Forms.Button BtnDelay;
        private System.Windows.Forms.Button BtnIO;
        private System.Windows.Forms.Button BtnAddArc;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button BtnAddPoint;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DispName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewButtonColumn set;
        private System.Windows.Forms.Button BtnDel;
    }
}
