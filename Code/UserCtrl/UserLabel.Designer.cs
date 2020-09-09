namespace UserCtrl
{
    partial class UserLabel
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
            this.label_State = new System.Windows.Forms.Label();
            this.label_Name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_State
            // 
            this.label_State.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label_State.Location = new System.Drawing.Point(1, 0);
            this.label_State.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_State.Name = "label_State";
            this.label_State.Size = new System.Drawing.Size(37, 25);
            this.label_State.TabIndex = 0;
            this.label_State.Text = "OFF";
            // 
            // label_Name
            // 
            this.label_Name.Location = new System.Drawing.Point(42, 0);
            this.label_Name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(111, 25);
            this.label_Name.TabIndex = 1;
            this.label_Name.Text = "Name";
            // 
            // UserLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_Name);
            this.Controls.Add(this.label_State);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserLabel";
            this.Size = new System.Drawing.Size(155, 28);
            this.Load += new System.EventHandler(this.UserLabel_Load);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Label label_State;
        private System.Windows.Forms.Label label_Name;
    }
}
