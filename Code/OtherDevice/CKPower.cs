using BaseDll;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherDevice
{
    public class CKPower
    {
        private SerialPortOperation cKPower = null;
        private string cKPowerReceive = "";
        private string err = "";

        private void InitCKPower(string comPortName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            try
            {
                if (cKPower == null)
                {
                    cKPower = new SerialPortOperation(comPortName, baudRate, parity, dataBits, stopBits);
                    cKPower.DataReceived += new SerialPortOperation.SerialPortDataReceiveEventArgs(CK_DataReceived);
                }

            }
            catch (Exception ex)
            {
                err = $"串口{comPortName}打开异常,{ex.ToString()}";
            }
        }
        private void CK_DataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits)
        {
            cKPowerReceive = Encoding.Default.GetString(bits);
        }
        public bool Init(ref string e)
        {
            SerialPortParamCommon operation = (SerialPortParamCommon)LoadTechingPara();
            if (operation == null)
            {
                err = "序列号文件出错，请检查文件";
            }
            InitCKPower(operation.m_strName, operation.m_nBaudRate, (Parity)operation.m_strPartiy, operation.m_nDataBit, (StopBits)operation.m_strStopBit);
            e = err;
            if (e != "")
            {
                return false;
            }
            return true;
        }
        public void SetAllCKPowerOn()
        {
            cKPower.SendData(":ALLOUTON");

        }
        public void SetAllCKPowerOff()
        {
            cKPower.SendData(":ALLOUTOFF");
        }
        public void SetVoltage(int num, double v)
        {
            try
            {
                cKPower.SendData($"VSET{num}:" + v.ToString());
            }
            catch
            {

            }
        }
        public void SetCurrent(int num, double C)
        {
            cKPower.SendData($"ISET{num}:" + C.ToString());
        }
        cUserTimer CommunicateTimeOut = new cUserTimer(2000);
        public bool GetCurrent(int num, ref double value)
        {
            cKPowerReceive = "";
            cKPower.SendData($"MEASure{num}:CURRent?");
            CommunicateTimeOut.ResetStartTimer();
            while (cKPowerReceive == "")
            {
                if (CommunicateTimeOut.IsTimerOver)
                {
                    return false;
                }
            }
            double.TryParse(cKPowerReceive.Trim(), out value);
            return true;

        }
        public bool GetVoltage(int num, ref double value)
        {
            cKPowerReceive = "";
            cKPower.SendData($"MEASure{num}:Voltage?");
            CommunicateTimeOut.ResetStartTimer();
            while (cKPowerReceive == "")
            {
                if (CommunicateTimeOut.IsTimerOver)
                {

                    return false;
                }
            }
            double.TryParse(cKPowerReceive.Trim(), out value);
            return true;
        }
        public void CloseCKPower()
        {
            cKPower?.closePort();
        }
        private object LoadTechingPara()
        {
            object obj = null;
            try
            {
                obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"\config\GppSetting.xml", typeof(SerialPortParamCommon));
            }
            catch
            {
                obj = null;
            }
            return obj;
        }
    }
    public class cUserTimer
    {
        private int timedelay = 0;

        private Stopwatch stopwatch = new Stopwatch();

        public bool IsTimerOver
        {
            get
            {
                bool flag = !this.stopwatch.IsRunning;
                return !flag && this.stopwatch.ElapsedMilliseconds > (long)this.timedelay;
            }
        }

        public bool IsRuning
        {
            get
            {
                return this.stopwatch.IsRunning;
            }
        }

        public cUserTimer(int time)
        {
            this.timedelay = time;
            this.stopwatch.Reset();
        }

        public void ResetStartTimer()
        {
            this.stopwatch.Reset();
            this.stopwatch.Restart();
        }

        public void Stop()
        {
            this.stopwatch.Stop();
        }

        public void Reset()
        {
            this.stopwatch.Reset();
        }
    }
    public class SerialPortOperation
    {
        public SerialPort _serialPort = null;

        //定义委托

        public delegate void SerialPortDataReceiveEventArgs(object sender, SerialDataReceivedEventArgs e, byte[] bits);

        //定义接收数据事件

        public event SerialPortDataReceiveEventArgs DataReceived;

        //定义接收错误事件

        //public event SerialErrorReceivedEventHandler Error;

        //接收事件是否有效 false表示有效

        public bool ReceiveEventFlag = false;

        #region 获取串口名

        private string protName;

        public string PortName

        {

            get { return _serialPort.PortName; }

            set

            {

                _serialPort.PortName = value;

                protName = value;

            }

        }

        #endregion

        #region 获取比特率

        private int baudRate;

        public int BaudRate

        {

            get { return _serialPort.BaudRate; }

            set

            {

                _serialPort.BaudRate = value;

                baudRate = value;

            }

        }

        #endregion

        #region 默认构造函数

        /// <summary>

        /// 默认构造函数，操作COM1，速度为9600，没有奇偶校验，8位字节，停止位为1 "COM1", 9600, Parity.None, 8, StopBits.One

        /// </summary>

        public SerialPortOperation()

        {

            _serialPort = new SerialPort();

        }

        #endregion

        #region 构造函数

        /// <summary>

        /// 构造函数,

        /// </summary>

        /// <param name="comPortName"></param>

        public SerialPortOperation(string comPortName)

        {

            _serialPort = new SerialPort(comPortName);

            _serialPort.BaudRate = 9600;

            _serialPort.Parity = Parity.Even;

            _serialPort.DataBits = 8;

            _serialPort.StopBits = StopBits.One;

            _serialPort.Handshake = Handshake.None;

            _serialPort.RtsEnable = true;

            _serialPort.ReadTimeout = 2000;

            setSerialPort();

        }

        #endregion

        #region 构造函数,可以自定义串口的初始化参数

        /// <summary>

        /// 构造函数,可以自定义串口的初始化参数

        /// </summary>

        /// <param name="comPortName">需要操作的COM口名称</param>

        /// <param name="baudRate">COM的速度</param>

        /// <param name="parity">奇偶校验位</param>

        /// <param name="dataBits">数据长度</param>

        /// <param name="stopBits">停止位</param>

        public SerialPortOperation(string comPortName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {


            _serialPort = new SerialPort(comPortName, baudRate, parity, dataBits, stopBits);

            _serialPort.RtsEnable = true;  //自动请求

            _serialPort.ReadTimeout = 3000;//超时

            setSerialPort();

        }

        #endregion

        #region 析构函数

        /// <summary>

        /// 析构函数，关闭串口

        /// </summary>

        ~SerialPortOperation()

        {

            if (_serialPort.IsOpen)

                _serialPort.Close();

        }

        #endregion

        #region 设置串口参数

        /// <summary>

        /// 设置串口参数

        /// </summary>

        /// <param name="comPortName">需要操作的COM口名称</param>

        /// <param name="baudRate">COM的速度</param>

        /// <param name="dataBits">数据长度</param>

        /// <param name="stopBits">停止位</param>

        public void setSerialPort(string comPortName, int baudRate, int dataBits, int stopBits)

        {

            if (_serialPort.IsOpen)

                _serialPort.Close();

            _serialPort.PortName = comPortName;

            _serialPort.BaudRate = baudRate;

            _serialPort.Parity = Parity.None;

            _serialPort.DataBits = dataBits;

            _serialPort.StopBits = (StopBits)stopBits;

            _serialPort.Handshake = Handshake.None;

            _serialPort.RtsEnable = false;

            _serialPort.ReadTimeout = 3000;

            _serialPort.NewLine = "/r/n";

            setSerialPort();

        }

        #endregion

        #region 设置接收函数

        /// <summary>

        /// 设置串口资源,还需重载多个设置串口的函数

        /// </summary>

        void setSerialPort()

        {

            if (_serialPort != null)

            {

                //设置触发DataReceived事件的字节数为1

                _serialPort.ReceivedBytesThreshold = 1;

                //接收到一个字节时，也会触发DataReceived事件

                _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);

                //接收数据出错,触发事件

                _serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(_serialPort_ErrorReceived);

                //打开串口

                openPort();

            }

        }

        #endregion

        #region 打开串口资源

        /// <summary>

        /// 打开串口资源

        /// <returns>返回bool类型</returns>

        /// </summary>

        public bool openPort()

        {

            bool ok = false;

            //如果串口是打开的，先关闭

            if (_serialPort.IsOpen)

                _serialPort.Close();

            try

            {

                //打开串口

                _serialPort.Open();

                ok = true;

            }

            catch (Exception Ex)

            {

                throw Ex;

            }

            return ok;

        }

        #endregion

        #region 关闭串口

        /// <summary>

        /// 关闭串口资源,操作完成后,一定要关闭串口

        /// </summary>

        public void closePort()

        {

            //如果串口处于打开状态,则关闭

            if (_serialPort.IsOpen)

                _serialPort.Close();

        }

        #endregion

        #region 接收串口数据事件

        /// <summary>

        /// 接收串口数据事件

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)

        {

            //禁止接收事件时直接退出

            if (ReceiveEventFlag)

            {

                return;

            }

            try

            {

                System.Threading.Thread.Sleep(20);

                byte[] _data = new byte[_serialPort.BytesToRead];

                _serialPort.Read(_data, 0, _data.Length);

                if (_data.Length == 0) { return; }

                if (DataReceived != null)

                {

                    DataReceived(sender, e, _data);

                }

                _serialPort.DiscardInBuffer();  //清空接收缓冲区  

            }

            catch (Exception ex)

            {

                throw ex;

            }

        }

        #endregion

        #region 接收数据出错事件

        /// <summary>

        /// 接收数据出错事件

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        void _serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)

        {

        }

        #endregion

        #region 发送数据string类型

        public void SendData(string data)

        {

            //发送数据

            //禁止接收事件时直接退出

            if (ReceiveEventFlag)

            {

                return;

            }

            if (_serialPort.IsOpen)

            {

                _serialPort.WriteLine(data);

            }

        }

        #endregion

        #region 发送数据byte类型

        /// <summary>

        /// 数据发送

        /// </summary>

        /// <param name="data">要发送的数据字节</param>

        public void SendData(byte[] data, int offset, int count)

        {

            //禁止接收事件时直接退出

            if (ReceiveEventFlag)

            {

                return;

            }

            try

            {

                if (_serialPort.IsOpen)

                {

                    //_serialPort.DiscardInBuffer();//清空接收缓冲区

                    _serialPort.Write(data, offset, count);

                }

            }

            catch (Exception ex)

            {

                throw ex;

            }

        }

        #endregion

        #region 发送命令

        /// <summary>

        /// 发送命令

        /// </summary>

        /// <param name="SendData">发送数据</param>

        /// <param name="ReceiveData">接收数据</param>

        /// <param name="Overtime">超时时间</param>

        /// <returns></returns>

        public int SendCommand(byte[] SendData, ref byte[] ReceiveData, int Overtime)

        {



            if (_serialPort.IsOpen)

            {

                try

                {

                    ReceiveEventFlag = true;        //关闭接收事件

                    _serialPort.DiscardInBuffer();  //清空接收缓冲区                

                    _serialPort.Write(SendData, 0, SendData.Length);

                    int num = 0, ret = 0;

                    System.Threading.Thread.Sleep(10);

                    ReceiveEventFlag = false;      //打开事件

                    while (num++ < Overtime)

                    {

                        if (_serialPort.BytesToRead >= ReceiveData.Length)

                            break;

                        System.Threading.Thread.Sleep(10);

                    }



                    if (_serialPort.BytesToRead >= ReceiveData.Length)

                    {

                        ret = _serialPort.Read(ReceiveData, 0, ReceiveData.Length);

                    }

                    else

                    {

                        ret = _serialPort.Read(ReceiveData, 0, _serialPort.BytesToRead);

                    }

                    ReceiveEventFlag = false;      //打开事件

                    return ret;

                }

                catch (Exception ex)

                {

                    ReceiveEventFlag = false;

                    throw ex;

                }

            }

            return -1;

        }

        #endregion

        #region 获取串口

        /// <summary>

        /// 获取所有已连接短信猫设备的串口

        /// </summary>

        /// <returns></returns>

        public string[] serialsIsConnected()

        {

            List<string> lists = new List<string>();

            string[] seriallist = getSerials();

            foreach (string s in seriallist)

            {

            }

            return lists.ToArray();

        }

        #endregion

        #region 获取当前全部串口资源

        /// <summary>

        /// 获得当前电脑上的所有串口资源

        /// </summary>

        /// <returns></returns>

        public string[] getSerials()

        {

            return SerialPort.GetPortNames();

        }

        #endregion

        #region 字节型转换16

        /// <summary>

        /// 把字节型转换成十六进制字符串

        /// </summary>

        /// <param name="InBytes"></param>

        /// <returns></returns>

        public static string ByteToString(byte[] InBytes)

        {

            string StringOut = "";

            foreach (byte InByte in InBytes)

            {

                StringOut = StringOut + String.Format("{0:X2} ", InByte);

            }

            return StringOut;

        }

        #endregion

        #region 十六进制字符串转字节型

        /// <summary>

        /// 把十六进制字符串转换成字节型(方法1)

        /// </summary>

        /// <param name="InString"></param>

        /// <returns></returns>

        public static byte[] StringToByte(string InString)

        {

            string[] ByteStrings;

            ByteStrings = InString.Split(" ".ToCharArray());

            byte[] ByteOut;

            ByteOut = new byte[ByteStrings.Length];

            for (int i = 0; i <= ByteStrings.Length - 1; i++)

            {

                //ByteOut[i] = System.Text.Encoding.ASCII.GetBytes(ByteStrings[i]);

                ByteOut[i] = Byte.Parse(ByteStrings[i], System.Globalization.NumberStyles.HexNumber);

                //ByteOut[i] =Convert.ToByte("0x" + ByteStrings[i]);

            }

            return ByteOut;

        }

        #endregion

        #region 十六进制字符串转字节型

        /// <summary>

        /// 字符串转16进制字节数组(方法2)

        /// </summary>

        /// <param name="hexString"></param>

        /// <returns></returns>

        public static byte[] strToToHexByte(string hexString)

        {

            hexString = hexString.Replace(" ", "");

            if ((hexString.Length % 2) != 0)

                hexString += " ";

            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)

                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return returnBytes;

        }
        /// <summary>
        /// 把16进制字符串转换成byte[]
        /// </summary>
        /// <param name="inSting"></param>
        /// <returns></returns>
        public static byte[] _16ToBtyes(string inSting)
        {
            inSting = DelGeLi(inSting);//去掉隔离符
            byte[] strBt = new byte[inSting.Length / 2];
            for (int i = 0, j = 0; i < inSting.Length; i = i + 2, j++)
            {
                try
                {
                    string s = inSting.Substring(i, 2);
                    strBt[j] = (byte)Convert.ToInt16(s, 16);
                }
                catch (Exception e)
                {
                    //throw new Exception("你填写的数据不是纯16进制数，请检查。");
                    System.Windows.Forms.MessageBox.Show("你填写的数据不是纯16进制数，请检查。", "警告！");

                }
            }
            return strBt;
        }
        /// <summary>
        /// 去掉16进制字符串中的隔离符
        /// </summary>
        /// <param name="inString">需要转换的字符串</param>
        /// <returns></returns>
        private static string DelGeLi(string inString)
        {
            string outString = "";
            string[] del = { " ", "0x", "0X" };
            if (inString.Contains(" ") || inString.Contains("0x") || inString.Contains("0X"))//存在隔离符
            {
                string[] strS = inString.Split(del, System.StringSplitOptions.RemoveEmptyEntries);//以隔离符进行转换数组，去掉隔离符,去掉空格。
                for (int i = 0; i < strS.Length; i++)
                {
                    outString += strS[i].ToString();
                }
                return outString;
            }
            else//不存在隔离符
            {
                return inString;
            }
        }
        #endregion

        #region 字节型转十六进制字符串

        /// <summary>

        /// 字节数组转16进制字符串

        /// </summary>

        /// <param name="bytes"></param>

        /// <returns></returns>

        public static string byteToHexStr(byte[] bytes)

        {

            string returnStr = "";

            if (bytes != null)

            {

                for (int i = 0; i < bytes.Length; i++)

                {

                    returnStr += bytes[i].ToString("X2");

                }

            }

            return returnStr;

        }

        #endregion

        /// <summary>
        /// 字符串转换成byte[]
        /// </summary>
        /// <param name="inSting"></param>
        /// <returns></returns>
        public static byte[] StringToBtyes(string inSting)
        {
            inSting = StringTo16(inSting, Enum16进制隔离符.无);//把字符串转换成16进制数
            return _16ToBtyes(inSting);//把16进制数转换成byte[]
        }

        #region 汉字、英文、纯16进制数、byte[]，之间的各种转换函数。
        /// <summary>
        /// 字符串转换成16进制
        /// </summary>
        /// <param name="inSting"></param>
        /// <param name="enum16"></param>
        /// <returns></returns>
        public static string StringTo16(string inSting, Enum16进制隔离符 enum16)
        {
            string outString = "";
            byte[] bytes = Encoding.Default.GetBytes(inSting + "\r\n");

            for (int i = 0; i < bytes.Length; i++)
            {
                int strInt = Convert.ToInt16(bytes[i] - '\0');
                string s = strInt.ToString("X");
                if (s.Length == 1)
                {
                    s = "0" + s;
                }
                s = s + AddGeLi(enum16);
                outString += s;
            }
            return outString;
        }
        #endregion
        /// <summary>
        /// 把Enum16进制隔离符转换成实际的字符串
        /// </summary>
        /// <param name="enum16">Enum16进制隔离符</param>
        /// <returns></returns>
        private static string AddGeLi(Enum16进制隔离符 enum16)
        {
            switch (enum16)
            {
                case Enum16进制隔离符.无:
                    return "";
                case Enum16进制隔离符.Ox:
                    return "0x";
                case Enum16进制隔离符.OX:
                    return "0X";
                case Enum16进制隔离符.空格:
                    return " ";
                default:
                    return "";
            }
        }
        public enum Enum16进制隔离符
        {
            无,
            空格,
            OX,
            Ox
        }
        public void CloseExposure()
        {
            byte[] bt = SerialPortOperation.StringToBtyes("#ISPW=808501");
            _serialPort.Write(bt, 0, bt.Length);
            Thread.Sleep(10);
            byte[] bt1 = SerialPortOperation.StringToBtyes("#ISPW=044180");
            _serialPort.Write(bt1, 0, bt1.Length);
            Thread.Sleep(10);
            byte[] bt2 = SerialPortOperation.StringToBtyes("#ISPW=044200");
            _serialPort.Write(bt2, 0, bt2.Length);
            Thread.Sleep(10);
            byte[] bt3 = SerialPortOperation.StringToBtyes("#ISPW=837000");
            _serialPort.Write(bt3, 0, bt3.Length);
            Thread.Sleep(10);
            byte[] bt4 = SerialPortOperation.StringToBtyes("#ISPW=150100");
            _serialPort.Write(bt4, 0, bt4.Length);
            Thread.Sleep(10);
            byte[] bt5 = SerialPortOperation.StringToBtyes("#ISPW=270300");
            _serialPort.Write(bt5, 0, bt5.Length);
        }
    }

}
