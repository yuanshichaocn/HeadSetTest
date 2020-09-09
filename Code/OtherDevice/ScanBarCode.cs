using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseDll;
using log4net;
using SerialDict;

namespace OtherDevice
{
    public abstract  class Scanner
    {

        protected const Byte STX = 0x02;
        protected const Byte ETX = 0x03;
        protected const Byte CR = 0x0d;
        protected const Byte CL = 0x0a;
        protected SerialPort serialPortInstance = null;
        protected SerialPortParam serialPortParam = new SerialPortParam();
        public object ReadParam()
        {
            object obj = null;
            object obj2 = null;
            try
            {
                obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"config\COM_KEYENCE_Scanner" + ".xml", serialPortParam.GetType());
                if (obj != null)
                    serialPortParam = (SerialPortParam)obj;
                else
                {
                    Save();
                }

            }
            catch
            {
                obj = null;
            }
            if (obj == null)
            {
                MessageBox.Show("lingt control 串口 数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return obj;
        }
        public void Save()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"\config\COM_KEYENCE_Scanner" + ".xml";
            AccessXmlSerializer.ObjectToXml(strPath, serialPortParam);
        }
        public  bool Init()
        {
            try
            {
                //serialPortInstance.BaudRate = serialPortParam.m_nBaudRate;         // 9600, 19200, 38400, 57600 or 115200
                //serialPortInstance.DataBits = serialPortParam.m_nDataBit;              // 7 or 8
                //serialPortInstance.Parity = (Parity)Enum.Parse(typeof(Parity), serialPortParam.m_strPartiy);    // Even or Odd
                //serialPortInstance.StopBits = (StopBits)Enum.Parse(typeof(Parity), serialPortParam.m_strStopBit);   // One or Two
                //serialPortInstance.PortName = string.Format("COM{0}", serialPortParam.m_nComNo);
               
                //if (serialPortInstance.IsOpen)
                //{
                //    this.serialPortInstance.Close();
                //}

                ////
                //// Open the COM port.
                ////
                //this.serialPortInstance.Open();

                ////
                //// Set 100 milliseconds to receive timeout.
                ////
                //this.serialPortInstance.ReadTimeout = 100;

                if (serialPortInstance == null)
                    serialPortInstance = new SerialPort("COM" + serialPortParam.m_nComNo.ToString())
                    {
                        BaudRate = serialPortParam.m_nBaudRate,
                        DataBits = serialPortParam.m_nDataBit,
                        Parity = (Parity)Enum.Parse(typeof(Parity), serialPortParam.m_strPartiy),
                        StopBits = (StopBits)Enum.Parse(typeof(StopBits), serialPortParam.m_strStopBit)
                    };
                if (serialPortInstance.IsOpen)
                    serialPortInstance.Close();
                serialPortInstance.ReadTimeout = 3000;
                serialPortInstance.WriteTimeout = 3000;
                if (!serialPortInstance.IsOpen)
                    serialPortInstance.Open();
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        public  bool DisconnectScanner()
        {
            try
            {
                if (serialPortInstance!=null &&serialPortInstance.IsOpen)
                    this.serialPortInstance?.Close();
            }
            catch (IOException ex)
            {

                return false;
            }
            return true;
        }

        public abstract bool LonScanner();
        public abstract bool LoffScanner();

        public abstract bool ReceiveScannerData(ref string result, int nTimeout = 200);
    }

    public class COM_KEYENCE_Scanner: Scanner
    {
        
        ~COM_KEYENCE_Scanner()
        {
            DisconnectScanner();
        }
      
        public override bool LonScanner()
        {
            //
            // Send "LON" command.
            // Set STX to command header and ETX to the terminator to distinguish between command respons
            // and read data when receives data from readers.
            // 
            string lon = "LON\x0D\x0A";   // <STX>LON<ETX>
            Byte[] sendBytes = ASCIIEncoding.ASCII.GetBytes(lon);

            if (this.serialPortInstance.IsOpen)
            {
                try
                {
                    serialPortInstance.DiscardOutBuffer();
                    this.serialPortInstance.Write(sendBytes, 0, sendBytes.Length);
                }
                catch (IOException ex)
                {

                    return false;
                }
            }
            else
            {

                return false;
            }
            return true;
        }

        public override bool LoffScanner()
        {
            //
            // Send "LOFF" command.
            // Set STX to command header and ETX to the terminator to distinguish between command respons
            // and read data when receives data from readers.
            // 
            string loff = "LOFF\x0D\x0A";   // <STX>LOFF<ETX>
            Byte[] sendBytes = ASCIIEncoding.ASCII.GetBytes(loff);

            if (this.serialPortInstance.IsOpen)
            {
                try
                {
                    serialPortInstance.DiscardOutBuffer();
                    this.serialPortInstance.Write(sendBytes, 0, sendBytes.Length);
                }
                catch (IOException ex)
                {

                    return false;
                }
            }
            else
            {

                return false;
            }

            return true;
        }

        public override  bool ReceiveScannerData(ref string result, int nTimeout = 5000)
        {
            Byte[] recvBytes = new Byte[1024];
            int recvSize = 0;
            int recvSizeTotal = 0;
            result = "";

            if (this.serialPortInstance.IsOpen == false)
            {

                return false;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            int nCount = 0;
            do
            {
                if (stopwatch.ElapsedMilliseconds > nTimeout)
                    return false;
                try
                {
                    Byte d = (Byte)serialPortInstance.ReadByte();
                    recvBytes[recvSize++] = d;
                }
                catch (Exception ex)
                {
                    //   MessageBox.Show(serialPortInstance.PortName + "\r\n" + ex.Message);    // disappeared

                    continue;
                }
                
                if (recvSize >= 2 && recvBytes[recvSize - 1] == 0x0A && recvBytes[recvSize - 2] == 0x0D)
                {
                    result = Encoding.GetEncoding("Shift_JIS").GetString(recvBytes);
                    result = result.Substring(0, recvSize - 1);
                    if (result.ToUpper().Contains("ERROR"))
                    {
                        return false;
                    }
                    return true;
                }

            }
            while (true);
          
        }
    }


}