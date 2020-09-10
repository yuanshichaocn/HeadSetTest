using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonTools;

namespace StationDemo
{
    public partial class Form_Set : Form
    {
      //  private StationForm m_currentForm = null;
        private Form m_currentForm = null;
        public Form_Set()
        {
            InitializeComponent();
        }
        //private Dictionary<StationForm, Stationbase> m_dicFromStation = new Dictionary<StationForm, Stationbase>();

        private ConcurrentDictionary<Form, Stationbase> m_dicFromStation = new ConcurrentDictionary<Form, Stationbase>();
        public Stationbase GetStationByStationForm(StationForm temp)
        {
            Stationbase sta = null;
            m_dicFromStation.TryGetValue(temp, out sta);
            return sta;   
        }
        private void Form_Set_Load(object sender, EventArgs e)
        {
           
            rightTab1.TabPages.Clear();
            List<string> StationNameList = StationMgr.GetInstance().GetAllStationName();
            int index = 0;
            Form_Manual ManualForm = new Form_Manual();
            ManualForm.TopLevel = false;
            ManualForm.Size = rightTab1.ClientSize;
            ManualForm.Dock = DockStyle.Fill;

            TabPage tabManual = new TabPage();
            tabManual.Name = "ManualOpreate";
            tabManual.Text = "手动操作";
            tabManual.Controls.Add(ManualForm);
            rightTab1.TabPages.Add(tabManual);
           

            Form_AllIO form_AllIO = new Form_AllIO();
            form_AllIO.TopLevel = false;
            form_AllIO.Size = rightTab1.ClientSize;
            form_AllIO.Dock = DockStyle.Fill;


            TabPage tabIo = new TabPage();
            tabIo.Name = "AllIo";
            tabIo.Text = "Io操作";
            tabIo.Controls.Add(form_AllIO);
            rightTab1.TabPages.Add(tabIo);

            Form_AxisTest form_AxisTest = new Form_AxisTest();
            form_AxisTest.TopLevel = false;
            form_AxisTest.Dock = DockStyle.Fill;      
            TabPage tabAxisTest = new TabPage();
            tabAxisTest.Name = "MotionParam";
            tabAxisTest.Text = "电机设置";
            tabAxisTest.Controls.Add(form_AxisTest);
            rightTab1.TabPages.Add(tabAxisTest);
            tabAxisTest.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;


            string strStationName = "";
            foreach (var st in StationMgr.GetInstance().GetDicStaion())
            {
                Form temp = (Form)st.Key;
                temp.Name = st.Value.Name;
                strStationName = st.Value.Name;
                m_dicFromStation.TryAdd(temp, StationMgr.GetInstance().GetStation(strStationName));
            
                temp.TopLevel = false;
                temp.Dock = DockStyle.Fill;

                TabPage tab = new TabPage();
                tab.Name = strStationName;
                tab.Text = strStationName;
                tab.Controls.Add(temp);
                rightTab1.TabPages.Add(tab);
                temp.Name = strStationName;
              
            }
            int stationIndexStart = 3;
            if (rightTab1.TabPages.Count>3)
                stationIndexStart = 3;
            else
                stationIndexStart = 0;
            rightTab1.SelectedIndex = stationIndexStart;
            rightTab1.TabPages[stationIndexStart].Controls[0].Show();
            m_currentForm = (Form)rightTab1.TabPages[stationIndexStart].Controls[0];

        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rightTab1.TabPages.Count <= 0)
                return;
            if (m_currentForm != null)
            {
                m_currentForm.Hide();
            }

            rightTab1.TabPages[0].Controls[0].Show();
            m_currentForm = (Form)rightTab1.TabPages[rightTab1.SelectedIndex].Controls[0];
            m_currentForm.Show();


        }
        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brush = new SolidBrush(this.ForeColor);
            Rectangle rect = rightTab1.GetTabRect(e.Index);

            //       g.TextRenderingHint = TextRenderingHint.AntiAlias;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            g.DrawString(rightTab1.Controls[e.Index].Text, this.Font, brush, rect, sf);
        }
        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
  

        }
    }
}
