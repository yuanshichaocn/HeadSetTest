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
    public class StationCheckHight : CommonTools.Stationbase
    {
  
        public StationCheckHight(CommonTools.Stationbase pb) : base(pb)
        {
            //m_listIoInput.Add("点胶头下降到位");
            //m_listIoInput.Add("UV光源上升到位");
            //m_listIoOutput.Add("Barrel中心吸真空");
            //m_listIoOutput.Add("Barrel外环破真空");
            //m_listIoOutput.Add("Barrel外环吸真空");
            //m_listIoOutput.Add("Barrel中心破真空");
            //m_listIoOutput.Add("点胶头下降");
            //m_listIoOutput.Add("UV光源下降");
        }
        enum StationStep
        {
            StepInit = 100,  
            Stp1,
            Stp2,
            Stp3,
            StepEnd,
        }
        protected override bool InitStation()
        {
            PushMultStep((int)StationStep.StepInit);
            return true;
        }
        public void fun1(bool bmanual)
        {

        }
      
       
        protected override void StationWork(int step)
        {

            switch ((StationStep)step)
            {
                case StationStep.StepInit:
                    PushMultStep((int)StationStep.Stp1);
                    DelCurrentStep();
                    break;
                case StationStep.Stp1:
                    fun1(true);
                    PushMultStep( (int)StationStep.Stp2, (int)StationStep.Stp3 );
                    DelCurrentStep();
                    break;
                case StationStep.Stp2:
                    DelCurrentStep();
                    break;
                case StationStep.Stp3:
                    PushMultStep((int)StationStep.Stp1  );
                    DelCurrentStep();
                    break;
                case StationStep.StepEnd:
                    DelCurrentStep();
                    break;
            }

        }
        

    }
}