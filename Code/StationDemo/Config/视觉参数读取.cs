using BaseDll;
using CommonTools;
using System;
using VisionProcess;

namespace StationDemo
{
    public static partial class UserConfig
    {
        //设备是否含有视觉处理
        public static bool bHaveVissionProcess = false;

        public static bool ReadVisionData()
        {
            //视觉初始化
            string strVisionConfigPath = AppDomain.CurrentDomain.BaseDirectory + @"config\Vision\" + "VisionMgr" + ".xml";
            VisionMgr.GetInstance().CurrentVisionProcessDir = ParamSetMgr.GetInstance().CurrentWorkDir + "\\" + ParamSetMgr.GetInstance().CurrentProductFile + "\\" + @"Config\Vision\";
            VisionMgr.GetInstance().Read();
            if (VisionMgr.GetInstance().dicVisionType.Count == 0 && bHaveVissionProcess)
            {
                AlarmMgr.GetIntance().WarnWithDlg("视觉处理文件文件丢失", null, CommonDlg.DlgWaranType.WaranOK, null, true);
            }
            return true;
        }
    }
}