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
namespace StationDemo
{
    public class StationLeftAA : CommonTools.Stationbase
    {
        public StationLeftAA(string strStationName, int[] arrAxis, string[] axisname, params string[] CameraName)
            : base(strStationName, arrAxis, axisname, CameraName)
        {

        }
        public StationLeftAA(CommonTools.Stationbase pb) : base(pb)
        {
            m_listIoInput.Add("左lens夹紧气缸夹紧到位");
            m_listIoInput.Add("左lens夹紧气缸张开原位");
            m_listIoInput.Add("左lens升降气缸上升到位");
            m_listIoInput.Add("左lens升降气缸下降到位");

            m_listIoInput.Add("左lens翻转手爪上升到位");
            m_listIoInput.Add("左lens翻转手爪下降到位");
            m_listIoInput.Add("左lens翻转手爪伸出到位");
            m_listIoInput.Add("左lens翻转手爪回退到位");

            m_listIoInput.Add("左Lens翻转手爪正转到位");
            m_listIoInput.Add("左Lens翻转手爪反转到位");


            m_listIoOutput.Add("左lens夹紧气缸");
            m_listIoOutput.Add("左lens升降气缸");
            m_listIoOutput.Add("左lens翻转手爪升降");
            m_listIoOutput.Add("左lens翻转手爪伸缩");
            m_listIoOutput.Add("左lens翻转手爪转动");
            m_listIoOutput.Add("左翻转手爪夹紧");


            m_listIoOutput.Add("DV5V电源选择");
            m_listIoOutput.Add("DV12V电源选择");
            m_listIoOutput.Add("平行光管点亮");
            m_listIoOutput.Add("光管气缸升降");


        }
        enum example
        {
           中国,
           日本,
        }
        enum StationStep
        {
            [Description("初始化")]
            Step_Init ,
            [Description("6轴Z回初始位")]
           
            Step_GoZInitPos,
           [Description("6轴X回初始位")]
          
            Step_GoXInitPos,
          [Description("6轴回初始位")]
          
            Step_GoInitPos,

          
           // [Description("lens翻转气缸上升")]
            Step_LensRollingOverCylinderUp,
            [Description("lens翻转气缸上升到位")]
            Step_LensRollingOverCylinderUpOn,
            [Description("lens翻转气缸下降")]
            Step_LensRollingOverCylinderDown,
            [Description("lens翻转气缸下降到位")]
            Step_LensRollingOverCylinderDownOn,

            [Description("lens翻转气缸伸出")]
            Step_LensRollingOverCylinderStrecthOut,
            [Description("lens翻转气缸伸出到位")]
            Step_LensRollingOverCylinderStrecthOutOn,
            [Description("lens翻转气缸退回")]
            Step_LensRollingOverCylinderStrecthBack,
            [Description("lens翻转气缸退回到位")]
            Step_LensRollingOverCylinderStrecthBackOn,


            [Description("lens翻转气缸夹紧")]
            Step_LensRollingOverCylinderClose,
            [Description("lens翻转气缸夹紧到位")]
            Step_LensRollingOverCylinderCloseOn,
            [Description("lens翻转气缸张开")]
            Step_LensRollingOverCylinderOpen,
            [Description("lens翻转气缸张开到位")]
            Step_LensRollingOverCylinderOpenOn,

            [Description("lens翻转气缸正转")]
            Step_LensRollingOverCylinderForward,
            [Description("lens翻转气缸反转")]
            Step_LensRollingOverCylinderReverse,

            [Description("lens升降气缸上升")]
            Step_LensLiftCylinderUp,
            [Description("lens升降气缸上升到位")]
            Step_LensLiftCylinderUpOn,
            [Description("lens升降气缸下降")]
            Step_LensLiftCylinderDown,
            [Description("lens升降气缸下降到位")]
            Step_LensLiftCylinderDownOn,

            [Description("Lens夹紧气缸张开")]
            Step_LensClampingCylinderOpen,
            [Description("Lens夹紧气缸张开到位")]
            Step_LensClampingCylinderOpenOn,
            [Description("Lens夹紧气缸夹紧")]
            Step_LensClampingCylinderClose,
            [Description("Lens夹紧气缸夹紧到位")]
            Step_LensClampingCylinderCloseOn,

            [Description("去Lens准备位")]
            Step_GoLensReadyPos,
            [Description("6轴平台准备好")]
            Step_6AxisPlaformReady,
            [Description("等待清洗完成")]
            Step_WaitClearFinih,

            [Description("去Lens夹紧位")]
            Step_GoLensClampPos,

            [Description("去Lens工作X工作位")]
            Step_GoXLensWorkPos,
            [Description("去Lens工作Z工作位")]
            Step_GoZLensWorkPos,
            [Description("等待预调焦结果")]
            Step_WaitPreFocusResult,
            [Description("预调焦")]
            Step_PreFocusing,

            [Description("等待Z轴移动Z指令")]
            Step_WaitAAZMove,

            [Description("AA-Z轴移动")]
            Step_AAZMove,

            [Description("AA-Z轴移动完成")]
            Step_AAZMoveFinish,

            [Description("等待AA")]
            Step_WaitAAReslut,

            [Description("AA调整")]
            Step_AA_Adjust,

            [Description("等待X退回准备位指令")]
            Step_WaitCmdXBack,
            [Description("X退回准备位到位，通知点胶工站")]
            Step_NotifyInXpos,



            StepEnd,
        }
        protected virtual bool InitStation()
        {
            //初始化工站
            PushStep((int)StationStep.Step_Init);
            return true;
        }
        protected virtual void StationWork(int step)
        {
            //工站流程
            switch(step)
            {
                case (int)StationStep.Step_Init:
                    Info("左AA初始化");
                    PushMultStep(
                         (int)StationStep.Step_LensRollingOverCylinderOpen,
                        (int)StationStep.Step_LensRollingOverCylinderOpenOn,
                        (int)StationStep.Step_LensClampingCylinderOpen,
                        (int)StationStep.Step_LensClampingCylinderOpenOn,
                        (int)StationStep.Step_LensLiftCylinderDown,
                        (int)StationStep.Step_LensLiftCylinderDownOn,
                        (int)StationStep.Step_LensRollingOverCylinderStrecthBack,
                        (int)StationStep.Step_LensRollingOverCylinderStrecthBackOn,
                        (int)StationStep.Step_LensRollingOverCylinderDown,
                        (int)StationStep.Step_LensRollingOverCylinderDownOn,

                       


                        (int)StationDefaultStep.HomeZ,
                        (int)StationDefaultStep.WaitHomeZ, 
                        (int)StationDefaultStep.HomeX,
                        (int)StationDefaultStep.WaitHomeX,
                        (int)StationDefaultStep.WaitHomeY,
                        (int)StationDefaultStep.WaitHomeTx,
                        (int)StationDefaultStep.WaitHomeTy,
                        (int)StationDefaultStep.WaitHomeU,
                        (int)StationDefaultStep.WaitHomeU,
                        (int)StationDefaultStep.WaitHomeTx,
                        (int)StationDefaultStep.WaitHomeTy,
                        (int)StationDefaultStep.WaitHomeU,
                        (int)StationStep.Step_GoZInitPos,
                        (int)StationDefaultStep.WaitZ,
                        (int)StationStep.Step_GoXInitPos,
                        (int)StationDefaultStep.WaitX,
                        (int)StationStep.Step_GoInitPos,
                        (int)StationDefaultStep.WaitY,
                        (int)StationDefaultStep.WaitU,
                        (int)StationDefaultStep.WaitTx,
                        (int)StationDefaultStep.WaitTy,
                        (int)StationStep.Step_LensLiftCylinderUp,
                        (int)StationStep.Step_LensLiftCylinderUpOn,
                        (int)StationStep.Step_GoLensReadyPos,
                        (int)StationDefaultStep.WaitX,
                        (int)StationDefaultStep.WaitY,
                         (int)StationStep.Step_6AxisPlaformReady,
                        (int)StationStep.Step_WaitClearFinih

                        );
                    sys.GetInstance().SetBoolParam("左6轴平台准备完成", false);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoZInitPos:
                    Info("去初始位Z位");
                    AbsMoveSingleAxis(AxisZ, GetStationPointDic()["初始位"].pointZ, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoXInitPos:
                    Info("去初始位X位");
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["初始位"].pointX, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoInitPos:
                    Info("去初始位位");
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["初始位"].pointY, 0);
                    AbsMoveSingleAxis(AxisU, GetStationPointDic()["初始位"].pointU, 0);
                    AbsMoveSingleAxis(AxisTx, GetStationPointDic()["初始位"].pointTx, 0);
                    AbsMoveSingleAxis(AxisTx, GetStationPointDic()["初始位"].pointTx, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoLensReadyPos:
                    Info("去准备位");
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["准备位"].pointX, 0);
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["准备位"].pointY, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_6AxisPlaformReady:
                    sys.GetInstance().SetBoolParam("左6轴平台准备完成", true);
                     DelCurrentStep();
                    break;
                case (int)StationStep.Step_LensRollingOverCylinderOpen:
                    IOMgr.GetInstace().WriteIoBit("左翻转手爪夹紧", true);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_LensRollingOverCylinderOpenOn:
                    WaitSingleIO("左lens夹紧气缸夹紧到位", IOType.Type_Input);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_LensRollingOverCylinderStrecthBack:
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_LensRollingOverCylinderStrecthBackOn:
                    DelCurrentStep();
                    break;

                case (int)StationStep.Step_LensRollingOverCylinderStrecthOut:
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_LensRollingOverCylinderStrecthOutOn:
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_LensRollingOverCylinderDownOn:
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_WaitClearFinih:
                    Info("等待清洗完成");
                    if(sys.GetInstance().GetBoolParam("通知左AA台装料"))
                    {
                        sys.GetInstance().SetBoolParam("左6轴平台准备完成", false);
                        sys.GetInstance().SetBoolParam("通知左AA台装料", false);
   
                        ClearAllStep();
                        PushMultStep((int)StationStep.Step_LensRollingOverCylinderUp,
                            (int)StationStep.Step_LensRollingOverCylinderUpOn

                            );
                    }
                    break;
             

            }
        }
    }
}