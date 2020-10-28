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
   
    public class MesCmdAnswer
    {
        public string CmdName = "BaseMesCmdAnswer";
        public string CmdType;
        public CmdResult cmdResult = CmdResult.NG;
        public static string LineEndChars = "$$$\r\n";
        public string strCodeFromCmd = "";
        public MesCmdAnswer()
        {
            CmdType = this.GetType().FullName;
        }
    }
    public enum EOprate
    {
        读取,
        写入,
    }

    public class MesCmd
    {
        public MesCmd()
        {
            log = LogManager.GetLogger(CmdName);
            CmdType = this.GetType().FullName;
            CmdOpreateType = EOprate.读取;
            CmdName = this.GetType().FullName;
        }
        public string sendor;
        public string CmdName = "BaseCmd";
        public string CmdType;
        public string strCmdCode = "";
        public EOprate CmdOpreateType = EOprate.读取;
        //     [JsonIgnore]
        //  public MachineInfo machineInfo = null;
        [JsonIgnore]
        protected ILog log = null;
        public static string LineEndChars = "$$$\r\n";
        public virtual bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager clientManager)
        {
            return true;
        }
        public virtual string  GenCmdCode()
        {
            strCmdCode = CmdType + sendor;
            return strCmdCode;
        }
        public void Send(TcpLink tcpLink)
        {
            try
            {

                GenCmdCode();
             
                if (tcpLink != null && tcpLink.IsConnected)
                {
                    string strCmd = JsonConvert.SerializeObject(this);
                    strCmd += MesCmd.LineEndChars;
                    byte[] bytes = Encoding.Default.GetBytes(strCmd);
                    try
                    {
                        File.AppendAllLines("D:\\发送.txt", new string[] { $"{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}:SendError:{strCmd}" });

                    }
                    catch { }

                    tcpLink.WriteData2(bytes, bytes.Length);
                }
                else
                {
                    File.AppendAllLines("D:\\SmallMes_CToS_SendError.txt", new string[] {  DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")+":"+"SendError" });
                   
                }
            }
            catch( Exception EX)
            {
                File.AppendAllLines("D:\\SmallMes_CToS_SendError.txt", new string[] { DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ":" + $"SendError {EX.Message}" });

            }

        }
    }

    public class SetMachineCmdAnswer : MesCmdAnswer
    {


    }

    public class SetMachineCmd : MesCmd
    {
        public SetMachineCmd()
        {
           
            CmdOpreateType = EOprate.写入;
        }
        public override string GenCmdCode()
        {
            strCmdCode = base.GenCmdCode() ;
            return strCmdCode;
        }

        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager Client)
        {
            MachineInfo machineInfo = null;
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
                    machineFeedState = MachineState.未知,
                    machineOutState = MachineState.未知,
                };
                MachineClients.TryAdd(sendor, machineInfo);
            }
            SetMachineCmdAnswer CmdAnswer = new SetMachineCmdAnswer();
            CmdAnswer.strCodeFromCmd = mesCmd.strCmdCode;
            CmdAnswer.cmdResult = CmdResult.OK;
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            //Client.SendCommand(strJson);
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }

 
    public class WriteStateCmd : MesCmd
    {
        public MachineState machineState = MachineState.未知;
    }
    public class ReadStateCmd : MesCmd
    {
        public string Other = "";
    }


    public class GetLineFeedStateCmd : ReadStateCmd
    {
        public GetLineFeedStateCmd()
        {
          
            CmdOpreateType = EOprate.读取;
        }
        

        public override string  GenCmdCode()
        {
            strCmdCode=  base.GenCmdCode()+ Other;
            return strCmdCode;
        }
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager Client)
        {
            GetStateCmdAnswer CmdAnswer = new GetStateCmdAnswer();
            CmdAnswer.strCodeFromCmd = mesCmd.strCmdCode;
            MachineInfo machineInfo = null;
            if (MachineClients.TryGetValue(Other, out machineInfo))
            {
                CmdAnswer.machineState = machineInfo.machineFeedState;
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            //Client.SendCommand(strJson);
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }


    public class SetLineFeedStateCmd : WriteStateCmd
    {
        public SetLineFeedStateCmd()
        {
         
            CmdOpreateType = EOprate.写入;

        }
  
        public string Other = "";
       
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager Client)
        {
            SetStateAnswer CmdAnswer = new SetStateAnswer();
            CmdAnswer.strCodeFromCmd = mesCmd.strCmdCode;
            MachineInfo machineInfoNew = new MachineInfo();
            if (MachineClients.TryGetValue(sendor, out machineInfoNew))
            {
                machineInfoNew.machineFeedState = machineState;
                MachineClients.AddOrUpdate(sendor, machineInfoNew, (key, value) => { return value = machineInfoNew; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            else
            {
                machineInfoNew.machineFeedState = machineState;
                machineInfoNew.MachineName = sendor;
                MachineClients.AddOrUpdate(sendor, machineInfoNew, (key, value) => { return value = machineInfoNew; });
            }
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            //Client.SendCommand(strJson);
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }

  
    public class GetLineOutStateCmd : ReadStateCmd
    {
        public GetLineOutStateCmd()
        {
          
            CmdOpreateType = EOprate.读取;
        }
        public override string GenCmdCode()
        {
            strCmdCode = base.GenCmdCode() + Other;
            return strCmdCode;
        }
        

        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager Client)
        {
            GetStateCmdAnswer CmdAnswer = new GetStateCmdAnswer();
            CmdAnswer.strCodeFromCmd = mesCmd.strCmdCode;
            MachineInfo machineInfo = null;
            if (MachineClients.TryGetValue(Other, out machineInfo))
            {
                CmdAnswer.machineState = machineInfo.machineOutState;
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            else
            {

            }
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            // Client.SendCommand(strJson);
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }


    public class SetLineOutStateCmd : WriteStateCmd
    {
        public SetLineOutStateCmd()
        {
          
            CmdOpreateType = EOprate.写入;
        }
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager Client)
        {
            SetStateAnswer CmdAnswer = new SetStateAnswer();
            CmdAnswer.strCodeFromCmd = mesCmd.strCmdCode;
            MachineInfo machineInfoNew = new MachineInfo();
            if (MachineClients.TryGetValue(sendor, out machineInfoNew))
            {
                machineInfoNew.machineOutState = machineState;
                MachineClients.AddOrUpdate(sendor, machineInfoNew, (key, value) => { return value = machineInfoNew; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            else
            {
                MachineInfo machineInfo = new MachineInfo();
                machineInfo.machineOutState = machineState;
                machineInfo.MachineName = sendor;
                MachineClients.AddOrUpdate(sendor, machineInfo, (key, value) => { return value = machineInfo; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            // Client.SendCommand(strJson);
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }

   
    public class HeartBeepCmdAnswer : MesCmdAnswer
    {
    }
    public class HeartBeep : WriteStateCmd
    {
        public HeartBeep()
        {
            CmdName = "HeartBeep";

        }
        public string Other = "";
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager Client)
        {
            HeartBeepCmdAnswer CmdAnswer = new HeartBeepCmdAnswer();
            CmdAnswer.strCodeFromCmd = mesCmd.strCmdCode;
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            // Client.SendCommand(strJson);
            Client.WriteData(strJson);
            return true;
        }
    }
  
}
