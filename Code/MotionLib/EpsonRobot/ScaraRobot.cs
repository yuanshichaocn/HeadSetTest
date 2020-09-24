using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using BaseDll;
using System.IO;
using System.Windows.Forms;

namespace EpsonRobot
{
    public class ScaraRobot : IScaraRobot, IDisposable
    {
        private readonly ILog _logger = LogManager.GetLogger(nameof(ScaraRobot));
        private readonly object _disposingLock = new object();
        private static readonly object _syncRoot = new object();
        private bool _isDisposed;
        private static ScaraRobot _scaraRobot;

        private RobotTcpClient _cmdClient;
        private RobotTcpClient _statesClient;
        private readonly string _cmdClientConfigPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Config\cmdClient.cfg";
        private readonly string _statesClientConfigPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Config\statesClient.cfg";
        public bool InPos { get; private set; }
        public delegate void ChangedPosHandle(Coordinate coordinate);
        public event ChangedPosHandle ChangedPosEvent=null;
        public Coordinate CurrentPosition { get => coordinate; private set
          {
                coordinate = value;
                // if(coordinate!=value)
                {
                    if (ChangedPosEvent != null)
                        ChangedPosEvent(value);
                }
                
            }
        }
        private Coordinate coordinate;
        public bool IsInit
        {
            private set;
            get;
        }
        public delegate void ChangedIoOutStateHandle(UInt32 state);
        public event ChangedIoOutStateHandle ChangedIoOutStateEvent=null;
        public UInt32 dOut
        {
            get => dOutIo;
            private set
            {
                if(value!= dOutIo)
                {
                    if (ChangedIoOutStateEvent != null)
                        ChangedIoOutStateEvent(value);
                }
                dOutIo = value;
            }
        }
        private UInt32 dOutIo;

        public delegate void ChangedIoInStateHandle(UInt32 state);
        public event ChangedIoInStateHandle ChangedIoInStateEvent=null;
        public UInt32 dIn
        {
            get => dInIo;
            private set
            {
                if (value != dInIo)
                {
                    if (ChangedIoInStateEvent != null)
                        ChangedIoInStateEvent(value);
                }
                dInIo = value;
            }
        }
        private UInt32 dInIo;
        private object lockObj = new object();
        public delegate void ChangedHandStateHandle(HandDirection currhandDirection);
        public event ChangedHandStateHandle ChangedHandStateEvent=null;
        public HandDirection  CurrentHandDirection
        {
            private set
            {
               // if(handDirection!=value)
                {
                    if (ChangedHandStateEvent != null)
                        ChangedHandStateEvent(value);
                }
                handDirection = value;
            }
            get
            {
                return handDirection;
            }
        }
       bool  bIsActionPause=false;
        public bool IsActionPause
        {
            private set
            {
                bIsActionPause = value;
            }
            get => bIsActionPause;




        }

       private HandDirection handDirection = new HandDirection();
        public bool GetOutBit(int index)
        {
            lock(lockObj)
            {
                return BitOperat.GetBit32(dOut, (byte)index) == 1;
            }
        }
        public bool GetInBit(int index)
        {
            lock (lockObj)
            {
                return BitOperat.GetBit32(dIn, (byte)index) == 1;
            }
        }


        public static ScaraRobot GetInstance()
        {
            lock (_syncRoot)
            {
                if (_scaraRobot == null)
                    _scaraRobot = new ScaraRobot();

                return _scaraRobot;
            }
        }

        private ScaraRobot()
        {

        }

        ~ScaraRobot()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            lock (_disposingLock)
            {
                if (_isDisposed)
                    return;

                _cmdClient?.Close();
                _statesClient?.Close();

                if (disposing)
                {

                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Exit();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public Coordinate GetCurrentImmediately()
        {
            Coordinate coordinate = new Coordinate();
            if (_cmdClient.bExit || !_cmdClient.bIsConnected)
                return coordinate;


            string strRobotAgl = _cmdClient.SendCommand($"007x@0");
            var arrayagl = strRobotAgl.Split(',');
            if (arrayagl.Length < 4)
                return coordinate;
            int index = 0;
            coordinate.X = arrayagl[index++].ToDouble();
            coordinate.Y = arrayagl[index++].ToDouble();
            coordinate.Z = arrayagl[index++].ToDouble();
            string strUpos = arrayagl[index++];
            int indexpos = strUpos.IndexOf("~");

            coordinate.U = strUpos.Substring(0,indexpos).ToDouble();
            return coordinate;
        }

        bool bCmdClinetOpen = false;
        bool bStateClientOpen = false;
        public bool Init(out string msg)
        {
            try
            {
                msg = "";
                
                if (IsInit)
                    return false;
                IsInit = false;
                #region robot client

                _cmdClient = new RobotTcpClient();

                    _cmdClient.Init(_cmdClientConfigPath);
                    _cmdClient.Connect();
                    bCmdClinetOpen = _cmdClient.bIsConnected;

                    string strRobotPos = _cmdClient.SendCommand($"003x@0");
                    string[] arraypos = strRobotPos.Split(',');
                    if (arraypos.Length < 4)
                        return false;
                
 
                    _statesClient = new RobotTcpClient();
                    _statesClient.Init(_statesClientConfigPath);
                    _statesClient.Connect();
                    bStateClientOpen = _statesClient.bIsConnected;
                    Coordinate CurrentGetPositionFormTcp = new Coordinate();


                    string strRobotAgl = _cmdClient.SendCommand($"005x@0");
                    var arrayagl = strRobotAgl.Split(',');
                    if (arrayagl.Length < 4)
                        return false;

                    GetCurrentImmediately();
                    double a1 = arrayagl[0].ToDouble();
                    double a2 = arrayagl[1].ToDouble();
                    double x = arraypos[0].ToDouble();
                    double y = arraypos[1].ToDouble();
                    double L1 = 0, L2 = 0;
                    CalibScara.GetRobotAlarm(a1, a2, x, y, ref L1, ref L2);
                    #region 更新状态的线程
                    Task.Factory.StartNew(() =>
                    {
                        double xx = 0, yy = 0;
                        int failedCount = 0;
                        int index = 0;
                        while (!_statesClient.bExit)
                        {
                       
                            try
                            {
                                if (!(bool)(_statesClient?.Client?.Connected))
                                    return;
                                    var response = _statesClient.SendCommand("GetStates");
                                var array = response.Split(',');
                                if (array.Length >= 6)
                                {
                                    lock (lockObj)
                                    {
                                        index = 0;
                                        InPos = (array[index++] == "1");
                                        //   if (CurrentPosition == null)
                                        //      CurrentPosition = new Coordinate();

                                        //  CalibScara.TransXY(array[1].ToDouble(), array[2].ToDouble(), L1, L2, ref xx , ref yy);
                                        CurrentGetPositionFormTcp.X = array[index++].ToDouble();
                                        CurrentGetPositionFormTcp.Y = array[index++].ToDouble();
                                        CurrentGetPositionFormTcp.Z = array[index++].ToDouble();
                                        CurrentGetPositionFormTcp.U = array[index++].ToDouble();
                                        CurrentPosition = CurrentGetPositionFormTcp;
                                        CurrentHandDirection = array[index++].ToDouble() <= 0 ? HandDirection.Lefty : HandDirection.Right;
                                        IsActionPause = array[index++].ToByte() > 0 ? true : false;
                                        dOut = UInt32.Parse(array[index++]);
                                        dIn = UInt32.Parse(array[index++]) | UInt32.Parse(array[index++]) << 16;

                                    }
                                }

                                else
                                {
                                    _logger.Warn("无效的Response:" + response);
                                }
                            }
                            catch (Exception ex)
                            {
                                failedCount++;
                                _logger.Warn("GetStates通讯失败:" + ex.Message);

                                if (failedCount > 5)
                                {
                                    IsInit = false;
                                    throw ex;
                                }

                            }
                            finally
                            {
                                System.Threading.Thread.Sleep(3);
                            }
                        }
                        if (_statesClient?.Client?.Connected == false)
                        {
                            IsInit = false;
                        }
                    });
                    #endregion




                #endregion

                msg = "";
                IsInit = true;
                return true;
            }
            catch (Exception ex)
            {
                //_logger.Error("机器人初始化失败:" + ex.Message);
                _cmdClient?.Close();
                _statesClient?.Close();

                throw ex;
            }
        }
        public void SeverOn(bool bSevero)
        {
            if(bSevero)
              _cmdClient.SendCommand("100motor on");
            else
                _cmdClient.SendCommand("100motor off");

        }
        public void ResetRobot()
        {
            _cmdClient.SendCommand("100Reset");
        }
        public void SetVel(double vel)
        {
            if(vel> 100 || vel< 0)
            {
                MessageBox.Show("机器人速度设置不合理", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            _cmdClient.SendCommand($"100Speed {vel}");
        }
        public void SetAcc(double acc)
        {
            if (acc > 100 || acc < 0)
            {
                MessageBox.Show("机器人速度设置不合理", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            _cmdClient.SendCommand($"100Accel {acc},{acc}");
        }
        public void Exit()
        {
            if(_statesClient!=null)
             _statesClient.bExit = true;
            if (_cmdClient != null)
                _cmdClient.bExit = true;
            _statesClient?.SendCommand("Exit");
            _cmdClient?.SendCommand($"100Exit");
          
        }

       
        public bool Jump(Coordinate coordinate, HandDirection direction, double limz, bool bCheckHandleSys=false)
        {
            StringBuilder sb = new StringBuilder();
            if (coordinate.U >= 360)
                coordinate.U = coordinate.U - 360;
            if (coordinate.U <= -360)
                coordinate.U = coordinate.U + 360;
            sb.Append($"100Till MemSw(1) = On;jump XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            if (direction != HandDirection.Default)
                sb.Append($" /{ Enum.GetName(typeof(HandDirection), direction)[0]}");
            if (limz != 0)
                sb.Append($" LimZ {limz}");
            sb.Append(" Till;MemOff 1");
            var result = _cmdClient.SendCommand(sb.ToString());
            if (result != "0")
            {
                _cmdClient.SendCommand("100MemOff 1");
                //      _cmdClient.SendCommand("100ERsume");
                return false;
            }
            return true;
             
        }
        public bool Go( Coordinate coordinate, HandDirection direction ,bool  bCheckHandleSys =false )
        {
            if(bCheckHandleSys)
            {
                if(!CheckRLSystem(direction))
                {
                    _logger.Info("Robot Go  机器人的手势不一致，停止运动");
                    return false;
                }
            }
            StringBuilder sb = new StringBuilder();
            if (coordinate.U >= 360)
                coordinate.U = coordinate.U - 360;
            if (coordinate.U <= -360)
                coordinate.U = coordinate.U + 360;
            sb.Append($"100Till MemSw(1)= On;GO XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            if (direction != HandDirection.Default)
                sb.Append($" /{ Enum.GetName(typeof(HandDirection), direction)[0]}");
            sb.Append(" Till;MemOff 1");
            var result = _cmdClient?.SendCommand(sb.ToString());
            if (result != "0")
            {
                _cmdClient?.SendCommand("100MemOff 1");
     
                return false;
            }
            return true;
        }
        public bool Move(Coordinate coordinate, HandDirection direction, bool bCheckHandleSys)
        {
            if (bCheckHandleSys)
            {
                if (!CheckRLSystem(direction))
                {
                    _logger.Info("Robot Move  机器人的手势不一致，停止运动");
                    return false;
                }
            }
            StringBuilder sb = new StringBuilder();
            if (coordinate.U >= 360)
                coordinate.U = coordinate.U - 360;
            if (coordinate.U <= -360)
                coordinate.U = coordinate.U + 360;
            sb.Append($"100Till MemSw(1) = On;Move XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            if (direction != HandDirection.Default)
                sb.Append($" /{ Enum.GetName(typeof(HandDirection), direction)[0]}");
            sb.Append(" Till;MemOff 1");
            var result = _cmdClient.SendCommand(sb.ToString());
            if (result != "0")
            {

                _cmdClient?.SendCommand("100MemOff 1");
                // _cmdClient.SendCommand("100ERsume");
                return false;
            }
            return true;
               
        }
        public void Stop()
        {
            //  var result = _cmdClient?.SendCommand("100Stop");
            Task.Run(() => {
                _cmdClient?.SendCommand("100MemOn 1");
               _cmdClient?.SendCommand("100MemOff 1");
            });
        }
        public void SetStopActionFlag()
        {
            Task.Run(() => { var result = _statesClient?.SendCommand("100MemOn 1", true); });
    
        }
        public void ReasetStopActionFlag()
        {
            Task.Run(() => {
                var result = _statesClient?.SendCommand("100MemOff 1", true);
            });

        }


        public bool Home()
        {
            var result = _cmdClient?.SendCommand("100Home");
            if (result != "0")
            {
                _cmdClient?.SendCommand("100MemOff 1");
       
                return false;
            }
            return true;
              
        }
        public void SetOutput(int index, bool value)
        {
            var cmd = value ? "On" : "Off";
            var result = _cmdClient.SendCommand($"100{ cmd }({index})");
            //if (result != "0")
            //    _cmdClient.SendCommand("100ERsume");
        }

        public bool ReadOutput(int index)
        {
            var result = _cmdClient.SendCommand($"002{index}@0");
            return result[0] == '1';
        }
        public bool ReadInput(int index)
        {
            var result = _cmdClient.SendCommand($"001{index}@0");
            return result[0] == '1';
        }

        public void SendCommand(string cmd)
        {
            _cmdClient.SendCommand(cmd);
        }
        public bool CheckRLSystem(HandDirection handDirection)
        {
            //将去位置 姿势不同 提示检查
            if (handDirection != ScaraRobot.GetInstance().CurrentHandDirection)
                return false;
            return true;
        }


    }
}
