using CameraLib;
using HalconDotNet;
using System;
using System.Windows.Forms;

namespace XYZDispensVision
{
    public partial class DispTraceElementSet : Form
    {
        public DispTraceElementSet()
        {
            InitializeComponent();
        }

        private void ReadImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;//注意这里写路径时要用c:\\而不是c:\
                openFileDialog.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "打开图片";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //string  fName = openFileDialog.FileName;
                    string strPath = openFileDialog.FileName;
                    HObject img = null;
                    HOperatorSet.ReadImage(out img, strPath);
                    visionControl1.DispImageFull(img);
                }
            }
        }

        private void BtnSanp_Click(object sender, EventArgs e)
        {
            string strCamName = "Top";
            CameraBase cameraBase = CameraMgr.GetInstance().GetCamera(strCamName);
            cameraBase.StopGrap();
            cameraBase.BindWindow(visionControl1);
            cameraBase.SetTriggerMode(CameraModeType.Software);
            CameraMgr.GetInstance().GetCamera(strCamName).StartGrab();
            CameraMgr.GetInstance().GetCamera(strCamName).GarbBySoftTrigger();
        }

        private void btnSnapSave_Click(object sender, EventArgs e)
        {
            string strCamName = "Top";
            CameraBase cameraBase = CameraMgr.GetInstance().GetCamera(strCamName);
            cameraBase.StopGrap();
            cameraBase.BindWindow(visionControl1);
            cameraBase.SetTriggerMode(CameraModeType.Software);
            CameraMgr.GetInstance().GetCamera(strCamName).StartGrab();

            CameraMgr.GetInstance().SaveImg(strCamName);
            CameraMgr.GetInstance().GetCamera(strCamName).GarbBySoftTrigger();
        }

        private void ContinuousSnap_Click(object sender, EventArgs e)
        {
            string strCamName = "Top";
            CameraBase cameraBase = CameraMgr.GetInstance().GetCamera(strCamName);
            cameraBase.StopGrap();
            cameraBase.SetTriggerMode(CameraModeType.Software);

            double valexposure = cameraBase.GetExposureTime();
            double valgain = cameraBase.GetGain();
            cameraBase.BindWindow(visionControl1);
            cameraBase.SetAcquisitionMode();
            cameraBase.StartGrab();
        }

        private void Show(string NameOfCtrl)
        {
        }

        private DispTraceBaseElement baseElement = null;
        private InterfaceDispTraceElementModeOnDebugDlg ctrSel = null;

        public void UpdateData(DispTraceBaseElement dispTraceBaseElement, bool bShow = false)
        {
            if (dispTraceBaseElement == null)
                return;
            baseElement = dispTraceBaseElement;

            foreach (Control tem in panelForElementCtrl.Controls)
                tem.Visible = false;
            if (dispTraceBaseElement.strType.Contains("Line"))
            {
                traceElementLine.Visible = true;
                ctrSel = traceElementLine;
            }
            if (dispTraceBaseElement.strType.Contains("Point"))
            {
                traceElementPoint.Visible = true;
                ctrSel = traceElementPoint;
            }
            if (dispTraceBaseElement.strType.Contains("Arc"))
            {
                traceElemementArc.Visible = true;
                ctrSel = traceElemementArc;
            }
            ctrSel.FlushToDlg(baseElement, this.visionControl1, bShow);

            bModify = bShow;
            this.TopLevel = true;
            this.Show();
            //if(visionControl1!=null && visionControl1.isOpen() && visionControl1.Img!=null && visionControl1.Img.IsInitialized())
            //{
            //    visionControl1.DispImageFull(visionControl1.Img);
            //}
            if (bShow)
                ctrSel.ShowObj();
            this.Focus();
        }

        private bool bModify = false;

        private void DispTraceElementSet_Load(object sender, EventArgs e)
        {
            visionControl1.InitWindow();
        }

        private void SaveClose_Click(object sender, EventArgs e)
        {
            if (ctrSel != null)
                ctrSel.SaveParm();
            if (baseElement != null)
            {
                if (baseElement.ItemName == "")
                {
                    MessageBox.Show($"项目无名 检查后重新输入 该项名称", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (bModify)
                {
                    //修改
                }
                else
                {
                    //新增加
                    if (DispTraceMgr.GetInstance().CheckReName(baseElement.ItemName))
                    {
                        MessageBox.Show($"{baseElement.ItemName}项目 重名了 检查后重新输入 该项名称", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    DispTraceMgr.GetInstance().AddItemToList(baseElement);
                }
            }
            this.Hide();
        }

        private void DispTraceElementSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}