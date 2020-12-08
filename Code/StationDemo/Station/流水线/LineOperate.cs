using BaseDll;
using CommonTools;
using MotionIoLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UserData;

namespace StationDemo
{
    public interface ILineOprater
    {
        void OpearteLineBeforeEntry(bool bmanual);

        bool IsCanEntry(bool bmanual);

        void OperateLineReadyIng(bool bmanual);

        bool IsbeReady(bool bmanual);

        bool IsCanLeave(bool bmanual);

        void OperateLineBeforeLevave(bool bmanual);

        bool IsCanPut(bool bmanual);

        void OprateOutFinishDeal(bool bmaual);

        bool IsOutFinishDealOK(bool bmaual);
    }

    public class OprateNormalLine : ILineOprater
    {
        public LineSegmentDataBase lb
        {
            set;
            get;
        }

        public bool IsOKAwaysOnLineRun(bool bmanual)
        {
            return true;
        }

        public void OpearteLineBeforeEntry(bool bmanual)
        {
            lb?.StopClinderUp(true);
            lb?.JackUpCliyderUp(false);
        }

        public bool IsCanEntry(bool bmanual)
        {
            return lb.CheckJackUpCliyderStateInPos(false) &&
                lb.CheckStopCliyderStateInPos(true);
        }

        public void OperateLineReadyIng(bool bmanual)
        {
            lb?.JackUpCliyderUp(true);
        }

        public bool IsbeReady(bool bmanual)
        {
            return lb.CheckJackUpCliyderStateInPos(false);
        }

        public bool IsCanLeave(bool bmanual)
        {
            return lb.CheckJackUpCliyderStateInPos(false) &&
                lb.CheckStopCliyderStateInPos(false);
        }

        public void OperateLineBeforeLevave(bool bmanual)
        {
            lb.JackUpCliyderUp(false);
            lb.StopClinderUp(false);
        }

        public void OprateOutFinishDeal(bool bmaual)
        {
            lb?.StopClinderUp(true);
        }

        public bool IsOutFinishDealOK(bool bmaual)
        {
            return lb.CheckStopCliyderStateInPos(true);
        }

        public bool IsCanPut(bool bmanual)
        {
            return true;
        }
    }

    public class LineSegmentAction : LineSegmentDataBase
    {
        public LineSegmentAction(string Name)
        {
            LineName = Name;
            DealAlarm();
        }

        public LineSegementState GetState()
        {
            return LineSegState;
        }

        public Stationbase sb = null;

        public delegate bool DoSomeThingHandler(Stationbase sb, bool bmaual);

        public event DoSomeThingHandler SomeThingsOnlineRun = null;

        public delegate bool ShowLastSegInfo();

        public event ShowLastSegInfo eventShowLastLineSegInfo = null;

        public delegate bool IsOKAwaysOnLineRunUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event IsOKAwaysOnLineRunUser eventIsOKAwaysOnLineRunUser = null;

        public delegate void OpearteLineBeforeEntryUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event OpearteLineBeforeEntryUser eventOpearteLineBeforeEntryUser = null;

        public delegate bool IsCanEntryUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event IsCanEntryUser eventIsCanEntryUser = null;

        public delegate void OpearteLineBeforeBackEntryUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event OpearteLineBeforeBackEntryUser eventOpearteLineBeforeBackEntryUser = null;

        public delegate bool IsCanBackEntryUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event IsCanBackEntryUser eventIsCanBackEntryUser = null;

        public delegate void OperateLineReadyIngUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event OperateLineReadyIngUser eventOperateLineReadyIngUser = null;

        public delegate bool IsbeReadyUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event IsbeReadyUser eventIsbeReadyUser = null;

        public delegate bool IsCanLeaveUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event IsCanLeaveUser eventIsCanLeaveUser = null;

        public delegate void OperateLineBeforeLevaveUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event OperateLineBeforeLevaveUser eventOperateLineBeforeLevaveUser = null;

        public delegate bool IsCanOutUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event IsCanOutUser eventIsCanOutUser = null;

        public delegate void OperateLineBeforeOutUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event OperateLineBeforeOutUser eventOperateLineBeforeOutUser = null;

        public delegate void OprateOutFinishDealUser(LineSegmentAction lineobj, Stationbase sb, bool bmaual);

        public event OprateOutFinishDealUser eventOprateOutFinishDealUser = null;

        public delegate bool IsOutFinishDealOKUser(LineSegmentAction lineobj, Stationbase sb, bool bmaual);

        public event IsOutFinishDealOKUser eventIsOutFinishDealOKUser = null;

        public delegate bool IsCanPutUser(LineSegmentAction lineobj, Stationbase sb, bool bmanual);

        public event IsCanPutUser eventIsCanPutUser = null;

        //public static  System.Collections.Hashtable htSyn = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        public static ConcurrentDictionary<string, LineException> htSyn = new ConcurrentDictionary<string, LineException>();

        public static int nIsStartCheck = 0;

        public static void DealAlarm()
        {
            Task.Run(() =>
            {
                //if (nIsStartCheck == 1)
                //    return;
                int s = Interlocked.CompareExchange(ref nIsStartCheck, 1, 0);
                if (s == 1 && nIsStartCheck == 1)
                    return;

                List<string> strAlarmMsgs = new List<string>(); ;
                while (true)
                {
                    strAlarmMsgs = htSyn.Keys.ToList();
                    for (int i = 0; strAlarmMsgs != null && i < strAlarmMsgs.Count; i++)
                    {
                        LineException exp = null;
                        if (htSyn.ContainsKey(strAlarmMsgs[i]))
                        {
                            htSyn[strAlarmMsgs[i]]?.AlarmDeal();
                            htSyn.TryRemove(strAlarmMsgs[i], out exp);
                        }
                    }
                    strAlarmMsgs.Clear();
                    Thread.Sleep(100);
                }
            }
            );
        }

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

        public void LineRun(Stationbase sb, LineSegementState FrontLineState, LineSegementState backLineState, bool bmaual)
        {
            // LineSegementState FrontLineState = FrontLine.LineSegState;
            //  LineSegementState backLineState = backLine.LineSegState;
            switch (feedMode)
            {
                case FeedMode.前进料:
                    ForwardRun(sb, FrontLineState, backLineState, bmaual);
                    break;

                case FeedMode.后进料:
                    BackRun(sb, backLineState, FrontLineState, bmaual);
                    break;
            }
        }

        protected cUserTimer cUserTimerCheck = new cUserTimer(30000);
        protected string strMsg = "";
        protected string strOldWarnMsg = "";
        protected bool bIsTimerStatrtFalg = false;

        public void InfoAndCheck(Stationbase sb, string strmsg, bool IsNeedTimer = false, LineException genLineException = null)
        {
            if (strmsg != strMsg)
            {
                cUserTimerCheck.Stop();
                bIsTimerStatrtFalg = true;
                if (IsNeedTimer)
                {
                    cUserTimerCheck.ResetStartTimer();
                }
                strMsg = strmsg;
                sb?.Info(strmsg);
            }
            else
            {
                if (cUserTimerCheck.IsRuning && cUserTimerCheck.IsTimerOver && IsNeedTimer && !htSyn.ContainsKey(strmsg))
                {
                    cUserTimerCheck.Stop();
                    PushLineException(strmsg, genLineException);
                    sb.Warn("超时:" + strmsg);
                }
            }
        }

        private void PushLineException(string strmsg, LineException genLineException)
        {
            if (genLineException != null)
            {
                genLineException.strAlarmmsg = strmsg;
                genLineException.lineObj = this;
            }
            if (strOldWarnMsg != strmsg || !htSyn.ContainsKey(strmsg))
            {
                htSyn.TryAdd(strmsg, genLineException);
            }
            strOldWarnMsg = strmsg;
        }

        private void LeaveOperate(Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            WaranResult waranResult;
            switch (LineSegState)
            {
                case LineSegementState.Have:
                    InfoAndCheck(sb, $"{LineName},状态 {LineSegState} 等待其他工站作业完成");
                    if (linePassMode == LinePassMode.直通)
                    {
                        InfoAndCheck(sb, $"{LineName} 直通模式 :状态 {LineSegState}--> { LineSegementState.Finish}");
                    }
                    break;

                case LineSegementState.Finish:
                    OperateLineBeforeLevave(bmaual);
                    if (!IsCanLeave(bmaual))
                    {
                        LineException exp = new LineExceptionLeaveingCheck(this);
                        InfoAndCheck(sb, $"{LineName}  :状态 {LineSegState} 正在使用IsCanLeave 方法检查是否可以离开 检查顶升气缸和阻挡气缸下降到位", true, exp);
                        return;
                    }
                    if (eventOperateLineBeforeLevaveUser != null)
                        eventOperateLineBeforeLevaveUser?.Invoke(this, sb, bmaual);
                    if (eventIsCanLeaveUser != null && !eventIsCanLeaveUser(this, sb, bmaual))
                    {
                        InfoAndCheck(sb, $"{LineName},状态 {LineSegState} 正在使用eventOperateLineBeforeLevaveUser 方法检查是否可以离开", true);
                        return;
                    }
                    LeaveDelayTimer.Stop();
                    LeaveTimer.Stop();
                    OutTimer.Stop();
                    //物料取料完成  加工完成
                    if (LeaveCheckIo == null || LeaveCheckIo == "")
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 无后端检测IO  状态： {LineSegState}--> { LineSegementState.Leaveing}");
                        if (backLineState == LineSegementState.None)
                            LineSegState = LineSegementState.Leaveing;
                    }
                    else if (LeaveCheckIo == InPosCheckIo)
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 有后端检测IO    没有信号 状态： {LineSegState}--> { LineSegementState.Leaveing}");
                        LineSegState = LineSegementState.Leaveing;
                    }
                    else if (!IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 有后端检测IO    没有信号 状态： {LineSegState}--> { LineSegementState.Leaveing}");
                        LineSegState = LineSegementState.Leaveing;
                    }
                    else
                    {
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName},{feedMode.ToString()}, 后端检测信号有, 物料可能卡住，或者有物料放置，摆好重试", null, new string[] { "重试", }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                            return;
                    }
                    break;

                case LineSegementState.Leaveing:
                    StopClinderUp(false);
                    if (!CheckStopCliyderStateInPos(false))
                    {
                        LineException exp = new LineExceptionLeaveingCheck(this);
                        InfoAndCheck(sb, $"{LineName}  :状态 {LineSegState} 用CheckStopCliyderStateInPos 方法检查阻挡气缸是否下降到位", true, exp);
                        return;
                    }
                    if (!LeaveTimer.IsRuning)
                        LeaveTimer.ResetStartTimer();
                    if (LeaveTimer.IsTimerOver)
                    {
                        // c 离开定时器  时间到
                        MotionStop();
                        LineException lineException = new LineExceptionLeavingTimeout(this);
                        PushLineException($"{LineName},{feedMode.ToString()}, 离开段超时,可能料被拿走 或者卡住", lineException);
                        //waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName},{feedMode.ToString()}, 离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试","已经人工拿走" });
                        //if (waranResult == WaranResult.Custom1)
                        //{
                        //    LeaveTimer.ResetStartTimer();
                        //    LeaveDelayTimer.Stop();
                        //}
                        //if (waranResult == WaranResult.Custom2)
                        //{
                        //    LineSegState = LineSegementState.None;
                        //}
                    }
                    else
                    {
                        if (LeaveDelayTimer.IsTimerOver)
                        {
                            // b.3 有后端检测IO感应  离开延时定时器时间到达 电机停止
                            LeaveDelayTimer.Stop();
                            LeaveTimer.Stop();
                            MotionStop();
                            InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 有后端检测IO 离开延时定时器到达 : 状态 {LineSegState}--> { LineSegementState.WaitOut}");
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
                            InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 无后端检测IO :状态 {LineSegState}-->{LineSegementState.WaitOut}");
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
                            InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 有后端检测IO 离开延时定时器开启 : 状态 {LineSegState}");
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
                    if (eventOperateLineBeforeOutUser != null)
                    {
                        eventOperateLineBeforeOutUser(this, sb, bmaual);
                    }
                    if (eventIsCanOutUser != null)
                    {
                        if (!eventIsCanOutUser(this, sb, bmaual))
                            return;
                    }

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
                        InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},  出料延时定时器开启 : 状态 {LineSegState}->{LineSegementState.Outing}");
                        LineSegState = LineSegementState.Outing;
                    }
                    else
                    {
                        InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},   : 状态 {LineSegState}");
                    }
                    break;

                case LineSegementState.Outing:
                    // f 后一段流水线没有物料 后端进料 本端 出料延时中
                    if (OutTimer.IsRuning)
                    {
                        if (OutTimer.IsTimerOver)
                        {
                            MotionStop();
                            if ((outFinishJudeType == OutFinishJudeType.信号 || outFinishJudeType == OutFinishJudeType.下一段流水线或信号)
                                && (LeaveCheckIo == null || LeaveCheckIo == ""))
                            {
                                InfoAndCheck(sb, $"{LineName}:{feedMode.ToString()}, 无后端检测IO, 本段是流水线最后一段 ，延时时间到 状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                LineSegState = LineSegementState.OutFinish;
                                OutTimer.Stop();
                                return;
                            }
                            LineException lineException = new LineExceptionOutTimeout(this);
                            PushLineException($"{LineName} :出料段超时,可能料被拿走 或者卡住", lineException);

                            //waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试", "已经人工拿走" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                            //if (waranResult == WaranResult.Custom1)
                            //    OutTimer.ResetStartTimer();
                            //if (waranResult == WaranResult.Custom2)
                            //    LineSegState= LineSegementState.None;
                        }
                        else
                        {
                            if (outFinishJudeType == OutFinishJudeType.下一段流水线或信号)
                            {
                                if (backLineState >= LineSegementState.ReadyIng)
                                {
                                    InfoAndCheck(sb, $"{LineName}：{feedMode.ToString()}, 下一段流水线状态改变成 准备状态 本段流水线状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                    LineSegState = LineSegementState.OutFinish;
                                    MotionStop();
                                    return;
                                }
                                if (!(LeaveCheckIo == null || LeaveCheckIo == "") && !IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                                {
                                    InfoAndCheck(sb, $"{LineName}：{feedMode.ToString()}, 信号消失 状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                    LineSegState = LineSegementState.OutFinish;
                                    MotionStop();
                                    return;
                                }
                            }
                            else if (outFinishJudeType == OutFinishJudeType.下一段流水线)
                            {
                                if (backLineState >= LineSegementState.ReadyIng)
                                {
                                    InfoAndCheck(sb, $"{LineName}：{feedMode.ToString()}, 下一段流水线状态改变成 准备状态 本段流水线 状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                    LineSegState = LineSegementState.OutFinish;
                                    MotionStop();
                                    return;
                                }
                            }
                            else if (outFinishJudeType == OutFinishJudeType.信号)
                            {
                                //  有出料感器 信号消失
                                if (!(LeaveCheckIo == null || LeaveCheckIo == "") && !IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                                {
                                    InfoAndCheck(sb, $"{LineName}：{feedMode.ToString()}, 信号消失 状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                    LineSegState = LineSegementState.OutFinish;
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
                    break;

                case LineSegementState.OutFinish:
                    if (!OutDelayTimer.IsRuning)
                        OutDelayTimer.ResetStartTimer();
                    if (OutDelayTimer.IsRuning && !OutDelayTimer.IsTimerOver)
                    {
                        if (bOutMotorRunDir)
                            MotionForwardRun();
                        else
                            MotionBackRun();
                        InfoAndCheck(sb, $"{LineName}：{feedMode.ToString()},   状态：{LineSegState} 流水线正在 延时出料 延时时间{OutDelayTimer.SetedTime}");
                        return;
                    }
                    MotionStop();
                    OprateOutFinishDeal(bmaual);
                    if (!IsOutFinishDealOK(bmaual))
                        return;
                    if (eventOprateOutFinishDealUser != null)
                        eventOprateOutFinishDealUser(this, sb, bmaual);
                    if (eventIsOutFinishDealOKUser != null && !eventIsOutFinishDealOKUser(this, sb, bmaual))
                        return;
                    if (eventShowLastLineSegInfo != null && !eventShowLastLineSegInfo())
                        return;
                    InfoAndCheck(sb, $"{LineName}：{feedMode.ToString()},  状态：{LineSegState}-->{ LineSegementState.None}");
                    LineSegState = LineSegementState.None;
                    break;
            }
        }

        public void ForwardRun(Stationbase sb, LineSegementState FrontLineState, LineSegementState backLineState, bool bmaual)
        {
            //LineSegementState FrontLineState = FrontLine.LineSegState;
            //LineSegementState backLineState = backLine.LineSegState;
            WaranResult waranResult;
            if (IsPause)
            {
                InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},状态{LineSegState} 流水线状态机 暂停");
                return;
            }
            if (!doSomingThingWhenLineRun(sb, bmaual))
            {
                InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},状态{LineSegState} 流水线状态机 不满做条件");
                return;
            }

            switch (LineSegState)
            {
                case LineSegementState.None:

                    OpearteLineBeforeEntry(bmaual);
                    if (!IsCanEntry(bmaual))
                    {
                        LineException exp = new LineExceptionEntreyCheck(this);
                        InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},状态{LineSegState}用方法IsCanEntry检查 不满足进入Entrying条件: 阻挡气缸上升是否到位，顶升气缸是否下降到位", true, exp);
                        return;
                    }
                    eventOpearteLineBeforeEntryUser?.Invoke(this, sb, bmaual);
                    if (eventIsCanEntryUser != null && !eventIsCanEntryUser(this, sb, bmaual))
                    {
                        InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},状态{LineSegState}用方法eventIsCanEntryUser检查 不满足进入Entrying条件", true);
                        return;
                    }
                    if (entryingJude == EntryingJudeType.信号 || entryingJude == EntryingJudeType.上一段流水线或信号)
                        FrontLineState = LineSegementState.Outing;

                    if (FrontLineState == LineSegementState.Outing)
                    {
                        if ((EnteryCheckIo == null || EnteryCheckIo == ""))
                        {
                            if ((InPosCheckIo == null || InPosCheckIo == ""))
                            {
                                InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 无前端检测IO 无中端检测IO :状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                            else if (!IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                            {
                                InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 无前端检测IO 有中端检测IO:《{InPosCheckIo}》 尚未检测到信号 :状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                            else if (IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                            {
                                InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 无前端检测IO 有中端检测IO:《{InPosCheckIo}》并检测到 :状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                        else if (InPosCheckIo == null || InPosCheckIo == "")
                        {
                            if ((EnteryCheckIo == null || EnteryCheckIo == ""))
                            {
                                InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 无中端检测IO 无前端检测IO:状态 {LineSegState}--> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                            else if (IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo))
                            {
                                InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 无中端检测IO 有前端检测IO:《{EnteryCheckIo}》 :状态 {LineSegState}--> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                        else if (EnteryCheckIo == InPosCheckIo && IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo))
                        {
                            InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 有中端检测IO:《{InPosCheckIo}》,有前端检测IO:《{EnteryCheckIo}》:状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 有中端检测IO:《{InPosCheckIo}》, 有前端检测IO:《{EnteryCheckIo}》有到位信号《{InPosCheckIo}》 :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo) && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 有中端检测IO:《{InPosCheckIo}》, 有前端检测IO:《{EnteryCheckIo}》  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()}, 有中端检测IO:《{InPosCheckIo}》并检测到 请取走 , 有前端检测IO:《{EnteryCheckIo}》 ");
                            return;
                        }
                    }
                    else
                    {
                        InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},状态{LineSegState} 等待上一段流水线 出料进入条件", true);
                    }
                    break;

                case LineSegementState.Entrying:
                    StopClinderUp(true);
                    if (!CheckStopCliyderStateInPos(true))
                    {
                        LineException exp = new LineExceptionEntreyCheck(this);
                        InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},状态{LineSegState}阻挡气缸上升没有满足条件", true, exp);
                        return;
                    }
                    if (!EnterTimer.IsRuning)
                        EnterTimer.ResetStartTimer();

                    if (EnterTimer.IsTimerOver)
                    {
                        MotionStop();
                        LineException lineException = new LineExceptionEntryTimeOut(this);
                        PushLineException($"{LineName} :{feedMode.ToString()}, 进入段超时,可能料被拿走 或者卡住", lineException);
                        //waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} :{feedMode.ToString()}, 进入段超时,可能料被拿走 或者卡住", null, new string[] { "重试" ,"已经人工拿走","进料完成"}, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        //if (waranResult == WaranResult.Custom1)
                        //{
                        //    EnterTimer.ResetStartTimer();
                        //    EnterDelayTimer.Stop();
                        //}
                        //if (waranResult == WaranResult.Custom2)
                        //    LineSegState = LineSegementState.None;
                        //if (waranResult == WaranResult.Custom3)
                        //    LineSegState = LineSegementState.ReadyIng;
                    }
                    else
                    {
                        //进料 到位延时
                        if (EnterDelayTimer.IsTimerOver)
                        {
                            EnterTimer.Stop();
                            EnterDelayTimer.Stop();
                            MotionStop();
                            InfoAndCheck(sb, $"{LineName}，{feedMode.ToString()}, 流水线运动{EnterDelayTimer.SetedTime} ms后 状态 {LineSegState}--> { LineSegementState.ReadyIng}");
                            LineSegState = LineSegementState.ReadyIng;
                        }
                        else if (EnterDelayTimer.IsRuning)
                        {
                            MotionForwardRun();
                            InfoAndCheck(sb, $" {LineName}，{feedMode.ToString()}, 流水线将运动{EnterDelayTimer.SetedTime} ms  ");
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "") && !EnterDelayTimer.IsRuning)
                        {
                            MotionForwardRun();
                            EnterDelayTimer.resumeTimer();
                            InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 无阻挡到位IO, : 启动 进入延时定时器， 流水线将运动{EnterDelayTimer.SetedTime} ms  ");
                        }
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                            MotionForwardRun();
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            IOMgr.GetInstace().ReadIoInBit(InPosCheckIo) && !EnterDelayTimer.IsRuning)
                        {
                            EnterDelayTimer.ResetStartTimer();
                            MotionForwardRun();
                            InfoAndCheck(sb, $" {LineName},{feedMode.ToString()}, 有阻挡到位IO《{InPosCheckIo}》, :启动 进入延时定时器， 流水线将运动{EnterDelayTimer.SetedTime} ms ");
                        }
                    }
                    break;

                case LineSegementState.ReadyIng:
                    eventOperateLineReadyIngUser?.Invoke(this, sb, bmaual);
                    if (eventIsbeReadyUser != null && !eventIsbeReadyUser(this, sb, bmaual))
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()},状态{LineSegState} 用方法eventIsbeReadyUser检查 不满足进入准备有料（have）条件", true);
                        return;
                    }
                    OperateLineReadyIng(bmaual);
                    if (IsbeReady(bmaual))
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 状态 {LineSegState}--> { LineSegementState.Have}", true);
                        LineSegState = LineSegementState.Have;
                    }
                    else
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()},状态{LineSegState} 用方法IsbeReady检查 不满足进入准备有料（have）条件", true);
                        return;
                    }
                    break;

                default:
                    LeaveOperate(sb, backLineState, bmaual);
                    break;
            }
        }

        public void BackRun(Stationbase sb, LineSegementState FrontLineState, LineSegementState backLineState, bool bmaual)
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
                    OpearteLineBeforeBackEntry(bmaual);
                    if (!IsCanBackEntry(bmaual))
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()},状态{LineSegState}用方法IsCanBackEntry检查 不满足进入Entrying条件", true);
                        return;
                    }
                    eventOpearteLineBeforeBackEntryUser?.Invoke(this, sb, bmaual);
                    if (eventIsCanBackEntryUser != null && !eventIsCanBackEntryUser(this, sb, bmaual))
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()},状态{LineSegState}用方法eventIsCanBackEntryUser检查 不满足进入Entrying条件", true);
                        return;
                    }
                    if (entryingJude == EntryingJudeType.信号 || entryingJude == EntryingJudeType.上一段流水线或信号)
                        FrontLineState = LineSegementState.Outing;
                    if (FrontLineState == LineSegementState.Outing)
                    {
                        if ((LeaveCheckIo == null || LeaveCheckIo == ""))
                        {
                            if ((InPosCheckIo == null || InPosCheckIo == ""))
                            {
                                InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 无后端检测IO 无中端检测IO :状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                            else if (!IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                            {
                                InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 无后端检测IO 有中端检测IO《{InPosCheckIo}》:状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                        else if (InPosCheckIo == null || InPosCheckIo == "")
                        {
                            if ((LeaveCheckIo == null || LeaveCheckIo == ""))
                            {
                                InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 无中端检测IO 无后端检测IO :状态 {LineSegState}--> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                            else if (IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                            {
                                InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 无中端检测IO 有后端检测IO《{LeaveCheckIo}》  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                        else if (LeaveCheckIo == InPosCheckIo && IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 有中端检测IOO《{InPosCheckIo}》 有后端检测IO O《{LeaveCheckIo}》  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo) && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 有中端检测IO 有后端检测IO《{LeaveCheckIo}》  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            return;
                        }
                    }
                    break;

                case LineSegementState.Entrying:

                    if (!EnterTimer.IsRuning)
                        EnterTimer.ResetStartTimer();
                    if (EnterTimer.IsTimerOver)
                    {
                        MotionStop();
                        LineException lineException = new LineExceptionEntryTimeOut(this);
                        PushLineException($"{LineName} ,{feedMode.ToString()}, :进入段超时,可能料被拿走 或者卡住", lineException);
                        //waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} ,{feedMode.ToString()}, :进入段超时,可能料被拿走 或者卡住", null, new string[] { "重试","已经人工拿走","" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        //if (waranResult == WaranResult.Custom1)
                        //{
                        //    EnterTimer.ResetStartTimer();
                        //    EnterDelayTimer.Stop();
                        //}
                        //if (waranResult == WaranResult.Custom2)
                        //    LineSegState = LineSegementState.None;
                        //if (waranResult == WaranResult.Custom3)
                        //    LineSegState = LineSegementState.ReadyIng;
                    }
                    else
                    {
                        if (EnterDelayTimer.IsTimerOver)
                        {
                            // 6
                            InfoAndCheck(sb, $"{LineName}，{feedMode.ToString()}, 进料延时到达，流水线状态{LineSegState}-->{LineSegementState.ReadyIng}");
                            MotionStop();
                            EnterDelayTimer.Stop();
                            EnterTimer.Stop();
                            LineSegState = LineSegementState.ReadyIng;
                        }
                        else if (EnterDelayTimer.IsRuning)
                        {
                            // 5
                            InfoAndCheck(sb, $"{LineName}，{feedMode.ToString()}, 进料延时中，流水线状态：{LineSegState}");
                            MotionForwardRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                            && !BackEnterDelayTimer.IsRuning)
                        {
                            InfoAndCheck(sb, $"{LineName}:{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 退料定时器开启 反向进料");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                            && (BackEnterDelayTimer.IsRuning))
                        {
                            InfoAndCheck(sb, $"{LineName}，{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 退料定时器开启 反向进料");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                           && BackEnterDelayTimer.IsTimerOver)
                        {
                            InfoAndCheck(sb, $"{LineName}，{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 进料定时器到达 退料结束  正向进料");
                            MotionStop();
                            StopClinderUp(true);
                            if (!CheckStopCliyderStateInPos(true))
                                return;
                            MotionForwardRun();
                            EnterDelayTimer.ResetStartTimer();
                            InfoAndCheck(sb, $"{LineName}:{feedMode.ToString()}, 流水线状态{LineSegState}->{LineSegementState.BackEntryReady} 无中段检测信号 进料定时器到达 退料结束  正向进料");
                            LineSegState = LineSegementState.BackEntryReady;
                        }
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            BackEnterDelayTimer.IsRuning && IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            // 3
                            InfoAndCheck(sb, $"{LineName}，{feedMode.ToString()}, 流水线状态{LineSegState} 有中段检测信号《{InPosCheckIo}》 并检测到 退料定时中， 继续退料");
                            MotionBackRun();
                        }
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            BackEnterDelayTimer.IsRuning && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            // 4
                            InfoAndCheck(sb, $"{LineName}，{feedMode.ToString()},  流水线状态{LineSegState} 无中段检测信号 并检测到下降沿 进料定时器开启  继续进料");
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
                            InfoAndCheck(sb, $"{LineName}，{feedMode.ToString()}, 流水线状态{LineSegState} 有中段检测信号《{InPosCheckIo}》 并检测到 退料定时器开启");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else
                        {
                            // 1
                            InfoAndCheck(sb, $"{LineName}: ,{feedMode.ToString()}, 流水线状态{LineSegState} 中段检测信号无");
                            MotionBackRun();
                            BackEnterDelayTimer.Stop();
                        }
                    }
                    break;

                case LineSegementState.BackEntryReady:
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
                            InfoAndCheck(sb, $"{LineName}:{feedMode.ToString()}, 进料延时到达，流水线状态{LineSegState}-->{LineSegementState.ReadyIng}");
                            MotionStop();
                            EnterDelayTimer.Stop();
                            EnterTimer.Stop();
                            LineSegState = LineSegementState.ReadyIng;
                        }
                        else if (EnterDelayTimer.IsRuning)
                        {
                            // 5
                            InfoAndCheck(sb, $"{LineName}:{feedMode.ToString()}, 进料延时中，流水线状态：{LineSegState}");
                            MotionForwardRun();
                        }
                    }

                    break;

                case LineSegementState.ReadyIng:
                    eventOperateLineReadyIngUser?.Invoke(this, sb, bmaual);
                    if (eventIsbeReadyUser != null && !eventIsbeReadyUser(this, sb, bmaual))
                    {
                        InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},状态{LineSegState} 用方法eventIsbeReadyUser检查 不满足进入准备有料（have）条件", true);
                        return;
                    }
                    OperateLineReadyIng(bmaual);
                    if (IsbeReady(bmaual))
                    {
                        InfoAndCheck(sb, $"{LineName},{feedMode.ToString()}, 状态 {LineSegState}--> { LineSegementState.Have}");
                        LineSegState = LineSegementState.Have;
                    }
                    else
                    {
                        LineException exp = new LineExceptionReadingCheck(this);
                        InfoAndCheck(sb, $"{LineName} ,{feedMode.ToString()},状态{LineSegState} 用方法IsbeReady检查 不满足进入准备有料（have）条件", true, exp);
                        return;
                    }

                    break;

                default:
                    LeaveOperate(sb, backLineState, bmaual);
                    break;
            }
        }

        public virtual bool IsOKAwaysOnLineRun(bool bmanual)
        {
            return true;
        }

        public virtual void OpearteLineBeforeEntry(bool bmanual)
        {
            StopClinderUp(true);
            JackUpCliyderUp(false);
        }

        public virtual bool IsCanEntry(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(false) &&
                CheckStopCliyderStateInPos(true);
        }

        public virtual void OpearteLineBeforeBackEntry(bool bmanual)
        {
            StopClinderUp(false);
            JackUpCliyderUp(false);
        }

        public virtual bool IsCanBackEntry(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(false) &&
                CheckStopCliyderStateInPos(false);
        }

        public virtual void OperateLineReadyIng(bool bmanual)
        {
            JackUpCliyderUp(true);
        }

        public virtual bool IsbeReady(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(true);
        }

        public virtual bool IsCanLeave(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(false) &&
                CheckStopCliyderStateInPos(false);
        }

        public virtual void OperateLineBeforeLevave(bool bmanual)
        {
            JackUpCliyderUp(false);
            StopClinderUp(false);
        }

        public virtual void OprateOutFinishDeal(bool bmaual)
        {
            StopClinderUp(true);
        }

        public virtual bool IsOutFinishDealOK(bool bmaual)
        {
            return CheckStopCliyderStateInPos(true);
        }

        public virtual bool IsCanPut(bool bmanual)
        {
            return LineSegState == LineSegementState.None;
        }
    }

    public class LineNg : LineSegmentAction
    {
        public LineNg(string name) : base(name)
        {
            IsLastSegLine = true;
            nEnteryTimeout = 10000;
            nEnteryDelay = 800;
            nOutTimeout = 200;
        }

        public string FullMaterialCheckIO = "";

        public override void OpearteLineBeforeEntry(bool bmanual)
        {
            if (IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO))
            {
                if (GlobalVariable.g_StationState == StationState.StationStateRun)
                {
                    StationMgr.GetInstance().Pause();
                    AlarmMgr.GetIntance().WarnWithDlg($"流水线{LineName}  在出料前，满料,程序暂停", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                }
            }
            else if (!IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo))
            {
                MotionStop();
            }
            else
            {
                MotionForwardRun();
            }
        }

        public override bool IsOKAwaysOnLineRun(bool bmanual)
        {
            return (FullMaterialCheckIO != null && FullMaterialCheckIO != "") && !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO);
        }

        public override bool IsCanEntry(bool bmanual)
        {
            return !IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo) && !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO);
        }

        public override void OperateLineReadyIng(bool bmanual)
        {
        }

        public override bool IsbeReady(bool bmanual)
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

        public override bool IsCanLeave(bool bmanual)
        {
            return !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO);
        }

        public override bool IsCanPut(bool bmanual)
        {
            return (FullMaterialCheckIO != null && FullMaterialCheckIO != "") && !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO) && IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo);
        }
    }
}