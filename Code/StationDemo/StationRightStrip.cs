using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTools;
using BaseDll;
using MotionIoLib;
using UserData;
using System.Threading;
using System.Windows.Forms;

namespace StationDemo
{
    public class StationRightStrip : Stationbase
    {
        const int dFineDistance = 20;//10个plus
        const int nSeparateCount = 10;
      //  const int nResolutionZ = 2000;
        const double nResolutionY = 10000/6.000;
        const double nResolutionX = 10000 / 4.000;
        const double nResolutionZ = 10000 / 6.0000;
        public StationRightStrip(Stationbase stationbase) : base(stationbase)
        {
            m_listIoInput.Add("右装料平台有无感应器");
            m_listIoInput.Add("右装料Z轴上升到位感应器");

            m_listIoInput.Add("右装料Z轴气缸原位");
            m_listIoInput.Add("右装料Z轴气缸到位");

            m_listIoInput.Add("右装料搬运气缸原位");
            m_listIoInput.Add("右装料搬运气缸到位");

            //m_listIoInput.Add("右剥料前推压紧气缸原位");
           // m_listIoInput.Add("右剥料前推压紧气缸到位");
            //m_listIoInput.Add("右剥料压紧气缸原位");
            //m_listIoInput.Add("右剥料压紧气缸到位");
            m_listIoInput.Add("右剥料后压紧气缸原位");
            m_listIoInput.Add("右剥料后压紧气缸到位");

            m_listIoInput.Add("右剥料气缸原位");
            m_listIoInput.Add("右剥料气缸到位");

            m_listIoInput.Add("右剥料夹料气缸原位");
            m_listIoInput.Add("右剥料夹料气缸到位");

            m_listIoInput.Add("右剥料真空检测");
            m_listIoInput.Add("右装料真空检测");





            m_listIoOutput.Add("右装料Z轴气缸电磁阀");
            m_listIoOutput.Add("右装料搬运气缸电磁阀");
            m_listIoOutput.Add("右装料真空吸电磁阀");
            m_listIoOutput.Add("右装料破真空电磁阀");

            
            // m_listIoOutput.Add("右剥料压紧气缸电磁阀");
            m_listIoOutput.Add("右剥料夹料气缸电磁阀");
            m_listIoOutput.Add("右剥料气缸电磁阀");
            m_listIoOutput.Add("右剥料后压紧气缸电磁阀");

            m_listIoOutput.Add("右剥料真空吸电磁阀");

            m_listIoOutput.Add("右剥料上吹气电磁阀");
            m_listIoOutput.Add("右剥料下吹气电磁阀");
        }
        public enum StationStep
        {
            step_init,
            step_jude_SeparateFinish,
            step_jude_LoadPlaneMoveFinish,
            Step_Pick,
            step_Carry,
            step_Separate,
            step_WaitGetFinish,
        }
        public enum LoadState
        {
            None,
            Find,
            Pick,
            Place,
            exceing,
        }
        LoadState loadState;
        Thread thread = new Thread((obj) =>
         {
             StationRightStrip stationrighttStrip = (StationRightStrip)obj;
             while (true)
             {
                 Thread.Sleep(10);
                 if ( ParamSetMgr.GetInstance().GetIntParam("右剥料屏蔽")==1)
                 {
                     return;
                 }
                 if (GlobalVariable.g_StationState == StationState.StationStateRun)
                     stationrighttStrip.LoadWortk();
             }
         });
        void DoSomethingWhenalarm()
        {

        }
        protected override bool InitStation()
        {
            ParamSetMgr.GetInstance().SetBoolParam("右搜寻蜂鸣器成功", false);
            ParamSetMgr.GetInstance().SetIntParam("右剥料次数", 0);
            ParamSetMgr.GetInstance().SetBoolParam("右装料平台上升到位", false);
            ParamSetMgr.GetInstance().SetBoolParam("右剥料回原点成功", false);
            AlarmMgr.GetIntance().DoWhenAlarmEvent += DoSomethingWhenalarm;
            if( !thread.IsAlive)
            {
                thread.IsBackground = true;
                thread.Start(this);
            }
            PushMultStep((int)StationStep.step_init);
            PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState = PlaneState.None;
            return true;
        }
        public bool Init(bool bmanual=false)
        {
            WaranResult waranResult;
        InitGoHome:
            IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", false);

            IOMgr.GetInstace().WriteIoBit("右剥料夹料气缸电磁阀", false);
            IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", false);
#if false
            //IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", false);
            //WaranResult waranResult = CheckIobyName("右剥料压紧气缸原位", true, "右剥料 归零：右剥料压紧气缸原位失败，", bmanual);
            // if (waranResult == WaranResult.Retry)
            //    goto InitGoHome;
            //IOMgr.GetInstace().WriteIoBit("右剥料前推压紧气缸电磁阀", false);
            //waranResult = CheckIobyName("右剥料前推压紧气缸原位", true, "右剥料 归零：右剥料前推压紧气缸原位失败，", bmanual);
            //if (waranResult == WaranResult.Retry)
            //    goto InitGoHome;
            IOMgr.GetInstace().WriteIoBit("右装料Z轴气缸电磁阀", false);
            waranResult = CheckIobyName("右装料Z轴气缸原位", true, "右剥料 归零：右装料Z轴气缸原位失败，", bmanual);
            if (waranResult == WaranResult.Retry)
                goto InitGoHome;
#endif
            IOMgr.GetInstace().WriteIoBit("右装料搬运气缸电磁阀", false);
            waranResult = CheckIobyName("右装料搬运气缸原位", true, "右剥料 归零：右装料搬运气缸原位失败，", bmanual);
            if (waranResult == WaranResult.Retry)
                goto InitGoHome;
#if false
            retry_inpos:
            IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", true);
            waranResult = CheckIobyName("右剥料压紧气缸到位", true, "右剥料 归零：右剥料压紧气缸到位失败，", bmanual);
            if (waranResult == WaranResult.Retry)
                goto retry_inpos;

#endif
            Info("RBZ 轴回原点");
            Retry_X_Home:
            waranResult = HomeSigleAxisPosWaitInpos(AxisX, this, 60000, bmanual);
            if (waranResult != WaranResult.Run)
            {
                waranResult = AlarmMgr.GetIntance().WarnWithDlg("右剥料 归零不成功： " + string.Format("{0} 轴回零失败", MotionMgr.GetInstace().GetAxisName(AxisX)), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto Retry_X_Home;
            }
            Info("RZZ 轴回原点");
            Retry_Z_Home:
            waranResult = HomeSigleAxisPosWaitInpos(AxisZ, this, 60000, bmanual);
            if (waranResult != WaranResult.Run)
            {
                waranResult= AlarmMgr.GetIntance().WarnWithDlg("右剥料 归零不成功： " + string.Format("{0} 轴回零失败", MotionMgr.GetInstace().GetAxisName(AxisZ)), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if(waranResult== WaranResult.Retry)
                  goto Retry_Z_Home;
            }
            Info("RBY 轴回原点");
            Retry_Y_Home:
            waranResult = HomeSigleAxisPosWaitInpos(AxisY, this, 60000, bmanual);
            if (waranResult != WaranResult.Run)
            {
               waranResult = AlarmMgr.GetIntance().WarnWithDlg("右剥料 归零不成功： " + string.Format("{0} 轴回零失败", MotionMgr.GetInstace().GetAxisName(AxisY)), this, CommonDlg.DlgWaranType.Waran_Stop_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto Retry_Y_Home;
            }
            Info("回剥料准备位");
            double xpos = GetStationPointDic()["剥料准备位"].pointX;
            double ypos = GetStationPointDic()["剥料准备位"].pointY;
            double zpos = GetStationPointDic()["装料原始位"].pointZ;
            Retry_YZ_InitPos:
            waranResult=MoveMulitAxisPosWaitInpos(new int[] { AxisX,AxisY, AxisZ }, new double[] { xpos, ypos, zpos }, new double[] { (double)SpeedType.High, (double)SpeedType.High, (double)SpeedType.High }, 20, true, this,60000);
            if (waranResult != WaranResult.Run)
            {
                waranResult = AlarmMgr.GetIntance().WarnWithDlg("右剥料 归零成功： " + "Y回准备或Z回原始位失败", this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, null, bmanual);
                if (waranResult == WaranResult.Retry)
                    goto Retry_YZ_InitPos;
            }
            ParamSetMgr.GetInstance().SetBoolParam("右剥料回原点成功", true);
            return true;
        }
        public WaranResult FindBuzzer(bool bManual = false)
        {
            if (sys.g_AppMode == AppMode.AirRun)
            {

                Info("右剥料站：空跑搜寻蜂鸣器成功");
                ParamSetMgr.GetInstance().SetBoolParam("右搜寻蜂鸣器成功", true);
                ParamSetMgr.GetInstance().SetBoolParam("右装料平台上升到位", true);
                return WaranResult.Run;
            }
            if (!IOMgr.GetInstace().ReadIoInBit("右装料平台有无感应器"))
            {
                double pos = GetStationPointDic()["装料原始位"].pointZ;

                MoveSigleAxisPosWaitInpos(AxisZ, pos, MotionMgr.GetInstace().GetAxisMovePrm(AxisZ).VelH, 20, bManual, this);
                MotionMgr.GetInstace().StopAxis(AxisZ);
                Info("右剥离工站：装料平台无料");
                WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg("右剥料：装料平台无料,请装料，装料完成点击<<重试>>按钮", bManual ? null : this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry);
                //if( WaranResult == WaranResult.)
                return waranResult;

            }
            else
            {

                Info("右剥料工站：装料平台寻料");
                // MotionMgr.GetInstace().ResetAxis()
                DoWhile doWhile = new DoWhile((time, dowhile, bmanual, obj) =>
                {
                    int pos = MotionMgr.GetInstace().GetAxisPos(AxisZ);
                    if (this.GetStationPointDic().ContainsKey("装料最高位"))
                    {
                        if (pos > this.GetStationPointDic()["装料最高位"].pointZ)
                        {
                            Warn("右剥料工站：右装料平台寻找蜂鸣器， 已经到达最高位");
                            Info("右剥离工站：搜寻蜂鸣器失败");
                            ParamSetMgr.GetInstance().SetBoolParam("右搜寻蜂鸣器成功", false);
                            MotionMgr.GetInstace().StopAxis(AxisZ);
                            return AlarmMgr.GetIntance().WarnWithDlg("右剥料：装料平台Z轴升至最高点 仍然没有检查到物料  请检查最高位设置，感应器之后，装料完成点击<<重试>>按钮", bManual ? null : this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry);
                        }
                        if (MotionMgr.GetInstace().IsAxisNormalStop(AxisZ) > AxisState.NormalStop)
                        {
                            Warn("右剥料工站：电机报警");
                            ParamSetMgr.GetInstance().SetBoolParam("右搜寻蜂鸣器成功", false);
                            return AlarmMgr.GetIntance().WarnWithDlg("右剥料：装料平台Z轴电机报警 请检查电机及驱动器，装料完成点击<<重试>>按钮", this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry);

                        }
                        if (IOMgr.GetInstace().ReadIoInBit("右装料Z轴上升到位感应器"))
                        {
                            MotionMgr.GetInstace().StopAxis(AxisZ);
                            double currentzaxisPos = MotionMgr.GetInstace().GetAxisPos(AxisZ);
                            currentzaxisPos = currentzaxisPos + ParamSetMgr.GetInstance().GetDoubleParam("装料抬升距离") * nResolutionZ;
                            if (currentzaxisPos > GetStationPointDic()["装料最高位"].pointZ)
                                currentzaxisPos = GetStationPointDic()["装料最高位"].pointZ - 10;
                            MoveSigleAxisPosWaitInpos(AxisZ, currentzaxisPos, MotionMgr.GetInstace().GetAxisMovePrm(AxisZ).VelH, 20, bManual,  this);

                            Info("右剥离工站：搜寻蜂鸣器成功");
                            ParamSetMgr.GetInstance().SetBoolParam("右搜寻蜂鸣器成功", true);
                            ParamSetMgr.GetInstance().SetBoolParam("右装料平台上升到位", true);
                            ParamSetMgr.GetInstance().SetIntParam("右蜂鸣器顶位", MotionMgr.GetInstace().GetAxisPos(AxisZ));

                          
                            return WaranResult.Run;
                        }
                        else
                        {
                            MotionMgr.GetInstace().JogMove(AxisZ, true, 0, (int)MotionMgr.GetInstace().GetAxisMovePrm(AxisZ).VelM);
                            return WaranResult.CheckAgain;
                        }

                    }
                    else
                    {
                        double position = GetStationPointDic()["装料原始位"].pointZ;
                        MoveSigleAxisPosWaitInpos(AxisZ, position, MotionMgr.GetInstace().GetAxisMovePrm(AxisZ).VelH, 20, bManual, bManual ? null : this);
                        MotionMgr.GetInstace().StopAxis(AxisZ);
                        Info("右剥离工站：装料平台无料");
                        WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg("右剥料：装料平台无料,请装料，装料完成点击<<重试>>按钮", bManual ? null : this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry);
                        return waranResult;
                    }
                }, int.MaxValue);
                return doWhile.doSomething(this, doWhile,bManual,null);
            }
        }


        public void CheckBuzzerPlaneAndUp(bool bManual = false)
        {
            if (IOMgr.GetInstace().ReadIoInBit("右装料平台有无感应器"))
            {

                Task task0 = new Task(() =>
                {
                    ParamSetMgr.GetInstance().SetBoolParam("右装料平台上升到位", false);
                    Info("装料步进运动");
                    int pos = MotionMgr.GetInstace().GetAxisPos(AxisZ);
                    pos = pos + (int)(ParamSetMgr.GetInstance().GetDoubleParam("装料步长") * nResolutionZ);
                    MoveSigleAxisPosWaitInpos(AxisZ, pos, MotionMgr.GetInstace().GetAxisMovePrm(AxisZ).VelH, 20, bManual, bManual ? null : this);
                    ParamSetMgr.GetInstance().SetBoolParam("右装料平台上升到位", true);
                });
                task0.Start();

            }
            else
            {
                Task task = new Task(() =>
                {
                    Info("回到原始位置");
                    double pos = GetStationPointDic()["装料原始位"].pointZ;
                    MoveSigleAxisPosWaitInpos(AxisZ, pos, MotionMgr.GetInstace().GetAxisMovePrm(AxisZ).VelH,20, bManual, this);
                    //AlarmMgr.GetIntance().WarnWithDlg("右剥料工站 装料平台无料，请添加蜂鸣片", bManual ? null : this, CommonDlg.DlgWaranType.WaranOK);
                    AlarmMgr.GetIntance().WarnWithDlg("右剥料工站 装料平台无料，请添加蜂鸣片,添加完成点击OK", this, CommonDlg.DlgWaranType.WaranOK);
                    FindBuzzer(bManual);
                    ParamSetMgr.GetInstance().SetBoolParam("右装料平台上升到位", true);
                });
                task.Start();
                task.Wait();
            }
        }

        public bool Pick(bool bmanul = false)
        {
            WaranResult waranResult;
           // CheckAndUp(bmanul);
        retry_carry_up:
            Info("右装料Z轴气缸电磁阀 上升");
            IOMgr.GetInstace().WriteIoBit("右装料Z轴气缸电磁阀", false);
            waranResult = CheckIobyName("右装料Z轴气缸原位", true, "右剥料工站：右装料Z轴气缸电磁阀 上升失败，请检查气缸和感应器 线路 ", bmanul);
            if (waranResult == WaranResult.Retry)
                goto retry_carry_up;

            if(bmanul || NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState == NozzleState.None)
            {
            retry_Self_Checking:
                IOMgr.GetInstace().WriteIoBit("右剥料真空吸电磁阀", true);
                waranResult = CheckIobyName("右剥料真空检测", false, "右剥料平台：Carray:蜂鸣器片 在剥料平台有料 请挪开", bmanul);
                if (waranResult == WaranResult.Retry)
                {
                    IOMgr.GetInstace().WriteIoBit("右剥料真空吸电磁阀", false);
                    goto retry_Self_Checking;
                }
                IOMgr.GetInstace().WriteIoBit("右剥料真空吸电磁阀", false);
                IOMgr.GetInstace().WriteIoBit("右装料破真空电磁阀", false);
                IOMgr.GetInstace().WriteIoBit("右装料真空吸电磁阀", true);
                waranResult = CheckIobyName("右装料真空检测", false, "右剥料平台：Carray:蜂鸣器片 右装料吸嘴可能堵住", bmanul);
                if (waranResult == WaranResult.Retry)
                {
                    IOMgr.GetInstace().WriteIoBit("右装料破真空电磁阀", false);
                    IOMgr.GetInstace().WriteIoBit("右装料真空吸电磁阀", false);
                    goto retry_Self_Checking;
                }

            retry_carry:
                IOMgr.GetInstace().WriteIoBit("右装料搬运气缸电磁阀", false);
                Info("右装料搬运气缸 移动到装料顶部");
                waranResult = CheckIobyName("右装料搬运气缸原位", true, "右剥料工站：右装料搬运气缸原位未到，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carry;

                Info("右装料Z轴气缸电磁阀 下降");
                IOMgr.GetInstace().WriteIoBit("右装料Z轴气缸电磁阀", true);
                waranResult = CheckIobyName("右装料Z轴气缸到位", true, "右剥料工站：右装料Z轴气缸电磁阀 下降失败，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carry;

                Info("右装料破真空电磁阀 关闭"); Info("右装料真空吸电磁阀 打开");
                IOMgr.GetInstace().WriteIoBit("右装料破真空电磁阀", false);
                IOMgr.GetInstace().WriteIoBit("右装料真空吸电磁阀", true);

                Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("吸真空延时"));

            retry_carry_1:
                Info("右装料Z轴气缸电磁阀 上升");
                IOMgr.GetInstace().WriteIoBit("右装料Z轴气缸电磁阀", false);
                waranResult = CheckIobyName("右装料Z轴气缸原位", true, "右剥料工站：右装料Z轴气缸电磁阀 上升失败，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carry_1;

                Info("右装料真空吸 检查");
                waranResult = CheckIobyName("右装料真空检测", true, "右剥料工站：右装料真空吸 检查失败，物料可能掉落，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carry;
                NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState = NozzleState.Have;
                ParamSetMgr.GetInstance().SetBoolParam("右装料平台上升到位", false);
            }
            return true;

        }

        public bool Carray(bool bmanul = false)
        {
            Info("右剥料工站：搬运气缸开始搬运物料");
            WaranResult waranResult;
        // CheckAndUp(bmanul);
        retry_carry_up:
            ParamSetMgr.GetInstance().SetBoolParam("右剥料完成", false);
            Info("右装料Z轴气缸电磁阀 上升");
            IOMgr.GetInstace().WriteIoBit("右装料Z轴气缸电磁阀", false);
            waranResult = CheckIobyName("右装料Z轴气缸原位", true, "右剥料工站：右装料Z轴气缸电磁阀 上升失败，请检查气缸和感应器 线路 ", bmanul);
            if (waranResult == WaranResult.Retry)
                goto retry_carry_up;
            StationRightPackage stationLeftPackage = (StationRightPackage)StationMgr.GetInstance().GetStation("右贴装站");
            int AxisYNooFPageBack = stationLeftPackage.AxisY;
            int AxiXYNooFPageBack = stationLeftPackage.AxisX;
            retry_carryMoving:
            if (MotionMgr.GetInstace().GetHomeFinishFlag(AxisY) == AxisHomeFinishFlag.Homed && MotionMgr.GetInstace().GetAxisPos(AxisY) > GetStationPointDic()["剥料准备位"].pointY - 50
                 && MotionMgr.GetInstace().GetHomeFinishFlag(AxisYNooFPageBack) == AxisHomeFinishFlag.Homed && MotionMgr.GetInstace().GetAxisPos(AxisYNooFPageBack) < stationLeftPackage.GetStationPointDic()["搬料安全位"].pointY - 50
                )
            {
#if false

                IOMgr.GetInstace().WriteIoBit("右剥料前推压紧气缸电磁阀", true);
                waranResult = CheckIobyName("右剥料前推压紧气缸到位", true, "右剥料工站：右剥料前推压紧气缸到位失败，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carryMoving;

                IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", true);
                waranResult = CheckIobyName("右剥料压紧气缸到位", true, "右剥料工站：右剥料压紧气缸到位失败，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carryMoving;
#endif
                Info("右装料搬运气缸电磁阀 移动搬料");
                IOMgr.GetInstace().WriteIoBit("右装料搬运气缸电磁阀", true);
                waranResult = CheckIobyName("右装料搬运气缸到位", true, "右剥料工站：右装料搬运气缸到位失败，物料可能掉落，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carryMoving;

              //  CheckBuzzerPlaneAndUp();

            retry_carry_down:
                Info("右装料Z轴气缸电磁阀 下降");
                IOMgr.GetInstace().WriteIoBit("右装料Z轴气缸电磁阀", true);
                waranResult = CheckIobyName("右装料Z轴气缸到位", true, "右剥料工站：右装料Z轴气缸原下降失败，物料可能掉落，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carry_down;

                Info("右装料真空吸电磁阀 关闭");
                Info("右装料破真空电磁阀 打开");
                Info("右剥料真空吸电磁阀 打开");
                IOMgr.GetInstace().WriteIoBit("右装料破真空电磁阀", true);
                IOMgr.GetInstace().WriteIoBit("右装料真空吸电磁阀", false);
                IOMgr.GetInstace().WriteIoBit("右剥料真空吸电磁阀", true);
                Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("吸真空延时"));

                IOMgr.GetInstace().WriteIoBit("右装料破真空电磁阀", false);
            retry_carry_up2:
                Info("右装料Z轴气缸电磁阀 上升");
                IOMgr.GetInstace().WriteIoBit("右装料Z轴气缸电磁阀", false);
                waranResult = CheckIobyName("右装料Z轴气缸原位", true, "右剥料工站：右装料Z轴气缸电磁阀 上升 失败，请检查气缸和感应器 线路 ", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carry_up2;

                waranResult = CheckIobyName("右剥料真空检测", true, "右剥料工站:Carray:蜂鸣器片 在剥料平台未吸紧", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carry_down;

                Info("右装料真空吸电磁阀 复检 检查料是否脱落");
                IOMgr.GetInstace().WriteIoBit("右装料破真空电磁阀", false);
                IOMgr.GetInstace().WriteIoBit("右装料真空吸电磁阀", true);
                waranResult = CheckIobyName("右装料真空检测", false, "右剥料工站 :Carray:蜂鸣器片 在装料抓手上未脱离", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carry_down;
                IOMgr.GetInstace().WriteIoBit("右装料真空吸电磁阀", false);

                
            retry_carryMovingback:
                Info("右装料搬运气缸电磁阀 移动搬料");
                IOMgr.GetInstace().WriteIoBit("右装料搬运气缸电磁阀", false);
                waranResult = CheckIobyName("右装料搬运气缸原位", true, "右剥料工站：Carray:搬运气缸回到原位", bmanul);
                if (waranResult == WaranResult.Retry)
                    goto retry_carryMovingback;
                IOMgr.GetInstace().WriteIoBit("右装料真空吸电磁阀", false);
                IOMgr.GetInstace().WriteIoBit("右装料破真空电磁阀", false);
                IOMgr.GetInstace().WriteIoBit("右剥料真空吸电磁阀", false);
                IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", false);
                ParamSetMgr.GetInstance().SetIntParam("右剥料次数", 0);
                NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState = NozzleState.None;
                PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState = PlaneState.Have;
                return true;
            }
            else
            {
                Info("Y轴 在剥料准备位前 不能移动 请挪开 或者 右剥料平台Y轴 没有回原点");
                return false;
                WaranResult waranResult2 = AlarmMgr.GetIntance().WarnWithDlg("Y轴 在剥料准备位前 不能移动 请挪开 或者 右剥料平台Y轴 没有回原点", bmanul ? null : this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry);
                if (waranResult2 == WaranResult.Retry)
                    goto retry_carryMoving;

            }



        }
        public void Separate(int index, bool bManual = false)
        {
            double pos = GetStationPointDic()["剥料起始位"].pointY;
            double posz=0, posy = 0;
            WaranResult waranResult;
            Info(string.Format("{0} {1}次剥料", bManual?"手动":"自动",index));
            if (index > 6)
            {
                   return;
            }
                
           switch (index)
                {
                    case 0:
#region 下压
                    ParamSetMgr.GetInstance().SetBoolParam("右剥料归位", false);
                        retry_pressup:
                        IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", false);
                        waranResult = CheckIobyName("右剥料后压紧气缸原位",true, "右剥料：Separate：右剥料后压紧气缸原位 到位失败", bManual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_pressup;

                        //IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", false);
                        //waranResult = CheckIobyName("右剥料压紧气缸原位", true, "右剥料：Separate：右剥料压紧气缸原位 到位失败", bManual);
                        //if (waranResult == WaranResult.Retry)
                        //    goto retry_pressup;
                       
                        IOMgr.GetInstace().WriteIoBit("右剥料夹料气缸电磁阀", false);
                        waranResult = CheckIobyName("右剥料夹料气缸原位", true, "右剥料：Separate：右剥料夹料气缸原位 到位失败", bManual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_pressup;


                    IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", true);
                   

                      double xpos = GetStationPointDic()["剥料下压位"].pointX;
                      double ypos = GetStationPointDic()["剥料下压位"].pointY;
                      Info("右剥料:Y轴 去剥料下压位,RBZ轴 去剥料前压位");
                    //MoveSigleAxisPosWaitInpos(AxisY, pos, MotionMgr.GetInstace().GetAxisMovePrm(AxisY).VelH, 20, bManual, this);
                      MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { xpos, ypos }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 20, bManual, this,60000);


                        retry_pressdown:
                        IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", true);
                        waranResult = CheckIobyName("右剥料后压紧气缸到位", true, "右剥料：Separate：右剥料后压紧气缸到位 到位失败", bManual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_pressdown;


#if false

                    IOMgr.GetInstace().WriteIoBit("右剥料前推压紧气缸电磁阀", true);
                        waranResult = CheckIobyName("右剥料前推压紧气缸到位", true, "右剥料：Separate：右剥料前推压紧气缸到位 到位失败", bManual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_pressdown;
#endif
                   
                    ypos = GetStationPointDic()["剥料前压位"].pointY;
                    Info("去剥料前压位");
                    MoveSigleAxisPosWaitInpos(AxisY, ypos, (double)SpeedType.High, 20, bManual,  this);

                    retry_pushfront_:
                    IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", true);
                    waranResult = CheckIobyName("右剥料气缸到位", true, "右剥料：Separate：右剥料气缸到位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_pushfront_;

#if false
                    retry_push:
                    IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", true);
                    waranResult = CheckIobyName("右剥料气缸到位", true, "右剥料：Separate：右剥料气缸到位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_push;
#endif


                    Info("右剥料 下拉");
                    xpos = GetStationPointDic()["剥料前压位"].pointX;
                    Info("去剥料前压位下压");
                    MoveSigleAxisPosWaitInpos(AxisX, xpos, (double)SpeedType.High,  20, bManual, this);

                    Info("右剥料 预剥料完成");

                    retry_camp:
                    IOMgr.GetInstace().WriteIoBit("右剥料夹料气缸电磁阀", true);
                    waranResult = CheckIobyName("右剥料夹料气缸到位", true, "右剥料：Separate：右剥料夹料气缸到位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_camp;


                    //IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", true);
                    //waranResult = CheckIobyName("右剥料压紧气缸到位", true, "右剥料：Separate：右剥料压紧气缸到位 到位失败", bManual);
                    //if (waranResult == WaranResult.Retry)
                    //    goto retry_pressdown;
                    break;
#endregion
                case 6:
                    #region 弃料     


                    //retry_pressbackdown:
                    //IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", true);
                    //waranResult = CheckIobyName("右剥料后压紧气缸到位", true, "右剥料：Separate：右剥料后压紧气缸到位 到位失败", bManual);
                    //if (waranResult == WaranResult.Retry)
                    //    goto retry_pressbackdown;

                    //retry_Spressup2:
                    //IOMgr.GetInstace().WriteIoBit("右剥料夹料气缸电磁阀", true);
                    //waranResult = CheckIobyName("右剥料夹料气缸到位", true, "右剥料：Separate：右剥料夹料气缸到位 到位失败", bManual);
                    //if (waranResult == WaranResult.Retry)
                    //    goto retry_Spressup2;
                    retry_pressbackup:
                    IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", false);
                    waranResult = CheckIobyName("右剥料后压紧气缸原位", true, "右剥料：Separate：右剥料后压紧气缸原位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_pressbackup;

                    posz = GetStationPointDic()["剥料弃料位"].pointX;
                   // posy= GetStationPointDic()["剥料弃料位"].pointY;
                    Info("右剥料：去弃料位");
                    MoveSigleAxisPosWaitInpos(AxisX, posz, MotionMgr.GetInstace().GetAxisMovePrm(AxisX).VelH, 20, bManual,  this);
                    //  MoveLine2DWaitInpos("右剥料YZ", new double[] { posz, posy }, 20, bManual);



                    IOMgr.GetInstace().WriteIoBit("右剥料上吹气电磁阀", true);
                    IOMgr.GetInstace().WriteIoBit("右剥料下吹气电磁阀", true);

                    retry_SFront2:
                    IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", false);
                    waranResult = CheckIobyName("右剥料气缸原位", true, "右剥料：Separate：右剥料气缸原位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_SFront2;
                    PulseGenerator pulseGenerator = new PulseGenerator(200, 8, new Action<int>((nCount) => {
                        if (nCount % 2 == 0)
                            IOMgr.GetInstace().WriteIoBit("右剥料下吹气电磁阀", false);
                        else
                            IOMgr.GetInstace().WriteIoBit("右剥料下吹气电磁阀", true);
                    }), new Action(() => {
                        IOMgr.GetInstace().WriteIoBit("右剥料下吹气电磁阀", false);
                    }));

                    retry_Spressup3:
                    IOMgr.GetInstace().WriteIoBit("右剥料夹料气缸电磁阀", false);
                    waranResult = CheckIobyName("右剥料夹料气缸原位", true, "右剥料：Separate：右剥料夹料气缸原位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_Spressup3;

                    pulseGenerator.StartPulseGenerator();
                    //retry_pressfrontpressup2:
                    //IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", false);
                    //    waranResult = CheckIobyName("右剥料压紧气缸原位", true, "右剥料：Separate：右剥料前推压紧气缸到位 到位失败", bManual);
                    //if (waranResult == WaranResult.Retry)
                    //        goto retry_pressfrontpressup2;

                    //retry_frontpush:
                    //IOMgr.GetInstace().WriteIoBit("右剥料前推压紧气缸电磁阀", true);
                    //waranResult = CheckIobyName("右剥料前推压紧气缸到位", true, "右剥料：Separate：右剥料前推压紧气缸到位 到位失败", bManual);
                    //if (waranResult == WaranResult.Retry)
                    //    goto retry_frontpush;

                    //retry_pressbackdown2:
                    //IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", true);
                    //waranResult = CheckIobyName("右剥料后压紧气缸到位", true, "右剥料：Separate：右剥料后压紧气缸原位 到位失败", bManual);
                    //if (waranResult == WaranResult.Retry)
                    // goto retry_pressbackdown2;

                    // retry_pressfrontup:
                    //IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", false);
                    // waranResult = CheckIobyName("右剥料压紧气缸原位", true, "右剥料：Separate：右剥料压紧气缸原位 到位失败", bManual);
                    //if (waranResult == WaranResult.Retry)
                    //  goto retry_pressfrontup;


                    retry_pressbackup5:
                       IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", false);
                       waranResult = CheckIobyName("右剥料后压紧气缸原位", true, "右剥料：Separate：右剥料后压紧气缸原位 到位失败", bManual);
                       if (waranResult == WaranResult.Retry)
                        goto retry_pressbackup5;

                       pos = GetStationPointDic()["剥料准备位"].pointY;
                       double  posX = GetStationPointDic()["剥料准备位"].pointX;
                       Info("右剥料：回剥料准备位");
                      MoveMulitAxisPosWaitInpos(new int[] { AxisX, AxisY }, new double[] { posX, pos }, new double[] { (double)SpeedType.High, (double)SpeedType.High }, 20, bManual, this, 60000);
                    IOMgr.GetInstace().WriteIoBit("右剥料上吹气电磁阀", false);
                    IOMgr.GetInstace().WriteIoBit("右剥料下吹气电磁阀", false);
                   
                    ParamSetMgr.GetInstance().SetBoolParam("右剥料归位", true);
                    PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState = PlaneState.None;
                    break;
#endregion
                default:
#region 剥料（1 2 3 4 5）

                    ParamSetMgr.GetInstance().SetBoolParam("右剥料归位", false);
#if false
                        retry_pressfrontpressup:
                        IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", true);
                        waranResult = CheckIobyName("右剥料压紧气缸到位", true, "右剥料：Separate：右剥料压紧气缸到位 到位失败", bManual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_pressfrontpressup;

                    retry_pressfrontfront2:
                    IOMgr.GetInstace().WriteIoBit("右剥料前推压紧气缸电磁阀", false);
                    waranResult = CheckIobyName("右剥料前推压紧气缸原位", true, "右剥料：Separate：右剥料前推压紧气缸到位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_pressfrontfront2;
#endif

#if true
                retry_Sback:
                    IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", true);
                    waranResult = CheckIobyName("右剥料气缸到位", true, "右剥料：Separate：右剥料气缸到位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_Sback;
#else
                    retry_push2:
                    IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", false);
                    waranResult = CheckIobyName("右剥料气缸原位", true, "右剥料：Separate：右剥料气缸电原位 到位失败", bManual);
                    if (waranResult == WaranResult.Retry)
                        goto retry_push2;
#endif
                    //retry_SFront:
                    //IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", false);
                    //waranResult = CheckIobyName("右剥料气缸原位", true, "右剥料：Separate：右剥料气缸原位 到位失败", bManual);
                    //if (waranResult == WaranResult.Retry)
                    //    goto retry_SFront;

                    retry_Spressup:
                        IOMgr.GetInstace().WriteIoBit("右剥料夹料气缸电磁阀", true);
                        waranResult = CheckIobyName("右剥料夹料气缸到位", true, "右剥料：Separate：右剥料夹料气缸到位 到位失败", bManual);
                        if (waranResult == WaranResult.Retry)
                            goto retry_Spressup;

                        pos = GetStationPointDic()["剥料起始位"].pointY;
                        double lenstep = ParamSetMgr.GetInstance().GetDoubleParam("剥料步长");
                        posy = pos - (int)((index - 1) * lenstep * nResolutionY);
                        pos = GetStationPointDic()["剥料起始位"].pointX;
                        posz = pos - (int)((index - 1) * lenstep * nResolutionX);
                        MoveLine2DWaitInpos("右剥料YZ", new double[] { posz, posy }, 10, bManual);

                    //Info("去剥料起始位Y");
                    //MoveSigleAxisPosWaitInpos(AxisY, pos, MotionMgr.GetInstace().GetAxisMovePrm(AxisY).VelH, 20, bManual,  this);

                    //Info("去剥料起始位LZ");
                    //MoveSigleAxisPosWaitInpos(AxisX, pos, MotionMgr.GetInstace().GetAxisMovePrm(AxisX).VelH, 20, bManual,  this);


                    Info("压紧气缸下压");

                      retry_pressbackdownagain:
                      IOMgr.GetInstace().WriteIoBit("右剥料后压紧气缸电磁阀", true);
                      waranResult = CheckIobyName("右剥料后压紧气缸到位", true, "右剥料：Separate：右剥料后压紧气缸到位 到位失败", bManual);
                      if (waranResult == WaranResult.Retry)
                        goto retry_pressbackdownagain;

                    //retry_pressfrontfront:
                    //    IOMgr.GetInstace().WriteIoBit("右剥料前推压紧气缸电磁阀", true);
                    //    waranResult = CheckIobyName("右剥料前推压紧气缸到位", true, "右剥料：Separate：右剥料前推压紧气缸到位 到位失败", bManual);
                    //    if (waranResult == WaranResult.Retry)
                    //        goto retry_pressfrontfront;

                        //retry_pressfrontpressdown:
                        //IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", true);
                        //waranResult = CheckIobyName("右剥料压紧气缸到位", true, "右剥料：Separate： 右剥料压紧气缸到位 到位失败", bManual);
                        //if (waranResult == WaranResult.Retry)
                        //    goto retry_pressfrontpressdown;
                        //retry_Spressdown:
                        //IOMgr.GetInstace().WriteIoBit("右剥料夹料气缸电磁阀", true);
                        //waranResult = CheckIobyName("右剥料夹料气缸到位", true, "右剥料：Separate：右剥料夹料气缸到位 到位失败", bManual);
                        //if (waranResult == WaranResult.Retry)
                        //    goto retry_Spressdown;

                      //Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("下压延时"));
                      //retry_Sback:
                      //  IOMgr.GetInstace().WriteIoBit("右剥料气缸电磁阀", true);
                      //  waranResult = CheckIobyName("右剥料气缸到位", true, "右剥料：Separate：右剥料气缸到位 到位失败", bManual);
                      //  if (waranResult == WaranResult.Retry)
                      //      goto retry_Sback;

                      // Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("拨料延时"));
                        //retry_pressfrontpressup3:
                        //IOMgr.GetInstace().WriteIoBit("右剥料压紧气缸电磁阀", false);
                        //waranResult = CheckIobyName("右剥料压紧气缸原位", true, "右剥料：Separate：右剥料压紧气缸原位 到位失败", bManual);
                        //if (waranResult == WaranResult.Retry)
                        //    goto retry_pressfrontpressup3;

                        //retry_pressfrontback:
                        //IOMgr.GetInstace().WriteIoBit("右剥料前推压紧气缸电磁阀", false);
                        //waranResult = CheckIobyName("右剥料前推压紧气缸原位", true, "右剥料：Separate：右剥料前推压紧气缸原位 到位失败", bManual);
                        //if (waranResult == WaranResult.Retry)
                        //    goto retry_pressfrontback;

                       double currentpos =MotionMgr.GetInstace().GetAxisPos(AxisX);
                       MoveSigleAxisPosWaitInpos(AxisX, currentpos + ParamSetMgr.GetInstance().GetDoubleParam("剥料后拉距离") * nResolutionY, (double)SpeedType.High, 20, bManual, this, 60000);
                        break;
#endregion
                }
            
            ParamSetMgr.GetInstance().SetIntParam("右剥料次数", ++index);


        }
        private void LoadWortk(bool bManual = false)
        {
            try
            {
                if (!ParamSetMgr.GetInstance().GetBoolParam("右剥料回原点成功"))
                    return;
                if (!IOMgr.GetInstace().ReadIoInBit("右装料平台有无感应器") 
                    && !ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功")
                    && loadState != LoadState.exceing
                    && sys.g_AppMode == AppMode.AirRun
                    && GlobalVariable.g_StationState == StationState.StationStateRun)
                {
                    Info(string.Format("右装料平台有无感应器{0},右搜寻蜂鸣器成功{1},右拨料平台状态{2},右剥料吸嘴状态{3} 右剥料归位{4}",
                         IOMgr.GetInstace().ReadIoInBit("右装料平台有无感应器"),
                         ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功"),
                         PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState,
                         NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState,
                         ParamSetMgr.GetInstance().GetBoolParam("右剥料归位")));
                    retry_find:
                    loadState = LoadState.exceing;
                     WaranResult waranResult =   FindBuzzer();
                    if (waranResult == WaranResult.Retry)
                        goto retry_find;
                    loadState = LoadState.None;
                }
               

                if (ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功") 
                    && ParamSetMgr.GetInstance().GetBoolParam("右装料平台上升到位")
                    && NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState == NozzleState.None 
                    && loadState != LoadState.exceing
                    && GlobalVariable.g_StationState == StationState.StationStateRun)
                {
                    Info(string.Format("右装料平台有无感应器{0},右搜寻蜂鸣器成功{1},右拨料平台状态{2},右剥料吸嘴状态{3} 右剥料归位{4}",
                         IOMgr.GetInstace().ReadIoInBit("右装料平台有无感应器"),
                          ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功"),
                          PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState,
                          NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState,
                          ParamSetMgr.GetInstance().GetBoolParam("右剥料归位")));
                    loadState = LoadState.exceing;
                    Pick();
                    loadState = LoadState.None;
                }
              
                if (ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功")
                && !ParamSetMgr.GetInstance().GetBoolParam("右装料平台上升到位")
                && GlobalVariable.g_StationState == StationState.StationStateRun)
                {
                    Info("装料步进运动");
                    int pos = MotionMgr.GetInstace().GetAxisPos(AxisZ);
                    double steplen = ParamSetMgr.GetInstance().GetDoubleParam("装料步长");
                    pos = pos + (int)(steplen * nResolutionZ);
                    MoveSigleAxisPosWaitInpos(AxisZ, pos, MotionMgr.GetInstace().GetAxisMovePrm(AxisZ).VelH, 20, bManual, this);
                    ParamSetMgr.GetInstance().SetBoolParam("右装料平台上升到位", true);
                }
            }
            catch( Exception e)
            {
                loadState = LoadState.None;
                Warn("异常发生"+e.Message);
                return;
            }
        }

        protected override void StationWork(int step)
        {
            if (ParamSetMgr.GetInstance().GetIntParam("右剥料屏蔽") == 1)
            {
                ParamSetMgr.GetInstance().SetBoolParam("右剥料完成", true);
                return;
            }
 
            WaranResult waranResult;
            bool bfind = ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功");
            bool bgooripos = ParamSetMgr.GetInstance().GetBoolParam("右剥料归位");
            PlaneState planeState = PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState;
            NozzleState nozzleState = NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState;
            if (ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功") && ParamSetMgr.GetInstance().GetBoolParam("右剥料归位")
                && PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState == PlaneState.None
                && NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState == NozzleState.Have
                && GlobalVariable.g_StationState == StationState.StationStateRun)
            {
                Info(string.Format("右装料平台有无感应器{0},右搜寻蜂鸣器成功{1},右拨料平台状态{2},右剥料吸嘴状态{3} 右剥料归位{4}",
                 IOMgr.GetInstace().ReadIoInBit("右装料平台有无感应器"),
                 ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功"),
                 PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState,
                 NozzleMgr.GetInstance().nozzleArr[(int)NozzleType.RightStripNozzle].nozzleState,
                 ParamSetMgr.GetInstance().GetBoolParam("右剥料归位")));
                ///loadState = LoadState.exceing;
                Carray();
               // loadState = LoadState.None;
            }
            switch (step)
            {
                case (int)StationStep.step_init:
                    Init();
               retry_FindBuzzer:
                    waranResult = FindBuzzer();
                    if (waranResult == WaranResult.Retry)
                        goto retry_FindBuzzer;

                    if (ParamSetMgr.GetInstance().GetBoolParam("右搜寻蜂鸣器成功"))
                    {
                    retry_go_蜂鸣器顶位:
                        if (sys.g_AppMode == AppMode.AirRun)
                            ParamSetMgr.GetInstance().SetIntParam("右蜂鸣器顶位", (int)GetStationPointDic()["装料原始位"].pointZ);
                      
                        WaranResult waranResult1 = MoveSigleAxisPosWaitInpos(AxisZ, ParamSetMgr.GetInstance().GetIntParam("右蜂鸣器顶位"), (int)MotionMgr.GetInstace().GetAxisMovePrm(AxisZ).VelH, 20, false, this, 60000);
                        if (waranResult1 == WaranResult.Retry)
                            goto retry_go_蜂鸣器顶位;
                    }
                    MoveSigleAxisPosWaitInpos(AxisY, GetStationPointDic()["剥料准备位"].pointY, (double)SpeedType.High, 20, false, this, 60000);
                    PushMultStep((int)StationStep.step_Separate);
                    ParamSetMgr.GetInstance().SetBoolParam("右剥料归位", true);
                    ParamSetMgr.GetInstance().SetIntParam("右剥料次数", 0);
                    DelCurrentStep();
                    break;
               
                case (int)StationStep.step_Separate:
                    ParamSetMgr.GetInstance().SetBoolParam("右剥料完成", false);
                    if (PlaneMgr.GetInstance().PlaneArr[(int)PlaneType.rightStripPlane].planeState == PlaneState.None ) break;
                   
                    int Count = ParamSetMgr.GetInstance().GetIntParam("右剥料次数");
                    if (Count > 6)
                    {
                      
                        PushMultStep( (int)StationStep.step_Separate);
                        DelCurrentStep();
                        break;
                    }
                 
                    Separate(Count);
                   
                    Count = ParamSetMgr.GetInstance().GetIntParam("右剥料次数");
                    if (Count >= 2 && Count<=6)
                    {
                        ParamSetMgr.GetInstance().SetBoolParam("右剥料完成", true);
                        PushMultStep((int)StationStep.step_WaitGetFinish);
                        DelCurrentStep();
                    }
                    else if (Count > 6)
                    {
                        ParamSetMgr.GetInstance().SetBoolParam("右剥料完成", false);
                        PushMultStep( (int)StationStep.step_Separate);
                       
                        DelCurrentStep();
                        break;
                    }
                    else
                    {
                        PushMultStep((int)StationStep.step_Separate);
                        DelCurrentStep();
                    }
                    break;
                case (int)StationStep.step_WaitGetFinish:
                    if (ParamSetMgr.GetInstance().GetBoolParam("右剥料取料完成"))
                    {
                        ParamSetMgr.GetInstance().SetBoolParam("右剥料取料完成", false);
                        PushMultStep((int)StationStep.step_Separate);
                        DelCurrentStep();
                    }
                    break;
            }





        }




    }
}
