using BaseDll;
using CommonTools;
using log4net;
using MotionIoLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StationDemo
{
    public partial class Form_AxisTest : Form, IUserRightSwitch
    {
        private ILog logger = LogManager.GetLogger("电机测试");

        public Form_AxisTest()
        {
            userRight = UserRight.调试工程师;
            InitializeComponent();
        }

        public UserRight userRight
        {
            set;
            get;
        }

        public void ChangedUserRight(User CurrntUser)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ChangedUserRight(CurrntUser)));
            }
            else
            {
                bool bEable = true;
                if ((int)CurrntUser._userRight >= (int)userRight)
                {
                    dataGridView_AxisParamSet.Enabled = bEable;
                    dataGridView_HomeSet.Enabled = bEable;
                    SaveMotorParam.Enabled = bEable;
                    if ((int)CurrntUser._userRight < (int)UserRight.软件工程师)
                    {
                        dataGridView_AxisParamSet.Columns[0].ReadOnly = true;//轴号
                        dataGridView_AxisParamSet.Columns[1].ReadOnly = true;//轴名
                        dataGridView_AxisParamSet.Columns[2].ReadOnly = true;//轴类型
                    }
                    else
                    {
                        dataGridView_AxisParamSet.Columns[0].ReadOnly = false;//轴号
                        dataGridView_AxisParamSet.Columns[1].ReadOnly = false;//轴名
                        dataGridView_AxisParamSet.Columns[2].ReadOnly = false;//轴类型
                        dataGridView_HomeSet.Columns[2].ReadOnly = false;
                        dataGridView_HomeSet.Columns[3].ReadOnly = false;
                        dataGridView_HomeSet.Columns[4].ReadOnly = false;
                        dataGridView_HomeSet.Columns[5].ReadOnly = false;
                        dataGridView_HomeSet.Columns[6].ReadOnly = false;
                        dataGridView_HomeSet.Columns[7].ReadOnly = false;
                        dataGridView_HomeSet.Columns[8].ReadOnly = false;
                        dataGridView_HomeSet.Columns[9].ReadOnly = false;
                    }
                }
                else
                {
                    bEable = false;
                    dataGridView_AxisParamSet.Enabled = bEable;
                    dataGridView_HomeSet.Enabled = bEable;
                    SaveMotorParam.Enabled = bEable;
                }
            }
        }

        private void SaveMotorParam_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile)
                || ParamSetMgr.GetInstance().CurrentProductFile == null || ParamSetMgr.GetInstance().CurrentProductFile == "")
            {
                MessageBox.Show("当前产品文件夹不存在，请创建载入或者创建当前产品", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TMovePrm prm = new TMovePrm();
            THomePrm homePrm = new THomePrm();
            int indexCell = 0;
            for (int index = 0; index < dataGridView_AxisParamSet.RowCount; index++)
            {
                int nAxisNo = Convert.ToInt32(dataGridView_AxisParamSet.Rows[index].Cells[0].Value);

                prm.VelH = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[3].Value);
                prm.AccH = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[4].Value);
                prm.DccH = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[5].Value);

                prm.VelM = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[6].Value);
                prm.AccM = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[7].Value);
                prm.DccM = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[8].Value);

                prm.VelL = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[9].Value);
                prm.AccL = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[10].Value);
                prm.DccL = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[11].Value);
                prm.PlusePerRote = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[12].Value);
                prm.AxisLeadRange = Convert.ToDouble(dataGridView_AxisParamSet.Rows[index].Cells[13].Value);
                MotionMgr.GetInstace().SetAxisMoveParam(nAxisNo, prm);
                MotionMgr.GetInstace().SetAxisName(nAxisNo, dataGridView_AxisParamSet.Rows[index].Cells[1].Value == null ? "NoNamedAxis" : dataGridView_AxisParamSet.Rows[index].Cells[1].Value.ToString());
                string strAxisType = dataGridView_AxisParamSet.Rows[index].Cells[2].Value == null ? MotorType.SEVER.ToString() : dataGridView_AxisParamSet.Rows[index].Cells[2].Value.ToString();

                MotionMgr.GetInstace().SetMotorType(nAxisNo, (MotorType)Enum.Parse(typeof(MotorType), strAxisType));

                indexCell = 2;
                homePrm._nHomeMode = Convert.ToInt32(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                homePrm._bHomeDir = Convert.ToBoolean(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                homePrm.VelH = Convert.ToDouble(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                homePrm.AccH = Convert.ToDouble(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                homePrm.DccH = Convert.ToDouble(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                homePrm.VelL = Convert.ToDouble(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                homePrm.AccL = Convert.ToDouble(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                homePrm.DccL = Convert.ToDouble(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                homePrm._iSeachOffstPluse = Convert.ToDouble(dataGridView_HomeSet.Rows[index].Cells[indexCell++].Value);
                MotionMgr.GetInstace().SetAxisHomeParam(nAxisNo, homePrm);
            }
            ConfigToolMgr.GetInstance().SaveMoveParam();
            ConfigToolMgr.GetInstance().SaveHomeParam();
        }

        public void UpdataMotonType()
        {
            DataGridViewComboBoxColumn dataColumnCollection = (DataGridViewComboBoxColumn)dataGridView_AxisParamSet.Columns[2];
            dataColumnCollection.Items.Clear();
            Array strItems = Enum.GetValues(typeof(MotorType));
            foreach (var temp in strItems)
            {
                dataColumnCollection.Items.Add(temp.ToString());
            }
        }

        private void updatadataGridView()
        {
            dataGridView_AxisParamSet.Rows.Clear();

            UpdataMotonType();
            dataGridView_HomeSet.Rows.Clear();
            List<MotionCardBase> list = MotionMgr.GetInstace().GetCardList();
            TMovePrm movePrm = new TMovePrm();
            THomePrm homePrm = new THomePrm();
            foreach (var temp in list)
            {
                for (int index = temp.GetMinAxisNo(); index <= temp.GetMaxAxisNo(); index++)
                {
                    movePrm = MotionMgr.GetInstace().GetAxisMovePrm(index);
                    dataGridView_AxisParamSet.Rows.Add(index.ToString(),
                        MotionMgr.GetInstace().GetAxisName(index),
                        MotionMgr.GetInstace().GetMotorType(index).ToString(),
                        movePrm.VelH.ToString(), movePrm.AccH.ToString(), movePrm.DccH.ToString(),
                        movePrm.VelM.ToString(), movePrm.AccM.ToString(), movePrm.DccM.ToString(),
                        movePrm.VelL.ToString(), movePrm.AccL.ToString(), movePrm.DccL.ToString(),
                        movePrm.PlusePerRote.ToString(), movePrm.AxisLeadRange.ToString());

                    homePrm = MotionMgr.GetInstace().GetAxisHomePrm(index);
                    dataGridView_HomeSet.Rows.Add(
                        index.ToString(),
                        MotionMgr.GetInstace().GetAxisName(index),
                        homePrm._nHomeMode.ToString(),
                        homePrm._bHomeDir.ToString(),
                        homePrm.VelH.ToString(), homePrm.AccH.ToString(), homePrm.DccH.ToString(),
                        homePrm.VelL.ToString(), homePrm.AccL.ToString(), homePrm.DccL.ToString(),
                        homePrm._iSeachOffstPluse.ToString());
                }
            }
        }

        private void Form_AxisTest_Load(object sender, EventArgs e)
        {
            dataGridView_AxisParamSet.Location = new Point(0, 0);
            updatadataGridView();
            ParamSetMgr.GetInstance().m_eventLoadProductFileUpadata += updatadataGridView;
            sys.g_eventRightChanged += ChangedUserRight;
            sys.g_User = sys.g_User;
        }

        private async void Test_Click(object sender, EventArgs e)
        {
            int nAxisNo = textBox_AxisNo.Text.ToInt();
            double dLen = textBox_MoveDistance.Text.ToDouble();
            int nCount = textBox_Count.Text.ToInt();
            int speedSel = 0;
            SpeedType speedType = SpeedType.Low;
            if (radioButton_HighSpeed.Checked)
            {
                speedType = SpeedType.High;
                speedSel = 1;
            }

            if (radioButton_MidSpeed.Checked)
            {
                speedSel = 2;
                speedType = SpeedType.Mid;
            }

            if (radioButton_LowSpeed.Checked)
            {
                speedSel = 3;
                speedType = SpeedType.Low;
            }
            if (speedSel == 0)
                return;
            Task task = Task.Run(() =>
            {
                try
                {
                    DoWhile.ResetCirculate();
                    for (int i = 0; i < nCount; i++)
                    {
                        MotionMgr.GetInstace().RelativeMove(nAxisNo, (int)dLen, (int)speedType);
                        DoWhile doWhile = new DoWhile((time, dowhile, bmanual2, obj) =>
                        {
                            AxisState axisState = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo);
                            if (axisState == AxisState.NormalStop)
                                return WaranResult.Run;
                            else if (axisState == AxisState.Moving)
                                return WaranResult.CheckAgain;
                            else
                            {
                                logger.Warn(string.Format("{0}轴状态:{1}", nAxisNo, axisState));
                                throw new Exception("电机测试报警停止" + string.Format("{0}轴状态:{1}", nAxisNo, axisState));
                            }
                        }, 3000000);

                        doWhile.doSomething(null, doWhile, true, null);
                        Thread.Sleep(textBox_InPosDelay.Text.ToInt());

                        MotionMgr.GetInstace().RelativeMove(nAxisNo, (int)(-dLen), (int)speedType);

                        DoWhile doWhile2 = new DoWhile((time, dowhile, bmanual2, obj) =>
                        {
                            AxisState axisState = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo);
                            if (axisState == AxisState.NormalStop)
                                return WaranResult.Run;
                            else if (axisState == AxisState.Moving)
                                return WaranResult.CheckAgain;
                            else
                            {
                                logger.Warn(string.Format("{0}轴状态:{1}", nAxisNo, axisState));
                                throw new Exception("电机测试报警停止" + string.Format("{0}轴状态:{1}", nAxisNo, axisState));
                            }
                        }, 3000000);

                        doWhile2.doSomething(null, doWhile2, true, null);
                        Thread.Sleep(textBox_InPosDelay.Text.ToInt());
                    }
                }
                catch (Exception ex)
                {
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    MessageBox.Show(ex.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            });
            await task;
        }

        private void Btn_TestStop_Click(object sender, EventArgs e)
        {
            DoWhile.StopCirculate();
            int nAxisNo = textBox_AxisNo.Text.ToInt();
            MotionMgr.GetInstace().StopAxis(nAxisNo);
        }

        public void Move(int nAxisNo, double distance, int speedtype)
        {
            MotionMgr.GetInstace().AbsMove(nAxisNo, distance, speedtype);
            DoWhile doWhile = new DoWhile((time, dowhile, bmanual2, obj) =>
            {
                AxisState axisState = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo);
                if (axisState == AxisState.NormalStop)
                    return WaranResult.Run;
                else if (axisState == AxisState.Moving)
                    return WaranResult.CheckAgain;
                else
                {
                    logger.Warn(string.Format("{0}轴状态:{1}", nAxisNo, axisState));
                    throw new Exception("电机测试报警停止" + string.Format("{0}轴状态:{1}", nAxisNo, axisState));
                }
            }, 3000000);
            doWhile.doSomething(null, doWhile, true, null);
        }

        private async void BtnXDLaserTest_Click(object sender, EventArgs e)
        {
            double dEndPos = txtEndPos.Text.ToDouble();
            double dStartPos = txtStartPos.Text.ToDouble();
            double nStepDistace = Math.Abs(txtStepDistance.Text.ToDouble());
            int nSteps = (int)(Math.Abs((dEndPos - dStartPos) / nStepDistace));
            nStepDistace = dEndPos > dStartPos ? nStepDistace : -nStepDistace;
            double dRlsDistance = Math.Abs(txtRlsDistance.Text.ToDouble());
            int nAxisNo = textBox_AxisNo.Text.ToInt();
            double dLen = textBox_MoveDistance.Text.ToDouble();
            int nCount = textBox_Count.Text.ToInt();
            int speedSel = 0;
            SpeedType speedType = SpeedType.Low;
            if (radioButton_HighSpeed.Checked)
            {
                speedType = SpeedType.High;
                speedSel = 1;
            }

            if (radioButton_MidSpeed.Checked)
            {
                speedSel = 2;
                speedType = SpeedType.Mid;
            }

            if (radioButton_LowSpeed.Checked)
            {
                speedSel = 3;
                speedType = SpeedType.Low;
            }
            if (speedSel == 0)
                return;
            dRlsDistance = dEndPos > dStartPos ? dRlsDistance : -dRlsDistance;
            int nSleep = textBox_InPosDelay.Text.ToInt();
            DoWhile.ResetCirculate();
            Task task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < nCount; i++)
                    {
                        for (int n = 0; n <= nSteps; n++)
                        {
                            Move(nAxisNo, dStartPos + nStepDistace * n, (int)speedType);
                            Thread.Sleep(nSleep);
                        }

                        Move(nAxisNo, dEndPos + dRlsDistance, (int)speedType);
                        Thread.Sleep(1000);

                        for (int n = 0; n <= nSteps; n++)
                        {
                            Move(nAxisNo, dEndPos - nStepDistace * n, (int)speedType);
                            Thread.Sleep(nSleep);
                        }
                        Move(nAxisNo, dStartPos - dRlsDistance, (int)speedType);
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    MessageBox.Show(ex.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            });
            await task;
        }
    }
}