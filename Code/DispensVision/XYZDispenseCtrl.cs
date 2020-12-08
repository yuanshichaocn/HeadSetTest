using BaseDll;
using CameraLib;
using HalconDotNet;
using log4net;
using MotionIoLib;
using OtherDevice;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionProcess;

namespace XYZDispensVision
{
    public partial class DispenseCtrl : UserControl, IUserRightSwitch
    {
        public DispenseCtrl()
        {
            try
            {
                InitializeComponent();
                Read();
            }
            catch (Exception e)
            {
                MessageBox.Show(" DispenseCtrl 异常：" + e.Message);
            }
        }

        ~DispenseCtrl()
        {
            string strCamName = TopCamName;

            if (strCamName != null)
            {
                CameraBase cameraBase = CameraMgr.GetInstance().GetCamera(strCamName);
                cameraBase?.Close();
            }
        }

        private ILog log = LogManager.GetLogger(nameof(DispenseCtrl));

        public DispCalibParam dispCalibParam = new DispCalibParam();

        public DispConfig dispConfig = new DispConfig();

        private VisionShapMatch shapeDispCalib = new VisionShapMatch("点胶标定");
        public VisionShapMatch shapeDst = new VisionShapMatch("目标设定");
        public XY_UR_Calib XYUR_Pin = new XY_UR_Calib();
        public XY_UR_Calib XYUR_Laser = new XY_UR_Calib();

        public void FlushToScreen()
        {
            double vel = 100, vellow = 0;
            double x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "标定点").MachinePoint.x;
            double y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "标定点").MachinePoint.y;
            double z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "标定点").MachinePoint.z;
            dataGridView_PointInfo.Rows.Add("标定点", x.ToString("F3"), y.ToString("F3"), z.ToString("F3"));

            x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "对针点").MachinePoint.x;
            y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "对针点").MachinePoint.y;
            z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "对针点").MachinePoint.z;
            dataGridView_PointInfo.Rows.Add("对针点", x.ToString("F3"), y.ToString("F3"), z.ToString("F3"));

            if (IsHaveLaset)
            {
                x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "对镭射点").MachinePoint.x;
                y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "对镭射点").MachinePoint.y;
                z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "对镭射点").MachinePoint.z;
                dataGridView_PointInfo.Rows.Add("对镭射点", x.ToString("F3"), y.ToString("F3"), z.ToString("F3"));

                x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "镭射测高点").MachinePoint.x;
                y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "镭射测高点").MachinePoint.y;
                z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "镭射测高点").MachinePoint.z;
                dataGridView_PointInfo.Rows.Add("镭射测高点", x.ToString("F3"), y.ToString("F3"), z.ToString("F3"));
            }
            x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.x;
            y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.y;
            z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "吐胶点").MachinePoint.z;
            dataGridView_PointInfo.Rows.Add("吐胶点", x.ToString("F3"), y.ToString("F3"), z.ToString("F3"));

            x = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "针头测高点").MachinePoint.x;
            y = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "针头测高点").MachinePoint.y;
            z = dispCalibParam.pointDispenseCalibs.Find(t => t.strPointName == "针头测高点").MachinePoint.z;
            dataGridView_PointInfo.Rows.Add("针头测高点", x.ToString("F3"), y.ToString("F3"), z.ToString("F3"));

            dXStep = dispCalibParam.dXStep;
            dYStep = dispCalibParam.dYStep;
            dLaserOffsetX = dispCalibParam.dLaserOffsetX;
            dLaserOffsetY = dispCalibParam.dLaserOffsetY;
            dPinOffsetX = dispCalibParam.dPinOffsetX;
            dPinOffsetY = dispCalibParam.dPinOffsetY;
            dNeedleZLatchHeight = dispCalibParam.dNeedleZLatchHeight;

            dCalibExposure = dispCalibParam.dCalibExposure;
            dCalibGain = dispCalibParam.dCalibGain;
            dDstExposure = dispCalibParam.dDstExposure;
            dCalibGain = dispCalibParam.dCalibGain;
        }

        public double dNeedleZLatchHeight
        {
            set
            {
                txtZLatchData.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dNeedleZLatchHeight = value;
            }
            get
            {
                return txtZLatchData.Text.ToDouble();
            }
        }

        public double dDstGain
        {
            set
            {
                textDstGain.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dDstGain = value;
            }
            get
            {
                return textDstGain.Text.ToDouble();
            }
        }

        public double dDstExposure
        {
            set
            {
                textDstExpsoure.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dDstExposure = value;
            }
            get
            {
                return textDstExpsoure.Text.ToDouble();
            }
        }

        public double dCalibExposure
        {
            set
            {
                textExpsoure.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dCalibExposure = value;
            }
            get
            {
                return textExpsoure.Text.ToDouble();
            }
        }

        public double dCalibGain
        {
            set
            {
                textGain.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dCalibGain = value;
            }
            get
            {
                return textGain.Text.ToDouble();
            }
        }

        public double dXStep
        {
            set
            {
                textXStep.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dXStep = value;
            }
            get
            {
                return textXStep.Text.ToDouble();
            }
        }

        public double dYStep
        {
            set
            {
                textYStep.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dYStep = value;
            }
            get
            {
                return textYStep.Text.ToDouble();
            }
        }

        public double dPinOffsetX
        {
            set
            {
                textPinXOffset.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dPinOffsetX = value;
            }
            get
            {
                return textPinXOffset.Text.ToDouble();
            }
        }

        public double dPinOffsetY
        {
            set
            {
                textPinYOffset.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dPinOffsetY = value;
            }
            get
            {
                return textPinYOffset.Text.ToDouble();
            }
        }

        public double dLaserOffsetX
        {
            set
            {
                textLaserXOffset.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dLaserOffsetX = value;
            }
            get
            {
                return textLaserXOffset.Text.ToDouble();
            }
        }

        public double dLaserOffsetY
        {
            set
            {
                textLaserYOffset.Text = value.ToString();
                if (dispCalibParam != null)
                    dispCalibParam.dLaserOffsetY = value;
            }
            get
            {
                return textLaserYOffset.Text.ToDouble();
            }
        }

        public void ScreenToData()
        {
            string strPointName = "";
            int indexCol = 1;
            double accSet, decSet, speedSet, speedLowSet;

            DataGridView dataGridView = dataGridView_PointInfo;
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                indexCol = 1;
                strPointName = dataGridView.Rows[i].Cells[0].Value.ToString();
                XYUZPoint pointpos = new XYUZPoint();
                pointpos.x = Convert.ToDouble(dataGridView.Rows[i].Cells[indexCol++].Value.ToString());
                pointpos.y = Convert.ToDouble(dataGridView.Rows[i].Cells[indexCol++].Value.ToString());
                pointpos.z = Convert.ToDouble(dataGridView.Rows[i].Cells[indexCol++].Value.ToString());

                dispCalibParam.SetDispenseCalibsPoint(new PointDispense()
                {
                    strPointName = strPointName,
                    MachinePoint = pointpos,
                });
            }
            dataGridView = dataGridView_DispPosList;
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                indexCol = 1;
                strPointName = dataGridView.Rows[i].Cells[0].Value.ToString();
                XYUZPoint pointpos = new XYUZPoint();
                pointpos.x = Convert.ToDouble(dataGridView.Rows[i].Cells[indexCol++].Value.ToString());
                pointpos.y = Convert.ToDouble(dataGridView.Rows[i].Cells[indexCol++].Value.ToString());
                pointpos.z = Convert.ToDouble(dataGridView.Rows[i].Cells[indexCol++].Value.ToString());

                dispCalibParam.SetDispenseOtherPoint(new PointDispense()
                {
                    strPointName = strPointName,
                    MachinePoint = pointpos,
                });
            }
            try
            {
                dDstExposure = dDstExposure;
                dDstGain = dDstGain;
                dCalibExposure = dCalibExposure;
                dCalibGain = dCalibGain;
                dXStep = dXStep;
                dYStep = dYStep;
                dPinOffsetX = dPinOffsetX;
                dPinOffsetY = dPinOffsetY;
                dLaserOffsetX = dLaserOffsetX;
                dLaserOffsetY = dLaserOffsetY;
                dNeedleZLatchHeight = dNeedleZLatchHeight;
            }
            catch (Exception e)
            {
                MessageBox.Show($"保存点胶标定参数中异常：" + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public int AxisX
        {
            set
            {
                dispConfig.AxisX = value;
            }
            get
            {
                return dispConfig.AxisX;
            }
        }

        public int AxisY
        {
            set
            {
                dispConfig.AxisY = value;
            }
            get
            {
                return dispConfig.AxisY;
            }
        }

        public int AxisZ
        {
            set
            {
                dispConfig.AxisZ = value;
            }
            get
            {
                return dispConfig.AxisZ;
            }
        }

        public int AxisU
        {
            set
            {
                dispConfig.AxisU = value;
            }
            get
            {
                return dispConfig.AxisU;
            }
        }

        public bool IsHaveLaset
        {
            set
            {
                dispConfig.IsHaveLaset = value;
            }
            get
            {
                return dispConfig.IsHaveLaset;
            }
        }//是否含有镭射

        public bool IsIoTriggerLight
        {
            set
            {
                dispConfig.IsIoTriggerLight = value;
            }
            get
            {
                return dispConfig.IsIoTriggerLight;
            }
        } // 是否使用IO触发光源

        public bool IsComTriggerLight
        {
            set
            {
                dispConfig.IsComTriggerLight = value;
            }
            get
            {
                return dispConfig.IsComTriggerLight;
            }
        } // 是否使用Com触发光源

        public string TriggerLightIoName
        {
            set
            {
                dispConfig.TriggerLightIoName = value;
            }
            get
            {
                return dispConfig.TriggerLightIoName;
            }
        }//触发光源的IO

        public string DispModleName
        {
            set
            {
                dispConfig.DispModleName = value;
            }
            get
            {
                return dispConfig.DispModleName;
            }
        } // 点胶模块名

        public string TopCamName
        {
            set
            {
                dispConfig.TopCamName = value;
            }
            get
            {
                return dispConfig.TopCamName;
            }
        }

        public void ChangeState(Label labelControl, bool bval)
        {
            labelControl.Text = bval ? "ON" : "OFF";
            labelControl.BackColor = bval ? Color.LightGreen : Color.LightBlue;
        }

        private void ChangeStateSeverOnBtn(Button button, bool bval)
        {
            button.Text = bval ? "伺服ON" : "伺服OFF";
            button.BackColor = bval ? Color.LightGreen : Color.LightBlue;
        }

        private void ChangeAxisPos(Label label, double pos)
        {
            label.Text = pos.ToString();
        }

        private void ChangeMotionConfig(int nAxis, AxisConfig axisConfig)
        {
            if (AxisX == nAxis)
                button_ServoOnX.Enabled = ((axisConfig.motorType >= MotorType.SEVER) ? true : false);

            if (AxisY == nAxis)
                button_ServoOnY.Enabled = ((axisConfig.motorType >= MotorType.SEVER) ? true : false);

            if (AxisZ == nAxis)
                button_ServoOnZ.Enabled = ((axisConfig.motorType >= MotorType.SEVER) ? true : false);

            if (AxisU == nAxis)
                button_ServoOnU.Enabled = ((axisConfig.motorType >= MotorType.SEVER) ? true : false);
        }

        public LightControler lightControler = null;

        public void UpdataDispData()
        {
            Read();
            InitCtr();
        }

        public void ChangeMotionIoStateAndPos(int index, bool[] bChangeBitGet, AxisIOState axisIOState, AxisPos axisPos)
        {
            bool[] bChangeBit = new bool[bChangeBitGet.Length];
            for (int i = 0; i < bChangeBit.Length; i++)
                bChangeBit[i] = bChangeBitGet[i];
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ChangeMotionIoStateAndPos(index, bChangeBit, axisIOState, axisPos)));
            }
            else
            {
                if (index == AxisX)
                {
                    if (bChangeBit[0])
                        ChangeStateSeverOnBtn(button_ServoOnX, axisIOState._bSeverOn);
                    if (bChangeBit[1])
                        ChangeState(labelControl_AlarmX, axisIOState._bAlarm);
                    if (bChangeBit[2])
                        ChangeState(labelControl_LimtPX, axisIOState._bLimtP);
                    if (bChangeBit[3])
                        ChangeState(labelControl_LimtNX, axisIOState._bLimtN);
                    if (bChangeBit[4])
                        ChangeState(labelControl_ORIX, axisIOState._bOrg);
                    if (bChangeBit[5])
                        ChangeState(labelControl_EMGX, axisIOState._bEmg);
                    if (bChangeBit[6])
                        ChangeAxisPos(label_ActPosX, axisPos._lActPos);
                    if (bChangeBit[7])
                        ChangeAxisPos(label_CmdPosX, axisPos._lCmdPos);
                }
                if (index == AxisY)
                {
                    if (bChangeBit[0])
                        ChangeStateSeverOnBtn(button_ServoOnY, axisIOState._bSeverOn);
                    if (bChangeBit[1])
                        ChangeState(labelControl_AlarmY, axisIOState._bAlarm);
                    if (bChangeBit[2])
                        ChangeState(labelControl_LimtPY, axisIOState._bLimtP);
                    if (bChangeBit[3])
                        ChangeState(labelControl_LimtNY, axisIOState._bLimtN);
                    if (bChangeBit[4])
                        ChangeState(labelControl_ORIY, axisIOState._bOrg);
                    if (bChangeBit[5])
                        ChangeState(labelControl_EMGY, axisIOState._bEmg);
                    if (bChangeBit[6])
                        ChangeAxisPos(label_ActPosY, axisPos._lActPos);
                    if (bChangeBit[7])
                        ChangeAxisPos(label_CmdPosY, axisPos._lCmdPos);
                }
                if (index == AxisZ)
                {
                    if (bChangeBit[0])
                        ChangeStateSeverOnBtn(button_ServoOnZ, axisIOState._bSeverOn);
                    if (bChangeBit[1])
                        ChangeState(labelControl_AlarmZ, axisIOState._bAlarm);
                    if (bChangeBit[2])
                        ChangeState(labelControl_LimtPZ, axisIOState._bLimtP);
                    if (bChangeBit[3])
                        ChangeState(labelControl_LimtNZ, axisIOState._bLimtN);
                    if (bChangeBit[4])
                        ChangeState(labelControl_ORIZ, axisIOState._bOrg);
                    if (bChangeBit[5])
                        ChangeState(labelControl_EMGZ, axisIOState._bEmg);
                    if (bChangeBit[6])
                        ChangeAxisPos(label_ActPosZ, axisPos._lActPos);
                    if (bChangeBit[7])
                        ChangeAxisPos(label_CmdPosZ, axisPos._lCmdPos);
                }
                if (index == AxisU)
                {
                    if (bChangeBit[0])
                        ChangeStateSeverOnBtn(button_ServoOnU, axisIOState._bSeverOn);
                    if (bChangeBit[1])
                        ChangeState(labelControl_AlarmU, axisIOState._bAlarm);
                    if (bChangeBit[2])
                        ChangeState(labelControl_LimtPU, axisIOState._bLimtP);
                    if (bChangeBit[3])
                        ChangeState(labelControl_LimtNU, axisIOState._bLimtN);
                    if (bChangeBit[4])
                        ChangeState(labelControl_ORIU, axisIOState._bOrg);
                    if (bChangeBit[5])
                        ChangeState(labelControl_EMGU, axisIOState._bEmg);
                    if (bChangeBit[6])
                        ChangeAxisPos(label_ActPosU, axisPos._lActPos);
                    if (bChangeBit[7])
                        ChangeAxisPos(label_CmdPosU, axisPos._lCmdPos);
                }
            }
        }

        private void UpdataAxisIoAndPos(int nAxisNo, AxisIOState axisIOState, AxisPos pos)
        {
            if (nAxisNo == -1)
                return;

            if (nAxisNo == AxisX)
            {
                ChangeStateSeverOnBtn(button_ServoOnX, axisIOState._bSeverOn);
                ChangeState(labelControl_AlarmX, axisIOState._bAlarm);
                ChangeState(labelControl_LimtPX, axisIOState._bLimtP);
                ChangeState(labelControl_LimtNX, axisIOState._bLimtN);
                ChangeState(labelControl_ORIX, axisIOState._bOrg);
                ChangeState(labelControl_EMGX, axisIOState._bEmg);
                ChangeAxisPos(label_ActPosX, pos._lActPos);
                ChangeAxisPos(label_CmdPosX, pos._lCmdPos);
            }
            if (nAxisNo == AxisY)
            {
                ChangeStateSeverOnBtn(button_ServoOnY, axisIOState._bSeverOn);
                ChangeState(labelControl_AlarmY, axisIOState._bAlarm);
                ChangeState(labelControl_LimtPY, axisIOState._bLimtP);
                ChangeState(labelControl_LimtNY, axisIOState._bLimtN);
                ChangeState(labelControl_ORIY, axisIOState._bOrg);
                ChangeState(labelControl_EMGY, axisIOState._bEmg);
                ChangeAxisPos(label_ActPosY, pos._lActPos);
                ChangeAxisPos(label_CmdPosY, pos._lCmdPos);
            }
            if (nAxisNo == AxisZ)
            {
                ChangeStateSeverOnBtn(button_ServoOnZ, axisIOState._bSeverOn);
                ChangeState(labelControl_AlarmZ, axisIOState._bAlarm);
                ChangeState(labelControl_LimtPZ, axisIOState._bLimtP);
                ChangeState(labelControl_LimtNZ, axisIOState._bLimtN);
                ChangeState(labelControl_ORIZ, axisIOState._bOrg);
                ChangeState(labelControl_EMGZ, axisIOState._bEmg);
                ChangeAxisPos(label_ActPosZ, pos._lActPos);
                ChangeAxisPos(label_CmdPosZ, pos._lCmdPos);
            }
            if (nAxisNo == AxisU)
            {
                ChangeStateSeverOnBtn(button_ServoOnU, axisIOState._bSeverOn);
                ChangeState(labelControl_AlarmU, axisIOState._bAlarm);
                ChangeState(labelControl_LimtPU, axisIOState._bLimtP);
                ChangeState(labelControl_LimtNU, axisIOState._bLimtN);
                ChangeState(labelControl_ORIU, axisIOState._bOrg);
                ChangeState(labelControl_EMGU, axisIOState._bEmg);
                ChangeAxisPos(label_ActPosU, pos._lActPos);
                ChangeAxisPos(label_CmdPosU, pos._lCmdPos);
            }
        }

        private void UpdataMotion()
        {
            AxisIOState axisIOState = new AxisIOState();
            AxisPos axisPos = new AxisPos();
            AxisConfig axisConfig = new AxisConfig();

            MotionMgr.GetInstace().GetAxisIOState(AxisX, ref axisIOState);
            MotionMgr.GetInstace().GetAxisPos(AxisX, ref axisPos);
            UpdataAxisIoAndPos(AxisX, axisIOState, axisPos);
            if (AxisX == -1 || MotionMgr.GetInstace().GetAxisConfig(AxisX, ref axisConfig))
            {
                button_ServoOnX.Enabled = axisConfig.motorType >= MotorType.SEVER ? true : false;
                label_ActPosX.Enabled = false;
                label_CmdPosX.Enabled = false;
                labelControl_AlarmX.Enabled = false;
                labelControl_LimtPX.Enabled = false;
                labelControl_LimtNX.Enabled = false;
                labelControl_ORIX.Enabled = false;
                labelControl_EMGX.Enabled = false;
            }

            MotionMgr.GetInstace().GetAxisIOState(AxisY, ref axisIOState);
            MotionMgr.GetInstace().GetAxisPos(AxisY, ref axisPos);
            UpdataAxisIoAndPos(AxisY, axisIOState, axisPos);

            if (AxisY == -1 || MotionMgr.GetInstace().GetAxisConfig(AxisY, ref axisConfig))
            {
                button_ServoOnY.Enabled = axisConfig.motorType >= MotorType.SEVER ? true : false;
                label_ActPosY.Enabled = false;
                label_CmdPosY.Enabled = false;
                labelControl_AlarmY.Enabled = false;
                labelControl_LimtPY.Enabled = false;
                labelControl_LimtNY.Enabled = false;
                labelControl_ORIY.Enabled = false;
                labelControl_EMGY.Enabled = false;
            }

            MotionMgr.GetInstace().GetAxisIOState(AxisZ, ref axisIOState);
            MotionMgr.GetInstace().GetAxisPos(AxisZ, ref axisPos);
            UpdataAxisIoAndPos(AxisZ, axisIOState, axisPos);
            if (AxisZ == -1 || MotionMgr.GetInstace().GetAxisConfig(AxisZ, ref axisConfig))
            {
                button_ServoOnZ.Enabled = axisConfig.motorType >= MotorType.SEVER ? true : false;
                label_ActPosZ.Enabled = false;
                label_CmdPosZ.Enabled = false;
                labelControl_AlarmZ.Enabled = false;
                labelControl_LimtPZ.Enabled = false;
                labelControl_LimtNZ.Enabled = false;
                labelControl_ORIZ.Enabled = false;
                labelControl_EMGZ.Enabled = false;
            }

            MotionMgr.GetInstace().GetAxisIOState(AxisU, ref axisIOState);
            MotionMgr.GetInstace().GetAxisPos(AxisU, ref axisPos);
            UpdataAxisIoAndPos(AxisU, axisIOState, axisPos);
            if (AxisU == -1 || MotionMgr.GetInstace().GetAxisConfig(AxisU, ref axisConfig))
            {
                button_ServoOnU.Enabled = axisConfig.motorType >= MotorType.SEVER ? true : false;
                label_ActPosU.Enabled = false;
                label_CmdPosU.Enabled = false;
                labelControl_AlarmU.Enabled = false;
                labelControl_LimtPU.Enabled = false;
                labelControl_LimtNU.Enabled = false;
                labelControl_ORIU.Enabled = false;
                labelControl_EMGU.Enabled = false;
            }

            //工站IO 更新
            //foreach (var tem in m_Stationbase.m_listIoInput)
            //    if (m_dicInput.ContainsKey(tem))
            //        userPanel_Input.SetLebalState(tem, IOMgr.GetInstace().ReadIoInBit(tem));

            //foreach (var tem in m_Stationbase.m_listIoOutput)
            //    if (m_dicOutput.ContainsKey(tem))
            //        userBtnPanel_Output.SetBtnState(tem, IOMgr.GetInstace().ReadIoOutBit(tem));
        }

        public void UpdataDataTraceDataGridView(string OperateName, object obj)
        {
            if (OperateName == ("Del"))
            {
                int nRow = (int)obj;
                dataGridViewDispTrace.Rows.RemoveAt(nRow);
            }
            else if (OperateName == ("Add"))
            {
                DispTraceBaseElement dispTraceBaseElement = (DispTraceBaseElement)obj;
                string typeofElement = "";
                if (dispTraceBaseElement.strType.Contains("Line"))
                    typeofElement = "线段";
                if (dispTraceBaseElement.strType.Contains("Point"))
                    typeofElement = "点";
                if (dispTraceBaseElement.strType.Contains("Arc"))
                    typeofElement = "圆弧";
                dataGridViewDispTrace.Rows.Add(dispTraceBaseElement.ItemName, typeofElement, "设置");
            }
            else if (OperateName == ("Clr"))
            {
                dataGridViewDispTrace.Rows.Clear();
            }
        }

        public void UpdataDataCalibDataGridView(string OperateName, object obj)
        {
            if (OperateName == ("SetDispenseCalibsPoint"))
            {
                object[] objarr = (object[])obj;
                int nRow = (int)objarr[0];
                PointDispense pointDispense = (PointDispense)objarr[1];
                dataGridView_PointInfo.Rows[nRow].Cells[0].Value = pointDispense.strPointName;
                dataGridView_PointInfo.Rows[nRow].Cells[1].Value = pointDispense.MachinePoint.x;
                dataGridView_PointInfo.Rows[nRow].Cells[2].Value = pointDispense.MachinePoint.y;
                dataGridView_PointInfo.Rows[nRow].Cells[3].Value = pointDispense.MachinePoint.z;
            }
            else if (OperateName == ("Add"))
            {
            }
            else if (OperateName == ("Clr"))
            {
                dataGridViewDispTrace.Rows.Clear();
            }
        }

        private void DispenseCtrl_Load(object sender, EventArgs e)
        {
            InitCtr();
            ParamSetMgr.GetInstance().m_eventLoadProductFileUpadata += UpdataDispData;
            DispTraceMgr.GetInstance().eventUpdataGridView += UpdataDataTraceDataGridView;
            dispCalibParam.eventUpdataGridViewForCalib += UpdataDataCalibDataGridView;
            dispCalibParam.eventUpdataGridViewForCalib += UpdataDataCalibDataGridView;

            dataGridView_PointInfo.Columns[0].ReadOnly = true;
            dataGridView_DispPosList.Columns[0].ReadOnly = true;

            bool bvisible = false;
            if (IsHaveLaset)
            {
                bvisible = true;
                BtnLaserCalib.Visible = bvisible;
                BtnLaserTest.Visible = bvisible;
                btnLaserReadData.Visible = bvisible;
                txtLaserReadData.Visible = bvisible;
            }
            else
            {
                bvisible = false;
                BtnLaserCalib.Visible = bvisible;
                BtnLaserTest.Visible = bvisible;
                btnLaserReadData.Visible = bvisible;
                txtLaserReadData.Visible = bvisible;
            }
            if (IsComTriggerLight)
            {
                bvisible = true;
                labelLightDispCalib.Visible = bvisible;
                textBox_DispCalbLightVal.Visible = bvisible;
                labelLightDispDst.Visible = bvisible;
                textBox_DispDstLightVal.Visible = bvisible;
            }
            else
            {
                bvisible = false;
                labelLightDispCalib.Visible = bvisible;
                textBox_DispCalbLightVal.Visible = bvisible;
                labelLightDispDst.Visible = bvisible;
                textBox_DispDstLightVal.Visible = bvisible;
            }
            if (lightControler != null)
            {
                //lightControler.SetItem("点胶标定光源", 1, 0);
                //lightControler.SetItem("点胶光源", 1, 0);
            }
            AxisU = -1;
            if (AxisU == -1)
            {
                button_Upositive.Enabled = false;
                button_Unegtive.Enabled = false;
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
                // userPanel_Input.SetLebalState(IoName, bStateCurrent);
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
                // userBtnPanel_Output.SetBtnState(IoName, bStateCurrent);
            }
        }

        private void button_ServoOnX_Click(object sender, EventArgs e)
        {
            if (button_ServoOnX.Text == "伺服OFF")
                MotionMgr.GetInstace().ServoOn((short)AxisX);
            else
                MotionMgr.GetInstace().ServoOff((short)AxisX);
        }

        private void button_ServoOnY_Click(object sender, EventArgs e)
        {
            if (button_ServoOnY.Text == "伺服OFF")
                MotionMgr.GetInstace().ServoOn((short)AxisY);
            else
                MotionMgr.GetInstace().ServoOff((short)AxisY);
        }

        private void button_ServoOnZ_Click(object sender, EventArgs e)
        {
            if (button_ServoOnZ.Text == "伺服OFF")
                MotionMgr.GetInstace().ServoOn((short)AxisZ);
            else
                MotionMgr.GetInstace().ServoOff((short)AxisZ);
        }

        private void button_ServoOnU_Click(object sender, EventArgs e)
        {
            if (button_ServoOnU.Text == "伺服OFF")
                MotionMgr.GetInstace().ServoOn((short)AxisU);
            else
                MotionMgr.GetInstace().ServoOff((short)AxisU);
        }

        public void HomeAction(int nAxisNo, Button button)
        {
            bStopMove = false;
            button.Text = "回0中...";
            button.BackColor = Color.LightBlue;
            button.Enabled = false;
            bool bHomeOK = false;
            Task ts = new Task(delegate ()
            {
                AxisConfig axiscofig = new AxisConfig();
                MotionMgr.GetInstace().GetAxisConfig(nAxisNo, ref axiscofig);
                // if (axiscofig.motorType == MotorType.SEVER)
                //     MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                bHomeOK = MotionMgr.GetInstace().Home(nAxisNo, 0);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                while (!MotionMgr.GetInstace().IsHomeNormalStop(nAxisNo))
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000 || bStopMove)
                    {
                        MotionMgr.GetInstace().StopAxis(nAxisNo);
                        MessageBox.Show(MotionMgr.GetInstace().GetAxisName(nAxisNo) + "90秒超时" + "回原点失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MotionMgr.GetInstace().ReasetAxisHomeFinishFlag(nAxisNo);
                        goto Exception_ENDX;
                    }
                }
                Thread.Sleep(800);
                if (bHomeOK)
                {
                    MotionMgr.GetInstace().SetAxisActualPos(nAxisNo, 0);
                    MotionMgr.GetInstace().SetAxisCmdPos(nAxisNo, 0);
                    MotionMgr.GetInstace().SetAxisHomeFinishFlag(nAxisNo);
                }
            Exception_ENDX:
                button.Invoke(
                      new Action(() =>
                      {
                          button.Enabled = true;
                          button.Text = "回原点";
                          button.BackColor = Color.LightGreen;
                      })
                 );
            });
        }

        private void button_homeX_Click(object sender, EventArgs e)
        {
            bStopMove = false;
            int nAxisNo = AxisX;
            HomeAction(nAxisNo, button_homeX);
        }

        private void button_homeY_Click(object sender, EventArgs e)
        {
            bStopMove = false;
            int nAxisNo = AxisY;
            HomeAction(nAxisNo, button_homeY);
        }

        private void button_homeZ_Click(object sender, EventArgs e)
        {
            bStopMove = false;
            int nAxisNo = AxisZ;

            HomeAction(nAxisNo, button_homeY);
        }

        private void button_homeU_Click(object sender, EventArgs e)
        {
            bStopMove = false;
            int nAxisNo = AxisU;
            //  bool isori = MotionMgr.GetInstace().isOrgTrig(nAxisNo);
            button_homeU.Text = "回0中...";
            button_homeU.BackColor = Color.LightBlue;
            button_homeU.Enabled = false;

            Task ts = new Task(delegate ()
            {
                AxisConfig axiscofig = new AxisConfig();
                MotionMgr.GetInstace().GetAxisConfig(nAxisNo, ref axiscofig);
                //   if (axiscofig.motorType == MotorType.SEVER)
                //       MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                MotionMgr.GetInstace().Home(nAxisNo, 0);
            });
            ts.Start();
            ts.ContinueWith((ts1) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                while (!MotionMgr.GetInstace().IsHomeNormalStop(nAxisNo))
                {
                    if (stopwatch.ElapsedMilliseconds > 90 * 1000 || bStopMove)
                    {
                        MotionMgr.GetInstace().StopAxis(nAxisNo);
                        MessageBox.Show(MotionMgr.GetInstace().GetAxisName(nAxisNo) + "90秒超时" + "回原点失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MotionMgr.GetInstace().ReasetAxisHomeFinishFlag(nAxisNo);
                        goto Exception_ENDU;
                    }
                }
                Thread.Sleep(300);
                MotionMgr.GetInstace().SetAxisActualPos(nAxisNo, 0);
                MotionMgr.GetInstace().SetAxisCmdPos(nAxisNo, 0);
                MotionMgr.GetInstace().SetAxisHomeFinishFlag(nAxisNo);
            Exception_ENDU:
                button_homeU.Invoke(
                         new Action(() =>
                         {
                             button_homeU.Enabled = true;
                             button_homeU.BackColor = Color.LightGreen;
                             button_homeU.Text = "回原点";
                         })
                    );
            });
        }

        private void VisionCalibSet_Click(object sender, EventArgs e)
        {
        }

        public bool InitFinished
        {
            private set;
            get;
        } = false;

        public UserRight userRight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Read()
        {
            string currentDispProductFile = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + DispModleName + ("\\") + DispModleName + ".json";
            string strvisionPath = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + DispModleName + ("\\");

            if (!Directory.Exists(ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile) ||
                ParamSetMgr.GetInstance().CurrentProductFile == null ||
                ParamSetMgr.GetInstance().CurrentProductFile == "")
            {
                MessageBox.Show("当前产品文件夹不存在，请创建载入或者创建当前产品", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(strvisionPath + "点胶标定\\"))
            {
                Directory.CreateDirectory(strvisionPath + "点胶标定\\");
            }
            if (!Directory.Exists(strvisionPath + "目标设定\\"))
            {
                Directory.CreateDirectory(strvisionPath + "目标设定\\");
            }
            DispCalibParam dispCalibParamtemp = DispCalibParam.Read(currentDispProductFile);

            if (dispCalibParamtemp != null)
            {
                dispCalibParam = dispCalibParamtemp;
                dispCalibParamtemp.FileSavePath = currentDispProductFile;
            }
            else
            {
                dispCalibParam.Save();
            }

            shapeDispCalib.strSavePath = visionMatchSetCtr1.strPath = strvisionPath + "点胶标定";
            shapeDst.strSavePath = visionMatchSetCtr2.strPath = strvisionPath + "目标设定";

            dispCalibParam.FileSavePath = strvisionPath + this.DispModleName + ".xml";

            shapeDispCalib.Read(strvisionPath + "点胶标定");

            shapeDst.Read(strvisionPath + "目标设定");
            if (dispCalibParam != null)
            {
                int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "对针点");
                if (nIndex == -1)
                    MessageBox.Show("不存在对针点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    PointDispense pointDispenseCalibPin = dispCalibParam.pointDispenseCalibs[nIndex];
                    XYUR_Pin.CalibPoint = pointDispenseCalibPin.MachinePoint;
                }
                nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "对镭射点");
                if (nIndex == -1)
                    MessageBox.Show("不存在对针点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    PointDispense pointDispenseCalibPin = dispCalibParam.pointDispenseCalibs[nIndex];
                    XYUR_Laser.CalibPoint = pointDispenseCalibPin.MachinePoint;
                }
            }
            HTuple hTupleVrow, hTupleVcol, hTupleMx, hTupleMy;
            string strvisioncalibPath = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + DispModleName + ("\\");
            if (File.Exists(strvisioncalibPath + "点胶标定\\" + "VM_VRow.tup")
                && File.Exists(strvisioncalibPath + "点胶标定\\" + "VM_VCol.tup")
                && File.Exists(strvisioncalibPath + "点胶标定\\" + "VM_Mx.tup")
                && File.Exists(strvisioncalibPath + "点胶标定\\" + "VM_My.tup")
                )
            {
                HOperatorSet.ReadTuple(strvisioncalibPath + "点胶标定\\" + "VM_VRow.tup", out hTupleVrow);
                HOperatorSet.ReadTuple(strvisioncalibPath + "点胶标定\\" + "VM_VCol.tup", out hTupleVcol);
                HOperatorSet.ReadTuple(strvisioncalibPath + "点胶标定\\" + "VM_Mx.tup", out hTupleMx);
                HOperatorSet.ReadTuple(strvisioncalibPath + "点胶标定\\" + "VM_My.tup", out hTupleMy);
                XYUR_Pin.CreateURCoor(hTupleVcol, hTupleVrow, hTupleMx, hTupleMy);
            }
            strvisioncalibPath = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + DispModleName + ("\\");
            if (File.Exists(strvisioncalibPath + "点胶标定\\" + "VM_LaserVRow.tup")
                && File.Exists(strvisioncalibPath + "点胶标定\\" + "VM_LaserVCol.tup")
                && File.Exists(strvisioncalibPath + "点胶标定\\" + "VM_LaserMx.tup")
                && File.Exists(strvisioncalibPath + "点胶标定\\" + "VM_LaserMy.tup")
                )
            {
                HOperatorSet.ReadTuple(strvisioncalibPath + "点胶标定\\" + "VM_LaserVRow.tup", out hTupleVrow);
                HOperatorSet.ReadTuple(strvisioncalibPath + "点胶标定\\" + "VM_LaserVCol.tup", out hTupleVcol);
                HOperatorSet.ReadTuple(strvisioncalibPath + "点胶标定\\" + "VM_LaserMx.tup", out hTupleMx);
                HOperatorSet.ReadTuple(strvisioncalibPath + "点胶标定\\" + "VM_LaserMy.tup", out hTupleMy);
                XYUR_Laser.CreateURCoor(hTupleVcol, hTupleVrow, hTupleMx, hTupleMy);
            }
        }

        public void InitCtr()
        {
            if (!visionControl1.isOpen())
                visionControl1.InitWindow();

            visionMatchSetCtr1.FlushToDlg(shapeDispCalib, visionControl1);
            visionMatchSetCtr2.FlushToDlg(shapeDst, visionControl1);
            FlushToScreen();
            InitFinished = true;
        }

        private void button_RecordPoint_Click(object sender, EventArgs e)
        {
            int indexRow = 0;
            string strCalibPointName = "";
            if (rightTab1.SelectedIndex == 0)
            {
                if (dataGridView_PointInfo.CurrentCell != null)
                {
                    indexRow = dataGridView_PointInfo.CurrentCell.RowIndex;
                    strCalibPointName = dataGridView_PointInfo.Rows[indexRow].Cells[0].Value.ToString();
                    int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == strCalibPointName);
                    if (nIndex != -1)
                    {
                        dispCalibParam.pointDispenseCalibs[nIndex] = new PointDispense()
                        {
                            strPointName = strCalibPointName,
                            MachinePoint = new XYUZPoint()
                            {
                                x = MotionMgr.GetInstace().GetAxisPos(AxisX),
                                y = MotionMgr.GetInstace().GetAxisPos(AxisY),
                                z = MotionMgr.GetInstace().GetAxisPos(AxisZ),
                            }
                        };
                        for (nIndex = 0; nIndex < dataGridView_PointInfo.Rows.Count; nIndex++)
                        {
                            if (dataGridView_PointInfo.Rows[nIndex].Cells[0].Value.ToString() == strCalibPointName)
                            {
                                dataGridView_PointInfo.Rows[nIndex].Cells[1].Value = MotionMgr.GetInstace().GetAxisPos(AxisX);
                                dataGridView_PointInfo.Rows[nIndex].Cells[2].Value = MotionMgr.GetInstace().GetAxisPos(AxisY);
                                dataGridView_PointInfo.Rows[nIndex].Cells[3].Value = MotionMgr.GetInstace().GetAxisPos(AxisZ);
                                break;
                            }
                        }
                    }
                    else
                    {
                        dispCalibParam.pointDispenseCalibs.Add(new PointDispense()
                        {
                            strPointName = strCalibPointName,
                            MachinePoint = new XYUZPoint()
                            {
                                x = MotionMgr.GetInstace().GetAxisPos(AxisX),
                                y = MotionMgr.GetInstace().GetAxisPos(AxisY),
                                z = MotionMgr.GetInstace().GetAxisPos(AxisZ),
                            }
                        });
                        for (nIndex = 0; nIndex < dataGridView_PointInfo.Rows.Count; nIndex++)
                        {
                            if (dataGridView_PointInfo.Rows[nIndex].Cells[0].Value.ToString() == strCalibPointName)
                            {
                                dataGridView_PointInfo.Rows[nIndex].Cells[1].Value = MotionMgr.GetInstace().GetAxisPos(AxisX);
                                dataGridView_PointInfo.Rows[nIndex].Cells[2].Value = MotionMgr.GetInstace().GetAxisPos(AxisY);
                                dataGridView_PointInfo.Rows[nIndex].Cells[3].Value = MotionMgr.GetInstace().GetAxisPos(AxisZ);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
            }
        }

        private async void button_SingleAxisMove_Click(object sender, EventArgs e)
        {
            BtnEable(false);
            int indexCol = -1;
            double val = 0;
            try
            {
                log.Info("SingleAxisMove");
                int indexRow = 0;
                string strPointName = "";
                if (rightTab1.SelectedIndex == 0)
                {
                    if (dataGridView_PointInfo.CurrentCell != null && dataGridView_PointInfo.CurrentCell.Value != null)
                    {
                        indexRow = dataGridView_PointInfo.CurrentCell.RowIndex;
                        strPointName = dataGridView_PointInfo.Rows[indexRow].Cells[0].Value.ToString();
                        indexCol = dataGridView_PointInfo.CurrentCell.ColumnIndex;
                        val = Convert.ToDouble(dataGridView_PointInfo.CurrentCell.Value.ToString());
                    }
                }
                else
                {
                    if (dataGridView_DispPosList.CurrentCell != null && dataGridView_DispPosList.CurrentCell.Value != null)
                    {
                        indexRow = dataGridView_DispPosList.CurrentCell.RowIndex;
                        strPointName = dataGridView_DispPosList.Rows[indexRow].Cells[0].Value.ToString();
                        indexCol = dataGridView_DispPosList.CurrentCell.ColumnIndex;

                        val = Convert.ToDouble(dataGridView_DispPosList.CurrentCell.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("SingleAxisMove" + ex.Message);
                BtnEable();
                return;
            }
            finally
            {
            }
            Task s = Task.Run(() =>
            {
                if (indexCol != 0)
                {
                    switch (indexCol)
                    {
                        case 1:
                            MotionMgr.GetInstace().AbsMove(AxisX, val, (int)SpeedType.Low);
                            break;

                        case 2:
                            MotionMgr.GetInstace().AbsMove(AxisY, val, (int)SpeedType.Low);
                            break;

                        case 3:
                            MotionMgr.GetInstace().AbsMove(AxisZ, val, (int)SpeedType.Low);
                            break;
                    }
                }
            });
            await s;
            BtnEable();
        }

        private async void button_AllAxisMove_Click(object sender, EventArgs e)
        {
            BtnEable(false);
            double val1 = 0;
            double val2 = 0;
            double val3 = 0;
            try
            {
                log.Info("AllAxisMove");
                int indexRow = 0;
                string strPointName = "";
                if (rightTab1.SelectedIndex == 0)
                {
                    if (dataGridView_PointInfo.CurrentCell != null && dataGridView_PointInfo.CurrentCell.Value != null)
                    {
                        indexRow = dataGridView_PointInfo.CurrentCell.RowIndex;
                        strPointName = dataGridView_PointInfo.Rows[indexRow].Cells[0].Value.ToString();
                        int indexCol = dataGridView_PointInfo.CurrentCell.ColumnIndex;
                        val1 = Convert.ToDouble(dataGridView_PointInfo.Rows[indexRow].Cells[1].Value.ToString());
                        val2 = Convert.ToDouble(dataGridView_PointInfo.Rows[indexRow].Cells[2].Value.ToString());
                        val3 = Convert.ToDouble(dataGridView_PointInfo.Rows[indexRow].Cells[3].Value.ToString());
                    }
                }
                else
                {
                    if (dataGridView_DispPosList.CurrentCell != null && dataGridView_DispPosList.CurrentCell.Value != null)
                    {
                        indexRow = dataGridView_DispPosList.CurrentCell.RowIndex;
                        strPointName = dataGridView_DispPosList.Rows[indexRow].Cells[0].Value.ToString();
                        int indexCol = dataGridView_DispPosList.CurrentCell.ColumnIndex;
                        val1 = Convert.ToDouble(dataGridView_DispPosList.Rows[indexRow].Cells[1].Value.ToString());
                        val2 = Convert.ToDouble(dataGridView_DispPosList.Rows[indexRow].Cells[2].Value.ToString());
                        val3 = Convert.ToDouble(dataGridView_DispPosList.Rows[indexRow].Cells[3].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warn("AllAxisMove" + ex.Message);
                BtnEable();
                return;
            }
            finally
            {
            }

            Task s = Task.Run(() =>
            {
                MotionMgr.GetInstace().AbsMove(AxisZ, val3, (int)SpeedType.Low);
                MotionMgr.GetInstace().AbsMove(AxisX, val1, (int)SpeedType.Low);
                MotionMgr.GetInstace().AbsMove(AxisY, val2, (int)SpeedType.Low);
            });
            await s;
            BtnEable();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string strpath = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile;
            if (!Directory.Exists(ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile) ||
                ParamSetMgr.GetInstance().CurrentProductFile == null ||
                ParamSetMgr.GetInstance().CurrentProductFile == "")
            {
                MessageBox.Show("当前产品文件夹不存在，请创建载入或者创建当前产品", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ScreenToData();
            visionMatchSetCtr1.SaveParm(shapeDispCalib);
            visionMatchSetCtr2.SaveParm(shapeDst);

            dispCalibParam?.Save();
            shapeDispCalib.Save();
            shapeDst.Save();

            DispTraceMgr.GetInstance().Save(ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ".json");
        }

        public void GetCameraParam()
        {
            string strCamName = TopCamName;
            CameraBase cameraBase = CameraMgr.GetInstance().GetCamera(strCamName);

            double? valexposure = cameraBase?.GetExposureTime();
            double? valgain = cameraBase?.GetGain();
            textExpsoureTimeVal.Text = valexposure == null ? "0" : valexposure.ToString();
            textGainVal.Text = valgain == null ? "0" : valgain.ToString();
        }

        private void DispenseCtrl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                GetCameraParam();
            }
            else
            {
            }
            if (Visible)
            {
                UpdataMotion();
                MotionMgr.GetInstace().m_eventChangeMotionIoOrPos += ChangeMotionIoStateAndPos;
                MotionMgr.GetInstace().m_eventChangeMotionConfig += ChangeMotionConfig;
                //IOMgr.GetInstace().m_eventIoInputChanageByName += ChangedIoInState;
                IOMgr.GetInstace().m_eventIoOutputChanageByName += ChangedIoOutState;
            }
            else
            {
                MotionMgr.GetInstace().m_eventChangeMotionIoOrPos -= ChangeMotionIoStateAndPos;
                MotionMgr.GetInstace().m_eventChangeMotionConfig -= ChangeMotionConfig;
                //IOMgr.GetInstace().m_eventIoInputChanageByName -= ChangedIoInState;
                IOMgr.GetInstace().m_eventIoOutputChanageByName -= ChangedIoOutState;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
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

        private void BtnRoiplus_Click(object sender, EventArgs e)
        {
            VisionSetpBase visionSetpBase = null;
            string strPath = "";
            string str1 = rightTab1.SelectedIndex == 0 ? "点胶标定" : "目标设定";
            string str = rightTab1.SelectedIndex == 0 ? "点胶标定" : "目标设定";
            if (rightTab1.SelectedIndex == 0)
            {
                strPath = visionMatchSetCtr1.strPath;
                visionSetpBase = shapeDispCalib;
            }
            else
            {
                strPath = visionMatchSetCtr2.strPath;
                visionSetpBase = shapeDst;
            }

            HObject oldRegion = null;
            HObject obj = null;
            try
            {
                //int index = str.LastIndexOf("\\");
                //if(index==-1)
                //{
                //    MessageBox.Show(str1 + ":路径不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //string strpath = str.Substring(0, index);
                BtnEable(false);
                obj = visionControl1.DrawShape();
                if (obj == null)
                    return;
                strPath = strPath + "\\" + visionSetpBase.m_strStepName + "_Roi" + ".hobj";
                ((VisionShapMatch)visionSetpBase).visionShapParam.RoiRegionPath = strPath;
                visionSetpBase.Save();
                visionSetpBase.Read();

                if (File.Exists(strPath))
                {
                    HOperatorSet.ReadRegion(out oldRegion, strPath);
                    if (oldRegion != null && oldRegion.IsInitialized())
                    {
                        HOperatorSet.Union2(oldRegion, obj, out obj);
                        oldRegion?.Dispose();
                    }
                }
                HOperatorSet.WriteRegion(obj, strPath);
                ((VisionShapMatch)visionSetpBase).SetRoiRegion(obj);
                HOperatorSet.DispObj(obj, visionControl1.GetHalconWindow());
                visionSetpBase.Save();
            }
            catch (HalconException e1)
            {
                MessageBox.Show(visionSetpBase.m_strStepName + "画ROi失败" + e1.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                oldRegion?.Dispose();
                obj?.Dispose();
            }
            BtnEable();
        }

        private void RoiSub_Click(object sender, EventArgs e)
        {
            HObject obj = null;
            HObject oldRegion = null;
            VisionSetpBase visionSetpBase = null;
            string strPath = "";
            string str1 = rightTab1.SelectedIndex == 0 ? "点胶标定" : "目标设定";
            string str = rightTab1.SelectedIndex == 0 ? "点胶标定" : "目标设定";
            if (rightTab1.SelectedIndex == 0)
            {
                strPath = visionMatchSetCtr1.strPath;
                visionSetpBase = shapeDispCalib;
            }
            else
            {
                strPath = visionMatchSetCtr2.strPath;
                visionSetpBase = shapeDst;
            }
            //int index = str.LastIndexOf("\\");
            //if (index == -1)
            //{
            //    MessageBox.Show(str1 + ":路径不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //string strPath = str.Substring(0, index);
            strPath = strPath + "\\" + visionSetpBase.m_strStepName + "_Roi" + ".hobj";
            ((VisionShapMatch)visionSetpBase).visionShapParam.RoiRegionPath = strPath;
            visionSetpBase.Read();
            BtnEable(false);
            try
            {
                obj = visionControl1.DrawShape();
                if (obj == null)
                    return;
                if (File.Exists(strPath))
                {
                    HOperatorSet.ReadRegion(out oldRegion, strPath);
                    if (oldRegion != null && oldRegion.IsInitialized())
                    {
                        HOperatorSet.Difference(oldRegion, obj, out obj);
                    }
                }
                HOperatorSet.WriteRegion(obj, strPath);

                ((VisionShapMatch)visionSetpBase).SetRoiRegion(obj);
                HOperatorSet.DispObj(obj, visionControl1.GetHalconWindow());
                visionSetpBase.Save();
            }
            catch (HalconException e1)
            {
                MessageBox.Show(visionSetpBase.m_strStepName + "画ROi失败" + e1.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                oldRegion?.Dispose();
                obj?.Dispose();
            }
            obj.Dispose(); BtnEable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VisionSetpBase visionSetpBase = null;
            if (rightTab1.SelectedIndex == 0)
                visionSetpBase = shapeDispCalib;
            else
                visionSetpBase = shapeDst;
            BtnEable(false);
            Action action = new Action(() => { visionSetpBase.Process_image(visionControl1.Img, visionControl1); });
            action.BeginInvoke((ar) =>
            {
                rightTab1.BeginInvoke(new MethodInvoker(() => { BtnEable(); }));
            }, null);
        }

        private FormMoveOperate formMoveOperate = null;

        private void btnMoveOperate_Click(object sender, EventArgs e)
        {
            if (formMoveOperate == null)
                formMoveOperate = new FormMoveOperate();

            System.Drawing.Point p = rightTab1.Location;
            p.X = p.X + rightTab1.Size.Width / 2 + 200;
            p.Y = rightTab1.Size.Height / 2;
            formMoveOperate.Location = p;

            formMoveOperate.Show();
        }

        private void BtnSanp_Click(object sender, EventArgs e)
        {
            string strCamName = TopCamName;
            CameraBase cameraBase = CameraMgr.GetInstance().GetCamera(strCamName);
            cameraBase.StopGrap();
            cameraBase.BindWindow(visionControl1);
            cameraBase.SetTriggerMode(CameraModeType.Software);
            CameraMgr.GetInstance().GetCamera(strCamName).StartGrab();
            CameraMgr.GetInstance().GetCamera(strCamName).GarbBySoftTrigger();
        }

        private void btnSnapSave_Click(object sender, EventArgs e)
        {
            string strCamName = TopCamName;
            CameraBase cameraBase = CameraMgr.GetInstance().GetCamera(strCamName);
            cameraBase.StopGrap();
            cameraBase.BindWindow(visionControl1);
            cameraBase.SetTriggerMode(CameraModeType.Software);
            CameraMgr.GetInstance().GetCamera(strCamName).StartGrab();

            CameraMgr.GetInstance().SaveImg(strCamName);
            CameraMgr.GetInstance().GetCamera(strCamName).GarbBySoftTrigger();
        }

        public bool CheckAllAxisState(int[] axisarr)
        {
            int axisno = 0;
            bool ballAxisNoState = true;
            for (int i = 0; i < axisarr.Length; i++)
            {
                axisno = axisarr[i];
                ballAxisNoState &= MotionMgr.GetInstace().IsAxisNormalStop(axisno) == AxisState.NormalStop;
            }
            return ballAxisNoState;
        }

        public bool CheckAllAxisState(int[] axisarr, double[] cmppos, double dfine)
        {
            int axisno = 0;
            bool ballAxisNoInPos = true;
            for (int i = 0; i < axisarr.Length; i++)
            {
                axisno = axisarr[i];
                ballAxisNoInPos &= (Math.Abs(MotionMgr.GetInstace().GetAxisPos(axisno) - cmppos[i]) <= dfine);
            }
            return ballAxisNoInPos;
        }

        public bool MoveMulitAxis(int[] axisarr, double[] cmppos, double[] speed, double dfine, int timeout = 60000)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            int axisno = 0;
            bool ballAxisNoInPos = true;
            while (true)
            {
                if (stopwatch.ElapsedMilliseconds > timeout)
                {
                    return false;
                }
                ballAxisNoInPos = true;
                for (int i = 0; i < axisarr.Length; i++)
                {
                    axisno = axisarr[i];
                    if (MotionMgr.GetInstace().IsAxisNormalStop(axisno) > AxisState.NormalStop && stopwatch.ElapsedMilliseconds < timeout * 0.95)
                        MotionMgr.GetInstace().ResetAxis(axisno);
                }
                for (int i = 0; i < axisarr.Length; i++)
                {
                    axisno = axisarr[i];
                    if (MotionMgr.GetInstace().IsAxisNormalStop(axisno) == AxisState.NormalStop &&
                         !(Math.Abs(MotionMgr.GetInstace().GetAxisPos(axisno) - cmppos[i]) <= dfine))
                    {
                        MotionMgr.GetInstace().AbsMove(axisno, cmppos[i], (int)speed[i]);
                    }
                }
                if (CheckAllAxisState(axisarr) && CheckAllAxisState(axisarr, cmppos, dfine))
                    return true;
            }

            return true;
        }

        public bool MoveSigleAxis(int axisNo, double cmppos, double speed, double dfine, int timeout = 60000)
        {
            return MoveMulitAxis(new int[] { axisNo }, new double[] { cmppos }, new double[] { speed }, dfine, timeout);
        }

        private void ContinuousSnap_Click(object sender, EventArgs e)
        {
            string strCamName = TopCamName;
            CameraBase cameraBase = CameraMgr.GetInstance().GetCamera(strCamName);
            cameraBase.StopGrap();
            cameraBase.SetTriggerMode(CameraModeType.Software);

            double valexposure = cameraBase.GetExposureTime();
            double valgain = cameraBase.GetGain();
            cameraBase.BindWindow(visionControl1);
            cameraBase.SetAcquisitionMode();
            cameraBase.StartGrab();
            CameraMgr.GetInstance().SetCamExposure(strCamName, Convert.ToDouble(textExpsoureTimeVal.Text));
            CameraMgr.GetInstance().SetCamGain(strCamName, Convert.ToDouble(textGainVal.Text));
        }

        private async void BtnCalibMotion_Click(object sender, EventArgs e)
        {
            double xstep = Convert.ToDouble(textXStep.Text);
            double ystep = Convert.ToDouble(textYStep.Text);
            int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "标定点");
            if (nIndex == -1)
            {
                MessageBox.Show("不存在标定点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PointDispense pointDispenseCalib = dispCalibParam.pointDispenseCalibs[nIndex];
            Point2d[] point2Ds = new Point2d[] {
                new Point2d( pointDispenseCalib.MachinePoint.x- xstep, pointDispenseCalib.MachinePoint.y - ystep),
                new Point2d( pointDispenseCalib.MachinePoint.x- xstep, pointDispenseCalib.MachinePoint.y ),
                new Point2d( pointDispenseCalib.MachinePoint.x- xstep, pointDispenseCalib.MachinePoint.y + ystep),

                new Point2d( pointDispenseCalib.MachinePoint.x, pointDispenseCalib.MachinePoint.y - ystep),
                new Point2d( pointDispenseCalib.MachinePoint.x, pointDispenseCalib.MachinePoint.y ),
                new Point2d( pointDispenseCalib.MachinePoint.x,pointDispenseCalib.MachinePoint.y+ ystep),

                new Point2d( pointDispenseCalib.MachinePoint.x+ xstep, pointDispenseCalib.MachinePoint.y - ystep),
                new Point2d( pointDispenseCalib.MachinePoint.x+ xstep, pointDispenseCalib.MachinePoint.y),
                new Point2d( pointDispenseCalib.MachinePoint.x+ xstep,pointDispenseCalib.MachinePoint.y + ystep)
            };

            nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "对针点");
            if (nIndex == -1)
            {
                MessageBox.Show("不存在对针点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (IsIoTriggerLight)
            {
                IOMgr.GetInstace().WriteIoBit(TriggerLightIoName, true);
            }

            MoveSigleAxis(AxisZ, pointDispenseCalib.MachinePoint.z, (double)SpeedType.High, 0.02);
            PointDispense pointDispenseCalibPin = dispCalibParam.pointDispenseCalibs[nIndex];
            double zheight = pointDispenseCalib.MachinePoint.z;
            CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
            cam.BindWindow(visionControl1);
            cam.StopGrap();
            cam.SetTriggerMode(CameraModeType.Software);
            cam.SetExposureTime(dispCalibParam.dCalibExposure);
            cam.SetGain(dispCalibParam.dCalibGain);
            cam.StartGrab();
            Stopwatch stopwatch = new Stopwatch();
            HObject img = null;
            BtnEable(false);
            Task task = Task.Run(() =>
            {
                try
                {
                    List<double> Vrow = new List<double>(); Vrow.Clear();
                    List<double> Vcol = new List<double>(); Vcol.Clear();

                    List<double> My = new List<double>(); My.Clear();
                    List<double> Mx = new List<double>(); Mx.Clear();
                    HTuple hTupleVrow = new HTuple();
                    HTuple hTupleVcol = new HTuple();
                    HTuple hTupleMx = new HTuple();
                    HTuple hTupleMy = new HTuple();
                    for (int i = 0; i < point2Ds.Length; i++)
                    {
                        //  Device.GetDevice().FRobot.MoveAbs(point2Ds[i].y, point2Ds[i].x, zheight, null);
                        MoveMulitAxis(new int[] { AxisX, AxisY }, new double[] { point2Ds[i].x, point2Ds[i].y },
                            new double[] { (double)SpeedType.High, (double)SpeedType.Low }, 0.05);
                        Thread.Sleep(20);
                        img = cam.GetImage();
                        if (img == null || !img.IsInitialized())
                        {
                            img = cam.GetImage();
                        }
                        shapeDispCalib.ClearResult();
                        stopwatch.Restart();
                        shapeDispCalib.Process_image(img, visionControl1);
                        VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDispCalib.GetResult();
                        while (true)
                        {
                            visionShapMatchobj = (VisionShapParam)shapeDispCalib.GetResult();
                            if (stopwatch.ElapsedMilliseconds > 3000)
                            {
                                MessageBox.Show($"{i + 1}次拍照,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                                break;
                        }
                        Vrow.Add(visionShapMatchobj.ResultRow[0]);
                        Vcol.Add(visionShapMatchobj.ResultCol[0]);
                        Mx.Add(MotionMgr.GetInstace().GetAxisPos(AxisX));
                        My.Add(MotionMgr.GetInstace().GetAxisPos(AxisY));
                        Thread.Sleep(20);
                    }
                    for (int i = 0; i < Vrow.Count; i++)
                    {
                        hTupleVrow.Append(Vrow[i]);
                        hTupleVcol.Append(Vcol[i]);
                    }
                    for (int i = 0; i < Vrow.Count; i++)
                    {
                        hTupleMx.Append(Mx[i] - pointDispenseCalibPin.MachinePoint.x);
                        hTupleMy.Append(My[i] - pointDispenseCalibPin.MachinePoint.y);
                    }
                    XYUR_Pin.CalibPoint = pointDispenseCalibPin.MachinePoint;
                    XYUR_Pin.CreateURCoor(hTupleVcol, hTupleVrow, hTupleMx, hTupleMy);
                    string strvisionPath = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + DispModleName + ("\\");

                    if (!Directory.Exists(strvisionPath + "点胶标定\\"))
                    {
                        Directory.CreateDirectory(strvisionPath + "点胶标定\\");
                    }
                    HOperatorSet.WriteTuple(hTupleVrow, strvisionPath + "点胶标定\\" + "VM_VRow.tup");
                    HOperatorSet.WriteTuple(hTupleVcol, strvisionPath + "点胶标定\\" + "VM_VCol.tup");
                    HOperatorSet.WriteTuple(hTupleMx, strvisionPath + "点胶标定\\" + "VM_Mx.tup");
                    HOperatorSet.WriteTuple(hTupleMy, strvisionPath + "点胶标定\\" + "VM_My.tup");
                }
                catch (Exception ex)
                {
                    log.Error("相机和点胶针头 标定失败:" + ex.Message);
                    MessageBox.Show("相机和点胶针头 标定失败:" + ex.Message);
                }
                finally
                {
                    if (IsIoTriggerLight)
                    {
                        IOMgr.GetInstace().WriteIoBit(TriggerLightIoName, false);
                    }
                }
            });
            await task;
            BtnEable();
        }

        private async void BtnPinTest_Click(object sender, EventArgs e)
        {
            BtnEable(false);
            Task task = Task.Run(() =>
            {
                try
                {
                    if (IsIoTriggerLight)
                    {
                        IOMgr.GetInstace().WriteIoBit(TriggerLightIoName, true);
                    }
                    int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "标定点");
                    if (nIndex == -1)
                    {
                        MessageBox.Show("不存在标定点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    PointDispense pointDispenseCalib = dispCalibParam.pointDispenseCalibs[nIndex];
                    MoveSigleAxis(AxisZ, pointDispenseCalib.MachinePoint.z, (double)SpeedType.High, 0.02);
                    Stopwatch stopwatch = new Stopwatch();
                    HObject img = null;
                    CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");

                    Thread.Sleep(100);
                    cam.BindWindow(visionControl1);
                    cam.StopGrap();
                    cam.SetTriggerMode(CameraModeType.Software);
                    cam.SetExposureTime(dispCalibParam.dCalibExposure);
                    cam.SetGain(dispCalibParam.dCalibGain);
                    cam.StartGrab();
                    img = cam.GetImage();
                    if (img == null || !img.IsInitialized())
                    {
                        img = cam.GetImage();
                    }
                    shapeDispCalib.ClearResult();
                    stopwatch.Restart();
                    shapeDispCalib.Process_image(img, visionControl1);
                    VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDispCalib.GetResult();
                    while (true)
                    {
                        visionShapMatchobj = (VisionShapParam)shapeDispCalib.GetResult();
                        if (stopwatch.ElapsedMilliseconds > 3000)
                        {
                            MessageBox.Show($"拍照,匹配失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                            break;
                    }
                    double x = 0;
                    double y = 0;
                    x = MotionMgr.GetInstace().GetAxisPos(AxisX);
                    y = MotionMgr.GetInstace().GetAxisPos(AxisY);
                    XYUPoint SnapPoint = new XYUPoint(x, y, 0);
                    XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], 0);
                    XYUPoint DstPoint = XYUR_Pin.GetDstPonit(visionPoint, SnapPoint);
                    MoveMulitAxis(new int[] { AxisX, AxisY }, new double[] { DstPoint.x, DstPoint.y }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 0.02);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (IsIoTriggerLight)
                    {
                        IOMgr.GetInstace().WriteIoBit(TriggerLightIoName, false);
                    }
                }
            });
            await task;
            BtnEable(true);
        }

        public void ReadCalibPin()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Pin\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Pin\");
            }
            try
            {
                HOperatorSet.ReadTuple(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Pin\VM_VRow.tup", out HTuple hTupleVrow);
                HOperatorSet.ReadTuple(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Pin\VM_VCol.tup", out HTuple hTupleVcol);
                HOperatorSet.ReadTuple(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Pin\VM_Mx.tup", out HTuple hTupleMx);
                HOperatorSet.ReadTuple(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Pin\VM_My.tup", out HTuple hTupleMy);
                XYUR_Pin.CreateURCoor(hTupleVcol, hTupleVrow, hTupleMx, hTupleMy);
                int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "对针点");
                if (nIndex != -1)
                {
                    XYUR_Laser.CalibPoint = dispCalibParam.pointDispenseCalibs[nIndex].MachinePoint;
                }
                else
                {
                    MessageBox.Show($"读取标定参数异常：" + "对针点丢失", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"读取标定参数异常：" + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private HTuple m_Hom2Dutr, Hom2D, m_Hom2Drtu;

        public async void ReadCalibLaser()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\");
            }
            try
            {
                HOperatorSet.ReadTuple(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\VM_VRow.tup", out HTuple hTupleVrow);
                HOperatorSet.ReadTuple(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\VM_VCol.tup", out HTuple hTupleVcol);
                HOperatorSet.ReadTuple(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\VM_Mx.tup", out HTuple hTupleMx);
                HOperatorSet.ReadTuple(AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\VM_My.tup", out HTuple hTupleMy);

                HOperatorSet.VectorToHomMat2d(hTupleVcol, hTupleVrow, hTupleMx, hTupleMy, out Hom2D);
                m_Hom2Dutr = Hom2D.Clone();
                HOperatorSet.VectorToHomMat2d(hTupleMx, hTupleMy, hTupleVcol, hTupleVrow, out Hom2D);
                m_Hom2Drtu = Hom2D.Clone();

                XYUR_Laser.CreateURCoor(hTupleVcol, hTupleVrow, hTupleMx, hTupleMy);
                int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "对镭射点");
                if (nIndex != -1)
                {
                    XYUR_Laser.CalibPoint = dispCalibParam.pointDispenseCalibs[nIndex].MachinePoint;
                }
                else
                {
                    MessageBox.Show($"读取标定参数异常：" + "对镭射点丢失", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"读取标定参数异常：" + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnLaserCalib_Click(object sender, EventArgs e)
        {
            double xstep = Convert.ToDouble(textXStep.Text);
            double ystep = Convert.ToDouble(textYStep.Text);
            int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "标定点");
            if (nIndex == -1)
            {
                MessageBox.Show("不存在标定点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PointDispense pointDispenseCalib = dispCalibParam.pointDispenseCalibs[nIndex];
            Point2d[] point2Ds = new Point2d[] {
                new Point2d( pointDispenseCalib.MachinePoint.x- xstep, pointDispenseCalib.MachinePoint.y - ystep),
                new Point2d( pointDispenseCalib.MachinePoint.x- xstep, pointDispenseCalib.MachinePoint.y ),
                new Point2d( pointDispenseCalib.MachinePoint.x- xstep, pointDispenseCalib.MachinePoint.y + ystep),

                new Point2d( pointDispenseCalib.MachinePoint.x, pointDispenseCalib.MachinePoint.y - ystep),
                new Point2d( pointDispenseCalib.MachinePoint.x, pointDispenseCalib.MachinePoint.y ),
                new Point2d( pointDispenseCalib.MachinePoint.x,pointDispenseCalib.MachinePoint.y+ ystep),

                new Point2d( pointDispenseCalib.MachinePoint.x+ xstep, pointDispenseCalib.MachinePoint.y - ystep),
                new Point2d( pointDispenseCalib.MachinePoint.x+ xstep, pointDispenseCalib.MachinePoint.y),
                new Point2d( pointDispenseCalib.MachinePoint.x+ xstep,pointDispenseCalib.MachinePoint.y + ystep)
            };

            nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "对镭射点");
            if (nIndex == -1)
            {
                MessageBox.Show("不存在对镭射点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PointDispense pointDispenseCalibLaser = dispCalibParam.pointDispenseCalibs[nIndex];
            double zheight = pointDispenseCalib.MachinePoint.z;
            CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
            cam.BindWindow(visionControl1);
            cam.StopGrap();
            cam.SetTriggerMode(CameraModeType.Software);
            cam.SetExposureTime(dispCalibParam.dCalibExposure);
            cam.SetGain(dispCalibParam.dCalibGain);
            cam.StartGrab();
            Stopwatch stopwatch = new Stopwatch();
            HObject img = null;
            BtnEable(false);
            //   Device.GetDevice().IO.CCDLight = true;
            Task task1 = Task.Run(() =>
            {
                try
                {
                    //  Device.GetDevice().IO.CCDLight = true;
                    List<double> Vrow = new List<double>(); Vrow.Clear();
                    List<double> Vcol = new List<double>(); Vcol.Clear();

                    List<double> My = new List<double>(); My.Clear();
                    List<double> Mx = new List<double>(); Mx.Clear();
                    HTuple hTupleVrow = new HTuple();
                    HTuple hTupleVcol = new HTuple();
                    HTuple hTupleMx = new HTuple();
                    HTuple hTupleMy = new HTuple();
                    for (int i = 0; i < point2Ds.Length; i++)
                    {
                        //    Device.GetDevice().FRobot.MoveAbs(point2Ds[i].y, point2Ds[i].x, zheight, null);
                        Thread.Sleep(20);
                        img = cam.GetImage();
                        if (img == null || !img.IsInitialized())
                        {
                            img = cam.GetImage();
                        }
                        shapeDispCalib.ClearResult();
                        stopwatch.Restart();
                        shapeDispCalib.Process_image(img, visionControl1);
                        VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDispCalib.GetResult();
                        while (true)
                        {
                            visionShapMatchobj = (VisionShapParam)shapeDispCalib.GetResult();
                            if (stopwatch.ElapsedMilliseconds > 3000)
                            {
                                MessageBox.Show($"{i + 1}次拍照,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                                break;
                        }
                        Vrow.Add(visionShapMatchobj.ResultRow[0]);
                        Vcol.Add(visionShapMatchobj.ResultCol[0]);
                        //   Mx.Add(Device.GetDevice().FRobot.DX.GetCmdPos());
                        //  My.Add(Device.GetDevice().FRobot.Y.GetActPos());
                        Thread.Sleep(20);
                    }
                    for (int i = 0; i < Vrow.Count; i++)
                    {
                        hTupleVrow.Append(Vrow[i]);
                        hTupleVcol.Append(Vcol[i]);
                    }
                    for (int i = 0; i < Vrow.Count; i++)
                    {
                        hTupleMx.Append(Mx[i] - pointDispenseCalibLaser.MachinePoint.x);
                        hTupleMy.Append(My[i] - pointDispenseCalibLaser.MachinePoint.y);
                    }
                    HOperatorSet.WriteTuple(hTupleVrow, AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\VM_VRow.tup");
                    HOperatorSet.WriteTuple(hTupleVcol, AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\VM_VCol.tup");
                    HOperatorSet.WriteTuple(hTupleMx, AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\VM_Mx.tup");
                    HOperatorSet.WriteTuple(hTupleMy, AppDomain.CurrentDomain.BaseDirectory + @"VisionCalb\Laser\VM_My.tup");
                    XYUR_Laser.CalibPoint = pointDispenseCalibLaser.MachinePoint;
                    XYUR_Laser.CreateURCoor(hTupleVcol, hTupleVrow, hTupleMx, hTupleMy);
                }
                catch (Exception ex)
                {
                    log.Error("相机和镭射 标定失败:" + ex.Message);
                }
                finally
                {
                    // Device.GetDevice().IO.CCDLight = false;
                }
            });
            await task1;
            BtnEable();
        }

        private async void BtnLaserTest_Click(object sender, EventArgs e)
        {
            BtnEable(false);
            Task task = Task.Run(() =>
            {
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    HObject img = null;
                    CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
                    cam.BindWindow(visionControl1);
                    //Device.GetDevice().IO.CCDLight = true;
                    Thread.Sleep(100);
                    cam.StopGrap();
                    cam.SetTriggerMode(CameraModeType.Software);
                    cam.SetExposureTime(dispCalibParam.dCalibExposure);
                    cam.SetGain(dispCalibParam.dCalibGain);
                    cam.StartGrab();
                    img = cam.GetImage();
                    if (img == null || !img.IsInitialized())
                    {
                        img = cam.GetImage();
                    }
                    shapeDispCalib.ClearResult();
                    stopwatch.Restart();
                    shapeDispCalib.Process_image(img, visionControl1);
                    VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDispCalib.GetResult();
                    while (true)
                    {
                        visionShapMatchobj = (VisionShapParam)shapeDispCalib.GetResult();
                        if (stopwatch.ElapsedMilliseconds > 3000)
                        {
                            MessageBox.Show($"拍照,匹配失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                            break;
                    }
                    double x = 0;// Device.GetDevice().FRobot.DX.GetCmdPos();
                    double y = 0;// Device.GetDevice().FRobot.Y.GetActPos();
                    XYUPoint SnapPoint = new XYUPoint(x, y, 0);
                    XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], 0);
                    XYUPoint DstPoint = XYUR_Laser.GetDstPonit(visionPoint, SnapPoint);
                    //Device.GetDevice().FRobot.MoveAbs(DstPoint.y, DstPoint.x, -0.5, null);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    // Device.GetDevice().IO.CCDLight = false;
                }
            });
            await task;
            BtnEable();
        }

        public void BtnEable(bool bEable = true)
        {
            rightTab1.Enabled = bEable;
            btnRead.Enabled = bEable;
            BtnSanp.Enabled = bEable;
            btnSnapSave.Enabled = bEable;
            ContinuousSnap.Enabled = bEable;
            BtnRoiplus.Enabled = bEable;
            RoiSub.Enabled = bEable;
            BtnPrTest.Enabled = bEable;
            BtnSave.Enabled = bEable;
        }

#if false

        private async void BtnSnapGoDisoPos1_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                try
                {
                    BtnEable(false);
                    int nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "拍目标点");
                    if (nIndex == -1)
                        return;
                    Device.GetDevice().FRobot.MoveAbs(dispCalibParam.pointDispenses[nIndex].MachinePoint.y, dispCalibParam.pointDispenses[nIndex].MachinePoint.x, -0.5, null);
                    Thread.Sleep(100);
                    Stopwatch stopwatch = new Stopwatch();
                    HObject img = null;
                    CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
                    cam.BindWindow(visionControl1);
                    Device.GetDevice().IO.CCDLight = true;
                    Thread.Sleep(100);
                    cam.StopGrap();
                    cam.SetTriggerMode(CameraModeType.Software);
                    cam.SetExposureTime(dispCalibParam.dCalibExposure);
                    cam.SetGain(dispCalibParam.dCalibGain);
                    cam.StartGrab();
                    img = cam.GetImage();
                    if (img == null || !img.IsInitialized())
                    {
                        img = cam.GetImage();
                    }
                    shapeDst.ClearResult();
                    stopwatch.Restart();
                    shapeDst.Process_image(img, visionControl1);
                    VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                    while (true)
                    {
                        visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                        if (stopwatch.ElapsedMilliseconds > 3000)
                        {
                            MessageBox.Show($"拍照,匹配失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                            break;
                    }
                    double x = Device.GetDevice().FRobot.DX.GetCmdPos();
                    double y = Device.GetDevice().FRobot.Y.GetActPos();
                    XYUPoint SnapPoint = new XYUPoint(x, y, 0);
                    XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], visionShapMatchobj.ResultAngle[0]);

                    nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点1");
                    if (nIndex == -1)
                        return;
                    XYUPoint oldLaserPoint = new XYUPoint(dispCalibParam.pointDispenses[nIndex].MachinePoint.x, dispCalibParam.pointDispenses[nIndex].MachinePoint.y, 0);
                    XYUPoint nowLaserPoint = shapeDst.GetAffineTransPointAffterMatch(oldLaserPoint.x, oldLaserPoint.y, visionPoint, visionControl1);
                    double zDispensePointHeight = dispCalibParam.pointDispenses[nIndex].MachinePoint.z;
                    XYUPoint PinGoDstPos;
                    PinGoDstPos = XYUR_Pin.GetDstPonit(nowLaserPoint, SnapPoint);
                  //  Device.GetDevice().FRobot.MoveAbs(PinGoDstPos.y, PinGoDstPos.x, -0.5, null);
                  //  Device.GetDevice().FRobot.MoveAbs(null, null, zDispensePointHeight, null);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                   // Device.GetDevice().IO.CCDLight = false;
                    BtnEable();
                }
            });
            await task;
        }

        private async void BtnSnapGoDisoPos2_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                try
                {
                    BtnEable(false);
                    int nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "拍目标点");
                    if (nIndex == -1)
                        return;
                    Device.GetDevice().FRobot.MoveAbs(dispCalibParam.pointDispenses[nIndex].MachinePoint.y, dispCalibParam.pointDispenses[nIndex].MachinePoint.x, -0.5, null);
                    Thread.Sleep(100);
                    Stopwatch stopwatch = new Stopwatch();
                    HObject img = null;
                    CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
                    cam.BindWindow(visionControl1);
                    Device.GetDevice().IO.CCDLight = true;
                    Thread.Sleep(100);
                    cam.StopGrap();
                    cam.SetTriggerMode(CameraModeType.Software);
                    cam.SetExposureTime(dispCalibParam.dCalibExposure);
                    cam.SetGain(dispCalibParam.dCalibGain);
                    cam.StartGrab();
                    img = cam.GetImage();
                    if (img == null || !img.IsInitialized())
                    {
                        img = cam.GetImage();
                    }
                    shapeDst.ClearResult();
                    stopwatch.Restart();
                    shapeDst.Process_image(img, visionControl1);
                    VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                    while (true)
                    {
                        visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                        if (stopwatch.ElapsedMilliseconds > 3000)
                        {
                            MessageBox.Show($"拍照,匹配失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                            break;
                    }
                    double x = Device.GetDevice().FRobot.DX.GetCmdPos();
                    double y = Device.GetDevice().FRobot.Y.GetActPos();
                    XYUPoint SnapPoint = new XYUPoint(x, y, 0);
                    XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], visionShapMatchobj.ResultAngle[0]);

                    nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点2");
                    if (nIndex == -1)
                        return;
                    XYUPoint oldLaserPoint = new XYUPoint(dispCalibParam.pointDispenses[nIndex].MachinePoint.x, dispCalibParam.pointDispenses[nIndex].MachinePoint.y, 0);
                    XYUPoint nowLaserPoint = shapeDst.GetAffineTransPointAffterMatch(oldLaserPoint.x, oldLaserPoint.y, visionPoint, visionControl1);
                    double zDispensePointHeight = dispCalibParam.pointDispenses[nIndex].MachinePoint.z;
                    XYUPoint PinGoDstPos;
                    PinGoDstPos = XYUR_Pin.GetDstPonit(nowLaserPoint, SnapPoint);
                    Device.GetDevice().FRobot.MoveAbs(PinGoDstPos.y, PinGoDstPos.x, -0.5, null);
                    Device.GetDevice().FRobot.MoveAbs(null, null, zDispensePointHeight, null);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    Device.GetDevice().IO.CCDLight = false;
                    BtnEable();
                }
            });
            await task;
        }

        private async void BtnSnapGoDisoPos3_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                try
                {
                    BtnEable(false);
                    int nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "拍目标点");
                    if (nIndex == -1)
                        return;
                    Device.GetDevice().FRobot.MoveAbs(dispCalibParam.pointDispenses[nIndex].MachinePoint.y, dispCalibParam.pointDispenses[nIndex].MachinePoint.x, -0.5, null);
                    Thread.Sleep(100);
                    Stopwatch stopwatch = new Stopwatch();
                    HObject img = null;
                    CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
                    cam.BindWindow(visionControl1);
                    Device.GetDevice().IO.CCDLight = true;
                    Thread.Sleep(100);
                    cam.StopGrap();
                    cam.SetTriggerMode(CameraModeType.Software);
                    cam.SetExposureTime(dispCalibParam.dCalibExposure);
                    cam.SetGain(dispCalibParam.dCalibGain);
                    cam.StartGrab();
                    img = cam.GetImage();
                    if (img == null || !img.IsInitialized())
                    {
                        img = cam.GetImage();
                    }
                    shapeDst.ClearResult();
                    stopwatch.Restart();
                    shapeDst.Process_image(img, visionControl1);
                    VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                    while (true)
                    {
                        visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                        if (stopwatch.ElapsedMilliseconds > 3000)
                        {
                            MessageBox.Show($"拍照,匹配失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                            break;
                    }
                    double x = Device.GetDevice().FRobot.DX.GetCmdPos();
                    double y = Device.GetDevice().FRobot.Y.GetActPos();
                    XYUPoint SnapPoint = new XYUPoint(x, y, 0);
                    XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], visionShapMatchobj.ResultAngle[0]);

                    nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点3");
                    if (nIndex == -1)
                        return;
                    XYUPoint oldLaserPoint = new XYUPoint(dispCalibParam.pointDispenses[nIndex].MachinePoint.x, dispCalibParam.pointDispenses[nIndex].MachinePoint.y, 0);
                    XYUPoint nowLaserPoint = shapeDst.GetAffineTransPointAffterMatch(oldLaserPoint.x, oldLaserPoint.y, visionPoint, visionControl1);
                    double zDispensePointHeight = dispCalibParam.pointDispenses[nIndex].MachinePoint.z;
                    XYUPoint PinGoDstPos;
                    PinGoDstPos = XYUR_Pin.GetDstPonit(nowLaserPoint, SnapPoint);
                    Device.GetDevice().FRobot.MoveAbs(PinGoDstPos.y, PinGoDstPos.x, -0.5, null);
                    Device.GetDevice().FRobot.MoveAbs(null, null, zDispensePointHeight, null);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    Device.GetDevice().IO.CCDLight = false;
                    BtnEable();
                }
            });
            await task;
        }

        private async void btnPinHeight_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                double xstep = Convert.ToDouble(textXStep.Text);
                double ystep = Convert.ToDouble(textYStep.Text);
                int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "针头测高点");
                if (nIndex == -1)
                {
                    MessageBox.Show("不存在针头测高点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PointDispense pointneedleHeightDetect = dispCalibParam.pointDispenseCalibs[nIndex];
                //点胶XY去测高位置
                bool Ret = Device.GetDevice().FRobot.MoveAbs(pointneedleHeightDetect.MachinePoint.y, pointneedleHeightDetect.MachinePoint.x, -0.5, null, true);
                if (!Ret)
                {
                    log.Error("点胶XY去针头侧高点失败!");
                    return;
                }
                //下降到预测高高度开启测高
                double zpos = 0;
                Ret = Device.GetDevice().GetDispenseZLatchData(pointneedleHeightDetect.MachinePoint.z, 1, ref zpos);
                if (!Ret)
                {
                    log.Error("针头校准测高失败！");
                    return;
                }
                //保存测高高度值
                txtZLatchData.Text = zpos.ToString();
                dispCalibParam.dNeedleZLatchHeight = zpos;
                dispCalibParam.Save();

                Ret = Device.GetDevice().DisableZLatchAndMovSafe();
                if (!Ret)
                {
                    log.Error("点胶Z轴回安全高度失败！");
                    return;
                }
                //激光移动到压力传感器上方测高
                nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "镭射测高点");
                if (nIndex == -1)
                {
                    MessageBox.Show("不存在镭射测高点,标定失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PointDispense LaserHeightDetect = dispCalibParam.pointDispenseCalibs[nIndex];
                // 点胶XY去测高位置
                Ret = Device.GetDevice().FRobot.MoveAbs(LaserHeightDetect.MachinePoint.y, LaserHeightDetect.MachinePoint.x, LaserHeightDetect.MachinePoint.z, null, true);
                if (!Ret)
                {
                    log.Error("点胶XY去镭射测高点失败!");
                    return;
                }
                //测高延时等待轴稳定
                Thread.Sleep(400);
                double height = 0;
                Ret = ComHLLaser.Instance.GetHighValue(out height);
                if (!Ret)
                {
                    log.Error("激光测高仪读数失败！");
                    return;
                }
                //记录测高读数
                dispCalibParam.dLaserHeightData = height;
                txtLaserReadData.Text = height.ToString();
                //针头和激光高度差
                dispCalibParam.dNeedleLaserHeightOffset = height - LaserHeightDetect.MachinePoint.z;
                dispCalibParam.Save();
            });
            await task;
        }

        public bool GetDispenseHeight(double zpos, double heightValve, ref double DispenseHeight)
        {
            double height = 0;
            double.TryParse(sys.g_dic["GlueParam"]["GlueHeightRelLaser"], out height);
            DispenseHeight = -(heightValve - zpos) + dispCalibParam.dNeedleZLatchHeight + dispCalibParam.dNeedleLaserHeightOffset + height;
            return true;
        }

        private async void BtnSnapGoDisoPos4_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                try
                {
                    BtnEable(false);
                    int nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "拍目标点");
                    if (nIndex == -1)
                        return;
                    Device.GetDevice().FRobot.MoveAbs(dispCalibParam.pointDispenses[nIndex].MachinePoint.y, dispCalibParam.pointDispenses[nIndex].MachinePoint.x, -0.5, null);
                    Thread.Sleep(100);
                    Stopwatch stopwatch = new Stopwatch();
                    HObject img = null;
                    CameraBase cam = CameraMgr.GetInstance().GetCamera("Top");
                    cam.BindWindow(visionControl1);
                    Device.GetDevice().IO.CCDLight = true;
                    Thread.Sleep(100);
                    cam.StopGrap();
                    cam.SetTriggerMode(CameraModeType.Software);
                    cam.SetExposureTime(dispCalibParam.dCalibExposure);
                    cam.SetGain(dispCalibParam.dCalibGain);
                    cam.StartGrab();
                    img = cam.GetImage();
                    if (img == null || !img.IsInitialized())
                    {
                        img = cam.GetImage();
                    }
                    shapeDst.ClearResult();
                    stopwatch.Restart();
                    shapeDst.Process_image(img, visionControl1);
                    VisionShapParam visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                    while (true)
                    {
                        visionShapMatchobj = (VisionShapParam)shapeDst.GetResult();
                        if (stopwatch.ElapsedMilliseconds > 3000)
                        {
                            MessageBox.Show($"拍照,匹配失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (visionShapMatchobj != null && visionShapMatchobj.GetResultNum() > 0)
                            break;
                    }
                    double x = Device.GetDevice().FRobot.DX.GetCmdPos();
                    double y = Device.GetDevice().FRobot.Y.GetActPos();
                    XYUPoint SnapPoint = new XYUPoint(x, y, 0);
                    XYUPoint visionPoint = new XYUPoint(visionShapMatchobj.ResultCol[0], visionShapMatchobj.ResultRow[0], visionShapMatchobj.ResultAngle[0]);

                    nIndex = dispCalibParam.pointDispenses.FindIndex(t => t.strPointName == "矩形顶点4");
                    if (nIndex == -1)
                        return;
                    XYUPoint oldLaserPoint = new XYUPoint(dispCalibParam.pointDispenses[nIndex].MachinePoint.x, dispCalibParam.pointDispenses[nIndex].MachinePoint.y, 0);
                    XYUPoint nowLaserPoint = shapeDst.GetAffineTransPointAffterMatch(oldLaserPoint.x, oldLaserPoint.y, visionPoint, visionControl1);
                    double zDispensePointHeight = dispCalibParam.pointDispenses[nIndex].MachinePoint.z;
                    XYUPoint PinGoDstPos;
                    PinGoDstPos = XYUR_Pin.GetDstPonit(nowLaserPoint, SnapPoint);
                    Device.GetDevice().FRobot.MoveAbs(PinGoDstPos.y, PinGoDstPos.x, -0.5, null);
                    Device.GetDevice().FRobot.MoveAbs(null, null, zDispensePointHeight, null);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    Device.GetDevice().IO.CCDLight = false;
                    BtnEable();
                }
            });
            await task;
        }
        /// <summary>
        /// 所有轴到点胶准备位置
        /// </summary>
        /// <returns></returns>
        public bool AllAXisToCaliReadyPos()
        {
            return true;
        }

        private void btnAllAXisToCaliReadyPos_Click(object sender, EventArgs e)
        {
            if (!AllAXisToCaliReadyPos())
            {
                log.Error("所有轴去点胶准备位置失败！");
                MessageBox.Show("所有轴去点胶准备位置失败！", "点胶系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            rightTab1.Enabled = true;
        }

        private void btnLaserReadData_Click(object sender, EventArgs e)
        {
            double height = 0;
            bool Ret = ComHLLaser.Instance.GetHighValue(out height);
            if (!Ret)
            {
                log.Error("激光测高仪读数失败！");
                return;
            }
            txtLaserReadData.Text = height.ToString();
        }

       private void timerScan_Tick(object sender, EventArgs e)
        {
            bool state = Device.GetDevice().IO.DispensePress;
            if (state)
            {
                lbldispensepressure.BackColor = Color.GreenYellow;
            }
            else
            {
                lbldispensepressure.BackColor = Color.Pink;
            }
        }
#endif

        private void btnDispenseOutManual_Click(object sender, EventArgs e)
        {
            int value = 0;
            if (int.TryParse(txtManualDispenseOutTime.Text, out value))
            {
                if (value > 1000)
                {
                    value = 1000;
                    txtManualDispenseOutTime.Text = "1000";
                }
            }
            else
            {
                log.Error("输入的出胶时间有误");
            }
        }

        private void btnLightONOFF_Click(object sender, EventArgs e)
        {
            Light(rightTab1.SelectedIndex);
        }

        public void Light(int nSel)
        {
            if (IsIoTriggerLight)
            {
                if (IsComTriggerLight)
                {
                    if (nSel == 0)
                    {
                        lightControler?.Light(DispModleName + "点胶标定光源");
                    }
                    else
                    {
                        lightControler?.Light(DispModleName + "产品点胶光源");
                    }
                }
                bool bLightIO = IOMgr.GetInstace().ReadIoOutBit(TriggerLightIoName);
                IOMgr.GetInstace().WriteIoBit(TriggerLightIoName, !bLightIO);
                if (bLightIO)
                    btnLightONOFF.BackColor = Color.LightBlue;
                else
                    btnLightONOFF.BackColor = Color.LightGreen;
            }
            else
            {
                if (IsComTriggerLight)
                {
                    if (nSel == 0)
                    {
                        lightControler?.Light(DispModleName + "点胶标定光源");
                    }
                    else
                    {
                        lightControler?.Light(DispModleName + "产品点胶光源");
                    }
                }
            }
        }

        public void AddBuffMove(string gpName)
        {
        }

        private async void btnDispenseTest_Click(object sender, EventArgs e)
        {
            BtnEable(false);
            Task task = Task.Run(() =>
            {
                try
                {
                    var C = MessageBox.Show(null, "是否开始画胶测试", "提示", MessageBoxButtons.OKCancel);
                    if (DialogResult.OK == C)
                    {
                        if (!VisionDispense())
                        {
                            log.Error("视觉画胶失败！");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("视觉画胶失败" + ex.Message);
                }
                finally
                {
                }
            });
            await task;
            BtnEable();
        }

        public bool VisionDispense(bool NoDispenseOut = false)
        {
            //先照位定位

            return true;
        }

        private void labelLightDispDst_Click(object sender, EventArgs e)
        {
            if (lightControler == null)
                return;
            FormLightSet formLightSet = new FormLightSet();
            if (formLightSet.ShowDialog() == DialogResult.OK)
            {
                int nCh = formLightSet.nCh;
                int lightval = formLightSet.nLightVal;
                if (lightval > 0)
                {
                    if (rightTab1.SelectedIndex == 0)
                    {
                        lightControler.SetItem(DispModleName + "点胶标定光源", nCh, lightval);
                        textBox_DispCalbLightVal.Text = lightval.ToString();
                    }
                    else
                    {
                        lightControler.SetItem(DispModleName + "产品点胶光源", nCh, lightval);
                        textBox_DispDstLightVal.Text = lightval.ToString();
                    }
                }
                else
                {
                    if (rightTab1.SelectedIndex == 0)
                    {
                        lightControler.SetItem(DispModleName + "点胶标定光源", nCh, lightval);
                        textBox_DispCalbLightVal.Text = lightval.ToString();
                    }
                    else
                    {
                        lightval = textBox_DispDstLightVal.Text.ToInt();
                        lightControler.SetItem(DispModleName + "产品点胶光源", nCh, lightval);
                    }
                }
            }
        }

        private void JogStart(object sender, MouseEventArgs e)
        {
            if (comboBox_SelMotionType.SelectedItem != null && comboBox_SelMotionType.SelectedItem.ToString() != "Jog")
                return;
            int nAxisNo = 1;
            switch (((Button)sender).Name)
            {
                case "button_Xpositive":
                    nAxisNo = AxisX;
                    if (nAxisNo < 0) return;
                    // MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;

                case "button_Xnegtive":
                    nAxisNo = AxisX;
                    if (nAxisNo < 0) return;
                    //   MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;

                case "button_Ypositive":
                    nAxisNo = AxisY;
                    if (nAxisNo < 0) return;
                    //   MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;

                case "button_Ynegtive":
                    nAxisNo = AxisY;
                    if (nAxisNo < 0) return;
                    //    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;

                case "button_Zpositive":
                    nAxisNo = AxisZ;
                    if (nAxisNo < 0) return;
                    //     MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;

                case "button_Znegtive":
                    nAxisNo = AxisZ;
                    if (nAxisNo < 0) return;
                    //    MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;

                case "button_Upositive":
                    nAxisNo = AxisU;
                    if (nAxisNo < 0) return;
                    //     MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    break;

                case "button_Unegtive":
                    nAxisNo = AxisU;
                    if (nAxisNo < 0) return;
                    //      MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    break;
                    //case "button_Txpositive":
                    //    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTx;
                    //    if (nAxisNo < 0) return;
                    //    //     MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    //    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    //    break;
                    //case "button_Txnegtive":
                    //    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTx;
                    //    if (nAxisNo < 0) return;
                    //    //      MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    //    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    //    break;
                    //case "button_Typositive":
                    //    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTy;
                    //    if (nAxisNo < 0) return;
                    //    //       MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    //    MotionMgr.GetInstace().JogMove(nAxisNo, true, 0, 2);
                    //    break;
                    //case "button_Tynegtive":
                    //    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTy;
                    //    if (nAxisNo < 0) return;
                    //    //      MotionMgr.GetInstace().ServoOn((short)nAxisNo);
                    //    MotionMgr.GetInstace().JogMove(nAxisNo, false, 0, 2);
                    //    break;
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
                    nAxisNo = AxisX;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;

                case "button_Xnegtive":
                    nAxisNo = AxisX;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;

                case "button_Ypositive":
                    nAxisNo = AxisY;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;

                case "button_Ynegtive":
                    nAxisNo = AxisY;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;

                case "button_Zpositive":
                    nAxisNo = AxisZ;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;

                case "button_Znegtive":
                    nAxisNo = AxisZ;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;

                case "button_Upositive":
                    nAxisNo = AxisU;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;

                case "button_Unegtive":
                    nAxisNo = AxisU;
                    if (nAxisNo < 0) return;
                    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    break;
                    //case "button_Txpositive":
                    //    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTx;
                    //    if (nAxisNo < 0) return;
                    //    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    //    break;
                    //case "button_Txnegtive":
                    //    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTx;
                    //    if (nAxisNo < 0) return;
                    //    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    //    break;
                    //case "button_Typositive":
                    //    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTy;
                    //    if (nAxisNo < 0) return;
                    //    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    //    break;
                    //case "button_Tynegtive":
                    //    nAxisNo = StationMgr.GetInstance().GetStation(this).AxisTy;
                    //    if (nAxisNo < 0) return;
                    //    MotionMgr.GetInstace().StopAxis(nAxisNo);
                    //    break;
            }
        }

        private bool bStopMove = false;

        private void SelMoveType(int nAxisNo, bool bpostive, int speed)
        {
            if (comboBox_SelMotionType.SelectedItem == null || comboBox_SelMotionType.SelectedItem.ToString() == "Jog")
                return;
            if (nAxisNo < 0)
                return;
            int k = bpostive ? 1 : -1;
            if (comboBox_SelMotionType.SelectedItem == null || comboBox_SelMotionType.Text == null || comboBox_SelMotionType.Text == "")
                return;

            MotionMgr.GetInstace().RelativeMove(nAxisNo, k * comboBox_SelMotionType.Text.ToDouble(), speed);
            //switch (comboBox_SelMotionType.SelectedItem.ToString())
            //{
            //    case "微动1":
            //        MotionMgr.GetInstace().RelativeMove(nAxisNo, k * 1, speed);
            //        break;
            //    case "微动5":
            //        MotionMgr.GetInstace().RelativeMove(nAxisNo, k * 5, speed);
            //        break;
            //    case "微动10":
            //        MotionMgr.GetInstace().RelativeMove(nAxisNo, k * 10, speed);
            //        break;
            //    case "微动50":
            //        MotionMgr.GetInstace().RelativeMove(nAxisNo, k * 50, speed);
            //        break;
            //    case "微动100":
            //        MotionMgr.GetInstace().RelativeMove(nAxisNo, k * 100, speed);
            //        break;
            //    case "微动1000":
            //        MotionMgr.GetInstace().RelativeMove(nAxisNo, k * 1000, speed);
            //        break;
            //    case "微动10000":
            //        MotionMgr.GetInstace().RelativeMove(nAxisNo, k * 10000, speed);
            //        break;
            //    case "微动100000":
            //        MotionMgr.GetInstace().RelativeMove(nAxisNo, k * 100000, speed);
            //        break;
            //}
        }

        private void button_Upositive_Click(object sender, EventArgs e)
        {
        }

        private void button_Unegtive_Click(object sender, EventArgs e)
        {
        }

        private void button_Xnegtive_Click(object sender, EventArgs e)
        {
            bStopMove = true;
            SelMoveType(AxisX, false, (int)SpeedType.Low);
        }

        private void button_Xpositive_Click(object sender, EventArgs e)
        {
            bStopMove = true;
            SelMoveType(AxisX, true, (int)SpeedType.Low);
        }

        private void button_Ypositive_Click(object sender, EventArgs e)
        {
            bStopMove = true;
            SelMoveType(AxisY, true, (int)SpeedType.Low);
        }

        private void button_Ynegtive_Click(object sender, EventArgs e)
        {
            bStopMove = true;
            SelMoveType(AxisY, false, (int)SpeedType.Low);
        }

        private void button_Zpositive_Click(object sender, EventArgs e)
        {
            bStopMove = true;
            SelMoveType(AxisZ, true, (int)SpeedType.Low);
        }

        private void button_Znegtive_Click(object sender, EventArgs e)
        {
            bStopMove = true;
            SelMoveType(AxisZ, false, (int)SpeedType.Low);
        }

        public XYUPoint GetPointL(XYUPoint OldVisoionModlePos, XYUPoint NowVisionModlePos, XYUPoint OldSnapModlePos, XYUPoint OldPinMachinePos, XYUPoint NowSnapMchinePos)
        {
            HOperatorSet.VectorAngleToRigid(OldVisoionModlePos.x, OldVisoionModlePos.y, OldVisoionModlePos.u, NowVisionModlePos.x, NowVisionModlePos.y, NowVisionModlePos.u, out HTuple hom2d);

            HTuple vx, vy;
            XYUPoint machineXY = new XYUPoint(OldSnapModlePos.x - OldPinMachinePos.x, OldSnapModlePos.y - OldPinMachinePos.y, 0);
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtu, -machineXY.x, -machineXY.y, out vx, out vy);

            HTuple qvx, qvy;
            HOperatorSet.AffineTransPixel(hom2d, vx, vy, out qvx, out qvy);

            XYUPoint NowPinMachinePoint = XYUR_Laser.GetDstPonit(new XYUPoint(qvx, qvy, 0), NowSnapMchinePos);

            return NowPinMachinePoint;
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (AxisX != -1)
                MotionMgr.GetInstace().StopAxis(AxisX);
            if (AxisX != -1)
                MotionMgr.GetInstace().StopAxis(AxisY);
            if (AxisZ != -1)
                MotionMgr.GetInstace().StopAxis(AxisZ);
            if (AxisU != -1)
                MotionMgr.GetInstace().StopAxis(AxisU);
        }

        private async void btnPinHeight_Click(object sender, EventArgs e)
        {
            BtnEable(false);
            string strpos = "";
            Task task = Task.Run(() =>
            {
                int nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "针头测高点");
                if (nIndex == -1)
                    return;

                nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "标定点");
                if (nIndex == -1)
                    return;
                if (!MoveSigleAxis(AxisZ, dispCalibParam.pointDispenseCalibs[nIndex].MachinePoint.z, (double)SpeedType.High, 0.02))
                {
                    MessageBox.Show($"针高标定过程中，探针下探到过程中异常", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                nIndex = dispCalibParam.pointDispenseCalibs.FindIndex(t => t.strPointName == "针头测高点");
                if (nIndex == -1)
                    return;
                if (!MoveMulitAxis(new int[] { AxisX, AxisY }, new double[] { dispCalibParam.pointDispenseCalibs[nIndex].MachinePoint.x, dispCalibParam.pointDispenseCalibs[nIndex].MachinePoint.y }, new double[] { (double)SpeedType.High, (double)SpeedType.High },
                    0.02))
                {
                    MessageBox.Show($"针高标定过程中，探针下探到过程中异常", "Err", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                IOMgr.GetInstace().InStopState("点胶针头定位检测", true);
                IOMgr.GetInstace().InStopEnable("点胶针头定位检测");

                MotionMgr.GetInstace().AbsMove(AxisZ, dispCalibParam.pointDispenseCalibs[nIndex].MachinePoint.z, (int)SpeedType.Mid);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                do
                {
                    if (stopwatch.ElapsedMilliseconds > 30000)
                        break;
                    if (MotionMgr.GetInstace().IsAxisNormalStop(AxisZ) == AxisState.NormalStop)
                        break;
                }
                while (true);
                MotionMgr.GetInstace().StopAxis(AxisZ);
                strpos = MotionMgr.GetInstace().GetAxisPos(AxisZ).ToString();
                IOMgr.GetInstace().InStopDisenable("点胶针头定位检测");
            }
             );
            await task;
            txtZLatchData.Text = strpos;
            if (strpos != "" && dispCalibParam.FileSavePath != null
                && dispCalibParam.FileSavePath != "")
            {
                dispCalibParam.Save();
            }
            BtnEable();
        }

        private void dataGridViewDispTrace_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int nCol = e.ColumnIndex;
            int nRow = e.RowIndex;
            if (nCol == 2 && nRow >= 0)
            {
                DispTraceBaseElement dispTraceBaseElement = DispTraceMgr.GetInstance().GetItem(nRow);
                if (!dispTraceElementSet.Visible)
                    dispTraceElementSet.UpdateData(dispTraceBaseElement, true);
                else
                    MessageBox.Show("之前产品设定尚未完成，请完成后修改", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DispTraceElementSet dispTraceElementSet = new DispTraceElementSet();

        private void BtnAddPoint_Click(object sender, EventArgs e)
        {
            DispTraceBaseElementPoint dispTraceBaseElementPoint = new DispTraceBaseElementPoint();
            //dispTraceBaseElementPoint.ItemName = "P1";
            // DispTraceMgr.GetInstance().AddItemToList(dispTraceBaseElementPoint);
            if (!dispTraceElementSet.Visible)
                dispTraceElementSet.UpdateData(dispTraceBaseElementPoint);
            else
                MessageBox.Show("之前产品设定尚未完成，请完成后添加", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            DispTraceBaseElementLine dispTraceBaseElementline = new DispTraceBaseElementLine();
            if (!dispTraceElementSet.Visible)
                dispTraceElementSet.UpdateData(dispTraceBaseElementline);
            else
                MessageBox.Show("之前产品设定尚未完成，请完成后添加", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // DispTraceMgr.GetInstance().AddItemToList(dispTraceBaseElementline);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //string  strjson= File.ReadAllText("D:\\1235.json");
            //List<string> OneTrace= ParseJsonString_List(strjson);
            // Dictionary<string, List<string>> dis = new Dictionary<string, List<string>>();
            // string strjson2 = File.ReadAllText("D:\\123.json");
            // ParseJsonString_DicList(strjson2, dis);
            // ParsejsonToTraces(strjson2);
        }

        private void DispTrace_Click(object sender, EventArgs e)
        {
        }

        private void BtnIO_Click(object sender, EventArgs e)
        {
        }

        private void BtnDelay_Click(object sender, EventArgs e)
        {
        }

        private void BtnAddArc_Click(object sender, EventArgs e)
        {
            DispTraceBaseElementArc dispTraceBaseElementArc = new DispTraceBaseElementArc();
            if (!dispTraceElementSet.Visible)
                dispTraceElementSet.UpdateData(dispTraceBaseElementArc);
            else
                MessageBox.Show("之前产品设定尚未完成，请完成后添加", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DispTraceMgr.GetInstance().Read("D:\\123.json");
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewDispTrace.SelectedCells != null && dataGridViewDispTrace.SelectedCells.Count >= 1)
            {
                int nRow = dataGridViewDispTrace.SelectedCells[0].RowIndex;
                DispTraceMgr.GetInstance().DelItemFromList(nRow);
            }
        }

        private async void BtnSnapGoMeasureHeightPos_Click(object sender, EventArgs e)
        {
            BtnEable(false);
            Task task = Task.Run(() =>
            {
                try
                {
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    //Device.GetDevice().IO.CCDLight = false;
                }
            });
            await task;
            BtnEable();
        }

        public void ChangedUserRight(User CurrentUser)
        {
        }
    }
}