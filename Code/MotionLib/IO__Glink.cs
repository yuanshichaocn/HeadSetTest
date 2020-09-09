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

    public class IoCtrl_Glink_16I_16O : IoCtrl
    {

        public IoCtrl_Glink_16I_16O(int nIndex, ulong nCardNo)
            :base( nIndex,  nCardNo)
        {
            m_strCardName = "IoCtrl_Glink";
        }
        private  const int nInBytes = 2;
        private  const int nOutBytes = 2;
        private static bool bOpen = false;
        public override bool Init()
        {
            m_bOpen = bOpen;
            if (bOpen)
            {
                logger.Info($"{ m_nIndex}号{m_strCardName} 初始化成功");
                return bOpen;
            }
           
               
            short rtn = glink.GT_GLinkInit(0);
            if (rtn == 0 )
            {
                logger.Info($"{ m_nIndex}号{m_strCardName} 初始化成功");
                return bOpen=m_bOpen = true;
            }
            else
            {
                MessageBox.Show($"{ m_nIndex}号{m_strCardName} 初始化失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                logger.Info($"{ m_nIndex}号{m_strCardName} 初始化失败");
                return false;
            }
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
            short srtn= glink.GT_GetGLinkDiBit( (short)m_nCardNo,(short) nBitIndex ,out byte pDi);
            if (pDi==1)
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

            byte pData = 0;
            short srtn = 0;
            int  nOffset = nBitIndex / 8;
            srtn = glink.GT_GetGLinkDo( (short)m_nCardNo, (ushort)nOffset, ref  pData, 1);
            int nBitOffset = nBitIndex % 8;
            if ( (pData& (0x01<< nBitOffset)) != 0 && srtn==0)
                return true;
            else
                return false;
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
            byte pData = 0;
            short srtn = glink.GT_SetGLinkDoBit((short)m_nCardNo,(short) nIoIndex, (byte)(bBit? 1: 0));
            if (srtn == 0)
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