using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using gts;
using System.Diagnostics;
/*
 * 固高卡号从0 开始
 * 轴号从1开始
 * Home 原点规则 ，不管信号是常开常闭，一律将其配置成
 * 挡片挡住原点时候，固高软件的原点指示灯亮
 * 挡片离开原点， 固高软件的原点指示灯灭
 * 配置完成后 配置文件di原点 
 * 信号是正常 则 m_ProPrietarySignal[轴号].oriSignal = 0;
 * 信号是取反 则 m_ProPrietarySignal[轴号].oriSignal = 1;
 */

namespace MotionIoLib
{

    public class Motion_Gts : MotionCardBase
    {

        public Motion_Gts(ulong indexCard, string strName, int nMinAxisNo, int nMaxAxisNo)
            : base(indexCard, strName, nMinAxisNo, nMaxAxisNo)
        {

        }
        public override bool TranMMToPluse(int nAxisNo, ref double dSpeed, ref double acc, ref double dec)
        {
           
            double speed = dSpeed;
            double Acc = m_MovePrm[nAxisNo - 1].AccH;
            double Dcc = m_MovePrm[nAxisNo - 1].AccL;
            switch (dSpeed)
            {
                case 0:
                    Acc = m_MovePrm[nAxisNo - 1].AccH;
                    Dcc = m_MovePrm[nAxisNo - 1].DccH;
                    speed = m_MovePrm[nAxisNo - 1].VelH;
                    break;
                case 1:
                    Acc = m_MovePrm[nAxisNo - 1].AccM;
                    Dcc = m_MovePrm[nAxisNo - 1].DccM;
                    speed = m_MovePrm[nAxisNo - 1].VelM;
                    break;
                case 2:
                    Acc = m_MovePrm[nAxisNo - 1].AccL;
                    Dcc = m_MovePrm[nAxisNo - 1].DccL;
                    speed = m_MovePrm[nAxisNo - 1].VelL;
                    break;

            }
            //mm/s--->plus/ms
            dSpeed = speed = (1 * speed / m_MovePrm[nAxisNo - 1].AxisLeadRange) * m_MovePrm[nAxisNo - 1].PlusePerRote / 1000;
            Acc = (1 * Acc / m_MovePrm[nAxisNo - 1].AxisLeadRange) * m_MovePrm[nAxisNo - 1].PlusePerRote / 1000000;
            Dcc = (1 * Dcc / m_MovePrm[nAxisNo - 1].AxisLeadRange) * m_MovePrm[nAxisNo - 1].PlusePerRote / 1000000;
            return true;
        }
        public override bool Open()
        {
            short rtn = 0;
            rtn |= gts.mc.GT_Open((short)m_nCardIndex, 0, 1);
            rtn |= gts.mc.GT_Reset((short)m_nCardIndex);
            rtn |= gts.mc.GT_LoadConfig((short)m_nCardIndex, "D:\\GTS800_" + m_nCardIndex.ToString() + ".cfg");
            m_bOpen = (rtn == 0);
            return rtn == 0;
        }
        public override bool IsOpen()
        {
            return m_bOpen;
        }
        public override bool Close()
        {
            short rtn = 0;
            rtn |= gts.mc.GT_Close((short)m_nCardIndex);
            return 0 == rtn;
        }
        public override bool ServoOn(short nAxisNo)
        {
            nAxisNo += 1;
            short rtn = 0;
            rtn |= gts.mc.GT_ClrSts((short)m_nCardIndex, (short)nAxisNo, 8);
            rtn |= gts.mc.GT_AxisOn((short)m_nCardIndex, (short)nAxisNo);
            return 0 == rtn;
        }
        public override bool ServoOff(short nAxisNo)
        {
            nAxisNo += 1;
            short rtn = 0;
            rtn |= gts.mc.GT_AxisOff((short)m_nCardIndex, (short)nAxisNo);
            return 0 == rtn;
        }
        public bool ClearAlarm(short nAxisNo)
        {
            nAxisNo += 1;
            short rtn = 0;
            rtn |= gts.mc.GT_ClrSts((short)m_nCardIndex, (short)nAxisNo, 1);
            return 0 == rtn;
        }
        public override bool AbsMove(int nAxisNo, double nPos, double nSpeed)
        {
            short rtn = 0;
            nAxisNo += 1;
            uint axisMark = (uint)1 << (nAxisNo - 1);
            rtn |= gts.mc.GT_SetAxisBand((short)m_nCardIndex, (short)nAxisNo, 20, 10);// 5);
            gts.mc.TTrapPrm trap = new gts.mc.TTrapPrm();
            double speed = nSpeed;
            double Acc = 0;
            double Dcc = 0;
            TranMMToPluse(nAxisNo, ref speed, ref Acc, ref Dcc);

            trap.acc = Acc;//3;
            trap.dec = Dcc;//3;
            trap.velStart = 0;
            trap.smoothTime = (short)m_nSmoothTimes[nAxisNo - 1];//20;
            rtn |= gts.mc.GT_ClrSts((short)m_nCardIndex, (short)nAxisNo, 8);
            rtn |= gts.mc.GT_PrfTrap((short)m_nCardIndex, (short)nAxisNo);
            rtn |= gts.mc.GT_SetTrapPrm((short)m_nCardIndex, (short)nAxisNo, ref trap);
            rtn |= gts.mc.GT_SetVel((short)m_nCardIndex, (short)nAxisNo, (double)nSpeed);
            rtn |= gts.mc.GT_SetPos((short)m_nCardIndex, (short)nAxisNo, (int)nPos);
            rtn |= gts.mc.GT_Update((short)m_nCardIndex, 1 << (nAxisNo - 1));
            if (rtn == 0)
                return true;
            else
                return false;
        }
        public override bool RelativeMove(int nAxisNo, double nPos, double nSpeed)
        {
            nAxisNo += 1;
            uint axisMark = (uint)1 << (nAxisNo - 1);
            short nRtn = 0;
            uint clk = 0;
            double dAxisCurPos = 0;
            // nRtn |= gts.mc.GT_GetEncPos((short)m_nCardIndex, (short)nAxisNo, out dAxisCurPos, 1, out clk);s
            gts.mc.GT_SetAxisBand((short)m_nCardIndex, (short)nAxisNo, 20, 10);// 5);
            double speed = nSpeed;
            double Acc = 0;
            double Dcc = 0;
            TranMMToPluse(nAxisNo, ref speed, ref Acc, ref Dcc);

            gts.mc.TTrapPrm trap = new gts.mc.TTrapPrm();
            speed = (1 * speed / m_MovePrm[nAxisNo - 1].AxisLeadRange) * m_MovePrm[nAxisNo - 1].PlusePerRote / 1000;
            Acc = (1 * Acc / m_MovePrm[nAxisNo - 1].AxisLeadRange) * m_MovePrm[nAxisNo - 1].PlusePerRote / 1000000;
            Dcc = (1 * Dcc / m_MovePrm[nAxisNo - 1].AxisLeadRange) * m_MovePrm[nAxisNo - 1].PlusePerRote / 1000000;
            trap.acc = Acc;//3;
            trap.dec = Dcc;//3;
            trap.velStart = 0;
            trap.smoothTime = (short)m_nSmoothTimes[nAxisNo - 1];//20;
            int extPos = 0;
           
            if (0 == gts.mc.GT_ClrSts((short)m_nCardIndex, (short)nAxisNo, 8)
                && 0 == gts.mc.GT_PrfTrap((short)m_nCardIndex, (short)nAxisNo)
                && 0 == gts.mc.GT_SetTrapPrm((short)m_nCardIndex, (short)nAxisNo, ref trap)
                && 0 == gts.mc.GT_SetVel((short)m_nCardIndex, (short)nAxisNo, (double)speed)
                && 0 == gts.mc.GT_GetPos((short)m_nCardIndex, (short)nAxisNo, out extPos)
                && 0 == gts.mc.GT_SetPos((short)m_nCardIndex, (short)nAxisNo, extPos + (int)nPos)
                && 0 == gts.mc.GT_Update((short)m_nCardIndex, 1 << (nAxisNo - 1))
                )
            {

                return true;
            }

            return false;
        }
        public override bool JogMove(int nAxisNo, bool bPositive, int bStart, double nSpeed)
        {
            nAxisNo += 1;
            double speed = nSpeed;
            double Acc = 0;
            double Dcc = 0;
            TranMMToPluse(nAxisNo, ref speed, ref Acc, ref Dcc);
           
            gts.mc.TJogPrm trap = new gts.mc.TJogPrm();
            if (0 == gts.mc.GT_ClrSts((short)m_nCardIndex, (short)nAxisNo, 8)
                && 0 == gts.mc.GT_PrfJog((short)m_nCardIndex, (short)nAxisNo)
                && 0 == gts.mc.GT_GetJogPrm((short)m_nCardIndex, (short)nAxisNo, out trap))
            {
                trap.acc = 0.1;
                trap.dec = 0.1;
                if (0 == gts.mc.GT_SetJogPrm((short)m_nCardIndex, (short)nAxisNo, ref trap)
                    && 0 == gts.mc.GT_SetVel((short)m_nCardIndex, (short)nAxisNo, bPositive ? (double)nSpeed : (double)-nSpeed)
                    && 0 == gts.mc.GT_Update((short)m_nCardIndex, 1 << (nAxisNo - 1)))
                {
                    return true;
                }
            }
            return false;
        }
        public override bool StopAxis(int nAxisNo)
        {
            nAxisNo += 1;
            uint axisMark = (uint)1 << (nAxisNo - 1);
            return 0 == gts.mc.GT_Stop((short)m_nCardIndex, 1 << (nAxisNo - 1), 0);
        }
        public override bool StopEmg(int nAxisNo)
        {
            nAxisNo += 1;
            uint axisMark = (uint)1 << (nAxisNo - 1);
            return 0 == gts.mc.GT_Stop((short)m_nCardIndex, 1 << (nAxisNo - 1), 1);
        }
        public override bool ReasetAxis(int nAxisNo)
        {
            int nSystemAxisNo = GetSystemAxisNo(nAxisNo);
            nAxisNo += 1;
            int nCore = nAxisNo <= 32 ? 1 : 2;
            nAxisNo = (short)(nAxisNo > 32 ? nAxisNo - 32 : nAxisNo);
            m_AxisStates[nAxisNo - 1] = AxisState.NormalStop;
            mc.GT_ClrSts((short)nCore, (short)nAxisNo, 8);
            IsAxisNormalStop(nSystemAxisNo);
            return 0 == mc.GT_ClrSts((short)nCore, (short)nAxisNo, 8);
        }
        public override long GetMotionIoState(int nAxisNo)
        {
            nAxisNo += 1;
            //0:保留
            //1:alarm
            //2:保留
            //3:保留
            //4:跟随误差
            //5：正限位触发
            //6：负限位触发
            //7：IO平滑停止触发
            //8：IO急停触发
            //9：电机使能标志
            //10：规划运动标志
            //11：电机到位标志
            uint clk = 0;
            int sts;
            long lStandardIo = 0;
            short rtn = 0;
            rtn |= gts.mc.GT_ClrSts((short)m_nCardIndex, (short)nAxisNo, 8);
            if (0 == gts.mc.GT_GetDi((short)m_nCardIndex, gts.mc.MC_HOME, out sts))
            {
                if ((sts & (0x01 << (nAxisNo - 1))) == 0)
                {
                    lStandardIo |= (0x01 << 3); //原点到位存在第4位
                    lStandardIo |= (0x01 << 5); //零位到位存在第6位
                }
            }
            if (0 == mc.GT_GetSts((short)m_nCardIndex, (short)nAxisNo, out sts, 1, out clk))
            {
                if ((sts & (0x01 << 1)) != 0)
                    lStandardIo |= (0x01 << 0); //驱动器报警存在第1位
                if ((sts & (0x01 << 5)) != 0)
                    lStandardIo |= (0x01 << 1); //正限位报警存在第2位
                if ((sts & (0x01 << 6)) != 0)
                    lStandardIo |= (0x01 << 2); //负限位报警存在第3位
                if ((sts & (0x01 << 8)) != 0)
                    lStandardIo |= (0x01 << 4); //急停触发存在第5位
                if ((sts & (0x01 << 11)) != 0)
                    lStandardIo |= (0x01 << 6); //电机到位标志存在第7位
                if ((sts & (0x01 << 9)) != 0)
                    lStandardIo |= (0x01 << 7); //电机使能标志存在第8位

                return lStandardIo;
            }
            else
            {
                return -1;
            }

        }
        public override bool GetServoState(int nAxisNo)
        {
            nAxisNo += 1;
            int sts = 0;
            uint clk = 0;
            if (0 == gts.mc.GT_GetSts((short)m_nCardIndex, (short)nAxisNo, out sts, 1, out clk))
            {
                return 0 != (sts & (1 << 9));
            }
            return true;
        }
        public override AxisState IsAxisNormalStop(int nAxisNo)
        {
            nAxisNo += 1;
            int sts = 0;
            uint clk = 0;
            if (0 == gts.mc.GT_GetSts((short)m_nCardIndex, (short)nAxisNo, out sts, 1, out clk))
            {
                if (0 != (sts & (1 << 1)))
                {
                    return AxisState.DriveAlarm;   //驱动器异常报警
                }
                else if (0 != (sts & (1 << 5)))
                {
                    return AxisState.LimtPStop;   //正限位触发
                }
                else if (0 != (sts & (1 << 6)))
                {
                    return AxisState.LimtNStop;   //负限位触发
                }
                if (0 == (sts & (1 << 10)))
                {
                    return AxisState.NormalStop;   //正常停止
                }
                    return AxisState.Moving;  //正在运动中
            }
            else
                   return AxisState.ErrAlarm;
        }
        public override bool isOrgTrig(int nAxisNo)
        {
            // 信号取反 m_ProPrietarySignal[nAxisNo - 1].oriSignal===1    
            //0 信号为正常m_ProPrietarySignal[nAxisNo - 1].oriSignal===0
            nAxisNo += 1;
            short sRtn = 0;
            int orgLimit = 0;
            sRtn |= gts.mc.GT_GetDi((short)m_nCardIndex, gts.mc.MC_HOME, out orgLimit);
            int signal = (orgLimit & (1 << (nAxisNo - 1)));
          
            return 1 == (signal > 0 ? 1 : 0);
        }
        public bool isNegLimit(int nAxisNo)
        {
            nAxisNo += 1;
            short sRtn = 0;
            int negLimit = 0;
            sRtn |= gts.mc.GT_GetDi((short)m_nCardIndex, gts.mc.MC_LIMIT_NEGATIVE, out negLimit);
            return 0 != (negLimit & (1 << (nAxisNo - 1)));
        }
        public bool isPosLimit(int nAxisNo)
        {
            nAxisNo += 1;
            short sRtn = 0;
            int posLimit = 0;
            sRtn |= gts.mc.GT_GetDi((short)m_nCardIndex, gts.mc.MC_LIMIT_POSITIVE, out posLimit);
            return 0 != (posLimit & (1 << (nAxisNo - 1)));
        }
        public bool isCapture(int nAxisNo, out int pos)
        {
            nAxisNo += 1;
            short capture = 0;
            //int pos = 0;
            uint pclock;
            gts.mc.GT_GetCaptureStatus((short)m_nCardIndex, (short)nAxisNo, out capture, out pos, 1, out pclock);//读取捕获状态
            if (1 == capture)
                return true;
            else
                return false;
        }
        private short MoveLSpeedSearch(int nAxisNo, bool bPositive, double vel)
        {
            nAxisNo += 1;
            short sRtn = 0;
            sRtn |= gts.mc.GT_Stop((short)m_nCardIndex, 1 << (nAxisNo - 1), 0);
            sRtn |= gts.mc.GT_PrfTrap((short)m_nCardIndex, (short)nAxisNo);
            gts.mc.TTrapPrm trap = new gts.mc.TTrapPrm();
            trap.acc = m_HomePrm[nAxisNo - 1].AccL;//3;
            trap.dec = m_HomePrm[nAxisNo - 1].DccL;//3;
            trap.velStart = 0;
            trap.smoothTime = 20;//20;
            gts.mc.GT_SetTrapPrm((short)m_nCardIndex, (short)nAxisNo, ref trap);
            sRtn |= gts.mc.GT_SetVel((short)m_nCardIndex, (short)nAxisNo, vel);
            int extPos = 0;
            gts.mc.GT_GetPos((short)m_nCardIndex, (short)nAxisNo, out extPos);
            if (bPositive)//m_HomePrm[nAxisNo - 1]._bHomeDir
                sRtn |= gts.mc.GT_SetPos((short)m_nCardIndex, (short)nAxisNo,
                    extPos + (int)m_HomePrm[nAxisNo - 1]._iSeachOffstPluse);
            else
                sRtn |= gts.mc.GT_SetPos((short)m_nCardIndex, (short)nAxisNo,
                    extPos - (int)m_HomePrm[nAxisNo - 1]._iSeachOffstPluse);
            sRtn |= gts.mc.GT_Update((short)m_nCardIndex, 1 << (nAxisNo - 1));
            return sRtn;
        }
        private short MoveHSpeedSearch(int nAxisNo, bool bPositive)
        {
            nAxisNo += 1;
            short sRtn = 0;
            sRtn |= gts.mc.GT_Stop((short)m_nCardIndex, 1 << (nAxisNo - 1), 0);
            sRtn |= gts.mc.GT_PrfTrap((short)m_nCardIndex, (short)nAxisNo);
            gts.mc.TTrapPrm trap = new gts.mc.TTrapPrm();
            trap.acc = m_HomePrm[nAxisNo - 1].AccH;//3;
            trap.dec = m_HomePrm[nAxisNo - 1].DccH;//3;
            trap.velStart = 0;
            trap.smoothTime = 20;//20;
            gts.mc.GT_SetTrapPrm((short)m_nCardIndex, (short)nAxisNo, ref trap);
            sRtn |= gts.mc.GT_SetVel((short)m_nCardIndex, (short)nAxisNo, m_HomePrm[nAxisNo - 1].VelH);
            int extPos = 0;
            gts.mc.GT_GetPos((short)m_nCardIndex, (short)nAxisNo, out extPos);
            if (bPositive)//m_HomePrm[nAxisNo - 1]._bHomeDir
                sRtn |= gts.mc.GT_SetPos((short)m_nCardIndex, (short)nAxisNo,
                    extPos + (int)m_HomePrm[nAxisNo - 1]._iSeachDistancePluse);
            else
                sRtn |= gts.mc.GT_SetPos((short)m_nCardIndex, (short)nAxisNo,
                    extPos - (int)m_HomePrm[nAxisNo - 1]._iSeachDistancePluse);
            sRtn |= gts.mc.GT_Update((short)m_nCardIndex, 1 << (nAxisNo - 1));
            return sRtn;
        }
      
        private short JogHome(int nAxisNo, bool bPositive, double vel)
        {
            nAxisNo += 1;
            short sRtn = 0;
            sRtn |= gts.mc.GT_ClrSts((short)m_nCardIndex, (short)nAxisNo, 1);
            sRtn |= gts.mc.GT_Stop((short)m_nCardIndex, 1 << (nAxisNo - 1), 0);
            sRtn |= gts.mc.GT_PrfJog((short)m_nCardIndex, (short)nAxisNo);
            gts.mc.TJogPrm jogparam = new mc.TJogPrm();
            sRtn |= gts.mc.GT_GetJogPrm((short)m_nCardIndex, (short)nAxisNo, out jogparam);
            jogparam.acc = 0.1;
            jogparam.dec = 0.1;
            sRtn |= gts.mc.GT_SetJogPrm((short)m_nCardIndex, (short)nAxisNo, ref jogparam);
            sRtn |= gts.mc.GT_SetVel((short)m_nCardIndex, (short)nAxisNo, bPositive ? (double)vel : (double)-vel);
            sRtn |= gts.mc.GT_Update((short)m_nCardIndex, 1 << (nAxisNo - 1));
            return sRtn;

        }
        public override bool TransMMToPluseForHomeParam(int nAxisNo, ref double dVelH, ref double dVelL, ref double dAccH, ref double dAccL, ref double dDecH, ref double dDecL)
        {
            nAxisNo = nAxisNo - 1;
            dVelH = (1 * m_HomePrm[nAxisNo].VelH / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1000;

            dVelL = (1 * m_HomePrm[nAxisNo].VelL / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1000;


            dAccH = (1 * m_HomePrm[nAxisNo].AccH / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1000000;
            dAccL = (1 * m_HomePrm[nAxisNo].AccL / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1000000;

            dDecH = (1 * m_HomePrm[nAxisNo].DccH / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1000000;
            dDecL = (1 * m_HomePrm[nAxisNo].DccL / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1000000;

            return true;
        }
        public override bool Home(int nAxisNo, int nParam)
        {
            nAxisNo += 1;
            double dVelH = 0, dVelL = 0, dAccH = 0, dDecH = 0, dAccL = 0, dDecL = 0;
            TransMMToPluseForHomeParam(nAxisNo, ref dVelH, ref dVelL, ref dAccH, ref dDecH, ref dAccL, ref dDecL);
            return true;
        }
        public override AxisState IsHomeNormalStop(int nAxisNo)
        {
            nAxisNo += 1;
            //return 0 == m_nHomeFinishFlag[nAxisNo - 1];
            return AxisState.NormalStop;
        }
        public override int GetAxisPos(int nAxisNo)
        {
            nAxisNo += 1;
            double pos;
            uint pclock;
            short sRtn = 0;
            sRtn |=gts.mc.GT_GetEncPos((short)m_nCardIndex, (short)nAxisNo, out pos, 1, out pclock);
            return sRtn == 0 ?(int) pos:int.MaxValue;
        }
        public override int GetAxisActPos(int nAxisNo)
        {
            nAxisNo += 1;
            double pos;
            uint pclock;
            short sRtn = 0;
            sRtn |= gts.mc.GT_GetEncPos((short)m_nCardIndex, (short)nAxisNo, out pos, 1, out pclock);
            return sRtn == 0 ? (int)pos : int.MaxValue;
        }
        public override bool SetActutalPos(int nAxisNo, double pos)
        {
           
            logger.Info(string.Format("{0}卡{1}轴设置实际位置", m_nCardIndex, nAxisNo));
            short Result = 0;
            Result |= gts.mc.GT_SetEncPos((short)m_nCardIndex, (short)nAxisNo, (int)pos);
            return Result == 0;

        }

        public override bool SetCmdPos(int nAxisNo, double pos)
        {
            logger.Info(string.Format("{0}卡{1}轴设置命令位置", m_nCardIndex, nAxisNo));
            short Result = 0;
            Result |= gts.mc.GT_SetPos((short)m_nCardIndex, (short)nAxisNo, (int)pos);
            return Result == 0;
        }
        public override int GetAxisCmdPos(int nAxisNo)
        {
            nAxisNo += 1;
            double pos;
            uint pclock;
            short sRtn = 0;
            sRtn |= gts.mc.GT_GetEncPos((short)m_nCardIndex, (short)nAxisNo, out pos, 1, out pclock);
            return sRtn == 0 ? (int)pos : int.MaxValue;
        }
        public override bool AddAxisToGroup(int[] Axisarr, ref object group)
        {

            return true;
        }
        public override bool AddBufMove(object objGroup,BufMotionType type, int mode, int nAxisNum, double velHigh, double velLow, double[] Point1, double[] Point2)
        {

            return true;
        }
        public override bool AddBufIo(object objGroup,string strIoName, bool bVal, int nAxisIndexInGroup)
        {

            return true;
        }

        public override bool AddBufDelay(object objGroup,int nTime)
        {

            return true;
        }
        public override bool ClearBufMove(object objGroup)
        {
            return true;
        }

        public override bool StartBufMove(object objGroup)
        {
            return true;
        }
        public override bool IsInpos(int nAxisNo)
        {
            return IsAxisNormalStop(nAxisNo) == 0;

        }
    }
}