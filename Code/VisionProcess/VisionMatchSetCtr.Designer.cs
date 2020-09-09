namespace VisionProcess
{
    partial class VisionMatchSetCtr
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.CreateParam = new System.Windows.Forms.TabPage();
            this.regionOut1 = new VisionProcess.RegionOut();
            this.comboSelPolarity = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox_PyramidHierarchy = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.checkBox_SetOutPoint = new System.Windows.Forms.CheckBox();
            this.roundButton_SetOutPoint = new AutoFrameUI.RoundButton();
            this.textBox_MinSize = new System.Windows.Forms.TextBox();
            this.label_MinSize = new System.Windows.Forms.Label();
            this.textBox_ContrastLow = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_ContrastHigh = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.roundButton_CreateMode = new AutoFrameUI.RoundButton();
            this.textBox_ScaleColMax = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_ScaleColMin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_AngleExtent = new System.Windows.Forms.TextBox();
            this.textBox_StartAngle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MatchParam = new System.Windows.Forms.TabPage();
            this.textBox_PyramidHierarchyHigh = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_PyramidHierarchyLow = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_OverLapArea = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_MatchScore = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_MatchNum = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comBoSelMatch = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.CreateParam.SuspendLayout();
            this.MatchParam.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.CreateParam);
            this.tabControl1.Controls.Add(this.MatchParam);
            this.tabControl1.Location = new System.Drawing.Point(3, 1);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(725, 187);
            this.tabControl1.TabIndex = 0;
            // 
            // CreateParam
            // 
            this.CreateParam.BackColor = System.Drawing.Color.Gold;
            this.CreateParam.Controls.Add(this.comBoSelMatch);
            this.CreateParam.Controls.Add(this.label5);
            this.CreateParam.Controls.Add(this.regionOut1);
            this.CreateParam.Controls.Add(this.comboSelPolarity);
            this.CreateParam.Controls.Add(this.label15);
            this.CreateParam.Controls.Add(this.textBox_PyramidHierarchy);
            this.CreateParam.Controls.Add(this.label14);
            this.CreateParam.Controls.Add(this.checkBox_SetOutPoint);
            this.CreateParam.Controls.Add(this.roundButton_SetOutPoint);
            this.CreateParam.Controls.Add(this.textBox_MinSize);
            this.CreateParam.Controls.Add(this.label_MinSize);
            this.CreateParam.Controls.Add(this.textBox_ContrastLow);
            this.CreateParam.Controls.Add(this.label11);
            this.CreateParam.Controls.Add(this.textBox_ContrastHigh);
            this.CreateParam.Controls.Add(this.label10);
            this.CreateParam.Controls.Add(this.roundButton_CreateMode);
            this.CreateParam.Controls.Add(this.textBox_ScaleColMax);
            this.CreateParam.Controls.Add(this.label4);
            this.CreateParam.Controls.Add(this.textBox_ScaleColMin);
            this.CreateParam.Controls.Add(this.label3);
            this.CreateParam.Controls.Add(this.textBox_AngleExtent);
            this.CreateParam.Controls.Add(this.textBox_StartAngle);
            this.CreateParam.Controls.Add(this.label2);
            this.CreateParam.Controls.Add(this.label1);
            this.CreateParam.Location = new System.Drawing.Point(22, 4);
            this.CreateParam.Name = "CreateParam";
            this.CreateParam.Padding = new System.Windows.Forms.Padding(3);
            this.CreateParam.Size = new System.Drawing.Size(699, 179);
            this.CreateParam.TabIndex = 0;
            this.CreateParam.Text = "创建参数";
            // 
            // regionOut1
            // 
            this.regionOut1.Location = new System.Drawing.Point(412, 59);
            this.regionOut1.Name = "regionOut1";
            this.regionOut1.Size = new System.Drawing.Size(267, 127);
            this.regionOut1.TabIndex = 25;
            // 
            // comboSelPolarity
            // 
            this.comboSelPolarity.FormattingEnabled = true;
            this.comboSelPolarity.Items.AddRange(new object[] {
            "use_polarity",
            "ignore_global_polarity",
            "ignore_local_polarity",
            "ignore_local_polarity",
            "ignore_color_polarity"});
            this.comboSelPolarity.Location = new System.Drawing.Point(303, 77);
            this.comboSelPolarity.Name = "comboSelPolarity";
            this.comboSelPolarity.Size = new System.Drawing.Size(100, 20);
            this.comboSelPolarity.TabIndex = 24;
            this.comboSelPolarity.Text = "use_polarity";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(197, 83);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 12);
            this.label15.TabIndex = 23;
            this.label15.Text = "极性";
            // 
            // textBox_PyramidHierarchy
            // 
            this.textBox_PyramidHierarchy.Location = new System.Drawing.Point(103, 153);
            this.textBox_PyramidHierarchy.Name = "textBox_PyramidHierarchy";
            this.textBox_PyramidHierarchy.Size = new System.Drawing.Size(88, 21);
            this.textBox_PyramidHierarchy.TabIndex = 22;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 159);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 21;
            this.label14.Text = "金字塔层级";
            // 
            // checkBox_SetOutPoint
            // 
            this.checkBox_SetOutPoint.AutoSize = true;
            this.checkBox_SetOutPoint.Location = new System.Drawing.Point(416, 39);
            this.checkBox_SetOutPoint.Name = "checkBox_SetOutPoint";
            this.checkBox_SetOutPoint.Size = new System.Drawing.Size(84, 16);
            this.checkBox_SetOutPoint.TabIndex = 20;
            this.checkBox_SetOutPoint.Text = "设置输出点";
            this.checkBox_SetOutPoint.UseVisualStyleBackColor = true;
            this.checkBox_SetOutPoint.CheckedChanged += new System.EventHandler(this.checkBox_SetOutPoint_CheckedChanged);
            // 
            // roundButton_SetOutPoint
            // 
            this.roundButton_SetOutPoint.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_SetOutPoint.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_SetOutPoint.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_SetOutPoint.FlatAppearance.BorderSize = 0;
            this.roundButton_SetOutPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_SetOutPoint.ImageHeight = 80;
            this.roundButton_SetOutPoint.ImageWidth = 80;
            this.roundButton_SetOutPoint.Location = new System.Drawing.Point(506, 32);
            this.roundButton_SetOutPoint.Name = "roundButton_SetOutPoint";
            this.roundButton_SetOutPoint.Radius = 24;
            this.roundButton_SetOutPoint.Size = new System.Drawing.Size(75, 29);
            this.roundButton_SetOutPoint.SpliteButtonWidth = 18;
            this.roundButton_SetOutPoint.TabIndex = 19;
            this.roundButton_SetOutPoint.Text = "输出点设置";
            this.roundButton_SetOutPoint.UseVisualStyleBackColor = false;
            this.roundButton_SetOutPoint.Click += new System.EventHandler(this.roundButton_SetOutPoint_Click);
            // 
            // textBox_MinSize
            // 
            this.textBox_MinSize.Location = new System.Drawing.Point(481, 9);
            this.textBox_MinSize.Name = "textBox_MinSize";
            this.textBox_MinSize.Size = new System.Drawing.Size(100, 21);
            this.textBox_MinSize.TabIndex = 18;
            // 
            // label_MinSize
            // 
            this.label_MinSize.AutoSize = true;
            this.label_MinSize.Location = new System.Drawing.Point(410, 12);
            this.label_MinSize.Name = "label_MinSize";
            this.label_MinSize.Size = new System.Drawing.Size(53, 12);
            this.label_MinSize.TabIndex = 17;
            this.label_MinSize.Text = "最小尺寸";
            // 
            // textBox_ContrastLow
            // 
            this.textBox_ContrastLow.Location = new System.Drawing.Point(303, 41);
            this.textBox_ContrastLow.Name = "textBox_ContrastLow";
            this.textBox_ContrastLow.Size = new System.Drawing.Size(100, 21);
            this.textBox_ContrastLow.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(197, 46);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 12);
            this.label11.TabIndex = 15;
            this.label11.Text = "对比度(低）";
            // 
            // textBox_ContrastHigh
            // 
            this.textBox_ContrastHigh.Location = new System.Drawing.Point(303, 4);
            this.textBox_ContrastHigh.Name = "textBox_ContrastHigh";
            this.textBox_ContrastHigh.Size = new System.Drawing.Size(100, 21);
            this.textBox_ContrastHigh.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(197, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 12);
            this.label10.TabIndex = 13;
            this.label10.Text = "对比度(高）";
            // 
            // roundButton_CreateMode
            // 
            this.roundButton_CreateMode.BackColor = System.Drawing.Color.Transparent;
            this.roundButton_CreateMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_CreateMode.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.roundButton_CreateMode.FlatAppearance.BorderSize = 0;
            this.roundButton_CreateMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton_CreateMode.ImageHeight = 80;
            this.roundButton_CreateMode.ImageWidth = 80;
            this.roundButton_CreateMode.Location = new System.Drawing.Point(604, 12);
            this.roundButton_CreateMode.Name = "roundButton_CreateMode";
            this.roundButton_CreateMode.Radius = 24;
            this.roundButton_CreateMode.Size = new System.Drawing.Size(75, 29);
            this.roundButton_CreateMode.SpliteButtonWidth = 18;
            this.roundButton_CreateMode.TabIndex = 12;
            this.roundButton_CreateMode.Text = "创建";
            this.roundButton_CreateMode.UseVisualStyleBackColor = false;
            this.roundButton_CreateMode.Click += new System.EventHandler(this.roundButton_CreateMode_Click);
            // 
            // textBox_ScaleColMax
            // 
            this.textBox_ScaleColMax.Location = new System.Drawing.Point(103, 116);
            this.textBox_ScaleColMax.Name = "textBox_ScaleColMax";
            this.textBox_ScaleColMax.Size = new System.Drawing.Size(88, 21);
            this.textBox_ScaleColMax.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "缩放系数(列最大）";
            // 
            // textBox_ScaleColMin
            // 
            this.textBox_ScaleColMin.Location = new System.Drawing.Point(103, 79);
            this.textBox_ScaleColMin.Name = "textBox_ScaleColMin";
            this.textBox_ScaleColMin.Size = new System.Drawing.Size(88, 21);
            this.textBox_ScaleColMin.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "缩放系数(列最小)";
            // 
            // textBox_AngleExtent
            // 
            this.textBox_AngleExtent.Location = new System.Drawing.Point(103, 42);
            this.textBox_AngleExtent.Name = "textBox_AngleExtent";
            this.textBox_AngleExtent.Size = new System.Drawing.Size(88, 21);
            this.textBox_AngleExtent.TabIndex = 3;
            // 
            // textBox_StartAngle
            // 
            this.textBox_StartAngle.Location = new System.Drawing.Point(103, 9);
            this.textBox_StartAngle.Name = "textBox_StartAngle";
            this.textBox_StartAngle.Size = new System.Drawing.Size(88, 21);
            this.textBox_StartAngle.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "角度范围";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始角度";
            // 
            // MatchParam
            // 
            this.MatchParam.BackColor = System.Drawing.Color.Yellow;
            this.MatchParam.Controls.Add(this.textBox_PyramidHierarchyHigh);
            this.MatchParam.Controls.Add(this.label13);
            this.MatchParam.Controls.Add(this.textBox_PyramidHierarchyLow);
            this.MatchParam.Controls.Add(this.label12);
            this.MatchParam.Controls.Add(this.textBox_OverLapArea);
            this.MatchParam.Controls.Add(this.label9);
            this.MatchParam.Controls.Add(this.textBox_MatchScore);
            this.MatchParam.Controls.Add(this.label8);
            this.MatchParam.Controls.Add(this.textBox_MatchNum);
            this.MatchParam.Controls.Add(this.label7);
            this.MatchParam.Location = new System.Drawing.Point(22, 4);
            this.MatchParam.Name = "MatchParam";
            this.MatchParam.Padding = new System.Windows.Forms.Padding(3);
            this.MatchParam.Size = new System.Drawing.Size(699, 179);
            this.MatchParam.TabIndex = 1;
            this.MatchParam.Text = "匹配参数";
            this.MatchParam.Click += new System.EventHandler(this.MatchParam_Click);
            // 
            // textBox_PyramidHierarchyHigh
            // 
            this.textBox_PyramidHierarchyHigh.Location = new System.Drawing.Point(333, 16);
            this.textBox_PyramidHierarchyHigh.Name = "textBox_PyramidHierarchyHigh";
            this.textBox_PyramidHierarchyHigh.Size = new System.Drawing.Size(100, 21);
            this.textBox_PyramidHierarchyHigh.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(227, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 12);
            this.label13.TabIndex = 12;
            this.label13.Text = "金字塔层级（高）";
            // 
            // textBox_PyramidHierarchyLow
            // 
            this.textBox_PyramidHierarchyLow.Location = new System.Drawing.Point(105, 134);
            this.textBox_PyramidHierarchyLow.Name = "textBox_PyramidHierarchyLow";
            this.textBox_PyramidHierarchyLow.Size = new System.Drawing.Size(100, 21);
            this.textBox_PyramidHierarchyLow.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 134);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 12);
            this.label12.TabIndex = 10;
            this.label12.Text = "金字塔层级（低）";
            // 
            // textBox_OverLapArea
            // 
            this.textBox_OverLapArea.Location = new System.Drawing.Point(105, 97);
            this.textBox_OverLapArea.Name = "textBox_OverLapArea";
            this.textBox_OverLapArea.Size = new System.Drawing.Size(100, 21);
            this.textBox_OverLapArea.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 8;
            this.label9.Text = "覆盖面积";
            // 
            // textBox_MatchScore
            // 
            this.textBox_MatchScore.Location = new System.Drawing.Point(105, 52);
            this.textBox_MatchScore.Name = "textBox_MatchScore";
            this.textBox_MatchScore.Size = new System.Drawing.Size(100, 21);
            this.textBox_MatchScore.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "匹配分数";
            // 
            // textBox_MatchNum
            // 
            this.textBox_MatchNum.Location = new System.Drawing.Point(105, 13);
            this.textBox_MatchNum.Name = "textBox_MatchNum";
            this.textBox_MatchNum.Size = new System.Drawing.Size(100, 21);
            this.textBox_MatchNum.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "匹配个数";
            // 
            // comBoSelMatch
            // 
            this.comBoSelMatch.FormattingEnabled = true;
            this.comBoSelMatch.Items.AddRange(new object[] {
            "形状",
            "灰度"});
            this.comBoSelMatch.Location = new System.Drawing.Point(304, 114);
            this.comBoSelMatch.Name = "comBoSelMatch";
            this.comBoSelMatch.Size = new System.Drawing.Size(100, 20);
            this.comBoSelMatch.TabIndex = 27;
            this.comBoSelMatch.Text = "形状";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "类型选择";
            // 
            // VisionMatchSetCtr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "VisionMatchSetCtr";
            this.Size = new System.Drawing.Size(727, 191);
            this.tabControl1.ResumeLayout(false);
            this.CreateParam.ResumeLayout(false);
            this.CreateParam.PerformLayout();
            this.MatchParam.ResumeLayout(false);
            this.MatchParam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage CreateParam;
        private System.Windows.Forms.TabPage MatchParam;
        private System.Windows.Forms.TextBox textBox_AngleExtent;
        private System.Windows.Forms.TextBox textBox_StartAngle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ScaleColMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_ScaleColMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_OverLapArea;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_MatchScore;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_MatchNum;
        private System.Windows.Forms.Label label7;
        private AutoFrameUI.RoundButton roundButton_CreateMode;
        private System.Windows.Forms.TextBox textBox_MinSize;
        private System.Windows.Forms.Label label_MinSize;
        private System.Windows.Forms.TextBox textBox_ContrastLow;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_ContrastHigh;
        private System.Windows.Forms.Label label10;
        private AutoFrameUI.RoundButton roundButton_SetOutPoint;
        private System.Windows.Forms.CheckBox checkBox_SetOutPoint;
        private System.Windows.Forms.TextBox textBox_PyramidHierarchyLow;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_PyramidHierarchy;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_PyramidHierarchyHigh;
        private System.Windows.Forms.ComboBox comboSelPolarity;
        private System.Windows.Forms.Label label15;
        private RegionOut regionOut1;
        private System.Windows.Forms.ComboBox comBoSelMatch;
        private System.Windows.Forms.Label label5;
    }
}
