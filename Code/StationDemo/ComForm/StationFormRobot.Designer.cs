
namespace StationDemo
{
    partial class StationFormRobot
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
            this.button_Ypositive = new System.Windows.Forms.Button();
            this.button_Upositive = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_Xpositive = new System.Windows.Forms.Button();
            this.button_Xnegtive = new System.Windows.Forms.Button();
            this.button_Zpositive = new System.Windows.Forms.Button();
            this.button_Znegtive = new System.Windows.Forms.Button();
            this.button_Ynegtive = new System.Windows.Forms.Button();
            this.button_Unegtive = new System.Windows.Forms.Button();
            this.dataGridView_PointInfo = new System.Windows.Forms.DataGridView();
            this.PosName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Xpos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ypos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.V = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.W = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HandStyle = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.button_SingleAxisMove = new System.Windows.Forms.Button();
            this.button_AllAxisMove = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.comboBox_SelMotionType = new System.Windows.Forms.ComboBox();
            this.button_homeX = new System.Windows.Forms.Button();
            this.button_ServoOnX = new System.Windows.Forms.Button();
            this.button_RecordPoint = new System.Windows.Forms.Button();
            this.button_ContinuousSnap = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox_SelCamera = new System.Windows.Forms.ComboBox();
            this.button_start = new System.Windows.Forms.Button();
            this.btn_Del = new System.Windows.Forms.Button();
            this.button_Txpositive = new System.Windows.Forms.Button();
            this.button_Txnegtive = new System.Windows.Forms.Button();
            this.button_Typositive = new System.Windows.Forms.Button();
            this.button_Tynegtive = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblRobot = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTest = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTeach = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAuto = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWarning = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSError = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSafeguard = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblEStop = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblError = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPaused = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRunning = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblReady = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblXPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblYPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblZPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHandStyle = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHand = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPowerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.label_PosX = new System.Windows.Forms.Label();
            this.label_PosY = new System.Windows.Forms.Label();
            this.label_PosZ = new System.Windows.Forms.Label();
            this.label_PosU = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelHandSystem = new System.Windows.Forms.Label();
            this.visionControl1 = new UserCtrl.VisionControl();
            this.comboBox_SelVisionPR = new System.Windows.Forms.ComboBox();
            this.roundButton_VisionPrTest = new AutoFrameUI.RoundButton();
            this.userBtnPanel_Output = new UserCtrl.UserBtnPanel();
            this.userPanel_Input = new UserCtrl.UserPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PointInfo)).BeginInit();
            this.StatusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Ypositive
            // 
            this.button_Ypositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Ypositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Ypositive.ForeColor = System.Drawing.Color.Red;
            this.button_Ypositive.Location = new System.Drawing.Point(74, 74);
            this.button_Ypositive.Name = "button_Ypositive";
            this.button_Ypositive.Size = new System.Drawing.Size(53, 50);
            this.button_Ypositive.TabIndex = 0;
            this.button_Ypositive.Text = "Y+";
            this.button_Ypositive.UseVisualStyleBackColor = true;
            this.button_Ypositive.Click += new System.EventHandler(this.button_Ypostive_Click);
            this.button_Ypositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Ypositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Upositive
            // 
            this.button_Upositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Upositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Upositive.ForeColor = System.Drawing.Color.Red;
            this.button_Upositive.Location = new System.Drawing.Point(5, 74);
            this.button_Upositive.Name = "button_Upositive";
            this.button_Upositive.Size = new System.Drawing.Size(53, 50);
            this.button_Upositive.TabIndex = 1;
            this.button_Upositive.Text = "U+";
            this.button_Upositive.UseVisualStyleBackColor = true;
            this.button_Upositive.Click += new System.EventHandler(this.button_Upostive_Click);
            this.button_Upositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Upositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(74, 128);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(53, 50);
            this.button_stop.TabIndex = 2;
            this.button_stop.Text = "stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_Xpositive
            // 
            this.button_Xpositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Xpositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Xpositive.ForeColor = System.Drawing.Color.Red;
            this.button_Xpositive.Location = new System.Drawing.Point(139, 128);
            this.button_Xpositive.Name = "button_Xpositive";
            this.button_Xpositive.Size = new System.Drawing.Size(53, 50);
            this.button_Xpositive.TabIndex = 3;
            this.button_Xpositive.Text = "X+";
            this.button_Xpositive.UseVisualStyleBackColor = true;
            this.button_Xpositive.Click += new System.EventHandler(this.button_Xpositive_Click);
            this.button_Xpositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Xpositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Xnegtive
            // 
            this.button_Xnegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Xnegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Xnegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Xnegtive.Location = new System.Drawing.Point(5, 128);
            this.button_Xnegtive.Name = "button_Xnegtive";
            this.button_Xnegtive.Size = new System.Drawing.Size(53, 50);
            this.button_Xnegtive.TabIndex = 4;
            this.button_Xnegtive.Text = "X-";
            this.button_Xnegtive.UseVisualStyleBackColor = true;
            this.button_Xnegtive.Click += new System.EventHandler(this.button_Xnegtive_Click);
            this.button_Xnegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Xnegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Zpositive
            // 
            this.button_Zpositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Zpositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Zpositive.ForeColor = System.Drawing.Color.Red;
            this.button_Zpositive.Location = new System.Drawing.Point(139, 74);
            this.button_Zpositive.Name = "button_Zpositive";
            this.button_Zpositive.Size = new System.Drawing.Size(53, 50);
            this.button_Zpositive.TabIndex = 5;
            this.button_Zpositive.Text = "Z+";
            this.button_Zpositive.UseVisualStyleBackColor = true;
            this.button_Zpositive.Click += new System.EventHandler(this.button_Zpostive_Click);
            this.button_Zpositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Zpositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Znegtive
            // 
            this.button_Znegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Znegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Znegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Znegtive.Location = new System.Drawing.Point(139, 180);
            this.button_Znegtive.Name = "button_Znegtive";
            this.button_Znegtive.Size = new System.Drawing.Size(53, 50);
            this.button_Znegtive.TabIndex = 6;
            this.button_Znegtive.Text = "Z-";
            this.button_Znegtive.UseVisualStyleBackColor = true;
            this.button_Znegtive.Click += new System.EventHandler(this.button_Znegtive_Click);
            this.button_Znegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Znegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Ynegtive
            // 
            this.button_Ynegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Ynegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Ynegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Ynegtive.Location = new System.Drawing.Point(74, 180);
            this.button_Ynegtive.Name = "button_Ynegtive";
            this.button_Ynegtive.Size = new System.Drawing.Size(53, 50);
            this.button_Ynegtive.TabIndex = 7;
            this.button_Ynegtive.Text = "Y-";
            this.button_Ynegtive.UseVisualStyleBackColor = true;
            this.button_Ynegtive.Click += new System.EventHandler(this.button_Ynegtive_Click);
            this.button_Ynegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Ynegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Unegtive
            // 
            this.button_Unegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Unegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Unegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Unegtive.Location = new System.Drawing.Point(5, 180);
            this.button_Unegtive.Name = "button_Unegtive";
            this.button_Unegtive.Size = new System.Drawing.Size(53, 50);
            this.button_Unegtive.TabIndex = 8;
            this.button_Unegtive.Text = "U-";
            this.button_Unegtive.UseVisualStyleBackColor = true;
            this.button_Unegtive.Click += new System.EventHandler(this.buttonU_negtive_Click);
            this.button_Unegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Unegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // dataGridView_PointInfo
            // 
            this.dataGridView_PointInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_PointInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PosName,
            this.Xpos,
            this.Ypos,
            this.ZPos,
            this.UPos,
            this.V,
            this.W,
            this.HandStyle});
            this.dataGridView_PointInfo.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView_PointInfo.Location = new System.Drawing.Point(0, 237);
            this.dataGridView_PointInfo.Name = "dataGridView_PointInfo";
            this.dataGridView_PointInfo.RowHeadersVisible = false;
            this.dataGridView_PointInfo.RowTemplate.Height = 20;
            this.dataGridView_PointInfo.Size = new System.Drawing.Size(476, 391);
            this.dataGridView_PointInfo.TabIndex = 9;
            this.dataGridView_PointInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView_PointInfo.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.PointDel);
            // 
            // PosName
            // 
            this.PosName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PosName.HeaderText = "点位名称";
            this.PosName.Name = "PosName";
            this.PosName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PosName.Width = 150;
            // 
            // Xpos
            // 
            this.Xpos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Xpos.HeaderText = "X";
            this.Xpos.Name = "Xpos";
            this.Xpos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Xpos.Width = 80;
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
            // UPos
            // 
            this.UPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.UPos.HeaderText = "U";
            this.UPos.Name = "UPos";
            this.UPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UPos.Width = 60;
            // 
            // V
            // 
            this.V.HeaderText = "V";
            this.V.Name = "V";
            this.V.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.V.Width = 60;
            // 
            // W
            // 
            this.W.HeaderText = "W";
            this.W.Name = "W";
            this.W.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.W.Width = 60;
            // 
            // HandStyle
            // 
            this.HandStyle.HeaderText = "左右手";
            this.HandStyle.Items.AddRange(new object[] {
            "左手系",
            "右手系"});
            this.HandStyle.Name = "HandStyle";
            this.HandStyle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.HandStyle.Width = 50;
            // 
            // button_SingleAxisMove
            // 
            this.button_SingleAxisMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_SingleAxisMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_SingleAxisMove.Enabled = false;
            this.button_SingleAxisMove.Location = new System.Drawing.Point(482, 329);
            this.button_SingleAxisMove.Name = "button_SingleAxisMove";
            this.button_SingleAxisMove.Size = new System.Drawing.Size(66, 40);
            this.button_SingleAxisMove.TabIndex = 10;
            this.button_SingleAxisMove.Text = "点动";
            this.button_SingleAxisMove.UseVisualStyleBackColor = false;
            this.button_SingleAxisMove.Visible = false;
            this.button_SingleAxisMove.Click += new System.EventHandler(this.button_SingleAxisMove_Click);
            // 
            // button_AllAxisMove
            // 
            this.button_AllAxisMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_AllAxisMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_AllAxisMove.Location = new System.Drawing.Point(482, 375);
            this.button_AllAxisMove.Name = "button_AllAxisMove";
            this.button_AllAxisMove.Size = new System.Drawing.Size(66, 40);
            this.button_AllAxisMove.TabIndex = 11;
            this.button_AllAxisMove.Text = "联动";
            this.button_AllAxisMove.UseVisualStyleBackColor = false;
            this.button_AllAxisMove.Click += new System.EventHandler(this.button_AllAxisMove_Click);
            // 
            // button_Save
            // 
            this.button_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_Save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Save.Location = new System.Drawing.Point(482, 237);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(66, 40);
            this.button_Save.TabIndex = 12;
            this.button_Save.Text = "保存点位";
            this.button_Save.UseVisualStyleBackColor = false;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // comboBox_SelMotionType
            // 
            this.comboBox_SelMotionType.FormattingEnabled = true;
            this.comboBox_SelMotionType.Items.AddRange(new object[] {
            "0.01",
            "0.05",
            "0.1",
            "0.5",
            "1",
            "5",
            "10",
            "50",
            "100"});
            this.comboBox_SelMotionType.Location = new System.Drawing.Point(7, 2);
            this.comboBox_SelMotionType.Name = "comboBox_SelMotionType";
            this.comboBox_SelMotionType.Size = new System.Drawing.Size(183, 20);
            this.comboBox_SelMotionType.TabIndex = 13;
            // 
            // button_homeX
            // 
            this.button_homeX.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeX.Location = new System.Drawing.Point(7, 29);
            this.button_homeX.Name = "button_homeX";
            this.button_homeX.Size = new System.Drawing.Size(51, 33);
            this.button_homeX.TabIndex = 14;
            this.button_homeX.Text = "回原点";
            this.button_homeX.UseVisualStyleBackColor = false;
            this.button_homeX.Click += new System.EventHandler(this.button_homeX_Click);
            // 
            // button_ServoOnX
            // 
            this.button_ServoOnX.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnX.Location = new System.Drawing.Point(74, 29);
            this.button_ServoOnX.Name = "button_ServoOnX";
            this.button_ServoOnX.Size = new System.Drawing.Size(51, 33);
            this.button_ServoOnX.TabIndex = 15;
            this.button_ServoOnX.Text = "伺服On";
            this.button_ServoOnX.UseVisualStyleBackColor = false;
            this.button_ServoOnX.Click += new System.EventHandler(this.button_ServoOnX_Click);
            // 
            // button_RecordPoint
            // 
            this.button_RecordPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_RecordPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_RecordPoint.Location = new System.Drawing.Point(482, 283);
            this.button_RecordPoint.Name = "button_RecordPoint";
            this.button_RecordPoint.Size = new System.Drawing.Size(66, 40);
            this.button_RecordPoint.TabIndex = 17;
            this.button_RecordPoint.Text = "记录点位";
            this.button_RecordPoint.UseVisualStyleBackColor = false;
            this.button_RecordPoint.Click += new System.EventHandler(this.button_RecordPoint_Click);
            // 
            // button_ContinuousSnap
            // 
            this.button_ContinuousSnap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_ContinuousSnap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_ContinuousSnap.Location = new System.Drawing.Point(202, 43);
            this.button_ContinuousSnap.Name = "button_ContinuousSnap";
            this.button_ContinuousSnap.Size = new System.Drawing.Size(81, 32);
            this.button_ContinuousSnap.TabIndex = 25;
            this.button_ContinuousSnap.Text = "连续采集";
            this.button_ContinuousSnap.UseVisualStyleBackColor = false;
            this.button_ContinuousSnap.Click += new System.EventHandler(this.button_ContinuousSnap_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Location = new System.Drawing.Point(203, 82);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 32);
            this.button2.TabIndex = 26;
            this.button2.Text = "单次采集";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox_SelCamera
            // 
            this.comboBox_SelCamera.FormattingEnabled = true;
            this.comboBox_SelCamera.Location = new System.Drawing.Point(203, 2);
            this.comboBox_SelCamera.Name = "comboBox_SelCamera";
            this.comboBox_SelCamera.Size = new System.Drawing.Size(82, 20);
            this.comboBox_SelCamera.TabIndex = 27;
            this.comboBox_SelCamera.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelCamera_SelectedIndexChanged);
            // 
            // button_start
            // 
            this.button_start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_start.Location = new System.Drawing.Point(202, 124);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(81, 32);
            this.button_start.TabIndex = 28;
            this.button_start.Text = "启动";
            this.button_start.UseVisualStyleBackColor = false;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_Del.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_Del.Location = new System.Drawing.Point(482, 421);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(66, 40);
            this.btn_Del.TabIndex = 89;
            this.btn_Del.Text = "删除";
            this.btn_Del.UseVisualStyleBackColor = false;
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // button_Txpositive
            // 
            this.button_Txpositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Txpositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Txpositive.ForeColor = System.Drawing.Color.Red;
            this.button_Txpositive.Location = new System.Drawing.Point(230, 162);
            this.button_Txpositive.Name = "button_Txpositive";
            this.button_Txpositive.Size = new System.Drawing.Size(53, 22);
            this.button_Txpositive.TabIndex = 29;
            this.button_Txpositive.Text = "TX+";
            this.button_Txpositive.UseVisualStyleBackColor = true;
            this.button_Txpositive.Visible = false;
            this.button_Txpositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Txpositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Txnegtive
            // 
            this.button_Txnegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Txnegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Txnegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Txnegtive.Location = new System.Drawing.Point(210, 180);
            this.button_Txnegtive.Name = "button_Txnegtive";
            this.button_Txnegtive.Size = new System.Drawing.Size(53, 23);
            this.button_Txnegtive.TabIndex = 30;
            this.button_Txnegtive.Text = "TX-";
            this.button_Txnegtive.UseVisualStyleBackColor = true;
            this.button_Txnegtive.Visible = false;
            this.button_Txnegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Txnegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Typositive
            // 
            this.button_Typositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Typositive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Typositive.ForeColor = System.Drawing.Color.Red;
            this.button_Typositive.Location = new System.Drawing.Point(210, 173);
            this.button_Typositive.Name = "button_Typositive";
            this.button_Typositive.Size = new System.Drawing.Size(53, 37);
            this.button_Typositive.TabIndex = 31;
            this.button_Typositive.Text = "TY+";
            this.button_Typositive.UseVisualStyleBackColor = true;
            this.button_Typositive.Visible = false;
            this.button_Typositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Typositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Tynegtive
            // 
            this.button_Tynegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Tynegtive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Tynegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Tynegtive.Location = new System.Drawing.Point(210, 180);
            this.button_Tynegtive.Name = "button_Tynegtive";
            this.button_Tynegtive.Size = new System.Drawing.Size(53, 23);
            this.button_Tynegtive.TabIndex = 32;
            this.button_Tynegtive.Text = "TY-";
            this.button_Tynegtive.UseVisualStyleBackColor = true;
            this.button_Tynegtive.Visible = false;
            this.button_Tynegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Tynegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGreen;
            this.button1.Location = new System.Drawing.Point(139, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 33);
            this.button1.TabIndex = 90;
            this.button1.Text = "重置";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.buttonResetRobot_Click);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblRobot,
            this.lblTest,
            this.lblTeach,
            this.lblAuto,
            this.lblWarning,
            this.lblSError,
            this.lblSafeguard,
            this.lblEStop,
            this.lblError,
            this.lblPaused,
            this.lblRunning,
            this.lblReady,
            this.ToolStripStatusLabel1,
            this.lblXPos,
            this.ToolStripStatusLabel2,
            this.lblYPos,
            this.ToolStripStatusLabel3,
            this.lblZPos,
            this.ToolStripStatusLabel4,
            this.lblUPos,
            this.ToolStripStatusLabel5,
            this.lblVPos,
            this.ToolStripStatusLabel6,
            this.lblWPos,
            this.lblHandStyle,
            this.lblHand,
            this.lblPowerStatus});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 645);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.StatusStrip1.ShowItemToolTips = true;
            this.StatusStrip1.Size = new System.Drawing.Size(1273, 22);
            this.StatusStrip1.SizingGrip = false;
            this.StatusStrip1.TabIndex = 130;
            this.StatusStrip1.Text = "Robot";
            // 
            // lblRobot
            // 
            this.lblRobot.AutoToolTip = true;
            this.lblRobot.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblRobot.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblRobot.Name = "lblRobot";
            this.lblRobot.Size = new System.Drawing.Size(39, 17);
            this.lblRobot.Text = "Robot";
            this.lblRobot.ToolTipText = "绿色代表已经与机器人建立远程以太网通讯\r\n红色代表断开与机器人的远程以太网通讯\r\n";
            // 
            // lblTest
            // 
            this.lblTest.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTest.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(33, 17);
            this.lblTest.Text = "Test";
            // 
            // lblTeach
            // 
            this.lblTeach.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTeach.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblTeach.Name = "lblTeach";
            this.lblTeach.Size = new System.Drawing.Size(39, 17);
            this.lblTeach.Text = "Teach";
            // 
            // lblAuto
            // 
            this.lblAuto.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblAuto.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblAuto.Name = "lblAuto";
            this.lblAuto.Size = new System.Drawing.Size(33, 17);
            this.lblAuto.Text = "Auto";
            // 
            // lblWarning
            // 
            this.lblWarning.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblWarning.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(51, 17);
            this.lblWarning.Text = "Warning";
            // 
            // lblSError
            // 
            this.lblSError.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblSError.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblSError.Name = "lblSError";
            this.lblSError.Size = new System.Drawing.Size(45, 17);
            this.lblSError.Text = "SError";
            // 
            // lblSafeguard
            // 
            this.lblSafeguard.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblSafeguard.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblSafeguard.Name = "lblSafeguard";
            this.lblSafeguard.Size = new System.Drawing.Size(63, 17);
            this.lblSafeguard.Text = "Safeguard";
            // 
            // lblEStop
            // 
            this.lblEStop.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblEStop.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblEStop.Name = "lblEStop";
            this.lblEStop.Size = new System.Drawing.Size(39, 17);
            this.lblEStop.Text = "EStop";
            // 
            // lblError
            // 
            this.lblError.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblError.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(39, 17);
            this.lblError.Text = "Error";
            // 
            // lblPaused
            // 
            this.lblPaused.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblPaused.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblPaused.Name = "lblPaused";
            this.lblPaused.Size = new System.Drawing.Size(45, 17);
            this.lblPaused.Text = "Paused";
            // 
            // lblRunning
            // 
            this.lblRunning.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblRunning.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblRunning.Name = "lblRunning";
            this.lblRunning.Size = new System.Drawing.Size(51, 17);
            this.lblRunning.Text = "Running";
            // 
            // lblReady
            // 
            this.lblReady.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblReady.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblReady.Name = "lblReady";
            this.lblReady.Size = new System.Drawing.Size(39, 17);
            this.lblReady.Text = "Ready";
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(21, 17);
            this.ToolStripStatusLabel1.Text = "X:";
            // 
            // lblXPos
            // 
            this.lblXPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblXPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblXPos.Name = "lblXPos";
            this.lblXPos.Size = new System.Drawing.Size(45, 17);
            this.lblXPos.Text = "      ";
            // 
            // ToolStripStatusLabel2
            // 
            this.ToolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
            this.ToolStripStatusLabel2.Size = new System.Drawing.Size(21, 17);
            this.ToolStripStatusLabel2.Text = "Y:";
            // 
            // lblYPos
            // 
            this.lblYPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblYPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblYPos.Name = "lblYPos";
            this.lblYPos.Size = new System.Drawing.Size(57, 17);
            this.lblYPos.Text = "        ";
            // 
            // ToolStripStatusLabel3
            // 
            this.ToolStripStatusLabel3.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3";
            this.ToolStripStatusLabel3.Size = new System.Drawing.Size(21, 17);
            this.ToolStripStatusLabel3.Text = "Z:";
            // 
            // lblZPos
            // 
            this.lblZPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblZPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblZPos.Name = "lblZPos";
            this.lblZPos.Size = new System.Drawing.Size(57, 17);
            this.lblZPos.Text = "        ";
            // 
            // ToolStripStatusLabel4
            // 
            this.ToolStripStatusLabel4.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4";
            this.ToolStripStatusLabel4.Size = new System.Drawing.Size(21, 17);
            this.ToolStripStatusLabel4.Text = "U:";
            // 
            // lblUPos
            // 
            this.lblUPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblUPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblUPos.Name = "lblUPos";
            this.lblUPos.Size = new System.Drawing.Size(57, 17);
            this.lblUPos.Text = "        ";
            // 
            // ToolStripStatusLabel5
            // 
            this.ToolStripStatusLabel5.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel5.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel5.Name = "ToolStripStatusLabel5";
            this.ToolStripStatusLabel5.Size = new System.Drawing.Size(21, 17);
            this.ToolStripStatusLabel5.Text = "V:";
            // 
            // lblVPos
            // 
            this.lblVPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblVPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblVPos.Name = "lblVPos";
            this.lblVPos.Size = new System.Drawing.Size(57, 17);
            this.lblVPos.Text = "        ";
            // 
            // ToolStripStatusLabel6
            // 
            this.ToolStripStatusLabel6.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel6.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel6.Name = "ToolStripStatusLabel6";
            this.ToolStripStatusLabel6.Size = new System.Drawing.Size(21, 17);
            this.ToolStripStatusLabel6.Text = "W:";
            // 
            // lblWPos
            // 
            this.lblWPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblWPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblWPos.Name = "lblWPos";
            this.lblWPos.Size = new System.Drawing.Size(57, 17);
            this.lblWPos.Text = "        ";
            // 
            // lblHandStyle
            // 
            this.lblHandStyle.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblHandStyle.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblHandStyle.Name = "lblHandStyle";
            this.lblHandStyle.Size = new System.Drawing.Size(39, 17);
            this.lblHandStyle.Text = "Hand:";
            // 
            // lblHand
            // 
            this.lblHand.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblHand.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblHand.Name = "lblHand";
            this.lblHand.Size = new System.Drawing.Size(45, 17);
            this.lblHand.Text = "      ";
            // 
            // lblPowerStatus
            // 
            this.lblPowerStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblPowerStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblPowerStatus.Name = "lblPowerStatus";
            this.lblPowerStatus.Size = new System.Drawing.Size(81, 17);
            this.lblPowerStatus.Text = "Power Status";
            // 
            // label_PosX
            // 
            this.label_PosX.Location = new System.Drawing.Point(293, 72);
            this.label_PosX.Name = "label_PosX";
            this.label_PosX.Size = new System.Drawing.Size(55, 19);
            this.label_PosX.TabIndex = 131;
            this.label_PosX.Text = "000.000";
            // 
            // label_PosY
            // 
            this.label_PosY.Location = new System.Drawing.Point(359, 72);
            this.label_PosY.Name = "label_PosY";
            this.label_PosY.Size = new System.Drawing.Size(55, 19);
            this.label_PosY.TabIndex = 132;
            this.label_PosY.Text = "000.000";
            // 
            // label_PosZ
            // 
            this.label_PosZ.Location = new System.Drawing.Point(291, 123);
            this.label_PosZ.Name = "label_PosZ";
            this.label_PosZ.Size = new System.Drawing.Size(55, 19);
            this.label_PosZ.TabIndex = 133;
            this.label_PosZ.Text = "000.000";
            // 
            // label_PosU
            // 
            this.label_PosU.Location = new System.Drawing.Point(357, 123);
            this.label_PosU.Name = "label_PosU";
            this.label_PosU.Size = new System.Drawing.Size(55, 19);
            this.label_PosU.TabIndex = 134;
            this.label_PosU.Text = "000.000";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(318, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 20);
            this.label5.TabIndex = 135;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(377, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 20);
            this.label6.TabIndex = 136;
            this.label6.Text = "Y";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(302, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 20);
            this.label7.TabIndex = 137;
            this.label7.Text = "Z";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(359, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 20);
            this.label8.TabIndex = 138;
            this.label8.Text = "U";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(420, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 20);
            this.label9.TabIndex = 139;
            this.label9.Text = "手系";
            // 
            // labelHandSystem
            // 
            this.labelHandSystem.Location = new System.Drawing.Point(420, 73);
            this.labelHandSystem.Name = "labelHandSystem";
            this.labelHandSystem.Size = new System.Drawing.Size(55, 19);
            this.labelHandSystem.TabIndex = 140;
            this.labelHandSystem.Text = "左手系";
            // 
            // visionControl1
            // 
            this.visionControl1.ImgHight = 1944;
            this.visionControl1.ImgWidth = 2592;
            this.visionControl1.Location = new System.Drawing.Point(565, 5);
            this.visionControl1.Name = "visionControl1";
            this.visionControl1.Size = new System.Drawing.Size(561, 228);
            this.visionControl1.TabIndex = 141;
            this.visionControl1.TabStop = false;
            // 
            // comboBox_SelVisionPR
            // 
            this.comboBox_SelVisionPR.FormattingEnabled = true;
            this.comboBox_SelVisionPR.Location = new System.Drawing.Point(304, 2);
            this.comboBox_SelVisionPR.Name = "comboBox_SelVisionPR";
            this.comboBox_SelVisionPR.Size = new System.Drawing.Size(82, 20);
            this.comboBox_SelVisionPR.TabIndex = 142;
            // 
            // roundButton_VisionPrTest
            // 
            this.roundButton_VisionPrTest.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_VisionPrTest.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_VisionPrTest.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_VisionPrTest.FlatAppearance.BorderSize = 0;
            this.roundButton_VisionPrTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_VisionPrTest.ImageHeight = 80;
            this.roundButton_VisionPrTest.ImageWidth = 80;
            this.roundButton_VisionPrTest.Location = new System.Drawing.Point(401, -1);
            this.roundButton_VisionPrTest.Name = "roundButton_VisionPrTest";
            this.roundButton_VisionPrTest.Radius = 24;
            this.roundButton_VisionPrTest.Size = new System.Drawing.Size(75, 23);
            this.roundButton_VisionPrTest.SpliteButtonWidth = 18;
            this.roundButton_VisionPrTest.TabIndex = 143;
            this.roundButton_VisionPrTest.Text = "测试";
            this.roundButton_VisionPrTest.UseVisualStyleBackColor = false;
            this.roundButton_VisionPrTest.Click += new System.EventHandler(this.roundButton_VisionPrTest_Click);
            // 
            // userBtnPanel_Output
            // 
            this.userBtnPanel_Output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userBtnPanel_Output.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userBtnPanel_Output.Location = new System.Drawing.Point(915, 237);
            this.userBtnPanel_Output.m_nNumPerPage = 50;
            this.userBtnPanel_Output.m_nNumPerRow = 2;
            this.userBtnPanel_Output.m_page = 0;
            this.userBtnPanel_Output.m_splitHigh = 30;
            this.userBtnPanel_Output.m_splitWidth = 150;
            this.userBtnPanel_Output.Margin = new System.Windows.Forms.Padding(1);
            this.userBtnPanel_Output.Name = "userBtnPanel_Output";
            this.userBtnPanel_Output.Size = new System.Drawing.Size(341, 515);
            this.userBtnPanel_Output.TabIndex = 145;
            // 
            // userPanel_Input
            // 
            this.userPanel_Input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userPanel_Input.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userPanel_Input.Location = new System.Drawing.Point(565, 237);
            this.userPanel_Input.m_nNumPerPage = 50;
            this.userPanel_Input.m_nNumPerRow = 2;
            this.userPanel_Input.m_page = 0;
            this.userPanel_Input.m_splitHigh = 30;
            this.userPanel_Input.m_splitWidth = 150;
            this.userPanel_Input.Margin = new System.Windows.Forms.Padding(4);
            this.userPanel_Input.Name = "userPanel_Input";
            this.userPanel_Input.Size = new System.Drawing.Size(345, 515);
            this.userPanel_Input.TabIndex = 144;
            // 
            // StationFormRobot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1273, 667);
            this.Controls.Add(this.userBtnPanel_Output);
            this.Controls.Add(this.userPanel_Input);
            this.Controls.Add(this.roundButton_VisionPrTest);
            this.Controls.Add(this.comboBox_SelVisionPR);
            this.Controls.Add(this.visionControl1);
            this.Controls.Add(this.labelHandSystem);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label_PosU);
            this.Controls.Add(this.label_PosZ);
            this.Controls.Add(this.label_PosY);
            this.Controls.Add(this.label_PosX);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_Del);
            this.Controls.Add(this.button_Tynegtive);
            this.Controls.Add(this.button_Typositive);
            this.Controls.Add(this.button_Txnegtive);
            this.Controls.Add(this.button_Txpositive);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.comboBox_SelCamera);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_ContinuousSnap);
            this.Controls.Add(this.button_RecordPoint);
            this.Controls.Add(this.button_ServoOnX);
            this.Controls.Add(this.button_homeX);
            this.Controls.Add(this.comboBox_SelMotionType);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_AllAxisMove);
            this.Controls.Add(this.button_SingleAxisMove);
            this.Controls.Add(this.dataGridView_PointInfo);
            this.Controls.Add(this.button_Unegtive);
            this.Controls.Add(this.button_Ynegtive);
            this.Controls.Add(this.button_Znegtive);
            this.Controls.Add(this.button_Zpositive);
            this.Controls.Add(this.button_Xnegtive);
            this.Controls.Add(this.button_Xpositive);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_Upositive);
            this.Controls.Add(this.button_Ypositive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StationFormRobot";
            this.Text = "StationForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseForm);
            this.Load += new System.EventHandler(this.StationForm_Load);
            this.Shown += new System.EventHandler(this.ShowFirist);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PointInfo)).EndInit();
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Ypositive;
        private System.Windows.Forms.Button button_Upositive;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_Xpositive;
        private System.Windows.Forms.Button button_Xnegtive;
        private System.Windows.Forms.Button button_Zpositive;
        private System.Windows.Forms.Button button_Znegtive;
        private System.Windows.Forms.Button button_Ynegtive;
        private System.Windows.Forms.Button button_Unegtive;
        private System.Windows.Forms.DataGridView dataGridView_PointInfo;
        private System.Windows.Forms.Button button_SingleAxisMove;
        private System.Windows.Forms.Button button_AllAxisMove;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.ComboBox comboBox_SelMotionType;
        private System.Windows.Forms.Button button_homeX;
        private System.Windows.Forms.Button button_ServoOnX;
        private System.Windows.Forms.Button button_RecordPoint;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Button button_ContinuousSnap;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox_SelCamera;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button btn_Del;
  
   
        private System.Windows.Forms.Button button_Txpositive;
        private System.Windows.Forms.Button button_Txnegtive;
        private System.Windows.Forms.Button button_Typositive;
        private System.Windows.Forms.Button button_Tynegtive;
      

        private System.Windows.Forms.Button button1;

      
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel lblRobot;
        internal System.Windows.Forms.ToolStripStatusLabel lblTest;
        internal System.Windows.Forms.ToolStripStatusLabel lblTeach;
        internal System.Windows.Forms.ToolStripStatusLabel lblAuto;
        internal System.Windows.Forms.ToolStripStatusLabel lblWarning;
        internal System.Windows.Forms.ToolStripStatusLabel lblSError;
        internal System.Windows.Forms.ToolStripStatusLabel lblSafeguard;
        internal System.Windows.Forms.ToolStripStatusLabel lblEStop;
        internal System.Windows.Forms.ToolStripStatusLabel lblError;
        internal System.Windows.Forms.ToolStripStatusLabel lblPaused;
        internal System.Windows.Forms.ToolStripStatusLabel lblRunning;
        internal System.Windows.Forms.ToolStripStatusLabel lblReady;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        internal System.Windows.Forms.ToolStripStatusLabel lblXPos;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel2;
        internal System.Windows.Forms.ToolStripStatusLabel lblYPos;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel3;
        internal System.Windows.Forms.ToolStripStatusLabel lblZPos;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel4;
        internal System.Windows.Forms.ToolStripStatusLabel lblUPos;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel5;
        internal System.Windows.Forms.ToolStripStatusLabel lblVPos;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel6;
        internal System.Windows.Forms.ToolStripStatusLabel lblWPos;
        internal System.Windows.Forms.ToolStripStatusLabel lblHandStyle;
        internal System.Windows.Forms.ToolStripStatusLabel lblHand;
        internal System.Windows.Forms.ToolStripStatusLabel lblPowerStatus;
        private System.Windows.Forms.Label label_PosX;
        private System.Windows.Forms.Label label_PosY;
        private System.Windows.Forms.Label label_PosZ;
        private System.Windows.Forms.Label label_PosU;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelHandSystem;
        private UserCtrl.VisionControl visionControl1;
        private System.Windows.Forms.ComboBox comboBox_SelVisionPR;
        private AutoFrameUI.RoundButton roundButton_VisionPrTest;
        private System.Windows.Forms.DataGridViewTextBoxColumn PosName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Xpos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ypos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn V;
        private System.Windows.Forms.DataGridViewTextBoxColumn W;
        private System.Windows.Forms.DataGridViewComboBoxColumn HandStyle;
        private UserCtrl.UserBtnPanel userBtnPanel_Output;
        private UserCtrl.UserPanel userPanel_Input;
    }
}