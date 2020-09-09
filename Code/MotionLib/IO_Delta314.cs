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
using PCI_M314;

namespace MotionIoLib
{

    public class IoCtrl_Delta314 : IoCtrl
    {

        IntPtr[] m_Axishand = null;
        IntPtr m_devPtr = new IntPtr();
        public short m_dev;
        public IoCtrl_Delta314(int nIndex, ulong nCardNo)
            : base(nIndex, nCardNo)
        {
            m_strCardName = "IoCtrl_IO_Delta314";


        }

        public override bool Init()
        {

               return true;

        }

        /// <summary>
        ///释放IO卡 
        /// </summary>
        public override void DeInit()
        {
            ;
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
            short rtn = 0;
            ushort a = 0;
            rtn |= CPCI_M314.CS_m314_get_dio_input((ushort)m_nCardNo, (ushort)nIndex,ref a);
            return a == 1;
        }

        /// <summary>
        ///按位获取输出信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            short rtn = 0;
            ushort a = 0;
            rtn |= CPCI_M314.CS_m314_get_dio_output((ushort)m_nCardNo, (ushort)nIndex, ref a);
            return a == 1;
        }


        /// <summary>
        /// 按位输出信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="bBit"></param>
        /// <returns></returns>
        public override bool WriteIoBit(int nIndex, bool bBit)
        {
            short rtn = 0;
            ushort a = 0;
            if(bBit)
            {
                a = 1;
            }
            rtn |= CPCI_M314.CS_m314_set_dio_output((ushort)m_nCardNo, (ushort)nIndex,  a);
            return a == 1;
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