namespace XYZDispensVision
{
    partial class TraceElementPoint
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
            this.comPartSet1 = new XYZDispensVision.ComPartSet();
            this.pointValSet = new XYZDispensVision.PointValSet();
            this.SuspendLayout();
            // 
            // comPartSet1
            // 
            this.comPartSet1.Acc = 0D;
            this.comPartSet1.bIsAllPointMachine = false;
            this.comPartSet1.ItemName = "";
            this.comPartSet1.Location = new System.Drawing.Point(3, 3);
            this.comPartSet1.Name = "comPartSet1";
            this.comPartSet1.Size = new System.Drawing.Size(183, 146);
            this.comPartSet1.TabIndex = 0;
            this.comPartSet1.VelHigh = 0D;
            this.comPartSet1.VelLow = 0D;
            // 
            // pointValSet
            // 
            this.pointValSet.dMx = 0D;
            this.pointValSet.dMy = 0D;
            this.pointValSet.dMz = 0D;
            this.pointValSet.dVx = 0D;
            this.pointValSet.dVy = 0D;
            this.pointValSet.Location = new System.Drawing.Point(8, 164);
            this.pointValSet.Name = "pointValSet";
            this.pointValSet.Size = new System.Drawing.Size(178, 179);
            this.pointValSet.TabIndex = 1;
            // 
            // TraceElementPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pointValSet);
            this.Controls.Add(this.comPartSet1);
            this.Name = "TraceElementPoint";
            this.Size = new System.Drawing.Size(190, 359);
            this.Load += new System.EventHandler(this.TraceElementPoint_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ComPartSet comPartSet1;
        private PointValSet pointValSet;
    }
}
