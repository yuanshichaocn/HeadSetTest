using System;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using BaseDll;

namespace Communicate
{
    /// <summary>
    /// 串口通讯类封装
    /// </summary>
    public class ComLink:LogView
    {
        /// <summary>
        ///串口号 
        /// </summary>
        public int m_nComNo;
        /// <summary>
        ///串口定义名称 
        /// </summary>
        public string m_strName;
        /// <summary>
        ///波特率 
        /// </summary>
        public int m_nBaudRate;
        /// <summary>
        ///数据位 
        /// </summary>
        public int m_nDataBit;
        /// <summary>
        ///校验位 
        /// </summary>
        public string m_strPartiy;
        /// <summary>
        ///停止位 
        /// </summary>
        public string m_strStopBit;
        /// <summary>
        ///流控制 
        /// </summary>
        public string m_strFlowCtrl;
        /// <summary>
        ///超时时间 
        /// </summary>
        public int m_nTime;
        /// <summary>
        ///缓冲区大小 
        /// </summary>
        public int m_nBufferSzie;
        /// <summary>
        ///命令分隔符标志 
        /// </summary>
        public string m_strLineFlag;

        /// <summary>
        ///命令分隔符 
        /// </summary>
        private string m_strLine;

        /// <summary>
        /// 状态变更委托
        /// </summary>
        /// <param name="com"></param>
        public delegate void StateChangedHandler(ComLink com);
        /// <summary>
        /// 定义状态变更事件
        /// </summary>
        public event StateChangedHandler StateChangedEvent;
   
        /// <summary>
        /// 系统串口类引用
        /// </summary>
        private SerialPort m_serialPort  = null;
  

        /// <summary>
        /// 读取数据过程中是否已经超时
        /// </summary>
        bool m_bTimeOut = false;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nComNo"></param>
        /// <param name="strName"></param>
        /// <param name="nBaudRate"></param>
        /// <param name="nDataBit"></param>
        /// <param name="strPartiy"></param>
        /// <param name="strStopBit"></param>
        /// <param name="strFlowCtrl"></param>
        /// <param name="nTime"></param>
        /// <param name="nBufferSzie"></param>
        /// <param name="strLine"></param>
        public ComLink(int nComNo, string strName, int nBaudRate, int nDataBit, string strPartiy,
            string strStopBit, string strFlowCtrl,int nTime,int nBufferSzie,string strLine)
        {
            m_nComNo = nComNo;
            m_strName = strName;
            m_nBaudRate = nBaudRate;
            m_nDataBit = nDataBit;
            m_strPartiy = strPartiy;
            m_strStopBit = strStopBit;
            m_strFlowCtrl = strFlowCtrl;
            m_nTime = nTime;
            m_nBufferSzie = nBufferSzie;

            m_strLineFlag = strLine;
            if (strLine == "CRLF")
            {
                m_strLine = "\r\n";
            }
            else if (strLine == "CR")
            {
                m_strLine = "\r";
            }
            else if (strLine == "LF")
            {
                m_strLine = "\n";
            }
            else if (strLine == "无")
            {
                m_strLine = "";
            }
        }

        /// <summary>
        ///打开串口
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            if(m_serialPort == null)
                m_serialPort = new SerialPort();

            if(m_serialPort.IsOpen == false)
            {
                m_serialPort.PortName = "COM" + m_nComNo.ToString();
                m_serialPort.BaudRate = m_nBaudRate;
                m_serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), m_strPartiy);
                m_serialPort.DataBits = m_nDataBit;
                m_serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), m_strStopBit);
                m_serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), m_strFlowCtrl);

                m_serialPort.ReadTimeout = m_nTime;
                m_serialPort.WriteTimeout = m_nTime;

                m_serialPort.NewLine = m_strLine;
                try
                {
                    m_serialPort.Open();
                    if(StateChangedEvent != null)
                        StateChangedEvent(this);
                }
                catch(Exception e)
                {
                    Debug.WriteLine("串口 {0} 打开失败\r\n{1}\r\n",m_strName, e.ToString());
                  
                }
            }            
            return m_serialPort.IsOpen;
        }

        /// <summary>
        /// 判断是否已经打开
        /// </summary>
        /// <returns></returns>
        public   bool IsOpen()
        {
            return m_serialPort != null && m_serialPort.IsOpen;
        }

        /// <summary>
        /// 判断是否超时
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOut()
        {
            return m_bTimeOut;
        }

           /// <summary>
        ///向串口写入数据 
        /// </summary>
        /// <param name="sendBytes"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public bool WriteData(byte[] sendBytes, int nLen)
        {
            if (m_serialPort.IsOpen)
            {
                m_serialPort.Write(sendBytes,0, nLen);
                ShowLog(System.Text.Encoding.Default.GetString(sendBytes));
                return true;
            }
            return false;
        }

        /// <summary>
        ///向串口写入字符串 
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public bool WriteString(string strData)
        {
            if (m_serialPort.IsOpen)
            {
                m_serialPort.Write(strData);
                ShowLog(strData);
                return true;
            }
            return false;
        }

        /// <summary>
        ///向串口写入一行字符 
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public bool WriteLine(string strData)
        {
            if (m_serialPort.IsOpen)
            {
                m_serialPort.WriteLine(strData);
                ShowLog(strData);
                return true;
            }
            return false;
        }

        /// <summary>
        ///从串口读取数据 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public int ReadData(byte[] bytes, int nLen)
        {
            m_bTimeOut = false;
            int n = 0;
            if (m_serialPort.IsOpen)
            {
                try
                {
                    m_serialPort.Read(bytes, 0, nLen);
                    if (n > 0)
                        ShowLog(System.Text.Encoding.Default.GetString(bytes));
                }
                catch/*(TimeoutException e)*/
                {
                    m_bTimeOut = true;
                    if (StateChangedEvent != null)
                        StateChangedEvent(this);
                }
            }
            return n;
        }

        /// <summary>
        ///从串口读取一行数据 
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public int ReadLine(out string strData)
        {
            m_bTimeOut = false;
            strData = "";
            if (m_serialPort.IsOpen)
            {               
                try
                {
                    strData = m_serialPort.ReadLine();
                    if (strData.Length > 0)
                        ShowLog(strData);
                }
                catch/*(TimeoutException e)*/
                {
                    m_bTimeOut = true;
                    if (StateChangedEvent != null)
                        StateChangedEvent(this);
                }
            }
            return strData.Length;
        }

        /// <summary>
        ///关闭串口 
        /// </summary>
        public void Close()
        {
            if(m_serialPort != null && m_serialPort.IsOpen)
            {
                m_serialPort.Close();
                m_serialPort = null;
                m_bTimeOut = false;
                if (StateChangedEvent != null)
                    StateChangedEvent(this);
            }
        }


        /// <summary>
        /// 清除缓冲区
        /// </summary>
        /// <param name="bIn">是否清除输入缓冲区</param>
        /// <param name="bOut">是否清除输出缓冲区</param>
        public void ClearBuffer(bool bIn, bool bOut)
        {
            if (m_serialPort != null)
            {
                if(bIn)
                    m_serialPort.DiscardInBuffer();
                if(bOut)
                    m_serialPort.DiscardOutBuffer();

            }
        }
    }
}
