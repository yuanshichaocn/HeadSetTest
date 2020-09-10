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
