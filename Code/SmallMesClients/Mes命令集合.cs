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
    /// <summary>
    /// 设置状态应答
    /// </summary>
    public class SetStateAnswer : MesCmdAnswer
    {
        
    }
    /// <summary>
    /// 设置 本机前段 输入状态 （本段机器进料，前端机器出料，当本段机器无料时 本段设置为待料 当本段进料完成时 设置为上料完成）
    /// </summary>
    public class SetInStateFrontSegCmd : WriteStateCmd
    {
        public SetInStateFrontSegCmd()
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
                machineInfoNew.machineFeedState = machineState;
                MachineClients.AddOrUpdate(sendor, machineInfoNew, (key, value) => { return value = machineInfoNew; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            else
            {
                MachineInfo machineInfo = new MachineInfo();
                machineInfo.machineFeedState = machineState;
                machineInfo.MachineName = sendor;
                MachineClients.AddOrUpdate(sendor, machineInfo, (key, value) => { return value = machineInfo; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }

   
    /// <summary>
    /// 设置 本机后段输入状态 （本段机器进料， 后端机器出料，当本段机器无料时 本段设置为待料 当本段进料完成时 设置为上料完成）
    /// </summary>
    public class SetInStateBackSegCmd : WriteStateCmd
    {
        public SetInStateBackSegCmd()
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
                machineInfoNew.machineFeedState2 = machineState;
                MachineClients.AddOrUpdate(sendor, machineInfoNew, (key, value) => { return value = machineInfoNew; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            else
            {
                MachineInfo machineInfo = new MachineInfo();
                machineInfo.machineFeedState2 = machineState;
                machineInfo.MachineName = sendor;
                MachineClients.AddOrUpdate(sendor, machineInfo, (key, value) => { return value = machineInfo; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }


   
    /// <summary>
    /// 设置本机前段 输出状态 （本段机器出料，前端机器进料 当本段机器等待出料 本段设置为等待出料 其他状态 设置为未知）
    /// </summary>
    public class SetOutStateFrontSegCmd : WriteStateCmd
    {
        public SetOutStateFrontSegCmd()
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
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }


    
    /// <summary>
    /// 设置 本机后段输入状态 （本段机器出料， 后端机器进料，当本段机器无料时 本段设置为待料 当本段进料完成时 设置为上料完成）
    /// </summary>
    public class SetOutStateBackSegCmd : WriteStateCmd
    {
        public SetOutStateBackSegCmd()
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
                machineInfoNew.machineOutState2 = machineState;
                MachineClients.AddOrUpdate(sendor, machineInfoNew, (key, value) => { return value = machineInfoNew; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            else
            {
                MachineInfo machineInfo = new MachineInfo();
                machineInfo.machineOutState2  = machineState;
                machineInfo.MachineName = sendor;
                MachineClients.AddOrUpdate(sendor, machineInfo, (key, value) => { return value = machineInfo; });
                CmdAnswer.cmdResult = CmdResult.OK;
            }
            String strJson = JsonConvert.SerializeObject(CmdAnswer) + MesCmdAnswer.LineEndChars;
            Client.WriteData(strJson);
            log.Info($"{Client.ClientName}   Oprate {sendor}   {CmdName}");
            return true;
        }
    }



    public class GetStateCmdAnswer : MesCmdAnswer
    {
        public MachineState machineState = MachineState.未知;

    }
    /// <summary>
    /// 获取前段机器的 进料状态 ( 待料，上料完成）
    /// </summary>
    public class GetInStateFrontSegCmd : ReadStateCmd
    {
        public GetInStateFrontSegCmd()
        {
           
            CmdOpreateType = EOprate.读取;
        }
        public override string GenCmdCode()
        {
            strCmdCode = base.GenCmdCode() + Other;
            return strCmdCode;
        }

        public string Other = "";
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
    /// <summary>
    /// 获取前段机器的 出料状态 ( 待料，上料完成）
    /// </summary>
    public class GetOutStateFrontSegCmd : ReadStateCmd
    {
        public GetOutStateFrontSegCmd()
        {
           
            CmdOpreateType = EOprate.读取;
        }
        public override string GenCmdCode()
        {
            strCmdCode = base.GenCmdCode() + Other;
            return strCmdCode;
        }

        public string Other = "";
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


    /// <summary>
    /// 获取后段机器的 进料状态 ( 待料，上料完成）
    /// </summary>
    public class GetInStateBackSegCmd : ReadStateCmd
    {
        public GetInStateBackSegCmd()
        {
           
            CmdOpreateType = EOprate.读取;
        }
        public override string GenCmdCode()
        {
            strCmdCode = base.GenCmdCode() + Other;
            return strCmdCode;
        }

        public string Other = "";
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager Client)
        {
            GetStateCmdAnswer CmdAnswer = new GetStateCmdAnswer();
            CmdAnswer.strCodeFromCmd = mesCmd.strCmdCode;
            MachineInfo machineInfo = null;
            if (MachineClients.TryGetValue(Other, out machineInfo))
            {
                CmdAnswer.machineState = machineInfo.machineFeedState2;
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

    /// <summary>
    /// 获取后段机器的 出料状态 ( 出料中 未知）
    /// </summary>

    public class GetOutStateBackSegCmd : ReadStateCmd
    {
        public GetOutStateBackSegCmd()
        {
            CmdOpreateType = EOprate.读取;
        }
        public override string GenCmdCode()
        {
            strCmdCode = base.GenCmdCode() + Other;
            return strCmdCode;
        }

        public string Other = "";
        public override bool Oprate(ConcurrentDictionary<string, MachineInfo> MachineClients, MesCmd mesCmd, ClientManager Client)
        {
            GetStateCmdAnswer CmdAnswer = new GetStateCmdAnswer();
            CmdAnswer.strCodeFromCmd = mesCmd.strCmdCode;
            MachineInfo machineInfo = null;
            if (MachineClients.TryGetValue(Other, out machineInfo))
            {
                CmdAnswer.machineState = machineInfo.machineOutState2;
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
}
