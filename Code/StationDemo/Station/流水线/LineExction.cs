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
using System.Threading;
using System.Collections.Concurrent;

namespace StationDemo
{

    public class LineException
    {
        public LineSegmentAction lineObj = null;
        public LineException(LineSegmentAction lineSegmentAction)
        {
            lineObj = null;
        }
        public string strAlarmmsg = "";

        public virtual void AlarmDeal()
        {

        }
    }
    public class LineExceptionEntreyCheck : LineException
    {
        public LineExceptionEntreyCheck(LineSegmentAction lineSegmentAction) : base(lineSegmentAction)
        {

        }
        public override void AlarmDeal()
        {
            WaranResult waranResult;
            string alarmmsg = "";
            if (lineObj != null)
            {
                alarmmsg = $"{lineObj.LineName},流水线状态{lineObj.LineSegState} 进入检查失败,请检查该流水线的顶升气缸 阻挡气缸的感应器";
            }
            waranResult = AlarmMgr.GetIntance().WarnWithDlg(strAlarmmsg, null, new string[] { "重试"});

        }
    }
    public class LineExceptionReadingCheck : LineException
    {
        public LineExceptionReadingCheck(LineSegmentAction lineSegmentAction) : base(lineSegmentAction)
        {

        }
        public override void AlarmDeal()
        {
            WaranResult waranResult;
            string alarmmsg = "";
            if (lineObj != null)
            {
                alarmmsg = $"{lineObj.LineName},流水线状态{lineObj.LineSegState},准备检查失败,请检查该流水线的顶升气缸 阻挡气缸的感应器等";
            }
            waranResult = AlarmMgr.GetIntance().WarnWithDlg(strAlarmmsg, null, new string[] { "重试" });

        }
    }
    public class LineExceptionLeaveingCheck : LineException
    {
        public LineExceptionLeaveingCheck(LineSegmentAction lineSegmentAction) : base(lineSegmentAction)
        {

        }
        public override void AlarmDeal()
        {
            WaranResult waranResult;
            string alarmmsg = "";
            if (lineObj != null)
            {
                alarmmsg = $"{lineObj.LineName},流水线状态{lineObj.LineSegState},离开检查失败,请检查该流水线的顶升气缸 阻挡气缸的感应器等";
            }
            waranResult = AlarmMgr.GetIntance().WarnWithDlg(strAlarmmsg, null, new string[] { "重试" });

        }
    } 

    public class LineExceptionEntryTimeOut:LineException
    {
        public LineExceptionEntryTimeOut(LineSegmentAction lineSegmentAction) : base(lineSegmentAction)
        {

        }

        public override void AlarmDeal()
        {
            WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg(strAlarmmsg, null, new string[] { "重试", "已经人工拿走", "进料完成" });
            if (waranResult == WaranResult.Custom1)
            {
                lineObj.EnterTimer.ResetStartTimer();
                lineObj.EnterDelayTimer.Stop();
            }
            if (waranResult == WaranResult.Custom2)
                lineObj.LineSegState = LineSegementState.None;
            if (waranResult == WaranResult.Custom3)
                lineObj.LineSegState = LineSegementState.ReadyIng;
        }
    }

    public class LineExceptionOutTimeout : LineException
    {
        public LineExceptionOutTimeout(LineSegmentAction lineSegmentAction) : base(lineSegmentAction)
        {

        }

        public override void AlarmDeal()
        {
            WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg(strAlarmmsg, null, new string[] { "重试", "已经人工拿走" });
            if (waranResult == WaranResult.Custom1)
            {
                lineObj.OutTimer.ResetStartTimer();
            }
            if (waranResult == WaranResult.Custom2)
            {
                lineObj.LineSegState = LineSegementState.None;
               
            }
            //waranResult = AlarmMgr.GetIntance().WarnWithDlg($"{LineName} :离开段超时,可能料被拿走 或者卡住", null, new string[] { "重试", "已经人工拿走" }, CommonDlg.DlgWaranType.Waran_Custom1, null, bmaual);
                            //if (waranResult == WaranResult.Custom1)
                            //    OutTimer.ResetStartTimer();
                            //if (waranResult == WaranResult.Custom2)
                            //    LineSegState= LineSegementState.None;
        }
    }

    public class LineExceptionLeavingTimeout : LineException
    {
        public LineExceptionLeavingTimeout(LineSegmentAction lineSegmentAction) : base(lineSegmentAction)
        {

        }

        public override void AlarmDeal()
        {
            WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg(strAlarmmsg, lineObj.sb, new string[] { "重试", "已经人工拿走" });
            if (waranResult == WaranResult.Custom1)
            {
                lineObj.LeaveTimer.ResetStartTimer();
                lineObj.LeaveDelayTimer.Stop();
            }
            if (waranResult == WaranResult.Custom2)
            {
                lineObj.LineSegState = LineSegementState.None;
            }
        }
    }


}