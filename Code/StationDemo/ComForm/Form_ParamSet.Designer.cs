using AutoFrameUI;

namespace StationDemo
{
    partial class Form_ParamSet
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ParamSet));
            this.dataGridView_Param = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParamType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ParamSetClass = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BtnSave = new AutoFrameUI.RoundButton();
            this.BtnOtherSave = new AutoFrameUI.RoundButton();
            this.treeView_ProdutFile = new System.Windows.Forms.TreeView();
            this.contextMenuStripParamItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnNew = new AutoFrameUI.RoundButton();
            this.BtnDel = new AutoFrameUI.RoundButton();
            this.BtnLoad = new AutoFrameUI.RoundButton();
            this.label_CurrentFile = new System.Windows.Forms.Label();
            this.AddNewClass = new AutoFrameUI.RoundButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Param)).BeginInit();
            this.contextMenuStripParamItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_Param
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Param.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_Param.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_Param.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.ParamType,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.ParamSetClass});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Param.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_Param.Location = new System.Drawing.Point(240, 1);
            this.dataGridView_Param.Name = "dataGridView_Param";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Param.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_Param.RowHeadersVisible = false;
            this.dataGridView_Param.RowTemplate.Height = 23;
            this.dataGridView_Param.Size = new System.Drawing.Size(853, 867);
            this.dataGridView_Param.TabIndex = 10;
            this.dataGridView_Param.Click += new System.EventHandler(this.dataGridView_Param_Click);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.FillWeight = 130F;
            this.Column1.HeaderText = "参数名称";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 200;
            // 
            // ParamType
            // 
            this.ParamType.HeaderText = "参数类型";
            this.ParamType.Items.AddRange(new object[] {
            "boolUnit",
            "intUnit",
            "doubleUnit",
            "stringUnit"});
            this.ParamType.Name = "ParamType";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.FillWeight = 130F;
            this.Column2.HeaderText = "值";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.HeaderText = "最大值";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 80;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.HeaderText = "最小值";
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 80;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column5.HeaderText = "权限";
            this.Column5.Items.AddRange(new object[] {
            "客户操作员",
            "调试工程师",
            "软件工程师",
            "超级管理员"});
            this.Column5.Name = "Column5";
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column5.Width = 80;
            // 
            // ParamSetClass
            // 
            this.ParamSetClass.HeaderText = "分类";
            this.ParamSetClass.Name = "ParamSetClass";
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnSave.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnSave.FlatAppearance.BorderSize = 0;
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.ImageHeight = 80;
            this.BtnSave.ImageWidth = 80;
            this.BtnSave.Location = new System.Drawing.Point(117, 366);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Radius = 24;
            this.BtnSave.Size = new System.Drawing.Size(81, 28);
            this.BtnSave.SpliteButtonWidth = 18;
            this.BtnSave.TabIndex = 11;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnOtherSave
            // 
            this.BtnOtherSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnOtherSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnOtherSave.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnOtherSave.FlatAppearance.BorderSize = 0;
            this.BtnOtherSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOtherSave.ImageHeight = 80;
            this.BtnOtherSave.ImageWidth = 80;
            this.BtnOtherSave.Location = new System.Drawing.Point(8, 366);
            this.BtnOtherSave.Margin = new System.Windows.Forms.Padding(2);
            this.BtnOtherSave.Name = "BtnOtherSave";
            this.BtnOtherSave.Radius = 24;
            this.BtnOtherSave.Size = new System.Drawing.Size(81, 28);
            this.BtnOtherSave.SpliteButtonWidth = 18;
            this.BtnOtherSave.TabIndex = 13;
            this.BtnOtherSave.Text = "另存";
            this.BtnOtherSave.UseVisualStyleBackColor = false;
            this.BtnOtherSave.Click += new System.EventHandler(this.BtnOtherSave_Click);
            // 
            // treeView_ProdutFile
            // 
            this.treeView_ProdutFile.CheckBoxes = true;
            this.treeView_ProdutFile.ContextMenuStrip = this.contextMenuStripParamItem;
            this.treeView_ProdutFile.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView_ProdutFile.LineColor = System.Drawing.Color.Aqua;
            this.treeView_ProdutFile.Location = new System.Drawing.Point(-8, 1);
            this.treeView_ProdutFile.Margin = new System.Windows.Forms.Padding(2);
            this.treeView_ProdutFile.Name = "treeView_ProdutFile";
            this.treeView_ProdutFile.Size = new System.Drawing.Size(243, 297);
            this.treeView_ProdutFile.StateImageList = this.imageList1;
            this.treeView_ProdutFile.TabIndex = 14;
            this.treeView_ProdutFile.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_ProdutFile_AfterSelect);
            // 
            // contextMenuStripParamItem
            // 
            this.contextMenuStripParamItem.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripParamItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.删除ToolStripMenuItem});
            this.contextMenuStripParamItem.Name = "contextMenuStripParamItem";
            this.contextMenuStripParamItem.Size = new System.Drawing.Size(101, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem1.Text = "添加";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Father.jpg");
            this.imageList1.Images.SetKeyName(1, "child.jpg");
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.btnNew.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.ImageHeight = 80;
            this.btnNew.ImageWidth = 80;
            this.btnNew.Location = new System.Drawing.Point(8, 313);
            this.btnNew.Margin = new System.Windows.Forms.Padding(2);
            this.btnNew.Name = "btnNew";
            this.btnNew.Radius = 24;
            this.btnNew.Size = new System.Drawing.Size(81, 28);
            this.btnNew.SpliteButtonWidth = 18;
            this.btnNew.TabIndex = 15;
            this.btnNew.Text = "新建";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // BtnDel
            // 
            this.BtnDel.BackColor = System.Drawing.Color.Transparent;
            this.BtnDel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnDel.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnDel.FlatAppearance.BorderSize = 0;
            this.BtnDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDel.ImageHeight = 80;
            this.BtnDel.ImageWidth = 80;
            this.BtnDel.Location = new System.Drawing.Point(117, 313);
            this.BtnDel.Margin = new System.Windows.Forms.Padding(2);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Radius = 24;
            this.BtnDel.Size = new System.Drawing.Size(81, 28);
            this.BtnDel.SpliteButtonWidth = 18;
            this.BtnDel.TabIndex = 16;
            this.BtnDel.Text = "删除";
            this.BtnDel.UseVisualStyleBackColor = false;
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.BackColor = System.Drawing.Color.Transparent;
            this.BtnLoad.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnLoad.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.BtnLoad.FlatAppearance.BorderSize = 0;
            this.BtnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLoad.ImageHeight = 80;
            this.BtnLoad.ImageWidth = 80;
            this.BtnLoad.Location = new System.Drawing.Point(8, 421);
            this.BtnLoad.Margin = new System.Windows.Forms.Padding(2);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Radius = 24;
            this.BtnLoad.Size = new System.Drawing.Size(81, 28);
            this.BtnLoad.SpliteButtonWidth = 18;
            this.BtnLoad.TabIndex = 17;
            this.BtnLoad.Text = "载入";
            this.BtnLoad.UseVisualStyleBackColor = false;
            this.BtnLoad.Click += new System.EventHandler(this.roundButtonLoad_Click);
            // 
            // label_CurrentFile
            // 
            this.label_CurrentFile.AutoSize = true;
            this.label_CurrentFile.Location = new System.Drawing.Point(17, 527);
            this.label_CurrentFile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CurrentFile.Name = "label_CurrentFile";
            this.label_CurrentFile.Size = new System.Drawing.Size(65, 12);
            this.label_CurrentFile.TabIndex = 18;
            this.label_CurrentFile.Text = "当前产品：";
            // 
            // AddNewClass
            // 
            this.AddNewClass.BackColor = System.Drawing.Color.Transparent;
            this.AddNewClass.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.AddNewClass.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(218)))), ((int)(((byte)(151)))));
            this.AddNewClass.FlatAppearance.BorderSize = 0;
            this.AddNewClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddNewClass.ImageHeight = 80;
            this.AddNewClass.ImageWidth = 80;
            this.AddNewClass.Location = new System.Drawing.Point(117, 421);
            this.AddNewClass.Margin = new System.Windows.Forms.Padding(2);
            this.AddNewClass.Name = "AddNewClass";
            this.AddNewClass.Radius = 24;
            this.AddNewClass.Size = new System.Drawing.Size(81, 28);
            this.AddNewClass.SpliteButtonWidth = 18;
            this.AddNewClass.TabIndex = 19;
            this.AddNewClass.Text = "添加分类";
            this.AddNewClass.UseVisualStyleBackColor = false;
            this.AddNewClass.Click += new System.EventHandler(this.AddNewClass_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1099, 1);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(314, 867);
            this.richTextBox1.TabIndex = 20;
            this.richTextBox1.Text = "";
            // 
            // Form_ParamSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1415, 898);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.AddNewClass);
            this.Controls.Add(this.label_CurrentFile);
            this.Controls.Add(this.BtnLoad);
            this.Controls.Add(this.BtnDel);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.treeView_ProdutFile);
            this.Controls.Add(this.BtnOtherSave);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.dataGridView_Param);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form_ParamSet";
            this.Text = "Form_ParamSet";
            this.Load += new System.EventHandler(this.Form_ParamSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Param)).EndInit();
            this.contextMenuStripParamItem.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Param;
        private AutoFrameUI.RoundButton BtnSave;
        private AutoFrameUI.RoundButton BtnOtherSave;
        private System.Windows.Forms.TreeView treeView_ProdutFile;
        private AutoFrameUI.RoundButton btnNew;
        private AutoFrameUI.RoundButton BtnDel;
        private System.Windows.Forms.ImageList imageList1;
        private AutoFrameUI.RoundButton BtnLoad;
        private System.Windows.Forms.Label label_CurrentFile;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripParamItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn ParamType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column5;
        private System.Windows.Forms.DataGridViewComboBoxColumn ParamSetClass;
        private AutoFrameUI.RoundButton AddNewClass;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}