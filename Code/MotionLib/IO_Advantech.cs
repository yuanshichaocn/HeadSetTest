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

    public class IoCtrl_Advantech : IoCtrl
    {
        
        IntPtr[] m_Axishand = null;
        IntPtr m_devPtr = new IntPtr();

        public IoCtrl_Advantech(int nIndex, ulong nCardNo)
            :base( nIndex,  nCardNo)
        {
            m_strCardName = "IoCtrl_Advantech";


        }
 
        public override bool Init()
        {
            m_bOpen = false;
            uint Result = Motion.mAcm_DevOpen((uint)m_nCardNo, ref m_devPtr);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                logger.Warn(string.Format("{0} 号卡{1} 打开失败 请检查配置", m_nCardNo,m_strCardName));
                MessageBox.Show(string.Format("{0} 号卡{1} 打开失败 请检查配置", m_nCardNo, m_strCardName), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            uint AxesPerDev = 0;
            Result = Motion.mAcm_GetU32Property(m_devPtr, (uint)PropertyID.FT_DevAxesCount, ref AxesPerDev);
            if (Result != (uint)ErrorCode.SUCCESS || AxesPerDev <= 0)
            {
                logger.Warn(string.Format("{0} 号卡{1} 打开失败 请检查配置", m_nCardNo, m_strCardName));
                MessageBox.Show(string.Format("{0} 号卡{1} 打开失败 请检查配置", m_nCardNo, m_strCardName), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                m_Axishand = new IntPtr[AxesPerDev];
                for (int i = 0; i < AxesPerDev; i++)
                {
                    Result = Motion.mAcm_AxOpen(m_devPtr, (UInt16)i, ref m_Axishand[i]);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        logger.Warn(string.Format("{0} 号卡{1} 打开轴失败 请检查配置", m_nCardNo, m_strCardName));
                        MessageBox.Show(string.Format("{0} 号卡{1} 打开轴失败 请检查配置", m_nCardNo, m_strCardName), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                m_bOpen = true;
                return true;
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
           
            byte bitData = 0;
            uint Result = Motion.mAcm_AxDiGetBit(m_Axishand[nAxisIndex], (ushort)nBitIndex, ref bitData);
            if (Result != (uint)ErrorCode.SUCCESS)
                return false;
            if (bitData == 0)
                return false;
            else
                return true;
            
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
            byte bitData = 0;
            Motion.mAcm_AxDoGetBit(m_Axishand[nAxisIndex],(ushort)nBitIndex, ref  bitData);
            if(bitData==0)
               return false;
            else
                return true;
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
            byte byteWrite = bBit ? (byte)1: (byte)0;
          // Motion.mAcm_AxDoSetBit(m_Axishand[nAxisNo], (ushort)nIoIndex, byteWrite);
            return 0 == Motion.mAcm_AxDoSetBit(m_Axishand[nAxisNo], (ushort)nIoIndex, byteWrite); 
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
            int nAxisNo = (nIndex & 0xff00) >> 8;
            int nIoIndex = nIndex & 0xff;
            uint AxesPerDev = (uint)Advantech.Motion.IN1StopEnable.STOP_ENABLE;
            uint Result = 0;
            switch (nIoIndex)
            {
                case 0:
                    AxesPerDev = (uint)Advantech.Motion.IN1StopEnable.STOP_ENABLE;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN1StopEnable, AxesPerDev);
                    break;
                case 1:
                    AxesPerDev = (uint)Advantech.Motion.IN2StopEnable.STOP_ENABLE;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN2StopEnable, AxesPerDev);
                    break;
                case 2:
                    AxesPerDev = (uint)Advantech.Motion.IN4StopEnable.STOP_ENABLE;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN4StopEnable, AxesPerDev);
                    break;
                case 3:
                    AxesPerDev = (uint)Advantech.Motion.IN5StopEnable.STOP_ENABLE;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN5StopEnable, AxesPerDev);
                    break;

            }  
            return Result==(uint)ErrorCode.SUCCESS;
        }
        public override bool InStopState(int nIndex,bool bState)
        {
            int nAxisNo = (nIndex & 0xff00) >> 8;
            int nIoIndex = nIndex & 0xff;
            uint AxesPerDev = (uint)Advantech.Motion.IN1StopLogic.STOP_ACT_HIGH;
            uint Result = 0;
            switch (nIoIndex)
            {
                case 0:
                    if(bState)
                        AxesPerDev = (uint)Advantech.Motion.IN1StopLogic.STOP_ACT_HIGH;
                    else
                        AxesPerDev = (uint)Advantech.Motion.IN1StopLogic.STOP_ACT_LOW;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN1StopLogic, AxesPerDev);
                    break;
                case 1:
                    if (bState)
                        AxesPerDev = (uint)Advantech.Motion.IN2StopLogic.STOP_ACT_HIGH;
                    else
                        AxesPerDev = (uint)Advantech.Motion.IN2StopLogic.STOP_ACT_LOW;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN1StopLogic, AxesPerDev);
                    break;
                case 2:
                    if (bState)
                        AxesPerDev = (uint)Advantech.Motion.IN4StopLogic.STOP_ACT_HIGH;
                    else
                        AxesPerDev = (uint)Advantech.Motion.IN4StopLogic.STOP_ACT_LOW;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN1StopLogic, AxesPerDev);
                    break;
                case 3:
                    if (bState)
                        AxesPerDev = (uint)Advantech.Motion.IN5StopLogic.STOP_ACT_HIGH;
                    else
                        AxesPerDev = (uint)Advantech.Motion.IN5StopLogic.STOP_ACT_LOW;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN1StopLogic, AxesPerDev);
                    break;

            }
            return Result == (uint)ErrorCode.SUCCESS;
        }


        public override bool InStopDisenable(int nIndex)
        {
            int nAxisNo = (nIndex & 0xff00) >> 8;
            int nIoIndex = nIndex & 0xff;
            uint AxesPerDev =0;
            uint Result = 0;
            switch (nIoIndex)
            {
                case 0:
                    AxesPerDev = (uint)Advantech.Motion.IN1StopEnable.STOP_DISABLE;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN1StopEnable, AxesPerDev);
                    break;
                case 1:
                    AxesPerDev = (uint)Advantech.Motion.IN2StopEnable.STOP_DISABLE;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN2StopEnable, AxesPerDev);
                    break;
                case 2:
                    AxesPerDev = (uint)Advantech.Motion.IN4StopEnable.STOP_DISABLE;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN4StopEnable, AxesPerDev);
                    break;
                case 3:
                    AxesPerDev = (uint)Advantech.Motion.IN5StopEnable.STOP_DISABLE;
                    Result = Motion.mAcm_SetU32Property(m_Axishand[nAxisNo], (uint)PropertyID.CFG_AxIN5StopEnable, AxesPerDev);
                    break;

            }

            return Result == (uint)ErrorCode.SUCCESS;
        }

    }
}