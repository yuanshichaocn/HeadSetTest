namespace StationDemo
{
    partial class Form_VisionStepBase
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
           // this.hWindowControl_ShowCtrl = new HalconDotNet.HWindowControl();
            this.button_SingleGrab = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // hWindowControl_ShowCtrl
            // 
            //this.hWindowControl_ShowCtrl.BackColor = System.Drawing.Color.Black;
            //this.hWindowControl_ShowCtrl.BorderColor = System.Drawing.Color.Black;
            //this.hWindowControl_ShowCtrl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            //this.hWindowControl_ShowCtrl.Location = new System.Drawing.Point(2, 1);
            //this.hWindowControl_ShowCtrl.Name = "hWindowControl_ShowCtrl";
            //this.hWindowControl_ShowCtrl.Size = new System.Drawing.Size(691, 474);
            //this.hWindowControl_ShowCtrl.TabIndex = 0;
            //this.hWindowControl_ShowCtrl.WindowSize = new System.Drawing.Size(691, 474);
            // 
            // button_SingleGrab
            // 
            this.button_SingleGrab.Location = new System.Drawing.Point(712, 12);
            this.button_SingleGrab.Name = "button_SingleGrab";
            this.button_SingleGrab.Size = new System.Drawing.Size(91, 46);
            this.button_SingleGrab.TabIndex = 1;
            this.button_SingleGrab.Text = "单次拍照";
            this.button_SingleGrab.UseVisualStyleBackColor = true;
            this.button_SingleGrab.Click += new System.EventHandler(this.button_SingleGrab_Click);
            // 
            // Form_VisionStepBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 483);
            this.Controls.Add(this.button_SingleGrab);
            this.Controls.Add(this.hWindowControl_ShowCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_VisionStepBase";
            this.Text = "Form_VisionStepBase";
            this.Load += new System.EventHandler(this.Form_VisionStepBase_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl_ShowCtrl;
        private System.Windows.Forms.Button button_SingleGrab;
    }
}