namespace XYZDispensVision
{
    partial class ComPartSet
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtAcc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVelLow = new System.Windows.Forms.TextBox();
            this.txtVelHigh = new System.Windows.Forms.TextBox();
            this.checkBoxSelMachie = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-2, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 34;
            this.label2.Text = "加速度：";
            // 
            // txtAcc
            // 
            this.txtAcc.Location = new System.Drawing.Point(50, 116);
            this.txtAcc.Name = "txtAcc";
            this.txtAcc.Size = new System.Drawing.Size(122, 21);
            this.txtAcc.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 32;
            this.label3.Text = "低速：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 31;
            this.label4.Text = "高速：";
            // 
            // txtVelLow
            // 
            this.txtVelLow.Location = new System.Drawing.Point(50, 82);
            this.txtVelLow.Name = "txtVelLow";
            this.txtVelLow.Size = new System.Drawing.Size(122, 21);
            this.txtVelLow.TabIndex = 30;
            // 
            // txtVelHigh
            // 
            this.txtVelHigh.Location = new System.Drawing.Point(50, 51);
            this.txtVelHigh.Name = "txtVelHigh";
            this.txtVelHigh.Size = new System.Drawing.Size(122, 21);
            this.txtVelHigh.TabIndex = 29;
            // 
            // checkBoxSelMachie
            // 
            this.checkBoxSelMachie.AutoSize = true;
            this.checkBoxSelMachie.Location = new System.Drawing.Point(50, 33);
            this.checkBoxSelMachie.Name = "checkBoxSelMachie";
            this.checkBoxSelMachie.Size = new System.Drawing.Size(96, 16);
            this.checkBoxSelMachie.TabIndex = 28;
            this.checkBoxSelMachie.Text = "是否机械坐标";
            this.checkBoxSelMachie.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 27;
            this.label1.Text = "名字：";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(50, 6);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(122, 21);
            this.txtName.TabIndex = 26;
            // 
            // ComPartSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAcc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtVelLow);
            this.Controls.Add(this.txtVelHigh);
            this.Controls.Add(this.checkBoxSelMachie);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Name = "ComPartSet";
            this.Size = new System.Drawing.Size(183, 146);
            this.Load += new System.EventHandler(this.ComPartSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAcc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtVelLow;
        private System.Windows.Forms.TextBox txtVelHigh;
        private System.Windows.Forms.CheckBox checkBoxSelMachie;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
    }
}
