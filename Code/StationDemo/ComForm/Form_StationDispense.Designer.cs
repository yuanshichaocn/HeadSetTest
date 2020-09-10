
using XYZDispensVision;

namespace StationDemo
{
    partial class Form_StationDispense
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
            this.dispenseCtrl1 = new XYZDispensVision.DispenseCtrl();
            this.SuspendLayout();
            // 
            // dispenseCtrl1
            // 
            this.dispenseCtrl1.AxisU = -1;
            this.dispenseCtrl1.AxisX = 0;
            this.dispenseCtrl1.AxisY = 0;
            this.dispenseCtrl1.AxisZ = 0;
            this.dispenseCtrl1.DispModleName = "Disp";
            this.dispenseCtrl1.IsComTriggerLight = false;
            this.dispenseCtrl1.IsHaveLaset = false;
            this.dispenseCtrl1.IsIoTriggerLight = false;
            this.dispenseCtrl1.Location = new System.Drawing.Point(-4, 1);
            this.dispenseCtrl1.Name = "dispenseCtrl1";
            this.dispenseCtrl1.Size = new System.Drawing.Size(1106, 760);
            this.dispenseCtrl1.TabIndex = 0;
            this.dispenseCtrl1.TriggerLightIoName = null;
            this.dispenseCtrl1.Load += new System.EventHandler(this.dispenseCtrl1_Load);
            // 
            // Form_StationDispense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 761);
            this.Controls.Add(this.dispenseCtrl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_StationDispense";
            this.Text = "Form_StationDispense";
            this.ResumeLayout(false);

        }

        #endregion

        private DispenseCtrl dispenseCtrl1;
    }
}