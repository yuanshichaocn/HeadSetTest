
namespace StationDemo
{
    partial class StationForm
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
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Xpos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ypos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TyPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_SingleAxisMove = new System.Windows.Forms.Button();
            this.button_AllAxisMove = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.comboBox_SelMotionType = new System.Windows.Forms.ComboBox();
            this.button_homeX = new System.Windows.Forms.Button();
            this.button_ServoOnX = new System.Windows.Forms.Button();
            this.button_RecordPoint = new System.Windows.Forms.Button();
            this.button_homeY = new System.Windows.Forms.Button();
            this.button_homeZ = new System.Windows.Forms.Button();
            this.button_homeU = new System.Windows.Forms.Button();
            this.button_ServoOnY = new System.Windows.Forms.Button();
            this.button_ServoOnZ = new System.Windows.Forms.Button();
            this.button_ServoOnU = new System.Windows.Forms.Button();
            this.button_ContinuousSnap = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox_SelCamera = new System.Windows.Forms.ComboBox();
            this.button_start = new System.Windows.Forms.Button();
            this.button_Txpositive = new System.Windows.Forms.Button();
            this.button_Txnegtive = new System.Windows.Forms.Button();
            this.button_Typositive = new System.Windows.Forms.Button();
            this.button_Tynegtive = new System.Windows.Forms.Button();
            this.button_homeTx = new System.Windows.Forms.Button();
            this.button_homeTy = new System.Windows.Forms.Button();
            this.button_ServoOnTx = new System.Windows.Forms.Button();
            this.button_ServoOnTy = new System.Windows.Forms.Button();
            this.labelControl_AlarmX = new System.Windows.Forms.Label();
            this.labelControl_LimtPX = new System.Windows.Forms.Label();
            this.labelControl_LimtNX = new System.Windows.Forms.Label();
            this.labelControl_EMGX = new System.Windows.Forms.Label();
            this.labelControl_ORIX = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelControl_EMGY = new System.Windows.Forms.Label();
            this.labelControl_ORIY = new System.Windows.Forms.Label();
            this.labelControl_LimtNY = new System.Windows.Forms.Label();
            this.labelControl_LimtPY = new System.Windows.Forms.Label();
            this.labelControl_AlarmY = new System.Windows.Forms.Label();
            this.labelControl_EMGZ = new System.Windows.Forms.Label();
            this.labelControl_ORIZ = new System.Windows.Forms.Label();
            this.labelControl_LimtNZ = new System.Windows.Forms.Label();
            this.labelControl_LimtPZ = new System.Windows.Forms.Label();
            this.labelControl_AlarmZ = new System.Windows.Forms.Label();
            this.labelControl_EMGU = new System.Windows.Forms.Label();
            this.labelControl_ORIU = new System.Windows.Forms.Label();
            this.labelControl_LimtNU = new System.Windows.Forms.Label();
            this.labelControl_LimtPU = new System.Windows.Forms.Label();
            this.labelControl_AlarmU = new System.Windows.Forms.Label();
            this.labelControl_EMGTx = new System.Windows.Forms.Label();
            this.labelControl_ORITx = new System.Windows.Forms.Label();
            this.labelControl_LimtNTx = new System.Windows.Forms.Label();
            this.labelControl_LimtPTx = new System.Windows.Forms.Label();
            this.labelControl_AlarmTx = new System.Windows.Forms.Label();
            this.labelControl_EMGTy = new System.Windows.Forms.Label();
            this.labelControl_ORITy = new System.Windows.Forms.Label();
            this.labelControl_LimtNTy = new System.Windows.Forms.Label();
            this.labelControl_LimtPTy = new System.Windows.Forms.Label();
            this.labelControl_AlarmTy = new System.Windows.Forms.Label();
            this.label_ActPosX = new System.Windows.Forms.Label();
            this.label_CmdPosX = new System.Windows.Forms.Label();
            this.label_CmdPosY = new System.Windows.Forms.Label();
            this.label_ActPosY = new System.Windows.Forms.Label();
            this.label_CmdPosZ = new System.Windows.Forms.Label();
            this.label_ActPosZ = new System.Windows.Forms.Label();
            this.label_CmdPosU = new System.Windows.Forms.Label();
            this.label_ActPosU = new System.Windows.Forms.Label();
            this.label_CmdPosTx = new System.Windows.Forms.Label();
            this.label_ActPosTx = new System.Windows.Forms.Label();
            this.label_CmdPosTy = new System.Windows.Forms.Label();
            this.label_ActPosTy = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            //this.dataGridView_ioInput = new System.Windows.Forms.DataGridView();
            this.IoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IoInputState = new System.Windows.Forms.DataGridViewTextBoxColumn();
           // this.dataGridView_IoOutput = new System.Windows.Forms.DataGridView();
            this.IoOutputNmae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IoOutputState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IoOutOperate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btn_Del = new System.Windows.Forms.Button();
            this.comboBox_SelVisionPR = new System.Windows.Forms.ComboBox();
            this.userBtnPanel_Output = new UserCtrl.UserBtnPanel();
            this.userPanel_Input = new UserCtrl.UserPanel();
            this.roundButton_VisionPrTest = new AutoFrameUI.RoundButton();
            this.visionControl1 = new UserCtrl.VisionControl();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PointInfo)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.dataGridView_ioInput)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.dataGridView_IoOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Ypositive
            // 
            this.button_Ypositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Ypositive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Ypositive.ForeColor = System.Drawing.Color.Red;
            this.button_Ypositive.Location = new System.Drawing.Point(425, 82);
            this.button_Ypositive.Name = "button_Ypositive";
            this.button_Ypositive.Size = new System.Drawing.Size(61, 23);
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
            this.button_Upositive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Upositive.ForeColor = System.Drawing.Color.Red;
            this.button_Upositive.Location = new System.Drawing.Point(425, 156);
            this.button_Upositive.Name = "button_Upositive";
            this.button_Upositive.Size = new System.Drawing.Size(61, 23);
            this.button_Upositive.TabIndex = 1;
            this.button_Upositive.Text = "U+";
            this.button_Upositive.UseVisualStyleBackColor = true;
            this.button_Upositive.Click += new System.EventHandler(this.button_Upostive_Click);
            this.button_Upositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Upositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(558, 43);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(44, 213);
            this.button_stop.TabIndex = 2;
            this.button_stop.Text = "stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_Xpositive
            // 
            this.button_Xpositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Xpositive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Xpositive.ForeColor = System.Drawing.Color.Red;
            this.button_Xpositive.Location = new System.Drawing.Point(425, 43);
            this.button_Xpositive.Name = "button_Xpositive";
            this.button_Xpositive.Size = new System.Drawing.Size(61, 23);
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
            this.button_Xnegtive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Xnegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Xnegtive.Location = new System.Drawing.Point(494, 44);
            this.button_Xnegtive.Name = "button_Xnegtive";
            this.button_Xnegtive.Size = new System.Drawing.Size(61, 23);
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
            this.button_Zpositive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Zpositive.ForeColor = System.Drawing.Color.Red;
            this.button_Zpositive.Location = new System.Drawing.Point(425, 118);
            this.button_Zpositive.Name = "button_Zpositive";
            this.button_Zpositive.Size = new System.Drawing.Size(61, 23);
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
            this.button_Znegtive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Znegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Znegtive.Location = new System.Drawing.Point(494, 118);
            this.button_Znegtive.Name = "button_Znegtive";
            this.button_Znegtive.Size = new System.Drawing.Size(61, 23);
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
            this.button_Ynegtive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Ynegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Ynegtive.Location = new System.Drawing.Point(494, 82);
            this.button_Ynegtive.Name = "button_Ynegtive";
            this.button_Ynegtive.Size = new System.Drawing.Size(61, 23);
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
            this.button_Unegtive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Unegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Unegtive.Location = new System.Drawing.Point(494, 156);
            this.button_Unegtive.Name = "button_Unegtive";
            this.button_Unegtive.Size = new System.Drawing.Size(61, 23);
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
            this.Column1,
            this.Xpos,
            this.Ypos,
            this.ZPos,
            this.UPos,
            this.TxPos,
            this.TyPos});
            this.dataGridView_PointInfo.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView_PointInfo.Location = new System.Drawing.Point(7, 290);
            this.dataGridView_PointInfo.Name = "dataGridView_PointInfo";
            this.dataGridView_PointInfo.RowHeadersVisible = false;
            this.dataGridView_PointInfo.RowTemplate.Height = 20;
            this.dataGridView_PointInfo.Size = new System.Drawing.Size(603, 523);
            this.dataGridView_PointInfo.TabIndex = 9;
            this.dataGridView_PointInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView_PointInfo.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.PointDel);
            // 
            // Column1
            // 
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "点位名称";
            this.Column1.MinimumWidth = 50;
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 180;
            // 
            // Xpos
            // 
            this.Xpos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Xpos.HeaderText = "X";
            this.Xpos.Name = "Xpos";
            this.Xpos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Xpos.Width = 50;
            // 
            // Ypos
            // 
            this.Ypos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Ypos.HeaderText = "Y";
            this.Ypos.Name = "Ypos";
            this.Ypos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Ypos.Width = 50;
            // 
            // ZPos
            // 
            this.ZPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ZPos.HeaderText = "Z";
            this.ZPos.Name = "ZPos";
            this.ZPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ZPos.Width = 50;
            // 
            // UPos
            // 
            this.UPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.UPos.HeaderText = "U";
            this.UPos.Name = "UPos";
            this.UPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UPos.Width = 50;
            // 
            // TxPos
            // 
            this.TxPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TxPos.HeaderText = "Tx";
            this.TxPos.Name = "TxPos";
            this.TxPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TxPos.Width = 50;
            // 
            // TyPos
            // 
            this.TyPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TyPos.HeaderText = "Ty";
            this.TyPos.Name = "TyPos";
            this.TyPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TyPos.Width = 50;
            // 
            // button_SingleAxisMove
            // 
            this.button_SingleAxisMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_SingleAxisMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_SingleAxisMove.Location = new System.Drawing.Point(616, 383);
            this.button_SingleAxisMove.Name = "button_SingleAxisMove";
            this.button_SingleAxisMove.Size = new System.Drawing.Size(66, 40);
            this.button_SingleAxisMove.TabIndex = 10;
            this.button_SingleAxisMove.Text = "点动";
            this.button_SingleAxisMove.UseVisualStyleBackColor = false;
            this.button_SingleAxisMove.Click += new System.EventHandler(this.button_SingleAxisMove_Click);
            // 
            // button_AllAxisMove
            // 
            this.button_AllAxisMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_AllAxisMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_AllAxisMove.Location = new System.Drawing.Point(616, 430);
            this.button_AllAxisMove.Name = "button_AllAxisMove";
            this.button_AllAxisMove.Size = new System.Drawing.Size(66, 40);
            this.button_AllAxisMove.TabIndex = 11;
            this.button_AllAxisMove.Text = "联动";
            this.button_AllAxisMove.UseVisualStyleBackColor = false;
            this.button_AllAxisMove.Visible = false;
            this.button_AllAxisMove.Click += new System.EventHandler(this.button_AllAxisMove_Click);
            // 
            // button_Save
            // 
            this.button_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_Save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Save.Location = new System.Drawing.Point(616, 290);
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
            "Jog",
            "0.01",
            "0.05",
            "0.1",
            "1",
            "5",
            "10",
            "50",
            "100",
            "90"});
            this.comboBox_SelMotionType.Location = new System.Drawing.Point(428, 6);
            this.comboBox_SelMotionType.Name = "comboBox_SelMotionType";
            this.comboBox_SelMotionType.Size = new System.Drawing.Size(102, 20);
            this.comboBox_SelMotionType.TabIndex = 13;
            // 
            // button_homeX
            // 
            this.button_homeX.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeX.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_homeX.Location = new System.Drawing.Point(2, 43);
            this.button_homeX.Name = "button_homeX";
            this.button_homeX.Size = new System.Drawing.Size(46, 30);
            this.button_homeX.TabIndex = 14;
            this.button_homeX.Text = "回原点";
            this.button_homeX.UseVisualStyleBackColor = false;
            this.button_homeX.Click += new System.EventHandler(this.button_homeX_Click);
            // 
            // button_ServoOnX
            // 
            this.button_ServoOnX.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnX.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ServoOnX.Location = new System.Drawing.Point(54, 44);
            this.button_ServoOnX.Name = "button_ServoOnX";
            this.button_ServoOnX.Size = new System.Drawing.Size(46, 30);
            this.button_ServoOnX.TabIndex = 15;
            this.button_ServoOnX.Text = "伺服On";
            this.button_ServoOnX.UseVisualStyleBackColor = false;
            this.button_ServoOnX.Click += new System.EventHandler(this.button_ServoOnX_Click);
            // 
            // button_RecordPoint
            // 
            this.button_RecordPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_RecordPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_RecordPoint.Location = new System.Drawing.Point(616, 337);
            this.button_RecordPoint.Name = "button_RecordPoint";
            this.button_RecordPoint.Size = new System.Drawing.Size(66, 40);
            this.button_RecordPoint.TabIndex = 17;
            this.button_RecordPoint.Text = "记录点位";
            this.button_RecordPoint.UseVisualStyleBackColor = false;
            this.button_RecordPoint.Click += new System.EventHandler(this.button_RecordPoint_Click);
            // 
            // button_homeY
            // 
            this.button_homeY.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeY.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_homeY.Location = new System.Drawing.Point(2, 80);
            this.button_homeY.Name = "button_homeY";
            this.button_homeY.Size = new System.Drawing.Size(46, 30);
            this.button_homeY.TabIndex = 18;
            this.button_homeY.Text = "回原点";
            this.button_homeY.UseVisualStyleBackColor = false;
            this.button_homeY.Click += new System.EventHandler(this.button_homeY_Click);
            // 
            // button_homeZ
            // 
            this.button_homeZ.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeZ.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_homeZ.Location = new System.Drawing.Point(2, 121);
            this.button_homeZ.Name = "button_homeZ";
            this.button_homeZ.Size = new System.Drawing.Size(46, 30);
            this.button_homeZ.TabIndex = 19;
            this.button_homeZ.Text = "回原点";
            this.button_homeZ.UseVisualStyleBackColor = false;
            this.button_homeZ.Click += new System.EventHandler(this.button_homeZ_Click);
            // 
            // button_homeU
            // 
            this.button_homeU.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeU.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_homeU.Location = new System.Drawing.Point(2, 158);
            this.button_homeU.Name = "button_homeU";
            this.button_homeU.Size = new System.Drawing.Size(46, 30);
            this.button_homeU.TabIndex = 20;
            this.button_homeU.Text = "回原点";
            this.button_homeU.UseVisualStyleBackColor = false;
            this.button_homeU.Click += new System.EventHandler(this.button_homeU_Click);
            // 
            // button_ServoOnY
            // 
            this.button_ServoOnY.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnY.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ServoOnY.Location = new System.Drawing.Point(54, 83);
            this.button_ServoOnY.Name = "button_ServoOnY";
            this.button_ServoOnY.Size = new System.Drawing.Size(46, 30);
            this.button_ServoOnY.TabIndex = 21;
            this.button_ServoOnY.Text = "伺服On";
            this.button_ServoOnY.UseVisualStyleBackColor = false;
            this.button_ServoOnY.Click += new System.EventHandler(this.button_ServoOnY_Click);
            // 
            // button_ServoOnZ
            // 
            this.button_ServoOnZ.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnZ.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ServoOnZ.Location = new System.Drawing.Point(54, 121);
            this.button_ServoOnZ.Name = "button_ServoOnZ";
            this.button_ServoOnZ.Size = new System.Drawing.Size(46, 30);
            this.button_ServoOnZ.TabIndex = 22;
            this.button_ServoOnZ.Text = "伺服On";
            this.button_ServoOnZ.UseVisualStyleBackColor = false;
            this.button_ServoOnZ.Click += new System.EventHandler(this.button_ServoOnZ_Click);
            // 
            // button_ServoOnU
            // 
            this.button_ServoOnU.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnU.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ServoOnU.Location = new System.Drawing.Point(54, 160);
            this.button_ServoOnU.Name = "button_ServoOnU";
            this.button_ServoOnU.Size = new System.Drawing.Size(46, 30);
            this.button_ServoOnU.TabIndex = 23;
            this.button_ServoOnU.Text = "伺服On";
            this.button_ServoOnU.UseVisualStyleBackColor = false;
            this.button_ServoOnU.Click += new System.EventHandler(this.button_ServoOnU_Click);
            // 
            // button_ContinuousSnap
            // 
            this.button_ContinuousSnap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_ContinuousSnap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_ContinuousSnap.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ContinuousSnap.Location = new System.Drawing.Point(613, 38);
            this.button_ContinuousSnap.Name = "button_ContinuousSnap";
            this.button_ContinuousSnap.Size = new System.Drawing.Size(66, 40);
            this.button_ContinuousSnap.TabIndex = 25;
            this.button_ContinuousSnap.Text = "连续采集";
            this.button_ContinuousSnap.UseVisualStyleBackColor = false;
            this.button_ContinuousSnap.Click += new System.EventHandler(this.button_ContinuousSnap_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(613, 91);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(66, 40);
            this.button2.TabIndex = 26;
            this.button2.Text = "单次采集";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox_SelCamera
            // 
            this.comboBox_SelCamera.FormattingEnabled = true;
            this.comboBox_SelCamera.Location = new System.Drawing.Point(536, 6);
            this.comboBox_SelCamera.Name = "comboBox_SelCamera";
            this.comboBox_SelCamera.Size = new System.Drawing.Size(82, 20);
            this.comboBox_SelCamera.TabIndex = 27;
            this.comboBox_SelCamera.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelCamera_SelectedIndexChanged);
            // 
            // button_start
            // 
            this.button_start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_start.Location = new System.Drawing.Point(2, 2);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(55, 24);
            this.button_start.TabIndex = 28;
            this.button_start.Text = "启动";
            this.button_start.UseVisualStyleBackColor = false;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_Txpositive
            // 
            this.button_Txpositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Txpositive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Txpositive.ForeColor = System.Drawing.Color.Red;
            this.button_Txpositive.Location = new System.Drawing.Point(425, 196);
            this.button_Txpositive.Name = "button_Txpositive";
            this.button_Txpositive.Size = new System.Drawing.Size(61, 23);
            this.button_Txpositive.TabIndex = 29;
            this.button_Txpositive.Text = "TX+";
            this.button_Txpositive.UseVisualStyleBackColor = true;
            this.button_Txpositive.Click += new System.EventHandler(this.button_Txpositive_Click);
            this.button_Txpositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Txpositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Txnegtive
            // 
            this.button_Txnegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Txnegtive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Txnegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Txnegtive.Location = new System.Drawing.Point(494, 193);
            this.button_Txnegtive.Name = "button_Txnegtive";
            this.button_Txnegtive.Size = new System.Drawing.Size(61, 23);
            this.button_Txnegtive.TabIndex = 30;
            this.button_Txnegtive.Text = "TX-";
            this.button_Txnegtive.UseVisualStyleBackColor = true;
            this.button_Txnegtive.Click += new System.EventHandler(this.button_Txnegtive_Click);
            this.button_Txnegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Txnegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Typositive
            // 
            this.button_Typositive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Typositive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Typositive.ForeColor = System.Drawing.Color.Red;
            this.button_Typositive.Location = new System.Drawing.Point(425, 237);
            this.button_Typositive.Name = "button_Typositive";
            this.button_Typositive.Size = new System.Drawing.Size(61, 23);
            this.button_Typositive.TabIndex = 31;
            this.button_Typositive.Text = "TY+";
            this.button_Typositive.UseVisualStyleBackColor = true;
            this.button_Typositive.Click += new System.EventHandler(this.button_Typositive_Click);
            this.button_Typositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Typositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_Tynegtive
            // 
            this.button_Tynegtive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Tynegtive.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Tynegtive.ForeColor = System.Drawing.Color.Red;
            this.button_Tynegtive.Location = new System.Drawing.Point(494, 236);
            this.button_Tynegtive.Name = "button_Tynegtive";
            this.button_Tynegtive.Size = new System.Drawing.Size(61, 23);
            this.button_Tynegtive.TabIndex = 32;
            this.button_Tynegtive.Text = "TY-";
            this.button_Tynegtive.UseVisualStyleBackColor = true;
            this.button_Tynegtive.Click += new System.EventHandler(this.button_Tynegtive_Click);
            this.button_Tynegtive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogStart);
            this.button_Tynegtive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JogEnd);
            // 
            // button_homeTx
            // 
            this.button_homeTx.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeTx.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_homeTx.Location = new System.Drawing.Point(2, 198);
            this.button_homeTx.Name = "button_homeTx";
            this.button_homeTx.Size = new System.Drawing.Size(46, 30);
            this.button_homeTx.TabIndex = 33;
            this.button_homeTx.Text = "回原点";
            this.button_homeTx.UseVisualStyleBackColor = false;
            this.button_homeTx.Click += new System.EventHandler(this.button_homeTx_Click);
            // 
            // button_homeTy
            // 
            this.button_homeTy.BackColor = System.Drawing.Color.LightGreen;
            this.button_homeTy.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_homeTy.Location = new System.Drawing.Point(2, 239);
            this.button_homeTy.Name = "button_homeTy";
            this.button_homeTy.Size = new System.Drawing.Size(46, 30);
            this.button_homeTy.TabIndex = 34;
            this.button_homeTy.Text = "回原点";
            this.button_homeTy.UseVisualStyleBackColor = false;
            this.button_homeTy.Click += new System.EventHandler(this.button_homeTy_Click);
            // 
            // button_ServoOnTx
            // 
            this.button_ServoOnTx.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnTx.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ServoOnTx.Location = new System.Drawing.Point(54, 199);
            this.button_ServoOnTx.Name = "button_ServoOnTx";
            this.button_ServoOnTx.Size = new System.Drawing.Size(46, 30);
            this.button_ServoOnTx.TabIndex = 35;
            this.button_ServoOnTx.Text = "伺服On";
            this.button_ServoOnTx.UseVisualStyleBackColor = false;
            this.button_ServoOnTx.Click += new System.EventHandler(this.button_ServoOnTx_Click);
            // 
            // button_ServoOnTy
            // 
            this.button_ServoOnTy.BackColor = System.Drawing.Color.LightGreen;
            this.button_ServoOnTy.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ServoOnTy.Location = new System.Drawing.Point(54, 237);
            this.button_ServoOnTy.Name = "button_ServoOnTy";
            this.button_ServoOnTy.Size = new System.Drawing.Size(46, 30);
            this.button_ServoOnTy.TabIndex = 36;
            this.button_ServoOnTy.Text = "伺服On";
            this.button_ServoOnTy.UseVisualStyleBackColor = false;
            this.button_ServoOnTy.Click += new System.EventHandler(this.button_ServoOnTy_Click);
            // 
            // labelControl_AlarmX
            // 
            this.labelControl_AlarmX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmX.Location = new System.Drawing.Point(259, 47);
            this.labelControl_AlarmX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmX.Name = "labelControl_AlarmX";
            this.labelControl_AlarmX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmX.TabIndex = 37;
            this.labelControl_AlarmX.Text = "OFF";
            // 
            // labelControl_LimtPX
            // 
            this.labelControl_LimtPX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPX.Location = new System.Drawing.Point(294, 47);
            this.labelControl_LimtPX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPX.Name = "labelControl_LimtPX";
            this.labelControl_LimtPX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPX.TabIndex = 38;
            this.labelControl_LimtPX.Text = "OFF";
            // 
            // labelControl_LimtNX
            // 
            this.labelControl_LimtNX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNX.Location = new System.Drawing.Point(361, 47);
            this.labelControl_LimtNX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNX.Name = "labelControl_LimtNX";
            this.labelControl_LimtNX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNX.TabIndex = 39;
            this.labelControl_LimtNX.Text = "OFF";
            // 
            // labelControl_EMGX
            // 
            this.labelControl_EMGX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGX.Location = new System.Drawing.Point(396, 46);
            this.labelControl_EMGX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGX.Name = "labelControl_EMGX";
            this.labelControl_EMGX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGX.TabIndex = 41;
            this.labelControl_EMGX.Text = "OFF";
            // 
            // labelControl_ORIX
            // 
            this.labelControl_ORIX.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORIX.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORIX.Location = new System.Drawing.Point(329, 47);
            this.labelControl_ORIX.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORIX.Name = "labelControl_ORIX";
            this.labelControl_ORIX.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORIX.TabIndex = 40;
            this.labelControl_ORIX.Text = "OFF";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(256, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "报警";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(289, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "正限";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(355, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "负限";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(322, 23);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "原点";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(388, 23);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "急停";
            // 
            // labelControl_EMGY
            // 
            this.labelControl_EMGY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGY.Location = new System.Drawing.Point(396, 84);
            this.labelControl_EMGY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGY.Name = "labelControl_EMGY";
            this.labelControl_EMGY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGY.TabIndex = 52;
            this.labelControl_EMGY.Text = "OFF";
            // 
            // labelControl_ORIY
            // 
            this.labelControl_ORIY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORIY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORIY.Location = new System.Drawing.Point(329, 85);
            this.labelControl_ORIY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORIY.Name = "labelControl_ORIY";
            this.labelControl_ORIY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORIY.TabIndex = 51;
            this.labelControl_ORIY.Text = "OFF";
            // 
            // labelControl_LimtNY
            // 
            this.labelControl_LimtNY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNY.Location = new System.Drawing.Point(361, 85);
            this.labelControl_LimtNY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNY.Name = "labelControl_LimtNY";
            this.labelControl_LimtNY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNY.TabIndex = 50;
            this.labelControl_LimtNY.Text = "OFF";
            // 
            // labelControl_LimtPY
            // 
            this.labelControl_LimtPY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPY.Location = new System.Drawing.Point(294, 85);
            this.labelControl_LimtPY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPY.Name = "labelControl_LimtPY";
            this.labelControl_LimtPY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPY.TabIndex = 49;
            this.labelControl_LimtPY.Text = "OFF";
            // 
            // labelControl_AlarmY
            // 
            this.labelControl_AlarmY.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmY.Location = new System.Drawing.Point(259, 85);
            this.labelControl_AlarmY.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmY.Name = "labelControl_AlarmY";
            this.labelControl_AlarmY.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmY.TabIndex = 48;
            this.labelControl_AlarmY.Text = "OFF";
            // 
            // labelControl_EMGZ
            // 
            this.labelControl_EMGZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGZ.Location = new System.Drawing.Point(396, 122);
            this.labelControl_EMGZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGZ.Name = "labelControl_EMGZ";
            this.labelControl_EMGZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGZ.TabIndex = 57;
            this.labelControl_EMGZ.Text = "OFF";
            // 
            // labelControl_ORIZ
            // 
            this.labelControl_ORIZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORIZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORIZ.Location = new System.Drawing.Point(329, 123);
            this.labelControl_ORIZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORIZ.Name = "labelControl_ORIZ";
            this.labelControl_ORIZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORIZ.TabIndex = 56;
            this.labelControl_ORIZ.Text = "OFF";
            // 
            // labelControl_LimtNZ
            // 
            this.labelControl_LimtNZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNZ.Location = new System.Drawing.Point(361, 123);
            this.labelControl_LimtNZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNZ.Name = "labelControl_LimtNZ";
            this.labelControl_LimtNZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNZ.TabIndex = 55;
            this.labelControl_LimtNZ.Text = "OFF";
            // 
            // labelControl_LimtPZ
            // 
            this.labelControl_LimtPZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPZ.Location = new System.Drawing.Point(294, 123);
            this.labelControl_LimtPZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPZ.Name = "labelControl_LimtPZ";
            this.labelControl_LimtPZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPZ.TabIndex = 54;
            this.labelControl_LimtPZ.Text = "OFF";
            // 
            // labelControl_AlarmZ
            // 
            this.labelControl_AlarmZ.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmZ.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmZ.Location = new System.Drawing.Point(259, 123);
            this.labelControl_AlarmZ.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmZ.Name = "labelControl_AlarmZ";
            this.labelControl_AlarmZ.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmZ.TabIndex = 53;
            this.labelControl_AlarmZ.Text = "OFF";
            // 
            // labelControl_EMGU
            // 
            this.labelControl_EMGU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGU.Location = new System.Drawing.Point(396, 160);
            this.labelControl_EMGU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGU.Name = "labelControl_EMGU";
            this.labelControl_EMGU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGU.TabIndex = 62;
            this.labelControl_EMGU.Text = "OFF";
            // 
            // labelControl_ORIU
            // 
            this.labelControl_ORIU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORIU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORIU.Location = new System.Drawing.Point(329, 161);
            this.labelControl_ORIU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORIU.Name = "labelControl_ORIU";
            this.labelControl_ORIU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORIU.TabIndex = 61;
            this.labelControl_ORIU.Text = "OFF";
            // 
            // labelControl_LimtNU
            // 
            this.labelControl_LimtNU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNU.Location = new System.Drawing.Point(361, 161);
            this.labelControl_LimtNU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNU.Name = "labelControl_LimtNU";
            this.labelControl_LimtNU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNU.TabIndex = 60;
            this.labelControl_LimtNU.Text = "OFF";
            // 
            // labelControl_LimtPU
            // 
            this.labelControl_LimtPU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPU.Location = new System.Drawing.Point(294, 161);
            this.labelControl_LimtPU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPU.Name = "labelControl_LimtPU";
            this.labelControl_LimtPU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPU.TabIndex = 59;
            this.labelControl_LimtPU.Text = "OFF";
            // 
            // labelControl_AlarmU
            // 
            this.labelControl_AlarmU.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmU.Location = new System.Drawing.Point(259, 161);
            this.labelControl_AlarmU.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmU.Name = "labelControl_AlarmU";
            this.labelControl_AlarmU.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmU.TabIndex = 58;
            this.labelControl_AlarmU.Text = "OFF";
            // 
            // labelControl_EMGTx
            // 
            this.labelControl_EMGTx.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGTx.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGTx.Location = new System.Drawing.Point(396, 198);
            this.labelControl_EMGTx.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGTx.Name = "labelControl_EMGTx";
            this.labelControl_EMGTx.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGTx.TabIndex = 67;
            this.labelControl_EMGTx.Text = "OFF";
            // 
            // labelControl_ORITx
            // 
            this.labelControl_ORITx.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORITx.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORITx.Location = new System.Drawing.Point(329, 199);
            this.labelControl_ORITx.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORITx.Name = "labelControl_ORITx";
            this.labelControl_ORITx.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORITx.TabIndex = 66;
            this.labelControl_ORITx.Text = "OFF";
            // 
            // labelControl_LimtNTx
            // 
            this.labelControl_LimtNTx.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNTx.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNTx.Location = new System.Drawing.Point(361, 199);
            this.labelControl_LimtNTx.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNTx.Name = "labelControl_LimtNTx";
            this.labelControl_LimtNTx.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNTx.TabIndex = 65;
            this.labelControl_LimtNTx.Text = "OFF";
            // 
            // labelControl_LimtPTx
            // 
            this.labelControl_LimtPTx.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPTx.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPTx.Location = new System.Drawing.Point(294, 199);
            this.labelControl_LimtPTx.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPTx.Name = "labelControl_LimtPTx";
            this.labelControl_LimtPTx.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPTx.TabIndex = 64;
            this.labelControl_LimtPTx.Text = "OFF";
            // 
            // labelControl_AlarmTx
            // 
            this.labelControl_AlarmTx.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmTx.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmTx.Location = new System.Drawing.Point(259, 199);
            this.labelControl_AlarmTx.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmTx.Name = "labelControl_AlarmTx";
            this.labelControl_AlarmTx.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmTx.TabIndex = 63;
            this.labelControl_AlarmTx.Text = "OFF";
            // 
            // labelControl_EMGTy
            // 
            this.labelControl_EMGTy.BackColor = System.Drawing.Color.Blue;
            this.labelControl_EMGTy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_EMGTy.Location = new System.Drawing.Point(396, 239);
            this.labelControl_EMGTy.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_EMGTy.Name = "labelControl_EMGTy";
            this.labelControl_EMGTy.Size = new System.Drawing.Size(28, 19);
            this.labelControl_EMGTy.TabIndex = 72;
            this.labelControl_EMGTy.Text = "OFF";
            // 
            // labelControl_ORITy
            // 
            this.labelControl_ORITy.BackColor = System.Drawing.Color.Blue;
            this.labelControl_ORITy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_ORITy.Location = new System.Drawing.Point(329, 240);
            this.labelControl_ORITy.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_ORITy.Name = "labelControl_ORITy";
            this.labelControl_ORITy.Size = new System.Drawing.Size(28, 19);
            this.labelControl_ORITy.TabIndex = 71;
            this.labelControl_ORITy.Text = "OFF";
            // 
            // labelControl_LimtNTy
            // 
            this.labelControl_LimtNTy.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtNTy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtNTy.Location = new System.Drawing.Point(361, 240);
            this.labelControl_LimtNTy.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtNTy.Name = "labelControl_LimtNTy";
            this.labelControl_LimtNTy.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtNTy.TabIndex = 70;
            this.labelControl_LimtNTy.Text = "OFF";
            // 
            // labelControl_LimtPTy
            // 
            this.labelControl_LimtPTy.BackColor = System.Drawing.Color.Blue;
            this.labelControl_LimtPTy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_LimtPTy.Location = new System.Drawing.Point(294, 240);
            this.labelControl_LimtPTy.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_LimtPTy.Name = "labelControl_LimtPTy";
            this.labelControl_LimtPTy.Size = new System.Drawing.Size(28, 19);
            this.labelControl_LimtPTy.TabIndex = 69;
            this.labelControl_LimtPTy.Text = "OFF";
            // 
            // labelControl_AlarmTy
            // 
            this.labelControl_AlarmTy.BackColor = System.Drawing.Color.Blue;
            this.labelControl_AlarmTy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_AlarmTy.Location = new System.Drawing.Point(259, 240);
            this.labelControl_AlarmTy.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl_AlarmTy.Name = "labelControl_AlarmTy";
            this.labelControl_AlarmTy.Size = new System.Drawing.Size(28, 19);
            this.labelControl_AlarmTy.TabIndex = 68;
            this.labelControl_AlarmTy.Text = "OFF";
            // 
            // label_ActPosX
            // 
            this.label_ActPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosX.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosX.Location = new System.Drawing.Point(102, 50);
            this.label_ActPosX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosX.Name = "label_ActPosX";
            this.label_ActPosX.Size = new System.Drawing.Size(72, 16);
            this.label_ActPosX.TabIndex = 73;
            this.label_ActPosX.Text = "00000000";
            // 
            // label_CmdPosX
            // 
            this.label_CmdPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosX.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosX.Location = new System.Drawing.Point(178, 49);
            this.label_CmdPosX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosX.Name = "label_CmdPosX";
            this.label_CmdPosX.Size = new System.Drawing.Size(75, 16);
            this.label_CmdPosX.TabIndex = 74;
            this.label_CmdPosX.Text = "00000000";
            // 
            // label_CmdPosY
            // 
            this.label_CmdPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosY.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosY.Location = new System.Drawing.Point(178, 87);
            this.label_CmdPosY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosY.Name = "label_CmdPosY";
            this.label_CmdPosY.Size = new System.Drawing.Size(75, 16);
            this.label_CmdPosY.TabIndex = 76;
            this.label_CmdPosY.Text = "00000000";
            // 
            // label_ActPosY
            // 
            this.label_ActPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosY.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosY.Location = new System.Drawing.Point(102, 88);
            this.label_ActPosY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosY.Name = "label_ActPosY";
            this.label_ActPosY.Size = new System.Drawing.Size(72, 16);
            this.label_ActPosY.TabIndex = 75;
            this.label_ActPosY.Text = "00000000";
            // 
            // label_CmdPosZ
            // 
            this.label_CmdPosZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosZ.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosZ.Location = new System.Drawing.Point(178, 127);
            this.label_CmdPosZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosZ.Name = "label_CmdPosZ";
            this.label_CmdPosZ.Size = new System.Drawing.Size(75, 16);
            this.label_CmdPosZ.TabIndex = 78;
            this.label_CmdPosZ.Text = "00000000";
            // 
            // label_ActPosZ
            // 
            this.label_ActPosZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosZ.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosZ.Location = new System.Drawing.Point(102, 128);
            this.label_ActPosZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosZ.Name = "label_ActPosZ";
            this.label_ActPosZ.Size = new System.Drawing.Size(72, 16);
            this.label_ActPosZ.TabIndex = 77;
            this.label_ActPosZ.Text = "00000000";
            // 
            // label_CmdPosU
            // 
            this.label_CmdPosU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosU.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosU.Location = new System.Drawing.Point(178, 167);
            this.label_CmdPosU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosU.Name = "label_CmdPosU";
            this.label_CmdPosU.Size = new System.Drawing.Size(75, 16);
            this.label_CmdPosU.TabIndex = 80;
            this.label_CmdPosU.Text = "00000000";
            // 
            // label_ActPosU
            // 
            this.label_ActPosU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosU.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosU.Location = new System.Drawing.Point(102, 168);
            this.label_ActPosU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosU.Name = "label_ActPosU";
            this.label_ActPosU.Size = new System.Drawing.Size(72, 16);
            this.label_ActPosU.TabIndex = 79;
            this.label_ActPosU.Text = "00000000";
            // 
            // label_CmdPosTx
            // 
            this.label_CmdPosTx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosTx.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosTx.Location = new System.Drawing.Point(178, 205);
            this.label_CmdPosTx.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosTx.Name = "label_CmdPosTx";
            this.label_CmdPosTx.Size = new System.Drawing.Size(75, 16);
            this.label_CmdPosTx.TabIndex = 82;
            this.label_CmdPosTx.Text = "00000000";
            // 
            // label_ActPosTx
            // 
            this.label_ActPosTx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosTx.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosTx.Location = new System.Drawing.Point(102, 206);
            this.label_ActPosTx.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosTx.Name = "label_ActPosTx";
            this.label_ActPosTx.Size = new System.Drawing.Size(72, 16);
            this.label_ActPosTx.TabIndex = 81;
            this.label_ActPosTx.Text = "00000000";
            // 
            // label_CmdPosTy
            // 
            this.label_CmdPosTy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CmdPosTy.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CmdPosTy.Location = new System.Drawing.Point(178, 244);
            this.label_CmdPosTy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CmdPosTy.Name = "label_CmdPosTy";
            this.label_CmdPosTy.Size = new System.Drawing.Size(75, 16);
            this.label_CmdPosTy.TabIndex = 84;
            this.label_CmdPosTy.Text = "00000000";
            // 
            // label_ActPosTy
            // 
            this.label_ActPosTy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ActPosTy.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ActPosTy.Location = new System.Drawing.Point(102, 245);
            this.label_ActPosTy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ActPosTy.Name = "label_ActPosTy";
            this.label_ActPosTy.Size = new System.Drawing.Size(72, 16);
            this.label_ActPosTy.TabIndex = 83;
            this.label_ActPosTy.Text = "00000000";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point(102, 23);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 13);
            this.label18.TabIndex = 85;
            this.label18.Text = "实际位置";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(183, 23);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 13);
            this.label19.TabIndex = 86;
            this.label19.Text = "命令位置";
            // 
            // dataGridView_ioInput
            // 
            //this.dataGridView_ioInput.AllowUserToAddRows = false;
            //this.dataGridView_ioInput.AllowUserToDeleteRows = false;
            //this.dataGridView_ioInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //this.dataGridView_ioInput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            //this.IoName,
            //this.IoInputState});
            //this.dataGridView_ioInput.GridColor = System.Drawing.SystemColors.Control;
            //this.dataGridView_ioInput.Location = new System.Drawing.Point(699, 297);
            //this.dataGridView_ioInput.Margin = new System.Windows.Forms.Padding(2);
            //this.dataGridView_ioInput.Name = "dataGridView_ioInput";
            //this.dataGridView_ioInput.ReadOnly = true;
            //this.dataGridView_ioInput.RowHeadersVisible = false;
            //this.dataGridView_ioInput.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //this.dataGridView_ioInput.RowTemplate.Height = 20;
            //this.dataGridView_ioInput.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            //this.dataGridView_ioInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            //this.dataGridView_ioInput.Size = new System.Drawing.Size(20, 7);
            //this.dataGridView_ioInput.TabIndex = 87;
            //this.dataGridView_ioInput.Visible = false;
            //this.dataGridView_ioInput.SelectionChanged += new System.EventHandler(this.OnSelectionDataGridView);
            // 
            // IoName
            // 
            this.IoName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IoName.HeaderText = "IO_IN名称";
            this.IoName.Name = "IoName";
            this.IoName.ReadOnly = true;
            this.IoName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IoName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IoName.Width = 160;
            // 
            // IoInputState
            // 
            this.IoInputState.HeaderText = "状态";
            this.IoInputState.Name = "IoInputState";
            this.IoInputState.ReadOnly = true;
            this.IoInputState.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IoInputState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IoInputState.ToolTipText = "InIo状态";
            this.IoInputState.Width = 40;
            // 
            // dataGridView_IoOutput
            // 
            //this.dataGridView_IoOutput.AllowUserToAddRows = false;
            //this.dataGridView_IoOutput.AllowUserToDeleteRows = false;
            //this.dataGridView_IoOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //this.dataGridView_IoOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            //this.IoOutputNmae,
            //this.IoOutputState,
            //this.IoOutOperate});
            //this.dataGridView_IoOutput.GridColor = System.Drawing.SystemColors.Control;
            //this.dataGridView_IoOutput.Location = new System.Drawing.Point(729, 297);
            //this.dataGridView_IoOutput.Margin = new System.Windows.Forms.Padding(2);
            //this.dataGridView_IoOutput.Name = "dataGridView_IoOutput";
            //this.dataGridView_IoOutput.ReadOnly = true;
            //this.dataGridView_IoOutput.RowHeadersVisible = false;
            //this.dataGridView_IoOutput.RowTemplate.Height = 20;
            //this.dataGridView_IoOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            //this.dataGridView_IoOutput.Size = new System.Drawing.Size(7, 10);
            //this.dataGridView_IoOutput.TabIndex = 88;
            //this.dataGridView_IoOutput.Visible = false;
            //this.dataGridView_IoOutput.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_IoOutput_CellContentClick);
            //this.dataGridView_IoOutput.SelectionChanged += new System.EventHandler(this.OnSelectionDataGridView);
            //// 
            // IoOutputNmae
            // 
            this.IoOutputNmae.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IoOutputNmae.HeaderText = "IO_Out名称";
            this.IoOutputNmae.Name = "IoOutputNmae";
            this.IoOutputNmae.ReadOnly = true;
            this.IoOutputNmae.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IoOutputNmae.Width = 160;
            // 
            // IoOutputState
            // 
            this.IoOutputState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IoOutputState.HeaderText = "状态";
            this.IoOutputState.Name = "IoOutputState";
            this.IoOutputState.ReadOnly = true;
            this.IoOutputState.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IoOutputState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IoOutputState.ToolTipText = "IoOut状态";
            this.IoOutputState.Width = 40;
            // 
            // IoOutOperate
            // 
            this.IoOutOperate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IoOutOperate.HeaderText = "操作";
            this.IoOutOperate.Name = "IoOutOperate";
            this.IoOutOperate.ReadOnly = true;
            this.IoOutOperate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IoOutOperate.Width = 30;
            // 
            // btn_Del
            // 
            this.btn_Del.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_Del.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_Del.Location = new System.Drawing.Point(616, 477);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(66, 40);
            this.btn_Del.TabIndex = 89;
            this.btn_Del.Text = "删除";
            this.btn_Del.UseVisualStyleBackColor = false;
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // comboBox_SelVisionPR
            // 
            this.comboBox_SelVisionPR.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_SelVisionPR.FormattingEnabled = true;
            this.comboBox_SelVisionPR.Location = new System.Drawing.Point(613, 161);
            this.comboBox_SelVisionPR.Name = "comboBox_SelVisionPR";
            this.comboBox_SelVisionPR.Size = new System.Drawing.Size(66, 19);
            this.comboBox_SelVisionPR.TabIndex = 91;
            // 
            // userBtnPanel_Output
            // 
            this.userBtnPanel_Output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userBtnPanel_Output.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userBtnPanel_Output.Location = new System.Drawing.Point(1038, 290);
            this.userBtnPanel_Output.m_nNumPerPage = 50;
            this.userBtnPanel_Output.m_nNumPerRow = 2;
            this.userBtnPanel_Output.m_page = 0;
            this.userBtnPanel_Output.m_splitHigh = 30;
            this.userBtnPanel_Output.m_splitWidth = 150;
            this.userBtnPanel_Output.Margin = new System.Windows.Forms.Padding(1);
            this.userBtnPanel_Output.Name = "userBtnPanel_Output";
            this.userBtnPanel_Output.Size = new System.Drawing.Size(341, 515);
            this.userBtnPanel_Output.TabIndex = 94;
            // 
            // userPanel_Input
            // 
            this.userPanel_Input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userPanel_Input.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userPanel_Input.Location = new System.Drawing.Point(688, 290);
            this.userPanel_Input.m_nNumPerPage = 50;
            this.userPanel_Input.m_nNumPerRow = 2;
            this.userPanel_Input.m_page = 0;
            this.userPanel_Input.m_splitHigh = 30;
            this.userPanel_Input.m_splitWidth = 150;
            this.userPanel_Input.Margin = new System.Windows.Forms.Padding(4);
            this.userPanel_Input.Name = "userPanel_Input";
            this.userPanel_Input.Size = new System.Drawing.Size(345, 515);
            this.userPanel_Input.TabIndex = 93;
            // 
            // roundButton_VisionPrTest
            // 
            this.roundButton_VisionPrTest.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_VisionPrTest.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_VisionPrTest.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_VisionPrTest.FlatAppearance.BorderSize = 0;
            this.roundButton_VisionPrTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_VisionPrTest.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.roundButton_VisionPrTest.ImageHeight = 80;
            this.roundButton_VisionPrTest.ImageWidth = 80;
            this.roundButton_VisionPrTest.Location = new System.Drawing.Point(613, 206);
            this.roundButton_VisionPrTest.Name = "roundButton_VisionPrTest";
            this.roundButton_VisionPrTest.Radius = 24;
            this.roundButton_VisionPrTest.Size = new System.Drawing.Size(66, 23);
            this.roundButton_VisionPrTest.SpliteButtonWidth = 18;
            this.roundButton_VisionPrTest.TabIndex = 92;
            this.roundButton_VisionPrTest.Text = "测试";
            this.roundButton_VisionPrTest.UseVisualStyleBackColor = false;
            this.roundButton_VisionPrTest.Click += new System.EventHandler(this.roundButton_Test_Click);
            // 
            // visionControl1
            // 
            this.visionControl1.ImgHight = 1944;
            this.visionControl1.ImgWidth = 2592;
            this.visionControl1.Location = new System.Drawing.Point(688, 12);
            this.visionControl1.Name = "visionControl1";
            this.visionControl1.Size = new System.Drawing.Size(425, 271);
            this.visionControl1.TabIndex = 90;
            this.visionControl1.TabStop = false;
            // 
            // StationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 835);
            this.Controls.Add(this.userBtnPanel_Output);
            this.Controls.Add(this.userPanel_Input);
            this.Controls.Add(this.roundButton_VisionPrTest);
            this.Controls.Add(this.comboBox_SelVisionPR);
            this.Controls.Add(this.visionControl1);
            this.Controls.Add(this.btn_Del);
            //this.Controls.Add(this.dataGridView_IoOutput);
            //this.Controls.Add(this.dataGridView_ioInput);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label_CmdPosTy);
            this.Controls.Add(this.label_ActPosTy);
            this.Controls.Add(this.label_CmdPosTx);
            this.Controls.Add(this.label_ActPosTx);
            this.Controls.Add(this.label_CmdPosU);
            this.Controls.Add(this.label_ActPosU);
            this.Controls.Add(this.label_CmdPosZ);
            this.Controls.Add(this.label_ActPosZ);
            this.Controls.Add(this.label_CmdPosY);
            this.Controls.Add(this.label_ActPosY);
            this.Controls.Add(this.label_CmdPosX);
            this.Controls.Add(this.label_ActPosX);
            this.Controls.Add(this.labelControl_EMGTy);
            this.Controls.Add(this.labelControl_ORITy);
            this.Controls.Add(this.labelControl_LimtNTy);
            this.Controls.Add(this.labelControl_LimtPTy);
            this.Controls.Add(this.labelControl_AlarmTy);
            this.Controls.Add(this.labelControl_EMGTx);
            this.Controls.Add(this.labelControl_ORITx);
            this.Controls.Add(this.labelControl_LimtNTx);
            this.Controls.Add(this.labelControl_LimtPTx);
            this.Controls.Add(this.labelControl_AlarmTx);
            this.Controls.Add(this.labelControl_EMGU);
            this.Controls.Add(this.labelControl_ORIU);
            this.Controls.Add(this.labelControl_LimtNU);
            this.Controls.Add(this.labelControl_LimtPU);
            this.Controls.Add(this.labelControl_AlarmU);
            this.Controls.Add(this.labelControl_EMGZ);
            this.Controls.Add(this.labelControl_ORIZ);
            this.Controls.Add(this.labelControl_LimtNZ);
            this.Controls.Add(this.labelControl_LimtPZ);
            this.Controls.Add(this.labelControl_AlarmZ);
            this.Controls.Add(this.labelControl_EMGY);
            this.Controls.Add(this.labelControl_ORIY);
            this.Controls.Add(this.labelControl_LimtNY);
            this.Controls.Add(this.labelControl_LimtPY);
            this.Controls.Add(this.labelControl_AlarmY);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelControl_EMGX);
            this.Controls.Add(this.labelControl_ORIX);
            this.Controls.Add(this.labelControl_LimtNX);
            this.Controls.Add(this.labelControl_LimtPX);
            this.Controls.Add(this.labelControl_AlarmX);
            this.Controls.Add(this.button_ServoOnTy);
            this.Controls.Add(this.button_ServoOnTx);
            this.Controls.Add(this.button_homeTy);
            this.Controls.Add(this.button_homeTx);
            this.Controls.Add(this.button_Tynegtive);
            this.Controls.Add(this.button_Typositive);
            this.Controls.Add(this.button_Txnegtive);
            this.Controls.Add(this.button_Txpositive);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.comboBox_SelCamera);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_ContinuousSnap);
            this.Controls.Add(this.button_ServoOnU);
            this.Controls.Add(this.button_ServoOnZ);
            this.Controls.Add(this.button_ServoOnY);
            this.Controls.Add(this.button_homeU);
            this.Controls.Add(this.button_homeZ);
            this.Controls.Add(this.button_homeY);
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
            this.Name = "StationForm";
            this.Text = "StationForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseForm);
            this.Load += new System.EventHandler(this.StationForm_Load);
            this.Shown += new System.EventHandler(this.ShowFirist);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PointInfo)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.dataGridView_ioInput)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.dataGridView_IoOutput)).EndInit();
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
        private System.Windows.Forms.Button button_homeY;
        private System.Windows.Forms.Button button_homeZ;
        private System.Windows.Forms.Button button_homeU;
        private System.Windows.Forms.Button button_ServoOnY;
        private System.Windows.Forms.Button button_ServoOnZ;
        private System.Windows.Forms.Button button_ServoOnU;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Button button_ContinuousSnap;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox_SelCamera;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_Txpositive;
        private System.Windows.Forms.Button button_Txnegtive;
        private System.Windows.Forms.Button button_Typositive;
        private System.Windows.Forms.Button button_Tynegtive;
        private System.Windows.Forms.Button button_homeTx;
        private System.Windows.Forms.Button button_homeTy;
        private System.Windows.Forms.Button button_ServoOnTx;
        private System.Windows.Forms.Button button_ServoOnTy;
        private System.Windows.Forms.Label labelControl_AlarmX;
        private System.Windows.Forms.Label labelControl_LimtPX;
        private System.Windows.Forms.Label labelControl_LimtNX;
       // private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.Label labelControl_EMGX;
        private System.Windows.Forms.Label labelControl_ORIX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelControl_EMGY;
        private System.Windows.Forms.Label labelControl_ORIY;
        private System.Windows.Forms.Label labelControl_LimtNY;
        private System.Windows.Forms.Label labelControl_LimtPY;
        private System.Windows.Forms.Label labelControl_AlarmY;
        private System.Windows.Forms.Label labelControl_EMGZ;
        private System.Windows.Forms.Label labelControl_ORIZ;
        private System.Windows.Forms.Label labelControl_LimtNZ;
        private System.Windows.Forms.Label labelControl_LimtPZ;
        private System.Windows.Forms.Label labelControl_AlarmZ;
        private System.Windows.Forms.Label labelControl_EMGU;
        private System.Windows.Forms.Label labelControl_ORIU;
        private System.Windows.Forms.Label labelControl_LimtNU;
        private System.Windows.Forms.Label labelControl_LimtPU;
        private System.Windows.Forms.Label labelControl_AlarmU;
        private System.Windows.Forms.Label labelControl_EMGTx;
        private System.Windows.Forms.Label labelControl_ORITx;
        private System.Windows.Forms.Label labelControl_LimtNTx;
        private System.Windows.Forms.Label labelControl_LimtPTx;
        private System.Windows.Forms.Label labelControl_AlarmTx;
        private System.Windows.Forms.Label labelControl_EMGTy;
        private System.Windows.Forms.Label labelControl_ORITy;
        private System.Windows.Forms.Label labelControl_LimtNTy;
        private System.Windows.Forms.Label labelControl_LimtPTy;
        private System.Windows.Forms.Label labelControl_AlarmTy;
        private System.Windows.Forms.Label label_ActPosX;
        private System.Windows.Forms.Label label_CmdPosX;
        private System.Windows.Forms.Label label_CmdPosY;
        private System.Windows.Forms.Label label_ActPosY;
        private System.Windows.Forms.Label label_CmdPosZ;
        private System.Windows.Forms.Label label_ActPosZ;
        private System.Windows.Forms.Label label_CmdPosU;
        private System.Windows.Forms.Label label_ActPosU;
        private System.Windows.Forms.Label label_CmdPosTx;
        private System.Windows.Forms.Label label_ActPosTx;
        private System.Windows.Forms.Label label_CmdPosTy;
        private System.Windows.Forms.Label label_ActPosTy;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
      //  private System.Windows.Forms.DataGridView dataGridView_ioInput;
     //   private System.Windows.Forms.DataGridView dataGridView_IoOutput;
        private System.Windows.Forms.Button btn_Del;
        private UserCtrl.VisionControl visionControl1;
        private System.Windows.Forms.ComboBox comboBox_SelVisionPR;
        private AutoFrameUI.RoundButton roundButton_VisionPrTest;
        private System.Windows.Forms.DataGridViewTextBoxColumn IoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn IoInputState;
        private System.Windows.Forms.DataGridViewTextBoxColumn IoOutputNmae;
        private System.Windows.Forms.DataGridViewTextBoxColumn IoOutputState;
        private System.Windows.Forms.DataGridViewButtonColumn IoOutOperate;
        private UserCtrl.UserPanel userPanel_Input;
        private UserCtrl.UserBtnPanel userBtnPanel_Output;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Xpos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ypos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn TxPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn TyPos;
    }
}