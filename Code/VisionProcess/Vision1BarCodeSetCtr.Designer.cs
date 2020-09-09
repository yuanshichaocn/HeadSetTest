namespace VisionProcess
{
    partial class Vision1BarCodeSetCtr
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
            this.roundButton_Create = new AutoFrameUI.RoundButton();
            this.SuspendLayout();
            // 
            // roundButton_Create
            // 
            this.roundButton_Create.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_Create.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Create.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_Create.FlatAppearance.BorderSize = 0;
            this.roundButton_Create.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_Create.ImageHeight = 80;
            this.roundButton_Create.ImageWidth = 80;
            this.roundButton_Create.Location = new System.Drawing.Point(426, 135);
            this.roundButton_Create.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.roundButton_Create.Name = "roundButton_Create";
            this.roundButton_Create.Radius = 24;
            this.roundButton_Create.Size = new System.Drawing.Size(112, 57);
            this.roundButton_Create.SpliteButtonWidth = 18;
            this.roundButton_Create.TabIndex = 0;
            this.roundButton_Create.Text = "创建";
            this.roundButton_Create.UseVisualStyleBackColor = false;
            this.roundButton_Create.Click += new System.EventHandler(this.roundButton_Create_Click);
            // 
            // Vision1BarCodeSetCtr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.roundButton_Create);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Vision1BarCodeSetCtr";
            this.Size = new System.Drawing.Size(756, 225);
            this.Load += new System.EventHandler(this.Vision1BarCodeSetCtr_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFrameUI.RoundButton roundButton_Create;
    }
}
