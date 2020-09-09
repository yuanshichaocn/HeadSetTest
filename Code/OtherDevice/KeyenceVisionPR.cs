using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseDll;
using Communicate;
using log4net;
using SerialDict;
using MotionIoLib;
using System.Threading;
using BaseDll;


namespace OtherDevice
{
    public class KeyneceVisionProcessor
    {

        ILog _logger = LogManager.GetLogger("KeyneceVisionProcessor");
        public enum SendCommandType
        {
            //标定（九点标定）
            CMD_Send_BarrelDispCalib = 0,  //取镜筒_点胶相机
            CMD_Send_PickUpCalib = 3,      //取物料_上相机
            CMD_Send_PackUpCalib = 6,      //组装_上相机
            CMD_Send_DispCalib = 11,       //点胶_点胶相机
            //注册
            CMD_Send_pickBarrelRegister = 1,
            CMD_Send_PickUpRegister = 4,
            CMD_Send_PackUpRegister = 9,
            //组装坐标统一
            CMD_Send_UnifyCoor_BottomCam = 7,
            CMD_Send_UnifyCoor_UpCam = 8,
            //点胶相机取料对位
            CMD_Send_PickCounterpoint_DispCam = 2,  //取镜筒_点胶相机对位
            //上相机取料对位
            CMD_Send_PickCounterpoint_UpCam = 5,    //取物料_上相机对位
            //获取keyence取料时上相机快门速度
            CMD_Send_GetPickShutter_UpCam = 50,     

            //相机组装对位
            CMD_Send_PackCounterpoint = 10,         //组立时对位
            //标定点胶旋转中心
            CMD_Send_CalibDispRCenter = 12,
            //点胶获取基准位
            CMD_Send_DispGetBasePot = 13,
            //获取点胶路径关键点
            CMD_Send_DispGetKeyPot = 14,            

            //点胶坐标统一
            CMD_Send_DispUnifyCoor_BottomCam = 15,
            CMD_Send_DispUnifyCoor_UpCam = 16,

            /********贴合重复性精度验证*************/
            //上相机取料对位
            CMD_Send_贴合_上相机对位 = 20,
            //组立
            CMD_Send_贴合_组装对位 = 21,
            //求偏心度
            CMD_Send_贴合_获取偏心度 = 22,
            //贴合基准位注册
            CMD_Send_贴合_注册 = 23,
            CMD_Send_注册收料吸嘴 = 24,
            CMD_Send_收料位置获取 = 25,
            /*************************************/
        }
        public enum MessType
        {
            //标定
            CMD_Answer_CalibStart = 1,
            CMD_Answer_CalibOffset,
            CMD_Answer_CalibEnd,
            CMD_Answer_CalibFail,
            //组装坐标统一
            CMD_Answer_UnifyCoorEnd_BottomCam,
            CMD_Answer_UnifyCoorEnd_UpCam,
            CMD_Answer_UnifyCoorFail_BottomCam,
            CMD_Answer_UnifyCoorFail_UpCam,
            //点胶相机取料
            CMD_Answer_DispPickEnd,
            CMD_Answer_DispPickFail,
            //上相机取料
            CMD_Answer_UpPickEnd,
            CMD_Answer_UpPickFail,

            //组装下相机
            CMD_Answer_BottomPackageEnd,
            CMD_Answer_BottomPackageFail,
            //组装
            CMD_Answer_PackageEnd,
            CMD_Answer_PackageFail,
            //获取点胶路径数据
            CMD_Answer_DispGetKeyPointEnd,
            CMD_Answer_DispGetKeyPointFail,
            //点胶（获取点胶针头基准位）
            CMD_Answer_DispGetBasePointEnd,
            CMD_Answer_DispGetBasePointFail,
            //获取取料时keyence快门速度
            CMD_Answer_PickGetShutterEnd,
            CMD_Answer_PickGetShutterFail,
            //点胶坐标统一
            CMD_Answer_DispUnifyCoorEnd_BottomCam,
            CMD_Answer_DispUnifyCoorEnd_UpCam,
            CMD_Answer_DispUnifyCoorFail_BottomCam,
            CMD_Answer_DispUnifyCoorFail_UpCam,

            /********贴合重复性精度验证*************/
            CMD_Answer_贴合_取料对位完成,
            CMD_Answer_贴合_取料对位失败,
            //组装下相机
            CMD_Answer_贴合_下相机对位完成,
            CMD_Answer_贴合_下相机对位失败,
            //组装
            CMD_Answer_贴合_组立完成,
            CMD_Answer_贴合_组立失败,
            //求偏心度
            CMD_Answer_贴合_求偏心度第一次完成,
            CMD_Answer_贴合_求偏心度第一次失败,
            CMD_Answer_贴合_求偏心度完成,
            CMD_Answer_贴合_求偏心度失败,
            //求吸嘴位置
            CMD_Answer_收料_获取位置成功,
            CMD_Answer_收料_获取位置失败,

        }
        public enum MsgStatus
        {
            Continue,
            End,
            Fail,
        }
        public enum Camera
        {
            BottomCamera=1,
            UpCamera,
            DispCamera,
        }


        //消息解析结构体
        public struct MessageStruct
        {
            public MessType messType;
            public List<double> shutterSpeed;
            public Point2d dispRotationsPoint;

            //public List<List<Point2d>> dispArcPoints;
            //public List<List<Point2d>> dispLinePoints;
            public List<Arc> dispArcs;
            public List<Line> dispLines;

            // public List<Point2d> dispKeyPoints;
            public MsgStatus status;
            public int separatorNum;  //分隔符数量
            public int comIndex;
            public int produceIndex;
            public int nozzelIndex;
            public double offsetX;
            public double offsetY;
            public double offsetT;
            public double X;
            public double Y;
            public double T;
            public double X1;
            public double Y1;
            public double X2;
            public double Y2;
        }

        public struct Arc
        {
            public Point2d roundCenter;
            public Point2d startPoint;
            public Point2d endPoint;
        }
        public struct Line
        {
            public Point2d startPoint;
            public Point2d endPoint;
        }

        public Queue<MessageStruct> m_messageQueue = new Queue<MessageStruct>();  //通讯的消息队列
        private static readonly object Lock = new object();
        public void clearMessageQueue()
        {
            lock (Lock)  //防止多线程访问冲突
            {
                m_messageQueue.Clear();
            }
        }
        public int OutMessageQueue(out MessageStruct mes)
        {
            mes = new MessageStruct();
            lock (Lock)  //防止多线程访问冲突
            {
                if (m_messageQueue.Count() > 0)
                {
                    mes = m_messageQueue.Dequeue();
                    _logger.Info(DateTime.Now.ToString() + $" : 消息已出队列");

                    return 0;
                }
                else
                    return -1;
            }
        }

        public void InMessageQueue(MessageStruct mes)
        {
            lock (Lock)  //防止多线程访问冲突
            {
                m_messageQueue.Enqueue(mes);
                _logger.Info(DateTime.Now.ToString() + $" : 消息已入队列");

            }
        }

        //机械坐标
        public XYUPoint? GetAssMData(string itemName)
        {
            if (dicVisionAssData.ContainsKey(itemName))
            {
                return dicVisionAssData[itemName][0];
            }
            return null;
        }
        //图像坐标
        public XYUPoint? GetAssVData(string itemName)
        {
            if (dicVisionAssData.ContainsKey(itemName))
            {
                return dicVisionAssData[itemName][1];
            }
            return null;
        }


        Dictionary<string, List<XYUPoint>> dicVisionAssData = new Dictionary<string, List<XYUPoint>>();
        ~KeyneceVisionProcessor()
        {

        }
        TcpLink link = null;
        public void linkKeyence()
        {
            if (link != null)
            {
                if (link.IsOpen())
                    link.Close();
           
            }
            link = new TcpLink(0, "Keyence", "192.168.33.33", 5000, 5000, "CR");
            //link = new TcpLink(0, "Keye", "192.168.33.100", 1024, 5000, "c");
            
            link.RecvStringMessageEvent += (object sender, AsyTcpSocketEventArgs e) =>
            {
                if(evenShowMsg!=null)
                evenShowMsg(e.Message);
                Parce(e.Message);
                _logger.Info(DateTime.Now.ToString() + " : " + e.Message);
            };
            link.SocketErrorEvent += (object sender, AsyTcpSocketEventArgs e) =>
             {
                 MessageBox.Show(e.Message + "请停止程序或重连Keyence");
                 return;
             };
           // link.Open();
        }
        //    SocketSever socketSever = new SocketSever();

        public delegate bool showMsg(string str);
        public event showMsg evenShowMsg = null;
        //类构造函数
        public KeyneceVisionProcessor(string Name, string ip, int port)
        {
           link = new TcpLink(0, "Keyence", "192.168.33.33", 5000, 5000, "CR");
            //link = new TcpLink(0, "Keye", "192.168.33.100", 1024, 5000, "c");

            link.RecvStringMessageEvent += (object sender, AsyTcpSocketEventArgs e) =>
              {
                  if (evenShowMsg != null)
                      evenShowMsg(e.Message);
                  _logger.Info(DateTime.Now.ToString() + " : " + e.Message);
                  Parce(e.Message);
                  
              };
           // link.Open();

            //SocketSeverMgr.GetInstace().Add(Name, socketSever);  //添加服务器
            //socketSever.Init(ip, port);                          //初始化服务器
            //RegisterProcessData();
        }

        public void Send(string cmd)  //向指定客户端发送指令
        {
            cmd = cmd + EndChar;
            // socketSever.Send(keyenceIP, keyenceCom, cmd);
            link.WriteString(cmd);
        }
        string strReciveData = "";
        //public void RegisterProcessData()     
        //{
        //    socketSever.ProcessData += (str) =>  //处理数据委托
        //    {
        //        strReciveData += str;
        //        if (IsWholeMsg(str))             //若是完整的消息
        //        {
        //            strReciveData = "";

        //            List<int> index = new List<int>();
        //            getAllSeparatorInd(str, "#", out index);
        //            string comStr = str.Substring(index[index.Count()-1] + 1);

        //            //离线调试取端口号（后续不要）
        //            //keyenceCom = str.Substring(index[0] + 1, index[1]-index[0]-1);

        //            Parce(comStr);                  //解析字符串，解析出来的坐标存在dicVisionAssData中，外部再去取
        //        }
        //    };
        //}


        public bool IsWholeMsg(string str)
        {
            if (str.EndsWith(EndChar))
                return true;
            else
                return false;
        }

        #region 变量
        //结束符
        public static string EndChar
        {
            get
            {
                char[] chr = new char[] { (char)13 };  // '/r'
                return new string(chr, 0, chr.Length);
            }
        }
        //分隔符
        public static string Separator
        {
            get
            {
                return ",";
            }
        }

        //keyence IP
        static string keyenceIP = "192.168.0.10";
        //keyence端口
        static string keyenceCom = "8500";
        //虚拟客户端
        //static string keyenceIP = "127.0.0.1";  
        //static string keyenceCom = "55754";
        //标定完成标志位
        bool m_endCalibration = false;
        bool m_oneStepOut = false;
        //取料时1~16号吸嘴不同的亮度
        //List<int> m_listLight = new List<int>();

        //单例模式
        public static KeyneceVisionProcessor m_keyenceVP;

        static public KeyneceVisionProcessor GetInstance()
        {
            if (m_keyenceVP == null)
            {
                m_keyenceVP = new KeyneceVisionProcessor("keyence", "192.168.66.66", 5000);
                //m_keyenceVP = new KeyneceVisionProcessor("keyence", "127.0.0.157", 502);  //虚拟服务端

            }
            return m_keyenceVP;
        }
        #endregion

        //获取所有分隔符位置索引
        private void getAllSeparatorInd(string str, string separator, out List<int> index)
        {
            index = new List<int>();
            int pos = str.IndexOf(separator); //0开始
            while (pos > -1)
            {
                index.Add(pos);
                pos += separator.Length;
                if (pos >= str.Length)
                {
                    break;
                }

                pos = str.IndexOf(separator, pos);
            }
        }

        #region 委托
        //标定
        public delegate bool CalibMove(int AxisX, int AxisY, int AxisT, double offsetx, double offdsety, double offdsetT);
        public event CalibMove eventCalibMove = null;
        public bool eventCalibMoveIsBeRegister()
        {
            if (eventCalibMove == null)
                return false;
            else
                return true;
        }
        public void clearEventCalibMoveRegister()
        {
            eventCalibMove = null;
               
        }
        //位置注册
        public delegate bool PickRegister(SendCommandType commandType, int nozzelInd, out double SnapX, out double SnapY, out double SnapT,
                                              out double PickX, out double PickY, out double PickT);
        public event PickRegister evenPickRegister = null;
        
        //上下相机坐标统一
        public delegate bool UnifyMove();
        public event UnifyMove evenUnifyMove = null;
        public bool evenUnifyMoveIsBeRegister()
        {
            if (evenUnifyMove == null)
                return false;
            else
                return true;
        }
        //取料（点胶相机、上相机）绝对坐标
        //public delegate bool PickProCounterpoint(int AxisX, int AxisY, int AxisT, double PickX, double PickY, double PickT);
        //public event PickProCounterpoint evenPickProCounterpointMove = null;
        //组装
        public delegate bool Package_toUpCamMove();
        public event Package_toUpCamMove evenPackage_toUpCamMove = null;
        //public delegate bool PackageMove(int AxisX, int AxisY, int AxisT, double offsetX, double offsetY, double offsetT);
        //public event PackageMove evenPackageMove = null;
        
        //点胶旋转中心标定
        public delegate bool Disp_vcmMove(int AxisX, int AxisY, int AxisT, double offsetx, double offsety, double offsetT);
        public event Disp_vcmMove evenDisp_vcmMove = null;
        #endregion


        #region 开始取料工站标定
        public int StartCalib(SendCommandType sendComm, string camName, int AxisX, int AxisY)  //可根据轴号不同取消掉sendComm参数
        {
            clearMessageQueue();
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();   //获取当前X、Y轴坐标
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            Send($"CC,{(int)sendComm},{AxisXPos},{AxisYPos},0");
            //while (true)
            //{
            //    Thread.Sleep(50);
            //    MessageStruct mes;
            //    int ret = OutMessageQueue(out mes);
            //    if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibStart) //确认基恩士收到
            //    {
            //        break;
            //    }
            //}
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Restart();
            m_endCalibration = false;
            
            while (true)
            {
                m_oneStepOut = false;
                
                int retValue = CalibMoveOneStep(camName, AxisX, AxisY, sendComm);
                if (m_oneStepOut)
                {
                    MessageBox.Show($"标定失败：相机等待keyence超时");

                    return -1;
                }
                if (retValue != 0)   //标定过程中失败
                {
                    return retValue;
                }
                if (m_endCalibration == true)  //完成标定
                {
                    m_endCalibration = false;
                    break;
                }
               

            }

            return 0;
            //int AxisX = 0, AxisY = 0;
            //switch(camName)
            //{
            //    case "DispCam":
            //        //AxisX = 1;

            //}
        }
        int CalibMoveOneStep(string camName, int AxisX, int AxisY, SendCommandType sendComm)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)
            {
                Thread.Sleep(1);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibOffset) //继续移动
                {
                    //移动轴
                    if (eventCalibMove != null)
                    {
                        if (eventCalibMove(AxisX, AxisY,0, mes.offsetX, mes.offsetY, mes.offsetT)) //若轴移动到位
                        {
                            //Send($"CC,{(int)sendComm},T");

                            double dCurrentPosX = MotionMgr.GetInstace().GetAxisPos(AxisX);
                            double dCurrentPosY = MotionMgr.GetInstace().GetAxisPos(AxisY);
                            Send($"CC,{(int)sendComm},{dCurrentPosX},{dCurrentPosY},0");
                            break;
                        }
                        else
                        {
                            MessageBox.Show($"{camName} 相机标定失败：轴没移动到位");
                            return -1;
                        }
                    }

                    else
                    {
                        MessageBox.Show($"{camName} 相机标定失败：没有注册运动函数");
                        return -2;
                    }
                    //double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);

                    //      MotionMgr.GetInstace().AbsMove()
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibEnd)  //完成标定
                {
                    MessageBox.Show($"{camName} 相机标定成功");
                    m_endCalibration = true;
                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibFail) //标定失败
                {
                    MessageBox.Show($"{camName} 相机标定失败，kenence搜索失败");
                    return -3;
                }
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    m_oneStepOut = true;
                    //MessageBox.Show($"标定失败：相机等待keyence超时");

                    return -1;
                }
            }
            return 0;

        }
        #endregion

        #region 开始点胶工站标定
        public int StartCalib(SendCommandType sendComm, string camName, int AxisX, int AxisY, out XYUPoint RCenterPot)  //可根据轴号不同取消掉sendComm参数
        {
            RCenterPot = new XYUPoint();
            clearMessageQueue();
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();   //获取当前X、Y轴坐标
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            Send($"CC,{(int)sendComm},{AxisXPos},{AxisYPos},0");
            m_endCalibration = false;

            while (true)
            {
                m_oneStepOut = false;

                int retValue = CalibMoveOneStep(camName, AxisX, AxisY, sendComm, out RCenterPot);
                if (m_oneStepOut)
                {
                    MessageBox.Show($"标定失败：相机等待keyence超时");

                    return -1;
                }
                if (retValue != 0)   //标定过程中失败
                {
                    return retValue;
                }
                if (m_endCalibration == true)  //完成标定
                {
                    m_endCalibration = false;
                    break;
                }


            }

            return 0;
            //int AxisX = 0, AxisY = 0;
            //switch(camName)
            //{
            //    case "DispCam":
            //        //AxisX = 1;

            //}
        }
        int CalibMoveOneStep(string camName, int AxisX, int AxisY, SendCommandType sendComm, out XYUPoint RCenterPot)
        {
            RCenterPot = new XYUPoint();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)
            {
                Thread.Sleep(1);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibOffset) //继续移动
                {
                    //移动轴
                    if (eventCalibMove != null)
                    {
                        if (eventCalibMove(AxisX, AxisY, 0, mes.offsetX, mes.offsetY, mes.offsetT)) //若轴移动到位
                        {
                            //Send($"CC,{(int)sendComm},T");

                            double dCurrentPosX = MotionMgr.GetInstace().GetAxisPos(AxisX);
                            double dCurrentPosY = MotionMgr.GetInstace().GetAxisPos(AxisY);
                            Send($"CC,{(int)sendComm},{dCurrentPosX},{dCurrentPosY},0");
                            break;
                        }
                        else
                        {
                            MessageBox.Show($"{camName} 相机标定失败：轴没移动到位");
                            return -1;
                        }
                    }

                    else
                    {
                        MessageBox.Show($"{camName} 相机标定失败：没有注册运动函数");
                        return -2;
                    }
                    //double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);

                    //      MotionMgr.GetInstace().AbsMove()
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibEnd)  //完成标定
                {
                    RCenterPot.x = mes.offsetX;
                    RCenterPot.y = mes.offsetY;
                    RCenterPot.u = mes.offsetT;
                    MessageBox.Show($"{camName} 相机标定成功");
                    m_endCalibration = true;
                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibFail) //标定失败
                {
                    MessageBox.Show($"{camName} 相机标定失败，kenence搜索失败");
                    return -3;
                }
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    m_oneStepOut = true;
                    //MessageBox.Show($"标定失败：相机等待keyence超时");

                    return -1;
                }
            }
            return 0;

        }
        #endregion

        #region 注册基准位
        public int RegisterBasePos(SendCommandType sendComm, int nozzelInd)
        {
            clearMessageQueue();
            string str = "";
            switch (sendComm)
            {
                case SendCommandType.CMD_Send_pickBarrelRegister:
                    str = "点胶相机取料";
                    break;
                case SendCommandType.CMD_Send_PickUpRegister:
                    str = "上相机取料";
                    break;
                case SendCommandType.CMD_Send_PackUpRegister:
                    str = "上相机组装";
                    break;
                default:
                    break;
            }

            //取料注册
            double SnapX, SnapY, SnapT, PickX, PickY, PickT;
            if (sendComm == SendCommandType.CMD_Send_pickBarrelRegister || sendComm == SendCommandType.CMD_Send_PickUpRegister)
            {
                if (evenPickRegister != null)  
                {
                    if (evenPickRegister(sendComm, nozzelInd, out SnapX, out SnapY, out SnapT,
                                             out PickX, out PickY, out PickT)) 
                    {
                        Send($"CC,{(int)sendComm},{nozzelInd}," +
                            $"{SnapX},{SnapY},0,{PickX},{PickY},0");
                    }
                    else
                    {
                        MessageBox.Show($"{str}基准位注册失败：没获取到相机、吸嘴坐标");
                        return -1;
                    }
                }
                else
                {
                    MessageBox.Show($"{str}基准位注册失败：没有注册函数");
                    return -2;
                }
            }
            //组装时上相机拍照位注册
            else if (sendComm == SendCommandType.CMD_Send_PackUpRegister)
            {
                //string CameraAxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();   //获取拍照位
                //string CameraAxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
                //string CameraAxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
                if (evenPickRegister != null)
                {
                    if (evenPickRegister(sendComm, nozzelInd, out SnapX, out SnapY, out SnapT,
                                         out PickX, out PickY, out PickT))
                    {
                        Send($"CC,{(int)sendComm}," +
                            $"{SnapX},{SnapY},0");
                    }
                    else
                    {
                        MessageBox.Show($"{str}基准位注册失败：没获取到相机、吸嘴坐标");
                        return -1;
                    }
                }
                else
                {
                    MessageBox.Show($"{str}基准位注册失败：没有注册函数");
                    return -2;
                }
            }
            return 0;
        }
        #endregion

        #region 上下相机坐标统一
        public int UnifyCoor()
        {
            MessageBox.Show($"确认已到下相机位置准备拍照");
            clearMessageQueue();
            //通知keyence拍照
            Send($"CC,{(int)SendCommandType.CMD_Send_UnifyCoor_BottomCam},T");
            //等待确认下相机状态

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            while (true)  //最好加上定时跳出
            {
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_UnifyCoorEnd_BottomCam) //下相机返回成功
                {
                    break;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_UnifyCoorFail_BottomCam) //下相机返回失败
                {
                    MessageBox.Show($"上下相机坐标系统一失败：下相机搜索失败");
                    return -1;
                }

                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    MessageBox.Show($"上下相机坐标系统一失败：下相机等待keyence超时");
                    return -1;
                }
            }

            //吸取标定纸到镜筒托台位后上相机到托台上拍照
            if (evenUnifyMove != null)
            {
                if (evenUnifyMove())
                {
                    Send($"CC,{(int)SendCommandType.CMD_Send_UnifyCoor_UpCam},T");
                }
                else
                {
                    MessageBox.Show($"上下相机坐标系统一失败：轴没移动到位");
                    return -1;
                }
            }
            else
            {
                MessageBox.Show("上下相机坐标系统一失败：没有注册运动函数");
                return -2;
            }
            //等待确认上相机状态
            while (true)  //最好加上定时跳出
            {
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_UnifyCoorEnd_UpCam) //上相机返回成功
                {
                    MessageBox.Show($"上下相机坐标系统一完成");
                    break;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_UnifyCoorFail_UpCam) //上相机返回成功
                {
                    MessageBox.Show($"上下相机坐标系统一失败：上相机搜索失败");
                    return -1;
                }
            }
            return 0;
        }

        #endregion

        #region 点胶相机取料对位
        public int CounterPoint_DispPick(int nozzelInd, int AxisX, int AxisY, int AxisT,out XYUPoint XYTPoint, int nTimeout = 3000)
        {
            Stopwatch stopwatch = new Stopwatch();
            XYTPoint = new XYUPoint();
            clearMessageQueue();
            //获取并发送当前拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();   
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            //string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            Send($"CC,{(int)SendCommandType.CMD_Send_PickCounterpoint_DispCam}," +
                                        $"{nozzelInd},{AxisXPos},{AxisYPos},{0}");
            _logger.Info(DateTime.Now.ToString() + $" :  点胶相机发出拍照取料识别指令");
            stopwatch.Restart();
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
              
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_DispPickEnd) //基恩士返回抓取点
                {
                    //不需要注册函数，直接返回mes.X, mes.Y, mes.T就可以
                    XYTPoint.x = mes.X;
                    XYTPoint.y = mes.Y;
                    XYTPoint.u = mes.T;
                    _logger.Info(DateTime.Now.ToString() + $" :  点胶相机取料对位成功消息已出队列，x={ mes.X},y={mes.Y},u={mes.T}");

                    break;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_DispPickFail) //基恩士返回抓取点失败
                {
                    _logger.Info(DateTime.Now.ToString() + $" :  点胶相机取料对位成功消息已出队列，x={ mes.X},y={mes.Y},u={mes.T}");
                   //MessageBox.Show($"抓取失败：keyence搜索失败");
                    return -3;
                }
                else if(stopwatch.ElapsedMilliseconds> nTimeout)
                {
                    return -4;
                }
            }
            return 0;
        }

        #endregion

        #region 上相机取料对位
        public int CounterPoint_UpPick(double shutter, int nozzelInd, int AxisX, int AxisY, int AxisT, out XYUPoint XYTPoint, int nTimeout = 3000)
        {
            XYTPoint = new XYUPoint();
            clearMessageQueue();
            Stopwatch stopwatch = new Stopwatch();
            //获取并发送当前拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            //string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            Send($"CC,{(int)SendCommandType.CMD_Send_PickCounterpoint_UpCam}," +
                                        $"{shutter},{nozzelInd},{AxisXPos},{AxisYPos},{0}");
            _logger.Info(DateTime.Now.ToString() + $" :  上相机取料对位成功命令发出"+ $"CC,{(int)SendCommandType.CMD_Send_PickCounterpoint_UpCam}," +
                                        $"{shutter},{nozzelInd},{AxisXPos},{AxisYPos},{0}");
            stopwatch.Restart();
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_UpPickEnd) //基恩士返回抓取点
                {
                    //不需要注册函数，直接返回mes.X, mes.Y, mes.T就可以
                    XYTPoint.x = mes.X;
                    XYTPoint.y = mes.Y;
                    XYTPoint.u = mes.T;
                    _logger.Info(DateTime.Now.ToString() + $" :  上相机取料对位成功消息已出队列，x={ mes.X},y={mes.Y},u={mes.T}" );

                    break;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_UpPickFail) //基恩士返回抓取点失败
                {
                    _logger.Info(DateTime.Now.ToString() + $" :  上相机取料对位失败消息已出队列");

                  //  MessageBox.Show($"抓取失败：keyence搜索失败");
                    return -3;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                 //   MessageBox.Show($"抓取失败：等待keyence超时");
                    return -4;
                }
               
            }
            return 0;
        }
        #endregion

        #region 组装       
        public int packageBottom(int nozzelInd, int AxisX, int AxisY, out XYUPoint Centerpoint, int nTimeout = 3000)
        {
            clearMessageQueue();
            Centerpoint = new XYUPoint();
            //获取并发送下相机拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            string AxisTPos = "";
            //string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            Send($"CC,{(int)SendCommandType.CMD_Send_PackCounterpoint}," +
                                        $"{(int)Camera.BottomCamera},{nozzelInd},{0},{AxisXPos},{AxisYPos},{0}");
            _logger.Info(DateTime.Now.ToString() + $" :  下相机发出拍照识别部件指令");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            //等待基恩士回复
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_BottomPackageEnd) //基恩士完成下相机
                {
                    //加上获取坐标
                    Centerpoint.x = mes.X;
                    Centerpoint.y = mes.Y;
                    Centerpoint.u = mes.T;

                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_BottomPackageFail) 
                {
                  //  MessageBox.Show($"组装失败：keyence搜索失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                 //   MessageBox.Show($"组装失败：等待keyence超时");
                    return -2;
                }
                
            }          
        }

        public int package(int nozzleInd, int AxisX, int AxisY, int AxisT,int mode, double angleoffset, out XYUPoint XYTOffset,int nTimeout=3000)
        {
            XYTOffset = new XYUPoint();
            clearMessageQueue();

            //获取并发送上相机拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            //string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            string AxisTPos = angleoffset.ToString();

            Send($"CC,{(int)SendCommandType.CMD_Send_PackCounterpoint}," +
                       $"{(int)Camera.UpCamera},{nozzleInd},{mode},{AxisXPos},{AxisYPos},{AxisTPos}");
            _logger.Info(DateTime.Now.ToString() + $" :  上相机发出拍照识别Barrel指令");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            //等待基恩士回复偏移量
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_PackageEnd) //基恩士完成上相机拍照搜索
                {
                    //不用委托，直接return（mes.offsetX, mes.offsetY, mes.offsetT）
                    XYTOffset.x = mes.offsetX;
                    XYTOffset.y = mes.offsetY;
                    XYTOffset.u = mes.offsetT;
                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_PackageFail) //基恩士返回抓取点失败
                {
                 //   MessageBox.Show($"组装失败：keyence搜索失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                 //   MessageBox.Show($"组装失败：等待keyence超时");
                    return -1;
                }
        
            }

        }
        #endregion

        #region 获取点胶基准位
        public int getDispBasePoint(int AxisX, int AxisY, int AxisT, out XYUPoint XYTOffset, int nTimeout = 3000)
        {
            XYTOffset = new XYUPoint();
            clearMessageQueue();
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();


            //获取并发送上相机拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            KeyneceVP.Send($"CC,{(int)SendCommandType.CMD_Send_DispGetBasePot},{AxisXPos},{AxisYPos},{AxisTPos}");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            //等待基恩士回复偏移量
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_DispGetBasePointEnd) //基恩士完成上相机拍照搜索
                {
                    //不用委托，直接return（mes.offsetX, mes.offsetY, mes.offsetT）
                    XYTOffset.x = mes.offsetX;
                    XYTOffset.y = mes.offsetY;
                    XYTOffset.u = mes.offsetT;
                    return 0;

                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_DispGetBasePointFail)
                {
                    MessageBox.Show($"获取点胶基准点失败：keyence搜索失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                    MessageBox.Show($"获取点胶基准点失败：等待keyence超时");
                    return -1;
                }

            }

        }

        #endregion

        #region 标定点胶旋转中心
        public int calibDispCenterPoint(int AxisX, int AxisY, int AxisT, out XYUPoint RCenterPot, int nTimeout = 3000)
        {
            clearMessageQueue();
            RCenterPot = new XYUPoint();
            //获取并发送点胶相机拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            Send($"CC,{(int)SendCommandType.CMD_Send_CalibDispRCenter},{AxisXPos},{AxisYPos},{AxisTPos}");

            m_endCalibration = false;

            while (true)
            {
                m_oneStepOut = false;
                int retValue = calibDispCenterPointOneStep(AxisX, AxisY, AxisT, 
                    SendCommandType.CMD_Send_CalibDispRCenter, out RCenterPot);
                if (m_oneStepOut)
                {
                    MessageBox.Show($"点胶旋转中心标定失败：等待keyence超时");

                    return -1;
                }
                if (retValue != 0)   //标定过程中失败
                {
                    return retValue;
                }
                if (m_endCalibration == true)  //完成标定
                {
                    m_endCalibration = false;
                    break;
                }
            }
            
            return 0;
        }

        int calibDispCenterPointOneStep(int AxisX, int AxisY, int AxisT, SendCommandType sendComm, out XYUPoint RCenterPot, int nTimeout = 3000)
        {
            RCenterPot = new XYUPoint();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)
            {
                Thread.Sleep(1);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibOffset) //继续移动
                {
                    //移动轴
                    if (evenDisp_vcmMove != null)
                    {
                        if (evenDisp_vcmMove(AxisX, AxisY, AxisT, mes.offsetX, mes.offsetY, mes.offsetT)) //若轴移动到位
                        {
                            //Send($"CC,{(int)sendComm},T");

                            double dCurrentPosX = MotionMgr.GetInstace().GetAxisPos(AxisX);
                            double dCurrentPosY = MotionMgr.GetInstace().GetAxisPos(AxisY);
                            double dCurrentPosT = MotionMgr.GetInstace().GetAxisPos(AxisT);

                            Send($"CC,{(int)sendComm},{dCurrentPosX},{dCurrentPosY},{dCurrentPosT}");
                            break;
                        }
                        else
                        {
                            MessageBox.Show("点胶旋转中心标定失败：轴没移动到位");
                            return -1;
                        }
                    }

                    else
                    {
                        MessageBox.Show($"点胶旋转中心标定失败：没有注册运动函数");
                        return -2;
                    }
                    //double currentx = MotionMgr.GetInstace().GetAxisPos(AxisX);

                    //      MotionMgr.GetInstace().AbsMove()
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibEnd)  //完成标定
                {
                    MessageBox.Show($"点胶旋转中心相机标定成功");
                    RCenterPot.x = mes.offsetX;
                    RCenterPot.y = mes.offsetY;
                    RCenterPot.u = mes.offsetT;

                    m_endCalibration = true;
                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_CalibFail) //标定失败
                {
                    MessageBox.Show($"点胶旋转中心标定失败，kenence搜索失败");
                    return -3;
                }
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    m_oneStepOut = true;
                    MessageBox.Show($"点胶旋转中心标定失败：等待keyence超时");

                    return -1;
                }
            }
            return 0;
            
        }

        #endregion

        #region 获取点胶机械坐标点
        public int getDispKeyPoint(int AxisX, int AxisY, int AxisT,
            out Point2d RotationsPoint,
            out List<Arc> arcPoint, 
            out List<Line> linePoint,
            int nTimeout = 5000)
        {
            clearMessageQueue();
            RotationsPoint = new Point2d();
            arcPoint = new List<Arc>();
            linePoint = new List<Line>();

            //获取并发送下相机拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            Send($"CC,{(int)SendCommandType.CMD_Send_DispGetKeyPot},{AxisXPos},{AxisYPos},{AxisTPos}");
            _logger.Info(DateTime.Now.ToString() + $" :  点胶相机获取关键点发出点胶拍照命令");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            //等待基恩士回复
            while (true)
            {
              
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_DispGetKeyPointEnd)
                {
                    //取出关键点
                    RotationsPoint = mes.dispRotationsPoint;
                    arcPoint = mes.dispArcs;
                    linePoint = mes.dispLines;
                    _logger.Info(DateTime.Now.ToString() + $" :  点胶相机获取关键点成功消息已出队列");

                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_DispGetKeyPointFail)
                {
                    MessageBox.Show($"获取点胶坐标失败：keyence搜索失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                    MessageBox.Show($"获取点胶坐标失败：等待keyence超时");
                    return -2;
                }

            }

            return 0;
        }

        #endregion

        #region 获取取料时keyence快门
        public int getPickShutterSpeed(out List<double> shutterVal)
        {
            shutterVal = new List<double>();
            clearMessageQueue();
            KeyneceVisionProcessor KeyneceVP = KeyneceVisionProcessor.GetInstance();
            SendCommandType sendComm = SendCommandType.CMD_Send_GetPickShutter_UpCam;
            //发送获取快门速度指令
            KeyneceVP.Send($"CC,{(int)sendComm}");
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            //等待基恩士回复偏移量
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_PickGetShutterEnd) //基恩士完成上相机拍照搜索
                {
                    shutterVal = mes.shutterSpeed;
                    return 0; 
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_PickGetShutterFail)
                {
                    MessageBox.Show($"获取keyence亮度失败：keyence返回失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > 3000)
                {
                    MessageBox.Show($"获取keyence亮度失败：等待keyence超时");
                    return -1;
                }

            }
            
        }

        #endregion

        #region 贴合重复性精度验证
        #region 上相机取料对位
        public int bonding_CounterPoint_UpPick(double shutter, int nozzelInd, int AxisX, int AxisY, int AxisT, out XYUPoint XYTPoint, int nTimeout = 3000)
        {
            XYTPoint = new XYUPoint();
            clearMessageQueue();
            Stopwatch stopwatch = new Stopwatch();
            //获取并发送当前拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            //string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            Send($"CC,{(int)SendCommandType.CMD_Send_贴合_上相机对位}," +
                                        $"{shutter},{nozzelInd},{AxisXPos},{AxisYPos},{0}");
            _logger.Info(DateTime.Now.ToString() + $" :  上相机取料对位成功命令发出" + $"CC,{(int)SendCommandType.CMD_Send_贴合_上相机对位}," +
                                        $"{shutter},{nozzelInd},{AxisXPos},{AxisYPos},{0}");
            stopwatch.Restart();
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_取料对位完成) //基恩士返回抓取点
                {
                    //不需要注册函数，直接返回mes.X, mes.Y, mes.T就可以
                    XYTPoint.x = mes.X;
                    XYTPoint.y = mes.Y;
                    XYTPoint.u = mes.T;
                    _logger.Info(DateTime.Now.ToString() + $" :  上相机取料对位成功消息已出队列，x={ mes.X},y={mes.Y},u={mes.T}");

                    break;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_取料对位失败) //基恩士返回抓取点失败
                {
                    _logger.Info(DateTime.Now.ToString() + $" :  上相机取料对位失败消息已出队列");

                   // MessageBox.Show($"抓取失败：keyence搜索失败");
                    return -3;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                    //MessageBox.Show($"抓取失败：等待keyence超时");
                    return -4;
                }

            }
            return 0;
        }
        #endregion

        #region 组装       
        //下相机
        public int bonding_packageBottom(int nozzelInd, int AxisX, int AxisY, out XYUPoint Centerpoint, int nTimeout = 3000)
        {
            clearMessageQueue();
            Centerpoint = new XYUPoint();
            //获取并发送下相机拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            string AxisTPos = "";
            //string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            Send($"CC,{(int)SendCommandType.CMD_Send_贴合_组装对位}," +
                                        $"{(int)Camera.BottomCamera},{nozzelInd},{0},{AxisXPos},{AxisYPos},{0}");
            _logger.Info(DateTime.Now.ToString() + $" :  下相机发出拍照识别部件指令");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            //等待基恩士回复
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_下相机对位完成) //基恩士完成下相机
                {
                    //加上获取坐标
                    Centerpoint.x = mes.X;
                    Centerpoint.y = mes.Y;
                    Centerpoint.u = mes.T;

                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_下相机对位失败) //基恩士返回抓取点失败
                {
                   // MessageBox.Show($"组装失败：keyence搜索失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                  //  MessageBox.Show($"组装失败：等待keyence超时");
                    return -2;
                }

            }
        }
        //上相机
        public int bonding_package(int nozzleInd, int AxisX, int AxisY, int AxisT, int mode, double angleoffset, out XYUPoint XYTOffset, int nTimeout = 3000)
        {
            XYTOffset = new XYUPoint();
            clearMessageQueue();

            //获取并发送上相机拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            //string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();
            string AxisTPos = angleoffset.ToString();

            Send($"CC,{(int)SendCommandType.CMD_Send_贴合_组装对位}," +
                       $"{(int)Camera.UpCamera},{nozzleInd},{mode},{AxisXPos},{AxisYPos},{AxisTPos}");
            _logger.Info(DateTime.Now.ToString() + $" :  上相机发出拍照识别Barrel指令");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            //等待基恩士回复偏移量
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_组立完成) //基恩士完成上相机拍照搜索
                {
                    //不用委托，直接return（mes.offsetX, mes.offsetY, mes.offsetT）
                    XYTOffset.x = mes.offsetX;
                    XYTOffset.y = mes.offsetY;
                    XYTOffset.u = mes.offsetT;
                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_组立失败) //基恩士返回抓取点失败
                {
                   // MessageBox.Show($"贴合失败：keyence搜索失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                  //  MessageBox.Show($"贴合失败：等待keyence超时");
                    return -1;
                }

            }

        }
        //获取圆心偏移量       
        public int bonding_getOffset(int nozzleInd, int time, int AxisX, int AxisY, int AxisT, out XYUPoint Circle1CenterPoint,out XYUPoint Circle2CenterPoint, int nTimeout = 3000)
        {
           // XYTOffset = new XYUPoint();
            clearMessageQueue();

            //获取并发送上相机拍照位
            string AxisXPos = MotionMgr.GetInstace().GetAxisPos(AxisX).ToString();
            string AxisYPos = MotionMgr.GetInstace().GetAxisPos(AxisY).ToString();
            string AxisTPos = MotionMgr.GetInstace().GetAxisPos(AxisT).ToString();

            Send($"CC,{(int)SendCommandType.CMD_Send_贴合_获取偏心度}," +
                       $"{time},{nozzleInd},{AxisXPos},{AxisYPos},{AxisTPos}");
            _logger.Info(DateTime.Now.ToString() + $" :  上相机发出拍照计算偏心度指令");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            Circle1CenterPoint.x = Circle1CenterPoint.y = Circle1CenterPoint.u = 0;
            Circle2CenterPoint.x = Circle2CenterPoint.y = Circle2CenterPoint.u = 0;

            //等待基恩士回复偏移量
            if (time == 1)
            {
                while (true)
                {
                    Thread.Sleep(10);
                    MessageStruct mes;
                    int ret = OutMessageQueue(out mes);
                    if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_求偏心度第一次完成) //基恩士完成上相机拍照搜索
                    {
                        //XYTOffset.x = mes.offsetX;
                        //XYTOffset.y = mes.offsetY;
                        //XYTOffset.u = mes.offsetT;
                        return 0;
                    }
                    else if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_求偏心度第一次失败) //基恩士返回抓取点失败
                    {
                        MessageBox.Show($"求偏心度第一次失败：keyence搜索失败");
                        return -1;
                    }
                    else if (stopwatch.ElapsedMilliseconds > nTimeout)
                    {
                        MessageBox.Show($"求偏心度第一次失败：等待keyence超时");
                        return -1;
                    }
                }
            }
            else if (time == 2)
            {
                 while (true)
                {
                    Thread.Sleep(10);
                    MessageStruct mes;
                    int ret = OutMessageQueue(out mes);
                    if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_求偏心度完成) //基恩士完成上相机拍照搜索
                    {
                        Circle1CenterPoint.x = mes.X1;
                        Circle1CenterPoint.y = mes.Y1;
                        Circle2CenterPoint.x = mes.X2;
                        Circle2CenterPoint.y = mes.Y2;
                        return 0;
                    }
                    else if (ret == 0 && mes.messType == MessType.CMD_Answer_贴合_求偏心度失败) //基恩士返回抓取点失败
                    {
                        MessageBox.Show($"求偏心度失败：keyence搜索失败");
                        return -1;
                    }
                    else if (stopwatch.ElapsedMilliseconds > nTimeout)
                    {
                        MessageBox.Show($"求偏心度失败：等待keyence超时");
                        return -1;
                    }
                }
            }
            return 0;
        }
        #endregion


        #endregion

        public int RegisterCollect( out XYUPoint xYU, int nTimeout = 6000)
        {
            clearMessageQueue();
            Send($"CC,{(int)SendCommandType.CMD_Send_注册收料吸嘴}");
            _logger.Info(DateTime.Now.ToString() + $" :  注册收料吸嘴");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            xYU.x = xYU.y = xYU.u = 0;
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_收料_获取位置成功) //基恩士完成上相机拍照搜索
                {
                    xYU.x = mes.X;
                    xYU.y = mes.Y;
                
                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_收料_获取位置失败) //基恩士返回抓取点失败
                {
                    MessageBox.Show($"收料_注册位置失败：keyence搜索失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                    MessageBox.Show($"收料_注册位置失败：等待keyence超时");
                    return -1;
                }
            }
            return 0;
        }

        //基恩士返回一个偏移值
        public int GetCollectPos(out  XYUPoint xYU, int nTimeout = 6000)
        {
            clearMessageQueue();
            Send($"CC,{(int)SendCommandType.CMD_Send_收料位置获取}");
            _logger.Info(DateTime.Now.ToString() + $" :  获取收料位置");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            xYU.x = xYU.y = xYU.u = 0;
            while (true)
            {
                Thread.Sleep(10);
                MessageStruct mes;
                int ret = OutMessageQueue(out mes);
                if (ret == 0 && mes.messType == MessType.CMD_Answer_收料_获取位置成功) //基恩士完成上相机拍照搜索
                {
                    xYU.x = mes.X;
                    xYU.y = mes.Y;
                    return 0;
                }
                else if (ret == 0 && mes.messType == MessType.CMD_Answer_收料_获取位置失败) //基恩士返回抓取点失败
                {
                    MessageBox.Show($"收料_获取位置失败：keyence搜索失败");
                    return -1;
                }
                else if (stopwatch.ElapsedMilliseconds > nTimeout)
                {
                    MessageBox.Show($"收料_获取位置失败：等待keyence超时");
                    return -1;
                }
            }
            return 0;
        }
        #region 解析字符串
        public void Parce(string str)
        {
            _logger.Info(DateTime.Now.ToString() + " :Start  Parce: " + str);
            List<int> SepIndex; //分隔符索引
            getAllSeparatorInd(str, Separator, out SepIndex);  //获取分隔符位置索引
            if (SepIndex.Count() < 1)
            {
                return;
            }
            string str_comInd = str.Substring(0, SepIndex[0]);
            int i_comInd = -999;
            try
            {
                i_comInd = Convert.ToInt32(str_comInd);
            }
            catch(Exception ex)
            {
                return;
            }
            

            //临时修改
            //if (i_comInd==2 && SepIndex.Count > 7)
            //{
            //    i_comInd = 14;
            //}




            _logger.Info(DateTime.Now.ToString() + $" :  Parce {i_comInd}: " + str);
            switch (i_comInd)
            {
                case 0:  //取镜筒点胶相机校正
                    {
                        ParceCablibMess(str, SepIndex);
                    }
                    break;

                case 1:     //点胶相机基准位注册
                    {
                        ParceRegisterPickMess(str, SepIndex, 2, i_comInd);
                    }
                    break;

                case 2:    //对位
                    {
                        
                        ParcePickMess(str, SepIndex, 5, MessType.CMD_Answer_DispPickEnd, MessType.CMD_Answer_DispPickFail);
                    }
                    break;

                case 3:  //上相机取料校正
                    {
                        ParceCablibMess(str, SepIndex);
                    }
                    break;

                case 4:   //取料上相机基准位注册
                    {
                        ParceRegisterPickMess(str, SepIndex, 2, i_comInd);
                    }
                    break;

                case 5:   //上相机对位
                    {
                        ParcePickMess(str, SepIndex, 6, MessType.CMD_Answer_UpPickEnd, MessType.CMD_Answer_UpPickFail);
                    }
                    break;

                case 6:   //上相机组装校正
                    {
                        ParceCablibMess(str, SepIndex);
                    }
                    break;

                case 7:  //组装坐标统一下相机
                    {
                      ParceUnifyCoor(str, SepIndex, 1, MessType.CMD_Answer_UnifyCoorEnd_BottomCam, MessType.CMD_Answer_UnifyCoorFail_BottomCam);
                    }
                    break;
                case 8:  //组装坐标统一上相机
                    {
                        ParceUnifyCoor(str, SepIndex, 1, MessType.CMD_Answer_UnifyCoorEnd_UpCam, MessType.CMD_Answer_UnifyCoorFail_UpCam);
                    }
                    break;
                case 9:      //上相机在镜筒托台上的基准位置注册
                    {
                        ParceRegisterPickMess(str, SepIndex, 1, i_comInd);
                    }
                    break;

                case 10:
                    {
                        //ParcePackageMess1(str, SepIndex); //用于循环获取数据验证重复性精度
                        ParcePackageMess(str, SepIndex);

                    }
                    break;

                case 11:  //点胶相机校正
                    {
                        ParceCablibMess(str, SepIndex);
                    }
                    break;

                case 12:  //点胶旋转中心标定
                    {
                        ParceCalibDispCenterMess(str, SepIndex);
                    }
                    break;

                case 13:  //获取点胶基准位
                    {
                        ParceDispBasePoint(str, SepIndex);
                    }
                    break;

                case 14:  //获取点胶关键点
                    {
                        ParceDispKeyPoint(str, SepIndex);
                    }
                    break;
                case 15:  //点胶坐标统一下相机
                    {
                        ParceUnifyCoor(str, SepIndex, 1, MessType.CMD_Answer_DispUnifyCoorEnd_BottomCam, MessType.CMD_Answer_DispUnifyCoorFail_BottomCam);
                    }
                    break;
                case 16:  //点胶坐标统一上相机
                    {
                        ParceUnifyCoor(str, SepIndex, 1, MessType.CMD_Answer_DispUnifyCoorEnd_UpCam, MessType.CMD_Answer_DispUnifyCoorFail_UpCam);
                    }
                    break;

                case 50:  //获取不同吸嘴对应的keyence快门速度
                    {
                        ParceShutterSpeed(str, SepIndex);
                    }
                    break;

                case 20:
                    {
                        ParceBondingPickMess(str, SepIndex, 6, MessType.CMD_Answer_贴合_取料对位完成, MessType.CMD_Answer_贴合_取料对位失败);
                    }
                    break;

                case 21:
                    {
                        ParceBondingPackageMess(str, SepIndex);
                    }
                    break;

                case 22:
                    {
                        ParceBondingOffset(str, SepIndex);
                    }
                    break;

                case 23:
                    {
                        ParceRegisterBondingPickMess(str, SepIndex, 2, i_comInd);
                    }
                    break;
                case 24:
                    {
                        ParceCollectPos(str, SepIndex);
                    }
                    break;
                case 25:
                    {
                        ParceCollectPos(str, SepIndex);
                    }
                    break;
                default:
                    break;
            }

            _logger.Info(DateTime.Now.ToString() + " :End  Parce: " + str);

        }
    #endregion

        #region 解析标定字符串
        void ParceCablibMess(string str, List<int> SepIndex)
        {
            if (SepIndex.Count == 2) //确认回应
            {
                MessageStruct tempMessage = new MessageStruct();
                tempMessage.messType = MessType.CMD_Answer_CalibStart;
                InMessageQueue(tempMessage);  //消息入队
            }
            else if (SepIndex.Count == 4)  //返回偏移量和状态信号
            {
                //状态信号
                string status = str.Substring(SepIndex[SepIndex.Count-1] + 1);
                int i_status = Convert.ToInt32(status);
                MessageStruct tempMessage = new MessageStruct();
                if (i_status == (int)MsgStatus.Continue) //继续标定
                {
                    //X,Y偏移量
                    double offsetX = Convert.ToDouble(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));
                    double offsetY = Convert.ToDouble(str.Substring(SepIndex[1] + 1, SepIndex[2] - SepIndex[1] - 1));
                    double offsetT = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));


                    tempMessage.messType = MessType.CMD_Answer_CalibOffset;
                    tempMessage.offsetX = offsetX;
                    tempMessage.offsetY = offsetY;
                    tempMessage.offsetT = offsetT;
                    InMessageQueue(tempMessage);  //消息入队
                }
                else if (i_status == (int)MsgStatus.End)  //完成标定
                {
                    tempMessage.messType = MessType.CMD_Answer_CalibEnd;
                    InMessageQueue(tempMessage);
                }
                else if (i_status == (int)MsgStatus.Fail)  //搜索失败
                {
                    tempMessage.messType = MessType.CMD_Answer_CalibFail;
                    InMessageQueue(tempMessage);
                }
            }
        }
        #endregion

        #region 解析取料注册字符串
        void ParceRegisterPickMess(string str, List<int> SepIndex, int stdSeparatorNum, int comInd)
        {
            if (SepIndex.Count != stdSeparatorNum) return;
            //状态信号
            string status = str.Substring(SepIndex[SepIndex.Count-1] + 1);
            int i_status = Convert.ToInt32(status);
            string strMsg = "";
            switch (comInd)
            {
                case (int)SendCommandType.CMD_Send_pickBarrelRegister:
                    strMsg = "点胶相机取Barrel";
                    break;
                case (int)SendCommandType.CMD_Send_PickUpRegister:
                    strMsg = "上相机取料";
                    break;
                case (int)SendCommandType.CMD_Send_PackUpRegister:
                    strMsg = "上相机放料";
                    break;
                default:
                    break;
            }
            if (i_status == (int)MsgStatus.End) //完成注册
            {
                //提醒完成基准位置注册
                MessageBox.Show(strMsg+"基准位置注册成功");
            }
            else if (i_status == (int)MsgStatus.Fail)  //注册失败
            {
                //提醒基准位置注册失败
                MessageBox.Show(strMsg+"基准位置注册失败");
            }
        }
        #endregion

        #region 解析坐标统一字符串
        void ParceUnifyCoor(string str, List<int> SepIndex, int stdSeparatorNum, MessType mesEnd, MessType mesFail)
        {
            if (SepIndex.Count != stdSeparatorNum) return;
            //状态信号
            string status = str.Substring(SepIndex[0] + 1);
            int i_status = Convert.ToInt32(status);
            MessageStruct tempMessage = new MessageStruct();
            if (i_status == (int)MsgStatus.End)  //下相机拍照成功
            {
                tempMessage.messType = mesEnd;
                InMessageQueue(tempMessage);
            }
            else if (i_status == (int)MsgStatus.Fail)  //搜索失败
            {
                tempMessage.messType = mesFail;
                InMessageQueue(tempMessage);
            }
        }
        #endregion

        #region 解析相机取料字符串
        void ParcePickMess(string str, List<int> SepIndex, int stdSeparatorNum, MessType mesEnd, MessType mesFail)
        {
            if (SepIndex.Count != stdSeparatorNum) 
                return;
           
            //状态信号
            string status = str.Substring(SepIndex[stdSeparatorNum-1] + 1);
            int i_status = Convert.ToInt32(status);
            MessageStruct tempMessage = new MessageStruct();
            if (i_status == (int)MsgStatus.End) 
            {
                //吸嘴号（略）
                //X,Y,T抓取绝对坐标
                double pickX, pickY, pickT;
                if (mesEnd == MessType.CMD_Answer_DispPickEnd)
                {
                    pickX = Convert.ToDouble(str.Substring(SepIndex[1] + 1, SepIndex[2] - SepIndex[1] - 1));
                    pickY = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));
                    pickT = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                }
                else
                {
                  pickX = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));
                  pickY = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                  pickT = Convert.ToDouble(str.Substring(SepIndex[4] + 1, SepIndex[5] - SepIndex[4] - 1));
                }
               

                tempMessage.messType = mesEnd;
                tempMessage.X = pickX;
                tempMessage.Y = pickY;
                tempMessage.T = pickT;
                InMessageQueue(tempMessage);  //消息入队
                _logger.Info(DateTime.Now.ToString() + $" :  取料对位成功消息已入队列，"+ str);
            }
            else if (i_status == (int)MsgStatus.Fail)  //搜索失败
            {
                tempMessage.messType = mesFail;
                InMessageQueue(tempMessage);
                _logger.Info(DateTime.Now.ToString() + $" :  取料对位失败消息已入队列，" + str);

            }
            return;
        }

        #endregion

        #region 解析相机组装字符串
        void ParcePackageMess(string str, List<int> SepIndex)
        {
            if (SepIndex.Count < 3)
                return;
            int i_cameraInd = Convert.ToInt32(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));
            int stdSeparatorNum = (i_cameraInd == (int)Camera.BottomCamera) ? 6 : 7;// 6 : 9;
            if (SepIndex.Count != stdSeparatorNum)
            {
                return;
            }
            //状态信号
            string status = str.Substring(SepIndex[SepIndex.Count - 1] + 1);
            int i_status = Convert.ToInt32(status);

            MessageStruct tempMessage = new MessageStruct();
            //下相机
            if (i_status == (int)MsgStatus.End && i_cameraInd == (int)Camera.BottomCamera)
            {
                tempMessage.X = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                tempMessage.Y = Convert.ToDouble(str.Substring(SepIndex[4] + 1, SepIndex[5] - SepIndex[4] - 1));
                tempMessage.T = 0;
                tempMessage.messType = MessType.CMD_Answer_BottomPackageEnd;
                InMessageQueue(tempMessage);  //消息入队
            }
            else if (i_status == (int)MsgStatus.Fail && i_cameraInd == (int)Camera.BottomCamera)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_BottomPackageFail;
                InMessageQueue(tempMessage);
            }
            //上相机
            else if (i_status == (int)MsgStatus.End && i_cameraInd == (int)Camera.UpCamera)  
            {
                //获取偏移量
                double offsetX = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                double offsetY = Convert.ToDouble(str.Substring(SepIndex[4] + 1, SepIndex[5] - SepIndex[4] - 1));
                double offsetT = Convert.ToDouble(str.Substring(SepIndex[5] + 1, SepIndex[6] - SepIndex[5] - 1));
                //if (ParamSetMgr.GetInstance().GetIntParam("数据记录")==1)
                //{
                //    string strMsg = $"{offsetX},{offsetY}\n";
                //    File.AppendAllText(@"E:\设备数据\拍镜筒静态重复性精度.csv", strMsg);
                //}



                /****************************/
                tempMessage.messType = MessType.CMD_Answer_PackageEnd;
                tempMessage.offsetX = offsetX;
                tempMessage.offsetY = offsetY;
                tempMessage.offsetT = offsetT;
                InMessageQueue(tempMessage);               
            }
            else if (i_status == (int)MsgStatus.Fail && i_cameraInd == (int)Camera.UpCamera)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_PackageFail;
                InMessageQueue(tempMessage);
            }
            return;
        }

        void ParcePackageMess1(string str, List<int> SepIndex)
        {
            if (SepIndex.Count < 3)
                return;
            int i_cameraInd = Convert.ToInt32(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));
            int stdSeparatorNum = (i_cameraInd == (int)Camera.BottomCamera) ? 3 : 9;
            if (SepIndex.Count != stdSeparatorNum)
            {
                return;
            }
            //状态信号
            string status = str.Substring(SepIndex[6-1] + 1,1);
            int i_status = Convert.ToInt32(status);

            MessageStruct tempMessage = new MessageStruct();
            //下相机
            if (i_status == (int)MsgStatus.End && i_cameraInd == (int)Camera.BottomCamera)
            {
                tempMessage.messType = MessType.CMD_Answer_BottomPackageEnd;
                InMessageQueue(tempMessage);  //消息入队
            }
            else if (i_status == (int)MsgStatus.Fail && i_cameraInd == (int)Camera.BottomCamera)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_BottomPackageFail;
                InMessageQueue(tempMessage);
            }
            //上相机
            else if (i_status == (int)MsgStatus.End && i_cameraInd == (int)Camera.UpCamera)
            {
                //获取偏移量
                double offsetX = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));
                double offsetY = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                double offsetT = Convert.ToDouble(str.Substring(SepIndex[4] + 1, SepIndex[5] - SepIndex[4] - 1));


                double X = Convert.ToDouble(str.Substring(SepIndex[6] + 1, SepIndex[7] - SepIndex[6] - 1));
                double Y = Convert.ToDouble(str.Substring(SepIndex[7] + 1, SepIndex[8] - SepIndex[7] - 1));
                double T = Convert.ToDouble(str.Substring(SepIndex[8] + 1));

                string strMsg = $"{X},{Y},{T}\n";

                File.AppendAllText(@"E:\拍镜筒重复性精度.csv", strMsg);

                //tempMessage.messType = MessType.CMD_Answer_PackageEnd;
                //tempMessage.offsetX = offsetX;
                //tempMessage.offsetY = offsetY;
                //tempMessage.offsetT = offsetT;
                //InMessageQueue(tempMessage);
            }
            else if (i_status == (int)MsgStatus.Fail && i_cameraInd == (int)Camera.UpCamera)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_PackageFail;
                InMessageQueue(tempMessage);
            }
            return;
        }
        #endregion

        #region 解析点胶旋转中心标定
        void ParceCalibDispCenterMess(string str, List<int> SepIndex)
        {
           if (SepIndex.Count == 4)  //返回偏移量和状态信号
            {
                //状态信号
                string status = str.Substring(SepIndex[SepIndex.Count - 1] + 1);
                int i_status = Convert.ToInt32(status);
                MessageStruct tempMessage = new MessageStruct();
                if (i_status == (int)MsgStatus.Continue) //继续标定
                {
                    //X,Y偏移量
                    double offsetX = Convert.ToDouble(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));
                    double offsetY = Convert.ToDouble(str.Substring(SepIndex[1] + 1, SepIndex[2] - SepIndex[1] - 1));
                    double offsetT = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));


                    tempMessage.messType = MessType.CMD_Answer_CalibOffset;
                    tempMessage.offsetX = offsetX;
                    tempMessage.offsetY = offsetY;
                    tempMessage.offsetT = offsetT;
                    InMessageQueue(tempMessage);  //消息入队
                }
                else if (i_status == (int)MsgStatus.End)  //完成标定
                {
                    tempMessage.messType = MessType.CMD_Answer_CalibEnd;
                    InMessageQueue(tempMessage);
                }
                else if (i_status == (int)MsgStatus.Fail)  //搜索失败
                {
                    tempMessage.messType = MessType.CMD_Answer_CalibFail;
                    InMessageQueue(tempMessage);
                }
            }
        }

            #endregion

        #region 解析点胶基准位注册
            void ParceDispBasePoint(string str, List<int> SepIndex)
        {
            if (SepIndex.Count != 3)
                return;
            string status = str.Substring(SepIndex[2] + 1);
            int i_status = Convert.ToInt32(status);

            MessageStruct tempMessage = new MessageStruct();
            
            if (i_status == (int)MsgStatus.End)
            {
                tempMessage.X = Convert.ToDouble(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));
                tempMessage.Y = Convert.ToDouble(str.Substring(SepIndex[1] + 1, SepIndex[2] - SepIndex[1] - 1));
                //tempMessage.T = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));
                tempMessage.messType = MessType.CMD_Answer_DispGetBasePointEnd;
                InMessageQueue(tempMessage);  //消息入队
            }
            else if (i_status == (int)MsgStatus.Fail)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_DispGetBasePointFail;
                InMessageQueue(tempMessage);
            }
           
            return;


        }

        #endregion

        #region 解析点胶关键点
        //void ParceDispKeyPoint1(string str, List<int> SepIndex)
        //{
        //    if (SepIndex.Count != 11)
        //        return;
        //    string status = str.Substring(SepIndex[SepIndex.Count - 1] + 1);
        //    int i_status = Convert.ToInt32(status);

        //    MessageStruct tempMessage = new MessageStruct();
        //    tempMessage.dispKeyPoints = new List<Point2d>();
        //    Point2d tempPoint = new Point2d();
        //    if (i_status == (int)MsgStatus.End)
        //    {
        //        //解析点胶关键点数据
        //        tempPoint.x = Convert.ToDouble(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));
        //        tempPoint.y = Convert.ToDouble(str.Substring(SepIndex[1] + 1, SepIndex[2] - SepIndex[1] - 1));
        //        tempMessage.dispKeyPoints.Add(tempPoint);
        //        for (int i = 0; i < 8; i=i+2)
        //        {
        //            tempPoint.x = Convert.ToDouble(str.Substring(SepIndex[2+i] + 1, SepIndex[2+i+1] - SepIndex[2+i] - 1));
        //            tempPoint.y = Convert.ToDouble(str.Substring(SepIndex[2 + i + 1] + 1, SepIndex[2 + i + 2] - SepIndex[2 + i + 1] - 1));
        //            tempMessage.dispKeyPoints.Add(tempPoint);
        //        }

        //        tempMessage.messType = MessType.CMD_Answer_DispGetKeyPointEnd;
        //        InMessageQueue(tempMessage);  //消息入队
        //    }
        //    else if (i_status == (int)MsgStatus.Fail)  //搜索失败
        //    {
        //        tempMessage.messType = MessType.CMD_Answer_DispGetKeyPointFail;
        //        InMessageQueue(tempMessage);
        //    }

        //    return;
        //}
        void ParceDispKeyPoint(string str, List<int> SepIndex)
        {
            List<int> tempIndex = new List<int>();
            List<int> arcIndex = new List<int>();
            List<int> lineIndex = new List<int>();

            string arc = "ARC";
            string line = "LINE";
            getAllSeparatorInd(str, arc, out arcIndex);
            getAllSeparatorInd(str, line, out lineIndex);
            if (arcIndex.Count == 0 && lineIndex.Count == 0)  //若没圆弧和直线数据
            {
                return;
            }

            string status = str.Substring(SepIndex[SepIndex.Count - 1] + 1); //状态值
            int i_status = Convert.ToInt32(status);

            MessageStruct tempMessage = new MessageStruct();
            tempMessage.dispRotationsPoint = new Point2d();


            //tempMessage.dispArcPoints = new List<List<Point2d>>();
            //tempMessage.dispLinePoints = new List<List<Point2d>>();
            tempMessage.dispArcs = new List<Arc>();
            tempMessage.dispLines = new List<Line>();

            //tempMessage.dispKeyPoints = new List<Point2d>();
            Point2d tempPoint = new Point2d();

            if (i_status == (int)MsgStatus.End)
            {
                //解析vcm旋转中心数据
                string str_RCenter = "RCenter";
                getAllSeparatorInd(str, str_RCenter, out tempIndex);
                if (tempIndex.Count != 1)  //若没有旋转中心或旋转中心发送不对
                {
                    return;
                }
                else
                {
                    tempMessage.dispRotationsPoint.x = Convert.ToDouble(str.Substring(SepIndex[1] + 1, SepIndex[2] - SepIndex[1] - 1));
                    tempMessage.dispRotationsPoint.y = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));
                }

                //解析圆弧数据
                if (arcIndex.Count > 0)
                {
                    for (int i = 0; i < arcIndex.Count; i++)
                    {
                        //List<Point2d> tempListPoint = new List<Point2d>();
                        Arc tempArc = new Arc();

                        int startInd = arcIndex[i] ;// + arc.Length+ 1
                        string subString = str.Substring(startInd);
                        getAllSeparatorInd(subString, ",", out tempIndex);
                        int Sindex, Strlength;
                        for (int pointInd = 0; pointInd < 6; pointInd+=2)
                        {
                            Sindex = tempIndex[pointInd] + 1;
                            Strlength = tempIndex[pointInd + 1] - tempIndex[pointInd] - 1;
                            tempPoint.x = Convert.ToDouble(subString.Substring(Sindex, Strlength));
                            Sindex = tempIndex[pointInd + 1] + 1;
                            Strlength = (tempIndex[pointInd + 2] - tempIndex[pointInd + 1] - 1);
                            tempPoint.y = Convert.ToDouble(subString.Substring(Sindex, Strlength));
                            switch (pointInd)
                            {
                                case 0:
                                    tempArc.roundCenter = new Point2d(tempPoint.x, tempPoint.y);
                                    break;
                                case 2:
                                    tempArc.startPoint = new Point2d(tempPoint.x, tempPoint.y);
                                    break;
                                case 4:
                                    tempArc.endPoint = new Point2d(tempPoint.x, tempPoint.y);
                                    break;
                                default:
                                    break;
                            }
                            //tempListPoint.Add(tempPoint);
                        }
                        //若该弧是一个圆（起点与终点重合）
                        double absX = Math.Abs(tempArc.startPoint.x - tempArc.endPoint.x);
                        double absY = Math.Abs(tempArc.startPoint.y - tempArc.endPoint.y);

                        if (absX < 0.000001 && absY < 0.000001)
                        {
                            Point2d center = tempArc.roundCenter;
                            Point2d startPot = tempArc.startPoint;
                            Point2d endPot = tempArc.endPoint;
                            //double rad = 0.05 / 180 * Math.PI;
                            ////double radius = Math.Sqrt((center.x - startPot.x) * (center.x - startPot.x) +
                            ////    (center.y - startPot.y) * (center.y - startPot.y));
                            ////起点逆时针旋转
                            //startPot.x = center.x + (startPot.x - center.x) * Math.Cos(rad) - (startPot.y - center.y) * Math.Sin(rad);
                            //startPot.y = center.y + (startPot.y - center.y) * Math.Cos(rad) + (startPot.x - center.x) * Math.Sin(rad);
                            ////终点顺时针旋转
                            //endPot.x = center.x + (startPot.x - center.x) * Math.Cos(rad) + (startPot.y - center.y) * Math.Sin(rad);
                            //endPot.y = center.y + (startPot.y - center.y) * Math.Cos(rad) - (startPot.x - center.x) * Math.Sin(rad);
                            tempArc.startPoint = startPot;
                            tempArc.endPoint = endPot;
                        }
                        tempMessage.dispArcs.Add(tempArc);
                    }
                }

                //解析直线数据
                //tempListPoint.Clear();
                if (lineIndex.Count > 0)
                {
                    for (int i = 0; i < lineIndex.Count; i++)
                    {
                        //List<Point2d> tempListPoint = new List<Point2d>();
                        Line tempLine = new Line();

                        int startInd = lineIndex[i] + line.Length + 1;
                        string subString = str.Substring(startInd);
                        getAllSeparatorInd(subString, ",", out tempIndex);

                        for (int pointInd = 0; pointInd < 4; pointInd += 2)
                        {
                            tempPoint.x = Convert.ToDouble(subString.Substring
                                (tempIndex[pointInd] + 1, tempIndex[pointInd + 1] - tempIndex[pointInd] - 1));
                            tempPoint.y = Convert.ToDouble(subString.Substring
                                (tempIndex[pointInd + 1] + 1, tempIndex[pointInd + 2] - tempIndex[pointInd + 1] - 1));
                            switch (pointInd)
                            {
                                case 0:
                                    tempLine.startPoint = new Point2d(tempPoint.x, tempPoint.y);
                                    break;
                                case 2:
                                    tempLine.endPoint = new Point2d(tempPoint.x, tempPoint.y);
                                    break;
                                default:
                                    break;
                            }
                            //tempListPoint.Add(tempPoint);
                        }
                        tempMessage.dispLines.Add(tempLine);
                    }
                }

                
                tempMessage.messType = MessType.CMD_Answer_DispGetKeyPointEnd;
                InMessageQueue(tempMessage);  //消息入队
                _logger.Info(DateTime.Now.ToString() + $" :  点胶数据回传成功并已入队列 " + str);

            }
            else if (i_status == (int)MsgStatus.Fail)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_DispGetKeyPointFail;
                InMessageQueue(tempMessage);
            }

            return;
        }
        #endregion

        #region 解析不同吸嘴对应的keyence快门
        void ParceShutterSpeed(string str, List<int> SepIndex)
        {
            if (SepIndex.Count != 17)
                return;
            string status = str.Substring(SepIndex[SepIndex.Count-1] + 1);
            int i_status = Convert.ToInt32(status);

            MessageStruct tempMessage = new MessageStruct();
            tempMessage.shutterSpeed = new List<double>();
            if (i_status == (int)MsgStatus.End)
            {
                for (int i = 1; i < 17; i++)
                {
                    double shutterVal = Convert.ToDouble(str.Substring(SepIndex[i-1] + 1, SepIndex[i] - SepIndex[i - 1] - 1));
                    tempMessage.shutterSpeed.Add(shutterVal);
                }
                
                tempMessage.messType = MessType.CMD_Answer_PickGetShutterEnd;
                InMessageQueue(tempMessage);  //消息入队
            }
            else if (i_status == (int)MsgStatus.Fail)  //失败
            {
                tempMessage.messType = MessType.CMD_Answer_PickGetShutterFail;
                InMessageQueue(tempMessage);
            }

            return;
        }
        #endregion

        #region 解析贴合重复性精度
        #region 解析贴合上相机取料对位
        void ParceBondingPickMess(string str, List<int> SepIndex, int stdSeparatorNum, MessType mesEnd, MessType mesFail)
        {
            if (SepIndex.Count != stdSeparatorNum)
                return;

            //状态信号
            string status = str.Substring(SepIndex[stdSeparatorNum - 1] + 1);
            int i_status = Convert.ToInt32(status);
            MessageStruct tempMessage = new MessageStruct();
            if (i_status == (int)MsgStatus.End)
            {
                //吸嘴号（略）
                //X,Y,T抓取绝对坐标
                double pickX, pickY, pickT;
                if (mesEnd == MessType.CMD_Answer_DispPickEnd)
                {
                    pickX = Convert.ToDouble(str.Substring(SepIndex[1] + 1, SepIndex[2] - SepIndex[1] - 1));
                    pickY = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));
                    pickT = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                }
                else
                {
                    pickX = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));
                    pickY = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                    pickT = Convert.ToDouble(str.Substring(SepIndex[4] + 1, SepIndex[5] - SepIndex[4] - 1));
                }


                tempMessage.messType = mesEnd;
                tempMessage.X = pickX;
                tempMessage.Y = pickY;
                tempMessage.T = pickT;
                InMessageQueue(tempMessage);  //消息入队
                _logger.Info(DateTime.Now.ToString() + $" :  取料对位成功消息已入队列，" + str);
            }
            else if (i_status == (int)MsgStatus.Fail)  //搜索失败
            {
                tempMessage.messType = mesFail;
                InMessageQueue(tempMessage);
                _logger.Info(DateTime.Now.ToString() + $" :  取料对位失败消息已入队列，" + str);

            }
            return;
        }
        #endregion

        #region 解析贴合组立
        void ParceBondingPackageMess(string str, List<int> SepIndex)
        {
            if (SepIndex.Count < 3)
                return;
            int i_cameraInd = Convert.ToInt32(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));
            int stdSeparatorNum = (i_cameraInd == (int)Camera.BottomCamera) ? 6 : 7;
            if (SepIndex.Count != stdSeparatorNum)
            {
                return;
            }
            //状态信号
            string status = str.Substring(SepIndex[SepIndex.Count - 1] + 1);
            int i_status = Convert.ToInt32(status);

            MessageStruct tempMessage = new MessageStruct();
            //下相机
            if (i_status == (int)MsgStatus.End && i_cameraInd == (int)Camera.BottomCamera)
            {
                tempMessage.messType = MessType.CMD_Answer_贴合_下相机对位完成;
                InMessageQueue(tempMessage);  //消息入队
            }
            else if (i_status == (int)MsgStatus.Fail && i_cameraInd == (int)Camera.BottomCamera)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_贴合_下相机对位失败;
                InMessageQueue(tempMessage);
            }
            //上相机
            else if (i_status == (int)MsgStatus.End && i_cameraInd == (int)Camera.UpCamera)
            {
                //获取偏移量
                double offsetX = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                double offsetY = Convert.ToDouble(str.Substring(SepIndex[4] + 1, SepIndex[5] - SepIndex[4] - 1));
                double offsetT = Convert.ToDouble(str.Substring(SepIndex[5] + 1, SepIndex[6] - SepIndex[5] - 1));

                tempMessage.messType = MessType.CMD_Answer_贴合_组立完成;
                tempMessage.offsetX = offsetX;
                tempMessage.offsetY = offsetY;
                tempMessage.offsetT = offsetT;
                InMessageQueue(tempMessage);

              

                /**********************************/
            }
            else if (i_status == (int)MsgStatus.Fail && i_cameraInd == (int)Camera.UpCamera)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_贴合_组立失败;
                InMessageQueue(tempMessage);
            }
            return;
        }
        #endregion

        //解析贴合偏心度
        void ParceBondingOffset(string str, List<int> SepIndex)
        {
            int stdSeparatorNum = 3;
            if (SepIndex.Count < stdSeparatorNum)
            {
                return;
            }
            //次数
            int mode = Convert.ToInt16(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));

            //状态信号
            string status = str.Substring(SepIndex[SepIndex.Count - 1] + 1);
            int i_status = Convert.ToInt32(status);

            MessageStruct tempMessage = new MessageStruct();
            //上相机
            if (i_status == (int)MsgStatus.End && mode == 1)
            {
                tempMessage.messType = MessType.CMD_Answer_贴合_求偏心度第一次完成;
               
                InMessageQueue(tempMessage);
            }
            else if (i_status == (int)MsgStatus.Fail && mode == 1)
            {
                tempMessage.messType = MessType.CMD_Answer_贴合_求偏心度第一次失败;

                InMessageQueue(tempMessage);
            }
            else if (i_status == (int)MsgStatus.End && mode==2)
            {
                //获取偏移量
                double x1 = Convert.ToDouble(str.Substring(SepIndex[2] + 1, SepIndex[3] - SepIndex[2] - 1));
                double y1 = Convert.ToDouble(str.Substring(SepIndex[3] + 1, SepIndex[4] - SepIndex[3] - 1));
                double x2 = Convert.ToDouble(str.Substring(SepIndex[4] + 1, SepIndex[5] - SepIndex[4] - 1));
                double y2 = Convert.ToDouble(str.Substring(SepIndex[5] + 1, SepIndex[6] - SepIndex[5] - 1));

                tempMessage.messType = MessType.CMD_Answer_贴合_求偏心度完成;
                tempMessage.X1 = x1;
                tempMessage.Y1 = y1;
                tempMessage.X2= x2;
                tempMessage.Y2 = y2;
            
                InMessageQueue(tempMessage);
            }
            else if (i_status == (int)MsgStatus.Fail && mode == 2)  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_贴合_求偏心度失败;
                InMessageQueue(tempMessage);
            }
            return;
        }

        void ParceCollectPos(string str, List<int> SepIndex)
        {
            int stdSeparatorNum = 2;
            if (SepIndex.Count < stdSeparatorNum)
            {
                return;
            }
           

            //状态信号
            string status = str.Substring(SepIndex[SepIndex.Count - 1] + 1);
            int i_status = Convert.ToInt32(status);
            MessageStruct tempMessage = new MessageStruct();
            //上相机（CCD3)
            if (i_status == (int)MsgStatus.End )
            {
                //获取偏移量
                double x1 = Convert.ToDouble(str.Substring(SepIndex[0] + 1, SepIndex[1] - SepIndex[0] - 1));
                double y1 = Convert.ToDouble(str.Substring(SepIndex[1] + 1, SepIndex[2] - SepIndex[1] - 1));
              

                tempMessage.messType = MessType.CMD_Answer_收料_获取位置成功;
                tempMessage.X = x1;
                tempMessage.Y = y1;

                InMessageQueue(tempMessage);
            }
            else if (i_status == (int)MsgStatus.Fail )  //搜索失败
            {
                tempMessage.messType = MessType.CMD_Answer_收料_获取位置失败;
                InMessageQueue(tempMessage);
            }
            return;
        }

        //解析贴合取料的注册
        #region 解析取料注册字符串
        void ParceRegisterBondingPickMess(string str, List<int> SepIndex, int stdSeparatorNum, int comInd)
        {
            if (SepIndex.Count != stdSeparatorNum) return;
            //状态信号
            string status = str.Substring(SepIndex[SepIndex.Count - 1] + 1);
            int i_status = Convert.ToInt32(status);
            string strMsg = "贴合验证取料，";
            //switch (comInd)
            //{
            //    case (int)SendCommandType.CMD_Send_pickBarrelRegister:
            //        strMsg = "点胶相机取Barrel";
            //        break;
            //    case (int)SendCommandType.CMD_Send_PickUpRegister:
            //        strMsg = "上相机取料";
            //        break;
            //    case (int)SendCommandType.CMD_Send_PackUpRegister:
            //        strMsg = "上相机放料";
            //        break;
            //    default:
            //        break;
            //}
            if (i_status == (int)MsgStatus.End) //完成注册
            {
                //提醒完成基准位置注册
                MessageBox.Show(strMsg + "基准位置注册成功");
            }
            else if (i_status == (int)MsgStatus.Fail)  //注册失败
            {
                //提醒基准位置注册失败
                MessageBox.Show(strMsg + "基准位置注册失败");
            }
        }
        #endregion
        #endregion
    }


}