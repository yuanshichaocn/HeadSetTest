using System;
using System.Windows.Forms;

namespace StationDemo
{
    public partial class From_OKCancel : Form
    {
        public From_OKCancel()
        {
            InitializeComponent();
        }

        private bool m_OperateContiue;
        private string m_OperateText;

        public bool OperateContiue
        {
            get { return m_OperateContiue; }
        }

        public string m_OperateInfoText
        {
            set
            {
                m_OperateText = value;
                label_OperateInfo.Text = m_OperateText;
                System.Drawing.Point p = label_OperateInfo.Location;
                if (this.Size.Width >= label_OperateInfo.Size.Width)
                    p.X = this.Size.Width / 2 - label_OperateInfo.Size.Width / 2;
                else
                    p.X = 0;
                label_OperateInfo.Location = p;
            }
            get { return m_OperateText; }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            m_OperateContiue = true;
            this.Close();
            this.Dispose();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            m_OperateContiue = false;
            this.Close();
            this.Dispose();
        }

        private void From_OKCancel_Load(object sender, EventArgs e)
        {
        }
    }
}