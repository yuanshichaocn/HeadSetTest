namespace StationDemo
{
    partial class Form_Set
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
            this.rightTab1 = new AutoFrameUI.RightTab();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rightTab1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rightTab1
            // 
            this.rightTab1.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.rightTab1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rightTab1.Controls.Add(this.tabPage1);
            this.rightTab1.Controls.Add(this.tabPage2);
            this.rightTab1.ItemSize = new System.Drawing.Size(25, 70);
            this.rightTab1.Location = new System.Drawing.Point(0, 0);
            this.rightTab1.Margin = new System.Windows.Forms.Padding(0);
            this.rightTab1.Multiline = true;
            this.rightTab1.Name = "rightTab1";
            this.rightTab1.SelectedIndex = 0;
            this.rightTab1.Size = new System.Drawing.Size(1291, 504);
            this.rightTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.rightTab1.TabColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(176)))), ((int)(((byte)(177)))));
            this.rightTab1.TabIndex = 0;
            this.rightTab1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
            this.rightTab1.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.rightTab1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(1213, 496);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(1183, 496);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form_Set
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 451);
            this.Controls.Add(this.rightTab1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Set";
            this.Text = "Form_Set";
            this.Load += new System.EventHandler(this.Form_Set_Load);
            this.rightTab1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AutoFrameUI.RightTab rightTab1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;

        #endregion

        //private System.Windows.Forms.TabControl rightTab1;
        // private AutoFrameUI.RightTab rightTab1;

    }
}