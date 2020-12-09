using BaseDll;
using Communicate;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MotionIoLib
{
    public class Robot_EPSON : RobotBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="indexCard"> 卡的编号</param>
        /// <param name="strName">卡的名字</param>
        /// <param name="nMinAxisNo"> 最小轴号</param>
        /// <param name="nMaxAxisNo">最大轴号</param>
        public Robot_EPSON(string strName, string strPwd, string strEth, int nAxisNoMin, int nAxisNoMax, RobotCommunicateType robotCommunicateType = RobotCommunicateType.Eth)
            : base(strName, strPwd, strEth, nAxisNoMin, nAxisNoMax, robotCommunicateType)

        {
            robotPoint.X = 0;
            robotPoint.Y = 0;
            robotPoint.Z = 0;
            robotPoint.U = 0;
            robotPoint.V = 0;
            robotPoint.W = 0;
            robotPoint.HandStyle = RobotHand.Unknow;
            this.RobotPause = false;
            this.RobotExecuteBusy = false;
            this.RobotPointMes = robotPoint;
            EpsonErrLog.ErrorCmd = "";
            EpsonErrLog.DeviceType = "Epson_Robot";
            EpsonErrLog.ErrorMess = "";
            ConnectRemoteEpsonTCPIPOk = false;

            if (CommuncateType == CommuncateItf.Internet)
            {
                RobotCommucate = (TcpLink)CommuncateInterface;
            }
        }

        /// <summary>
        /// 构造命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="args">参数</param>
        /// <returns></returns>"$GetIO,0\"\r\n"
        private string CreateCMD(ERemotCMD cmd, params string[] args)
        {
            string strPara = string.Empty;
            if (cmd == ERemotCMD.Execute)
            {
                if (args.Length > 1)
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        strPara += args[i];
                        strPara += ',';
                    }
                    strPara = strPara.Substring(0, strPara.Length - 1);

                    string a = string.Format("${0},\"{1} {2}\"", cmd.ToString(), args[0], strPara);
                    return string.Format("${0},\"{1} {2}\"", cmd.ToString(), args[0], strPara);
                }
                else
                {
                    string a = string.Format("${0},\"{1} {2}\"", cmd.ToString(), args[0], strPara);
                    return string.Format("${0},\"{1}\"", cmd.ToString(), args[0]);
                }
            }
            else if (cmd == ERemotCMD.GetIO)
            {
                string a = string.Format("${0},{1}\" {2}", cmd.ToString(), args[0], RobotCommucate.GetEndFlag());
                return a;// string.Format("${0}{1}", cmd.ToString(), strPara);
            }
            else if (cmd == ERemotCMD.GetStatus)
            {
                string a = string.Format("${0},{1}", cmd.ToString(), RobotCommucate.GetEndFlag());
                return a;
            }
            else
            {
                foreach (var v in args)
                {
                    strPara += ",";
                    strPara += v;
                }
                string a = string.Format("${0},\" {1}{2}\"", cmd.ToString(), args[0], strPara);
                return string.Format("${0}{1}", cmd.ToString(), strPara);
            }
        }

        /// <summary>
        /// 转换错误ID
        /// </summary>
        /// <param name="errorID">错误ID</param>
        /// <returns>错误说明</returns>
        private string ConvertErrorID(string errorID)
        {
            int lengthEndFlag = RobotCommucate.GetEndFlag().Length;
            if (errorID.Length > lengthEndFlag)
                errorID = errorID.Substring(0, errorID.Length - lengthEndFlag);
            switch (errorID)
            {
                case "10": return "远程命令未以$开头";
                case "11": return "远程命令错误，请先登陆.";
                case "12": return "远程命令格式错误";
                case "13": return "远程登陆密码错误";
                case "14": return "要获取的指定数量超出范围（小于1或大于100）,忽略了要获取的数量,指定了一个字符串参数";
                case "15": return "参数不存在,参数尺寸错误,调用了超出了范围的元素";
                case "19": return " 请求超时";
                case "20": return "控制器未准备好";
                case "21": return "因为正在运行Execute，所以无法执行";
                case "99": return "系统错误通信错误";
                default: return "错误代码解码失败，ErrorID不存在:" + errorID;
            }
        }

        //接收数据回调函数
        private void Receive_Data(object sender, AsyTcpSocketEventArgs args)
        {
            try
            {
                if (ReceiveFilt(sender, args)) return;
                //登陆
                if (args.Message.Contains("Login"))
                {
                    if (args.Message.Contains("#Login"))
                    {
                        WResponseInfo(ERemotCMD.Login, true);
                        rs.Login = true;
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Login, false);
                    }
                }
                //登出
                else if (args.Message.Contains("Logout"))
                {
                    if (args.Message.Contains("#Logout"))
                    {
                        WResponseInfo(ERemotCMD.Logout, true);
                        rs.Login = false;
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Logout, false);
                    }
                }
                //执行函数
                else if (args.Message.Contains("Start"))
                {
                    if (args.Message == "#Start")
                    {
                        WResponseInfo(ERemotCMD.Start, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Start, false);
                    }
                }
                //停止
                else if (args.Message.Contains("Stop"))
                {
                    if (args.Message == "#Stop")
                    {
                        WResponseInfo(ERemotCMD.Stop, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Stop, false);
                    }
                }
                //暂停
                else if (args.Message.Contains("Pause"))
                {
                    if (args.Message == "#Pause")
                    {
                        WResponseInfo(ERemotCMD.Pause, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Pause, false);
                    }
                }
                //继续
                else if (args.Message.Contains("Continue"))
                {
                    if (args.Message == "#Continue")
                    {
                        WResponseInfo(ERemotCMD.Continue, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Continue, false);
                    }
                }
                //重置
                else if (args.Message.Contains("Reset"))
                {
                    if (args.Message.Contains("#Reset"))
                    {
                        WResponseInfo(ERemotCMD.Reset, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Reset, false);
                    }
                }
                //开启机器人电机
                else if (args.Message.Contains("SetMotorsOn"))
                {
                    if (args.Message.Contains("#SetMotorsOn"))
                    {
                        WResponseInfo(ERemotCMD.SetMotorsOn, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetMotorsOn, false);
                    }
                }
                //关闭机器人电机
                else if (args.Message.Contains("SetMotorsOff"))
                {
                    if (args.Message.Contains("#SetMotorsOff"))
                    {
                        WResponseInfo(ERemotCMD.SetMotorsOff, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetMotorsOff, false);
                    }
                }
                //选择机器人
                else if (args.Message.Contains("SetCurRobot"))
                {
                    if (args.Message.Contains("#SetCurRobot"))
                    {
                        WResponseInfo(ERemotCMD.SetCurRobot, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetCurRobot, false);
                    }
                }
                //获取当前机器人编号
                else if (args.Message.Contains("GetCurRobot"))
                {
                    if (args.Message.Contains("#GetCurRobot"))
                    {
                        WResponseInfo(ERemotCMD.GetCurRobot, true);
                        Status.CurrentRobot = Convert.ToInt32(args.Message.Split(',')[1]);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.GetCurRobot, false);
                    }
                }
                //原点
                else if (args.Message.Contains("Home"))
                {
                    if (args.Message.Contains("#Home"))
                    {
                        WResponseInfo(ERemotCMD.Home, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Home, false);
                    }
                }
                //获取指定的 I/O端口(8)
                else if (args.Message.Contains("GetIOByte"))
                {
                    if (args.Message.Contains("#GetIOByte"))
                    {
                        WResponseInfo(ERemotCMD.GetIOByte, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.GetIOByte, false);
                    }
                }
                //设置指定的 I/O端口
                else if (args.Message.Contains("SetIO"))
                {
                    if (args.Message.Contains("#SetIO"))
                    {
                        WResponseInfo(ERemotCMD.SetIO, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetIO, false);
                    }
                }
                //设置指定的 I/O端口(8)
                else if (args.Message.Contains("SetIOByte"))
                {
                    if (args.Message.Contains("#SetIOByte"))
                    {
                        WResponseInfo(ERemotCMD.SetIOByte, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetIOByte, false);
                    }
                }
                //获取指定的 I/O端口(16)
                else if (args.Message.Contains("GetIOWord"))
                {
                    if (args.Message.Contains("#GetIOWord"))
                    {
                        WResponseInfo(ERemotCMD.GetIOWord, true);
                        string bit16low = args.Message.Split(',')[1].Substring(2, 2);
                        string bit16high = args.Message.Split(',')[1].Substring(0, 2);
                        Status.IoIn0 = Int16.Parse(bit16low, NumberStyles.HexNumber);
                        Status.IoIn1 = Int16.Parse(bit16high, NumberStyles.HexNumber);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.GetIOWord, false);
                    }
                }
                //获取指定的 I/O端口(16)
                else if (args.Message.Contains("GetIO"))
                {
                    if (args.Message.Contains("#GetIO"))
                    {
                        WResponseInfo(ERemotCMD.GetIO, true);
                        int nIndex = Convert.ToInt32(CEParam);
                        string str = args.Message.Split(',')[1];
                        int temp = ((short)(Convert.ToUInt16(str) << nIndex));
                        if (temp > 0)
                            Status.IoIn0 = (short)((int)Status.IoIn0 | temp);
                        else
                            Status.IoIn0 = (short)((int)Status.IoIn0 & temp);
                        //if (CEParam == "0") Status.IoIn0 = Int16.Parse(args.Message.Split(',')[1], NumberStyles.HexNumber);
                        // if (CEParam == "1") Status.IoIn1 = Int16.Parse(args.Message.Split(',')[1], NumberStyles.HexNumber);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.GetIO, false);
                    }
                }
                //设置指定的 I/O端口(16)
                else if (args.Message.Contains("SetIOWord"))
                {
                    if (args.Message.Contains("#SetIOWord"))
                    {
                        WResponseInfo(ERemotCMD.SetIOWord, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetIOWord, false);
                    }
                }
                //获取指定的内存 I/O位
                else if (args.Message.Contains("GetMemIO"))
                {
                    if (args.Message.Contains("#GetMemIO"))
                    {
                        WResponseInfo(ERemotCMD.GetMemIO, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.GetMemIO, false);
                    }
                }
                //设置指定的内存 I/O位
                else if (args.Message.Contains("SetMemIO"))
                {
                    if (args.Message.Contains("#SetMemIO"))
                    {
                        WResponseInfo(ERemotCMD.SetMemIO, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetMemIO, false);
                    }
                }
                //获取指定的内存 I/O端口(8)
                else if (args.Message.Contains("GetMemIOByte"))
                {
                    if (args.Message.Contains("#GetMemIOByte"))
                    {
                        WResponseInfo(ERemotCMD.GetMemIOByte, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.GetMemIOByte, false);
                    }
                }
                //设置指定的内存 I/O端口(8)
                else if (args.Message.Contains("SetMemIOByte"))
                {
                    if (args.Message.Contains("#SetMemIOByte"))
                    {
                        WResponseInfo(ERemotCMD.SetMemIOByte, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetMemIOByte, false);
                    }
                }
                //获取指定的内存 I/O端口(16)
                else if (args.Message.Contains("GetMemIOWord"))
                {
                    if (args.Message.Contains("#GetMemIOWord"))
                    {
                        WResponseInfo(ERemotCMD.GetMemIOWord, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.GetMemIOWord, false);
                    }
                }
                //设置指定的内存 I/O端口(16)
                else if (args.Message.Contains("SetMemIOWord"))
                {
                    if (args.Message.Contains("#SetMemIOWord"))
                    {
                        WResponseInfo(ERemotCMD.SetMemIOWord, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetMemIOWord, false);
                    }
                }
                //获取参数值
                else if (args.Message.Contains("SetVariable"))
                {
                    if (args.Message.Contains("#SetVariable"))
                    {
                        WResponseInfo(ERemotCMD.SetVariable, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.SetVariable, false);
                    }
                }
                //获取状态
                else if (args.Message.Contains("GetStatus"))
                {
                    if (args.Message.Contains("#GetStatus"))
                    {
                        WResponseInfo(ERemotCMD.GetStatus, true);
                        ResolveControlInfo(args.Message);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.GetStatus, false);
                    }
                }
                //执行命令
                else if (args.Message.Contains("Execute"))
                {
                    if (args.Message.Contains("#Execute"))
                    {
                        WResponseInfo(ERemotCMD.Execute, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Execute, false);
                    }
                    if (RResponseInfo(ERemotCMD.Execute) == true)
                    {
                        //机器人状态
                        if (CEParam.Contains(Spel.PrintRobotInfo_0))
                        {
                            if (args.Message.Length > 12) ResolveRobotInfo(args.Message);
                        }
                        //当前位置信息
                        else if (CEParam.Contains(Spel.RealPos))
                        {
                            ResolveCurPos(args.Message);
                        }
                        //是否在原点
                        else if (CEParam.Contains(Spel.PrintRobotInfo_2))
                        {
                            ResolveRobotHomeInfo(args.Message);
                        }
                        //起始点配置
                        else if (CEParam.Contains(Spel.HomeSet))
                        {
                            if (args.Message.Length > 12) ResolveRobotHomeSet(args.Message);
                        }
                        //关节脉冲值
                        else if (CEParam.Contains(Spel.PrintPls_1) ||
                            CEParam.Contains(Spel.PrintPls_2) ||
                            CEParam.Contains(Spel.PrintPls_3) ||
                            CEParam.Contains(Spel.PrintPls_4))
                        {
                            if (args.Message.Length > 12) ResolvePls(args.Message);
                        }
                        //各轴次序
                        else if (CEParam.Contains(Spel.Hordr))
                        {
                            if (args.Message.Length > 12) ResolveHordr(args.Message);
                        }
                    }
                }
                //中止命令
                else if (args.Message.Contains("Abort"))
                {
                    if (args.Message.Contains("#Abort"))
                    {
                        WResponseInfo(ERemotCMD.Execute, true);
                    }
                    else
                    {
                        WResponseInfo(ERemotCMD.Abort, false);
                    }
                }
                //非指令 接收点数据
                if (args.Message.Contains("= XY("))
                {
                    ResolvePoint(args.Message);
                }
                ReceiveError(sender, args);
                ReceiveOthre(sender, args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("recive exception");
                ShowLog(ex.ToString());
            }
        }

        ///接收错误响应信息
        private void ReceiveError(object sender, AsyTcpSocketEventArgs args)
        {
            if (args.Message.Substring(0, 1) == "!")
            {
                string[] array = args.Message.ToString().Split(',');
                if (array.Length > 1)
                {
                    Console.WriteLine("recive ReceiveError");
                    // Warn(string.Format("应答:错误内容 {0}，{1},{2}", args.Message, CEParam, ConvertErrorID(array[1])));
                    int lengthEndFlag = RobotCommucate.GetEndFlag().Length;
                    string str = args.Message.Length > lengthEndFlag ? args.Message.Substring(0, args.Message.Length - lengthEndFlag) : args.Message;

                    Warn(string.Format("应答:错误内容 {0}，{1},{2}", str, CEParam, ConvertErrorID(array[1])) + "\r\n");
                }
            }
        }

        ///接收其他响应信息
        private void ReceiveOthre(object sender, AsyTcpSocketEventArgs args)
        {
            //if (config.DisplayResponce)

            {
                if (!args.Message.Contains("!") &&
                    !args.Message.Contains("#GetStatus") &&
                     !args.Message.Contains("#GetIOWord") &&
                    !CEParam.Contains(Spel.PrintRobotInfo_0) &&
                    !CEParam.Contains(Spel.PrintRobotInfo_2))
                {
                    // if (config.EnableDisplayInfo)
                    {
                        Console.WriteLine("recive ReceiveOthre");
                        int lengthEndFlag = RobotCommucate.GetEndFlag().Length;
                        string str = args.Message.Length > lengthEndFlag ? args.Message.Substring(0, args.Message.Length - lengthEndFlag) : args.Message;
                        ShowLog(string.Format("应答:内容 {0} 用时 {1} ", str, swOutTime.ElapsedMilliseconds) + "\r\n");
                    }
                }
            }
        }

        ///过滤响应信息
        private bool ReceiveFilt(object sender, AsyTcpSocketEventArgs args)
        {
            if (args.Message == "" || args.Message == "\"")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 解析

        // 解析控制器状态
        private bool ResolveControlInfo(string str)
        {
            try
            {
                if (!str.Contains("#GetStatus")) return false;
                Status.ControlInfo.ErrorCode = Convert.ToInt32(str.Split(',')[2]);
                str = str.Split(',')[1];
                if (str.Substring(0, 1) == "1")
                {
                    Status.ControlInfo.Test = true;
                }
                else
                {
                    Status.ControlInfo.Test = false;
                }
                if (str.Substring(1, 1) == "1")
                {
                    Status.ControlInfo.Teach = true;
                }
                else
                {
                    Status.ControlInfo.Teach = false;
                }

                if (str.Substring(2, 1) == "1")
                {
                    Status.ControlInfo.Auto = true;
                }
                else
                {
                    Status.ControlInfo.Auto = false;
                }
                if (str.Substring(3, 1) == "1")
                {
                    Status.ControlInfo.Waring = true;
                }
                else
                {
                    Status.ControlInfo.Waring = false;
                }
                if (str.Substring(4, 1) == "1")
                {
                    Status.ControlInfo.SError = true;
                }
                else
                {
                    Status.ControlInfo.SError = false;
                }
                if (str.Substring(5, 1) == "1")
                {
                    Status.ControlInfo.Safeguard = true;
                }
                else
                {
                    Status.ControlInfo.Safeguard = false;
                }
                if (str.Substring(6, 1) == "1")
                {
                    Status.ControlInfo.EStop = true;
                }
                else
                {
                    Status.ControlInfo.EStop = false;
                }
                if (str.Substring(7, 1) == "1")
                {
                    Status.ControlInfo.Error = true;
                }
                else
                {
                    Status.ControlInfo.Error = false;
                }
                if (str.Substring(8, 1) == "1")
                {
                    Status.ControlInfo.Paused = true;
                }
                else
                {
                    Status.ControlInfo.Paused = false;
                }
                if (str.Substring(9, 1) == "1")
                {
                    Status.ControlInfo.Running = true;
                }
                else
                {
                    Status.ControlInfo.Running = false;
                }
                if (str.Substring(10, 1) == "1")
                {
                    Status.ControlInfo.Ready = true;
                }
                else
                {
                    Status.ControlInfo.Ready = false;
                }
                //if (statusOld.ControlInfo != Status.ControlInfo)
                if (m_GetRobotStateChangedHandle != null)
                    m_GetRobotStateChangedHandle(statusOld.ControlInfo);
                statusOld.ControlInfo = Status.ControlInfo;
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
                return false;
            }
            return true;
        }

        // 解析机器人状态
        private bool ResolveRobotInfo(string str)
        {
            RobotInfo robotInfo = new RobotInfo();
            try
            {
                int value = Convert.ToInt32(str.Split(',')[1].Substring(2, str.Split(',')[1].Length - 2));
                Status.RobotInfo.Enable = BitOperat.GetBit32B(value, 3); // bitOperat.GetBit32B(value, 3);
                Status.RobotInfo.Power = BitOperat.GetBit32B(value, 4);// bitOperat.GetBit32B(value, 4);
                Status.RobotInfo.Halt = BitOperat.GetBit32B(value, 8); //bitOperat.GetBit32B(value, 8);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString());
                return false;
            }
            return true;
        }

        //解析机器人原点状态
        private bool ResolveRobotHomeInfo(string str)
        {
            try
            {
                int value = Convert.ToInt32(str.Split(',')[1].Substring(2, str.Split(',')[1].Length - 2));
                Status.RobotInfo.Home = BitOperat.GetBit32B(value, 8); //bitOperat.GetBit32B(value, 0);
                return true;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString());
                return false;
            }
        }

        //解析机起始点
        private bool ResolveRobotHomeSet(string str)
        {
            try
            {
                Status.HomeSet = Array.ConvertAll(str.Substring(10, str.Length - 10).Split(','), p => Convert.ToInt32(p));
                return true;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString());
                return false;
            }
        }

        //解析 各轴动作次序
        private bool ResolveHordr(string str)
        {
            try
            {
                Status.Hordr = Array.ConvertAll(str.Substring(10, str.Length - 10).Split(','), p => Convert.ToInt32(p));
                return true;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString());
                return false;
            }
        }

        //解析当前位置
        private bool ResolveCurPos(string str)
        {
            try
            {
                string[] value = str.Split(',')[1].Split(':');
                Status.PosInfo.X = double.Parse(value[1].Substring(0, 9).Replace(" ", ""));
                Status.PosInfo.Y = double.Parse(value[2].Substring(0, 9).Replace(" ", ""));
                Status.PosInfo.Z = double.Parse(value[3].Substring(0, 9).Replace(" ", ""));
                Status.PosInfo.U = double.Parse(value[4].Substring(0, 9).Replace(" ", ""));
                Status.PosInfo.V = double.Parse(value[5].Substring(0, 9).Replace(" ", ""));
                Status.PosInfo.W = double.Parse(value[6].Substring(0, 9).Replace(" ", ""));
                Status.PosInfo.Hand = value[6].Contains("R") ? true : false;
                Status.PosInfo.LocalNo = Convert.ToInt32(value[6].Substring(6, value.Length - 6));
                //   if(Status.PosInfo!= statusOld.PosInfo)
                if (m_UpdateRobotStateChangedHandle != null)
                    m_UpdateRobotStateChangedHandle(Status.PosInfo);
                statusOld.PosInfo = Status.PosInfo;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString());
                return false;
            }
            return true;
        }

        //解析点信息
        private bool ResolvePoint(string str)
        {
            try
            {
                PositionInfo pi = new PositionInfo();
                string[] value = str.Split(',');
                if (value.Count() > 4)
                {
                    pi.PID = value[1].Substring(1, 3);
                    pi.X = double.Parse(value[1].Substring(14, 7));
                    pi.Y = double.Parse(value[2].Substring(0, 9));
                    pi.Z = double.Parse(value[3].Substring(0, 9));
                    pi.U = double.Parse(value[4].Substring(0, 9));
                    pi.Hand = value[4].Contains("R") ? true : false;
                    pi.LocalNo = Convert.ToInt32(value[4].Substring(17, 1));
                    pi.Tage = value[4].Substring(19, value[4].Length - 19);
                }
                else
                {
                    pi.PID = value[0].Substring(0, 3);
                    pi.X = double.Parse(value[0].Substring(13, 6));
                    pi.Y = double.Parse(value[1].Substring(0, 9));
                    pi.Z = double.Parse(value[2].Substring(0, 9));
                    pi.U = double.Parse(value[3].Substring(0, 9));
                    pi.Hand = value[3].Contains("R") ? true : false;
                    pi.LocalNo = Convert.ToInt32(value[3].Substring(17, 1));
                    pi.Tage = value[3].Substring(19, value[3].Length - 19);
                }
                Status.Points.Add(pi);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString());
                return false;
            }
            return true;
        }

        //解析个关节脉冲
        private bool ResolvePls(string str)
        {
            try
            {
                if (CEParam.Contains(Spel.PrintPls_1))
                {
                    Status.PosInfo.P1 = Convert.ToInt32(str.Substring(10, str.Length - 10));
                }
                else if (CEParam.Contains(Spel.PrintPls_2))
                {
                    Status.PosInfo.P2 = Convert.ToInt32(str.Substring(10, str.Length - 10));
                }
                else if (CEParam.Contains(Spel.PrintPls_3))
                {
                    Status.PosInfo.P3 = Convert.ToInt32(str.Substring(10, str.Length - 10));
                }
                else if (CEParam.Contains(Spel.PrintPls_4))
                {
                    Status.PosInfo.P4 = Convert.ToInt32(str.Substring(10, str.Length - 10));
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion 解析

        /// <summary>
        /// 执行的go函数
        /// </summary>
        /// <param name="EpsonPoint"></param>
        /// <returns></returns>
        public override bool AbsMove(RobotPoint EpsonPoint)
        {
            try
            {
                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                string TempStr = "";
                if (EpsonPoint.HandStyle == RobotHand.LeftHand)
                {
                    TempStr = "/L";
                }
                else
                {
                    TempStr = "/R";
                }

                if (FeedBackMessageFromRobot[i + 3] == '1'
                    & FeedBackMessageFromRobot[i + 11] == '1')
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Go XY(" + EpsonPoint.X.ToString() + ","
                        + EpsonPoint.Y.ToString() + "," + EpsonPoint.Z.ToString() + "," + EpsonPoint.U.ToString()
                        + "," + EpsonPoint.V.ToString() + "," + EpsonPoint.W.ToString() + ")" + TempStr + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "AbsMove";
                if (ErrorDisPlay != null)
                    ErrorDisPlay(this, ea);
                RobotExecuteBusy = false;
                return false;
            }
        }

        public override bool AbsMove(string EpsonPoint)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
                //----------------------------------------------------

                //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if (Strings.Mid(EpsonPoint.ToUpper(), 1, 1) != "P"
                    | (Convert.ToUInt16(Strings.Mid(EpsonPoint.ToUpper(), 2, EpsonPoint.Length - 1)) < 0
                    & Convert.ToUInt16(Strings.Mid(EpsonPoint.ToUpper(), 2, EpsonPoint.Length - 1)) > 999))
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1'
                    & FeedBackMessageFromRobot[i + 11] == '1')
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Go " + EpsonPoint + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "AbsMove";
                ErrorDisPlay(this, ea);
                RobotExecuteBusy = false;
                return false;
            }
        }

        public override bool Close()
        {
            if (Logout() == false)
            {
                return false;
            }
            else
            {
                EpsonErrLog.ErrorMess = "current robot not open\r\n";
                EpsonErrLog.ErrorCmd = "close";
                ErrorDisPlay(this, new EventArgs());
                return false;
            }
        }

        /// <summary>
        /// 获取实际坐标值（不可用）
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <returns></returns>
        public override int GetAxisActPos(int nAxisNo)
        {
            return -1;
        }

        public override bool GetAxisActPos(ref PositionInfo EpsonPos)
        {
            try
            {
                bool? bRtn = ExecuteCMD(ERemotCMD.Execute, Spel.RealPos);
                if (bRtn == null)
                    return false;
                else if (bRtn == true)
                {
                    EpsonPos = Status.PosInfo;
                    return true;
                }
                else
                    return false;

                #region old code

#if false
                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetAxisActPos";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "GetAxisActPos";
                //   // ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //if (FeedBackMessageFromRobot[i + 3] == '1'
                //    & FeedBackMessageFromRobot[i + 11] == '1')
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Print RealPos\"");
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "GetAxisActPos";
                //   // ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //$Execute,"Print RealPos"
                //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

                //$execute,"print curpos"
                //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    try
                    {
                        int i;
                        i = FeedBackMessageFromRobot.IndexOf(":", 0);
                        EpsonPos.X = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        //EpsonPos.X = Convert.ToInt32(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPos.Y = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        //EpsonPos.Y = Convert.ToInt32(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPos.Z = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        //EpsonPos.Z = Convert.ToInt32(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPos.U = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        //EpsonPos.U = Convert.ToInt32(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPos.V = Convert.ToInt32(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPos.W = Convert.ToInt32(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        //判断左右手势
                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/L") != -1)
                        {
                            EpsonPos.HandStyle = RobotHand.LeftHand;
                        }

                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/R") != -1)
                        {
                            EpsonPos.HandStyle = RobotHand.RightHand;
                        }
                    }
                    catch (Exception ex)
                    {
                        EpsonErrLog.ErrorMess = ex.Message; ;
                        EpsonErrLog.ErrorCmd = "GetAxisActPos";
                        //  ErrorDisPlay(this, ea);
                        this.RobotExecuteBusy = false;
                        return false;
                    }

                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetAxisActPos";
                    //ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
#endif

                #endregion old code
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GetAxisActPos";
                //ErrorDisPlay(this, ea);
                RobotExecuteBusy = false;
                return false;
            }
        }

        /// <summary>
        /// 获取当前位置坐标值（不可用）
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <returns></returns>
        public override int GetAxisCmdPos(int nAxisNo)
        {
            return -1;
        }

        public override int GetAxisPos(int nAxisNo)
        {
            return -1;
        }

        public override long GetMotionIoState(int nAxisNo)
        {
            long WordStatus = 0;
            string StrErrLog = string.Empty;
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //GetIOWord   I/O字端口号   获得指定的I/O字端口（16位）   随时可用
                //----------------------------------------------------------------

                WordStatus = 0;

                if (nAxisNo > 1 | nAxisNo < 0)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "GetMotionIoState";
                    ErrorDisPlay(this, ea);
                    return -1;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetMotionIoState";
                    ErrorDisPlay(this, ea);
                    return -1;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "GetMotionIoState";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return -1;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$GetIOWord," + nAxisNo);
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "GetMotionIoState";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return -1;
                //}

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    //在此添加判断返回的位的值为0或者1
                    //GetIOWord    #GetIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端

                    string[] TempStr = FeedBackMessageFromRobot.Split(',');

                    if (TempStr[0] != "#GetIOWord")
                    {
                        RobotExecuteBusy = false;
                        return -1;
                    }

                    WordStatus = Convert.ToUInt16(HexToDecimal(Strings.Right(FeedBackMessageFromRobot, 4), StrErrLog));

                    RobotExecuteBusy = false;
                    return WordStatus;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetMotionIoState";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return -1;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "AbsMove";
                ErrorDisPlay(this, ea);
                return -1;
            }
        }

        public override bool GetServoState(int nAxisNo)
        {
            return ServoFlag;
        }

        public override bool Home(int nAxisNo, int nParam)
        {
            try
            {
                bool? bRtn = ExecuteCMD(ERemotCMD.Execute, Spel.Home);
                if (bRtn == null)
                    return false;
                else
                    return (bool)bRtn;
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GoHome";
                // ErrorDisPlay(this, ea);
                RobotExecuteBusy = false;
                return false;
            }
        }

        public override int IsAxisNormalStop(int nAxisNo)
        {
            return -1;
        }

        public override bool IsHomeNormalStop(int nAxisNo)
        {
            return true;
        }

        public override bool JogMove(int nAxisNo, bool bPositive, int bStart, int nSpeed)
        {
            return RelativeMove(nAxisNo, 1, nSpeed);
        }

        public void OnReciveDataFromEpson(object sender, AsyTcpSocketEventArgs e)
        {
            Receive_Data(null, e);
        }

        public override bool Open()
        {
            if (this.SuccessBuiltNew == true)
            {
                EpsonErrLog.ErrorMess = "Device is Open\r\n";
                EpsonErrLog.ErrorCmd = "Open";
                ErrorDisPlay(this, new EventArgs());
                return false;
            }
            else
            {
                Thread.Sleep(100);
                if (ConnectEPSON() == false)
                {
                    return false;
                }
                RobotCommucate.RecvStringMessageEvent += OnReciveDataFromEpson;
                Thread.Sleep(100);
                swExeEndTime.Restart();
                robotInfo.timeout = 30000;
                if (Login(this.robotInfo.PassWard) == false)
                {
                    return false;
                }
                if (!ResetRobot())
                    return false;
                ShowLog("机器人重置成功");
                Thread.Sleep(100);
                if (ServoOn() == false)
                {
                    return false;
                }
                isOpenFlag = true;
                //if (SetPowerMode(RobotPower.HIGHPOWER) == true)
                //{
                //    return false;
                //}
                ////设置速度
                //if (!SetSpeed(30)) { return false; }
                //isOpenFlag = true;
            }
            return true;
        }

        public bool? ExecuteCMD(ERemotCMD cmd, params string[] param)
        {
            if (!RobotCommucate.IsConnected) { return null; }
            bool bIsLogin = rs.Login == null ? false : (bool)rs.Login;
            if (!bIsLogin) { Warn("未连接登陆,执行远程命令失败."); return null; }
            lock (lockER)
            {
                try
                {
                    swOutTime.Restart();
                    while (swExeEndTime.ElapsedMilliseconds < 2) { Thread.Sleep(1); }
                    WResponseInfo(cmd, null);
                    if (param.Count() > 0) CEParam = param[0];
                    string strCmd = CreateCMD(cmd, param);
                    strCmd = strCmd + RobotCommucate.GetEndFlag();
                    ShowLog("RobotSend:" + strCmd);
                    byte[] buffer = System.Text.Encoding.Default.GetBytes(strCmd);

                    RobotCommucate.WriteData(buffer, strCmd.Length);

                    if (false)//config.DisplayTime
                    {
                        //if (cmd != ERemotCMD.GetStatus && cmd != ERemotCMD.GetIOWord)
                        //{
                        //    if (config.EnableDisplayInfo)
                        //        NotifyG.Info(string.Format("请求:命令 {0}", CreateCMD(cmd, param)));
                        //}
                    }
                    while (RResponseInfo(cmd) == null)
                    {
                        if (swOutTime.ElapsedMilliseconds > robotInfo.timeout)
                        {
                            ShowLog(string.Format("应答超时：命令 {0} 超时 {1}", cmd, robotInfo));
                            return false;
                        }
                        Thread.Sleep(1);
                    }
                    swExeEndTime.Restart();
                }
                catch (Exception ex)
                {
                    ShowLog(ex.Message);
                    return false;
                }
            }
            return RResponseInfo(cmd);
        }

        public override bool ReasetAxis(int nAxisNo)
        {
            return ResetRobot();
        }

        /// <summary>
        /// 默认是本地坐标系
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <param name="nPos"></param>
        /// <param name="nSpeed"></param>
        /// <returns></returns>
        public override bool RelativeMove(int nAxisNo, double nPos, int nSpeed)
        {
            string AxisNum = string.Empty;

            if (false == SetSpeed(nSpeed))
            {
                EpsonErrLog.ErrorMess = "Speed set failure\r\n";
                EpsonErrLog.ErrorCmd = "RelativeMove";
                //ErrorDisPlay(this, ea);
                return false;
            }
            switch (nAxisNo)
            {
                case 0:
                    AxisNum = "X";
                    break;

                case 1:
                    AxisNum = "Y";
                    break;

                case 2:
                    AxisNum = "Z";
                    break;

                case 3:
                    AxisNum = "U";
                    break;

                case 4:
                    AxisNum = "V";
                    break;

                case 5:
                    AxisNum = "W";
                    break;

                default:
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "RelativeMove";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    return false;
            }
            try
            {
                bool? brtn;
                brtn = ExecuteCMD(ERemotCMD.Execute, new string[3] { Spel.Accel, "20", "20" });
                brtn = brtn & ExecuteCMD(ERemotCMD.Execute, new string[3] { Spel.Accels, "20", "20" });
                string dis = string.Format("Here + {0}({1})", AxisNum, nPos.ToString());
                if (nAxisNo != 3)
                {
                    brtn &= ExecuteCMD(ERemotCMD.Execute, new string[] { Spel.Move, dis });
                }
                else
                {
                    brtn &= ExecuteCMD(ERemotCMD.Execute, new string[2] { Spel.Go, dis });
                }
                if (brtn == null)
                    return false;
                else
                    return (bool)brtn;
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "RelativeMove";
                if (ErrorDisPlay != null)
                    ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool RelativeMove(int nAxisNo, double nPos, int nSpeed, CoordinateSys EpsonCoord)
        {
            string Cmd = string.Empty;
            if (EpsonCoord == CoordinateSys.Local)
            {
                Cmd = "Move";
            }
            else if (EpsonCoord == CoordinateSys.Tool)
            {
                Cmd = "TMove";
            }
            else
            {
                EpsonErrLog.ErrorMess = "not select Coordinate\r\n";
                EpsonErrLog.ErrorCmd = "RelativeMove";
                ErrorDisPlay(this, ea);
                return false;
            }
            string AxisNum = string.Empty;

            if (false == SetSpeed(nSpeed))
            {
                EpsonErrLog.ErrorMess = "Speed set failure\r\n";
                EpsonErrLog.ErrorCmd = "RelativeMove";
                ErrorDisPlay(this, ea);
                return false;
            }
            switch (nAxisNo)
            {
                case 0:
                    AxisNum = "X";
                    break;

                case 1:
                    AxisNum = "Y";
                    break;

                case 2:
                    AxisNum = "Z";
                    break;

                case 3:
                    AxisNum = "U";
                    break;

                case 4:
                    AxisNum = "V";
                    break;

                case 5:
                    AxisNum = "W";
                    break;

                default:
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "RelativeMove";
                    ErrorDisPlay(this, ea);
                    return false;
            }
            try
            {
                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "RelativeMove";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "RelativeMove";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"" + Cmd + " Here + " + AxisNum + "(" +
                    nPos + ")" + "\"");
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "RelativeMove";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "RelativeMove";
                    //   ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "RelativeMove";
                // ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool ServoOff()
        {
            // nAxisNo = 1;
            try
            {
                bool? brtn = ExecuteCMD(ERemotCMD.SetMotorsOff, new string[] { "1" });
                bool bok = brtn == null ? false : (bool)brtn;
                return bok;
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "ServoOff";
                //  ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool ServoOn()
        {
            // nAxisNo = 1;
            try
            {
                bool? brtn = ExecuteCMD(ERemotCMD.SetMotorsOn, new string[] { "1" });
                bool bok = brtn == null ? false : (bool)brtn;
                return bok;
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "ServoOn";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool StopAxis(int nAxisNo)
        {
            try
            {
                if (nAxisNo < 1 | nAxisNo > 6)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "StopAxis";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "StopAxis";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "StopAxis";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //Brake
                //用于打开和关闭当前机械手指定关节的制动器。
                //格式
                //Brake 状态, 关节编号
                //参数
                //状态 施加制动时：使用On。
                //解除制动时：使用Off。
                //关节编号 指定1～6 的关节编号。
                //说明
                //Brake 命令用于对垂直6 轴型机械手的一个关节施加或解除制动。这是仅可通过命令使用的命令。此
                //命令设计为只有维修作业人员才可以使用。
                //如果执行Brake 命令，则会对机械手控制参数进行初始化。

                //brake on, 1
                //brake off, 1

                //if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Brake on, " +
                    nAxisNo + "\"");
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "StopAxis";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "StopAxis";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "StopAxis";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override string RobotSendCmd(string Command)
        {
            string strReceived = "";
            try
            {
                if (Command == "")
                {
                    EpsonErrLog.ErrorMess = "Not Message Command\r\n";
                    EpsonErrLog.ErrorCmd = "SendCmd\r\n";
                    ErrorDisPlay(this, ea);
                    return "";
                }
                if (ConnectRemoteEpsonTCPIPOk == false)
                {
                    EpsonErrLog.ErrorMess = "Not Connect\r\n";
                    EpsonErrLog.ErrorCmd = "SendCmd\r\n";
                    ErrorDisPlay(this, ea);
                    return "";
                }
#if true
                byte[] BytesReceived = new byte[4096];
                byte[] SendBytes = new byte[4096];

                Command = Command + RobotCommucate.GetEndFlag();
                //发送
                SendBytes = System.Text.Encoding.ASCII.GetBytes(Command);
                RobotCommucate.WriteData(SendBytes, SendBytes.Length);
                strReceived = "";
                //发送完之后接收EPSON返回内容
                Thread.Sleep(80);
                FeedBackMessageFromRobot = "";
                int i = RobotCommucate.ReadData(BytesReceived, 4096);
                FeedBackMessageFromRobot = System.Text.Encoding.ASCII.GetString(BytesReceived, 0, i);
                strReceived = FeedBackMessageFromRobot;
                return strReceived;
#endif
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message;
                EpsonErrLog.ErrorCmd = "RobotSendCmd";
                ErrorDisPlay(this, ea);
                return strReceived;
            }
        }

        public override bool Login(string LoginRobotPassword)
        {
            try
            {
                string strCmd = CreateCMD(ERemotCMD.Login, robotInfo.PassWard);
                strCmd = strCmd + RobotCommucate.GetEndFlag();
                byte[] buffer = System.Text.Encoding.Default.GetBytes(strCmd);
                RobotCommucate.WriteData(buffer, strCmd.Length);
                Stopwatch stopwatch = new Stopwatch();
                Thread.Sleep(200);
                bool bExceRtn = false;
                stopwatch.Restart();
                while (rs.Login == null)
                {
                    if (stopwatch.ElapsedMilliseconds > 3000)
                        break;
                    Thread.Sleep(1);
                }
                if (rs.Login == null)
                { ShowLog("登陆机器人失败"); return false; }
                else if (rs.Login == false)
                {
                    ShowLog("登陆机器人失败"); return false;
                }
                else
                {
                    ShowLog("登陆机器人成功");
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message;
                EpsonErrLog.ErrorCmd = "Login";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool Logout()
        {
            try
            {
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "Logout";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    this.RobotExecuteBusy = true;
                }
                FeedBackMessageFromRobot = RobotSendCmd("$Logout,");
                if (FeedBackMessageFromRobot.IndexOf("!") == -1)  //未找到
                {
                    this.RobotExecuteBusy = false;
                    EpsonErrLog.ErrorMess = "Not use Command\r\n";
                    EpsonErrLog.ErrorCmd = "Logout";
                    ErrorDisPlay(this, new EventArgs());
                    return false;
                }
                else
                {
                    this.RobotExecuteBusy = false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message;
                EpsonErrLog.ErrorCmd = "Login";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool StartMission(int NumberOfMission)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Start               功能编号               执行指定编号的功能                                Auto开/Ready开/Error关/EStop关/Safeguard开
                //--------------------------------------------------------------

                if (NumberOfMission < 0 | NumberOfMission > 7)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "StartMission";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "StartMission";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    this.RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    this.RobotExecuteBusy = false;
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "StartMission";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1'
                    & FeedBackMessageFromRobot[i + 11] == '1')
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Start," + NumberOfMission);
                }
                else
                {
                    this.RobotExecuteBusy = false;
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "StartMission";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1)
                {
                    this.RobotExecuteBusy = false;

                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "StartMission";
                    ErrorDisPlay(this, ea);
                    this.RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "StartMission";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool GetRobotStatus(ref string Statuss, bool bGetFromBuff = false)
        {
            try
            {
                //  if(bGetFromBuff)

                //   bool? brtn = ExecuteCMD(ERemotCMD.GetStatus);
                ExecuteCMD(ERemotCMD.GetIOWord, "0");
                // ExecuteCMD(ERemotCMD.Execute, Spel.RealPos);
                //  int nBit = BitOperat.GetBit16((ushort)Status.IoIn0, 1);
                // ShowLog(nBit.ToString());
                return true;
            }
            catch (Exception ex)
            {
                this.RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GetRobotStatus";
                if (ErrorDisPlay != null)
                    ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override string ProcessResponse(string ResponseString)
        {
            string TempResponse = "", TempStr = "";

            try
            {
                //错误响应
                //如果控制器不能正确接收远程命令,将以下列格式显示错误响应。
                //格式：![远程命令],[错误代码]终端
                if (ResponseString == "") { MessageBox.Show("没收到控制器的答复"); return TempResponse + "没收到控制器的答复"; }
                if (Strings.InStr(ResponseString, "!") == -1)
                {
                    return ResponseString + "   " + "成功执行完远程命令";
                }
                else
                {
                    ushort ResponseCode = 0;
                    string[] StrArray;
                    StrArray = Strings.Split(ResponseString, ",");
                    ResponseCode = Convert.ToUInt16(StrArray[1]);

                    switch (ResponseCode)
                    {
                        case 10:
                            //10              远程命令未以$开头
                            TempResponse = ResponseString + "   " + "远程命令未以$开头";
                            break;

                        case 11:
                            //11              远程命令错误 / 未执行Login
                            TempResponse = ResponseString + "   " + "远程命令错误 / 未执行Login";
                            break;

                        case 12:
                            //12              远程命令格式错误
                            TempResponse = ResponseString + "   " + "远程命令格式错误";
                            break;

                        case 13:
                            //13              Login命令密码错误
                            TempResponse = ResponseString + "   " + "Login命令密码错误";
                            break;

                        case 14:
                            //14              要获取的指定数量超出范围(小于1或大于100) / 忽略了要获取的数量 / 指定了一个字符串参数
                            TempResponse = ResponseString + "   " + "要获取的指定数量超出范围(小于1或大于100) / 忽略了要获取的数量 / 指定了一个字符串参数";
                            break;

                        case 15:
                            //15              参数不存在 / 参数尺寸错误 / 调用了超出了范围的元素
                            TempResponse = ResponseString + "   " + "参数不存在 / 参数尺寸错误 / 调用了超出了范围的元素";
                            break;

                        case 19:
                            //19              请求超时
                            TempResponse = ResponseString + "   " + "请求超时";
                            break;

                        case 20:
                            //20              控制器未准备好
                            TempResponse = ResponseString + "   " + "控制器未准备好";
                            break;

                        case 21:
                            //21              因为正在运行Execute,所以无法执行
                            TempResponse = ResponseString + "   " + "因为正在运行Execute,所以无法执行";
                            break;

                        case 99:
                            //99              系统错误 / 通信错误
                            TempResponse = ResponseString + "   " + "系统错误 / 通信错误";
                            break;

                        default:
                            TempResponse = ResponseString + "   " + "未知错误";
                            break;
                    }
                }
                TempStr = TempResponse;
                return TempResponse;
            }
            catch (Exception ex)
            {
                this.RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message;
                EpsonErrLog.ErrorCmd = "ProcessResponse";
                ErrorDisPlay(this, ea);
                return "";
            }
        }

        public override bool GetPointPos(string PointName, ref RobotPoint PointData)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令     Auto开/Ready开/Error关/EStop关/Safeguard关
                //----------------------------------------------------

                //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if (Strings.Mid(PointName.ToUpper(), 1, 1) != "P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) < 0
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) > 999))
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "GetPointPos";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetPointPos";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    this.RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "GetPointPos";
                //    if (ErrorDisPlay != null)
                //        ErrorDisPlay(this, ea);
                //    this.RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //if (FeedBackMessageFromRobot[i + 3] == '1'
                //    & FeedBackMessageFromRobot[i + 11] == '1')
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Print "
                    + PointName + "\"");
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "GetPointPos";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //$Execute,"Print RealPos"
                //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

                //$execute,"print curpos"
                //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    try
                    {
                        //i = FeedBackMessageFromRobot.IndexOf(":", 0);
                        //PointData.X = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        //i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        //PointData.Y = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        //i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        //PointData.Z = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        //i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        //PointData.U = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        //i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        //PointData.V = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        //i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        //PointData.W = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        //判断左右手势
                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/L") != -1)
                        {
                            PointData.HandStyle = RobotHand.LeftHand;
                        }

                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/R") != -1)
                        {
                            PointData.HandStyle = RobotHand.RightHand;
                        }
                    }
                    catch (Exception ex)
                    {
                        EpsonErrLog.ErrorMess = ex.Message; ;
                        EpsonErrLog.ErrorCmd = "GetPointPos";
                        ErrorDisPlay(this, ea);
                        this.RobotExecuteBusy = false;
                        return false;
                    }

                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetPointPos";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GetPointPos";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        //获取当前位置的坐标值
        /// <summary>
        /// 获取当前位置的坐标值
        /// </summary>
        /// <param name="PointData">EPSON点数据结构</param>
        /// <returns></returns>
        public override bool GetCurrentPos(ref RobotPoint PointData)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
                //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "GetPointPos";
                //    if(ErrorDisPlay!=null )
                //    ErrorDisPlay(this, ea);
                //    this.RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //if (FeedBackMessageFromRobot[i + 3] == '1'
                //    & FeedBackMessageFromRobot[i + 11] == '1')
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Print  RealPos\"");
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "GetPointPos";
                //    if (ErrorDisPlay != null)
                //        ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //$Execute,"Print RealPos"
                //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

                //$execute,"print curpos"
                //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    try
                    {
                        int i;
                        i = FeedBackMessageFromRobot.IndexOf(":", 0);
                        PointData.X = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.Y = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.Z = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.U = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.V = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.W = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

                        //判断左右手势
                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/L") != -1)
                        {
                            PointData.HandStyle = RobotHand.LeftHand;
                        }

                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/R") != -1)
                        {
                            PointData.HandStyle = RobotHand.RightHand;
                        }
                    }
                    catch (Exception ex)
                    {
                        EpsonErrLog.ErrorMess = ex.Message; ;
                        EpsonErrLog.ErrorCmd = "GetPointPos";
                        if (ErrorDisPlay != null)
                            ErrorDisPlay(this, ea);
                        this.RobotExecuteBusy = false;
                        return false;
                    }

                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetPointPos";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                EpsonErrLog.ErrorCmd = "GetPointPos";
                if (ErrorDisPlay != null)
                    ErrorDisPlay(this, ea);
                RobotExecuteBusy = false;
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SetPointPos(string PointID, string strLabel, PositionInfo NewPointData)
        {
            try
            {
                int index = 0;
                if (PointID.Length > 1)
                    index = int.Parse(PointID.Substring(1, PointID.Length - 1));
                else
                    return false;
                bool? bRtn;

                string[] cmd = new string[2]{(PointID),string.Format("=XY({0},{1},{2},{3},{4},{5})/{6}/{7}",
                    NewPointData.X,NewPointData.Y,NewPointData.Z,
                    NewPointData.U,NewPointData.V,NewPointData.W,
                    NewPointData.Hand?"R":"L",NewPointData.LocalNo)};

                ExecuteCMD(ERemotCMD.Execute, cmd);
                bRtn = ExecuteCMD(ERemotCMD.Execute, string.Format("{0} {1},\"{2}\"", Spel.PLabel, index, strLabel));

                if (m_dicPointIdLabel.ContainsKey(PointID))
                    m_dicPointIdLabel[PointID] = strLabel;
                else
                    m_dicPointIdLabel.Add(PointID, strLabel);

                if (m_dicLabelPointId.ContainsKey(strLabel))
                    m_dicLabelPointId[strLabel] = PointID;
                else
                    m_dicLabelPointId.Add(strLabel, PointID);
                if (bRtn == null)
                    return false;
                else
                    return (bool)bRtn;
#if false
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
                //-----------------------------------------------------

                //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if (Strings.Mid(PointName.ToUpper(), 1, 1) != "P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) < 0
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) > 999))
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SetPointPos";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SetPointPos";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "SetPointPos";
                //    //ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                string TempStr = "";
                if (NewPointData.HandStyle == RobotHand.LeftHand)
                {
                    TempStr = "/L";
                }
                else if (NewPointData.HandStyle == RobotHand.RightHand)
                {
                    TempStr = "/R";
                }
                else if (NewPointData.HandStyle == RobotHand.Unknow)
                {
                    TempStr = "";
                }

                //if (FeedBackMessageFromRobot[i + 3] == '1'
                //    & FeedBackMessageFromRobot[i + 11] == '1')
                //{
                // X: -156.600 Y:  274.958 Z:  -14.076 U:  -48.417 V:    0.000 W:    0.000 /L /0
                FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"" + PointName + "=XY(" + NewPointData.X.ToString() + ","
                    + NewPointData.Y.ToString() + "," + NewPointData.Z.ToString() + "," + NewPointData.U.ToString() + "," + NewPointData.V.ToString()
                    + "," + NewPointData.W.ToString() + ")" + TempStr + "\"");
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "SetPointPos";
                //    ErrorDisPlay(this, ea);
                //    this.RobotExecuteBusy = false;
                //    return false;
                //}

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SetPointPos";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
#endif
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SetPointPos";
                MessageBox.Show("SetPointPos" + ex.ToString());
                //ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool Jump(string PointName)
        {
            try
            {
                string cmd = "Jump";
                string point = PointName;
                bool? bRtn = ExecuteCMD(ERemotCMD.Execute, string.Format("{0} {1} ",
                        cmd,
                        point));

                if (bRtn == null)
                    return false;
                else
                    return (bool)bRtn;
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "Jump";
                //ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SavePointPos()
        {
            try
            {
                bool? bRtn = ExecuteCMD(ERemotCMD.Execute, string.Format("{0} \"{1}\"", Spel.SavePoints, "robot1.pts"));
                if (bRtn == null)
                    return false;
                else
                    return (bool)bRtn;
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SavePointPos";
                // ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool SavePointPosWithSaveDialog()
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
                //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                SaveFileDialog TempSaveFile = new SaveFileDialog();
                TempSaveFile.AddExtension = true;
                TempSaveFile.CheckFileExists = true;
                TempSaveFile.DefaultExt = "pts";
                TempSaveFile.Filter = "EPSON点数据文件|*.pts";

                if (TempSaveFile.ShowDialog() == DialogResult.OK)
                {
                    if (TempSaveFile.FileName != "")
                    {
                        //文件名称需要进行处理或者验证此方法是否可行
                        int i;
                        i = FeedBackMessageFromRobot.IndexOf(",");

                        if (FeedBackMessageFromRobot[i + 3] == '1'
                            & FeedBackMessageFromRobot[i + 11] == '1')
                        {
                            FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"SavePoints " + TempSaveFile.FileName + "\"");
                            FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"LoadPoints " + TempSaveFile.FileName + "\"");
                        }
                        else
                        {
                            RobotExecuteBusy = false;
                            return false;
                        }
                    }
                    else
                    {
                        EpsonErrLog.ErrorMess = "file deil error\r\n";
                        EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
                        ErrorDisPlay(this, ea);
                        RobotExecuteBusy = false;
                        return false;
                    }
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool ConnectEPSON()
        {
            if (ConnectRemoteEpsonTCPIPOk == false)
            {
                if (RobotCommucate.IsOpen() == true)
                {
                    EpsonErrLog.ErrorMess = "InterNet is Connect\r\n";
                    EpsonErrLog.ErrorCmd = "ConnectEPSON";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    ConnectRemoteEpsonTCPIPOk = true;
                    return true;
                }
                if (RobotCommucate.Open() == false)
                {
                    EpsonErrLog.ErrorMess = "InterNet is Connect failure\r\n";
                    EpsonErrLog.ErrorCmd = "ConnectEPSON";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    ConnectRemoteEpsonTCPIPOk = false;
                    return false;
                }
                ConnectRemoteEpsonTCPIPOk = true;
                if (Login(RobotInfo1.PassWard) == false)
                {
                    RobotCommucate.Close();
                    ConnectRemoteEpsonTCPIPOk = false;
                    return false;
                }
                else
                {
                    ConnectRemoteEpsonTCPIPOk = false;
                    return true;
                }
            }
            else
            {
                EpsonErrLog.ErrorMess = "InterNet is Connect 1\r\n";
                EpsonErrLog.ErrorCmd = "ConnectEPSON";
                if (ErrorDisPlay != null)
                    ErrorDisPlay(this, ea);
                ConnectRemoteEpsonTCPIPOk = false;
                return false;
            }
        }

        public override bool GetVariable(ref string VariableName, ref string[] Value)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //GetVariable         参数名称{, type}       获取备份（Global Preserve）参数的值               随时可用
                //                    ***********************************************************
                //                    (参数名称)（数组元
                //                    素）,(参数名称类
                //                    型),(获取的编号)      获取备份（Global Preserve）数组参数的值
                //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String}。
                //指定的类型： 在参数名称和类型相同时用于备份参数。
                //未指定的类型： 在参数名称相同时用于备份参数。
                //(*9) 对于数组元素,指定以下您想获取的元素：
                //如果是从数组头处获取的,您需要指定一个元素。
                //1维数组 参数名称 (0) 从头部获取。
                //参数名称（元素编号） 从指定的元素编号中获取。
                //2维数组 参数名称 (0,0) 从头部获取。
                //参数名称（元素编号1,2） 从指定的元素编号中获取。
                //3维数组 参数名称 (0,0,0) 从头部获取。
                //参数名称（元素编号1,2,3） 从指定的元素编号中获取。
                //您不能忽略要获取的参数类型和编号。
                //您不能指定一个参数类型string。
                //可获取的可用数量多达100个。如果您在数组元素编号上指定一个号码,会发生错误。
                //如）“$GetVariable,gby2(3,0),Byte,3”
                //它获得字节型2维数组参数gby2的gby2(3,0)、gby2(3,1)、gby2(3,2)的值。
                //--------------------------------------------------------------------------------

                if (VariableName == "")
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "GetVariable";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetVariable";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "GetVariable";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$GetVariable," + VariableName);
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "GetVariable";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    try
                    {
                        //GetVariable                    #GetVariable,[参数值] 终端
                        //---------------------------------------------------------------------------------
                        //GetVariable（如果是数组）      #GetVariable,[参数值 1],[参数值 2],...,终端 *4
                        //*4 返回要获取的编号中指定编号的值。

                        string[] TempStr = FeedBackMessageFromRobot.Split(',');

                        if (TempStr[0] != "#GetVariable")
                        {
                            EpsonErrLog.ErrorMess = "return error\r\n";
                            EpsonErrLog.ErrorCmd = "GetVariable";
                            ErrorDisPlay(this, ea);
                            Value = null;
                            return false;
                        }

                        if (TempStr.Length == 2)
                        {
                            Value = new string[1];
                            Value[0] = TempStr[1];
                        }
                        else
                        {
                            Value = new string[TempStr.Length];
                            for (int a = 0; a < TempStr.Length; a++)
                            {
                                Value[a] = TempStr[a];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EpsonErrLog.ErrorMess = ex.Message; ;
                        EpsonErrLog.ErrorCmd = "GetVariable";
                        ErrorDisPlay(this, ea);
                        RobotExecuteBusy = false;
                        //AddText(ex.Message);
                        return false;
                    }

                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetVariable";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GetVariable";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SetVariable(string VariableName, string Value, RobotVariable VariableType)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //SetVariable        参数名称和值{,类型}    设置备份（Global Preserve）参数中的值             Auto开/Ready开
                //---------------------------------------------------------------------------------------------------
                //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String | Short | UByte | UShort | Int32 |
                //UInt32 | Int64 | UInt64}。
                //指定类型：在参数名称和类型相同时用于备份参数。
                //未指定类型：在参数名称相同时用于备份参数。

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SetVariable";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SetVariable";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$SetVariable,"
                        + VariableName + "," + Value + "," +
                        Strings.Mid(VariableType.ToString(), 3,
                        VariableType.ToString().Length - 2));
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SetVariable";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SetVariable";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SetVariable";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool GetToolSetting(int NumberOfTool)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
                //----------------------------------------------------

                //TLSet
                //用于设置/显示工具坐标系。

                //格式
                //(1) TLSet 工具坐标系编号, 工具设置数据
                //(2) TLSet 工具坐标系编号
                //(3) TLSet

                //参数
                //工具坐标系编号 以1～15 的整数值指定要设置的工具。（Tool 0 为默认工具,不能变更。）
                //工具设置数据 以P 编号、P（表达式）、点标签点或表达式指定要设置的工具坐标系的原点和
                //方向。

                //结果
                //如果省略所有参数,则显示所有的TLSet 设置。
                //如果只指定工具编号,则显示指定的TLSet 设置。

                //说明
                //指定针对Tool 0 坐标系（夹具末端坐标系）的相对原点位置和相对旋转角度,定义工具坐标系
                //Tool 1、Tool 2 、Tool 3。
                //        TLSet(1, XY(50, 100, -20, 30)) ----  【X,Y,Z,U】
                //TLSet(2, P10 + X(20))
                //上述情况时,引用坐标值P10 并在X 值上加上20。无视机械臂属性和本地坐标系编号。
                //ツール座標系の原点のZ(軸方向位置)
                //ツール座標系の原点のY(軸方向位置(次図b))
                //ツール座標系の原点のX(軸方向位置(次図a))
                //ツール座標系番号
                //TLSET(1, XY(100, 60, -20, 30))

                if (NumberOfTool < 1 | NumberOfTool > 15)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "GetToolSetting";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetToolSetting";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "GetToolSetting";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"print TLSET(" + NumberOfTool + ")\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "GetToolSetting";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    // 需要得到返回值得样本再进行处理
                    // $Execute,"tlset 1"
                    // !Execute, 99
                    // $Execute,"tlset"
                    // #Execute,0
                    // $Execute,"print tlset"
                    // !Execute, 99
                    // $Execute,"print tlset 0"
                    // !Execute, 99
                    // $Execute,"print tlset 1"
                    // !Execute, 99
                    // $Execute,"tlset 1"
                    // !Execute, 99
                    // i = FeedBackMessageFromRobot.IndexOf(":", 0)
                    // ToolX = FeedBackMessageFromRobot.Substring(i + 1, 10)
                    // i = FeedBackMessageFromRobot.IndexOf(":", i + 1)
                    // ToolY = FeedBackMessageFromRobot.Substring(i + 1, 10)

                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetToolSetting";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GetToolSetting";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SetTool(int NumberOfTool, double X, double Y, double Z, double U)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
                //----------------------------------------------------

                //TLSet
                //用于设置/显示工具坐标系。

                //格式
                //(1) TLSet 工具坐标系编号, 工具设置数据
                //(2) TLSet 工具坐标系编号
                //(3) TLSet

                //参数
                //工具坐标系编号 以1～15 的整数值指定要设置的工具。（Tool 0 为默认工具,不能变更。）
                //工具设置数据 以P 编号、P（表达式）、点标签点或表达式指定要设置的工具坐标系的原点和
                //方向。

                //结果
                //如果省略所有参数,则显示所有的TLSet 设置。
                //如果只指定工具编号,则显示指定的TLSet 设置。

                //说明
                //指定针对Tool 0 坐标系（夹具末端坐标系）的相对原点位置和相对旋转角度,定义工具坐标系
                //Tool 1、Tool 2 、Tool 3。
                //        TLSet(1, XY(50, 100, -20, 30)) ----  【X,Y,Z,U】
                //TLSet(2, P10 + X(20))
                //上述情况时,引用坐标值P10 并在X 值上加上20。无视机械臂属性和本地坐标系编号。
                //ツール座標系の原点のZ(軸方向位置)
                //ツール座標系の原点のY(軸方向位置(次図b))
                //ツール座標系の原点のX(軸方向位置(次図a))
                //ツール座標系番号
                //TLSET(1, XY(100, 60, -20, 30))

                if (NumberOfTool < 1 | NumberOfTool > 15)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SetTool";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SetTool";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SetTool";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                string strToolSetting = "";
                strToolSetting = "XY(" + X.ToString() + "," + Y.ToString() + "," +
                    Z.ToString() + "," + U.ToString() + ")";

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"TLSET(" + NumberOfTool
                        + "," + strToolSetting + ")\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SetTool";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SetTool";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SetTool";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool Abort()
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Abort     中止命令的执行       Auto开
                //-------------------------------------

                //此处是否不需要考虑其它命令正在执行，待验证
                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "Abort";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "Abort";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$Abort");
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "Abort";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "Abort";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "Abort";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SetPowerMode(RobotPower PowerMode)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SetPowerMode";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SetPowerMode";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //Power
                //用于将功率模式设为High 或Low，并显示当前的模式。
                //格式
                //(1) Power { High | Low }
                //(2) Power
                //参数
                //High | Low 设置High 或Low。默认设置为Low。
                //结果
                //如果省略参数，则显示当前的功率模式。
                //说明
                //用于将功率模式设为High 或Low。另外，显示当前的功率模式。
                //Low ： 如果将功率模式设为Low，低功率模式则会变为ON 状态。这表示机械手缓慢地（250mm/sec
                //以下的速度）进行动作。另外，将电动机功率输出限制在较低水平。
                //High ： 如果将功率模式设为High，低功率模式则会变为OFF状态。这表示机械手以由Speed、Accel、
                //SpeedS、AccelS 指定的速度、加减速度进行动作。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    if (PowerMode == RobotPower.HIGHPOWER)
                    {
                        FeedBackMessageFromRobot = RobotSendCmd("$Execute, \"Power High\"");
                    }
                    else if (PowerMode == RobotPower.LOWPOWER)
                    {
                        FeedBackMessageFromRobot = RobotSendCmd("$Execute, \"Power Low\"");
                    }
                    else
                    {
                        RobotExecuteBusy = false;
                        return false;
                    }
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SetPowerMode";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SetPowerMode";
                    if (ErrorDisPlay != null)
                        ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SetPowerMode";
                if (ErrorDisPlay != null)
                    ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SLock(int TargetAxis)
        {
            try
            {
                if (TargetAxis < 0 | TargetAxis > 6)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SLock";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SLock";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SLock";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //SLock
                //用于解除指定关节的SFree，并重新开始电动机励磁。
                //格式
                //SLock 关节编号 [关节编号,...]
                //参数
                //关节编号 以表达式或数值指定关节编号（1～9 的整数）。
                //附加轴的S 轴为8，T 轴为9。
                //说明
                //SLock 用于重新开始对因SFree 命令而进入SFree 状态的关节的电动机进行励磁，以便进行直接示教
                //或安装工件等。
                //如果省略关节编号，则重新开始对所有关节的电动机进行励磁。
                //如果对第3 关节重新进行励磁，电磁制动器则会被解除。
                //可替代SLock，使用Motor On 进行所有关节的励磁。
                //如果在Motor Off 状态下执行SLock，则会发生错误。
                //如果执行SLock 命令，则会对机械手控制参数进行初始化。

                //SLock 1, 2 '对J1 和J2 进行励磁

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"SLock " +
                        TargetAxis + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SLock";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SLock";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SLock";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SLockAll(int AxisQty)
        {
            try
            {
                if (AxisQty != 4 & AxisQty != 6)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SLockAll";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                bool TempResult = true;
                for (ushort a = 1; a <= AxisQty; a++)
                {
                    TempResult &= SLock(a);
                }

                if (TempResult == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SLockAll";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SFree(int TargetAxis)
        {
            try
            {
                if (TargetAxis < 0 | TargetAxis > 6)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SFree";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SFree";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SFree";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //SFree
                //用于切断指定关节的电动机电源。
                //格式
                //SFree 关节编号[, 关节编号,...]
                //参数
                //关节编号 以表达式或数值指定关节编号（1～9 的整数）。
                //附加轴的S 轴为8，T 轴为9。
                //说明
                //SFree 用于切断指定关节的电动机电源。此时的状态称为SFree。该命令用于进行直接示教，或仅切
                //断特定关节的励磁进行嵌入等。要再次对该关节进行励磁时，执行SLock 命令或Motor On。
                //如果执行SFree 命令，则会对机械手控制参数进行初始化。
                //详情请参阅Motor On。
                //注意
                //执行SFree 时，部分系统设置会被初始化
                //SFree 用于对有关机械手动作速度或加减速度的参数（Speed、SpeedS、Accel、AccelS）和LimZ 参
                //数进行初始化，以确保安全。

                //SFree 1, 2 '将J1 和J2 设为非励磁状态，然后移动Z 和U 关节以安装部件

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"SFree " + TargetAxis + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SFree";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SFree";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SFree";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SFreeAll(int AxisQty)
        {
            try
            {
                if (AxisQty != 4 & AxisQty != 6)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SFreeAll";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                bool TempResult = true;
                for (ushort a = 1; a <= AxisQty; a++)
                {
                    TempResult &= SFree(a);
                }

                if (TempResult == true)
                {
                    //ExecutingBusy = false;
                    return true;
                }
                else
                {
                    //ExecutingBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SFreeAll";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool ResetRobot()
        {
            try
            {
                ShowLog("机器人开始重置");
                bool? bRtn = ExecuteCMD(ERemotCMD.Execute, Spel.Reset);
                if (bRtn != null)
                    return (bool)bRtn;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override bool SetSpeed(int TargetSpeed)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                if (TargetSpeed > 100 | TargetSpeed < 1)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SetSpeed";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                // FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Speed " + TargetSpeed + "\"");
                bool? brtn = ExecuteCMD(ERemotCMD.Execute, new string[2] { Spel.Speed, TargetSpeed.ToString() });
                if (brtn == null)
                    return false;
                else
                    return (bool)brtn;
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SetSpeed";
                // ErrorDisPlay(this, ea);
                return false;
            }
        }

        /// <summary>
        /// 查询EPSON机械手返回的状态位的所有位代表的含义
        /// </summary>
        /// <param name="StatusCode">EPSON机械手返回的状态位: 11位长度0和1组合的状态位</param>
        /// <returns>返回所有位状态位的数据结构</returns>
        public override RobotStatusBits NewProcessStatusCode(string StatusCode)
        {
            RobotStatusBits TempStatus = new RobotStatusBits();
            TempStatus.Auto = false;
            TempStatus.Error = false;
            TempStatus.EStop = false;
            TempStatus.Paused = false;
            TempStatus.Ready = false;
            TempStatus.Running = false;
            TempStatus.Safeguard = false;
            TempStatus.SError = false;
            TempStatus.Teach = false;
            TempStatus.Test = false;
            TempStatus.Warning = false;

            try
            {
                //*3 错误/警告代码
                //以 4 位数字表示。如果没有错误和警告,则为 0000。

                //例如）1： #GetStatus,0100000001,0000
                //Auto 位和 Ready 位为开（1）。
                //表示自动模式开启并处于准备就绪状态。已启用命令执行。
                //例如）2： #GetStatus,0110000010,0517
                //这意味着运行过程中发生警告。对警告代码采取适当的行动。（在这种情况下,警告代码为 0517）

                //标志(内容)
                //----------------------------------------------------------------------------------------
                //Test(在TEST模式下打开)
                //----------------------------------------------------------------------------------------
                //Teach(在TEACH模式下打开)
                //----------------------------------------------------------------------------------------
                //Auto(在远程输入接受条件下打开)
                //----------------------------------------------------------------------------------------
                //Warnig(在警告条件下打开)
                //                甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
                //----------------------------------------------------------------------------------------
                //SError(在严重错误状态下打开)
                //                发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
                //----------------------------------------------------------------------------------------
                //Safeguard(安全门打开时打开)
                //----------------------------------------------------------------------------------------
                //EStop(在紧急状态下打开)
                //----------------------------------------------------------------------------------------
                //Error 在错误状态下打开
                //                使用“Reset 输入”从错误状态中恢复。
                //----------------------------------------------------------------------------------------
                //Paused(打开暂停的任务)
                //----------------------------------------------------------------------------------------
                //Running(执行任务时打开)
                //                在“Paused 输出”为开时关闭。
                //----------------------------------------------------------------------------------------
                //Ready(控制器完成启动且无任务执行时打开)
                //----------------------------------------------------------------------------------------

                if (StatusCode.Length != 11)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "NewProcessStatusCode";
                    ErrorDisPlay(this, ea);
                    return TempStatus;
                }

                //bool TempCheck=false;
                for (int a = 0; a < StatusCode.Length; a++)
                {
                    if (StatusCode[a] != '0' & StatusCode[a] != '1')
                    {
                        EpsonErrLog.ErrorMess = "Status error\r\n";
                        EpsonErrLog.ErrorCmd = "NewProcessStatusCode";
                        ErrorDisPlay(this, ea);
                        return TempStatus;
                    }
                }

                //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关

                //string TempStr="";
                //0 - Test            在TEST模式下打开
                if (StatusCode[0] == '0')
                {
                    TempStatus.Test = false;
                }
                else if (StatusCode[0] == '1')
                {
                    TempStatus.Test = true;
                }

                //1 - Teach           在TEACH模式下打开
                if (StatusCode[1] == '0')
                {
                    TempStatus.Teach = false;
                }
                else if (StatusCode[1] == '1')
                {
                    TempStatus.Teach = true;
                }

                //2 - Auto            在远程输入接受条件下打开
                if (StatusCode[2] == '0')
                {
                    TempStatus.Auto = false;
                }
                else if (StatusCode[2] == '1')
                {
                    TempStatus.Auto = true;
                }

                //3 - Warnig 在警告条件下打开,甚至在警告条件下也可以像往常一样执行任务。
                //但是,应尽快采取警告行动。
                if (StatusCode[3] == '0')
                {
                    TempStatus.Warning = false;
                }
                else if (StatusCode[3] == '1')
                {
                    TempStatus.Warning = true;
                }

                //4 - SError   在严重错误状态下打开,发生严重错误时,重新启动控制器,
                //以便从错误状态中恢复。“Reset 输入”不可用。
                if (StatusCode[4] == '0')
                {
                    TempStatus.SError = false;
                }
                else if (StatusCode[4] == '1')
                {
                    TempStatus.SError = true;
                }

                //5 - Safeguard       安全门打开时打开
                if (StatusCode[5] == '0')
                {
                    TempStatus.Safeguard = false;
                }
                else if (StatusCode[5] == '1')
                {
                    TempStatus.Safeguard = true;
                }

                //6 - EStop           在紧急状态下打开
                if (StatusCode[6] == '0')
                {
                    TempStatus.EStop = false;
                }
                else if (StatusCode[6] == '1')
                {
                    TempStatus.EStop = true;
                }

                //7 - Error           在错误状态下打开,使用“Reset 输入”从错误状态中恢复。
                if (StatusCode[7] == '0')
                {
                    TempStatus.Error = false;
                }
                else if (StatusCode[7] == '1')
                {
                    TempStatus.Error = true;
                }

                //8 - Paused          打开暂停的任务
                if (StatusCode[8] == '0')
                {
                    TempStatus.Paused = false;
                }
                else if (StatusCode[8] == '1')
                {
                    TempStatus.Paused = true;
                }

                //9 - Running         执行任务时打开,在“Paused 输出”为开时关闭。
                if (StatusCode[9] == '0')
                {
                    TempStatus.Running = false;
                }
                else if (StatusCode[9] == '1')
                {
                    TempStatus.Running = true;
                }

                //10 - Ready           控制器完成启动且无任务执行时打开
                if (StatusCode[10] == '0')
                {
                    TempStatus.Ready = false;
                }
                else if (StatusCode[10] == '1')
                {
                    TempStatus.Ready = true;
                }

                return TempStatus;
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "NewProcessStatusCode";
                ErrorDisPlay(this, ea);
                return TempStatus;
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        //public void Dispose()
        //{
        //    try
        //    {
        //        if (RobotCommucate != null)
        //        {
        //            RobotCommucate.Close();
        //            RobotCommucate = null;
        //        }
        //        GC.Collect();

        //    }
        //    catch (Exception ex)
        //    {
        //        EpsonErrLog.ErrorMess = ex.Message; ;
        //        EpsonErrLog.ErrorCmd = "Dispose";
        //        ErrorDisPlay(this, ea);
        //    }
        //}

        public override bool SetCurrentRobot(int NumberOfRobot)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //SetCurRobot         机械手编号             选择机械手                                        Auto开/Ready开
                //-----------------------------------------------------

                if (NumberOfRobot < 0 | NumberOfRobot > 16)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$SetCurRobot," + NumberOfRobot);
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SetCurrentRobot";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        public override int GetCurrentRobot()
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //GetCurRobot      获取当前的机械手编号              随时可用
                //--------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    return -1;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "GetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return -1;
                }
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$GetCurRobot");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "GetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return -1;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    //#[远程命令],[0]终端
                    //$GetCurRobot
                    //#GetCurRobot,1

                    string[] TempStr;
                    TempStr = FeedBackMessageFromRobot.Split(',');

                    if (TempStr[0] != "#GetCurRobot")
                    {
                        RobotExecuteBusy = false;
                        return -1;
                    }

                    RobotExecuteBusy = false;
                    return Convert.ToInt32(TempStr[1]);
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetCurrentRobot";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return -1;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GetCurrentRobot";
                ErrorDisPlay(this, ea);
                return -1;
                //MessageBox.Show(ex.Message);
            }
        }

        public override bool SetACCELSpeed(ushort TargetAccelSpeed, ushort TargetDecelSpeed)
        {
            try
            {
                if (TargetAccelSpeed < 1 | TargetAccelSpeed > 100)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //Accel
                //用于设置和显示利用Go、Jump、Pulse 等的PTP 动作的加减速度。
                //格式
                //(1) Accel 加速设定值, 减速设定值, [转移加速设定值, 转移减速设定值, 接近加速设定值, 接近减
                //速设定值]
                //(2) Accel
                //参数
                //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
                //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）
                //转移加速设定值 以大于1 的整数指定Jump 时的转移加速度。
                //可省略。仅Jump 命令时可设置。
                //转移减速设定值 以大于1 的整数指定Jump 时的转移减速度。
                //可省略。仅Jump 命令时可设置。
                //接近加速设定值 以大于1 的整数指定Jump 时的接近加速度。
                //可省略。仅Jump 命令时可设置。
                //接近减速设定值 以大于1 的整数指定Jump 时的接近减速度。
                //可省略。仅Jump 命令时可设置。
                //结果
                //如果省略参数，将返回当前的Accel 参数。

                //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
                //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute\"Accel " +
                        TargetAccelSpeed + "," + TargetDecelSpeed);
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SetACCELSpeed";
                ErrorDisPlay(this, ea);
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前速度
        /// </summary>
        /// <returns></returns>
        public override int GetSpeed()
        {
            try
            {
                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetSpeed";
                    ErrorDisPlay(this, ea);
                    return -1;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "GetSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return -1;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //Speed
                //用于设置/显示利用Go、Jump、Pulse 命令等的PTP 动作速度。
                //格式
                //(2) Speed
                //如果省略参数，则显示当前的Speed 设定值。
                //说明
                //Speed 用于指定所有PTP 动作命令的速度。其中包括有关Go、Jump、Pulse 等动作命令的速度设置。
                //速度设置是指以1～100 的整数指定相对于最大速度的比例（%）。如果指定“100”，则以最大速度进
                //行动作。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Speed" + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "GetSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return -1;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    //$Execute,"Speed"
                    //#Execute,"Low Power Mode
                    //1
                    //1	   1

                    string[] TempStr = Strings.Split(FeedBackMessageFromRobot, "\r\n");

                    if (TempStr.Length != 3)//(TempStr[0]!="#Execute")
                    {
                        RobotExecuteBusy = false;
                        return -1;
                    }

                    RobotExecuteBusy = false;
                    return Convert.ToUInt16(TempStr[1]);
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return -1;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GetSpeed";
                ErrorDisPlay(this, ea);
                return -1;
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取加速度
        /// </summary>
        /// <returns></returns>
        public override RobotAccel GetAccelSpeed()
        {
            RobotAccel TempAccel;
            TempAccel.AccelSpeed = -1;
            TempAccel.DecelSpeed = -1;

            try
            {
                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetAccelSpeed";
                    ErrorDisPlay(this, ea);
                    return TempAccel;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "GetAccelSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return TempAccel;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //Accel
                //用于设置和显示利用Go、Jump、Pulse 等的PTP 动作的加减速度。
                //格式
                //(2) Accel
                //如果省略参数，将返回当前的Accel 参数

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Accel" + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "GetAccelSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return TempAccel;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
                    //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）
                    //$Execute,"Accel"
                    //#Execute,"Low Power Mode
                    //10	  10
                    //10	  10
                    //10	  10

                    //添加返回结果值的代码，需要测试返回结果才能处理【需要验证】
                    string[] TempStr;//,TempStr2;
                    TempStr = Strings.Split(FeedBackMessageFromRobot, "\r\n");
                    //TempStr2=TempStr[1].Split(',');
                    //if(TempStr2[0]!="#Execute")
                    //    {
                    //    ExecutingBusy = false;
                    //    return TempAccel;
                    //    }

                    if (TempStr.Length < 4)
                    {
                        RobotExecuteBusy = false;
                        return TempAccel;
                    }

                    TempStr = TempStr[1].Split(' ');

                    TempAccel.AccelSpeed = Convert.ToInt32(TempStr[0]);
                    TempAccel.DecelSpeed = Convert.ToInt32(TempStr[1]);

                    RobotExecuteBusy = false;
                    return TempAccel;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "GetAccelSpeed";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return TempAccel;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "GetAccelSpeed";
                ErrorDisPlay(this, ea);
                return TempAccel;
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前功率模式
        /// </summary>
        /// <returns></returns>
        public override RobotPower GetPowerStatus()
        {
            RobotPower TempPower = RobotPower.UnKonw;

            try
            {
                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "GetPowerStatus";
                    ErrorDisPlay(this, ea);
                    return TempPower;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                //    EpsonErrLog.ErrorCmd = "GetPowerStatus";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return TempPower;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //Power
                //用于将功率模式设为High 或Low，并显示当前的模式。
                //格式
                //(2) Power
                //参数
                //High | Low 设置High 或Low。默认设置为Low。
                //结果
                //如果省略参数，则显示当前的功率模式。

                //if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Speed" + "\"");
                //FeedBackMessageFromRobot = SendCommand("$Execute,\"Power" + "\"" + Suffix);
                //}
                //else
                //{
                //    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                //    EpsonErrLog.ErrorCmd = "GetPowerStatus";
                //    ErrorDisPlay(this, ea);
                //    RobotExecuteBusy = false;
                //    return TempPower;
                //}

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    //$Execute,"Power"
                    //!Execute,99

                    //$Execute,"Speed"
                    //#Execute,"Low Power Mode
                    //1
                    //1	   1

                    //**************
                    //注意：用Split(\r\n)之后，其它的前面都有\r\n,所有要去掉这个才能得到正确值【需要验证】
                    //**************

                    string[] TempStr;
                    string TempRet;
                    TempStr = Strings.Split(FeedBackMessageFromRobot, "\r\n");
                    TempRet = TempStr[0].ToUpper();

                    if (Strings.InStr(TempRet, "LOW") != -1)
                    {
                        TempPower = RobotPower.LOWPOWER;
                    }
                    else if (Strings.InStr(TempRet, "HIGH") != -1)
                    {
                        TempPower = RobotPower.HIGHPOWER;
                    }
                    else
                    {
                        TempPower = RobotPower.UnKonw;
                    }

                    RobotExecuteBusy = false;
                    return TempPower;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "AbsMove";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return TempPower;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "AbsMove";
                ErrorDisPlay(this, ea);
                return TempPower;
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置原点坐标
        /// </summary>
        /// <param name="HomePoint"></param>
        /// <returns></returns>
        public override bool SetHome(RobotPoint HomePoint)
        {
            try
            {
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "SetHome";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "SetHome";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1'
                    & FeedBackMessageFromRobot[i + 11] == '1')
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"homeset " + HomePoint.X.ToString() + ","
                        + HomePoint.Y.ToString() + "," + HomePoint.Z.ToString() + "," + HomePoint.U.ToString() + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "SetHome";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "SetHome";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "SetHome";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool Loacl(int LocalNo, RobotPoint LocalPoint)
        {
            try
            {
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "Loacl";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "Loacl";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1'
                    & FeedBackMessageFromRobot[i + 11] == '1')
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Local " + LocalNo + "," + "XY(" + LocalPoint.X.ToString() + ","
                        + LocalPoint.Y.ToString() + "," + LocalPoint.Z.ToString() + "," + LocalPoint.U.ToString() + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "Loacl";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "Loacl";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "Loacl";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        public override bool Base(RobotPoint BasePoint)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
                //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "Base";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "Base";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1'
                    & FeedBackMessageFromRobot[i + 11] == '1')
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Base XY(" + BasePoint.X.ToString() + ","
                        + BasePoint.Y.ToString() + "," + BasePoint.Z.ToString() + "," + BasePoint.U.ToString() + ")" + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "Base";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "Base";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "Base";
                ErrorDisPlay(this, ea);
                return false;
            }
        }

        /// <summary>
        /// 松开刹车功能
        /// </summary>
        /// <param name="TargetAxis"></param>
        /// <returns></returns>
        public override bool StopAxisOff(int TargetAxis)
        {
            try
            {
                if (TargetAxis < 1 | TargetAxis > 6)
                {
                    EpsonErrLog.ErrorMess = "Param is error\r\n";
                    EpsonErrLog.ErrorCmd = "StopAxisOff";
                    ErrorDisPlay(this, ea);
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (this.RobotExecuteBusy == true)
                {
                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
                    EpsonErrLog.ErrorCmd = "StopAxisOff";
                    ErrorDisPlay(this, ea);
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                {
                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
                    EpsonErrLog.ErrorCmd = "StopAxisOff";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //Brake
                //用于打开和关闭当前机械手指定关节的制动器。
                //格式
                //Brake 状态, 关节编号
                //参数
                //状态 施加制动时：使用On。
                //解除制动时：使用Off。
                //关节编号 指定1～6 的关节编号。
                //说明
                //Brake 命令用于对垂直6 轴型机械手的一个关节施加或解除制动。这是仅可通过命令使用的命令。此
                //命令设计为只有维修作业人员才可以使用。
                //如果执行Brake 命令，则会对机械手控制参数进行初始化。

                //brake on, 1
                //brake off, 1

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                {
                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Brake off, " +
                        TargetAxis + "\"");
                }
                else
                {
                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
                    EpsonErrLog.ErrorCmd = "StopAxisOff";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
                    EpsonErrLog.ErrorCmd = "StopAxisOff";
                    ErrorDisPlay(this, ea);
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "StopAxisOff";
                ErrorDisPlay(this, ea);
                RobotExecuteBusy = false;
                return false;
            }
        }

        //获得指定输入位的当前状态
        /// <summary>
        /// 获得指定输入位的当前状态
        /// </summary>
        /// <param name="TargetInputBit">读取的目标位【0~23】</param>
        /// <param name="IsOn">返回值【true：ON,false：OFF】</param>
        /// <returns></returns>
        public override bool GetInputBit(int TargetInputBit, ref bool IsOn)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //GetIO       I/O位号    获得指定的I/O位       随时可用
                //-----------------------------------------------------

                if (TargetInputBit < 0 | TargetInputBit > 23)
                {
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (RobotExecuteBusy == true)
                {
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$GetIO," + TargetInputBit + "\"");
                //}
                //else
                //{
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    //在此添加判断返回的位的值为0或者1()
                    //GetIO    #GetIO,[0 | 1]终端

                    string[] TempStr = FeedBackMessageFromRobot.Split(',');
                    if (TempStr[0] != "#GetIO")
                    {
                        RobotExecuteBusy = false;
                        IsOn = false;
                        return false;
                    }

                    if (TempStr[1][0] == '1')
                    {
                        IsOn = true;
                    }
                    else
                    {
                        IsOn = false;
                    }

                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                return false;
                //MessageBox.Show(ex.Message);
            }
        }

        //设置指定输出位的状态【true：打开此位,false：关闭此位】
        /// <summary>
        /// 设置指定输出位的状态【true：打开此位,false：关闭此位】
        /// </summary>
        /// <param name="TargetOutputBit">操作的目标位【0~15】</param>
        /// <param name="TurnOn">true：打开此位,false：关闭此位</param>
        /// <returns></returns>
        public override bool SetOutputBit(int TargetOutputBit, bool TurnOn)
        {
            try
            {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //SetIO     I/O位号和值      设置I/O指定的位
                //1:        打开此位
                //0:        关闭此位(Ready开)
                //---------------------------------------------------------

                if (TargetOutputBit < 0 | TargetOutputBit > 15)
                {
                    MessageBox.Show("The parameter 'TargetOutputBit' should be 0~15, please revise it and retry.");
                    return false;
                }

                //判断是否正在执行命令，不然会丢失指令
                if (RobotExecuteBusy == true)
                {
                    return false;
                }
                else
                {
                    RobotExecuteBusy = true;
                }

                //if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                //{
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                //int i;
                //i = FeedBackMessageFromRobot.IndexOf(",");

                //int TempOut = 0;
                //TempOut = (TurnOn == true) ? 1 : 0;

                //if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1'
                //{
                FeedBackMessageFromRobot = RobotSendCmd("$GetIO," + TargetOutputBit + "\"");
                //}
                //else
                //{
                //    RobotExecuteBusy = false;
                //    return false;
                //}

                if (FeedBackMessageFromRobot.IndexOf("!") == -1 && FeedBackMessageFromRobot != "") //& FeedBackMessageFromRobot.Length > 40
                {
                    RobotExecuteBusy = false;
                    return true;
                }
                else
                {
                    RobotExecuteBusy = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RobotExecuteBusy = false;
                return false;
                //MessageBox.Show(ex.Message);
            }
        }
    }
}