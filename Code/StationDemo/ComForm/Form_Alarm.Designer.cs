namespace StationDemo
{
    partial class Form_Alarm
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
            this.rightTab_Alarm = new AutoFrameUI.RightTab();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rightTab_Alarm.SuspendLayout();
            this.SuspendLayout();
            // 
            // rightTab_Alarm
            // 
            this.rightTab_Alarm.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.rightTab_Alarm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rightTab_Alarm.Controls.Add(this.tabPage1);
            this.rightTab_Alarm.Controls.Add(this.tabPage2);
            this.rightTab_Alarm.ItemSize = new System.Drawing.Size(25, 100);
            this.rightTab_Alarm.Location = new System.Drawing.Point(8, 2);
            this.rightTab_Alarm.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rightTab_Alarm.Multiline = true;
            this.rightTab_Alarm.Name = "rightTab_Alarm";
            this.rightTab_Alarm.SelectedIndex = 0;
            this.rightTab_Alarm.Size = new System.Drawing.Size(1189, 390);
            this.rightTab_Alarm.TabColor = System.Drawing.SystemColors.Control;
            this.rightTab_Alarm.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Size = new System.Drawing.Size(1081, 382);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage2.Size = new System.Drawing.Size(1081, 329);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form_Alarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 400);
            this.Controls.Add(this.rightTab_Alarm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form_Alarm";
            this.Text = "Form_Alarm";
            this.rightTab_Alarm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFrameUI.RightTab rightTab_Alarm;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}