#region "命令说明"
//*******************************************************************
//String .IndexOf 方法 (Char)    报告指定 Unicode 字符在此字符串中的第一个匹配项的索引。

//命名空间：   System
//程序集：   mscorlib（在 mscorlib.dll 中） 

//Public Function IndexOf( ByVal value As Char ) As Integer

//参数
//value    类型:    System.Char    要查找的 Unicode 字符。

//返回值  类型:     System.Int32   如果找到该字符,则为 value 的从零开始的索引位置；如果未找到,则为 -1。 
//*******************************************************************
#endregion

#region "EPSON远程以太网控制命令响应说明"

//如果控制器接收到正确的命令,将在执行命令时显示下列格式的响应。
//---------------------------------------------------------------------------------------------------------------
//命令                                            格式
//---------------------------------------------------------------------------------------------------------------
//获取数值的远程命令GetIO、
//GetVariable和GetStatus 除外                     #[远程命令],[0]终端
//---------------------------------------------------------------------------------------------------------------
//GetIO                                           #GetIO,[0 | 1]终端
//---------------------------------------------------------------------------------------------------------------
//GetMemIO                                        #GetMemIO,[0 | 1]终端 *1
//---------------------------------------------------------------------------------------------------------------
//GetIOByte                                       #GetIOByte,[字节（8 位）的十六进制字符串（00到 FF）]终端
//---------------------------------------------------------------------------------------------------------------
//GetMemIOByte                                    #GetMemIOByte,[字节（8 位）的十六进制字符串（00 到 FF）]终端
//---------------------------------------------------------------------------------------------------------------
//GetIOWord                                       #GetIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端
//---------------------------------------------------------------------------------------------------------------
//GetIOMemWord                                    #GetMemIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端
//---------------------------------------------------------------------------------------------------------------
//GetVariable                                     #GetVariable,[参数值] 终端
//---------------------------------------------------------------------------------------------------------------
//GetVariable（如果是数组）                       #GetVariable,[参数值 1],[参数值 2],...,终端 *4
//---------------------------------------------------------------------------------------------------------------
//GetStatus                                       #GetStatus,[状态],[错误,警告代码]终端
//                                                例如） #GetStatus,aaaaaaaaaa,bbbb
//                                                                       *2     *3
//---------------------------------------------------------------------------------------------------------------
//Execute                                        如果作为命令执行的结果返回数值  #Execute,“[执行结果]” 终端
//---------------------------------------------------------------------------------------------------------------

//*1 [0 | 1] I/O 位 开：1/关：0
//----------------------------------------------------------------------------------------
//*2 状态
//在上例中,10 位数字“aaaaaaaaaa”用于以下 10 个标志。
//Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关
//如果 Teach 和 Auto 为开,则为 1100000000。
//----------------------------------------------------------------------------------------
//*3 错误/警告代码
//以 4 位数字表示。如果没有错误和警告,则为 0000。

//例如）1： #GetStatus,0100000001,0000
//Auto 位和 Ready 位为开（1）。
//表示自动模式开启并处于准备就绪状态。已启用命令执行。

//例如）2： #GetStatus,0110000010,0517
//这意味着运行过程中发生警告。对警告代码采取适当的行动。（在这种情况下,警告代码为 0517）

//标志                 内容
//----------------------------------------------------------------------------------------
//Test            在TEST模式下打开
//----------------------------------------------------------------------------------------
//Teach           在TEACH模式下打开
//----------------------------------------------------------------------------------------
//Auto            在远程输入接受条件下打开
//----------------------------------------------------------------------------------------
//Warnig          在警告条件下打开
//                甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
//----------------------------------------------------------------------------------------
//SError          在严重错误状态下打开
//                发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
//----------------------------------------------------------------------------------------
//Safeguard       安全门打开时打开
//----------------------------------------------------------------------------------------
//EStop           在紧急状态下打开
//----------------------------------------------------------------------------------------
//Error           在错误状态下打开
//                使用“Reset 输入”从错误状态中恢复。
//----------------------------------------------------------------------------------------
//Paused          打开暂停的任务
//----------------------------------------------------------------------------------------
//Running         执行任务时打开
//                在“Paused 输出”为开时关闭。
//----------------------------------------------------------------------------------------
//Ready           控制器完成启动且无任务执行时打开
//----------------------------------------------------------------------------------------

//*4 返回要获取的编号中指定编号的值。

//*******************************************************************

#endregion

#region "EPSON远程以太网控制说明"
//执行远程以太网控制
//通过以下步骤可设置远程控制。
//(1) 从客户端设备连接到控制器远程以太网中指定的端口上。
//(2) 将远程以太网中设置的密码指定到该参数上并发送Login 命令。
//(3) 执行远程命令前,客户端设备须等到Auto（GetStatus 命令响应）为ON 为止。
//(4) 现在,远程命令将被接受。
//每个命令执行输入接受条件的功能。

//调试远程以太网控制
//从EPSON RC+ 7.0 开发环境中调试程序的能力如下所述。
//(1) 照例创建一个程序。
//(2) 打开“运行”窗口,单击<Ethernet 启用>按钮。
//如果您使用远程以太网控制只是获得了该值,则不显示<Ethernet 启用>按钮。单
//击指定为控制装置的设备的<开始>按钮。
//(3) 现在,远程命令将被接受。
//“运行”窗口中的断点设置和输出是可用的。
//如果5分钟内未从外部设备中Login,该连接将被自动切断。Login后,如果命令未在
//远程以太网的超时时间内发送出去,连接将被切断。在这种情况下,重新建立连
//接。
//如果发生错误,执行操作命令之前请执行复位命令,以清除错误条件。若要通过监
//控从外部设备上清除错误条件,执行“GetStatus”和“Reset”命令。

//如果在(超时)框中设置“0”,则超时时间为无限。在这种情况下,该任务继续执
//行,即使没有来自客户端的通信。这意味着机械手可能会继续移动并给设备造成
//意外损坏。确保使用除通信以外的方式来停止该任务。

//-----------*********************===================
//远程以太网命令格式：$ 远程命令{, parameter....} 终端
//---------------------------------------------------------------------------------------------------
//远程命令            参数                   内容                                              输入接受条件
//Login               密码                    启动控制器远程以太网功能   
//                                           通过密码验证
//                                           正确执行登录,并执行命令,直到退出                随时可用
//---------------------------------------------------------------------------------------------------
//Logout                                     退出控制器远程以太网功能
//                                           退出登录后,执行登录命令来启动远程以太网功能。
//                                           在任务执行期间退出会导致错误发生。                随时可用
//---------------------------------------------------------------------------------------------------
//Start               功能编号               执行指定编号的功能                                Auto开/Ready开/Error关/EStop关/Safeguard开
//---------------------------------------------------------------------------------------------------
//Stop                                       停止所有的任务和命令。                            Auto开
//---------------------------------------------------------------------------------------------------
//Pause                                      暂停所有任务                                      Auto 开/Running 开
//---------------------------------------------------------------------------------------------------
//Continue                                   继续暂停了的任务                                  Auto 开/Paused 开
//---------------------------------------------------------------------------------------------------
//Reset                                      清除紧急停止和错误                                Auto 开/Ready 开
//---------------------------------------------------------------------------------------------------
//SetMotorsOn         机械手编号             打开机械手电机                                    Auto开/Ready开/EStop关/Safeguard关
//---------------------------------------------------------------------------------------------------
//SetMotorsOff        机械手编号             关闭机械手电机                                    Auto开/Ready开
//---------------------------------------------------------------------------------------------------
//SetCurRobot         机械手编号             选择机械手                                        Auto开/Ready开
//---------------------------------------------------------------------------------------------------
//GetCurRobot                                获取当前的机械手编号                              随时可用
//---------------------------------------------------------------------------------------------------
//Home                机械手编号             将机械手手臂移动到由用户定义的起始点位置上        Auto开/Ready开/Error关/EStop关/Safeguard 关
//---------------------------------------------------------------------------------------------------
//GetIO               I/O位号                获得指定的I/O位                                   随时可用
//---------------------------------------------------------------------------------------------------
//SetIO               I/O位号和值            设置I/O指定的位
//                                             1：打开此位
//                                             0：关闭此位                                     Ready开
//---------------------------------------------------------------------------------------------------
//GetIOByte           I/O位号                获得指定的I/O端口（8位）                          随时可用
//---------------------------------------------------------------------------------------------------
//SetIOByte           I/O端口号和值          设置I/O指定的端口（8位）                          Ready开
//---------------------------------------------------------------------------------------------------
//GetIOWord           I/O字端口号            获得指定的I/O字端口（16位）                       随时可用
//---------------------------------------------------------------------------------------------------
//SetIOWord           I/O字端口号和值        设置I/O指定的端口（16位）                         Auto开/Ready开
//---------------------------------------------------------------------------------------------------
//GetMemIO            内存I/O位号            获得指定的内存I/O位                               随时可用
//---------------------------------------------------------------------------------------------------
//SetMemIO            内存I/O位号和值        设置指定的内存I/O位
//                                               1: 打开此位
//                                               0: 关闭此位                                   Auto开/Ready开
//---------------------------------------------------------------------------------------------------
//GetMemIOByte        内存I/O端口号          获得指定的内存I/O端口（8位）                      随时可用
//---------------------------------------------------------------------------------------------------
//SetMemIOByte        内存I/O端口号和值      设置指定的内存I/O端口（8位）                      Auto开/Ready开
//---------------------------------------------------------------------------------------------------
//GetMemIOWord        内存I/O字端口号        获得指定的内存I/O字端口（16位）                   随时可用
//---------------------------------------------------------------------------------------------------
//SetMemIOWord        内存I/O字端口号和值    设置指定的内存I/O字端口（16位）                   Auto开/Ready开
//---------------------------------------------------------------------------------------------------
//GetVariable         参数名称{, type}       获取备份（Global Preserve）参数的值               随时可用
//                    ***********************************************************
//                    (参数名称)（数组元
//                    素）,(参数名称类
//                    型),(获取的编号)      获取备份（Global Preserve）数组参数的值
//---------------------------------------------------------------------------------------------------
//SetVariable        参数名称和值{,类型}    设置备份（Global Preserve）参数中的值             Auto开/Ready开
//---------------------------------------------------------------------------------------------------
//GetStatus                                  获取控制器的状态                                  随时可用
//---------------------------------------------------------------------------------------------------
//Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//---------------------------------------------------------------------------------------------------
//Abort                                      中止命令的执行                                    Auto开
//---------------------------------------------------------------------------------------------------

//(*7) 如果“0”被指定为机械手编号,所有的机械手将进行操作。
//如果您想操作具体的机械手,指定目标机械手的机械手编号（1到16）。
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
//(*10) 在双引号中指定命令和参数。
//待执行的命令字符串和执行结果字符串被限制在4060字节。
//机械手动作命令将被执行到所选的机械手上。执行命令之前检查使用GetCurRobot选中的机械手。

//运行Execute时可用的命令
//远程命令
//Abort
//GetStatus
//SetIO
//SetIOByte
//SetIOWord
//SetMemIO
//SetMemIOByte
//SetMemIOWord
//Execute执行命令和输出命令
//如果在（SetIO, SetIOByte, SetIOWord, SetMemIO, SetMemIOByte, SetMemIOWrod）
//中指定的命令是相同的并且同时执行,后来执行的命令将导致错误。确保在执行
//Execute命令和输出命令后使用正在执行Execute命令的GetStatus来检查执行结果。
//(*11) 若要执行PCDaemon功能的命令,务必要在连接了RC+ 7.0时执行。如果未连
//接RC+ 7.0,执行命令将会导致错误。

//************************************************

//$Execute,"print realpos"
//#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
//$Execute,"move here +x(-1)"
//#Execute,0
//$Execute,"print realpos"
//#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
//$Execute,"move here -x(1)"
//#Execute,0
//$Execute,"print realpos"
//#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0

//************************************************
//【move here -x(1)和move here +x(1)的作用是一样的】
//************************************************ 
#endregion

#region "已经处理好的事项"
//1、需要对进行点位运动的命令参数进行验证,验证正确后再执行,否则提示错误；【OK】
//2、实例化时需要将相关参数进行验证；【OK】

//6、添加错误处理代码；【OK】
//7、添加GetStatus返回值得判断处理代码；【OK】
//8、添加所有的返回值处理代码；【OK】
//9、想办法将信息集中更新到新添加的RichTextBox中；【OK】

//12、用FeedBackMessageFromRobot来保存从EPSON返回的任何结果,可在外部进行读写,公共变量；【OK】
//13、execute,的后面不能有空格,否则就执行错误；【OK】 
#endregion

#region "待处理事项"
//3、对点位运动的命令添加可选参数,例如Z轴限位高度；【】
//4、函数GetToolSetting不对,需要仔细检查或者如果EPSON SPEL语音中没有就去掉此函数；【已经处理了一部分,需要检查返回的值然后再修改程序】
//5、添加导出点数据到Excel文件的代码；【】
//10、在EPSON控制器中位、字节和字是从0开始,但是在给用户使用此DLL时,为避免误解,全部改为1开始,然后在相应函数里面全部-1；【】
//11、需要矫正内存操作的参数值范围；【】
//14、需要检查GET命令返回后的值处理是否正确，因为返回的命令里面最后有结束符；【OK】
//15、添加LoadPoints;【OK】
//16、添加十进制转换为2进制的代码；【】
//17、添加设置机械手模式【自动/程序模式】的代码；【】
//18、旋转U轴需要在 U 前面加上 “:” 号，否则报错，与X、Y、Z轴不同；【OK】
//    【现在又报错，改方法：先获取当前位置，然后再绝对定位+U的角度】【OK】
//19、因为在调试过程中出现EPSON断开远程服务器，所以改为Private NewClientForConnectRemoteEpson 
//    As SimpleClientStation来与EPSON服务器建立通讯；待测试；【】
//20、需要监控对比远程IO的实际IO信号的变化，与远程以太网相比较，哪些IO是无法手动控制的，是自动输出的；【】
//21、GetOutByteStatus和GetOutWordStatus执行指令时报错；暂时设置为private【】
//22、对各个点位运动的距离进行最大/最小值检查，避免超范围运动；【】 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
#region "以太网"
using Communicate;
#endregion
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using BaseDll;
using System.Diagnostics;
using UserCtrl;

namespace MotionIoLib
{
    public enum RobotCommunicateType
    {
        Eth,
        Com,
    }
  
    public abstract class RobotBase:LogView
    {
        protected int m_nAxisNoMin, m_nAxisNoMax;
        protected string m_strPwd;
        protected string m_strEth;
  

        public  ResponseStatus rs = new ResponseStatus();
        protected BitOperat bitOperat = new BitOperat();
        protected object lockER = new object();
        protected Stopwatch swOutTime = new Stopwatch();
        protected Stopwatch swExeEndTime = new Stopwatch();


        /// <summary>
        /// 设备状态
        /// </summary>
        public Status Status { get { return status; } }
        private Status status = new Status();
        protected Status statusOld = new Status();
        protected Dictionary<string, string> m_dicPointIdLabel = new Dictionary<string, string>();
        protected Dictionary<string, string> m_dicLabelPointId = new Dictionary<string, string>();
        //当前执行指令参数
        protected string CEParam = "";

        public RobotBase(string strName, string strPwd,string strEth,int nAxisNoMin,int nAxisNoMax, RobotCommunicateType robotCommunicateType= RobotCommunicateType.Eth)
        {
            m_nAxisNoMin = nAxisNoMin;
            m_nAxisNoMax = nAxisNoMax;
            m_strPwd = strPwd;
            robotInfo.PassWard = strPwd;
            RobotInfo1 = robotInfo;

            RobotCommucate = TcpMgr.GetInstance().GetTcpLink(strEth);
            if(RobotCommucate==null)
            {
                MessageBox.Show("机器人通讯网口设置失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public delegate void UpdateRobotStateChangedHandle(ControlInfo controlInfo);
        public UpdateRobotStateChangedHandle m_GetRobotStateChangedHandle;

        public delegate void UpdateRobotPosChangedHandle(PositionInfo controlInfo);
        public UpdateRobotPosChangedHandle m_UpdateRobotStateChangedHandle;

        public int GetRobotAxisMax() { return m_nAxisNoMax; }
        public int GetRobotAxisMin() { return m_nAxisNoMin; }
        /// <summary>
        /// 根据指定指令 读取应答状态
        /// </summary>
        /// <param name="cmd">指令</param>
        /// <returns>状态：true=执行OK，false=执行false，null=超时</returns>
        protected bool? RResponseInfo(ERemotCMD cmd)
        {
            try
            {
                switch (cmd)
                {
                    case ERemotCMD.Abort: return rs.Abort;
                    case ERemotCMD.Continue: return rs.Continue;
                    case ERemotCMD.Execute: return rs.Execute;
                    case ERemotCMD.GetCurRobot: return rs.GetCurRobot;
                    case ERemotCMD.GetIO: return rs.GetIO;
                    case ERemotCMD.GetIOByte: return rs.GetIOByte;
                    case ERemotCMD.GetIOWord: return rs.GetIOWord;
                    case ERemotCMD.GetMemIOByte: return rs.GetMemIOByte;
                    case ERemotCMD.GetMemIOWord: return rs.GetMemIOWord;
                    case ERemotCMD.GetStatus: return rs.GetStatus;
                    case ERemotCMD.GetVariable: return rs.GetVariable;
                    case ERemotCMD.Home: return rs.Home;
                    case ERemotCMD.Login: return rs.Login;
                    case ERemotCMD.Logout: return rs.Logout;
                    case ERemotCMD.Pause: return rs.Pause;
                    case ERemotCMD.Reset: return rs.Reset;
                    case ERemotCMD.SetCurRobot: return rs.SetCurRobot;
                    case ERemotCMD.SetIO: return rs.SetIO;
                    case ERemotCMD.SetIOByte: return rs.SetIOByte;
                    case ERemotCMD.SetIOWord: return rs.SetIOWord;
                    case ERemotCMD.SetMemIO: return rs.SetMemIO;
                    case ERemotCMD.SetMemIOByte: return rs.SetMemIOByte;
                    case ERemotCMD.SetMotorsOff: return rs.SetMotorsOff;
                    case ERemotCMD.SetMotorsOn: return rs.SetMotorsOn;
                    case ERemotCMD.SetVariable: return rs.SetVariable;
                    case ERemotCMD.Start: return rs.Start;
                    case ERemotCMD.Stop: return rs.Stop;
                    default: ShowLog("读取机器手应答状态失败：未找到命令->" + cmd.ToString()); return null;
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 根据指定指令 修改应答状态
        /// </summary>
        /// <param name="cmd">指令</param>
        /// <param name="value">状态：true=执行OK，false=执行false，null=超时</param>
        /// <returns>true=修改成功，false=修改失败</returns>
        protected bool WResponseInfo(ERemotCMD cmd, bool? value)
        {
            try
            {
                switch (cmd)
                {
                    case ERemotCMD.Abort: rs.Abort = value; break;
                    case ERemotCMD.Continue: rs.Continue = value; break;
                    case ERemotCMD.Execute: rs.Execute = value; break;
                    case ERemotCMD.GetCurRobot: rs.GetCurRobot = value; break;
                    case ERemotCMD.GetIO: rs.GetIO = value; break;
                    case ERemotCMD.GetIOByte: rs.GetIOByte = value; break;
                    case ERemotCMD.GetIOWord: rs.GetIOWord = value; break;
                    case ERemotCMD.GetMemIOByte: rs.GetMemIOByte = value; break;
                    case ERemotCMD.GetMemIOWord: rs.GetMemIOWord = value; break;
                    case ERemotCMD.GetStatus: rs.GetStatus = value; break;
                    case ERemotCMD.GetVariable: rs.GetVariable = value; break;
                    case ERemotCMD.Home: rs.Home = value; break;
                    case ERemotCMD.Login: rs.Login = value; break;
                    case ERemotCMD.Logout: rs.Logout = value; break;
                    case ERemotCMD.Pause: rs.Pause = value; break;
                    case ERemotCMD.Reset: rs.Reset = value; break;
                    case ERemotCMD.SetCurRobot: rs.SetCurRobot = value; break;
                    case ERemotCMD.SetIO: rs.SetIO = value; break;
                    case ERemotCMD.SetIOByte: rs.SetIOByte = value; break;
                    case ERemotCMD.SetIOWord: rs.SetIOWord = value; break;
                    case ERemotCMD.SetMemIO: rs.SetMemIO = value; break;
                    case ERemotCMD.SetMemIOByte: rs.SetMemIOByte = value; break;
                    case ERemotCMD.SetMotorsOff: rs.SetMotorsOff = value; break;
                    case ERemotCMD.SetMotorsOn: rs.SetMotorsOn = value; break;
                    case ERemotCMD.SetVariable: rs.SetVariable = value; break;
                    case ERemotCMD.Start: rs.Start = value; break;
                    case ERemotCMD.Stop: rs.Stop = value; break;
                 //  default: //.Error("写入机器手应答状态失败：未找到命令->" + cmd.ToString()); return false;
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
                return false;
            }
            return true;
        }


     

        protected RobotInfo robotInfo= new RobotInfo();
        /// <summary>
        /// 机械手的字段
        /// </summary>
        protected bool ServoFlag = false;
        public CommuncateItf CommuncateType { get; set; }

        protected object CommuncateInterface = null;
        public RobotInfo RobotInfo1 { get => robotInfo1; set => robotInfo1 = value; }
        public RobotInfo RobotInfomation { get => robotInfo; set => robotInfo = value; }
        /// <summary>
        /// 错误输出时间
        /// </summary>
        // public event EventHandler ErrorDisPlay;
        public delegate void ErrorDisPlayHandle(RobotBase sa, EventArgs ea);
        public ErrorDisPlayHandle ErrorDisPlay;
        /// <summary>
        /// Epson错误日志
        /// </summary>
        public MotionErrorMess EpsonErrLog;
        /// <summary>
        /// 成功构造标志，密码正确标志
        /// </summary>
        protected bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        protected RobotPoint robotPoint;
        /// <summary>
        /// 从EPSON机械手返回的信息
        /// </summary>
        public string FeedBackMessageFromRobot = "";

        protected bool ConnectRemoteEpsonTCPIPOk;
        protected EventArgs ea = new EventArgs();
        /// <summary>
        /// 机械手是否准备完成
        /// </summary>
        protected bool isOpenFlag = false;
        protected TcpLink RobotCommucate = null;
        #region "机械手执行标志定义"
        private RobotInfo robotInfo1;
        /// <summary>
        /// 机械手是否已经暂停
        /// </summary>
        protected bool RobotPause { get; set; }
        /// <summary>
        /// 当前机械手是否正在执行命令
        /// </summary>
        protected bool RobotExecuteBusy { get; set; }
        protected RobotPoint RobotPointMes { get; set; }
        #endregion


        public abstract bool AbsMove(RobotPoint EpsonPoint);


        public abstract bool AbsMove(string EpsonPoint);

        public abstract bool Close();

        /// <summary>
        /// 获取实际坐标值（不可用）
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <returns></returns>
        public abstract int GetAxisActPos(int nAxisNo);

        public abstract bool GetAxisActPos(ref PositionInfo EpsonPos);
        /// <summary>
        /// 获取当前位置坐标值（不可用）
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <returns></returns>
        public abstract int GetAxisCmdPos(int nAxisNo);

        public abstract int GetAxisPos(int nAxisNo);


        public abstract long GetMotionIoState(int nAxisNo);

        public abstract bool GetServoState(int nAxisNo);

        public abstract bool Home(int nAxisNo, int nParam);

        public abstract int IsAxisNormalStop(int nAxisNo);


        public abstract bool IsHomeNormalStop(int nAxisNo);


        public bool IsOpen()
        {
            return isOpenFlag;
        }
        public abstract bool JogMove(int nAxisNo, bool bPositive, int bStart, int nSpeed);

        public abstract bool Open();
        public abstract bool ReasetAxis(int nAxisNo);

        /// <summary>
        /// 默认是本地坐标系
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <param name="nPos"></param>
        /// <param name="nSpeed"></param>
        /// <returns></returns>
        public abstract bool RelativeMove(int nAxisNo, double nPos, int nSpeed);
        public abstract bool RelativeMove(int nAxisNo, double nPos, int nSpeed, CoordinateSys EpsonCoord);
        public abstract bool ServoOff();
        
        public abstract bool ServoOn();

        public abstract bool StopAxis(int nAxisNo);
       
        public abstract string RobotSendCmd(string Command);

        public abstract bool Login(string LoginRobotPassword);

        public abstract bool Logout();
       
        public abstract bool StartMission(int NumberOfMission);

        public abstract bool GetRobotStatus(ref string Status , bool bGetFromBuff = false );
      
        public abstract string ProcessResponse(string ResponseString);
        public abstract bool GetPointPos(string PointName, ref RobotPoint PointData);
        public abstract bool GetCurrentPos( ref RobotPoint PointData);
        public abstract bool SetPointPos(string PointName,string strLabel, PositionInfo NewPointData);
        public abstract bool Jump(string PointName);
       
        public abstract bool SavePointPos();
       
        public abstract bool SavePointPosWithSaveDialog();

        public abstract bool ConnectEPSON();

        public abstract bool GetVariable(ref string VariableName, ref string[] Value);
        public abstract bool SetVariable(string VariableName, string Value, RobotVariable VariableType);
        public abstract bool GetToolSetting(int NumberOfTool);
       
        public abstract bool SetTool(int NumberOfTool, double X, double Y, double Z, double U);

        public abstract bool Abort();

        public abstract bool SetPowerMode(RobotPower PowerMode);
        public abstract bool SLock(int TargetAxis);

        public abstract bool SLockAll(int AxisQty);


        public abstract bool SFree(int TargetAxis);


        public abstract bool SFreeAll(int AxisQty);


        public abstract bool ResetRobot();

        public double HexToDecimal(string TargetHex, string strErrorLog)
        {
            if (PasswordIsCorrect == false
                || SuccessBuiltNew == false)
            {
                return 0;
            }

            //十六----> 十 
            //（19.A）（十六）            
            //整数部分:
            //1*16（1）+9*16（0）=25 
            //小数部分:
            //10*16（-1）=0.625 
            //所以(19.A)(十六) = (25.625)(十)

            try
            {
                TargetHex = TargetHex.ToUpper();
                double TempInt = 0.0;

                //检查字符串是否为0~9,A~F
                for (int a = 0; a < TargetHex.Length; a++)
                {
                    if (TargetHex[a] != '0' & TargetHex[a] != '1' &
                        TargetHex[a] != '2' & TargetHex[a] != '3' &
                        TargetHex[a] != '4' & TargetHex[a] != '5' &
                        TargetHex[a] != '6' & TargetHex[a] != '7' &
                        TargetHex[a] != '8' & TargetHex[a] != '9' &
                        TargetHex[a] != 'A' & TargetHex[a] != 'B' &
                        TargetHex[a] != 'C' & TargetHex[a] != 'D' &
                        TargetHex[a] != 'E' & TargetHex[a] != 'F')
                    {
                        strErrorLog = "The value for parameter 'TargetHex' should be 0~9 and A~F.";
                        return -1;
                    }
                    else if (TargetHex[a] != '.')
                    {
                        TempInt += 1;
                    }
                }

                //只能有一个点号
                if (TempInt > 1)
                {
                    strErrorLog = "The parameter 'TargetHex' has " + TempInt + ".('dots'), invalid operation.";
                    return -1;
                }

                TempInt = 0;

                //判断是否有小数点
                if (TargetHex.IndexOf(".") == -1)
                {
                    //无小数点号
                    //计算转换16进制
                    for (int a = 0; a < TargetHex.Length; a++)
                    {
                        switch (TargetHex[a])
                        {
                            case '0':
                                TempInt += 0;
                                break;

                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'A':
                                TempInt += 10 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'B':
                                TempInt += 11 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'C':
                                TempInt += 12 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'D':
                                TempInt += 13 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'E':
                                TempInt += 14 * (Math.Pow(16, (TargetHex.Length - a - 1)));
                                break;

                            case 'F':
                                TempInt += 15 * (Math.Pow(16, (TargetHex.Length - a - 1)));
                                break;
                        }
                    }
                }
                else
                {
                    //有小数点号
                    //先计算转换16进制整数部分：
                    string TempStr = Strings.Mid(TargetHex, 1, TargetHex.IndexOf("."));

                    for (int a = 0; a < TempStr.Length; a++)
                    {

                        switch (TempStr[a])
                        {

                            case '0':
                                TempInt += 0;
                                break;

                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'A':
                                TempInt += 10 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'B':
                                TempInt += 11 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'C':
                                TempInt += 12 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'D':
                                TempInt += 13 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'E':
                                TempInt += 14 * (Math.Pow(16, (TargetHex.Length - a - 1)));
                                break;

                            case 'F':
                                TempInt += 15 * (Math.Pow(16, (TargetHex.Length - a - 1)));
                                break;
                        }
                    }

                    //再计算转换16进制小数部分：
                    TempStr = Strings.Mid(TargetHex, TargetHex.IndexOf(".") + 2, TargetHex.Length - TargetHex.IndexOf(".") - 1);

                    for (int a = 0; a < TempStr.Length; a++)
                    {

                        switch (TempStr[a])
                        {

                            case '0':
                                TempInt += 0;
                                break;

                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, -(a + 1));
                                break;

                            case 'A':
                                TempInt += 10 * Math.Pow(16, -(a + 1));
                                break;

                            case 'B':
                                TempInt += 11 * Math.Pow(16, -(a + 1));
                                break;

                            case 'C':
                                TempInt += 12 * Math.Pow(16, -(a + 1));
                                break;

                            case 'D':
                                TempInt += 13 * Math.Pow(16, -(a + 1));
                                break;

                            case 'E':
                                TempInt += 14 * Math.Pow(16, -(a + 1));
                                break;

                            case 'F':
                                TempInt += 15 * Math.Pow(16, -(a + 1));
                                break;
                        }
                    }
                }
                return TempInt;

            }
            catch (Exception ex)
            {
                strErrorLog = ex.Message;
                return -1;
            }
        }
        public abstract bool SetSpeed(int TargetSpeed);

        public abstract RobotStatusBits NewProcessStatusCode(string StatusCode);

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (RobotCommucate != null)
                {
                    RobotCommucate.Close();
                    RobotCommucate = null;
                }
                GC.Collect();

            }
            catch (Exception ex)
            {
                EpsonErrLog.ErrorMess = ex.Message; ;
                EpsonErrLog.ErrorCmd = "Dispose";
                ErrorDisPlay(this, ea);
            }
        }

        public abstract bool SetCurrentRobot(int NumberOfRobot);


        public abstract int GetCurrentRobot();


        public abstract bool SetACCELSpeed(ushort TargetAccelSpeed, ushort TargetDecelSpeed);

        /// <summary>
        /// 获取当前速度
        /// </summary>
        /// <returns></returns>
        public abstract int GetSpeed();

        /// <summary>
        /// 获取加速度
        /// </summary>
        /// <returns></returns>
        public abstract RobotAccel GetAccelSpeed();

        /// <summary>
        /// 获取当前功率模式
        /// </summary>
        /// <returns></returns>
        public abstract RobotPower GetPowerStatus();

        /// <summary>
        /// 设置原点坐标
        /// </summary>
        /// <param name="HomePoint"></param>
        /// <returns></returns>
        public abstract bool SetHome(RobotPoint HomePoint);
        public abstract bool Loacl(int LocalNo, RobotPoint LocalPoint);
        public abstract bool Base(RobotPoint BasePoint);
        /// <summary>
        /// 松开刹车功能
        /// </summary>
        /// <param name="TargetAxis"></param>
        /// <returns></returns>
        public abstract bool StopAxisOff(int TargetAxis);
        public abstract bool GetInputBit(int TargetInputBit, ref bool IsOn);
        public abstract bool SetOutputBit(int TargetOutputBit, bool TurnOn);

    }

   
    public class RobotMgr
    {
      private RobotMgr()
        {

        }
      private Thread m_thread = null;
      private static RobotMgr rotobMgr = null;
      private static object _lock = new object();
      public static RobotMgr GetInstance()
      {
            if(rotobMgr==null)
            {
                lock (_lock)
                {
                    if (rotobMgr == null)
                    {
                        rotobMgr = new RobotMgr();
                    }
                }
            }
            return rotobMgr;
      }
        Dictionary<string, RobotBase> m_dicRobot = new Dictionary<string, RobotBase>();
        public void  AddRobot(string strRobotName , string strEthMane,string strType,string strPwd,int AxisMin,int AxisMax)
        {
            RobotBase robot = null;  //new RotobBase(strRobotName, strPwd, AxisMin, AxisMax,)
            strType = strType.ToUpper();
            switch (strType)
            {
                case "EPSON":
                    robot = new Robot_EPSON(strRobotName, strPwd, strEthMane, AxisMin, AxisMax);
                    break;
            }
            m_dicRobot.Add(strRobotName, robot);
        }
        public string[] GetRobotNames()
        {
            string[] RobotNames = m_dicRobot.Keys.ToArray();
            return RobotNames;
        }
        public bool Open(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                if (!m_dicRobot[strRobotName].IsOpen())
                    return m_dicRobot[strRobotName].Open();
                else
                    return true;
            }
            return false;

        }
        public int GetRobotAxisMax(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
               return m_dicRobot[strRobotName].GetRobotAxisMax();
            }
            return -1;
        }

        public int GetRobotAxisMin(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetRobotAxisMin();
            }
            return -1;
        }
        public void StartThread()
        {
            if (m_thread == null)
            {
                m_thread = new Thread(GetRobotIoAndState);
                m_thread.IsBackground = true;
                m_thread.Start();
            }
               
        }
        private void GetRobotIoAndState()
        {
            string str = "";
            while (true)
            {
                foreach(var temp in  m_dicRobot)
                {
                   if(temp.Value.IsOpen())
                    {
                        temp.Value.GetRobotStatus(ref str);
                    }

                }
                Thread.Sleep(30);
            }

        }
        public RobotBase GetRobot(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
                return m_dicRobot[strRobotName];
            return null;
        }
        public void  SetListBox( string strRobotName, ListLog listBox)
        {
            m_dicRobot[strRobotName].SetShowListBox(listBox);
        }
        public bool GetInputBit(string strRobotName,int TargetInputBit, ref bool IsOn)
        {
           if( m_dicRobot.ContainsKey(strRobotName))
           {
                return m_dicRobot[strRobotName].GetInputBit(TargetInputBit,ref IsOn);
            }
            return false;

        }
        public bool SetOutputBit(string strRobotName, int TargetInputBit, bool TurnOn)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SetOutputBit(TargetInputBit, TurnOn);
            }
            return false;

        }
        public  bool AbsMove(string strRobotName, RobotPoint EpsonPoint)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].AbsMove(EpsonPoint);
            }
            else { return false; }
            
        }
        public  bool AbsMove(string strRobotName, string EpsonPoint)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].AbsMove(EpsonPoint);
            }
            else { return false; }
        }
        public  bool Close(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].Close();
            }
            else { return false; }

        }
        public  bool GetAxisActPos(string strRobotName, ref PositionInfo EpsonPos)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetAxisActPos(ref  EpsonPos);
            }
            else { return false; }
        }

        public  int GetAxisCmdPos(string strRobotName, int nAxisNo)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetAxisCmdPos(nAxisNo);
            }
            else { return -1; }
        }

        public  int GetAxisPos(string strRobotName, int nAxisNo)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetAxisCmdPos(nAxisNo);
            }
            else { return -1; }
        }

        public  long GetMotionIoState(string strRobotName, int nAxisNo)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetMotionIoState(nAxisNo);
            }
            else { return -1; }
        }

        public  bool GetServoState(string strRobotName, int nAxisNo)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetServoState(nAxisNo);
            }
            else { return false; }
        }

        public  bool Home(string strRobotName, int nAxisNo, int nParam)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].Home(nAxisNo, nParam);
            }
            else { return false; }
        }
        public  int IsAxisNormalStop(string strRobotName, int nAxisNo)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].IsAxisNormalStop(nAxisNo);
            }
            else { return -1; }
        }

        public  bool IsHomeNormalStop(string strRobotName, int nAxisNo)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].IsHomeNormalStop(nAxisNo);
            }
            else { return false; }
        }

        public  bool JogMove(string strRobotName, int nAxisNo, bool bPositive, int bStart, int nSpeed)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].JogMove(nAxisNo, bPositive, bStart, nSpeed);
            }
            else { return false; }
        }
        public  bool ReasetAxis(string strRobotName, int nAxisNo)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].ReasetAxis(nAxisNo);
            }
            else { return false; }
        }

        public  bool RelativeMove(string strRobotName, int nAxisNo, double  nPos, int nSpeed)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].RelativeMove(nAxisNo, nPos, nSpeed);
            }
            else {
                return false;
            }
        }
        public bool RelativeMove(string strRobotName, int nAxisNo, int nPos, int nSpeed, CoordinateSys EpsonCoord)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].RelativeMove(nAxisNo, nPos, nSpeed);
            }
            else { return false; }
        }

        public bool ServoOff(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].ServoOff();
            }
            else
            {
                return false;
            }
        }
        public bool ServoOn(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].ServoOn();
            }
            else
            {
                return false;
            }
        }

        public bool StopAxis(string strRobotName, int nAxisNo)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].StopAxis(nAxisNo);
            }
            else
            {
                return false;
            }
        }

        public bool Login(string strRobotName, string LoginRobotPassword)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].Login(LoginRobotPassword);
            }
            else
            {
                return false;
            }
        }

        public bool Logout(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].Logout();
            }
            else
            {
                return false;
            }
        }

        public bool StartMission(string strRobotName, int NumberOfMission)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].StartMission(NumberOfMission);
            }
            else
            {
                return false;
            }
        }

        public bool GetRobotStatus(string strRobotName, ref string Status)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetRobotStatus(ref Status);
            }
            else
            {
                return false;
            }
        }

        //public string ProcessResponse(string strRobotName, string ResponseString)
        //{
        //    if (m_dicRobot.ContainsKey(strRobotName))
        //    {
        //        return m_dicRobot[strRobotName].ProcessResponse(ResponseString);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        public bool GetPointPos(string strRobotName, string PointName, ref RobotPoint PointData)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetPointPos(PointName, ref PointData);
            }
            else
            {
                return false;
            }
        }
        public bool GetCurrentPos(string strRobotName, ref RobotPoint PointData)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetCurrentPos( ref PointData);
            }
            else
            {
                return false;
            }
        }
        public bool SetPointPos(string strRobotName, string PointId, string strLabel,PositionInfo NewPointData)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SetPointPos(PointId, strLabel, NewPointData);
            }
            else
            {
                return false;
            }
        }

        public bool Jump(string strRobotName, string PointName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].Jump(PointName);
            }
            else
            {
                return false;
            }
        }

        public bool SavePointPos(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SavePointPos();
            }
            else
            {
                return false;
            }
        }                


        public bool SavePointPosWithSaveDialog(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SavePointPosWithSaveDialog();
            }
            else
            {
                return false;
            }
        }

        public bool ConnectEPSON(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].ConnectEPSON();
            }
            else
            {
                return false;
            }
        }

        public bool GetVariable(string strRobotName, ref string VariableName, ref string[] Value)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetVariable(ref VariableName,ref Value);
            }
            else
            {
                return false;
            }
        }

        public bool SetVariable(string strRobotName, string VariableName, string Value, RobotVariable VariableType)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SetVariable(VariableName,Value,VariableType);
            }
            else
            {
                return false;
            }
        }

        public bool GetToolSetting(string strRobotName, int NumberOfTool)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetToolSetting(NumberOfTool);
            }
            else
            {
                return false;
            }
        }

        public bool SetTool(string strRobotName, int NumberOfTool, double X, double Y, double Z, double U)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SetTool(NumberOfTool,X,Y,Z,U);
            }
            else
            {
                return false;
            }
        }

        public bool Abort(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].Abort();
            }
            else
            {
                return false;
            }
        }

        public bool SetPowerMode(string strRobotName, RobotPower PowerMode)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SetPowerMode(PowerMode);
            }
            else
            {
                return false;
            }
        }

        public bool SLock(string strRobotName, int TargetAxis)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SLock(TargetAxis);
            }
            else
            {
                return false;
            }
        }

        public bool SFree(string strRobotName, int TargetAxis)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SFree(TargetAxis);
            }
            else
            {
                return false;
            }
        }

        public bool SLockAll(string strRobotName, int AxisQty)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SLockAll(AxisQty);
            }
            else
            {
                return false;
            }
        }
        public bool SFreeAll(string strRobotName, int AxisQty)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SFreeAll(AxisQty);
            }
            else
            {
                return false;
            }
        }

        public bool ResetRobot(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].ResetRobot();
            }
            else
            {
                return false;
            }
        }

        public bool SetSpeed(string strRobotName, int TargetSpeed)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SetSpeed(TargetSpeed);
            }
            else
            {
                return false;
            }
        }

        public RobotStatusBits NewProcessStatusCode(string strRobotName, string StatusCode)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].NewProcessStatusCode(StatusCode);
            }
            else
            {
                RobotStatusBits TempStatus = new RobotStatusBits();
                return TempStatus;
            }
        }

        public bool SetCurrentRobot(string strRobotName, int NumberOfRobot)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SavePointPosWithSaveDialog();
            }
            else
            {
                return false;
            }
        }

        public int GetCurrentRobot(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetCurrentRobot();
            }
            else
            {
                return -1;
            }
        }

        public bool SetACCELSpeed(string strRobotName, ushort TargetAccelSpeed, ushort TargetDecelSpeed)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SavePointPosWithSaveDialog();
            }
            else
            {
                return false;
            }
        }

        public int GetSpeed(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetSpeed();
            }
            else
            {
                return -1;
            }
        }

        public RobotAccel GetAccelSpeed(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetAccelSpeed();
            }
            else
            {
                RobotAccel TempAccel;
                TempAccel.AccelSpeed = -1;
                TempAccel.DecelSpeed = -1;
                return TempAccel;
            }
        }

        public RobotPower GetPowerStatus(string strRobotName)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].GetPowerStatus();
            }
            else
            {
                return RobotPower.UnKonw;
            }
        }

        public bool SetHome(string strRobotName, RobotPoint HomePoint)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SavePointPosWithSaveDialog();
            }
            else
            {
                return false;
            }
        }

        public bool Loacl(string strRobotName, int LocalNo, RobotPoint LocalPoint)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SavePointPosWithSaveDialog();
            }
            else
            {
                return false;
            }
        }

        public bool Base(string strRobotName, RobotPoint BasePoint)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SavePointPosWithSaveDialog();
            }
            else
            {
                return false;
            }
        }
        public bool StopAxisOff(string strRobotName, int TargetAxis)
        {
            if (m_dicRobot.ContainsKey(strRobotName))
            {
                return m_dicRobot[strRobotName].SavePointPosWithSaveDialog();
            }
            else
            {
                return false;
            }
        }
    
    }


}