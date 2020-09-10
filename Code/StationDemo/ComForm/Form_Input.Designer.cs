namespace StationDemo
{
    partial class Form_Input
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
            this.roundButton1 = new AutoFrameUI.RoundButton();
            this.label_InputInfo = new System.Windows.Forms.Label();
            this.textBox_Input = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // roundButton1
            // 
            this.roundButton1.BackColor = System.Drawing.Color.Transparent;
            this.roundButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton1.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton1.FlatAppearance.BorderSize = 0;
            this.roundButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton1.ImageHeight = 80;
            this.roundButton1.ImageWidth = 80;
            this.roundButton1.Location = new System.Drawing.Point(80, 106);
            this.roundButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.roundButton1.Name = "roundButton1";
            this.roundButton1.Radius = 24;
            this.roundButton1.Size = new System.Drawing.Size(101, 32);
            this.roundButton1.SpliteButtonWidth = 18;
            this.roundButton1.TabIndex = 0;
            this.roundButton1.Text = "写入完成";
            this.roundButton1.UseVisualStyleBackColor = false;
            this.roundButton1.Click += new System.EventHandler(this.roundButton1_Click);
            // 
            // label_InputInfo
            // 
            this.label_InputInfo.AutoSize = true;
            this.label_InputInfo.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_InputInfo.Location = new System.Drawing.Point(51, 22);
            this.label_InputInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_InputInfo.Name = "label_InputInfo";
            this.label_InputInfo.Size = new System.Drawing.Size(110, 24);
            this.label_InputInfo.TabIndex = 1;
            this.label_InputInfo.Text = "请输入：";
            // 
            // textBox_Input
            // 
            this.textBox_Input.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Input.Location = new System.Drawing.Point(44, 57);
            this.textBox_Input.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_Input.Name = "textBox_Input";
            this.textBox_Input.Size = new System.Drawing.Size(189, 32);
            this.textBox_Input.TabIndex = 2;
            // 
            // Form_Input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 157);
            this.Controls.Add(this.textBox_Input);
            this.Controls.Add(this.label_InputInfo);
            this.Controls.Add(this.roundButton1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Input";
            this.Text = "输入";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AutoFrameUI.RoundButton roundButton1;
        private System.Windows.Forms.Label label_InputInfo;
        private System.Windows.Forms.TextBox textBox_Input;
    }
}