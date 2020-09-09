using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GTN;
using System.Diagnostics;
using Advantech.Motion;
using log4net;
using System.Windows.Forms;

namespace MotionIoLib
{

    public class IoCtrl_Gen_6AxisModle : IoCtrl
    {

        private int nInNum = 12;
        private int nOutNum = 8;
        public IoCtrl_Gen_6AxisModle(int nIndex, ulong nCardNo)
            :base( nIndex,  nCardNo)
        {
            m_strCardName = "IoCtrl_Gen_6AxisModle";

        }
 
        public override bool Init()
        {
            short rtn = mc.GTN_IsEcatReady(1, out short pStatus);
            if (rtn == 0 && pStatus == 1)
            {
                logger.Info($"{ m_nIndex}号{m_strCardName} 初始化成功");
                return m_bOpen = true;
            }
            else
            {
                MessageBox.Show($"{ m_nIndex}号{m_strCardName} 初始化失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                logger.Info($"{ m_nIndex}号{m_strCardName} 初始化失败");
                return false;
            }
            //for (short i = 1; i <= nOutNum; i++)
            //{
            //    rtn |= mc.GTN_RelateEcatSlaveToMcGpoBit(1, i, 0, 1, i, 0);
            //}
        }

        /// <summary>
        ///释放IO卡 
        /// </summary>
        public override void DeInit()
        {
          
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
            int nAxisIndex = (nIndex & 0xff00)>>8;
            int nBitIndex = nIndex & 0x00ff;
            if (nBitIndex > 12)
                return false;
            int  nCore = m_nCardNo > 32 ? 2 : 1;
           short srtn= mc.GTN_GetEcatAxisDI((short)nCore, (short)m_nCardNo, out uint pDi);
            if (((pDi & (0x01 << (nBitIndex + 18))) == 0)&& srtn==0)
                return true;
            else
                return false;
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
            int nAxisIndex = (nIndex & 0xff00) >> 8;
            int nBitIndex = nIndex & 0x00ff;
            int nCore = m_nCardNo > 32 ? 2 : 1;
            short srtn =  mc.GTN_GetEcatAxisDOBit((short)nCore, (short)m_nCardNo, (short)nBitIndex, out byte DobitValue);
            if (DobitValue == 0 && srtn == 0)
                return true;
            else
                return false;
            
            //short srtn = mc.GTN_GetEcatAxisDO((short)nCore, (short)m_nCardNo, out uint pDo);
            //if (((pDo & (0x01 << (nBitIndex + 0))) != 0) && srtn == 0)
            //    return true;
            //else
            //    return false;
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
            int nAxisNo = (nIndex & 0xff00) >> 8;
            int nIoIndex = nIndex & 0xff;
            short rtn = 0;
            int nCore = m_nCardNo > 32 ? 2 : 1;
            short srtn =  mc.GTN_SetEcatAxisDOBit((short)nCore, (short)m_nCardNo, (short)nIoIndex,(byte)( bBit?0:1));
            if ( srtn == 0)
                return true;
            else
                return false;
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
        public override bool InStopState(int nIndex,bool bState)
        {
            return true;
        }


        public override bool InStopDisenable(int nIndex)
        {
            return true;
        }

    }
}