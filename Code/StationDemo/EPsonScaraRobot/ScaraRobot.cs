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
        public event ChangedPosHandle ChangedPosEvent;
        public Coordinate CurrentPosition { get => coordinate; private set
            {

              if(coordinate!=value)
                {
                    if (ChangedPosEvent != null)
                        ChangedPosEvent(value);
                }
                coordinate = value;
            }
        }
        private Coordinate coordinate;
        public delegate void ChangedIoOutStateHandle(UInt32 state);
        public event ChangedIoOutStateHandle ChangedIoOutStateEvent;
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
        public event ChangedIoInStateHandle ChangedIoInStateEvent;
        public UInt32 dIn
        {
            get => dInIo;
            private set
            {
                if (value != dInIo)
                {
                    if (ChangedIoOutStateEvent != null)
                        ChangedIoOutStateEvent(value);
                }
                dInIo = value;
            }
        }
        private UInt32 dInIo;
        private object lockObj = new object();
        public delegate void ChangedHandStateHandle(HandDirection currhandDirection);
        public event ChangedHandStateHandle ChangedHandStateEvent;
        public HandDirection  CurrentHandDirection
        {
            private set
            {
                if(handDirection!=value)
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


        internal static ScaraRobot GetInstance()
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
                #region robot client
                _cmdClient = new RobotTcpClient();
                _cmdClient.Init(_cmdClientConfigPath);
                _cmdClient.Connect();

                _statesClient = new RobotTcpClient();
                _statesClient.Init(_statesClientConfigPath);
                _statesClient.Connect();

                #region 更新状态的线程
                Task.Factory.StartNew(() =>
                {
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
                                    if (CurrentPosition == null)
                                        CurrentPosition = new Coordinate();
                                    CurrentPosition.X = array[1].ToDouble();
                                    CurrentPosition.Y = array[2].ToDouble();
                                    CurrentPosition.Z = array[3].ToDouble();
                                    CurrentPosition.U = array[4].ToDouble();
                                    CurrentHandDirection = array[5].ToDouble() > 0 ? HandDirection.Lefty : HandDirection.Right;
                                    dOut = UInt32.Parse(array[6]);
                                    dIn = UInt32.Parse(array[7]);

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
                                throw ex;
                        }
                        finally
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                });
                #endregion
                #endregion

                msg = "";
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

        public void Jump(Coordinate coordinate, HandDirection direction, double limz)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"100jump XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            if (direction != HandDirection.Default)
                sb.Append($" /{ Enum.GetName(typeof(HandDirection), direction)[0]}");
            if (limz != 0)
                sb.Append($" LimZ {limz}");
            var result = _cmdClient.SendCommand(sb.ToString());
            if (result != "0")
                _cmdClient.SendCommand("100Reset Error");
        }
        public void Go(Coordinate coordinate, HandDirection direction)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"100GO XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            if (direction != HandDirection.Default)
                sb.Append($" /{ Enum.GetName(typeof(HandDirection), direction)[0]}");
            var result = _cmdClient.SendCommand(sb.ToString());
            if (result != "0")
                _cmdClient.SendCommand("100Reset Error");
        }
        public void Move(Coordinate coordinate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"100Move XY({coordinate.X},{coordinate.Y},{coordinate.Z},{coordinate.U})");
            var result = _cmdClient.SendCommand(sb.ToString());
            if (result != "0")
                _cmdClient.SendCommand("100Reset Error");
        }
        public void Stop()
        {
            var result = _cmdClient.SendCommand("100Stop");
            if (result != "0")
                _cmdClient.SendCommand("100Reset Error");
        }
        public void Home()
        {
            var result = _cmdClient.SendCommand("100Home");
            if (result != "0")
                _cmdClient.SendCommand("100Reset Error");
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
    }
}
