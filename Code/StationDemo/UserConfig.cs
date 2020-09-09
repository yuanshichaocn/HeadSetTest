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
using LightControler;
using CameraLib;
using OtherDevice;
using XYZDispensVision;
using MachineSafe;


namespace StationDemo
{
    public static class UserConfig
    {
        public static void CloseHardWork()
        {
            //for (int i = 0; i < 6; i++)
            //    LightControl.GetInstance().CloseLight(i);
            CameraMgr.GetInstance().Close();
            TcpMgr.GetInstance().CloseAllEth();
            MotionMgr.GetInstace().Close();
            IOMgr.GetInstace().Close();
            //  LightControl.GetInstance().Close();
            //  Weighing.GetInstance().Close();

        }


        public static void AddStation()
        {
         
            //添加工站
            StationMgr.GetInstance().AddStation(new StationForm(), "流水线", new LineStation(StationMgr.GetInstance().GetStation("流水线")));
            StationMgr.GetInstance().AddStation(new StationFormRobot(), "机械手", new LineStation(StationMgr.GetInstance().GetStation("机械手")));


            Dictionary<Form, Stationbase> keyValuePairs = StationMgr.GetInstance().GetDicStaion();
        }
        public static void InitHalconWindow(Form_Auto formauto)
        {

        }
        public static void BandStationWithVisionCtr(Form_Auto formauto)
        {
            //formauto.visionControl1.InitWindow();
            //formauto.visionControl2.InitWindow();
            //formauto.visionControl3.InitWindow();

            //formauto.visionControl4.InitWindow();
            //formauto.visionControl5.InitWindow();

        }
        public static void InitCam(Form_Auto formauto)
        {
            List<CameraBase> cameraBases = new List<CameraBase>();
           // int nCamNum = CameraMgr.GetInstance().EnumDevices(new BaslerCamera(""), out cameraBases);
            foreach (var temp in cameraBases)
            {

                temp.Open();
                temp.SetExposureTime(1000);
                temp.SetGain(1);
                temp.RegisterCallBack();
                temp.SetAcquisitionMode();

                temp.StartGrab();
                //if (temp.Name == "FDCCD")
                //    temp.BindWindow(formauto.visionControl1);
                //if (temp.Name == "RUCCD")
                //    temp.BindWindow(formauto.visionControl2);
                //if (temp.Name == "DispCCD")
                //    temp.BindWindow(formauto.visionControl5);
               
            }
          

        }


       
        public static void BandFlushAutoScreen(Form_Auto form_Auto)
        {
            //   Form_Auto.ShowEventOnAutoScreen+=
        }

        public static void InitHandWareWithUI(Form_Auto form_Auto)
        {


        }
        /// <summary>
        /// 增加自动界面 标志位
        /// </summary>
        /// <param name="formauto"></param>
        public static void AddFlag(Form_Auto formauto)
        {




            formauto.AddFlag("系统空跑", sys.g_AppMode == AppMode.AirRun);
        }
        /// <summary>
        /// 初始其他硬件状态硬件状态
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
            //if (!Directory.Exists(@"E:\设备数据\"))
            //    Directory.CreateDirectory(@"E:\设备数据\");

        }

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
        


        public static void UpdataTrayData()
        {
            return;
            for (int index = 1; index <= TrayMgr.GetInstance().trayDataLoadArr.Length; index++)
            {
                try
                {
                    if(index<= 25)
                    {
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].eSparepart = (Sparepart)Enum.Parse(typeof(Sparepart),
                           ParamSetMgr.GetInstance().GetStringParam($"料盘{index}类型"));
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].RowCount = ParamSetMgr.GetInstance().GetIntParam($"料盘{index}行数");
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].ColCount = ParamSetMgr.GetInstance().GetIntParam($"料盘{index}列数");
                        UpdataTrayData("取料站", $"料盘_{index}_M_SP");
                        UpdataTrayData("取料站", $"料盘_{index}_M_EP");
                        UpdataTrayData("取料站", $"料盘_{index}_V_SP");
                        UpdataTrayData("取料站", $"料盘_{index}_V_EP");
                        UpdataTrayData("Barrel站", $"料盘_{index}_M_SP");
                        UpdataTrayData("Barrel站", $"料盘_{index}_M_EP");
                        UpdataTrayData("Barrel站", $"料盘_{index}_V_SP");
                        UpdataTrayData("Barrel站", $"料盘_{index}_V_EP");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //设备是否含有视觉处理
        public static bool bHaveVissionProcess = false;
        /// <summary>
        /// 初始化产品数据参数
        /// </summary>
        public static void InitProductOnAutoScreenLoad()
        {
            //视觉初始化
            string strVisionConfigPath = AppDomain.CurrentDomain.BaseDirectory + @"config\Vision\" + "VisionMgr" + ".xml";
            VisionMgr.GetInstance().CurrentVisionProcessDir = ParamSetMgr.GetInstance().CurrentWorkDir + "\\" + ParamSetMgr.GetInstance().CurrentProductFile + "\\" + @"Config\Vision\";
            VisionMgr.GetInstance().Read();
            if (VisionMgr.GetInstance().dicVisionType.Count == 0 && bHaveVissionProcess)
            {
                AlarmMgr.GetIntance().WarnWithDlg("视觉处理文件文件丢失", null, CommonDlg.DlgWaranType.WaranOK, null, true);
                /// MessageBox.Show("视觉处理文件文件丢失", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            NozzleMgr.GetInstance().Read();

            //标定初始化
            
        }
        public static void InitProductParamOnMainScreenLoad()
        {
        }


        public static void UpdataBarrelTrayData(string strStationName, string posName)
        {
            if ("收放料Barrel" == strStationName && posName.Contains("料盘"))
            {

                for (int index = 1; index < 3; index++)
                {
                    try
                    {
                        if (posName.Contains($"_{index}_M_SP"))
                        {
                            TrayMgr.GetInstance().trayDataLoadArr[index - 1].PlaceLeftTopcoordinate =
                            TrayMgr.GetInstance().trayDataLoadArr[index - 1].PickLeftTopcoordinate =
                               new Coordinate()
                               {
                                   X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                                   Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                                   Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                               };
                            TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init2();

                        }
                        if (posName.Contains($"_{index}_M_EP"))
                        {
                            TrayMgr.GetInstance().trayDataLoadArr[index - 1].PlaceRightBottomcoordinate =
                            TrayMgr.GetInstance().trayDataLoadArr[index - 1].PickRightBottomcoordinate =
                            new Coordinate()
                            {
                                X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                                Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                                Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                            };
                            TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init2();
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }

            }
        }
        public static void UpdataTrayData(string strStationName, string posName)
        {
            if (("取料站" == strStationName || "Barrel站" == strStationName) && posName.Contains("料盘"))
            {

               if(StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic().ContainsKey(posName))
                {
                    int indexofstring = posName.IndexOf("_");
                    string sub = posName.Substring(indexofstring + 1);
                    indexofstring = sub.IndexOf("_");
                    sub = sub.Substring(0, indexofstring);
                    int index = sub.ToInt();
                    if (posName.Contains($"_M_SP"))
                    {

                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].PlaceLeftTopcoordinate =
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].PickLeftTopcoordinate =
                           new Coordinate()
                           {
                               X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                               Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                               Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                           };
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init2();
                    }
                    if (posName.Contains($"_M_EP"))
                    {
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].PlaceRightBottomcoordinate =
                            TrayMgr.GetInstance().trayDataLoadArr[index - 1].PickRightBottomcoordinate =
                            new Coordinate()
                            {
                                X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                                Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                                Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                            };
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init2();
                    }
                    if (posName.Contains($"_V_SP"))
                    {

                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].SnapLeftTopcoordinate =
                           new Coordinate()
                           {
                               X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                               Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                               Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                           };
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init2();
                    }
                    if (posName.Contains($"_V_EP"))
                    {
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].SnapRightBottomcoordinate =
                            new Coordinate()
                            {
                                X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                                Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                                Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                            };
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init2();
                    }
                }
            }
        }


    }
}
