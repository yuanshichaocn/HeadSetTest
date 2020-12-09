using BaseDll;
using CommonTools;
using log4net;
using MotionIoLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using VisionProcess;

namespace StationDemo
{
    public partial class Form1 : Form, IUserRightSwitch
    {
        public delegate void ShowSomeOnMainFrameScreenHander(string dealtype, params object[] osbjs);

        public static ShowSomeOnMainFrameScreenHander ShowEventOnMainFrameScreen;

        public Form1()
        {
            InitializeComponent();
        }

        private Dictionary<Button, Form> m_dicAllWindows = new Dictionary<Button, Form>();
        private Form m_currentForm = null;

        public UserRight userRight
        {
            get;
            set;
        }

        public void ChangedUserRight(User Currentuser)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ChangedUserRight(Currentuser)));
            }
            else
            {
                int indx = this.Text.IndexOf(":");
                if (indx > 0)
                {
                    this.Text = this.Text.Substring(0, indx);
                    this.Text = this.Text + " : " + Currentuser._userName + "登陆成功";
                }
                else
                    this.Text = this.Text + " : " + Currentuser._userName + "登陆成功";
                bool bEnable = false;
                if (Currentuser._userRight == UserRight.客户操作员)
                {
                    button_vision.Enabled = bEnable;
                    button_Sys.Enabled = bEnable;
                    button_UserSMgr.Enabled = true;
                    button_Sys.Enabled = bEnable;
                }
                else if (Currentuser._userRight == UserRight.调试工程师)
                {
                    bEnable = true;
                    button_vision.Enabled = bEnable;
                    bEnable = false;
                    button_UserSMgr.Enabled = bEnable;
                    button_Sys.Enabled = bEnable;
                }
                else if (Currentuser._userRight == UserRight.软件工程师)
                {
                    bEnable = true;
                    button_vision.Enabled = bEnable;
                    button_Sys.Enabled = bEnable;
                    button_UserSMgr.Enabled = bEnable;
                    button_Sys.Enabled = bEnable;
                }
                else if (Currentuser._userRight == UserRight.超级管理员)
                {
                    bEnable = true;
                    button_vision.Enabled = bEnable;
                    button_Sys.Enabled = bEnable;
                    button_UserSMgr.Enabled = bEnable;
                    button_Sys.Enabled = bEnable;
                }
            }
        }

        private void SwitchWindow(Button btn)
        {
            Form NextForm = null;
            m_dicAllWindows.TryGetValue(btn, out NextForm);
            if (NextForm != null && NextForm == m_currentForm)
                return;
            if (NextForm != null && m_currentForm != null)
            {
                m_currentForm.Hide();
                m_currentForm = NextForm;
            }
            if (NextForm != null)
            {
                NextForm.TopLevel = false;
                NextForm.Parent = this.panel_window;
                NextForm.Dock = DockStyle.Fill;
                NextForm.Show();
            }
        }

        private void button_Home_Click(object sender, EventArgs e)
        {
            SwitchWindow((Button)sender);
        }

        private void button_set_Click(object sender, EventArgs e)
        {
            SwitchWindow((Button)sender);
        }

        private void button_vision_Click(object sender, EventArgs e)
        {
            SwitchWindow((Button)sender);
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
                        button_start.ImageIndex = 0;
                        button_start.Text = "恢复";
                        this.button_start.BackgroundImage = global::StationDemo.Properties.Resources.resume;
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                            IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                            IOMgr.GetInstace().WriteIoBit("红灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                            IOMgr.GetInstace().WriteIoBit("绿灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                            IOMgr.GetInstace().WriteIoBit("黄灯", true);

                        break;

                    case StationState.StationStateRun:
                        button_start.ImageIndex = 1;
                        button_start.Text = "暂停";
                        this.button_start.BackgroundImage = global::StationDemo.Properties.Resources.pause;
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                            IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                            IOMgr.GetInstace().WriteIoBit("红灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                            IOMgr.GetInstace().WriteIoBit("绿灯", true);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                            IOMgr.GetInstace().WriteIoBit("黄灯", false);
                        break;

                    case StationState.StationStateStop:
                        button_start.ImageIndex = 0;
                        this.button_start.BackgroundImage = global::StationDemo.Properties.Resources.Start;
                        button_start.Text = "开始";
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                            IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                            IOMgr.GetInstace().WriteIoBit("红灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                            IOMgr.GetInstace().WriteIoBit("绿灯", true);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                            IOMgr.GetInstace().WriteIoBit("黄灯", true);
                        break;
                }
            }
        }

        private bool bAlreadyEmg = false;

        /// <summary>
        /// 在启动/恢复程序前，检查安全门光栅
        /// </summary>
        /// <returns></returns>
        public bool IsSafeDoorAndGrating()
        {
            if (ParamSetMgr.GetInstance().GetBoolParam("启用安全门"))
            {
                if (IOMgr.GetInstace().GetOutputDic().ContainsKey("安全门") && !IOMgr.GetInstace().ReadIoInBit("安全门"))
                {
                    MessageBox.Show("安全门打开", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg("安全门打开", null, DlgWaranType.WaranOK);
                    return false;
                }
            }
            if (IOMgr.GetInstace().GetOutputDic().ContainsKey("气源检测") && IOMgr.GetInstace().GetInputDic().ContainsKey("气源检测"))
            {
                if (!IOMgr.GetInstace().ReadIoInBit("气源检测"))
                {
                    MessageBox.Show("气源检测失败，请打开气源", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg("气源检测失败，请打开气源", null, DlgWaranType.WaranOK);
                    return false;
                }
            }
            if (IOMgr.GetInstace().GetOutputDic().ContainsKey("真空气源检测") && IOMgr.GetInstace().GetInputDic().ContainsKey("真空气源检测"))
            {
                if (!IOMgr.GetInstace().ReadIoInBit("真空气源检测"))
                {
                    MessageBox.Show("真空气源检测失败,请打开真空气源", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //  WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg("真空气源检测失败,请打开真空气源", null, DlgWaranType.WaranOK);
                    return false;
                }
            }

            return true;
        }

        public void ProcessSysIo(string strIoName, bool bCurrentState)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ProcessSysIo(strIoName, bCurrentState)));
            }
            else
            {
                if (strIoName == "急停" && !bCurrentState && GlobalVariable.g_StationState != StationState.StationStateEmg)
                {
                    bAlreadyEmg = true;
                    if (GlobalVariable.g_StationState == StationState.StationStateRun || GlobalVariable.g_StationState == StationState.StationStatePause)
                    {
                        MotionMgr.GetInstace().StopEmg();
                        StationMgr.GetInstance().Stop();
                        MotionMgr.GetInstace().StopEmg();
                    }
                    GlobalVariable.g_StationState = StationState.StationStateEmg;
                    if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                        IOMgr.GetInstace().WriteIoBit("绿灯", false);
                    if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                        IOMgr.GetInstace().WriteIoBit("红灯", false);
                    if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                        IOMgr.GetInstace().WriteIoBit("黄灯", false);
                    DoWhile.StopCirculate();
                    AlarmMgr.GetIntance().Warn("急停被按下", AlarmType.AlarmType_Emg);
                }

                if (strIoName == "气源压力检测" && !bCurrentState && !bAlreadyEmg)
                {
                    bAlreadyEmg = true;
                    if (GlobalVariable.g_StationState == StationState.StationStateRun || GlobalVariable.g_StationState == StationState.StationStatePause)
                    {
                        MotionMgr.GetInstace().StopEmg();
                        StationMgr.GetInstance().Stop();
                        MotionMgr.GetInstace().StopEmg();
                    }
                    GlobalVariable.g_StationState = StationState.StationStateStop;
                    if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                        IOMgr.GetInstace().WriteIoBit("绿灯", false);
                    if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                        IOMgr.GetInstace().WriteIoBit("红灯", false);
                    if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                        IOMgr.GetInstace().WriteIoBit("黄灯", false);

                    DoWhile.StopCirculate();
                    AlarmMgr.GetIntance().Warn("气源压力检测 失败", AlarmType.AlarmType_Emg);
                }

                if (strIoName == "安全门" && !bCurrentState)
                {
                    if (ParamSetMgr.GetInstance().GetBoolParam("启用安全门"))
                    {
                        if (GlobalVariable.g_StationState == StationState.StationStateRun)
                        {
                            StationMgr.GetInstance().Pause();
                            IOMgr.GetInstace().WriteIoBit("绿灯", false);
                            if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                                IOMgr.GetInstace().WriteIoBit("红灯", false);
                            if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                                IOMgr.GetInstace().WriteIoBit("黄灯", true);
                            if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                                IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                            //AlarmMgr.GetIntance().Warn("安全门打开");
                            MessageBox.Show("安全门打开", "Waran", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //  WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg("安全门打开", null, DlgWaranType.WaranOK);
                        }
                    }
                }
                if (strIoName == "暂停" && bCurrentState)
                {
                    if (GlobalVariable.g_StationState == StationState.StationStateRun)
                    {
                        StationMgr.GetInstance().Pause();
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                            IOMgr.GetInstace().WriteIoBit("绿灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                            IOMgr.GetInstace().WriteIoBit("红灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                            IOMgr.GetInstace().WriteIoBit("黄灯", true);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                            IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                        //AlarmMgr.GetIntance().Warn("安全门打开");
                        // WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg("安全门打开", null, DlgWaranType.WaranOK);
                    }
                }
                if (strIoName == "安全光栅" && !bCurrentState)
                {
                    if (ParamSetMgr.GetInstance().GetBoolParam("启用安全光栅"))
                    {
                        if (ParamSetMgr.GetInstance().GetBoolParam("启用安全光栅"))
                        {
                            if (GlobalVariable.g_StationState == StationState.StationStateRun)
                            {
                                StationMgr.GetInstance().Pause();
                                IOMgr.GetInstace().WriteIoBit("绿灯", false);
                                if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                                    IOMgr.GetInstace().WriteIoBit("红灯", false);
                                if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                                    IOMgr.GetInstace().WriteIoBit("黄灯", true);
                                if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                                    IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                                //  AlarmMgr.GetIntance().Warn("安全光栅打开");
                                //WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg("安全光栅打开", null, DlgWaranType.WaranOK);
                                MessageBox.Show("安全光栅打开", "Waran", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                if (strIoName == "启动" && bCurrentState)
                {
                    if (GlobalVariable.g_StationState == StationState.StationStateStop)
                    {
                        if (!IsSafeDoorAndGrating())
                            return;
                        StationMgr.GetInstance().Run();
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                            IOMgr.GetInstace().WriteIoBit("绿灯", true);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                            IOMgr.GetInstace().WriteIoBit("红灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                            IOMgr.GetInstace().WriteIoBit("黄灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                            IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                    }
                    else if (GlobalVariable.g_StationState == StationState.StationStatePause)
                    {
                        if (!IsSafeDoorAndGrating())
                            return;
                        StationMgr.GetInstance().Resume();
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                            IOMgr.GetInstace().WriteIoBit("绿灯", true);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                            IOMgr.GetInstace().WriteIoBit("红灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                            IOMgr.GetInstace().WriteIoBit("黄灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                            IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                    }
                    else
                    {
                        MessageBox.Show("发生错误，请先复位", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (strIoName == "复位" && bCurrentState)
                {
                    if (GlobalVariable.g_StationState == StationState.StationStateEmg)
                    {
                        GlobalVariable.g_StationState = StationState.StationStateStop;
                        MotionMgr.GetInstace().ResetAxis();
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                            IOMgr.GetInstace().WriteIoBit("绿灯", true);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                            IOMgr.GetInstace().WriteIoBit("红灯", false);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                            IOMgr.GetInstace().WriteIoBit("黄灯", true);
                        if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                            IOMgr.GetInstace().WriteIoBit("蜂鸣", false);

                        bStartAlarmTimer = false;
                        bAlreadyEmg = false;
                    }
                }
            }
        }

        private ILog _logger = LogManager.GetLogger(nameof(Form1));

        // System.Timers.Timer alarmtimer = new System.Timers.Timer();
        private bool bStartAlarmTimer = false;

        private Mutex mutex = new Mutex(true, "StationInstance");

        private void Form1_Load(object sender, EventArgs e)
        {
            //读取网口串口配置
            ParamSetMgr.GetInstance().SetBoolParam("启用安全门", true);
            ParamSetMgr.GetInstance().SetBoolParam("启用安全光栅", true);

            ConfigToolMgr.GetInstance().ReadEthConfig();
            ConfigToolMgr.GetInstance().ReadComConfig();
            //读硬件配置 并创建IO,Motion 卡类 对象 并以默认参数配置卡
            ConfigToolMgr.GetInstance().ReadMotionCardConfig();
            ConfigToolMgr.GetInstance().ReadIoCardConfig();
            ConfigToolMgr.GetInstance().ReadIoInputConfig();
            ConfigToolMgr.GetInstance().ReadIoOutputConfig();

            //读工站配置
            ConfigToolMgr.GetInstance().ReadStationConfig();

            //读取用户设置
            ConfigToolMgr.GetInstance().ReadUserConfig();
            //读取产品文件
            ConfigToolMgr.GetInstance().ReadProductDir_Name();
            //读运动配置
            ConfigToolMgr.GetInstance().ReadMoveParamConfig();
            ConfigToolMgr.GetInstance().ReadHomeParamConfig();
            ConfigToolMgr.GetInstance().ReadUserParam();

            //视觉x文件路径初始化
            VisionMgr.GetInstance().CurrentVisionProcessDir = ParamSetMgr.GetInstance().CurrentWorkDir + "\\" + ParamSetMgr.GetInstance().CurrentProductFile + "\\" + @"Config\Vision\";
            string str = VisionMgr.GetInstance().CurrentVisionProcessDir;

            //初始化 Motion IO
            MotionMgr.GetInstace().OpenAllCard();
            IOMgr.GetInstace().initAllIoCard();

            //AA参数加载

            UserConfig.BandEventOnForm1(this);

            this.WindowState = FormWindowState.Maximized;
            int BtnHeight = button_stop.Height + 10;
            panel_window.Location = new System.Drawing.Point(0, BtnHeight);

            panel_window.Size = new Size(this.Width - 30, this.Height - BtnHeight - 1);
            Form_Auto autoform = new Form_Auto();
            m_dicAllWindows.Add(button_Home, autoform);
            m_dicAllWindows.Add(button_Set, new Form_Set());
            m_dicAllWindows.Add(button_vision, new Form_VisionDebug());
            m_dicAllWindows.Add(button_Param, new Form_ParamSet());
            m_dicAllWindows.Add(button_UserSMgr, new UserManger());
            m_currentForm = autoform;

            UserConfig.AddStation();
            //读取工站位置坐标
            Dictionary<string, PointInfo> dicPonit = new Dictionary<string, PointInfo>();
            foreach (var tem in StationMgr.GetInstance().GetAllStationName())
            {
                ConfigToolMgr.GetInstance().ReadPoint(tem, out dicPonit);
                StationMgr.GetInstance().GetStation(tem).SetStationPointDic(dicPonit);
            }
            // 注册安全函数
            UserConfig.AddIoSafeOperate();
            UserConfig.AddAxisSafeOperate();

            //初始化 工站状态
            GlobalVariable.g_StationState = StationState.StationStateStop;
            IOMgr.GetInstace().m_deltgateSystemSingl += ProcessSysIo;
            MotionMgr.GetInstace().m_eventAxisSingl += ProcessSysIo;
            autoform.TopLevel = false;
            autoform.Dock = DockStyle.Fill;
            autoform.Parent = this.panel_window;
            autoform.Show();

            //初始化权限
            sys.g_eventRightChanged += ChangedUserRight;
            User user = new User();
            int index = sys.g_listUser.FindIndex(t => t._userName == "admin");
            if (index == -1)
            {
                user = new User() { _userName = "admin", _userPassWord = "admin", _userRight = UserRight.超级管理员 };
                sys.g_listUser.Add(user);
            }
            index = sys.g_listUser.FindIndex(t => t._userName == "user");
            if (index == -1)
            {
                user = new User() { _userName = "user", _userPassWord = "user", _userRight = UserRight.客户操作员 };
                sys.g_listUser.Add(user);
            }
            index = sys.g_listUser.FindIndex(t => t._userName == "debug");
            if (index == -1)
            {
                user = new User() { _userName = "debug", _userPassWord = "debug", _userRight = UserRight.调试工程师 };
                sys.g_listUser.Add(user);
            }
            index = sys.g_listUser.FindIndex(t => t._userName == "engineer");
            if (index == -1)
            {
                user = new User() { _userName = "engineer", _userPassWord = "engineer", _userRight = UserRight.软件工程师 };
                sys.g_listUser.Add(user);
            }
            GlobalVariable.g_eventStationStateChanged += StationStateChangedHandler;
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            // ProcessSysIo("启动", true);
            if (GlobalVariable.g_StationState == StationState.StationStateStop)
            {
                if (!IsSafeDoorAndGrating())
                    return;

                ParamSetMgr.GetInstance().SetBoolParam("启动清料", false);
                if (IOMgr.GetInstace().GetOutputDic().ContainsKey("绿灯"))
                    IOMgr.GetInstace().WriteIoBit("绿灯", false);
                if (IOMgr.GetInstace().GetOutputDic().ContainsKey("红灯"))
                    IOMgr.GetInstace().WriteIoBit("红灯", false);
                if (IOMgr.GetInstace().GetOutputDic().ContainsKey("黄灯"))
                    IOMgr.GetInstace().WriteIoBit("黄灯", true);
                if (IOMgr.GetInstace().GetOutputDic().ContainsKey("蜂鸣"))
                    IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                StationMgr.GetInstance().Run();
                // sys.g_StationState = StationState.StationStateRun;
            }
            else if (GlobalVariable.g_StationState == StationState.StationStateRun)
            {
                // sys.g_StationState = StationState.StationStatePause;
                //暂停
                StationMgr.GetInstance().Pause();
            }
            else if (GlobalVariable.g_StationState == StationState.StationStatePause)
            {
                if (!IsSafeDoorAndGrating())
                    return;
                IOMgr.GetInstace().WriteIoBit("绿灯", false);
                IOMgr.GetInstace().WriteIoBit("红灯", false);
                IOMgr.GetInstace().WriteIoBit("黄灯", true);
                IOMgr.GetInstace().WriteIoBit("蜂鸣", false);
                StationMgr.GetInstance().Resume();
            }
            else if (GlobalVariable.g_StationState == StationState.StationStateEmg)
            {
                MessageBox.Show("发生错误，请先复位", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            StationMgr.GetInstance().Pause();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确定停止", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;
            _logger.Info("stop:开始");
            if (GlobalVariable.g_StationState == StationState.StationStateRun || GlobalVariable.g_StationState == StationState.StationStatePause)
                StationMgr.GetInstance().Stop();
            else
                DoWhile.StopCirculate();
            _logger.Info("stop:结束");
            AlarmMgr.GetIntance().StopAlarmBeet();
        }

        private void CloseFrom(object sender, FormClosedEventArgs e)
        {
            UserConfig.CloseHardWork();
            // System.Environment.Exit(0);
        }

        private void button_Param_Click(object sender, EventArgs e)
        {
            SwitchWindow((Button)sender);
        }

        private void button_AalarmReport_Click(object sender, EventArgs e)
        {
            SwitchWindow((Button)sender);
        }

        private void button_UserSet_Click(object sender, EventArgs e)
        {
            SwitchWindow((Button)sender);
        }

        private void button_LoadInOut_Click(object sender, EventArgs e)
        {
            Form_Load form_Load = new Form_Load();
            form_Load.ShowDialog();
            form_Load.Dispose();
        }

        private void ClosingMainFrom(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否确定关闭软件", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
        }
    }
}