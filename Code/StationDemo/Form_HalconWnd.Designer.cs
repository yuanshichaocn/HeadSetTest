using HalconDotNet;
//using MvCamCtrl;


namespace StationDemo
{
    partial class Form_HalconWnd
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
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.hWindowControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.hWindowControl1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.hWindowControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(-12, -13);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(397, 365);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(395, 363);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(391, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form_HalconWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 350);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.hWindowControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_HalconWnd";
            this.Load += new System.EventHandler(this.Form_HalconWnd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HWindowControl hWindowControl1;
        private System.Windows.Forms.Button button1;





    }
}