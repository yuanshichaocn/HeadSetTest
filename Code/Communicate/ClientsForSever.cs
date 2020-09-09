#define _Not_Uset_NS_
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.IO;
using log4net;

namespace Communicate
{
    /// <summary>
    /// The class that contains some methods and properties to manage the remote clients.
    /// </summary>
    /// 

    /// <summary>
    /// The class that contains information about received command.
    /// </summary>
    public class CommandEventArgs : EventArgs
    {
        private string command;
        /// <summary>
        /// The received command.
        /// </summary>
        public string Command
        {
            get { return command; }
        }

        /// <summary>
        /// Creates an instance of CommandEventArgs class.
        /// </summary>
        /// <param name="cmd">The received command.</param>
        public CommandEventArgs(string cmd)
        {
            this.command = cmd;
        }
    }


    /// <summary>
    /// Client event args.
    /// </summary>
    public class ClientEventArgs : EventArgs
    {
        private Socket socket;
        /// <summary>
        /// The ip address of remote client.
        /// </summary>
        public IPAddress IP
        {
            get { return ((IPEndPoint)this.socket.RemoteEndPoint).Address; }
        }
        /// <summary>
        /// The port of remote client.
        /// </summary>
        public int Port
        {
            get { return ((IPEndPoint)this.socket.RemoteEndPoint).Port; }
        }
        /// <summary>
        /// Creates an instance of ClientEventArgs class.
        /// </summary>
        /// <param name="clientManagerSocket">The socket of server side socket that comunicates with the remote client.</param>
        public ClientEventArgs(Socket clientManagerSocket)
        {
            this.socket = clientManagerSocket;
        }
    }
    public class ClientManager
    {
        private readonly ILog _logger = LogManager.GetLogger(nameof(ClientManager));
        /// <summary>
        /// Gets the IP address of connected remote client.This is 'IPAddress.None' if the client is not connected.
        /// </summary>
        public IPAddress IP
        {
            get
            {
                if (this.socket != null)
                    return ((IPEndPoint)this.socket.RemoteEndPoint).Address;
                else
                    return IPAddress.None;
            }
        }
        /// <summary>
        /// Gets the port number of connected remote client.This is -1 if the client is not connected.
        /// </summary>
        public int Port
        {
            get
            {
                if (this.socket != null)
                    return ((IPEndPoint)this.socket.RemoteEndPoint).Port;
                else
                    return -1;
            }
        }
        /// <summary>
        /// [Gets] The value that specifies the remote client is connected to this server or not.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (this.socket != null)
                    return this.socket.Connected;
                else
                    return false;
            }
            set
            {
                this.Connected = value;
            }
        }

        private Socket socket;
        private string clientName;
        /// <summary>
        /// The name of remote client.
        /// </summary>
        public string ClientName
        {
            get { return this.clientName; }
            set { this.clientName = value; }
        }
#if _Not_Uset_NS_

#else
        NetworkStream networkStream;
#endif//endif _Not_Uset_NS_
        private BackgroundWorker bwReceiver;

        #region EVENT

        /// <summary>
        /// Occurs when a remote client had been disconnected from the server.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">The client information.</param>
        public delegate void DisconnectedEventHandler(object sender, ClientEventArgs e);

        /// <summary>
        /// Occurs when a client disconnected from this server.
        /// </summary>
        public event DisconnectedEventHandler Disconnected;
        /// <summary>
        /// Occurs when a client disconnected from this server.
        /// </summary>
        /// <param name="e">Client information.</param>
        protected virtual void OnDisconnected(ClientEventArgs e)
        {
            if (Disconnected != null)
                Disconnected(this, e);
        }


        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">The received command object.</param>
        public delegate void CommandReceivedEventHandler(object sender, CommandEventArgs e);
        /// <summary>
        /// Occurs when a command received from a remote client.
        /// </summary>
        public event CommandReceivedEventHandler CommandReceived;
        /// <summary>
        /// Occurs when a command received from a remote client.
        /// </summary>
        /// <param name="e">Received command.</param>
        protected virtual void OnCommandReceived(CommandEventArgs e)
        {
            if (CommandReceived != null)
                CommandReceived(this, e);
        }

        /// <summary>
        /// Occurs when a command had been sent to the remote client successfully.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        public delegate void CommandSentEventHandler(object sender, EventArgs e);


        /// <summary>
        /// Occurs when a command sending action had been failed.This is because disconnection or sending exception.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        public delegate void CommandSendingFailedEventHandler(object sender, EventArgs e);


        #endregion

        #region Constructor
        /// <summary>
        /// Creates an instance of ClientManager class to comunicate with remote clients.
        /// </summary>
        /// <param name="clientSocket">The socket of ClientManager.</param>
        public ClientManager(Socket clientSocket)
        {
            this.socket = clientSocket;
#if _Not_Uset_NS_
#else
            this.networkStream = new NetworkStream(this.socket);
#endif//endif _Not_Uset_NS_
            this.bwReceiver = new BackgroundWorker();
            this.bwReceiver.DoWork += new DoWorkEventHandler(StartReceive);
            this.bwReceiver.RunWorkerAsync();
        }
        #endregion
        #region serverReceive
        private void StartReceive(object sender, DoWorkEventArgs e)
        {
            int sleecount = 0;
            while (this.socket.Connected)
            {
#if _Not_Uset_NS_
                #region   直接使用Socket收数据
                if (socket != null && socket.Connected)
                {
                    if (socket.Poll(1, SelectMode.SelectRead))
                    {
                        try
                        {
                            byte[] buffer = new byte[1024];
                            int readBytes = socket.Receive(buffer);
                            if (readBytes == 0)
                            {
                                //连接断开
                                break;
                            }
                            else
                            {
                                byte[] rbuf = new byte[readBytes];
                                Array.Copy(buffer, rbuf, readBytes);
                                string cmdTarget =IP.ToString()+"#"+Port.ToString()+"#"+ System.Text.Encoding.Default.GetString(rbuf);
                                this.OnCommandReceived(new CommandEventArgs(cmdTarget));
                            }
                        }
                        catch
                        {
                            //连接断开
                            break;
                        }
                    }
                }//end if socket is OK and data is OK

                #endregion
#else
                if (networkStream.DataAvailable)
                {
                    //if (this.networkStream.Length != 0)
                    //{
                    int temp = 2048;//256;
                                    //  temp = (int)this.networkStream.Length;
                    byte[] buffer = new byte[temp];
                    int readBytes = this.networkStream.Read(buffer, 0, temp);
                    if (readBytes == 0)
                        break;
                    string cmdTarget = "";

                    byte[] rbuf = new byte[readBytes];

                    Array.Copy(buffer, rbuf, readBytes);
                    cmdTarget = System.Text.Encoding.Default.GetString(rbuf);
                    this.OnCommandReceived(new CommandEventArgs(cmdTarget.Trim()));
                    //}
                }
#endif//endif _Not_Uset_NS_
                //try
                //{
                //    if (sleecount >= 100)
                //    {
                //        sleecount = 0;
                //    }
                //}
                //catch (System.Exception)
                //{

                //    break;
                //}
                System.Threading.Thread.Sleep(1);
                //sleecount++;

            }
            this.OnDisconnected(new ClientEventArgs(this.socket));
            this.Disconnect();
        }
        #endregion
        #region ServerSend
        /// <summary>
        /// Sends a command to the remote client if the connection is alive.
        /// </summary>
        /// <param name="cmd">The command to send.</param>
        public void SendCommand(string cmd)
        {
            if (this.socket != null && this.socket.Connected)
            {
                BackgroundWorker bwSender = new BackgroundWorker();
                bwSender.DoWork += new DoWorkEventHandler(bwSender_DoWork);
                bwSender.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSender_RunWorkerCompleted);
                bwSender.RunWorkerAsync(cmd);
            }
            else
                this.OnCommandFailed(new EventArgs());
        }
        private void bwSender_DoWork(object sender, DoWorkEventArgs e)
        {
            string cmd = (string)e.Argument;
            e.Result = this.SendCommandToClient(cmd);
        }

        //This Semaphor is to protect the critical section from concurrent access of sender threads.
        System.Threading.Semaphore semaphor = new System.Threading.Semaphore(1, 1);
        private bool SendCommandToClient(string cmd)
        {
            try
            {
                semaphor.WaitOne();
                File.AppendAllLines("D:\\1.txt", new string[] {  DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")+":"
                    + this.IP.ToString()+","+this.Port.ToString()+":"+cmd });
                //Meta Data.
            
                if (cmd == null || cmd == "")
                    cmd = "\n";
                cmd = cmd + "\r\n";
                byte[] metaBuffer = Encoding.UTF8.GetBytes(cmd);
#if _Not_Uset_NS_
                socket.Send(metaBuffer);
#else
                this.networkStream.Write(metaBuffer, 0, metaBuffer.Length);
                this.networkStream.Flush();
#endif//endif _Not_Uset_NS_
                semaphor.Release();
                return true;
            }
            catch
            {
                semaphor.Release();
                return false;
            }
        }
        private void bwSender_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null && ((bool)e.Result))
                this.OnCommandSent(new EventArgs());
            else
                this.OnCommandFailed(new EventArgs());

            ((BackgroundWorker)sender).Dispose();
            GC.Collect();
        }

        /// <summary>
        /// Occurs when a command had been sent to the remote client successfully.
        /// </summary>
        public event CommandSentEventHandler CommandSent;
        /// <summary>
        /// Occurs when a command had been sent to the remote client successfully.
        /// </summary>
        /// <param name="e">The sent command.</param>
        protected virtual void OnCommandSent(EventArgs e)
        {
            if (CommandSent != null)
                CommandSent(this, e);
        }

        /// <summary>
        /// Occurs when a command sending action had been failed.This is because disconnection or sending exception.
        /// </summary>
        public event CommandSendingFailedEventHandler CommandFailed;
        /// <summary>
        /// Occurs when a command sending action had been failed.This is because disconnection or sending exception.
        /// </summary>
        /// <param name="e">The sent command.</param>
        protected virtual void OnCommandFailed(EventArgs e)
        {
            if (CommandFailed != null)
                CommandFailed(this, e);
        }
        #endregion
        /// <summary>
        /// Disconnect the current client manager from the remote client and returns true if the client had been disconnected from the server.
        /// </summary>
        /// <returns>True if the remote client had been disconnected from the server,otherwise false.</returns>
        public bool Disconnect()
        {
            if (this.socket != null && this.socket.Connected)
            {
                try
                {
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return true;
        }
    }
}