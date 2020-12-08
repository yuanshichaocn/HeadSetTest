using BaseDll;
using Communicate;
using Newtonsoft.Json;
using SmallMesClients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace StationDemo
{
    public class cmdofMes
    {
        public string ip;
        public string port;

        public string cmd;
        public ClientManager Client;
    }

    public class CmdFromClient
    {
        public MesCmd cmd;
        public ClientManager Client;
    }

    public class userQueueSever
    {
        private Queue<CmdFromClient> queue = new Queue<CmdFromClient>();
        private Queue<string> queueKey = new Queue<string>();
        private object objlock = new object();
        public Dictionary<string, CmdFromClient> CmdFromClientCmds = new Dictionary<string, CmdFromClient>();

        public CmdFromClient Take()
        {
            lock (objlock)
            {
                if (queue.Count > 0)
                {
                    string str = queueKey.Dequeue();
                    if (CmdFromClientCmds.ContainsKey(str))
                        CmdFromClientCmds.Remove(str);
                    return queue.Dequeue();
                }
                else return null;
            }
        }

        public void Add(CmdFromClient obj)
        {
            lock (objlock)
            {
                if (obj != null && obj.Client != null && obj.cmd != null)
                {
                    string str = obj.Client.IP.ToString() + "#" + obj.cmd.strCmdCode;
                    if (CmdFromClientCmds.ContainsKey(str))
                    {
                        CmdFromClientCmds[str] = obj;
                    }
                    else
                    {
                        CmdFromClientCmds.Add(str, obj);
                        queue.Enqueue(obj);
                        queueKey.Enqueue(str);
                    }
                }
            }
        }

        public void Clear()
        {
            lock (objlock)
            {
                queue.Clear();
            }
        }
    }

    public enum MachineName
    {
        前壳锁付,
        镜头锁付,
        Plasma,
        AA,
        固化,
    }

    public class SmallMes : LogView
    {
        public static List<Type> GetAllSubClassType(Type basetype, string assemblypath)
        {
            List<Type> SubClassTypeName = new List<Type>();
            Assembly assembly = Assembly.LoadFile(assemblypath);
            var types = assembly.GetTypes();
            var baseType = basetype;

            foreach (var t in types)
            {
                var tmp = t.BaseType;
                int i = 0;
                while (tmp != null)
                {
                    if (tmp.Name == baseType.Name)
                    {
                        SubClassTypeName.Add(t);

                        break;
                    }
                    else
                    {
                        tmp = tmp.BaseType;
                    }
                }
            }
            if (SubClassTypeName.Count == 0)
                return null;
            else
                return SubClassTypeName;
        }

        private static readonly SmallMes smallMes = new SmallMes();
        public static SmallMes Ins { get => smallMes; }

        /// <summary>
        /// 机台命和对应的链接
        /// </summary>
        private ConcurrentDictionary<string, ClientManager> Clients = new ConcurrentDictionary<string, ClientManager>();

        private ConcurrentDictionary<string, MachineInfo> MachineClients = new ConcurrentDictionary<string, MachineInfo>();

        public delegate void ShowStateOfMes(string strClientIp, string state);

        public event ShowStateOfMes eventOfMes = null;

        private List<Type> types = null;

        private SmallMes()
        {
            types = GetAllSubClassType(typeof(MesCmd), AppDomain.CurrentDomain.BaseDirectory + "SmallMesClients.dll");
        }

        private Type FindCmdType(string typefullname)
        {
            if (types != null && types.Count > 0)
            {
                Type type = types.Find(t => t.FullName == typefullname);
                return type;
            }
            return null;
        }

        private SocketSever socketSever = new SocketSever();
        private Queue<cmdofMes> cmdofMess = new Queue<cmdofMes>();
        private userQueue<CmdFromClient> userQueue = new userQueue<CmdFromClient>();
        private userQueueSever userQueueSever = new userQueueSever();
        private object objlock = new object();

        public void EnteryUserQueue(cmdofMes cmdofMes)
        {
            lock (objlock)
            {
                cmdofMess.Enqueue(cmdofMes);
            }
        }

        public void ExcQueueCmd()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    //    cmdofMes cmdofMesobj = OutQueue();
                    //    if (cmdofMesobj == null)
                    //        continue;
                    //    MesCmd mesCmdObj = null;
                    //    try
                    //    {
                    //         mesCmdObj = JsonConvert.DeserializeObject<MesCmd>(cmdofMesobj.cmd);
                    //    }
                    //    catch(Exception exs)
                    //    {
                    //        continue;
                    //    }

                    //    if (mesCmdObj != null)
                    //    {
                    //        Type type = FindCmdType(mesCmdObj.CmdType);
                    //        if (type != null)
                    //        {
                    //            try
                    //            {
                    //                MesCmd mesCmdObjInstace = JsonConvert.DeserializeObject(cmdofMesobj.cmd, type) as MesCmd;
                    //                mesCmdObjInstace.Oprate(MachineClients, cmdofMesobj.Client);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                string timestr = DateTime.Now.ToString("yyyy - MM - dd - hh - mm - ss");
                    //                string strMes = $"[{ timestr}]" + cmdofMesobj.Client.ClientName + $":{ex} ";
                    //                File.AppendAllLines("D:\\ErrSeverInfo.text", new string[] { strMes });
                    //            }
                    //        }
                    //        else
                    //        {
                    //        }
                    //    }
                    //    else
                    //    {
                    //    }
                    CmdFromClient cmd = userQueueSever.Take();
                    // ConcurrentDictionary<string, MachineInfo> temp = new ConcurrentDictionary<string, MachineInfo>();

                    //if (cmd != null && cmd.cmd != null)
                    //  {
                    //      cmd.cmd.Oprate(MachineClients, cmd.cmd, cmd.Client);
                    //  }
                    ExcCmd(cmd, MachineClients);
                }
            }
            );
        }

        public void ExcCmd(CmdFromClient cmd, ConcurrentDictionary<string, MachineInfo> MachineClients)
        {
            Task.Run(() =>
            {
                if (cmd != null && cmd.cmd != null)
                {
                    cmd.cmd.Oprate(MachineClients, cmd.cmd, cmd.Client);
                }
            });
        }

        public cmdofMes OutQueue()
        {
            lock (objlock)
            {
                cmdofMes cmdofMesobj = null;
                if (cmdofMess.Count > 0)
                    cmdofMesobj = cmdofMess.Dequeue();
                return cmdofMesobj;
            }
        }

        private void InfoClientConnected(ClientManager clientManager)
        {
            ParamSetMgr.GetInstance().SetBoolParam(socketSever.IP.ToString(), true);
            Clients.AddOrUpdate(clientManager.IP.ToString(), clientManager, (key, value) => { return value = clientManager; });

            if (eventOfMes != null)
            {
                eventOfMes(socketSever.IP.ToString(), "链接成功");
            }
            Info(clientManager.IP.ToString() + ":链接成功");
        }

        private MesCmd Prace(string cmd)
        {
            MesCmd mesCmdObj = null;
            try
            {
                mesCmdObj = JsonConvert.DeserializeObject<MesCmd>(cmd);
            }
            catch (Exception exs)
            {
            }

            if (mesCmdObj != null)
            {
                Type type = FindCmdType(mesCmdObj.CmdType);
                if (type != null)
                {
                    try
                    {
                        mesCmdObj = JsonConvert.DeserializeObject(cmd, type) as MesCmd;
                    }
                    catch (Exception ex)
                    {
                        string timestr = DateTime.Now.ToString("yyyy - MM - dd - hh - mm - ss");
                        string strMes = $"[{ timestr}]" + $":{ex} ";
                        File.AppendAllLines("D:\\ErrSeverInfo.text", new string[] { strMes });
                    }
                }
                else
                {
                }
            }
            else
            {
            }

            return mesCmdObj;
        }

        public void Add(string machineName)
        {
            MachineClients.TryAdd(machineName, new MachineInfo()
            {
                machineFeedState = MachineState.未知,
                machineFeedState2 = MachineState.未知,
                machineOutState = MachineState.未知,
                machineOutState2 = MachineState.未知,
                MachineName = machineName,
            });
        }

        public void SeverInit()
        {
            Add(MachineName.前壳锁付.ToString());
            Add(MachineName.镜头锁付.ToString());
            Add(MachineName.Plasma.ToString());
            Add(MachineName.AA.ToString());
            Add(MachineName.固化.ToString());

            socketSever.ClientConnected += InfoClientConnected;

            socketSever.ClientDisConnected += (ClientManager clientManager) =>
            {
                ParamSetMgr.GetInstance().SetBoolParam(socketSever.IP.ToString(), true);
                ParamSetMgr.GetInstance().SetBoolParam(socketSever.IP.ToString(), false);
                Clients.TryRemove(clientManager.IP.ToString() + "#" + clientManager.Port.ToString(), out ClientManager clientManager2);
                if (eventOfMes != null)
                {
                    eventOfMes(socketSever.IP.ToString(), "断开成功");
                }
                Warn(clientManager.IP.ToString() + ":断开成功");
            };
            socketSever.ProcessData += (string mes) =>
            {
                string[] split = mes.Split(new string[] { "#", "$" }, StringSplitOptions.RemoveEmptyEntries);
                if (split == null) return;
                //if (split.Length < 5) return;
                int index = 0;
                string ipName = split[index++];
                string portName = split[index++];
                string Cmd = split[index++];

                cmdofMes cmdofMes = new cmdofMes()
                {
                    ip = ipName,
                    port = portName,
                    cmd = Cmd,
                };
                ClientManager clientManager = null;
                Clients.TryGetValue(ipName, out clientManager);
                if (clientManager == null)
                    Clients.AddOrUpdate(clientManager.IP.ToString(), clientManager, (key, value) => { return value = clientManager; });
                cmdofMes.Client = clientManager;
                Info(ipName + "#" + portName + ":" + Cmd);
                CmdFromClient cmdFromClient = new CmdFromClient
                {
                    Client = clientManager,
                    cmd = Prace(Cmd),
                };
                userQueueSever.Add(cmdFromClient);
                //  userQueue.Add(cmdFromClient);
            };
            socketSever.m_strLine = MesCmd.LineEndChars;
            socketSever.Init("127.0.0.1", 5000);
            ExcQueueCmd();
        }
    }
}