namespace XYZDispensVision
{
    partial class TraceElemementArc
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
            this.btnDrawCircle = new System.Windows.Forms.Button();
            this.btnDrawStartPoint = new System.Windows.Forms.Button();
            this.btnDrawEndPoint = new System.Windows.Forms.Button();
            this.checkSelDir = new System.Windows.Forms.CheckBox();
            this.checkBoxIsArc = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFiristPonit = new System.Windows.Forms.TabPage();
            this.pointValSetArcStartPoint = new XYZDispensVision.PointValSet();
            this.tabSecondPonit = new System.Windows.Forms.TabPage();
            this.pointValSetArcEndPoint = new XYZDispensVision.PointValSet();
            this.tabCircleCenterPoint = new System.Windows.Forms.TabPage();
            this.pointValSetArcCenterPoint = new XYZDispensVision.PointValSet();
            this.comPartSet1 = new XYZDispensVision.ComPartSet();
            this.tabControl1.SuspendLayout();
            this.tabFiristPonit.SuspendLayout();
            this.tabSecondPonit.SuspendLayout();
            this.tabCircleCenterPoint.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDrawCircle
            // 
            this.btnDrawCircle.Location = new System.Drawing.Point(69, 152);
            this.btnDrawCircle.Name = "btnDrawCircle";
            this.btnDrawCircle.Size = new System.Drawing.Size(143, 35);
            this.btnDrawCircle.TabIndex = 1;
            this.btnDrawCircle.Text = "绘制/记录圆形（心）";
            this.btnDrawCircle.UseVisualStyleBackColor = true;
            this.btnDrawCircle.Click += new System.EventHandler(this.btnDrawCircle_Click);
            // 
            // btnDrawStartPoint
            // 
            this.btnDrawStartPoint.Location = new System.Drawing.Point(0, 193);
            this.btnDrawStartPoint.Name = "btnDrawStartPoint";
            this.btnDrawStartPoint.Size = new System.Drawing.Size(104, 35);
            this.btnDrawStartPoint.TabIndex = 2;
            this.btnDrawStartPoint.Text = "绘制/记录起点";
            this.btnDrawStartPoint.UseVisualStyleBackColor = true;
            this.btnDrawStartPoint.Click += new System.EventHandler(this.btnDrawStartPoint_Click);
            // 
            // btnDrawEndPoint
            // 
            this.btnDrawEndPoint.Location = new System.Drawing.Point(117, 193);
            this.btnDrawEndPoint.Name = "btnDrawEndPoint";
            this.btnDrawEndPoint.Size = new System.Drawing.Size(101, 35);
            this.btnDrawEndPoint.TabIndex = 3;
            this.btnDrawEndPoint.Text = "绘制/记录终点";
            this.btnDrawEndPoint.UseVisualStyleBackColor = true;
            this.btnDrawEndPoint.Click += new System.EventHandler(this.btnDrawEndPoint_Click);
            // 
            // checkSelDir
            // 
            this.checkSelDir.AutoSize = true;
            this.checkSelDir.Location = new System.Drawing.Point(0, 152);
            this.checkSelDir.Name = "checkSelDir";
            this.checkSelDir.Size = new System.Drawing.Size(60, 16);
            this.checkSelDir.TabIndex = 4;
            this.checkSelDir.Text = "顺时针";
            this.checkSelDir.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsArc
            // 
            this.checkBoxIsArc.AutoSize = true;
            this.checkBoxIsArc.Location = new System.Drawing.Point(0, 174);
            this.checkBoxIsArc.Name = "checkBoxIsArc";
            this.checkBoxIsArc.Size = new System.Drawing.Size(48, 16);
            this.checkBoxIsArc.TabIndex = 5;
            this.checkBoxIsArc.Text = "圆弧";
            this.checkBoxIsArc.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabFiristPonit);
            this.tabControl1.Controls.Add(this.tabSecondPonit);
            this.tabControl1.Controls.Add(this.tabCircleCenterPoint);
            this.tabControl1.Location = new System.Drawing.Point(3, 237);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(197, 177);
            this.tabControl1.TabIndex = 27;
            // 
            // tabFiristPonit
            // 
            this.tabFiristPonit.Controls.Add(this.pointValSetArcStartPoint);
            this.tabFiristPonit.Location = new System.Drawing.Point(4, 22);
            this.tabFiristPonit.Name = "tabFiristPonit";
            this.tabFiristPonit.Padding = new System.Windows.Forms.Padding(3);
            this.tabFiristPonit.Size = new System.Drawing.Size(189, 151);
            this.tabFiristPonit.TabIndex = 0;
            this.tabFiristPonit.Text = "起始点";
            this.tabFiristPonit.UseVisualStyleBackColor = true;
            // 
            // pointValSetArcStartPoint
            // 
            this.pointValSetArcStartPoint.dMx = 0D;
            this.pointValSetArcStartPoint.dMy = 0D;
            this.pointValSetArcStartPoint.dMz = 0D;
            this.pointValSetArcStartPoint.dVx = 0D;
            this.pointValSetArcStartPoint.dVy = 0D;
            this.pointValSetArcStartPoint.Location = new System.Drawing.Point(3, 6);
            this.pointValSetArcStartPoint.Name = "pointValSetArcStartPoint";
            this.pointValSetArcStartPoint.Size = new System.Drawing.Size(178, 148);
            this.pointValSetArcStartPoint.TabIndex = 0;
            // 
            // tabSecondPonit
            // 
            this.tabSecondPonit.Controls.Add(this.pointValSetArcEndPoint);
            this.tabSecondPonit.Location = new System.Drawing.Point(4, 22);
            this.tabSecondPonit.Name = "tabSecondPonit";
            this.tabSecondPonit.Padding = new System.Windows.Forms.Padding(3);
            this.tabSecondPonit.Size = new System.Drawing.Size(189, 151);
            this.tabSecondPonit.TabIndex = 1;
            this.tabSecondPonit.Text = "终止点";
            this.tabSecondPonit.UseVisualStyleBackColor = true;
            // 
            // pointValSetArcEndPoint
            // 
            this.pointValSetArcEndPoint.dMx = 0D;
            this.pointValSetArcEndPoint.dMy = 0D;
            this.pointValSetArcEndPoint.dMz = 0D;
            this.pointValSetArcEndPoint.dVx = 0D;
            this.pointValSetArcEndPoint.dVy = 0D;
            this.pointValSetArcEndPoint.Location = new System.Drawing.Point(3, 2);
            this.pointValSetArcEndPoint.Name = "pointValSetArcEndPoint";
            this.pointValSetArcEndPoint.Size = new System.Drawing.Size(178, 154);
            this.pointValSetArcEndPoint.TabIndex = 0;
            // 
            // tabCircleCenterPoint
            // 
            this.tabCircleCenterPoint.Controls.Add(this.pointValSetArcCenterPoint);
            this.tabCircleCenterPoint.Location = new System.Drawing.Point(4, 22);
            this.tabCircleCenterPoint.Name = "tabCircleCenterPoint";
            this.tabCircleCenterPoint.Padding = new System.Windows.Forms.Padding(3);
            this.tabCircleCenterPoint.Size = new System.Drawing.Size(189, 151);
            this.tabCircleCenterPoint.TabIndex = 2;
            this.tabCircleCenterPoint.Text = "圆心点";
            this.tabCircleCenterPoint.UseVisualStyleBackColor = true;
            // 
            // pointValSetArcCenterPoint
            // 
            this.pointValSetArcCenterPoint.dMx = 0D;
            this.pointValSetArcCenterPoint.dMy = 0D;
            this.pointValSetArcCenterPoint.dMz = 0D;
            this.pointValSetArcCenterPoint.dVx = 0D;
            this.pointValSetArcCenterPoint.dVy = 0D;
            this.pointValSetArcCenterPoint.Location = new System.Drawing.Point(5, 3);
            this.pointValSetArcCenterPoint.Name = "pointValSetArcCenterPoint";
            this.pointValSetArcCenterPoint.Size = new System.Drawing.Size(178, 154);
            this.pointValSetArcCenterPoint.TabIndex = 1;
            // 
            // comPartSet1
            // 
            this.comPartSet1.Acc = 0D;
            this.comPartSet1.bIsAllPointMachine = false;
            this.comPartSet1.ItemName = "";
            this.comPartSet1.Location = new System.Drawing.Point(12, 0);
            this.comPartSet1.Name = "comPartSet1";
            this.comPartSet1.Size = new System.Drawing.Size(183, 146);
            this.comPartSet1.TabIndex = 0;
            this.comPartSet1.VelHigh = 0D;
            this.comPartSet1.VelLow = 0D;
            // 
            // TraceElemementArc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.checkBoxIsArc);
            this.Controls.Add(this.checkSelDir);
            this.Controls.Add(this.btnDrawEndPoint);
            this.Controls.Add(this.btnDrawStartPoint);
            this.Controls.Add(this.btnDrawCircle);
            this.Controls.Add(this.comPartSet1);
            this.Name = "TraceElemementArc";
            this.Size = new System.Drawing.Size(221, 440);
            this.Load += new System.EventHandler(this.TraceElemementArc_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabFiristPonit.ResumeLayout(false);
            this.tabSecondPonit.ResumeLayout(false);
            this.tabCircleCenterPoint.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComPartSet comPartSet1;
        private System.Windows.Forms.Button btnDrawCircle;
        private System.Windows.Forms.Button btnDrawStartPoint;
        private System.Windows.Forms.Button btnDrawEndPoint;
        private System.Windows.Forms.CheckBox checkSelDir;
        private System.Windows.Forms.CheckBox checkBoxIsArc;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFiristPonit;
        private PointValSet pointValSetArcStartPoint;
        private System.Windows.Forms.TabPage tabSecondPonit;
        private PointValSet pointValSetArcEndPoint;
        private System.Windows.Forms.TabPage tabCircleCenterPoint;
        private PointValSet pointValSetArcCenterPoint;
    }
}
