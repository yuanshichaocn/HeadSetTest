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
