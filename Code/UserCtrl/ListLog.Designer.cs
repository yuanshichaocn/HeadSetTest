namespace UserCtrl
{
    partial class ListLog
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
            this.listBox_ColorLog = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox_ColorLog
            // 
            this.listBox_ColorLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBox_ColorLog.FormattingEnabled = true;
            this.listBox_ColorLog.Location = new System.Drawing.Point(0, 0);
            this.listBox_ColorLog.Name = "listBox_ColorLog";
            this.listBox_ColorLog.Size = new System.Drawing.Size(120, 95);
            this.listBox_ColorLog.TabIndex = 0;
            this.listBox_ColorLog.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawItem);
            // 
            // ListLog
            // 
            this.ItemHeight = 18;
            this.Size = new System.Drawing.Size(120, 94);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_ColorLog;
    }
}
