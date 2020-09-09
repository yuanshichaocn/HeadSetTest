namespace XYZDispensVision
{
    partial class DispTraceElementSet
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
            this.visionControl1 = new UserCtrl.VisionControl();
            this.ReadImg = new System.Windows.Forms.Button();
            this.BtnSanp = new System.Windows.Forms.Button();
            this.ContinuousSnap = new System.Windows.Forms.Button();
            this.btnSnapSave = new System.Windows.Forms.Button();
            this.panelForElementCtrl = new System.Windows.Forms.Panel();
            this.SaveClose = new System.Windows.Forms.Button();
            this.traceElementLine = new XYZDispensVision.TraceElementLine();
            this.traceElemementArc = new XYZDispensVision.TraceElemementArc();
            this.traceElementPoint = new XYZDispensVision.TraceElementPoint();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).BeginInit();
            this.panelForElementCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // visionControl1
            // 
            this.visionControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.visionControl1.ImgHight = 1944;
            this.visionControl1.ImgWidth = 2592;
            this.visionControl1.Location = new System.Drawing.Point(2, 1);
            this.visionControl1.Name = "visionControl1";
            this.visionControl1.Size = new System.Drawing.Size(537, 400);
            this.visionControl1.TabIndex = 0;
            this.visionControl1.TabStop = false;
            // 
            // ReadImg
            // 
            this.ReadImg.Location = new System.Drawing.Point(12, 419);
            this.ReadImg.Name = "ReadImg";
            this.ReadImg.Size = new System.Drawing.Size(82, 47);
            this.ReadImg.TabIndex = 1;
            this.ReadImg.Text = "读取图片";
            this.ReadImg.UseVisualStyleBackColor = true;
            this.ReadImg.Click += new System.EventHandler(this.ReadImg_Click);
            // 
            // BtnSanp
            // 
            this.BtnSanp.Location = new System.Drawing.Point(100, 419);
            this.BtnSanp.Name = "BtnSanp";
            this.BtnSanp.Size = new System.Drawing.Size(82, 47);
            this.BtnSanp.TabIndex = 6;
            this.BtnSanp.Text = "采集图片";
            this.BtnSanp.UseVisualStyleBackColor = true;
            this.BtnSanp.Click += new System.EventHandler(this.BtnSanp_Click);
            // 
            // ContinuousSnap
            // 
            this.ContinuousSnap.Location = new System.Drawing.Point(100, 486);
            this.ContinuousSnap.Name = "ContinuousSnap";
            this.ContinuousSnap.Size = new System.Drawing.Size(82, 47);
            this.ContinuousSnap.TabIndex = 13;
            this.ContinuousSnap.Text = "连续采集";
            this.ContinuousSnap.UseVisualStyleBackColor = true;
            this.ContinuousSnap.Click += new System.EventHandler(this.ContinuousSnap_Click);
            // 
            // btnSnapSave
            // 
            this.btnSnapSave.Location = new System.Drawing.Point(12, 486);
            this.btnSnapSave.Name = "btnSnapSave";
            this.btnSnapSave.Size = new System.Drawing.Size(82, 47);
            this.btnSnapSave.TabIndex = 12;
            this.btnSnapSave.Text = "采集保存";
            this.btnSnapSave.UseVisualStyleBackColor = true;
            this.btnSnapSave.Click += new System.EventHandler(this.btnSnapSave_Click);
            // 
            // panelForElementCtrl
            // 
            this.panelForElementCtrl.Controls.Add(this.traceElementLine);
            this.panelForElementCtrl.Controls.Add(this.traceElemementArc);
            this.panelForElementCtrl.Controls.Add(this.traceElementPoint);
            this.panelForElementCtrl.Location = new System.Drawing.Point(545, 1);
            this.panelForElementCtrl.Name = "panelForElementCtrl";
            this.panelForElementCtrl.Size = new System.Drawing.Size(237, 420);
            this.panelForElementCtrl.TabIndex = 14;
            // 
            // SaveClose
            // 
            this.SaveClose.BackColor = System.Drawing.Color.Fuchsia;
            this.SaveClose.Location = new System.Drawing.Point(558, 455);
            this.SaveClose.Name = "SaveClose";
            this.SaveClose.Size = new System.Drawing.Size(224, 78);
            this.SaveClose.TabIndex = 15;
            this.SaveClose.Text = "保存并关闭";
            this.SaveClose.UseVisualStyleBackColor = false;
            this.SaveClose.Click += new System.EventHandler(this.SaveClose_Click);
            // 
            // traceElementLine
            // 
            this.traceElementLine.Location = new System.Drawing.Point(3, 0);
            this.traceElementLine.Name = "traceElementLine";
            this.traceElementLine.Size = new System.Drawing.Size(221, 420);
            this.traceElementLine.TabIndex = 1;
            this.traceElementLine.Visible = false;
            // 
            // traceElemementArc
            // 
            this.traceElemementArc.bIsArc = false;
            this.traceElemementArc.bIsArcDir = false;
            this.traceElemementArc.Location = new System.Drawing.Point(3, 0);
            this.traceElemementArc.Name = "traceElemementArc";
            this.traceElemementArc.Size = new System.Drawing.Size(221, 420);
            this.traceElemementArc.TabIndex = 0;
            this.traceElemementArc.Visible = false;
            // 
            // traceElementPoint
            // 
            this.traceElementPoint.Location = new System.Drawing.Point(0, 3);
            this.traceElementPoint.Name = "traceElementPoint";
            this.traceElementPoint.Size = new System.Drawing.Size(221, 420);
            this.traceElementPoint.TabIndex = 2;
            this.traceElementPoint.Visible = false;
            // 
            // DispTraceElementSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 545);
            this.Controls.Add(this.SaveClose);
            this.Controls.Add(this.panelForElementCtrl);
            this.Controls.Add(this.ContinuousSnap);
            this.Controls.Add(this.btnSnapSave);
            this.Controls.Add(this.BtnSanp);
            this.Controls.Add(this.ReadImg);
            this.Controls.Add(this.visionControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DispTraceElementSet";
            this.Text = "DispTraceElementSet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DispTraceElementSet_FormClosing);
            this.Load += new System.EventHandler(this.DispTraceElementSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).EndInit();
            this.panelForElementCtrl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserCtrl.VisionControl visionControl1;
        private System.Windows.Forms.Button ReadImg;
        private System.Windows.Forms.Button BtnSanp;
        private System.Windows.Forms.Button ContinuousSnap;
        private System.Windows.Forms.Button btnSnapSave;
        private System.Windows.Forms.Panel panelForElementCtrl;
        private TraceElementPoint traceElementPoint;
        private TraceElementLine traceElementLine;
        private TraceElemementArc traceElemementArc;
        private System.Windows.Forms.Button SaveClose;
    }
}