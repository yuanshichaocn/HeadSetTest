using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using BaseDll;
using log4net;

namespace StationDemo
{
    public class WeightSerialPortParam
    {
        #region 属性
        /// <summary>
        ///串口号 
        /// </summary>
        public int m_nComNo = 8;
        /// <summary>
        ///串口定义名称 
        /// </summary>
        public string m_strName = "";
        /// <summary>
        ///波特率 
        /// </summary>
        public int m_nBaudRate = 9600;
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
        public string m_strStopBit = "1";
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


    public class Weighing
    {
        private ILog logger = LogManager.GetLogger("Weighing");
        #region GetInstance()

        private static Weighing _instance;

        public static Weighing GetInstance()
        {
            return _instance ?? (_instance = new Weighing());
        }
        #endregion
        SerialPort sp = new SerialPort();
        bool Connect;//通讯是否连接
        public Weighing()
        {
            //ResetReadBuffer();

        }
        object lockobj = new object();

        public delegate void ShowHandler(int nNum,int[] vals);
        public event ShowHandler eventShowHandle = null;
        public int CH1 { private set; get; }
        public int CH2 { private set; get; }
        public int CH3 { private set; get; }
        public int CH4 { private set; get; }

        public int CH5 { private set; get; }
        public int CH6 { private set; get; }
        public int CH7 { private set; get; }
        public int CH8 { private set; get; }
        public int[] ReadAData()
        {
            lock(lockobj)
            {
                byte[] readbytes = new byte[1024];
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                byte[] byteWrite = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x08, 0x44, 0x0C };
                _serialPort.Write(byteWrite, 0, byteWrite.Length);
                Thread.Sleep(150);
                int nRead = _serialPort.Read(readbytes, 0, 1024);
                int a1 = 0; int a2 = 0; int a3 = 0; int a4 = 0;
                int[] ch = new int[4];
                if (nRead == 21)
                {
                    int s = 0;
                    for (int i = 3; i < 19; i += 4)
                    {
                        a1 = readbytes[i];
                        a2 = readbytes[i + 1];
                        a3 = readbytes[i + 2];
                        a4 = readbytes[i + 3];
                        ch[s++] = a3 << 24 | a4 << 16 | a1 << 8 | a2;
                    }
                    for (int k = 0; k < 4; k++)
                    {
                        switch (k)
                        {
                            case 0:
                                CH1 = ch[k];
                                break;
                            case 1:
                                CH2 = ch[k];
                                break;
                            case 2:
                                CH3 = ch[k];
                                break;
                            case 3:
                                CH4 = ch[k];
                                break;
                        }
                    }
                    eventShowHandle?.Invoke(1, ch);
                    return ch;
                }
                return null;
            }
         
        }
        public int[] ReadBData()
        {
            lock (lockobj)
            {
                byte[] readbytes = new byte[1024];
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                byte[] byteWrite2 = new byte[] { 0x02, 0x03, 0x00, 0x00, 0x00, 0x08 };
                int crc = crc16_modbus(byteWrite2, 6);
                byte[] byteWrite = new byte[] { 0x02, 0x03, 0x00, 0x00, 0x00, 0x08, 0x44, 0x0C };
                byteWrite[6] = (byte)crc;
                byteWrite[7] = (byte)(crc >> 8);
                _serialPort.Write(byteWrite, 0, byteWrite.Length);
                Thread.Sleep(150);
                int nRead = _serialPort.Read(readbytes, 0, 1024);
                int a1 = 0; int a2 = 0; int a3 = 0; int a4 = 0;
                int[] ch = new int[4];

                if (nRead == 21)
                {
                    int s = 0;
                    for (int i = 3; i < 19; i += 4)
                    {
                        a1 = readbytes[i];
                        a2 = readbytes[i + 1];
                        a3 = readbytes[i + 2];
                        a4 = readbytes[i + 3];
                        ch[s++] = a3 << 24 | a4 << 16 | a1 << 8 | a2;

                    }
                    for (int k = 0; k < 4; k++)
                    {
                        switch (k)
                        {
                            case 0:
                                CH5 = ch[k];
                                break;
                            case 1:
                                CH6 = ch[k];
                                break;
                            case 2:
                                CH7 = ch[k];
                                break;
                            case 3:
                                CH8 = ch[k];
                                break;
                        }
                    }
                   
                    eventShowHandle?.Invoke(2, ch);
                    return ch;
                }
                return null;
            }
        }
        ~Weighing()
        {
            if (sp != null && sp.IsOpen)
            {
                sp.DiscardInBuffer();
                sp.Close();
                sp.Dispose();
            }
        }
        public void Update(int[] ch1, int[] ch2)
        {
            eventShowHandle?.Invoke(1, ch1);
            eventShowHandle?.Invoke(2, ch2);
        }

        WeightSerialPortParam weightSerialPortParam = new WeightSerialPortParam();

        public object ReadParam()
        {
            object obj = null;

            try
            {
                obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"\config\weight.xml", weightSerialPortParam.GetType());
                if (obj != null)
                    weightSerialPortParam = (WeightSerialPortParam)obj;

            }
            catch
            {
                obj = null;
            }
            if (obj == null)
            {
                MessageBox.Show("称重 串口 数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return obj;
        }
        public void Save()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"\config\weight.xml";
            AccessXmlSerializer.ObjectToXml(strPath, weightSerialPortParam);
        }
        SerialPort _serialPort;
        //模块串口初始化
        public bool InitWeightComport()
        {
            bool result = true;
            try
            {
                if (ReadParam() == null) 
                Save();
                if (_serialPort == null)
                    _serialPort = new SerialPort("COM" + weightSerialPortParam.m_nComNo.ToString())
                    {
                        BaudRate = weightSerialPortParam.m_nBaudRate,
                        DataBits = weightSerialPortParam.m_nDataBit,
                        Parity = (Parity)Enum.Parse(typeof(Parity), weightSerialPortParam.m_strPartiy),
                        StopBits = (StopBits)Enum.Parse(typeof(StopBits), weightSerialPortParam.m_strStopBit)
                    };
                if (_serialPort.IsOpen)
                    _serialPort.Close();
                _serialPort.ReadTimeout = 300;
                _serialPort.WriteTimeout = 300;
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                result = _serialPort.IsOpen;

                logger.Info("光源控制器开打串口成功");
                return result;
            }
            catch (Exception exp)
            {
                logger.Warn("光源控制器开打串口失败：" + exp.Message);
                throw new Exception(exp.Message);
            }
        }

        #region Open / Close Procedures
        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            //Ensure port isn't already opened:
            if (!sp.IsOpen)
            {
                //Assign desired settings to the serial port:
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = databits;
                sp.Parity = parity;
                sp.StopBits = stopBits;
                //These timeouts are default and cannot be editted through the class at this point:
                sp.ReadTimeout = 1000;
                sp.WriteTimeout = 1000;
                sp.DtrEnable = true;

                try
                {
                    sp.Open();
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
                return true;
            }
            return false;
        }
        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (sp.IsOpen)
            {
                try
                {
                    sp.Close();
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
                return true;
            }
            return false;
        }
        #endregion

        //字符串转换为字节数组
        public byte[] GetDataArray(string str)
        {
            string[] Command = str.Split(' ');
            byte[] RecieveData = new byte[Command.Length];
            for (int i = 0; i < Command.Length; i++)
            {
                RecieveData[i] = byte.Parse(Command[i], System.Globalization.NumberStyles.HexNumber);
            }
            return RecieveData;
        }

        //读模块1的4个通道寄存器
        public void FourChannels()
        {
            Modbus_ReadReg(1, 0, 8);
        }

        //读模块1的1号通道寄存器
        public void FirstChannels()
        {
            Modbus_ReadReg(1, 0, 2);
        }
        //读模块1的2号通道寄存器
        public void SecondChannels()
        {
            Modbus_ReadReg(1, 2, 2);
        }
        //读模块1的3号通道寄存器
        public void ThirdChannels()
        {
            Modbus_ReadReg(1, 4, 2);
        }

        //读模块1的4号通道寄存器
        public void FourthChannels()
        {
            Modbus_ReadReg(1, 6, 2);
        }

        //读寄存器
        public int Modbus_ReadReg(int slaveaddr, int startreg, int regnum)
        {
            byte[] modbusdata = new byte[0x100];
            int length = 0;
            if (((((slaveaddr > 0xfe) || (slaveaddr < 1)) || ((startreg < 0) || (startreg > 0x10000))) || (regnum < 0)) || (regnum > 120))
            {
                return -1;
            }
            modbusdata[length++] = (byte)(slaveaddr & 0xff);
            modbusdata[length++] = 3;
            modbusdata[length++] = (byte)((startreg >> 0x8) & 0xff);
            modbusdata[length++] = (byte)(startreg & 0xff);
            modbusdata[length++] = (byte)((regnum >> 8) & 0xff);
            modbusdata[length++] = (byte)(regnum & 0xff);
            int num2 = this.crc16_modbus(modbusdata, length);
            modbusdata[length++] = (byte)num2;
            modbusdata[length++] = (byte)(num2 >> 8);
            try
            {
                sp.Write(modbusdata, 0, length);
            }
            catch (Exception)
            {
                MessageBox.Show("端口发送调用异常，请检查端口");
                return -2;
            }
            return 0;
        }

        //CRC校验
        public int crc16_modbus(byte[] modbusdata, int length)
        {
            int num3 = 0xffff;
            for (int i = 0; i < length; i++)
            {
                num3 ^= modbusdata[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((num3 & 1) == 1)
                    {
                        num3 = (num3 >> 1) ^ 0xa001;
                    }
                    else
                    {
                        num3 = num3 >> 1;
                    }
                }
            }
            return num3;
        }

        //向寄存器写数据
        public int Modbus_WriteReg_06(int slaveaddr, int startreg, int Value)
        {
            byte[] modbusdata = new byte[0x100];
            int length = 0;
            if ((((slaveaddr > 0xfe) || (slaveaddr < 1)) || (startreg < 0)) || (startreg > 0x10000))
            {
                return -1;
            }
            modbusdata[length++] = (byte)(slaveaddr & 0xff);
            modbusdata[length++] = 6;
            modbusdata[length++] = (byte)((startreg >> 8) & 0xff);
            modbusdata[length++] = (byte)(startreg & 0xff);
            modbusdata[length++] = (byte)((Value >> 8) & 0xff);
            modbusdata[length++] = (byte)(Value & 0xff);
            int num2 = this.crc16_modbus(modbusdata, length);
            modbusdata[length++] = (byte)num2;
            modbusdata[length++] = (byte)(num2 >> 8);
            try
            {
                sp.Write(modbusdata, 0, length);
            }
            catch (Exception)
            {

                return -2;
            }
            return 0;
        }

        //写入多路寄存器
        public int Modbus_WriteReg_16(int slaveaddr, int startreg, int regnum, byte[] value)
        {
            byte[] array = new byte[256];
            int num = 0;
            bool flag = slaveaddr > 254 || slaveaddr < 1 || regnum < 0 || regnum > 120 || startreg < 0 || startreg > 65536;
            int result;
            if (flag)
            {
                result = -1;
            }
            else
            {
                array[num++] = (byte)(slaveaddr & 255);
                array[num++] = 16;
                array[num++] = (byte)(startreg >> 8 & 255);
                array[num++] = (byte)(startreg & 255);
                array[num++] = (byte)(regnum >> 8 & 255);
                array[num++] = (byte)(regnum & 255);
                array[num++] = (byte)(regnum * 2 & 255);
                for (int i = 0; i < regnum * 2; i++)
                {
                    array[num++] = value[i];
                }
                int num2 = this.crc16_modbus(array, num);
                array[num++] = (byte)num2;
                array[num++] = (byte)(num2 >> 8);
                try
                {
                    sp.Write(array, 0, num);
                }
                catch (Exception)
                {
                    MessageBox.Show("端口发送调用异常，请检查端口");
                    result = -2;
                    return result;
                }
                result = 0;
            }
            return result;
        }

        //接收串口数据
        public void RecieveSerialData(ref string str)
        {
            int num = 0, remainNum = 0;
            Stopwatch monitor = new Stopwatch();
            while (true)
            {
                Thread.Sleep(10);
                string strtemp = sp.ReadByte().ToString("X2");
                num++;
                str += strtemp;
                str += " ";
                if ((strtemp == "01" || strtemp == "02") && num == 0)
                {
                    monitor.Restart();
                }
                if (num == 3)
                {
                    if (strtemp == "10")
                    {
                        remainNum = 18;
                    }
                    else
                    {
                        remainNum = 5;
                    }
                    for (int i = 0; i < remainNum; i++)
                    {
                        strtemp = sp.ReadByte().ToString("X2");
                        str += strtemp;

                        if (i == remainNum - 1)
                        {
                            return;
                        }
                        str += " ";
                    }
                }
                if (monitor.Elapsed.TotalMilliseconds > sp.ReadTimeout)
                {
                    str = "";
                    return;
                }
            }

        }

        //模块四个通道重量数据转换
        public void Weight(string str, out int pressure0, out int pressure1, out int pressure2, out int pressure3)
        {
            pressure0 = 0;
            pressure1 = 0;
            pressure2 = 0;
            pressure3 = 0;
            byte[] Recievedata = GetDataArray(str);
            if (Recievedata.Length == 21)
            {
                //通道1
                byte[] temp0 = new byte[4];
                temp0[0] = Recievedata[4];
                temp0[1] = Recievedata[3];
                temp0[2] = Recievedata[6];
                temp0[3] = Recievedata[5];
                pressure0 = BitConverter.ToInt32(temp0, 0);

                //通道2
                byte[] temp1 = new byte[4];
                temp1[0] = Recievedata[8];
                temp1[1] = Recievedata[7];
                temp1[2] = Recievedata[10];
                temp1[3] = Recievedata[9];
                pressure1 = BitConverter.ToInt32(temp1, 0);

                //通道3
                byte[] temp2 = new byte[4];
                temp2[0] = Recievedata[12];
                temp2[1] = Recievedata[11];
                temp2[2] = Recievedata[14];
                temp2[3] = Recievedata[13];
                pressure2 = BitConverter.ToInt32(temp2, 0);

                //通道4
                byte[] temp3 = new byte[4];
                temp3[0] = Recievedata[16];
                temp3[1] = Recievedata[15];
                temp3[2] = Recievedata[18];
                temp3[3] = Recievedata[17];
                pressure3 = BitConverter.ToInt32(temp3, 0);
            }
            return;
        }
    }
}