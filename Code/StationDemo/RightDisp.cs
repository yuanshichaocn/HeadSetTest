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
    public class StationRightDisp : CommonTools.Stationbase
    {
        public StationRightDisp(string strStationName, int[] arrAxis, string[] axisname, params string[] CameraName)
            : base(strStationName, arrAxis, axisname, CameraName)
        {

        }
        public StationRightDisp(CommonTools.Stationbase pb) : base(pb)
        {
            m_listIoInput.Add("光管升降左气缸上升到位");
            m_listIoInput.Add("光管升降左气缸下降到位");
            m_listIoInput.Add("光管升降右气缸上升到位");
            m_listIoInput.Add("光管升降右气缸下降到位");
            m_listIoInput.Add("UV固化灯上升到位");
            m_listIoInput.Add("UV固化灯下降到位");
            m_listIoInput.Add("胶水液位检测");

            m_listIoOutput.Add("点胶Z轴伺服刹车");
            m_listIoOutput.Add("点胶阀打开");
            m_listIoOutput.Add("CCD环形光源");
            m_listIoOutput.Add("UV固化灯气缸升降");
            m_listIoOutput.Add("UV灯打开");
            m_listIoOutput.Add("左lens升降气缸");
        }
        enum StationStep
        {
           
            StepInit = 100,//回原点
            [Description("XY回上料预备位置")]
            Step_GoXYReadyLoadPos,//XY回上料预备位置
            [Description("判断是否可以上料")]
            Step_JudgeCanLoad, //判断是否可以上料
            [Description("回上料位置")]
            Step_GoXYFeedPos,//XY 回上料位置
            [Description("等待上料完成")]
            Step_WaitLoadFinish,//等待上料完成
            [Description("检查Hoder")]
            Step_CheckHoder,
            [Description("Z轴拍照准备位置")]
            Step_GoZSanpReadyPos,//Z轴拍照准备位置
            [Description("去拍照位置")]
            Step_GoSanpPos,//去拍照位置
            [Description("拍照")]
            Step_SanpPos,//拍照
            [Description("拍照失败")]
            Step_SanpPosFailture,
            [Description("去清洗Hoder位置")]
            Step_GoHoderPlasumaPos,//去清洗Hoder位置
            [Description("Hoder清洗")]
            Step_HoderPlasuma,//Hoder清洗
            [Description("去清洗Lens位置")]
            Step_GoLensPlasumaPos,//去清洗Hoder位置
            [Description("Lens清洗")]
            Step_LensPlasuma,//Lens清洗
            [Description("去点胶高度探测位置")]
            Step_GoXYDispHighTestPos,//去点胶高度探测位置
            [Description("点胶高度探测")]
            Step_DispHighTest,//点胶高度探测
            [Description("Hoder 点胶")]
            Step_DispHoder,//Hoder 点胶
            [Description("通知AA工站点胶清洗完成")]
            Step_NotifyAADispAndClearFinish,
            [Description("Z轴回预备位置")]
            Step_GoZReadyPos,
            [Description("去预调焦点位置")]
            Step_GoPreAAPos,
            [Description("等待AA预调焦点完成")]
            Step_WaitPreAAFinish,
            [Description("判断下一步")]
            Step_JudgeNextStep,
            [Description("去预备位置")]
            Step_GoAAReadyPos,
            [Description("平行光管下降")]
            Step_CollimatorDown,
            [Description("AA 点亮")]
            Step_Light,
            [Description("AA 对心")]
            Step_ClibCenter,
            [Description("AA Z轴移动")]
            Step_GoZAA,
            [Description("AA Snap")]
            Step_GoSnap,
            [Description("AA Tito")]
            Step_TifoAdjust,
            [Description("AA 复检")]
            Step_ReExamination,
            [Description("UV预备位置")]
            Step_GoUVReadyPos,
            [Description("UV灯下降")]
            Step_UVDown,
            [Description("UV灯固化")]
            Step_UVLight,
            [Description("UV灯上升")]
            Step_UVUp,

            [Description("X回预备收料位置")]
            Step_GoXPreFeedPos,
            //[Description("XY回收料位置")]
            //Step_GoXYFeedPos,
            StepEnd,
        }
        //public override void ProcessWork()
        //{
        //    m_listStep.Clear();
        //    PushStep((int)StationStep.StepInit);
        //    m_bExit = false;

        //    while (!m_bExit)
        //    {
        //        if (sys.g_StationState == StationState.StationStateStop)
        //            return;
        //        if (ChcekState())
        //        {
        //            Thread.Sleep(10);
        //            continue;
        //        }
               
        //        m_iCurrentStep = GetNextStep();
        //        switch (m_iCurrentStep)
        //        {
        //            default:
        //                ProcessDefault((StationDefaultStep)m_iCurrentStep);
        //                break;
        //        }
        //        Thread.Sleep(10);
        //    }
        //}
    }
}