using CameraLib;
using System.Collections.Generic;

namespace StationDemo
{
    public static partial class UserConfig
    {
        public static void InitCam(Form_Auto formauto)
        {
            List<CameraBase> cameraBases = new List<CameraBase>();
            //默认添加的是海康相机 如果换相机 填上对应的类
            CameraMgr.GetInstance().EnumDevices(new HikVisionCamera(""), out cameraBases);
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