using BaseDll;
using log4net;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OtherDevice
{
    public class ElecCmp
    {
        [NonSerialized]
        private static SerialPort _serialPort = null;

        [NonSerialized]
        private readonly ILog _logger = LogManager.GetLogger(nameof(ElecCmp));

        [NonSerialized]
        private readonly object _disposingLock = new object();

        [NonSerialized]
        private static readonly object _syncRoot = new object();

        [NonSerialized]
        private IModbusSerialMaster _master;

        private SerialPortParam serialPortParam = new SerialPortParam();

        public object ReadParam()
        {
            object obj = null;
            try
            {
                obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"\config\ElecCmp.xml", serialPortParam.GetType());
                if (obj != null)
                    serialPortParam = (SerialPortParam)obj;
                else
                    Save();
            }
            catch
            {
                obj = null;
            }
            if (obj == null)
            {
                MessageBox.Show("电抓  串口 数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return obj;
        }

        public void Save()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"\config\ElecCmp.xml";
            AccessXmlSerializer.ObjectToXml(strPath, serialPortParam);
        }

        public bool Init()
        {
            // m_bOpen = m_comLink ==null ? false : m_comLink.IsOpen();
            if (ReadParam() == null)
            {
                Save();
            }
            try
            {
                if (_serialPort == null)
                    _serialPort = new SerialPort("COM" + serialPortParam.m_nComNo.ToString())
                    {
                        BaudRate = serialPortParam.m_nBaudRate,
                        DataBits = serialPortParam.m_nDataBit,
                        Parity = (Parity)Enum.Parse(typeof(Parity), serialPortParam.m_strPartiy),
                        StopBits = (StopBits)Enum.Parse(typeof(StopBits), serialPortParam.m_strStopBit)
                    };
                // if (_serialPort.IsOpen)
                //     _serialPort.Close();
                _serialPort.ReadTimeout = 3000;
                _serialPort.WriteTimeout = 3000;
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
            }
            catch (Exception ex)
            {
                _logger.Warn(string.Format(" 电抓 COM{0} 打开失败 请检查配置", serialPortParam.m_nComNo));
                MessageBox.Show(string.Format(" 电抓 COM{0} 打开失败 请检查配置", serialPortParam.m_nComNo), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // throw new Exception("IO控制器串口初始化失败," + ex.Message, ex);
                return false;
            }
            return true;
        }

        public object LoadTechingPara()
        {
            object obj = null;
            try
            {
                obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"\config\Teaching.xml", typeof(List<GripperTeachingPara>));
            }
            catch
            {
                obj = null;
            }
            if (obj == null)
            {
                MessageBox.Show("电抓  串口 数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return obj;
        }

        public bool SaveTechingPara(List<GripperTeachingPara> para)
        {
            string gripperTeachingPath = AppDomain.CurrentDomain.BaseDirectory + @"\config\Teaching.xml";
            bool result = false;
            try
            {
                result = AccessXmlSerializer.ObjectToXml(gripperTeachingPath, para);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        ///释放IO卡
        /// </summary>
        public void DeInit()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void WriteRegRTU(byte slaveAddress, long Address, int val)
        {
            try
            {
                byte[] CMD = new byte[8];
                CMD[0] = slaveAddress;
                CMD[1] = 0X06;

                CMD[2] = (byte)((Address & 0X00FF00) >> 8);
                CMD[3] = (byte)((Address & 0X00FF) >> 0);
                CMD[4] = (byte)((val & 0X00FF00) >> 8);
                CMD[5] = (byte)((val & 0X00FF) >> 0);
                int VALCRC = ParityType.CRC16(CMD, 6);
                CMD[6] = (byte)(VALCRC & 0X00FF);
                CMD[7] = (byte)((VALCRC & 0XFF00) >> 8);
                _serialPort.DiscardInBuffer();
                _serialPort.Write(CMD, 0, CMD.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show("电抓  串口 写入数据异常 " + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool WaitCMDCompete(int Index, int nTimeout = 3000)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            do
            {
                if (stopwatch.ElapsedMilliseconds > nTimeout)
                    return false;
                try
                {
                    int F = (int)OtherDevices.elecCmp.ReadRegRTU(1, Convert.ToUInt16("1008", 16), nTimeout); Thread.Sleep(10);
                    if (F == Index)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            while (true);
        }

        public double ReadRegRTU(byte slaveAddress, long Address, int nTimeout = 1000)
        {
            try
            {
                Byte[] recvBytes = new Byte[1024];
                int recvSize = 0;
                byte[] CMD = new byte[8];
                CMD[0] = slaveAddress;
                CMD[1] = 0X03;
                CMD[2] = (byte)((Address & 0X00FF00) >> 8);
                CMD[3] = (byte)((Address & 0X00FF) >> 0);
                CMD[4] = 0;
                CMD[5] = 0X01;
                int VALCRC = ParityType.CRC16(CMD, 6);
                CMD[6] = (byte)(VALCRC & 0X00FF);
                CMD[7] = (byte)((VALCRC & 0XFF00) >> 8);
                _serialPort.DiscardInBuffer();
                _serialPort.Write(CMD, 0, CMD.Length);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                int data = 0;
                do
                {
                    if (stopwatch.ElapsedMilliseconds > nTimeout)
                        return double.NaN;
                    try
                    {
                        Byte d = (Byte)_serialPort.ReadByte();
                        recvBytes[recvSize++] = d;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                    if (recvSize >= 2 && recvBytes[1] == 0x02)
                    {
                        return double.NaN;
                    }

                    if (recvSize >= 7 && recvBytes[1] == 0x03)
                    {
                        data |= recvBytes[3] << 8;
                        data |= recvBytes[4];
                        return data;
                    }
                }
                while (true);
            }
            catch (Exception e)
            {
                MessageBox.Show("电抓  串口 读取数据异常 " + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return double.NaN;
            }
        }

        public double ReadRegRTU2(byte slaveAddress, long Address, int nTimeout = 1000)
        {
            try
            {
                Byte[] recvBytes = new Byte[1024];
                int recvSize = 0;
                byte[] CMD = new byte[8];
                CMD[0] = slaveAddress;
                CMD[1] = 0X03;
                CMD[2] = (byte)((Address & 0X00FF00) >> 8);
                CMD[3] = (byte)((Address & 0X00FF) >> 0);
                CMD[4] = 0;
                CMD[5] = 0X02;
                int VALCRC = ParityType.CRC16(CMD, 6);
                CMD[6] = (byte)(VALCRC & 0X00FF);
                CMD[7] = (byte)((VALCRC & 0XFF00) >> 8);
                _serialPort.DiscardInBuffer();
                _serialPort.Write(CMD, 0, CMD.Length);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                int data = 0;
                do
                {
                    if (stopwatch.ElapsedMilliseconds > nTimeout)
                        return double.NaN;
                    try
                    {
                        Byte d = (Byte)_serialPort.ReadByte();
                        recvBytes[recvSize++] = d;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                    if (recvSize >= 2 && recvBytes[1] == 0x02)
                    {
                        return double.NaN;
                    }

                    if (recvSize >= 9 && recvBytes[1] == 0x03)
                    {
                        data |= recvBytes[3] << 24;
                        data |= recvBytes[4] << 16;
                        data |= recvBytes[5] << 8;
                        data |= recvBytes[6];
                        return data;
                    }
                }
                while (true);
            }
            catch (Exception e)
            {
                MessageBox.Show("电抓  串口 读取数据异常 " + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return double.NaN;
            }
        }

        public double ReadRegAscii(byte slaveAddress, long Address, int nTimeout = 1000)
        {
            Byte[] recvBytes = new Byte[1024];
            int recvSize = 0;
            byte[] CMD = new byte[17];
            CMD[0] = 0;
            string temp = (slaveAddress).ToString("X2");
            //站号
            CMD[1] = (byte)(slaveAddress).ToString("X2").ToCharArray()[0];
            CMD[2] = (byte)(slaveAddress).ToString("X2").ToCharArray()[1];
            //命令
            CMD[3] = (byte)'0';
            CMD[4] = (byte)'3';
            //地址
            byte addr = (byte)((Address & 0XFF00) >> 8);
            temp = ((byte)((Address & 0XFF00) >> 4)).ToString("X2");
            CMD[5] = (byte)((byte)((Address & 0XFF00) >> 8)).ToString("X2").ToCharArray()[0];
            CMD[6] = (byte)((byte)((Address & 0XFF00) >> 8)).ToString("X2").ToCharArray()[1];
            temp = ((byte)((Address & 0X00FF) >> 0)).ToString("X2");
            CMD[7] = (byte)((byte)((Address & 0X00FF) >> 0)).ToString("X2").ToCharArray()[0];
            CMD[8] = (byte)((byte)((Address & 0X00FF) >> 0)).ToString("X2").ToCharArray()[1];

            //读出个数
            CMD[9] = (byte)'0';
            CMD[10] = (byte)'0';
            CMD[11] = (byte)'0';
            CMD[12] = (byte)'1';

            byte[] bytesAscii = new Byte[6];
            bytesAscii[0] = slaveAddress;
            bytesAscii[1] = 0x03;
            bytesAscii[2] = (byte)((Address & 0XFF00) >> 8);
            bytesAscii[3] = (byte)(Address & 0X00FF);
            bytesAscii[4] = 0x00;
            bytesAscii[5] = 0x01;

            int VALLRC = ParityType.LRC(bytesAscii, 6);
            temp = ((byte)((VALLRC & 0XF0) >> 4)).ToString("X");
            CMD[13] = (byte)((byte)((VALLRC & 0XF0) >> 4)).ToString("X").ToCharArray()[0];
            CMD[14] = (byte)((byte)(VALLRC & 0X0F)).ToString("X").ToCharArray()[0];
            temp = ((byte)((VALLRC & 0X0F) >> 0)).ToString("X");
            CMD[15] = 0X0D;
            CMD[16] = 0X0A;
            CMD[0] = (byte)':';
            _serialPort.DiscardInBuffer();
            _serialPort.Write(CMD, 0, CMD.Length);
            int data = 0;
            string result = "";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            do
            {
                if (stopwatch.ElapsedMilliseconds > nTimeout)
                    return double.NaN;
                try
                {
                    Byte d = (Byte)_serialPort.ReadByte();
                    recvBytes[recvSize++] = d;
                }
                catch (Exception ex)
                {
                    continue;
                }
                if (recvSize >= 5 && recvBytes[3] == (Byte)'8' && recvBytes[3] == (Byte)'3')
                {
                    return double.NaN;
                }

                if (recvSize >= 15 && recvBytes[13] == 0x0D && recvBytes[14] == 0x0a)
                {
                    result = Encoding.Default.GetString(recvBytes);
                    result = result.Substring(7, 4);
                    data = Convert.ToInt32(result, 16);
                    return data;
                }
            }
            while (true);
        }

        /// <summary>
        /// 三种状态 回原点 松开 夹紧
        /// </summary>
        /// <param name="strProgramname"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public bool ExceCmpPro(string strProgramname, ref string err)
        {
            bool result = false;
            err = "";
            switch (strProgramname)
            {
                case "回原点":
                    result = CommonRun(MotionType.Home, 1, ref err);
                    break;

                case "松开":
                    result = CommonRun(MotionType.Open, 2, ref err);
                    break;

                case "夹紧":
                    result = CommonRun(MotionType.Close, 4, ref err);
                    break;
            }
            return result;
        }

        public bool SavePostionToExceCmpP(BindingList<GripperTeachingPara> grippers)
        {
            for (int index = 0; index <= 5; index++)
            {
                GripperTeachingPara gripper = grippers.FirstOrDefault(p => p.Index == index);//获取第一个坐标
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}0", 16), (int)gripper.Mode); Thread.Sleep(10);
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}1", 16), gripper.MovePosition); Thread.Sleep(10);
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}3", 16), gripper.Speed); Thread.Sleep(10);
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}4", 16), gripper.Torque); Thread.Sleep(10);
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}5", 16), gripper.Sign); Thread.Sleep(10);
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}6", 16), (int)gripper.PositionLimitNegative); Thread.Sleep(10);
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}8", 16), (int)gripper.PositionLimitPositive); Thread.Sleep(10);
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}A", 16), gripper.DelayTime); Thread.Sleep(10);
                OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"90{index}B", 16), gripper.Order); Thread.Sleep(10);
            }
            OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16($"9999", 16), 1); Thread.Sleep(10);
            return false;
        }

        /// <summary>
        /// 回原点，对应程序0 1
        /// </summary>
        /// <returns></returns>
        private bool CommonRun(MotionType type, int index, ref string err)
        {
            OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16("2015", 16), (int)MotionType.Clear); Thread.Sleep(20);
            OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16("2015", 16), (int)type); Thread.Sleep(20);
            return WaitCMDCompete(index, 1000);
            //OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16("2015", 16), (int)MotionType.Clear); Thread.Sleep(20);

            //OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16("2015", 16), (int)MotionType.End); Thread.Sleep(20);
            //WaitCMDCompete(8, 1000);
            //OtherDevices.elecCmp.WriteRegRTU(1, Convert.ToUInt16("2015", 16), (int)MotionType.Clear); Thread.Sleep(20);
        }

        //public double ReadRegAscii2(byte slaveAddress, long Address, int nTimeout = 1000)
        //{
        //    Byte[] recvBytes = new Byte[1024];
        //    int recvSize = 0;
        //    byte[] CMD = new byte[17];

        //    byte[] bytesAscii = new Byte[6];
        //    bytesAscii[0] = slaveAddress;
        //    bytesAscii[1] = 0x03;
        //    bytesAscii[2] = (byte)((Address & 0XFF00) >> 8);
        //    bytesAscii[3] = (byte)(Address & 0X00FF);
        //    bytesAscii[4] = 0x00;
        //    bytesAscii[5] = 0x01;
        //    string temp = bytesAscii[2].ToString("X2");
        //    string temp2 = bytesAscii[3].ToString("X2");
        //    int VALLRC = ParityType.LRC(bytesAscii, 6);
        //    string strCMD = ":0" + slaveAddress.ToString() + "03" + bytesAscii[2].ToString("X2") + bytesAscii[3].ToString("X2") + "0001" + ((byte)VALLRC).ToString("X2")+"\r\n";
        //    _serialPort.DiscardOutBuffer();
        //    _serialPort.WriteLine(strCMD);
        //    int data = 0;
        //    string result = "";
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Restart();
        //    do
        //    {
        //        if (stopwatch.ElapsedMilliseconds > nTimeout)
        //            return double.NaN;
        //        try
        //        {
        //            Byte d = (Byte)_serialPort.ReadByte();
        //            recvBytes[recvSize++] = d;
        //        }
        //        catch (Exception ex)
        //        {
        //            continue;
        //        }
        //        if (recvSize >= 5 && recvBytes[3] == (Byte)'8' && recvBytes[3] == (Byte)'3')
        //        {
        //            return double.NaN;
        //        }

        //        if (recvSize >= 15 && recvBytes[13] == 0x0D && recvBytes[14] == 0x0a)
        //        {
        //            result = Encoding.Default.GetString(recvBytes);
        //            result = result.Substring(7, 4);
        //            data = Convert.ToInt32(result, 16);
        //            return data;
        //        }

        //    }
        //    while (true);

        //}
    }
}