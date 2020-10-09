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

        static bool bIsHaveMes = false;
        public static bool InitSamllMesSever()
        {
            if (!bIsHaveMes)
                return true;
            SmallMes.Ins.SeverInit();
                return true;
         }


    }
}
