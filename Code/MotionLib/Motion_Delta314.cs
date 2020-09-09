using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using gts;
using System.Diagnostics;
using Advantech.Motion;
using System.Windows.Forms;
using log4net;
using PCI_M314;

namespace MotionIoLib
{


    public class Motion_Delta314 : MotionCardBase
    {

        public Motion_Delta314(ulong indexCard, string strName, int nMinAxisNo, int nMaxAxisNo)
            : base(indexCard, strName, nMinAxisNo, nMaxAxisNo)
        {

        }
        //public override int GetAxisNo(int IndexAxis)
        //{
        //    return AxisInRang(IndexAxis) ? (IndexAxis - GetMinAxisNo() ) : int.MaxValue;
        //}
        public override bool Open()
        {
            m_bOpen = false;
            ushort cardID = 0;
            short rtn;
            //================ M314 Initial =================//
            ushort gCardNum = 0;
            rtn = CPCI_M314.CS_m314_open(ref gCardNum);//可以多次枚举
            if (gCardNum == 0)
            {
                MessageBox.Show("PCI_M314 not found!!", "Error");
                return false;
            }

            rtn |= CPCI_M314.CS_m314_get_cardno((ushort)m_nCardIndex, ref cardID);
            rtn = CPCI_M314.CS_m314_initial_card(cardID);
            if (rtn != 0)
            {
                return false;
            }
            string strConfigPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}PCI_M314_0.cfg";
            rtn = CPCI_M314.CS_m314_config_from_file(cardID, strConfigPath);
            //for (i = 0; i < 4; i++)
            //{
            //    //----Generate Interface IO Function----//
            //    CPCI_M314.CS_m314_set_alm((ushort)m_nCardIndex, i, 1, 0); 0 1 2 3
            //    CPCI_M314.CS_m314_set_ez((ushort)m_nCardIndex, i, 0);
            //    CPCI_M314.CS_m314_set_erc((ushort)m_nCardIndex, i, 32);
            //    CPCI_M314.CS_m314_set_sd((ushort)m_nCardIndex, i, 0, 0, 0);
            //    CPCI_M314.CS_m314_set_ell((ushort)m_nCardIndex, i, 0);
            //    //----Generate Pulse IO Function----//
            //    CPCI_M314.CS_m314_set_pls_outmode((ushort)m_nCardIndex, i, 6);   //4: Cw/CCw, 6: AB Phase
            //    CPCI_M314.CS_m314_set_pls_outwidth((ushort)m_nCardIndex, i, 1);
            //    CPCI_M314.CS_m314_set_feedback_src((ushort)m_nCardIndex, i, 0);  //0: external, 1: internal
            //    CPCI_M314.CS_m314_set_move_ratio((ushort)m_nCardIndex, i, 1);
            //}
            //if(cardID==2)
            //{
            //    rtn = CPCI_M314.CS_m314_set_gear(2, 3, 1, 10, 1);//AA机器默认设置
            //}
            short a1 = 0;
            short a2 = 0;
            ushort a3 = 0;
            CPCI_M314.CS_m314_get_gear(2, 3, ref a1, ref a2, ref a3);
            m_bOpen = true;
            return (rtn == 0);
        }
        public override bool IsOpen()
        {
            return m_bOpen;
        }
        public override bool Close()
        {
            short rtn = 0;
            rtn = CPCI_M314.CS_m314_close((ushort)m_nCardIndex);
            return (rtn == 0);
        }
        public override bool ServoOn(short nAxisNo)
        {
            short rtn = 0;
            rtn = CPCI_M314.CS_m314_set_servo((ushort)m_nCardIndex, (ushort)nAxisNo, 1);
            return (rtn == 0);
        }
        public override bool ServoOff(short nAxisNo)
        {
            short rtn = 0;
            rtn = CPCI_M314.CS_m314_set_servo((ushort)m_nCardIndex, (ushort)nAxisNo, 0);
            return (rtn == 0);
        }
        public bool ClearAlarm(short nAxisNo)
        {
            short rtn = 0;
            rtn |= CPCI_M314.CS_m314_set_servo((ushort)m_nCardIndex, (ushort)nAxisNo, 0);
            Thread.Sleep(20);
            rtn |= CPCI_M314.CS_m314_set_servo((ushort)m_nCardIndex, (ushort)nAxisNo, 1);
            return (rtn == 0);
        }
        public override bool AbsMove(int nAxisNo, double nPos, double nSpeed)
        {
            short rtn = 0;
            double speed = nSpeed;
            double Acc = m_MovePrm[nAxisNo].AccH;
            double Dcc = m_MovePrm[nAxisNo].DccH;
            switch (nSpeed)
            {
                case (double)0:
                    Acc = m_MovePrm[nAxisNo].AccH;
                    Dcc = m_MovePrm[nAxisNo].DccH;
                    speed = m_MovePrm[nAxisNo].VelH;
                    break;
                case (double)SpeedType.Mid:
                    Acc = m_MovePrm[nAxisNo].AccM;
                    Dcc = m_MovePrm[nAxisNo].DccM;
                    speed = m_MovePrm[nAxisNo].VelM;
                    break;
                case (double)SpeedType.Low:
                    Acc = m_MovePrm[nAxisNo].AccL;
                    Dcc = m_MovePrm[nAxisNo].DccL;
                    speed = m_MovePrm[nAxisNo].VelL;
                    break;

            }
            double Tacc = (speed - 0) / Acc;
            double Tdec = (speed - 0) / Dcc;
            if (m_nCardIndex == 2 && nAxisNo == 3)
            {
                rtn |= CPCI_M314.CS_m314_start_ta_move((ushort)m_nCardIndex, (ushort)nAxisNo, (int)(nPos / 10.0), 0, (int)(speed / 10.0), Tacc / 10.0, Tdec / 10.0);
            }
            else
            {
                rtn |= CPCI_M314.CS_m314_start_ta_move((ushort)m_nCardIndex, (ushort)nAxisNo, (int)nPos, 0, (int)speed, Tacc, Tdec);
            }
            return (rtn == 0);
        }
        public override bool RelativeMove(int nAxisNo, double nPos, double nSpeed)
        {
            short rtn = 0;
            double speed = nSpeed;
            double Acc = m_MovePrm[nAxisNo].AccH;
            double Dcc = m_MovePrm[nAxisNo].DccH;
            switch (nSpeed)
            {
                case (double)0:
                    Acc = m_MovePrm[nAxisNo].AccH;
                    Dcc = m_MovePrm[nAxisNo].DccH;
                    speed = m_MovePrm[nAxisNo].VelH;
                    break;
                case (double)SpeedType.Mid:
                    Acc = m_MovePrm[nAxisNo].AccM;
                    Dcc = m_MovePrm[nAxisNo].DccM;
                    speed = m_MovePrm[nAxisNo].VelM;
                    break;
                case (double)SpeedType.Low:
                    Acc = m_MovePrm[nAxisNo].AccL;
                    Dcc = m_MovePrm[nAxisNo].DccL;
                    speed = m_MovePrm[nAxisNo].VelL;
                    break;

            }
            double Tacc = (speed - 0) / Acc;
            double Tdec = (speed - 0) / Dcc;
            if (false && m_nCardIndex == 2 && nAxisNo == 3)
            {
                rtn |= CPCI_M314.CS_m314_start_tr_move((ushort)m_nCardIndex, (ushort)nAxisNo, (int)(nPos / 10.0), 0, (int)(speed / 10.0), Tacc / 10.0, Tdec / 10.0);
            }
            else
            {
                rtn |= CPCI_M314.CS_m314_start_tr_move((ushort)m_nCardIndex, (ushort)nAxisNo, (int)nPos, 0, (int)speed, Tacc, Tdec);
            }
            return (rtn == 0);

        }
        public override bool JogMove(int nAxisNo, bool bPositive, int bStart, double nSpeed)
        {
            short rtn = 0;
            double speed = nSpeed;
            double Acc = m_MovePrm[nAxisNo].AccH;
            double Dcc = m_MovePrm[nAxisNo].DccH;
            switch (nSpeed)
            {
                case (double)0:
                    Acc = m_MovePrm[nAxisNo].AccH;
                    Dcc = m_MovePrm[nAxisNo].DccH;
                    speed = m_MovePrm[nAxisNo].VelH;
                    break;
                case (double)SpeedType.Mid:
                    Acc = m_MovePrm[nAxisNo].AccM;
                    Dcc = m_MovePrm[nAxisNo].DccM;
                    speed = m_MovePrm[nAxisNo].VelM;
                    break;
                case (double)SpeedType.Low:
                    Acc = m_MovePrm[nAxisNo].AccL;
                    Dcc = m_MovePrm[nAxisNo].DccL;
                    speed = m_MovePrm[nAxisNo].VelL;
                    break;

            }
            double Tacc = (speed - 0) / Acc;
            double Tdec = (speed - 0) / Dcc;
            short dir = bPositive ? (short)0 : (short)1;
            if (false && m_nCardIndex == 2 && nAxisNo == 3)
            {
                //((ushort)m_nCardIndex, (ushort)nAxisNo, (int)(nPos / 10.0), 0, (int)(speed / 10.0), Tacc / 10.0, Tdec / 10.0);
                rtn |= CPCI_M314.CS_m314_tv_move((ushort)m_nCardIndex, (ushort)nAxisNo, 0, (int)(speed / 10.0), Tacc / 10.0, dir);
            }
            else
            {
                rtn |= CPCI_M314.CS_m314_tv_move((ushort)m_nCardIndex, (ushort)nAxisNo, 0, (int)(speed), Tacc, dir);
            }
            return (rtn == 0);
        }
        public bool Istop = false;
        public override bool StopAxis(int nAxisNo)
        {
            Istop = true;
            double dccTime = GetTdec(nAxisNo, 0);
            short rtn = 0;
            return (rtn == CPCI_M314.CS_m314_sd_stop((ushort)m_nCardIndex, (ushort)nAxisNo, dccTime));
        }
        public override bool StopEmg(int nAxisNo)
        {
            short rtn = 0;
            rtn |= CPCI_M314.CS_m314_emg_stop((ushort)m_nCardIndex, (ushort)nAxisNo);
            return (rtn == 0);
        }
        public override bool Home(int nAxisNo, int nParam)
        {
            Istop = false;
            short rtn = 0;
            double acc = m_HomePrm[nAxisNo].AccH;
            double vel = m_HomePrm[nAxisNo].VelH;
            double Tacc = (vel - 0) / acc;
            double Tdcc = (vel - 0) / m_HomePrm[nAxisNo].DccH;
            int dir = m_HomePrm[nAxisNo]._bHomeDir ? 0 : 1;
            rtn |= CPCI_M314.CS_m314_set_servo((ushort)m_nCardIndex, (ushort)nAxisNo, 1);
            rtn |= CPCI_M314.CS_m314_set_home_finish_reset((ushort)m_nCardIndex, (ushort)nAxisNo, 1);
            rtn |= CPCI_M314.CS_m314_set_home_config((ushort)m_nCardIndex, (ushort)nAxisNo, (short)m_HomePrm[nAxisNo]._nHomeMode, 0, 0, 0);
            rtn |= CPCI_M314.CS_m314_set_home_offset_position((ushort)m_nCardIndex, (ushort)nAxisNo, (int)m_HomePrm[nAxisNo]._iSeachOffstPluse);
            rtn |= CPCI_M314.CS_m314_home_move((ushort)m_nCardIndex, (ushort)nAxisNo, 0, (int)vel, Tacc, (short)dir);
            if (rtn != 0)
            {
                return false;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            ushort MoSt = 1;
            do
            {
                if (Istop)
                {
                    rtn = -1;
                    break;
                }
                if (stopwatch.ElapsedMilliseconds > 30000)
                {
                    m_AxisStates[nAxisNo] = AxisState.ErrAlarm;
                    //超时停止
                    CPCI_M314.CS_m314_sd_stop((ushort)m_nCardIndex, (ushort)nAxisNo, Tdcc);
                    CPCI_M314.CS_m314_set_servo((ushort)m_nCardIndex, (ushort)nAxisNo, 0);
                    logger.Warn($"{m_nCardIndex}卡{nAxisNo}号轴 回0 过程中超时");
                    return false;
                }
                if (rtn == 0)
                {
                    rtn |= CPCI_M314.CS_m314_reset_error_counter((ushort)m_nCardIndex, (ushort)nAxisNo);
                    rtn |= CPCI_M314.CS_m314_set_home_finish_reset((ushort)m_nCardIndex, (ushort)nAxisNo, 0);
                }
                rtn |= CPCI_M314.CS_m314_motion_done((ushort)m_nCardIndex, (ushort)nAxisNo, ref MoSt);
                Thread.Sleep(10);
            }
            while (MoSt == 0);
            return (rtn == 0);
        }
        public override bool ReasetAxis(int nAxisNo)
        {
            short rtn = 0;
            rtn |= CPCI_M314.CS_m314_set_command((ushort)m_nCardIndex, (ushort)nAxisNo, 0);
            rtn |= CPCI_M314.CS_m314_set_position((ushort)m_nCardIndex, (ushort)nAxisNo, 0.0);
            return (rtn == 0);
        }
        public override long GetMotionIoState(int nAxisNo)
        {
            short rtn = 0;
            ushort evenStatus = 0;
            long lStandardIo = 0;
            rtn |= CPCI_M314.CS_m314_get_io_status((ushort)m_nCardIndex, (ushort)nAxisNo, ref evenStatus);
            if (rtn == 0)
            {
                short p = 3;
                var k = p << 1;
                var t = evenStatus & k;
                if ((evenStatus & ((ushort)1 << 1)) == 2L)
                {
                    lStandardIo |= (0x01 << 1);//驱动器报警存在第1位
                }
                if ((evenStatus & ((ushort)1 << 2)) == 4L)
                {
                    lStandardIo |= (0x01 << 1); //正限位报警存在第2位
                }
                if ((evenStatus & ((ushort)1 << 3)) == 8L)
                {
                    lStandardIo |= (0x01 << 2); //负限位报警存在第3位
                }
                if ((evenStatus & ((ushort)1 << 4)) == 16L)
                {
                    lStandardIo |= (0x01 << 3); //原点到位存在第4位
                }
                if ((evenStatus & ((ushort)1 << 6)) == 64L)
                {
                    lStandardIo |= (0x01 << 4); //急停触发存在第5位
                }
                if ((evenStatus & ((ushort)1 << 14)) == (1 << 14))
                {
                    lStandardIo |= (0x01 << 7); //电机使能标志存在第8位
                }
                return lStandardIo;

            }

            return -1;
        }
        public override bool GetServoState(int nAxisNo)
        {
            return false;
        }
        public override AxisState IsAxisNormalStop(int nAxisNo)
        {
            short rtn = 0;
            ushort MoSt = 1;
            rtn |= CPCI_M314.CS_m314_motion_done((ushort)m_nCardIndex, (ushort)nAxisNo, ref MoSt);

            if (rtn == 0 && MoSt == 0)
            {
                return AxisState.NormalStop;
            }
            else
            {
                return AxisState.Moving;
            }
        }



        public override bool SetActutalPos(int nAxisNo, double pos)
        {
            logger.Info(string.Format("{0}卡{1}轴设置实际位置", m_nCardIndex, nAxisNo));
            short rtn = 0;
            rtn |= CPCI_M314.CS_m314_set_position((ushort)m_nCardIndex, (ushort)nAxisNo, pos);
            return (rtn == 0);
        }

        public override bool SetCmdPos(int nAxisNo, double pos)
        {
            logger.Info(string.Format("{0}卡{1}轴设置命令位置", m_nCardIndex, nAxisNo));
            short rtn = 0;
            rtn |= CPCI_M314.CS_m314_set_command((ushort)m_nCardIndex, (ushort)nAxisNo, (int)pos);
            return (rtn == 0);
        }
        public override AxisState IsHomeNormalStop(int nAxisNo)
        {
            short rtn = 0;
            ushort MoSt = 1;
            rtn |= CPCI_M314.CS_m314_motion_done((ushort)m_nCardIndex, (ushort)nAxisNo, ref MoSt);

            if (rtn == 0 && MoSt == 0)
            {
                return AxisState.NormalStop;
            }
            else if (rtn == 0 && MoSt != 0)
            {
                return AxisState.Homeing;
            }
            else
            {
                return AxisState.ErrAlarm;
            }
        }
        public override int GetAxisActPos(int nAxisNo)
        {
            short rtn = 0;
            double post = 0; int postCmd = 0;
            if (m_MotorType[nAxisNo] >= MotorType.SEVER)
            {
                rtn |= CPCI_M314.CS_m314_get_position((ushort)m_nCardIndex, (ushort)nAxisNo, ref post);
            }
            else
            {
                rtn |= CPCI_M314.CS_m314_get_command((ushort)m_nCardIndex, (ushort)nAxisNo, ref postCmd);
                post = postCmd;
            }
            if (false && m_nCardIndex == 2 && nAxisNo == 3)
            {
                post = post / 10.0;
            }
            if (rtn == 0)
                return (int)post;
            else
                return 0;
        }
        public override int GetAxisCmdPos(int nAxisNo)
        {
            short rtn = 0;
            int post = 0;
            rtn |= CPCI_M314.CS_m314_get_command((ushort)m_nCardIndex, (ushort)nAxisNo, ref post);
            if (false && m_nCardIndex == 2 && nAxisNo == 3)
            {
                post = (int)(post / 10.0);
            }

            if (rtn == 0)
                return (int)post;
            else
                return 0;

        }
        public override int GetAxisPos(int nAxisNo)
        {
            int pos = 0;
            if (m_MotorType[nAxisNo] >= MotorType.SEVER)
                pos = GetAxisActPos(nAxisNo);
            else
                pos = GetAxisCmdPos(nAxisNo);
            return pos;
        }


        public override bool isOrgTrig(int nAxisNo)
        {
            //  throw new NotImplementedException();
            return true;
        }
        public override bool AddAxisToGroup(int[] nAxisArr, ref object group)
        {
            return true;
        }
        //public override bool CloseAxisGroup(ref IntPtr group)
        //{
        //    uint reslut = 0;
        //    return false;
        //}
        //public override GpState GetGpState(IntPtr group)
        //{
        //    uint reslut = 0;
        //    ushort groupstate = 0;
        //    logger.Info("获取群组状态异常（GetGpState）");


        //    return GpState.GpErrStop;

        //}
        //public override bool StopGp(IntPtr group)
        //{
        //    uint reslut = 0;

        //    return reslut == (uint)ErrorCode.SUCCESS;
        //}
        //public override bool ResetGpErr(IntPtr group)
        //{
        //    uint reslut = 0;

        //    return reslut == (uint)ErrorCode.SUCCESS;
        //}
        public override bool Line2Axisabs(IntPtr group, int xAxis, int yAxis, double xpos, double ypos, double acc, double dec, double velrun, double velori = 0)
        {

            try
            {
                uint reslut = 0;
                return false;
            }
            catch (Exception e)
            {
                logger.Info("Line2Axisabs运动异常：" + e.Message);

                return false;
            }
            return false;

        }
        /// <summary>
        /// 与其他的控制卡不兼容，因为这个M314没有buf'运动
        /// </summary>
        /// <param name="objGroup"></param>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        /// <param name="nAxisNum">nAxisNum 3=1&2</param>
        /// <param name="velHigh"></param>
        /// <param name="velLow"></param>
        /// <param name="Point1">Point1[0]= centerX,Point1[1]=centerY</param>
        /// <param name="Point2">Point2[0]=angle</param>
        /// <returns></returns>
        public override bool AddBufMove(object objGroup, BufMotionType type, int mode, int nAxisNum, double velHigh, double velLow, double[] Point1, double[] Point2)
        {
            if (mode == 1)
            {
                short rtn = 0;
                ushort[] array = new ushort[3] { (ushort)nAxisNum, (ushort)(nAxisNum + 1),(ushort)(nAxisNum-1)};

                double speed = velHigh;
                double Acc = m_MovePrm[2].AccH;
                double Dcc = m_MovePrm[2].DccH;
                switch ((int)velHigh)
                {
                    case 0:
                        Acc = m_MovePrm[2].AccH;
                        Dcc = m_MovePrm[2].DccH;
                        speed = m_MovePrm[2].VelH;
                        break;
                    case (int)SpeedType.Mid:
                        Acc = m_MovePrm[2].AccM;
                        Dcc = m_MovePrm[2].DccM;
                        speed = m_MovePrm[2].VelM;
                        break;
                    case (int)SpeedType.Low:
                        Acc = m_MovePrm[2].AccL;
                        Dcc = m_MovePrm[2].DccL;
                        speed = m_MovePrm[2].VelL;
                        break;

                }
                double Tacc = (speed - 0) / Acc;
                double Tdec = (speed - 0) / Dcc;
         
              //  rtn |= CPCI_M314.CS_m314_start_ta_arc_xy((ushort)m_nCardIndex, ref array[0], (int)Point1[0], (int)(Point1[1]/10.0), Point2[0], 0, (int)speed, Tacc, Tdec);
                rtn |= CPCI_M314.CS_m314_start_tr_heli_xy((ushort)m_nCardIndex, ref array[0], (int)Point1[0], (int)(Point1[1] / 10.0),(int)10, 9, 0,0, (int)speed, Tacc, Tdec);
                return rtn == 0;
            }



            return false;
        }
        public override bool AddBufIo(object objGroup, string strIoName, bool bVal, int nAxisIndexInGroup)
        {
            return true;
        }

        public override bool AddBufDelay(object objGroup, int nTime)
        {
            uint reslut = 0;
            return reslut == (uint)ErrorCode.SUCCESS;
        }
        public override bool ClearBufMove(object objGroup)
        {
            return false;
        }

        public override bool StartBufMove(object objGroup)
        {
            uint reslut = 0;
            return reslut == (uint)ErrorCode.SUCCESS;
        }
        public override bool SetBufMoveParam(object objGroup, double velhigh, double vellow, double acc, double dec)
        {
            uint reslut = 0;
            return reslut == 0;
        }

        public override bool IsInpos(int nAxisNo)
        {
            return IsAxisNormalStop(nAxisNo) == AxisState.NormalStop;

        }
        private double GetTdec(int nAxisNo, int type)
        {
            double dcc = 1.0;
            double vel = 1.0;
            if (type == 0)
            {
                dcc = m_MovePrm[nAxisNo].DccH;
                vel = m_MovePrm[nAxisNo].VelH;
                return (vel - 0) / dcc;
            }
            else if (type == 1)
            {
                dcc = m_MovePrm[nAxisNo].DccM;
                vel = m_MovePrm[nAxisNo].VelM;
                return (vel - 0) / dcc;
            }
            else
            {
                dcc = m_MovePrm[nAxisNo].DccL;
                vel = m_MovePrm[nAxisNo].VelL;
                return (vel - 0) / dcc;
            }
        }
        private double GetTAcc(int nAxisNo, int type)
        {
            double acc = 1.0;
            double vel = 1.0;
            if (type == 0)
            {
                acc = m_MovePrm[nAxisNo].AccH;
                vel = m_MovePrm[nAxisNo].VelH;
                return (vel - 0) / acc;
            }
            else if (type == 1)
            {
                acc = m_MovePrm[nAxisNo].AccM;
                vel = m_MovePrm[nAxisNo].VelM;
                return (vel - 0) / acc;
            }
            else
            {
                acc = m_MovePrm[nAxisNo].AccL;
                vel = m_MovePrm[nAxisNo].VelL;
                return (vel - 0) / acc;
            }

        }

        public override bool TranMMToPluse(int nAxisNo, ref double dSpeed, ref double acc, ref double dec)
        {
            double speed = dSpeed;
            double Acc = m_MovePrm[nAxisNo].AccH;
            double Dcc = m_MovePrm[nAxisNo].AccL;
            switch (dSpeed)
            {
                case (int)SpeedType.High:
                    Acc = m_MovePrm[nAxisNo].AccH;
                    Dcc = m_MovePrm[nAxisNo].DccH;
                    speed = m_MovePrm[nAxisNo].VelH;
                    break;
                case (int)SpeedType.Mid:
                    Acc = m_MovePrm[nAxisNo].AccM;
                    Dcc = m_MovePrm[nAxisNo].DccM;
                    speed = m_MovePrm[nAxisNo].VelM;
                    break;
                case (int)SpeedType.Low:
                    Acc = m_MovePrm[nAxisNo].AccL;
                    Dcc = m_MovePrm[nAxisNo].DccL;
                    speed = m_MovePrm[nAxisNo].VelL;
                    break;
            }
            //新添转换
            dSpeed = speed = (1 * speed / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;//- 1- 1
            acc = Acc = (1 * Acc / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;
            dec = Dcc = (1 * Dcc / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;
            return true;

        }

        public override bool TransMMToPluseForHomeParam(int nAxisNo, ref double dVelH, ref double dVelL, ref double dAccH, ref double dAccL, ref double dDecH, ref double dDecL)
        {
            dVelH = (1 * m_HomePrm[nAxisNo].VelH / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;

            dVelL = (1 * m_HomePrm[nAxisNo].VelL / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;


            dAccH = (1 * m_HomePrm[nAxisNo].AccH / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;
            dAccL = (1 * m_HomePrm[nAxisNo].AccL / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;

            dDecH = (1 * m_HomePrm[nAxisNo].DccH / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;
            dDecL = (1 * m_HomePrm[nAxisNo].DccL / m_MovePrm[nAxisNo].AxisLeadRange) * m_MovePrm[nAxisNo].PlusePerRote / 1;

            return true;

        }
    }
}