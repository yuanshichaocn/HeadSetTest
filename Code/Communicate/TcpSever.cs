using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace Communicate
{
    public class SocketSever
    {
        private ClientManager _clientManager;
        private readonly ILog _logger = LogManager.GetLogger(nameof(SocketSever));
        private ConcurrentDictionary<string, ClientManager> dicClientMgr = new ConcurrentDictionary<string, ClientManager>();
       
        private Socket _socket;
        private BackgroundWorker _background;

        public Action<bool> ConnectEvent;
        public AutoResetEvent Event = new AutoResetEvent(false);
        public string IP;
        public int Port;
        public string ReceiveStr;

        //回调委托

        public delegate void DeleagteProcessData(string data);

        public delegate void DeleagteClientConnected( ClientManager client);


        public DeleagteProcessData ProcessData = null;//处理收到的消息
        public DeleagteClientConnected ClientConnected = null; //处理连接建立
        public DeleagteClientConnected ClientDisConnected = null;//处理连接断开

        public void Init(string ip, int port)
        {
            IP = "127.0.0.1";
            Port = port;
            _background = new BackgroundWorker();
            _background.WorkerSupportsCancellation = true;
            _background.DoWork += StartToListen;
            _background.RunWorkerAsync();
        }

        private void StartToListen(object sender, DoWorkEventArgs e)
        {
            try
            {
                //循环监听客户端状态，如客户端退出，此处继续等待客户端新的链接。
                IPAddress serverIP = IPAddress.Any;//IPAddress.Parse(IP);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(new IPEndPoint(serverIP, Port));
                _socket.Listen(200);
                while (true)
                {
                    Socket sockTmp = _socket.Accept();
                    _clientManager = new ClientManager(sockTmp);
                    _clientManager.CommandReceived += CommandReceived;
                    _clientManager.Disconnected    += ClientManagerOnDisconnected;
                    _clientManager.CommandFailed   += (obj1, obj2) =>
                    {
                        _logger.Info(_clientManager.IP.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ":" + "发送失败");
                        MessageBox.Show(_clientManager.IP.ToString()+"发送失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    };
                    dicClientMgr.TryAdd(_clientManager.IP.ToString()+"#"+_clientManager.Port.ToString(), _clientManager);
                    ConnectEvent?.Invoke(true);
                    if (ClientConnected != null)
                    {
                        ClientConnected(_clientManager);
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void ClientManagerOnDisconnected(object sender, ClientEventArgs clientEventArgs)
        {
            string stripport = clientEventArgs.IP.ToString() + "#" + clientEventArgs.Port.ToString();
            ClientManager clientManager = null;
            if (dicClientMgr.ContainsKey(stripport))
            {
                dicClientMgr.TryRemove(stripport,out   clientManager);
                clientManager.Disconnect();
            }
          
            ConnectEvent?.Invoke(false);
            if (ClientDisConnected != null)
            {
                ClientDisConnected(clientManager);
            }
          
        }

        private void CommandReceived(object sender, CommandEventArgs e)
        {
            ReceiveStr = e.Command;
            Event.Set();
            if (ProcessData != null)
            {
                ProcessData(ReceiveStr);
            }
        }

        public void Send(string info)
        {
            ReceiveStr = "";
            Event.Reset();
            _clientManager.SendCommand(info);
        }

        public void Send(string ip,string port,string info)
        {
            ReceiveStr = "";
            Event.Reset();
            if(dicClientMgr.ContainsKey(ip+"#"+port))
            {
                _clientManager = dicClientMgr[ip + "#" + port];
                _clientManager.SendCommand(info);
            }
          //  
        }
        public void Stop()
        {
            if (_clientManager != null)
            {
                _clientManager.Disconnect();
            }
        }
        public string []  GetClients()
        {
            string[] arrClients = new string[dicClientMgr.Count];
            int i = 0;
           foreach(var tem in  dicClientMgr)
                arrClients[i++] = tem.Value.IP.ToString() + "#" + tem.Value.Port.ToString();
            return arrClients;
        }
       

        public bool IsConnect => (_clientManager == null ? false : _clientManager.Connected);
    }
    public class SocketSeverMgr
    {
        static object lockObj = new object();
        public static SocketSeverMgr socketMgr;
        private SocketSeverMgr()
            {
            }
        public   static SocketSeverMgr GetInstace()
        {
            if(socketMgr==null)
            {
                lock(lockObj)
                {
                    if (socketMgr == null)
                    {
                        socketMgr = new SocketSeverMgr();
                    }
                }
            }
            return socketMgr;
        }
        public Dictionary<string, SocketSever> dicSever = new Dictionary<string, SocketSever>();
        public void  Add(string strname, SocketSever socketSever)
        {
            if (dicSever.ContainsKey(strname))
            {
                dicSever[strname] = socketSever;
            }
            else
                dicSever.Add(strname, socketSever);
        }
        public SocketSever  GetSever(string strname)
        {
            if (dicSever.ContainsKey(strname))
                return dicSever[strname];
            else
                return null;
        }


    }

}
