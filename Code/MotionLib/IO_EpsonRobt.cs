using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using gts;
using System.Diagnostics;
using Advantech.Motion;
using EpsonRobot;
using BaseDll;

namespace MotionIoLib
{

    public class IoCtrl_EpsonRobot : IoCtrl
    {
        
        IntPtr[] m_Axishand = null;
        IntPtr m_devPtr = new IntPtr();

        public IoCtrl_EpsonRobot(int nIndex, ulong nCardNo)
            :base( nIndex,  nCardNo)
        {
            m_strCardName = "IoCtrl_EpsonRobot";


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
            if (!ScaraRobot.GetInstance().IsInit)
                return false;
            UInt32 t= ScaraRobot.GetInstance().dIn;
            return BitOperat.GetBit32(ScaraRobot.GetInstance().dIn, nIndex) == 1;
           // return true;
        }

        /// <summary>
        ///按位获取输出信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            if (!ScaraRobot.GetInstance().IsInit)
                return false;
            return 1== BitOperat.GetBit32(ScaraRobot.GetInstance().dOut, (byte)nIndex);
          
        }
        /// <summary>
        /// 按位输出信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="bBit"></param>
        /// <returns></returns>
        public override bool WriteIoBit(int nIndex, bool bBit)
        {
            if (!ScaraRobot.GetInstance().IsInit)
                return false;
            ScaraRobot.GetInstance().SetOutput(nIndex, bBit);
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
        public override bool InStopDisenable(int nIndex)
        {
            return false;
        }

    }
}