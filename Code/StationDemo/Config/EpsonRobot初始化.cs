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
using System.Threading.Tasks;

namespace StationDemo
{
    public static partial class UserConfig
    {

        public static bool IsHaveEpson = false;
        public static IOOut robotStart;
        public static IOOut robotReaset;
        /// <summary>
        /// 初始化Epson 4轴机械手 
        /// </summary>
        public static void InitEpson4Robot()
        {
            if (IsHaveEpson == false)
                return;
                Task.Run(() => {
                    IOMgr.GetInstace().WriteIoBit("机器人程序启动", true);
                    Thread.Sleep(10000);
                    ScaraRobot.GetInstance().Init(out string msg);
                });
        }
   
   


    


    
       

       
   


    }
}
