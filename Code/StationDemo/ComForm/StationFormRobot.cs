using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MotionIoLib;
using System.Threading.Tasks;
using System.Threading;
using HalconDotNet;
using CameraLib;

using CommonTools;
using System.Diagnostics;
using MotionIoLib;
using EpsonRobot;
using BaseDll;
using VisionProcess;
//using HalconLib;
namespace StationDemo
{

    public delegate bool ChangeStatusLabelTextDelegate(ref Button InvokeButton, ref System.Windows.Forms.ToolStripStatusLabel TargetControl, string TargetText);
    public delegate bool ChangeStatusLabelBackColorDelegate(ref Button InvokeButton, ref System.Windows.Forms.ToolStripStatusLabel TargetControl, System.Drawing.Color TargetColor);
    public partial class StationFormRobot : Form, IUserRightSwitch
    {
        Stationbase m_Stationbase = null;
        //整个IO列表
        Dictionary<string, IOMgr.IoDefine> m_dicInput;
        Dictionary<string, IOMgr.IoDefine> m_dicOutput;
        //工站IO-->datagridview 第一列名
        Dictionary<string, int> m_dicNameIndexInput = new Dictionary<string, int>();
        Dictionary<string, int> m_dicNameIndexOutput = new Dictionary<string, int>();

        public UserRight userRight { get; set; }
        protected PictureBox[] InputSignalPictureBoxArray, OutputSignalPictureBoxArray;
        PictureBox TempPictureBox;
        int TempOutputBit = 0;
        private bool[] OutputBitStatus = new bool[16];
        public StationFormRobot()
        {
            InitializeComponent();
        }

        public void SetBtnStartEnable(bool bEnable)
        {
            button_start.Visible = bEnable;
        }
        void ChangeState(Label labelControl, bool bval)
        {
            labelControl.Text = bval ? "ON" : "OFF";
            labelControl.BackColor = bval ? Color.LightGreen : Color.LightBlue;
        }
        void ChangeStateSeverOnBtn(Button button, bool bval)
        {
            button.Text = bval ? "伺服ON" : "伺服OFF";
            button.BackColor = bval ? Color.LightGreen : Color.LightBlue;
        }
        void ChangeAxisPos(Label label, long pos)
        {
            label.Text = pos.ToString();
        }
      void UpdatadataGridView_PointInfo()
        {
            dataGridView_PointInfo.Rows.Clear();
            Dictionary<string, PointInfo> tempdic = StationMgr.GetInstance().GetStation(this.Name).GetStationPointDic();
            foreach (var temp in tempdic)
            {
                dataGridView_PointInfo.Rows.Add(temp.Key, temp.Value.pointX.ToString(),
                    temp.Value.pointY.ToString(), temp.Value.pointZ.ToString(), temp.Value.pointU.ToString(),
                    temp.Value.pointTx.ToString(), temp.Value.pointTy.ToString(), temp.Value.handedSystem ? "右手系" : "左手系"
                    );
            }
        }
        public void OutIoWhenClickBtn(string str)
        {
            bool bState = IOMgr.GetInstace().ReadIoOutBit(str);
            IOMgr.GetInstace().WriteIoBit(str, !bState);
        }
        public void OutIoWhenClickDownBtn(string str)
        {
        
        }
        public void OutIoWhenClickUpBtn(string str)
        {
            
        }
        private void StationForm_Load(object sender, EventArgs e)
        {
           

            UpdataGridViewHeader();
            int width = 0; int height = 0;
            m_Stationbase = StationMgr.GetInstance().GetStation(this);
            comboBox_SelMotionType.SelectedIndex = 0;
            UpdatadataGridView_PointInfo();
            ParamSetMgr.GetInstance().m_eventLoadProductFileUpadata += UpdatadataGridView_PointInfo;
            dataGridView_PointInfo.AllowUserToDeleteRows = true;
            m_dicInput = IOMgr.GetInstace().GetInputDic();
            m_dicOutput = IOMgr.GetInstace().GetOutputDic();

    #region IO 配置
            int indexInput = 0;
            foreach (var tem in m_Stationbase.m_listIoInput)
            {
                if (m_dicInput.ContainsKey(tem))
                {
                    userPanel_Input.AddFlag(tem);
                    userPanel_Input.SetLebalState(tem, IOMgr.GetInstace().ReadIoInBit(tem));
                }
                else
                {
                    MessageBox.Show(tem + "不在IO 表中，请检查工站IO配置", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (userPanel_Input.Count == 0)
            {
                userPanel_Input.Visible = false;
            }
            userPanel_Input.m_nNumPerRow = 2;
            userPanel_Input.Update();
            int indexOutput = 0;
            foreach (var tem in m_Stationbase.m_listIoOutput)
            {

                if (m_dicOutput.ContainsKey(tem))
                {
 
                    indexOutput++;
                    userBtnPanel_Output.AddFlag(tem);
                    userBtnPanel_Output.SetBtnClickEvent(tem, OutIoWhenClickBtn);
                    userBtnPanel_Output.SetBtnClickDownEvent(tem, OutIoWhenClickDownBtn);
                    userBtnPanel_Output.SetBtnClickUpEvent(tem, OutIoWhenClickUpBtn);
                    userBtnPanel_Output.SetBtnState(tem, IOMgr.GetInstace().ReadIoOutBit(tem));
                }

                else
                {
                    MessageBox.Show(tem + "不在IO 表中，请检查工站IO配置", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (userBtnPanel_Output.Count == 0)
            {

                userBtnPanel_Output.Visible = false;
            }
            userBtnPanel_Output.m_nNumPerRow = 2;
            userBtnPanel_Output.Update();
#endregion
            sys.g_eventRightChanged += ChangedUserRight;
            sys.g_User = sys.g_User;
            visionControl1.InitWindow();
            Thread.Sleep(10);
            List<string> camname = CameraMgr.GetInstance().GetCameraNameArr();
            foreach (var temp in camname)
                comboBox_SelCamera.Items.Add(temp.ToString());
            if (camname != null && camname.Count > 0)
                comboBox_SelCamera.SelectedIndex = 0;
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
            HOperatorSet.SetDraw(visionControl1.GetHalconWindow(), "margin");
        }
        public void ChagedPrItem(string name)
        {
            if( InvokeRequired )
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
        public void UpdataGridViewHeader()
        {
            int RobotAxisNum = 4;//RobotMgr.GetInstance().GetRobotAxisMax("抓放料机械手") - RobotMgr.GetInstance().GetRobotAxisMin("抓放料机械手")+1;
          if (RobotAxisNum != 4 && RobotAxisNum != 6)
                return;
          if(RobotAxisNum==4)
            {
                dataGridView_PointInfo.Columns[5].Width = 0;
                dataGridView_PointInfo.Columns[5].Visible = false;

                dataGridView_PointInfo.Columns[6].Width = 0;
                dataGridView_PointInfo.Columns[6].Visible = false;
            }
            if (RobotAxisNum == 6)
            {
                dataGridView_PointInfo.Columns[7].Width = 0;
                dataGridView_PointInfo.Columns[7].Visible = false;
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }



        private void button_homeX_Click(object sender, EventArgs e)
        {
            //int nAxisNo = StationMgr.GetInstance().GetStation(this.Name).AxisX;
            button_homeX.Text = "回0中...";
            button_homeX.BackColor = Color.LightBlue;
            button_homeX.Enabled = false;
            Task ts = new Task(delegate ()
            {
                ScaraRobot.GetInstance().Home();
                //RobotMgr.GetInstance().Home("抓放料机械手", 0, 0);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                while (!ts.IsCompleted)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "回原点失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_homeX.Invoke(
                      new Action(() =>
                      {
                          button_homeX.Enabled = true;
                          button_homeX.Text = "回原点";
                          button_homeX.BackColor = Color.LightGreen;
                      })
                     );

            });

        }
        #region 机器人点动
        private void JogStart(object sender, MouseEventArgs e)
        {

            if (comboBox_SelMotionType.SelectedItem != null && comboBox_SelMotionType.SelectedItem.ToString() != "Jog")
                return;
            int nAxisNo = 1;
            switch (((Button)sender).Name)
            {
                case "button_Xpositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisX;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;
                case "button_Xnegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisX;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;
                case "button_Ypositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisY;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;
                case "button_Ynegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisY;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;
                case "button_Zpositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisZ;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;
                case "button_Znegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisZ;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;
                case "button_Upositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisU;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;
                case "button_Unegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisU;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;
                case "button_Txpositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTx;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;
                case "button_Txnegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTx;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;
                case "button_Typositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTy;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;
                case "button_Tynegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTy;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;

            }
        }
        private void JogEnd(object sender, MouseEventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem != null && comboBox_SelMotionType.SelectedItem.ToString() != "Jog")
                return;

            int nAxisNo;
            switch (((Button)sender).Name)
            {
                case "button_Xpositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisX;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Xnegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisX;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Ypositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisY;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Ynegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisY;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Zpositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisZ;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Znegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisZ;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Upositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisU;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Unegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisU;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Txpositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTx;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Txnegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTx;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Typositive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTy;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                case "button_Tynegtive":
                    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTy;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;

            }
        }
        private void SelMoveType(string strRobotName, string strDistance, int nAxisNo, bool bpostive, int speed)
        {

            int k = bpostive ? 1 : -1;
            switch (strDistance)
            {
                case "0.1":
                    RobotMgr.GetInstance().RelativeMove(strRobotName, nAxisNo, k * 0.1, speed);
                    break;
                case "0.5":
                    RobotMgr.GetInstance().RelativeMove(strRobotName, nAxisNo, k * 0.5, speed);
                    break;
                case "1":
                    RobotMgr.GetInstance().RelativeMove(strRobotName, nAxisNo, k * 1, speed);
                    break;
                case "5":
                    RobotMgr.GetInstance().RelativeMove(strRobotName, nAxisNo, k * 5, speed);
                    break;
                case "10":
                    RobotMgr.GetInstance().RelativeMove(strRobotName, nAxisNo, k * 10, speed);
                    break;
                case "50":
                    RobotMgr.GetInstance().RelativeMove(strRobotName, nAxisNo, k * 50, speed);
                    break;
                case "100":
                    RobotMgr.GetInstance().RelativeMove(strRobotName, nAxisNo, k * 100, speed);
                    break;
                case "1000":
                    RobotMgr.GetInstance().RelativeMove(strRobotName, nAxisNo, k * 1000, speed);
                    break;


            }

        }
        private void button_Xpositive_Click(object sender, EventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem == null)
                return;
            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);

            string strSelDistance = comboBox_SelMotionType.SelectedItem.ToString();
            button_Xpositive.Text = "X+Moving";
            button_Xpositive.BackColor = Color.LightBlue;
            button_Xpositive.Enabled = false;
            EnableBtns(false);
            Task ts = new Task(delegate ()
            {
                // SelMoveType("抓放料机械手", strSelDistance, 0, true, 10);
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition.Copy();
                coordinate.X = coordinate.X + strSelDistance.ToDouble();
                HandDirection handDirection = ScaraRobot.GetInstance().CurrentHandDirection;
                //  ScaraRobot.GetInstance().Jump(coordinate, handDirection, -1);
                ScaraRobot.GetInstance().Go(coordinate, handDirection);
                Thread.Sleep(100);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                while (/*!ts.IsCompleted*/!ScaraRobot.GetInstance().InPos)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "运动失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_Xpositive.Invoke(
                      new Action(() =>
                      {
                          button_Xpositive.Enabled = true;
                          button_Xpositive.Text = "X+";
                          button_Xpositive.BackColor = Color.LightGreen;
                          EnableBtns(true);
                      })
                     );

            });
        }
        private void button_Xnegtive_Click(object sender, EventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem == null)
                return;

            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);

            string strSelDistance = comboBox_SelMotionType.SelectedItem.ToString();
            button_Xnegtive.Text = "X-Moving";
            button_Xnegtive.BackColor = Color.LightBlue;
            button_Xnegtive.Enabled = false;
            EnableBtns(false);
            Task ts = new Task(delegate ()
            {
                // SelMoveType("抓放料机械手", strSelDistance, 0, false, 10);
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition.Copy();
                coordinate.X = coordinate.X - strSelDistance.ToDouble();
                HandDirection handDirection = ScaraRobot.GetInstance().CurrentHandDirection;
                // ScaraRobot.GetInstance().Jump(coordinate, handDirection, -1);
                ScaraRobot.GetInstance().Go(coordinate, handDirection);
                Thread.Sleep(100);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                while (/*!ts.IsCompleted*/!ScaraRobot.GetInstance().InPos)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "运动失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_Xnegtive.Invoke(
                      new Action(() =>
                      {
                          button_Xnegtive.Enabled = true;
                          button_Xnegtive.Text = "X-";
                          button_Xnegtive.BackColor = Color.LightGreen;
                          EnableBtns(true);
                      })
                     );

            });
        }
        private void button_Ypostive_Click(object sender, EventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem == null)
                return;

            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);

            string strSelDistance = comboBox_SelMotionType.SelectedItem.ToString();
            button_Ypositive.Text = "Y+Moving";
            button_Ypositive.BackColor = Color.LightBlue;
            button_Ypositive.Enabled = false;
            EnableBtns(false);
            Task ts = new Task(delegate ()
            {
                //  SelMoveType("抓放料机械手", strSelDistance, 1, true, 10);
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition.Copy();
                coordinate.Y = coordinate.Y + strSelDistance.ToDouble();
                HandDirection handDirection = ScaraRobot.GetInstance().CurrentHandDirection;
                //ScaraRobot.GetInstance().Jump(coordinate, handDirection, -1);
                ScaraRobot.GetInstance().Go(coordinate, handDirection);
                Thread.Sleep(100);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                //while (!ts.IsCompleted)
                while (/*!ts.IsCompleted*/!ScaraRobot.GetInstance().InPos)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "运动失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_Ypositive.Invoke(
                      new Action(() =>
                      {
                          button_Ypositive.Enabled = true;
                          button_Ypositive.Text = "Y+";
                          button_Ypositive.BackColor = Color.LightGreen;
                          EnableBtns(true);
                      })
                     );

            });
        }
        private void button_Ynegtive_Click(object sender, EventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem == null)
                return;

            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);

            string strSelDistance = comboBox_SelMotionType.SelectedItem.ToString();
           
            button_Ynegtive.Text = "Y-Moving";
            button_Ynegtive.BackColor = Color.LightBlue;
            button_Ynegtive.Enabled = false;
            EnableBtns(false);
            Task ts = new Task(delegate ()
            {
                //  SelMoveType("抓放料机械手", strSelDistance, 1, false, 10);
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition.Copy();
                coordinate.Y = coordinate.Y - strSelDistance.ToDouble();
                HandDirection handDirection = ScaraRobot.GetInstance().CurrentHandDirection;
                // ScaraRobot.GetInstance().Jump(coordinate, handDirection, -1);
                ScaraRobot.GetInstance().Go(coordinate, handDirection);
                Thread.Sleep(100);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                // while (!ts.IsCompleted)
                while (/*!ts.IsCompleted*/!ScaraRobot.GetInstance().InPos)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "运动失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_Ynegtive.Invoke(
                      new Action(() =>
                      {
                          button_Ynegtive.Enabled = true;
                          button_Ynegtive.Text = "Y-";
                          button_Ynegtive.BackColor = Color.LightGreen;
                          EnableBtns(true);
                      })
                     );

            });
        }
        private void button_Zpostive_Click(object sender, EventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem == null)
                return;

            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);

            string strSelDistance = comboBox_SelMotionType.SelectedItem.ToString();
            button_Zpositive.Text = "Z+Moving";
            button_Zpositive.BackColor = Color.LightBlue;
            button_Zpositive.Enabled = false;
            EnableBtns(false);
            Task ts = new Task(delegate ()
            {
                //SelMoveType("抓放料机械手", strSelDistance, 2, true, 10);
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition.Copy();
                coordinate.Z = coordinate.Z + strSelDistance.ToDouble();
                HandDirection handDirection = ScaraRobot.GetInstance().CurrentHandDirection;
                ScaraRobot.GetInstance().Go(coordinate, handDirection);
                Thread.Sleep(100);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
               // while (!ts.IsCompleted)
               while(!ScaraRobot.GetInstance().InPos)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "运动失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_Zpositive.Invoke(
                      new Action(() =>
                      {
                          button_Zpositive.Enabled = true;
                          button_Zpositive.Text = "Z+";
                          button_Zpositive.BackColor = Color.LightGreen;
                          EnableBtns(true);
                      })
                     );

            });


        }
        private void button_Znegtive_Click(object sender, EventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem == null)
                return;

            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);

            string strSelDistance = comboBox_SelMotionType.SelectedItem.ToString();
            button_Znegtive.Text = "Z-Moving";
            button_Znegtive.BackColor = Color.LightBlue;
            button_Znegtive.Enabled = false;
            EnableBtns(false);
            Task ts = new Task(delegate ()
            {
                // SelMoveType("抓放料机械手", strSelDistance, 2, false, 10);
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition.Copy();
                coordinate.Z = coordinate.Z - strSelDistance.ToDouble();
                HandDirection handDirection = ScaraRobot.GetInstance().CurrentHandDirection;
                ScaraRobot.GetInstance().Go(coordinate, handDirection);
               
                Thread.Sleep(100);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                //while (!ts.IsCompleted)
               while (!ScaraRobot.GetInstance().InPos)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "运动失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_Znegtive.Invoke(
                      new Action(() =>
                      {
                          button_Znegtive.Enabled = true;
                          button_Znegtive.Text = "Z-";
                          button_Znegtive.BackColor = Color.LightGreen;
                          EnableBtns(true);
                      })
                     );

            });

        }
        private void button_Upostive_Click(object sender, EventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem == null)
                return;

            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);

            string strSelDistance = comboBox_SelMotionType.SelectedItem.ToString();
            button_Upositive.Text = "U+Moving";
            button_Upositive.BackColor = Color.LightBlue;
            button_Upositive.Enabled = false;
            EnableBtns(false);
            Task ts = new Task(delegate ()
            {
                //SelMoveType("抓放料机械手", strSelDistance, 3, true, 10);
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition.Copy();
                coordinate.U = coordinate.U + strSelDistance.ToDouble();
                HandDirection handDirection = ScaraRobot.GetInstance().CurrentHandDirection;
                // ScaraRobot.GetInstance().Jump(coordinate, handDirection, -1);
                ScaraRobot.GetInstance().Go(coordinate, handDirection);
                Thread.Sleep(100);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
               // while (!ts.IsCompleted)
               while(!ScaraRobot.GetInstance().InPos)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "运动失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_Upositive.Invoke(
                      new Action(() =>
                      {
                          button_Upositive.Enabled = true;
                          button_Upositive.Text = "U+";
                          button_Upositive.BackColor = Color.LightGreen;
                          EnableBtns(true);
                      })
                     );

            });
        }
        private void buttonU_negtive_Click(object sender, EventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem == null)
                return;

            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
                if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);

            string strSelDistance = comboBox_SelMotionType.SelectedItem.ToString();
          //  SelMoveType("抓放料机械手", strSelDistance, 3, false, 10);
            button_Unegtive.Text = "U-Moving";
            button_Unegtive.BackColor = Color.LightBlue;
            button_Unegtive.Enabled = false;
            EnableBtns(false);
            Task ts = new Task(delegate ()
            {
                //  SelMoveType("抓放料机械手", strSelDistance, 3, false, 10);
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition.Copy();
                coordinate.U = coordinate.U - strSelDistance.ToDouble();
                HandDirection handDirection = ScaraRobot.GetInstance().CurrentHandDirection;
                // ScaraRobot.GetInstance().Jump(coordinate, handDirection, -1);
                ScaraRobot.GetInstance().Go(coordinate, handDirection);
                Thread.Sleep(100);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                // while (!ts.IsCompleted)
                while (!ScaraRobot.GetInstance().InPos)
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000)
                    {
                        MessageBox.Show("机械人90秒超时" + "运动失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                button_Unegtive.Invoke(
                      new Action(() =>
                      {
                          button_Unegtive.Enabled = true;
                          button_Unegtive.Text = "U-";
                          button_Unegtive.BackColor = Color.LightGreen;
                          EnableBtns(true);
                      })
                     );

            });

        }
        private void button_stop_Click(object sender, EventArgs e)
        {

        }
        #endregion 
        private void CloseForm(object sender, FormClosedEventArgs e)
        {

        }

        private void button_AllAxisMove_Click(object sender, EventArgs e)
        {
            From_OKCancel tempInfoForm = new From_OKCancel();
            System.Drawing.Point p = this.Location;
            p.X = this.Size.Width / 2;
            p.Y = this.Size.Height / 2;
            tempInfoForm.Location = p;
            tempInfoForm.m_OperateInfoText = "是否进行联动，注意设置速度，点位。请注意安全";
            tempInfoForm.ShowDialog();
            if (!tempInfoForm.OperateContiue)
                return;
            int indexRow = dataGridView_PointInfo.CurrentCell.RowIndex;
            int indexCol = dataGridView_PointInfo.CurrentCell.ColumnIndex;
            int index = 1;
            string posID = "P" + indexRow;
            Coordinate coordinate = new Coordinate();
            coordinate.X = dataGridView_PointInfo.Rows[indexRow].Cells[1].Value.ToString().ToDouble();
            coordinate.Y = dataGridView_PointInfo.Rows[indexRow].Cells[2].Value.ToString().ToDouble();
            coordinate.Z = dataGridView_PointInfo.Rows[indexRow].Cells[3].Value.ToString().ToDouble();
            coordinate.U = dataGridView_PointInfo.Rows[indexRow].Cells[4].Value.ToString().ToDouble();

            HandDirection handDirection = dataGridView_PointInfo.Rows[indexRow].Cells[7].Value.ToString() == "左手系" ? HandDirection.Lefty : HandDirection.Right;
            button_AllAxisMove.Enabled = false;
            EnableBtns(false);
            Action action = new Action(() => {
                // RobotMgr.GetInstance().Jump("抓放料机械手", posID);

                double limtZ = coordinate.Z > -50 ? coordinate.Z + 0.2 : -50;
                limtZ = limtZ >= ScaraRobot.GetInstance().CurrentPosition.Z ? limtZ : ScaraRobot.GetInstance().CurrentPosition.Z + 0.2;
                ScaraRobot.GetInstance().Jump(coordinate, handDirection, limtZ);
            });
            action.BeginInvoke((ar) => 
            {
                button_AllAxisMove.Invoke(
                    (MethodInvoker)(() => { button_AllAxisMove.Enabled = true;
                        EnableBtns(true);
                    }), null
                    );
            }, null);
        }

        private void button_SingleAxisMove_Click(object sender, EventArgs e)
        {
            From_OKCancel tempInfoForm = new From_OKCancel();
            System.Drawing.Point p = this.Location;
            p.X = this.Size.Width / 2;
            p.Y = this.Size.Height / 2;
            tempInfoForm.Location = p;

            tempInfoForm.m_OperateInfoText = "是否进行单轴运动，请注意安全";
            tempInfoForm.ShowDialog();
            if (!tempInfoForm.OperateContiue)
                return;
            if (dataGridView_PointInfo.CurrentCell == null)
                return;
            int indexRow = dataGridView_PointInfo.CurrentCell.RowIndex;
            int indexCol = dataGridView_PointInfo.CurrentCell.ColumnIndex;
            int nAxis = -1;

            switch (indexCol)
            {
                case 1:
                    nAxis = StationMgr.GetInstance().GetStation(this).AxisX;
                    break;
                case 2:
                    nAxis = StationMgr.GetInstance().GetStation(this).AxisY;
                    break;
                case 3:
                    nAxis = StationMgr.GetInstance().GetStation(this).AxisZ;
                    break;
                case 4:
                    nAxis = StationMgr.GetInstance().GetStation(this).AxisU;
                    break;
                default:
                    tempInfoForm.m_OperateInfoText = "轴号选择错误";
                    tempInfoForm.ShowDialog();
                    return;
            }
            int pos = Convert.ToInt32(dataGridView_PointInfo.CurrentCell.Value);
            if (nAxis != -1)
                MotionMgr.GetInstace().AbsMove(nAxis, pos, 2);
        }
        private bool CheckReName(string[] staPointName)
        {
            if (staPointName == null)
                return false;
            if (staPointName.Length <= 0)
                return false;
            for (int i = 0; i < staPointName.Length; i++)
            {
                for (int j = i + 1; j < staPointName.Length; j++)
                {
                    if (staPointName[i] == staPointName[j])
                        return true;
                }
            }
            return false;
        }
        private void button_Save_Click(object sender, EventArgs e)
        {
            From_OKCancel tempInfoForm = new From_OKCancel();
            System.Drawing.Point p = this.Location;
            p.X = this.Size.Width / 2;
            p.Y = this.Size.Height / 2;
            tempInfoForm.Location = p;

            tempInfoForm.m_OperateInfoText = "是否保存所有点位";
            tempInfoForm.ShowDialog();
            if (!tempInfoForm.OperateContiue)
                return;
            string[] strPointName = new string[dataGridView_PointInfo.Rows.Count];
            for (int i = 0; i < dataGridView_PointInfo.Rows.Count; i++)
            {
                if (dataGridView_PointInfo.Rows[i].Cells[0].Value != null && dataGridView_PointInfo.Rows[i].Cells[0].Value.ToString() != "")
                    strPointName[i] = dataGridView_PointInfo.Rows[i].Cells[0].Value.ToString();
            }
            if (CheckReName(strPointName))
            {
                tempInfoForm.m_OperateInfoText = "点位有重名，请更正";
                tempInfoForm.ShowDialog();
                return;
            }

            Dictionary<string, PointInfo> staPoint = new Dictionary<string, PointInfo>();
            StationMgr.GetInstance().GetStation(this.Name).GetStationPointDic(ref staPoint);
            string strPonitName = "";
            string strPonitId = "";
            PointInfo temp;
            int index = 1;
            List<string> PointArrIndataGridView_PointInfo = new List<string>();
            PointArrIndataGridView_PointInfo.Clear();
            for (int i = 0; i < dataGridView_PointInfo.Rows.Count; i++)
            {
                index = 1;
                if (dataGridView_PointInfo.Rows[i].Cells[0].Value != null && dataGridView_PointInfo.Rows[i].Cells[0].Value.ToString() != "")
                {
                    //strPonitId = dataGridView_PointInfo.Rows[i].Cells[0].Value.ToString();
                    strPonitName = dataGridView_PointInfo.Rows[i].Cells[0].Value.ToString();
                    temp.pointX = Convert.ToDouble(dataGridView_PointInfo.Rows[i].Cells[index++].Value);
                    temp.pointY = Convert.ToDouble(dataGridView_PointInfo.Rows[i].Cells[index++].Value);
                    temp.pointZ = Convert.ToDouble(dataGridView_PointInfo.Rows[i].Cells[index++].Value);
                    temp.pointU = Convert.ToDouble(dataGridView_PointInfo.Rows[i].Cells[index++].Value);
                    temp.pointTx = Convert.ToDouble(dataGridView_PointInfo.Rows[i].Cells[index++].Value);
                    temp.pointTy = Convert.ToDouble(dataGridView_PointInfo.Rows[i].Cells[index++].Value);
                    temp.handedSystem = dataGridView_PointInfo.Rows[i].Cells[index++].Value.ToString() == "左手系" ? false : true;
                    if (staPoint.ContainsKey(strPonitName))
                    {
                        staPoint[strPonitName] = temp;
                    }
                    else
                    {
                        staPoint.Add(strPonitName, temp);
                    }
                    PointArrIndataGridView_PointInfo.Add(strPonitName);
       
                }
            }
            List<string> DeletePointName = new List<string>();
            DeletePointName.Clear();
            foreach ( var tem in staPoint)
            {
                if (!PointArrIndataGridView_PointInfo.Contains(tem.Key))
                    DeletePointName.Add(tem.Key);
            }
            foreach(var tem in DeletePointName)
            {
                staPoint.Remove(tem);
            }
            ConfigToolMgr.GetInstance().SavePoint(this.Name, staPoint);
        }

        private void button_RecordPoint_Click(object sender, EventArgs e)
        {
            From_OKCancel tempInfoForm = new From_OKCancel();
            System.Drawing.Point p = this.Location;
            p.X = this.Size.Width / 2;
            p.Y = this.Size.Height / 2;
            tempInfoForm.Location = p;
            string strPonitId = "";
            tempInfoForm.m_OperateInfoText = "是否记录此点到点位表格";
            tempInfoForm.ShowDialog();
            if (!tempInfoForm.OperateContiue)
                return;
            int nAxisNot = StationMgr.GetInstance().GetStation(this).AxisX;

            if (dataGridView_PointInfo.CurrentCell != null)
            {
                int indexRow = dataGridView_PointInfo.CurrentCell.RowIndex;
                int indexCol = dataGridView_PointInfo.CurrentCell.ColumnIndex;
                strPonitId = "P" + indexRow;
                string strPointName = dataGridView_PointInfo.Rows[indexRow].Cells[0].Value.ToString();
                string[] strPointNameArr = new string[dataGridView_PointInfo.Rows.Count];
                for (int i = 0; i < dataGridView_PointInfo.Rows.Count; i++)
                {
                    if (dataGridView_PointInfo.Rows[i].Cells[0].Value != null && dataGridView_PointInfo.Rows[i].Cells[0].Value.ToString() != "")
                        strPointNameArr[i] = dataGridView_PointInfo.Rows[i].Cells[0].Value.ToString();
                }
                if (CheckReName(strPointNameArr))
                {
                    tempInfoForm.m_OperateInfoText = "点位有重名，请更正";
                    tempInfoForm.ShowDialog();
                    return;
                }
                PointInfo tempPoint;

                PositionInfo robotPoint = new PositionInfo();
         
                
               

                 indexRow = dataGridView_PointInfo.CurrentCell.RowIndex;
                 indexCol = dataGridView_PointInfo.CurrentCell.ColumnIndex;
                string posID = "P" + indexRow;
                Thread.Sleep(200);
                Action GetAndSaveRobotPos = new Action(() => {
                    #region 远程控制
#if false
                    if (!RobotMgr.GetInstance().GetAxisActPos("抓放料机械手", ref robotPoint))
                    {
                        if (!RobotMgr.GetInstance().GetAxisActPos("抓放料机械手", ref robotPoint))
                        { MessageBox.Show("获取机器人当前坐标失败"); return; }
                    }
                    if (!RobotMgr.GetInstance().SetPointPos("抓放料机械手", posID, strPointName, robotPoint)) { MessageBox.Show("示教设置点位失败"); return; }
                    if (!RobotMgr.GetInstance().SavePointPos("抓放料机械手")) { MessageBox.Show("示教保存到点位到robit.pts文件失败"); return; }
#endif
                    #endregion
                    robotPoint.X= tempPoint.pointX = ScaraRobot.GetInstance().CurrentPosition.X;
                    robotPoint.Y= tempPoint.pointY = ScaraRobot.GetInstance().CurrentPosition.Y;
                    robotPoint.Z= tempPoint.pointZ = ScaraRobot.GetInstance().CurrentPosition.Z;
                    robotPoint.U= tempPoint.pointU = ScaraRobot.GetInstance().CurrentPosition.U;
                    robotPoint.V= tempPoint.pointTx = 0;
                    robotPoint.W= tempPoint.pointTy = 0;
                    robotPoint.Hand = ScaraRobot.GetInstance().CurrentHandDirection == HandDirection.Right;
                });
              GetAndSaveRobotPos.BeginInvoke((ar) =>
                {
                    button_AllAxisMove.Invoke(
                        (MethodInvoker)(() => {
                            dataGridView_PointInfo.Rows[indexRow].Cells[1].Value = tempPoint.pointX = robotPoint.X;
                            dataGridView_PointInfo.Rows[indexRow].Cells[2].Value = tempPoint.pointY = robotPoint.Y;
                            dataGridView_PointInfo.Rows[indexRow].Cells[3].Value = tempPoint.pointZ = robotPoint.Z;
                            dataGridView_PointInfo.Rows[indexRow].Cells[4].Value = tempPoint.pointU = robotPoint.U;

                            dataGridView_PointInfo.Rows[indexRow].Cells[5].Value = tempPoint.pointTx = robotPoint.V;
                            dataGridView_PointInfo.Rows[indexRow].Cells[6].Value = tempPoint.pointTy = robotPoint.W;
                            tempPoint.handedSystem = robotPoint.Hand;
                            dataGridView_PointInfo.Rows[indexRow].Cells[7].Value = robotPoint.Hand ? "右手系" : "左手系";
                            tempPoint.pointTx = 0; tempPoint.pointTy = 0;
                            StationMgr.GetInstance().GetStation(this.Name).AddPoint(strPointName, tempPoint);

                        }), null
                        );
                }, null);
               

            }
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

        private void button_start_Click(object sender, EventArgs e)
        {
            
            string strName = this.Name;
            Stationbase sta = StationMgr.GetInstance().GetStation(strName);
            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
            {
               // if (comboBox_SelCamera.Items[i].ToString() == comboBox_SelCamera.Text) continue;
                CameraMgr.GetInstance().ClaerPr(comboBox_SelCamera.Items[i].ToString());
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
            }
            Action action = new Action( ()=>
            {
                sta.VisionControl = visionControl1;
                sta.ManualProcessWork();
            });
            button_start.Enabled = false;
            action.BeginInvoke((ar) =>
            {
                button_start.Invoke(
                    (MethodInvoker)(
                    () => {
                            button_start.Enabled = true;
                          }
                    ),null
                    );
            }, null);

        }

        private void ShowFirist(object sender, EventArgs e)
        {
            string[] CameraNameArr = StationMgr.GetInstance().GetStation(this.Name).GetCameraArr();
            if (CameraNameArr != null && CameraNameArr.Length > 0)
            {
                for (int i = 0; i < CameraNameArr.Length; i++)
                {
                    comboBox_SelCamera.Items.Add(CameraNameArr[i]);
                    //  CameraMgr.GetInstance().GetCamera(CameraNameArr[i]).wnd = hWindowControl1.HalconID;
                }
                comboBox_SelCamera.SelectedIndex = 0;


            }

        }


        private void PointDel(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //    int indexRow = 0;
            //  DataGridViewRow selectedRow = dataGridView_PointInfo.CurrentRow;
            //  int s=  selectedRow.Index;
            //    string strPointName = dataGridView_PointInfo.Rows[indexRow].Cells[0].ToString();
            //    ConfigToolMgr.GetInstance().DelPoint(this.Name, strPointName);
        }
        void UpdataRobotPos(Coordinate coordinate)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new Action(() => { UpdataRobotPos(coordinate); }));
            }
            else
            {
                label_PosX.Text = coordinate.X.ToString("F3");
                label_PosY.Text = coordinate.Y.ToString("F3");
                label_PosZ.Text = coordinate.Z.ToString("F3");
                label_PosU.Text = coordinate.U.ToString("F3");
            }

        }
        public  void  UpdataHandle(HandDirection handDirection)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new Action(() => { UpdataHandle(handDirection) ; }));
            
            }
            else
            {
                labelHandSystem.Text = handDirection == HandDirection.Lefty ? "左手系" : "右手系";
            }
        }
        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                //RobotMgr.GetInstance().GetRobot("抓放料机械手").m_GetRobotStateChangedHandle += UpdateRobotStatus;
                // RobotMgr.GetInstance().GetRobot("抓放料机械手").m_UpdateRobotStateChangedHandle += UpdateRobotPos;
                ScaraRobot.GetInstance().ChangedPosEvent += UpdataRobotPos;
                ScaraRobot.GetInstance().ChangedHandStateEvent += UpdataHandle;
                IOMgr.GetInstace().m_eventIoInputChanageByName += ChangedIoInState;
                IOMgr.GetInstace().m_eventIoOutputChanageByName += ChangedIoOutState;

            }
            else
            {
                //RobotMgr.GetInstance().GetRobot("抓放料机械手").m_GetRobotStateChangedHandle -= UpdateRobotStatus;
                // RobotMgr.GetInstance().GetRobot("抓放料机械手").m_UpdateRobotStateChangedHandle -= UpdateRobotPos;
                ScaraRobot.GetInstance().ChangedPosEvent -= UpdataRobotPos;
                ScaraRobot.GetInstance().ChangedHandStateEvent -= UpdataHandle;
                IOMgr.GetInstace().m_eventIoInputChanageByName -= ChangedIoInState;
                IOMgr.GetInstace().m_eventIoOutputChanageByName -= ChangedIoOutState;

            }
        }


        private void EnableBtns(bool bEnable)
        {
            button_Zpositive.Enabled = bEnable;
            button_Znegtive.Enabled = bEnable;
            button_Ypositive.Enabled = bEnable;
            button_Ynegtive.Enabled = bEnable;
            button_Upositive.Enabled = bEnable;
            button_Unegtive.Enabled = bEnable;
            button_Xnegtive.Enabled = bEnable;
            button_Xpositive.Enabled = bEnable;
            button_AllAxisMove.Enabled = bEnable;
        }
        private void button_ServoOnX_Click(object sender, EventArgs e)
        {
            if (button_ServoOnX.Text == "伺服OFF")
            {
                // RobotMgr.GetInstance().ServoOn("抓放料机械手");
                button_ServoOnX.Text = "伺服On";
                ScaraRobot.GetInstance().SeverOn(true);
                EnableBtns(true);
            }
            else
            {
               // RobotMgr.GetInstance().ServoOff("抓放料机械手");
                button_ServoOnX.Text = "伺服OFF";
                ScaraRobot.GetInstance().SeverOn(false);
                EnableBtns(false);
            }

        }



        public void ChangedIoInState(string IoName, bool bStateCurrent)
        {
            int nRow = 0;
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ChangedIoInState(IoName, bStateCurrent)));
            }
            else
            {
                userPanel_Input.SetLebalState(IoName, bStateCurrent);

            }
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
                userBtnPanel_Output.SetBtnState(IoName, bStateCurrent);
            }

        }


        private void OnSelectionDataGridView(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            dgv.ClearSelection();
        }

     

        public void ChangedUserRight(User CurrentUser)
        {
            if (InvokeRequired)
                this.BeginInvoke(new Action(() => ChangedUserRight(CurrentUser)));
            else
            {

                switch ((int)CurrentUser._userRight)
                {
                    case (int)UserRight.客户操作员:
                        button_RecordPoint.Enabled = false;
                        button_Save.Enabled = false;
                        button_SingleAxisMove.Enabled = false;
                        button_AllAxisMove.Enabled = false;
                        dataGridView_PointInfo.AllowUserToAddRows = false;
                        dataGridView_PointInfo.AllowUserToDeleteRows = false;
                        dataGridView_PointInfo.ReadOnly = true;

                        break;
                    case (int)UserRight.调试工程师:
                        button_RecordPoint.Enabled = true;
                        button_Save.Enabled = true;
                        button_SingleAxisMove.Enabled = true;
                        button_AllAxisMove.Enabled = true;
                        dataGridView_PointInfo.AllowUserToAddRows = false;
                        dataGridView_PointInfo.AllowUserToDeleteRows = false;
                        dataGridView_PointInfo.ReadOnly = true;
                        dataGridView_PointInfo.Columns[0].ReadOnly = true;
                        break;
                    case (int)UserRight.软件工程师:
                        button_RecordPoint.Enabled = true;
                        button_Save.Enabled = true;
                        button_SingleAxisMove.Enabled = true;
                        button_AllAxisMove.Enabled = true;
                        dataGridView_PointInfo.AllowUserToAddRows = true;
                        dataGridView_PointInfo.AllowUserToDeleteRows = true;

                        dataGridView_PointInfo.ReadOnly = false;
                        break;
                    case (int)UserRight.超级管理员:
                        button_RecordPoint.Enabled = true;
                        button_Save.Enabled = true;
                        button_SingleAxisMove.Enabled = true;
                        button_AllAxisMove.Enabled = true;
                        dataGridView_PointInfo.AllowUserToAddRows = true;
                        dataGridView_PointInfo.AllowUserToDeleteRows = true;
                        dataGridView_PointInfo.ReadOnly = false;
                        break;
                }
                bool bEnable = true;
                if ((int)CurrentUser._userRight >= (int)UserRight.调试工程师)
                {
                    bEnable = true;
                    button_homeX.Enabled = bEnable;
                    button_ServoOnX.Enabled = bEnable;


                    button_Xpositive.Enabled = bEnable;
                    button_Ypositive.Enabled = bEnable;
                    button_Zpositive.Enabled = bEnable;
                    button_Upositive.Enabled = bEnable;
                    button_Txpositive.Enabled = bEnable;
                    button_Typositive.Enabled = bEnable;
                    foreach (var temp in this.Controls)
                    {
                        ((Control)temp).Enabled = bEnable;
                    }
                }
                else
                {
                    bEnable = false;
                    button_homeX.Enabled = bEnable;
                    button_ServoOnX.Enabled = bEnable;


                    button_Xpositive.Enabled = bEnable;
                    button_Ypositive.Enabled = bEnable;
                    button_Zpositive.Enabled = bEnable;
                    button_Upositive.Enabled = bEnable;
                    button_Txpositive.Enabled = bEnable;
                    button_Typositive.Enabled = bEnable;
                    foreach (var temp in this.Controls)
                    {
                        ((Control)temp).Enabled = bEnable;
                    }

                }

            }

        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            button_Save.Location = new Point(dataGridView_PointInfo.Location.X + dataGridView_PointInfo.Width + 5, dataGridView_PointInfo.Location.Y);
            button_RecordPoint.Location = new Point(dataGridView_PointInfo.Location.X + dataGridView_PointInfo.Width + 5, button_RecordPoint.Location.Y);
            button_SingleAxisMove.Location = new Point(dataGridView_PointInfo.Location.X + dataGridView_PointInfo.Width + 5, button_SingleAxisMove.Location.Y);
            button_AllAxisMove.Location = new Point(dataGridView_PointInfo.Location.X + dataGridView_PointInfo.Width + 5, button_AllAxisMove.Location.Y);
            btn_Del.Location = new Point(dataGridView_PointInfo.Location.X + dataGridView_PointInfo.Width + 5, btn_Del.Location.Y);
            userPanel_Input.Location = new Point(button_Save.Location.X + button_Save.Width + 5, dataGridView_PointInfo.Location.Y);
            userBtnPanel_Output.Location = new Point(button_Save.Location.X + button_Save.Width + userPanel_Input.Width + 5, dataGridView_PointInfo.Location.Y);

        }
        private void btn_Del_Click(object sender, EventArgs e)
        {
            Dictionary<string, PointInfo> tempdic = StationMgr.GetInstance().GetStation(this.Name).GetStationPointDic();
            if (dataGridView_PointInfo.SelectedCells.Count <= 0)
                return;

            int CellSelectedRow = dataGridView_PointInfo.SelectedCells[0].RowIndex;
            if (dataGridView_PointInfo.Rows[CellSelectedRow].Cells[0].Value == null)
                return;
            string strPointName = dataGridView_PointInfo.Rows[CellSelectedRow].Cells[0].Value.ToString();

            DataGridViewRow row = dataGridView_PointInfo.Rows[CellSelectedRow];
            dataGridView_PointInfo.Rows.Remove(row);
            PointInfo pointInfo = new PointInfo();
            tempdic.TryGetValue(strPointName, out pointInfo);
            tempdic.Remove(strPointName);
            ConfigToolMgr.GetInstance().DeletePoint(this.Name, strPointName);
            ConfigToolMgr.GetInstance().SavePoint(this.Name, tempdic);
        }

        private void buttonResetRobot_Click(object sender, EventArgs e)
        {
            //RobotMgr.GetInstance().ResetRobot("抓放料机械手");
            ScaraRobot.GetInstance().ResetRobot();
        }



        public void UpdateRobotStatus(ControlInfo controlInfo)
        {

            try
            {

                //   RCStatusBits = RobotMgr.GetInstance().NewProcessStatusCode("抓放料机械手", RobotStatusCode);
                //0 - Test            在TEST模式下打开
                if (controlInfo.Test == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblTest, Color.Green);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblTest, Color.Red);
                }

                //1 - Teach           在TEACH模式下打开
                if (controlInfo.Teach == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblTeach, Color.Green);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblTeach, Color.Red);
                }

                //2 - Auto            在远程输入接受条件下打开
                if (controlInfo.Auto == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblAuto, Color.Green);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblAuto, Color.Red);
                }

                //3 - Warnig   在警告条件下打开,甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
                if (controlInfo.Waring == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblWarning, Color.Red);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblWarning, Color.Green);
                }

                //4 - SError   在严重错误状态下打开,发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
                if (controlInfo.SError == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblSError, Color.Red);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblSError, Color.Green);
                }

                //5 - Safeguard       安全门打开时打开
                if (controlInfo.Safeguard == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblSafeguard, Color.Red);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblSafeguard, Color.Green);
                }

                //6 - EStop           在紧急状态下打开
                if (controlInfo.EStop == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblEStop, Color.Red);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblEStop, Color.Green);
                }

                //7 - Error           在错误状态下打开,使用“Reset 输入”从错误状态中恢复。
                if (controlInfo.Error == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblError, Color.Red);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblError, Color.Green);
                }

                //8 - Paused          打开暂停的任务
                if (controlInfo.Paused == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblPaused, Color.Red);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblPaused, Color.Green);
                }

                //9 - Running         执行任务时打开,在“Paused 输出”为开时关闭。
                if (controlInfo.Running == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblRunning, Color.Green);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblRunning, Color.Red);
                }

                //10 - Ready           控制器完成启动且无任务执行时打开
                if (controlInfo.Ready == true)
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblReady, Color.Green);
                }
                else
                {
                    ChangeStatusLabelBackColor(ref button_Ypositive, ref lblReady, Color.Red);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("获取机械人状态异常" + ex.ToString());
            }


        }
        public void UpdateRobotPos(PositionInfo posInfo)
        {
            //if (RobotMgr.GetInstance().GetCurrentPos("抓放料机械手", ref RCPoint) == true)
            //{
            ChangeStatusLabelText(ref button_Ypositive, ref lblXPos, posInfo.X.ToString());
            ChangeStatusLabelText(ref button_Ypositive, ref lblYPos, posInfo.Y.ToString());
            ChangeStatusLabelText(ref button_Ypositive, ref lblZPos, posInfo.Z.ToString());
            ChangeStatusLabelText(ref button_Ypositive, ref lblUPos, posInfo.U.ToString());
            ChangeStatusLabelText(ref button_Ypositive, ref lblVPos, posInfo.V.ToString());
            ChangeStatusLabelText(ref button_Ypositive, ref lblWPos, posInfo.W.ToString());
            ChangeStatusLabelText(ref button_Ypositive, ref lblHand,
               (posInfo.Hand == Convert.ToBoolean(RobotHand.LeftHand)) ? "左手势" : "右手势");
            //}
            //else
            //{
            //    MessageBox.Show("获取机械人位置失败");
            //}
        }
        public bool ChangeStatusLabelBackColor(ref Button InvokeButton,
            ref System.Windows.Forms.ToolStripStatusLabel TargetControl,
            System.Drawing.Color TargetColor)
        {
            try
            {

                if (InvokeButton == null)
                {
                    return false;
                }

                if (InvokeButton.InvokeRequired == true)
                {
                    ChangeStatusLabelBackColorDelegate ExecuteChangeBackColorDelegate = new ChangeStatusLabelBackColorDelegate(ChangeStatusLabelBackColor);
                    InvokeButton.Invoke(ExecuteChangeBackColorDelegate, new object[] { InvokeButton, TargetControl, TargetColor });
                    return true;
                }
                else
                {
                    TargetControl.BackColor = TargetColor;
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

     

        /// <summary>
        /// 跨线程安全修改目标窗体中状态条的文字
        /// </summary>
        /// <param name="InvokeButton">执行Invoke[委托]的控件</param>
        /// <param name="TargetControl">目标状态条</param>
        /// <param name="TargetText">需要变更的新文字内容</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeStatusLabelText(ref Button InvokeButton,
            ref System.Windows.Forms.ToolStripStatusLabel TargetControl,
            string TargetText)
        {
            try
            {

                if (InvokeButton == null)
                {
                    return false;
                }

                if (InvokeButton.InvokeRequired == true)
                {
                    ChangeStatusLabelTextDelegate ExecuteChangeTextDelegate = new ChangeStatusLabelTextDelegate(ChangeStatusLabelText);
                    InvokeButton.Invoke(ExecuteChangeTextDelegate, new object[] { InvokeButton, TargetControl, TargetText });
                    return true;
                }
                else
                {
                    TargetControl.Text = TargetText;
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        private void roundButton_VisionPrTest_Click(object sender, EventArgs e)
        {
            if (comboBox_SelVisionPR.SelectedIndex == -1)
                return;
            string strVisionPrName =comboBox_SelVisionPR.Items[comboBox_SelVisionPR.SelectedIndex].ToString();
           
            Action action = new Action(() => {
                string camname= VisionMgr.GetInstance().GetCamName(strVisionPrName);
                double?  Expouse = VisionMgr.GetInstance().GetExpourseTime(strVisionPrName);
                double? Gain = VisionMgr.GetInstance().GetGain(strVisionPrName);

                CameraMgr.GetInstance().SetCamExposure(camname,(double) Expouse);
                CameraMgr.GetInstance().SetCamGain(camname,(double)Gain);
                CameraMgr.GetInstance().BindWindow(camname, visionControl1);
                CameraMgr.GetInstance().ClaerPr(camname);
                CameraMgr.GetInstance().GetCamera(camname).SetTriggerMode(CameraModeType.Software);


                // CameraMgr.GetInstance().GetCamera(camname).StartGrab();

                //CameraMgr.GetInstance().AddPr(camname, strVisionPrName);
                //CameraMgr.GetInstance().GetCamera(camname).GarbBySoftTrigger();
                //VisionSetpBase visionSetpBase = VisionMgr.GetInstance().GetItem(strVisionPrName);
              //  Action action2 = new Action(() => { visionSetpBase.Process_image(visionControl1.Img, visionControl1); });
                  HObject Img=  CameraMgr.GetInstance().GetImg(camname);
                 VisionMgr.GetInstance().ProcessImage(strVisionPrName, Img, visionControl1);
                 if (Img != null && Img.IsInitialized())
                      Img.Dispose();
            }
            );

            action.BeginInvoke((ar) => { }, null);
            
        }

        private void comboBox_SelCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox_SelCamera.Items.Count; i++)
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCamera.Items[i].ToString());
           // comboBox_SelCamera.Text = CameraMgr.GetInstance().GetCamExposure(comboBox_SelCam.Text).ToString();
          //  textBox_GainVal.Text = CameraMgr.GetInstance().GetCamGain(comboBox_SelCam.Text).ToString();
            CameraMgr.GetInstance().BindWindow(comboBox_SelCamera.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCamera.Text);
        }

        public void OutPutSignalPictureBoxArray_Click(object sender, EventArgs e)
        {

            try
            {
                TempPictureBox = (PictureBox)sender;
                TempOutputBit = Convert.ToUInt16(TempPictureBox.Tag);

                if (OutputBitStatus[TempOutputBit] == false)
                {
                    if (RobotMgr.GetInstance().SetOutputBit("抓放料机械手", TempOutputBit, true) == false)
                    {
                        MessageBox.Show("Failed to set output bit " + (TempOutputBit + 1) + " ON...");
                    }
                    else
                    {
                        OutputBitStatus[TempOutputBit] = true;
                        OutputSignalPictureBoxArray[TempOutputBit].Image = Properties.Resources.light_green;
                    }
                }
                else
                {
                    if (RobotMgr.GetInstance().SetOutputBit("抓放料机械手", TempOutputBit, false) == false)
                    {
                        MessageBox.Show("Failed to set output bit " + (TempOutputBit + 1) + " OFF...");
                    }
                    else
                    {
                        OutputBitStatus[TempOutputBit] = false;
                        OutputSignalPictureBoxArray[TempOutputBit].Image = Properties.Resources.light_gray;
                    }
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }

   
    
}
