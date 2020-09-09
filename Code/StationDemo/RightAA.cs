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
    public class StationRightAA : CommonTools.Stationbase
    {
        public StationRightAA(string strStationName, int[] arrAxis, string[] axisname, params string[] CameraName)
            : base(strStationName, arrAxis, axisname, CameraName)
        {

        }
        public StationRightAA(CommonTools.Stationbase pb) : base(pb)
        {
            m_listIoInput.Add("右lens夹紧气缸夹紧到位");
            m_listIoInput.Add("右lens夹紧气缸张开原位");
            m_listIoInput.Add("右lens升降气缸上升到位");
            m_listIoInput.Add("右lens升降气缸下降到位");

            m_listIoInput.Add("右lens翻转手爪上升到位");
            m_listIoInput.Add("右lens翻转手爪下降到位");
            m_listIoInput.Add("右lens翻转手爪伸出到位");
            m_listIoInput.Add("右lens翻转手爪回退到位");

            m_listIoInput.Add("右Lens翻转手爪正转到位");
            m_listIoInput.Add("右Lens翻转手爪反转到位");


            m_listIoOutput.Add("右lens夹紧气缸");
            m_listIoOutput.Add("右lens升降气缸");
            m_listIoOutput.Add("右lens翻转手爪升降");
            m_listIoOutput.Add("右lens翻转手爪伸缩");
            m_listIoOutput.Add("右lens翻转手爪转动");
            m_listIoOutput.Add("右翻转手爪夹紧");

            m_listIoOutput.Add("DV5V电源选择");
            m_listIoOutput.Add("DV12V电源选择");
            m_listIoOutput.Add("平行光管点亮");
            m_listIoOutput.Add("光管气缸升降");

           

        }
        enum StationStep
        {
            StepInit = 100,//回原点
           

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