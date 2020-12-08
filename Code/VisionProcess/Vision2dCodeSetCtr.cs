using HalconDotNet;
using System;
using System.Windows.Forms;
using UserCtrl;

namespace VisionProcess
{
    public partial class Vision2dCodeSetCtr : VisionBaseCtr
    {
        public Vision2dCodeSetCtr()
        {
            InitializeComponent();
        }

        public HObject m_imgObj = null;

        private Vision2dCode m_vision2dCode = null;
        public string strPath { set; get; } = "";

        private void roundButton_Create2dCode_Click(object sender, EventArgs e)
        {
            if (m_vision2dCode == null)
                return;
            if (m_visionControl == null)
                return;
            if (m_imgObj != null && m_imgObj.IsInitialized())
                m_imgObj.Dispose();
            HOperatorSet.CopyImage(m_visionControl.Img, out m_imgObj);
            if (m_imgObj == null || !m_imgObj.IsInitialized())
            {
                MessageBox.Show(m_vision2dCode.m_strStepName + "创建2code模板时， 没有图片，请先读取图片", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            SaveParm(m_vision2dCode);
            if (strPath == "")
                m_vision2dCode.Save();
            else
                m_vision2dCode.Save(strPath);
            m_vision2dCode.GenObj(m_imgObj, m_visionControl);
        }

        public override void FlushToDlg(VisionSetpBase visionSetp, VisionControl visionControl, Control Farther = null)
        {
            if (visionSetp == null)
                return;
            //if (Farther != null)
            //    this.Location = new Point(Farther.Location.X, Farther.Location.Y);
            m_visionControl = visionControl;
            m_vision2dCode = visionSetp as Vision2dCode;
            if (m_vision2dCode == null)
                return;
            m_vision2dCode = (Vision2dCode)visionSetp;
            if (m_vision2dCode == null)
            {
                MessageBox.Show("视觉类型为空 刷新失败，请选择", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_vision2dCode != null)
            {
                comboBox_CodeSystem.Text = m_vision2dCode.vision2dCodeParam.Code2dSystem;
                comboBox_ContrastTolerance.Text = m_vision2dCode.vision2dCodeParam.ContrastTolerance;
            }
        }

        public override void SaveParm(VisionSetpBase visionSetp)
        {
            if (m_vision2dCode == null)
            {
                MessageBox.Show("视觉类型为空 保存失败，请选择", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m_vision2dCode.vision2dCodeParam.Code2dSystem = comboBox_CodeSystem.Text;
            m_vision2dCode.vision2dCodeParam.ContrastTolerance = comboBox_ContrastTolerance.Text;
            if (strPath == "")
                m_vision2dCode.Save();
            else
                m_vision2dCode.Save(strPath);
        }

        private void Vision2dCodeSetCtr_Load(object sender, EventArgs e)
        {
        }
    }
}