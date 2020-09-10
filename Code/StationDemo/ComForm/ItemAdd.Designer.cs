namespace StationDemo
{
    partial class ItemAdd
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
            this.comboBox_SelVisionProcessType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_camSel = new System.Windows.Forms.ComboBox();
            this.roundButton_Add = new AutoFrameUI.RoundButton();
            this.textBox_ExposureTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Gain = new System.Windows.Forms.TextBox();
            this.textBox_ItemName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLightVal = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboBox_SelVisionProcessType
            // 
            this.comboBox_SelVisionProcessType.FormattingEnabled = true;
            this.comboBox_SelVisionProcessType.Location = new System.Drawing.Point(68, 71);
            this.comboBox_SelVisionProcessType.Name = "comboBox_SelVisionProcessType";
            this.comboBox_SelVisionProcessType.Size = new System.Drawing.Size(121, 20);
            this.comboBox_SelVisionProcessType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "处理类型";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "相机选择";
            // 
            // comboBox_camSel
            // 
            this.comboBox_camSel.FormattingEnabled = true;
            this.comboBox_camSel.Location = new System.Drawing.Point(68, 118);
            this.comboBox_camSel.Name = "comboBox_camSel";
            this.comboBox_camSel.Size = new System.Drawing.Size(121, 20);
            this.comboBox_camSel.TabIndex = 2;
            // 
            // roundButton_Add
            // 
            this.roundButton_Add.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_Add.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Add.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Add.FlatAppearance.BorderSize = 0;
            this.roundButton_Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_Add.ImageHeight = 80;
            this.roundButton_Add.ImageWidth = 80;
            this.roundButton_Add.Location = new System.Drawing.Point(116, 213);
            this.roundButton_Add.Name = "roundButton_Add";
            this.roundButton_Add.Radius = 24;
            this.roundButton_Add.Size = new System.Drawing.Size(111, 29);
            this.roundButton_Add.SpliteButtonWidth = 18;
            this.roundButton_Add.TabIndex = 4;
            this.roundButton_Add.Text = "增加";
            this.roundButton_Add.UseVisualStyleBackColor = false;
            this.roundButton_Add.Click += new System.EventHandler(this.roundButton_Add_Click);
            // 
            // textBox_ExposureTime
            // 
            this.textBox_ExposureTime.Location = new System.Drawing.Point(246, 70);
            this.textBox_ExposureTime.Name = "textBox_ExposureTime";
            this.textBox_ExposureTime.Size = new System.Drawing.Size(100, 21);
            this.textBox_ExposureTime.TabIndex = 5;
            this.textBox_ExposureTime.Text = "10000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "曝光";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(210, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "增益";
            // 
            // textBox_Gain
            // 
            this.textBox_Gain.Location = new System.Drawing.Point(246, 117);
            this.textBox_Gain.Name = "textBox_Gain";
            this.textBox_Gain.Size = new System.Drawing.Size(100, 21);
            this.textBox_Gain.TabIndex = 7;
            this.textBox_Gain.Text = "1";
            // 
            // textBox_ItemName
            // 
            this.textBox_ItemName.Location = new System.Drawing.Point(99, 12);
            this.textBox_ItemName.Name = "textBox_ItemName";
            this.textBox_ItemName.Size = new System.Drawing.Size(140, 21);
            this.textBox_ItemName.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "名称";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "光    照";
            // 
            // txtLightVal
            // 
            this.txtLightVal.Location = new System.Drawing.Point(68, 165);
            this.txtLightVal.Name = "txtLightVal";
            this.txtLightVal.Size = new System.Drawing.Size(121, 21);
            this.txtLightVal.TabIndex = 11;
            this.txtLightVal.Text = "1";
            // 
            // ItemAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 254);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLightVal);
            this.Controls.Add(this.textBox_ItemName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_Gain);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_ExposureTime);
            this.Controls.Add(this.roundButton_Add);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_camSel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_SelVisionProcessType);
            this.Name = "ItemAdd";
            this.Text = "ItemAdd";
            this.Load += new System.EventHandler(this.ItemAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_SelVisionProcessType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_camSel;
        private AutoFrameUI.RoundButton roundButton_Add;
        private System.Windows.Forms.TextBox textBox_ExposureTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Gain;
        private System.Windows.Forms.TextBox textBox_ItemName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLightVal;
    }
}