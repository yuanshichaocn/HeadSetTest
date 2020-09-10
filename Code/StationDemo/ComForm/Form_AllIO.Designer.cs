namespace StationDemo
{
    partial class Form_AllIO
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
            this.groupBox_InPut = new System.Windows.Forms.GroupBox();
            this.groupBox_OutPut = new System.Windows.Forms.GroupBox();
            this.button_IoInputNextPage = new System.Windows.Forms.Button();
            this.button_IoInputUpPage = new System.Windows.Forms.Button();
            this.button_IoOutUppage = new System.Windows.Forms.Button();
            this.button_IoOutputNextPage = new System.Windows.Forms.Button();
            this.button_IoOutputUpPage = new System.Windows.Forms.Button();
            this.button_IoOutputDownPage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // groupBox_InPut
            // 
            this.groupBox_InPut.Location = new System.Drawing.Point(8, 11);
            this.groupBox_InPut.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_InPut.Name = "groupBox_InPut";
            this.groupBox_InPut.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_InPut.Size = new System.Drawing.Size(535, 799);
            this.groupBox_InPut.TabIndex = 0;
            this.groupBox_InPut.TabStop = false;
            this.groupBox_InPut.Text = "输入IO";
            // 
            // groupBox_OutPut
            // 
            this.groupBox_OutPut.Location = new System.Drawing.Point(547, 11);
            this.groupBox_OutPut.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_OutPut.Name = "groupBox_OutPut";
            this.groupBox_OutPut.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_OutPut.Size = new System.Drawing.Size(634, 799);
            this.groupBox_OutPut.TabIndex = 1;
            this.groupBox_OutPut.TabStop = false;
            this.groupBox_OutPut.Text = "输出IO";
            // 
            // button_IoInputNextPage
            // 
            this.button_IoInputNextPage.Location = new System.Drawing.Point(364, 827);
            this.button_IoInputNextPage.Margin = new System.Windows.Forms.Padding(2);
            this.button_IoInputNextPage.Name = "button_IoInputNextPage";
            this.button_IoInputNextPage.Size = new System.Drawing.Size(66, 23);
            this.button_IoInputNextPage.TabIndex = 2;
            this.button_IoInputNextPage.Text = "下一页";
            this.button_IoInputNextPage.UseVisualStyleBackColor = true;
            this.button_IoInputNextPage.Click += new System.EventHandler(this.button_IoInputNextPage_Click);
            // 
            // button_IoInputUpPage
            // 
            this.button_IoInputUpPage.Location = new System.Drawing.Point(276, 827);
            this.button_IoInputUpPage.Margin = new System.Windows.Forms.Padding(2);
            this.button_IoInputUpPage.Name = "button_IoInputUpPage";
            this.button_IoInputUpPage.Size = new System.Drawing.Size(66, 23);
            this.button_IoInputUpPage.TabIndex = 3;
            this.button_IoInputUpPage.Text = "上一页";
            this.button_IoInputUpPage.UseVisualStyleBackColor = true;
            this.button_IoInputUpPage.Click += new System.EventHandler(this.button_IoInputUpPage_Click);
            // 
            // button_IoOutUppage
            // 
            this.button_IoOutUppage.Location = new System.Drawing.Point(1372, 827);
            this.button_IoOutUppage.Margin = new System.Windows.Forms.Padding(2);
            this.button_IoOutUppage.Name = "button_IoOutUppage";
            this.button_IoOutUppage.Size = new System.Drawing.Size(66, 23);
            this.button_IoOutUppage.TabIndex = 5;
            this.button_IoOutUppage.Text = "上一页";
            this.button_IoOutUppage.UseVisualStyleBackColor = true;
            this.button_IoOutUppage.Click += new System.EventHandler(this.button_IoOutUppage_Click);
            // 
            // button_IoOutputNextPage
            // 
            this.button_IoOutputNextPage.Location = new System.Drawing.Point(1458, 827);
            this.button_IoOutputNextPage.Margin = new System.Windows.Forms.Padding(2);
            this.button_IoOutputNextPage.Name = "button_IoOutputNextPage";
            this.button_IoOutputNextPage.Size = new System.Drawing.Size(66, 23);
            this.button_IoOutputNextPage.TabIndex = 4;
            this.button_IoOutputNextPage.Text = "下一页";
            this.button_IoOutputNextPage.UseVisualStyleBackColor = true;
            this.button_IoOutputNextPage.Click += new System.EventHandler(this.button_IoOutputNextPage_Click);
            // 
            // button_IoOutputUpPage
            // 
            this.button_IoOutputUpPage.Location = new System.Drawing.Point(676, 838);
            this.button_IoOutputUpPage.Margin = new System.Windows.Forms.Padding(2);
            this.button_IoOutputUpPage.Name = "button_IoOutputUpPage";
            this.button_IoOutputUpPage.Size = new System.Drawing.Size(66, 23);
            this.button_IoOutputUpPage.TabIndex = 7;
            this.button_IoOutputUpPage.Text = "上一页";
            this.button_IoOutputUpPage.UseVisualStyleBackColor = true;
            this.button_IoOutputUpPage.Click += new System.EventHandler(this.button_IoOutUppage_Click);
            // 
            // button_IoOutputDownPage
            // 
            this.button_IoOutputDownPage.Location = new System.Drawing.Point(764, 838);
            this.button_IoOutputDownPage.Margin = new System.Windows.Forms.Padding(2);
            this.button_IoOutputDownPage.Name = "button_IoOutputDownPage";
            this.button_IoOutputDownPage.Size = new System.Drawing.Size(66, 23);
            this.button_IoOutputDownPage.TabIndex = 6;
            this.button_IoOutputDownPage.Text = "下一页";
            this.button_IoOutputDownPage.UseVisualStyleBackColor = true;
            this.button_IoOutputDownPage.Click += new System.EventHandler(this.button_IoOutputNextPage_Click);
            // 
            // Form_AllIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 1044);
            this.Controls.Add(this.button_IoOutputUpPage);
            this.Controls.Add(this.button_IoOutputDownPage);
            this.Controls.Add(this.button_IoOutUppage);
            this.Controls.Add(this.button_IoOutputNextPage);
            this.Controls.Add(this.button_IoInputUpPage);
            this.Controls.Add(this.button_IoInputNextPage);
            this.Controls.Add(this.groupBox_OutPut);
            this.Controls.Add(this.groupBox_InPut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form_AllIO";
            this.Text = "Form_AllIO";
            this.Load += new System.EventHandler(this.Form_AllIO_Load);
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_InPut;
        private System.Windows.Forms.GroupBox groupBox_OutPut;
        private System.Windows.Forms.Button button_IoInputNextPage;
        private System.Windows.Forms.Button button_IoInputUpPage;
        private System.Windows.Forms.Button button_IoOutUppage;
        private System.Windows.Forms.Button button_IoOutputNextPage;
        private System.Windows.Forms.Button button_IoOutputUpPage;
        private System.Windows.Forms.Button button_IoOutputDownPage;
    }
}