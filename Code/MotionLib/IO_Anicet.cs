using System;
using log4net;
using System.IO.Ports;
using BaseDll;
using System.Windows.Forms;
using Modbus;
using Modbus.Device;

namespace MotionIoLib
{
    public class AnicetSerialPortParam
    {
#region 属性
        /// <summary>
        ///串口号 
        /// </summary>
        public int m_nComNo = 7;
        /// <summary>
        ///串口定义名称 
        /// </summary>
        public string m_strName = "";
        /// <summary>
        ///波特率 
        /// </summary>
        public int m_nBaudRate = 19200;
        /// <summary>
        ///数据位 
        /// </summary>
        public int m_nDataBit = 8;
        /// <summary>
        ///校验位 
        /// </summary>
        public string m_strPartiy = "None";
        /// <summary>
        ///停止位 
        /// </summary>
        public string m_strStopBit = "2";
        /// <summary>
        ///流控制 
        /// </summary>
        public string m_strFlowCtrl = "None";
        /// <summary>
        ///超时时间 
        /// </summary>
        public int m_nTime = 1000;
        /// <summary>
        ///缓冲区大小 
        /// </summary>
        public int m_nBufferSzie = 4096;
        /// <summary>
        ///命令分隔符标志 
        /// </summary>
        public string m_strLineFlag = "CRLF";
        /// <summary>
        ///命令分隔符 
        /// </summary>
        private string m_strLine;
#endregion

    }

    public class IoCtrl_Anicet : IoCtrl
    {
        [NonSerialized]
        private static SerialPort _serialPort = null;
        [NonSerialized]
        private readonly ILog _logger = LogManager.GetLogger(nameof(IoCtrl_Anicet));
        [NonSerialized]
        private readonly object _disposingLock = new object();
        [NonSerialized]
        private static readonly object _syncRoot = new object();
        [NonSerialized]
        private IModbusSerialMaster _master;

        static AnicetSerialPortParam anicetParam = new AnicetSerialPortParam();

        public IoCtrl_Anicet(int nIndex, ulong nCardNo)
            : base(nIndex, nCardNo)
        {
            m_strCardName = "IoCtrl_Anicet";
        }
        object ReadParam()
        {
            object obj = null;
            try
            {
                obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"\config\Anicet.xml", anicetParam.GetType());
                anicetParam = (AnicetSerialPortParam)obj;
            }
            catch
            {
                obj = null;
            }
            if (obj == null)
            {
                MessageBox.Show("Anicet IO 串口 数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return obj;
        }
        void Save()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"\config\Anicet.xml";
            AccessXmlSerializer.ObjectToXml(strPath, anicetParam);
        }

        public override bool Init()
        {
            // m_bOpen = m_comLink ==null ? false : m_comLink.IsOpen();
            if (ReadParam() == null)
            {
                Save();
            }
            try
            {
                if (_serialPort == null)
                    _serialPort = new SerialPort("COM" + anicetParam.m_nComNo.ToString())
                    {
                        BaudRate = anicetParam.m_nBaudRate,
                        DataBits = anicetParam.m_nDataBit,
                        Parity = (Parity)Enum.Parse(typeof(Parity), anicetParam.m_strPartiy),
                        StopBits = (StopBits)Enum.Parse(typeof(StopBits), anicetParam.m_strStopBit)
                    };
                // if (_serialPort.IsOpen)
                //     _serialPort.Close();
                _serialPort.ReadTimeout = 300;
                _serialPort.WriteTimeout = 300;
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                m_bOpen = _serialPort.IsOpen;
                _master = ModbusSerialMaster.CreateRtu(_serialPort);
                m_bOpen = true;
            }
            catch (Exception ex)
            {
                m_bOpen = false;
                logger.Warn(string.Format("{0} 号卡{1} 打开失败 请检查配置", m_nCardNo, m_strCardName));
                MessageBox.Show(string.Format("{0} 号卡{1} 打开失败 请检查配置", m_nCardNo, m_strCardName), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // throw new Exception("IO控制器串口初始化失败," + ex.Message, ex);
            }
            return m_bOpen;
        }

        /// <summary>
        ///释放IO卡 
        /// </summary>
        public override void DeInit()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
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
            int nAxisIndex = (nIndex & 0xff00) >> 8;
            int nBitIndex = nIndex & 0x00ff;
            byte slaveAddress = (byte)nAxisIndex;
            ushort coilAddress = (ushort)(nBitIndex + 16);
            bool brtn = false;
            try
            {
                lock (_syncRoot)
                {
                    brtn= _master.ReadCoils(slaveAddress, coilAddress, 1)[0];
                }
                return brtn;   
            }
            catch (Exception ex)
            {
                //if (_serialPort.IsOpen)
                //    _serialPort.Close();
                //if (!_serialPort.IsOpen)
                //    _serialPort.Open();
                //_master.Dispose();
                //_master = ModbusSerialMaster.CreateRtu(_serialPort);
                _logger.Warn(ex.Message);
                return false;
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
            int nAxisIndex = (nIndex & 0xff00) >> 8;
            int nBitIndex = nIndex & 0x00ff;
            byte bitData = 0;
            byte slaveAddress = (byte)m_nCardNo;
            ushort coilAddress = (ushort)nBitIndex;
            try
            {
                lock (_syncRoot)
                    return _master.ReadCoils(slaveAddress, (ushort)(coilAddress + 80), 1)[0];
            }
            catch (Exception ex)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();
                if(!_serialPort.IsOpen)
                _serialPort.Open();
                _master.Dispose();
                _master= ModbusSerialMaster.CreateRtu(_serialPort);
                _logger.Warn(ex.Message);
                return false;
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
            int nAxisNo = (nIndex & 0xff00) >> 8;
            int nIoIndex = nIndex & 0xff;
            byte byteWrite = bBit ? (byte)1 : (byte)0;
            // Motion.mAcm_AxDoSetBit(m_Axishand[nAxisNo], (ushort)nIoIndex, byteWrite);
            byte slaveAddress = (byte)m_nCardNo;
            ushort coilAddress = (ushort)(nIoIndex + 80);
            try
            {
                lock (_syncRoot)
                    _master.WriteSingleCoil(slaveAddress, coilAddress, bBit);
                return true;
            }
            catch (Exception ex)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                _master.Dispose();
                _master = ModbusSerialMaster.CreateRtu(_serialPort);
                _logger.Warn(ex.Message);
                return false;
            }
        }
        public override bool WriteIo(int nData)
        {
            return true;
        }

        public override bool InStopEnable(int nIndex)
        {
            return true;
        }
        public override bool InStopDisenable(int nIndex)
        {
            return true;
        }
    }



}


//public bool ReadIO(string address)
//{
//    if (IOInit)
//        return ReadIO(address.Split('_')[0].ToByte(), (ushort)(address.Split('_')[1].ToUshort() + 16));
//    else
//        return false;
//}

//public bool ReadIO(byte slaveAddress, ushort coilAddress)
//{
//    lock (_syncRoot)
//        return _master.ReadCoils(slaveAddress, coilAddress, 1)[0];
//}
//public bool ReadDO(string address)
//{
//    if (IOInit)
//        return ReadDO(address.Split('_')[0].ToByte(), (ushort)(address.Split('_')[1].ToUshort()));
//    else
//        return false;
//}
//public bool ReadDO(byte slaveAddress, ushort coilAddress)
//{
//    lock (_syncRoot)
//        return _master.ReadCoils(slaveAddress, (ushort)(coilAddress + 80), 1)[0];
//}

//public void WriteIO(string address, bool value)
//{
//    if (IOInit)
//        WriteIO(address.Split('_')[0].ToByte(), (ushort)(address.Split('_')[1].ToUshort() + 80), value);
//    else
//        return;
//}

//public void WriteIO(byte slaveAddress, ushort coilAddress, bool value)
//{
//    try
//    {
//        lock (_syncRoot)
//            _master.WriteSingleCoil(slaveAddress, coilAddress, value);
//    }
//    catch (Exception ex)
//    {
//        _logger.Warn(ex.Message);
//    }
//}