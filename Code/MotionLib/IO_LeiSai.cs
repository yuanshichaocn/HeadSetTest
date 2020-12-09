using LS;
using System;
using System.Windows.Forms;

namespace MotionIoLib
{
    public class IoCtrl_LeiSai : IoCtrl
    {
        private IntPtr[] m_Axishand = null;
        private IntPtr m_devPtr = new IntPtr();

        public IoCtrl_LeiSai(int nIndex, ulong nCardNo)
            : base(nIndex, nCardNo)
        {
            m_strCardName = "IoCtrl_LeiSai";
        }

        public override bool Init()
        {
            IOC0640.ioc_board_close();
            m_bOpen = false;
            int nCard = 0;
            nCard = IOC0640.ioc_board_init();
            if (nCard <= 0)//控制卡初始化
            {
                logger.Warn(string.Format("{0} 号卡{1} 打开失败 请检查配置", m_nCardNo, m_strCardName));
                MessageBox.Show(string.Format("{0} 号卡{1} 打开失败 请检查配置", m_nCardNo, m_strCardName), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            m_bOpen = true;
            return true;
        }

        /// <summary>
        ///释放IO卡
        /// </summary>
        public override void DeInit()
        {
            IOC0640.ioc_board_close();
        }

        /// <summary>
        ///获取卡所有的输入信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool ReadIOIn(ref int nData)
        {
            return true;
        }

        /// <summary>
        ///获取卡所有的输出信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool ReadIOOut(ref int nData)
        {
            return true;
        }

        /// <summary>
        ///按位获取输入信号
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public override bool ReadIoInBit(int nIndex)
        {
            if (!m_bOpen)
                return false;
            int nAxisIndex = (nIndex & 0xff00) >> 8;
            int nBitIndex = nIndex & 0x00ff;

            int a = IOC0640.ioc_read_inbit((ushort)m_nCardNo, (ushort)nIndex);
            return a == 0;
        }

        /// <summary>
        ///按位获取输出信号
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            if (!m_bOpen)
                return false;
            int a = IOC0640.ioc_read_outbit((ushort)m_nCardNo, (ushort)nIndex);
            return a == 0;
        }

        /// <summary>
        /// 按位输出信号
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="bBit"></param>
        /// <returns></returns>
        public override bool WriteIoBit(int nIndex, bool bBit)
        {
            if (!m_bOpen)
                return false;
            uint a = IOC0640.ioc_write_outbit((ushort)m_nCardNo, (ushort)nIndex, bBit ? 0 : 1);
            return a == 0;
        }

        /// <summary>
        /// 输出整个卡的信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool WriteIo(int nData)
        {
            return true;
        }

        public override bool InStopEnable(int nIndex)
        {
            return true;
        }

        public override bool InStopState(int nIndex, bool bState)
        {
            return true;
        }

        public override bool InStopDisenable(int nIndex)
        {
            return true;
        }
    }
}