using BaseDll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MotionIoLib;
namespace UserData
{

    public enum LineSegementState
    {

        
        /// <summary>
        /// 流水线无料状态
        /// </summary>
        None,
        /// <summary>
        /// 流水线前段有设置信号 流水流动 启动根据 前段IO 无信号，直达中段感应信号有信号 延时（EnterTimer） 后流水线到达，整个过程超时由定时器（EnterTimer） 
        /// 流水线前段无设置信号 流水流动 启动根据 中段IO 无信号，直达中段感应信号有信号 延时（EnterTimer） 后流水线到达，整个过程超时由定时器（EnterTimer） 
        /// </summary>
        Entrying,//进料阶段

        /// <summary>
        /// 流水线到位 对流水线上相关部件（治具，Tray）处理，处理完成 进入Have状态
        /// </summary>
        BeforeHave,//有料前准备
        /// <summary>
        /// 流水线到位 相关工站 开始工作，流水线等待
        /// </summary>
        Have,//有料
        /// <summary>
        /// 流水线等待 相关工站 工作完成，流水线可以继续
        /// </summary>
        Finish,//完成
        /// <summary>
        /// 流水线离开阶段 ，如有后段检测信号，遇到信号延时停止 无信号 直接到出料状态
        /// </summary>
        Leaveing,//出料

        /// <summary>
        /// 流水线出料阶段 ，如有后一段流水线无料 出料
        /// </summary>
        WaitOut,
        /// <summary>
        /// 没有下一段流水线
        /// </summary>
        NoNextLine,


    }



    public enum LineSegementType
    {
        A,
        B,
    }
    /// <summary>
    /// 皮带通过方式
    /// </summary>
    public enum LinePassMode
    {
        自动处理,
        直通,
        人工处理,

    }
    /// <summary>
    /// 皮带线进料方式
    /// </summary>
    public enum FeedMode
    {
        前进料,
        后进料,

    }
    /// <summary>
    /// 电机运动方向
    /// </summary>
    public enum MotorMoveDir
    {
        停止,
        前,
        后
    }



    public class LineSegmentDataBase
    {

        public FeedMode feedMode = FeedMode.前进料;
        public LinePassMode linePassMode = LinePassMode.自动处理;
        public object objLock = new object();
        private bool bIsPause = false;
        public bool IsPause
        {
            set
            {
                lock(objLock)
                {
                     bIsPause = value;
                    if(value)
                        PauseDeal();
                    else
                        ResumeDeal();
                }
            }
            get
            {
                return bIsPause;
            }
        }
        /// <summary>
        /// 是否最后一段
        /// </summary>
        public bool IsLastSegLine=false;

        private MotorMoveDir _motorMoveDir;
        public MotorMoveDir motorMoveDir
        {

            private set
            {
                _motorMoveDir = value;
            }
            get
            {

                if (ForwardMotorIo != null && ForwardMotorIo != "")
                    if (IOMgr.GetInstace().ReadIoInBit(ForwardMotorIo))
                        return MotorMoveDir.前;
                if (BackMotorIo != null && BackMotorIo != "")
                    if (IOMgr.GetInstace().ReadIoInBit(BackMotorIo))
                        return MotorMoveDir.后;
                return MotorMoveDir.停止;
            }

        }

        public int Index = 0;
        public string strBarCode2d;
        public string strBarCode1d;
        private LineSegementState lineSegementState = LineSegementState.None;
        public string LineName = "";
        public void StopAllTimer()
        {
            EnterDelayTimer.Stop();
            EnterTimer.Stop();
            BackEnterDelayTimer.Stop();
            LeaveTimer.Stop();
            LeaveDelayTimer.Stop();
            OutTimer.Stop();
        }
        public LineSegementState LineSegState
        {

            set
            {
                switch (value)
                {
                    case LineSegementState.None:
                        Index = 0;
                        strBarCode2d = "";
                        strBarCode1d = "";
                        StopAllTimer();
                        break;
                    case LineSegementState.BeforeHave:
                    case LineSegementState.Have:
                        StopAllTimer();
                        break;
                    case LineSegementState.Leaveing:
                        EnterDelayTimer.Stop();
                        EnterTimer.Stop();
                        OutTimer.Stop();
                        break;
                    case LineSegementState.WaitOut:
                        EnterDelayTimer.Stop();
                        EnterTimer.Stop();
                        LeaveDelayTimer.Stop();
                        LeaveTimer.Stop();
                        break;
                    case LineSegementState.Entrying:
                        LeaveDelayTimer.Stop();
                        LeaveTimer.Stop();
                        OutTimer.Stop();
                        break;
                       


                }

                lineSegementState = value;
            }
            get
            {
                return lineSegementState;
            }
        }
        public string ForwardMotorIo
        {
            set;
            get;
        }
        public string BackMotorIo
        {
            set;
            get;
        }
        public void PauseDeal()
        {
            EnterTimer.PauseTimer();
            EnterDelayTimer.PauseTimer();
            BackEnterDelayTimer.PauseTimer();
            LeaveTimer.PauseTimer();
            LeaveDelayTimer.PauseTimer();
            OutTimer.PauseTimer();
            motorMoveDir = motorMoveDir;
            MotionStop();
            Pause();
        }
        public virtual void Pause()
        {
           


        }
        public  void ResumeDeal()
        {
            EnterTimer.resumeTimer();
            EnterDelayTimer.resumeTimer();
            BackEnterDelayTimer.resumeTimer();
            LeaveTimer.resumeTimer();
            LeaveDelayTimer.resumeTimer();
            OutTimer.resumeTimer();
            switch (motorMoveDir)
            {

                case MotorMoveDir.停止:
                    MotionStop();
                    break;
                case MotorMoveDir.前:
                    MotionForwardRun();
                    break;
                case MotorMoveDir.后:
                    MotionBackRun();
                    break;
            }
            Resume();
        }
        public virtual void Resume()
        {
            
        }

        public void MotionForwardRun()
        {
            if (ForwardMotorIo != null && ForwardMotorIo != "")
                IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, true);
            if (BackMotorIo != null && BackMotorIo != "")
                IOMgr.GetInstace().WriteIoBit(BackMotorIo, false);
        }

        public void MotionBackRun()
        {
            if (BackMotorIo != null && BackMotorIo != "")
                IOMgr.GetInstace().WriteIoBit(BackMotorIo, true);
            if (ForwardMotorIo != null && ForwardMotorIo != "")
                IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, false);
        }
        public void MotionStop()
        {
            if (BackMotorIo != null && BackMotorIo != "")
                IOMgr.GetInstace().WriteIoBit(BackMotorIo, false);
            if (ForwardMotorIo != null && ForwardMotorIo != "")
                IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, false);
        }
        public int nEnteryTimeout
        {
            set;
            get;
        }

        public int nLeaveTimeout
        {
            set;
            get;
        }

        /// <summary>
        /// 阻挡气缸上升
        /// </summary>
        public string StopCylinderUpIo = "";

        /// <summary>
        /// 阻挡气缸下降
        /// </summary>
        public string StopCylinderDownIo = "";

        /// <summary>
        /// 阻挡气缸上升到位
        /// </summary>
        public string StopCylinderUpInPosIo = "";

        /// <summary>
        /// 阻挡气缸下降到位
        /// </summary>
        public string StopCylinderDwonInPosIo = "";

        /// <summary>
        /// 顶升气缸上升
        /// </summary>
        public string JackUpCylinderUpIo = "";

        /// <summary>
        /// 顶升气缸下降
        /// </summary>
        public string JackUpCylinderDownIo = "";

        /// <summary>
        /// 顶升气缸上升到位
        /// </summary>
        public string JackUpCylinderUpIoInPos = "";

        /// <summary>
        /// 顶升气缸下降到位
        /// </summary>
        public string JackUpCylinderDownIoInPos = "";

        ///// <summary>
        ///// 前阻挡气缸上升
        ///// </summary>
        //public string FrontStopCylinderUpIo = "";

        ///// <summary>
        ///// 前阻挡气缸下降
        ///// </summary>
        //public string FrontStopCylinderDownIo = "";

        ///// <summary>
        /////  前阻挡气缸上升到位
        ///// </summary>
        //public string FrontStopCylinderUpInPosIo = "";


        ///// <summary>
        ///// 前阻挡气缸下降到位
        ///// </summary>
        //public string FrontStopCylinderDwonInPosIo = "";


        ///// <summary>
        ///// 后阻挡气缸上升
        ///// </summary>
        //public string BackStopCylinderUpIo = "";

        ///// <summary>
        ///// 后阻挡气缸下降
        ///// </summary>
        //public string BackStopCylinderDownIo = "";

        ///// <summary>
        /////  后阻挡气缸上升到位
        ///// </summary>
        //public string BackStopCylinderUpInPosIo = "";


        ///// <summary>
        ///// 后阻挡气缸下降到位
        ///// </summary>
        //public string BackStopCylinderDwonInPosIo = "";



        /// <summary>
        /// 入料感应
        /// </summary>
        public string EnteryCheckIo = "";


        /// <summary>
        /// 出料感应
        /// </summary>
        public string LeaveCheckIo = "";

        /// <summary>
        /// 到位感应
        /// </summary>
        public string InPosCheckIo = "";

        ///// <summary>
        ///// 后阻挡气缸
        ///// </summary>
        //public string BackStopCliyder = "";

        ///// <summary>
        ///// 前阻挡气缸
        ///// </summary>
        //public string FrontStopCliyder = "";


        public double dCurrentAngle = 0;
        public int TrayIndexFrom = 0;//Barrel 来自那个盘
        public int TrayCellIndexFrom = 0;//Barrel 来自盘的哪个位置
        public void ClearData()
        {
            Index = 0;
            strBarCode2d = "";
            strBarCode1d = "";

        }


        public void Save(string strPath)
        {

        }
        public void Read()
        {

        }
        public cUserTimer EnterTimer = new cUserTimer(10000);
        public cUserTimer EnterDelayTimer = new cUserTimer(100);
        public cUserTimer BackEnterDelayTimer = new cUserTimer(10000);
        public cUserTimer LeaveTimer = new cUserTimer(10000);
        public cUserTimer LeaveDelayTimer = new cUserTimer(100);
        public cUserTimer OutTimer = new cUserTimer(10000);

        public void StopClinderUp(bool bUp)
        {
            if (bUp)
            {
                if (StopCylinderUpIo != null && StopCylinderUpIo != "")
                    IOMgr.GetInstace().WriteIoBit(StopCylinderUpIo, true);
                if (StopCylinderDownIo != null && StopCylinderDownIo != "")
                    IOMgr.GetInstace().WriteIoBit(StopCylinderDownIo, false);
            }
            else
            {

                if (StopCylinderUpIo != null && StopCylinderUpIo != "")
                    IOMgr.GetInstace().WriteIoBit(StopCylinderUpIo, false);
                if (StopCylinderDownIo != null && StopCylinderDownIo != "")
                    IOMgr.GetInstace().WriteIoBit(StopCylinderDownIo, true);
            }
        }
        public void JackUpCliyderUp(bool bUp)
        {

            if (JackUpCylinderUpIo != null && JackUpCylinderUpIo != "")
            {
                IOMgr.GetInstace().WriteIoBit(JackUpCylinderUpIo, bUp);
            }
            if (JackUpCylinderDownIo != null && JackUpCylinderDownIo != "")
            {
                IOMgr.GetInstace().WriteIoBit(JackUpCylinderDownIo, !bUp);
            }
        }
        public bool CheckJackUpCliyderStateInPos(bool bUp)
        {
            if (bUp && (JackUpCylinderUpIoInPos == null || JackUpCylinderUpIoInPos == ""))
                return true;
            if (bUp && IOMgr.GetInstace().ReadIoInBit(JackUpCylinderUpIoInPos))
                return true;
            if (!bUp && (JackUpCylinderDownIoInPos == null || JackUpCylinderDownIoInPos == ""))
                return true;
            if (!bUp && IOMgr.GetInstace().ReadIoInBit(JackUpCylinderDownIoInPos))
                return true;
            
            return false;

        }

        public bool CheckStopCliyderStateInPos(bool bUp)
        {
            if (bUp && (StopCylinderUpInPosIo == null || StopCylinderUpInPosIo == ""))
                return true;
            if (bUp && IOMgr.GetInstace().ReadIoInBit(StopCylinderUpInPosIo))
                return true;
            if (!bUp && (StopCylinderDwonInPosIo == null || StopCylinderDwonInPosIo == ""))
                return true;
            if (!bUp && IOMgr.GetInstace().ReadIoInBit(StopCylinderDwonInPosIo))
                return true;
            return false;
        }
        public void Init()
        {
            LeaveTimer.Stop();
            EnterTimer.Stop();
            EnterDelayTimer.Stop();
            LeaveDelayTimer.Stop(); ;
            MotionStop();
            lineSegementState = LineSegementState.None;

        }
        //出料电机运动 true 正向  false 反向
        public bool bOutMotorRunDir = true;
    }







}