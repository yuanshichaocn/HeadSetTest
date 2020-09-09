using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using UserCtrl;

namespace VisionProcess
{
    public partial class Vision1BarCodeSetCtr : VisionBaseCtr
    {
        public Vision1BarCodeSetCtr()
        {
            InitializeComponent();
        }
        public override void FlushToDlg(VisionSetpBase visionSetp, VisionControl visionControl, Control Farther = null)
        {
            if (visionSetp == null)
                return;
            m_visionControl = visionControl;
            //if(Farther!=null)
            //   this.Location = new Point(Farther.Location.X, Farther.Location.Y);
            m_vision1dCode = visionSetp as Vision1dCode;
            if (m_vision1dCode == null)
                return;
            m_vision1dCode = (Vision1dCode)visionSetp;
           
            //  comboBox_CodeSystem.Text = m_vision1dCode.vision2dCodeParam.Code2dSystem;
            //  comboBox_ContrastTolerance.Text = m_vision1dCode.vision2dCodeParam.ContrastTolerance;
        }

        public override void SaveParm(VisionSetpBase visionSetp)
        {
            //  m_vision1dCode.vision1dCodeParam.Code2dSystem = comboBox_CodeSystem.Text;
            //  m_vision1dCode.vision1dCodeParam.ContrastTolerance = comboBox_ContrastTolerance.Text;
            if (strPath == "")
                m_vision1dCode?.Save();
            else
                m_vision1dCode?.Save(strPath);
            
        }
        public HObject m_imgObj = null;
       
        Vision1dCode m_vision1dCode = null;
        public string strPath { set; get; } = "";
        private void roundButton_Create_Click(object sender, EventArgs e)
        {
            if (m_vision1dCode == null)
                return;
            if (m_visionControl == null)
                return;
            if (m_imgObj != null && m_imgObj.IsInitialized())
                m_imgObj.Dispose();
            HOperatorSet.CopyImage(m_visionControl.Img, out m_imgObj);
            if (m_imgObj == null || !m_imgObj.IsInitialized())
            {
                MessageBox.Show(m_vision1dCode.m_strStepName + "创建1code模板时， 没有图片，请先读取图片", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveParm(m_vision1dCode);
            if(strPath=="")
               m_vision1dCode?.Save();
            else
                m_vision1dCode?.Save(strPath);
            m_vision1dCode.GenObj(m_imgObj, m_visionControl);
        }

        private void Vision1BarCodeSetCtr_Load(object sender, EventArgs e)
        {

        }
    }
}
