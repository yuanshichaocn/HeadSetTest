//using MotionIoLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//#region "以太网"
//using System.Net.Sockets;
//using System.Net;
//using Communicate;
//#endregion
//using Microsoft.VisualBasic;
//using System.Windows.Forms;
//namespace MotionIoLib
//{
//    #region "命令说明"
//    //*******************************************************************
//    //String .IndexOf 方法 (Char)    报告指定 Unicode 字符在此字符串中的第一个匹配项的索引。

//    //命名空间：   System
//    //程序集：   mscorlib（在 mscorlib.dll 中） 

//    //Public Function IndexOf( ByVal value As Char ) As Integer

//    //参数
//    //value    类型:    System.Char    要查找的 Unicode 字符。

//    //返回值  类型:     System.Int32   如果找到该字符,则为 value 的从零开始的索引位置；如果未找到,则为 -1。 
//    //*******************************************************************
//    #endregion

//    #region "EPSON远程以太网控制命令响应说明"

//    //如果控制器接收到正确的命令,将在执行命令时显示下列格式的响应。
//    //---------------------------------------------------------------------------------------------------------------
//    //命令                                            格式
//    //---------------------------------------------------------------------------------------------------------------
//    //获取数值的远程命令GetIO、
//    //GetVariable和GetStatus 除外                     #[远程命令],[0]终端
//    //---------------------------------------------------------------------------------------------------------------
//    //GetIO                                           #GetIO,[0 | 1]终端
//    //---------------------------------------------------------------------------------------------------------------
//    //GetMemIO                                        #GetMemIO,[0 | 1]终端 *1
//    //---------------------------------------------------------------------------------------------------------------
//    //GetIOByte                                       #GetIOByte,[字节（8 位）的十六进制字符串（00到 FF）]终端
//    //---------------------------------------------------------------------------------------------------------------
//    //GetMemIOByte                                    #GetMemIOByte,[字节（8 位）的十六进制字符串（00 到 FF）]终端
//    //---------------------------------------------------------------------------------------------------------------
//    //GetIOWord                                       #GetIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端
//    //---------------------------------------------------------------------------------------------------------------
//    //GetIOMemWord                                    #GetMemIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端
//    //---------------------------------------------------------------------------------------------------------------
//    //GetVariable                                     #GetVariable,[参数值] 终端
//    //---------------------------------------------------------------------------------------------------------------
//    //GetVariable（如果是数组）                       #GetVariable,[参数值 1],[参数值 2],...,终端 *4
//    //---------------------------------------------------------------------------------------------------------------
//    //GetStatus                                       #GetStatus,[状态],[错误,警告代码]终端
//    //                                                例如） #GetStatus,aaaaaaaaaa,bbbb
//    //                                                                       *2     *3
//    //---------------------------------------------------------------------------------------------------------------
//    //Execute                                        如果作为命令执行的结果返回数值  #Execute,“[执行结果]” 终端
//    //---------------------------------------------------------------------------------------------------------------

//    //*1 [0 | 1] I/O 位 开：1/关：0
//    //----------------------------------------------------------------------------------------
//    //*2 状态
//    //在上例中,10 位数字“aaaaaaaaaa”用于以下 10 个标志。
//    //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关
//    //如果 Teach 和 Auto 为开,则为 1100000000。
//    //----------------------------------------------------------------------------------------
//    //*3 错误/警告代码
//    //以 4 位数字表示。如果没有错误和警告,则为 0000。

//    //例如）1： #GetStatus,0100000001,0000
//    //Auto 位和 Ready 位为开（1）。
//    //表示自动模式开启并处于准备就绪状态。已启用命令执行。

//    //例如）2： #GetStatus,0110000010,0517
//    //这意味着运行过程中发生警告。对警告代码采取适当的行动。（在这种情况下,警告代码为 0517）

//    //标志                 内容
//    //----------------------------------------------------------------------------------------
//    //Test            在TEST模式下打开
//    //----------------------------------------------------------------------------------------
//    //Teach           在TEACH模式下打开
//    //----------------------------------------------------------------------------------------
//    //Auto            在远程输入接受条件下打开
//    //----------------------------------------------------------------------------------------
//    //Warnig          在警告条件下打开
//    //                甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
//    //----------------------------------------------------------------------------------------
//    //SError          在严重错误状态下打开
//    //                发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
//    //----------------------------------------------------------------------------------------
//    //Safeguard       安全门打开时打开
//    //----------------------------------------------------------------------------------------
//    //EStop           在紧急状态下打开
//    //----------------------------------------------------------------------------------------
//    //Error           在错误状态下打开
//    //                使用“Reset 输入”从错误状态中恢复。
//    //----------------------------------------------------------------------------------------
//    //Paused          打开暂停的任务
//    //----------------------------------------------------------------------------------------
//    //Running         执行任务时打开
//    //                在“Paused 输出”为开时关闭。
//    //----------------------------------------------------------------------------------------
//    //Ready           控制器完成启动且无任务执行时打开
//    //----------------------------------------------------------------------------------------

//    //*4 返回要获取的编号中指定编号的值。

//    //*******************************************************************

//    #endregion

//    #region "EPSON远程以太网控制说明"
//    //执行远程以太网控制
//    //通过以下步骤可设置远程控制。
//    //(1) 从客户端设备连接到控制器远程以太网中指定的端口上。
//    //(2) 将远程以太网中设置的密码指定到该参数上并发送Login 命令。
//    //(3) 执行远程命令前,客户端设备须等到Auto（GetStatus 命令响应）为ON 为止。
//    //(4) 现在,远程命令将被接受。
//    //每个命令执行输入接受条件的功能。

//    //调试远程以太网控制
//    //从EPSON RC+ 7.0 开发环境中调试程序的能力如下所述。
//    //(1) 照例创建一个程序。
//    //(2) 打开“运行”窗口,单击<Ethernet 启用>按钮。
//    //如果您使用远程以太网控制只是获得了该值,则不显示<Ethernet 启用>按钮。单
//    //击指定为控制装置的设备的<开始>按钮。
//    //(3) 现在,远程命令将被接受。
//    //“运行”窗口中的断点设置和输出是可用的。
//    //如果5分钟内未从外部设备中Login,该连接将被自动切断。Login后,如果命令未在
//    //远程以太网的超时时间内发送出去,连接将被切断。在这种情况下,重新建立连
//    //接。
//    //如果发生错误,执行操作命令之前请执行复位命令,以清除错误条件。若要通过监
//    //控从外部设备上清除错误条件,执行“GetStatus”和“Reset”命令。

//    //如果在(超时)框中设置“0”,则超时时间为无限。在这种情况下,该任务继续执
//    //行,即使没有来自客户端的通信。这意味着机械手可能会继续移动并给设备造成
//    //意外损坏。确保使用除通信以外的方式来停止该任务。

//    //-----------*********************===================
//    //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//    //---------------------------------------------------------------------------------------------------
//    //远程命令            参数                   内容                                              输入接受条件
//    //Login               密码                    启动控制器远程以太网功能   
//    //                                           通过密码验证
//    //                                           正确执行登录,并执行命令,直到退出                随时可用
//    //---------------------------------------------------------------------------------------------------
//    //Logout                                     退出控制器远程以太网功能
//    //                                           退出登录后,执行登录命令来启动远程以太网功能。
//    //                                           在任务执行期间退出会导致错误发生。                随时可用
//    //---------------------------------------------------------------------------------------------------
//    //Start               功能编号               执行指定编号的功能                                Auto开/Ready开/Error关/EStop关/Safeguard开
//    //---------------------------------------------------------------------------------------------------
//    //Stop                                       停止所有的任务和命令。                            Auto开
//    //---------------------------------------------------------------------------------------------------
//    //Pause                                      暂停所有任务                                      Auto 开/Running 开
//    //---------------------------------------------------------------------------------------------------
//    //Continue                                   继续暂停了的任务                                  Auto 开/Paused 开
//    //---------------------------------------------------------------------------------------------------
//    //Reset                                      清除紧急停止和错误                                Auto 开/Ready 开
//    //---------------------------------------------------------------------------------------------------
//    //SetMotorsOn         机械手编号             打开机械手电机                                    Auto开/Ready开/EStop关/Safeguard关
//    //---------------------------------------------------------------------------------------------------
//    //SetMotorsOff        机械手编号             关闭机械手电机                                    Auto开/Ready开
//    //---------------------------------------------------------------------------------------------------
//    //SetCurRobot         机械手编号             选择机械手                                        Auto开/Ready开
//    //---------------------------------------------------------------------------------------------------
//    //GetCurRobot                                获取当前的机械手编号                              随时可用
//    //---------------------------------------------------------------------------------------------------
//    //Home                机械手编号             将机械手手臂移动到由用户定义的起始点位置上        Auto开/Ready开/Error关/EStop关/Safeguard 关
//    //---------------------------------------------------------------------------------------------------
//    //GetIO               I/O位号                获得指定的I/O位                                   随时可用
//    //---------------------------------------------------------------------------------------------------
//    //SetIO               I/O位号和值            设置I/O指定的位
//    //                                             1：打开此位
//    //                                             0：关闭此位                                     Ready开
//    //---------------------------------------------------------------------------------------------------
//    //GetIOByte           I/O位号                获得指定的I/O端口（8位）                          随时可用
//    //---------------------------------------------------------------------------------------------------
//    //SetIOByte           I/O端口号和值          设置I/O指定的端口（8位）                          Ready开
//    //---------------------------------------------------------------------------------------------------
//    //GetIOWord           I/O字端口号            获得指定的I/O字端口（16位）                       随时可用
//    //---------------------------------------------------------------------------------------------------
//    //SetIOWord           I/O字端口号和值        设置I/O指定的端口（16位）                         Auto开/Ready开
//    //---------------------------------------------------------------------------------------------------
//    //GetMemIO            内存I/O位号            获得指定的内存I/O位                               随时可用
//    //---------------------------------------------------------------------------------------------------
//    //SetMemIO            内存I/O位号和值        设置指定的内存I/O位
//    //                                               1: 打开此位
//    //                                               0: 关闭此位                                   Auto开/Ready开
//    //---------------------------------------------------------------------------------------------------
//    //GetMemIOByte        内存I/O端口号          获得指定的内存I/O端口（8位）                      随时可用
//    //---------------------------------------------------------------------------------------------------
//    //SetMemIOByte        内存I/O端口号和值      设置指定的内存I/O端口（8位）                      Auto开/Ready开
//    //---------------------------------------------------------------------------------------------------
//    //GetMemIOWord        内存I/O字端口号        获得指定的内存I/O字端口（16位）                   随时可用
//    //---------------------------------------------------------------------------------------------------
//    //SetMemIOWord        内存I/O字端口号和值    设置指定的内存I/O字端口（16位）                   Auto开/Ready开
//    //---------------------------------------------------------------------------------------------------
//    //GetVariable         参数名称{, type}       获取备份（Global Preserve）参数的值               随时可用
//    //                    ***********************************************************
//    //                    (参数名称)（数组元
//    //                    素）,(参数名称类
//    //                    型),(获取的编号)      获取备份（Global Preserve）数组参数的值
//    //---------------------------------------------------------------------------------------------------
//    //SetVariable        参数名称和值{,类型}    设置备份（Global Preserve）参数中的值             Auto开/Ready开
//    //---------------------------------------------------------------------------------------------------
//    //GetStatus                                  获取控制器的状态                                  随时可用
//    //---------------------------------------------------------------------------------------------------
//    //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//    //---------------------------------------------------------------------------------------------------
//    //Abort                                      中止命令的执行                                    Auto开
//    //---------------------------------------------------------------------------------------------------

//    //(*7) 如果“0”被指定为机械手编号,所有的机械手将进行操作。
//    //如果您想操作具体的机械手,指定目标机械手的机械手编号（1到16）。
//    //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String}。
//    //指定的类型： 在参数名称和类型相同时用于备份参数。
//    //未指定的类型： 在参数名称相同时用于备份参数。
//    //(*9) 对于数组元素,指定以下您想获取的元素：
//    //如果是从数组头处获取的,您需要指定一个元素。
//    //1维数组 参数名称 (0) 从头部获取。
//    //参数名称（元素编号） 从指定的元素编号中获取。
//    //2维数组 参数名称 (0,0) 从头部获取。
//    //参数名称（元素编号1,2） 从指定的元素编号中获取。
//    //3维数组 参数名称 (0,0,0) 从头部获取。
//    //参数名称（元素编号1,2,3） 从指定的元素编号中获取。
//    //您不能忽略要获取的参数类型和编号。
//    //您不能指定一个参数类型string。
//    //可获取的可用数量多达100个。如果您在数组元素编号上指定一个号码,会发生错误。
//    //如）“$GetVariable,gby2(3,0),Byte,3”
//    //它获得字节型2维数组参数gby2的gby2(3,0)、gby2(3,1)、gby2(3,2)的值。
//    //(*10) 在双引号中指定命令和参数。
//    //待执行的命令字符串和执行结果字符串被限制在4060字节。
//    //机械手动作命令将被执行到所选的机械手上。执行命令之前检查使用GetCurRobot选中的机械手。

//    //运行Execute时可用的命令
//    //远程命令
//    //Abort
//    //GetStatus
//    //SetIO
//    //SetIOByte
//    //SetIOWord
//    //SetMemIO
//    //SetMemIOByte
//    //SetMemIOWord
//    //Execute执行命令和输出命令
//    //如果在（SetIO, SetIOByte, SetIOWord, SetMemIO, SetMemIOByte, SetMemIOWrod）
//    //中指定的命令是相同的并且同时执行,后来执行的命令将导致错误。确保在执行
//    //Execute命令和输出命令后使用正在执行Execute命令的GetStatus来检查执行结果。
//    //(*11) 若要执行PCDaemon功能的命令,务必要在连接了RC+ 7.0时执行。如果未连
//    //接RC+ 7.0,执行命令将会导致错误。

//    //************************************************

//    //$Execute,"print realpos"
//    //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
//    //$Execute,"move here +x(-1)"
//    //#Execute,0
//    //$Execute,"print realpos"
//    //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
//    //$Execute,"move here -x(1)"
//    //#Execute,0
//    //$Execute,"print realpos"
//    //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0

//    //************************************************
//    //【move here -x(1)和move here +x(1)的作用是一样的】
//    //************************************************ 
//    #endregion

//    #region "已经处理好的事项"
//    //1、需要对进行点位运动的命令参数进行验证,验证正确后再执行,否则提示错误；【OK】
//    //2、实例化时需要将相关参数进行验证；【OK】

//    //6、添加错误处理代码；【OK】
//    //7、添加GetStatus返回值得判断处理代码；【OK】
//    //8、添加所有的返回值处理代码；【OK】
//    //9、想办法将信息集中更新到新添加的RichTextBox中；【OK】

//    //12、用FeedBackMessageFromRobot来保存从EPSON返回的任何结果,可在外部进行读写,公共变量；【OK】
//    //13、execute,的后面不能有空格,否则就执行错误；【OK】 
//    #endregion

//    #region "待处理事项"
//    //3、对点位运动的命令添加可选参数,例如Z轴限位高度；【】
//    //4、函数GetToolSetting不对,需要仔细检查或者如果EPSON SPEL语音中没有就去掉此函数；【已经处理了一部分,需要检查返回的值然后再修改程序】
//    //5、添加导出点数据到Excel文件的代码；【】
//    //10、在EPSON控制器中位、字节和字是从0开始,但是在给用户使用此DLL时,为避免误解,全部改为1开始,然后在相应函数里面全部-1；【】
//    //11、需要矫正内存操作的参数值范围；【】
//    //14、需要检查GET命令返回后的值处理是否正确，因为返回的命令里面最后有结束符；【OK】
//    //15、添加LoadPoints;【OK】
//    //16、添加十进制转换为2进制的代码；【】
//    //17、添加设置机械手模式【自动/程序模式】的代码；【】
//    //18、旋转U轴需要在 U 前面加上 “:” 号，否则报错，与X、Y、Z轴不同；【OK】
//    //    【现在又报错，改方法：先获取当前位置，然后再绝对定位+U的角度】【OK】
//    //19、因为在调试过程中出现EPSON断开远程服务器，所以改为Private NewClientForConnectRemoteEpson 
//    //    As SimpleClientStation来与EPSON服务器建立通讯；待测试；【】
//    //20、需要监控对比远程IO的实际IO信号的变化，与远程以太网相比较，哪些IO是无法手动控制的，是自动输出的；【】
//    //21、GetOutByteStatus和GetOutWordStatus执行指令时报错；暂时设置为private【】
//    //22、对各个点位运动的距离进行最大/最小值检查，避免超范围运动；【】 
//    #endregion



//    public class Motion_EPSON : MotionCardBase, RobotInterface
//    {
//        private RobotInfo robotInfo;
//        /// <summary>
//        /// 机械手的字段
//        /// </summary>
//        private bool ServoFlag = false;

//        public RobotInfo robotInfo { get => robotInfo; set => robotInfo = value; }
//        /// <summary>
//        /// 错误输出时间
//        /// </summary>
//        public event EventHandler ErrorDisPlay;
//        /// <summary>
//        /// Epson错误日志
//        /// </summary>
//        public MotionErrorMess EpsonErrLog;
//        /// <summary>
//        /// 成功构造标志，密码正确标志
//        /// </summary>
//        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

//        private RobotPoint robotPoint;
//        /// <summary>
//        /// 从EPSON机械手返回的信息
//        /// </summary>
//        public string FeedBackMessageFromRobot = "";

//        private bool ConnectRemoteEpsonTCPIPOk;
//        EventArgs ea = new EventArgs();
//        /// <summary>
//        /// 机械手是否准备完成
//        /// </summary>
//        private bool isOpenFlag = false;
//        private TcpLink RobotCommucate = null;
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="indexCard"> 卡的编号</param>
//        /// <param name="strName">卡的名字</param>
//        /// <param name="nMinAxisNo"> 最小轴号</param>
//        /// <param name="nMaxAxisNo">最大轴号</param>
//        public Motion_EPSON(ulong indexCard, string strName, int nMinAxisNo, int nMaxAxisNo)
//            : base(indexCard, strName, nMinAxisNo, nMaxAxisNo)
//        {
//            robotPoint.X = 0;
//            robotPoint.Y = 0;
//            robotPoint.Z = 0;
//            robotPoint.U = 0;
//            robotPoint.V = 0;
//            robotPoint.W = 0;
//            robotPoint.HandStyle = RobotHand.Unknow;
//            this.RobotPause = false;
//            this.RobotExecuteBusy = false;
//            this.RobotPointMes = robotPoint;
//            EpsonErrLog.ErrorCmd = "";
//            EpsonErrLog.DeviceType = "Epson_Robot";
//            EpsonErrLog.ErrorMess = "";
//            ConnectRemoteEpsonTCPIPOk = false;
//            if (CommuncateType == CommuncateItf.Internet)
//            {
//                RobotCommucate = (TcpLink)CommuncateInterface;
//            }
//        }
//        public override bool AbsMove(int nAxisNo, int nPos, int nSpeed)
//        {

//            return false;
//        }
//        /// <summary>
//        /// 执行的go函数
//        /// </summary>
//        /// <param name="EpsonPoint"></param>
//        /// <returns></returns>
//        public bool AbsMove(RobotPoint EpsonPoint)
//        {
//            try
//            {
//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                string TempStr = "";
//                if (EpsonPoint.HandStyle == RobotHand.LeftHand)
//                {
//                    TempStr = "/L";
//                }
//                else
//                {
//                    TempStr = "/R";
//                }

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Go XY(" + EpsonPoint.X.ToString() + ","
//                        + EpsonPoint.Y.ToString() + "," + EpsonPoint.Z.ToString() + "," + EpsonPoint.U.ToString()
//                        + "," + EpsonPoint.V.ToString() + "," + EpsonPoint.W.ToString() + ")" + TempStr + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "AbsMove";
//                ErrorDisPlay(this, ea);
//                RobotExecuteBusy = false;
//                return false;
//            }
//        }
//        public bool AbsMove(string EpsonPoint)
//        {
//            try
//            {

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//                //----------------------------------------------------

//                //如果PointName的第一个字母不是P或者数字大于999或者小于0
//                if (Strings.Mid(EpsonPoint.ToUpper(), 1, 1) != "P"
//                    | (Convert.ToUInt16(Strings.Mid(EpsonPoint.ToUpper(), 2, EpsonPoint.Length - 1)) < 0
//                    & Convert.ToUInt16(Strings.Mid(EpsonPoint.ToUpper(), 2, EpsonPoint.Length - 1)) > 999))
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Go " + EpsonPoint + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "AbsMove";
//                ErrorDisPlay(this, ea);
//                RobotExecuteBusy = false;
//                return false;
//            }
//        }
//        public override bool AddAxisToGroup(int[] nAxisarr, ref object group)
//        {
//            return false;
//        }

//        public override bool AddBufDelay(object objGroup, int nTime)
//        {
//            return false;
//        }

//        public override bool AddBufIo(object objGroup, string strIoName, bool bVal, int nAxisIndexInGroup = 0)
//        {
//            return false;
//        }

//        public override bool AddBufMove(object objGroup, BufMotionType type, int mode, int nAxisNum, double velHigh, double velLow, double[] Point1, double[] Point2)
//        {
//            return false;
//        }

//        public override bool ClearBufMove(object objGroup)
//        {
//            return false;
//        }

//        public override bool Close()
//        {

//            if (Logout() == false)
//            {
//                return false;
//            }
//            else
//            {
//                EpsonErrLog.ErrorMess = "current robot not open\r\n";
//                EpsonErrLog.ErrorCmd = "close";
//                ErrorDisPlay(this, new EventArgs());
//                return false;
//            }
//        }
//        /// <summary>
//        /// 获取实际坐标值（不可用）
//        /// </summary>
//        /// <param name="nAxisNo"></param>
//        /// <returns></returns>
//        public override int GetAxisActPos(int nAxisNo)
//        {
//            return -1;
//        }
//        public bool GetAxisActPos(ref RobotPoint EpsonPos)
//        {
//            try
//            {
//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetAxisActPos";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetAxisActPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Print RealPos\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetAxisActPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                //$Execute,"Print RealPos"
//                //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

//                //$execute,"print curpos"
//                //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {

//                    try
//                    {

//                        i = FeedBackMessageFromRobot.IndexOf(":", 0);
//                        EpsonPos.X = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        EpsonPos.Y = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        EpsonPos.Z = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        EpsonPos.U = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        EpsonPos.V = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        EpsonPos.W = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));

//                        //判断左右手势
//                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/L") != -1)
//                        {
//                            EpsonPos.HandStyle = RobotHand.LeftHand;
//                        }

//                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/R") != -1)
//                        {
//                            EpsonPos.HandStyle = RobotHand.RightHand;
//                        }

//                    }
//                    catch (Exception ex)
//                    {
//                        EpsonErrLog.ErrorMess = ex.Message; ;
//                        EpsonErrLog.ErrorCmd = "GetAxisActPos";
//                        ErrorDisPlay(this, ea);
//                        this.RobotExecuteBusy = false;
//                        return false;
//                    }

//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GetAxisActPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GetAxisActPos";
//                ErrorDisPlay(this, ea);
//                RobotExecuteBusy = false;
//                return false;
//            }
//        }
//        /// <summary>
//        /// 获取当前位置坐标值（不可用）
//        /// </summary>
//        /// <param name="nAxisNo"></param>
//        /// <returns></returns>
//        public override int GetAxisCmdPos(int nAxisNo)
//        {
//            return -1;
//        }
//        public override int GetAxisPos(int nAxisNo)
//        {
//            return -1;
//        }

//        public override long GetMotionIoState(int nAxisNo)
//        {
//            long WordStatus = 0;
//            string StrErrLog = string.Empty;
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //GetIOWord   I/O字端口号   获得指定的I/O字端口（16位）   随时可用
//                //----------------------------------------------------------------

//                WordStatus = 0;

//                if (nAxisNo > 1 | nAxisNo < 0)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "GetMotionIoState";
//                    ErrorDisPlay(this, ea);
//                    return -1;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetMotionIoState";
//                    ErrorDisPlay(this, ea);
//                    return -1;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetMotionIoState";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$GetIOWord," + nAxisNo);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetMotionIoState";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    //在此添加判断返回的位的值为0或者1
//                    //GetIOWord    #GetIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端

//                    string[] TempStr = FeedBackMessageFromRobot.Split(',');

//                    if (TempStr[0] != "#GetIOWord")
//                    {
//                        RobotExecuteBusy = false;
//                        return -1;
//                    }

//                    WordStatus = Convert.ToUInt16(HexToDecimal(Strings.Right(FeedBackMessageFromRobot, 4), StrErrLog));

//                    RobotExecuteBusy = false;
//                    return WordStatus;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GetMotionIoState";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "AbsMove";
//                ErrorDisPlay(this, ea);
//                return -1;
//            }
//        }

//        public override bool GetServoState(int nAxisNo)
//        {
//            return ServoFlag;
//        }

//        public override bool Home(int nAxisNo, int nParam)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Home    机械手编号   将机械手手臂移动到由用户定义的起始点位置上   Auto开/Ready开/Error关/EStop关/Safeguard 关
//                //---------------------------------------------------------------------------------------------------

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GoHome";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GoHome";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Home," + 1);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GoHome";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GoHome";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GoHome";
//                ErrorDisPlay(this, ea);
//                RobotExecuteBusy = false;
//                return false;
//            }
//        }

//        public override int IsAxisNormalStop(int nAxisNo)
//        {
//            return -1;
//        }

//        public override bool IsHomeNormalStop(int nAxisNo)
//        {
//            return true;
//        }

//        public override bool IsOpen()
//        {
//            return isOpenFlag;
//        }

//        public override bool isOrgTrig(int nAxisNo)
//        {
//            return true;
//        }

//        public override bool JogMove(int nAxisNo, bool bPositive, int bStart, int nSpeed)
//        {
//            return RelativeMove(nAxisNo, 1, nSpeed);
//        }

//        public override bool Open()
//        {
//            if (this.SuccessBuiltNew == true)
//            {
//                EpsonErrLog.ErrorMess = "Device is Open\r\n";
//                EpsonErrLog.ErrorCmd = "Open";
//                ErrorDisPlay(this, new EventArgs());
//                return false;
//            }
//            else
//            {
//                if (ConnectEPSON() == false)
//                {
//                    return false;
//                }

//                if (Login(this.robotInfo.PassWard) == false)
//                {
//                    return false;
//                }
//                if (ServoOn(1) == false)
//                {
//                    return false;
//                }
//                if (SetPowerMode(RobotPower.HIGHPOWER) == false)
//                {
//                    return false;
//                }
//                isOpenFlag = true;
//            }
//            return true;
//        }

//        public override bool ReasetAxis(int nAxisNo)
//        {
//            return ResetRobot();
//        }
//        /// <summary>
//        /// 默认是本地坐标系
//        /// </summary>
//        /// <param name="nAxisNo"></param>
//        /// <param name="nPos"></param>
//        /// <param name="nSpeed"></param>
//        /// <returns></returns>
//        public override bool RelativeMove(int nAxisNo, int nPos, int nSpeed)
//        {
//            string AxisNum = string.Empty;

//            if (false == SetSpeed(nSpeed))
//            {
//                EpsonErrLog.ErrorMess = "Speed set failure\r\n";
//                EpsonErrLog.ErrorCmd = "RelativeMove";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//            switch (nAxisNo)
//            {
//                case 0:
//                    AxisNum = "X";
//                    break;
//                case 1:
//                    AxisNum = "Y";
//                    break;
//                case 2:
//                    AxisNum = "Z";
//                    break;
//                case 3:
//                    AxisNum = "U";
//                    break;
//                case 4:
//                    AxisNum = "V";
//                    break;
//                case 5:
//                    AxisNum = "W";
//                    break;
//                default:
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    return false;
//            }
//            try
//            {

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Move Here + " + AxisNum + "(" +
//                        nPos + ")" + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "RelativeMove";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }
//        public bool RelativeMove(int nAxisNo, int nPos, int nSpeed, Coordinate EpsonCoord)
//        {
//            string Cmd = string.Empty;
//            if (EpsonCoord == Coordinate.Local)
//            {
//                Cmd = "Move";
//            }
//            else if (EpsonCoord == Coordinate.Tool)
//            {
//                Cmd = "TMove";
//            }
//            else
//            {
//                EpsonErrLog.ErrorMess = "not select Coordinate\r\n";
//                EpsonErrLog.ErrorCmd = "RelativeMove";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//            string AxisNum = string.Empty;

//            if (false == SetSpeed(nSpeed))
//            {
//                EpsonErrLog.ErrorMess = "Speed set failure\r\n";
//                EpsonErrLog.ErrorCmd = "RelativeMove";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//            switch (nAxisNo)
//            {
//                case 0:
//                    AxisNum = "X";
//                    break;
//                case 1:
//                    AxisNum = "Y";
//                    break;
//                case 2:
//                    AxisNum = "Z";
//                    break;
//                case 3:
//                    AxisNum = "U";
//                    break;
//                case 4:
//                    AxisNum = "V";
//                    break;
//                case 5:
//                    AxisNum = "W";
//                    break;
//                default:
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    return false;
//            }
//            try
//            {

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"" + Cmd + " Here + " + AxisNum + "(" +
//                        nPos + ")" + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "RelativeMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "RelativeMove";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }


//        public override bool ServoOff(short nAxisNo)
//        {
//            nAxisNo = 1;
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //SetMotorsOff         机械手编号             关闭机械手电机                                    Auto开/Ready开/EStop关/Safeguard关
//                //---------------------------------------------------------
//                //判断是否正在执行命令，不然会丢失指令
//                if (RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "ServoOff";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "ServoOff";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$SetMotorsOff," + 1);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "ServoOff";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    ServoFlag = false;
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "ServoOff";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "ServoOff";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public override bool ServoOn(short nAxisNo)
//        {
//            nAxisNo = 1;
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //SetMotorsOn         机械手编号             打开机械手电机                                    Auto开/Ready开/EStop关/Safeguard关
//                //---------------------------------------------------------
//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "ServoOn";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "ServoOn";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$SetMotorsOn," + 1);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "ServoOn";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    ServoFlag = true;
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "ServoOn";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "ServoOn";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public override bool StartBufMove(object objGroup)
//        {
//            return false;
//        }

//        public override bool StopAxis(int nAxisNo)
//        {
//            try
//            {

//                if (nAxisNo < 1 | nAxisNo > 6)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "StopAxis";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "StopAxis";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "StopAxis";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //Brake
//                //用于打开和关闭当前机械手指定关节的制动器。
//                //格式
//                //Brake 状态, 关节编号
//                //参数
//                //状态 施加制动时：使用On。
//                //解除制动时：使用Off。
//                //关节编号 指定1～6 的关节编号。
//                //说明
//                //Brake 命令用于对垂直6 轴型机械手的一个关节施加或解除制动。这是仅可通过命令使用的命令。此
//                //命令设计为只有维修作业人员才可以使用。
//                //如果执行Brake 命令，则会对机械手控制参数进行初始化。

//                //brake on, 1
//                //brake off, 1

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Brake on, " +
//                        nAxisNo + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "StopAxis";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "StopAxis";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "StopAxis";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public override bool StopEmg(int nAxisNo)
//        {
//            return false;
//        }
//        public string RobotSendCmd(string Command)
//        {
//            string strReceived = "";
//            try
//            {
//                if (Command == "")
//                {
//                    EpsonErrLog.ErrorMess = "Not Message Command\r\n";
//                    EpsonErrLog.ErrorCmd = "SendCmd\r\n";
//                    ErrorDisPlay(this, ea);
//                    return "";
//                }
//                if (ConnectRemoteEpsonTCPIPOk == false)
//                {
//                    EpsonErrLog.ErrorMess = "Not Connect\r\n";
//                    EpsonErrLog.ErrorCmd = "SendCmd\r\n";
//                    ErrorDisPlay(this, ea);
//                    return "";
//                }
//#if true
//                byte[] BytesReceived = new byte[4096];
//                byte[] SendBytes = new byte[4096];

//                //发送
//                SendBytes = System.Text.Encoding.ASCII.GetBytes(Command);
//                RobotCommucate.WriteData(SendBytes, SendBytes.Length);
//                strReceived = "";
//                //发送完之后接收EPSON返回内容
//                FeedBackMessageFromRobot = "";
//                int i = RobotCommucate.ReadData(BytesReceived, 4096);
//                FeedBackMessageFromRobot = System.Text.Encoding.ASCII.GetString(BytesReceived, 0, i);
//                strReceived = FeedBackMessageFromRobot;
//                return strReceived;
//#endif
//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message;
//                EpsonErrLog.ErrorCmd = "RobotSendCmd";
//                ErrorDisPlay(this, ea);
//                return strReceived;
//            }
//        }

//        public bool Login(string LoginRobotPassword)
//        {
//            try
//            {
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "Login";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    this.RobotExecuteBusy = true;
//                }
//                FeedBackMessageFromRobot = RobotSendCmd("$Login," + LoginRobotPassword);
//                if (FeedBackMessageFromRobot.IndexOf("!") == -1)  //未找到
//                {
//                    this.RobotExecuteBusy = false;
//                    EpsonErrLog.ErrorMess = "Not use Command\r\n";
//                    EpsonErrLog.ErrorCmd = "Login";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    this.RobotExecuteBusy = false;
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                this.RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message;
//                EpsonErrLog.ErrorCmd = "Login";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public bool Logout()
//        {
//            try
//            {
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "Logout";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    this.RobotExecuteBusy = true;
//                }
//                FeedBackMessageFromRobot = RobotSendCmd("$Logout,");
//                if (FeedBackMessageFromRobot.IndexOf("!") == -1)  //未找到
//                {
//                    this.RobotExecuteBusy = false;
//                    EpsonErrLog.ErrorMess = "Not use Command\r\n";
//                    EpsonErrLog.ErrorCmd = "Logout";
//                    ErrorDisPlay(this, new EventArgs());
//                    return false;
//                }
//                else
//                {
//                    this.RobotExecuteBusy = false;
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                this.RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message;
//                EpsonErrLog.ErrorCmd = "Login";
//                ErrorDisPlay(this, ea);
//                return false;
//            }

//        }

//        public bool StartMission(int NumberOfMission)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Start               功能编号               执行指定编号的功能                                Auto开/Ready开/Error关/EStop关/Safeguard开
//                //--------------------------------------------------------------

//                if (NumberOfMission < 0 | NumberOfMission > 7)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "StartMission";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "StartMission";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    this.RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    this.RobotExecuteBusy = false;
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "StartMission";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Start," + NumberOfMission);
//                }
//                else
//                {
//                    this.RobotExecuteBusy = false;
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "StartMission";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1)
//                {
//                    this.RobotExecuteBusy = false;

//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "StartMission";
//                    ErrorDisPlay(this, ea);
//                    this.RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                this.RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "StartMission";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }
//        public bool GetRobotStatus(ref string Status)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //GetStatus     获取控制器的状态                                  随时可用
//                //-----------------------------------------------------------------------
//                //GetStatus      #GetStatus,[状态],[错误,警告代码]终端
//                //               例如） #GetStatus,aaaaaaaaaa,bbbb
//                //                       *2     *3
//                //------------------------------------------------------------------------

//                FeedBackMessageFromRobot = RobotSendCmd("$GetStatus");

//                //#GetStatus,00100000001,0000
//                //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关
//                if (FeedBackMessageFromRobot.IndexOf("!") == -1)
//                {
//                    string[] TempStr;
//                    try
//                    {
//                        TempStr = Strings.Split(FeedBackMessageFromRobot, ",");
//                        if (TempStr.Length == 3)
//                        {
//                            Status = TempStr[1];
//                            return true;
//                        }
//                        else
//                        {
//                            EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                            EpsonErrLog.ErrorCmd = "GetRobotStatus";
//                            ErrorDisPlay(this, ea);
//                            Status = FeedBackMessageFromRobot;
//                            return false;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        this.RobotExecuteBusy = false;
//                        EpsonErrLog.ErrorMess = ex.Message; ;
//                        EpsonErrLog.ErrorCmd = "GetRobotStatus";
//                        ErrorDisPlay(this, ea);
//                        return false;
//                    }
//                }
//                else
//                {
//                    this.RobotExecuteBusy = false;
//                    Status = ProcessResponse(FeedBackMessageFromRobot);
//                    EpsonErrLog.ErrorMess = Status;
//                    EpsonErrLog.ErrorCmd = "GetRobotStatus";
//                    ErrorDisPlay(this, new EventArgs());
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                this.RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GetRobotStatus";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }


//        public string ProcessResponse(string ResponseString)
//        {
//            string TempResponse = "", TempStr = "";

//            try
//            {
//                //错误响应
//                //如果控制器不能正确接收远程命令,将以下列格式显示错误响应。
//                //格式：![远程命令],[错误代码]终端

//                if (Strings.InStr(ResponseString, "!") == -1)
//                {
//                    return ResponseString + "   " + "成功执行完远程命令";
//                }
//                else
//                {
//                    ushort ResponseCode = 0;
//                    string[] StrArray;
//                    StrArray = Strings.Split(ResponseString, ",");
//                    ResponseCode = Convert.ToUInt16(StrArray[1]);

//                    switch (ResponseCode)
//                    {
//                        case 10:
//                            //10              远程命令未以$开头
//                            TempResponse = ResponseString + "   " + "远程命令未以$开头";
//                            break;

//                        case 11:
//                            //11              远程命令错误 / 未执行Login
//                            TempResponse = ResponseString + "   " + "远程命令错误 / 未执行Login";
//                            break;

//                        case 12:
//                            //12              远程命令格式错误
//                            TempResponse = ResponseString + "   " + "远程命令格式错误";
//                            break;

//                        case 13:
//                            //13              Login命令密码错误
//                            TempResponse = ResponseString + "   " + "Login命令密码错误";
//                            break;

//                        case 14:
//                            //14              要获取的指定数量超出范围(小于1或大于100) / 忽略了要获取的数量 / 指定了一个字符串参数
//                            TempResponse = ResponseString + "   " + "要获取的指定数量超出范围(小于1或大于100) / 忽略了要获取的数量 / 指定了一个字符串参数";
//                            break;

//                        case 15:
//                            //15              参数不存在 / 参数尺寸错误 / 调用了超出了范围的元素
//                            TempResponse = ResponseString + "   " + "参数不存在 / 参数尺寸错误 / 调用了超出了范围的元素";
//                            break;

//                        case 19:
//                            //19              请求超时
//                            TempResponse = ResponseString + "   " + "请求超时";
//                            break;

//                        case 20:
//                            //20              控制器未准备好
//                            TempResponse = ResponseString + "   " + "控制器未准备好";
//                            break;

//                        case 21:
//                            //21              因为正在运行Execute,所以无法执行
//                            TempResponse = ResponseString + "   " + "因为正在运行Execute,所以无法执行";
//                            break;

//                        case 99:
//                            //99              系统错误 / 通信错误
//                            TempResponse = ResponseString + "   " + "系统错误 / 通信错误";
//                            break;

//                        default:
//                            TempResponse = ResponseString + "   " + "未知错误";
//                            break;
//                    }
//                }
//                TempStr = TempResponse;
//                return TempResponse;
//            }
//            catch (Exception ex)
//            {
//                this.RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message;
//                EpsonErrLog.ErrorCmd = "ProcessResponse";
//                ErrorDisPlay(this, ea);
//                return "";
//            }
//        }

//        public bool GetPointPos(string PointName, ref RobotPoint PointData)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令     Auto开/Ready开/Error关/EStop关/Safeguard关
//                //----------------------------------------------------

//                //如果PointName的第一个字母不是P或者数字大于999或者小于0
//                if (Strings.Mid(PointName.ToUpper(), 1, 1) != "P"
//                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) < 0
//                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) > 999))
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "GetPointPos";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetPointPos";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    this.RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetPointPos";
//                    ErrorDisPlay(this, ea);
//                    this.RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Print "
//                        + PointName + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetPointPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                //$Execute,"Print RealPos"
//                //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

//                //$execute,"print curpos"
//                //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {

//                    try
//                    {

//                        i = FeedBackMessageFromRobot.IndexOf(":", 0);
//                        PointData.X = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));


//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        PointData.Y = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));


//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        PointData.Z = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));


//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        PointData.U = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));


//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        PointData.V = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));


//                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
//                        PointData.W = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));


//                        //判断左右手势
//                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/L") != -1)
//                        {
//                            PointData.HandStyle = RobotHand.LeftHand;
//                        }

//                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/R") != -1)
//                        {
//                            PointData.HandStyle = RobotHand.RightHand;
//                        }

//                    }
//                    catch (Exception ex)
//                    {
//                        EpsonErrLog.ErrorMess = ex.Message; ;
//                        EpsonErrLog.ErrorCmd = "GetPointPos";
//                        ErrorDisPlay(this, ea);
//                        this.RobotExecuteBusy = false;
//                        return false;
//                    }

//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GetPointPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GetPointPos";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SetPointPos(string PointName, RobotPoint NewPointData)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//                //-----------------------------------------------------

//                //如果PointName的第一个字母不是P或者数字大于999或者小于0
//                if (Strings.Mid(PointName.ToUpper(), 1, 1) != "P"
//                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) < 0
//                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) > 999))
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SetPointPos";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SetPointPos";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SetPointPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                string TempStr = "";
//                if (NewPointData.HandStyle == RobotHand.LeftHand)
//                {
//                    TempStr = "/L";
//                }
//                else if (NewPointData.HandStyle == RobotHand.RightHand)
//                {
//                    TempStr = "/R";
//                }
//                else if (NewPointData.HandStyle == RobotHand.Unknow)
//                {
//                    TempStr = "";
//                }

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    // X: -156.600 Y:  274.958 Z:  -14.076 U:  -48.417 V:    0.000 W:    0.000 /L /0
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"" + PointName + "=XY(" + NewPointData.X.ToString() + ","
//                        + NewPointData.Y.ToString() + "," + NewPointData.Z.ToString() + "," + NewPointData.U.ToString() + "," + NewPointData.V.ToString()
//                        + "," + NewPointData.W.ToString() + ")" + TempStr + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SetPointPos";
//                    ErrorDisPlay(this, ea);
//                    this.RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SetPointPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SetPointPos";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public bool Jump(string PointName)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//                //----------------------------------------------------

//                //如果PointName的第一个字母不是P或者数字大于999或者小于0
//                if (Strings.Mid(PointName.ToUpper(), 1, 1) != "P"
//                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) < 0
//                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(), 2, PointName.Length - 1)) > 999))
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "Jump";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "Jump";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    this.RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "Jump";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Jump " + PointName + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "Jump";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "Jump";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "Jump";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SavePointPos()
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//                //----------------------------------------------------

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SavePointPos";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SavePointPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"SavePoints Points.pts" + "\"");
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"LoadPoints Points.pts" + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SavePointPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SavePointPos";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SavePointPos";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public bool SavePointPosWithSaveDialog()
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//                //----------------------------------------------------

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                SaveFileDialog TempSaveFile = new SaveFileDialog();
//                TempSaveFile.AddExtension = true;
//                TempSaveFile.CheckFileExists = true;
//                TempSaveFile.DefaultExt = "pts";
//                TempSaveFile.Filter = "EPSON点数据文件|*.pts";

//                if (TempSaveFile.ShowDialog() == DialogResult.OK)
//                {
//                    if (TempSaveFile.FileName != "")
//                    {
//                        //文件名称需要进行处理或者验证此方法是否可行
//                        int i;
//                        i = FeedBackMessageFromRobot.IndexOf(",");

//                        if (FeedBackMessageFromRobot[i + 3] == '1'
//                            & FeedBackMessageFromRobot[i + 11] == '1')
//                        {
//                            FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"SavePoints " + TempSaveFile.FileName + "\"");
//                            FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"LoadPoints " + TempSaveFile.FileName + "\"");
//                        }
//                        else
//                        {
//                            RobotExecuteBusy = false;
//                            return false;
//                        }
//                    }
//                    else
//                    {
//                        EpsonErrLog.ErrorMess = "file deil error\r\n";
//                        EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
//                        ErrorDisPlay(this, ea);
//                        RobotExecuteBusy = false;
//                        return false;
//                    }
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SavePointPosWithSaveDialog";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public bool ConnectEPSON()
//        {
//            if (ConnectRemoteEpsonTCPIPOk == false)
//            {
//                if (RobotCommucate.IsOpen() == true)
//                {
//                    EpsonErrLog.ErrorMess = "InterNet is Connect\r\n";
//                    EpsonErrLog.ErrorCmd = "ConnectEPSON";
//                    ErrorDisPlay(this, ea);
//                    ConnectRemoteEpsonTCPIPOk = true;
//                    return false;
//                }
//                if (RobotCommucate.Open() == false)
//                {
//                    EpsonErrLog.ErrorMess = "InterNet is Connect failure\r\n";
//                    EpsonErrLog.ErrorCmd = "ConnectEPSON";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                ConnectRemoteEpsonTCPIPOk = true;
//                if (Login(RobotInfo1.PassWard) == false)
//                {
//                    RobotCommucate.Close();
//                    ConnectRemoteEpsonTCPIPOk = false;
//                    return false;
//                }
//                else
//                {
//                    ConnectRemoteEpsonTCPIPOk = true;
//                    return true;
//                }

//            }
//            else
//            {
//                EpsonErrLog.ErrorMess = "InterNet is Connect 1\r\n";
//                EpsonErrLog.ErrorCmd = "ConnectEPSON";
//                ErrorDisPlay(this, ea);
//                ConnectRemoteEpsonTCPIPOk = false;
//                return false;
//            }
//        }

//        public bool GetVariable(ref string VariableName, ref string[] Value)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //GetVariable         参数名称{, type}       获取备份（Global Preserve）参数的值               随时可用
//                //                    ***********************************************************
//                //                    (参数名称)（数组元
//                //                    素）,(参数名称类
//                //                    型),(获取的编号)      获取备份（Global Preserve）数组参数的值
//                //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String}。
//                //指定的类型： 在参数名称和类型相同时用于备份参数。
//                //未指定的类型： 在参数名称相同时用于备份参数。
//                //(*9) 对于数组元素,指定以下您想获取的元素：
//                //如果是从数组头处获取的,您需要指定一个元素。
//                //1维数组 参数名称 (0) 从头部获取。
//                //参数名称（元素编号） 从指定的元素编号中获取。
//                //2维数组 参数名称 (0,0) 从头部获取。
//                //参数名称（元素编号1,2） 从指定的元素编号中获取。
//                //3维数组 参数名称 (0,0,0) 从头部获取。
//                //参数名称（元素编号1,2,3） 从指定的元素编号中获取。
//                //您不能忽略要获取的参数类型和编号。
//                //您不能指定一个参数类型string。
//                //可获取的可用数量多达100个。如果您在数组元素编号上指定一个号码,会发生错误。
//                //如）“$GetVariable,gby2(3,0),Byte,3”
//                //它获得字节型2维数组参数gby2的gby2(3,0)、gby2(3,1)、gby2(3,2)的值。
//                //--------------------------------------------------------------------------------

//                if (VariableName == "")
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "GetVariable";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetVariable";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetVariable";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$GetVariable," + VariableName);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetVariable";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    try
//                    {
//                        //GetVariable                    #GetVariable,[参数值] 终端
//                        //---------------------------------------------------------------------------------
//                        //GetVariable（如果是数组）      #GetVariable,[参数值 1],[参数值 2],...,终端 *4
//                        //*4 返回要获取的编号中指定编号的值。

//                        string[] TempStr = FeedBackMessageFromRobot.Split(',');

//                        if (TempStr[0] != "#GetVariable")
//                        {
//                            EpsonErrLog.ErrorMess = "return error\r\n";
//                            EpsonErrLog.ErrorCmd = "GetVariable";
//                            ErrorDisPlay(this, ea);
//                            Value = null;
//                            return false;
//                        }

//                        if (TempStr.Length == 2)
//                        {
//                            Value = new string[1];
//                            Value[0] = TempStr[1];
//                        }
//                        else
//                        {
//                            Value = new string[TempStr.Length];
//                            for (int a = 0; a < TempStr.Length; a++)
//                            {
//                                Value[a] = TempStr[a];
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        EpsonErrLog.ErrorMess = ex.Message; ;
//                        EpsonErrLog.ErrorCmd = "GetVariable";
//                        ErrorDisPlay(this, ea);
//                        RobotExecuteBusy = false;
//                        //AddText(ex.Message);
//                        return false;
//                    }

//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GetVariable";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GetVariable";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SetVariable(string VariableName, string Value, RobotVariable VariableType)
//        {
//            try
//            {

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //SetVariable        参数名称和值{,类型}    设置备份（Global Preserve）参数中的值             Auto开/Ready开
//                //---------------------------------------------------------------------------------------------------
//                //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String | Short | UByte | UShort | Int32 |
//                //UInt32 | Int64 | UInt64}。
//                //指定类型：在参数名称和类型相同时用于备份参数。
//                //未指定类型：在参数名称相同时用于备份参数。

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SetVariable";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SetVariable";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$SetVariable,"
//                        + VariableName + "," + Value + "," +
//                        Strings.Mid(VariableType.ToString(), 3,
//                        VariableType.ToString().Length - 2));
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SetVariable";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SetVariable";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SetVariable";
//                ErrorDisPlay(this, ea);
//                return false;

//            }
//        }

//        public bool GetToolSetting(int NumberOfTool)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//                //----------------------------------------------------

//                //TLSet
//                //用于设置/显示工具坐标系。

//                //格式
//                //(1) TLSet 工具坐标系编号, 工具设置数据
//                //(2) TLSet 工具坐标系编号
//                //(3) TLSet

//                //参数
//                //工具坐标系编号 以1～15 的整数值指定要设置的工具。（Tool 0 为默认工具,不能变更。）
//                //工具设置数据 以P 编号、P（表达式）、点标签点或表达式指定要设置的工具坐标系的原点和
//                //方向。

//                //结果
//                //如果省略所有参数,则显示所有的TLSet 设置。
//                //如果只指定工具编号,则显示指定的TLSet 设置。

//                //说明
//                //指定针对Tool 0 坐标系（夹具末端坐标系）的相对原点位置和相对旋转角度,定义工具坐标系
//                //Tool 1、Tool 2 、Tool 3。
//                //        TLSet(1, XY(50, 100, -20, 30)) ----  【X,Y,Z,U】
//                //TLSet(2, P10 + X(20))
//                //上述情况时,引用坐标值P10 并在X 值上加上20。无视机械臂属性和本地坐标系编号。
//                //ツール座標系の原点のZ(軸方向位置)
//                //ツール座標系の原点のY(軸方向位置(次図b))
//                //ツール座標系の原点のX(軸方向位置(次図a))
//                //ツール座標系番号
//                //TLSET(1, XY(100, 60, -20, 30))

//                if (NumberOfTool < 1 | NumberOfTool > 15)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "GetToolSetting";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetToolSetting";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetToolSetting";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"print TLSET(" + NumberOfTool + ")\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetToolSetting";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {

//                    // 需要得到返回值得样本再进行处理
//                    // $Execute,"tlset 1"
//                    // !Execute, 99
//                    // $Execute,"tlset"
//                    // #Execute,0
//                    // $Execute,"print tlset"
//                    // !Execute, 99
//                    // $Execute,"print tlset 0"
//                    // !Execute, 99
//                    // $Execute,"print tlset 1"
//                    // !Execute, 99
//                    // $Execute,"tlset 1"
//                    // !Execute, 99
//                    // i = FeedBackMessageFromRobot.IndexOf(":", 0)
//                    // ToolX = FeedBackMessageFromRobot.Substring(i + 1, 10)
//                    // i = FeedBackMessageFromRobot.IndexOf(":", i + 1)
//                    // ToolY = FeedBackMessageFromRobot.Substring(i + 1, 10)

//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GetToolSetting";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GetToolSetting";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SetTool(int NumberOfTool, double X, double Y, double Z, double U)
//        {
//            try
//            {

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//                //----------------------------------------------------

//                //TLSet
//                //用于设置/显示工具坐标系。

//                //格式
//                //(1) TLSet 工具坐标系编号, 工具设置数据
//                //(2) TLSet 工具坐标系编号
//                //(3) TLSet

//                //参数
//                //工具坐标系编号 以1～15 的整数值指定要设置的工具。（Tool 0 为默认工具,不能变更。）
//                //工具设置数据 以P 编号、P（表达式）、点标签点或表达式指定要设置的工具坐标系的原点和
//                //方向。

//                //结果
//                //如果省略所有参数,则显示所有的TLSet 设置。
//                //如果只指定工具编号,则显示指定的TLSet 设置。

//                //说明
//                //指定针对Tool 0 坐标系（夹具末端坐标系）的相对原点位置和相对旋转角度,定义工具坐标系
//                //Tool 1、Tool 2 、Tool 3。
//                //        TLSet(1, XY(50, 100, -20, 30)) ----  【X,Y,Z,U】
//                //TLSet(2, P10 + X(20))
//                //上述情况时,引用坐标值P10 并在X 值上加上20。无视机械臂属性和本地坐标系编号。
//                //ツール座標系の原点のZ(軸方向位置)
//                //ツール座標系の原点のY(軸方向位置(次図b))
//                //ツール座標系の原点のX(軸方向位置(次図a))
//                //ツール座標系番号
//                //TLSET(1, XY(100, 60, -20, 30))

//                if (NumberOfTool < 1 | NumberOfTool > 15)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SetTool";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SetTool";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SetTool";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                string strToolSetting = "";
//                strToolSetting = "XY(" + X.ToString() + "," + Y.ToString() + "," +
//                    Z.ToString() + "," + U.ToString() + ")";

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"TLSET(" + NumberOfTool
//                        + "," + strToolSetting + ")\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SetTool";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SetTool";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SetTool";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public bool Abort()
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Abort     中止命令的执行       Auto开
//                //-------------------------------------

//                //此处是否不需要考虑其它命令正在执行，待验证
//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "Abort";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "Abort";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Abort");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "Abort";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {

//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "Abort";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "Abort";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SetPowerMode(RobotPower PowerMode)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SetPowerMode";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SetPowerMode";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //Power
//                //用于将功率模式设为High 或Low，并显示当前的模式。
//                //格式
//                //(1) Power { High | Low }
//                //(2) Power
//                //参数
//                //High | Low 设置High 或Low。默认设置为Low。
//                //结果
//                //如果省略参数，则显示当前的功率模式。
//                //说明
//                //用于将功率模式设为High 或Low。另外，显示当前的功率模式。
//                //Low ： 如果将功率模式设为Low，低功率模式则会变为ON 状态。这表示机械手缓慢地（250mm/sec
//                //以下的速度）进行动作。另外，将电动机功率输出限制在较低水平。
//                //High ： 如果将功率模式设为High，低功率模式则会变为OFF状态。这表示机械手以由Speed、Accel、
//                //SpeedS、AccelS 指定的速度、加减速度进行动作。

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    if (PowerMode == RobotPower.HIGHPOWER)
//                    {
//                        FeedBackMessageFromRobot = RobotSendCmd("$Execute, \"Power High\"");
//                    }
//                    else if (PowerMode == RobotPower.LOWPOWER)
//                    {
//                        FeedBackMessageFromRobot = RobotSendCmd("$Execute, \"Power Low\"");
//                    }
//                    else
//                    {
//                        RobotExecuteBusy = false;
//                        return false;
//                    }
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SetPowerMode";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SetPowerMode";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SetPowerMode";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SLock(int TargetAxis)
//        {
//            try
//            {

//                if (TargetAxis < 0 | TargetAxis > 6)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SLock";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SLock";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SLock";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //SLock
//                //用于解除指定关节的SFree，并重新开始电动机励磁。
//                //格式
//                //SLock 关节编号 [关节编号,...]
//                //参数
//                //关节编号 以表达式或数值指定关节编号（1～9 的整数）。
//                //附加轴的S 轴为8，T 轴为9。
//                //说明
//                //SLock 用于重新开始对因SFree 命令而进入SFree 状态的关节的电动机进行励磁，以便进行直接示教
//                //或安装工件等。
//                //如果省略关节编号，则重新开始对所有关节的电动机进行励磁。
//                //如果对第3 关节重新进行励磁，电磁制动器则会被解除。
//                //可替代SLock，使用Motor On 进行所有关节的励磁。
//                //如果在Motor Off 状态下执行SLock，则会发生错误。
//                //如果执行SLock 命令，则会对机械手控制参数进行初始化。

//                //SLock 1, 2 '对J1 和J2 进行励磁

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"SLock " +
//                        TargetAxis + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SLock";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SLock";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SLock";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SLockAll(int AxisQty)
//        {
//            try
//            {

//                if (AxisQty != 4 & AxisQty != 6)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SLockAll";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                bool TempResult = true;
//                for (ushort a = 1; a <= AxisQty; a++)
//                {
//                    TempResult &= SLock(a);
//                }

//                if (TempResult == true)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SLockAll";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SFree(int TargetAxis)
//        {
//            try
//            {

//                if (TargetAxis < 0 | TargetAxis > 6)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SFree";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SFree";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SFree";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //SFree
//                //用于切断指定关节的电动机电源。
//                //格式
//                //SFree 关节编号[, 关节编号,...]
//                //参数
//                //关节编号 以表达式或数值指定关节编号（1～9 的整数）。
//                //附加轴的S 轴为8，T 轴为9。
//                //说明
//                //SFree 用于切断指定关节的电动机电源。此时的状态称为SFree。该命令用于进行直接示教，或仅切
//                //断特定关节的励磁进行嵌入等。要再次对该关节进行励磁时，执行SLock 命令或Motor On。
//                //如果执行SFree 命令，则会对机械手控制参数进行初始化。
//                //详情请参阅Motor On。
//                //注意
//                //执行SFree 时，部分系统设置会被初始化
//                //SFree 用于对有关机械手动作速度或加减速度的参数（Speed、SpeedS、Accel、AccelS）和LimZ 参
//                //数进行初始化，以确保安全。

//                //SFree 1, 2 '将J1 和J2 设为非励磁状态，然后移动Z 和U 关节以安装部件

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"SFree " + TargetAxis + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SFree";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SFree";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SFree";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SFreeAll(int AxisQty)
//        {
//            try
//            {

//                if (AxisQty != 4 & AxisQty != 6)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SFreeAll";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                bool TempResult = true;
//                for (ushort a = 1; a <= AxisQty; a++)
//                {
//                    TempResult &= SFree(a);
//                }

//                if (TempResult == true)
//                {
//                    //ExecutingBusy = false;
//                    return true;
//                }
//                else
//                {
//                    //ExecutingBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SFreeAll";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool ResetRobot()
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //           Reset      清除紧急停止和错误            Auto 开/Ready 开
//                //           -------------------------------------------------------

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "ResetRobot";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "ResetRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Reset");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "ResetRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "ResetRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "ResetRobot";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        private double HexToDecimal(string TargetHex, string strErrorLog)
//        {
//            if (PasswordIsCorrect == false
//                || SuccessBuiltNew == false)
//            {
//                return 0;
//            }

//            //十六----> 十 
//            //（19.A）（十六）            
//            //整数部分:
//            //1*16（1）+9*16（0）=25 
//            //小数部分:
//            //10*16（-1）=0.625 
//            //所以(19.A)(十六) = (25.625)(十)

//            try
//            {
//                TargetHex = TargetHex.ToUpper();
//                double TempInt = 0.0;

//                //检查字符串是否为0~9,A~F
//                for (int a = 0; a < TargetHex.Length; a++)
//                {
//                    if (TargetHex[a] != '0' & TargetHex[a] != '1' &
//                        TargetHex[a] != '2' & TargetHex[a] != '3' &
//                        TargetHex[a] != '4' & TargetHex[a] != '5' &
//                        TargetHex[a] != '6' & TargetHex[a] != '7' &
//                        TargetHex[a] != '8' & TargetHex[a] != '9' &
//                        TargetHex[a] != 'A' & TargetHex[a] != 'B' &
//                        TargetHex[a] != 'C' & TargetHex[a] != 'D' &
//                        TargetHex[a] != 'E' & TargetHex[a] != 'F')
//                    {
//                        strErrorLog = "The value for parameter 'TargetHex' should be 0~9 and A~F.";
//                        return -1;
//                    }
//                    else if (TargetHex[a] != '.')
//                    {
//                        TempInt += 1;
//                    }
//                }

//                //只能有一个点号
//                if (TempInt > 1)
//                {
//                    strErrorLog = "The parameter 'TargetHex' has " + TempInt + ".('dots'), invalid operation.";
//                    return -1;
//                }

//                TempInt = 0;

//                //判断是否有小数点
//                if (TargetHex.IndexOf(".") == -1)
//                {
//                    //无小数点号
//                    //计算转换16进制
//                    for (int a = 0; a < TargetHex.Length; a++)
//                    {
//                        switch (TargetHex[a])
//                        {
//                            case '0':
//                                TempInt += 0;
//                                break;

//                            case '1':
//                            case '2':
//                            case '3':
//                            case '4':
//                            case '5':
//                            case '6':
//                            case '7':
//                            case '8':
//                            case '9':
//                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'A':
//                                TempInt += 10 * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'B':
//                                TempInt += 11 * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'C':
//                                TempInt += 12 * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'D':
//                                TempInt += 13 * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'E':
//                                TempInt += 14 * (Math.Pow(16, (TargetHex.Length - a - 1)));
//                                break;

//                            case 'F':
//                                TempInt += 15 * (Math.Pow(16, (TargetHex.Length - a - 1)));
//                                break;
//                        }
//                    }
//                }
//                else
//                {
//                    //有小数点号
//                    //先计算转换16进制整数部分：
//                    string TempStr = Strings.Mid(TargetHex, 1, TargetHex.IndexOf("."));

//                    for (int a = 0; a < TempStr.Length; a++)
//                    {

//                        switch (TempStr[a])
//                        {

//                            case '0':
//                                TempInt += 0;
//                                break;

//                            case '1':
//                            case '2':
//                            case '3':
//                            case '4':
//                            case '5':
//                            case '6':
//                            case '7':
//                            case '8':
//                            case '9':
//                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'A':
//                                TempInt += 10 * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'B':
//                                TempInt += 11 * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'C':
//                                TempInt += 12 * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'D':
//                                TempInt += 13 * Math.Pow(16, (TargetHex.Length - a - 1));
//                                break;

//                            case 'E':
//                                TempInt += 14 * (Math.Pow(16, (TargetHex.Length - a - 1)));
//                                break;

//                            case 'F':
//                                TempInt += 15 * (Math.Pow(16, (TargetHex.Length - a - 1)));
//                                break;
//                        }
//                    }

//                    //再计算转换16进制小数部分：
//                    TempStr = Strings.Mid(TargetHex, TargetHex.IndexOf(".") + 2, TargetHex.Length - TargetHex.IndexOf(".") - 1);

//                    for (int a = 0; a < TempStr.Length; a++)
//                    {

//                        switch (TempStr[a])
//                        {

//                            case '0':
//                                TempInt += 0;
//                                break;

//                            case '1':
//                            case '2':
//                            case '3':
//                            case '4':
//                            case '5':
//                            case '6':
//                            case '7':
//                            case '8':
//                            case '9':
//                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, -(a + 1));
//                                break;

//                            case 'A':
//                                TempInt += 10 * Math.Pow(16, -(a + 1));
//                                break;

//                            case 'B':
//                                TempInt += 11 * Math.Pow(16, -(a + 1));
//                                break;

//                            case 'C':
//                                TempInt += 12 * Math.Pow(16, -(a + 1));
//                                break;

//                            case 'D':
//                                TempInt += 13 * Math.Pow(16, -(a + 1));
//                                break;

//                            case 'E':
//                                TempInt += 14 * Math.Pow(16, -(a + 1));
//                                break;

//                            case 'F':
//                                TempInt += 15 * Math.Pow(16, -(a + 1));
//                                break;
//                        }
//                    }
//                }
//                return TempInt;

//            }
//            catch (Exception ex)
//            {
//                strErrorLog = ex.Message;
//                return -1;
//            }
//        }
//        public bool SetSpeed(int TargetSpeed)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                if (TargetSpeed > 100 | TargetSpeed < 1)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SetSpeed";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SetSpeed";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SetSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //Speed
//                //用于设置/显示利用Go、Jump、Pulse 命令等的PTP 动作速度。
//                //格式
//                //(1) Speed 速度设定值 [, 转移速度, 接近速度]
//                //(2) Speed
//                //参数
//                //速度设定值 以表达式或数值指定相对于最大动作速度（PTP 动作）的比例（1～100 的整数，单
//                //位：%）。
//                //转移速度 以表达式或数值指定Jump 命令时的转移动作速度（1～100 的整数，单位：%）。
//                //可省略。仅Jump 命令时可设置。
//                //接近速度 以表达式或数值指定Jump 命令时的接近动作速度（1～100 的整数，单位：%）。
//                //可省略。仅Jump 命令时可设置。
//                //结果
//                //如果省略参数，则显示当前的Speed 设定值。
//                //说明
//                //Speed 用于指定所有PTP 动作命令的速度。其中包括有关Go、Jump、Pulse 等动作命令的速度设置。
//                //速度设置是指以1～100 的整数指定相对于最大速度的比例（%）。如果指定“100”，则以最大速度进
//                //行动作。

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Speed " + TargetSpeed + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SetSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SetSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SetSpeed";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }
//        public RobotStatusBits NewProcessStatusCode(string StatusCode)
//        {
//            RobotStatusBits TempStatus = new RobotStatusBits();
//            TempStatus.Auto = false;
//            TempStatus.Error = false;
//            TempStatus.EStop = false;
//            TempStatus.Paused = false;
//            TempStatus.Ready = false;
//            TempStatus.Running = false;
//            TempStatus.Safeguard = false;
//            TempStatus.SError = false;
//            TempStatus.Teach = false;
//            TempStatus.Test = false;
//            TempStatus.Warning = false;

//            try
//            {

//                //*3 错误/警告代码
//                //以 4 位数字表示。如果没有错误和警告,则为 0000。

//                //例如）1： #GetStatus,0100000001,0000
//                //Auto 位和 Ready 位为开（1）。
//                //表示自动模式开启并处于准备就绪状态。已启用命令执行。

//                //例如）2： #GetStatus,0110000010,0517
//                //这意味着运行过程中发生警告。对警告代码采取适当的行动。（在这种情况下,警告代码为 0517）

//                //标志(内容)
//                //----------------------------------------------------------------------------------------
//                //Test(在TEST模式下打开)
//                //----------------------------------------------------------------------------------------
//                //Teach(在TEACH模式下打开)
//                //----------------------------------------------------------------------------------------
//                //Auto(在远程输入接受条件下打开)
//                //----------------------------------------------------------------------------------------
//                //Warnig(在警告条件下打开)
//                //                甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
//                //----------------------------------------------------------------------------------------
//                //SError(在严重错误状态下打开)
//                //                发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
//                //----------------------------------------------------------------------------------------
//                //Safeguard(安全门打开时打开)
//                //----------------------------------------------------------------------------------------
//                //EStop(在紧急状态下打开)
//                //----------------------------------------------------------------------------------------
//                //Error 在错误状态下打开
//                //                使用“Reset 输入”从错误状态中恢复。
//                //----------------------------------------------------------------------------------------
//                //Paused(打开暂停的任务)
//                //----------------------------------------------------------------------------------------
//                //Running(执行任务时打开)
//                //                在“Paused 输出”为开时关闭。
//                //----------------------------------------------------------------------------------------
//                //Ready(控制器完成启动且无任务执行时打开)
//                //----------------------------------------------------------------------------------------

//                if (StatusCode.Length != 11)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "NewProcessStatusCode";
//                    ErrorDisPlay(this, ea);
//                    return TempStatus;
//                }

//                //bool TempCheck=false;
//                for (int a = 0; a < StatusCode.Length; a++)
//                {
//                    if (StatusCode[a] != '0' & StatusCode[a] != '1')
//                    {
//                        EpsonErrLog.ErrorMess = "Status error\r\n";
//                        EpsonErrLog.ErrorCmd = "NewProcessStatusCode";
//                        ErrorDisPlay(this, ea);
//                        return TempStatus;
//                    }

//                }


//                //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关

//                //string TempStr="";
//                //0 - Test            在TEST模式下打开
//                if (StatusCode[0] == '0')
//                {
//                    TempStatus.Test = false;
//                }
//                else if (StatusCode[0] == '1')
//                {
//                    TempStatus.Test = true;
//                }

//                //1 - Teach           在TEACH模式下打开
//                if (StatusCode[1] == '0')
//                {
//                    TempStatus.Teach = false;
//                }
//                else if (StatusCode[1] == '1')
//                {
//                    TempStatus.Teach = true;
//                }

//                //2 - Auto            在远程输入接受条件下打开
//                if (StatusCode[2] == '0')
//                {
//                    TempStatus.Auto = false;
//                }
//                else if (StatusCode[2] == '1')
//                {
//                    TempStatus.Auto = true;
//                }

//                //3 - Warnig 在警告条件下打开,甚至在警告条件下也可以像往常一样执行任务。
//                //但是,应尽快采取警告行动。
//                if (StatusCode[3] == '0')
//                {
//                    TempStatus.Warning = false;
//                }
//                else if (StatusCode[3] == '1')
//                {
//                    TempStatus.Warning = true;
//                }

//                //4 - SError   在严重错误状态下打开,发生严重错误时,重新启动控制器,
//                //以便从错误状态中恢复。“Reset 输入”不可用。
//                if (StatusCode[4] == '0')
//                {
//                    TempStatus.SError = false;
//                }
//                else if (StatusCode[4] == '1')
//                {
//                    TempStatus.SError = true;
//                }

//                //5 - Safeguard       安全门打开时打开
//                if (StatusCode[5] == '0')
//                {
//                    TempStatus.Safeguard = false;
//                }
//                else if (StatusCode[5] == '1')
//                {
//                    TempStatus.Safeguard = true;
//                }

//                //6 - EStop           在紧急状态下打开
//                if (StatusCode[6] == '0')
//                {
//                    TempStatus.EStop = false;
//                }
//                else if (StatusCode[6] == '1')
//                {
//                    TempStatus.EStop = true;
//                }

//                //7 - Error           在错误状态下打开,使用“Reset 输入”从错误状态中恢复。
//                if (StatusCode[7] == '0')
//                {
//                    TempStatus.Error = false;
//                }
//                else if (StatusCode[7] == '1')
//                {
//                    TempStatus.Error = true;
//                }

//                //8 - Paused          打开暂停的任务
//                if (StatusCode[8] == '0')
//                {
//                    TempStatus.Paused = false;
//                }
//                else if (StatusCode[8] == '1')
//                {
//                    TempStatus.Paused = true;
//                }

//                //9 - Running         执行任务时打开,在“Paused 输出”为开时关闭。
//                if (StatusCode[9] == '0')
//                {
//                    TempStatus.Running = false;
//                }
//                else if (StatusCode[9] == '1')
//                {
//                    TempStatus.Running = true;
//                }

//                //10 - Ready           控制器完成启动且无任务执行时打开
//                if (StatusCode[10] == '0')
//                {
//                    TempStatus.Ready = false;
//                }
//                else if (StatusCode[10] == '1')
//                {
//                    TempStatus.Ready = true;
//                }

//                return TempStatus;

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "NewProcessStatusCode";
//                ErrorDisPlay(this, ea);
//                return TempStatus;
//                //MessageBox.Show(ex.Message);
//            }
//        }
//        /// <summary>
//        /// 释放资源
//        /// </summary>
//        public void Dispose()
//        {
//            try
//            {
//                if (RobotCommucate != null)
//                {
//                    RobotCommucate.Close();
//                    RobotCommucate = null;
//                }
//                GC.Collect();

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "Dispose";
//                ErrorDisPlay(this, ea);
//            }
//        }

//        public bool SetCurrentRobot(int NumberOfRobot)
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //SetCurRobot         机械手编号             选择机械手                                        Auto开/Ready开
//                //-----------------------------------------------------

//                if (NumberOfRobot < 0 | NumberOfRobot > 16)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$SetCurRobot," + NumberOfRobot);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SetCurrentRobot";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public int GetCurrentRobot()
//        {
//            try
//            {
//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //GetCurRobot      获取当前的机械手编号              随时可用
//                //--------------------------------------------------------

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    return -1;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$GetCurRobot");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    //#[远程命令],[0]终端
//                    //$GetCurRobot
//                    //#GetCurRobot,1

//                    string[] TempStr;
//                    TempStr = FeedBackMessageFromRobot.Split(',');

//                    if (TempStr[0] != "#GetCurRobot")
//                    {
//                        RobotExecuteBusy = false;
//                        return -1;
//                    }

//                    RobotExecuteBusy = false;
//                    return Convert.ToInt32(TempStr[1]);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GetCurrentRobot";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GetCurrentRobot";
//                ErrorDisPlay(this, ea);
//                return -1;
//                //MessageBox.Show(ex.Message);
//            }
//        }

//        public bool SetACCELSpeed(ushort TargetAccelSpeed, ushort TargetDecelSpeed)
//        {
//            try
//            {

//                if (TargetAccelSpeed < 1 | TargetAccelSpeed > 100)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //Accel
//                //用于设置和显示利用Go、Jump、Pulse 等的PTP 动作的加减速度。
//                //格式
//                //(1) Accel 加速设定值, 减速设定值, [转移加速设定值, 转移减速设定值, 接近加速设定值, 接近减
//                //速设定值]
//                //(2) Accel
//                //参数
//                //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
//                //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）
//                //转移加速设定值 以大于1 的整数指定Jump 时的转移加速度。
//                //可省略。仅Jump 命令时可设置。
//                //转移减速设定值 以大于1 的整数指定Jump 时的转移减速度。
//                //可省略。仅Jump 命令时可设置。
//                //接近加速设定值 以大于1 的整数指定Jump 时的接近加速度。
//                //可省略。仅Jump 命令时可设置。
//                //接近减速设定值 以大于1 的整数指定Jump 时的接近减速度。
//                //可省略。仅Jump 命令时可设置。
//                //结果
//                //如果省略参数，将返回当前的Accel 参数。

//                //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
//                //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute\"Accel " +
//                        TargetAccelSpeed + "," + TargetDecelSpeed);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SetACCELSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SetACCELSpeed";
//                ErrorDisPlay(this, ea);
//                return false;
//                //MessageBox.Show(ex.Message);
//            }
//        }
//        /// <summary>
//        /// 获取当前速度
//        /// </summary>
//        /// <returns></returns>
//        public int GetSpeed()
//        {
//            try
//            {

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetSpeed";
//                    ErrorDisPlay(this, ea);
//                    return -1;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //Speed
//                //用于设置/显示利用Go、Jump、Pulse 命令等的PTP 动作速度。
//                //格式
//                //(2) Speed
//                //如果省略参数，则显示当前的Speed 设定值。
//                //说明
//                //Speed 用于指定所有PTP 动作命令的速度。其中包括有关Go、Jump、Pulse 等动作命令的速度设置。
//                //速度设置是指以1～100 的整数指定相对于最大速度的比例（%）。如果指定“100”，则以最大速度进
//                //行动作。

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Speed" + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {

//                    //$Execute,"Speed"
//                    //#Execute,"Low Power Mode
//                    //1
//                    //1	   1

//                    string[] TempStr = Strings.Split(FeedBackMessageFromRobot, "\r\n");

//                    if (TempStr.Length != 3)//(TempStr[0]!="#Execute")
//                    {
//                        RobotExecuteBusy = false;
//                        return -1;
//                    }

//                    RobotExecuteBusy = false;
//                    return Convert.ToUInt16(TempStr[1]);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GetSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return -1;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GetSpeed";
//                ErrorDisPlay(this, ea);
//                return -1;
//                //MessageBox.Show(ex.Message);
//            }
//        }
//        /// <summary>
//        /// 获取加速度
//        /// </summary>
//        /// <returns></returns>
//        public RobotAccel GetAccelSpeed()
//        {
//            RobotAccel TempAccel;
//            TempAccel.AccelSpeed = -1;
//            TempAccel.DecelSpeed = -1;

//            try
//            {

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetAccelSpeed";
//                    ErrorDisPlay(this, ea);
//                    return TempAccel;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetAccelSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return TempAccel;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //Accel
//                //用于设置和显示利用Go、Jump、Pulse 等的PTP 动作的加减速度。
//                //格式
//                //(2) Accel
//                //如果省略参数，将返回当前的Accel 参数

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Accel" + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetAccelSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return TempAccel;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
//                    //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）
//                    //$Execute,"Accel"
//                    //#Execute,"Low Power Mode
//                    //10	  10
//                    //10	  10
//                    //10	  10

//                    //添加返回结果值的代码，需要测试返回结果才能处理【需要验证】
//                    string[] TempStr;//,TempStr2;
//                    TempStr = Strings.Split(FeedBackMessageFromRobot, "\r\n");
//                    //TempStr2=TempStr[1].Split(',');
//                    //if(TempStr2[0]!="#Execute")
//                    //    {
//                    //    ExecutingBusy = false;                        
//                    //    return TempAccel;
//                    //    }

//                    if (TempStr.Length < 4)
//                    {
//                        RobotExecuteBusy = false;
//                        return TempAccel;
//                    }

//                    TempStr = TempStr[1].Split(' ');

//                    TempAccel.AccelSpeed = Convert.ToInt32(TempStr[0]);
//                    TempAccel.DecelSpeed = Convert.ToInt32(TempStr[1]);

//                    RobotExecuteBusy = false;
//                    return TempAccel;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "GetAccelSpeed";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return TempAccel;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "GetAccelSpeed";
//                ErrorDisPlay(this, ea);
//                return TempAccel;
//                //MessageBox.Show(ex.Message);
//            }
//        }
//        /// <summary>
//        /// 获取当前功率模式
//        /// </summary>
//        /// <returns></returns>
//        public RobotPower GetPowerStatus()
//        {
//            RobotPower TempPower = RobotPower.UnKonw;

//            try
//            {

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "GetPowerStatus";
//                    ErrorDisPlay(this, ea);
//                    return TempPower;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "GetPowerStatus";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return TempPower;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //Power
//                //用于将功率模式设为High 或Low，并显示当前的模式。
//                //格式
//                //(2) Power
//                //参数
//                //High | Low 设置High 或Low。默认设置为Low。
//                //结果
//                //如果省略参数，则显示当前的功率模式。

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Speed" + "\"");
//                    //FeedBackMessageFromRobot = SendCommand("$Execute,\"Power" + "\"" + Suffix);
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "GetPowerStatus";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return TempPower;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    //$Execute,"Power"
//                    //!Execute,99

//                    //$Execute,"Speed"
//                    //#Execute,"Low Power Mode
//                    //1
//                    //1	   1

//                    //**************
//                    //注意：用Split(\r\n)之后，其它的前面都有\r\n,所有要去掉这个才能得到正确值【需要验证】
//                    //**************

//                    string[] TempStr;
//                    string TempRet;
//                    TempStr = Strings.Split(FeedBackMessageFromRobot, "\r\n");
//                    TempRet = TempStr[0].ToUpper();

//                    if (Strings.InStr(TempRet, "LOW") != -1)
//                    {
//                        TempPower = RobotPower.LOWPOWER;
//                    }
//                    else if (Strings.InStr(TempRet, "HIGH") != -1)
//                    {
//                        TempPower = RobotPower.HIGHPOWER;
//                    }
//                    else
//                    {
//                        TempPower = RobotPower.UnKonw;
//                    }

//                    RobotExecuteBusy = false;
//                    return TempPower;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "AbsMove";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return TempPower;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "AbsMove";
//                ErrorDisPlay(this, ea);
//                return TempPower;
//                //MessageBox.Show(ex.Message);
//            }
//        }
//        /// <summary>
//        /// 设置原点坐标
//        /// </summary>
//        /// <param name="HomePoint"></param>
//        /// <returns></returns>
//        public bool SetHome(RobotPoint HomePoint)
//        {
//            try
//            {
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "SetHome";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "SetHome";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"homeset " + HomePoint.X.ToString() + ","
//                        + HomePoint.Y.ToString() + "," + HomePoint.Z.ToString() + "," + HomePoint.U.ToString() + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "SetHome";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "SetHome";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "SetHome";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }
//        public bool Loacl(int LocalNo, RobotPoint LocalPoint)
//        {
//            try
//            {
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "Loacl";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "Loacl";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Local " + LocalNo + "," + "XY(" + LocalPoint.X.ToString() + ","
//                        + LocalPoint.Y.ToString() + "," + LocalPoint.Z.ToString() + "," + LocalPoint.U.ToString() + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "Loacl";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "Loacl";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "Loacl";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }

//        public bool Base(RobotPoint BasePoint)
//        {
//            try
//            {

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
//                //----------------------------------------------------

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "Base";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "Base";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                if (FeedBackMessageFromRobot[i + 3] == '1'
//                    & FeedBackMessageFromRobot[i + 11] == '1')
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Base XY(" + BasePoint.X.ToString() + ","
//                        + BasePoint.Y.ToString() + "," + BasePoint.Z.ToString() + "," + BasePoint.U.ToString() + ")" + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "Base";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "Base";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                RobotExecuteBusy = false;
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "Base";
//                ErrorDisPlay(this, ea);
//                return false;
//            }
//        }
//        /// <summary>
//        /// 松开刹车功能
//        /// </summary>
//        /// <param name="TargetAxis"></param>
//        /// <returns></returns>
//        public bool StopAxisOff(int TargetAxis)
//        {
//            try
//            {

//                if (TargetAxis < 1 | TargetAxis > 6)
//                {
//                    EpsonErrLog.ErrorMess = "Param is error\r\n";
//                    EpsonErrLog.ErrorCmd = "StopAxisOff";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }

//                //判断是否正在执行命令，不然会丢失指令
//                if (this.RobotExecuteBusy == true)
//                {
//                    EpsonErrLog.ErrorMess = "Currently executing other commands\r\n";
//                    EpsonErrLog.ErrorCmd = "StopAxisOff";
//                    ErrorDisPlay(this, ea);
//                    return false;
//                }
//                else
//                {
//                    RobotExecuteBusy = true;
//                }

//                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
//                {
//                    EpsonErrLog.ErrorMess = "get Status failure\r\n";
//                    EpsonErrLog.ErrorCmd = "StopAxisOff";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                int i;
//                i = FeedBackMessageFromRobot.IndexOf(",");

//                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
//                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
//                //---------------------------------------------------------------------------

//                //Brake
//                //用于打开和关闭当前机械手指定关节的制动器。
//                //格式
//                //Brake 状态, 关节编号
//                //参数
//                //状态 施加制动时：使用On。
//                //解除制动时：使用Off。
//                //关节编号 指定1～6 的关节编号。
//                //说明
//                //Brake 命令用于对垂直6 轴型机械手的一个关节施加或解除制动。这是仅可通过命令使用的命令。此
//                //命令设计为只有维修作业人员才可以使用。
//                //如果执行Brake 命令，则会对机械手控制参数进行初始化。

//                //brake on, 1
//                //brake off, 1

//                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
//                {
//                    FeedBackMessageFromRobot = RobotSendCmd("$Execute,\"Brake off, " +
//                        TargetAxis + "\"");
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = "Current Status Can not Motion\r\n";
//                    EpsonErrLog.ErrorCmd = "StopAxisOff";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
//                {
//                    RobotExecuteBusy = false;
//                    return true;
//                }
//                else
//                {
//                    EpsonErrLog.ErrorMess = ProcessResponse(FeedBackMessageFromRobot); ;
//                    EpsonErrLog.ErrorCmd = "StopAxisOff";
//                    ErrorDisPlay(this, ea);
//                    RobotExecuteBusy = false;
//                    return false;
//                }

//            }
//            catch (Exception ex)
//            {
//                EpsonErrLog.ErrorMess = ex.Message; ;
//                EpsonErrLog.ErrorCmd = "StopAxisOff";
//                ErrorDisPlay(this, ea);
//                RobotExecuteBusy = false;
//                return false;
//            }
//        }
//    }
//}
