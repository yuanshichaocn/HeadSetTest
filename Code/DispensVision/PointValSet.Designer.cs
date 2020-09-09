namespace XYZDispensVision
{
    partial class PointValSet
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
            this.BtnRecordZ = new System.Windows.Forms.Button();
            this.BtnSetXY = new System.Windows.Forms.Button();
            this.labelZ = new System.Windows.Forms.Label();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.labelY = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnRecordZ
            // 
            this.BtnRecordZ.Location = new System.Drawing.Point(3, 32);
            this.BtnRecordZ.Name = "BtnRecordZ";
            this.BtnRecordZ.Size = new System.Drawing.Size(167, 23);
            this.BtnRecordZ.TabIndex = 18;
            this.BtnRecordZ.Text = "记录Z";
            this.BtnRecordZ.UseVisualStyleBackColor = true;
            this.BtnRecordZ.Click += new System.EventHandler(this.BtnRecordZ_Click);
            // 
            // BtnSetXY
            // 
            this.BtnSetXY.Location = new System.Drawing.Point(3, 3);
            this.BtnSetXY.Name = "BtnSetXY";
            this.BtnSetXY.Size = new System.Drawing.Size(167, 23);
            this.BtnSetXY.TabIndex = 17;
            this.BtnSetXY.Text = "设置并记录XY";
            this.BtnSetXY.UseVisualStyleBackColor = true;
            this.BtnSetXY.Click += new System.EventHandler(this.BtnSetXY_Click);
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(1, 117);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(23, 12);
            this.labelZ.TabIndex = 16;
            this.labelZ.Text = "Z：";
            // 
            // txtZ
            // 
            this.txtZ.Location = new System.Drawing.Point(48, 117);
            this.txtZ.Name = "txtZ";
            this.txtZ.Size = new System.Drawing.Size(122, 21);
            this.txtZ.TabIndex = 15;
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(3, 90);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(23, 12);
            this.labelY.TabIndex = 14;
            this.labelY.Text = "Y：";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(3, 64);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(23, 12);
            this.labelX.TabIndex = 13;
            this.labelX.Text = "X：";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(50, 90);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(122, 21);
            this.txtY.TabIndex = 12;
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(48, 61);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(122, 21);
            this.txtX.TabIndex = 11;
            // 
            // PointValSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnRecordZ);
            this.Controls.Add(this.BtnSetXY);
            this.Controls.Add(this.labelZ);
            this.Controls.Add(this.txtZ);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.txtX);
            this.Name = "PointValSet";
            this.Size = new System.Drawing.Size(178, 149);
            this.Load += new System.EventHandler(this.PointValSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnRecordZ;
        private System.Windows.Forms.Button BtnSetXY;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtX;
    }
}
