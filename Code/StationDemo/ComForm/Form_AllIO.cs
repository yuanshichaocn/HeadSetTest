using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutoFrameUI;
using BaseDll;
using CommonTools;
using MotionIoLib;
using UserCtrl;

namespace StationDemo
{
    public partial class Form_AllIO : Form
    {
        public Form_AllIO()
        {
            InitializeComponent();
        }
        int m_page = 0;
        //每行的列数
        int m_nNumPerRow = 3;
        //每页的行数
        int m_nNumPerPage = 20;
        int nCtrlWidth = 140;
        int splitHigh = 30;
         int splitWidth = 150;

        UserLabel[] m_labelControl_IoInput;
        UserButton[] m_labelControl_IoOutput;

        public void ChangedUserRight(User CurrentUser)
        {
            if (InvokeRequired)
                this.BeginInvoke(new Action(() => ChangedUserRight(CurrentUser)));
            else
            {


                bool bEnable = true;
                if ((int)CurrentUser._userRight >= (int)UserRight.调试工程师)
                {
                    bEnable = true;
                    foreach (var temp in this.Controls)
                    {
                        ((Control)temp).Enabled = bEnable;
                    }

                }
                else
                {
                    bEnable = false;

                    foreach (var temp in this.Controls)
                    {
                        ((Control)temp).Enabled = bEnable;
                    }

                }

            }

        }


        private void Form_AllIO_Load(object sender, EventArgs e)
        {
            int inputNum = IOMgr.GetInstace().GetIoInputNum();
            int outputNum = IOMgr.GetInstace().GetIoOutputNum();

            sys.g_eventRightChanged += ChangedUserRight;
            sys.g_User = sys.g_User;
            m_labelControl_IoInput = new UserLabel[inputNum];
            m_labelControl_IoOutput= new UserButton[outputNum];
            Dictionary<string ,IOMgr.IoDefine> dicInput= IOMgr.GetInstace().GetInputDic();
            int Row = 0;
            int Col = 0;
            int page = 0;
            int index = 0;
            foreach(var tem in  dicInput)
            {
                UserLabel labelControl = new UserLabel();
                if (Col >= m_nNumPerRow && Row < m_nNumPerPage)
                {
                    Col = 0;
                    Row++;
                }
                if (Row >= m_nNumPerPage )
                {
                    page++;
                    Col = 0;
                    Row = 0;
                }
                labelControl.Name = tem.Key;
                Point loc = new Point(groupBox_InPut.Location.X + Col * splitWidth + 10, groupBox_InPut.Location.Y + Row * splitHigh );
                Col++;
                labelControl.Width = nCtrlWidth;
               // labelControl.AutoSize = true;
                labelControl.Location = loc;
                groupBox_InPut.Controls.Add(labelControl);
                m_labelControl_IoInput[index++] = labelControl;
                if(index <= m_nNumPerRow * m_nNumPerPage)
                {
                    labelControl.Visible = true;
                    labelControl.State = true;
                }
                else
                {
                    labelControl.Visible = false;
                    labelControl.State = false;
                }
                
            }

             Row = 0;  Col = 0;  page = 0; index = 0;
            Dictionary<string, IOMgr.IoDefine> dicOutput = IOMgr.GetInstace().GetOutputDic();
            foreach (var tem in dicOutput)
            {
                UserButton labelControl = new UserButton();
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
              
                labelControl.Name = tem.Key;
                Point loc = new Point(0 + Col * splitWidth + 10, groupBox_OutPut.Location.Y + Row * splitHigh);
                Col++;
                labelControl.Width = nCtrlWidth;
                // labelControl.AutoSize = true;
                labelControl.Location = loc;
                groupBox_OutPut.Controls.Add(labelControl);
                m_labelControl_IoOutput[index++] = labelControl;
                labelControl.m_eventClick += OutIoWhenClickBtn;
                if (index <= m_nNumPerRow * m_nNumPerPage)
                {
                  
                    labelControl.Visible = true;
                    labelControl.Text = tem.Key;
                    labelControl.State = true;
                }
                else
                {
                    labelControl.Visible = false;
                    labelControl.Text = tem.Key;
                }
            }   
        }
        bool teststate = false;
        public void UpDataAllIo()
        {
            Dictionary<string, IOMgr.IoDefine> dicInput = IOMgr.GetInstace().GetInputDic();
            int indexIn = 0;
            foreach (var tem in dicInput)
                m_labelControl_IoInput[indexIn++].State= IOMgr.GetInstace().ReadIoInBit(tem.Key);

            Dictionary<string, IOMgr.IoDefine> dicOutput = IOMgr.GetInstace().GetOutputDic();
            int indexOut = 0;
            foreach (var tem in dicOutput)
                m_labelControl_IoOutput[indexOut++].State= IOMgr.GetInstace().ReadIoOutBit(tem.Key);
            
        }

        private void button_IoInputNextPage_Click(object sender, EventArgs e)
        {
            
            int pageNum = m_labelControl_IoInput.Length / (m_nNumPerPage * m_nNumPerRow);
            pageNum=(m_labelControl_IoInput.Length % (m_nNumPerPage * m_nNumPerRow)) > 0 ? pageNum+1 :  pageNum;
            int nextPage = m_page >= pageNum ? m_page : m_page + 1;
            if (m_page >= pageNum-1)
                return;
            for (int index= (m_nNumPerPage * m_nNumPerRow) *m_page; index < (m_nNumPerPage * m_nNumPerRow) * m_page + (m_nNumPerPage * m_nNumPerRow); index++)
            {
                if (index <=  m_labelControl_IoInput.Length - 1)
                    m_labelControl_IoInput[index].Visible = false;
            }
            for (int index = (m_nNumPerPage * m_nNumPerRow) * nextPage; index < (m_nNumPerPage * m_nNumPerRow) * nextPage + (m_nNumPerPage * m_nNumPerRow); index++)
            {
                if (index <= m_labelControl_IoInput.Length - 1)
                {
                    m_labelControl_IoInput[index].Visible = true;
                }
                    
            }
            m_page = m_page >= pageNum ? m_page : m_page + 1;
        }

        private void button_IoInputUpPage_Click(object sender, EventArgs e)
        {
            int pageNum = m_labelControl_IoInput.Length / (m_nNumPerPage * m_nNumPerRow);
            pageNum = (m_labelControl_IoInput.Length % (m_nNumPerPage * m_nNumPerRow)) > 0 ? pageNum + 1 : pageNum;
            int upPage = m_page <= 0 ? m_page : m_page -1;
            if (m_page <= 0)
                return;
            for (int index = (m_nNumPerPage * m_nNumPerRow) * m_page; index < (m_nNumPerPage * m_nNumPerRow) * m_page + (m_nNumPerPage * m_nNumPerRow); index++)
            {
                if (index <= m_labelControl_IoInput.Length - 1)
                    m_labelControl_IoInput[index].Visible = false;
            }
            for (int index = (m_nNumPerPage * m_nNumPerRow) * upPage; index < (m_nNumPerPage * m_nNumPerRow) * upPage + (m_nNumPerPage * m_nNumPerRow); index++)
            {
                if (index <= m_labelControl_IoInput.Length - 1)
                    m_labelControl_IoInput[index].Visible = true;
            }
            m_page = m_page <=0 ? m_page : m_page - 1;
        }
        int m_Outpage = 0;
        private void button_IoOutputNextPage_Click(object sender, EventArgs e)
        {
            int pageNum = m_labelControl_IoOutput.Length / (m_nNumPerPage * m_nNumPerRow);
            pageNum = (m_labelControl_IoOutput.Length % (m_nNumPerPage * m_nNumPerRow)) > 0 ? pageNum + 1 : pageNum;
            int nextPage = m_Outpage >= pageNum ? m_Outpage : m_Outpage + 1;
            if (m_Outpage >= pageNum - 1)
                return;
            for (int index = (m_nNumPerPage * m_nNumPerRow) * m_Outpage; index < (m_nNumPerPage * m_nNumPerRow) * m_Outpage + (m_nNumPerPage * m_nNumPerRow); index++)
            {
                if (index <= m_labelControl_IoOutput.Length - 1)
                    m_labelControl_IoOutput[index].Visible = false;
            }
            for (int index = (m_nNumPerPage * m_nNumPerRow) * nextPage; index < (m_nNumPerPage * m_nNumPerRow) * nextPage + (m_nNumPerPage * m_nNumPerRow); index++)
            {
                if (index <= m_labelControl_IoOutput.Length - 1)
                {
                    m_labelControl_IoOutput[index].Visible = true;
                }
            }
            m_Outpage = m_Outpage >= pageNum ? m_Outpage : m_Outpage + 1;
        }

        private void button_IoOutUppage_Click(object sender, EventArgs e)
        {
            int pageNum = m_labelControl_IoOutput.Length / (m_nNumPerPage * m_nNumPerRow);
            pageNum = (m_labelControl_IoOutput.Length % (m_nNumPerPage * m_nNumPerRow)) > 0 ? pageNum + 1 : pageNum;
            int upPage = m_Outpage <= 0 ? m_Outpage : m_Outpage - 1;
            if (m_Outpage <= 0)
                return;
            for (int index = (m_nNumPerPage * m_nNumPerRow) * m_Outpage; index < (m_nNumPerPage * m_nNumPerRow) * m_Outpage + (m_nNumPerPage * m_nNumPerRow); index++)
            {
                if (index <= m_labelControl_IoOutput.Length - 1)
                    m_labelControl_IoOutput[index].Visible = false;
            }
            for (int index = (m_nNumPerPage * m_nNumPerRow) * upPage; index < (m_nNumPerPage * m_nNumPerRow) * upPage + (m_nNumPerPage * m_nNumPerRow); index++)
            {
                if (index <= m_labelControl_IoOutput.Length - 1)
                    m_labelControl_IoOutput[index].Visible = true;
            }
            m_Outpage = m_Outpage <= 0 ? m_Outpage : m_Outpage - 1;
        }
        public void OutIoWhenClickBtn(string str)
        {
            bool bState = IOMgr.GetInstace().ReadIoOutBit(str);
            IOMgr.GetInstace().WriteIoBit(str,!bState);
        }
        public void ChangedIoInState(int index,bool bStateCurrent )
        {
           
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ChangedIoInState(index, bStateCurrent)));
            }
            else
                m_labelControl_IoInput[index].State = bStateCurrent;
        }
        public void ChangedIoOutState(int index, bool bStateCurrent)
        { 
            if(InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ChangedIoOutState(index, bStateCurrent)));
            }
            else
                m_labelControl_IoOutput[index].State = bStateCurrent;
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if(Visible==true)
            {
                UpDataAllIo();
                IOMgr.GetInstace().m_eventIoInputChanage += ChangedIoInState;
                IOMgr.GetInstace().m_eventIoOutputChanage += ChangedIoOutState;

            }
            else
            {
                IOMgr.GetInstace().m_eventIoInputChanage -= ChangedIoInState;
                IOMgr.GetInstace().m_eventIoOutputChanage -= ChangedIoOutState;

            }
        }
    }
}
