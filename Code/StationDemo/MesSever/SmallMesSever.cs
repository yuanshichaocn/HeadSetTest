using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTools;
using BaseDll;
using MotionIoLib;
using UserData;
using System.Threading;
using System.Collections.Concurrent;
using Communicate;
using Newtonsoft.Json;
using log4net;
using SmallMesClients;

namespace StationDemo
{
    public class cmdofMes
    {
        public string ip;
        public string port;
      
        public string cmd;
        public ClientManager Client;
      
    }
    public class SmallMes
    {

        private static readonly SmallMes smallMes = new SmallMes();
        public static SmallMes Ins { get=>   smallMes;}
        /// <summary>
        /// 机台命和对应的链接
        /// </summary>
        ConcurrentDictionary<string, ClientManager> Clients = new ConcurrentDictionary<string, ClientManager>();
        ConcurrentDictionary<string, MachineInfo> MachineClients = new ConcurrentDictionary<string, MachineInfo>();
        public delegate void ShowStateOfMes(string strClientIp, string state);
        public event ShowStateOfMes eventOfMes = null;

        SocketSever socketSever = new SocketSever();
        private Queue<cmdofMes> cmdofMess = new Queue<cmdofMes>();
        private object objlock = new object();
        public void EnteryUserQueue(cmdofMes cmdofMes)
        {
            lock(objlock)
            {
                cmdofMess.Enqueue(cmdofMes);
            }

        }
        public void ExcQueueCmd()
        {
            Task.Run(() =>
            {
                cmdofMes cmdofMesobj = OutQueue();
                if (cmdofMesobj == null)
                    return;
                MachineInfo machineInfo =null;

                MesCmd mesCmdObj = JsonConvert.DeserializeObject<MesCmd>(cmdofMesobj.cmd);
                MesCmd mesCmdObjInstace =JsonConvert.DeserializeObject(cmdofMesobj.cmd, mesCmdObj.CmdType) as MesCmd;
                mesCmdObjInstace.Oprate(MachineClients, cmdofMesobj.Client);
           

            });
          
        }
        public cmdofMes OutQueue()
        {
            lock (objlock)
            {
                cmdofMes cmdofMesobj = null;
                if (cmdofMess.Count> 0)
                  cmdofMesobj =  cmdofMess.Dequeue();
                return cmdofMesobj;
            }

        }

        public void SeverInit()
        {
            socketSever.ClientConnected += (ClientManager clientManager) => {
                ParamSetMgr.GetInstance().SetBoolParam(socketSever.IP.ToString(), true);
                Clients.AddOrUpdate(clientManager.IP.ToString() + "#" + clientManager.Port.ToString(), clientManager, (key, value) => { return value = clientManager; });

                if (eventOfMes != null)
                {
                      eventOfMes(socketSever.IP.ToString(), "链接成功");
                }
            };
            socketSever.ClientDisConnected += (ClientManager clientManager) => {
                ParamSetMgr.GetInstance().SetBoolParam(socketSever.IP.ToString(), true);
                ParamSetMgr.GetInstance().SetBoolParam(socketSever.IP.ToString(), false);
                Clients.TryRemove(clientManager.IP.ToString() + "#" + clientManager.Port.ToString(), out ClientManager clientManager2);
                if (eventOfMes != null)
                {
                   
                    eventOfMes(socketSever.IP.ToString(), "断开成功");
                }
            };
            socketSever.ProcessData += (string mes) =>
            {
                string[] split = mes.Split(new string[] { "#",  }, StringSplitOptions.RemoveEmptyEntries);
                if (split == null) return;
                //if (split.Length < 5) return;
                int index = 0;
                string ipName = split[index++];
                string portName = split[index++];
                string Cmd = split[index++];
            
                cmdofMes cmdofMes = new cmdofMes() {
                    ip = ipName,
                    port = portName,
                    cmd = Cmd,

                };
               

                ClientManager clientManager = null;
                Clients.TryGetValue(ipName + "#" + portName, out  clientManager) ;
                if (clientManager == null)
                    Clients.AddOrUpdate(clientManager.IP.ToString() + "#" + clientManager.Port.ToString(), clientManager, (key, value) => { return value = clientManager; });
                cmdofMes.Client = clientManager;
                
                EnteryUserQueue(cmdofMes);
                ExcQueueCmd();

            };
            socketSever.Init("127.0.0.1",5000);
        }

    
    }

}