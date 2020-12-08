using CommonTools;

namespace StationDemo
{
    public static partial class UserConfig
    {
        /// <summary>
        /// 添加工站
        /// </summary>
        public static void AddStation()
        {
            //添加工站
            StationMgr.GetInstance().AddStation(new StationForm(), "流水线", new LineStation(StationMgr.GetInstance().GetStation("流水线")));
            StationMgr.GetInstance().AddStation(new StationFormRobot(), "机械手", new LineStation(StationMgr.GetInstance().GetStation("机械手")));
        }

        /// <summary>
        /// 初始化主界面Haclon窗口
        /// </summary>
        /// <param name="formauto"></param>
        public static void InitHalconWindow(Form_Auto formauto)
        {
        }

        /// <summary>
        /// 工站绑定主界面视觉窗口
        /// </summary>
        /// <param name="formauto"></param>
        public static void BandStationWithVisionCtr(Form_Auto formauto)
        {
        }

        /// <summary>
        /// 绑定工站坐标数据更新
        /// </summary>
        /// <param name="form1"></param>
        public static void BandEventForStation(Form1 form1)
        {
            //StationForm.eventStationPosChanged += UpdataTrayData;
        }
    }
}