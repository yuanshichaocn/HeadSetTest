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
        /// 初始化产品数据参数 在Form_Auto上
        /// </summary>
        public static void BandEventOnAutoScreenLoad(Form_Auto formauto)
        {
            // public delegate void ShowSomeOnAutoScreenHander(string dealtype, params object[] osbjs);
            Form_Auto.ShowEventOnAutoScreen = null;

        }
        /// <summary>
        /// 初始化产品数据参数 在Form1上
        /// </summary>
        public static void BandEventOnForm1(Form1 form1)
        {
            // public delegate void ShowSomeOnMainFrameScreenHander(string dealtype, params object[] osbjs);
            //  Form1.ShowEventOnMainFrameScreen ;
 
        }
    


    }
}
