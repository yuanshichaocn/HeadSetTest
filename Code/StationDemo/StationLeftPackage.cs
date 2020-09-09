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
using System.Runtime.Remoting.Messaging;
using VisionProcess;
using UserCtrl;
using LightControler;
using CameraLib;
using System.Diagnostics;

namespace StationDemo
{
    public class StationLeftPackage : Stationbase
    {
        const int dFineDistance = 10;//10个plus
        const int nSeparateCount = 10;
        const double nResultionZ = 10000 / 10;
        public int indexPick = 0;
        VisionSetpBase[] visionSetpBasesSnapBlackCaps = new VisionSetpBase[] { null, null, null, null };
        VisionSetpBase[] visionSetpBasesSnapBuzzers = new VisionSetpBase[] { null, null, null, null };
        VisionSetpBase[] visoModleLocation = new VisionSetpBase[] { null, null, null, null };
        int[] NgBuzzerArr = new int[4];
        int[] NgBlackArr = new int[4];
        public void ResetBuzzerFlag()
        {
            for (int i = 0; i < NgBuzzerArr.Length; i++)
            {
                NgBuzzerArr[i] = 0;
            }
        }
        public void ResetBlackCapFlag()
        {
            for (int i = 0; i < NgBlackArr.Length; i++)
            {
                NgBlackArr[i] = 0;
            }
        }

        public void LoadPrUpadate(string itemname)
        {
            try
            {
                if (itemname == "蜂鸣器定位" && VisionMgr.GetInstance().dicVision.ContainsKey("蜂鸣器定位"))
                {
                    visionSetpBasesSnapBuzzers[0] = VisionMgr.GetInstance().dicVision["蜂鸣器定位"].Clone();
                    visionSetpBasesSnapBuzzers[1] = VisionMgr.GetInstance().dicVision["蜂鸣器定位"].Clone();
                    visionSetpBasesSnapBuzzers[2] = VisionMgr.GetInstance().dicVision["蜂鸣器定位"].Clone();
                    visionSetpBasesSnapBuzzers[3] = VisionMgr.GetInstance().dicVision["蜂鸣器定位"].Clone();
                }
                if (itemname == "黑帽定位" && VisionMgr.GetInstance().dicVision.ContainsKey("黑帽定位"))
                {

                    visoModleLocation[0] = VisionMgr.GetInstance().dicVision["左贴装模板"].Clone();
                    visoModleLocation[1] = VisionMgr.GetInstance().dicVision["左贴装模板"].Clone();
                    visoModleLocation[2] = VisionMgr.GetInstance().dicVision["左贴装模板"].Clone();
                    visoModleLocation[3] = VisionMgr.GetInstance().dicVision["左贴装模板"].Clone();
                    visionSetpBasesSnapBlackCaps[0] = VisionMgr.GetInstance().dicVision["黑帽定位"].Clone();
                    visionSetpBasesSnapBlackCaps[1] = VisionMgr.GetInstance().dicVision["黑帽定位"].Clone();
                    visionSetpBasesSnapBlackCaps[2] = VisionMgr.GetInstance().dicVision["黑帽定位"].Clone();
                    visionSetpBasesSnapBlackCaps[3] = VisionMgr.GetInstance().dicVision["黑帽定位"].Clone();
                }

            }
            catch
            {
                return;
            }

        }
        public StationLeftPackage(Stationbase stationbase) : base(stationbase)
        {

            m_listIoInput.Add("左蜂鸣器Z轴气缸1原位");
            m_listIoInput.Add("左蜂鸣器Z轴气缸1到位");


            m_listIoInput.Add("左蜂鸣器Z轴气缸2原位");
            m_listIoInput.Add("左蜂鸣器Z轴气缸2到位");

            m_listIoInput.Add("左蜂鸣器Z轴气缸3原位");
            m_listIoInput.Add("左蜂鸣器Z轴气缸3到位");

            m_listIoInput.Add("左蜂鸣器Z轴气缸4原位");
            m_listIoInput.Add("左蜂鸣器Z轴气缸4到位");

            m_listIoInput.Add("左蜂鸣器真空检测1");
            m_listIoInput.Add("左蜂鸣器真空检测2");

            m_listIoInput.Add("左蜂鸣器真空检测3");
            m_listIoInput.Add("左蜂鸣器真空检测4");


            m_listIoOutput.Add("左蜂鸣器Z轴气缸1电磁阀");
            m_listIoOutput.Add("左蜂鸣器Z轴气缸2电磁阀");
            m_listIoOutput.Add("左蜂鸣器Z轴气缸3电磁阀");
            m_listIoOutput.Add("左蜂鸣器Z轴气缸4电磁阀");

            m_listIoOutput.Add("左蜂鸣器真空吸1电磁阀");
            m_listIoOutput.Add("左蜂鸣器破真空1电磁阀");
            m_listIoOutput.Add("左蜂鸣器真空吸2电磁阀");
            m_listIoOutput.Add("左蜂鸣器破真空2电磁阀");
            m_listIoOutput.Add("左蜂鸣器真空吸3电磁阀");
            m_listIoOutput.Add("左蜂鸣器破真空3电磁阀");
            m_listIoOutput.Add("左蜂鸣器真空吸4电磁阀");
            m_listIoOutput.Add("左蜂鸣器破真空4电磁阀");

            LoadPrUpadate("蜂鸣器定位");
            LoadPrUpadate("黑帽定位");
            VisionMgr.GetInstance().PrItemChangedEvent += LoadPrUpadate;


        }
        public override void ExctionDeal(string strmsg)
        {
            LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
            LightControl.GetInstance().CloseLight("左上黑帽定位");
        }

        public enum StationStep
        {
            step_init,
            step_jude_plane,
            step_jude_line,
            step_Pick,
            step_Snap_Buzzer,
            step_Snap_cap,
            step_place,
        }

        void DoSomethingWhenalarm()
        {

        }

        protected override bool InitStation()
        {
            indexPick = 0;
            AlarmMgr.GetIntance().DoWhenAlarmEvent += DoSomethingWhenalarm;
            PushMultStep((int)StationStep.step_init);
            indexPick = 0;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 = NozzleState.None;
            bool bhave = MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenLeftPackageXYAxisMoveing);
            if (bhave)
                MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenLeftPackageXYAxisMoveing;
            return true;
        }
        public void PickFromPlane(ref int index, int indexpick = 0, bool bmanual = false)
        {
            WaranResult waranResult;
            double x = 0; double y = 0;
            double z = 0;
            Info(string.Format("开始{0}移动到{1}取料位置，", bmanual ? "手动" : "自动", index == 1 ? "左" : "右"));
            retry_cylider_up:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", false);
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸1原位", true, "左贴装站：左蜂鸣器Z轴气缸1原位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_cylider_up;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸2原位", true, "左贴装站：左蜂鸣器Z轴气缸2原位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_cylider_up;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站：左蜂鸣器Z轴气缸3原位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_cylider_up;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸4原位", true, "左贴装站：左蜂鸣器Z轴气缸4原位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_cylider_up;
  


            switch (index)
            {
                case 0:
                    Info("左贴装站：开始去左取料位置");
                    x = GetStationPointDic()["左取料位置"].pointX;
                    y = GetStationPointDic()["左取料位置"].pointY;
                    z = GetStationPointDic()["左取料位置"].pointZ;
                    //  MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this);

                    break;
                case 1:
                    Info("左贴装站：开始去右取料位置");
                    x = GetStationPointDic()["右取料位置"].pointX;
                    y = GetStationPointDic()["右取料位置"].pointY;
                    z = GetStationPointDic()["右取料位置"].pointZ;
                    // MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this);
                    break;

            }
            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);
            double SafeZ = GetStationPointDic()["安全左上"].pointZ;
            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 5, bmanual, this, 30000);

            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual);
            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 5, bmanual, this, 30000);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器破真空1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器破真空2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器破真空3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器破真空4电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸1电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸2电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸3电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸4电磁阀", true);
            retry_cylider_down:
            switch (indexpick)
            {
                case 0:
                case 1:
                    Info("左贴装站：一次行取料");
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", true);
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", true);
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", true);
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", true);
                    waranResult = CheckIobyName("左蜂鸣器Z轴气缸1到位", true, "左贴装站：左蜂鸣器Z轴气缸1到位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    waranResult = CheckIobyName("左蜂鸣器Z轴气缸2到位", true, "左贴装站：左蜂鸣器Z轴气缸2到位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    waranResult = CheckIobyName("左蜂鸣器Z轴气缸3到位", true, "左贴装站：左蜂鸣器Z轴气缸3到位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    waranResult = CheckIobyName("左蜂鸣器Z轴气缸4到位", true, "左贴装站：左蜂鸣器Z轴气缸4到位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸1电磁阀", true);
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸2电磁阀", true);
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸3电磁阀", true);
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸4电磁阀", true);
                    break;
                case 11:
                    Info("左贴装站：1号取料");
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", true);
                    waranResult = CheckIobyName("左蜂鸣器Z轴气缸1到位", true, "左贴装站：左蜂鸣器Z轴气缸1到位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸1电磁阀", true);
                    break;
                case 12:
                    Info("左贴装站：2号取料");
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", true);
                    waranResult = CheckIobyName("左蜂鸣器Z轴气缸2到位", true, "左贴装站：左蜂鸣器Z轴气缸2到位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸2电磁阀", true);
                    break;
                case 13:
                    Info("左贴装站：3号取料");
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", true);
                    waranResult = CheckIobyName("左蜂鸣器Z轴气缸3到位", true, "左贴装站：左蜂鸣器Z轴气缸3到位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸3电磁阀", true);
                    break;
                case 14:
                    Info("左贴装站：4号取料");
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", true);
                    waranResult = CheckIobyName("左蜂鸣器Z轴气缸4到位", true, "左贴装站：左蜂鸣器Z轴气缸4到位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸4电磁阀", true);
                    break;
            }

            Info("左贴装站：吸嘴开始从左剥料平台吸料");

            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("吸真空延时"));
            double lendistance = ParamSetMgr.GetInstance().GetDoubleParam("取料上拉距离");
            if (lendistance < 1)
                lendistance = 1;
            else if (lendistance > 5)
                lendistance = 5;

            z = MotionMgr.GetInstace().GetAxisPos(AxisZ) + lendistance * nResultionZ;
            MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.Mid, 10, bmanual, this, 60000);

            Info("左贴装站：吸嘴开始升起");
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", false);

            Info("左贴装站：吸嘴开始真空检测");
            switch (indexpick)
            {
                case 0:
                case 1:
                    waranResult = CheckIobyName("左蜂鸣器真空检测1", true, "左贴装站：左蜂鸣器真空检测1信号没有到达，请检查蜂鸣器是否脱落 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    waranResult = CheckIobyName("左蜂鸣器真空检测2", true, "左贴装站：左蜂鸣器真空检测2信号没有到达，请检查蜂鸣器是否脱落 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;

                    waranResult = CheckIobyName("左蜂鸣器真空检测3", true, "左贴装站：左蜂鸣器真空检测3信号没有到达，请检查蜂鸣器是否脱落 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;

                    waranResult = CheckIobyName("左蜂鸣器真空检测4", true, "左贴装站：左蜂鸣器真空检测4信号没有到达，请检查蜂鸣器是否脱落 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState = NozzleState.Have;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState = NozzleState.Have;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState = NozzleState.Have;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState = NozzleState.Have;
                    if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None)
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 = NozzleState.Have;
                    if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.None)
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 = NozzleState.Have;
                    if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.None)
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 = NozzleState.Have;
                    if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.None)
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 = NozzleState.Have;
                    break;
                case 11:
                    waranResult = CheckIobyName("左蜂鸣器真空检测1", true, "左贴装站：左蜂鸣器真空检测1信号没有到达，请检查蜂鸣器是否脱落 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState = NozzleState.Have;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 = NozzleState.Have;
                    break;
                case 12:
                    waranResult = CheckIobyName("左蜂鸣器真空检测2", true, "左贴装站：左蜂鸣器真空检测2信号没有到达，请检查蜂鸣器是否脱落 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState = NozzleState.Have;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 = NozzleState.Have;
                    break;
                case 13:
                    waranResult = CheckIobyName("左蜂鸣器真空检测3", true, "左贴装站：左蜂鸣器真空检测3信号没有到达，请检查蜂鸣器是否脱落 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState = NozzleState.Have;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 = NozzleState.Have;
                    break;
                case 14:
                    waranResult = CheckIobyName("左蜂鸣器真空检测4", true, "左贴装站：左蜂鸣器真空检测4信号没有到达，请检查蜂鸣器是否脱落 ", bmanual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_cylider_down;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState = NozzleState.Have;
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 = NozzleState.Have;
                    break;
            }
            Info("左贴装站：吸嘴左剥料平台吸料完成");
            double snapposx = GetStationPointDic()["蜂鸣器拍照左"].pointX;
            double snapposy = GetStationPointDic()["蜂鸣器拍照左"].pointY;
            double snapposz = GetStationPointDic()["蜂鸣器拍照左"].pointZ;
            Info("左贴装站：取料完成，去拍照位置");
            x = snapposx;
            y = snapposy;
            z = snapposz;
            currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);
            SafeZ = GetStationPointDic()["安全左上"].pointZ;

            bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);

            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ && z < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 30, bmanual, this, 60000);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ && z >= SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 30, bmanual, this, 60000);
            retry_cylider_up2:
            Info("左贴装站：吸嘴开始升起");
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", false);
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸1原位", true, "左贴装站：左蜂鸣器Z轴气缸1原位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_cylider_up2;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸2原位", true, "左贴装站：左蜂鸣器Z轴气缸2原位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_cylider_up2;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站：左蜂鸣器Z轴气缸3原位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_cylider_up2;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸4原位", true, "左贴装站：左蜂鸣器Z轴气缸4原位信号没有到达，请检查气缸是否卡住，或者线路 ", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_cylider_up2;

            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ },
               new double[] { snapposx, snapposy, snapposz },
               new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this, 60000);
            //MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 15, bmanual, this, 60000);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 15, bmanual, this, 60000);

            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 10, bmanual, this, 60000);

            //MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ },
            //    new double[] { snapposx, snapposy, snapposz },
            //    new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this, 60000);

            if (index == 1)
            {
                Info("置位系统变量《左剥料取料完成》，复位系统变量《左剥料完成》");
                ParamSetMgr.GetInstance().SetBoolParam("左剥料取料完成", true);
                ParamSetMgr.GetInstance().SetBoolParam("左剥料完成", false);
            }


            index = (index + 1) % 2;
        }
        bool pr(string strVisionPrName, HObject img, int index, VisionControl visionControl)
        {
            try
            {
                if (strVisionPrName == "蜂鸣器定位")
                {
                    visionSetpBasesSnapBuzzers[index - 1].ClearResult();
                    HObject ho_Image, ho_Region, ho_ConnectedRegions;
                    HObject ho_SelectedRegions, ho_RegionFillUp;
                    // Local control variables 
                    HTuple hv_Row = null, hv_Column = null, hv_Radius = null;
                    // Initialize local and output iconic variables 
                    HOperatorSet.GenEmptyObj(out ho_Image);
                    HOperatorSet.GenEmptyObj(out ho_Region);
                    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                    HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(img, out ho_Region, 200, 255);

                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);

                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                        70);

                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
                    HOperatorSet.SmallestCircle(ho_RegionFillUp, out hv_Row, out hv_Column, out hv_Radius);
                    HOperatorSet.CountObj(ho_RegionFillUp, out HTuple num);
                    if (num.I == 0)
                        return false;
                    VisionFitCircircle vcbase = (VisionFitCircircle)visionSetpBasesSnapBuzzers[index - 1];
                    vcbase.visionFitCircleParam.point2DRoixy.x = hv_Column[0].D;
                    vcbase.visionFitCircleParam.point2DRoixy.y = hv_Row[0].D;
                    vcbase.visionFitCircleParam.RadiusRoi = hv_Radius[0].D;
                    visionSetpBasesSnapBuzzers[index - 1].Process_image(img, visionControl);
                    ho_SelectedRegions?.Dispose();
                    ho_Region?.Dispose();
                    ho_ConnectedRegions?.Dispose();
                    ho_RegionFillUp?.Dispose();
                    ho_SelectedRegions?.Dispose();

                }
                else if (strVisionPrName == "黑帽定位")
                {
                    visionSetpBasesSnapBlackCaps[index - 1].ClearResult();
                    VisionFitCircircle vcbase = (VisionFitCircircle)visionSetpBasesSnapBlackCaps[index - 1];
                    vcbase.ClearResult();
                    visoModleLocation[index - 1].ClearResult();
                    shapeparam shapeparamInstance;
                    visoModleLocation[index - 1].Process_image(img, visionControl);
                    object obj = visoModleLocation[index - 1].GetResult();
                    if (obj != null && obj is VisionShapParam && ((VisionShapParam)obj).ResultRow.Count > 0)
                    {
                        shapeparamInstance = visoModleLocation[index - 1].GetRegionOuts().Find(t => t.name == "k1");
                        if ((UsrShapeCircle)shapeparamInstance.usrshape != null)
                        {
                            vcbase.visionFitCircleParam.point2DRoixy.x = ((UsrShapeCircle)shapeparamInstance.usrshape).CircleCenterX;
                            vcbase.visionFitCircleParam.point2DRoixy.y = ((UsrShapeCircle)shapeparamInstance.usrshape).CircleCenterY;
                            vcbase.visionFitCircleParam.RadiusRoi = ((UsrShapeCircle)shapeparamInstance.usrshape).CircleRadius;
                            return visionSetpBasesSnapBlackCaps[index - 1].Process_image(img, visionControl);
                        }
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Info("左贴装站：pr异常" + e.Message);
                return false;
            }

            // VisionMgr.GetInstance().ProcessImage(strVisionPrName, (HObject)img, visionControl);
            return true;
        }
        XYUPoint[] snapbuzzermachinePos = new XYUPoint[4];
        XYUPoint[] snapcapmachinePos = new XYUPoint[4];
        public void ZCyliderUp(bool bmanual = false)
        {
            WaranResult waranResult;
            retry_up_z_cylider:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", false);
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸1原位", true, " 左贴装站：左蜂鸣器Z轴气缸1原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸2原位", true, "左贴装站：左蜂鸣器Z轴气缸2原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站：左蜂鸣器Z轴气缸3原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站：左蜂鸣器Z轴气缸3原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
        }
        public void ZCyliderDown(bool bmanual = false)
        {
            WaranResult waranResult;
            retry_up_z_cylider:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", true);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", true);
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸1到位", true, " 左贴装站：左蜂鸣器Z轴气缸1到位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸2到位", true, "左贴装站：左蜂鸣器Z轴气缸2到位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3到位", true, "左贴装站：左蜂鸣器Z轴气缸3到位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3到位", true, "左贴装站：左蜂鸣器Z轴气缸3到位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
        }
        public void SanpBuzzer(int index, VisionControl vc, bool bmanual = false)//= this.VisionControl
        {
            ParamSetMgr.GetInstance().SetBoolParam("左边最后1个蜂鸣器拍照完成", false);
            ParamSetMgr.GetInstance().SetBoolParam(string.Format("左贴装站:{0}号蜂鸣器定位OK", index), false);
            Info(string.Format("开始{0}移动到{1}位置，拍照处理", bmanual ? "手动" : "自动", index));
            WaranResult waranResult;
            double z = GetStationPointDic()["蜂鸣器拍照左"].pointZ;
            double lx = GetStationPointDic()["蜂鸣器拍照左"].pointX;
            double ly = GetStationPointDic()["蜂鸣器拍照左"].pointY;
            double rx = GetStationPointDic()["蜂鸣器拍照右"].pointX;
            double ry = GetStationPointDic()["蜂鸣器拍照右"].pointY;
            double x = lx + (index - 1) * (rx - lx) / 3.000;
            double y = ly + (index - 1) * (ry - ly) / 3.000;

            LightControl.GetInstance().CloseLight("左上黑帽定位");
            LightControl.GetInstance().Light("左下蜂鸣器定位");

            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);

            bool XInpos = Math.Abs(currentx - x) < 50;
            bool YInpos = Math.Abs(currenty - y) < 50;
            bool XYInpos = XInpos & YInpos;
            if (bmanual && !XYInpos)
            {
                //手动状态 XY不在此位置
                ZCyliderUp(bmanual);
            }
            else if (!bmanual && !XYInpos && index == 1)
            {
                //自动状态 XY不在此位置 第一个黑帽
                ZCyliderUp(bmanual);
            }

            double SafeZ = GetStationPointDic()["安全左上"].pointZ;
            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 15, bmanual, this, 30000);

            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);

            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 15, bmanual, this, 30000);
            if (!bmanual && index == 1)
            {
                //自动状态 XY不在此位置 第一个黑帽
                ZCyliderDown(bmanual);
            }
            retry_z_down:
            IOMgr.GetInstace().WriteIoBit(string.Format("左蜂鸣器Z轴气缸{0}电磁阀", index), true);
            waranResult = CheckIobyName(string.Format("左蜂鸣器Z轴气缸{0}到位", index), true, string.Format("左贴装站：左蜂鸣器Z轴气缸{0}到位 到位失败，请检查感应器及气缸", index), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_down;
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));
            //采集
            HObject img = null;
            CameraMgr.GetInstance().BindWindow("LeftDownCCD", vc);
            CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetTriggerMode(CameraModeType.Software);
            CameraMgr.GetInstance().GetCamera("LeftDownCCD").StartGrab();
            double? exp = VisionMgr.GetInstance().GetExpourseTime("蜂鸣器定位");
            double? gain = VisionMgr.GetInstance().GetGain("蜂鸣器定位");
            CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetExposureTime((double)exp);
            CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetGain((double)gain);
            img = CameraMgr.GetInstance().GetCamera("LeftDownCCD").GetImage();

            //  LightControl.GetInstance().CloseLight(3);
            visionSetpBasesSnapBuzzers[index - 1].ClearResult();
            XYUPoint SanpMachinePos = new XYUPoint(MotionMgr.GetInstace().GetAxisPos(AxisX), MotionMgr.GetInstace().GetAxisPos(AxisY), 0);
            retry_z_up2:
            IOMgr.GetInstace().WriteIoBit(string.Format("左蜂鸣器Z轴气缸{0}电磁阀", index), false);
            if( bmanual)
            {
                waranResult = CheckIobyName(string.Format("左蜂鸣器Z轴气缸{0}原位", index), true, string.Format("左贴装站：左蜂鸣器Z轴气缸{0}原位 到位失败，请检查感应器及气缸", index), bmanual);
                if (waranResult == WaranResult.Retry)
                    goto retry_z_up2;
            }
            snapbuzzermachinePos[index - 1] = SanpMachinePos;
            Func<string, HObject, int, VisionControl, bool> func = new Func<string, HObject, int, VisionControl, bool>(pr);
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)index - 1].nozzleState = NozzleState.HaveSnaped1;
            func.BeginInvoke("蜂鸣器定位", img, index, vc, (iar) =>
            {
                AsyncResult asyncobj = (AsyncResult)iar;
                Func<string, HObject, int, VisionControl, bool> funobj = (Func<string, HObject, int, VisionControl, bool>)asyncobj.AsyncDelegate;
                bool bresult = funobj.EndInvoke(iar);
                Info(string.Format("左贴装站:第{0}号吸嘴蜂鸣器定位状态{1}", (int)iar.AsyncState, bresult ? "OK" : "NG"));

                object objresult = visionSetpBasesSnapBuzzers[(int)iar.AsyncState - 1].GetResult(); //VisionMgr.GetInstance().GetResult(strVisionPrName);
                if (objresult != null)
                {
                    VisionFitCircleParam visionShapParam = (VisionFitCircleParam)objresult;
                    if (visionShapParam.GetResultNum() > 0)
                    {
                        Info(string.Format("左贴装站:{0}号蜂鸣器定位OK", (int)(int)iar.AsyncState));
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("左贴装站:{0}号蜂鸣器定位OK", (int)iar.AsyncState), true);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)iar.AsyncState - 1].nozzleState = NozzleState.HaveSnapOK1;
                        XYUPoint visionLocationPos = new XYUPoint(visionShapParam.Resultpoint2D.x, visionShapParam.Resultpoint2D.y, 0);
                        XYUPoint objmachine = VisionAddtion.xyleftCalib.GetObjPonit(visionLocationPos, snapbuzzermachinePos[(int)iar.AsyncState - 1]);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)iar.AsyncState - 1].ObjMachinePos = objmachine;
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)iar.AsyncState - 1].ObjSnapMachinePos = snapbuzzermachinePos[(int)iar.AsyncState - 1];
                    }
                    else
                    {
                        Info(string.Format("左贴装站:{0}号蜂鸣器定位NG", (int)(int)iar.AsyncState));
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("左贴装站:{0}号蜂鸣器定位OK", (int)iar.AsyncState), false);
                    }
                }
                if (sys.g_AppMode == AppMode.AirRun)
                {
                    Info(string.Format("左贴装站:{0}号蜂鸣器定位OK", (int)(int)iar.AsyncState));
                    ParamSetMgr.GetInstance().SetBoolParam(string.Format("左贴装站:{0}号蜂鸣器定位OK", (int)iar.AsyncState), true);
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)iar.AsyncState - 1].nozzleState = NozzleState.HaveSnapOK1;
                }
                if ((int)iar.AsyncState == 4)
                    ParamSetMgr.GetInstance().SetBoolParam("左边最后1个蜂鸣器拍照完成", true);
                else
                    ParamSetMgr.GetInstance().SetBoolParam("左边最后1个蜂鸣器拍照完成", false);


            }, index);
        }
        public void SanpBuzzerAndProcess(int index, bool bmanual = false)
        {
            ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号蜂鸣器定位OK", index), false);
            Info(string.Format("开始{0}移动到{1}位置，拍照处理", bmanual ? "手动" : "自动", index));
            WaranResult waranResult;
            ZCyliderUp(bmanual);
            double z = GetStationPointDic()["蜂鸣器拍照左"].pointZ;
            double lx = GetStationPointDic()["蜂鸣器拍照左"].pointX;
            double ly = GetStationPointDic()["蜂鸣器拍照左"].pointY;
            double rx = GetStationPointDic()["蜂鸣器拍照右"].pointX;
            double ry = GetStationPointDic()["蜂鸣器拍照右"].pointY;
            double x = lx + (index - 1) * (rx - lx) / 3.000;
            double y = ly + (index - 1) * (ry - ly) / 3.000;
            LightControl.GetInstance().CloseLight("左上黑帽定位");
            LightControl.GetInstance().Light("左下蜂鸣器定位");

            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);
            double SafeZ = GetStationPointDic()["安全左上"].pointZ;
            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 5, bmanual, this, 30000);

            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);

            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 5, bmanual, this, 30000);

            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));
            HObject img = null;
            DoWhile doWhile = new DoWhile((time, dowhile, bmanual2, obj) =>
            {
                object objresult = visionSetpBasesSnapBuzzers[(int)index - 1].GetResult();
                if (objresult != null)
                {
                    VisionFitCircleParam visionShapParam = (VisionFitCircleParam)objresult;
                    if (visionShapParam.GetResultNum() > 0)
                    {
                        Info(string.Format("{0}号蜂鸣器定位OK", index));
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号蜂鸣器定位OK", index), true);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState = NozzleState.HaveSnapOK1;

                        XYUPoint visionLocationPos = new XYUPoint(visionShapParam.Resultpoint2D.x, visionShapParam.Resultpoint2D.y, 0);
                        XYUPoint objmachine = VisionAddtion.xyleftCalib.GetObjPonit(visionLocationPos, snapbuzzermachinePos[index - 1]);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].ObjMachinePos = objmachine;
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].ObjSnapMachinePos = snapbuzzermachinePos[index - 1];
                        return WaranResult.Run;
                    }
                    else
                    {
                        Info(string.Format("{0}号蜂鸣器定位NG", index));
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState = NozzleState.HaveSnapNG1;
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号蜂鸣器定位OK", index), false);
                        if (obj != null && obj.Length > 0 && ((int)obj[0]) == 1)
                            return AlarmMgr.GetIntance().WarnWithDlg(string.Format("{0}号蜂鸣器定位NG,", index), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                        else
                            return WaranResult.Failture;
                    }
                }
                if (time > 500)
                {
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState = NozzleState.HaveSnapNG1;
                    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号蜂鸣器定位OK", index), false);
                    if (obj != null && obj.Length > 0 && ((int)obj[0]) == 1)
                        return AlarmMgr.GetIntance().WarnWithDlg(string.Format("{0}号蜂鸣器定位NG 超时,", index), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                    else
                        return WaranResult.TimeOut;
                }
                else
                    return WaranResult.CheckAgain;

            }, 20000);

            double? exp = VisionMgr.GetInstance().GetExpourseTime("蜂鸣器定位");
            double? gain = VisionMgr.GetInstance().GetGain("蜂鸣器定位");
            double exporetimeval = (double)exp;
            double gainval = (double)gain;
            int ch = LightControl.GetInstance().itemlightdic["左下蜂鸣器定位"].nCh;
            int lightval = LightControl.GetInstance().itemlightdic["左下蜂鸣器定位"].lightval;
            for (int s = 0; s < 5; s++)
            {
                LightControl.GetInstance().Light(ch, lightval + s * 5);
                CameraMgr.GetInstance().BindWindow("LeftDownCCD", this.VisionControl2);
                CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetTriggerMode(CameraModeType.Software);
                CameraMgr.GetInstance().GetCamera("LeftDownCCD").StartGrab();
                exporetimeval = (double)exp + 200 * s;

                CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetExposureTime((double)exporetimeval);
                CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetGain((double)gainval);

                img = CameraMgr.GetInstance().GetCamera("LeftDownCCD").GetImage();
                LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                int timedelay = ParamSetMgr.GetInstance().GetIntParam("开光源延时");
                Thread.Sleep(timedelay);
                visionSetpBasesSnapBuzzers[index - 1].ClearResult();
                XYUPoint SanpMachinePos = new XYUPoint(MotionMgr.GetInstace().GetAxisPos(AxisX), MotionMgr.GetInstace().GetAxisPos(AxisY), 0);
                snapbuzzermachinePos[index - 1] = SanpMachinePos;
                bool bPR = pr("蜂鸣器定位", img, index, VisionControl2);
                img?.Dispose();
                if (sys.g_AppMode == AppMode.AirRun)
                {
                    Info(string.Format("{0}号蜂鸣器定位OK", index));
                    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号蜂鸣器定位OK", index), true);
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState = NozzleState.HaveSnapOK1;
                    return;
                }
                if (!bPR)
                    continue;
                waranResult = doWhile.doSomething(this, doWhile, bmanual, 0);
                if (waranResult == WaranResult.Run)
                    return;
            }
            if (ParamSetMgr.GetInstance().GetIntParam("是否抛料处理") == 1)
            {
                //抛料 处理

                NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState = NozzleState.HaveSnapNG1;
            }
            else
            {
                int nRetryCount = 0;

                Retry_SanpCap:
                LightControl.GetInstance().Light("左下蜂鸣器定位");
                LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                if (nRetryCount % 2 == 0)
                    LightControl.GetInstance().Light(ch, lightval + nRetryCount * 5);
                else
                    LightControl.GetInstance().Light(ch, lightval - nRetryCount * 5);
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 3, bmanual);
                Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));

                CameraMgr.GetInstance().BindWindow("LeftDownCCD", this.VisionControl2);
                CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetTriggerMode(CameraModeType.Software);
                CameraMgr.GetInstance().GetCamera("LeftDownCCD").StartGrab();
                exp = VisionMgr.GetInstance().GetExpourseTime("蜂鸣器定位");
                gain = VisionMgr.GetInstance().GetGain("蜂鸣器定位");
                CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetExposureTime((double)exp);
                CameraMgr.GetInstance().GetCamera("LeftDownCCD").SetGain((double)gain);
                img = CameraMgr.GetInstance().GetCamera("LeftDownCCD").GetImage();
                visionSetpBasesSnapBuzzers[index - 1].ClearResult();
                XYUPoint SanpMachinePos = new XYUPoint(MotionMgr.GetInstace().GetAxisPos(AxisX), MotionMgr.GetInstace().GetAxisPos(AxisY), 0);
                snapbuzzermachinePos[index - 1] = SanpMachinePos;
                bool bPR = pr("蜂鸣器定位", img, index, VisionControl2);
                img?.Dispose();
                if (sys.g_AppMode == AppMode.AirRun)
                {
                    Info(string.Format("{0}号蜂鸣器定位OK", index));
                    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号蜂鸣器定位OK", index), true);
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState = NozzleState.HaveSnapOK1;
                    return;
                }
                waranResult = doWhile.doSomething(this, doWhile, bmanual, 1);
                if (waranResult == WaranResult.Run)
                    return;
                if (waranResult == WaranResult.Retry)
                    goto Retry_SanpCap;
            }

        }

        public void SanpBlackCap(int index, VisionControl vc, bool bmanual = false)
        {
            ParamSetMgr.GetInstance().SetBoolParam("左边最后1个黑帽拍照完成", false);
            WaranResult waranResult;
            ParamSetMgr.GetInstance().SetBoolParam(string.Format("左贴装站:{0}号黑帽定位OK", index), false);
            ZCyliderUp(bmanual);
            Info(string.Format("开始{0}移动到{1}位置，拍照处理", bmanual ? "手动" : "自动", index));

            double z = GetStationPointDic()["黑帽拍照左"].pointZ;
            double lx = GetStationPointDic()["黑帽拍照左"].pointX;
            double ly = GetStationPointDic()["黑帽拍照左"].pointY;
            double rx = GetStationPointDic()["黑帽拍照右"].pointX;
            double ry = GetStationPointDic()["黑帽拍照右"].pointY;
            double x = lx + (index - 1) * (rx - lx) / 3.000;
            double y = ly + (index - 1) * (ry - ly) / 3.000;
            LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
            LightControl.GetInstance().Light("左上黑帽定位");

            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);
            double SafeZ = GetStationPointDic()["安全左上"].pointZ;
            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 5, bmanual, this, 30000);

            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);

            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 5, bmanual, this, 30000);
            // MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 3, bmanual);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));
            //采集

            HObject img = null;
            CameraMgr.GetInstance().BindWindow("LeftUpCCD", vc);
            CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetTriggerMode(CameraModeType.Software);
            CameraMgr.GetInstance().GetCamera("LeftUpCCD").StartGrab();
            double? exp = VisionMgr.GetInstance().GetExpourseTime("黑帽定位");
            double? gain = VisionMgr.GetInstance().GetGain("黑帽定位");
            CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetExposureTime((double)exp);
            CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetGain((double)gain);
            img = CameraMgr.GetInstance().GetCamera("LeftUpCCD").GetImage();

            //LightControl.GetInstance().CloseLight(3);
            XYUPoint SanpMachinePos = new XYUPoint(MotionMgr.GetInstace().GetAxisPos(AxisX), MotionMgr.GetInstace().GetAxisPos(AxisY), 0);
            snapcapmachinePos[index - 1] = SanpMachinePos;
            visionSetpBasesSnapBlackCaps[index - 1].ClearResult();

            Func<string, HObject, int, VisionControl, bool> func = new Func<string, HObject, int, VisionControl, bool>(pr);
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)index - 1].nozzleState2 = NozzleState.HaveSnaped2;
            func.BeginInvoke("黑帽定位", img, index, vc, (iar) =>
            {
                AsyncResult asyncobj = (AsyncResult)iar;
                Func<string, HObject, int, VisionControl, bool> funobj = (Func<string, HObject, int, VisionControl, bool>)asyncobj.AsyncDelegate;
                bool bresult = funobj.EndInvoke(iar);
                Info(string.Format("左贴装站:第{0}号黑帽定位状态{1}", (int)iar.AsyncState, bresult ? "OK" : "NG"));
                object objresult = visionSetpBasesSnapBlackCaps[(int)iar.AsyncState - 1].GetResult(); //VisionMgr.GetInstance().GetResult(strVisionPrName);

                if (objresult != null)
                {
                    VisionFitCircleParam visionShapParam = (VisionFitCircleParam)objresult;
                    if (visionShapParam.GetResultNum() > 0)
                    {
                        Info(string.Format("{0}号黑帽定位OK", (int)(int)iar.AsyncState));
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号黑帽定位OK", (int)(int)iar.AsyncState), true);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)iar.AsyncState - 1].nozzleState2 = NozzleState.HaveSnapOK2;
                        XYUPoint DstVisionPos = new XYUPoint(visionShapParam.Resultpoint2D.x, visionShapParam.Resultpoint2D.y, 0);
                        XYUPoint dstmachinepos = VisionAddtion.xyleftCalib.GetDstPonit(DstVisionPos, snapcapmachinePos[(int)iar.AsyncState - 1]);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)iar.AsyncState - 1].DstMachinePos = dstmachinepos;
                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + (int)iar.AsyncState - 1].Cellstate2 = SocketCellState.CellStateOK;
                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + (int)iar.AsyncState - 1].pos.x = dstmachinepos.x;
                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + (int)iar.AsyncState - 1].pos.y = dstmachinepos.y;
                    }
                    else
                    {
                        Info(string.Format("{0}号黑帽定位NG", (int)(int)iar.AsyncState));
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号黑帽定位OK", (int)(int)iar.AsyncState), false);
                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].Cellstate2 = SocketCellState.CellStateNG;
                    }
                }
                if (!bresult)
                {
                    Info(string.Format("{0}号黑帽定位NG", (int)(int)iar.AsyncState));
                    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号黑帽定位OK", (int)(int)iar.AsyncState), false);
                    SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].Cellstate2 = SocketCellState.CellStateNG;
                }
                //if (sys.g_AppMode == AppMode.AirRun)
                //{
                //    Info(string.Format("左贴装站:{0}号黑帽定位OK", (int)(int)iar.AsyncState));
                //    ParamSetMgr.GetInstance().SetBoolParam(string.Format("左贴装站:{0}号黑帽定位OK", (int)iar.AsyncState), true);
                //    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + (int)iar.AsyncState - 1].nozzleState2 = NozzleState.HaveSnapOK2;
                //}
                if ((int)iar.AsyncState == 4)
                    ParamSetMgr.GetInstance().SetBoolParam("左边最后1个黑帽拍照完成", true);
                else
                    ParamSetMgr.GetInstance().SetBoolParam("左边最后1个黑帽拍照完成", false);

            }, index);


        }

        public void SanpBlackCapAndProcess(int index, bool bmanual = false)
        {
            WaranResult waranResult;
            ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号黑帽定位OK", index), false);
            ZCyliderUp(bmanual);
            Info(string.Format("开始{0}移动到{1}位置，拍照处理", bmanual ? "手动" : "自动", index));

            double z = GetStationPointDic()["黑帽拍照左"].pointZ;
            double lx = GetStationPointDic()["黑帽拍照左"].pointX;
            double ly = GetStationPointDic()["黑帽拍照左"].pointY;
            double rx = GetStationPointDic()["黑帽拍照右"].pointX;
            double ry = GetStationPointDic()["黑帽拍照右"].pointY;
            double x = lx + (index - 1) * (rx - lx) / 3.000;
            double y = ly + (index - 1) * (ry - ly) / 3.000;
            LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
            LightControl.GetInstance().Light("左上黑帽定位");

            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);
            double SafeZ = GetStationPointDic()["安全左上"].pointZ;
            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 5, bmanual, this, 30000);

            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);

            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 5, bmanual, this, 30000);
            // MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 3, bmanual);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));
            DoWhile doWhile = new DoWhile((time, dowhile, bmanual2, obj) =>
            {
                object objresult = visionSetpBasesSnapBlackCaps[(int)index - 1].GetResult();
                if (objresult != null)
                {
                    VisionFitCircleParam visionShapParam = (VisionFitCircleParam)objresult;
                    if (visionShapParam.GetResultNum() > 0)
                    {
                        Info(string.Format("{0}号黑帽定位OK", index));
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号黑帽定位OK", index), true);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState2 = NozzleState.HaveSnapOK2;
                        XYUPoint DstVisionPos = new XYUPoint(visionShapParam.Resultpoint2D.x, visionShapParam.Resultpoint2D.y, 0);
                        XYUPoint dstmachinepos = VisionAddtion.xyleftCalib.GetDstPonit(DstVisionPos, snapcapmachinePos[index - 1]);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].DstMachinePos = dstmachinepos;

                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].pos.x = dstmachinepos.x;
                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].pos.y = dstmachinepos.y;
                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].Cellstate2 = SocketCellState.CellStateOK;
                        return WaranResult.Run;
                    }
                    else
                    {
                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].Cellstate2 = SocketCellState.CellStateNG;
                        Info(string.Format("{0}号黑帽定位NG", index));
                        ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号黑帽定位OK", index), false);
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState2 = NozzleState.HaveSnapNG2;
                        if (obj != null && obj.Length > 0 && ((int)obj[0]) == 1)
                            return AlarmMgr.GetIntance().WarnWithDlg(string.Format("{0}号黑帽定位NG,", index), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                        else
                            return WaranResult.Failture;
                    }

                }
                if (time > 500)
                {
                    SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].Cellstate2 = SocketCellState.CellStateNG;
                    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号黑帽定位OK", index), false);
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState2 = NozzleState.HaveSnapNG2;
                    if (obj != null && obj.Length > 0 && ((int)obj[0]) == 1)
                        return AlarmMgr.GetIntance().WarnWithDlg(string.Format("{0}号黑帽定位NG 超时", index), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                    else
                        return WaranResult.TimeOut;
                }


                else
                    return WaranResult.CheckAgain;

            }, 20000);
            HObject img = null;
            double? exp = VisionMgr.GetInstance().GetExpourseTime("黑帽定位");
            double? gain = VisionMgr.GetInstance().GetGain("黑帽定位");
            double exptimeval = (double)exp;
            double gainval = (double)gain;
            int lightval = LightControl.GetInstance().itemlightdic["左上黑帽定位"].lightval;
            int nch = LightControl.GetInstance().itemlightdic["左上黑帽定位"].nCh;
            for (int s = 0; s < 5; s++)
            {

                //采集
                LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                LightControl.GetInstance().Light(nch, 5 * s + lightval);
                CameraMgr.GetInstance().BindWindow("LeftUpCCD", this.VisionControl);
                CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetTriggerMode(CameraModeType.Software);
                CameraMgr.GetInstance().GetCamera("LeftUpCCD").StartGrab();
                exptimeval = (double)exp + s * 100;
                CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetExposureTime(exptimeval);
                CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetGain(gainval);
                int timedelay = ParamSetMgr.GetInstance().GetIntParam("开光源延时");
                Thread.Sleep(timedelay);
                img = CameraMgr.GetInstance().GetCamera("LeftUpCCD").GetImage();
                visionSetpBasesSnapBlackCaps[index - 1].ClearResult();
                //LightControl.GetInstance().CloseLight("左上黑帽定位");
                XYUPoint SanpMachinePos = new XYUPoint(MotionMgr.GetInstace().GetAxisPos(AxisX), MotionMgr.GetInstace().GetAxisPos(AxisY), 0);
                snapcapmachinePos[index - 1] = SanpMachinePos;
                if (img == null || !img.IsInitialized())
                {
                    Thread.Sleep(100);
                    img = CameraMgr.GetInstance().GetCamera("LeftUpCCD").GetImage();
                }
                bool brtn = pr("黑帽定位", img, index, VisionControl);
                img?.Dispose();
                if (!brtn)
                    continue;
                waranResult = doWhile.doSomething(this, doWhile, bmanual, 0);
                if (waranResult == WaranResult.Run)
                    return;
            }
            if (ParamSetMgr.GetInstance().GetIntParam("是否抛料处理") == 1)
            {
                //抛料 处理
                NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState2 = NozzleState.HaveSnapNG2;
                SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].Cellstate2 = SocketCellState.CellStateNG;
            }
            else
            {
                int nRetryCount = 0;
                Retry_SanpCap:
                LightControl.GetInstance().Light("左上黑帽定位");
                LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                if (nRetryCount % 2 == 0)
                    LightControl.GetInstance().Light(nch, lightval + nRetryCount * 5);
                else
                    LightControl.GetInstance().Light(nch, lightval - nRetryCount * 5);
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual);
                Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("电机停止时间"));

                CameraMgr.GetInstance().BindWindow("LeftUpCCD", this.VisionControl2);
                CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetTriggerMode(CameraModeType.Software);
                CameraMgr.GetInstance().GetCamera("LeftUpCCD").StartGrab();
                exp = VisionMgr.GetInstance().GetExpourseTime("黑帽定位");
                gain = VisionMgr.GetInstance().GetGain("黑帽定位");
                CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetExposureTime((double)exp);
                CameraMgr.GetInstance().GetCamera("LeftUpCCD").SetGain((double)gain);
                img = CameraMgr.GetInstance().GetCamera("LeftUpCCD").GetImage();
                visionSetpBasesSnapBlackCaps[index - 1].ClearResult();
                XYUPoint SanpMachinePos = new XYUPoint(MotionMgr.GetInstace().GetAxisPos(AxisX), MotionMgr.GetInstace().GetAxisPos(AxisY), 0);
                snapcapmachinePos[index - 1] = SanpMachinePos;
                bool bPR = pr("黑帽定位", img, index, VisionControl);
                img?.Dispose();
                //if (sys.g_AppMode == AppMode.AirRun)
                //{
                //    Info(string.Format("{0}号黑帽定位OK", index));
                //    ParamSetMgr.GetInstance().SetBoolParam(string.Format("{0}号黑帽定位OK", index), true);
                //    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState2 = NozzleState.HaveSnapOK2;
                //    return;
                //}
                waranResult = doWhile.doSomething(this, doWhile, bmanual, 1);
                if (waranResult == WaranResult.Run)
                    return;
                if (waranResult == WaranResult.Retry)
                    goto Retry_SanpCap;
                if (waranResult == WaranResult.Ignore)
                    SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + index - 1].Cellstate2 = SocketCellState.CellStateNG;
            }



        }

        public override void PauseDeal()
        {
            bool bhave = MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenLeftPackageXYAxisMoveing);
            if (!bhave)
                MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenLeftPackageXYAxisMoveing;
        }
        public override void ResumeDeal()
        {
            bool bhave = MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenLeftPackageXYAxisMoveing);
            if (bhave)
                MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenLeftPackageXYAxisMoveing;
        }
        public override void EmgDeal()
        {
            bool bhave = MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenLeftPackageXYAxisMoveing);
            if (!bhave)
                MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenLeftPackageXYAxisMoveing;
        }
        public override void StopDeal()
        {
            LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
            LightControl.GetInstance().CloseLight("左上黑帽定位");
            bool bhave = MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenLeftPackageXYAxisMoveing);
            if (!bhave)
                MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenLeftPackageXYAxisMoveing;
        }


        public bool CheckSocketSubItem(int indexSocket = (int)SocketType.stick1, int indexCel = 0)
        {
            return SocketMgr.GetInstance().socketArr[indexSocket].socketcells[indexCel + 4].Cellstate2 == SocketCellState.CellStateOK &&
                   SocketMgr.GetInstance().socketArr[indexSocket].socketcells[indexCel + 4].Cellstate == SocketCellState.CellStateOK;
        }
        List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();
        List<Tuple<int, int>> tuplesCopey = new List<Tuple<int, int>>();
        List<Tuple<int, NozzleState>> listNozzles = new List<Tuple<int, NozzleState>>();
        List<Tuple<int, SocketState>> listSocket = new List<Tuple<int, SocketState>>();
        List<int> nListDiscard = new List<int>();
        List<int> nListplace = new List<int>();
        public void PlaceToSocket2(bool bmanual = false)
        {
            LightControl.GetInstance().CloseLight("左上黑帽定位");
            LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
            Info("左贴装站：进行贴装");
            int nSumCapOK = 0;
            int nSumNozzle = 0;
            nListDiscard.Clear();
            nListplace.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (CheckSocketSubItem((int)SocketType.stick1, i))
                {
                    if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].nozzleState == NozzleState.HaveSnapOK1)
                    {
                        if (sys.g_AppMode != AppMode.AirRun)
                        {
                            string strposname = string.Format("吸嘴{0}放料点", i + 1);
                            double xoffset = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].xYUOffset.x * 1000;
                            double yoffset = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].xYUOffset.y * 1000;
                            double vecx = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].DstMachinePos.x - NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].ObjMachinePos.x;
                            double vecy = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].DstMachinePos.y - NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].ObjMachinePos.y;
                            double x = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].ObjSnapMachinePos.x + vecx + xoffset;
                            double y = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].ObjSnapMachinePos.y + vecy + yoffset;
                            double z = GetStationPointDic()[strposname].pointZ;
                            double PbMaxDistance = ParamSetMgr.GetInstance().GetDoubleParam("贴装XY最大距离");
                            if (PbMaxDistance < 0)
                                PbMaxDistance = 0;
                            if (Math.Abs(x - GetStationPointDic()[strposname].pointX) > PbMaxDistance* 1000 || Math.Abs(y - GetStationPointDic()[strposname].pointY) > PbMaxDistance * 1000)
                                nListDiscard.Add(i);
                            else
                                nListplace.Add(i);
                        }
                        else
                            nListplace.Add(i);
                    }
                    else
                        nListDiscard.Add(i);
                }
                else
                    nListDiscard.Add(i);
            }
            foreach (var temp in nListDiscard)
            {
                GoDiscradProductPos(temp, bmanual);
            }
            foreach (var temp in nListplace)
            {
                PlaceToSocket(temp + 1, bmanual);
            }
            for (int i = 0; i < 4; i++)
            {
                NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].nozzleState = NozzleState.None;
                NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].nozzleState2 = NozzleState.None;
            }
            SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState = SocketState.HaveHaftOK;
            for (int i = 4; i < 8; i++)
            {
                SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[i].Cellstate = SocketCellState.CellStateNone;
                SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[i].Cellstate2 = SocketCellState.CellStateNone;
            }

        }
        public void PlaceToSocket(bool bmanual = false)
        {
            Info("左贴装站：进行贴装");
            cUserTimersnapcap.Stop();
            cUserTimersnapbuzzer.Stop();
            listNozzles.Clear();
            listSocket.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (CheckSocketSubItem((int)SocketType.stick1, i))
                    listSocket.Add(new Tuple<int, SocketState>(i, SocketState.HaveOK));
                else
                    listSocket.Add(new Tuple<int, SocketState>(i, SocketState.HaveNG));

                if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].nozzleState == NozzleState.HaveSnapOK1)
                    listNozzles.Add(new Tuple<int, NozzleState>(i, NozzleState.HaveOK));
                else
                    listNozzles.Add(new Tuple<int, NozzleState>(i, NozzleState.HaveNg));
            }

            for (int i = 0; i < 4; i++)
            {
                if (listSocket[i].Item2 == SocketState.HaveOK && listNozzles[i].Item2 == NozzleState.HaveOK)
                {
                    tuples.Add(new Tuple<int, int>(i, i));
                    tuplesCopey.Add(new Tuple<int, int>(i, i));
                }
            }




            for (int i = 0; i < tuples.Count; i++)
            {
                Tuple<int, NozzleState> tupleNozzletemp = listNozzles.Find(t => tuples[i].Item1 == t.Item1);
                Tuple<int, SocketState> tupleSockettemp = listSocket.Find(t => tuples[i].Item1 == t.Item1);
                listSocket.Remove(tupleSockettemp);
                listNozzles.Remove(tupleNozzletemp);
            }
            List<Tuple<int, SocketState>> listokSocket = listSocket.FindAll(t => SocketState.HaveOK == t.Item2);
            List<Tuple<int, NozzleState>> listokNozzle = listNozzles.FindAll(t => NozzleState.HaveOK == t.Item2);

            int nNumSocketOK = 0; nNumSocketOK = listokSocket == null ? 0 : listokSocket.Count;
            int nNumNozzleOK = 0; nNumNozzleOK = listokNozzle == null ? 0 : listokNozzle.Count;

            int indexGet = 0;
            int nNum = nNumNozzleOK < nNumSocketOK ? nNumNozzleOK : nNumSocketOK;
            if (nNumSocketOK <= nNumNozzleOK)
            {
                indexGet = 0;
                for (int i = 0; i < nNum; i++)
                {
                    indexGet++;
                    tuples.Add(new Tuple<int, int>(listokNozzle[i].Item1, listokSocket[i].Item1));
                }
                // indexGet++; ;
                for (int j = indexGet; j < listokNozzle.Count; j++)
                {
                    GoDiscradProductPos(listokNozzle[j].Item1);
                }
            }
            else
            {
                for (int i = 0; i < nNum; i++)
                {
                    indexGet++;
                    tuples.Add(new Tuple<int, int>(listokNozzle[i].Item1, listokSocket[i].Item1));
                }
            }


            for (int i = 0; i < 4; i++)
            {
                if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].nozzleState == NozzleState.HaveSnapNG1)
                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].nozzleState = NozzleState.None;

            }
            for (int i = 0; i <= tuples.Count; i++)
            {
                int indexnozzle = tuples[i].Item1 + 1;
                int indexCap = tuples[i].Item2 + 1;
                PlaceToSocket(indexnozzle, indexCap, bmanual);
            }
            SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState = SocketState.HaveHaftOK;

        }
        public void PlaceToSocket(int index, bool bmanual = false)
        {
            string strVacName = string.Format("左蜂鸣器真空吸{0}电磁阀", index);
            string strVacbreakName = string.Format("左蜂鸣器破真空{0}电磁阀", index);
            string strVacCheckName = string.Format("左蜂鸣器真空检测{0}", index);
            string strcylidername = string.Format("左蜂鸣器Z轴气缸{0}电磁阀", index);
            string strcyliderCheckOriPosname = string.Format("左蜂鸣器Z轴气缸{0}原位", index);
            string strcyliderCheckInPosname = string.Format("左蜂鸣器Z轴气缸{0}到位", index);
            WaranResult waranResult;
            ZCyliderUp(bmanual);

            double x = 0;
            double y = 0;
            double z = 0;
            string strposname = string.Format("吸嘴{0}放料点", index);
            if (sys.g_AppMode == AppMode.AirRun)
            {
                Info("左贴装站： 空跑到" + strposname);
                x = GetStationPointDic()[strposname].pointX;
                y = GetStationPointDic()[strposname].pointY;
                z = GetStationPointDic()[strposname].pointZ;
                //  MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z },
                //      new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, false, this, 60000);
            }
            else
            {
                double xoffset = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].xYUOffset.x * 1000;
                double yoffset = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].xYUOffset.y * 1000;
                double vecx = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].DstMachinePos.x - NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].ObjMachinePos.x;
                double vecy = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].DstMachinePos.y - NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].ObjMachinePos.y;
                x = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].ObjSnapMachinePos.x + vecx + xoffset;
                y = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].ObjSnapMachinePos.y + vecy + yoffset;
                z = GetStationPointDic()[strposname].pointZ;
                //  MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] {x, y, z },
                //        new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 3, bmanual, this, 60000);


            }
            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);
            double SafeZ = GetStationPointDic()["安全左上"].pointZ;

            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 5, bmanual, this, 30000);

            if (bYIsSafe)
            {
                bool bhave = MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenLeftPackageXYAxisMoveing);
                if (bhave)
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenLeftPackageXYAxisMoveing;
                MoveMulitAxisPosWaitInpos2(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000
                //    new Action(() => {
                //    bool bxInpos = Math.Abs(MotionMgr.GetInstace().GetAxisPos(AxisX) - x) < 100;
                //    bool bYInpos = Math.Abs(MotionMgr.GetInstace().GetAxisPos(AxisY) - y) < 100;
                //    if (bxInpos && bYInpos)
                //    {
                //        IOMgr.GetInstace().WriteIoBit(strcylidername, true);
                //    }
                //})
                );
            }
            else
            {
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);
                IOMgr.GetInstace().WriteIoBit(strcylidername, true);
            }
            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 5, bmanual, this, 30000);

            Stopwatch stopwatch = new Stopwatch();
            retry_z_cylider_down:
            IOMgr.GetInstace().WriteIoBit(strcylidername, true);
            waranResult = CheckIobyName(strcyliderCheckInPosname, true, string.Format("左贴装站:{0}  没有到位，请检查气缸感应器及气路，线路", strcyliderCheckInPosname), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_down;
            int downMintime = ParamSetMgr.GetInstance().GetIntParam("贴装z轴气缸下降最小时间");
            long timems = stopwatch.ElapsedMilliseconds;
            if (timems < downMintime)
                Thread.Sleep(downMintime - (int)timems);

            IOMgr.GetInstace().WriteIoBit(strVacbreakName, false);
            IOMgr.GetInstace().WriteIoBit(strVacName, false);
            Thread.Sleep(10);
            IOMgr.GetInstace().WriteIoBit(strVacbreakName, true);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("破真空延时"));

            retry_z_cylider_upagain:
            IOMgr.GetInstace().WriteIoBit(strcylidername, false);
            waranResult = CheckIobyName(strcyliderCheckOriPosname, true, string.Format("左贴装站:{0}  没有到位，请检查气缸感应器及气路，线路", strcyliderCheckOriPosname), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_upagain;

            waranResult = CheckIobyName(strVacCheckName, false, string.Format("左贴装站:{0}  有信号，请检查气缸感应器及物料是否分离", strVacCheckName), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_down;
            IOMgr.GetInstace().WriteIoBit(strVacbreakName, false);
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index - 1].nozzleState2 = NozzleState.None;
        }

        public void PlaceToSocket(int indexNozzle, int indexCap, bool bmanual = false)
        {

            string info = string.Format("{0} 吸嘴 上蜂鸣器 贴装到{1} 黑帽上", indexNozzle, indexCap);
            Info(info);
            WaranResult waranResult;
            retry_z_cylider_up:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", false);
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸1原位", true, "左贴装站:左蜂鸣器Z轴气缸1原位 没有到位，请检查气缸感应器及气路，线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_up;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸2原位", true, "左贴装站:左蜂鸣器Z轴气缸2原位 没有到位，请检查气缸感应器及气路，线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_up;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站:左蜂鸣器Z轴气缸3原位 没有到位，请检查气缸感应器及气路，线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_up;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸4原位", true, "左贴装站:左蜂鸣器Z轴气缸4原位 没有到位，请检查气缸感应器及气路，线路", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_up;

            double x = 0;
            double y = 0;
            double z = 0;
            string strposname = string.Format("吸嘴{0}放料点", indexNozzle);
            if (sys.g_AppMode == AppMode.AirRun)
            {
                Info("左贴装站： 空跑到" + strposname);
                x = GetStationPointDic()[strposname].pointX;
                y = GetStationPointDic()[strposname].pointY;
                z = GetStationPointDic()[strposname].pointZ;
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z },
                    new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, false, this, 60000);
            }
            else
            {
                double xoffset = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + indexNozzle - 1].xYUOffset.x * 1000;
                double yoffset = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + indexNozzle - 1].xYUOffset.y * 1000;
                double dstx = SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + indexCap - 1].pos.x;
                double dsty = SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[4 + indexCap - 1].pos.y;
                double vecx = dstx - NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + indexNozzle - 1].ObjMachinePos.x;
                double vecy = dsty - NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + indexNozzle - 1].ObjMachinePos.y;
                double xx = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + indexNozzle - 1].ObjSnapMachinePos.x + vecx + xoffset;
                double yy = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + indexNozzle - 1].ObjSnapMachinePos.y + vecy + yoffset;
                z = GetStationPointDic()[strposname].pointZ;
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xx, yy, z },
                     new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 4, bmanual, this, 60000);


            }
            string strcylidername = string.Format("左蜂鸣器Z轴气缸{0}电磁阀", indexNozzle);
            string strcyliderCheckOriPosname = string.Format("左蜂鸣器Z轴气缸{0}原位", indexNozzle);
            string strcyliderCheckInPosname = string.Format("左蜂鸣器Z轴气缸{0}到位", indexNozzle);

            retry_z_cylider_down:
            IOMgr.GetInstace().WriteIoBit(strcylidername, true);
            waranResult = CheckIobyName(strcyliderCheckInPosname, true, string.Format("左贴装站:{0}  没有到位，请检查气缸感应器及气路，线路", strcyliderCheckInPosname), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_down;

            string strVacName = string.Format("左蜂鸣器真空吸{0}电磁阀", indexNozzle);
            string strVacbreakName = string.Format("左蜂鸣器破真空{0}电磁阀", indexNozzle);
            string strVacCheckName = string.Format("左蜂鸣器真空检测{0}", indexNozzle);
            IOMgr.GetInstace().WriteIoBit(strVacbreakName, false);
            IOMgr.GetInstace().WriteIoBit(strVacName, false);
            Thread.Sleep(10);
            IOMgr.GetInstace().WriteIoBit(strVacbreakName, true);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("破真空延时"));

            retry_z_cylider_upagain:
            IOMgr.GetInstace().WriteIoBit(strcylidername, false);
            waranResult = CheckIobyName(strcyliderCheckOriPosname, true, string.Format("左贴装站:{0}  没有到位，请检查气缸感应器及气路，线路", strcyliderCheckOriPosname), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_upagain;

            waranResult = CheckIobyName(strVacCheckName, false, string.Format("左贴装站:{0}  有信号，请检查气缸感应器及物料是否分离", strVacCheckName), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_z_cylider_down;
            IOMgr.GetInstace().WriteIoBit(strVacbreakName, false);
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + indexNozzle - 1].nozzleState = NozzleState.None;
            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + indexNozzle - 1].nozzleState2 = NozzleState.None;
        }

        public void init(bool bmanual = false)
        {
            WaranResult waranResult;
            double x = GetStationPointDic()["吸料准备位置"].pointX;
            double y = GetStationPointDic()["吸料准备位置"].pointY;
            double z = GetStationPointDic()["吸料准备位置"].pointZ;
            Info("左贴装站: 气缸开始自检");
            ParamSetMgr.GetInstance().SetBoolParam("左黑帽拍照完成", false);
            ParamSetMgr.GetInstance().SetBoolParam("左蜂鸣器拍照完成", false);
            retry_up_z_cylider:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", false);
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸1原位", true, " 左贴装站：左蜂鸣器Z轴气缸1原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸2原位", true, "左贴装站：左蜂鸣器Z轴气缸2原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站：左蜂鸣器Z轴气缸3原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站：左蜂鸣器Z轴气缸3原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;



            IOMgr.GetInstace().WriteIoBit("左蜂鸣器破真空1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器破真空2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器破真空3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器破真空4电磁阀", false);

            retry_check_vac1:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸1电磁阀", true);
            waranResult = CheckIobyName("左蜂鸣器真空检测1", false, "左贴装站：左蜂鸣器真空检测1 到位失败 可能被堵住，请检查感应器及吸嘴", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_check_vac1;
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸1电磁阀", false);

            retry_check_vac2:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸2电磁阀", true);
            waranResult = CheckIobyName("左蜂鸣器真空检测2", false, "左贴装站：左蜂鸣器真空检测2 到位失败 可能被堵住，请检查感应器及吸嘴", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_check_vac2;
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸2电磁阀", false);

            retry_check_vac3:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸3电磁阀", true);
            waranResult = CheckIobyName("左蜂鸣器真空检测3", false, "左贴装站：左蜂鸣器真空检测3 到位失败 可能被堵住，请检查感应器及吸嘴", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_check_vac3;
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸3电磁阀", false);

            retry_check_vac4:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸4电磁阀", true);
            waranResult = CheckIobyName("左蜂鸣器真空检测4", false, "左贴装站：左蜂鸣器真空检测4 到位失败 可能被堵住，请检查感应器及吸嘴", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_check_vac4;
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸4电磁阀", false);

            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器真空吸4电磁阀", false);


            retry_go_home_Z:
            Info("左贴装站 Z回原点");
            waranResult = HomeSigleAxisPosWaitInpos(AxisZ, this, 600000, bmanual);
            if (waranResult != WaranResult.Run)
            {
                AlarmMgr.GetIntance().WarnWithDlg(string.Format("左贴装{0}轴报警：{1}", MotionMgr.GetInstace().GetAxisName(AxisZ), waranResult), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto retry_go_home_Z;
            }
            retry_go_home_Y:
            Info("左贴装站 Y回原点");
            waranResult = HomeSigleAxisPosWaitInpos(AxisY, this, 600000, bmanual);
            if (waranResult != WaranResult.Run)
            {
                AlarmMgr.GetIntance().WarnWithDlg(string.Format("左贴装{0}轴报警：{1}", MotionMgr.GetInstace().GetAxisName(AxisY), waranResult), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto retry_go_home_Y;
            }
            retry_go_home_X:
            Info("左贴装站 X回原点");
            waranResult = HomeSigleAxisPosWaitInpos(AxisX, this, 600000, bmanual);
            if (waranResult != WaranResult.Run)
            {
                AlarmMgr.GetIntance().WarnWithDlg(string.Format("左贴装{0}轴报警：{1}", MotionMgr.GetInstace().GetAxisName(AxisX), waranResult), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto retry_go_home_X;
            }

            retry_go:
            Info("去吸料准备位置");
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (int)SpeedType.High, (int)SpeedType.High, (int)SpeedType.High, }, 10, bmanual, this, 60000);
            if (waranResult != WaranResult.Run)
            {
                AlarmMgr.GetIntance().WarnWithDlg(string.Format("左贴装 去吸料准备位置 报警：{0}", waranResult), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto retry_go;
            }
        }
        Tuple<int, NozzleState>[] tuple_resultsnapbuzzer = new Tuple<int, NozzleState>[4];
        Tuple<int, NozzleState>[] tuple_resultsnapcap = new Tuple<int, NozzleState>[4];
        cUserTimer cUserTimersnapbuzzer = new cUserTimer(2000);
        cUserTimer cUserTimersnapcap = new cUserTimer(2000);
        public int CheckBuzzerSnapState()
        {
            int s = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i - 1].nozzleState == NozzleState.HaveSnapOK1)
                    s++;

                tuple_resultsnapbuzzer[i - 1] = Tuple.Create<int, NozzleState>(i, NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i - 1].nozzleState);
            }
            return s;
        }
        public int CheckCapSnapState()
        {
            int s = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i - 1].nozzleState2 == NozzleState.HaveSnapOK2)
                    s++;

                tuple_resultsnapcap[i - 1] = Tuple.Create<int, NozzleState>(i, NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i - 1].nozzleState2);
            }
            return s;
        }
        public void GoDiscradProductPos(int index, bool bmanual = false)
        {
            string strmsg = string.Format("{0}吸嘴弃料", index);
            WaranResult waranResult;
            retry_up_z_cylider:
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸1电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸2电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸3电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("左蜂鸣器Z轴气缸4电磁阀", false);
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸1原位", true, " 左贴装站：左蜂鸣器Z轴气缸1原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸2原位", true, "左贴装站：左蜂鸣器Z轴气缸2原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站：左蜂鸣器Z轴气缸3原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            waranResult = CheckIobyName("左蜂鸣器Z轴气缸3原位", true, "左贴装站：左蜂鸣器Z轴气缸3原位 到位失败，请检查感应器及气缸", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider;
            string strposname = string.Format("抛料位{0}", index + 1);
            double x = GetStationPointDic()[strposname].pointX;
            double y = GetStationPointDic()[strposname].pointY;
            double z = GetStationPointDic()[strposname].pointZ;

            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);
            double SafeZ = GetStationPointDic()["安全左上"].pointZ;
            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 5, bmanual, this, 30000);

            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 5, bmanual, this, 60000);
            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 5, bmanual, this, 30000);
            //  MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 10,
            //      bmanual, this, 60000);
            string strCylider = string.Format("左蜂鸣器Z轴气缸{0}电磁阀", index + 1);
            string strCyliderUp = string.Format("左蜂鸣器Z轴气缸{0}原位", index + 1);
            string strCyliderDown = string.Format("左蜂鸣器Z轴气缸{0}到位", index + 1);
            string strvac = string.Format("左蜂鸣器真空吸{0}电磁阀", index + 1);
            string strvacbreak = string.Format("左蜂鸣器破真空{0}电磁阀", index + 1);
            string strvaccheck = string.Format("左蜂鸣器真空检测{0}", index + 1);
            IOMgr.GetInstace().WriteIoBit(strCylider, true);
            retry_down_z_cylider:
            waranResult = CheckIobyName(strCyliderDown, true, string.Format(" 左贴装站：{0} 到位失败，请检查感应器及气缸", strCyliderDown), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_down_z_cylider;
            IOMgr.GetInstace().WriteIoBit(strvacbreak, false);

            //retry_check_vac1:
            //IOMgr.GetInstace().WriteIoBit(strvac, true);
            //waranResult = CheckIobyName(strvaccheck, true, string.Format("左贴装站：{0} 到位失败 可能掉落，请检查感应器及吸嘴", strvaccheck), bmanual);
            //if (waranResult == WaranResult.Retry)
            //    goto retry_check_vac1;
            IOMgr.GetInstace().WriteIoBit(strvac, false);
            Thread.Sleep(5);
            IOMgr.GetInstace().WriteIoBit(strvacbreak, true);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("破真空延时"));

            retry_check_vac2:
            IOMgr.GetInstace().WriteIoBit(strvac, true);
            waranResult = CheckIobyName(strvaccheck, false, string.Format("左贴装站：{0} 到位失败 可能堵住，请检查感应器及吸嘴", strvaccheck), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_check_vac2;
            IOMgr.GetInstace().WriteIoBit(strvacbreak, false);
            IOMgr.GetInstace().WriteIoBit(strvac, false);

            IOMgr.GetInstace().WriteIoBit(strCylider, false);
            retry_up_z_cylider2:
            waranResult = CheckIobyName(strCyliderUp, true, string.Format(" 左贴装站：{0} 到位失败，请检查感应器及气缸", strCyliderUp), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_up_z_cylider2;

            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + index].nozzleState = NozzleState.HaveSnapNG1;

        }
        public void GoCarrySafePos()
        {
            double x = GetStationPointDic()["搬料安全位"].pointX;
            double y = GetStationPointDic()["搬料安全位"].pointY - 500;
            double z = GetStationPointDic()["搬料安全位"].pointZ;
            Info("左贴装站：取料尚未完成 ，去搬料安全位负方向等待");


            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);

            bool bXInPos = Math.Abs(x - currentx) < 30;
            bool bYInPos = Math.Abs(y - currenty) < 30;
            bool bZInPos = Math.Abs(z - currentz) < 30;
            if (bXInPos & bYInPos & bZInPos)
                return;

            double SafeZ = GetStationPointDic()["安全左上"].pointZ;
            bool bmanual = false;
            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ && z < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 10, bmanual, this, 60000);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ && z >= SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 10, bmanual, this, 60000);
            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ },
               new double[] { x, y, z },
               new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this, 60000);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 15, bmanual, this, 60000);

            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 10, bmanual, this, 60000);
        }
        public void GoSnapBlackCapPos()
        {
            double z = GetStationPointDic()["黑帽拍照左"].pointZ;
            double x = GetStationPointDic()["黑帽拍照左"].pointX;
            double y = GetStationPointDic()["黑帽拍照左"].pointY;

            Info("左贴装站:蜂鸣器拍照完成，去拍黑帽位置等待");

            double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double currenty = MotionMgr.GetInstace().GetAxisPos(AxisY);
            double currentz = MotionMgr.GetInstace().GetAxisPos(AxisZ);

            bool bXInPos = Math.Abs(x - currentx) < 30;
            bool bYInPos = Math.Abs(y - currenty) < 30;
            bool bZInPos = Math.Abs(z - currentz) < 30;
            if (bXInPos & bYInPos & bZInPos)
                return;

            double SafeZ = GetStationPointDic()["安全左上"].pointZ;
            bool bmanual = false;
            bool bYIsSafe = Safe.SafeRegionLeft.IsSafe("Y", currentx, currenty, currentz, x, y, z);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ && z < SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, SafeZ, (double)SpeedType.High, 10, bmanual, this, 60000);
            if (!bYIsSafe && MotionMgr.GetInstace().GetAxisPos(AxisZ) < SafeZ && z >= SafeZ)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 10, bmanual, this, 60000);
            if (bYIsSafe)
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ },
               new double[] { x, y, z },
               new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 10, bmanual, this, 60000);
            else
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x, y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 15, bmanual, this, 60000);

            if (!bYIsSafe)
                MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 10, bmanual, this, 60000);
            Info("左贴装: 去到黑帽拍照位置上方");
        }
        protected override void StationWork(int step)
        {
            if (ParamSetMgr.GetInstance().GetIntParam("左贴装屏蔽") == 1)
            {
                if (SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.Have)
                    SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState = SocketState.HaveHaftOK;
                return;
            }
            //double x = GetStationPointDic()["吸料准备位置"].pointX;
            //double y = GetStationPointDic()["吸料准备位置"].pointY;
            //double z = GetStationPointDic()["吸料准备位置"].pointZ;
            WaranResult waranResult;

            NozzleState NozzleStateL11 = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState;
            NozzleState NozzleStateL12 = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState;
            NozzleState NozzleStateL13 = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState;
            NozzleState NozzleStateL14 = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState;
            NozzleState NozzleStateL21 = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2;
            NozzleState NozzleStateL22 = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2;
            NozzleState NozzleStateL23 = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2;
            NozzleState NozzleStateL24 = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2;
            bool bsperate = ParamSetMgr.GetInstance().GetBoolParam("左剥料完成");
            bool bsperate2 = ParamSetMgr.GetInstance().GetBoolParam("左边最后1个黑帽拍照完成");
            switch (step)
            {
                case (int)StationStep.step_init:
                    init();
                    PushMultStep((int)StationStep.step_jude_plane);
                    DelCurrentStep();
                    break;
                case (int)StationStep.step_jude_plane:
                    #region 平台侧判断
                    //  MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (int)SpeedType.High, (int)SpeedType.High, (int)SpeedType.High, });
                    if (ParamSetMgr.GetInstance().GetBoolParam("左剥料完成") &&
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.None &&
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.None &&
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.None &&
                        NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.None
                        )
                    {
                        LightControl.GetInstance().CloseLight("左上黑帽定位");
                        LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                        Info(" 左贴装站：剥料完成 开始取料");
                        PushMultStep((int)StationStep.step_Pick, (int)StationStep.step_jude_plane);
                        DelCurrentStep();
                        break;
                    }
                    else if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.Have &&
                            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.Have &&
                            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.Have &&
                            NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.Have
                          )
                    {
                        Info("左贴装站： 取料完成 开始拍蜂鸣器");
                        PushMultStep((int)StationStep.step_Snap_Buzzer, (int)StationStep.step_jude_plane);
                        DelCurrentStep();
                        break;
                    }
                    else if (
                          (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapNG1)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapNG1)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapNG1)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapNG1)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.HaveSnapNG2)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.HaveSnapNG2)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.HaveSnapNG2)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.HaveSnapNG2)
                    )
                    {
                        LightControl.GetInstance().CloseLight("左上黑帽定位");
                        LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                        // 蜂鸣器和黑帽 都拍照完成 开始贴装
                        Info(" 左贴装站：蜂鸣器和 黑帽成工开始 组装");
                        PushMultStep((int)StationStep.step_place, (int)StationStep.step_jude_plane);
                        DelCurrentStep();
                        break;
                    }
                    else if (
                           (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapNG1)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapNG1)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapNG1)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapNG1)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.None)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.None)
                       && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.None)
                       )
                    {
                        LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                        // 蜂鸣器拍照完成 黑帽没有拍照，开始检查socket 流水线
                        Info(" 左贴装站：蜂鸣器拍照成工开始拍 黑帽");
                        PushMultStep((int)StationStep.step_jude_line);
                        DelCurrentStep();
                        break;
                    }

                    else if (ParamSetMgr.GetInstance().GetBoolParam("左边最后1个蜂鸣器拍照完成") && CheckBuzzerSnapState() != 4 &&
                           (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnaped1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapOK1) &&
                           (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnaped1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapOK1) &&
                           (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnaped1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapOK1) &&
                           (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnaped1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapOK1)
                        )
                    {
                        Info("左贴装站：蜂鸣器有拍照失败 重新拍照");
                        //蜂鸣器第一轮拍照完成  有失败的 重新拍照
                        //再次判断  如果失败 判为NG
                        foreach (var temp in tuple_resultsnapbuzzer)
                        {
                            if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + temp.Item1 - 1].nozzleState != NozzleState.HaveSnapOK1)
                                SanpBuzzerAndProcess(temp.Item1);
                        }


                        for (int i = 0; i < 4; i++)
                        {
                            //蜂鸣器拍照失败的  直接弃料
                            if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + i].nozzleState == NozzleState.HaveSnapNG1)
                            {
                                GoDiscradProductPos(i);
                            }
                        }

                        LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                        PushMultStep((int)StationStep.step_jude_plane);
                        DelCurrentStep();
                        break;
                    }
                    else
                    {
                        PushMultStep((int)StationStep.step_jude_line);
                        DelCurrentStep();
                        break;
                    }
                #endregion
                case (int)StationStep.step_jude_line:
                    if (SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.HaveHaftOK)
                    {
                        if (
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapNG1) &&
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapNG1) &&
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapNG1) &&
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapNG1) &&
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None) &&
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.None) &&
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.None) &&
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.None)
                            )
                        {
                            Info("左贴装站： 蜂鸣器拍照完成，黑帽尚未拍照 Socket流水线完成 等待下一次，去黑帽拍照位置等待");
                            GoSnapBlackCapPos();
                        }
                        PushMultStep((int)StationStep.step_jude_plane);
                        DelCurrentStep();
                        break;
                    }
                    else if (SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.None)
                    {
                        if (
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapNG1) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapNG1) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapNG1) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapNG1) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.None)
                           )
                        {
                            Info("左贴装站： 蜂鸣器拍照完成，黑帽尚未拍照 Socket流水线尚未完成，去黑帽拍照位置等待");
                            GoSnapBlackCapPos();

                        }
                        PushMultStep((int)StationStep.step_jude_plane);
                        DelCurrentStep();
                        break;
                    }
                    else if (
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapNG1) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapNG1) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapNG1) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapNG1) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.None) &&
                        SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.Have)
                    {
                        // 蜂鸣器拍照完成 黑帽没有拍照，socket 流水线OK 黑帽开始第一轮拍照
                        LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                        Info("左贴装站：蜂鸣器拍照完成， 黑帽开始拍照");
                        PushMultStep((int)StationStep.step_Snap_cap, (int)StationStep.step_jude_line);
                        DelCurrentStep();
                        break;
                    }

                    else if (
                      NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.None &&
                      NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.None &&
                      NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.None &&
                      NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.None &&
                      (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None) &&
                      (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.None) &&
                      (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.None) &&
                      (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.Have || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.None) &&
                      SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.Have)
                    {
                        // 吸嘴无 蜂鸣器 ，黑帽没有拍照，socket 流水线OK 黑帽开始第一轮拍照
                        Info("左贴装站：蜂鸣器没有开始拍照， 黑帽开始拍照");
                        PushMultStep((int)StationStep.step_Snap_cap, (int)StationStep.step_jude_line);
                        DelCurrentStep();
                        break;
                    }
                    else if (
                         (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.HaveSnapNG1)
                      && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.HaveSnapNG1)
                      && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.HaveSnapNG1)
                      && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapOK1 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.HaveSnapNG1)
                      && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.HaveSnapNG2)
                      && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.HaveSnapNG2)
                      && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.HaveSnapNG2)
                      && (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.HaveSnapNG2)
                      && SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.Have)

                    {
                        LightControl.GetInstance().CloseLight("左上黑帽定位");
                        LightControl.GetInstance().CloseLight("左下蜂鸣器定位");
                        // 蜂鸣器和黑帽 都拍照完成 开始贴装
                        Info("左贴装站：黑帽和蜂鸣器 定位成功,去贴装");
                        PushMultStep((int)StationStep.step_place, (int)StationStep.step_jude_plane);
                        DelCurrentStep();
                        break;
                    }
                    else if (
                        ParamSetMgr.GetInstance().GetBoolParam("左边最后1个黑帽拍照完成") && CheckCapSnapState() != 4 && SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.Have &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.HaveSnaped2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.HaveSnaped2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.HaveSnaped2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None) &&
                        (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.HaveSnaped2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.None)
                     )
                    {
                        Info("左贴装站：黑帽定位有拍照失败 重新拍照");
                        foreach (var temp in tuple_resultsnapcap)
                        {
                            if (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + temp.Item1 - 1].nozzleState2 != NozzleState.HaveSnapOK2)
                            {
                                if (SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketcells[0 + temp.Item1 - 1].Cellstate == SocketCellState.CellStateOK)
                                    SanpBlackCapAndProcess(temp.Item1);
                                else//黑帽失败 直接判NG2
                                    NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1 + temp.Item1 - 1].nozzleState2 = NozzleState.HaveSnapNG2;

                            }
                        }
                        LightControl.GetInstance().CloseLight("左上黑帽定位");
                        PushMultStep((int)StationStep.step_jude_line);
                        DelCurrentStep();
                        break;
                    }
                    else if (
                             (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState2 == NozzleState.HaveSnapNG2) &&
                             (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState2 == NozzleState.HaveSnapNG2) &&
                             (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState2 == NozzleState.HaveSnapNG2) &&
                             (NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.HaveSnapOK2 || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState2 == NozzleState.HaveSnapNG2) &&
                             NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L1].nozzleState == NozzleState.None &&
                             NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L2].nozzleState == NozzleState.None &&
                             NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L3].nozzleState == NozzleState.None &&
                             NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.BuzzerNozzle_L4].nozzleState == NozzleState.None &&
                             SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.Have)
                    {
                        LightControl.GetInstance().CloseLight("左上黑帽定位");
                        // 吸嘴无 蜂鸣器 ，黑帽完成拍照，socket 流水线OK 去吸料
                        Info("左贴装站：黑帽定位有拍照成功 去定位 抓取 定位蜂鸣器");
                        if (!ParamSetMgr.GetInstance().GetBoolParam("左剥料完成"))
                        {
                            GoCarrySafePos();
                        }
                        PushMultStep((int)StationStep.step_jude_plane);
                        DelCurrentStep();
                    }
                    else
                    {
                        PushMultStep((int)StationStep.step_jude_plane);
                        DelCurrentStep();
                        break;
                    }
                    break;
                case (int)StationStep.step_Pick:
                    Info(string.Format("左贴装站：开始抓取{0}4个蜂鸣器", indexPick == 0 ? "左边" : "右边"));
                    PickFromPlane(ref indexPick);

                    DelCurrentStep();
                    break;
                case (int)StationStep.step_place:
                    Info(string.Format("左贴装站：开始贴装蜂鸣器"));
                    PlaceToSocket2();
                    DelCurrentStep();
                    break;
                case (int)StationStep.step_Snap_cap:
                    Info(string.Format("左贴装站：开始拍4个黑帽"));
                    for (int i = 1; i <= 4; i++)
                        SanpBlackCap(i, this.VisionControl);
                    //LightControl.GetInstance().CloseLight("左上黑帽定位");
                    DelCurrentStep();
                    break;
                case (int)StationStep.step_Snap_Buzzer:
                    Info(string.Format("左贴装站：开始拍4个蜂鸣器"));
                    for (int i = 1; i <= 4; i++)
                        SanpBuzzer(i, this.VisionControl2);
                   for( int indexSel = 1; indexSel <= 4; indexSel++)
                    {
                        retry_zClyder_up:
                        IOMgr.GetInstace().WriteIoBit(string.Format("左蜂鸣器Z轴气缸{0}电磁阀", indexSel), false);
                        waranResult = CheckIobyName(string.Format("左蜂鸣器Z轴气缸{0}原位", indexSel), true, string.Format("左贴装站：左蜂鸣器Z轴气缸{0}原位 到位失败，请检查感应器及气缸", indexSel), false);
                        if (waranResult == WaranResult.Retry)
                            goto retry_zClyder_up;
                    }
                    DelCurrentStep();
                    break;
            }


        }




    }
}
