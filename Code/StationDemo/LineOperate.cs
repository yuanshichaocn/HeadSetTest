using CommonTools;
using MotionIoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserData;
using BaseDll;
using System.Reflection;

namespace StationDemo
{

    public interface  LineOprater
    {
         void OpearteLineBeforeEntry(bool bmanual);

         bool CheckLineBeforeEntry(LineSegmentDataBase lb, bool bmanual);
        
         void OperateLineBeforeHave(LineSegmentDataBase lb, bool bmanual);

         bool CheckBeforeHave(LineSegmentDataBase lb, bool bmanual);
        
        bool CheckBeforeLeave(LineSegmentDataBase lb, bool bmanual);
      
        void OperateLineBeforeLevave(LineSegmentDataBase lb, bool bmanual);

    }

    public class LineSegmentAction : LineSegmentDataBase
    {

        public LineSegmentAction(string Name)
        {
            LineName = Name;
        }
        public LineSegementState GetState()
        {
            return LineSegState;
        }

        public delegate bool DoSomeThingHandler(Stationbase sb, bool bmaual);

        public event DoSomeThingHandler SomeThingsOnlineRun = null;
        public virtual bool doSomingThingWhenLineRun(Stationbase sb, bool bmaual)
        {
            bool bSave = true;
            if (SomeThingsOnlineRun != null)
            {
                Delegate[] delegates = SomeThingsOnlineRun.GetInvocationList();

                foreach (var tem in delegates)
                {
                    MethodInfo methodInfo = tem.GetMethodInfo();
                    bSave = bSave & (bool)methodInfo.Invoke(null, new object[] { sb, bmaual });
                }
            }
            return bSave;
        }

        public void LineRun( Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            switch (feedMode)
            {
                case FeedMode.前进料:
                    ForwardRun( sb,  backLineState,  bmaual);
                    break;
                case FeedMode.后进料:
                    BackRun( sb,  backLineState,  bmaual);
                    break;
            }
        }
        void LeaveOperate(Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            WaranResult waranResult;
            switch (LineSegState)
            {
                case LineSegementState.Have:
                    if (backLineState == LineSegementState.NoNextLine)
                    {
                        LineSegState = LineSegementState.None;
                        return;
                    }
                    if (linePassMode == LinePassMode.直通)
                    {
                        sb?.Info($"{LineName} 直通模式 :状态 {LineSegState}--> { LineSegementState.Finish}");
                    }
                    //sb?.Info($"{LineName} :状态{LineSegState}");
                    break;
                case LineSegementState.Finish:
                    OperateLineBeforeLevave(bmaual);
                    if (!CheckBeforeLeave(bmaual))
                        return;
                    LeaveDelayTimer.Stop();
                    LeaveTimer.Stop();
                    OutTimer.Stop();
                    //物料取料完成  加工完成  
                    if (LeaveCheckIo == null || LeaveCheckIo == "")
                    {

                        sb?.Info($"{LineName} ,{feedMode.ToString()}, 无后端检测IO  状态： {LineSegState}--> { LineSegementState.Leaveing}");
                        if (backLineState == LineSegementState.None)
                            LineSegState = LineSegementState.Leaveing;

                    }
                    else if (!IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                    {
                        sb?.Info($"{LineName} ,{feedMode.ToString()}, 有后端检测IO    没有信号 状态： {LineSegState}--> { LineSegementState.Leaveing}");
                        LineSegState = LineSegementState.Leaveing;
                    }
                    else
                    {
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} ,{feedMode.ToString()}, 后端检测信号有, 物料可能卡住，或者有物料放置，请拿走重试", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                            return;
                    }
                    break;
                case LineSegementState.Leaveing:
                    StopClinderUp(false);
                    if (!CheckStopCliyderStateInPos(false))
                        return;
                    if (!LeaveTimer.IsRuning)
                        LeaveTimer.ResetStartTimer();
                    if (LeaveTimer.IsTimerOver)
                    {
                        // c 离开定时器  时间到
                        MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} :,{feedMode.ToString()}, 离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                        {
                            LeaveTimer.ResetStartTimer();
                            LeaveDelayTimer.Stop();
                        }

                    }
                    else
                    {

                        if (LeaveDelayTimer.IsTimerOver)
                        {
                            // b.3 有后端检测IO感应  离开延时定时器时间到达 电机停止
                            LeaveDelayTimer.Stop();
                            LeaveTimer.Stop();
                            MotionStop();
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 有后端检测IO 离开延时定时器到达 : 状态 {LineSegState}--> { LineSegementState.WaitOut}");
                            LineSegState = LineSegementState.WaitOut;
                        }
                        else if (LeaveDelayTimer.IsRuning)
                        {
                            if (bOutMotorRunDir)
                                MotionForwardRun();
                            else
                                MotionBackRun();
                        }
                        else if ((LeaveCheckIo == null || LeaveCheckIo == "") && !LeaveDelayTimer.IsRuning)
                        {
                            // a 无后端检测IO  直接到出料状态
                            MotionStop();
                            LeaveDelayTimer.Stop();
                            LeaveTimer.Stop();
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无后端检测IO :状态 {LineSegState}-->{LineSegementState.WaitOut}");
                            LineSegState = LineSegementState.WaitOut;
                        }
                        else if (!(LeaveCheckIo == null || LeaveCheckIo == "") &&
                            IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo) && !LeaveDelayTimer.IsRuning)
                        {
                            // b.2 有后端检测IO感应到  离开延时定时器开启   电机运动
                            if (!LeaveDelayTimer.IsRuning)
                                LeaveDelayTimer.ResetStartTimer();
                            if (bOutMotorRunDir)
                                MotionForwardRun();
                            else
                                MotionBackRun();
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 有后端检测IO 离开延时定时器开启 : 状态 {LineSegState}");

                        }
                        else
                        {
                            // b.1 有后端检测IO感应不到 离开延时定时器时间到达 电机停止

                            if (bOutMotorRunDir)
                                MotionForwardRun();
                            else
                                MotionBackRun();
                        }
                    }
                    break;
                case LineSegementState.WaitOut:
                    if (backLineState == LineSegementState.None && !OutTimer.IsRuning)
                    {
                        // e 后一段流水线没有物料 后端进料 本端 可以出料
                        if (LeaveCheckIo == null || LeaveCheckIo == "")
                        {
                            // e 无后端检测IO  阻挡气缸下降
                            StopClinderUp(false);
                            if (!CheckStopCliyderStateInPos(false))
                                return;
                        }
                        else
                        {
                            // e 有后端检测IO  阻挡气缸上升
                            StopClinderUp(true);
                            if (!CheckStopCliyderStateInPos(true))
                                return;
                        }
                        if (!OutTimer.IsRuning)
                            OutTimer.ResetStartTimer();
                        if (bOutMotorRunDir)
                            MotionForwardRun();
                        else
                            MotionBackRun();
                    }
                    // f 后一段流水线没有物料 后端进料 本端 出料延时中
                    if (OutTimer.IsRuning)
                    {
                        if (OutTimer.IsTimerOver)
                        {

                            MotionStop();
                            if (IsLastSegLine && (LeaveCheckIo == null || LeaveCheckIo == ""))
                            {
                                sb?.Info($"{LineName}： ,{feedMode.ToString()}, 无后端检测IO, 本段是流水线最后一段 ，延时时间到 状态：{LineSegState}-->{ LineSegementState.None}");
                                LineSegState = LineSegementState.None;
                                OutTimer.Stop();
                                if (eventShowLastLineSegInfo != null)
                                    eventShowLastLineSegInfo();


                                return;
                            }
                            waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                            if (waranResult == WaranResult.Custom1)
                                OutTimer.ResetStartTimer();
                        }
                        else
                        {
                            if ((LeaveCheckIo == null || LeaveCheckIo == "") && !IsLastSegLine)
                            {
                                //  没有出料感器 下端进料完成
                                if (backLineState >= LineSegementState.BeforeHave)
                                {
                                    sb?.Info($"{LineName}： 状态：{LineSegState}-->{ LineSegementState.None}");
                                    LineSegState = LineSegementState.None;
                                    OutTimer.Stop();
                                    MotionStop();
                                    return;
                                }
                            }
                            else
                            {
                                if (IsLastSegLine)
                                {
                                    if (IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                                    {
                                        sb?.Info($"{LineName}：,{feedMode.ToString()}, 信号消失 状态：{LineSegState}-->{ LineSegementState.None}");
                                        LineSegState = LineSegementState.None;
                                        MotionStop();
                                        if (eventShowLastLineSegInfo != null)
                                            eventShowLastLineSegInfo();
                                        return;
                                    }
                                }
                                else
                                {
                                    //  有出料感器 暂停
                                    if (!IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                                    {
                                        sb?.Info($"{LineName}：,{feedMode.ToString()}, 信号消失 状态：{LineSegState}-->{ LineSegementState.None}");
                                        LineSegState = LineSegementState.None;
                                        MotionStop();
                                        return;
                                    }
                                }

                            }
                            if (bOutMotorRunDir)
                                MotionForwardRun();
                            else
                                MotionBackRun();
                        }
                    }
                    break;
            }
        }
        public void ForwardRun(Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            WaranResult waranResult;
            if (IsPause)
            {
                return;
            }
            if (!doSomingThingWhenLineRun(sb, bmaual))
                return;
            switch (LineSegState)
            {
                case LineSegementState.None:

                    OpearteLineBeforeEntry(bmaual);
                    if (!CheckLineBeforeEntry(bmaual))
                        return;

                    if ((EnteryCheckIo == null || EnteryCheckIo == ""))
                    {
                        if ((InPosCheckIo == null || InPosCheckIo == ""))
                        {
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无前端检测IO 无中端检测IO :状态{LineSegState} --> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (!IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无前端检测IO 有中端检测IO:状态{LineSegState} --> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (InPosCheckIo == null || InPosCheckIo == "")
                    {
                        if ((EnteryCheckIo == null || EnteryCheckIo == ""))
                        {
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无中端检测IO 有前端检测IO :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo))
                        {
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无中端检测IO 有前端检测IO  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (EnteryCheckIo == InPosCheckIo && IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo))
                    {
                        sb?.Info($"{LineName} ,{feedMode.ToString()}, 有中端检测IO 有前端检测IO  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                        LineSegState = LineSegementState.Entrying;
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo) && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                    {
                        sb?.Info($"{LineName} ,{feedMode.ToString()}, 有中端检测IO 有前端检测IO  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                        LineSegState = LineSegementState.Entrying;
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                    {
                        return;
                    }
                    break;
                case LineSegementState.Entrying:
                    StopClinderUp(true);
                    if (!CheckStopCliyderStateInPos(true))
                        return;
                    if (!EnterTimer.IsRuning)
                        EnterTimer.ResetStartTimer();

                    if (EnterTimer.IsTimerOver)
                    {
                        MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} :,{feedMode.ToString()}, 进入段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                        {
                            EnterTimer.ResetStartTimer();
                            EnterDelayTimer.Stop();
                        }
                    }
                    else
                    {
                        //进料 到位延时
                        if (EnterDelayTimer.IsTimerOver)
                        {
                            EnterTimer.Stop();
                            EnterDelayTimer.Stop();
                            MotionStop();
                            sb?.Info($"{LineName}  :,{feedMode.ToString()}, 流水线运动{EnterDelayTimer.SetedTime} ms后 状态 {LineSegState}--> { LineSegementState.BeforeHave}");
                            LineSegState = LineSegementState.BeforeHave;
                        }
                        else if (EnterDelayTimer.IsRuning)
                        {
                            MotionForwardRun();
                            sb?.Info($" {LineName}  :,{feedMode.ToString()}, 流水线将运动{EnterDelayTimer.SetedTime} ms  ");
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "") && !EnterDelayTimer.IsRuning)
                        {
                            MotionForwardRun();
                            EnterDelayTimer.resumeTimer();
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无阻挡到位IO, : 启动 进入延时定时器， 流水线将运动{EnterDelayTimer.SetedTime} ms  ");
                        }
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                            MotionForwardRun();
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            IOMgr.GetInstace().ReadIoInBit(InPosCheckIo) && !EnterDelayTimer.IsRuning)
                        {
                            EnterDelayTimer.ResetStartTimer();
                            MotionForwardRun();
                            sb?.Info($" {LineName} ,{feedMode.ToString()}, 有阻挡到位IO, :启动 进入延时定时器， 流水线将运动{EnterDelayTimer.SetedTime} ms ");
                        }
                    }
                    break;
                case LineSegementState.BeforeHave:
                    OperateLineBeforeHave(bmaual);
                    if (CheckBeforeHave(bmaual))
                    {
                        sb?.Info($"{LineName}  :,{feedMode.ToString()}, 状态 {LineSegState}--> { LineSegementState.Have}");
                        LineSegState = LineSegementState.Have;
                    }
                    else
                        return;
                    break;
                default:
                    LeaveOperate(sb, backLineState, bmaual);
                    break;

             
            }
        }
        public void BackRun(Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            WaranResult waranResult;
            if (IsPause)
            {
                return;
            }
            switch (LineSegState)
            {
                case LineSegementState.None:
                    OpearteLineBeforeEntry(bmaual);
                    if (!CheckLineBeforeEntry(bmaual))
                        return;
                    if ((LeaveCheckIo == null || LeaveCheckIo == ""))
                    {
                        if ((InPosCheckIo == null || InPosCheckIo == ""))
                        {
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无后端检测IO 无中端检测IO :状态{LineSegState} --> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (!IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无后端检测IO 有中端检测IO:状态{LineSegState} --> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (InPosCheckIo == null || InPosCheckIo == "")
                    {
                        if ((LeaveCheckIo == null || LeaveCheckIo == ""))
                        {
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无中端检测IO 无后端检测IO :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                        {
                            sb?.Info($"{LineName} ,{feedMode.ToString()}, 无中端检测IO 有后端检测IO  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo) && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                    {
                        sb?.Info($"{LineName} ,{feedMode.ToString()}, 有中端检测IO 有后端检测IO  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                        LineSegState = LineSegementState.Entrying;
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                    {
                        return;
                    }
                    break;
                case LineSegementState.Entrying:

                    if (!EnterTimer.IsRuning)
                        EnterTimer.ResetStartTimer();
                    if (EnterTimer.IsTimerOver)
                    {
                        MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} ,{feedMode.ToString()}, :进入段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                        {
                            EnterTimer.ResetStartTimer();
                            EnterDelayTimer.Stop();
                        }

                    }
                    else
                    {
                        if (EnterDelayTimer.IsTimerOver)
                        {
                            // 6
                            sb?.Info($"{LineName}:,{feedMode.ToString()}, 进料延时到达，流水线状态{LineSegState}-->{LineSegementState.BeforeHave}");
                            MotionStop();
                            EnterDelayTimer.Stop();
                            EnterTimer.Stop();
                            LineSegState = LineSegementState.BeforeHave;
                        }
                        else if (EnterDelayTimer.IsRuning)
                        {
                            // 5
                            sb?.Info($"{LineName}:,{feedMode.ToString()}, 进料延时中，流水线状态：{LineSegState}");
                            MotionForwardRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                            && !BackEnterDelayTimer.IsRuning)
                        {
                            sb?.Info($"{LineName}: ,{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 退料定时器开启 反向进料");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                            && (BackEnterDelayTimer.IsRuning))
                        {
                            sb?.Info($"{LineName}: ,{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 退料定时器开启 反向进料");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                           && BackEnterDelayTimer.IsTimerOver)
                        {
                            sb?.Info($"{LineName}: ,{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 进料定时器到达 退料结束  正向进料");
                            MotionStop();
                            StopClinderUp(true);
                            if (!CheckStopCliyderStateInPos(true))
                                return;
                            MotionForwardRun();
                            EnterDelayTimer.ResetStartTimer();
                        }
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            BackEnterDelayTimer.IsRuning && IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            // 3
                            sb?.Info($"{LineName}: ,{feedMode.ToString()}, 流水线状态{LineSegState} 有中段检测信号 并检测到 退料定时中， 继续退料");
                            MotionBackRun();
                        }

                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            BackEnterDelayTimer.IsRuning && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            // 4
                            sb?.Info($"{LineName}:,{feedMode.ToString()},  流水线状态{LineSegState} 无中段检测信号 并检测到下降沿 进料定时器开启  继续进料");
                            MotionStop();
                            StopClinderUp(true);
                            if (!CheckStopCliyderStateInPos(true))
                                return;
                            MotionForwardRun();
                            EnterDelayTimer.ResetStartTimer();

                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            // 2
                            sb?.Info($"{LineName}: ,{feedMode.ToString()}, 流水线状态{LineSegState} 有中段检测信号 并检测到 退料定时器开启");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else
                        {
                            // 1
                            sb?.Info($"{LineName}: ,{feedMode.ToString()}, 流水线状态{LineSegState} 中段检测信号无");
                            MotionBackRun();
                            BackEnterDelayTimer.Stop();
                        }
                    }
                    break;
                case LineSegementState.BeforeHave:
                    OperateLineBeforeHave(bmaual);
                    if (CheckBeforeHave(bmaual))
                    {
                        LineSegState = LineSegementState.Have;
                    }
                    else
                        return;
                    break;
                default:
                    LeaveOperate(sb, backLineState, bmaual);
                    break;
            }
        }

        public virtual void OpearteLineBeforeEntry(bool bmanual)
        {
            StopClinderUp(false);
            JackUpCliyderUp(false);

        }
        public virtual bool CheckLineBeforeEntry(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(false) &&
                CheckStopCliyderStateInPos(false);
        }
        public virtual void OperateLineBeforeHave(bool bmanual)
        {
            JackUpCliyderUp(true);
        }
        public virtual bool CheckBeforeHave(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(false);
        }
        public delegate void ShowLastSegInfo();
        public event ShowLastSegInfo eventShowLastLineSegInfo = null;


        public virtual bool CheckBeforeLeave(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(false) &&
                CheckStopCliyderStateInPos(false);
        }
        public virtual void OperateLineBeforeLevave(bool bmanual)
        {
            JackUpCliyderUp(false);
        }
    }


    public class LineSegmentUpDownUseMotor : LineSegmentAction
    {

        public LineSegmentUpDownUseMotor(string Name)
            : base(Name)
        {
            LineName = Name;
        }
        /// <summary>
        /// 升降轴
        /// </summary>
        public int nAxisNo = -1;
        /// <summary>
        /// 进料位置（高度）
        /// </summary>

        public double dFeedPos = 0;
        /// <summary>
        /// 出料高度
        /// </summary>
        public double dDischargePos = 0;



        public override  void OpearteLineBeforeEntry(bool bmanual)
        {

            JackUpCliyderUp(false);
            MotionMgr.GetInstace().AbsMove(nAxisNo, dFeedPos, (double)SpeedType.High);

        }
        public override bool CheckLineBeforeEntry(bool bmanual)
        {
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) > AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{LineName}  在进料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            return CheckJackUpCliyderStateInPos(false) && bIsAxisStop;

        }
        public override void OperateLineBeforeHave(bool bmanual)
        {
            JackUpCliyderUp(true);
        }
        public override bool CheckBeforeHave(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(false);
        }
        public override bool CheckBeforeLeave(bool bmanual)
        {
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) > AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{LineName}  在出料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            return CheckJackUpCliyderStateInPos(false) && bIsAxisStop;
        }
        public override void OperateLineBeforeLevave(bool bmanual)
        {
            JackUpCliyderUp(false);
            MotionMgr.GetInstace().AbsMove(nAxisNo, dDischargePos, (double)SpeedType.High);
        }
    }

   

    public class LineNg : LineSegmentAction
    {
        public LineNg(string name):base(name)
        {

        }
        public string  FullMaterialCheckIO ="";
        public override void OpearteLineBeforeEntry(bool bmanual)
        {
            if(IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO))
            {
                if (GlobalVariable.g_StationState == StationState.StationStateRun)
                {
                    StationMgr.GetInstance().Pause();
                    AlarmMgr.GetIntance().WarnWithDlg($"流水线{LineName}  在出料前，满料,程序暂停", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                }  
            }
           else  if(!IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo))
            {
                MotionStop();
            }
            else
            {
                MotionForwardRun();
            }

        }
        public override bool CheckLineBeforeEntry(bool bmanual)
        {
            return !IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo) && !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO);
        }
        public override void OperateLineBeforeHave(bool bmanual)
        {
            
        }
        public override bool CheckBeforeHave(bool bmanual)
        {
            return true;
        }
        public override void OperateLineBeforeLevave(bool bmanual)
        {
            if (!IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO))
            {
                MotionStop();
            }
            else
            {
                MotionForwardRun();
            }
        }
        public override bool CheckBeforeLeave(bool bmanual)
        {
            return  !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO);
        }
       

    }


}
