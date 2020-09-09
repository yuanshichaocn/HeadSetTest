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
using XYZDispensVision;
using BaseDll;
using HalconDotNet;
using VisionProcess;
using UserCtrl;
using UserData;
using OtherDevice;
using System.Threading.Tasks;

namespace StationDemo
{
    public class StationDisp : CommonTools.Stationbase
    {
        public StationDisp(string strStationName, int[] arrAxis, string[] axisname, params string[] CameraName)
            : base(strStationName, arrAxis, axisname, CameraName)
        {

        }
        public delegate void FlushBarcodeHander(int index, string strcode);
        public event FlushBarcodeHander eventFlushBarcode = null;
        public delegate void DelegateShowCT();
        public static event DelegateShowCT delegateShowCT;
        public StationDisp(CommonTools.Stationbase pb) : base(pb)
        {
            m_listIoInput.Add("胶桶液位检测");
            m_listIoInput.Add("点胶针头定位检测");
            m_listIoInput.Add("1#Len气缸上升感应");
            m_listIoInput.Add("1#Len气缸下降感应");
            m_listIoInput.Add("1#治具合盖气缸闭合感应");
            m_listIoInput.Add("1#治具合盖气缸打开感应");
            m_listIoInput.Add("1#治具探针气缸上升感应");
            m_listIoInput.Add("1#治具探针气缸下降感应");

            m_listIoInput.Add("2#Len气缸上升感应");
            m_listIoInput.Add("2#Len气缸下降感应");
            m_listIoInput.Add("2#治具合盖气缸闭合感应");
            m_listIoInput.Add("2#治具合盖气缸打开感应");
            m_listIoInput.Add("2#治具探针气缸上升感应");
            m_listIoInput.Add("2#治具探针气缸下降感应");

            m_listIoInput.Add("1#LENS真空检测");
            m_listIoInput.Add("2#LENS真空检测");

            m_listIoOutput.Add("点胶");
            m_listIoOutput.Add("UV固化");
            m_listIoOutput.Add("点胶光源点亮");

            m_listIoOutput.Add("1#LENS气缸电磁阀");
            m_listIoOutput.Add("1#治具合盖气缸电磁阀");
            m_listIoOutput.Add("1#治具探针气缸电磁阀");
            m_listIoOutput.Add("1#LENS真空吸");
            m_listIoOutput.Add("1#LENS破真空");

            m_listIoOutput.Add("2#LENS气缸电磁阀");
            m_listIoOutput.Add("2#治具合盖气缸电磁阀");
            m_listIoOutput.Add("2#治具探针气缸电磁阀");
            m_listIoOutput.Add("2#LENS真空吸");
            m_listIoOutput.Add("2#LENS破真空");
            dispCalibParam = UserConfig.dispCalibParam;
            dispConfig = UserConfig.dispConfig;
            dispCalibParam = UserConfig.dispCalibParam;
            dispConfig = UserConfig.dispConfig;
            shapeDst = UserConfig.shapeDst;
            XYUR_Pin = UserConfig.XYUR_Pin;


        }
        enum StationStep
        {
            StepInit = 100,
            StepCheckIpos,
            StepGoSnap,


            StepEnd,
        }
        DoWhile dowhileCheckDispFinish = new DoWhile((time, dowhile2, bmauanl, objs) =>
        {
            double[] cmpposs = null;
            int[] axisnos = null;
            bool bSucess = true;
            GpState gps = MotionMgr.GetInstace().GetGpState("点胶群组");
            if (time > 20000)
            {
                return WaranResult.TimeOut;
            }
            if (gps == GpState.GpReady)
            {
                if (objs != null && objs.Length > 1)
                {
                    axisnos = (int[])objs[0];
                    cmpposs = (double[])objs[1];
                    bSucess = true;
                    for (int i = 0; i < axisnos.Length; i++)
                    {
                        bSucess = bSucess & (Math.Abs(MotionMgr.GetInstace().GetAxisPos(axisnos[i]) - cmpposs[i]) < 0.05);
                    }
                    if (bSucess)
                        return WaranResult.Run;
                    else
                        return WaranResult.CheckAgain;
                }
                else
                    return WaranResult.Run;


            }
            else
            {
                return WaranResult.CheckAgain;
            }
        }, 30000000);

        int nTimeWaitStart = 120000;
        DoWhile doWhileCheckStartSignal = new DoWhile((time, dowhileobj, bmanual, paramobjs) =>
        {
            int nTimeWaitStartSinal = 60000;
            nTimeWaitStartSinal = ParamSetMgr.GetInstance().GetIntParam("等待启动信号");
            StationDisp stationDisp = null;
            if (paramobjs.Length > 0)
                stationDisp = (StationDisp)paramobjs[0];
            if (ParamSetMgr.GetInstance().GetBoolParam("启动清料"))
            {
                Thread.Sleep(2000);
                return WaranResult.Run;
            }
            if (IOMgr.GetInstace().ReadIoInBit("左启动按钮")
            && IOMgr.GetInstace().ReadIoInBit("右启动按钮"))
            {
                if (!ProductInfo.StarCT)
                {
                    ProductInfo.starTime = DateTime.Now;
                    ProductInfo.StarCT = true;
                }
                else
                {
                    ProductInfo.EndCT = false;
                }
                ProductInfo.ProductCount++;
                ProductInfo.ProductCompete = ProductInfo.ProductCount - 2;
                return WaranResult.Run;
            }
            else if (time > nTimeWaitStartSinal)
                return AlarmMgr.GetIntance().WarnWithDlg("等待启动超时", stationDisp, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, dowhileobj, bmanual);
            else return WaranResult.CheckAgain;
        }, int.MaxValue);
        public void CloseSocketFailDeal(int index, bool bmanual)
        {
            WaranResult waranResult;
            string strPinIoControlName = string.Format("{0}#治具探针气缸电磁阀", index);
            string strOpenSocketIoControlName = string.Format("{0}#治具合盖气缸电磁阀", index);
            string strLenUpDownIoControlName = string.Format("{0}#LENS气缸电磁阀", index);

            string strLenDownInpos = string.Format("{0}#LENS气缸下降感应", index);
            string strSocketOpenInpos = string.Format("{0}#治具合盖气缸打开感应", index);
            string strPinDownIoControlName = string.Format("{0}#治具探针气缸下降感应", index);
        retry_CloseSockeFaildeal:

            IOMgr.GetInstace().WriteIoBit(strPinIoControlName, false);
            IOMgr.GetInstace().WriteIoBit(strLenUpDownIoControlName, false);
            IOMgr.GetInstace().WriteIoBit(strOpenSocketIoControlName, false);
            waranResult = CheckIobyName(strLenDownInpos, true, string.Format("手动放料 失败 时 ,{0}#Len下降到位失败 ", index), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_CloseSockeFaildeal;
            waranResult = CheckIobyName(strSocketOpenInpos, true, string.Format("手动放料 失败 时,{0}#治具合盖气缸打开失败 ", index), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_CloseSockeFaildeal;
            waranResult = CheckIobyName(strPinDownIoControlName, true, string.Format("手动放料 失败 时,{0}#治具探针下降失败 ", index), bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_CloseSockeFaildeal;

        }
        public void CloseSocket(int index, bool bmanual)
        {
            string strOpenSocketIoControlName = string.Format("{0}#治具合盖气缸电磁阀", index);
            string strLenUpDownIoControlName = string.Format("{0}#LENS气缸电磁阀", index);
            string strPinDownIoControlName = string.Format("{0}#治具探针气缸上升感应", index);
            string strSocketCloseInpos = string.Format("{0}#治具合盖气缸闭合感应", index);
            string strLenDownInpos = string.Format("{0}#LENS气缸上升感应", index);
            IOMgr.GetInstace().WriteIoBit(strOpenSocketIoControlName, true);
            IOMgr.GetInstace().WriteIoBit(strLenUpDownIoControlName, true);
        }
        public void PlaceToSocket(int index, bool bmanual = false)
        {
            WaranResult waranResult;
            //   string strPinIoControlName = string.Format("{0}#治具探针气缸电磁阀", index);
            string strCloseSocketIoControlName = string.Format("{0}#治具合盖气缸电磁阀", index);
            string strLenUpDownIoControlName = string.Format("{0}#LENS气缸电磁阀", index);
            string strLenVacIoControlName = string.Format("{0}#LENS真空吸", index);
        // string strLenBreakVacIoControlName = string.Format("{0}#LENS破真空", index);

        retry_place:
            IOMgr.GetInstace().WriteIoBit(strLenVacIoControlName, true);
            // IOMgr.GetInstace().WriteIoBit(strLenBreakVacIoControlName, false);
            IOMgr.GetInstace().WriteIoBit(strLenUpDownIoControlName, false);
            //IOMgr.GetInstace().WriteIoBit(strCloseSocketIoControlName, true);
            //Thread.Sleep(200);
            string strLenDownInpos = string.Format("{0}#LENS气缸下降感应", index);
            string strSocketCloseInpos = string.Format("{0}#治具合盖气缸闭合感应", index);
            //  string strPinUpIoControlName = string.Format("{0}#治具探针气缸上升感应", index);


            waranResult = CheckIobyName(strLenDownInpos, true, string.Format("手动放料时,{0}#Lens气缸下降失败 ", index), bmanual, 3000, new Action(() => { CloseSocketFailDeal(index, bmanual); }));
            if (waranResult == WaranResult.Retry)
                goto retry_place;
            CloseSocket(index, bmanual);


            waranResult = CheckIobyName(strSocketCloseInpos, true, string.Format("手动放料时,{0}#治具合盖气缸打开失败 ", index), bmanual, 3000, new Action(() => { CloseSocketFailDeal(index, bmanual); }));
            if (waranResult == WaranResult.Retry)
                goto retry_place;
            CloseSocket(index, bmanual);

            // IOMgr.GetInstace().WriteIoBit(strPinIoControlName, true);
            //waranResult = CheckIobyName(strPinUpIoControlName, true, string.Format("手动放料时,{0}#治具探针气缸上升失败 ", index), bmanual, 3000, new Action(() => { CloseSocketFailDeal(index, bmanual); }));
            // if (waranResult == WaranResult.Retry)
            //   goto retry_place;
            //IOMgr.GetInstace().WriteIoBit(strPinIoControlName, true);
            CloseSocket(index, bmanual);
        retry_vacagain:
            IOMgr.GetInstace().WriteIoBit(strLenVacIoControlName, true);
            string strVacCheck = string.Format("{0}#LENS真空检测", index);
            string ab = index == 1 ? "A" : "B";
            bool lensAirEnable = ParamSetMgr.GetInstance().GetBoolParam($"是否启用{ab}工位吸气检测");
            if (lensAirEnable)
            {
                waranResult = CheckIobyName(strVacCheck, true, string.Format("手动放料时,{0}#LENS真空检测失败 ", index), bmanual, 3000, null);
            }
            if (waranResult == WaranResult.Retry)
                goto retry_vacagain;
        }

        public void PickFromSockt(int index, bool bmanual = false)
        {
            WaranResult waranResult;
            string strPinIoControlName = string.Format("{0}#治具探针气缸电磁阀", index);
            string strOpenSocketIoControlName = string.Format("{0}#治具合盖气缸电磁阀", index);
            string strLenUpDownIoControlName = string.Format("{0}#LENS气缸电磁阀", index);
            string strLenVacIoControlName = string.Format("{0}#LENS真空吸", index);
            //  string strLenBreakVacIoControlName = string.Format("{0}#LENS破真空", index);

            string strLenDownInpos = string.Format("{0}#LENS气缸下降感应", index);
            string strSocketOpenInpos = string.Format("{0}#治具合盖气缸打开感应", index);
            string strPinDownIoControlName = string.Format("{0}#治具探针气缸下降感应", index);

        retry_open:

            // IOMgr.GetInstace().WriteIoBit(strLenBreakVacIoControlName, false);
            IOMgr.GetInstace().WriteIoBit(strLenUpDownIoControlName, false);
            IOMgr.GetInstace().WriteIoBit(strPinIoControlName, false);
            IOMgr.GetInstace().WriteIoBit(strOpenSocketIoControlName, false);
            IOMgr.GetInstace().WriteIoBit(strLenVacIoControlName, false);
            waranResult = CheckIobyName(strLenDownInpos, true, string.Format("手动取料时,{0}#Len下降到位失败 ", index), bmanual);
            if (waranResult == WaranResult.Retry) goto retry_open;
            waranResult = CheckIobyName(strSocketOpenInpos, true, string.Format("手动取料时,{0}#治具合盖气缸打开失败 ", index), bmanual);
            if (waranResult == WaranResult.Retry) goto retry_open;
            waranResult = CheckIobyName(strPinDownIoControlName, true, string.Format("手动取料时,{0}#治具探针气缸下降失败 ", index), bmanual);
            if (waranResult == WaranResult.Retry) goto retry_open;
        }
        string strCode2d;

        public void GoSanpDisp(VisionControl visionControl, bool bmanual = false)
        {
            WaranResult waranResult;
            Info("去拍目标点，点胶准备开会");
            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", true);
            double xphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.x;
            double yphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.y;
            double zphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.z;
            bool DispEnale = ParamSetMgr.GetInstance().GetBoolParam("自动运行是否出胶");
            string pathImage = ParamSetMgr.GetInstance().GetStringParam("保存相机图片路径");
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xphoto, yphoto, zphoto },
                new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);


            HObject img = null;
        retry_pr:
            CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
            cam.BindWindow(visionControl);

            Thread.Sleep(100);
            cam.StopGrap();
            cam.SetTriggerMode(CameraModeType.Software);
            cam.SetExposureTime(dispCalibParam.dDstExposure);
            cam.SetGain(dispCalibParam.dDstGain);
            cam.StartGrab();
            img = cam.GetImage();
            if (img == null || !img.IsInitialized())
            {
                img = cam.GetImage();
            }
            else
            {
                string path = pathImage + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\Up\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Task.Run(() => HOperatorSet.WriteImage(img.Clone(), "bmp", 0, $"{path}{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp"));
            }
            shapeDst.ClearResult();
            shapeDst.Process_image(img, visionControl);
            VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
            DoWhile dowhile = new DoWhile((time, dowhile1, bmanual2, objs) =>
            {
                visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                if (time > 1000)
                {
                    return AlarmMgr.GetIntance().WarnWithDlg("图像处理超时", this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, dowhile1, bmanual);

                }
                if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                    return WaranResult.Run;
                else
                    return WaranResult.CheckAgain;
            }, 3000);
            waranResult = dowhile.doSomething(this, dowhile, bmanual, null);
            if (waranResult == WaranResult.Retry)
            {
                goto retry_pr;
            }
            if (visionShapMatchobj == null || visionShapMatchobj.ResultCol.Count == 0)
            {
                return;
            }
            double x = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double y = MotionMgr.GetInstace().GetAxisPos(AxisY);
            XYUPoint SnapPoint = new XYUPoint(x, y, 0);
            XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], visionShapMatchobj.ResultAngle[0]);
            XYUPoint oldLaserPoint;
            XYUPoint nowLaserPoint;
            XYUPoint LaserGoDstPos;
            double DispenseHeight = 0;
            int nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (0 + 1).ToString());
            double pinOffSetX = dispCalibParam.dPinOffsetX;
            double pinOffSetY = dispCalibParam.dPinOffsetY;
            bool brtnExc = true;
            MotionMgr.GetInstace().ClearBufMove("点胶群组");
            MotionMgr.GetInstace().RestGpErr("点胶群组");
            GpState gps = MotionMgr.GetInstace().GetGpState("点胶群组");
            XYUPoint PinGoDstPosFistPoint = new XYUPoint();
            XYUPoint PinGoDstPosSecondPoint = new XYUPoint();

            for (int i = 0; i < 6; i++)
            {
                int count = i;
                if (i >= 4)
                {
                    count = count - 4;
                }
                nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (count + 1).ToString());
                if (nIndex == -1)
                    return;
                oldLaserPoint = new XYUPoint(dispCalibParam.pointDispenses[nIndex].MachinePoint.x, dispCalibParam.pointDispenses[nIndex].MachinePoint.y, 0);
                nowLaserPoint = shapeDst.GetAffineTransPointAffterMatch(oldLaserPoint.x, oldLaserPoint.y, visionPoint, visionControl);
                XYUPoint PinGoDstPos;
                PinGoDstPos = XYUR_Pin.GetDstPonit(nowLaserPoint, SnapPoint);

                if (i == 0)
                {

                    PinGoDstPosFistPoint = PinGoDstPos;
                    brtnExc &= MotionMgr.GetInstace().AddBufMove("点胶群组", BufMotionType.buf_Line2dAbs, 0, 2,
         dispCalibParam.pointDispenses[nIndex].vel, dispCalibParam.pointDispenses[nIndex].vellow, new double[] { PinGoDstPos.x + pinOffSetX, PinGoDstPos.y + pinOffSetY }, null);
                    Info($"第{i}个位置PinGoDstPos.x:{PinGoDstPos.x + dispCalibParam.dPinOffsetX};PinGoDstPos.y:{PinGoDstPos.y + dispCalibParam.dPinOffsetY};");
                }
                else if (i == 1)
                {
                    PinGoDstPosSecondPoint = PinGoDstPos;
                    brtnExc &= MotionMgr.GetInstace().AddBufMove("点胶群组", BufMotionType.buf_Line2dAbs, 0, 2,
         dispCalibParam.pointDispenses[nIndex].vel, dispCalibParam.pointDispenses[nIndex].vellow, new double[] { PinGoDstPos.x + pinOffSetX, PinGoDstPos.y + pinOffSetY }, null);
                    Info($"第{i}个位置PinGoDstPos.x:{PinGoDstPos.x + dispCalibParam.dPinOffsetX};PinGoDstPos.y:{PinGoDstPos.y + dispCalibParam.dPinOffsetY};");
                }
                else if (i == 2)
                {
                    MotionMgr.GetInstace().AddBufIo("点胶群组", "点胶", DispEnale);

                    if (dispCalibParam.dDelayBeforeDispense > 0)
                        MotionMgr.GetInstace().AddBufDelay("点胶群组", (int)dispCalibParam.dDelayBeforeDispense);
                    brtnExc &= MotionMgr.GetInstace().AddBufMove("点胶群组", BufMotionType.buf_Line2dAbs, 0, 2,
            dispCalibParam.pointDispenses[nIndex].vel, dispCalibParam.pointDispenses[nIndex].vellow, new double[] { PinGoDstPos.x + pinOffSetX, PinGoDstPos.y + pinOffSetY }, null);
                    Info($"第{i}个位置PinGoDstPos.x:{PinGoDstPos.x + dispCalibParam.dPinOffsetX};PinGoDstPos.y:{PinGoDstPos.y + dispCalibParam.dPinOffsetY};");
                }
                else
                {
                    brtnExc &= MotionMgr.GetInstace().AddBufMove("点胶群组", BufMotionType.buf_Line2dAbs, 0, 2,
                    dispCalibParam.pointDispenses[nIndex].vel, dispCalibParam.pointDispenses[nIndex].vellow, new double[] { PinGoDstPos.x + pinOffSetX, PinGoDstPos.y + pinOffSetY }, null);
                    Info($"第{i}个位置PinGoDstPos.x:{PinGoDstPos.x + dispCalibParam.dPinOffsetX};PinGoDstPos.y:{PinGoDstPos.y + dispCalibParam.dPinOffsetY};");
                }
            }
            brtnExc &= MotionMgr.GetInstace().AddBufIo("点胶群组", "点胶", false);
            brtnExc &= MotionMgr.GetInstace().AddBufEnd("点胶群组");

            double z = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.z;
            MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 0.02, bmanual, this, 60000);
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { PinGoDstPosFistPoint.x + pinOffSetX, PinGoDstPosFistPoint.y + pinOffSetY }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this, 60000);

            nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (0 + 1).ToString());
            z = dispCalibParam.pointDispenses[nIndex].MachinePoint.z;
            MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 0.02, bmanual, this, 60000);

            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", false);
            dowhileCheckDispFinish.doSomething(this, dowhileCheckDispFinish, bmanual, null);
            MotionMgr.GetInstace().RestGpErr("点胶群组");
            brtnExc &= MotionMgr.GetInstace().BufTrans("点胶群组");
            MotionMgr.GetInstace().SetBufMoveParam("点胶群组", dispCalibParam.dDstVel, dispCalibParam.dDstVel, dispCalibParam.dDstAcc, dispCalibParam.dDstDec);
            MotionMgr.GetInstace().BufStart("点胶群组");
            Thread.Sleep(900);
            dowhileCheckDispFinish.doSomething(this, dowhileCheckDispFinish, bmanual, new int[] { AxisX, AxisY }, new double[] { PinGoDstPosSecondPoint.x + pinOffSetX, PinGoDstPosSecondPoint.y + pinOffSetY });
            if (dispCalibParam.dUpDistance > 0)
                MoveSigleAxisPosWaitInpos(AxisZ, (double)dispCalibParam.dUpDistance, (double)SpeedType.High, 0.02, false, this);
            Thread.Sleep(100);
            Info("点胶完成，回吐胶点");
            nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (0 + 1).ToString());
            z = dispCalibParam.pointDispenses[nIndex].MachinePoint.z + 5;
            MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 0.02, bmanual, this, 60000);
            x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.x;
            y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.y;
            z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.z;
            //涂完胶水后 回到拍照位置拍照，记录
            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", true);
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xphoto, yphoto, zphoto },
    new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);

            img = cam.GetImage();
            if (img == null || !img.IsInitialized())
            {
                img = cam.GetImage();
            }
            else
            {
                string path = pathImage + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\Up\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Task.Run(() => HOperatorSet.WriteImage(img.Clone(), "bmp", 0, $"{path}{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp"));
            }

            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.01, bmanual, this, 600000);
        }
        public void GoSanpDispNew(VisionControl visionControl, bool bmanual = false)
        {
            WaranResult waranResult;
            Info("去拍目标点，点胶准备开会");
            string pathImage = ParamSetMgr.GetInstance().GetStringParam("保存相机图片路径");
            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", true);
            double xphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.x;
            double yphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.y;
            double zphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.z;
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xphoto, yphoto, zphoto },
                new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);


            HObject img = null;
        retry_pr:
            CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
            cam.BindWindow(visionControl);

            Thread.Sleep(100);
            cam.StopGrap();
            cam.SetTriggerMode(CameraModeType.Software);
            cam.SetExposureTime(dispCalibParam.dDstExposure);
            cam.SetGain(dispCalibParam.dDstGain);
            cam.StartGrab();
            img = cam.GetImage();
            if (img == null || !img.IsInitialized())
            {
                img = cam.GetImage();
            }
            else
            {
                string path = pathImage + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\Up\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Task.Run(() => HOperatorSet.WriteImage(img.Clone(), "bmp", 0, $"{path}{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp"));
            }
            shapeDst.ClearResult();
            shapeDst.Process_image(img, visionControl);
            VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
            DoWhile dowhile = new DoWhile((time, dowhile1, bmanual2, objs) =>
            {
                visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                if (time > 1000)
                {
                    return AlarmMgr.GetIntance().WarnWithDlg("图像处理超时", this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, dowhile1, bmanual);

                }
                if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                    return WaranResult.Run;
                else
                    return WaranResult.CheckAgain;
            }, 3000);
            waranResult = dowhile.doSomething(this, dowhile, bmanual, null);
            if (waranResult == WaranResult.Retry)
            {
                goto retry_pr;
            }
            if (visionShapMatchobj == null || visionShapMatchobj.ResultCol.Count == 0)
            {
                return;
            }
            double x = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double y = MotionMgr.GetInstace().GetAxisPos(AxisY);
            XYUPoint SnapPoint = new XYUPoint(x, y, 0);
            XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], visionShapMatchobj.ResultAngle[0]);
            XYUPoint oldLaserPoint;
            XYUPoint nowLaserPoint;
            XYUPoint LaserGoDstPos;
            double DispenseHeight = 0;
            int nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (0 + 1).ToString());
            double pinOffSetX = dispCalibParam.dPinOffsetX;
            double pinOffSetY = dispCalibParam.dPinOffsetY;
            bool brtnExc = true;
            MotionMgr.GetInstace().ClearBufMove("点胶群组");
            MotionMgr.GetInstace().RestGpErr("点胶群组");
            GpState gps = MotionMgr.GetInstace().GetGpState("点胶群组");
            XYUPoint PinGoDstPosFistPoint = new XYUPoint();
            XYUPoint PinGoDstPosFinallyPoint = new XYUPoint();

            //坐标点个数 
            //第一个点是否用来加速，如果是 那么第二个点开启点胶，最后回到第一个点开始收胶，如果不是那么第一个点开启点胶，最后回到第最后一个点开始收胶

            int nEnableCount = ParamSetMgr.GetInstance().GetIntParam("使用点胶点个数");
            bool FirstUseToUpDcc = ParamSetMgr.GetInstance().GetBoolParam("是否第一个点开始点胶");
            bool DispEnale = ParamSetMgr.GetInstance().GetBoolParam("自动运行是否出胶");
            double dDelayBeforeDispense = ParamSetMgr.GetInstance().GetDoubleParam("出胶开启前延迟") * 1000;
            double dDelayAfterDispense = ParamSetMgr.GetInstance().GetDoubleParam("出胶关闭后延迟") * 1000;
            double closeDipZ = ParamSetMgr.GetInstance().GetDoubleParam("拉胶距离Z轴");
            for (int i = 0; i < nEnableCount + 2; i++)
            {
                int count = i;
                if (i > nEnableCount - 1)
                {
                    count = count - nEnableCount;
                }
                nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (count + 1).ToString());
                if (nIndex == -1)
                    return;
                oldLaserPoint = new XYUPoint(dispCalibParam.pointDispenses[nIndex].MachinePoint.x, dispCalibParam.pointDispenses[nIndex].MachinePoint.y, 0);
                nowLaserPoint = shapeDst.GetAffineTransPointAffterMatch(oldLaserPoint.x, oldLaserPoint.y, visionPoint, visionControl);
                XYUPoint PinGoDstPos;
                PinGoDstPos = XYUR_Pin.GetDstPonit(nowLaserPoint, SnapPoint);
                if (i == 0)
                {
                    PinGoDstPosFistPoint = PinGoDstPos;//设置第一个点
                }
                if (FirstUseToUpDcc)
                {
                    if (i == 1)
                    {

                        PinGoDstPosFinallyPoint = PinGoDstPos;//第一个点用来加速 那么第二个点就是最后回来的位置
                    }
                    if (i == 2)
                    {

                        MotionMgr.GetInstace().AddBufIo("点胶群组", "点胶", DispEnale);//第一个点用来加速 那么第三个点之前增加点胶启动
                        if (dDelayBeforeDispense > 0)
                            MotionMgr.GetInstace().AddBufDelay("点胶群组", (int)dDelayBeforeDispense);
                        Info($"第{i}个位置增加点胶开启");
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        PinGoDstPosFinallyPoint = PinGoDstPos;
                        if (dDelayBeforeDispense > 0)
                            MotionMgr.GetInstace().AddBufDelay("点胶群组", (int)dDelayBeforeDispense);
                        MotionMgr.GetInstace().AddBufIo("点胶群组", "点胶", DispEnale);//第一个点不用来加速 那么第一个点之前增加点胶启动
                        Info($"第{i}个位置增加点胶开启");
                    }
                    if (i > nEnableCount - 1)
                    {
                        break;
                    }

                }


                brtnExc &= MotionMgr.GetInstace().AddBufMove("点胶群组", BufMotionType.buf_Line2dAbs, 0, 2,
         dispCalibParam.pointDispenses[nIndex].vel, dispCalibParam.pointDispenses[nIndex].vellow, new double[] { PinGoDstPos.x + pinOffSetX, PinGoDstPos.y + pinOffSetY }, null);
                Info($"第{i}个位置PinGoDstPos.x:{PinGoDstPos.x + dispCalibParam.dPinOffsetX};PinGoDstPos.y:{PinGoDstPos.y + dispCalibParam.dPinOffsetY};");
            }
            if (dDelayAfterDispense < 0)
            {
                MotionMgr.GetInstace().AddBufDelay("点胶群组", (int)Math.Abs(dDelayAfterDispense));
            }
            brtnExc &= MotionMgr.GetInstace().AddBufIo("点胶群组", "点胶", false);
            if (dDelayAfterDispense > 0)
            {
                MotionMgr.GetInstace().AddBufDelay("点胶群组", (int)dDelayAfterDispense);
            }

            brtnExc &= MotionMgr.GetInstace().AddBufEnd("点胶群组");

            double z = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.z;
            MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 0.02, bmanual, this, 60000);
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { PinGoDstPosFistPoint.x + pinOffSetX, PinGoDstPosFistPoint.y + pinOffSetY }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this, 60000);

            nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (0 + 1).ToString());
            z = dispCalibParam.pointDispenses[nIndex].MachinePoint.z;
            MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 0.02, bmanual, this, 60000);

            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", false);
            dowhileCheckDispFinish.doSomething(this, dowhileCheckDispFinish, bmanual, null);
            MotionMgr.GetInstace().RestGpErr("点胶群组");
            brtnExc &= MotionMgr.GetInstace().BufTrans("点胶群组");
            MotionMgr.GetInstace().SetBufMoveParam("点胶群组", dispCalibParam.dDstVel, dispCalibParam.dDstVel, dispCalibParam.dDstAcc, dispCalibParam.dDstDec);
            MotionMgr.GetInstace().BufStart("点胶群组");
            Thread.Sleep(2000);
            dowhileCheckDispFinish.doSomething(this, dowhileCheckDispFinish, bmanual, new int[] { AxisX, AxisY }, new double[] { PinGoDstPosFinallyPoint.x + pinOffSetX, PinGoDstPosFinallyPoint.y + pinOffSetY });
            //if (closeDipZ > 0)
            //    MoveSigleAxisPosWaitInpos(AxisZ, z+closeDipZ, (double)SpeedType.High, 0.02, false, this);
            //if (dispCalibParam.dUpDistance > 0)
            //    MoveSigleAxisPosWaitInpos(AxisZ, (double)dispCalibParam.dUpDistance, (double)SpeedType.High, 0.02, false, this);
            Thread.Sleep(100);
            Info("点胶完成，回吐胶点");
            nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (0 + 1).ToString());
            z = dispCalibParam.pointDispenses[nIndex].MachinePoint.z + 20;
            MoveSigleAxisPosWaitInpos(AxisZ, z, (double)SpeedType.High, 0.02, bmanual, this, 60000);
            x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.x;
            y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.y;
            z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.z;
            //涂完胶水后 回到拍照位置拍照，记录
            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", true);
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xphoto, yphoto, zphoto },
    new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);

            img = cam.GetImage();
            if (img == null || !img.IsInitialized())
            {
                img = cam.GetImage();
            }
            else
            {
                string path = pathImage + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\Up\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Task.Run(() => HOperatorSet.WriteImage(img.Clone(), "bmp", 0, $"{path}{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp"));
            }

            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.01, bmanual, this, 600000);
        }

        public bool GoSanpDispNew2(VisionControl visionControl, bool bmanual = false)
        {
            WaranResult waranResult;
            Info("去拍目标点，点胶准备开会");
            string pathImage = ParamSetMgr.GetInstance().GetStringParam("保存相机图片路径");
            string pathImageIm = ParamSetMgr.GetInstance().GetStringParam("保存重要图片路径");
            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", true);
            double xphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.x;
            double yphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.y;
            double zphoto = dispCalibParam.pointDispenses.Find(t => t.strPointName == "拍目标点").MachinePoint.z;
            MoveSigleAxisPosWaitInpos(AxisZ, zphoto, (double)SpeedType.High, 0.02, bmanual, this, 60000);
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xphoto, yphoto, zphoto },
                new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);


            HObject img = null; HObject img1 = null; HObject img2 = null;
        retry_pr:
            CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
            cam.BindWindow(visionControl);

            Thread.Sleep(100);
            cam.StopGrap();
            cam.SetTriggerMode(CameraModeType.Software);
            cam.SetExposureTime(dispCalibParam.dDstExposure);
            cam.SetGain(dispCalibParam.dDstGain);
            cam.StartGrab();
            img = cam.GetImage();
            if (img == null || !img.IsInitialized())
            {
                img = cam.GetImage();
            }
            else
            {
                string path1 = pathImage + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\Up\\";
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                string path2 = pathImageIm + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\";
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                string imagepath1 = $"{path1}{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp";
                string imagepath2 = $"{path2}{DateTime.Now.ToString("1_HHmmssfff")}.bmp";
                Task.Run(() => { HOperatorSet.WriteImage(img.Clone(), "bmp", 0, imagepath1); File.Copy(imagepath1, imagepath2, true); });
            }
            img1 = img.Clone();
            shapeDst.ClearResult();
            shapeDst.Process_image(img.Clone(), visionControl);
            VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
            DoWhile dowhile = new DoWhile((time, dowhile1, bmanual2, objs) =>
            {
                visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                if (time > 1000)
                {
                    return AlarmMgr.GetIntance().WarnWithDlg("图像处理超时", this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, dowhile1, bmanual);

                }
                if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                    return WaranResult.Run;
                else
                    return WaranResult.CheckAgain;
            }, 3000);
            waranResult = dowhile.doSomething(this, dowhile, bmanual, null);
            if (waranResult == WaranResult.Retry)
            {
                goto retry_pr;
            }
            if (visionShapMatchobj == null || visionShapMatchobj.ResultCol.Count == 0)
            {
                return false;
            }
            double x = MotionMgr.GetInstace().GetAxisPos(AxisX);
            double y = MotionMgr.GetInstace().GetAxisPos(AxisY);
            XYUPoint SnapPoint = new XYUPoint(x, y, 0);
            XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], visionShapMatchobj.ResultAngle[0]);
            XYUPoint oldLaserPoint;
            XYUPoint nowLaserPoint;
            XYUPoint LaserGoDstPos;
            double DispenseHeight = 0;
            int nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (0 + 1).ToString());
            double pinOffSetX = dispCalibParam.dPinOffsetX;
            double pinOffSetY = dispCalibParam.dPinOffsetY;
            bool brtnExc = true;
            MotionMgr.GetInstace().ClearBufMove("点胶群组");
            MotionMgr.GetInstace().RestGpErr("点胶群组");
            GpState gps = MotionMgr.GetInstace().GetGpState("点胶群组");
            XYUPoint PinGoDstPosFistPoint = new XYUPoint();
            XYUPoint PinGoDstPosFinallyPoint = new XYUPoint();
            double DipZ = dispCalibParam.pointDispenses[nIndex].MachinePoint.z;

            //坐标点个数 
            //第一个点是否用来加速，如果是 那么第二个点开启点胶，最后回到第一个点开始收胶，如果不是那么第一个点开启点胶，最后回到第最后一个点开始收胶

            int nEnableCount = ParamSetMgr.GetInstance().GetIntParam("使用点胶点个数");
            bool FirstUseToUpDcc = ParamSetMgr.GetInstance().GetBoolParam("是否第一个点开始点胶");
            bool DispEnale = ParamSetMgr.GetInstance().GetBoolParam("自动运行是否出胶");
            double dDelayBeforeDispense = ParamSetMgr.GetInstance().GetDoubleParam("出胶开启前延迟") * 1000;
            double dDelayAfterDispense = ParamSetMgr.GetInstance().GetDoubleParam("出胶关闭后延迟") * 1000;
            double closeDipZ = ParamSetMgr.GetInstance().GetDoubleParam("拉胶距离Z轴");
            bool guleCheck = ParamSetMgr.GetInstance().GetBoolParam("点胶检查");
            bool enableMachineXY = ParamSetMgr.GetInstance().GetBoolParam("使用机械坐标点胶");
            double x1 = 0;
            double y1 = 0;
            //XY移动到第一个点,Z移到第一个点
            for (int i = 0; i < nEnableCount; i++)
            {
                if (i == nEnableCount - 1)
                {
                    IOMgr.GetInstace().WriteIoBit("点胶", false);
                }

                if (enableMachineXY)
                {
                    double xMachine = dispCalibParam.pointDispenses.Find(t => t.strPointName == $"机械{i + 1}点").MachinePoint.x;
                    double yMachine = dispCalibParam.pointDispenses.Find(t => t.strPointName == $"机械{i + 1}点").MachinePoint.y;
                    x1 = xMachine + pinOffSetX;
                    y1 = yMachine + pinOffSetY;
                }
                else
                {
                    nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点" + (i + 1).ToString());
                    if (nIndex == -1)
                        return false;
                    oldLaserPoint = new XYUPoint(dispCalibParam.pointDispenses[nIndex].MachinePoint.x, dispCalibParam.pointDispenses[nIndex].MachinePoint.y, 0);
                    nowLaserPoint = shapeDst.GetAffineTransPointAffterMatch(oldLaserPoint.x, oldLaserPoint.y, visionPoint, visionControl);
                    XYUPoint PinGoDstPos;
                    PinGoDstPos = XYUR_Pin.GetDstPonit(nowLaserPoint, SnapPoint);
                    x1 = PinGoDstPos.x + pinOffSetX;
                    y1 = PinGoDstPos.y + pinOffSetY;
                }
                Info($"第{i}个位置PinGoDstPos.x:{x1}.y:{y1};");



                if (i == 0)
                {
                    MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x1, y1 }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);
                    MoveSigleAxisPosWaitInpos(AxisZ, DipZ + 10, (double)SpeedType.High, 0.02, bmanual, this, 60000);
                    MoveSigleAxisPosWaitInpos(AxisZ, DipZ, (double)SpeedType.Low, 0.02, bmanual, this, 60000);
                    IOMgr.GetInstace().WriteIoBit("点胶", DispEnale);
                    if (dDelayBeforeDispense > 0)
                        Thread.Sleep((int)dDelayBeforeDispense);
                }
                else
                {
                    MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { x1, y1 }, new double[] { (double)SpeedType.Mid, (double)SpeedType.Mid }, 0.02, bmanual, this);

                }
            }
            MoveSigleAxisPosWaitInpos(AxisZ, closeDipZ + DipZ, (double)SpeedType.Mid, 0.02, bmanual, this, 60000);
            MoveSigleAxisPosWaitInpos(AxisZ, zphoto, (double)SpeedType.High, 0.02, bmanual, this, 60000);
            //出胶前延迟

            //出胶水

            //移到第二个点

            //移到第三个点

            //移到第四个点

            //移到第五个点


            //收胶水

            //移到第六个点

            //Z轴上抬拉胶距离
            //Z轴上抬20


            //涂完胶水后 回到拍照位置拍照，记录
            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", true);
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xphoto, yphoto, zphoto },
    new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);

            img2 = cam.GetImage();
            if (img2 == null || !img2.IsInitialized())
            {
                img2 = cam.GetImage();
            }
            else
            {
                string path1 = pathImage + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\Up\\";
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                string path2 = pathImageIm + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\";
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                string imagepath1 = $"{path1}{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp";
                string imagepath2 = $"{path2}{DateTime.Now.ToString("2_HHmmssfff")}.bmp";
                Task.Run(() => { HOperatorSet.WriteImage(img2.Clone(), "bmp", 0, imagepath1); File.Copy(imagepath1, imagepath2, true); });
            }



            double xfanilly = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.x;
            double yfanilly = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.y;
            double zfanilly = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.z;
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xfanilly, yfanilly, zfanilly },
new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);
            Info($"胶水检查开始");
            if (DispEnale && guleCheck)
            {
                if (!GlueCheck(img1, img2))
                {
                    Err($"胶水检查失败");
                    return false;
                }
            }
            Info($"点胶完成");
            return true;
        }

        public void GoSanpSave(bool bmanual = true)
        {
            double xfanilly = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.x;
            double yfanilly = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.y;
            double zfanilly = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.z;
            MoveSigleAxisPosWaitInpos(AxisZ, zfanilly, (double)SpeedType.Low, 0.02, bmanual, this, 60000);
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { xfanilly, yfanilly, zfanilly },
new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.02, bmanual, this);
        }
        public void GoSanpHome(bool bmanual = true)
        {
            MotionMgr.GetInstace().ServoOn((short)AxisZ);
            MotionMgr.GetInstace().ServoOn((short)AxisX);
            MotionMgr.GetInstace().ServoOn((short)AxisY);
            HomeSigleAxisPosWaitInpos(AxisZ, this, 120000, bmanual);
            HomeSigleAxisPosWaitInpos(AxisX, this, 120000, bmanual);
            HomeSigleAxisPosWaitInpos(AxisY, this, 120000, bmanual);
        }
        public bool GlueCheck(HObject Image1, HObject Image2)
        {
            bool result = true;
            string pathImage = ParamSetMgr.GetInstance().GetStringParam("保存相机图片路径");
            string path = pathImage + "\\" + DateTime.Now.ToString("yy-MM-dd") + "\\Fail\\";
            HObject ModleImage = null; HTuple Number1 = null; HTuple Number2 = null; HTuple Number3 = null;
            HObject modelROI = null; HTuple ModelID = null;
            HObject ModleRoiCircle = null; HObject RoiCircle1 = null; HObject RoiCircle2 = null;
            HObject ModleRoiRectangle = null; HObject RoiRectangle1 = null; HObject RoiRectangle2 = null;
            HObject RegionDifference = null; HObject RegionDifference1 = null; HObject RegionDifference2 = null;
            HObject ImageReduced = null; HObject ImageReduced1 = null; HObject ImageReduced2 = null;
            HObject ImageSub = null; HObject RegionThreshold = null; HObject ConnectedRegions = null;
            HObject SelectedRegions = null; HObject RegionClosing1 = null; HObject RegionClosing2 = null;
            HObject Skeleton = null; HObject SelectedSkeleton = null; HObject SelectedSkeleton2 = null; HObject RegionUnion = null;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //读取模板路径，模板参数
                //string Modelpath = "D:\ProductFile\DF\Disp\GlueCheck\ModelImage.bmp";
                //HTuple threshold = 30;
                //HTuple closing = 2;
                //HTuple modelCircleRow = 512.71; HTuple modelCircleCol = 838.492; HTuple modelCircleR = 97.1676;
                //HTuple RoiCircleRow = 515.131; HTuple RoiCircleCol = 635.466; HTuple RoiCircleR = 61.896;
                //HTuple RoiRectangleRow1 = 411.037; HTuple RoiRectangleCol1 = 542.693; HTuple RoiRectangleRow2 = 611.963; HTuple RoiRectangleCol2 = 724.206;

                string Modelpath = ParamSetMgr.GetInstance().GetStringParam("点胶检查路径模板").Replace("\\", "\\\\");
                HTuple threshold = ParamSetMgr.GetInstance().GetIntParam("点胶检查阈值");
                HTuple closing = ParamSetMgr.GetInstance().GetIntParam("点胶检查闭合像素");

                HTuple modelCircleRow = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查模板Row");
                HTuple modelCircleCol = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查模板Col");
                HTuple modelCircleR = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查模板R");
                HTuple RoiCircleRow = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查CircleRow");
                HTuple RoiCircleCol = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查CircleCol");
                HTuple RoiCircleR = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查CircleR");
                HTuple RoiRectangleRow1 = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查RectangleRow1");
                HTuple RoiRectangleCol1 = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查RectangleCol1");
                HTuple RoiRectangleRow2 = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查RectangleRow2");
                HTuple RoiRectangleCol2 = ParamSetMgr.GetInstance().GetDoubleParam("点胶检查RectangleCol2");

                HTuple Row1 = null; HTuple Column1 = null; HTuple Angle1 = null; HTuple Score1 = null;
                HTuple Row2 = null; HTuple Column2 = null; HTuple Angle2 = null; HTuple Score2 = null;
                HOperatorSet.ReadImage(out ModleImage, Modelpath);
                HOperatorSet.GenCircle(out modelROI, modelCircleRow, modelCircleCol, modelCircleR);
                HOperatorSet.GenCircle(out ModleRoiCircle, RoiCircleRow, RoiCircleCol, RoiCircleR);
                HOperatorSet.GenRectangle1(out ModleRoiRectangle, RoiRectangleRow1, RoiRectangleCol1, RoiRectangleRow2, RoiRectangleCol2);
                HOperatorSet.Difference(ModleRoiRectangle, ModleRoiCircle, out RegionDifference);
                HOperatorSet.ReduceDomain(Image1, modelROI, out ImageReduced);
                HOperatorSet.CreateNccModel(ImageReduced, 3, -0.39, 0.79, 0.1, "use_polarity", out ModelID);
                HOperatorSet.FindNccModel(Image1, ModelID, -0.39, 0.78, 0.8, 1, 0.5, "true", 0, out Row1, out Column1, out Angle1, out Score1);
                HOperatorSet.FindNccModel(Image2, ModelID, -0.39, 0.78, 0.8, 1, 0.5, "true", 0, out Row2, out Column2, out Angle2, out Score2);
                HOperatorSet.GenCircle(out RoiCircle1, RoiCircleRow - modelCircleRow + Row1, RoiCircleCol - modelCircleCol + Column1, RoiCircleR);
                HOperatorSet.GenCircle(out RoiCircle2, RoiCircleRow - modelCircleRow + Row2, RoiCircleCol - modelCircleCol + Column2, RoiCircleR);
                HOperatorSet.GenRectangle1(out RoiRectangle1, RoiRectangleRow1 - modelCircleRow + Row1, RoiRectangleCol1 - modelCircleCol + Column1, RoiRectangleRow2 - modelCircleRow + Row1, RoiRectangleCol2 - modelCircleCol + Column1);
                HOperatorSet.GenRectangle1(out RoiRectangle2, RoiRectangleRow1 - modelCircleRow + Row2, RoiRectangleCol1 - modelCircleCol + Column2, RoiRectangleRow2 - modelCircleRow + Row2, RoiRectangleCol2 - modelCircleCol + Column2);
                HOperatorSet.Difference(RoiRectangle1, RoiCircle1, out RegionDifference1);
                HOperatorSet.Difference(RoiRectangle2, RoiCircle2, out RegionDifference2);
                HOperatorSet.ReduceDomain(Image1, RegionDifference1, out ImageReduced1);
                HOperatorSet.ReduceDomain(Image2, RegionDifference2, out ImageReduced2);
                HOperatorSet.SubImage(ImageReduced1, ImageReduced2, out ImageSub, 1, 0);
                HOperatorSet.Threshold(ImageSub, out RegionThreshold, threshold, 255);
                HOperatorSet.ClosingCircle(RegionThreshold, out RegionClosing1, closing);
                HOperatorSet.Connection(RegionClosing1, out ConnectedRegions);
                HOperatorSet.SelectShape(ConnectedRegions, out SelectedRegions, "area", "and", 800, 99999);
                HOperatorSet.CountObj(SelectedRegions, out Number1);
                if (Number1.I < 1)
                {
                    HOperatorSet.WriteImage(Image1.Clone(), "bmp", 0, $"{path}LoseGlue1_{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp");
                    HOperatorSet.WriteImage(Image2.Clone(), "bmp", 0, $"{path}LoseGlue2_{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp");
                    return false;//无特征，漏胶
                }
                HOperatorSet.ClosingCircle(SelectedRegions, out RegionClosing2, closing);
                HOperatorSet.Skeleton(RegionClosing2, out Skeleton);
                HOperatorSet.SelectShape(Skeleton, out SelectedSkeleton, (((new HTuple("area")).TupleConcat("holes_num")).TupleConcat("column")).TupleConcat("row"), "and", (((new HTuple(300)).TupleConcat(0)).TupleConcat(300)).TupleConcat(300), (((new HTuple(1000000)).TupleConcat(1000)).TupleConcat(1000)).TupleConcat(1000));
                // HOperatorSet.Union1(SelectedSkeleton, out RegionUnion);
                HOperatorSet.CountObj(SelectedSkeleton, out Number2);
                if (Number2.I < 1)
                {
                    HOperatorSet.WriteImage(Image1.Clone(), "bmp", 0, $"{path}OpenGlueNoSkeleton1_{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp");
                    HOperatorSet.WriteImage(Image2.Clone(), "bmp", 0, $"{path}OpenGlueNoSkeleton2_{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp");
                    return false;//无特征，断胶
                }
                else
                {
                    HOperatorSet.SelectShape(SelectedSkeleton, out SelectedSkeleton2, "area_holes", "and", 10000, 99999999);
                    HOperatorSet.CountObj(SelectedSkeleton2, out Number3);
                    if (Number3.I != 1)
                    {
                        HOperatorSet.WriteImage(Image1.Clone(), "bmp", 0, $"{path}OpenGlue1AreaNum_{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp");
                        HOperatorSet.WriteImage(Image2.Clone(), "bmp", 0, $"{path}OpenGlue2AreaNum_{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp");
                        return false;//无特征，断胶
                    }

                }
            }
            catch
            {
                if (Image1 != null)
                {
                    HOperatorSet.WriteImage(Image1.Clone(), "bmp", 0, $"{path}GlueErr1_{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp");
                }
                if (Image2 != null)
                {
                    HOperatorSet.WriteImage(Image2.Clone(), "bmp", 0, $"{path}GlueErr2_{DateTime.Now.ToString("yyMMdd_HHmmssfff")}.bmp");
                }
                result = false;
            }
            finally
            {
                if (ModleImage != null) ModleImage.Dispose();
                if (modelROI != null) modelROI.Dispose();
                if (ModleRoiCircle != null) ModleRoiCircle.Dispose();
                if (RoiCircle1 != null) RoiCircle1.Dispose();
                if (RoiCircle2 != null) RoiCircle2.Dispose();
                if (ModleRoiRectangle != null) ModleRoiRectangle.Dispose();
                if (RoiRectangle1 != null) RoiRectangle1.Dispose();
                if (RoiRectangle2 != null) RoiRectangle2.Dispose();
                if (RegionDifference != null) RegionDifference.Dispose();
                if (RegionDifference1 != null) RegionDifference1.Dispose();
                if (RegionDifference2 != null) RegionDifference2.Dispose();
                if (ImageReduced != null) ImageReduced.Dispose();
                if (ImageReduced1 != null) ImageReduced1.Dispose();
                if (ImageReduced2 != null) ImageReduced2.Dispose();
                if (ImageSub != null) ImageSub.Dispose();
                if (RegionThreshold != null) RegionThreshold.Dispose();
                if (ConnectedRegions != null) ConnectedRegions.Dispose();
                if (SelectedRegions != null) SelectedRegions.Dispose();
                if (RegionClosing1 != null) RegionClosing1.Dispose();
                if (RegionClosing2 != null) RegionClosing2.Dispose();
                if (Skeleton != null) Skeleton.Dispose();
                if (SelectedSkeleton != null) SelectedSkeleton.Dispose();
                if (RegionUnion != null) RegionUnion.Dispose();
                if (Image1 != null) Image1.Dispose();
                if (Image2 != null) Image2.Dispose();
            }
            return result;
        }
        public void Init(bool bmanual = false)
        {
            Info("点胶开始初始化");
            double x, y, z;
            MotionMgr.GetInstace().ServoOn((short)AxisZ);
            MotionMgr.GetInstace().ServoOn((short)AxisX);
            MotionMgr.GetInstace().ServoOn((short)AxisY);
            HomeSigleAxisPosWaitInpos(AxisZ, this, 120000, bmanual);
            HomeSigleAxisPosWaitInpos(AxisX, this, 120000, bmanual);
            HomeSigleAxisPosWaitInpos(AxisY, this, 120000, bmanual);
            x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.x;
            y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.y;
            z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.z;
            MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ }, new double[] { x, y, z }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 0.01, bmanual, this, 600000);
            Info("点胶初始化完成");
            ParamSetMgr.GetInstance().SetBoolParam("点胶初始化完成", true);
        }

        public DispCalibParam dispCalibParam;
        DispConfig dispConfig;
        VisionShapMatch shapeDst;
        XY_UR_Calib XYUR_Pin;
        protected override bool InitStation()
        {
            ClearAllStep();
            dispCalibParam = UserConfig.dispCalibParam;
            dispConfig = UserConfig.dispConfig;
            shapeDst = UserConfig.shapeDst;
            XYUR_Pin = UserConfig.XYUR_Pin;
            IOMgr.GetInstace().WriteIoBit("点胶", false);
            IOMgr.GetInstace().WriteIoBit("点胶光源点亮", false);
            PushMultStep((int)StationStep.StepInit);
            //  x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.x;
            //  y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.y;
            //  z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.z;
            return true;
        }
        int SocketNumOfUnloadLoad = 0;// 上料站对应夹具号
        string strStationName = "";
        protected override void StationWork(int step)
        {
            WaranResult waranResult;
            double x, y, z;
            switch (step)
            {

                case (int)StationStep.StepInit:
                    Init();
                    DelCurrentStep();
                    PushMultStep((int)StationStep.StepCheckIpos);
                    break;
                case (int)StationStep.StepCheckIpos:
                    bool bA_UnLoadLoadStart = TableData.GetInstance().GetStationStartCmd("A_UnLoadLoad");
                    bool bB_UnLoadLoadStart = TableData.GetInstance().GetStationStartCmd("B_UnLoadLoad");
                    if (bA_UnLoadLoadStart || bB_UnLoadLoadStart)
                    {
                        TableData.GetInstance().ResetStartCmd("A_UnLoadLoad");
                        TableData.GetInstance().ResetStartCmd("B_UnLoadLoad");
                        strStationName = TableData.GetInstance().GetStationName();
                        if (strStationName == "A_Pick" || strStationName == "B_Pick")
                        {
                            TableData.GetInstance().SetStationResult("A_UnLoadLoad", true);
                            TableData.GetInstance().SetStationResult("B_UnLoadLoad", true);
                            return;
                        }

                        IOMgr.GetInstace().WriteIoBit("PASS指示灯（绿）", false);
                        IOMgr.GetInstace().WriteIoBit("NG指示灯（红）", false);
                        Info("开始上下料，安全光栅开始屏蔽");
                        ParamSetMgr.GetInstance().SetBoolParam("启用安全光栅", false);
                        ParamSetMgr.GetInstance().SetBoolParam("可以上下料", true);
                        SocketNumOfUnloadLoad = TableData.GetInstance().GetSocketNum(1, 0.5);
                        string pathCSV = ParamSetMgr.GetInstance().GetStringParam("保存CSV路径");
                        string pathRslt = ParamSetMgr.GetInstance().GetStringParam("保存Rslt路径");
                        string pathTar1 = ParamSetMgr.GetInstance().GetStringParam("保存Tar1路径");
                        string pathTar2 = ParamSetMgr.GetInstance().GetStringParam("保存Tar2路径");
                        string pathRsltBackUp = ParamSetMgr.GetInstance().GetStringParam("保存Rslt路径BackUp");
                        string pathTar1BackUp = ParamSetMgr.GetInstance().GetStringParam("保存Tar1路径BackUp");
                        string pathTar2BackUp = ParamSetMgr.GetInstance().GetStringParam("保存Tar2路径BackUp");
                        SocketState state = SocketMgr.GetInstance().socketArr[SocketNumOfUnloadLoad - 1].socketState;
                        if (state == SocketState.HaveOK || state == SocketState.HaveNG)
                        {
                            string lightColor = state == SocketState.HaveOK ? "PASS指示灯（绿）" : "NG指示灯（红）";
                            string fp = state == SocketState.HaveOK ? "P" : "F";
                            ProductInfo.CountNow++;
                            ProductInfo.ProductCompete++;
                            ProductInfoIni.Write("CT", "ProductCount", ProductInfo.ProductCompete.ToString());
                            PatsTest errCode = UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestResult == "F");
                            string failCode = "";
                            if (errCode != null)
                            {
                                failCode = errCode.TestCode;
                                UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].FailureTestCode = failCode;
                                UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].FailStep = failCode + errCode.Info;
                                if (SocketNumOfUnloadLoad == 1) ProductInfo.FailA++; else ProductInfo.FailB++;
                                fp = "F";
                                lightColor = "NG指示灯（红）";
                                ProductInfoIni.Write("CT", "FailA", ProductInfo.FailA.ToString());
                                ProductInfoIni.Write("CT", "FailB", ProductInfo.FailB.ToString());
                            }
                            else
                            {
                                UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].FailStep = "N/A";
                            }
                            string getModelNumber = GlobalParaSet.GetModelNumber(UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].SN);
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].TarB = getModelNumber;
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarB = "B" + getModelNumber;
                            ProductInfo.EndCT = true;
                            ProductInfo.endTime = DateTime.Now;
                            UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].Result = fp == "P" ? "PASS" : "FAIL";
                            UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].EndTime = DateTime.Now;
                            UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].TestTime = (UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].EndTime - UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].StarTime).TotalSeconds;
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarEndTime = $"]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarT = "T" + fp;
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].FailTesp ="F"+ failCode;
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].TestResult = fp;
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].TestTime = UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].TestTime.ToString("0.0000");
                            string errCsv = CSVHelper.Instance.SaveToCSVPath(pathCSV, UserTest.TestResultAB[SocketNumOfUnloadLoad - 1]);
                            string errRslt = RsltHelper.Instance.SaveToRsltPath(pathRslt, pathRsltBackUp, UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1]);
                            string errTar = TarHelper.Instance.SaveToTarPath(pathTar1, pathTar2, pathTar1BackUp, pathTar2BackUp, UserTest.TarResultAB[SocketNumOfUnloadLoad - 1]);
                            //ParamSetMgr.GetInstance().SetDoubleParam("",0.33);
                            delegateShowCT();
                            IOMgr.GetInstace().WriteIoBit(lightColor, true);
                            Info($"保存OK结果：cvs={errCsv}，rslt={errRslt}，tar={errTar}.");
                        }
                        PickFromSockt(SocketNumOfUnloadLoad);
                        if (ParamSetMgr.GetInstance().GetBoolParam("启动清料"))
                        {

                            StationMgr.GetInstance().Stop();
                            Info("启动清料stop:结束");
                            AlarmMgr.GetIntance().StopAlarmBeet();
                        }
                    retry_check_Start:
                        waranResult = doWhileCheckStartSignal.doSomething(this, doWhileCheckStartSignal, false, new object[] { this });
                        if (waranResult == WaranResult.Retry)
                            goto retry_check_Start;
                        ProductInfo.ClearShow = false;
                        if (ParamSetMgr.GetInstance().GetBoolParam("启动清料"))
                        {
                            DelCurrentStep();
                            PushMultStep((int)StationStep.StepGoSnap);
                            Thread.Sleep(1000);
                            break;

                        }
                        IOMgr.GetInstace().WriteIoBit("PASS指示灯（绿）", false);
                        IOMgr.GetInstace().WriteIoBit("NG指示灯（红）", false);
                        UserTest.TestResultAB[SocketNumOfUnloadLoad - 1] = new TestResult();
                        UserTest.TarResultAB[SocketNumOfUnloadLoad - 1] = new TarResult();
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1] = new RsltResult();
                        UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].ListTarTestObj = new List<TarTestObj>();
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST = new List<PatsTest>();
                        UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].StarTime = DateTime.Now;
                        UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].GlueInformation = ParamSetMgr.GetInstance().GetStringParam("胶水信息");
                        UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].SurplusInformation = ParamSetMgr.GetInstance().GetStringParam("胶水总量");
                        //默认数据写入Rslt
                        {
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ProcessCode = ParamSetMgr.GetInstance().GetStringParam("ProcessCode");
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].StationCode = ParamSetMgr.GetInstance().GetStringParam("StationCode");
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BarCodeScanning, Index = "1", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "扫码失败！" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.RoeCheck, Index = "2", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "真空检查失败！" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.Gluecheck, Index = "3", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "胶水检查失败！" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.UUTCheck, Index = "4", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "抓取失败！" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.PreFocusXLine, Index = "5", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(FindcenterAfterX)AA前X宽度" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.PreFocusXLineum, Index = "6", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(FindcenterAfterXum)AA前X宽度实际值" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.FocusY, Index = "7", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(FindcenterAfterCrossY)AA前坐标Y" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.FocusX, Index = "8", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(FindcenterAfterCrossX)AA前坐标X" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.FocusBeamX, Index = "9", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(AAAfterBreamprofileX)AA后BeamX" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeOffsetBeamX, Index = "10", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(GlueBreamprofileX)胶缩前BeamX" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeXLine, Index = "11", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVBeforeX)UV前X宽度" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeXLineum, Index = "12", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVBeforeXum)UV前X宽度实际值" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeYLine, Index = "13", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVBeforeY)UV前Y宽度" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeYLineum, Index = "14", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVBeforeYum)UV前Y宽度实际值" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeCureCenterX, Index = "15", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVBeforeCrossX)UV前坐标X" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeCureCenterY, Index = "16", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVBeforeCrossY)UV前坐标Y" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeCureBeamX, Index = "17", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVBeforeBreamprofileX)UV前BeamX" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.BeforeCurePower, Index = "18", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVBeforePower)UV前功率" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterXLine, Index = "19", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterX)UV后X宽度" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterXLineum, Index = "20", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterXum)UV后X宽度实际值" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterYLine, Index = "21", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterY)UV后Y宽度" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterYLineum, Index = "22", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterYum)UV后Y宽度实际值" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterCureCenterX, Index = "23", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterCrossX)UV后坐标X" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterCureCenterY, Index = "24", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterCrossY)UV后坐标Y" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterCureBeamX, Index = "25", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterBreamprofileX)UV后BeamX" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterCureBeamY, Index = "26", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterBreamprofileY)UV后BeamY" });
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Add(new PatsTest { TestCode = PatsTestEnum.AfterCurePower, Index = "27", TestValue = "0", LowerLimit = "0", UpperLimit = "0", TestResult = "F", Info = "(UVAfterAfterPower)UV后功率" });
                            //默认数据写入Tar
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarS = ParamSetMgr.GetInstance().GetStringParam("Tar_S");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarC = ParamSetMgr.GetInstance().GetStringParam("Tar_C");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarB = ParamSetMgr.GetInstance().GetStringParam("Tar_B");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarN = ParamSetMgr.GetInstance().GetStringParam("Tar_N");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarP = ParamSetMgr.GetInstance().GetStringParam("Tar_P");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].Tars = ParamSetMgr.GetInstance().GetStringParam("Tar_s");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarD = ParamSetMgr.GetInstance().GetStringParam("Tar_D");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarR = ParamSetMgr.GetInstance().GetStringParam("Tar_R");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].Tarn = ParamSetMgr.GetInstance().GetStringParam("Tar_n");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].Tarr = ParamSetMgr.GetInstance().GetStringParam("Tar_r");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarW = ParamSetMgr.GetInstance().GetStringParam("Tar_W");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarT = ParamSetMgr.GetInstance().GetStringParam("Tar_T");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarO = ParamSetMgr.GetInstance().GetStringParam("Tar_O");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarL = ParamSetMgr.GetInstance().GetStringParam("Tar_L");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].Tarp = ParamSetMgr.GetInstance().GetStringParam("Tar_p");
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarBeginTime = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                        }
                        DateTime timeCheckLens = DateTime.Now;
                        PlaceToSocket(SocketNumOfUnloadLoad);
                        string testTimeCheckLens = (DateTime.Now - timeCheckLens).TotalSeconds.ToString("0.0000");
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.RoeCheck).TestValue = "1";
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.RoeCheck).TestResult = "P";
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.RoeCheck).TestTime = testTimeCheckLens;
                        UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].ListTarTestObj.Add(new TarTestObj { TestName = PatsTestEnum.RoeCheck, TestValue = "1", TestUint = "NA" });
                        ParamSetMgr.GetInstance().SetBoolParam("启用安全光栅", true);
                        ParamSetMgr.GetInstance().SetBoolParam("可以上下料", false);
                        Info("开始上下料，安全光栅开始启用");
                        DateTime ScannerStar = DateTime.Now;
                        string ScannerResult = "F";
                    retry_scan_again:
                        OtherDevices.cOM_KEYENCE_Scanner.LonScanner();
                        if (OtherDevices.cOM_KEYENCE_Scanner.ReceiveScannerData(ref strCode2d))
                        {
                            SocketMgr.GetInstance().socketArr[SocketNumOfUnloadLoad - 1].strBarCode2d = strCode2d;
                            UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].SerialNumber = strCode2d.Trim();
                            UserTest.TestResultAB[SocketNumOfUnloadLoad - 1].SocketerNumber = SocketNumOfUnloadLoad == 1 ? "A" : "B";
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].TarS = "S" + strCode2d.Trim();
                            UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].Tars = "s" + SocketNumOfUnloadLoad.ToString();
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].SN = strCode2d.Trim();
                            UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].FixtureCode = SocketNumOfUnloadLoad.ToString();
                            if (eventFlushBarcode != null)
                            {
                                eventFlushBarcode?.Invoke(SocketNumOfUnloadLoad, strCode2d);
                            }
                            ScannerResult = "P";
                        }
                        else
                        {
                            OtherDevices.cOM_KEYENCE_Scanner.LoffScanner();
                            OtherDevices.cOM_KEYENCE_Scanner.LonScanner();
                            if (AlarmMgr.GetIntance().WarnWithDlg("转盘站：扫2维码失败", this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null, false) == WaranResult.Retry)
                                goto retry_scan_again;
                        }
                        string testTimeScanner = (DateTime.Now - ScannerStar).TotalSeconds.ToString("0.0000");
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.BarCodeScanning).TestValue = ScannerResult == "P" ? "1" : "0";
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.BarCodeScanning).TestResult = ScannerResult;
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.BarCodeScanning).TestTime = testTimeScanner;
                        UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].ListTarTestObj.Add(new TarTestObj { TestName = PatsTestEnum.BarCodeScanning, TestValue = "1", TestUint = "NA" });
                        SocketMgr.GetInstance().SetSocketState(SocketNumOfUnloadLoad, SocketState.Have);

                        DelCurrentStep();
                        PushMultStep((int)StationStep.StepGoSnap);
                    }
                    break;
                case (int)StationStep.StepGoSnap:
                    DateTime timeDisp = DateTime.Now;
                retry_Disp:
                    bool result = false;
                    if (!ParamSetMgr.GetInstance().GetBoolParam("启动清料"))
                    {
                        result = GoSanpDispNew2(this.VisionControl, false);
                        if (!result)
                        {
                            PickFromSockt(SocketNumOfUnloadLoad);
                            ParamSetMgr.GetInstance().SetBoolParam("启用安全光栅", false);
                            WaranResult r = AlarmMgr.GetIntance().WarnWithDlg("胶水检测失败，请重新更换物料点胶,如果点击忽略 30秒后停止", this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry);
                            if (r == WaranResult.Ignore)
                            {
                                Err($"胶水检查失败,10秒后停止。。。");
                                Thread.Sleep(10000);
                                StationMgr.GetInstance().Stop();
                                AlarmMgr.GetIntance().StopAlarmBeet();
                            }
                            else
                            {
                                PlaceToSocket(SocketNumOfUnloadLoad);
                            }
                            ParamSetMgr.GetInstance().SetBoolParam("启用安全光栅", true);
                            goto retry_Disp;
                        }
                        string testTimeDisp = (DateTime.Now - timeDisp).TotalSeconds.ToString("0.0000");
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.Gluecheck).TestValue = "1";
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.Gluecheck).TestResult = result ? "P" : "F";
                        UserTest.RsltResultAB[SocketNumOfUnloadLoad - 1].ListPATS_TEST.Find(p => p.TestCode == PatsTestEnum.Gluecheck).TestTime = testTimeDisp;
                        UserTest.TarResultAB[SocketNumOfUnloadLoad - 1].ListTarTestObj.Add(new TarTestObj { TestName = PatsTestEnum.Gluecheck, TestValue = "1", TestUint = "NA" });

                    }
                    ParamSetMgr.GetInstance().SetBoolParam("点胶完成", true);
                    TableData.GetInstance().SetStationResult("A_UnLoadLoad", true);
                    TableData.GetInstance().SetStationResult("B_UnLoadLoad", true);
                    DelCurrentStep();
                    PushMultStep((int)StationStep.StepCheckIpos);
                    break;


            }


        }

    }
}