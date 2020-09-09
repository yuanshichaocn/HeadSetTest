using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using BaseDll;



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
            Dispose(true);
            GC.SuppressFinalize(this);
        }
      
        public bool Init(out string msg)
        {
            try
            {
                msg = "";
                IsInit = false;
                #region robot client
                _cmdClient = new RobotTcpClient();
                _cmdClient.Init(_cmdClientConfigPath);
                _cmdClient.Connect();

                _statesClient = new RobotTcpClient();
                _statesClient.Init(_statesClientConfigPath);
                _statesClient.Connect();
                Coordinate CurrentGetPositionFormTcp= new Coordinate();

                string strRobotPos = _cmdClient.SendCommand($"003x@0");
                var arraypos = strRobotPos.Split(',');
                if (arraypos.Length < 4)
                    return false ;
                string strRobotAgl = _cmdClient.SendCommand($"005x@0");
                var arrayagl = strRobotAgl.Split(',');
                if (arrayagl.Length < 4)
                    return false ;

                double a1 = arrayagl[0].ToDouble();
                double a2 = arrayagl[1].ToDouble();
                double x = arraypos[0].ToDouble();
                double y = arraypos[1].ToDouble();
                double L1 = 0, L2 = 0;
                CalibScara.GetRobotAlarm(a1, a2,x,y,ref L1, ref L2);

                #region 更新状态的线程
                Task.Factory.StartNew(() =>
                {
                    double xx = 0, yy = 0;
                    int failedCount = 0;
                    while (_statesClient?.Client?.Connected == true)
                    {
                        try
                        {
                            var response = _statesClient.SendCommand("GetStates");
                            var array = response.Split(',');
                            if (array.Length >= 6)
                            {
                                lock(lockObj)
                                {
                                    InPos = (array[0] == "1");
                                 //   if (CurrentPosition == null)
                                  //      CurrentPosition = new Coordinate();

                                  //  CalibScara.TransXY(array[1].ToDouble(), array[2].ToDouble(), L1, L2, ref xx , ref yy);
                                    CurrentGetPositionFormTcp.X = array[1].ToDouble();
                                    CurrentGetPositionFormTcp.Y = array[2].ToDouble();
                                    CurrentGetPositionFormTcp.Z = array[3].ToDouble();
                                    CurrentGetPositionFormTcp.U = array[4].ToDouble();
                                    CurrentPosition = CurrentGetPositionFormTcp;
                                    CurrentHandDirection = array[5].ToDouble() <= 0 ? HandDirection.Lefty : HandDirection.Right;
                                    dOut = UInt32.Parse(array[6]);
                                    dIn = UInt32.Parse(array[7]) | UInt32.Parse(array[8])<<16;

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
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                    if(_statesClient?.Client?.Connected == false)
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

        public bool Jump(Coordinate coordinate, HandDirection direction, double limz, bool bCheckHandleSys=false)
        {
            StringBuilder sb = new StringBuilder();
            if (coordinate.U >= 360)
                coordinate.U = coordinate.U - 360;
            if (coordinate.U <= -360)
                coordinate.U = coordinate.U + 360;
            sb.Append($"100jump XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            if (direction != HandDirection.Default)
                sb.Append($" /{ Enum.GetName(typeof(HandDirection), direction)[0]}");
            if (limz != 0)
                sb.Append($" LimZ {limz}");
            var result = _cmdClient.SendCommand(sb.ToString());
            if (result != "0")
            {
                _cmdClient.SendCommand("100Reset Error");
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
            sb.Append($"100GO XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            if (direction != HandDirection.Default)
                sb.Append($" /{ Enum.GetName(typeof(HandDirection), direction)[0]}");
            var result = _cmdClient.SendCommand(sb.ToString());
            if (result != "0")
            {
                _cmdClient.SendCommand("100Reset Error");
                return false;
            }
            return true;
        }
        public bool Move(Coordinate coordinate, bool bCheckHandleSys)
        {
            StringBuilder sb = new StringBuilder();
            if (coordinate.U >= 360)
                coordinate.U = coordinate.U - 360;
            if (coordinate.U <= -360)
                coordinate.U = coordinate.U + 360;
            sb.Append($"100Move XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            var result = _cmdClient.SendCommand(sb.ToString());
            if (result != "0")
            {
                _cmdClient.SendCommand("100Reset Error");
                return false;
            }
            return true;
               
        }
        public void Stop()
        {
            var result = _cmdClient.SendCommand("100Stop");
            if (result != "0")
                _cmdClient.SendCommand("100Reset Error");
        }
        public bool Home()
        {
            var result = _cmdClient.SendCommand("100Home");
            if (result != "0")
            {
                _cmdClient.SendCommand("100Reset Error");
                return false;
            }
            return true;
              
        }
        public void SetOutput(int index, bool value)
        {
            var cmd = value ? "On" : "Off";
            var result = _cmdClient.SendCommand($"100{ cmd }({index})");
            if (result != "0")
                _cmdClient.SendCommand("100Reset Error");
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
