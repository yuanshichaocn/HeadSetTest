using BaseDll;

//using HalconLib;
using CommonTools;
using MotionIoLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UserCtrl;
using VisionProcess;

namespace StationDemo
{
    public partial class Form_Auto : Form
    {
        public Form_Auto()
        {
            InitializeComponent();
        }

        public delegate void ShowSomeOnAutoScreenHander(string dealtype, params object[] osbjs);

        public static ShowSomeOnAutoScreenHander ShowEventOnAutoScreen;

        private List<string> m_listFlag = new List<string>();
        private List<string> m_listInt = new List<string>();
        private List<string> m_listDouble = new List<string>();

        public void AddFlag(string strFlagName, bool bInitState)
        {
            m_listFlag.Add(strFlagName);
            userPanel_Flag.AddFlag(strFlagName);
            userPanel_Flag.SetLebalState(strFlagName, bInitState);
        }

        private void AddDoubleRtn(string strDoubleName, double dVal)
        {
            m_listDouble.Add(strDoubleName);
            Rectangle rc = dataGridView_Sum.GetColumnDisplayRectangle(2, true);

            dataGridView_Sum.Width = dataGridView_Sum.Columns[0].Width + dataGridView_Sum.Columns[1].Width + rc.Width + 15;
            dataGridView_Sum.Rows.Add(strDoubleName, dVal.ToString());
            dataGridView_Sum.Rows[dataGridView_Sum.Rows.Count - 1].Cells[1].Value = dVal.ToString();
            int height = 0;
            for (int i = 0; i < dataGridView_Sum.Rows.Count; i++)
                height += dataGridView_Sum.Rows[dataGridView_Sum.Rows.Count - 1].Height;
            dataGridView_Sum.Height = height + dataGridView_Sum.ColumnHeadersHeight + 2;
            //groupBox_Sum.Width = dataGridView_Sum.Width;
            // groupBox_Sum.Height = dataGridView_Sum.Height + 10;
        }

        private void LoadProductFile(string strFile)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => LoadProductFile(strFile)));
            }
            else
            {
                label_CurrentFile.Text = "当前产品:" + strFile;
                //读取工站位置坐标
                ConfigToolMgr.GetInstance().UpdatePointFilePath();
                Dictionary<string, PointInfo> dicPonit = new Dictionary<string, PointInfo>();
                foreach (var tem in StationMgr.GetInstance().GetAllStationName())
                {
                    ConfigToolMgr.GetInstance().ReadPoint(tem, out dicPonit);
                    StationMgr.GetInstance().GetStation(tem).SetStationPointDic(dicPonit);
                }
                ConfigToolMgr.GetInstance().UpdataMoveparamconfigPath();

                ConfigToolMgr.GetInstance().ReadMoveParamConfig();
                ConfigToolMgr.GetInstance().ReadHomeParamConfig();

                VisionMgr.GetInstance().CurrentVisionProcessDir = ParamSetMgr.GetInstance().CurrentWorkDir + "\\" + ParamSetMgr.GetInstance().CurrentProductFile + "\\" + @"Config\Vision\";
                VisionMgr.GetInstance().Read();

                ParamSetMgr.GetInstance().m_eventLoadProductFileUpadata?.Invoke();

                //VisionMgr.GetInstance().PrItemChangedEvent
                GC.Collect();
            }
        }

        private void StationStateChangedHandler(StationState currState)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => StationStateChangedHandler(currState)));
            }
            else
            {
                switch (currState)
                {
                    case StationState.StationStatePause:
                        MachineStateEmg.State = false;
                        MachineStateStop.State = false;
                        MachineStateAuto.State = false;
                        MachineStatePause.State = true;
                        break;

                    case StationState.StationStateRun:
                        MachineStateEmg.State = false;
                        MachineStateStop.State = false;
                        MachineStateAuto.State = true;
                        MachineStatePause.State = false;
                        break;

                    case StationState.StationStateStop:
                        MachineStateEmg.State = false;
                        MachineStateStop.State = true;
                        MachineStateAuto.State = false;
                        MachineStatePause.State = false;
                        break;

                    case StationState.StationStateEmg:
                        MachineStateEmg.State = true;
                        MachineStateStop.State = false;
                        MachineStateAuto.State = false;
                        MachineStatePause.State = false;
                        break;
                }
            }
        }

        private object objlock = new object();
        private const int nCount = 2000;

        private void ShowStationMsg(ListLog listlog, string msg)
        {
            if (listlog == null)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ShowStationMsg(listlog, msg)));
            }
            else
            {
                listlog.AddMsg(msg);
                listlog.TopIndex = listlog.Items.Count - (int)(listlog.Height / listlog.ItemHeight);
            }
        }

        private void ShowStationMsgOnRichTxtBox(RichTxtBoxLog richTextBox, string msg)
        {
            if (richTextBox == null)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ShowStationMsgOnRichTxtBox(richTextBox, msg)));
            }
            else
            {
                richTextBox.AddMsg(msg);
                //   richTextBox.TopIndex = richTextBox.Items.Count - (int)(richTextBox.Height / richTextBox.ItemHeight);
            }
        }

        public void FlushWeightVal(int num, int[] WeightVal)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => { FlushWeightVal(num, WeightVal); }));
            }
            else
            {
                if (WeightVal == null || WeightVal.Length != 4)
                    return;
            }
        }

        public class objtest
        {
            public int a;
            public int b;
        }

        private objtest sdf = new objtest();

        private void Form_Auto_Load(object sender, EventArgs e)
        {
            GlobalVariable.g_eventStationStateChanged += StationStateChangedHandler;
            tabControl_Log.Controls.Clear();
            ParamSetMgr.GetInstance().m_eventChangedBoolSysVal += Form_Auto_m_eventChangedBoolSysVal;
            ParamSetMgr.GetInstance().m_eventChangedDoubleSysVal += Form_Auto_m_eventChangedDoubleSysVal;
            ParamSetMgr.GetInstance().m_eventLoadProductFile += LoadProductFile;
            label_CurrentFile.Text = "当前产品:" + ParamSetMgr.GetInstance().CurrentProductFile;
            foreach (var tem in StationMgr.GetInstance().GetAllStationName())
            {
                RichTxtBoxLog richTextBox = new RichTxtBoxLog();
                Control control = null;
                //ListLog listLog = new ListLog();
                //listLog.ItemHeight = 25;
                //listLog.HorizontalScrollbar = true;
                //listLog.ScrollAlwaysVisible = true;
                //listLog.Size = new Size(tabControl_Log.Size.Width - 20, tabControl_Log.Size.Height - 20);
                //control= (Control)listLog;

                richTextBox.Size = new Size(tabControl_Log.Size.Width - 30, tabControl_Log.Size.Height - 30);
                richTextBox.ScrollBars = RichTextBoxScrollBars.Both;

                richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
                richTextBox.HideSelection = false;
                richTextBox.Location = new System.Drawing.Point(0, 0);
                richTextBox.Name = "richTextBox1";
                richTextBox.Size = new System.Drawing.Size(150, 150);
                richTextBox.TabIndex = 0;
                richTextBox.Text = "";
                richTextBox.WordWrap = false;
                control = richTextBox;
                TabPage tabStaion = new TabPage();
                tabStaion.Name = tem;
                tabStaion.Text = tem;

                tabStaion.Controls.Add((Control)control);
                tabControl_Log.TabPages.Add(tabStaion);

                // StationMgr.GetInstance().GetStation(tem).SetShowListBox(listLog);
                // StationMgr.GetInstance().GetStation(tem).m_eventListBoxShow += ShowStationMsg;
                richTextBox.Multiline = true;

                StationMgr.GetInstance().GetStation(tem).SetShowRichTextBox(richTextBox);
                StationMgr.GetInstance().GetStation(tem).m_eventRichBoxShow += ShowStationMsgOnRichTxtBox;
                ;

                StationMgr.GetInstance().GetStation(tem).Info(tem + $" sd加载成功");
                //for (int i = 0; i < 300; i++)
                //{
                //    StationMgr.GetInstance().GetStation(tem).Info(tem + $"加载成功{i}");
                //    StationMgr.GetInstance().GetStation(tem).Err(tem + $"加载成功err{i}");
                //}
            }

            MachineStateEmg.Name = "急停";
            MachineStateStop.Name = "停止";
            MachineStateAuto.Name = "自动";
            MachineStatePause.Name = "暂停";
            //添加 ------- 标志--------///
            userPanel_Flag.Visible = false;
            UserConfig.AddFlag(this);
            if (m_listFlag.Count > 0)
                userPanel_Flag.Visible = true;
            userPanel_Flag.Update();

            //添加 ------- 标志--------///
            //添加 ------- double param--------///
            AddDoubleRtn("产品计数", 0);
            ParamSetMgr.GetInstance().SetDoubleParam("产品计数", 0);

            AddDoubleRtn("CT", 0);
            ParamSetMgr.GetInstance().SetDoubleParam("CT", 0);

            AddDoubleRtn("UPH", 0);
            ParamSetMgr.GetInstance().SetDoubleParam("UPH", 0);

            UserConfig.InitHalconWindow(this);
            UserConfig.BandStationWithVisionCtr(this);
            UserConfig.InitCam(this);

            //默认用户登陆
            sys.g_User = sys.g_listUser.Find(t => t._userName == "admin");

            UserConfig.InitEpson4Robot();
            UserConfig.InitHardWare();
            UserConfig.CalibDataRead();
            UserConfig.ReadVisionData();
            UserConfig.UpdataTrayData();
            UserConfig.ReadAndUpdatStatisticaldata(this);
        }

        private void Form_Auto_m_eventChangedDoubleSysVal(string key, double val)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => Form_Auto_m_eventChangedDoubleSysVal(key, val)));
            }
            else
            {
                int index = m_listDouble.FindIndex(t => t == key);
                if (index != -1)
                {
                    double dval = 0;
                    if (index < dataGridView_Sum.Rows.Count)
                    {
                        dataGridView_Sum.Rows[index].Cells[1].Value = val.ToString();
                    }
                }
            }
        }

        private void Form_Auto_m_eventChangedBoolSysVal(string key, bool val)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => Form_Auto_m_eventChangedBoolSysVal(key, val)));
            }
            else
            {
                int index = m_listFlag.FindIndex(t => t == key);
                if (index != -1)
                {
                    if (index < m_listFlag.Count)
                    {
                        userPanel_Flag.SetLebalState(key, val);
                        if (val)
                        {
                            // dataGridView_Flag.Rows[index].Cells[1].Value = "ON";
                            //  dataGridView_Flag.Rows[index].Cells[1].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            //  dataGridView_Flag.Rows[index].Cells[1].Value = "OFF";
                            //  dataGridView_Flag.Rows[index].Cells[1].Style.BackColor = Color.LightBlue;
                        }
                    }
                }
            }
        }

        private void ShowImg(object sender, EventArgs e)
        {
            //   HObject img = CameraMgr.GetInstance().GetCamera("CHP").GetImage();
            //    HOperatorSet.DispObj(img, (HTuple)CameraMgr.GetInstance().GetCamera("CHP").wnd);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            IOMgr.GetInstace().m_deltgateSystemSingl("复位", true);
            AlarmMgr.GetIntance().StopAlarmBeet();
        }

        private void CloseMainForm(object sender, FormClosedEventArgs e)
        {
        }

        public void ChangeBtnClearAllProductState()
        {
            BtnClearAllProduct.Text = "清料";
        }

        private void BtnClearAllProduct_Click(object sender, EventArgs e)
        {
            //BtnClearAllProduct.Text = "清料中。。。";
            ParamSetMgr.GetInstance().SetBoolParam("启动清料", true);
        }
    }

    public class TolNum
    {
        private TolNum()
        {
        }

        private static TolNum tol = new TolNum();
        private static object lockobj = new object();

        public static TolNum GetIntance()
        {
            if (tol == null)
            {
                lock (lockobj)
                {
                    tol = new TolNum();
                }
            }
            return tol;
        }

        public double nSumProduct = 0;
        public double nLeftProduct = 0;
        public double nRightProduct = 0;
        public double nLoadNozzleWorkNum = 0;
        public double nUnLoadNozzleWorkNum = 0;
        public double nLeftNozzleWorkNum = 0;
        public double nRightNozzleWorkNum = 0;

        public void Save()
        {
            tol.nSumProduct = ParamSetMgr.GetInstance().GetDoubleParam("产品计数");
            tol.nLeftProduct = ParamSetMgr.GetInstance().GetDoubleParam("左废纸数");
            tol.nRightProduct = ParamSetMgr.GetInstance().GetDoubleParam("右废纸数");
            tol.nLoadNozzleWorkNum = ParamSetMgr.GetInstance().GetDoubleParam("上料吸嘴数");
            tol.nUnLoadNozzleWorkNum = ParamSetMgr.GetInstance().GetDoubleParam("下料吸嘴数");
            tol.nLeftNozzleWorkNum = ParamSetMgr.GetInstance().GetDoubleParam("左贴装吸嘴数");
            tol.nRightNozzleWorkNum = ParamSetMgr.GetInstance().GetDoubleParam("右贴装吸嘴数");
            AccessXmlSerializer.ObjectToXml("d:\\Sum.xml", this);
        }

        public TolNum Read()
        {
            TolNum tol1 = (TolNum)AccessXmlSerializer.XmlToObject("d:\\Sum.xml", typeof(TolNum));
            if (tol1 == null)
            {
                tol.Save();
            }
            else
                tol = tol1;
            ParamSetMgr.GetInstance().SetDoubleParam("产品计数", tol.nSumProduct);
            ParamSetMgr.GetInstance().SetDoubleParam("左废纸数", tol.nLeftProduct);
            ParamSetMgr.GetInstance().SetDoubleParam("右废纸数", tol.nRightProduct);

            ParamSetMgr.GetInstance().SetDoubleParam("上料吸嘴数", tol.nLoadNozzleWorkNum);
            ParamSetMgr.GetInstance().SetDoubleParam("下料吸嘴数", tol.nUnLoadNozzleWorkNum);
            ParamSetMgr.GetInstance().SetDoubleParam("左贴装吸嘴数", tol.nLeftNozzleWorkNum);
            ParamSetMgr.GetInstance().SetDoubleParam("右贴装吸嘴数", tol.nRightNozzleWorkNum);
            return tol;
        }
    }
}