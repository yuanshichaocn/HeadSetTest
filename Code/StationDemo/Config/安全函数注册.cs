namespace StationDemo
{
    public static partial class UserConfig
    {
        /// <summary>
        /// 增加IO处理前安全判断函数
        /// </summary>
        public static void AddIoSafeOperate()
        {
            //    IOMgr.GetInstace().m_eventIsSafeWhenOutIo += Safe.IsSafeYAxisCliyder;
        }

        /// <summary>
        /// 添加运动处理前的安全判断函数
        /// </summary>
        public static void AddAxisSafeOperate()
        {
            // MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenXYMoveDisp;
        }
    }
}