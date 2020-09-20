using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using log4net;

namespace EpsonRobot
{
    public class RobotTcpClient
    {
        private readonly ILog _logger = LogManager.GetLogger(nameof(RobotTcpClient));
        public TcpClient Client { get; private set; }

        public TcpParameter Parameter { get; set; }

        public void Init(string configFilePath)
        {
            Parameter = TcpParameter.Load(configFilePath);
            Client = new TcpClient();
        }

        public bool bIsConnected
        {
            get => Client.Connected;
        }

        public void Connect()
        {
            Client?.Connect(IPAddress.Parse(Parameter.IpAddress), Parameter.PortNum);
        }

        public void Close()
        {
            Client?.Close();
        }
        object lockSendObj = new object();
        byte[] ReciveData = new byte[1024];

        public string SendCommand(string cmd, bool bIgnorRtn = false)
        {

            lock (lockSendObj)
            {

                try
                {
                    int offset = 0;
                    var client = Client.Client;
                    client.Send(Encoding.ASCII.GetBytes(cmd + Parameter.EndString));
                    if (bIgnorRtn)
                        return "0";
                    byte[] readBuffer = new byte[1024];
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (true)
                    {
                        if (stopwatch.ElapsedMilliseconds >= Parameter.Timeout)
                        {
                            _logger.Warn("Wait response timeout");
                            stopwatch.Stop();
                            throw new TimeoutException($"等待{Parameter.Name}的响应超时");
                        }
                        if (client.Available == 0)
                        {
                            System.Threading.Thread.Sleep(1);
                            continue;
                        }
                        System.Threading.Thread.Sleep(10);

                        if (client.Poll(1, SelectMode.SelectRead))
                        {
                            var bytesRead = client.Receive(ReciveData, offset,readBuffer.Length, SocketFlags.None);
                            offset += bytesRead;
                            if (offset >= 2 && ReciveData[offset - 2] == 0x0D && ReciveData[offset - 1] == 0x0A)
                                return Encoding.ASCII.GetString(ReciveData, 0, offset - 2);
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("指令发送失败," + ex.Message, ex);
                }
            }
         
        }

    }
}
