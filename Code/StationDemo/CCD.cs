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
    public class StationCCD : CommonTools.Stationbase
    {
        public StationCCD(string strStationName, int[] arrAxis, string[] axisname, params string[] CameraName)
            : base(strStationName, arrAxis, axisname, CameraName)
        {

        }
        public StationCCD(CommonTools.Stationbase pb) : base(pb)
        {
            m_listIoInput.Add("启动");
            m_listIoInput.Add("停止");
            m_listIoInput.Add("hoder到位");
            m_listIoInput.Add("hoder原位");

            m_listIoOutput.Add("黄灯");
            m_listIoOutput.Add("hoder夹紧气缸");

        }
        enum StationStep
        {
            StepInit = 100,
          
            StepEnd,
        }
       
    }
}