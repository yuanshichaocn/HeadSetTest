using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserCtrl
{
    public partial class TrayTable : UserControl
    {
        int rowNum = 5;
        [Category("自定义")]
        public int RowNum
        {
            get { return rowNum; }
            set { rowNum = value; InitTrayTable(); }
        } 

        int colNum = 5;
        [Category("自定义")]
        public int ColNum
        {
            get { return colNum; }
            set { colNum = value; InitTrayTable(); }
        }

        Color cellBackColor = Color.Gray;
        [Category("自定义")]
        public Color CellBackColor
        {
            get { return cellBackColor; }
            set { cellBackColor = value; InitTrayTable(); }
        }

        public TrayTable()
        {
            InitializeComponent();
            ControlSizeChanged();
            InitTrayTable();
        }

        public void SetRowColNum(int rowNum/*行数*/, int colNum/*列数*/)
        {
            this.RowNum = rowNum;
            this.ColNum = colNum;
        }

        public void SetCellBackColor(Color cellBackColor)
        {
            this.CellBackColor = cellBackColor;
        }

        private void InitTrayTable()
        {
            listView1.Clear();

            if (RowNum <= 0)
            {
                MessageBox.Show("未设置行数");
                return;
            }

            if (ColNum <= 0)
            {
                MessageBox.Show("未设置列数");
                return;
            }

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.HeaderStyle = ColumnHeaderStyle.None;

            //增加列
            for (int col = 0; col <= ColNum; ++col)//增加多一列
            {
                ColumnHeader colHeader = new ColumnHeader();
                colHeader.TextAlign = HorizontalAlignment.Center;
                listView1.Columns.Add(colHeader);
            }

            //增加行
            for (int row = 0; row < RowNum; ++row)
            {
                ListViewItem lvitem = new ListViewItem((row + 1).ToString());
                for (int col = 0; col <= ColNum; ++col)
                {
                    lvitem.SubItems.Add("");
                }
                listView1.Items.Add(lvitem);
            }

            //设置单元格显示内容
            for (int row = 0; row < RowNum; ++row)
            {
                listView1.Items[row].UseItemStyleForSubItems = false;
                for (int col = 0; col <= ColNum; ++col)// 0 1 2 3 4 5
                {
                    if (col == 0) continue;

                    listView1.Items[row].SubItems[col].Text = ((row * ColNum) + (col)).ToString();
                }
            }
            ControlSizeChanged();
            InitCellBackColor();
        }

        public void SetCellBackColor(int cellNo, Color cellBackColor)
        {
            if(InvokeRequired)
            {
                this.BeginInvoke((new Action(() => SetCellBackColor(cellNo, cellBackColor))));
            }
            else
            {
                try
                {
                    if (cellNo >= 0 && cellNo < RowNum*ColNum)
                    {
                        cellNo++;
                        int col = (cellNo - 1) % ColNum;
                        int row = (cellNo - 1) / ColNum;


                        listView1.Items[row].SubItems[col+1].BackColor = cellBackColor;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }


        public void InitCellBackColor()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((new Action(() => InitCellBackColor())));
            }
            else
            {
                try
                {
                    for (int row = 0; row < RowNum; ++row)
                    {
                        for (int col = 0; col <= ColNum; ++col)
                        {
                            listView1.Items[row].SubItems[col].BackColor = CellBackColor;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            
        }

        private void ControlSizeChanged()
        {
            int avrWidth = listView1.Width / ColNum;
            int avrHeight = listView1.Height / RowNum;
            imageList1.ImageSize = new Size(1, avrHeight);

            for (int col = 0; col < listView1.Columns.Count; ++col)
            {
                if (col == 0)
                    listView1.Columns[col].Width = 0;
                else if (col == listView1.Columns.Count - 1)
                    listView1.Columns[col].Width = avrWidth;
                else
                    listView1.Columns[col].Width = avrWidth;

                listView1.Columns[col].TextAlign = HorizontalAlignment.Center;
            }
        }

        private void TrayTable_SizeChanged(object sender, EventArgs e)
        {
            ControlSizeChanged();
        }
    }
}
