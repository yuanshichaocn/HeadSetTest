namespace UserCtrl
{
    partial class UserButton
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
            this.button_Operate = new System.Windows.Forms.Button();
            this.label_Name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Operate
            // 
            this.button_Operate.Location = new System.Drawing.Point(0, 2);
            this.button_Operate.Margin = new System.Windows.Forms.Padding(2);
            this.button_Operate.Name = "button_Operate";
            this.button_Operate.Size = new System.Drawing.Size(41, 25);
            this.button_Operate.TabIndex = 0;
            this.button_Operate.Text = "OFF";
            this.button_Operate.UseVisualStyleBackColor = true;
            this.button_Operate.Click += new System.EventHandler(this.button_Operate_Click);
            this.button_Operate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_Operate_MouseDown);
            this.button_Operate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_Operate_MouseUp);
            // 
            // label_Name
            // 
            this.label_Name.Location = new System.Drawing.Point(45, 2);
            this.label_Name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(94, 25);
            this.label_Name.TabIndex = 2;
            this.label_Name.Text = "label_Name";
            // 
            // UserButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_Name);
            this.Controls.Add(this.button_Operate);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserButton";
            this.Size = new System.Drawing.Size(141, 29);
            this.Load += new System.EventHandler(this.UserButton_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Operate;
        private System.Windows.Forms.Label label_Name;
    }
}
