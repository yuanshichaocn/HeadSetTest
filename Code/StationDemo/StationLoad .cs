using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTools;
using BaseDll;
using MotionIoLib;
using UserData;
using System.Threading;
using HalconDotNet;
using UserCtrl;
using VisionProcess;
using System.Runtime.Remoting.Messaging;
using CameraLib;
using System.Windows.Forms;
using System.Diagnostics;
using LightControler;

namespace StationDemo
{
    public class StationLoad : Stationbase
    {
        const int dFineDistance = 10;//10个plus
        const int nSeparateCount = 10;
        const int nResolutionZ = 10000;
        const int nResolutionY = 10000;
        VisionSetpBase[] visionSetpBases = new VisionSetpBase[] { null, null, null, null };

        public StationLoad(Stationbase stationbase) : base(stationbase)
        {
            m_listIoInput.Add("上料吸嘴气缸原位");
            m_listIoInput.Add("上料吸嘴气缸到位");
            m_listIoInput.Add("上料位开SOCKET气缸原位");
            m_listIoInput.Add("上料位开SOCKET气缸到位");
            m_listIoInput.Add("上料位夹紧SOCKET气缸原位");
            m_listIoInput.Add("上料位夹紧SOCKET气缸到位");
   
            m_listIoInput.Add("上料位SOCKET真空检测1");
            m_listIoInput.Add("上料位SOCKET真空检测2");
            m_listIoInput.Add("上料位SOCKET真空检测3");
            m_listIoInput.Add("上料位SOCKET真空检测4");
            m_listIoInput.Add("上料位SOCKET真空检测5");
            m_listIoInput.Add("上料位SOCKET真空检测6");
            m_listIoInput.Add("上料位SOCKET真空检测7");
            m_listIoInput.Add("上料位SOCKET真空检测8");
            m_listIoInput.Add("上料位电机升降气缸原位");
            m_listIoInput.Add("上料位电机升降气缸到位");
            m_listIoInput.Add("上料位定位气缸原位");
            m_listIoInput.Add("上料位定位气缸到位");
            m_listIoInput.Add("上料位阻挡气缸原位");
            m_listIoInput.Add("上料位阻挡气缸到位");
            m_listIoInput.Add("上料吸嘴真空检测");


            m_listIoOutput.Add("上料吸头气缸伸出电磁阀");
            m_listIoOutput.Add("上料吸头气缸退回电磁阀");
            m_listIoOutput.Add("上料开SOCKET电磁阀");
            m_listIoOutput.Add("上料夹紧SOCKET气缸电磁阀");
           

            m_listIoOutput.Add("上料真空吸电磁阀");
            m_listIoOutput.Add("上料破真空电磁阀");
            m_listIoOutput.Add("上料真空吸1电磁阀");
            m_listIoOutput.Add("上料真空吸2电磁阀");
            m_listIoOutput.Add("上料真空吸3电磁阀");
            m_listIoOutput.Add("上料真空吸4电磁阀");
            m_listIoOutput.Add("上料真空吸5电磁阀");
            m_listIoOutput.Add("上料真空吸6电磁阀");
            m_listIoOutput.Add("上料真空吸7电磁阀");
            m_listIoOutput.Add("上料真空吸8电磁阀");

            m_listIoOutput.Add("上料定位电机升降气缸电磁阀");
            m_listIoOutput.Add("上料位定位气缸电磁阀");
            m_listIoOutput.Add("上料位阻挡气缸电磁阀");

            if (VisionMgr.GetInstance().dicVision.ContainsKey("方向判定"))
            {
                visionSetpBases[0] = VisionMgr.GetInstance().dicVision["方向判定"];
                visionSetpBases[1] = visionSetpBases[0].Clone();
                visionSetpBases[2] = visionSetpBases[0].Clone();
                visionSetpBases[3] = visionSetpBases[0].Clone();
            }



            VisionMgr.GetInstance().PrItemChangedEvent += (prname) =>{
                if (VisionMgr.GetInstance().dicVision.ContainsKey("方向判定")  && prname== "方向判定")
                {
                    visionSetpBases[0] = VisionMgr.GetInstance().dicVision["方向判定"];
                    visionSetpBases[1] = visionSetpBases[0].Clone();
                    visionSetpBases[2] = visionSetpBases[0].Clone();
                    visionSetpBases[3] = visionSetpBases[0].Clone();
                }
            };

        }
        public enum StationStep
        {
            step_init,
            step_CheckInPos,
            step_Pick,
            step_CheckSocketLineInPos,
            step_Snap,

            step_place,
            step_WaitallVisionProcessFinish,



        }

        void DoSomethingWhenalarm()
        {

        }
        protected override bool InitStation()
        {
            if (VisionMgr.GetInstance().dicVision.ContainsKey("方向判定"))
            {
                visionSetpBases[0] = VisionMgr.GetInstance().dicVision["方向判定"];
                visionSetpBases[1] = visionSetpBases[0].Clone();
                visionSetpBases[2] = visionSetpBases[0].Clone();
                visionSetpBases[3] = visionSetpBases[0].Clone();
            }
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.LoadNozzle].nozzleState = NozzleState.None;
            TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].index = 0;
            TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index = 0;
            AlarmMgr.GetIntance().DoWhenAlarmEvent += DoSomethingWhenalarm;
            ParamSetMgr.GetInstance().SetBoolParam("上料站复位成功", false);
            PushMultStep((int)StationStep.step_init);
            ParamSetMgr.GetInstance().SetBoolParam("上料站Pr结束", false);
            ParamSetMgr.GetInstance().SetBoolParam("Socket流水线初始化完成", false);
  
            return true;
        }
        public void PickFromTray(int index, bool bmanual = false)
        {
            WaranResult waranResult;

        retry_pick_check:

            IOMgr.GetInstace().WriteIoBit("上料破真空电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
            waranResult = CheckIobyName("上料吸嘴真空检测", false, "上料站： 上料吸嘴可能被堵住，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_pick_check;

         

            IOMgr.GetInstace().WriteIoBit("上料位定位气缸电磁阀", true);
            waranResult = CheckIobyName("上料位定位气缸到位", true, "上料站： 上料位定位气缸到位(下降)失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_pick_check;

            IOMgr.GetInstace().WriteIoBit("上料吸头气缸退回电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸伸出电磁阀", false);
            waranResult = CheckIobyName("上料吸嘴气缸原位", true, "上料站： 上料吸头气缸退回失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_pick_check;


            retry_pick_go:
            double x = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].trayCells[index].Pickcoordinate.X;
            double y = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].trayCells[index].Pickcoordinate.Y;
            #region 安全过渡
            double safePosx = GetStationPointDic()["安全位置"].pointX;
            double safePosy = GetStationPointDic()["安全位置"].pointY;
            bool bSameRegion = (MotionMgr.GetInstace().GetAxisPos(AxisY) <= safePosy && y <= safePosy) || (MotionMgr.GetInstace().GetAxisPos(AxisY) > safePosy && y > safePosy);
            if (!bSameRegion && MotionMgr.GetInstace().GetAxisPos(AxisX) < safePosx)
            {
                Info(string.Format("{0}当前X位置：{1}<安全坐标X轴位置：{2}，去安全位置Y坐标 过渡 在去抓料位置：X:{3} Y:{4}",
                    bmanual ? "手动" : "自动", MotionMgr.GetInstace().GetAxisPos(AxisX), safePosx, x, y));
                waranResult = MoveSigleAxisPosWaitInpos(AxisX, safePosx, (double)SpeedType.High, 10, bmanual, bmanual ? null : this, 60000);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("上料站： 去抓取位置过程中,去安全位置X位置失败", this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_pick_go;
                }
            }
            if (!bSameRegion && x < safePosx)
            {
                Info(string.Format("{0}X轴目标位置：{1}<安全坐标X轴位置：{2}，去安全位置Y坐标 过渡 在去抓料位置：X:{3} Y:{4}", bmanual ? "手动" : "自动", x, safePosx, x, y));
                waranResult = MoveSigleAxisPosWaitInpos(AxisY, safePosy, (double)SpeedType.High, 10, bmanual, bmanual ? null : this, 60000);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("上料站： 去抓取位置过程中,去安全位置Y位置失败", this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_pick_go;
                }
            }
            #endregion 安全过渡
            Info(string.Format("{0} 去抓取{1}格子物料 ", bmanual ? "手动" : "自动", index * 8));
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this, 60000);


            //retry_pick_opensockkt:
            //IOMgr.GetInstace().WriteIoBit("上料开SOCKET电磁阀", true);
            //waranResult = CheckIobyName("上料位夹紧SOCKET气缸到位", true, "上料站： socket治具打开失败，请拿开", bmanual);
            //if (waranResult == WaranResult.Retry)
            //    goto retry_pick_opensockkt;

            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料破真空电磁阀", false);

        retry_pick_stretchout:
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸退回电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸伸出电磁阀", true);
            waranResult = CheckIobyName("上料吸嘴气缸到位", true, "上料站： 上料吸嘴气缸伸出失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_pick_stretchout;

            retry_pick_suck_up:
            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("吸真空延时"));


        retry_pick_stretchback:
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸退回电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸伸出电磁阀", false);
            waranResult = CheckIobyName("上料吸嘴气缸原位", true, "上料站： 上料吸嘴气缸退回失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_pick_stretchback;

            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
            waranResult = CheckIobyName("上料吸嘴真空检测", true, "上料站： 上料吸嘴可能料掉了，请检查", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_pick_stretchout;

            //retry_pick_closesockkt:
            //IOMgr.GetInstace().WriteIoBit("上料开SOCKET电磁阀", false);
            //waranResult = CheckIobyName("上料位夹紧SOCKET气缸原位", true, "上料站： socket治具打开失败，请拿开", bmanual);
            //if (waranResult == WaranResult.Retry)
            //    goto retry_pick_closesockkt;

            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.LoadNozzle].nozzleState = NozzleState.Have;
            int s = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index;
            TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index + 1;
        }
        bool CheckSocketVacuum(bool bmanual = false)
        {
            WaranResult waranResult;
        retry_place_check:
            IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测1", false, "上料站： 上料位SOCKET真空检测1失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测2", false, "上料站： 上料位SOCKET真空检测2失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测3", false, "上料站： 上料位SOCKET真空检测3失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测4", false, "上料站： 上料位SOCKET真空检测4失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测5", false, "上料站： 上料位SOCKET真空检测5失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测6", false, "上料站： 上料位SOCKET真空检测6失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            return true;

        }

        public void MoveToPlacePos(bool bmanual = false)
        {
            WaranResult waranResult;

        retry_place_check:
            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
            waranResult = CheckIobyName("上料吸嘴真空检测", true, "上料站： 上料吸嘴料可能丢失，请检查", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测1", false, "上料站： 上料位SOCKET真空检测1失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测2", false, "上料站： 上料位SOCKET真空检测2失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测3", false, "上料站： 上料位SOCKET真空检测3失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测4", false, "上料站： 上料位SOCKET真空检测4失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测5", false, "上料站： 上料位SOCKET真空检测5失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测6", false, "上料站： 上料位SOCKET真空检测6失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸7电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测7", false, "上料站： 上料位SOCKET真空检测7失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;


            IOMgr.GetInstace().WriteIoBit("上料真空吸8电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测8", false, "上料站： 上料位SOCKET真空检测8失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸7电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸8电磁阀", false);

        retry_pick_stretchback:
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸退回电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸伸出电磁阀", false);
            waranResult = CheckIobyName("上料吸嘴气缸原位", true, "上料站： 上料吸嘴气缸退回失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_pick_stretchback;
            double x = GetStationPointDic()["放料位"].pointX;
            double y = GetStationPointDic()["放料位"].pointY;
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this);

        }

        public bool CheckLineReadly()
        {
            return SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketState == SocketState.None;

        }

        public void PlaceToSocket(bool bmanual = false)
        {
            WaranResult waranResult;
            IOMgr.GetInstace().WriteIoBit("上料夹紧SOCKET气缸电磁阀", true);
        // IOMgr.GetInstace().WriteIoBit("上料开SOCKET电磁阀", true);
        retry_place_check:
            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
            waranResult = CheckIobyName("上料吸嘴真空检测", true, "上料站： 上料吸嘴料可能丢失，请检查", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
            waranResult = CheckIobyName("上料吸嘴真空检测", true, "上料站： 上料吸嘴料可能丢失，请检查", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测1", false, "上料站： 上料位SOCKET真空检测1失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测2", false, "上料站： 上料位SOCKET真空检测2失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测3", false, "上料站： 上料位SOCKET真空检测3失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测4", false, "上料站： 上料位SOCKET真空检测4失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测5", false, "上料站： 上料位SOCKET真空检测5失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;
            IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测6", false, "上料站： 上料位SOCKET真空检测6失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸7电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测7", false, "上料站： 上料位SOCKET真空检测7失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;

            IOMgr.GetInstace().WriteIoBit("上料真空吸8电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测8", false, "上料站： 上料位SOCKET真空检测8失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check;


            IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸7电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸8电磁阀", false);

        retry_pick_stretchback:
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸退回电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸伸出电磁阀", false);
            waranResult = CheckIobyName("上料吸嘴气缸原位", true, "上料站： 上料吸嘴气缸退回失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_pick_stretchback;

            retry_place_check_Clamp:
            IOMgr.GetInstace().WriteIoBit("上料夹紧SOCKET气缸电磁阀", true);
            waranResult = CheckIobyName("上料位夹紧SOCKET气缸到位", true, "上料站： 上料位夹紧SOCKET气缸原位 到位失败，请检查", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_check_Clamp;

            retry_place_MotorCliderUp:
            IOMgr.GetInstace().WriteIoBit("上料定位电机升降气缸电磁阀", true);
            waranResult = CheckIobyName("上料位电机升降气缸到位", true, "上料站： 上料位电机升降气缸到位（上升）失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_place_MotorCliderUp;

            
            double x = GetStationPointDic()["放料位"].pointX;
            double y = GetStationPointDic()["放料位"].pointY;
        #region 安全过渡
        retry_safe_go:
            double safePosx = GetStationPointDic()["安全位置"].pointX;
            double safePosy = GetStationPointDic()["安全位置"].pointY;
            bool bSameRegion = (MotionMgr.GetInstace().GetAxisPos(AxisY) <= safePosy && y <= safePosy) || (MotionMgr.GetInstace().GetAxisPos(AxisY) > safePosy && y > safePosy);
            if (!bSameRegion && MotionMgr.GetInstace().GetAxisPos(AxisX) < safePosx)
            {
                Info(string.Format("{0}当前X位置：{1}<安全坐标X轴位置：{2}，去安全位置Y坐标 过渡 在去抓料位置：X:{3} Y:{4}",
                    bmanual ? "手动" : "自动", MotionMgr.GetInstace().GetAxisPos(AxisX), safePosx, x, y));
                waranResult = MoveSigleAxisPosWaitInpos(AxisX, safePosx, (double)SpeedType.High, 10, bmanual, bmanual ? null : this, 60000);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("上料站： 去抓取位置过程中,去安全位置X位置失败", this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_safe_go;
                }
            }
            if (!bSameRegion && x < safePosx)
            {
                Info(string.Format("{0}X轴目标位置：{1}<安全坐标X轴位置：{2}，去安全位置Y坐标 过渡 在去抓料位置：X:{3} Y:{4}", bmanual ? "手动" : "自动", x, safePosx, x, y));
                waranResult = MoveSigleAxisPosWaitInpos(AxisY, safePosy, (double)SpeedType.High, 10, bmanual, bmanual ? null : this, 60000);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("上料站： 去抓取位置过程中,去安全位置Y位置失败", this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_safe_go;
                }
            }
            #endregion 安全过渡

            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this);

            DoWhile doWhile = new DoWhile(
                (ntime, dowhile, bIsmanual, obj) =>
                {
                    if (bIsmanual || SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketState == SocketState.None)
                    {
                        Info("上料站： 8个底座开始吸料");
                        IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", true);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", true);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", true);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", true);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", true);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", true);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸7电磁阀", true);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸8电磁阀", true);
                    retry_place_opensocket:
                        IOMgr.GetInstace().WriteIoBit("上料开SOCKET电磁阀", true);
                        waranResult = CheckIobyName("上料位开SOCKET气缸到位", true, "上料站： 上料位开SOCKET气缸到位（上升）失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_opensocket;
                        retry_place_stretchout:
                        IOMgr.GetInstace().WriteIoBit("上料吸头气缸退回电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料吸头气缸伸出电磁阀", true);
                        waranResult = CheckIobyName("上料吸嘴气缸到位", true, "上料站： 上料吸嘴气缸到位（伸出）失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_stretchout;
                        IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料破真空电磁阀", true);
                        ParamSetMgr.GetInstance().GetIntParam("吸真空延时");


                    retry_place_stretchback:
                        IOMgr.GetInstace().WriteIoBit("上料吸头气缸退回电磁阀", true);
                        IOMgr.GetInstace().WriteIoBit("上料吸头气缸伸出电磁阀", false);
                        waranResult = CheckIobyName("上料吸嘴气缸原位", true, "上料站： 上料吸嘴气缸原位（退回）失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_stretchback;

                        retry_place_down_check:
                        IOMgr.GetInstace().WriteIoBit("上料破真空电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
                        waranResult = CheckIobyName("上料吸嘴真空检测", false, "上料站： 上料吸嘴可能被堵住，物料未脱离，请检查真空值及物料气管", bmanual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_down_check;

                        Info("上料站：关开Socket  定位黑帽到初始位");
                        retry_place_closesocket:
                        IOMgr.GetInstace().WriteIoBit("上料开SOCKET电磁阀", false);
                        waranResult = CheckIobyName("上料位开SOCKET气缸原位", true, "上料站： 上料位开SOCKET气缸原位（夹紧）失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_closesocket;
                        Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("关Socket延时"));
                    retry_place_opensocket2:
                        IOMgr.GetInstace().WriteIoBit("上料开SOCKET电磁阀", true);
                        waranResult = CheckIobyName("上料位开SOCKET气缸到位", true, "上料站： 上料位开SOCKET气缸到位（松开）失败，请检查线路 气路 和上料吸头气缸是否卡住", bmanual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_opensocket2;
                        // int index = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index;
                        //  double xpos = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].trayCells[index].Pickcoordinate.X;
                        //   double ypos = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].trayCells[index].Pickcoordinate.Y;
                        //   Info(string.Format("{0} 去抓取{1}格子物料 ", bmanual ? "手动" : "自动", index * 8));
                        //   MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { xpos, ypos }, new double[] { (double)SpeedType.High, (double)SpeedType.High });
                        // NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.LoadNozzle].nozzleState = NozzleState.None;
                        // SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketState = SocketState.Have;
                        return WaranResult.Run;
                    }
                    else
                        return WaranResult.CheckAgain;
                }, int.MaxValue);
            doWhile.doSomething(bmanual ? null : this, doWhile, bmanual, null);

            for (int i = 0; i < 8; i++)
            {
                SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[i].Cellstate = SocketCellState.CellStateNone;
                SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[i].Cellstate2 = SocketCellState.CellStateNone;
            }

            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料破真空电磁阀", false);
            double ProductNumDone = ParamSetMgr.GetInstance().GetDoubleParam("产品计数");
            ParamSetMgr.GetInstance().SetDoubleParam("产品计数", ProductNumDone + 1);
        }

        public double JudgeAngle(double Degori)
        {
            double deg1 = Math.Abs(Degori);
            double deg2 = 360 - deg1;
            double deg = deg1 >= deg2 ? deg2 : deg1;

            if (Degori > 0 && deg1 < 180)
                return deg;
            if (Degori > 0 && deg1 > 180)
                return -deg;
            if (Degori < 0 && deg1 < 180)
                return -deg;
            if (Degori < 0 && deg1 > 180)
                return deg;
            return deg;

        }
      
        bool pr(string strVisionPrName, HObject img, int index, VisionControl visionControl)
        {
            visionSetpBases[index].ClearResult();
            Stopwatch sp = new Stopwatch();
            sp.Restart();
            if (img != null && img.IsInitialized())
                visionSetpBases[index].Process_image(img, visionControl);
            else
                MessageBox.Show("图片不在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            img.Dispose();
            Info(string.Format("上料站：方向判断{0} 花费时间{1} ms", index+1, sp.ElapsedMilliseconds));
            //switch ((int)index)
            //{
            //    case 1:
            //        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)index), true);
            //        break;
            //    case 2:
            //        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)index), true);
            //        break;
            //    case 3:
            //        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)index), true);
            //        break;
            //    case 4:
            //        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)index), true);
            //        break;
            //}
            //VisionMgr.GetInstance().ProcessImage(strVisionPrName, (HObject)img, visionControl);
            return true;
        }
        public override void StopDeal()
        {
            LightControl.GetInstance().CloseLight("上料方向判定");
        }
        public override void ExctionDeal(string strmsg)
        {
            LightControl.GetInstance().CloseLight("上料方向判定");
        }
        public void Init(bool bmanual = false)
        {
            WaranResult waranResult;
            Info("上料站: 气缸开始自检");
        retry_init:
            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
            Thread.Sleep(10);
            waranResult = CheckIobyName("上料吸嘴真空检测", false, "上料站： 上料吸嘴可能被堵住，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;

            IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料破真空电磁阀", false);

            IOMgr.GetInstace().WriteIoBit("上料吸头气缸退回电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("上料吸头气缸伸出电磁阀", false);
            waranResult = CheckIobyName("上料吸嘴气缸原位", true, "上料站： 上料吸头退回卡住或者感应器不到位", bmanual);

            IOMgr.GetInstace().WriteIoBit("上料位定位气缸电磁阀", false);
            waranResult = CheckIobyName("上料位定位气缸原位", true, "上料站： 上料位定位气缸上身失败，请检查感应器 或者气缸卡住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;
            IOMgr.GetInstace().WriteIoBit("上料定位电机升降气缸电磁阀", false);
            waranResult = CheckIobyName("上料位电机升降气缸原位", true, "上料站： 上料位电机升降气缸上升失败，请检查感应器 或者气缸卡住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;
            IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测1", false, "上料站： 上料位SOCKET真空检测1失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;
            IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测2", false, "上料站： 上料位SOCKET真空检测2失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;
            IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测3", false, "上料站： 上料位SOCKET真空检测3失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;
            IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测4", false, "上料站： 上料位SOCKET真空检测4失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;
            IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测5", false, "上料站： 上料位SOCKET真空检测5失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;
            IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测6", false, "上料站： 上料位SOCKET真空检测6失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;

            IOMgr.GetInstace().WriteIoBit("上料真空吸7电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测7", false, "上料站： 上料位SOCKET真空检测7失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;

            IOMgr.GetInstace().WriteIoBit("上料真空吸8电磁阀", true);
            waranResult = CheckIobyName("上料位SOCKET真空检测8", false, "上料站： 上料位SOCKET真空检测8失败，请检查感应器 或者被堵住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;

            IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸7电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("上料真空吸8电磁阀", false);


            IOMgr.GetInstace().WriteIoBit("上料定位电机升降气缸电磁阀", false);
            waranResult = CheckIobyName("上料位电机升降气缸原位", true, "上料站： 上料位电机升降气缸原位(下降)失败，请检查感应器 或者气缸卡住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;

            IOMgr.GetInstace().WriteIoBit("上料位定位气缸电磁阀", false);
            waranResult = CheckIobyName("上料位定位气缸原位", true, "上料站： 上上料位定位气缸原位失败，请检查感应器 或者气缸卡住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;

            IOMgr.GetInstace().WriteIoBit("上料开SOCKET电磁阀", false);
            waranResult = CheckIobyName("上料位开SOCKET气缸原位", true, "上料站： 上料位开SOCKET气缸原位失败，请检查感应器 或者气缸卡住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;

            IOMgr.GetInstace().WriteIoBit("上料位定位气缸电磁阀", true);
            waranResult = CheckIobyName("上料位定位气缸到位", true, "上料站： 上料位定位气缸到位失败，请检查感应器 或者气缸卡住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;



            IOMgr.GetInstace().WriteIoBit("上料夹紧SOCKET气缸电磁阀", false);
            waranResult = CheckIobyName("上料位夹紧SOCKET气缸原位", true, "上料站： 上料位夹紧SOCKET气缸原位 失败，请检查感应器 或者气缸卡住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_init;
            Retry_X_Home:
            waranResult = HomeSigleAxisPosWaitInpos(AxisX, bmanual ? null : this, 60000, bmanual);
            if (waranResult != WaranResult.Run)
            {
                waranResult = AlarmMgr.GetIntance().WarnWithDlg("上料站 归零不成功： " + string.Format("{0} 轴回零失败", MotionMgr.GetInstace().GetAxisName(AxisX)), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto Retry_X_Home;
            }
        Retry_Y_Home:
            waranResult = HomeSigleAxisPosWaitInpos(AxisY, bmanual ? null : this, 60000, bmanual);
            if (waranResult != WaranResult.Run)
            {
                waranResult = AlarmMgr.GetIntance().WarnWithDlg("上料站 归零不成功： " + string.Format("{0} 轴回零失败", MotionMgr.GetInstace().GetAxisName(AxisY)), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto Retry_Y_Home;
            }
           retry_press:
            IOMgr.GetInstace().WriteIoBit("上料位定位气缸电磁阀", false);
            waranResult = CheckIobyName("上料位定位气缸原位", true, "上料站： 上料位定位气缸原位到位失败，请检查感应器 或者气缸卡住，检查线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_press;
            ParamSetMgr.GetInstance().SetBoolParam("上料站复位成功", true);
            DoWhile doWhile = new DoWhile((time, dowhile, banual, obj) => {
                if (time > 25000)
                    return AlarmMgr.GetIntance().WarnWithDlg("Socket流水线XY气缸自检超时请检查  线路气路感应器 ", this, CommonDlg.DlgWaranType.Waran_Stop_Retry);
                else if (ParamSetMgr.GetInstance().GetBoolParam("Socket流水线初始化完成"))
                {
                    return WaranResult.Run;
                }
                else
                {
                    return WaranResult.CheckAgain;
                }


            }, 300000);
            if( !bmanual)
            {
                retry_check:
                doWhile.doSomething(this, doWhile, bmanual, null);
                if (waranResult == WaranResult.Retry)
                    goto retry_check;
            }
 
        }
        int oldStep = 0;
        Tuple<double, double>[] tupleDegResultArr = new Tuple<double, double>[4];
        public bool GetAllStepMotorState()
        {
            AxisState axisState;
            bool ballstepState = true;
            for (int i = 8; i < 8 + 8; i++)
                ballstepState &= MotionMgr.GetInstace().IsAxisNormalStop(i) == AxisState.NormalStop;
            return ballstepState;
        }
        cUserTimer cUserTimerLastPr = new cUserTimer(200);
        Tuple<int, int, int, int>[] tupleSnapposWithAxis = new Tuple<int, int, int, int>[] {
            new Tuple<int, int, int, int>(8,9,0,1),
            new Tuple<int, int, int, int>(12,13,4,5),
            new Tuple<int, int, int, int>(10,11,2,3),
            new Tuple<int, int, int, int>(14,15,6,7)
        };
        public void SnapAndRotate(int i, VisionControl  visctr,bool bmanual =false)
        {
            WaranResult waranResult;
            LightControl.GetInstance().Light("上料方向判定");
            ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", i + 1), false);
            Info(string.Format("上料站：开始去{0}拍照位置", i + 1));
           

        #region 安全过渡
        retry_pick_go:
            double snapx = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].trayCells[i].Snapcoordinate.X;
            double snapy = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].trayCells[i].Snapcoordinate.Y;
            double x = snapx;
            double y = snapy;
            double safePosx = GetStationPointDic()["安全位置"].pointX;
            double safePosy = GetStationPointDic()["安全位置"].pointY;
            bool bSameRegion = (MotionMgr.GetInstace().GetAxisPos(AxisY) <= safePosy && y <= safePosy) || (MotionMgr.GetInstace().GetAxisPos(AxisY) > safePosy && y > safePosy);
            if (!bSameRegion && MotionMgr.GetInstace().GetAxisPos(AxisX) < safePosx)
            {
                Info(string.Format("{0}当前X位置：{1}<安全坐标X轴位置：{2}，去安全位置Y坐标 过渡 在去抓料位置：X:{3} Y:{4}",
                    bmanual ? "手动" : "自动", MotionMgr.GetInstace().GetAxisPos(AxisX), safePosx, x, y));
                waranResult = MoveSigleAxisPosWaitInpos(AxisX, safePosx, (double)SpeedType.High, 10, bmanual, bmanual ? null : this, 60000);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("上料站： 去抓取位置过程中,去安全位置X位置失败", this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_pick_go;
                }
            }
            if (!bSameRegion && x < safePosx)
            {
                Info(string.Format("{0}X轴目标位置：{1}<安全坐标X轴位置：{2}，去安全位置Y坐标 过渡 在去抓料位置：X:{3} Y:{4}", bmanual ? "手动" : "自动", x, safePosx, x, y));
                waranResult = MoveSigleAxisPosWaitInpos(AxisY, safePosy, (double)SpeedType.High, 10, bmanual, bmanual ? null : this, 60000);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("上料站： 去抓取位置过程中,去安全位置Y位置失败", this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_pick_go;
                }
            }
            #endregion 安全过渡
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { snapx, snapy }, new double[] { (double)SpeedType.Mid, (double)SpeedType.Mid }, 10, bmanual);
            //Task<int> task = new Task<int>(new Func<object,int>((ncount)=> { return 1; },1));
            //  Func<int, bool> S = new Func<int, bool>((ncount) => { return true; });
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));
            Info(string.Format("上料站：{0}拍照位置到达", i + 1));
            Stopwatch sp = new Stopwatch();
            sp.Restart();
            HObject img = null;

        retry_snap:
            string camname = VisionMgr.GetInstance().GetCamName("方向判定");
            CameraMgr.GetInstance().GetCamera(camname).BindWindow(visctr);
            CameraMgr.GetInstance().SetTriggerSoftMode(camname/*"LoadCCD"*/);
            double expourseTime = (double)VisionMgr.GetInstance().GetExpourseTime("方向判定");
            double gain = (double)VisionMgr.GetInstance().GetGain("方向判定");
            CameraMgr.GetInstance().SetCamGain(camname/*"LoadCCD"*/, gain);
            CameraMgr.GetInstance().SetCamExposure(camname/*"LoadCCD"*/, expourseTime);

           // CameraMgr.GetInstance().GetCamera(camname/*"LoadCCD"*/).GarbBySoftTrigger();
            Info(string.Format("上料站：方向判断{0} 取图像花费1时间{1} ms", i+1, sp.ElapsedMilliseconds));
            img = CameraMgr.GetInstance().GetImg(camname);
            if (img == null)
            {
             
                CameraMgr.GetInstance().GetCamera(camname).GarbBySoftTrigger();
                Thread.Sleep(40);
                img = CameraMgr.GetInstance().GetImg(camname);
            }
            DoWhile doWhile = new DoWhile((time, dowhile, bmanual2, obj) =>
            {
                if (img != null && img.IsInitialized())
                    return WaranResult.Run;
             
                CameraMgr.GetInstance().GetCamera(camname/*"LoadCCD"*/).GarbBySoftTrigger();
             
                img = CameraMgr.GetInstance().GetImg(camname);
                if (img != null && img.IsInitialized())
                    return WaranResult.Run;
                if (time > 3000)
                    return AlarmMgr.GetIntance().WarnWithDlg("上料站：取图失败", this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null, false);
                else
                    return WaranResult.CheckAgain;

            }, 3000);
           
            Info(string.Format("上料站：方向判断{0} 取图像花费时间{1} ms", i+1, sp.ElapsedMilliseconds));
            ParamSetMgr.GetInstance().SetBoolParam("上料位头一回最后一个拍照处理完成", false);
            Info(string.Format("{0}拍照完成开始处理", i + 1));
            try
            {
                HOperatorSet.WriteImage(img, "bmp", 0, "D:\\1.bmp");
                if (visctr != null && visctr.isOpen())
                    HOperatorSet.SetDraw(visctr.GetHalconWindow(), "margin");
            }
            catch(Exception e)
            {
                Warn("上料站： " + e.Message);
            }
           
            Func<string, HObject, int, VisionControl, bool> func = new Func<string, HObject, int, VisionControl, bool>(pr);
            func.BeginInvoke("方向判定", img.Clone(), i, visctr, (iar) =>
            {
                AsyncResult asyncobj = (AsyncResult)iar;
                Func<string, HObject, int, VisionControl, bool> funobj = (Func<string, HObject, int, VisionControl, bool>)asyncobj.AsyncDelegate;
                bool bresult = funobj.EndInvoke(iar);
                Info(string.Format("上料站：第{0}号处理结果{1}", (int)iar.AsyncState + 1, bresult));
                object objresult = visionSetpBases[(int)iar.AsyncState].GetResult(); //VisionMgr.GetInstance().GetResult(strVisionPrName);
                if ((int)iar.AsyncState == TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].TotalCount - 1)
                    ParamSetMgr.GetInstance().SetBoolParam("上料位头一回最后一个拍照处理完成", true);
   
        
                int indexCell1 = tupleSnapposWithAxis[(int)iar.AsyncState].Item3;
                int indexCell2 = tupleSnapposWithAxis[(int)iar.AsyncState].Item4;
                SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell1].Cellstate = SocketCellState.CellStateNG;
                SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell2].Cellstate = SocketCellState.CellStateNG;

                if (objresult != null && bresult)
                {
                    VisionShapParam visionShapParam = (VisionShapParam)objresult;
                    if (visionShapParam != null && visionShapParam.GetResultNum() == 2)
                    {
                       
                        double deg1 = 0; double deg2 = 0;
                        if (visionShapParam.ResultCol[0] < visionShapParam.ResultCol[1])
                        {
                            bool bSucess = true;
                            if (visionShapParam.ResultCol[0] >= 1626 / 2)
                            {
                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)(int)iar.AsyncState + 1), false);
                                Info(string.Format("上料站：{0}号定位NG", (int)(int)iar.AsyncState + 1));
                                bSucess &= false;
                               
                            }
                            if (visionShapParam.ResultCol[1] <= 1626 / 2)
                            {
                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)iar.AsyncState + 1), false);
                                Info(string.Format("上料站：{0}号定位NG", (int)(int)iar.AsyncState + 1));
                                bSucess &= false;
                                
                            }
                            if (!bSucess)
                            {
                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)iar.AsyncState + 1), false);
                                Info(string.Format("{0}号定位NG", (int)iar.AsyncState + 1));
                                return;
                            }
                            deg1 =180 * visionShapParam.ResultAngle[0] / Math.PI;
                            deg2 = 180 * visionShapParam.ResultAngle[1] / Math.PI;
                            deg1 = JudgeAngle(deg1);
                            deg2 = JudgeAngle(deg2);
                        }
                        else
                        {
                            bool bSucess = true;
                            if (visionShapParam.ResultCol[0] <= 1626 / 2)
                            {
                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)(int)iar.AsyncState + 1), false);
                                Info(string.Format("上料站：{0}号定位NG", (int)(int)iar.AsyncState + 1));
                                bSucess &= false;
                              
                            }
                            if (visionShapParam.ResultCol[1] >=1626 / 2)
                            {
                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)iar.AsyncState + 1), false);
                                Info(string.Format("上料站：{0}号定位NG", (int)(int)iar.AsyncState + 1));
                                bSucess &= false;
                               
                            }
                            if (!bSucess)
                            {
                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)iar.AsyncState + 1), false);
                                Info(string.Format("{0}号定位NG", (int)iar.AsyncState + 1));
                                return;
                            }
                            deg1 = 180 * visionShapParam.ResultAngle[1] / Math.PI;
                            deg2 = 180 * visionShapParam.ResultAngle[0] / Math.PI;
                            deg1 = JudgeAngle(deg1);
                            deg2 = JudgeAngle(deg2);
                        }
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)iar.AsyncState + 1), true);
                        Info(string.Format("{0}号定位OK", (int)iar.AsyncState + 1));
                        tupleDegResultArr[(int)iar.AsyncState] = Tuple.Create<double, double>(visionShapParam.ResultAngle[0] * 180 / Math.PI, visionShapParam.ResultAngle[1] * 180 / Math.PI);

                        // Info( string.Format("{0}轴旋转{1}度 相对转动{"))
                        int axis1 = tupleSnapposWithAxis[(int)iar.AsyncState].Item1;
                        int axis2 = tupleSnapposWithAxis[(int)iar.AsyncState].Item2;
                        MotionMgr.GetInstace().SetAxisCmdPos(axis1, 0);
                        MotionMgr.GetInstace().SetAxisCmdPos(axis2, 0);

                        Info(String.Format("{0}-{1}轴转动{2}度当前脉冲{3} 多少脉冲{4}",
                           axis1, MotionMgr.GetInstace().GetAxisName(axis1), deg1, MotionMgr.GetInstace().GetAxisPos(axis1), (int)((deg1 / 360.00) * 1600)));
                        Info(String.Format("{0}-{1}轴转动{2}度当前脉冲{3} 多少脉冲{4}",
                          axis2, MotionMgr.GetInstace().GetAxisName(axis2), deg2, MotionMgr.GetInstace().GetAxisPos(axis2), (int)((deg2 / 360.00) * 1600)));
                        
                        MotionMgr.GetInstace().RelativeMove(axis1, (int)((deg1 / 360.00) * 1600), (int)SpeedType.Low);
                        MotionMgr.GetInstace().RelativeMove(axis2, (int)((deg2 / 360.00) * 1600), (int)SpeedType.Low);
                        SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell1].Cellstate = SocketCellState.CellStateOK;
                        SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell2].Cellstate = SocketCellState.CellStateOK;
                    }
                    else if (visionShapParam != null && visionShapParam.GetResultNum() ==1)
                    {
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)iar.AsyncState + 1), false);
                        tupleDegResultArr[(int)iar.AsyncState] = Tuple.Create<double, double>(visionShapParam.ResultAngle[0] * 180 / Math.PI, visionShapParam.ResultAngle[0] * 180 / Math.PI);
                        //HOperatorSet.GetImageSize(img, out HTuple imgwidth, out HTuple imgheight);
                        Info(string.Format("{0}号定位NG{1}", (int)iar.AsyncState + 1, visionShapParam.ResultCol[0] <= 1626/2?"左":"右"));
                        double degSigle=0;
                        degSigle = 180 * visionShapParam.ResultAngle[0] / Math.PI;
                        degSigle = JudgeAngle(degSigle);
                        int axisSigle = 0;
                       
                        if (visionShapParam.ResultCol[0] <= 1626 / 2)
                        {
                            axisSigle= tupleSnapposWithAxis[(int)iar.AsyncState].Item1;
                            SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell1].Cellstate = SocketCellState.CellStateOK;
                        }
                        else
                        {
                            axisSigle = tupleSnapposWithAxis[(int)iar.AsyncState].Item2;
                            SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell2].Cellstate = SocketCellState.CellStateOK;
                        }
                        MotionMgr.GetInstace().SetAxisCmdPos(axisSigle, 0);
 
                        Info(String.Format("{0}-{1}轴转动{2}度当前脉冲{3} 多少脉冲{4}", 
                            axisSigle, MotionMgr.GetInstace().GetAxisName(axisSigle), degSigle, MotionMgr.GetInstace().GetAxisPos(axisSigle), (int)((degSigle / 360.00) * 1600)));
                        MotionMgr.GetInstace().RelativeMove(axisSigle, (int)((degSigle / 360.00) * 1600), (int)SpeedType.Low);
                    }
                    else
                    {
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)iar.AsyncState + 1), false);
                        Info(string.Format("{0}号定位NG", (int)iar.AsyncState + 1));
                        tupleDegResultArr[(int)iar.AsyncState] = Tuple.Create<double, double>(0, 0);
                    }
                }
                else
                {
                    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)iar.AsyncState + 1), false);
                    Info(string.Format("{0}号定位NG", (int)iar.AsyncState + 1));
                    tupleDegResultArr[(int)iar.AsyncState] = Tuple.Create<double, double>(0, 0);
                   
                }
            }, i);
            img.Dispose();
        }
        bool bpr = true;
        int nNumMaxFail = -1;
        protected override void StationWork(int step)
        {
            WaranResult waranResult;
            switch (step)
            {
                case (int)StationStep.step_init:
                    Init();
                    DelCurrentStep();
                    PushMultStep((int)StationStep.step_CheckInPos);
                    break;
                case (int)StationStep.step_CheckInPos:
                    if(oldStep!=step)
                    {
                        Info(string.Format("进料状态{0} 已经取料个数{1}", LineMgr.GetInstance().LoadState, TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index * 8));
                        oldStep = step;
                    }
                   
                    if (LineMgr.GetInstance().LoadState == LineState.Have &&
                        (TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].TotalCount - TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index > 0))
                    {
                    retry_cylider_down:
                        IOMgr.GetInstace().WriteIoBit("上料位定位气缸电磁阀", true);
                        waranResult = CheckIobyName("上料位定位气缸到位", true, "上料站： 上料位定位气缸下降失败，请检查感应器 或者气缸卡住，检查线路");
                        if (waranResult == WaranResult.Retry)
                            goto retry_cylider_down;
                        Info(string.Format("上料位： 开始从托盘取料 上料位状态变成{0}", LineMgr.GetInstance().LoadState));
                        PushMultStep((int)StationStep.step_Pick, (int)StationStep.step_CheckSocketLineInPos, (int)StationStep.step_place, (int)StationStep.step_Snap, (int)StationStep.step_WaitallVisionProcessFinish, (int)StationStep.step_CheckInPos);
                        DelCurrentStep();
                    }
                    //else if((TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].TotalCount - TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index<=0))
                    //{

                    //   // DelCurrentStep();
                    //}

                    break;
                case (int)StationStep.step_Pick:
                    Info(string.Format("上料站: 从Tray {0}中取料", TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index));
                    PickFromTray(TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index);
                    DelCurrentStep();
                    break;
                case (int)StationStep.step_CheckSocketLineInPos:
                    if (CheckLineReadly())
                    {
                        IOMgr.GetInstace().WriteIoBit("上料夹紧SOCKET气缸电磁阀", true);
                        DelCurrentStep();
                    }
                    else
                    {
                        Info("上料站： 取料后 Socket流水线为准备好，去安全位置等待");
                        double safeX = GetStationPointDic()["安全位置"].pointX;
                        double safeY = GetStationPointDic()["安全位置"].pointY;
                        MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { safeX, safeY }, new double[] { (double)SpeedType.High, (double)SpeedType.High },
                            10, false, this, 60000);
                    }
                    break;
                case (int)StationStep.step_Snap:
                    Info("上料站：开始拍照 进行方向判定");
                    for (int j = 0; j <= TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].TotalCount; j++)
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", j + 1), false);
                    ParamSetMgr.GetInstance().SetBoolParam("上料站Pr结束",false);
                    for (int i = 0; i < TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].TotalCount; i++)
                    {
                        SnapAndRotate(i,this.VisionControl);

                    }
                    DelCurrentStep();
                    break;
                case (int)StationStep.step_place:
                    Info("上料站：放料到Sokcet中");
                    PlaceToSocket();
                    DelCurrentStep();
                    break;
                case (int)StationStep.step_WaitallVisionProcessFinish:
                    if (!ParamSetMgr.GetInstance().GetBoolParam("上料位头一回最后一个拍照处理完成"))
                        break;
                        
                    Info("方向定位结果检查");
                     bpr = true;
                    for (int j = 0; j < TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].TotalCount; j++)
                    {
                        bpr &= ParamSetMgr.GetInstance().GetBoolParam(string.Format("{0}号定位OK", j + 1));
                        if (!ParamSetMgr.GetInstance().GetBoolParam(string.Format("{0}号定位OK", j + 1)))
                            nNumMaxFail = j;
                    }
                       
                    if (!bpr && ParamSetMgr.GetInstance().GetIntParam("是否抛料处理") == 1)
                    {
                        bpr = ParamSetMgr.GetInstance().GetBoolParam("上料站Pr结束");
                    }
                    //### AppMode.AirRun
                    if ((bpr && GetAllStepMotorState()))/*|| sys.g_AppMode == AppMode.AirRun*/
                    {
                        LightControl.GetInstance().CloseLight("上料方向判定");
                        int delayofStepMotorMoveFinish=   ParamSetMgr.GetInstance().GetIntParam("步进运动完成延时");
                        if (delayofStepMotorMoveFinish < 100)
                            delayofStepMotorMoveFinish = 100;
                        else if (delayofStepMotorMoveFinish > 1000)
                            delayofStepMotorMoveFinish = 1000;
                        Thread.Sleep(delayofStepMotorMoveFinish);
                        Info("上料站：拍照 旋转定位完成");
                        IOMgr.GetInstace().WriteIoBit("上料真空吸1电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸2电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸3电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸4电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸5电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸6电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸7电磁阀", false);
                        IOMgr.GetInstace().WriteIoBit("上料真空吸8电磁阀", false);
                    retry_place_check_MotorDown:
                        IOMgr.GetInstace().WriteIoBit("上料定位电机升降气缸电磁阀", false);
                        waranResult = CheckIobyName("上料位电机升降气缸原位", true, "上料站： 上料位电机升降气缸原位 到位失败，请检查", false);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_check_MotorDown;
                        retry_place_closesocket:
                        IOMgr.GetInstace().WriteIoBit("上料开SOCKET电磁阀", false);
                        waranResult = CheckIobyName("上料位开SOCKET气缸原位", true, "上料站：上料位开SOCKET气缸原位失败 socket治具关闭失败，请拿开", false);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_closesocket;
                        retry_place_check_Clamp:
                        IOMgr.GetInstace().WriteIoBit("上料夹紧SOCKET气缸电磁阀", false);
                        waranResult = CheckIobyName("上料位夹紧SOCKET气缸原位", true, "上料站： 上料位夹紧SOCKET气缸原位 到位失败，请检查", false);
                        if (waranResult == WaranResult.Retry)
                            goto retry_place_check_Clamp;

                       
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.LoadNozzle].nozzleState = NozzleState.None;
                        SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketState = SocketState.Have;
                        DelCurrentStep();
                    }
                    else if(bpr  && !GetAllStepMotorState())
                    {
                        break;
                    }
                    else
                    {
                        // ### AppMode.AirRun
                        //  if (sys.g_AppMode == AppMode.AirRun)
                        //      DelCurrentStep();
                        HObject img = null;
                        for (int j = 0; j < visionSetpBases.Length; j++)
                        {
                        
                            if (ParamSetMgr.GetInstance().GetBoolParam(string.Format("{0}号定位OK", j + 1)))
                                  continue;
                            DoWhile doWhile = new DoWhile((time, dowhile, bmanual2, obj) =>
                            {
                                object objresult = visionSetpBases[j].GetResult(); //VisionMgr.GetInstance().GetResult(strVisionPrName);
                                int indexCell1 = tupleSnapposWithAxis[j].Item3;
                                int indexCell2 = tupleSnapposWithAxis[j].Item4;
                                SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell1].Cellstate = SocketCellState.CellStateNG;
                                SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell2].Cellstate = SocketCellState.CellStateNG;

                                if (objresult != null)
                                {
                                    VisionShapParam visionShapParam = (VisionShapParam)objresult;
                                    //### AppMode.AirRun
                                    //if (sys.g_AppMode == AppMode.AirRun)
                                    //{
                                    //    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), true);
                                    //    return WaranResult.Run;
                                    //}
                                    int num = visionShapParam.GetResultNum();
                                    
                                  
                                    if ( num ==2  )
                                    {
                                        
                                        double deg1 = 0; double deg2 = 0;
                                        if (visionShapParam.ResultCol[0] < visionShapParam.ResultCol[1]   )
                                        {
                                            bool bSucess = true;
                                            if (visionShapParam.ResultCol[0] >= 1626 / 2)
                                            {
                                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), false);
                                                Info(string.Format("上料站：{0}号定位NG", (int)j + 1));
                                                bSucess &= false;
                                            }
                                            if (visionShapParam.ResultCol[1] <= 1626 / 2)
                                            {
                                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), false);
                                                Info(string.Format("上料站：{0}号定位NG", (int)j + 1));
                                                bSucess &= false;
                                            }
                                            if(!bSucess)
                                            {
                                                int jude = (int)obj[0];
                                                if (jude == 1)
                                                    return AlarmMgr.GetIntance().WarnWithDlg(string.Format("上料站：{0}号定位NG", j + 1), this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null);
                                                else
                                                    return WaranResult.Failture;
                                            }
                                            deg1 = 180 * visionShapParam.ResultAngle[0] / Math.PI;
                                            deg2 = 180 * visionShapParam.ResultAngle[1] / Math.PI;
                                            deg1 = JudgeAngle(deg1);
                                            deg2 = JudgeAngle(deg2);
                                        }
                                        else
                                        {
                                            bool bSucess = true;
                                            if (visionShapParam.ResultCol[0] <= 1626 / 2)
                                            {
                                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), false);
                                                Info(string.Format("上料站：{0}号定位NG", (int)j + 1));
                                                bSucess &= false;
                                            }
                                            if (visionShapParam.ResultCol[1] >=1626 / 2)
                                            {
                                                ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), false);
                                                Info(string.Format("上料站：{0}号定位NG", (int)j + 1));
                                                bSucess &= false;
                                            }
                                            if (!bSucess)
                                            {
                                                int jude = (int)obj[0];
                                                if (jude == 1)
                                                    return AlarmMgr.GetIntance().WarnWithDlg(string.Format("上料站：{0}号定位NG", j + 1), this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null);
                                                else
                                                    return WaranResult.Failture;
                                            }
                                            deg1 = 180 * visionShapParam.ResultAngle[1] / Math.PI;
                                            deg2 = 180 * visionShapParam.ResultAngle[0] / Math.PI;
                                            deg1 = JudgeAngle(deg1);
                                            deg2 = JudgeAngle(deg2);
                                        }

                                        // Info( string.Format("{0}轴旋转{1}度 相对转动{"))
                                        int axis1 = tupleSnapposWithAxis[(int)j].Item1;
                                        int axis2 = tupleSnapposWithAxis[(int)j].Item2;
                                        MotionMgr.GetInstace().SetAxisCmdPos(axis1, 0);
                                        MotionMgr.GetInstace().SetAxisCmdPos(axis2, 0);

                                        Info(String.Format("{0}-{1}轴转动{2}度当前脉冲{3} 多少脉冲{4}", axis1, MotionMgr.GetInstace().GetAxisName(axis1),deg1, MotionMgr.GetInstace().GetAxisPos(axis1), (int)((deg1 / 360.00) * 1600)));
                                        Info(String.Format("{0}-{1}轴转动{2}度当前脉冲{3} 多少脉冲{4}", axis2, MotionMgr.GetInstace().GetAxisName(axis2),deg2, MotionMgr.GetInstace().GetAxisPos(axis2), (int)((deg2 / 360.00) * 1600)));
                                        MotionMgr.GetInstace().RelativeMove(axis1, (int)((deg1 / 360.00) * 1600), (int)SpeedType.Low);
                                        MotionMgr.GetInstace().RelativeMove(axis2, (int)((deg2 / 360.00) * 1600), (int)SpeedType.Low);
                                        SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell1].Cellstate = SocketCellState.CellStateOK;
                                        SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell2].Cellstate = SocketCellState.CellStateOK;
                                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), true);
                                        Info(string.Format("上料站：{0}号定位OK", (int)j + 1));
                                        return WaranResult.Run;
                                    }
                                    else if (num == 1)
                                    {
                                       
                                        double degSigle = 0;
                                        degSigle = 180 * visionShapParam.ResultAngle[0] / Math.PI;
                                        degSigle = JudgeAngle(degSigle);
                                        int AxisSigle = 0;
                                        if (visionShapParam.ResultCol[0] < 1626 / 2)
                                        {
                                            AxisSigle = tupleSnapposWithAxis[(int)j].Item1;
                                            SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell1].Cellstate = SocketCellState.CellStateOK;
                                        }
                                        else
                                        {
                                            AxisSigle = tupleSnapposWithAxis[(int)j].Item2;
                                            SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketcells[indexCell2].Cellstate = SocketCellState.CellStateOK;
                                        }
                                        MotionMgr.GetInstace().SetAxisCmdPos(AxisSigle, 0);
                                        Info(String.Format("{0}-{1}轴转动{2}度当前脉冲{3} 多少脉冲{4}",
                                            AxisSigle, MotionMgr.GetInstace().GetAxisName(AxisSigle), degSigle,
                                            MotionMgr.GetInstace().GetAxisPos(AxisSigle), (int)((degSigle / 360.00) * 1600)));

                                        MotionMgr.GetInstace().RelativeMove(AxisSigle, (int)((degSigle / 360.00) * 1600), (int)SpeedType.Low);
                                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), false);
                                        Info(string.Format("上料站：{0}号定位NG", (int)j + 1));
                                        int jude = (int)obj[0];
                                        if (jude == 1)
                                            return AlarmMgr.GetIntance().WarnWithDlg(string.Format("上料站：{0}号定位NG", j + 1), this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null);
                                        else
                                            return WaranResult.Failture;

                                    }
                                    else
                                    {
                                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), false);
                                        Info(string.Format("上料站：{0}号定位NG", (int)j + 1));
                                        int jude = (int)obj[0];
                                        if (jude == 1)
                                            return AlarmMgr.GetIntance().WarnWithDlg(string.Format("上料站：{0}号定位NG", j + 1), this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null);
                                        else
                                            return WaranResult.Failture;
                                    }
                                }
                                if (time > 500)
                                {
                                    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号定位OK", (int)j + 1), false);
                                    Info(string.Format("上料站：{0}号定位NG 超时", (int)j + 1));
                                    int jude = (int)obj[0];
                                    if (jude == 1)
                                        return AlarmMgr.GetIntance().WarnWithDlg(string.Format("上料站：{0}号定位NG 超时", j + 1), this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null);
                                    else
                                        return WaranResult.TimeOut;
                                }
                                else
                                    return WaranResult.CheckAgain;

                            }, 8000);
                            
                            for( int i=0;i<5;i++)
                            {
                                try
                                {
                                    double snapx = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].trayCells[j].Snapcoordinate.X;
                                    double snapy = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].trayCells[j].Snapcoordinate.Y;
                                    MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { snapx, snapy }, new double[] { (double)SpeedType.Mid, (double)SpeedType.Mid }, 10);
                                    Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));
                                    int val = LightControl.GetInstance().itemlightdic["上料方向判定"].lightval;
                                    int nch = LightControl.GetInstance().itemlightdic["上料方向判定"].nCh;
                                    LightControl.GetInstance().Light(nch, (val + 5 * i) >= 255 ? 255 : (val + 5 * i));
                                    double? exporetime = VisionMgr.GetInstance().GetExpourseTime("方向判定");
                                    if(exporetime== null )
                                        exporetime = VisionMgr.GetInstance().GetExpourseTime("方向判定");
                                    double exp = (double)exporetime + 500 * i;
                                    string camname = VisionMgr.GetInstance().GetCamName("方向判定");
                                    CameraMgr.GetInstance().GetCamera(camname).ClearAllPr();
                                    CameraMgr.GetInstance().GetCamera(camname).GarbBySoftTrigger();
                                    CameraMgr.GetInstance().SetCamExposure(camname/*"LoadCCD"*/, exp);
                                    if(exp != CameraMgr.GetInstance().GetCamExposure(camname/*"LoadCCD"*/))
                                        CameraMgr.GetInstance().SetCamExposure(camname/*"LoadCCD"*/, exp);
                                    int timedelay = ParamSetMgr.GetInstance().GetIntParam("开光源延时");
                                    Thread.Sleep(timedelay);
                                    img = CameraMgr.GetInstance().GetImg(camname);
                                    if (img == null)
                                    {
                                        CameraMgr.GetInstance().GetCamera(camname).GarbBySoftTrigger();
                                        img = CameraMgr.GetInstance().GetImg(camname);
                                    }
                               
                                    visionSetpBases[j].ClearResult();
                                    bool brtn = visionSetpBases[j].Process_image(img, this.VisionControl);
                                    if(!brtn )
                                        continue;
                                    waranResult = doWhile.doSomething(this, doWhile, false, 0);
                                    if (waranResult == WaranResult.Run)
                                        break;
                                    else
                                        continue;
                                
                                }
                                catch (Exception E)
                                {
                                    Info("上料站： 异常" + E.Message);
                                }
                                finally
                                {
                                    img?.Dispose();
                                }
                            }
                            if(ParamSetMgr.GetInstance().GetIntParam("是否抛料处理")==1)
                            {
                                //抛料处理  自动进行5次检查后 仍然异常 抛料处理
                                if (nNumMaxFail==j)
                                ParamSetMgr.GetInstance().SetBoolParam("上料站Pr结束", true);
                            }
                            else
                            {
                                //不抛料处理  自动进行5次检查后 仍然异常 人工干预处理
                                if (ParamSetMgr.GetInstance().GetBoolParam(string.Format("{0}号定位OK", j + 1)))
                                    continue;
                                retry_snap:
                                ParamSetMgr.GetInstance().SetBoolParam("上料站Pr结束", false);
                                waranResult = AlarmMgr.GetIntance().WarnWithDlg(string.Format("上料站：{0}号定位NG", j + 1), this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null);
                                if (waranResult == WaranResult.Retry)
                                {
                                    Info(string.Format("上料站：{0}重新定位拍照", j));
                                    LightControl.GetInstance().Light("上料方向判定");
                                    double snapx = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].trayCells[j].Snapcoordinate.X;
                                    double snapy = TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.LoadSnap].trayCells[j].Snapcoordinate.Y;
                                    MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { snapx, snapy }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 10);
                                    Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));

                                    string camname = VisionMgr.GetInstance().GetCamName("方向判定");
                                    CameraMgr.GetInstance().GetCamera(camname).BindWindow(this.VisionControl);
                                    CameraMgr.GetInstance().SetTriggerSoftMode(camname/*"LoadCCD"*/);
                                    double expourseTime = (double)VisionMgr.GetInstance().GetExpourseTime("方向判定");
                                    double gain = (double)VisionMgr.GetInstance().GetGain("方向判定");
                                    CameraMgr.GetInstance().SetCamGain(camname, gain);
                                    CameraMgr.GetInstance().SetCamExposure(camname, expourseTime);
                                    CameraMgr.GetInstance().GetCamera(camname).GarbBySoftTrigger();
                                    int timedelay = ParamSetMgr.GetInstance().GetIntParam("开光源延时");
                                    Thread.Sleep(timedelay);
                                    img = CameraMgr.GetInstance().GetImg(camname);
                                    if (img == null)
                                    {
                                        CameraMgr.GetInstance().SetCamGain(camname, gain);
                                        CameraMgr.GetInstance().SetCamExposure(camname, expourseTime);
                                        CameraMgr.GetInstance().GetCamera(camname).GarbBySoftTrigger();
                                        img = CameraMgr.GetInstance().GetImg(camname);
                                    }
                              
                                    visionSetpBases[j].ClearResult();
                                    visionSetpBases[j].Process_image(img, this.VisionControl);
                                    waranResult = doWhile.doSomething(this, doWhile, false, 1);
                                    if (waranResult == WaranResult.Retry)
                                        goto retry_snap;
                                }
                            }
                           
                        }
                    }
                    break;
            }

            if (TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].TotalCount - TrayMgr.GetInstance().trayDataLoadArr[(int)TrayType.Load].index <= 0
                && LineMgr.GetInstance().LoadState == LineState.Have)
            {
                double safeY = GetStationPointDic()["安全位置"].pointY;
                if (MotionMgr.GetInstace().GetAxisPos(AxisY) < safeY - 10)
                    return;
                LineMgr.GetInstance().LoadState = LineState.Finished;
                Info(string.Format("上料位： 料已经取完 上料位状态变成{0}", LineMgr.GetInstance().LoadState));
                 retry_cylider_up:
                IOMgr.GetInstace().WriteIoBit("上料位定位气缸电磁阀", false);
                waranResult = CheckIobyName("上料位定位气缸原位", true, "上料站： 上料位定位气缸上升失败，请检查感应器 或者气缸卡住，检查线路");
                if (waranResult == WaranResult.Retry)
                    goto retry_cylider_up;

            }
        }




    }
}
