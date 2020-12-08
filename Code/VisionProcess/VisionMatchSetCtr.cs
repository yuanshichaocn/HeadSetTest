using BaseDll;
using HalconDotNet;
using System;
using System.Drawing;
using System.Windows.Forms;
using UserCtrl;

namespace VisionProcess
{
    public partial class VisionMatchSetCtr : VisionBaseCtr
    {
        public VisionMatchSetCtr()
        {
            InitializeComponent();
        }

        public override void FlushToDlg(VisionSetpBase visionSetp, VisionControl visionControl, Control Farther = null)
        {
            if (visionSetp == null)
                return;
            //if (Farther != null)
            //    this.Location = new Point(Farther.Location.X, Farther.Location.Y);
            m_visionShapMatch = visionSetp as VisionShapMatch;
            if (m_visionShapMatch == null)
                return;
            m_visionShapMatch = (VisionShapMatch)visionSetp;
            m_visionControl = visionControl;
            if (Farther != null)
                this.Location = new Point(Farther.Location.X, Farther.Location.Y);

            textBox_StartAngle.Text = m_visionShapMatch.visionShapParam.AngleStart.ToString();
            textBox_AngleExtent.Text = m_visionShapMatch.visionShapParam.AngleExtent.ToString();
            textBox_ScaleColMin.Text = m_visionShapMatch.visionShapParam.MinColScale.ToString();
            textBox_ScaleColMax.Text = m_visionShapMatch.visionShapParam.MaxColScale.ToString();

            textBox_MatchNum.Text = m_visionShapMatch.visionShapParam.nNum.ToString();
            textBox_MatchScore.Text = m_visionShapMatch.visionShapParam.dSorce.ToString();
            textBox_OverLapArea.Text = m_visionShapMatch.visionShapParam.MaxOverlap.ToString();
            textBox_PyramidHierarchy.Text = m_visionShapMatch.visionShapParam.CratePyramid.ToString();
            textBox_PyramidHierarchyHigh.Text = m_visionShapMatch.visionShapParam.MatchPyamidHigh.ToString();
            textBox_PyramidHierarchyLow.Text = m_visionShapMatch.visionShapParam.MatchPyamidLow.ToString();

            textBox_ContrastHigh.Text = m_visionShapMatch.visionShapParam.ContrastHigh.ToString();
            textBox_ContrastLow.Text = m_visionShapMatch.visionShapParam.ContrastLow.ToString();
            textBox_MinSize.Text = m_visionShapMatch.visionShapParam.MinSize.ToString();
            roundButton_SetOutPoint.Enabled = m_visionShapMatch.visionShapParam.bSetOutPoint;
            roundButton_SetOutPoint.Enabled = m_visionShapMatch.visionShapParam.bSetOutPoint;
            comboSelPolarity.Text = m_visionShapMatch.visionShapParam.strPolaritySel;
            comBoSelMatch.Text = m_visionShapMatch.visionShapParam.ModeType.ToString();
            regionOut1.Flush(visionControl, m_visionShapMatch.shapeslist);
            this.Show();
        }

        public override void SaveParm(VisionSetpBase visionSetp)
        {
            if (visionSetp == null) return;
            m_visionShapMatch = (VisionShapMatch)visionSetp;

            m_visionShapMatch.visionShapParam.AngleStart = textBox_StartAngle.Text.ToDouble();
            m_visionShapMatch.visionShapParam.AngleExtent = textBox_AngleExtent.Text.ToDouble();
            m_visionShapMatch.visionShapParam.MinColScale = textBox_ScaleColMin.Text.ToDouble();
            m_visionShapMatch.visionShapParam.MaxColScale = textBox_ScaleColMax.Text.ToDouble();
            m_visionShapMatch.visionShapParam.nNum = textBox_MatchNum.Text.ToInt();
            m_visionShapMatch.visionShapParam.dSorce = textBox_MatchScore.Text.ToDouble();
            m_visionShapMatch.visionShapParam.MaxOverlap = textBox_OverLapArea.Text.ToDouble();
            m_visionShapMatch.visionShapParam.ContrastHigh = textBox_ContrastHigh.Text.ToDouble();
            m_visionShapMatch.visionShapParam.ContrastLow = textBox_ContrastLow.Text.ToDouble();
            m_visionShapMatch.visionShapParam.MinSize = textBox_MinSize.Text.ToDouble();
            m_visionShapMatch.visionShapParam.CratePyramid = textBox_PyramidHierarchy.Text.ToInt();
            m_visionShapMatch.visionShapParam.MatchPyamidHigh = textBox_PyramidHierarchyHigh.Text.ToInt();
            m_visionShapMatch.visionShapParam.MatchPyamidLow = textBox_PyramidHierarchyLow.Text.ToInt();
            m_visionShapMatch.visionShapParam.strPolaritySel = comboSelPolarity.Text == null ? "use_polarity" : comboSelPolarity.Text;
            m_visionShapMatch.visionShapParam.ModeType = (eModleType)Enum.Parse(typeof(eModleType), comBoSelMatch.Text);
            if (strPath == "")
                m_visionShapMatch.Save();
            else
                m_visionShapMatch.Save(strPath);
        }

        private void MatchParam_Click(object sender, EventArgs e)
        {
        }

        public HObject m_imgObj = null;

        private VisionShapMatch m_visionShapMatch = null;
        public string strPath { set; get; } = "";

        private void roundButton_CreateMode_Click(object sender, EventArgs e)
        {
            string str = "";
            if (m_visionShapMatch == null)
                return;
            if (m_visionControl == null)
                return;
            if (m_imgObj != null && m_imgObj.IsInitialized())
                m_imgObj.Dispose();
            HOperatorSet.CopyImage(m_visionControl.Img, out m_imgObj);
            if (m_imgObj == null || !m_imgObj.IsInitialized())
            {
                MessageBox.Show(m_visionShapMatch.m_strStepName + "创建模板时， 没有图片，请先读取图片", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveParm(m_visionShapMatch);
            if (strPath == "")
                m_visionShapMatch.Save();
            else
                m_visionShapMatch.Save(strPath);
            if (strPath == "")
                m_visionShapMatch.GenObj(m_imgObj, m_visionControl);
            else
                m_visionShapMatch.GenObj(m_imgObj, strPath, m_visionControl);
        }

        private void roundButton_SetOutPoint_Click(object sender, EventArgs e)
        {
            HTuple row = null, col = null;
            m_visionControl?.DrawPoint(out row, out col);
            if (row != null)
            {
                m_visionShapMatch.visionShapParam.bSetOutPoint = true;
                m_visionShapMatch.visionShapParam.OutPointInModleImage = new Point2d(col.D, row.D);
                if (strPath == "")
                    m_visionShapMatch.Save();
                else
                    m_visionShapMatch.Save(strPath);
            }
        }

        private void checkBox_SetOutPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SetOutPoint.Checked)
            {
                roundButton_SetOutPoint.Enabled = true;
            }
            else
            {
                m_visionShapMatch.visionShapParam.bSetOutPoint = false;
                roundButton_SetOutPoint.Enabled = false;
            }
        }
    }
}