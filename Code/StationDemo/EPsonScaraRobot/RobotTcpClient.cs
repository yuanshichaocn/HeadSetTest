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
       
        public void Connect()
        {
            Client?.Connect(IPAddress.Parse(Parameter.IpAddress), Parameter.PortNum);
        }

        public void Close()
        {
            Client?.Close();
        }

        public string SendCommand(string cmd)
        {
            try
            {
                var client = Client.Client;
                client.Send(Encoding.ASCII.GetBytes(cmd + Parameter.EndString));

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
                    var bytesRead = client.Receive(readBuffer, readBuffer.Length,SocketFlags.None);

                    if (readBuffer[bytesRead - 2] == 0x0D && readBuffer[bytesRead - 1] == 0x0A)
                        return Encoding.ASCII.GetString(readBuffer, 0, bytesRead - 2);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("指令发送失败," + ex.Message, ex);
            }
        }

    }
}
