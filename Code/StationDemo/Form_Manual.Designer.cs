namespace StationDemo
{
    partial class Form_Manual
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
            this.roundButton_VisionPrTest = new AutoFrameUI.RoundButton();
            this.comboBox_SelVisionPR = new System.Windows.Forms.ComboBox();
            this.comboBox_SelCamera = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button_ContinuousSnap = new System.Windows.Forms.Button();
            this.btnUnLockBtns = new AutoFrameUI.RoundButton();
            this.visionControl1 = new UserCtrl.VisionControl();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // roundButton_VisionPrTest
            // 
            this.roundButton_VisionPrTest.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_VisionPrTest.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_VisionPrTest.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_VisionPrTest.FlatAppearance.BorderSize = 0;
            this.roundButton_VisionPrTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_VisionPrTest.ImageHeight = 80;
            this.roundButton_VisionPrTest.ImageWidth = 80;
            this.roundButton_VisionPrTest.Location = new System.Drawing.Point(661, 54);
            this.roundButton_VisionPrTest.Margin = new System.Windows.Forms.Padding(2);
            this.roundButton_VisionPrTest.Name = "roundButton_VisionPrTest";
            this.roundButton_VisionPrTest.Radius = 24;
            this.roundButton_VisionPrTest.Size = new System.Drawing.Size(61, 25);
            this.roundButton_VisionPrTest.SpliteButtonWidth = 18;
            this.roundButton_VisionPrTest.TabIndex = 156;
            this.roundButton_VisionPrTest.Text = "测试";
            this.roundButton_VisionPrTest.UseVisualStyleBackColor = false;
            this.roundButton_VisionPrTest.Click += new System.EventHandler(this.roundButton_VisionPrTest_Click);
            // 
            // comboBox_SelVisionPR
            // 
            this.comboBox_SelVisionPR.FormattingEnabled = true;
            this.comboBox_SelVisionPR.Location = new System.Drawing.Point(661, 7);
            this.comboBox_SelVisionPR.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_SelVisionPR.Name = "comboBox_SelVisionPR";
            this.comboBox_SelVisionPR.Size = new System.Drawing.Size(62, 20);
            this.comboBox_SelVisionPR.TabIndex = 155;
            // 
            // comboBox_SelCamera
            // 
            this.comboBox_SelCamera.FormattingEnabled = true;
            this.comboBox_SelCamera.Location = new System.Drawing.Point(579, 7);
            this.comboBox_SelCamera.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_SelCamera.Name = "comboBox_SelCamera";
            this.comboBox_SelCamera.Size = new System.Drawing.Size(62, 20);
            this.comboBox_SelCamera.TabIndex = 154;
            this.comboBox_SelCamera.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelCamera_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Location = new System.Drawing.Point(580, 67);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 32);
            this.button2.TabIndex = 153;
            this.button2.Text = "单次采集";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_ContinuousSnap
            // 
            this.button_ContinuousSnap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_ContinuousSnap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_ContinuousSnap.Location = new System.Drawing.Point(579, 31);
            this.button_ContinuousSnap.Margin = new System.Windows.Forms.Padding(2);
            this.button_ContinuousSnap.Name = "button_ContinuousSnap";
            this.button_ContinuousSnap.Size = new System.Drawing.Size(61, 32);
            this.button_ContinuousSnap.TabIndex = 152;
            this.button_ContinuousSnap.Text = "连续采集";
            this.button_ContinuousSnap.UseVisualStyleBackColor = false;
            this.button_ContinuousSnap.Click += new System.EventHandler(this.button_ContinuousSnap_Click);
            // 
            // btnUnLockBtns
            // 
            this.btnUnLockBtns.BackColor = System.Drawing.Color.Transparent;
            this.btnUnLockBtns.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.btnUnLockBtns.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.btnUnLockBtns.FlatAppearance.BorderSize = 0;
            this.btnUnLockBtns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnLockBtns.ImageHeight = 80;
            this.btnUnLockBtns.ImageWidth = 80;
            this.btnUnLockBtns.Location = new System.Drawing.Point(470, 3);
            this.btnUnLockBtns.Margin = new System.Windows.Forms.Padding(2);
            this.btnUnLockBtns.Name = "btnUnLockBtns";
            this.btnUnLockBtns.Radius = 24;
            this.btnUnLockBtns.Size = new System.Drawing.Size(58, 26);
            this.btnUnLockBtns.SpliteButtonWidth = 18;
            this.btnUnLockBtns.TabIndex = 170;
            this.btnUnLockBtns.Text = "按钮解锁";
            this.btnUnLockBtns.UseVisualStyleBackColor = false;
            this.btnUnLockBtns.Click += new System.EventHandler(this.btnUnLockBtns_Click);
            // 
            // visionControl1
            // 
            this.visionControl1.ImgHight = 1944;
            this.visionControl1.ImgWidth = 2592;
            this.visionControl1.Location = new System.Drawing.Point(951, 7);
            this.visionControl1.Name = "visionControl1";
            this.visionControl1.Size = new System.Drawing.Size(135, 109);
            this.visionControl1.TabIndex = 151;
            this.visionControl1.TabStop = false;
            // 
            // Form_Manual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 871);
            this.Controls.Add(this.btnUnLockBtns);
            this.Controls.Add(this.roundButton_VisionPrTest);
            this.Controls.Add(this.comboBox_SelVisionPR);
            this.Controls.Add(this.comboBox_SelCamera);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_ContinuousSnap);
            this.Controls.Add(this.visionControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Manual";
            this.Text = "Form_Manual";
            this.Load += new System.EventHandler(this.Form_Manual_Load);
     
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private UserCtrl.VisionControl visionControl1;
        private AutoFrameUI.RoundButton roundButton_VisionPrTest;
        private System.Windows.Forms.ComboBox comboBox_SelVisionPR;
        private System.Windows.Forms.ComboBox comboBox_SelCamera;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_ContinuousSnap;
        private AutoFrameUI.RoundButton btnUnLockBtns;
    }
}