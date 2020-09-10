using BaseDll;
using EpsonRobot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserData;
using VisionProcess;
using HalconDotNet;
using CommonTools;
using UserCtrl;
using CameraLib;
using System.IO;
using System.Diagnostics;
using System.Threading;
using MotionIoLib;
using System.Threading.Tasks;

using System.Reflection;
using OtherDevice;

using CommonDlg;
using MachineSafe;

namespace StationDemo
{
    public partial class Form_Manual : Form
    {
        public Form_Manual()
        {
            InitializeComponent();
        }
        public void ChagedPrItem(string name)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => { ChagedPrItem(name); }));
            }
            else
            {
                comboBox_SelVisionPR.Items.Clear();
                foreach (var tem in VisionMgr.GetInstance().GetItemNamesAndTypes())
                {
                    comboBox_SelVisionPR.Items.Add(tem.Key);
                }
            }
        }

        private void Form_Manual_Load(object sender, EventArgs e)
        {
            visionControl1.InitWindow();
            Thread.Sleep(10);
            List<string> camname = CameraMgr.GetInstance().GetCameraNameArr();
            foreach (var temp in camname)
                comboBox_SelCamera.Items.Add(temp.ToString());
            if (camname != null && camname.Count > 0)
            {
                comboBox_SelCamera.Visible = true;
                comboBox_SelCamera.SelectedIndex = 0;
            }
            else
            {
                visionControl1.Visible = false;
                button_ContinuousSnap.Visible = false;
                button2.Visible = false;
                comboBox_SelVisionPR.Visible = false;
                roundButton_VisionPrTest.Visible = false;
            }
            VisionMgr.GetInstance().PrItemChangedEvent += ChagedPrItem;
            ChagedPrItem("");

 
        }

       
      


     
     



      

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {

               // IOMgr.GetInstace().m_eventIoOutputChanageByName += ChangedIoOutState;
            }
            else
            {

               // IOMgr.GetInstace().m_eventIoOutputChanageByName -= ChangedIoOutState;
            }
        }

        private void comboBox_SelCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);
        }

        private void button_ContinuousSnap_Click(object sender, EventArgs e)
        {
            if (comboBox_SelCamera.SelectedItem == null)
                return;
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraBase pCamer = CameraMgr.GetInstance().GetCamera(comboBox_SelCamera.SelectedItem.ToString());
            pCamer.SetTriggerMode(CameraModeType.Software);
            Thread.Sleep(100);
            CameraMgr.GetInstance().GetCamera(comboBox_SelCamera.SelectedItem.ToString()).SetAcquisitionMode();
            // CameraMgr.GetInstance().GetCamera(comboBox_SelCamera.SelectedItem.ToString()).wnd = hWindowControl1.HalconID;
            CameraMgr.GetInstance().GetCamera(comboBox_SelCamera.SelectedItem.ToString()).StartGrab();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox_SelCamera.SelectedItem == null)
                return;
            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().GetCamera(comboBox_SelCamera.SelectedItem.ToString()).SetTriggerMode(CameraModeType.Software);
            Thread.Sleep(100);
            //CameraMgr.GetInstance().GetCamera(comboBox_SelCamera.SelectedItem.ToString()).wnd = hWindowControl1.HalconID;
            CameraMgr.GetInstance().GetCamera(comboBox_SelCamera.SelectedItem.ToString()).StartGrab();
            CameraMgr.GetInstance().GetCamera(comboBox_SelCamera.SelectedItem.ToString()).GarbBySoftTrigger();
        }

        private void roundButton_VisionPrTest_Click(object sender, EventArgs e)
        {
            if (comboBox_SelVisionPR.SelectedIndex == -1)
                return;
            string strVisionPrName = comboBox_SelVisionPR.Items[comboBox_SelVisionPR.SelectedIndex].ToString();
            roundButton_VisionPrTest.Enabled = false;
            Action action = new Action(() =>
            {
                string camname = VisionMgr.GetInstance().GetCamName(strVisionPrName);
                double? Expouse = VisionMgr.GetInstance().GetExpourseTime(strVisionPrName);
                double? Gain = VisionMgr.GetInstance().GetGain(strVisionPrName);

                CameraMgr.GetInstance().SetCamExposure(camname, (double)Expouse);
                CameraMgr.GetInstance().SetCamGain(camname, (double)Gain);
                CameraMgr.GetInstance().BindWindow(camname, visionControl1);
                CameraMgr.GetInstance().ClaerPr(camname);
                CameraMgr.GetInstance().GetCamera(camname).SetTriggerMode(CameraModeType.Software);

 
            }
            );

            action.BeginInvoke((ar) =>
            {
                roundButton_VisionPrTest.Invoke((MethodInvoker)(() => { roundButton_VisionPrTest.Enabled = true; }), null);
            }, null);
        }

        private void btnUnLockBtns_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(" 按钮将会解锁，正在运动的操作将会停止 是否继续", "Err", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (dialogResult == DialogResult.Yes)
            {
                DoWhile.StopCirculate();

            }

        }

        public void BtnEnable(bool benable = true)
        {
     
        }

     

       

   
    }
}
