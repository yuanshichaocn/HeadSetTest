using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserCtrl;
using BaseDll;
using HalconDotNet;

namespace VisionProcess
{
    public partial class VisionFindCircleCtr : VisionBaseCtr
    {
        public VisionFindCircleCtr()
        {
            InitializeComponent();
        }

        public override void FlushToDlg(VisionSetpBase visionSetp, VisionControl visionControl, Control Farther=null)
        {
            m_visionControl = visionControl;
           if (visionSetp!=null)
            {
                FitCircircle = visionSetp as VisionFitCircircle;
                if (FitCircircle == null)
                    return;
                FitCircircle = (VisionFitCircircle)visionSetp;
                textBox_TestStartAngle.Text = FitCircircle.visionFitCircleParam.startAngledeg.ToString();
                textBox_TestEndAngle.Text = FitCircircle.visionFitCircleParam.endAngleDeg.ToString();
                textBox_TestNum.Text = FitCircircle.visionFitCircleParam.nTestNum.ToString();
                textBox_TestThreasoldVal.Text = FitCircircle.visionFitCircleParam.thresoldVal.ToString();
                textBox_Len1.Text = FitCircircle.visionFitCircleParam.nLen1.ToString();
                textBox_Len2.Text = FitCircircle.visionFitCircleParam.nLen2.ToString();
                comboBox_TestDir.Text = FitCircircle.visionFitCircleParam.testDir.ToString();
                comboBox_TestPolarity.Text = FitCircircle.visionFitCircleParam.testPolarity.ToString();
                comboBox_TestPos.Text = FitCircircle.visionFitCircleParam.testPos.ToString();

            }
        }

        public override void SaveParm(VisionSetpBase visionSetp)
        {
            if(visionSetp!=null)
            {
                FitCircircle.visionFitCircleParam.startAngledeg = textBox_TestStartAngle.Text.ToDouble();
                FitCircircle.visionFitCircleParam.endAngleDeg   = textBox_TestEndAngle.Text.ToDouble();
                FitCircircle.visionFitCircleParam.nTestNum = textBox_TestNum.Text.ToInt();
                FitCircircle.visionFitCircleParam.thresoldVal = textBox_TestThreasoldVal.Text.ToDouble();
                FitCircircle.visionFitCircleParam.nLen1=textBox_Len1.Text.ToInt();
                FitCircircle.visionFitCircleParam.nLen2=textBox_Len2.Text.ToInt();
                FitCircircle.visionFitCircleParam.testDir= (TestDir)Enum.Parse(typeof(TestDir), comboBox_TestDir.Text);
                FitCircircle.visionFitCircleParam.testPolarity=(TestPolarity)Enum.Parse(typeof(TestPolarity), comboBox_TestPolarity.Text);
                FitCircircle.visionFitCircleParam.testPos = (TestPos)Enum.Parse(typeof(TestPos), comboBox_TestPos.Text);

                FitCircircle = (VisionFitCircircle)visionSetp;

                if (strPath == "")
                    FitCircircle.Save();
                else
                    FitCircircle.Save(strPath);

            }
        }
        VisionFitCircircle  FitCircircle = null;
       
        public string strPath { set; get; } = "";
        private void btnCreateCircle_Click(object sender, EventArgs e)
        {
            //   m_visionControl?.DrawRectangle
            HTuple y = 0;  HTuple x = 0; HTuple raduis=0;
           HObject hObject= m_visionControl?.DrawCircle(out  y, out  x, out  raduis);
            if (hObject != null)
            {
                FitCircircle.visionFitCircleParam.point2DRoixy.y = y[0].D ;
                FitCircircle.visionFitCircleParam.point2DRoixy.x = x[0].D;
                FitCircircle.visionFitCircleParam.RadiusRoi = raduis[0].D;
                if(strPath=="")
                    FitCircircle.Save();
                else
                    FitCircircle.Save(strPath);
            }
          
        }
    }
}
