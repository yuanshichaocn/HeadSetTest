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
    

  

        public static void InitCam(Form_Auto formauto)
        {
            List<CameraBase> cameraBases = new List<CameraBase>();
            foreach (var temp in cameraBases)
            {

                temp.Open();
                temp.SetExposureTime(1000);
                temp.SetGain(1);
                temp.RegisterCallBack();
                temp.SetAcquisitionMode();

                temp.StartGrab();
            }
          
        }




    }
}
