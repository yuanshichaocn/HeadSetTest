namespace XYZDispensVision
{
    partial class FormLightSet
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSelCH = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLightVal = new System.Windows.Forms.TextBox();
            this.btnSure = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "通道：";
            // 
            // comboBoxSelCH
            // 
            this.comboBoxSelCH.FormattingEnabled = true;
            this.comboBoxSelCH.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.comboBoxSelCH.Location = new System.Drawing.Point(73, 23);
            this.comboBoxSelCH.Name = "comboBoxSelCH";
            this.comboBoxSelCH.Size = new System.Drawing.Size(121, 20);
            this.comboBoxSelCH.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "亮度值：";
            // 
            // textBoxLightVal
            // 
            this.textBoxLightVal.Location = new System.Drawing.Point(73, 66);
            this.textBoxLightVal.Name = "textBoxLightVal";
            this.textBoxLightVal.Size = new System.Drawing.Size(121, 21);
            this.textBoxLightVal.TabIndex = 3;
            // 
            // btnSure
            // 
            this.btnSure.BackColor = System.Drawing.Color.Yellow;
            this.btnSure.Location = new System.Drawing.Point(14, 107);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(180, 39);
            this.btnSure.TabIndex = 4;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = false;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // FormLightSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 161);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.textBoxLightVal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxSelCH);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLightSet";
            this.Text = "光源设置";
            this.Load += new System.EventHandler(this.FormLightSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSelCH;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLightVal;
        private System.Windows.Forms.Button btnSure;
    }
}