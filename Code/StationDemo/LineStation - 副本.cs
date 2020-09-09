using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using CameraLib;
//using HalconLib;
using MotionIoLib;
using System.IO;
using CommonTools;
using UserData;
namespace StationDemo
{
    public interface IterfanceLineOperate
    {
        
        void OpearteLineBeforeEntry(bool bmanual);
        bool CheckLineBeforeEntry(bool bmanual);
        void OperateLineBeforeHave(bool bmanual);
        void OperateLineBeforeLevave(bool bmanual);

        bool CheckBeforeHave(bool bmanual);

        bool CheckBeforeLeave(bool bmanual);
        
    }
    

    public class LineSegmentAction : IterfanceLineOperate
    {

        public LineSegmentAction(string Name)
        {
            lineSegmentDataBase.LineName = Name;
        }
        LineSegement3Singl lineSegmentDataBase = new LineSegement3Singl();

        public LineSegementState GetState()
        {
            return lineSegmentDataBase.LineSegState;

        }
        public void ForwardRun(Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            WaranResult waranResult;
            switch (lineSegmentDataBase.LineSegState)
            {
                case LineSegementState.None:
                    
                    OpearteLineBeforeEntry(bmaual);
                    if (!CheckLineBeforeEntry(bmaual))
                        return;
                    sb?.Info($"{lineSegmentDataBase.LineName} :状态{lineSegmentDataBase.LineSegState}");
                    lineSegmentDataBase.MotionStop();
                    if ((lineSegmentDataBase.EnteryCheckIo == null || lineSegmentDataBase.EnteryCheckIo == ""))
                    {
                        if ((lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == ""))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无前端检测IO 无中端检测IO :状态{lineSegmentDataBase.LineSegState} --> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                        else if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无前端检测IO 有中端检测IO:状态{lineSegmentDataBase.LineSegState} --> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == "")
                    {
                        if ((lineSegmentDataBase.EnteryCheckIo == null || lineSegmentDataBase.EnteryCheckIo == ""))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无中端检测IO 有前端检测IO :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.EnteryCheckIo))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无中端检测IO 有前端检测IO  :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.EnteryCheckIo) && !IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                    {
                        sb?.Info($"后进料：{lineSegmentDataBase.LineName} 有中端检测IO 有前端检测IO  :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                        lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                    {
                        return;
                    }
                  
                    break;
                case LineSegementState.Entrying:
                    lineSegmentDataBase.StopClinderUp(true);
                    if (!lineSegmentDataBase.CheckStopCliyderStateInPos(true))
                        return;
                    if (!lineSegmentDataBase.EnterTimer.IsRuning)
                        lineSegmentDataBase.EnterTimer.ResetStartTimer();

                    if (lineSegmentDataBase.EnterTimer.IsTimerOver)
                    {
                        lineSegmentDataBase.MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :进入段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                        {
                            lineSegmentDataBase.EnterTimer.ResetStartTimer();
                            lineSegmentDataBase.EnterDelayTimer.Stop();
                        }

                    }
                    else
                    {
                        //进料 到位延时
                        if((lineSegmentDataBase.InPosCheckIo ==null || lineSegmentDataBase.InPosCheckIo=="") && !lineSegmentDataBase.EnterDelayTimer.IsRuning)
                        {
                            lineSegmentDataBase.MotionForwardRun();
                            lineSegmentDataBase.EnterDelayTimer.resumeTimer();
                            sb?.Info($"{lineSegmentDataBase.LineName} 无阻挡到位IO, :流水线将运动{lineSegmentDataBase.EnterDelayTimer.SetedTime} ms  ");
                        }
                        else if((lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == "") && lineSegmentDataBase.EnterDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.EnterTimer.Stop();
                            lineSegmentDataBase.EnterDelayTimer.Stop();
                            lineSegmentDataBase.MotionStop();
                            sb?.Info($"{lineSegmentDataBase.LineName} 无阻挡到位IO, :流水线运动{lineSegmentDataBase.EnterDelayTimer.SetedTime} ms后 状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Have}");
                            lineSegmentDataBase.LineSegState = LineSegementState.BeforeHave;
                        }
                       else if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                            lineSegmentDataBase.MotionForwardRun();
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo) && !lineSegmentDataBase.EnterDelayTimer.IsRuning)
                        {
                            lineSegmentDataBase.EnterDelayTimer.ResetStartTimer();
                            lineSegmentDataBase.MotionForwardRun();
                            sb?.Info($"{lineSegmentDataBase.LineName} 有阻挡到位IO, :流水线将运动{lineSegmentDataBase.EnterDelayTimer.SetedTime} ms ");

                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo) && lineSegmentDataBase.EnterDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.EnterTimer.Stop();
                            lineSegmentDataBase.EnterDelayTimer.Stop();
                            lineSegmentDataBase.MotionStop();
                            sb?.Info($"{lineSegmentDataBase.LineName} 有阻挡到位IO  流水线运动{lineSegmentDataBase.EnterDelayTimer.SetedTime} ms后停止 状态： {lineSegmentDataBase.LineSegState}--> { LineSegementState.Have}");
                            lineSegmentDataBase.LineSegState = LineSegementState.BeforeHave;
                        }
                    }
                    break;
                case LineSegementState.BeforeHave:
                    OperateLineBeforeHave(bmaual);
                    if (CheckBeforeHave(bmaual))
                    {
                        sb?.Info($"{lineSegmentDataBase.LineName}  :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Have}");
                        lineSegmentDataBase.LineSegState = LineSegementState.Have;
                    }
                    else
                        return;
                    break;
                case LineSegementState.Have:
            
                    sb?.Info($"{lineSegmentDataBase.LineName} :状态{lineSegmentDataBase.LineSegState}");
                
                    break;
                case LineSegementState.Finish:
                    OperateLineBeforeLevave(bmaual);
                    if (CheckBeforeLeave(bmaual))
                        return;
                    //物料取料完成  加工完成  
                    if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                    {
                        if (backLineState == LineSegementState.None)
                            lineSegmentDataBase.LineSegState = LineSegementState.Leaveing;
                        sb?.Info($"{lineSegmentDataBase.LineName} 无后端检测IO  状态： {lineSegmentDataBase.LineSegState}--> { LineSegementState.Leaveing}");

                    }
                    else if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo))
                    {
                        sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO    没有信号 状态： {lineSegmentDataBase.LineSegState}--> { LineSegementState.Leaveing}");
                        lineSegmentDataBase.LineSegState = LineSegementState.Leaveing;
                    }
                    else
                    {
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :后端检测信号有, 物料可能卡住，或者有物料放置，请拿走重试", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                            return;
                    }
                        
                    break;
                case LineSegementState.Leaveing:
                    lineSegmentDataBase.StopClinderUp(false);
                    if (!lineSegmentDataBase.CheckStopCliyderStateInPos(false))
                        return;
                    if (!lineSegmentDataBase.LeaveTimer.IsRuning)
                        lineSegmentDataBase.LeaveTimer.ResetStartTimer();
                    if (lineSegmentDataBase.LeaveTimer.IsTimerOver)
                    {
                        lineSegmentDataBase.MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                        {
                            lineSegmentDataBase.LeaveTimer.ResetStartTimer();
                            lineSegmentDataBase.LeaveDelayTimer.Stop();
                        }
                           
                    }
                    else
                    {
                        if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                        {
                            lineSegmentDataBase.MotionStop();
                            lineSegmentDataBase.LeaveDelayTimer.Stop();
                            lineSegmentDataBase.LeaveTimer.Stop();
                            sb?.Info($"{lineSegmentDataBase.LineName} 无后端检测IO :状态 {lineSegmentDataBase.LineSegState}-->{LineSegementState.WaitOut}");
                            lineSegmentDataBase.LineSegState = LineSegementState.WaitOut;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo) && !lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                        {
                            if (!lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                                lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                            lineSegmentDataBase.MotionForwardRun();
                            sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO 离开延时定时器开启 : 状态 {lineSegmentDataBase.LineSegState}");

                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo) && lineSegmentDataBase.LeaveDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.LeaveDelayTimer.Stop();
                            lineSegmentDataBase.LeaveTimer.Stop();
                            lineSegmentDataBase.MotionStop();
                            lineSegmentDataBase.LineSegState = LineSegementState.WaitOut;
                            sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO 离开延时定时器开启 : 状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.WaitOut}");
                        }
                        else
                        {
                            lineSegmentDataBase.MotionForwardRun();
                        }
                    }
                    break;
                case LineSegementState.WaitOut:
                    if (backLineState == LineSegementState.None && !lineSegmentDataBase.LeaveDelayTimer.IsRuning )
                    {
                        //后一段流水线没有物料 开始进料
                        if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                        {
                            //  没有出料感器
                            lineSegmentDataBase.StopClinderUp(false);
                            if (!lineSegmentDataBase.CheckStopCliyderStateInPos(false))
                                return;
                        }
                        else
                        {
                            //  有出料感器 暂停
                            lineSegmentDataBase.StopClinderUp(true);
                            if (!lineSegmentDataBase.CheckStopCliyderStateInPos(true))
                                return;
                        }
                        if (!lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                            lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                        lineSegmentDataBase.MotionForwardRun();
                    }

                    if (lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                    {
                        lineSegmentDataBase.MotionForwardRun();
                        if (lineSegmentDataBase.LeaveDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.MotionStop();
                            waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                            if (waranResult == WaranResult.Custom1)
                                lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                        }
                        else
                        {
                            if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                            {
                                //  没有出料感器 下端进料完成
                                if (backLineState == LineSegementState.Have)
                                {
                                    sb?.Info($"{lineSegmentDataBase.LeaveCheckIo}： 状态：{lineSegmentDataBase.LineSegState}-->{ LineSegementState.None}");
                                    lineSegmentDataBase.LineSegState = LineSegementState.None;
                                    lineSegmentDataBase.MotionStop();
                                    return;
                                }
                            }
                            else
                            {
                                //  有出料感器 暂停
                                if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo))
                                {
                                    sb?.Info($"{lineSegmentDataBase.LeaveCheckIo}：信号消失 状态：{lineSegmentDataBase.LineSegState}-->{ LineSegementState.None}");
                                    lineSegmentDataBase.LineSegState = LineSegementState.None;
                                    lineSegmentDataBase.MotionStop();
                                    return;
                                }
                            }
                            if (lineSegmentDataBase.bOutMotorRunDir)
                                lineSegmentDataBase.MotionForwardRun();
                            else
                                lineSegmentDataBase.MotionBackRun();
                        }
                    }
                    break;
            }
        }
        public void BackRun(Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            WaranResult waranResult;
            switch (lineSegmentDataBase.LineSegState)
            {
                case LineSegementState.None:
                    OpearteLineBeforeEntry(bmaual);
                    if (!CheckLineBeforeEntry(bmaual))
                        return;
                    sb?.Info($"{lineSegmentDataBase.LineName} :状态{lineSegmentDataBase.LineSegState}");
                    lineSegmentDataBase.MotionStop();
                  
                    if ((lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")  )    
                    {
                        if ((lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == ""))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无后端检测IO 无中端检测IO :状态{lineSegmentDataBase.LineSegState} --> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                        else if(!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无后端检测IO 有中端检测IO:状态{lineSegmentDataBase.LineSegState} --> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == "")
                    {
                        if ((lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == ""))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无中端检测IO 无后端检测IO :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo) )
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无中端检测IO 有后端检测IO  :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if(IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo)  && !IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                    {
                        sb?.Info($"后进料：{lineSegmentDataBase.LineName} 有中端检测IO 有后端检测IO  :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                        lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                    {
                        return;
                    }
                    break;
                case LineSegementState.Entrying:
             
                    if (!lineSegmentDataBase.EnterTimer.IsRuning)
                         lineSegmentDataBase.EnterTimer.ResetStartTimer();
                    if (lineSegmentDataBase.EnterTimer.IsTimerOver)
                    {
                        lineSegmentDataBase.MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :进入段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                        {
                            lineSegmentDataBase.EnterTimer.ResetStartTimer();
                            lineSegmentDataBase.EnterDelayTimer.Stop();
                        }
                              
                    }
                    else
                    {
                        if (lineSegmentDataBase.EnterDelayTimer.IsTimerOver)
                        {
                            // 6
                            sb?.Info($"{lineSegmentDataBase.LineName}:进料延时到达，流水线状态{lineSegmentDataBase.LineSegState}-->{LineSegementState.BeforeHave}");
                            lineSegmentDataBase.MotionStop();
                            lineSegmentDataBase.LineSegState = LineSegementState.BeforeHave;
                        }
                        else if (lineSegmentDataBase.EnterDelayTimer.IsRuning)
                        {
                            // 5
                            sb?.Info($"{lineSegmentDataBase.LineName}:进料延时中，流水线状态：{lineSegmentDataBase.LineSegState}");
                            lineSegmentDataBase.MotionForwardRun();
                        }
                        else if(lineSegmentDataBase.BackEnterDelayTimer.IsRuning &&  IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                        {
                            // 3
                            sb?.Info($"{lineSegmentDataBase.LineName}: 流水线状态{lineSegmentDataBase.LineSegState} 中段检测信号有 退料定时中， 继续退料");
                            lineSegmentDataBase.MotionBackRun();
                        }

                        else if(lineSegmentDataBase.BackEnterDelayTimer.IsRuning && !IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                        {
                            // 4
                            sb?.Info($"{lineSegmentDataBase.LineName}: 流水线状态{lineSegmentDataBase.LineSegState} 中段检测信号无 进料定时器开启  继续进料");
                            lineSegmentDataBase.MotionStop();
                            lineSegmentDataBase.StopClinderUp(true);
                            if (!lineSegmentDataBase.CheckStopCliyderStateInPos(true))
                                return;
                            lineSegmentDataBase.MotionForwardRun();
                            lineSegmentDataBase.EnterDelayTimer.ResetStartTimer();

                        }
                        else if(IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                        {
                            // 2
                            sb?.Info($"{lineSegmentDataBase.LineName}: 流水线状态{lineSegmentDataBase.LineSegState} 中段检测信号有 退料定时器开启");
                            if (!lineSegmentDataBase.BackEnterDelayTimer.IsRuning)
                                lineSegmentDataBase.BackEnterDelayTimer.ResetStartTimer();
                            lineSegmentDataBase.MotionBackRun();
                        }
                        else
                        {
                            // 1
                            sb?.Info($"{lineSegmentDataBase.LineName}: ，流水线状态{lineSegmentDataBase.LineSegState} 中段检测信号无");
                            lineSegmentDataBase.MotionBackRun();
                            lineSegmentDataBase.BackEnterDelayTimer.Stop();
                        }
                    }
                    break;
                case LineSegementState.BeforeHave:
                    OperateLineBeforeHave(bmaual);
                    if (CheckBeforeHave(bmaual))
                    {
                        lineSegmentDataBase.LineSegState = LineSegementState.Have;
                    }
                    else
                        return;
                    break;
                case LineSegementState.Have:

                    sb?.Info($"{lineSegmentDataBase.LineName} :状态{lineSegmentDataBase.LineSegState}");

                    break;
                case LineSegementState.Finish:
                    OperateLineBeforeLevave(bmaual);
                    if (CheckBeforeLeave(bmaual))
                        return;
                    //物料取料完成  加工完成  
                    if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                    {
                        if (backLineState == LineSegementState.None)
                            lineSegmentDataBase.LineSegState = LineSegementState.Leaveing;
                        sb?.Info($"{lineSegmentDataBase.LineName} 无后端检测IO  状态： {lineSegmentDataBase.LineSegState}--> { LineSegementState.Leaveing}");

                    }
                    else if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo))
                    {
                        sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO    没有信号 状态： {lineSegmentDataBase.LineSegState}--> { LineSegementState.Leaveing}");
                        lineSegmentDataBase.LineSegState = LineSegementState.Leaveing;
                    }
                    else
                    {
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :后端检测信号有, 物料可能卡住，或者有物料放置，请拿走重试", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                            return;
                    }

                    break;
                case LineSegementState.Leaveing:
                    lineSegmentDataBase.StopClinderUp(false);
                    if (!lineSegmentDataBase.CheckStopCliyderStateInPos(false))
                        return;
                    if (!lineSegmentDataBase.LeaveTimer.IsRuning)
                        lineSegmentDataBase.LeaveTimer.ResetStartTimer();
                    if (lineSegmentDataBase.LeaveTimer.IsTimerOver)
                    {
                        lineSegmentDataBase.MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                        {
                            lineSegmentDataBase.LeaveTimer.ResetStartTimer();
                            lineSegmentDataBase.LeaveDelayTimer.Stop();
                        }
                    }
                    else
                    {
                        if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                        {
                            lineSegmentDataBase.MotionStop();
                            lineSegmentDataBase.LeaveDelayTimer.Stop();
                            lineSegmentDataBase.LeaveTimer.Stop();
                            sb?.Info($"{lineSegmentDataBase.LineName} 无后端检测IO :状态 {lineSegmentDataBase.LineSegState}-->{LineSegementState.WaitOut}");
                            lineSegmentDataBase.LineSegState = LineSegementState.WaitOut;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo) && !lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                        {
                            if (!lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                                lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                            lineSegmentDataBase.MotionForwardRun();
                            sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO 离开延时定时器开启 : 状态 {lineSegmentDataBase.LineSegState}");

                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo) && lineSegmentDataBase.LeaveDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.LeaveDelayTimer.Stop();
                            lineSegmentDataBase.LeaveTimer.Stop();
                            lineSegmentDataBase.MotionStop();
                            lineSegmentDataBase.LineSegState = LineSegementState.WaitOut;
                            sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO 离开延时定时器开启 : 状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.WaitOut}");
                        }
                        else
                        {
                            lineSegmentDataBase.MotionForwardRun();
                        }
                    }
                    break;
                case LineSegementState.WaitOut:
                    if (backLineState == LineSegementState.None && !lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                    {
                        //后一段流水线没有物料 开始进料
                        if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                        {
                            //  没有出料感器
                            lineSegmentDataBase.StopClinderUp(false);
                            if (!lineSegmentDataBase.CheckStopCliyderStateInPos(false))
                                return;
                        }
                        else
                        {
                            //  有出料感器 暂停
                            lineSegmentDataBase.StopClinderUp(true);
                            if (!lineSegmentDataBase.CheckStopCliyderStateInPos(true))
                                return;
                        }
                        if (!lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                            lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                        lineSegmentDataBase.MotionForwardRun();
                    }

                    if (lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                    {
                        lineSegmentDataBase.MotionForwardRun();
                        if (lineSegmentDataBase.LeaveDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.MotionStop();
                            waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                            if (waranResult == WaranResult.Custom1)
                                lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                        }
                        else
                        {
                            if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                            {
                                //  没有出料感器 下端进料完成
                                if (backLineState == LineSegementState.Have)
                                {
                                    sb?.Info($"{lineSegmentDataBase.LeaveCheckIo}： 状态：{lineSegmentDataBase.LineSegState}-->{ LineSegementState.None}");
                                    lineSegmentDataBase.LineSegState = LineSegementState.None;
                                    lineSegmentDataBase.MotionStop();
                                    return;
                                }
                            }
                            else
                            {
                                //  有出料感器 暂停
                                if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo))
                                {
                                    sb?.Info($"{lineSegmentDataBase.LeaveCheckIo}：信号消失 状态：{lineSegmentDataBase.LineSegState}-->{ LineSegementState.None}");
                                    lineSegmentDataBase.LineSegState = LineSegementState.None;
                                    lineSegmentDataBase.MotionStop();
                                    return;
                                }
                            }
                            if (lineSegmentDataBase.bOutMotorRunDir)
                                lineSegmentDataBase.MotionForwardRun();
                            else
                                lineSegmentDataBase.MotionBackRun();
                        }
                    }
                    break;
            }
        }

        public void OpearteLineBeforeEntry(bool bmanual)
        {
            lineSegmentDataBase.StopClinderUp(false);
            lineSegmentDataBase.JackUpCliyderUp(false);

        }
        public bool CheckLineBeforeEntry(bool bmanual)
        {
            return  lineSegmentDataBase.CheckJackUpCliyderStateInPos(false) &&
                lineSegmentDataBase.CheckStopCliyderStateInPos(false);
        }
        public void OperateLineBeforeHave(bool bmanual)
        {
            lineSegmentDataBase.JackUpCliyderUp(true);
        }
        public bool CheckBeforeHave(bool bmanual)
        {
            return lineSegmentDataBase.CheckJackUpCliyderStateInPos(false);
        }
        public bool CheckBeforeLeave(bool bmanual)
        {
            return lineSegmentDataBase.CheckJackUpCliyderStateInPos(false) &&
                lineSegmentDataBase.CheckStopCliyderStateInPos(false);
        }
        public void OperateLineBeforeLevave(bool bmanual)
        {
            lineSegmentDataBase.JackUpCliyderUp(false);
        }
    }


    public class LineSegmentUpDownUseMotor : IterfanceLineOperate
    {

        public LineSegmentUpDownUseMotor(string Name)
        {
            lineSegmentDataBase.LineName = Name;
        }
        LineSegement3Singl lineSegmentDataBase = new LineSegement3Singl();
        public int nAxisNo = -1;
        /// <summary>
        /// 进料位置（高度）
        /// </summary>
        
        public double dFeedPos = 0;
        /// <summary>
        /// 出料高度
        /// </summary>
        public double dDischargePos = 0;

        public LineSegementState GetState()
        {
            return lineSegmentDataBase.LineSegState;

        }
        public void ForwardRun(Stationbase sb, LineSegementState backLineState, bool bmaual)
        {
            WaranResult waranResult;
            switch (lineSegmentDataBase.LineSegState)
            {
                case LineSegementState.None:

                    OpearteLineBeforeEntry(bmaual);
                    if (!CheckLineBeforeEntry(bmaual))
                        return;
                    sb?.Info($"{lineSegmentDataBase.LineName} :状态{lineSegmentDataBase.LineSegState}");
                    lineSegmentDataBase.MotionStop();
                    if ((lineSegmentDataBase.EnteryCheckIo == null || lineSegmentDataBase.EnteryCheckIo == ""))
                    {
                        if ((lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == ""))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无前端检测IO 无中端检测IO :状态{lineSegmentDataBase.LineSegState} --> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                        else if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无前端检测IO 有中端检测IO:状态{lineSegmentDataBase.LineSegState} --> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == "")
                    {
                        if ((lineSegmentDataBase.EnteryCheckIo == null || lineSegmentDataBase.EnteryCheckIo == ""))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无中端检测IO 有前端检测IO :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.EnteryCheckIo))
                        {
                            sb?.Info($"后进料：{lineSegmentDataBase.LineName} 无中端检测IO 有前端检测IO  :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                            lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                        }
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.EnteryCheckIo) && !IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                    {
                        sb?.Info($"后进料：{lineSegmentDataBase.LineName} 有中端检测IO 有前端检测IO  :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Entrying}");
                        lineSegmentDataBase.LineSegState = LineSegementState.Entrying;
                    }
                    else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                    {
                        return;
                    }

                    break;
                case LineSegementState.Entrying:
                    lineSegmentDataBase.StopClinderUp(true);
                    if (!lineSegmentDataBase.CheckStopCliyderStateInPos(true))
                        return;
                    if (!lineSegmentDataBase.EnterTimer.IsRuning)
                        lineSegmentDataBase.EnterTimer.ResetStartTimer();

                    if (lineSegmentDataBase.EnterTimer.IsTimerOver)
                    {
                        lineSegmentDataBase.MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :进入段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                            lineSegmentDataBase.EnterTimer.ResetStartTimer();
                    }
                    else
                    {
                        //进料 到位延时
                        if ((lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == "") && !lineSegmentDataBase.EnterDelayTimer.IsRuning)
                        {
                            lineSegmentDataBase.MotionForwardRun();
                            lineSegmentDataBase.EnterDelayTimer.resumeTimer();
                            sb?.Info($"{lineSegmentDataBase.LineName} 无阻挡到位IO, :流水线将运动{lineSegmentDataBase.EnterDelayTimer.SetedTime} ms  ");
                        }
                        else if ((lineSegmentDataBase.InPosCheckIo == null || lineSegmentDataBase.InPosCheckIo == "") && lineSegmentDataBase.EnterDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.EnterTimer.Stop();
                            lineSegmentDataBase.EnterDelayTimer.Stop();
                            lineSegmentDataBase.MotionStop();
                            sb?.Info($"{lineSegmentDataBase.LineName} 无阻挡到位IO, :流水线运动{lineSegmentDataBase.EnterDelayTimer.SetedTime} ms后 状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Have}");
                            lineSegmentDataBase.LineSegState = LineSegementState.BeforeHave;
                        }
                        else if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo))
                            lineSegmentDataBase.MotionForwardRun();
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo) && !lineSegmentDataBase.EnterDelayTimer.IsRuning)
                        {
                            lineSegmentDataBase.EnterDelayTimer.ResetStartTimer();
                            lineSegmentDataBase.MotionForwardRun();
                            sb?.Info($"{lineSegmentDataBase.LineName} 有阻挡到位IO, :流水线将运动{lineSegmentDataBase.EnterDelayTimer.SetedTime} ms ");

                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.InPosCheckIo) && lineSegmentDataBase.EnterDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.EnterTimer.Stop();
                            lineSegmentDataBase.EnterDelayTimer.Stop();
                            lineSegmentDataBase.MotionStop();
                            sb?.Info($"{lineSegmentDataBase.LineName} 有阻挡到位IO  流水线运动{lineSegmentDataBase.EnterDelayTimer.SetedTime} ms后停止 状态： {lineSegmentDataBase.LineSegState}--> { LineSegementState.Have}");
                            lineSegmentDataBase.LineSegState = LineSegementState.BeforeHave;
                        }
                    }
                    break;
                case LineSegementState.BeforeHave:
                    OperateLineBeforeHave(bmaual);
                    if (CheckBeforeHave(bmaual))
                    {
                        sb?.Info($"{lineSegmentDataBase.LineName}  :状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.Have}");
                        lineSegmentDataBase.LineSegState = LineSegementState.Have;
                    }
                    else
                        return;
                    break;
                case LineSegementState.Have:

                    sb?.Info($"{lineSegmentDataBase.LineName} :状态{lineSegmentDataBase.LineSegState}");

                    break;
                case LineSegementState.Finish:
                    OperateLineBeforeLevave(bmaual);
                    if (CheckBeforeLeave(bmaual))
                        return;
                    //物料取料完成  加工完成  
                    if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                    {
                        if (backLineState == LineSegementState.None)
                            lineSegmentDataBase.LineSegState = LineSegementState.Leaveing;
                        sb?.Info($"{lineSegmentDataBase.LineName} 无后端检测IO  状态： {lineSegmentDataBase.LineSegState}--> { LineSegementState.Leaveing}");

                    }
                    else if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo))
                    {
                        sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO    没有信号 状态： {lineSegmentDataBase.LineSegState}--> { LineSegementState.Leaveing}");
                        lineSegmentDataBase.LineSegState = LineSegementState.Leaveing;
                    }
                    else
                    {
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :后端检测信号有, 物料可能卡住，或者有物料放置，请拿走重试", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                            return;
                    }

                    break;
                case LineSegementState.Leaveing:
                    lineSegmentDataBase.StopClinderUp(false);
                    if (!lineSegmentDataBase.CheckStopCliyderStateInPos(false))
                        return;
                    if (!lineSegmentDataBase.LeaveTimer.IsRuning)
                        lineSegmentDataBase.LeaveTimer.ResetStartTimer();
                    if (lineSegmentDataBase.LeaveTimer.IsTimerOver)
                    {
                        lineSegmentDataBase.MotionStop();
                        waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                        if (waranResult == WaranResult.Custom1)
                            lineSegmentDataBase.LeaveTimer.ResetStartTimer();
                    }
                    else
                    {
                        if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                        {
                            lineSegmentDataBase.MotionStop();
                            lineSegmentDataBase.LeaveDelayTimer.Stop();
                            lineSegmentDataBase.LeaveTimer.Stop();
                            sb?.Info($"{lineSegmentDataBase.LineName} 无后端检测IO :状态 {lineSegmentDataBase.LineSegState}-->{LineSegementState.WaitOut}");
                            lineSegmentDataBase.LineSegState = LineSegementState.WaitOut;
                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo) && !lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                        {
                            if (!lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                                lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                            lineSegmentDataBase.MotionForwardRun();
                            sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO 离开延时定时器开启 : 状态 {lineSegmentDataBase.LineSegState}");

                        }
                        else if (IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo) && lineSegmentDataBase.LeaveDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.LeaveDelayTimer.Stop();
                            lineSegmentDataBase.LeaveTimer.Stop();
                            lineSegmentDataBase.MotionStop();
                            lineSegmentDataBase.LineSegState = LineSegementState.WaitOut;
                            sb?.Info($"{lineSegmentDataBase.LineName} 有后端检测IO 离开延时定时器开启 : 状态 {lineSegmentDataBase.LineSegState}--> { LineSegementState.WaitOut}");
                        }
                        else
                        {
                            lineSegmentDataBase.MotionForwardRun();
                        }
                    }
                    break;
                case LineSegementState.WaitOut:
                    if (backLineState == LineSegementState.None && !lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                    {
                        //后一段流水线没有物料 开始进料
                        if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                        {
                            //  没有出料感器
                            lineSegmentDataBase.StopClinderUp(false);
                            if (!lineSegmentDataBase.CheckStopCliyderStateInPos(false))
                                return;
                        }
                        else
                        {
                            //  有出料感器 暂停
                            lineSegmentDataBase.StopClinderUp(true);
                            if (!lineSegmentDataBase.CheckStopCliyderStateInPos(true))
                                return;
                        }
                        if (!lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                            lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                        if (lineSegmentDataBase.bOutMotorRunDir)
                            lineSegmentDataBase.MotionForwardRun();
                        else
                            lineSegmentDataBase.MotionBackRun();
                    }

                    if (lineSegmentDataBase.LeaveDelayTimer.IsRuning)
                    {
                        lineSegmentDataBase.MotionForwardRun();
                        if (lineSegmentDataBase.LeaveDelayTimer.IsTimerOver)
                        {
                            lineSegmentDataBase.MotionStop();
                            waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{lineSegmentDataBase.LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                            if (waranResult == WaranResult.Custom1)
                                lineSegmentDataBase.LeaveDelayTimer.ResetStartTimer();
                        }
                        else
                        {
                            if (lineSegmentDataBase.LeaveCheckIo == null || lineSegmentDataBase.LeaveCheckIo == "")
                            {
                                //  没有出料感器 下端进料完成
                                if (backLineState == LineSegementState.Have)
                                {
                                    sb?.Info($"{lineSegmentDataBase.LeaveCheckIo}： 状态：{lineSegmentDataBase.LineSegState}-->{ LineSegementState.None}");
                                    lineSegmentDataBase.LineSegState = LineSegementState.None;
                                    lineSegmentDataBase.MotionStop();
                                    return;
                                }
                            }
                            else
                            {
                                //  有出料感器 暂停
                                if (!IOMgr.GetInstace().ReadIoInBit(lineSegmentDataBase.LeaveCheckIo))
                                {
                                    sb?.Info($"{lineSegmentDataBase.LeaveCheckIo}：信号消失 状态：{lineSegmentDataBase.LineSegState}-->{ LineSegementState.None}");
                                    lineSegmentDataBase.LineSegState = LineSegementState.None;
                                    lineSegmentDataBase.MotionStop();
                                    return;
                                }
                            }
                            if (lineSegmentDataBase.bOutMotorRunDir)
                                lineSegmentDataBase.MotionForwardRun();
                            else
                                lineSegmentDataBase.MotionBackRun();
                        }
                    }
                    break;
            }
        }


        public void OpearteLineBeforeEntry(bool bmanual)
        {
          
            lineSegmentDataBase.JackUpCliyderUp(false);
            MotionMgr.GetInstace().AbsMove(nAxisNo, dFeedPos, (double)SpeedType.High);

        }
        public bool CheckLineBeforeEntry(bool bmanual)
        {
            if(MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo)>AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{lineSegmentDataBase.LineName}  在进料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            return lineSegmentDataBase.CheckJackUpCliyderStateInPos(false) &&bIsAxisStop;
               
        }
        public void OperateLineBeforeHave(bool bmanual)
        {
            lineSegmentDataBase.JackUpCliyderUp(true);
        }
        public bool CheckBeforeHave(bool bmanual)
        {
            return lineSegmentDataBase.CheckJackUpCliyderStateInPos(false);
        }
        public bool CheckBeforeLeave(bool bmanual)
        {
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) > AxisState.NormalStop)
            {
                AlarmMgr.GetIntance().WarnWithDlg($"流水线{lineSegmentDataBase.LineName}  在出料前，轴{MotionMgr.GetInstace().GetAxisName(nAxisNo)} ,报警{MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo).ToString()},程序停止", null, CommonDlg.DlgWaranType.WaranOK, null, bmanual);
                StationMgr.GetInstance().Stop();
            }
            bool bIsAxisStop = MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;
            return lineSegmentDataBase.CheckJackUpCliyderStateInPos(false) && bIsAxisStop;
        }
        public void OperateLineBeforeLevave(bool bmanual)
        {
            lineSegmentDataBase.JackUpCliyderUp(false);
            MotionMgr.GetInstace().AbsMove(nAxisNo, dDischargePos, (double)SpeedType.High);
        }
    }


    public class LineStation : CommonTools.Stationbase
    {

     

        public LineStation(CommonTools.Stationbase pb) : base(pb)
        {
            //m_listIoInput.Add("点胶头下降到位");
            //m_listIoInput.Add("UV光源上升到位");
            //m_listIoOutput.Add("Barrel中心吸真空");
            //m_listIoOutput.Add("Barrel外环破真空");
            //m_listIoOutput.Add("Barrel外环吸真空");
            //m_listIoOutput.Add("Barrel中心破真空");
            //m_listIoOutput.Add("点胶头下降");
            //m_listIoOutput.Add("UV光源下降");
        }
        enum StationStep
        {
            StepInit = 100,
            Stp1,
            Stp2,
            Stp3,
            StepEnd,
        }
        protected override bool InitStation()
        {
            PushMultStep((int)StationStep.StepInit);
            return true;
        }
       
    


        protected override void StationWork(int step)
        {

        

        }

    }

}
  