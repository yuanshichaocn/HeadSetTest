using CameraLib;
using Communicate;
using MotionIoLib;
using System;

namespace StationDemo
{
    public static partial class UserConfig
    {
        /// <summary>
        /// 初始化硬件
        /// </summary>

        public static void InitHardWare()
        {
            try
            {
            }
            catch (Exception e)
            {
                return;
            }
        }

        public static void CloseHardWork()
        {
            CameraMgr.GetInstance().Close();
            TcpMgr.GetInstance().CloseAllEth();
            MotionMgr.GetInstace().Close();
            IOMgr.GetInstace().Close();
        }
    }
}