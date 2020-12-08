using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UserCtrl;

namespace VisionProcess
{
    public partial class RegionOut : UserControl
    {
        public RegionOut()
        {
            InitializeComponent();
        }

        private int SelIndex = -1;
        private VisionControl vc = null;
        private List<shapeparam> shapeparams = null;

        public void Flush(VisionControl visionControl, List<shapeparam> listshapelist)
        {
            shapeparams = listshapelist;
            //if (shapeparams == null || shapeparams.Count == 0)
            //    return;
            vc = visionControl;
            dataGridView_ItemAllElement.Rows.Clear();
            foreach (var temp in listshapelist)
            {
                dataGridView_ItemAllElement.Rows.Add("False", temp.name, temp.shapeType.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView_ItemAllElement.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType() == typeof(DataGridViewCheckBoxCell))
            {
                SelIndex = -1;
                for (int i = 0; i < this.dataGridView_ItemAllElement.RowCount; i++)
                {
                    this.dataGridView_ItemAllElement.Rows[i].Cells[e.ColumnIndex].Value = false;
                }
                //if((bool)this.dataGridView_ItemAllElement.Rows[e.RowIndex].Cells[e.ColumnIndex].Value==true)
                SelIndex = e.RowIndex;
            }
        }

        private bool CheckItem()
        {
            if (SelIndex == -1)
                return false;
            if ((bool)this.dataGridView_ItemAllElement.Rows[SelIndex].Cells[0].Value == false)
            {
                MessageBox.Show("区域名称没有，请勾选", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (this.dataGridView_ItemAllElement.Rows[SelIndex].Cells[1].Value == null || this.dataGridView_ItemAllElement.Rows[SelIndex].Cells[1].Value.ToString() == "")
            {
                MessageBox.Show("区域名称为空，请填写", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (this.dataGridView_ItemAllElement.Rows[SelIndex].Cells[2].Value == null || this.dataGridView_ItemAllElement.Rows[SelIndex].Cells[2].Value.ToString() == "")
            {
                MessageBox.Show("区域类型为空，请填写", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public void btnDraw_Click(object sender, EventArgs e)
        {
            if (SelIndex == -1)
                return;

            if (!CheckItem())
                return;
            if (vc == null)
            {
                MessageBox.Show("窗口句柄为空", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            shapeparam shapeparamInstance;
            shapeparamInstance = new shapeparam();
            string strshapetype = dataGridView_ItemAllElement.Rows[SelIndex].Cells[2].Value.ToString();
            ShapeType shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), strshapetype);
            shapeparamInstance.name = dataGridView_ItemAllElement.Rows[SelIndex].Cells[1].Value.ToString();
            shapeparamInstance.shapeType = shapeType;
            switch (shapeType)
            {
                case ShapeType.点:
                    shapeparamInstance.usrshape = new UsrShapePoint();
                    break;

                case ShapeType.圆形:
                    shapeparamInstance.usrshape = new UsrShapeCircle();
                    break;

                case ShapeType.矩形:
                    shapeparamInstance.usrshape = new UsrShapeRect();
                    break;

                case ShapeType.仿射矩形:
                    shapeparamInstance.usrshape = new UsrShapeRect2();
                    break;
            }

            shapeparamInstance.usrshape.bDraw(vc);
            if (shapeparams == null)
                shapeparams = new List<shapeparam>();
            if (SelIndex <= shapeparams.Count - 1)
                shapeparams[SelIndex] = shapeparamInstance;
            if (SelIndex > shapeparams.Count - 1)
                shapeparams.Add(shapeparamInstance);
            if (SelIndex <= shapeparams.Count - 1)
                shapeparams[SelIndex].usrshape.Save();
        }
    }
}