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
    public partial class UserBtnPanel : UserControl
    {
        public UserBtnPanel()
        {
            InitializeComponent();
        }
        private List<string> m_Btns = new List<string>();
        public int m_page { set; get; } = 0;
        public int m_nNumPerRow { set; get; } = 3;
        public int m_nNumPerPage { set; get; } = 10;
        public int m_splitHigh { set; get; } = 30;
        public int m_splitWidth { set; get; } = 170;

        List<UserButton> m_labelControl_IoOutput = new List<UserButton>();
        public void AddFlag(string strBtns)
        {
            if(!m_Btns.Contains(strBtns))
            {
                m_Btns.Add(strBtns);
                m_labelControl_IoOutput.Add(new UserButton());
            }
           
        }
        public List<string> GetBtnsNams()
        {
            return m_Btns;
        }
        public int  Count{
            get => m_Btns.Count;
         }
        private object lockobj = new object();

        public void SetBtnState(string strkey, bool bState)
        {
            int index = m_Btns.FindIndex((t) => t == strkey);
            if (index != -1 && index < m_labelControl_IoOutput.Count)
            {
                lock(lockobj)
                {
                    UserButton tem = m_labelControl_IoOutput[index];
                    tem.State = bState;
                }
                
            }
            else
            {
                //MessageBox.Show("SetBtnState:Btns集合中没有没有：" + strkey, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void SetBtnClickEvent(string strkey, UserButton.ClickFunHandler Tem_m_eventClick)
        {
            int index = m_Btns.FindIndex((t) => t == strkey);
            if (index != -1 && index < m_labelControl_IoOutput.Count)
            {
                UserButton tem = m_labelControl_IoOutput[index];
                tem.m_eventClick += Tem_m_eventClick;
            }
            else
            {
               // MessageBox.Show("SetBtnClickEvent:Btns集合中没有没有：" + strkey, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void SetBtnClickDownEvent(string strkey, UserButton.ClickDownFunHandler Tem_m_eventClickDown)
        {
            int index = m_Btns.FindIndex((t) => t == strkey);
            if (index != -1 && index < m_labelControl_IoOutput.Count)
            {
                UserButton tem = m_labelControl_IoOutput[index];
                tem.m_eventClickDown += Tem_m_eventClickDown;
            }
            else
            {
                // MessageBox.Show("SetBtnClickEvent:Btns集合中没有没有：" + strkey, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void SetBtnClickUpEvent(string strkey, UserButton.ClickUpFunHandler Tem_m_eventClickup)
        {
            int index = m_Btns.FindIndex((t) => t == strkey);
            if (index != -1 && index < m_labelControl_IoOutput.Count)
            {
                UserButton tem = m_labelControl_IoOutput[index];
                tem.m_eventClickUp += Tem_m_eventClickup;
            }
            else
            {
                // MessageBox.Show("SetBtnClickEvent:Btns集合中没有没有：" + strkey, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void UserBtnPanel_Load(object sender, EventArgs e)
        {
            panel1.Size = this.Size;
            if (m_nNumPerRow <= 0)
                m_nNumPerRow = 1;
       
        }

        public void Update()
        {
            if (m_Btns.Count <= 0)
            {
                this.Visible = false;
                return;
            }
            int Row = 0; int splitHigh = m_splitHigh;
            int Col = 0; int splitWidth = m_splitWidth;
            int page = 0;
            int index = 0;
            foreach (var tem in m_Btns)
            {
                UserButton BtnControl = m_labelControl_IoOutput[index];
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
                BtnControl.Name = tem;
                Point loc = new Point(panel1.Location.X + Col * splitWidth + 10, panel1.Location.Y + Row * splitHigh);
                Col++;
                //BtnControl.AutoSize = true;
                BtnControl.Location = loc;
                panel1.Controls.Add(BtnControl);
                //m_labelControl_IoOutput[index++] = BtnControl;
                index++;
                if (index <= m_nNumPerRow * m_nNumPerPage)
                {
                    BtnControl.Visible = true;
                    // labelControl.State = true;
                }
                else
                {
                    BtnControl.Visible = false;
                    //  labelControl.State = false;
                }

            }
            if (m_labelControl_IoOutput.Count <= 0)
            {
                this.Visible = false;
                return;
            }
            if (m_labelControl_IoOutput.Count >= m_nNumPerRow)
            {
                panel1.Width = m_labelControl_IoOutput[m_nNumPerRow - 1].Width + m_labelControl_IoOutput[m_nNumPerRow - 1].Location.X;
            }
            else if (m_labelControl_IoOutput.Count > 0)
            {
                panel1.Width = m_labelControl_IoOutput[m_labelControl_IoOutput.Count - 1].Width + m_labelControl_IoOutput[m_labelControl_IoOutput.Count - 1].Location.X;

            }
            else
            {
                panel1.Width = 0;
                this.Visible = false;
                return;
            }
            panel1.Height = m_labelControl_IoOutput[m_labelControl_IoOutput.Count - 1].Location.Y + m_labelControl_IoOutput[m_labelControl_IoOutput.Count - 1].Height;
            GC.Collect();
        }
    }
}
