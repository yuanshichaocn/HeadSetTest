using Leadshine;
using System;

namespace MotionIoLib
{
    public class IoCtrl_LTSMC : IoCtrl
    {
        private ushort _ConnecNo = 0;

        public IoCtrl_LTSMC(int nIndex, ulong nCardNo)
            : base(nIndex, nCardNo)
        {
        }

        public override void DeInit()
        {
        }

        public override bool Init()
        {
            m_bOpen = false;
            _ConnecNo = (ushort)m_nCardNo;
            //short res = LTSMC.smc_board_init((ushort)_ConnecNo, 2, $"192.168.5.{m_nCardNo}", 115200);//连接控制器
            //if (res != 0)
            //{
            //    return false;
            //}
            m_bOpen = true;
            return true;
        }

        public override bool InStopDisenable(int nIndex)
        {
            throw new NotImplementedException();
        }

        public override bool InStopEnable(int nIndex)
        {
            throw new NotImplementedException();
        }

        public override bool ReadIOIn(ref int nData)
        {
            nData = (int)LTSMC.smc_read_inport(_ConnecNo, 0);
            return true;
        }

        public override bool ReadIoInBit(int nIndex)
        {
            //输入口
            short n = LTSMC.smc_read_inbit(_ConnecNo, (ushort)nIndex);
            return n == 0;
        }

        public override bool ReadIOOut(ref int nData)
        {
            throw new NotImplementedException();
        }

        public override bool ReadIoOutBit(int nIndex)
        {
            //输入口
            short n = LTSMC.smc_read_outbit(_ConnecNo, (ushort)nIndex);
            return n == 0;
        }

        public override bool WriteIo(int nData)
        {
            throw new NotImplementedException();
        }

        public override bool WriteIoBit(int nIndex, bool bBit)
        {
            LTSMC.smc_write_outbit(_ConnecNo, (ushort)nIndex, bBit ? (ushort)0 : (ushort)1);
            return true;
        }
    }
}