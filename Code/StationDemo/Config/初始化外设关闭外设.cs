using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools;
using MotionIoLib;
using Communicate;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using UserCtrl;
using EpsonRobot;
using BaseDll;
using UserData;
using VisionProcess;
using HalconDotNet;
using System.IO;

using CameraLib;
using OtherDevice;
using XYZDispensVision;
using MachineSafe;


namespace StationDemo
{
    public static  partial class UserConfig
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
