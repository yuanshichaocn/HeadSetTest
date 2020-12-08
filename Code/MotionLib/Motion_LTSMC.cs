using Leadshine;
using System;
using System.IO;
using System.Text;

namespace MotionIoLib
{
    public class Motion_LTSMC : MotionCardBase
    {
        private ushort _ConnecNo = 0;
        private ushort[] axis = new ushort[2];//插补轴,0,1

        public Motion_LTSMC(ulong indexCard, string strName, int nMinAxisNo, int nMaxAxisNo)
    : base(indexCard, strName, nMinAxisNo, nMaxAxisNo)
        {
        }

        public override bool AbsMove(int nAxisNo, double nPos, double nSpeed)
        {
            //if(nAxisNo == 0)
            //{
            //    int ret = 0;
            //    LTSMC.smc_get_stop_reason(_ConnecNo, (ushort)nAxisNo, ref ret);
            //    MessageBox.Show(ret.ToString());
            //}
            double pos = GetAxisActPos(nAxisNo);
            pos = nPos - pos;
            RelativeMove(nAxisNo, pos, nSpeed);
            return true;
        }

        public override bool AddAxisToGroup(int[] nAxisarr, ref object groupId)
        {
            axis[0] = (ushort)nAxisarr[0];
            axis[1] = (ushort)nAxisarr[1];
            //LTSMC.smc_conti_set_lookahead_mode(_ConnecNo, 0, 2, 2, 0.1, 1000);
            LTSMC.smc_set_vector_profile_unit(_ConnecNo, 0, 0,
                         m_MovePrm[nAxisarr[0]].VelM,
                          m_MovePrm[nAxisarr[0]].AccM,
                         m_MovePrm[nAxisarr[0]].DccM,
                         0
                         );
            LTSMC.smc_conti_set_lookahead_mode(_ConnecNo, 0, 2, 20, 0.1, m_MovePrm[nAxisarr[0]].AccM);
            var ret = LTSMC.smc_conti_open_list(_ConnecNo, 0, 2, axis);
            return true;
        }

        public override bool AddBufDelay(object objGroup, int nTime)
        {
            LTSMC.smc_conti_delay(_ConnecNo, 0, nTime, 1);
            return true;
        }

        public override bool AddBufIo(object objGroup, string strIoName, bool bVal, int nAxisIndexInGroup = 0)
        {
            var nIoIndex = IOMgr.GetInstace().GetOutputDic()[strIoName]._IoIndex;
            LTSMC.smc_conti_write_outbit(_ConnecNo, 0, (ushort)nIoIndex, (ushort)(bVal ? 1 : 0), 0);
            return true;
        }

        public override bool AddBufMove(object objGroup, BufMotionType type, int mode, int nAxisNum, double velHigh, double velLow, double[] Point1, double[] Point2)
        {
            if (type == BufMotionType.buf_Arc2dAbsCCW || type == BufMotionType.buf_Arc2dAbsCW)
            {
                var ret = LTSMC.smc_conti_arc_move_center_unit(_ConnecNo, 0, 2, axis, new double[] { Point2[0], Point2[1] }, Point1, 0, 0, 1, 1);
            }
            return true;
        }

        public override bool ClearBufMove(object objGroup)
        {
            LTSMC.smc_conti_close_list(_ConnecNo, 0);
            return true;
        }

        public override bool Close()
        {
            LTSMC.smc_board_close(_ConnecNo);
            return true;
        }

        public override int GetAxisActPos(int nAxisNo)
        {
            double pos = 0;
            LTSMC.smc_get_position_unit(_ConnecNo, (ushort)nAxisNo, ref pos);
            return (int)pos;
        }

        public override int GetAxisCmdPos(int nAxisNo)
        {
            double pos = 0;
            var ret = LTSMC.smc_get_target_position_unit(_ConnecNo, (ushort)nAxisNo, ref pos);
            return (int)pos;
        }

        public override int GetAxisPos(int nAxisNo)
        {
            double pos = 0;
            LTSMC.smc_get_position_unit(_ConnecNo, (ushort)nAxisNo, ref pos);
            return (int)pos;
        }

        public override long GetMotionIoState(int nAxisNo)
        {
            long lStandardIo = 0;
            uint status = LTSMC.smc_axis_io_status(_ConnecNo, (ushort)nAxisNo);
            if ((status & 1) == 1)
                lStandardIo |= (0x01 << 0);
            if ((status & (int)Math.Pow(2, 1)) == (int)Math.Pow(2, 1))
                lStandardIo |= (0x01 << 1);
            if ((status & (int)Math.Pow(2, 2)) == (int)Math.Pow(2, 2))
                lStandardIo |= (0x01 << 2);
            if ((status & (int)Math.Pow(2, 3)) == (int)Math.Pow(2, 3))
                lStandardIo |= (0x01 << 4);
            if (GetServoState(nAxisNo))
            {
                lStandardIo |= (0x01 << 7);
            }
            return lStandardIo;
        }

        public override bool GetServoState(int nAxisNo)
        {
            var ret = LTSMC.smc_read_sevon_pin(_ConnecNo, (ushort)nAxisNo);
            return ret == 0;
        }

        public override bool Home(int nAxisNo, int nParam)
        {
            //LTSMC.smc_set_pulse_outmode(_ConnecNo, (ushort)nAxisNo, 0);//设置脉冲模式
            //LTSMC.smc_set_home_pin_logic(_ConnecNo, (ushort)nAxisNo, 0, 0);//设置原点低电平有效
            LTSMC.smc_set_home_profile_unit(_ConnecNo, (ushort)nAxisNo, m_HomePrm[nAxisNo].VelL, m_HomePrm[nAxisNo].VelH, m_MovePrm[nAxisNo].AccL, m_MovePrm[nAxisNo].DccL);//设置起始速度、运行速度、停止速度、加速时间、减速时间
            LTSMC.smc_set_homemode(_ConnecNo, (ushort)nAxisNo, m_HomePrm[nAxisNo]._bHomeDir ? (ushort)1 : (ushort)0, 1, (ushort)m_HomePrm[nAxisNo]._nHomeMode, 0);//设置回零模式
            LTSMC.smc_set_home_position_unit(_ConnecNo, (ushort)nAxisNo, 0, 0);//设置偏移模式
            LTSMC.smc_home_move(_ConnecNo, (ushort)nAxisNo);//启动回零
            return true;
        }

        public override AxisState IsAxisNormalStop(int nAxisNo)
        {
            var ret = IsInpos(nAxisNo);
            return ret ? AxisState.Homeing : AxisState.NormalStop;
        }

        public override AxisState IsHomeNormalStop(int nAxisNo)
        {
            return IsInpos(nAxisNo) ? AxisState.Homeing : AxisState.NormalStop;
        }

        public override bool IsInpos(int nAxisNo)
        {
            return LTSMC.smc_check_done(_ConnecNo, (ushort)nAxisNo) == 0;
        }

        public override bool IsOpen()
        {
            return m_bOpen;
        }

        public override bool isOrgTrig(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override bool JogMove(int nAxisNo, bool bPositive, int bStart, double nSpeed)
        {
            //LTSMC.smc_set_pulse_outmode(_ConnecNo, (ushort)nAxisNo, 0);//设置脉冲模式
            //LTSMC.smc_set_s_profile(_ConnecNo, (ushort)nAxisNo, 0, 0.01);//设置S段时间（0-0.05s)
            short ret = 0;
            switch (nSpeed)
            {
                case 0:
                    ret = LTSMC.smc_set_profile_unit(_ConnecNo, (ushort)nAxisNo, 500, m_MovePrm[nAxisNo].VelH, m_MovePrm[nAxisNo].AccH, m_MovePrm[nAxisNo].DccH, 10000);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                    break;

                case 1:
                    ret = LTSMC.smc_set_profile_unit(_ConnecNo, (ushort)nAxisNo, 500, m_MovePrm[nAxisNo].VelM, m_MovePrm[nAxisNo].AccM, m_MovePrm[nAxisNo].DccM, 10000);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                    break;

                case 2:
                    ret = LTSMC.smc_set_profile_unit(_ConnecNo, (ushort)nAxisNo, 500, m_MovePrm[nAxisNo].VelL, m_MovePrm[nAxisNo].AccL, m_MovePrm[nAxisNo].DccL, 10000);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                    break;
            }
            LTSMC.smc_set_dec_stop_time(_ConnecNo, (ushort)nAxisNo, m_MovePrm[nAxisNo].DccL);//设置减速停止时间
            LTSMC.smc_vmove(_ConnecNo, (ushort)nAxisNo, (ushort)(bPositive ? 1 : 0));//连续运动
            return true;
        }

        public override bool Open()
        {
            _ConnecNo = (ushort)m_nCardIndex;
            short res = LTSMC.smc_board_init(_ConnecNo, 2, $"192.168.5.{m_nCardIndex}", 115200);//连接控制器
            if (res != 0)
            {
                return false;
            }
            string str = System.AppDomain.CurrentDomain.BaseDirectory;
            string strConfigPath = "Motion_" + m_nCardIndex + ".cfg";
            strConfigPath = str + strConfigPath;

            string ext = Path.GetExtension(strConfigPath);
            ushort type = 0;
            if (string.Compare(ext, ".bas", true) == 0)
            {
                type = 0;
            }
            else if (string.Compare(ext, ".g", true) == 0 || string.Compare(ext, ".nc", true) == 0)
            {
                type = 1;
            }
            else if (string.Compare(ext, ".cfg", true) == 0)
            {
                type = 2;
            }
            FileStream fs = File.Open(strConfigPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            str = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            byte[] name = Encoding.UTF8.GetBytes(Path.GetFileName(strConfigPath));
            LTSMC.smc_download_memfile(_ConnecNo, buffer, (uint)buffer.Length, name, type);
            m_bOpen = true;
            return true;
        }

        public override bool ReasetAxis(int nAxisNo)
        {
            LTSMC.smc_write_erc_pin(_ConnecNo, (ushort)nAxisNo, (ushort)0);
            return true;
        }

        public override bool RelativeMove(int nAxisNo, double nPos, double nSpeed)
        {
            //LTSMC.smc_set_pulse_outmode(_ConnecNo, (ushort)nAxisNo, 0);//设置脉冲模式
            //LTSMC.smc_set_s_profile(_ConnecNo, (ushort)nAxisNo, 0, 0.1);//设置S段时间（0-0.05s)
            //LTSMC.smc_set_profile_unit(_ConnecNo, (ushort)nAxisNo, 5, nSpeed, m_MovePrm[nAxisNo].AccM, m_MovePrm[nAxisNo].DccM, 1000);//设置起始速度、运行速度、停止速度、加速时间、减速时间
            //LTSMC.smc_set_dec_stop_time(_ConnecNo, (ushort)nAxisNo, m_MovePrm[nAxisNo].DccL);//设置减速停止时间
            short ret = 0;
            switch (nSpeed)
            {
                case 0:
                    ret = LTSMC.smc_set_profile_unit(_ConnecNo, (ushort)nAxisNo, 500, m_MovePrm[nAxisNo].VelH, m_MovePrm[nAxisNo].AccH, m_MovePrm[nAxisNo].DccH, 10000);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                    break;

                case 1:
                    ret = LTSMC.smc_set_profile_unit(_ConnecNo, (ushort)nAxisNo, 500, m_MovePrm[nAxisNo].VelM, m_MovePrm[nAxisNo].AccM, m_MovePrm[nAxisNo].DccM, 10000);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                    break;

                case 2:
                    ret = LTSMC.smc_set_profile_unit(_ConnecNo, (ushort)nAxisNo, 500, m_MovePrm[nAxisNo].VelL, m_MovePrm[nAxisNo].AccL, m_MovePrm[nAxisNo].DccL, 10000);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                    break;
            }

            if (ret != 0)
            {
                return false;
            }
            ret = LTSMC.smc_set_dec_stop_time(_ConnecNo, (ushort)nAxisNo, 0.1);//设置减速停止时间
            if (ret != 0)
            {
                return false;
            }
            LTSMC.smc_pmove_unit(_ConnecNo, (ushort)nAxisNo, nPos, 0);//连续运动
            return true;
        }

        public override bool ServoOff(short nAxisNo)
        {
            LTSMC.smc_write_sevon_pin(_ConnecNo, (ushort)nAxisNo, (ushort)1);
            return true;
        }

        public override bool ServoOn(short nAxisNo)
        {
            LTSMC.smc_write_sevon_pin(_ConnecNo, (ushort)nAxisNo, (ushort)0);
            return true;
        }

        public override bool SetActutalPos(int nAxisNo, double pos)
        {
            LTSMC.smc_reset_target_position_unit(_ConnecNo, (ushort)nAxisNo, 0);//位置清零
            return true;
        }

        public override bool SetCmdPos(int nAxisNo, double Pos)
        {
            LTSMC.smc_set_position_unit(_ConnecNo, (ushort)nAxisNo, 0);//位置清零
            return true;
        }

        public override bool StartBufMove(object objGroup)
        {
            LTSMC.smc_conti_start_list(_ConnecNo, 0);
            return true;
        }

        public override bool StopAxis(int nAxisNo)
        {
            LTSMC.smc_stop(_ConnecNo, (ushort)nAxisNo, 0);
            return true;
        }

        public override bool StopEmg(int nAxisNo)
        {
            LTSMC.smc_stop(_ConnecNo, (ushort)nAxisNo, 1);
            return true;
        }

        public override bool TranMMToPluse(int nAxisNo, ref double dSpeed, ref double acc, ref double dec)
        {
            throw new NotImplementedException();
        }

        public override bool TransMMToPluseForHomeParam(int nAxisNo, ref double dVelH, ref double dVelL, ref double dAccH, ref double dAccL, ref double dDecH, ref double dDecL)
        {
            throw new NotImplementedException();
        }
    }
}