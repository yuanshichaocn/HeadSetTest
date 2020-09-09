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
using LightControler;
using System.Reflection;
using OtherDevice;
using CmdEdit;

namespace StationDemo
{
    public partial class Form_Manual : Form
    {
        public Form_Manual()
        {
            InitializeComponent();
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

            KeyneceVisionProcessor.GetInstance().evenShowMsg += new KeyneceVisionProcessor.showMsg(listBostShow);
            //delegate (string str)
            //{
            //    listBox1.Items.Add(str);
            //    return true;
            //};


            //LeftCly.Name = "机械手1号气缸气缸电磁阀";
            //Vac.Name = "机械手吸料真空1";
            //vacBreak.Name = "机械手吸料破真空1";
            //Vac.m_eventClick += OutIoWhenClickBtn;
            //vacBreak.m_eventClick+= OutIoWhenClickBtn;
            //LeftCly.m_eventClick += OutIoWhenClickBtn;
        }

        bool listBostShow(string str)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => { listBostShow(str); }));
            }
            else
            {
                listBox1.Items.Add(str);
                listBox1.TopIndex = listBox1.Items.Count - (int)(listBox1.Height / listBox1.ItemHeight);

                //listBox1.SelectedIndex = listBox1.Items.Count-1;
            }
            return true;
        }
        public void OutIoWhenClickBtn(string str)
        {
            bool bState = IOMgr.GetInstace().ReadIoOutBit(str);
            IOMgr.GetInstace().WriteIoBit(str, !bState);
        }
        public void ChangedIoOutState(string IoName, bool bStateCurrent)
        {
            int nRow = 0;
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ChangedIoOutState(IoName, bStateCurrent)));
            }
            else
            {


            }

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




        bool pr(string strVisionPrName, VisionControl visionControl1)
        {

            object objresults = VisionMgr.GetInstance().GetResult(strVisionPrName);
            IOperateParam visionShapParams = (IOperateParam)objresults;
            visionShapParams.ClearResult();
            string camname = VisionMgr.GetInstance().GetCamName(strVisionPrName);
            CameraMgr.GetInstance().BindWindow(camname, visionControl1);
            CameraMgr.GetInstance().ClaerPr(camname);

            CameraMgr.GetInstance().GetCamera(camname).SetTriggerMode(CameraModeType.Software);


            double? Expouse = VisionMgr.GetInstance().GetExpourseTime(strVisionPrName);
            double? Gain = VisionMgr.GetInstance().GetGain(strVisionPrName);

            CameraMgr.GetInstance().SetCamExposure(camname, (double)Expouse);
            CameraMgr.GetInstance().SetCamGain(camname, (double)Gain);
            CameraMgr.GetInstance().GetCamera(camname).GarbBySoftTrigger();
            HObject img = CameraMgr.GetInstance().GetCamera(camname).GetImage();
            VisionMgr.GetInstance().ProcessImage(strVisionPrName, img, visionControl1);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            bool bfail = false;
            while (true)
            {
                object objresult = VisionMgr.GetInstance().GetResult(strVisionPrName);
                if (objresult != null)
                {
                    IOperateParam visionShapParam = (IOperateParam)objresult;
                    if (visionShapParam.GetResultNum() > 0)
                    {
                        img.Dispose();
                        return true;
                    }
                }
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    bfail = true;
                    img.Dispose();
                    return false;
                }

            }
            if (bfail)
                return false;
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {

                IOMgr.GetInstace().m_eventIoOutputChanageByName += ChangedIoOutState;
            }
            else
            {

                IOMgr.GetInstace().m_eventIoOutputChanageByName -= ChangedIoOutState;
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

                // CameraMgr.GetInstance().AddPr(camname, strVisionPrName);
                //   CameraMgr.GetInstance().GetCamera(camname).GarbBySoftTrigger(); 
                pr(strVisionPrName, visionControl1);
            }
            );

            action.BeginInvoke((ar) =>
            {
                roundButton_VisionPrTest.Invoke((MethodInvoker)(() => { roundButton_VisionPrTest.Enabled = true; }), null);
            }, null);
        }




        private void BtnStop_Click(object sender, EventArgs e)
        {
            DoWhile.StopCirculate();
        }






        int RightYZTeestCout = 0;


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
            btnCalib.Enabled = benable;
            btnNinePointCalbUpCam.Enabled = benable;
            btnCalib.Enabled = benable;
            comboSelUpCam.Enabled = benable;
            comboBox_DownCCDSelItem.Enabled = benable;
            comboSelNozzle.Enabled = benable;
            BtnTestHigh.Enabled = benable;
        }

        private async void BtnUpDwon_Click(object sender, EventArgs e)
        {
            DoWhile.ResetCirculate();

            string UpCCDName = "";
            string DwonCCDName = "";
            string strExp = "";
            string strE = "";
            int nCh = 0;
            int nVal = 0;
            if (comboSelUpCCD.SelectedIndex > -1)
            {

                UpCCDName = comboSelUpCCD.SelectedIndex == 0 ? "Left" : "Right";
            }
            else
            {
                MessageBox.Show(" 请选择 左右相机", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboSelDownCCD.SelectedIndex > -1)
            {
                DwonCCDName = comboSelDownCCD.SelectedIndex == 0 ? "Front" : "Back";
            }
            else
            {
                MessageBox.Show(" 请选择 前后相机", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{UpCCDName}_{DwonCCDName}"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{UpCCDName}_{DwonCCDName}");
            StationAssemble station = (StationAssemble)StationMgr.GetInstance().GetStation("组立站");
            if (MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisX) != AxisHomeFinishFlag.Homed ||
               MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisY) != AxisHomeFinishFlag.Homed ||
               MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisZ) != AxisHomeFinishFlag.Homed)
            {
                MessageBox.Show(" 组立站没回原点", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            bool bmanual = true;
        retry_cylider_up:
            for (int i = 1; i < 16; i++)
                IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));

            Cmdbase.GoZSafePos(true);

            WaranResult waranResult;
            string strPointName = DwonCCDName == "Front" ? "前上下相机标定位" : "后上下相机标定位";
            Point2d pointUpDownCamCalibPos = new Point2d(station.GetStationPointDic()[strPointName].pointX,
              station.GetStationPointDic()[strPointName].pointY);
            double z = station.GetStationPointDic()[strPointName].pointZ;
            HObject img = null;
            Task task = Task.Run(() =>
            {
                try
                {
                    int nAxisX = station.AxisX;
                    int nAxisY = station.AxisY;
                    int nAxisZ = station.AxisZ;
                    List<double> VrowU = new List<double>();
                    List<double> VcolU = new List<double>();
                    List<double> VrowD = new List<double>();
                    List<double> VcolD = new List<double>();
                    nCh = UpCCDName == "Left" ? 1 : 2;
                    nVal = UpCCDName == "Left" ? ParamSetMgr.GetInstance().GetIntParam("左上下标定光源") : ParamSetMgr.GetInstance().GetIntParam("右上下标定光源");

                    OtherDevices.lightControler.Light(nCh, nVal);
                    OtherDevices.lightControler.Light(3, 0);
                    waranResult = station.MoveMulitAxisPosWaitInpos(
                       new int[] { nAxisX, nAxisY, nAxisZ },
                       new double[] { pointUpDownCamCalibPos.x, pointUpDownCamCalibPos.y, z },
                       new double[] { (double)SpeedType.Low, (double)SpeedType.Low, (double)SpeedType.Low },
                       0.005, true, null, 50000);
                    if (waranResult == WaranResult.Alarm || waranResult == WaranResult.TimeOut || waranResult == WaranResult.Stop)
                    {
                        MessageBox.Show("运动到" + "上下相机标定位" + "电机异常或者超时", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    CameraMgr.GetInstance().BindWindow(UpCCDName, visionControl1);
                    CameraMgr.GetInstance().GetCamera(UpCCDName).SetTriggerMode(CameraModeType.Software);
                    CameraMgr.GetInstance().GetCamera(UpCCDName).StartGrab();
                    //int exp = ParamSetMgr.GetInstance().GetIntParam($"上下标定上"+"曝光");
                    //int gain = ParamSetMgr.GetInstance().GetIntParam($"上下标定上"+"增益");
                    //CameraMgr.GetInstance().GetCamera(UpCCDName).SetExposureTime((double)exp);
                    //CameraMgr.GetInstance().GetCamera(UpCCDName).SetGain((double)gain);

                    img = CameraMgr.GetInstance().GetCamera(UpCCDName).GetImage();
                    if (img != null && img.IsInitialized())
                        HOperatorSet.WriteImage(img, "bmp", 0, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{UpCCDName}_{DwonCCDName}\\" + $"{UpCCDName}_{DwonCCDName}_{UpCCDName}.bmp");
                    else
                    {
                        MessageBox.Show("右边上相机标定失败");
                        return;
                    }
                    OtherDevices.lightControler.Light(nCh, 0);
                    if (!VisionAddtion.CalibUpDown(UpCCDName, 1595.71, 1923, -0.0, 1798.0, 1207.5, 128, 255, "row", "false", TestPolarity.从暗到明,
                    img, visionControl1, VrowU, VcolU))
                    {
                        MessageBox.Show("右边上相机标定失败");
                        return;
                    }
                    img?.Dispose();
                    MessageBox.Show("右边上相机拍照结束");

                    nCh = DwonCCDName == "Front" ? 1 : 2;
                    nVal = DwonCCDName == "Front" ? ParamSetMgr.GetInstance().GetIntParam("前上下标定光源") : ParamSetMgr.GetInstance().GetIntParam("后上下标定光源");

                    OtherDevices.lightControler.Light(nCh, nVal);
                    OtherDevices.lightControler.Light(3, 0);
                    Thread.Sleep(100);
                    //  exp = ParamSetMgr.GetInstance().GetIntParam($"上下标定下" + "曝光");
                    //  gain = ParamSetMgr.GetInstance().GetIntParam($"上下标定下" + "增益");
                    CameraMgr.GetInstance().BindWindow(DwonCCDName, visionControl1);
                    //CameraMgr.GetInstance().GetCamera(DwonCCDName).SetTriggerMode(CameraModeType.Software);
                    //CameraMgr.GetInstance().GetCamera(DwonCCDName).SetExposureTime((double)exp);
                    //CameraMgr.GetInstance().GetCamera(DwonCCDName).SetGain((double)gain);
                    CameraMgr.GetInstance().GetCamera(DwonCCDName).StartGrab();
                    img = CameraMgr.GetInstance().GetCamera(DwonCCDName).GetImage();
                    if (img != null && img.IsInitialized())
                        HOperatorSet.WriteImage(img, "bmp", 0, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{UpCCDName}_{DwonCCDName}\\" + $"{UpCCDName}_{DwonCCDName}_{DwonCCDName}.bmp");
                    else
                    {
                        MessageBox.Show("右边下相机标定失败");
                        return;
                    }

                    if (!VisionAddtion.CalibUpDown(DwonCCDName, 1356.55, 2013.01, 0, 1818, 1207, 0, 128, "row", "true", TestPolarity.从明到暗,
                     img, visionControl1, VrowD, VcolD))
                    {
                        MessageBox.Show("右边下相机标定失败");
                        return;
                    }
                    img?.Dispose();
                    OtherDevices.lightControler.Light(nCh, 0);
                    MessageBox.Show("右边上下相机拍照结束");


                    HTuple hTupleUR = new HTuple(), hTupleUC = new HTuple(), hTupleDR = new HTuple(), hTupleDC = new HTuple();
                    for (int I = 0; I < VrowU.Count; I++)
                    {
                        hTupleUR.Append(VrowU[I]);
                        hTupleUC.Append(VcolU[I]);
                        hTupleDR.Append(VrowD[I]);
                        hTupleDC.Append(VcolD[I]);
                    }
                    HOperatorSet.WriteTuple(hTupleUR, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{UpCCDName}_{DwonCCDName}\\" + "VrowU.tup");
                    HOperatorSet.WriteTuple(hTupleUC, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{UpCCDName}_{DwonCCDName}\\" + "VcolU.tup");
                    HOperatorSet.WriteTuple(hTupleDR, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{UpCCDName}_{DwonCCDName}\\" + "VrowD.tup");
                    HOperatorSet.WriteTuple(hTupleDC, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{UpCCDName}_{DwonCCDName}\\" + "VcolD.tup");
                    if (UpCCDName == "Left" && DwonCCDName == "Front")
                        //VisionCalib.LeftFrontCalib.CreateUpDownCameraCoor(VcolU.ToArray(), VrowU.ToArray(), VcolD.ToArray(), VrowD.ToArray());
                        VisionCalib.XYFrontCalib.CreateUpDownCameraCoor(VcolU.ToArray(), VrowU.ToArray(), VcolD.ToArray(), VrowD.ToArray());
                    if (UpCCDName == "Right" && DwonCCDName == "Back")
                        // VisionCalib.RightBackCalib.CreateUpDownCameraCoor(VcolU.ToArray(), VrowU.ToArray(), VcolD.ToArray(), VrowD.ToArray());
                        VisionCalib.XYBackCalib.CreateUpDownCameraCoor(VcolU.ToArray(), VrowU.ToArray(), VcolD.ToArray(), VrowD.ToArray());

                }
                catch (Exception ex)
                {


                    MotionMgr.GetInstace().StopAxis(station.AxisX);
                    MotionMgr.GetInstace().StopAxis(station.AxisY);
                    MotionMgr.GetInstace().StopAxis(station.AxisZ);
                    int s = 0;
                    MessageBox.Show("右边上下相机标定失败" + ex.Message);
                }

            });
            await task;
            OtherDevices.lightControler.Light(nCh, 0);

        }

        private async void btnCalib_Click(object sender, EventArgs e)
        {
            DoWhile.ResetCirculate();

            string strCCDName = "";
            if (comboBox_DownCCDSelItem.SelectedIndex > -1)
                strCCDName = comboBox_DownCCDSelItem.SelectedIndex == 0 ? "Front" : "Back";
            else
                return;
            if (strCCDName == "")
            {
                MessageBox.Show(" 请选择相机", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string strLightItem = strCCDName == "Front" ? "前机械标定光源" : "后机械标定光源";
            int nCh = strCCDName == "Front" ? 3 : 4;
            int nVal = ParamSetMgr.GetInstance().GetIntParam(strLightItem);
            StationAssemble station = (StationAssemble)StationMgr.GetInstance().GetStation("组立站");
            if (MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisX) != AxisHomeFinishFlag.Homed ||
               MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisY) != AxisHomeFinishFlag.Homed ||
               MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisZ) != AxisHomeFinishFlag.Homed)
            {
                MessageBox.Show(" 组立站 XYZ没回原点", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            bool bmanual = true;
            string strposNome = string.Format("吸嘴{0}标定点", 1);
            Point2d pointUpDownCamCalibPos = new Point2d(station.GetStationPointDic()[strposNome].pointX,
               station.GetStationPointDic()[strposNome].pointY);
            BtnEnable(false);
            double dx = ParamSetMgr.GetInstance().GetDoubleParam("前机械标定X步长");
            double dy = ParamSetMgr.GetInstance().GetDoubleParam("前机械标定Y步长");
            Point2d[] point2Ds = new Point2d[] {
                new Point2d( pointUpDownCamCalibPos.x- dx, pointUpDownCamCalibPos.y -dy),
                new Point2d( pointUpDownCamCalibPos.x- dx, pointUpDownCamCalibPos.y ),
                new Point2d( pointUpDownCamCalibPos.x- dx, pointUpDownCamCalibPos.y + dy),

                new Point2d( pointUpDownCamCalibPos.x, pointUpDownCamCalibPos.y - dy),
                new Point2d( pointUpDownCamCalibPos.x, pointUpDownCamCalibPos.y ),
                new Point2d( pointUpDownCamCalibPos.x,pointUpDownCamCalibPos.y+  dy),

                new Point2d( pointUpDownCamCalibPos.x+  dx, pointUpDownCamCalibPos.y -  dy),
                new Point2d( pointUpDownCamCalibPos.x+  dx, pointUpDownCamCalibPos.y),
                new Point2d( pointUpDownCamCalibPos.x+  dx,pointUpDownCamCalibPos.y + dy)
            };
            for (int i = 1; i < 16; i++)
                IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));
            Cmdbase.GoZSafePos(true);

            HObject Img = null;
            DoWhile.ResetCirculate();
            Task task = Task.Run(() =>
            {

                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}");
                try
                {
                    List<double> Vrow = new List<double>(); Vrow.Clear();
                    List<double> Vcol = new List<double>(); Vcol.Clear();

                    List<double> My = new List<double>(); My.Clear();
                    List<double> Mx = new List<double>(); Mx.Clear();




                    OtherDevices.lightControler.Light(nCh, nVal);
                    for (int j = 0; j < 9; j++)
                    {

                        IOMgr.GetInstace().WriteIoBit("吸嘴1下降", false);
                        Thread.Sleep(1000);
                        WaranResult waranResult = station.MoveMulitAxisPosWaitInpos(new int[] { station.AxisX, station.AxisY },
                        new double[] { point2Ds[j].x, point2Ds[j].y }, new double[] { (double)SpeedType.Low, (double)SpeedType.Low }, 2, true, null, 50000);
                        if (waranResult == WaranResult.Alarm || waranResult == WaranResult.TimeOut || waranResult == WaranResult.Stop)
                        {
                            MessageBox.Show("运动到" + strposNome + "电机异常或者超时", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        station.MoveMulitAxisPosWaitInpos(new int[] { station.AxisZ, },
                     new double[] { station.GetStationPointDic()[strposNome].pointZ }, new double[] { (double)SpeedType.Low }, 0.05, true, null, 50000);
                    retry_cylider_down:
                        IOMgr.GetInstace().WriteIoBit("吸嘴1下降", true);
                        waranResult = station.CheckIobyName("吸嘴1下降到位", true, $"下相机与机械手标定的过程中：吸嘴1下降到位失败，请检查气缸是否卡住，或者线路 ", bmanual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_cylider_down;

                        Thread.Sleep(2000);

                        CameraMgr.GetInstance().BindWindow(strCCDName, visionControl1);
                        CameraMgr.GetInstance().SetCamExposure(strCCDName, 20000);
                        CameraMgr.GetInstance().SetCamGain(strCCDName, 300);
                        CameraMgr.GetInstance().GetCamera(strCCDName).SetTriggerMode(CameraModeType.Software);
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Restart();

                        Img = CameraMgr.GetInstance().GetCamera(strCCDName).GetImage();
                        long time = stopwatch.ElapsedMilliseconds;
                        station.Info($"相机{strCCDName}与XYZ标定:采图花费的时间：  " + time.ToString());
                        if (!VisionAddtion.FindNozzle("右下相机标定吸嘴", Img, visionControl1, Vrow, Vcol))
                        {
                            waranResult = AlarmMgr.GetIntance().WarnWithDlg($"相机{strCCDName}与XYZ标定：本次标定失败，是否重试？", null, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                            if (waranResult == WaranResult.Retry)
                                goto retry_cylider_down;
                            if (waranResult == WaranResult.Stop)
                                throw new Exception($"相机{strCCDName}与XYZ标定停止");
                        }
                        HOperatorSet.WriteImage(Img, "bmp", 0, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\" + j.ToString() + ".bmp");

                        Mx.Add(MotionMgr.GetInstace().GetAxisPos(station.AxisX));
                        My.Add(MotionMgr.GetInstace().GetAxisPos(station.AxisY));
                    }

                    HTuple hTupleVrow = new HTuple();
                    HTuple hTupleVcol = new HTuple();
                    HTuple hTupleMx = new HTuple();
                    HTuple hTupleMy = new HTuple();
                    for (int i = 0; i < Vrow.Count; i++)
                    {
                        hTupleVrow.Append(Vrow[i]);
                        hTupleVcol.Append(Vcol[i]);
                    }
                    for (int i = 0; i < Vrow.Count; i++)
                    {
                        hTupleMx.Append(Mx[i]);
                        hTupleMy.Append(My[i]);
                    }
                    if (strCCDName == "Front")
                        VisionCalib.XYFrontCalib.CreateDownRobotCoor(Vcol.ToArray(), Vrow.ToArray(), Mx.ToArray(), My.ToArray());
                    if (strCCDName == "Back")
                        VisionCalib.XYBackCalib.CreateDownRobotCoor(Vcol.ToArray(), Vrow.ToArray(), Mx.ToArray(), My.ToArray());
                    HOperatorSet.WriteTuple(hTupleVrow, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\VM_VRow.tup");
                    HOperatorSet.WriteTuple(hTupleVcol, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\VM_VCol.tup");
                    HOperatorSet.WriteTuple(hTupleMx, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\VM_Mx.tup");
                    HOperatorSet.WriteTuple(hTupleMy, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\VM_My.tup");
                    IOMgr.GetInstace().WriteIoBit("吸嘴1下降", false);
                    MessageBox.Show($"相机{strCCDName}与XYZ标定：标定成功");
                }
                catch
                {
                    MotionMgr.GetInstace().StopAxis(station.AxisX);
                    MotionMgr.GetInstace().StopAxis(station.AxisY);
                    MotionMgr.GetInstace().StopAxis(station.AxisZ);


                }
                IOMgr.GetInstace().WriteIoBit("吸嘴1下降", false);
            });
            await task;
            Img?.Dispose();
            OtherDevices.lightControler.Light(nCh, 0);
            //MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenRightPackageXYAxisMoveing;
            BtnEnable(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboChanel.SelectedIndex == -1)
            {
                MessageBox.Show(" 请选择通道", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int nCh = comboChanel.SelectedIndex;
            int lightVal = txtTestLightVal.Text.ToInt();
            OtherDevices.lightControler.SetLight(nCh, lightVal);
        }

        private void BtnCloseChannel_Click(object sender, EventArgs e)
        {
            if (comboChanel.SelectedIndex == -1)
            {
                MessageBox.Show(" 请选择通道", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int nCh = comboChanel.SelectedIndex;
            int lightVal = 0;
            OtherDevices.lightControler.SetLight(nCh, lightVal);
        }

        private async void btnNinePointCalbUpCam_Click(object sender, EventArgs e)
        {
            DoWhile.ResetCirculate();
            string strCCDName = "";
            if (comboBox_DownCCDSelItem.SelectedIndex > -1)
                strCCDName = comboBox_DownCCDSelItem.SelectedIndex == 0 ? "Left" : "Right";
            else
                return;
            if (strCCDName == "")
            {
                MessageBox.Show(" 请选择相机", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string strLightItem = strCCDName == "Left" ? "左机械标定光源" : "右机械标定光源";
            int nCh = strCCDName == "Front" ? 3 : 4;
            int nVal = ParamSetMgr.GetInstance().GetIntParam(strLightItem);
            StationFeedCollect station = (StationFeedCollect)StationMgr.GetInstance().GetStation("取料站");
            if (MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisX) != AxisHomeFinishFlag.Homed ||
               MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisY) != AxisHomeFinishFlag.Homed ||
               MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisZ) != AxisHomeFinishFlag.Homed)
            {
                MessageBox.Show(" 取料站 XYZ没回原点", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            bool bmanual = true;
            string strposNome = strCCDName == "Left" ? string.Format("左标定拍照点") : string.Format("右标定拍照点");
            Point2d pointUpDownCamCalibPos = new Point2d(station.GetStationPointDic()[strposNome].pointX,
               station.GetStationPointDic()[strposNome].pointY);

            string strposAidNome = strCCDName == "Left" ? $"左吸嘴{comboSelNozzle.Text.ToInt()}对吸嘴点" : $"右吸嘴{comboSelNozzle.Text.ToInt()}对吸嘴点";
            Point2d pointAid = new Point2d(station.GetStationPointDic()[strposAidNome].pointX, station.GetStationPointDic()[strposAidNome].pointY);
            string stepNama = strCCDName == "Left" ? "左机械标定" : "右机械标定";
            double dx = ParamSetMgr.GetInstance().GetDoubleParam($"{stepNama}X步长");
            double dy = ParamSetMgr.GetInstance().GetDoubleParam($"{stepNama}Y步长");
            Point2d[] point2Ds = new Point2d[] {
                new Point2d( pointUpDownCamCalibPos.x- dx, pointUpDownCamCalibPos.y -dy),
                new Point2d( pointUpDownCamCalibPos.x- dx, pointUpDownCamCalibPos.y ),
                new Point2d( pointUpDownCamCalibPos.x- dx, pointUpDownCamCalibPos.y + dy),

                new Point2d( pointUpDownCamCalibPos.x, pointUpDownCamCalibPos.y - dy),
                new Point2d( pointUpDownCamCalibPos.x, pointUpDownCamCalibPos.y ),
                new Point2d( pointUpDownCamCalibPos.x,pointUpDownCamCalibPos.y+  dy),

                new Point2d( pointUpDownCamCalibPos.x+  dx, pointUpDownCamCalibPos.y -  dy),
                new Point2d( pointUpDownCamCalibPos.x+  dx, pointUpDownCamCalibPos.y),
                new Point2d( pointUpDownCamCalibPos.x+  dx,pointUpDownCamCalibPos.y + dy)
            };

            Cmdbase.GoZSafePos(true);
            for (int i = 1; i < 16; i++)
                IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));
            BtnEnable(false);
            HObject Img = null;
            DoWhile.ResetCirculate();
            Task task = Task.Run(() =>
            {

                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}");
                try
                {
                    List<double> Vrow = new List<double>(); Vrow.Clear();
                    List<double> Vcol = new List<double>(); Vcol.Clear();

                    List<double> My = new List<double>(); My.Clear();
                    List<double> Mx = new List<double>(); Mx.Clear();




                    OtherDevices.lightControler.Light(nCh, nVal);
                    for (int j = 0; j < 9; j++)
                    {


                        Thread.Sleep(1000);
                        WaranResult waranResult = station.MoveMulitAxisPosWaitInpos(new int[] { station.AxisX, station.AxisY },
                        new double[] { point2Ds[j].x, point2Ds[j].y }, new double[] { (double)SpeedType.Low, (double)SpeedType.Low }, 2, true, null, 50000);
                        if (waranResult == WaranResult.Alarm || waranResult == WaranResult.TimeOut || waranResult == WaranResult.Stop)
                        {
                            MessageBox.Show("运动到" + strposNome + "电机异常或者超时", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        station.MoveMulitAxisPosWaitInpos(new int[] { station.AxisZ, },
                       new double[] { station.GetStationPointDic()[strposNome].pointZ }, new double[] { (double)SpeedType.Low }, 0.05, true, null, 50000);
                        Thread.Sleep(200);

                        bool bPrResult = true;


                        string CammeraName = "Left";
                    retrysnap:
                        CameraMgr.GetInstance().SetTriggerSoftMode(CammeraName);

                        Img = CameraMgr.GetInstance().GetImg(CammeraName);
                        bPrResult = VisionAddtion.FindCircleCenter(Img, visionControl1, out HTuple cx, out HTuple cy);


                        HOperatorSet.WriteImage(Img, "bmp", 0, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\" + j.ToString() + ".bmp");
                        if (bPrResult)
                        {
                            Vrow.Add(cy.D);
                            Vcol.Add(cx.D);
                        }
                        else
                        {
                            waranResult = AlarmMgr.GetIntance().WarnWithDlg($"相机{strCCDName}与XYZ标定：本次标定失败，是否重试？", null, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                            if (waranResult == WaranResult.Retry)
                                goto retrysnap;

                        }
                        Thread.Sleep(2000);
                        Img.Dispose();
                        Mx.Add(MotionMgr.GetInstace().GetAxisPos(station.AxisX));
                        My.Add(MotionMgr.GetInstace().GetAxisPos(station.AxisY));
                    }

                    HTuple hTupleVrow = new HTuple();
                    HTuple hTupleVcol = new HTuple();
                    HTuple hTupleMx = new HTuple();
                    HTuple hTupleMy = new HTuple();
                    for (int i = 0; i < Vrow.Count; i++)
                    {
                        hTupleVrow.Append(Vrow[i]);
                        hTupleVcol.Append(Vcol[i]);
                    }
                    for (int i = 0; i < Vrow.Count; i++)
                    {
                        pointAid.x = 0;
                        pointAid.y = 0;
                        hTupleMx.Append(Mx[i] - pointAid.x);
                        hTupleMy.Append(My[i] - pointAid.y);
                    }
                    XY_C_Follow_Calib xY_UR_Calib = null;
                    if (strCCDName == "Left")
                        xY_UR_Calib = VisionCalib.XYLeftCalib;

                    if (strCCDName == "Right")
                        xY_UR_Calib = VisionCalib.XYRightCalib;


                    xY_UR_Calib.CreateURCoor(Vcol.ToArray(), Vrow.ToArray(), Mx.ToArray(), My.ToArray());

                    HOperatorSet.WriteTuple(hTupleVrow, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\VM_VRow.tup");
                    HOperatorSet.WriteTuple(hTupleVcol, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\VM_VCol.tup");
                    HOperatorSet.WriteTuple(hTupleMx, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\VM_Mx.tup");
                    HOperatorSet.WriteTuple(hTupleMy, AppDomain.CurrentDomain.BaseDirectory + @"\VisionCalb\" + $"{strCCDName}\\VM_My.tup");

                    MessageBox.Show($"相机{strCCDName}与XYZ标定：标定成功");
                }
                catch (Exception EX)
                {
                    MotionMgr.GetInstace().StopAxis(station.AxisX);
                    MotionMgr.GetInstace().StopAxis(station.AxisY);
                    MotionMgr.GetInstace().StopAxis(station.AxisZ);
                }

            });
            await task;
            for (int i = 1; i < 16; i++)
                IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));
            BtnEnable(true);

            OtherDevices.lightControler.Light(nCh, 0);

        }

        private void button3_Click(object sender, EventArgs e)
        {

            int val = txtMaxTroque.Text.ToInt();
            int nVCMAxisZ = 3;
            EtherCardFun etherCardFun = MotionMgr.GetInstace().GetCardByIndexAxis(nVCMAxisZ) as EtherCardFun;
            etherCardFun.SetMaxTorque(nVCMAxisZ, 3, val);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int nVCMAxisZ = 3;
            EtherCardFun etherCardFun = MotionMgr.GetInstace().GetCardByIndexAxis(nVCMAxisZ) as EtherCardFun;
            double Torque = etherCardFun.GetEcatAxisAtlTorque(nVCMAxisZ);

            labelTroqueVal.Text = Torque.ToString();
        }

        private void btnReadCurrent_Click(object sender, EventArgs e)
        {
            int nVCMAxisZ = 3;
            EtherCardFun etherCardFun = MotionMgr.GetInstace().GetCardByIndexAxis(nVCMAxisZ) as EtherCardFun;
            double current = etherCardFun.GetEcatAxisAtlCurrent(nVCMAxisZ);

            labelCurrentVal.Text = current.ToString();
        }

        private void ReadSdo_Click(object sender, EventArgs e)
        {
            int nVCMAxisZ = 3;
            EtherCardFun etherCardFun = MotionMgr.GetInstace().GetCardByIndexAxis(nVCMAxisZ) as EtherCardFun;
            short nSalve = (short)txtAxisNo.Text.ToInt();
            ushort index = (ushort)Convert.ToUInt16(txtIndex.Text);
            byte nSubIndex = (byte)txtSubIndex.Text.ToInt();
            uint Num = Convert.ToUInt32(txtBytes.Text);
            if (Num <= 0)
                return;
            byte[] vs = etherCardFun.ReadSDOData(nVCMAxisZ, nSalve, index, nSubIndex, Num);
            int val = 0;
            for (int i = 0; i < vs.Length; i++)
            {
                val |= vs[i] << i * 8;
            }
            labelSDOVal.Text = val.ToString();
        }

        private async void bFCalib_Click(object sender, EventArgs e)
        {
            bool bmanual = true;
            DoWhile.ResetCirculate();
            StationAssemble station = (StationAssemble)StationMgr.GetInstance().GetStation("组立站");
            int AxisX = station.AxisX;
            int AxisY = station.AxisY;
            int AxisZ = station.AxisZ;
            int AxisU = station.AxisU;
            int AxisVcmZ = station.AxisTx;
            if (MotionMgr.GetInstace().GetHomeFinishFlag(AxisX) != AxisHomeFinishFlag.Homed ||
             MotionMgr.GetInstace().GetHomeFinishFlag(AxisY) != AxisHomeFinishFlag.Homed ||
             MotionMgr.GetInstace().GetHomeFinishFlag(AxisZ) != AxisHomeFinishFlag.Homed ||
             MotionMgr.GetInstace().GetHomeFinishFlag(AxisU) != AxisHomeFinishFlag.Homed ||
             MotionMgr.GetInstace().GetHomeFinishFlag(AxisVcmZ) != AxisHomeFinishFlag.Homed
             )
            {
                MessageBox.Show(" 组立站  有轴没回原点", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double xpos = station.GetStationPointDic()["力标定起始点"].pointX;
            double ypos = station.GetStationPointDic()["力标定起始点"].pointY;
            double zpos = station.GetStationPointDic()["力标定起始点"].pointZ;
            double upos = station.GetStationPointDic()["力标定起始点"].pointU;
            double vcmpos = station.GetStationPointDic()["力标定起始点"].pointTx;
            for (int i = 1; i < 16; i++)
                IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));
            BtnEnable(false);

            await Task.Run(() =>
            {
                string strMsg = "电流限制,当前位置,目标位置,空载电流,当前电流,当前电流增量,当前力矩,力传感器读数\n";
                Dictionary<double, double> temp = new Dictionary<double, double>();
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    int nCalibCount = ParamSetMgr.GetInstance().GetIntParam("力标定次数");
                    CurrentTroqueData.GetInstance().Clear();
                    for (int n = 0; n < nCalibCount; n++)
                    {
                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ, AxisU, AxisVcmZ },
                         new double[] { xpos, ypos, zpos, upos, vcmpos }, new double[] { (double)SpeedType.High, (double)SpeedType.High,
                       (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High, }, 0.05, bmanual, null);
                        IOMgr.GetInstace().WriteIoBit($"吸嘴{1}下降", true);

                        WaranResult waranResult = station.CheckIobyName("吸嘴1下降到位", true, $"力传感器标定的过程中：吸嘴1下降到位失败，请检查气缸是否卡住，或者线路 ", bmanual);
                        MessageBox.Show($"请放置好力传感器,放置完成点击确定，将进行第{n + 1}次标定");
                        Thread.Sleep(1000);
                        station.Info($"请放置好力传感器,放置完成点击确定，将进行第{n + 1}次标定");
                        Form_MaxFroce form_MaxFroce = null;// new Form_MaxFroce();
                        //form_MaxFroce.TextInfo = $"请输入{n + 1}次最大电流：";
                        //if (form_MaxFroce.ShowDialog() == DialogResult.Cancel)
                        //{
                        //    form_MaxFroce.Dispose();
                        //    return;
                        //}
                        double dStatrtCurrent = ParamSetMgr.GetInstance().GetDoubleParam("力标定起始电流");
                        double dEndCurrent = ParamSetMgr.GetInstance().GetDoubleParam("力标定终止电流");
                        double dCurrentStep = Math.Abs(dEndCurrent - dStatrtCurrent) / nCalibCount;
                        double dMaxCurrent = dStatrtCurrent + n * dCurrentStep;
                        int maxTorque = (int)(dMaxCurrent / 2.0000 * 1000);
                        // form_MaxFroce.Dispose();
                        int nVCMAxisZ = 3;
                        EtherCardFun etherCardFun = MotionMgr.GetInstace().GetCardByIndexAxis(nVCMAxisZ) as EtherCardFun;
                        if (!etherCardFun.SetMaxCuurent(nVCMAxisZ, 3, maxTorque))
                            return;
                        byte[] vs = etherCardFun.ReadSDOData(nVCMAxisZ, 3, 0x6074, 0, 2);
                        int val = 0;
                        for (int i = 0; i < vs.Length; i++)
                        {
                            val |= vs[i] << i * 8;
                        }

                        double currentori = val; //etherCardFun.GetEcatAxisAtlCurrent(nVCMAxisZ);
                        double vcmZPos = double.NaN;
                        //  MessageBox.Show($"第{n + 1}次标定，读取最大力矩值：{val}");
                        station.Info($"第{n + 1}次标定，读取最大力矩值：{val}");
                        stopwatch.Stop();
                        double Torque = 0;
                        double current = 0;
                        try
                        {
                            station.Info($"第{n + 1}次标定，VCM电机上升{ParamSetMgr.GetInstance().GetDoubleParam("力标定上升距离")}，目标位置：{ vcmpos + ParamSetMgr.GetInstance().GetDoubleParam("力标定上升距离")}");
                            station.MoveMulitAxisPosWaitInpos2(new int[] { AxisVcmZ },
                                  new double[] { vcmpos + ParamSetMgr.GetInstance().GetDoubleParam("力标定上升距离") },
                                  new double[] { (double)SpeedType.High, }, 0.05, bmanual, null, 6000,
                                  new Action(() =>
                                  {
                                      double currentpos = MotionMgr.GetInstace().GetAxisPos(AxisVcmZ);
                                      if (vcmZPos != currentpos)
                                          vcmZPos = currentpos;
                                      else
                                      {
                                          if (!stopwatch.IsRunning)
                                          {
                                              stopwatch.Reset();
                                              stopwatch.Restart();
                                          }
                                          if (stopwatch.ElapsedMilliseconds > 2000)
                                          {
                                              Torque = etherCardFun.GetEcatAxisAtlTorque(nVCMAxisZ);
                                              current = etherCardFun.GetEcatAxisAtlCurrent(nVCMAxisZ);
                                              throw new Exception("力矩到达，轴已经停止");
                                          }

                                      }

                                  }));
                        }
                        catch (Exception es)
                        {
                            if (es.Message == "力矩到达，轴已经停止")
                                MotionMgr.GetInstace().StopAxis(AxisVcmZ);
                            else
                                throw es;
                        }
                        form_MaxFroce = new Form_MaxFroce();
                        form_MaxFroce.TextInfo = $"请输入第{n + 1}次测量力：";
                        if (form_MaxFroce.ShowDialog() == DialogResult.Cancel)
                        {
                            form_MaxFroce.Dispose();
                            return;
                        }
                        double mearsureTorque = form_MaxFroce.dMearsureVal;
                        form_MaxFroce.Dispose();
                        double Torque2 = etherCardFun.GetEcatAxisAtlTorque(nVCMAxisZ);
                        vs = etherCardFun.ReadSDOData(nVCMAxisZ, 3, 0x6074, 0, 2);
                        val = 0;
                        for (int i = 0; i < vs.Length; i++)
                        {
                            val |= vs[i] << i * 8;
                        }
                        double current2 = val;// etherCardFun.GetEcatAxisAtlCurrent(nVCMAxisZ);
                        temp.Add(dMaxCurrent, mearsureTorque);
                        strMsg = strMsg + $"{dMaxCurrent},{MotionMgr.GetInstace().GetAxisPos(nVCMAxisZ)},{vcmpos + ParamSetMgr.GetInstance().GetDoubleParam("力标定上升距离")},{ currentori * 2 / 1000},{current * 2 / 1000},{(current - currentori) * 2 / 1000},{Torque} ,{mearsureTorque}\n";
                        // MessageBox.Show($"第{n + 1}次标定完成："+ $"设定限制电流：{maxTorque},当前位置：{MotionMgr.GetInstace().GetAxisPos(nVCMAxisZ)},目标位置：{vcmpos + ParamSetMgr.GetInstance().GetDoubleParam("力标定上升距离")},初始电流：{ currentori * 2 / 1000},当前电流：{current * 2 / 1000},当前电流增量:{(current- currentori)*2/1000},当前力矩：{Torque}" +
                        //      $"力传感器读数{mearsureTorque}");
                    }
                    if (temp.Count == nCalibCount)
                    {
                        foreach (var s in temp)
                            CurrentTroqueData.GetInstance().Add(s.Key, s.Value);
                        CurrentTroqueData.GetInstance().trans();
                        CurrentTroqueData.GetInstance().Save(@"E:\TorqueTest.json");
                    }

                    //using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    //{
                    //    //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;//注意这里写路径时要用c:\\而不是c:\

                    //    openFileDialog.Filter = "所有文件|*.*";
                    //    openFileDialog.RestoreDirectory = true;
                    //    openFileDialog.FilterIndex = 1;
                    //    openFileDialog.Multiselect = false;
                    //    openFileDialog.Title = "打开工作流";
                    //    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    //    {
                    //        string strPath = openFileDialog.FileName;
                    //        CmdMgr.GetInstance().Read(strPath);
                    //    }

                    //}

                }
                catch (Exception ex)
                {
                    MessageBox.Show("力标定停止！！");
                }
                finally
                {
                    File.AppendAllText(@"E:\TorqueTest.csv", strMsg);
                }

            });

            BtnEnable(true);
            for (int i = 1; i < 16; i++)
                IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));

        }

        private void CurrentLimt_Click(object sender, EventArgs e)
        {

            int nVCMAxisZ = 3;
            EtherCardFun etherCardFun = MotionMgr.GetInstace().GetCardByIndexAxis(nVCMAxisZ) as EtherCardFun;
            int val = txtMaxCurrent.Text.ToInt();
            etherCardFun.WriteSDOData(nVCMAxisZ, 3, val, 0x6073, 0, 2);
        }

        private async void BtnTestHigh_Click(object sender, EventArgs e)
        {
            int nIndex = ComboxTestHighSelNozzle.SelectedIndex;
            if (nIndex == -1 || ComboxTestHighSelNozzle.Text == null || ComboxTestHighSelNozzle.Text == "")
                return;
            nIndex = ComboxTestHighSelNozzle.Text.ToInt();
            bool bmanual = true;
            DoWhile.ResetCirculate();
            double currentpos = 0;
            labelNozzlePos.Text = "测定中...";
            StationAssemble station = (StationAssemble)StationMgr.GetInstance().GetStation("组立站");
            int AxisX = station.AxisX;
            int AxisY = station.AxisY;
            int AxisZ = station.AxisZ;
            int AxisU = station.AxisU;
            int AxisVcmZ = station.AxisTx;
            if (MotionMgr.GetInstace().GetHomeFinishFlag(AxisX) != AxisHomeFinishFlag.Homed ||
             MotionMgr.GetInstace().GetHomeFinishFlag(AxisY) != AxisHomeFinishFlag.Homed ||
             MotionMgr.GetInstace().GetHomeFinishFlag(AxisZ) != AxisHomeFinishFlag.Homed ||
             MotionMgr.GetInstace().GetHomeFinishFlag(AxisU) != AxisHomeFinishFlag.Homed ||
             MotionMgr.GetInstace().GetHomeFinishFlag(AxisVcmZ) != AxisHomeFinishFlag.Homed
             )
            {
                MessageBox.Show(" 组立站  有轴没回原点", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double xpos = station.GetStationPointDic()["吸嘴探高点"].pointX + (1 - nIndex) * ParamSetMgr.GetInstance().GetDoubleParam("物料吸嘴间距");
            double ypos = station.GetStationPointDic()["吸嘴探高点"].pointY;
            double zpos = station.GetStationPointDic()["吸嘴探高点"].pointZ;
            double upos = station.GetStationPointDic()["吸嘴探高点"].pointU;
            double vcmpos = station.GetStationPointDic()["吸嘴探高点"].pointTx;
            for (int i = 1; i < 16; i++)
                IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));
            double dDownDistance = ParamSetMgr.GetInstance().GetDoubleParam("吸嘴校高距离");
            double speed = ParamSetMgr.GetInstance().GetDoubleParam("吸嘴校高速度");
            double dAvage = 0;
            BtnEnable(false);
            bool bAllSucess = true;
            await Task.Run(() =>
            {
                try
                {
                    int nCount = ParamSetMgr.GetInstance().GetIntParam("吸嘴校高次数");
                    if (nCount <= 0)
                        return;
                    double dSum = 0;
                    for (int n = 0; n < nCount; n++)
                    {
                        double dSafeHigh = StationMgr.GetInstance().GetStation("组立站").GetStationPointDic()["安全高度"].pointZ;

                        station.MoveMulitAxisPosWaitInpos(new int[] {AxisZ},
                          new double[] { dSafeHigh}, new double[] { (double)SpeedType.High }, 0.09, bmanual, null);

                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY/*, AxisU*/},
                          new double[] { xpos, ypos/*, upos*/}, new double[] { /*(double)SpeedType.High,*/
                       (double)SpeedType.High, (double)SpeedType.High}, 0.09, bmanual, null);

                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisVcmZ },
                         new double[] {  vcmpos }, new double[] { (double)SpeedType.High }, 0.09, bmanual, null);

                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisZ },
                         new double[] { zpos }, new double[] { (double)SpeedType.High }, 0.09, bmanual, null);

                        // station.MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ, AxisU, AxisVcmZ },
                        //   new double[] { xpos, ypos, zpos, upos, vcmpos }, new double[] { (double)SpeedType.High, (double)SpeedType.High,
                        //(double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High, }, 0.09, bmanual, null);
                        IOMgr.GetInstace().WriteIoBit($"Barrel夹持气缸打开", true);
                        IOMgr.GetInstace().WriteIoBit($"吸嘴{nIndex}下降", true);

                        WaranResult waranResult = station.CheckIobyName($"吸嘴{nIndex}下降到位", true, $"吸嘴校高的过程中：吸嘴{nIndex}下降到位失败，请检查气缸是否卡住，或者线路 ", bmanual);
                        Thread.Sleep(1000);

                        int nVCMAxisZ = 3;
                        EtherCardFun etherCardFun = MotionMgr.GetInstace().GetCardByIndexAxis(nVCMAxisZ) as EtherCardFun;

                        byte[] vs = etherCardFun.ReadSDOData(nVCMAxisZ, 3, 0x6074, 0, 2);
                        int val = 0;
                        for (int i = 0; i < vs.Length; i++)
                        {
                            val |= vs[i] << i * 8;
                        }
                        double vcmZPos = double.NaN;

                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Stop();
                        double Torque = 0;

                        double currentori = val;
                        bool bIsTouch = false;
                        try
                        {
                            station.MoveMulitAxisPosWaitInpos2(new int[] { AxisZ },
                               new double[] { zpos - dDownDistance },
                               new double[] { speed }, 0.05, bmanual, null, 60000,
                               new Action(() =>
                               {
                                   byte[] vs2 = etherCardFun.ReadSDOData(nVCMAxisZ, 3, 0x6074, 0, 2);
                                   int val2 = 0;
                                   for (int i = 0; i < vs.Length; i++)
                                       val2 |= vs2[i] << i * 8;
                                   if (val2 >= currentori * 1.05)
                                   {
                                       currentpos = MotionMgr.GetInstace().GetAxisPos(AxisZ);
                                       dSum = dSum + currentpos;
                                       MotionMgr.GetInstace().StopAxis(AxisZ);
                                       File.AppendAllLines(@"E:\NozzleTest.csv", new string[] { currentpos.ToString("F4") });
                                       bIsTouch = true;
                                       throw new Exception("吸嘴接触，位置到达");
                                   }
                               }));
                        }
                        catch (Exception es)
                        {
                            if (es.Message == "吸嘴接触，位置到达")
                                MotionMgr.GetInstace().StopAxis(AxisZ);
                            else
                                throw es;   
                        }
                        if(!bIsTouch)
                        {
                            bAllSucess &= false;
                            MessageBox.Show("吸嘴校准失败");
                            return;
                        }
                    }
                    dAvage = dSum / nCount;
                }
                catch (Exception ec)
                {

                }
            });
            BtnEnable(true);
            IOMgr.GetInstace().WriteIoBit($"Barrel夹持气缸打开", false);

            for (int i = 1; i < 16; i++)
                IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));
            if(bAllSucess)
            {
                NozzleMgr.GetInstance().SetNozzleAssBottomPos(nIndex, dAvage);
                NozzleMgr.GetInstance().Save();
            }
                
            labelNozzlePos.Text = dAvage.ToString("F4");
        }

        private async void BtnRotate_Click(object sender, EventArgs e)
        {
            bool bmanual = true;
            DoWhile.ResetCirculate();
            StationAssemble station = (StationAssemble)StationMgr.GetInstance().GetStation("点胶站");
            int AxisX = station.AxisX;
            int AxisY = station.AxisY;
            int AxisZ = station.AxisZ;
            int AxisU = station.AxisU;
            int AxisVcmZ = station.AxisTx;
            if (MotionMgr.GetInstace().GetHomeFinishFlag(AxisX) != AxisHomeFinishFlag.Homed ||
                MotionMgr.GetInstace().GetHomeFinishFlag(AxisY) != AxisHomeFinishFlag.Homed ||
                MotionMgr.GetInstace().GetHomeFinishFlag(AxisZ) != AxisHomeFinishFlag.Homed ||

                MotionMgr.GetInstace().GetHomeFinishFlag(AxisU) != AxisHomeFinishFlag.Homed

             )
            {
                MessageBox.Show(" 组立站  有轴没回原点", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            double xpos = station.GetStationPointDic()["圆心标定点"].pointX;
            double ypos = station.GetStationPointDic()["圆心标定点"].pointY;
            double zpos = station.GetStationPointDic()["圆心标定点"].pointZ;
            double upos = station.GetStationPointDic()["圆心标定点"].pointU;
            double vcmpos = station.GetStationPointDic()["圆心标定点"].pointTx;
            for (int i = 1; i < 2; i++)
            {
                IOMgr.GetInstace().WriteIoBit($"Barrel吸嘴{i}下降", false);
                IOMgr.GetInstace().WriteIoBit($"Barrel抓手{i}下降", false);
            }
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));
            int nCountFindCenter = ParamSetMgr.GetInstance().GetIntParam("圆心标定次数");
            BtnEnable(false);
            await Task.Run(() =>
            {
                station.MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY, AxisZ, AxisU, AxisVcmZ },
                            new double[] { xpos, ypos, zpos, upos, vcmpos }, new double[] { (double)SpeedType.High, (double)SpeedType.High,
                       (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High, }, 0.05, bmanual, null);
                string PrName = "点胶标定圆";
                string CammeraName = "";
                bool bPrResult;
                double currentpos = MotionMgr.GetInstace().GetAxisPos(AxisU);
                double stepDistance = 360 / nCountFindCenter;
                List<double> listRow = new List<double>();
                List<double> listCol = new List<double>();
                HTuple hTupleRow = new HTuple();
                HTuple hTupleCol = new HTuple();
                for (int i = 0; i < nCountFindCenter; i++)
                {
                    station.MoveMulitAxisPosWaitInpos(new int[] { AxisU }, new double[] { currentpos + stepDistance }, new double[] { }, 0.05, bmanual, null);
                    Thread.Sleep(1200);
                    try
                    {

                        VisionMgr.GetInstance().GetItem(PrName);
                        double dGain = (double)VisionMgr.GetInstance().GetGain(PrName);
                        double dExpoursetime = (double)VisionMgr.GetInstance().GetExpourseTime(PrName);
                        CammeraName = VisionMgr.GetInstance().GetCamName(PrName);
                        CameraMgr.GetInstance().SetTriggerSoftMode(CammeraName);
                        CameraMgr.GetInstance().SetCamGain(CammeraName, dGain);
                        CameraMgr.GetInstance().SetCamExposure(CammeraName, dExpoursetime);
                        HObject img = CameraMgr.GetInstance().GetImg(CammeraName);
                        VisionMgr.GetInstance().ClearResult(PrName);
                        bPrResult = VisionMgr.GetInstance().ProcessImage(PrName, img, visionControl1);
                        VisionResulut visionParam = (VisionResulut)VisionMgr.GetInstance().GetResult(PrName);
                        hTupleRow.Append(visionParam.dResultCircleY);
                        hTupleCol.Append(visionParam.dResultCircleX);
                        listRow.Add(visionParam.dResultCircleY);
                        listCol.Add(visionParam.dResultCircleX);
                    }
                    catch (Exception ex)
                    {

                    }


                }
                HOperatorSet.GenContourPolygonXld(out HObject hObject, hTupleRow, hTupleCol);

                HOperatorSet.FitCircleContourXld(hObject, "atukey", -1, 0, 0, 3, 2, out HTuple RowCircle, out HTuple ColCircle, out HTuple Radius, out HTuple startangle, out HTuple endAngle, out HTuple order);

            });
            BtnEnable(true);
            for (int i = 1; i < 2; i++)
            {
                IOMgr.GetInstace().WriteIoBit($"Barrel吸嘴{i}下降", false);
                IOMgr.GetInstace().WriteIoBit($"Barrel抓手{i}下降", false);
            }
            Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));
        }

        private void btnReadHigh_Click(object sender, EventArgs e)
        {
            labelValofHigh.Text = "测试中";
            double dHightVal = OtherDevices.Keyence_High.GetMeasureDate(1, 0);

            labelValofHigh.Text = dHightVal.ToString();
        }


        private async void BtnPinCAILB_Click(object sender, EventArgs e)
        {
            double highTestPos = 0;
            BtnPinCAILB.Enabled = false;
            labelValofHigh.Text = "测试中";
            double dAvage = 0;
            double dSum = 0;
            await Task.Run(() =>
            {
                try
                {
                    StationDispense station = (StationDispense)StationMgr.GetInstance().GetStation("点胶站");
                    int nCount = ParamSetMgr.GetInstance().GetIntParam("探针次数");
                    for (int i = 0; i < nCount; i++)
                    {
                        if (MotionMgr.GetInstace().GetHomeFinishFlag(station.AxisZ) != AxisHomeFinishFlag.Homed)
                        {
                            MessageBox.Show(" 点胶站 XYZ没回原点", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        bool bIsCheck = false;
                        station.MoveSigleAxisPosWaitInpos2(station.AxisZ, 0, (double)SpeedType.High, 0.02, true, null, 300000);
                        Thread.Sleep(2000);
                        double highval = ParamSetMgr.GetInstance().GetDoubleParam("探针底部");
                        try
                        {

                            station.MoveSigleAxisPosWaitInpos2(station.AxisZ, highval, (double)SpeedType.High, 0.02, true, null, 300000, new Action(() =>
                            {
                                if (IOMgr.GetInstace().ReadIoInBit("点胶针头高度检测"))
                                {
                                    throw new Exception("高度到达");
                                }
                            }));
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "高度到达")
                            {
                                MotionMgr.GetInstace().StopAxis(station.AxisZ);
                                bIsCheck = true;
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                        if (bIsCheck)
                        {
                            try
                            {
                                double vel = ParamSetMgr.GetInstance().GetDoubleParam("探针速度");
                                double dCurrentPos = MotionMgr.GetInstace().GetAxisPos(station.AxisZ);
                                station.MoveSigleAxisPosWaitInpos(station.AxisZ, dCurrentPos + 5, (double)SpeedType.High, 0.02, true, null, 300000);
                                Thread.Sleep(2000);
                                station.MoveMulitAxisPosWaitInpos2(new int[] { station.AxisZ }, new double[] { dCurrentPos - 1 }, new double[] { vel }, 0.03, true, null, 60000, new Action(() =>
                                {
                                    if (IOMgr.GetInstace().ReadIoInBit("点胶针头高度检测"))
                                    {
                                        highTestPos = MotionMgr.GetInstace().GetAxisPos(station.AxisZ);
                                        throw new Exception("高度到达2");
                                    }
                                }));
                            }
                            catch (Exception ex2)
                            {
                                if (ex2.Message == "高度到达2")
                                {
                                    MotionMgr.GetInstace().StopAxis(station.AxisZ);
                                    File.AppendAllLines(@"E:\PinHighTest.csv", new string[] { highTestPos.ToString("F4") });
                                }
                                else
                                {
                                    throw ex2;
                                }
                            }
                            dSum += highTestPos;
                        }
                        else
                        {
                            MessageBox.Show("高度检测失败");
                            return;
                        }
                    }
                    dAvage = dSum / nCount;
                }
                catch (Exception exs)
                {

                }


            });
            labelValofHigh.Text = dAvage.ToString("F4");
            BtnPinCAILB.Enabled = true;
        }

        private void btnDispCalib_Click(object sender, EventArgs e)
        {
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_BarrelDispCalib;
            string stationName = "Barrel站";
            string camName = "DispCam";
            int AxisX1 = 6;
            int AxisY1 = 5;
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            
            if (!KeyneceVP.eventCalibMoveIsBeRegister())
            {
                KeyneceVP.eventCalibMove += delegate (int AxisX, int AxisY, int AxisT, double offsetx, double offsety, double offsetT)
                {
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenAssXMove;
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenAssYMove;
                    try
                    {
                        //离线调试
                        //return true;
                        StationFeedCollectBarrel station = (StationFeedCollectBarrel)StationMgr.GetInstance().GetStation(stationName);
                        //控制站的移动
                        double dCurrentPosX = MotionMgr.GetInstace().GetAxisPos(AxisX);
                        double dCurrentPosY = MotionMgr.GetInstace().GetAxisPos(AxisY);
                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisX1, AxisY1 },
                                                          new double[] { dCurrentPosX + offsetx, dCurrentPosY + offsetx },
                                                          new double[] { Convert.ToDouble(SpeedType.High), Convert.ToDouble(SpeedType.High) }, 0.02, true, null, 6000);
                        Thread.Sleep(1000);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                        //throw;
                    }
                    finally
                    {
                        MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssXMove;
                        MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssYMove;
                    }

                };
            }
            KeyneceVP.StartCalib(sendComm, camName, AxisX1, AxisY1);
        }

        private void btnUpPickCalib_Click(object sender, EventArgs e)
        {
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_PickUpCalib;
            string stationName = "取料站";
            string camName = "UpPickCam";
            int AxisX1 = 0;
            int AxisY1 = 5;
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            if (!KeyneceVP.eventCalibMoveIsBeRegister())
            {
                KeyneceVP.eventCalibMove += delegate (int AxisX, int AxisY, int AxisT, double offsetx, double offsety, double offsetT)
                {
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenAssXMove;
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenAssYMove;
                    try
                    {
                        StationFeedCollect station = (StationFeedCollect)StationMgr.GetInstance().GetStation(stationName);
                        //控制站的移动

                        double dCurrentPosX = MotionMgr.GetInstace().GetAxisPos(AxisX);
                        double dCurrentPosY = MotionMgr.GetInstace().GetAxisPos(AxisY);
                        station.MoveMulitAxisPosWaitInpos(new int[] { station.AxisX, station.AxisY },  //AxisX1, AxisY1
                                                          new double[] { dCurrentPosX + offsetx, dCurrentPosY + offsety },
                                                          new double[] { Convert.ToDouble(SpeedType.High), Convert.ToDouble(SpeedType.High) }, 0.02, true, null, 6000);

                        Thread.Sleep(1000);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                        //throw;
                    }
                    finally
                    {
                        MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssXMove;
                        MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssYMove;
                    }

                };
            }
            KeyneceVP.StartCalib(sendComm, camName, AxisX1, AxisY1);
        }

        private void btnUpDischargeCalib_Click(object sender, EventArgs e)
        {
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_PackUpCalib;
            string stationName = "组立站";
            string camName = "UpPackCam";
            int AxisX1 = 0;
            int AxisY1 = 2;
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            if (!KeyneceVP.eventCalibMoveIsBeRegister())
            {
                KeyneceVP.eventCalibMove += delegate (int AxisX, int AxisY, int AxisT, double offsetx, double offsety, double offsetT)
                {
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenAssXMove;
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenAssYMove;
                    try
                    {
                        StationAssemble station = (StationAssemble)StationMgr.GetInstance().GetStation(stationName);
                        //控制站的移动
                        double dCurrentPosX = MotionMgr.GetInstace().GetAxisPos(AxisX);
                        double dCurrentPosY = MotionMgr.GetInstace().GetAxisPos(AxisY);
                        double dCurrentPosT = MotionMgr.GetInstace().GetAxisPos(4);

                        station.MoveMulitAxisPosWaitInpos(new int[] { station.AxisX, station.AxisY, station.AxisU },
                                                          new double[] { dCurrentPosX + offsetx, dCurrentPosY + offsety, dCurrentPosT+ offsetT },
                                                          new double[] { Convert.ToDouble(SpeedType.High), Convert.ToDouble(SpeedType.High), Convert.ToDouble(SpeedType.High) }, 0.02, true, null, 6000);
                        Thread.Sleep(1000);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                        //throw;
                    }
                    finally
                    {
                        MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssXMove;
                        MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssYMove;
                    }
                };
            }
        
            KeyneceVP.StartCalib(sendComm, camName, AxisX1, AxisY1);
        }

        private void btnDispReg_Click(object sender, EventArgs e)
        {
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_DispRegister;
            KeyenceRegisterBasePos(sendComm);
        }

        private void btnUpPickReg_Click(object sender, EventArgs e)
        {
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_PickUpRegister;
            KeyenceRegisterBasePos(sendComm);
        }

        private void btnUpDischargeReg_Click(object sender, EventArgs e)
        {
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_PackUpRegister;
            KeyenceRegisterBasePos(sendComm);
            
            //KeyneceVP.evenPickRegister += (KeyneceVisionProcessor.SendCommandType commandType, int nozzelInd2, out double x1, out double y1,out double t1, out double x2,out double y2, out double t2) =>
            // {
            //     x1 = x2 = y1 = y2 = t1 = t2 = 0;
            //     return true;
            // };
            //KeyneceVP.RegisterBasePos(sendComm, nozzelInd);
        }

        int KeyenceRegisterBasePos(KeyneceVisionProcessor.SendCommandType commandType)
        {
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            KeyneceVP.clearMessageQueue();
            string stationName = "";
            string strCamPointName = "";
            string strPickPointName = "";
            int flag = 0;
            int nozzelInd = 1;
            Stationbase station = null;
            switch (commandType)
            {
                case KeyneceVisionProcessor.SendCommandType.CMD_Send_DispRegister:
                    stationName = "Barrel站";
                    strCamPointName = "keyence校正拍照位";
                    strPickPointName = "keyence校正吸取位1";
                    flag = -1;
                    station = (StationFeedCollectBarrel)StationMgr.GetInstance().GetStation(stationName);
                    break;
                case KeyneceVisionProcessor.SendCommandType.CMD_Send_PickUpRegister:
                    stationName = "取料站";
                    
                    flag = -2;
                    nozzelInd = cbbNozInd_UpPick.Text.ToInt();
                    strCamPointName = $"k校正拍照位{nozzelInd}"; 
                     strPickPointName = $"k校正吸取位{nozzelInd}";
                    station = (StationFeedCollect)StationMgr.GetInstance().GetStation(stationName);
                    break;
                case KeyneceVisionProcessor.SendCommandType.CMD_Send_PackUpRegister:
                    stationName = "组立站";
                    strCamPointName = "keyence校正拍照位1";
                    flag = 1;
                    station = (StationAssemble)StationMgr.GetInstance().GetStation(stationName);

                    //strPickPointName = "keyence校正吸取位";
                    break;
                default:
                    break;
            }

            //StationFeedCollectBarrel station = (StationFeedCollectBarrel)StationMgr.GetInstance().GetStation(stationName);

            //离线调试
            //Point2d pointCamPos = new Point2d(10.11, 12.22);
            Point2d pointCamPos = new Point2d(station.GetStationPointDic()[strCamPointName].pointX,
                                              station.GetStationPointDic()[strCamPointName].pointY);
            if (flag < 0)
            {
                //离线调试
                //Point2d pointPickPos = new Point2d(1.1, 2.2);
                Point2d pointPickPos = new Point2d(station.GetStationPointDic()[strPickPointName].pointX,
                                                   station.GetStationPointDic()[strPickPointName].pointY);
                KeyneceVP.Send($"CC,{(int)commandType},{nozzelInd},{pointCamPos.x},{pointCamPos.y},0," +
                                                $"{pointPickPos.x},{pointPickPos.y},0");
            }
            else if (flag > 0)
            {
                KeyneceVP.Send($"CC,{(int)commandType},{pointCamPos.x},{pointCamPos.y},0");
            }
            
            return 0;
        }

        private void btnUnifyCoor_Click(object sender, EventArgs e)
        {
            //确认先移到下相机拍照位
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_PackUpCalib;
            bool bmanual = true;
            //string strBotCamPointName = "";
            string strPickPointName = "keyence坐标统一吸料点";
            string strUpCamPointName = "keyence坐标统一上相机拍照点";
            string strDischargePointName = "keyence坐标统一放料点";    //事先示教得到
            StationAssemble station = (StationAssemble)StationMgr.GetInstance().GetStation("组立站");
            int AxisX = station.AxisX;
            int AxisY = station.AxisY;
            int AxisZ = station.AxisZ;
            int AxisT = station.AxisU;

            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            //注册回调函数
            if (!KeyneceVP.evenUnifyMoveIsBeRegister())
            {
                KeyneceVP.evenUnifyMove += delegate ()
                {
                    try
                    {
                        //离线调试
                        //return true;


                        int nIndex = ParamSetMgr.GetInstance().GetIntParam("坐标统一吸嘴号");
                      
                        for (int i = 1; i < 16; i++)
                            IOMgr.GetInstace().WriteIoBit($"吸嘴{i}下降", false);  //吸嘴上升
                        Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("气缸上升延时"));  //延时

                        Cmdbase.GoZSafePos(true);  //回到安全位

                        Point2d pointCamPos = new Point2d(station.GetStationPointDic()[strPickPointName].pointX,  //获取坐标统一吸料点
                                              station.GetStationPointDic()[strPickPointName].pointY);
                        //移动到坐标统一吸料点，参数：轴数组，指定轴要到达的位置，轴的运动速度，
                        //当到达指定的轴的目标位置的正负dFineDistance（相当于一个偏差）则停止，设置为手动模式（默认自动,两种模式的停止方式会不一样）
                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY },    
                                                         new double[] { pointCamPos.x, pointCamPos.y },      
                                                         new double[] { Convert.ToDouble(SpeedType.High), Convert.ToDouble(SpeedType.High) },0.002, bmanual);
                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisZ },       //
                                                      new double[] { station.GetStationPointDic()[strPickPointName].pointZ },
                                                      new double[] { Convert.ToDouble(SpeedType.High), }, 0.002, bmanual);
                        rety_cylinder_down:
                        //吸嘴1下降（对应systemCfg）的IO口设为true
                        IOMgr.GetInstace().WriteIoBit($"吸嘴{nIndex}下降", true);
                        //手动模式下，如果在规定时间30000ms内“吸嘴n还没下降到位”，则报出"吸嘴n下降到位失败"的警告
                        WaranResult waranResult = station.CheckIobyName($"吸嘴{nIndex}下降到位", true, $"吸嘴{nIndex}下降到位失败", bmanual, 30000);
                        //若返回重试（station.CheckIobyName只会返回Retry和ok）
                        if (waranResult == WaranResult.Retry)
                            goto rety_cylinder_down;
                        //吸嘴下降到位后吸气（延时一会）
                        IOMgr.GetInstace().WriteIoBit($"吸嘴{nIndex}吸真空", true);
                        Thread.Sleep(1000);
                        //吸嘴上升回去
                        IOMgr.GetInstace().WriteIoBit($"吸嘴{nIndex}下降", false);
                        Cmdbase.GoZSafePos(true); //回到安全位
                        Thread.Sleep(200);

                        //移动到keyence坐标统一放料点
                        pointCamPos = new Point2d(station.GetStationPointDic()[strDischargePointName].pointX,  //获取坐标统一放料点
                                              station.GetStationPointDic()[strDischargePointName].pointY);
                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY },
                                                         new double[] { pointCamPos.x, pointCamPos.y },
                                                         new double[] { Convert.ToDouble(SpeedType.High), Convert.ToDouble(SpeedType.High) }, 0.002, bmanual);
                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisZ },       //
                                                      new double[] { station.GetStationPointDic()[strDischargePointName].pointZ },
                                                      new double[] { Convert.ToDouble(SpeedType.High), }, 0.002, bmanual);
                        rety_cylinder_down1:
                        IOMgr.GetInstace().WriteIoBit($"吸嘴{nIndex}下降", true);
                        waranResult = station.CheckIobyName($"吸嘴{nIndex}下降到位", true, $"吸嘴{nIndex}下降到位失败", bmanual, 30000);
                        //若返回重试（station.CheckIobyName只会返回Retry和ok）
                        if (waranResult == WaranResult.Retry)
                            goto rety_cylinder_down1;
                        //吸嘴下降到位后破气（延时一会）即放下标定板
                        IOMgr.GetInstace().WriteIoBit($"吸嘴{nIndex}吸真空", false);
                        Thread.Sleep(500);
                        //吸嘴上升回去
                        IOMgr.GetInstace().WriteIoBit($"吸嘴{nIndex}下降", false);
                        Cmdbase.GoZSafePos(true); //回到安全位
                        Thread.Sleep(200);

                        //移动到坐标统一上相机拍照位
                        pointCamPos = new Point2d(station.GetStationPointDic()[strUpCamPointName].pointX,  //获取坐标统一上相机拍照点
                                              station.GetStationPointDic()[strUpCamPointName].pointY);
                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY },
                                                         new double[] { pointCamPos.x, pointCamPos.y },
                                                         new double[] { Convert.ToDouble(SpeedType.High), Convert.ToDouble(SpeedType.High) }, 0.002, bmanual);
                        station.MoveMulitAxisPosWaitInpos(new int[] { AxisZ },       //
                                                      new double[] { station.GetStationPointDic()[strUpCamPointName].pointZ },
                                                      new double[] { Convert.ToDouble(SpeedType.High), }, 0.002, bmanual);
                        ////控制站的移动
                        //double dCurrentPosX = MotionMgr.GetInstace().GetAxisPos(AxisX1);
                        //double dCurrentPosY = MotionMgr.GetInstace().GetAxisPos(AxisY1);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                        //throw;
                    }
                };
            }
            KeyneceVP.UnifyCoor();
        }

        private void btnUnifyCoor_Bot_Click(object sender, EventArgs e)
        {
            DialogResult MsgBoxResult;//设置对话框的返回值
            MsgBoxResult = MessageBox.Show("确认已到下相机位置准备拍照",//对话框的显示内容

            "提示",//对话框的标题

            MessageBoxButtons.YesNo,//定义对话框的按钮，这里定义了YSE和NO两个按钮

            MessageBoxIcon.Exclamation,//定义对话框内的图表式样，这里是一个黄色三角型内加一个感叹号

            MessageBoxDefaultButton.Button2);//定义对话框的按钮式样

            if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）

            {

            }

            if (MsgBoxResult == DialogResult.No)//如果对话框的返回值是NO（按"N"按钮）

            {
                return;
            }


            //MessageBox.Show($"确认已到下相机位置准备拍照");
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            KeyneceVP.clearMessageQueue();
            //通知keyence下相机拍照
            KeyneceVP.Send($"CC,{(int)KeyneceVisionProcessor.SendCommandType.CMD_Send_UnifyCoor_BottomCam}");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            while (true)  //最好加上定时跳出
            {
                KeyneceVisionProcessor.MessageStruct mes;
                int ret = KeyneceVP.OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == KeyneceVisionProcessor.MessType.CMD_Answer_UnifyCoorEnd_BottomCam) //下相机返回成功
                {
                    MessageBox.Show($"上下相机坐标系统一完成");
                    break;
                }
                else if (ret == 0 && mes.messType == KeyneceVisionProcessor.MessType.CMD_Answer_UnifyCoorFail_BottomCam) //下相机返回失败
                {
                    MessageBox.Show($"上下相机坐标系统一失败：下相机搜索失败");
                    return;
                }

                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    MessageBox.Show($"上下相机坐标系统一失败：下相机等待keyence超时");
                    return;
                }
            }
        }

        private void btnUnifyCoor_Up_Click(object sender, EventArgs e)
        {
            DialogResult MsgBoxResult;//设置对话框的返回值
            MsgBoxResult = MessageBox.Show("确认标定板到位，上相机位置准备拍照",//对话框的显示内容

            "提示",//对话框的标题

            MessageBoxButtons.YesNo,//定义对话框的按钮，这里定义了YSE和NO两个按钮

            MessageBoxIcon.Exclamation,//定义对话框内的图表式样，这里是一个黄色三角型内加一个感叹号

            MessageBoxDefaultButton.Button2);//定义对话框的按钮式样

            if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）

            {

            }

            if (MsgBoxResult == DialogResult.No)//如果对话框的返回值是NO（按"N"按钮）

            {
                return;
            }
            //MessageBox.Show($"确认已到上相机位置准备拍照");
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            KeyneceVP.clearMessageQueue();
            //通知keyence上相机拍照
            KeyneceVP.Send($"CC,{(int)KeyneceVisionProcessor.SendCommandType.CMD_Send_UnifyCoor_UpCam}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)  //最好加上定时跳出
            {
                KeyneceVisionProcessor.MessageStruct mes;
                int ret = KeyneceVP.OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == KeyneceVisionProcessor.MessType.CMD_Answer_UnifyCoorEnd_UpCam) //上相机返回成功
                {
                    MessageBox.Show($"上下相机坐标系统一完成");
                    break;
                }
                else if (ret == 0 && mes.messType == KeyneceVisionProcessor.MessType.CMD_Answer_UnifyCoorFail_UpCam) //上相机返回成功
                {
                    MessageBox.Show($"上下相机坐标系统一失败：上相机搜索失败");
                    return;
                }

                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    MessageBox.Show($"上下相机坐标系统一失败：上相机等待keyence超时");
                    return;
                }
            }           
        }

       

        private void BtnTest_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            KeyneceVisionProcessor.GetInstance().Send(str);
        }

        private void btnTcpLink_Click(object sender, EventArgs e)
        {

            KeyneceVisionProcessor.GetInstance().linkKeyence();
        }

        private void butDispCalib_Click(object sender, EventArgs e)
        {
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_DispCalib;
            string stationName = "点胶站";
            string camName = "DispCam";
            int AxisX1 = 6;
            int AxisY1 = 2;
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            if (!KeyneceVP.eventCalibMoveIsBeRegister())
            {
                KeyneceVP.eventCalibMove += delegate (int AxisX, int AxisY, int AxisT, double offsetx, double offsety, double offsetT)
                {
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenAssXMove;
                    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove -= Safe.IsSafeWhenAssYMove;
                    try
                    {
                        StationFeedCollect station = (StationFeedCollect)StationMgr.GetInstance().GetStation(stationName);
                        //控制站的移动

                        double dCurrentPosX = MotionMgr.GetInstace().GetAxisPos(AxisX);
                        double dCurrentPosY = MotionMgr.GetInstace().GetAxisPos(AxisY);
                        station.MoveMulitAxisPosWaitInpos(new int[] { station.AxisX, station.AxisY },  //AxisX1, AxisY1
                                                          new double[] { dCurrentPosX + offsetx, dCurrentPosY + offsety },
                                                          new double[] { Convert.ToDouble(SpeedType.High), Convert.ToDouble(SpeedType.High) }, 0.02, true, null, 6000);

                        Thread.Sleep(2000);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                        //throw;
                    }
                    finally
                    {
                        MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssXMove;
                        MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssYMove;
                    }

                };
            }
            KeyneceVP.StartCalib(sendComm, camName, AxisX1, AxisY1);
        }

        private void butDispCenterCalib_Click(object sender, EventArgs e)
        {

        }

        private void butGetBasePot_Click(object sender, EventArgs e)
        {
            string stationName = "点胶站";
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            KeyneceVisionProcessor.SendCommandType sendComm = KeyneceVisionProcessor.SendCommandType.CMD_Send_DispGetBasePot;

            StationFeedCollect station = (StationFeedCollect)StationMgr.GetInstance().GetStation(stationName);
            int nTimeout = 3000;
            XYUPoint basePoint;
            KeyneceVP.getDispBasePoint(station.AxisX, station.AxisY, station.AxisU, out basePoint, nTimeout);

            //将基准点basePoint存哪？？

        }
    }
}
