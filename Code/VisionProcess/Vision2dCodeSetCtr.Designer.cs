namespace VisionProcess
{
    partial class Vision2dCodeSetCtr
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
            this.comboBox_CodeSystem = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.roundButton_Create2dCode = new AutoFrameUI.RoundButton();
            this.comboBox_ContrastTolerance = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_CodeSystem
            // 
            this.comboBox_CodeSystem.FormattingEnabled = true;
            this.comboBox_CodeSystem.Items.AddRange(new object[] {
            "Data Matrix ECC 200",
            "QR Code",
            "Micro QR Code",
            "PDF417",
            "Aztec Code",
            "Micro QR Code"});
            this.comboBox_CodeSystem.Location = new System.Drawing.Point(28, 56);
            this.comboBox_CodeSystem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_CodeSystem.Name = "comboBox_CodeSystem";
            this.comboBox_CodeSystem.Size = new System.Drawing.Size(180, 26);
            this.comboBox_CodeSystem.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "码值";
            // 
            // roundButton_Create2dCode
            // 
            this.roundButton_Create2dCode.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_Create2dCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Create2dCode.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Create2dCode.FlatAppearance.BorderSize = 0;
            this.roundButton_Create2dCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_Create2dCode.ImageHeight = 80;
            this.roundButton_Create2dCode.ImageWidth = 80;
            this.roundButton_Create2dCode.Location = new System.Drawing.Point(290, 117);
            this.roundButton_Create2dCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.roundButton_Create2dCode.Name = "roundButton_Create2dCode";
            this.roundButton_Create2dCode.Radius = 24;
            this.roundButton_Create2dCode.Size = new System.Drawing.Size(152, 51);
            this.roundButton_Create2dCode.SpliteButtonWidth = 18;
            this.roundButton_Create2dCode.TabIndex = 2;
            this.roundButton_Create2dCode.Text = "创建";
            this.roundButton_Create2dCode.UseVisualStyleBackColor = false;
            this.roundButton_Create2dCode.Click += new System.EventHandler(this.roundButton_Create2dCode_Click);
            // 
            // comboBox_ContrastTolerance
            // 
            this.comboBox_ContrastTolerance.FormattingEnabled = true;
            this.comboBox_ContrastTolerance.Items.AddRange(new object[] {
            "high",
            "low"});
            this.comboBox_ContrastTolerance.Location = new System.Drawing.Point(28, 129);
            this.comboBox_ContrastTolerance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_ContrastTolerance.Name = "comboBox_ContrastTolerance";
            this.comboBox_ContrastTolerance.Size = new System.Drawing.Size(180, 26);
            this.comboBox_ContrastTolerance.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "对比度容忍";
            // 
            // Vision2dCodeSetCtr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_ContrastTolerance);
            this.Controls.Add(this.roundButton_Create2dCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_CodeSystem);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Vision2dCodeSetCtr";
            this.Size = new System.Drawing.Size(888, 249);
            this.Load += new System.EventHandler(this.Vision2dCodeSetCtr_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_CodeSystem;
        private System.Windows.Forms.Label label1;
        private AutoFrameUI.RoundButton roundButton_Create2dCode;
        private System.Windows.Forms.ComboBox comboBox_ContrastTolerance;
        private System.Windows.Forms.Label label2;
    }
}
