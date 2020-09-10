using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

    }
    public enum CmdResult
    {
        OK,
        NG,

    }

    public class MachineInfo
    {
        public string MachineName;
        public MachineState machineState;
    }

    public class MesCmdAnswer
    {
        public string CmdName = "BaseMesCmdAnswer";
        public Type CmdType;
        public CmdResult cmdResult = CmdResult.NG;
        public MesCmdAnswer()
        {
            CmdType = this.GetType();
        }
    }


    public class MesCmd
    {
        public MesCmd()
        {
            log = LogManager.GetLogger(CmdName);
            CmdType = this.GetType();
        }
        public string sendor;
        public string CmdName = "BaseCmd";
        public Type CmdType;
        [JsonIgnore]
        public MachineInfo machineInfo = null;
        [JsonIgnore]
        protected ILog log = null;
        public virtual bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, ClientManager clientManager)
        {
            return true;
        }

     
    }

    public class SetMachineCmdAnswer : MesCmdAnswer
    {


    }

    public class SetMachineCmd : MesCmd
    {
        public SetMachineCmd()
        {
            CmdName = "SetMachineCmd";

        }
       
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, ClientManager Client)
        {
            if (MachineClients.TryGetValue(sendor, out machineInfo))
            {
                machineInfo.MachineName = sendor;
                Client.ClientName = sendor;
            }
            else
            {
                machineInfo = new MachineInfo()
                {
                    MachineName = sendor,
                    machineState = MachineState.未知,
                };
                MachineClients.TryAdd(sendor, machineInfo);
            }
            SetMachineCmdAnswer setMachineCmdAnswer = new SetMachineCmdAnswer();
            setMachineCmdAnswer.cmdResult = CmdResult.OK;
            String strJson = JsonConvert.SerializeObject(setMachineCmdAnswer) + "\r\n";
            Client.SendCommand(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }


  
    public class GetLineStateCmdAnswer : MesCmdAnswer
    {
        public MachineState machineState = MachineState.未知;

    }
    public class GetLineStateCmd : MesCmd
    {
        public GetLineStateCmd()
        {
            CmdName = "GetLineStateCmd";

        }
        public string Other = "";

        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, ClientManager Client)
        {
            GetLineStateCmdAnswer getLineStateCmdAnswer = new GetLineStateCmdAnswer();
            if (MachineClients.TryGetValue(Other, out machineInfo))
            {
                getLineStateCmdAnswer.machineState = machineInfo.machineState;
                getLineStateCmdAnswer.cmdResult = CmdResult.OK;
             
            }
            String strJson = JsonConvert.SerializeObject(getLineStateCmdAnswer) +"\r\n";
            Client.SendCommand(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }

    public class SetLineStateCmdAnswer : MesCmdAnswer
    {
      

    }
    public class SetLineStateCmd : MesCmd
    {
        public SetLineStateCmd()
        {
            CmdName = "SetLineStateCmd";

        }
        public string Other = "";
        public MachineState machineState = MachineState.未知;
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, ClientManager Client)
        {
            SetLineStateCmdAnswer setLineStateCmdAnswer = new SetLineStateCmdAnswer();
            if (MachineClients.TryGetValue(Other, out machineInfo))
            {
                machineInfo.machineState = machineState;
                setLineStateCmdAnswer.cmdResult = CmdResult.OK;
            }
            String strJson = JsonConvert.SerializeObject(setLineStateCmdAnswer) + "\r\n";
            Client.SendCommand(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }
    public class HeartBeepCmdAnswer : MesCmdAnswer
    {


    }
    public class HeartBeep : MesCmd
    {
        public HeartBeep()
        {
            CmdName = "HeartBeep";

        }
        public string Other = "";
        public MachineState machineState = MachineState.未知;
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, ClientManager Client)
        {
            HeartBeepCmdAnswer heartBeepCmdAnswer = new HeartBeepCmdAnswer();
            String strJson = JsonConvert.SerializeObject(heartBeepCmdAnswer) + "\r\n";
            Client.SendCommand(strJson);
            return true;
        }
    }
    public class SmallMesClient
    {
        TcpLink m_TcpLink = null;
        ILog    log = LogManager.GetLogger("SmallMesClient");

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
                mesCmdAnswer = (MesCmdAnswer)JsonConvert.DeserializeObject(answer, mesCmdAnswer.CmdType);
                ts.Add(mesCmdAnswer);
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
            m_TcpLink = new TcpLink(0, MachineName, ip,port,10000,"CRLF");
            if (!m_TcpLink.IsOpen())
                bRtn &= m_TcpLink.Open();
            if (!bRtn)
                return false;

            m_TcpLink.RecvStringMessageEvent += (object sender, AsyTcpSocketEventArgs e) =>
            {
                string answer = e.Message;
                MesCmdAnswer mesCmdAnswer = JsonConvert.DeserializeObject<MesCmdAnswer>(answer);
                mesCmdAnswer = (MesCmdAnswer)JsonConvert.DeserializeObject(answer, mesCmdAnswer.CmdType);
                ts.Add(mesCmdAnswer);
            };
            return true;
        }
         ~SmallMesClient()
        {
            m_TcpLink?.Close();
     
        }
        private ConcurrentBag<MesCmdAnswer> ts = new ConcurrentBag<MesCmdAnswer>();
        public string ClientName
        { set; get; }
        private void BagClear()
        {
            while (!ts.IsEmpty)
                ts.TryTake(out MesCmdAnswer temp);
        }
        private MesCmdAnswer GetCmdAnswer(int nTimeout)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            MesCmdAnswer mesCmdAnswer = null;
            while (!ts.TryTake(out mesCmdAnswer))
            {
                if (stopwatch.ElapsedMilliseconds > nTimeout)
                    break;
            }
            if(mesCmdAnswer== null)
             ts.TryTake(out mesCmdAnswer);
            return mesCmdAnswer;

        }
        public AutoResetEvent autoResetEvent = new AutoResetEvent(true);
        public MesCmdAnswer SetLineStateCmdExc(string machineName, MachineState machineState,int  timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                SetLineStateCmd setLineStateCmd = new SetLineStateCmd();
                setLineStateCmd.sendor = machineName;
                setLineStateCmd.machineState = machineState;
                string strCmd = JsonConvert.SerializeObject(setLineStateCmd);
                strCmd += "\r\n";
                byte[] bytes = Encoding.Default.GetBytes(strCmd);
                 m_TcpLink.WriteData(bytes, bytes.Length);
                MesCmdAnswer mesCmdAnswer= GetCmdAnswer(timeout);
                autoResetEvent.Set();
                return mesCmdAnswer;
            }
            return null;
        }

        public MesCmdAnswer GetLineStateCmdExc(string machineName,String Other,  int timeout)
        {
            if (m_TcpLink == null)
                return null;
            if (autoResetEvent.WaitOne(timeout, true))
            {
                BagClear();
                GetLineStateCmd getLineStateCmd = new GetLineStateCmd();
                getLineStateCmd.sendor = machineName;
                getLineStateCmd.Other = Other;
                string strCmd = JsonConvert.SerializeObject(getLineStateCmd);
                strCmd += "\r\n";
                byte[] bytes = Encoding.Default.GetBytes(strCmd);
                m_TcpLink.WriteData(bytes, bytes.Length);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswer(timeout);
                autoResetEvent.Set();
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
                string strCmd = JsonConvert.SerializeObject(cmdobj);
                strCmd += "\r\n";
                byte[] bytes = Encoding.Default.GetBytes(strCmd);

                m_TcpLink.WriteData(bytes, bytes.Length);
                MesCmdAnswer mesCmdAnswer = GetCmdAnswer(timeout);
                autoResetEvent.Set();
            }
            return null;

        }


    }
}
