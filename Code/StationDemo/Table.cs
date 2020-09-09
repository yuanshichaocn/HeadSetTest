using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using CameraLib;
//using HalconLib;
using MotionIoLib;
using System.IO;
using CommonTools;
using BaseDll;
using UserData;

namespace StationDemo
{
    public class StationTable : CommonTools.Stationbase
    {
        public StationTable(string strStationName, int[] arrAxis, string[] axisname, params string[] CameraName)
            : base(strStationName, arrAxis, axisname, CameraName)
        {

        }
        public StationTable(CommonTools.Stationbase pb) : base(pb)
        {
           

        }
        enum StationStep
        {
            StepInit = 100,
            Step_GoloaduNLoadPos,//去上下料位置
            Step_StationCheck,//工位检测
            Step_WaitLoadFinish,//等待上料完成
            Step_WaitDepenseFinish,//等待点胶完成
            Step_WaitAAFinish,//等待AA结束
            Step_WaitPickFinish,//等待 夹取
            Step_WaitAllStationFinsh,
            Step_WaitUnLoadLoadFinish,//等待上料 下料完成
            StepEnd,
        }
        protected override bool InitStation()
        {

            ParamSetMgr.GetInstance().SetIntParam("工位选择", -1);
            //ParamSetMgr.GetInstance().SetBoolParam
            PushMultStep((int)StationStep.StepInit);
            return true;
        }

        public void PickFromPlane(int indexpick, bool bmanual)
        {
            if (bmanual || PlaneMgr.GetInstance().GetPlaneState() == PlaneState.Have)
            {

                string strPickPosName = indexpick == 0 ? "A工位夹取位" : "B工位夹取位";
                double x = GetStationPointDic()[strPickPosName].pointX;
                double y = GetStationPointDic()[strPickPosName].pointX;
                double z = GetStationPointDic()[strPickPosName].pointX;
                Info("去" + strPickPosName + "位置,开始抓取物料");
                MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z },
                    new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High, }, 0.05, bmanual, this, 30000);
                MoveSigleAxisPosWaitInpos(AxisX, z + 1, (double)SpeedType.High, 0.05, bmanual, this, 30000);
                Info("去" + strPickPosName + "位置,抓取物料成功");
            }

        }
        int nTimeWaitStart = 120000;
        DoWhile doWhileCheckStartSignal = new DoWhile((time, dowhileobj, bmanual, paramobjs) =>
        {

            int nTimeWaitStartSinal = 12000;
            nTimeWaitStartSinal = ParamSetMgr.GetInstance().GetIntParam("等待启动信号");
            StationTable stationTable = null;
            if (paramobjs.Length > 0)
                stationTable = (StationTable)paramobjs[0];

            if (ParamSetMgr.GetInstance().GetBoolParam("左启动按钮")
            && ParamSetMgr.GetInstance().GetBoolParam("右启动按钮"))
            {
                return WaranResult.Run;
            }
            else if (time > nTimeWaitStartSinal)
                return AlarmMgr.GetIntance().WarnWithDlg("等待启动超时", stationTable, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, dowhileobj, bmanual);
            else return WaranResult.CheckAgain;
        }, int.MaxValue);


        DoWhile doWhileCheckPickFinish = new DoWhile((time, dowhileobj, bmanual, paramobjs) =>
       {
           StationTable stationTable = null;
           if (paramobjs.Length > 0)
               stationTable = (StationTable)paramobjs[0];
           if (ParamSetMgr.GetInstance().GetBoolParam("抓取完成"))
           {
               return WaranResult.Run;
           }
           else if (time > 120000)
               return AlarmMgr.GetIntance().WarnWithDlg(string.Format("抓取完成超时"), stationTable, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, dowhileobj, bmanual);
           else return WaranResult.CheckAgain;
       }, int.MaxValue);
        DoWhile doWhileWaitInit = new DoWhile((time, dowhileobj, bmanual, paramobjs) =>
        {
            if (ParamSetMgr.GetInstance().GetBoolParam("AA工站初始化完成")
            && ParamSetMgr.GetInstance().GetBoolParam("点胶工站初始化完成"))
            {
                return WaranResult.Run;
            }
            else if (time > 120000)
                return WaranResult.TimeOut;
            else return WaranResult.CheckAgain;
        }, int.MaxValue);

        DoWhile doWhileWaitCheckDispenseFinish = new DoWhile((time, dowhileobj, bmanual, paramobjs) =>
        {
            StationTable stationTable = null;
            if (paramobjs.Length > 0)
                stationTable = (StationTable)paramobjs[0];
            if (ParamSetMgr.GetInstance().GetBoolParam("点胶完成") && ParamSetMgr.GetInstance().GetBoolParam("AA完成"))
            {
                return WaranResult.Run;
            }
            else if (time > 120000)
                return AlarmMgr.GetIntance().WarnWithDlg(string.Format("等待工站超时"), stationTable, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, dowhileobj, bmanual);
            else return WaranResult.CheckAgain;
        }, int.MaxValue);
        int SocketNumOfUnloadLoad = 0;// 上料站对应夹具号
        int SocketNumOfAA = 0;// AA站对应夹具号
        protected override void StationWork(int step)
        {
            WaranResult waranResult;
            double A_AAPos = GetStationPointDic()["A工位AA位"].pointU;
            double B_AAPos = GetStationPointDic()["B工位AA位"].pointU;
            double A_PickPos = GetStationPointDic()["A工位夹取位"].pointU;
            double B_PickPos = GetStationPointDic()["B工位夹取位"].pointU;
            double TableRunPos = 0;

            switch (step)
            {
                case (int)StationStep.StepInit:
                    Info("转盘站开始初始化");
                    waranResult = doWhileWaitInit.doSomething(this, doWhileWaitInit, false, null);
                    if (waranResult == WaranResult.TimeOut)
                    {
                        AlarmMgr.GetIntance().WarnWithDlg("转盘站回原点前，点胶工站或者AA工站复位时间过长，请检查，程序将会停止", this, CommonDlg.DlgWaranType.WaranOK, doWhileWaitInit, false);
                        ClearAllStep();
                        GlobalVariable.g_StationState = StationState.StationStateStop;
                        return;
                    }
                    waranResult = HomeSigleAxisPosWaitInpos(AxisU, this, 2000000, false);
                    if (!SocketMgr.GetInstance().socketArr[(int)SocketType.A].bEable && !SocketMgr.GetInstance().socketArr[(int)SocketType.B].bEable)
                    {
                        AlarmMgr.GetIntance().WarnWithDlg("AB 工位都屏蔽，程序退出", this, CommonDlg.DlgWaranType.WaranOK, doWhileWaitInit, false);
                        ClearAllStep();
                        GlobalVariable.g_StationState = StationState.StationStateStop;
                        return;
                    }
                    ClearAllStep();
                    PushMultStep((int)StationStep.Step_GoloaduNLoadPos, (int)StationStep.Step_WaitLoadFinish, (int)StationStep.Step_WaitAllStationFinsh);
                    ParamSetMgr.GetInstance().SetBoolParam("转盘初始化成功", true);
                    break;
                case (int)StationStep.Step_StationCheck:
                    if (SocketMgr.GetInstance().socketArr[(int)SocketType.A].bEable && SocketMgr.GetInstance().socketArr[(int)SocketType.A].socketState == SocketState.None)
                    {
                        DelCurrentStep();
                        PushMultStep((int)StationStep.Step_GoloaduNLoadPos, (int)StationStep.Step_StationCheck);
                    }
                    else if (SocketMgr.GetInstance().socketArr[(int)SocketType.A].bEable && SocketMgr.GetInstance().socketArr[(int)SocketType.A].socketState == SocketState.Have)
                    {

                        MoveSigleAxisPosWaitInpos(AxisU, A_PickPos, (double)SpeedType.High, 0.05, false, this, 60000);
                        ParamSetMgr.GetInstance().SetBoolParam("抓取完成", false);
                        ParamSetMgr.GetInstance().SetBoolParam("抓取启动", true);
                        DelCurrentStep();
                        PushMultStep((int)StationStep.Step_WaitPickFinish, (int)StationStep.Step_StationCheck);
                    }
                    else if (SocketMgr.GetInstance().socketArr[(int)SocketType.A].bEable && SocketMgr.GetInstance().socketArr[(int)SocketType.A].socketState == SocketState.Picked)
                    {
                        MoveSigleAxisPosWaitInpos(AxisU, A_AAPos, (double)SpeedType.High, 0.05, false, this, 60000);
                        DelCurrentStep();
                        PushMultStep((int)StationStep.Step_WaitLoadFinish, (int)StationStep.Step_WaitAllStationFinsh);
                    }
                    else if (SocketMgr.GetInstance().socketArr[(int)SocketType.B].bEable && SocketMgr.GetInstance().socketArr[(int)SocketType.B].socketState == SocketState.None)
                    {
                        DelCurrentStep();
                        PushMultStep((int)StationStep.Step_GoloaduNLoadPos, (int)StationStep.Step_StationCheck);
                    }
                    else if (SocketMgr.GetInstance().socketArr[(int)SocketType.B].bEable && SocketMgr.GetInstance().socketArr[(int)SocketType.B].socketState == SocketState.Have)
                    {

                        ParamSetMgr.GetInstance().SetBoolParam("抓取完成", false);
                        ParamSetMgr.GetInstance().SetBoolParam("抓取启动", true);
                        MoveSigleAxisPosWaitInpos(AxisU, B_PickPos, (double)SpeedType.High, 0.05, false, this, 60000);
                        DelCurrentStep();
                        PushMultStep((int)StationStep.Step_WaitPickFinish, (int)StationStep.Step_StationCheck);
                    }
                    else if (SocketMgr.GetInstance().socketArr[(int)SocketType.B].bEable && SocketMgr.GetInstance().socketArr[(int)SocketType.B].socketState == SocketState.Picked)
                    {
                        MoveSigleAxisPosWaitInpos(AxisU, B_AAPos, (double)SpeedType.High, 0.05, false, this, 60000);
                        DelCurrentStep();
                        PushMultStep((int)StationStep.Step_WaitLoadFinish, (int)StationStep.Step_WaitAllStationFinsh);
                    }
                    break;
                case (int)StationStep.Step_WaitPickFinish:
                    Info("等待抓取完成");
                    waranResult = doWhileCheckPickFinish.doSomething(this, doWhileCheckPickFinish, false, new object[] { this });
                    if (waranResult == WaranResult.Retry)
                        return;
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoloaduNLoadPos:
                    if (SocketMgr.GetInstance().socketArr[(int)SocketType.A].bEable && SocketMgr.GetInstance().socketArr[(int)SocketType.A].socketState == SocketState.None)
                    {
                        MoveSigleAxisPosWaitInpos(AxisU, B_AAPos, (double)SpeedType.High, 0.05, false, this, 60000);
                    }
                    else if (SocketMgr.GetInstance().socketArr[(int)SocketType.B].bEable && SocketMgr.GetInstance().socketArr[(int)SocketType.B].socketState == SocketState.None)
                    {
                        MoveSigleAxisPosWaitInpos(AxisU, A_AAPos, (double)SpeedType.High, 0.05, false, this, 60000);
                    }
                    DelCurrentStep();
                    ParamSetMgr.GetInstance().SetBoolParam("可以上下料", true);
                    break;
                case (int)StationStep.Step_WaitLoadFinish:
                    Info("开始上下料，安全光栅开始屏蔽");

                    waranResult = doWhileCheckStartSignal.doSomething(this, doWhileCheckStartSignal, false, new object[] { this });
                    if (waranResult == WaranResult.Retry)
                        return;
                    Info("开始上下料，安全光栅开始启用");
                    SocketNumOfUnloadLoad = TableData.GetInstance().GetSocketNum(1, 0.5);
                    SocketMgr.GetInstance().socketArr[SocketNumOfUnloadLoad - 1].socketState = SocketState.Have;
                    SocketMgr.GetInstance().socketArr[SocketNumOfUnloadLoad].socketState = SocketState.None;
                    ParamSetMgr.GetInstance().SetBoolParam("可以上下料", false);
                    DelCurrentStep();
                    ParamSetMgr.GetInstance().SetBoolParam("点胶完成", false);
                    ParamSetMgr.GetInstance().SetBoolParam("AA完成", false);
                    ParamSetMgr.GetInstance().SetBoolParam("启动点胶", true);
                    ParamSetMgr.GetInstance().SetBoolParam("启动AA", true);


                    break;
                case (int)StationStep.Step_WaitAllStationFinsh:
                    waranResult = doWhileWaitCheckDispenseFinish.doSomething(this, doWhileWaitCheckDispenseFinish, false, new object[] { this });
                    if (waranResult == WaranResult.Retry)
                        return;
                    ParamSetMgr.GetInstance().SetBoolParam("启动点胶", false);
                    ParamSetMgr.GetInstance().SetBoolParam("启动AA", false);
                  
                    DelCurrentStep();
                    PushMultStep((int)StationStep.Step_StationCheck);
                    break;

            }

        }


    }
}