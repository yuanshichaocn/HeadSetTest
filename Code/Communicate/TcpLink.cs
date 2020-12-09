using BaseDll;
using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communicate

{
    public class AsyTcpSocketEventArgs
    {
        private string _message;

        public AsyTcpSocketEventArgs(string message)
        {
            _message = message;
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public byte[] _recvData;

        public AsyTcpSocketEventArgs(byte[] data)
        {
            _recvData = data;
            _message = strRecv;
        }

        public string strRecv
        {
            get
            {
                if (_recvData != null)
                    return Encoding.UTF8.GetString(_recvData);//.Trim();
                                                              //return Encoding.Default.GetString(_recvData);//.Trim();
                else
                    return "";
            }
        }
    }

    #region 公共代理函数

    /// <summary>
    /// 服务器接受客户端连接请求代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SocketAcceptDelegate(object sender, AsyTcpSocketEventArgs e);

    /// <summary>
    /// SOCKET接收非字符串数据代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RecvFileMessageDelegate(object sender, AsyTcpSocketEventArgs e);

    /// <summary>
    /// SOCKET接收到字符串数据代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RecvStringMessageDelegate(object sender, AsyTcpSocketEventArgs e);

    /// <summary>
    /// SOCKET错误函数代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SocketErrorDelegate(object sender, AsyTcpSocketEventArgs e);

    /// <summary>
    /// 开始准备接收数据代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void BeginRecvBufferDelegate(object sender, AsyTcpSocketEventArgs e);

    /// <summary>
    /// 正在接收数据代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RecvBufferDelegate(object sender, AsyTcpSocketEventArgs e);

    /// <summary>
    /// 准备发送函数代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void BeginSendBufferDelegate(object sender, AsyTcpSocketEventArgs e);

    /// <summary>
    /// 正在发送函数代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SendBufferDelegate(object sender, AsyTcpSocketEventArgs e);

    #endregion 公共代理函数

    /// <summary>
    /// 网络连接封装类
    /// </summary>
    public class TcpLink : LogView
    {
        private ILog _logger = LogManager.GetLogger("TcpLink");

        /// <summary>
        ///网口号
        /// </summary>
        public int m_nIndex;

        /// <summary>
        ///网口定义
        /// </summary>
        public string m_strName;

        /// <summary>
        ///对方IP地址
        /// </summary>
        public string m_strIP;

        /// <summary>
        ///端口号
        /// </summary>
        public int m_nPort;

        /// <summary>
        ///超时时间
        /// </summary>
        public int m_nTime;

        /// <summary>
        ///命令分隔
        /// </summary>
       // public string m_strLineFlag;
        private int m_count = 0;

        private const int m_MaxCountConnet = 10;

        private StringBuilder sbReceiveDataBuffer = new StringBuilder();

        /// <summary>
        /// 内部接受数据数组
        /// </summary>
        private byte[] m_recvBuffer = new byte[1500];

        /// <summary>
        /// 已经接收到数据的大小
        /// </summary>
        private int m_recvLength = 0;

        /// <summary>
        /// 接收到字符串缓冲
        /// </summary>
        private string m_recvString = "";

        /// <summary>
        /// 判断当前是否已经接收到消息的头部
        /// </summary>
        private bool m_isRecvHeadMessage = false;

        /// <summary>
        /// 控制发送线程信号量
        /// </summary>
        private ManualResetEvent m_sendEvent = new ManualResetEvent(false);

        /// <summary>
        /// 文件保存的地址
        /// </summary>
        private static string m_path = "";

        /// <summary>
        ///命令分隔符
        /// </summary>
        public string m_strLine;

        private TcpClient m_client = null;
        private bool m_bTimeOut = false;

        /// <summary>
        /// 状态变更委托函数定义
        /// </summary>
        /// <param name="tcp"></param>
        public delegate void StateChangedHandler(TcpLink tcp);

        /// <summary>
        /// 状态变更委托事件
        /// </summary>
        public event StateChangedHandler StateChangedEvent;

        private static bool m_bConnectSuccess = false;
        private static Exception socketException;
        private ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        private byte[] m_ArrSendByte;

        #region 外部调用的事件

        /// <summary>
        /// 接受到非字符消息触发事件
        /// </summary>
        public event RecvFileMessageDelegate RecvFileMessageEvent = null;

        /// <summary>
        /// 接收到字符串消息触发事件
        /// </summary>
        public event RecvStringMessageDelegate RecvStringMessageEvent = null;

        /// <summary>
        /// 当SOCKET发生异常的时候触发事件
        /// </summary>
        public event SocketErrorDelegate SocketErrorEvent = null;

        /// <summary>
        /// 开始准备接收数据触发事件
        /// </summary>
        public event BeginRecvBufferDelegate BeginRecvBufferEvent = null;

        /// <summary>
        /// 接收到数据触发事件
        /// </summary>
        public event RecvBufferDelegate RecvBufferEvent = null;

        /// <summary>
        /// 准备发送数据触发事件
        /// </summary>
        public event BeginSendBufferDelegate BeginSendBufferEvent = null;

        /// <summary>
        /// 每次发送一个数据块的触发事件
        /// </summary>
        public event SendBufferDelegate SendBufferEvent = null;

        // 服务器接收当前连接请求
        public event SocketAcceptDelegate SocketAcceptEvent = null;

        /// <summary>
        /// 接收到数据事件
        /// </summary>
       // public event EventHandler<AsyTcpSocketEventArgs> DataReceived;

        private void RaiseDataReceived(byte[] data)
        {
            if (RecvStringMessageEvent != null)
            {
                RecvStringMessageEvent(this, new AsyTcpSocketEventArgs(data));
                m_recvString = "";
            }
        }

        //是否同步
        private bool SynchronizationCom
        {
            set;
            get;
        }

        #endregion 外部调用的事件

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="strName"></param>
        /// <param name="strIP"></param>
        /// <param name="nPort"></param>
        /// <param name="nTime"></param>
        /// <param name="strLine"></param>
        public TcpLink(int nIndex, string strName, string strIP, int nPort, int nTime, string strLine)
        {
            m_nIndex = nIndex;
            m_strName = strName;
            m_strIP = strIP;
            m_nPort = nPort;
            m_nTime = nTime;
            SynchronizationCom = false;
            m_strLine = strLine;
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
        /// 接收导数据是否处理数据
        /// </summary>
        public bool IsReceiveProess { get { return isReceiveProess; } set { isReceiveProess = value; } }

        private bool isReceiveProess = true;

        public string GetEndFlag()
        {
            return m_strLine;
        }

        /// <summary>
        /// 判断是否超时
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOut()
        {
            return m_bTimeOut;
        }

        public bool IsConnected
        {
            get
            {
                return m_client.Connected;
            }
        }

        private int m_nDelayTimeWhenErr = 0;

        private void DisconnectCallBack(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndDisconnect(ar);
                client = null;
            }
            catch (Exception ex)
            {
                if (SocketErrorEvent != null)
                {
                    SocketErrorEvent(this, new AsyTcpSocketEventArgs("关闭远程连接出现错误!"));
                }
            }
        }

        /// <summary>
        /// 关闭远程服务器连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                m_client.Client.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), this);
                //_AsySocket.Close();
            }
            catch (Exception ex)
            {
                if (SocketErrorEvent != null)
                {
                    SocketErrorEvent(this, new AsyTcpSocketEventArgs("准备关闭远程连接出现错误!"));
                }
                else
                {
                    throw new Exception("准备关闭远程连接出现错误!");
                }
            }
        }

        private byte[] rbuf = new byte[2048];
        private int nOffset = 0;

        /// <summary>
        ///  获取网口缓存数据    直到有结束符
        ///  /// </summary>
        /// <param name="count"></param>
        private void ReceiveProess(int count)
        {
            string receiveString = Encoding.Default.GetString(m_recvBuffer, 0, count);
            //  _logger.Info(DateTime.Now.ToString() + $"IP:{m_strIP} Port:{m_nPort} 结束符:{m_strLine} 接收字串:" + receiveString);
            int tsLength = m_strLine.Length;
            int indexofarr = -1;

            try
            {
                string cmdTarget = "";
                int indexofArr = 0;
                Array.Copy(m_recvBuffer, nOffset, rbuf, nOffset, count);
                nOffset += count;
                if (!CommunicateFun.CheckEndChar(rbuf, System.Text.Encoding.Default.GetBytes(m_strLine), out indexofArr))
                {
                }
                else
                {
                    try
                    {
                        byte[] bytearr = new byte[indexofArr];
                        Array.Copy(rbuf, bytearr, indexofArr);
                        this.RaiseDataReceived(bytearr);
                    }
                    catch (Exception ex)
                    {
                    }

                    nOffset = 0;
                    Array.Clear(rbuf, 0, rbuf.Length);
                    Array.Clear(m_recvBuffer, 0, m_recvBuffer.Length);
                }
            }
            catch (Exception es)
            {
                //连接断开
                nOffset = 0;
                Array.Clear(rbuf, 0, rbuf.Length);
            }

            //遍历字符串 直到结束符
            //for (int i = 0; i < receiveString.Length;)
            //{
            //    if (i <= receiveString.Length - tsLength)
            //    {
            //        if (receiveString.Substring(i, tsLength) != m_strLine)
            //        {
            //            sbReceiveDataBuffer.Append(receiveString[i]);
            //            i++;
            //        }
            //        else
            //        {
            //            try
            //            {
            //                _logger.Info(DateTime.Now.ToString() + $"IP:{m_strIP} Port:{m_nPort} 找到结束符{m_strLine} 字串:" + Encoding.Default.GetBytes(sbReceiveDataBuffer.ToString()));
            //                this.RaiseDataReceived(Encoding.Default.GetBytes(sbReceiveDataBuffer.ToString().ToCharArray()));
            //            }
            //            catch(Exception EX)
            //            {
            //            }
            //            finally
            //            {
            //                sbReceiveDataBuffer.Clear();
            //                i += tsLength;
            //            }

            //        }
            //    }
            //    else
            //    {
            //        sbReceiveDataBuffer.Append(receiveString[i]);
            //        i++;
            //    }
            //}
        }

        /// <summary>
        /// 接收数据操作回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void RecvCallBack(IAsyncResult ar)
        {
            TcpLink asyTcpSocket = null;

            try
            {
                asyTcpSocket = (TcpLink)ar.AsyncState;
                m_nDelayTimeWhenErr = 30;
                //获取异步SOCKET从网络上接收到的数据的数量
                int bytesRead = asyTcpSocket.m_client.Client.EndReceive(ar);

                //_logger.Warn($"{DateTime.Now.ToString()}:{m_strIP},{m_nPort}:Receive bytes:" + bytesRead);

                //if (bytesRead == 0)
                //{
                //    if (!asyTcpSocket.m_client.Client.Connected)
                //        asyTcpSocket.Disconnect();//20180616
                //    m_recvString = "";
                //    m_recvLength = 0;
                //    _logger.Warn($"{m_strIP},{m_nPort}:接收数据过程中出现错误，关闭连接!11");
                //    if (SocketErrorEvent != null)
                //    {
                //        SocketErrorEvent(this, new AsyTcpSocketEventArgs($"{m_strIP},{m_nPort}:接收数据过程中出现错误，关闭连接!"));
                //    }

                //}
                if (bytesRead > 0)
                {
                    m_nDelayTimeWhenErr = 30;
                    if (isReceiveProess)
                    {
                        //处理数据  获取结束符
                        ReceiveProess(bytesRead);
                    }
                    else
                    {
                        //不处理数据 给接收事件者处理
                        //根据接收到数据的数量动态申请控件来保存接收的数据
                        byte[] recvbuffer = new byte[bytesRead];
                        m_recvLength = bytesRead;
                        m_recvString = Encoding.UTF8.GetString(recvbuffer);//.Replace("\0",null);//.Trim();
                        if (m_recvString.Length > 0)
                            RaiseDataReceived(recvbuffer);
                    }
                }
            }
            //如果出现错误的话就释放所有的资源，这样能够保证对于下次的数据传输不会造成影响
            catch (Exception ex)
            {
                m_nDelayTimeWhenErr = 200;
                m_recvString = "";
                m_recvLength = 0;
                if (SocketErrorEvent != null)
                {
                    _logger.Warn($"{m_strIP},{m_nPort}:接收数据过程中出现错误!22");
                    SocketErrorEvent(this, new AsyTcpSocketEventArgs($"{m_strIP},{m_nPort}:接收数据过程中出现错误!"));
                }
                sbReceiveDataBuffer.Clear();
                Err($"{m_strIP},{m_nPort}:接收数据过程中出现错误!22");
                //  if (!asyTcpSocket.m_client.Client.Connected)
                //      asyTcpSocket.Disconnect();//20180616
            }
            finally
            {
                //重复效用回调函数，准备接收数据
                try
                {
                    if (m_nDelayTimeWhenErr != 0)
                        Thread.Sleep(m_nDelayTimeWhenErr);
                    m_nDelayTimeWhenErr = 0;
                    m_client.Client.BeginReceive(m_recvBuffer, 0, m_recvBuffer.Length, 0, new AsyncCallback(RecvCallBack), this);
                }
                catch (Exception ex)
                {
                    sbReceiveDataBuffer.Clear();
                    if (SocketErrorEvent != null)
                    {
                        _logger.Warn($"{m_strIP},{m_nPort}:准备接收数据过程中出现错误!33");
                        SocketErrorEvent(this, new AsyTcpSocketEventArgs($"{m_strIP},{m_nPort}:准备接收数据过程中出现错误!"));
                    }
                    if (m_client.Client.Connected)
                        m_client.Client.Disconnect(true);
                    m_client.Client.BeginConnect(m_strIP, m_nPort, new AsyncCallback(CallBackConnect), this);
                }
            }
        }

        public void StartRecvMessage()
        {
            try
            {
                m_client.Client.BeginReceive(m_recvBuffer, 0, m_recvBuffer.Length, 0, new AsyncCallback(RecvCallBack), this);
            }
            catch (Exception ex)
            {
                if (SocketErrorEvent != null)
                {
                    SocketErrorEvent(this, new AsyTcpSocketEventArgs($"{m_strIP},{m_nPort}:准备接收数据过程中出现错误!"));
                }
                else
                {
                    throw new Exception($"{m_strIP},{m_nPort}:准备接收数据过程中出现错误!");
                }
            }
        }

        private Task taskConnetAgain = null;

        /// <summary>
        ///网口打开时通过回调检测是否连接超时。 5秒种
        /// </summary>
        /// <param name="asyncResult"></param>
        private void CallBackConnect(IAsyncResult asyncResult)
        {
            try
            {
                m_bConnectSuccess = false;
                TcpLink tcpLink = asyncResult.AsyncState as TcpLink;
                //TcpClient tcpClient = asyncResult.AsyncState as TcpClient;
                TcpClient tcpClient = tcpLink.m_client;
                if (tcpClient.Client != null && tcpClient.Connected)
                {
                    tcpClient.EndConnect(asyncResult);
                    m_bConnectSuccess = true;
                    if (!SynchronizationCom)//异步通讯
                        StartRecvMessage();
                    TimeoutObject.Set();
                }
                else if (tcpClient.Client != null && !tcpClient.Connected)
                {
                    tcpClient.Client.Shutdown(SocketShutdown.Both);
                    tcpClient.Client.Disconnect(true);
                    tcpClient.EndConnect(asyncResult);
                    tcpClient.Client.BeginConnect(tcpLink.m_strIP, tcpLink.m_nPort, new AsyncCallback(CallBackConnect), tcpLink);
                    tcpLink.m_count++;
                }
            }
            catch (Exception ex)
            {
                m_bConnectSuccess = false;
                socketException = ex;

                TcpLink tcpLink = asyncResult.AsyncState as TcpLink;
                TcpClient tcpClient = tcpLink.m_client;
                ConnectServer();
                tcpLink.m_count++;
            }
            finally
            {
                //TimeoutObject.Set();
            }
        }

        private int nIsReconnecting = 0;//是否重连中;

        //连接服务器
        public void ConnectServer()
        {
            Task.Run(() =>
            {
                Interlocked.CompareExchange(ref nIsReconnecting, 1, 0);
                if (nIsReconnecting == 0)
                    return;
                while (!m_client.Connected)
                {
                    try
                    {
                        m_client.Close();
                        m_client.Dispose();
                        m_client = new TcpClient();
                        m_client.Connect(IPAddress.Parse(m_strIP), m_nPort);
                        if (m_client.Connected)
                        {
                            m_client.Close();
                            m_client = new TcpClient();
                            m_client.BeginConnect(m_strIP, m_nPort, new AsyncCallback(CallBackConnect), this);
                        }
                    }
                    catch (Exception ex2)
                    {
                        _logger.Warn($"{m_strIP},{m_nPort}:重新链接服务器中 每隔5s链接一次");
                    }
                    Thread.Sleep(5000);
                }
                Interlocked.CompareExchange(ref nIsReconnecting, 0, 1);
            });
        }

        /// <summary>
        ///打开网口
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            if (m_client == null)
            {
                m_client = new TcpClient();
            }
            if (m_client.Connected == true)
            {
                m_client.Client.Shutdown(SocketShutdown.Both);
                m_client.Client.Close();
            }
            if (m_client.Connected == false)
            {
                m_client.SendBufferSize = 4096;
                m_client.SendTimeout = m_nTime;
                m_client.ReceiveTimeout = m_nTime;
                m_client.ReceiveBufferSize = 4096;

                try
                {
                    TimeoutObject.Reset();
                    socketException = null;
                    m_client.BeginConnect(m_strIP, m_nPort, new AsyncCallback(CallBackConnect), this);

                    if (TimeoutObject.WaitOne(100000, false))
                    {
                        if (m_bConnectSuccess)
                        {
                            if (StateChangedEvent != null)
                                StateChangedEvent(this);
                            return m_client.Connected;
                        }
                        else
                            throw socketException;
                    }
                    else
                    {
                        throw new TimeoutException("TimeOut Exception");
                    }

                    //     m_client.Connect(m_strIP, m_nPort);
                }
                catch (Exception e)
                {
                    m_bTimeOut = true;
                    Debug.WriteLine(string.Format("{0}:{1}{2}\r\n", m_strIP, m_nPort, e.Message));

                    if (StateChangedEvent != null)
                        StateChangedEvent(this);
                }
            }
            return m_client.Connected;
        }

        /// <summary>
        /// 判断网口是否打开
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return m_client != null && m_client.Connected;
        }

        private void CallBackSend(IAsyncResult iar)
        {
            try
            {
                TcpLink tcpLink = iar.AsyncState as TcpLink;
                // ((Socket)iar.AsyncState).EndSend(iar);
                tcpLink.m_client.Client.EndSend(iar);
            }
            catch (Exception ex)
            {
                m_nDelayTimeWhenErr += 200;
                if (m_ArrSendByte != null && m_ArrSendByte.Length > 0)
                {
                    Thread.Sleep(m_nDelayTimeWhenErr);
                    WriteData(m_ArrSendByte, m_ArrSendByte.Length);
                    //   ShowLog(ex.Message);
                }
            }
        }

        /// <summary>
        ///向网口写入数据
        /// </summary>
        /// <param name="sendBytes"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public bool WriteData(byte[] sendBytes, int nLen)
        {
            if (m_client.Connected)
            {
                if (SynchronizationCom)
                {
                    NetworkStream netStream = m_client.GetStream();
                    if (netStream.CanWrite)
                    {
                        netStream.Write(sendBytes, 0, nLen);
                        ShowLog(System.Text.Encoding.Default.GetString(sendBytes));
                    }
                    //netStream.Close();
                    return true;
                }
                else
                {
                    //异步发送数据

                    m_ArrSendByte = sendBytes;

                    //开始异步发送
                    m_client.Client.BeginSend(m_ArrSendByte, 0, m_ArrSendByte.Length, SocketFlags.None, new AsyncCallback(CallBackSend), this);
                    string strMsg = System.Text.Encoding.Default.GetString(m_ArrSendByte);
                    //   ShowLog(strMsg);
                }
            }
            else
            {
                File.AppendAllLines("D:\\SmallMes_CToS_ConnentError.txt", new string[] { DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ":ConnentError" });

                ConnectServer();
            }

            return false;
        }

        public void WriteData2(byte[] sendBytes, int nLen)
        {
            string cmd = "";
            int nRetryCount = 0;
        retrySend:
            try
            {
                m_client.Client.Send(sendBytes);
            }
            catch (Exception ex)
            {
                nRetryCount++;
                if (nRetryCount > 3)
                {
                    cmd = Encoding.Default.GetString(sendBytes);
                    File.AppendAllLines("D:\\SmallMes_CToS_SendError.txt", new string[] {  DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")+":"+
                    cmd+","+ex.Message });
                    return;
                }
                else
                {
                    goto retrySend;
                }
            }
        }

        /// <summary>
        ///向网口写入字符串
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public bool WriteString(string strData)
        {
            if (m_client.Connected)
            {
                NetworkStream netStream = m_client.GetStream();
                if (netStream.CanWrite)
                {
                    Byte[] sendBytes = Encoding.UTF8.GetBytes(strData);
                    netStream.Write(sendBytes, 0, sendBytes.Length);
                    ShowLog(strData);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        ///向网口写入一行字符
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public bool WriteLine(string strData)
        {
            if (m_client.Connected)
            {
                NetworkStream netStream = m_client.GetStream();
                if (netStream.CanWrite)
                {
                    Byte[] sendBytes = Encoding.UTF8.GetBytes(strData + m_strLine);
                    netStream.Write(sendBytes, 0, sendBytes.Length);
                    ShowLog(strData);
                }
                //netStream.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        ///从网口读取数据
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public int ReadData(byte[] bytes, int nLen)
        {
            m_bTimeOut = false;
            int n = 0;
            if (m_client.Connected)
            {
                try
                {
                    NetworkStream netStream = m_client.GetStream();
                    if (netStream.CanRead)
                    {
                        n = netStream.Read(bytes, 0, nLen);
                        if (n > 0)
                        {
                            ShowLog(System.Text.Encoding.Default.GetString(bytes));
                        }
                    }
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
        ///从网口读取一行数据
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public int ReadLine(out string strData)
        {
            m_bTimeOut = false;
            strData = "";
            if (m_client.Connected)
            {
                try
                {
                    NetworkStream netStream = m_client.GetStream();
                    if (netStream.CanRead)
                    {
                        byte[] bytes = new byte[m_client.ReceiveBufferSize];
                        int n = netStream.Read(bytes, 0, (int)m_client.ReceiveBufferSize);
                        strData = Encoding.UTF8.GetString(bytes, 0, n);
                        if (strData.Length > 0)
                        {
                            ShowLog(strData);
                        }
                    }
                }
                catch /*(TimeoutException e)*/
                {
                    m_bTimeOut = true;
                    if (StateChangedEvent != null)
                        StateChangedEvent(this);
                }
            }
            return strData.Length;
        }

        /// <summary>
        ///关闭网口
        /// </summary>
        public void Close()
        {
            if (m_client != null)
            {
                if (m_client.Connected)
                {
                    // NetworkStream netStream = m_client.GetStream();
                    // netStream.Close();
                }
                m_client.Close();

                m_client = null;
                m_bTimeOut = false;
                RecvStringMessageEvent = null;
                SocketErrorEvent = null;
                BeginRecvBufferEvent = null;
                RecvBufferEvent = null;
                if (StateChangedEvent != null)
                    StateChangedEvent(this);
            }
        }

        /// <summary>
        /// 清除缓冲区
        /// </summary>
        public void ClearBuffer()
        {
            if (m_client != null)
            {
                NetworkStream netStream = m_client.GetStream();
                m_client.GetStream().Flush();
                netStream.Close();
            }
        }
    }
}