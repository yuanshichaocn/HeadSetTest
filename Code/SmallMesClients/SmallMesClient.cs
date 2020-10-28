using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communicate;
using log4net;
using Newtonsoft.Json;

namespace SmallMesClients
{
    public enum MachineState
    {
        未知,
        待料,
        上料完成,
        忙碌,
        等待出料,
        出料完成,
        出OK料,
        出NG料

    }
    public enum CmdResult
    {
        OK,
        NG,

    }

    public class MachineInfo
    {
        public string MachineName;
        public MachineState machineFeedState;
        public MachineState machineOutState;
       
        public MachineState machineFeedState2;
        public MachineState machineOutState2;
    }


    public class userQueue<T>
    {

        private Queue<T> queue = new Queue<T>();
        private object objlock = new object();
         public T Take()
        {
            lock (objlock)
            {
                if (queue.Count > 0)
                    return queue.Dequeue();
                else return default(T);

            }

        }
        public void Add(T obj)
        {
            lock (objlock)
            {
                queue.Enqueue(obj);
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
        //指令回应
        //是否被设置



    public class SmallMesClient
    {
        TcpLink m_TcpLink = null;
        ILog    log = LogManager.GetLogger("SmallMesClient");
        List<Type> types = null;
        public SmallMesClient()
        {
            types = GetAllSubClassType(typeof(MesCmdAnswer), AppDomain.CurrentDomain.BaseDirectory + "SmallMesClients.dll");
        }
        public ConcurrentDictionary<string, MesCmdAnswer> ReciveAllAnswer = new ConcurrentDictionary<string, MesCmdAnswer>();
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
        Type FindCmdType(string typefullname)
        {
            if (types != null && types.Count > 0)
            {
                Type type = types.Find(t => t.FullName == typefullname);
                return type;
            }
            return null;
        }
        public bool Init(TcpLink tcpLink, TcpLink tcpLinkHeart)
        {
            m_TcpLink = tcpLink;
            bool bRtn = true;
            if (tcpLink == null)
                return false;
            tcpLink.RecvStringMessageEvent += (object sender, AsyTcpSocketEventArgs e) =>
            {
                string answer = e.Message;
                MesCmdAnswer mesCmdAnswer = JsonConvert.DeserializeObject<MesCmdAnswer>(answer);
                if (mesCmdAnswer != null)
                {
                    Type type = FindCmdType(mesCmdAnswer.CmdType);
                    if (type != null)
                    {
                        mesCmdAnswer = (MesCmdAnswer)JsonConvert.DeserializeObject(answer, type);
                        if (mesCmdAnswer == null)
                        {
                            string timestr = DateTime.Now.ToString("yyyy - MM - dd - hh - mm - ss");
                            string strMes = $"[{ timestr}]" + $": 回应为空 ";
                            File.AppendAllLines("D:\\ErrSeverInfo.text", new string[] { strMes });
                        }
                        ts.Add(mesCmdAnswer);
                        //CmdAnswer cmdAnswer = new CmdAnswer() {
                        //    _bHaveSet = true,
                        //    _mesCmdAnswer = mesCmdAnswer,
                        //};
                         lock( objLock)
                        {
                            ReciveAllAnswer.AddOrUpdate(mesCmdAnswer.strCodeFromCmd, mesCmdAnswer, (key, value) => { return value = mesCmdAnswer; });
                        }
                      
                        return;
                    }

                }
                ts.Add(null);


            };
            if (!tcpLink.IsOpen())
                bRtn&= tcpLink.Open();
            if (!bRtn)
                return false;
           
            return true;
        }
       public  bool Init(string MachineName, string ip,int port)
        {
            m_TcpLink?.Close();
            bool bRtn = true;
            m_TcpLink = new TcpLink(0, MachineName, ip,port,10000, MesCmd.LineEndChars);
            if (!m_TcpLink.IsOpen())
                bRtn &= m_TcpLink.Open();
            if (!bRtn)
                return false;

            m_TcpLink.RecvStringMessageEvent += (object sender, AsyTcpSocketEventArgs e) =>
            {
                try
                {
                    string answer = e.Message;
                    MesCmdAnswer mesCmdAnswer = JsonConvert.DeserializeObject<MesCmdAnswer>(answer);
                    if (mesCmdAnswer != null)
                    {
                        Type type = FindCmdType(mesCmdAnswer.CmdType);
                        if (type != null)
                        {
                            mesCmdAnswer = (MesCmdAnswer)JsonConvert.DeserializeObject(answer, type);
                            if (mesCmdAnswer == null)
                            {
                                string timestr = DateTime.Now.ToString("yyyy - MM - dd - hh - mm - ss");
                                string strMes = $"[{ timestr}]" + $": 回应为空 ";
                                File.AppendAllLines("D:\\ErrSeverInfo.text", new string[] { strMes });
                            }
                            ts.Add(mesCmdAnswer);
                            //CmdAnswer cmdAnswer = new CmdAnswer()
                            //{
                            //    _bHaveSet = true,
                            //    _mesCmdAnswer = mesCmdAnswer,
                            //};
                        
                            ReciveAllAnswer.AddOrUpdate(mesCmdAnswer.strCodeFromCmd, mesCmdAnswer, (key, value) => { return value = mesCmdAnswer; });
                            return;
                        }

                    }
                   
                }
                catch(Exception es)
                {
                    string timestr = DateTime.Now.ToString("yyyy - MM - dd - hh - mm - ss");
                    string strMes = $"[{ timestr}]" + $": {es} ";
                    File.AppendAllLines("D:\\ErrSeverInfo.text", new string[] { strMes });
                    ts.Add(null);
                }
  


            };
            return true;
        }
         ~SmallMesClient()
        {
            m_TcpLink?.Close();
     
        }
        private userQueue<MesCmdAnswer> ts = new userQueue<MesCmdAnswer>();
        //private ConcurrentDictionary<string,>
        public string ClientName
        { set; get; }
        private void BagClear()
        {
            ts.Clear();
        }
        private MesCmdAnswer GetCmdAnswerFrowSever(int nTimeout)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            MesCmdAnswer mesCmdAnswer = ts.Take();
            while (mesCmdAnswer==null)
            {
                if (stopwatch.ElapsedMilliseconds > nTimeout)
                    break;
                mesCmdAnswer = ts.Take();
            }
          
            if (mesCmdAnswer == null)
            {

            }
                return mesCmdAnswer;

        }
        private MesCmdAnswer GetCmdAnswerFromMem( string  strcmdcode)
        {
           
            MesCmdAnswer mesCmdAnswer = null;
            ReciveAllAnswer.TryGetValue(strcmdcode, out mesCmdAnswer);
            return mesCmdAnswer == null ? null :mesCmdAnswer;

        }
        public object objLock = new object();
        ConcurrentDictionary<string, MachineState> WriteStateFlags = new ConcurrentDictionary<string, MachineState>();

        public MesCmdAnswer CheckCMD(MesCmd mesCmd, MachineState machineState,bool Skip = false)
        {
           if(Skip)
            return null;
            string strCode = mesCmd.GenCmdCode();
           if (mesCmd.CmdOpreateType == EOprate.写入)  
            {
 
                    if (WriteStateFlags.ContainsKey(strCode))
                    {
                        if (WriteStateFlags[strCode] == machineState)
                        {
                            return new SetStateAnswer();
                        }
                    }
            }


            return null;
        }
        public AutoResetEvent autoResetEvent = new AutoResetEvent(true);
        public MesCmdAnswer SetLineFeedStateCmdExc(string machineName, MachineState machineState, int timeout, bool SkipCheckLoction=false)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                SetLineFeedStateCmd setLineStateCmd = new SetLineFeedStateCmd();
                setLineStateCmd.sendor = machineName;
                setLineStateCmd.machineState = machineState;
                //string strCmd = JsonConvert.SerializeObject(setLineStateCmd);
                //strCmd += MesCmd.LineEndChars;
                //byte[] bytes = Encoding.Default.GetBytes(strCmd);
                // m_TcpLink.WriteData(bytes, bytes.Length);
                MesCmdAnswer answer = CheckCMD(setLineStateCmd, machineState, SkipCheckLoction);
                if (answer != null)
                {
                    autoResetEvent.Set();
                    return answer;
                }
                  

                setLineStateCmd.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer= GetCmdAnswerFrowSever(timeout);
                if (mesCmdAnswer != null)
                    WriteStateFlags.AddOrUpdate(setLineStateCmd.GenCmdCode(), machineState, (key, value) => { return value = machineState; });
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }

        public MesCmdAnswer GetLineFeedStateCmdExc(string machineName,String Other,  int timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                GetLineFeedStateCmd getLineStateCmd = new GetLineFeedStateCmd();
                getLineStateCmd.sendor = machineName;
                getLineStateCmd.Other = Other;
                //string strCmd = JsonConvert.SerializeObject(getLineStateCmd);
                //strCmd += MesCmd.LineEndChars;
                //byte[] bytes = Encoding.Default.GetBytes(strCmd);
                //m_TcpLink.WriteData(bytes, bytes.Length);
           
                getLineStateCmd.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;

        }

        public MesCmdAnswer SetLineOutStateCmdExc(string machineName, MachineState machineState, int timeout, bool SkipCheckLoction = false)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                SetLineOutStateCmd setLineStateCmd = new SetLineOutStateCmd();
                setLineStateCmd.sendor = machineName;
                setLineStateCmd.machineState = machineState;
                //string strCmd = JsonConvert.SerializeObject(setLineStateCmd);
                //strCmd += MesCmd.LineEndChars;
                //byte[] bytes = Encoding.Default.GetBytes(strCmd);
                //m_TcpLink.WriteData(bytes, bytes.Length);
                MesCmdAnswer answer = CheckCMD(setLineStateCmd, machineState,SkipCheckLoction);
                if (answer != null)
                {
                    autoResetEvent.Set();
                    return answer;
                }

                setLineStateCmd.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                if (mesCmdAnswer != null)
                    WriteStateFlags.AddOrUpdate(setLineStateCmd.GenCmdCode(), machineState, (key, value) => { return value = machineState; });
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }

        public MesCmdAnswer GetLineOutStateCmdExc(string machineName, String Other, int timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                GetLineOutStateCmd getLineStateCmd = new GetLineOutStateCmd();
                getLineStateCmd.sendor = machineName;
                getLineStateCmd.Other = Other;

                //string strCmd = JsonConvert.SerializeObject(getLineStateCmd);
                //strCmd += MesCmd.LineEndChars;
                //byte[] bytes = Encoding.Default.GetBytes(strCmd);
                //m_TcpLink.WriteData(bytes, bytes.Length);
         
                getLineStateCmd.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;

        }

        public MesCmdAnswer SetMachineCmdExc(string machineName, int timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                SetMachineCmd cmdobj = new SetMachineCmd();
                cmdobj.sendor = machineName;
                //string strCmd = JsonConvert.SerializeObject(cmdobj);
                //strCmd += MesCmd.LineEndChars;
                //byte[] bytes = Encoding.Default.GetBytes(strCmd);
                //m_TcpLink.WriteData(bytes, bytes.Length);
            
                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
              
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;

        }

        /// <summary>
        /// 查询机器前段的进料状态  （前机返回 待料，进料完成）
        /// </summary>
        /// <param name="sendor">本段机器名称</param>
        /// <param name="Other">前段机器名称</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MesCmdAnswer QueryFrontSegFeedState(string sendor, String Other, int timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                GetInStateFrontSegCmd cmdobj = new GetInStateFrontSegCmd();
                cmdobj.sendor = sendor;
                cmdobj.Other = Other;

            

                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }
        /// <summary>
        /// 查询机器的前端状态 (前机返回 等待出料  出料完成）
        /// </summary>
        /// <param name="sendor">本段机器名称</param>
        /// <param name="Other">前段机器名称</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MesCmdAnswer QueryFrontSegOutState(string sendor, String Other, int timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                GetOutStateFrontSegCmd cmdobj = new GetOutStateFrontSegCmd();
                cmdobj.sendor = sendor;
                cmdobj.Other = Other;

             

                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }
        /// <summary>
        /// 设置本段机器的前段进料状态（前机返回 待料，上料完成）
        /// </summary>
        /// <param name="sendor">本段机器名称</param>
        /// <param name="machineState">本段机器跟前段设备的进料状态</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MesCmdAnswer SetFeedStateFrontSeg(string sendor, MachineState machineState, int timeout, bool SkipCheckLoction = false)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                SetInStateFrontSegCmd cmdobj = new SetInStateFrontSegCmd();
                cmdobj.sendor = sendor;
                cmdobj.machineState = machineState;

                MesCmdAnswer answer = CheckCMD(cmdobj,machineState,SkipCheckLoction);
                if (answer != null)
                {
                    autoResetEvent.Set();
                    return answer;
                }

                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                if (mesCmdAnswer != null)
                    WriteStateFlags.AddOrUpdate(cmdobj.GenCmdCode(), machineState, (key, value) => { return value = machineState; });
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }
        /// <summary>
        /// 设置本段机器的前段进料状态( 前机返回 等待出料 出料完成）
        /// </summary>
        /// <param name="sendor">本段机器名称</param>
        /// <param name="Other">前段机器名称</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MesCmdAnswer SetOutStateFrontSeg(string sendor, MachineState machineState, int timeout, bool SkipCheckLoction = false)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                 SetOutStateFrontSegCmd cmdobj = new SetOutStateFrontSegCmd();
                cmdobj.sendor = sendor;
                cmdobj.machineState = machineState;

                MesCmdAnswer answer = CheckCMD(cmdobj,machineState,SkipCheckLoction);
                if (answer != null)
                {
                    autoResetEvent.Set();
                    return answer;
                }

                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                if (mesCmdAnswer != null)
                    WriteStateFlags.AddOrUpdate(cmdobj.GenCmdCode(), machineState, (key, value) => { return value = machineState; });
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }

        /// <summary>
        /// 查询机器后段的进料状态 （后机返回 待料，进料完成）
        /// </summary>
        /// <param name="sendor">本段机器名称</param>
        /// <param name="Other">后段机器名称</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MesCmdAnswer QueryBackSegFeedState(string sendor, String Other, int timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                GetInStateFrontSegCmd cmdobj = new GetInStateFrontSegCmd();
                cmdobj.sendor = sendor;
                cmdobj.Other = Other;

            

                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }
        /// <summary>
        /// 查询机器后段的出料状态 （后机返回 等待出料 出料完成）
        /// </summary>
        /// <param name="sendor">本段机器名称</param>
        /// <param name="Other">后段机器名称</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MesCmdAnswer QueryBackSegOutState(string sendor, String Other, int timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                GetOutStateBackSegCmd cmdobj = new GetOutStateBackSegCmd();
                cmdobj.sendor = sendor;
                cmdobj.Other = Other;


                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }

        /// <summary>
        /// 设置本段机器的后段进料状态（待料，上料完成）
        /// </summary>
        /// <param name="sendor">本段机器名称</param>
        /// <param name="machineState">本段机器跟前段设备的进料状态</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MesCmdAnswer SetFeedStateBackSeg(string sendor, MachineState machineState, int timeout, bool SkipCheckLoction = false)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                SetInStateBackSegCmd cmdobj = new SetInStateBackSegCmd();
                cmdobj.sendor = sendor;
                cmdobj.machineState = machineState;

                MesCmdAnswer answer = CheckCMD(cmdobj,machineState,SkipCheckLoction);
                if (answer != null)
                {
                    autoResetEvent.Set();
                    return answer;
                }

                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                if (mesCmdAnswer != null)
                    WriteStateFlags.AddOrUpdate(cmdobj.GenCmdCode(), machineState, (key, value) => { return value = machineState; });
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }
        /// <summary>
        /// 设置本段机器的后段出料状态(等待出料  出料完成）
        /// </summary>
        /// <param name="sendor">本段机器名称</param>
        /// <param name="Other">前段机器名称</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public MesCmdAnswer SetOutStateBackSeg(string sendor, MachineState machineState, int timeout, bool SkipCheckLoction = false)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                SetOutStateBackSegCmd cmdobj = new SetOutStateBackSegCmd();
                cmdobj.sendor = sendor;
                cmdobj.machineState = machineState;

                MesCmdAnswer answer = CheckCMD(cmdobj,machineState,SkipCheckLoction);
                if (answer != null)
                {
                    autoResetEvent.Set();
                    return answer;
                }

                cmdobj.Send(m_TcpLink);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswerFrowSever(timeout);
                if (mesCmdAnswer != null)
                    WriteStateFlags.AddOrUpdate(cmdobj.GenCmdCode(), machineState, (key, value) => { return value = machineState; });
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }

    }
}
