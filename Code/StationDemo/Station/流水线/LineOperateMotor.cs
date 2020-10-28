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

        //public ILineOprater lineOprater = new LineOpreateWithMotor();
        public override bool IsOKAwaysOnLineRun(bool bmanual)
        {
            return true;
        }
        public override void OpearteLineBeforeEntry(bool bmanual)
        {

            JackUpCliyderUp(false);
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop)
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
            return CheckJackUpCliyderStateInPos(false) && bIsAxisStop && bIsAxisInPosOnFeedPos;

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
            bool bIsAxisInPosOnOutPos = Math.Abs(MotionMgr.GetInstace().GetAxisActPos(nAxisNo) -dDischargePos) < 0.3;

            return CheckJackUpCliyderStateInPos(false) && bIsAxisStop && bIsAxisInPosOnOutPos;
        }
        public override void OperateLineBeforeLevave(bool bmanual)
        {
            JackUpCliyderUp(false);
            if (MotionMgr.GetInstace().IsAxisNormalStop(nAxisNo) == AxisState.NormalStop)
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

}
