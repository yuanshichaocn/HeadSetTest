using BaseDll;
using CommonTools;
using Communicate;
using MotionIoLib;
using System;
using System.Windows.Forms;

namespace StationDemo
{
    public class APP
    {
        #region 单例

        private static readonly Lazy<APP> instance = new Lazy<APP>(() => new APP());

        private APP()
        {
            MotionMgr = MotionMgr.GetInstace();
            IOMgr = IOMgr.GetInstace();
            AlarmMgr = AlarmMgr.GetIntance();
            ConfigToolMgr = ConfigToolMgr.GetInstance();
            StationMgr = StationMgr.GetInstance();
            ParamSetMgr = ParamSetMgr.GetInstance();
            ComMgr = ComMgr.GetInstance();
            TcpMgr = TcpMgr.GetInstance();
            SocketSeverMgr = SocketSeverMgr.GetInstace();
        }

        public static APP Instance
        {
            get
            {
                return instance.Value;
            }
        }

        #endregion 单例

        #region 管理类
        public MotionMgr MotionMgr;
        public IOMgr IOMgr;
        public AlarmMgr AlarmMgr;
        public ConfigToolMgr ConfigToolMgr;
        public StationMgr StationMgr;
        public ParamSetMgr ParamSetMgr;
        public ComMgr ComMgr;
        public TcpMgr TcpMgr;
        public SocketSeverMgr SocketSeverMgr;
        #endregion

        #region 界面  
        public Form MainForm;
        public Form AutoForm;
        public Form Form_Set;
        public Form Form_VisionDebug;
        public Form Form_ParamSet;
        public Form UserManger;
        #endregion

    }
}