namespace XYZDispensVision
{
    partial class TraceElementLine
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFiristPonit = new System.Windows.Forms.TabPage();
            this.pointValSetStartPoint = new XYZDispensVision.PointValSet();
            this.tabSecondPonit = new System.Windows.Forms.TabPage();
            this.pointValSetEndPoint = new XYZDispensVision.PointValSet();
            this.comPartSet1 = new XYZDispensVision.ComPartSet();
            this.BtnModifyLine = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabFiristPonit.SuspendLayout();
            this.tabSecondPonit.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabFiristPonit);
            this.tabControl1.Controls.Add(this.tabSecondPonit);
            this.tabControl1.Location = new System.Drawing.Point(3, 192);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(197, 177);
            this.tabControl1.TabIndex = 26;
            // 
            // tabFiristPonit
            // 
            this.tabFiristPonit.Controls.Add(this.pointValSetStartPoint);
            this.tabFiristPonit.Location = new System.Drawing.Point(4, 22);
            this.tabFiristPonit.Name = "tabFiristPonit";
            this.tabFiristPonit.Padding = new System.Windows.Forms.Padding(3);
            this.tabFiristPonit.Size = new System.Drawing.Size(189, 151);
            this.tabFiristPonit.TabIndex = 0;
            this.tabFiristPonit.Text = "起始点";
            this.tabFiristPonit.UseVisualStyleBackColor = true;
            // 
            // pointValSetStartPoint
            // 
            this.pointValSetStartPoint.dMx = 0D;
            this.pointValSetStartPoint.dMy = 0D;
            this.pointValSetStartPoint.dMz = 0D;
            this.pointValSetStartPoint.dVx = 0D;
            this.pointValSetStartPoint.dVy = 0D;
            this.pointValSetStartPoint.Location = new System.Drawing.Point(3, 6);
            this.pointValSetStartPoint.Name = "pointValSetStartPoint";
            this.pointValSetStartPoint.Size = new System.Drawing.Size(178, 148);
            this.pointValSetStartPoint.TabIndex = 0;
            // 
            // tabSecondPonit
            // 
            this.tabSecondPonit.Controls.Add(this.pointValSetEndPoint);
            this.tabSecondPonit.Location = new System.Drawing.Point(4, 22);
            this.tabSecondPonit.Name = "tabSecondPonit";
            this.tabSecondPonit.Padding = new System.Windows.Forms.Padding(3);
            this.tabSecondPonit.Size = new System.Drawing.Size(189, 151);
            this.tabSecondPonit.TabIndex = 1;
            this.tabSecondPonit.Text = "终止点";
            this.tabSecondPonit.UseVisualStyleBackColor = true;
            // 
            // pointValSetEndPoint
            // 
            this.pointValSetEndPoint.dMx = 0D;
            this.pointValSetEndPoint.dMy = 0D;
            this.pointValSetEndPoint.dMz = 0D;
            this.pointValSetEndPoint.dVx = 0D;
            this.pointValSetEndPoint.dVy = 0D;
            this.pointValSetEndPoint.Location = new System.Drawing.Point(3, 6);
            this.pointValSetEndPoint.Name = "pointValSetEndPoint";
            this.pointValSetEndPoint.Size = new System.Drawing.Size(178, 154);
            this.pointValSetEndPoint.TabIndex = 0;
            // 
            // comPartSet1
            // 
            this.comPartSet1.Acc = 0D;
            this.comPartSet1.bIsAllPointMachine = false;
            this.comPartSet1.ItemName = "";
            this.comPartSet1.Location = new System.Drawing.Point(5, 3);
            this.comPartSet1.Name = "comPartSet1";
            this.comPartSet1.Size = new System.Drawing.Size(183, 146);
            this.comPartSet1.TabIndex = 27;
            this.comPartSet1.VelHigh = 0D;
            this.comPartSet1.VelLow = 0D;
            // 
            // BtnModifyLine
            // 
            this.BtnModifyLine.Location = new System.Drawing.Point(31, 155);
            this.BtnModifyLine.Name = "BtnModifyLine";
            this.BtnModifyLine.Size = new System.Drawing.Size(144, 31);
            this.BtnModifyLine.TabIndex = 28;
            this.BtnModifyLine.Text = "修改直线";
            this.BtnModifyLine.UseVisualStyleBackColor = true;
            this.BtnModifyLine.Click += new System.EventHandler(this.BtnModifyLine_Click);
            // 
            // TraceElementLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnModifyLine);
            this.Controls.Add(this.comPartSet1);
            this.Controls.Add(this.tabControl1);
            this.Name = "TraceElementLine";
            this.Size = new System.Drawing.Size(205, 389);
            this.Load += new System.EventHandler(this.TraceElementLine_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabFiristPonit.ResumeLayout(false);
            this.tabSecondPonit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFiristPonit;
        private System.Windows.Forms.TabPage tabSecondPonit;
        private ComPartSet comPartSet1;
        private PointValSet pointValSetStartPoint;
        private PointValSet pointValSetEndPoint;
        private System.Windows.Forms.Button BtnModifyLine;
    }
}
