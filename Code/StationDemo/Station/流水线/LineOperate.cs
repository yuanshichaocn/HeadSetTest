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

    public interface  ILineOprater
    {

    

        void OpearteLineBeforeEntry(  bool bmanual);

         bool IsCanEntry(  bool bmanual);
        
         void OperateLineReadyIng(  bool bmanual);

         bool IsbeReady(  bool bmanual);
        
        bool IsCanLeave(  bool bmanual);
      
        void OperateLineBeforeLevave(  bool bmanual);
        bool IsCanPut(  bool bmanual);

        void OprateOutFinishDeal(  bool bmaual);
      
        bool IsOutFinishDealOK(  bool bmaual);
       
    }
    public class OprateNormalLine : ILineOprater
    {

        public LineSegmentDataBase lb
        {
            set;
            get;

        }
        public  bool IsOKAwaysOnLineRun( bool bmanual)
        {
            return true;
        }
        public  void OpearteLineBeforeEntry( bool bmanual)
        {
            lb?.StopClinderUp(true);
            lb?.JackUpCliyderUp(false);

        }
        public  bool IsCanEntry( bool bmanual)
        {
            return lb.CheckJackUpCliyderStateInPos(false) &&
                lb.CheckStopCliyderStateInPos(true);
        }
        public  void OperateLineReadyIng( bool bmanual)
        {
            lb?.JackUpCliyderUp(true);
        }
        public  bool IsbeReady(  bool bmanual)
        {
            return lb.CheckJackUpCliyderStateInPos(false);
        }
        public  bool IsCanLeave(  bool bmanual)
        {
            return lb.CheckJackUpCliyderStateInPos(false) &&
                lb.CheckStopCliyderStateInPos(false);
        }
        public  void OperateLineBeforeLevave(  bool bmanual)
        {
            lb.JackUpCliyderUp(false);
            lb.StopClinderUp(false);
        }
        public  void OprateOutFinishDeal(  bool bmaual)
        {
            lb?.StopClinderUp(true);
        }
        public  bool IsOutFinishDealOK(  bool bmaual)
        {
            return lb.CheckStopCliyderStateInPos(true);
        }
        public  bool IsCanPut(  bool bmanual)
        {
            return true;
        }

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

        public ILineOprater lineOprater = new OprateNormalLine();
        public delegate bool DoSomeThingHandler(Stationbase sb, bool bmaual);

        public event DoSomeThingHandler SomeThingsOnlineRun = null;
        public delegate void ShowLastSegInfo();
        public event ShowLastSegInfo eventShowLastLineSegInfo = null;

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

        public void LineRun( Stationbase sb, LineSegementState FrontLineState, LineSegementState backLineState, bool bmaual)
        {
            switch (feedMode)
            {
                case FeedMode.前进料:
                    ForwardRun( sb, FrontLineState,  backLineState,  bmaual);
                    break;
                case FeedMode.后进料:
                    BackRun( sb, backLineState , FrontLineState,  bmaual);
                    break;
            }
        }
        private string strMsg = "";
       public void Info(Stationbase sb,string strmsg)
        {
            if(strmsg!= strMsg)
            {
                strMsg = strmsg;
                sb?.Info(strmsg);
            }

        }
        void LeaveOperate(Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            WaranResult waranResult;
            switch (LineSegState)
            {
                case LineSegementState.Have:
                   
                    if (linePassMode == LinePassMode.直通)
                    {
                        Info(sb,$"{LineName} 直通模式 :状态 {LineSegState}--> { LineSegementState.Finish}");
                    }
                    //Info(sb,($"{LineName} :状态{LineSegState}");
                    break;
                case LineSegementState.Finish:
                     OperateLineBeforeLevave(bmaual);
                    if (! IsCanLeave(bmaual))
                        return;
                    LeaveDelayTimer.Stop();
                    LeaveTimer.Stop();
                    OutTimer.Stop();
                    //物料取料完成  加工完成  
                    if (LeaveCheckIo == null || LeaveCheckIo == "")
                    {

                        Info(sb,$"{LineName} ,{feedMode.ToString()}, 无后端检测IO  状态： {LineSegState}--> { LineSegementState.Leaveing}");
                        if (backLineState == LineSegementState.None)
                            LineSegState = LineSegementState.Leaveing;

                    }
                    else if (!IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                    {
                       Info(sb,$"{LineName} ,{feedMode.ToString()}, 有后端检测IO    没有信号 状态： {LineSegState}--> { LineSegementState.Leaveing}");
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
                           Info(sb,$"{LineName} ,{feedMode.ToString()}, 有后端检测IO 离开延时定时器到达 : 状态 {LineSegState}--> { LineSegementState.WaitOut}");
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
                           Info(sb,$"{LineName} ,{feedMode.ToString()}, 无后端检测IO :状态 {LineSegState}-->{LineSegementState.WaitOut}");
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
                           Info(sb,$"{LineName} ,{feedMode.ToString()}, 有后端检测IO 离开延时定时器开启 : 状态 {LineSegState}");

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
                       Info(sb,$"{LineName} ,{feedMode.ToString()},  出料延时定时器开启 : 状态 {LineSegState}->{LineSegementState.Outing}");
                        LineSegState = LineSegementState.Outing;
                    }
                    break;
                case LineSegementState.Outing:
                    // f 后一段流水线没有物料 后端进料 本端 出料延时中
                    if (OutTimer.IsRuning)
                    {
                        if (OutTimer.IsTimerOver)
                        {
                            MotionStop();
                            if (IsLastSegLine && (LeaveCheckIo == null || LeaveCheckIo == ""))
                            {
                               Info(sb,$"{LineName}:{feedMode.ToString()}, 无后端检测IO, 本段是流水线最后一段 ，延时时间到 状态：{LineSegState}-->{ LineSegementState.None}");
                                LineSegState = LineSegementState.OutFinish;
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
                            if ((LeaveCheckIo == null || LeaveCheckIo == ""))
                            {
                                if(!IsLastSegLine)
                                {
                                    //不是最后一段
                                    //  没有出料感器 下端进料完成 
                                    if (backLineState >= LineSegementState.BackEntryReady)
                                    {
                                       Info(sb,$"{LineName}： 状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                        LineSegState = LineSegementState.OutFinish;
                                        OutTimer.Stop();
                                        MotionStop();
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (IsLastSegLine)
                                {
                                    if (IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                                    {
                                       Info(sb,$"{LineName}：{feedMode.ToString()}, 信号消失 状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                        LineSegState = LineSegementState.OutFinish;
                                        MotionStop();
                                        if (eventShowLastLineSegInfo != null)
                                            eventShowLastLineSegInfo();
                                        return;
                                    }
                                }
                                else
                                {
                                    if(outFinishJudeType == OutFinishJudeType.下一段流水线)
                                    {

                                        if (backLineState >= LineSegementState.ReadyIng)
                                        {
                                           Info(sb,$"{LineName}：{feedMode.ToString()}, 信号消失 状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                            LineSegState = LineSegementState.OutFinish;
                                            MotionStop();
                                            return;
                                        }
                                    }
                                    else if( outFinishJudeType == OutFinishJudeType.信号)
                                    {
                                        //  有出料感器 信号消失 
                                        if (!IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo) )
                                        {
                                           Info(sb,$"{LineName}：{feedMode.ToString()}, 信号消失 状态：{LineSegState}-->{ LineSegementState.OutFinish}");
                                            LineSegState = LineSegementState.OutFinish;
                                            MotionStop();
                                            return;
                                        }
                                    }
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
                     OprateOutFinishDeal(bmaual);
                    if (! IsOutFinishDealOK(bmaual))
                        return;
                   Info(sb,$"{LineName}：{feedMode.ToString()},  状态：{LineSegState}-->{ LineSegementState.None}");
                    LineSegState = LineSegementState.None;
                    break;
            }
        }
        public void ForwardRun(Stationbase sb, LineSegementState FrontLineState ,LineSegementState backLineState, bool bmaual)
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
                    if (!IsCanEntry(bmaual))
                        return;

                    if ((EnteryCheckIo == null || EnteryCheckIo == ""))
                    {
                        if ((InPosCheckIo == null || InPosCheckIo == ""))
                        {
                            
                            if(FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无前端检测IO 无中端检测IO :状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                              
                        }
                        else if (!IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            if (FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无前端检测IO 有中端检测IO:{InPosCheckIo} :状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                               
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            if (FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无前端检测IO 有中端检测IO:{InPosCheckIo}并检测到 {InPosCheckIo}:状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                    }
                    else if (InPosCheckIo == null || InPosCheckIo == "")
                    {
                        if ((EnteryCheckIo == null || EnteryCheckIo == ""))
                        {
                             if (FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无中端检测IO 有前端检测IO:{EnteryCheckIo} :状态 {LineSegState}--> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }    
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo))
                        {
                            if (FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无中端检测IO 有前端检测IO:{EnteryCheckIo}  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                    }
                    
                    else if (EnteryCheckIo == InPosCheckIo && IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo))
                    {
                        if (FrontLineState == LineSegementState.Outing)
                        {
                           Info(sb,$"{LineName} ,{feedMode.ToString()}, 有中端检测IO:{InPosCheckIo} ,有前端检测IO:{EnteryCheckIo}   :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (  IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                    {
                        if (FrontLineState == LineSegementState.Outing)
                        {
                           Info(sb,$"{LineName} ,{feedMode.ToString()}, 有中端检测IO:{InPosCheckIo} , 有前端检测IO:{EnteryCheckIo}  有到位信号 :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo) && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                    {
                        if (FrontLineState == LineSegementState.Outing)
                        {
                           Info(sb,$"{LineName} ,{feedMode.ToString()}, 有中端检测IO:{InPosCheckIo} , 有前端检测IO:{EnteryCheckIo}  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                            LineSegState = LineSegementState.Entrying;
                        }
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
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} :{feedMode.ToString()}, 进入段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
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
                           Info(sb,$"{LineName}  :,{feedMode.ToString()}, 流水线运动{EnterDelayTimer.SetedTime} ms后 状态 {LineSegState}--> { LineSegementState.ReadyIng}");
                            LineSegState = LineSegementState.ReadyIng;
                        }
                        else if (EnterDelayTimer.IsRuning)
                        {
                            MotionForwardRun();
                           Info(sb,$" {LineName}  :,{feedMode.ToString()}, 流水线将运动{EnterDelayTimer.SetedTime} ms  ");
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "") && !EnterDelayTimer.IsRuning)
                        {
                            MotionForwardRun();
                            EnterDelayTimer.resumeTimer();
                           Info(sb,$"{LineName} ,{feedMode.ToString()}, 无阻挡到位IO, : 启动 进入延时定时器， 流水线将运动{EnterDelayTimer.SetedTime} ms  ");
                        }
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                            MotionForwardRun();
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            IOMgr.GetInstace().ReadIoInBit(InPosCheckIo) && !EnterDelayTimer.IsRuning)
                        {
                            EnterDelayTimer.ResetStartTimer();
                            MotionForwardRun();
                           Info(sb,$" {LineName} ,{feedMode.ToString()}, 有阻挡到位IO, :启动 进入延时定时器， 流水线将运动{EnterDelayTimer.SetedTime} ms ");
                        }
                    }
                    break;
                case LineSegementState.ReadyIng:
                     OperateLineReadyIng(bmaual);
                    if ( IsbeReady(bmaual))
                    {
                       Info(sb,$"{LineName}  :,{feedMode.ToString()}, 状态 {LineSegState}--> { LineSegementState.Have}");
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
                    if (! IsCanBackEntry(bmaual))
                        return;
                    if ((LeaveCheckIo == null || LeaveCheckIo == ""))
                    {
                        if ((InPosCheckIo == null || InPosCheckIo == ""))
                        {
                            if (FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无后端检测IO 无中端检测IO :状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                        else if (!IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            if (FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无后端检测IO 有中端检测IO:状态{LineSegState} --> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                    }
                    else if (InPosCheckIo == null || InPosCheckIo == "")
                    {
                        if ((LeaveCheckIo == null || LeaveCheckIo == ""))
                        {
                            if (FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无中端检测IO 无后端检测IO :状态 {LineSegState}--> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo))
                        {
                            if (FrontLineState == LineSegementState.Outing)
                            {
                               Info(sb,$"{LineName} ,{feedMode.ToString()}, 无中端检测IO 有后端检测IO  :状态 {LineSegState}--> { LineSegementState.Entrying}");
                                LineSegState = LineSegementState.Entrying;
                            }
                        }
                    }
                    else if (LeaveCheckIo == InPosCheckIo && IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                    {
                       Info(sb,$"{LineName} ,{feedMode.ToString()}, 有中端检测IO 有后端检测IO   :状态 {LineSegState}--> { LineSegementState.Entrying}");
                        LineSegState = LineSegementState.Entrying;
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(LeaveCheckIo) && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                    {
                       Info(sb,$"{LineName} ,{feedMode.ToString()}, 有中端检测IO 有后端检测IO  :状态 {LineSegState}--> { LineSegementState.Entrying}");
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
                             Info(sb,$"{LineName}:{feedMode.ToString()}, 进料延时到达，流水线状态{LineSegState}-->{LineSegementState.ReadyIng}");
                            MotionStop();
                            EnterDelayTimer.Stop();
                            EnterTimer.Stop();
                            LineSegState = LineSegementState.ReadyIng;
                        }
                        else if (EnterDelayTimer.IsRuning)
                        {
                            // 5
                            Info(sb,$"{LineName}:{feedMode.ToString()}, 进料延时中，流水线状态：{LineSegState}");
                            MotionForwardRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                            && !BackEnterDelayTimer.IsRuning)
                        {
                           Info(sb,$"{LineName}:{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 退料定时器开启 反向进料");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                            && (BackEnterDelayTimer.IsRuning))
                        {
                           Info(sb,$"{LineName}:{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 退料定时器开启 反向进料");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else if ((InPosCheckIo == null || InPosCheckIo == "")
                           && BackEnterDelayTimer.IsTimerOver)
                        {
                           Info(sb,$"{LineName}:{feedMode.ToString()}, 流水线状态{LineSegState} 无中段检测信号 进料定时器到达 退料结束  正向进料");
                            MotionStop();
                            StopClinderUp(true);
                            if (!CheckStopCliyderStateInPos(true))
                                return;
                            MotionForwardRun();
                            EnterDelayTimer.ResetStartTimer();
                            Info(sb, $"{LineName}:{feedMode.ToString()}, 流水线状态{LineSegState}->{LineSegementState.BackEntryReady} 无中段检测信号 进料定时器到达 退料结束  正向进料");
                            LineSegState = LineSegementState.BackEntryReady;

                        }
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            BackEnterDelayTimer.IsRuning && IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            // 3
                           Info(sb,$"{LineName}: {feedMode.ToString()}, 流水线状态{LineSegState} 有中段检测信号 并检测到 退料定时中， 继续退料");
                            MotionBackRun();
                        }
                        else if (!(InPosCheckIo == null || InPosCheckIo == "") &&
                            BackEnterDelayTimer.IsRuning && !IOMgr.GetInstace().ReadIoInBit(InPosCheckIo))
                        {
                            // 4
                           Info(sb,$"{LineName}:{feedMode.ToString()},  流水线状态{LineSegState} 无中段检测信号 并检测到下降沿 进料定时器开启  继续进料");
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
                           Info(sb,$"{LineName}: {feedMode.ToString()}, 流水线状态{LineSegState} 有中段检测信号 并检测到 退料定时器开启");
                            if (!BackEnterDelayTimer.IsRuning)
                                BackEnterDelayTimer.ResetStartTimer();
                            MotionBackRun();
                        }
                        else
                        {
                            // 1
                           Info(sb,$"{LineName}: ,{feedMode.ToString()}, 流水线状态{LineSegState} 中段检测信号无");
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
                            Info(sb, $"{LineName}:{feedMode.ToString()}, 进料延时到达，流水线状态{LineSegState}-->{LineSegementState.ReadyIng}");
                            MotionStop();
                            EnterDelayTimer.Stop();
                            EnterTimer.Stop();
                            LineSegState = LineSegementState.ReadyIng;
                        }
                        else if (EnterDelayTimer.IsRuning)
                        {
                            // 5
                            Info(sb, $"{LineName}:{feedMode.ToString()}, 进料延时中，流水线状态：{LineSegState}");
                            MotionForwardRun();
                        }
                       
                    }

                    break;
                case LineSegementState.ReadyIng:
                     OperateLineReadyIng(bmaual);
                    if ( IsbeReady(bmaual))
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
            return CheckJackUpCliyderStateInPos(false);
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
            return true;
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

        public ILineOprater lineOprater = new LineOpreateWithMotor();
        public override bool IsOKAwaysOnLineRun(bool bmanual)
        {
            return true;
        }
        public override void OpearteLineBeforeEntry(bool bmanual)
        {

            JackUpCliyderUp(false);
            MotionMgr.GetInstace().AbsMove(nAxisNo, dFeedPos, (double)SpeedType.High);

        }
        public override bool IsCanEntry(bool bmanual)
        {
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) > AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{LineName}  在进料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            bool bIsAxisInPosOnFeedPos = Math.Abs(MotionMgr.GetInstace().GetAxisActPos(nAxisNo) - dFeedPos) < 0.3;
            return CheckJackUpCliyderStateInPos(false) && bIsAxisStop;

        }
        public override void OperateLineReadyIng(bool bmanual)
        {
            JackUpCliyderUp(true);
        }
        public override bool IsbeReady(bool bmanual)
        {
            return CheckJackUpCliyderStateInPos(false);
        }
        public override bool IsCanLeave(bool bmanual)
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
        public override void OprateOutFinishDeal(bool bmaual)
        {
            MotionMgr.GetInstace().AbsMove(nAxisNo, dFeedPos, (double)SpeedType.High);
        }
        public override bool IsOutFinishDealOK(bool bmaual)
        {
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) > AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{LineName}  在出料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmaual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            double pos = MotionMgr.GetInstace().GetAxisActPos(nAxisNo);
            bool bIsAxisInPosOnFeedPos = Math.Abs(MotionMgr.GetInstace().GetAxisActPos(nAxisNo) - dFeedPos) < 0.3;
            return bIsAxisStop & bIsAxisInPosOnFeedPos;
        }
    }

    public class LineOpreateWithMotor : ILineOprater
    {

        public LineSegmentDataBase lb
        {
            set;
            get;

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


        public  bool IsOKAwaysOnLineRun(bool bmanual)
        {
            return true;
        }
        public   void OpearteLineBeforeEntry(bool bmanual)
        {

            lb.JackUpCliyderUp(false);
            MotionMgr.GetInstace().AbsMove(nAxisNo, dFeedPos, (double)SpeedType.High);

        }
        public  bool IsCanEntry(bool bmanual)
        {
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) > AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{lb.LineName}  在进料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            bool bIsAxisInPosOnFeedPos = Math.Abs(MotionMgr.GetInstace().GetAxisActPos(nAxisNo) - dFeedPos) < 0.3;
            return lb.CheckJackUpCliyderStateInPos(false) && bIsAxisStop;

        }
        public  void OperateLineReadyIng(bool bmanual)
        {
            lb.JackUpCliyderUp(true);
        }
        public  bool IsbeReady(bool bmanual)
        {
            return lb.CheckJackUpCliyderStateInPos(false);
        }
        public  bool IsCanLeave(bool bmanual)
        {
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) > AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{lb.LineName}  在出料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            return lb.CheckJackUpCliyderStateInPos(false) && bIsAxisStop;
        }
        public  void OperateLineBeforeLevave(bool bmanual)
        {
            lb.JackUpCliyderUp(false);
            MotionMgr.GetInstace().AbsMove(nAxisNo, dDischargePos, (double)SpeedType.High);
        }
        public  void OprateOutFinishDeal(bool bmaual)
        {
            MotionMgr.GetInstace().AbsMove(nAxisNo,dFeedPos, (double)SpeedType.High);
        }
        public  bool IsOutFinishDealOK(bool bmaual)
        {
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) > AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{lb.LineName}  在出料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmaual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            double pos = MotionMgr.GetInstace().GetAxisActPos(nAxisNo);
            bool bIsAxisInPosOnFeedPos = Math.Abs(MotionMgr.GetInstace().GetAxisActPos(nAxisNo) - dFeedPos) < 0.3;
            return bIsAxisStop & bIsAxisInPosOnFeedPos;
        }
        public bool IsCanPut(bool bmanual)
        {
            return true;
        }
    }

   

    public class LineNg : LineSegmentAction
    {
        public LineNg(string name):base(name)
        {
            IsLastSegLine = true;
            nEnteryTimeout = 10000;
            nEnteryDelay = 800;
            nOutTimeout = 200;
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
        public override bool IsOKAwaysOnLineRun(bool bmanual)
        {
            return (FullMaterialCheckIO!= null&& FullMaterialCheckIO != "" )&& !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO);
        
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
            return  !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO);
        }
        public override bool IsCanPut( bool bmanual)
        {
            return (FullMaterialCheckIO != null && FullMaterialCheckIO != "") && !IOMgr.GetInstace().ReadIoInBit(FullMaterialCheckIO) && IOMgr.GetInstace().ReadIoInBit(EnteryCheckIo);
        }


    }


}
