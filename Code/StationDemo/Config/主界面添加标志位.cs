using BaseDll;
using CommonTools;

namespace StationDemo
{
    public static partial class UserConfig
    {
        /// <summary>
        /// 增加自动界面 标志位
        /// </summary>
        /// <param name="formauto"></param>
        public static void AddFlag(Form_Auto formauto)
        {
            formauto.AddFlag("系统空跑", sys.g_AppMode == AppMode.AirRun);
        }
    }
}