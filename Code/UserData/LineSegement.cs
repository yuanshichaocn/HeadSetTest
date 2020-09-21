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
using System.IO.Ports;

namespace UserData
{

    public enum LineSegementState
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        UnKnow,
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
        /// 后段进料准备阶段
        /// </summary>
        BackEntryReady,
        /// <summary>
        /// 流水线到位 对流水线上相关部件（治具，Tray）处理，处理完成 进入Have状态
        /// </summary>
        ReadyIng,//有料前准备
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
        Leaveing,

        /// <summary>
        /// 流水线出料阶段 ，如有后一段流水线无料 出料
        /// </summary>
        WaitOut,
        /// <summary>
        /// 出料中
        /// </summary>
        /// 
        
        Outing,
        /// <summary>
        /// 出料完成 马上进入到None
        /// </summary>
        OutFinish,
        
      


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
    public enum OutFinishJudeType
    { 
        信号,
        下一段流水线
    
    
    }


    public class LineSegmentDataBase
    {

        public FeedMode feedMode = FeedMode.前进料;
        public LinePassMode linePassMode = LinePassMode.自动处理;
        public OutFinishJudeType outFinishJudeType = OutFinishJudeType.下一段流水线;
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

                if (IOMotionDir == null || IOMotionDir == "")
                {
                    if (ForwardMotorIo != null && ForwardMotorIo != "")
                        if (IOMgr.GetInstace().ReadIoOutBit(ForwardMotorIo))
                            return MotorMoveDir.前;
                    if (BackMotorIo != null && BackMotorIo != "")
                        if (IOMgr.GetInstace().ReadIoOutBit(BackMotorIo))
                            return MotorMoveDir.后;
                }
                else
                {
                        if (IOMgr.GetInstace().ReadIoOutBit(IOMotionDir))
                            return MotorMoveDir.前;
                       else
                            return MotorMoveDir.后;

                }
                 
                return MotorMoveDir.停止;
            }

        }

        public int Index = 0;
        public string strBarCode2d;
        public string strBarCode1d;
        private LineSegementState lineSegementState = LineSegementState.UnKnow;
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
                    case LineSegementState.UnKnow:
                        Index = 0;
                        strBarCode2d = "";
                        strBarCode1d = "";
                        StopAllTimer();
                        break;
                    case LineSegementState.ReadyIng:
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
        /// <summary>
        /// 电机正转IO
        /// </summary>
        public string ForwardMotorIo
        {
            set;
            get;
        }
        /// <summary>
        /// 电机反转IO
        /// </summary>
        public string BackMotorIo
        {
            set;
            get;
        }

        /// <summary>
        /// 电机方IO
        /// </summary>
        public string IOMotionDir
        {
            get;
            set;
        }

        protected void PauseDeal()
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

        public virtual  void Stop()
        {
         ;

        }
        public virtual void StopDeal()
        {
           
            EnterTimer.Stop();
            EnterDelayTimer.Stop();
            BackEnterDelayTimer.Stop();
            LeaveTimer.Stop();
            LeaveDelayTimer.Stop();
            OutTimer.Stop();
            MotionStop();
            Stop();
        }
        protected void ResumeDeal()
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

        public void MotionForwardRun(bool bRunDir=true)
        {
            if (IOMotionDir == null || IOMotionDir == "")
            {
                if (ForwardMotorIo != null && ForwardMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, true);
                if (BackMotorIo != null && BackMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(BackMotorIo, false);
            }
            else
            {
                if (ForwardMotorIo != null && ForwardMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, true);
                if (IOMotionDir != null && IOMotionDir != "")
                    IOMgr.GetInstace().WriteIoBit(IOMotionDir, bRunDir);
            }
        }

        public void MotionBackRun( bool bRunDir = false)
        {
            if (IOMotionDir == null || IOMotionDir == "")
            {
                if (BackMotorIo != null && BackMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(BackMotorIo, true);
                if (ForwardMotorIo != null && ForwardMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, false);
            }
            else
            {
                if (ForwardMotorIo != null && ForwardMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, true);
                if (IOMotionDir != null && IOMotionDir != "")
                    IOMgr.GetInstace().WriteIoBit(IOMotionDir, bRunDir);

            }
          
        }
        public void MotionStop()
        {
            if (IOMotionDir == null || IOMotionDir == "")
            {

                if (BackMotorIo != null && BackMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(BackMotorIo, false);
                if (ForwardMotorIo != null && ForwardMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, false);
            }
            else
            {
                if (ForwardMotorIo != null && ForwardMotorIo != "")
                    IOMgr.GetInstace().WriteIoBit(ForwardMotorIo, false);
                if (IOMotionDir != null && IOMotionDir != "")
                    IOMgr.GetInstace().WriteIoBit(IOMotionDir, false);
            }
        }
               
        public long nEnteryTimeout
        {
            set=> EnterTimer.SetTimeDelay(value);
            get=>EnterTimer.SetedTime;
            
        }
        public long nEnteryDelay
        {
            set =>  EnterDelayTimer.SetTimeDelay(value);
            get => EnterDelayTimer.SetedTime;
        }

        public long nLeaveDelay
        {
            set => LeaveDelayTimer.SetTimeDelay(value);
            get =>  LeaveDelayTimer.SetedTime;
        }
        public long nOutTimeout
        {
            set => OutTimer.SetTimeDelay(value);
            get =>  OutTimer.SetedTime;
 
        }
        public long nBackEnterDelay
        {
            set => BackEnterDelayTimer.SetTimeDelay(value);
            get =>  BackEnterDelayTimer.SetedTime;
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
        protected cUserTimer EnterTimer = new cUserTimer(10000);
        protected cUserTimer EnterDelayTimer = new cUserTimer(100);

        protected cUserTimer BackEnterDelayTimer = new cUserTimer(10000);
        protected cUserTimer LeaveTimer = new cUserTimer(10000);
        protected cUserTimer LeaveDelayTimer = new cUserTimer(100);
        protected cUserTimer OutTimer = new cUserTimer(10000);
        /// <summary>
        /// 阻挡气缸上升 true 上升 false 下降
        /// </summary>
        /// <param name="bUp"></param>
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
        /// <summary>
        /// 顶升气缸上升 bUP true 顶升 false  下降
        /// </summary>
        /// <param name="bUp"></param>
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

        /// <summary>
        /// 检查顶升气缸 到位 bUp true 顶升到位，false 下降到位
        /// </summary>
        /// <param name="bUp"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 检查阻挡气缸到位情况 up 阻挡顶升到位 down 顶升下降到位
        /// </summary>
        /// <param name="bUp"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 流水线初始化
        /// </summary>
        public void Init()
        {
            LeaveTimer.Stop();
            EnterTimer.Stop();
            EnterDelayTimer.Stop();
            LeaveDelayTimer.Stop(); ;
            MotionStop();
            lineSegementState = LineSegementState.None;

        }
        /// <summary>
        ///  出料电机运动方向 true 正向  false 反向
        /// </summary>
        public bool bOutMotorRunDir = true;
    }







}