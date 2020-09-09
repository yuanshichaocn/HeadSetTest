using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using gts;
using System.Diagnostics;
using Advantech.Motion;
using log4net;
using System.Windows.Forms;

namespace MotionIoLib
{

    public class IoCtrl_Adlink_7230 : IoCtrl
    {

        IntPtr[] m_Axishand = null;
        IntPtr m_devPtr = new IntPtr();
        public short m_dev;
        public IoCtrl_Adlink_7230(int nIndex, ulong nCardNo)
            : base(nIndex, nCardNo)
        {
            m_strCardName = "IoCtrl_IO_Adlink_7230";


        }

        public override bool Init()
        {
            m_bOpen = false;
            m_dev = DASK.Register_Card(DASK.PCI_7230,(ushort) m_nCardNo);
            if (m_dev == 0)
            {
                m_bOpen = true;
            }
            return m_bOpen;
        }

        /// <summary>
        ///释放IO卡 
        /// </summary>
        public override void DeInit()
        {
            short ret;
            if (m_dev >= 0)
            {
                ret = DASK.Release_Card((ushort)m_dev);
            }
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
            short ret;
            uint int_value;
            ret = DASK.DI_ReadPort((ushort)m_dev, 0, out int_value);
            if (ret < 0)
            {
                return false;//获取失败
            }
            if ((int_value & (1 << nIndex)) == 0L)
            {
                return false;
            }
            else
            {
                return true;
            }
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
            short ret;
            uint int_value;
            ret = DASK.DO_ReadPort((ushort)m_dev, 0, out int_value);
            if (ret < 0)
            {
                return false;//获取失败
            }
            if ((int_value & (1 << nIndex)) == 0L)
            {
                return false;
            }
            else
            {
                return true;
            }
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
            short ret;
            uint int_value;
            int num = DASK.DO_ReadPort((ushort)m_dev, 0, out int_value);
            if(num!=0)
            {
                return false;
            }
            uint result = 0;
            if (bBit)
            {
                result = int_value | (uint)(1 << nIndex);
            }
            else
            {
                result = int_value &= ~(uint)(1 << nIndex);
            }  
            ret = DASK.DO_WritePort((ushort)m_dev, 0, result);
            if (ret < 0)
            {
                return false;//获取失败
            }
            return true;
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

            return false;
        }
        public override bool InStopState(int nIndex, bool bState)
        {
            return false;
        }


        public override bool InStopDisenable(int nIndex)
        {
            return false;
        }

    }
}