namespace Log
{
    partial class LogHelper
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
            ShowText = new System.Windows.Forms.RichTextBox();
            SuspendLayout();
            // 
            // ShowText
            // 
            ShowText.Dock = System.Windows.Forms.DockStyle.Fill;
            ShowText.Location = new System.Drawing.Point(0, 0);
            ShowText.Name = "ShowText";
            ShowText.ReadOnly = true;
            ShowText.Size = new System.Drawing.Size(100, 96);
            ShowText.TabIndex = 0;
            ShowText.Text = "";
            ResumeLayout(false);

        }
        #endregion

        private static System.Windows.Forms.RichTextBox ShowText;
    }
}
