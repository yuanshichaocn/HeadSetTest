namespace VisionProcess
{
    partial class RegionOut
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
            this.RegionStyple = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RegionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegionSel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridView_ItemAllElement = new System.Windows.Forms.DataGridView();
            this.btnDraw = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ItemAllElement)).BeginInit();
            this.SuspendLayout();
            // 
            // RegionStyple
            // 
            this.RegionStyple.HeaderText = "区域类型";
            this.RegionStyple.Items.AddRange(new object[] {
                "点",
            "矩形",
            "仿射矩形",
            "圆形"});
            this.RegionStyple.Name = "RegionStyple";
            // 
            // RegionName
            // 
            this.RegionName.HeaderText = "区域名称";
            this.RegionName.Name = "RegionName";
            this.RegionName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RegionName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RegionSel
            // 
            this.RegionSel.HeaderText = "";
            this.RegionSel.Name = "RegionSel";
            this.RegionSel.Width = 20;
            // 
            // dataGridView_ItemAllElement
            // 
            this.dataGridView_ItemAllElement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ItemAllElement.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RegionSel,
            this.RegionName,
            this.RegionStyple});
            this.dataGridView_ItemAllElement.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_ItemAllElement.Name = "dataGridView_ItemAllElement";
            this.dataGridView_ItemAllElement.RowHeadersVisible = false;
            this.dataGridView_ItemAllElement.RowTemplate.Height = 23;
            this.dataGridView_ItemAllElement.Size = new System.Drawing.Size(213, 112);
            this.dataGridView_ItemAllElement.TabIndex = 0;
            this.dataGridView_ItemAllElement.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(221, 0);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(44, 38);
            this.btnDraw.TabIndex = 1;
            this.btnDraw.Text = "绘制";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // RegionOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.dataGridView_ItemAllElement);
            this.Name = "RegionOut";
            this.Size = new System.Drawing.Size(266, 115);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ItemAllElement)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewComboBoxColumn RegionStyple;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegionName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RegionSel;
        private System.Windows.Forms.DataGridView dataGridView_ItemAllElement;
        private System.Windows.Forms.Button btnDraw;
    }
}
