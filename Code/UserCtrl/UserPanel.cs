using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UserCtrl
{
    public partial class UserPanel : UserControl
    {
        public UserPanel()
        {
            InitializeComponent();
        }

        private List<string> m_lebeal = new List<string>();
        public int m_page { set; get; } = 0;
        public int m_nNumPerRow { set; get; } = 3;
        public int m_nNumPerPage { set; get; } = 10;
        public int m_splitHigh { set; get; } = 30;
        public int m_splitWidth { set; get; } = 170;

        private List<UserLabel> m_labelControl_IoInput = new List<UserLabel>();

        public void AddFlag(string strlebeal)
        {
            if (!m_lebeal.Contains(strlebeal))
            {
                m_lebeal.Add(strlebeal);
                m_labelControl_IoInput.Add(new UserLabel());
            }
        }

        public List<string> GetLebealNams()
        {
            return m_lebeal;
        }

        public void SetLebalState(string strkey, bool bState)
        {
            int index = m_lebeal.FindIndex((t) => t == strkey);
            if (index != -1 && index < m_labelControl_IoInput.Count)
            {
                UserLabel tem = m_labelControl_IoInput[index];
                tem.State = bState;
            }
            else
            {
                //  MessageBox.Show("SetLebalState:Btns集合中没有没有：" + strkey, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        new public void Update()
        {
            // m_labelControl_IoInput = new UserLabel[inputNum];
            // m_labelControl_IoOutput = new UserButton[outputNum];
            // Dictionary<string, IOMgr.IoDefine> dicInput = IOMgr.GetInstace().GetInputDic();

            int Row = 0; int splitHigh = m_splitHigh;
            int Col = 0; int splitWidth = m_splitWidth;
            int page = 0;
            int index = 0;
            foreach (var tem in m_lebeal)
            {
                UserLabel labelControl = m_labelControl_IoInput[index++];
                if (Col >= m_nNumPerRow && Row < m_nNumPerPage)
                {
                    Col = 0;
                    Row++;
                }
                if (Row >= m_nNumPerPage)
                {
                    page++;
                    Col = 0;
                    Row = 0;
                }
                labelControl.Name = tem;
                Point loc = new Point(panel1.Location.X + Col * splitWidth + 10, panel1.Location.Y + Row * splitHigh);
                Col++;
                labelControl.AutoSize = true;
                labelControl.Location = loc;
                panel1.Controls.Add(labelControl);
                //m_labelControl_IoInput[index++] = labelControl;
                if (index <= m_nNumPerRow * m_nNumPerPage)
                {
                    labelControl.Visible = true;
                    // labelControl.State = true;
                }
                else
                {
                    labelControl.Visible = false;
                    //  labelControl.State = false;
                }
            }
            if (m_lebeal.Count <= 0)
            {
                this.Visible = false;
                return;
            }
            if (m_lebeal.Count >= m_nNumPerRow)
            {
                panel1.Width = m_labelControl_IoInput[m_nNumPerRow - 1].Width + m_labelControl_IoInput[m_nNumPerRow - 1].Location.X;
            }
            else if (m_lebeal.Count > 0)
            {
                panel1.Width = m_labelControl_IoInput[m_lebeal.Count - 1].Width + m_labelControl_IoInput[m_lebeal.Count - 1].Location.X;
            }
            else
            {
                panel1.Width = 0;
                this.Visible = false;
                return;
            }
            panel1.Height = m_labelControl_IoInput[m_lebeal.Count - 1].Location.Y + m_labelControl_IoInput[m_lebeal.Count - 1].Height;

            GC.Collect();
        }

        private void UserPanel_Load(object sender, EventArgs e)
        {
            panel1.Size = this.Size;
            if (m_nNumPerRow <= 0)
                m_nNumPerRow = 1;
        }

        public int Count
        {
            get => m_lebeal.Count;
        }
    }
}