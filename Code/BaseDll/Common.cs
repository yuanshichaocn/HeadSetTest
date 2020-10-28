using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UserCtrl;
using System.Timers;
using System.ComponentModel;
using System.Reflection;
using log4net;

namespace BaseDll
{
    /// <summary>
    /// 脉冲发生器
    /// </summary>
    public class PulseGenerator
    {
        public Action<int> m_action;
        public Action m_actionCallBack;
        private bool m_bRuning = false;
        System.Timers.Timer PulseTimer = new System.Timers.Timer();
        int m_nPulseCount = 50;
        int m_ElapsedNum = 0;
        public PulseGenerator(int Time,int Count, Action<int> action,Action actionCallBack)
        {
            PulseTimer.Interval = Time;
            m_nPulseCount = Count;
            m_action = action;
            m_actionCallBack = actionCallBack;
            m_ElapsedNum = 0;
            PulseTimer.Elapsed += (sendor,e)=> {

                if(m_ElapsedNum <m_nPulseCount)
                {
                    if(action!=null)
                    action(m_ElapsedNum);
                    m_ElapsedNum++;
                    m_bRuning = false;
                }
                else
                {
                    m_bRuning = false;
                    m_ElapsedNum = 0;
                    PulseTimer.Stop();
                    if(actionCallBack!= null)
                    actionCallBack();
                }
            };
        }
        public bool IsRuning
        {
            get=> m_bRuning;
        }
        public  void StartPulseGenerator()
        {
            m_bRuning = true;
            PulseTimer.Start();
           
        }
        public  void StopPluseGenerator()
        {
            m_bRuning = false;
            PulseTimer.Stop();
           ///if (m_actionCallBack != null)
           //    m_actionCallBack();
        }
       

    }
    /// <summary>
    /// 位操作
    /// </summary>
    public class BitOperat
    {
        /// <summary>
        /// 获取位状态(16位版)
        /// </summary>
        /// <param name="data">对象值</param>
        /// <param name="index">第几位</param>
        /// <returns></returns>
       public static bool GetBit16B(Int16 data, int index)
        {
            return ((data & (1 << index)) > 0) ? true : false;
        }
        /// <summary>
        /// 获取位状态(16位版)
        /// </summary>
        /// <param name="data">对象值</param>
        /// <param name="index">第几位</param>
        /// <returns></returns>
        public static int GetBit16(UInt16 data, byte index)
        {
            return ((data & (1 << index)) > 0) ? 1 : 0;
        }
        /// <summary>
        /// 获取位状态(32位版)
        /// </summary>
        /// <param name="data">对象值</param>
        /// <param name="index">第几位</param>
        /// <returns></returns>
        public static int GetBit32(UInt32 data, byte index)
        {
            return ((data & (1 << index)) > 0) ? 1 : 0;
        }

        public static int GetBit32(UInt32 data, int index)
        {
            return ((data & (1 << index)) > 0) ? 1 : 0;
        }
        /// <summary>
        /// 获取位状态(32位版)
        /// </summary>
        /// <param name="data">对象值</param>
        /// <param name="index">第几位</param>
        /// <returns></returns>
        public static int GetBit32(Int32 data, byte index)
        {
            return ((data & (1 << index)) > 0) ? 1 : 0;
        }
        /// <summary>
        /// 获取位状态(32位版)
        /// </summary>
        /// <param name="data">对象值</param>
        /// <param name="index">第几位</param>
        /// <returns></returns>
        public static bool GetBit32B(Int32 data, byte index)
        {
            return ((data & (1 << index)) > 0) ? true : false;
        }
    }
    /// <summary>
    /// 定时器
    /// </summary>
    public class cUserTimer
    {
        public cUserTimer(long time)
        {
            timedelay = time;
            stopwatch.Reset();

        }
        public void SetTimeDelay(long nTime)
        {
            timedelay = nTime;
        }
        long timedelay = 0;
        Stopwatch stopwatch = new Stopwatch();
        public void ResetStartTimer()
        {
            stopwatch.Reset();
            stopwatch.Restart();
            IsPause = false;
        }
        public long NowTime
        {
            get
            {
                if (IsPause)
                    return stopwatch.ElapsedMilliseconds;
                if (!stopwatch.IsRunning)
                    return -1;
                else 
                  return   stopwatch.ElapsedMilliseconds;
            }
        }
        public long SetedTime
        {
            get=>  timedelay;
        }

        public bool IsTimerOver
        {
            get
            {
                if (IsPause)
                    return false;
                if (!stopwatch.IsRunning)
                    return false;
                return stopwatch.ElapsedMilliseconds > timedelay;
            }
        }
        public void Stop()
        {
            stopwatch.Stop();
            stopwatch.Reset();
            IsPause = false;
        }
        public bool IsPause
        {
            private set;
            get;
        } = false;
        public bool IsRuning
        {
            get => ( stopwatch.IsRunning || IsPause);
        }
        public void Reset()
        {
            stopwatch.Reset();
        }
       
        public long  PauseTimer()
        {
           
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                IsPause = true;
            }
             
            return stopwatch.ElapsedMilliseconds;
        }
        public  void resumeTimer()
        {
            if(IsPause)
            {
                stopwatch.Start();
                IsPause = false;
            }
            
        }
    }

    public struct Coordinate
    {
      //  object obj = new object();
        public double X
        {
            get => x;
            set
            {
                //  lock (obj)
                { x = value; }
            }
        }

        public double Y
        {
            get => y;
            set
            {
                // lock (obj)
                { y = value; }
            }
        }
        public double Z
        {
            get => z;
            set
            {
                // lock (obj)
                { z = value; }
            }
        }

        public double U
        {
            get => u;
            set
            {
                // lock (obj)
                { u = value; }
            }
        }
        double x, y, z, u;

        public Coordinate Copy()
        {
            Coordinate coordinate;
          //  lock (obj)
            {
                coordinate = new Coordinate()
                {

                    X = this.X,
                    Y = this.Y,
                    Z = this.Z,
                    U = this.U

                };

            }
            return coordinate;


        }
    }
    /// <summary>
    /// Scara Robot 臂长标定
    /// </summary>
    public class CalibScara
        {
        public  static void GetRobotAlarm(  double a1, double a2, double x, double y,
                                   ref double L1, ref double L2)
       {
        //string strRobotPos = _cmdClient.SendCommand($"003x@0");
        //var arraypos = strRobotPos.Split(',');
        //if (arraypos.Length < 4)
        //    return;
        //string strRobotAgl = _cmdClient.SendCommand($"005x@0");
        //var arrayagl = strRobotAgl.Split(',');
        //if (arrayagl.Length < 4)
        //    return;

        //double a1 = arrayagl[0].ToDouble();
        //double a2 = a1 + arrayagl[1].ToDouble();
        //double x = arraypos[0].ToDouble();
        //double y = arraypos[1].ToDouble();

         double alg2 = a2;
           a2 = a1 + a2;
          double fm = Math.Sin(a2 * Math.PI / 180.000) * Math.Cos(a1 * Math.PI / 180.000) - Math.Sin(Math.PI * a1 / 180.000) * Math.Cos(Math.PI * a2 / 180.000);
          double fz = y * Math.Cos(a1 * Math.PI / 180.000) - x * Math.Sin(a1 * Math.PI / 180.00);
          L2 = fz / fm;
           fz = y - L2 * Math.Sin(a2 * Math.PI / 180.0000);
           fm = Math.Sin(a1 * Math.PI / 180.0000);
          L1 = fz / fm;
          double x1 = 0, y1 = 0;
          TransXY(a1, alg2, L1, L2, ref x1, ref y1);

      }
     public static void TransXY(double alg1, double alg2, double L1, double L2, ref double x, ref double y)
    {

        double a1 = alg1;
        double a2 = a1 + alg2;
        x = L1 * Math.Cos(a1 * Math.PI / 180.000) + L2 * Math.Cos(a2 * Math.PI / 180.000);
        y = L1 * Math.Sin(a1 * Math.PI / 180.000) + L2 * Math.Sin(a2 * Math.PI / 180.000);
    }


}

    /// <summary>
    /// 日志显示
    /// </summary>
    public class LogView
    {

        // private object lockMsg = new object();
        // public LogView(ListLog listLog = null)
        // {
        //     m_listLog = listLog;
        // }
        // ListLog m_listLog;
        // string m_strOldMsg;
        // public void SetShowListBox(ListLog listLog)
        // {
        //     m_listLog = listLog;
        //    // m_listLog.Show();
        //     // m_listLog.ReadOnly = true;

        // }
        // public delegate void logListBoxShowHandle(ListLog Sationloglist, string msg);
        // public event logListBoxShowHandle m_eventListBoxShow = null;
        // public void Info(string msg)
        // {

        //        // if (msg != m_strOldMsg)
        //             if (m_eventListBoxShow != null)
        //                 m_eventListBoxShow(m_listLog, "Info-" +"[" +DateTime.Now.ToString()+"]" + " : " + msg);


        //     //m_listLog?.Invoke(new Action(() =>
        //     //{
        //     //    if (m_listLog == null)
        //     //        return;
        //     //    m_listLog?.Items.Add("Info-" + msg);
        //     //    //m_listLog.SelectedIndex = m_listLog.Items.Count - 1;
        //     //    while (m_listLog.Items.Count > 10)
        //     //        m_listLog.Items.RemoveAt(0);
        //     //}));
        //     m_strOldMsg = msg;
        // }
        // public void Warn(string msg)
        // {

        //         if (msg != m_strOldMsg)
        //             if (m_eventListBoxShow != null)
        //                 m_eventListBoxShow(m_listLog, "Warn-"+"["+ DateTime.Now.ToString() + "]" + " : " + msg);

        //     //m_listLog?.Invoke(new Action(() =>
        //     //    {
        //     //        if (m_listLog == null)
        //     //            return;
        //     //        m_listLog?.Items.Add("Warn-" + msg);
        //     //        while (m_listLog.Items.Count > 10)
        //     //            m_listLog.Items.RemoveAt(0);
        //     //        //m_listLog.SelectedIndex = m_listLog.Items.Count - 1;
        //     //    }));
        //     m_strOldMsg = msg;
        // }
        // public void Err(string msg)
        // {

        //         if (msg != m_strOldMsg)
        //             if (m_eventListBoxShow != null)
        //                 m_eventListBoxShow(m_listLog, "Err-" + "[" +DateTime.Now.ToString() + "]" + " : " + msg);


        //     //m_listLog?.Invoke(new Action(() =>
        //     //{
        //     //    if (m_listLog == null)
        //     //        return;
        //     //    m_listLog?.Items.Add("Err-" + msg);
        //     //    while (m_listLog.Items.Count > 10)
        //     //        m_listLog.Items.RemoveAt(0);
        //     //  //  m_listLog.SelectedIndex = m_listLog.Items.Count - 1;
        //     //}));
        //     m_strOldMsg = msg;
        // }
        //public  void ShowLog(string strmsg)
        // {
        //     Info( strmsg);
        // }
      

            public LogView(ListLog listLog = null, RichTxtBoxLog richTxtBoxLog=null)
            {
                m_listLog = listLog;
                m_richTextBox = richTxtBoxLog;
            }
            ListLog m_listLog;
            RichTxtBoxLog m_richTextBox;
            string m_strOldMsg;
            public void SetShowListBox(ListLog listLog)
            {
                m_listLog = listLog;
                m_listLog.Show();
              

            }
            public void SetShowRichTextBox(RichTxtBoxLog richTextBox)
            {
                m_richTextBox = richTextBox;
                m_richTextBox.Show();
                

            }
            public delegate void logListBoxShowHandle(ListLog Sationloglist, string msg);
            public event logListBoxShowHandle m_eventListBoxShow = null;

            public delegate void logRichBoxShowHandle(RichTxtBoxLog richTextBox, string msg);
            public event logRichBoxShowHandle m_eventRichBoxShow = null;
            public void Info(string msg)
            {
                ILog _logger = LogManager.GetLogger(GetType().Name);

                if (msg != m_strOldMsg)
                {
                    if (m_eventListBoxShow != null && m_listLog != null)
                        m_eventListBoxShow(m_listLog, "Info-" + DateTime.Now.ToString() + ":" + msg);
                    if (m_eventRichBoxShow != null && m_richTextBox != null)
                        m_eventRichBoxShow(m_richTextBox, "Info-" + DateTime.Now.ToString() + ":" + msg);

                    _logger.Info(msg);
                }


            
                m_strOldMsg = msg;
            }
            public void Warn(string msg)
            {
                ILog _logger = LogManager.GetLogger(GetType().Name);

                if (msg != m_strOldMsg)
                {
                    if (m_eventListBoxShow != null)
                        m_eventListBoxShow(m_listLog, "Warn-" + DateTime.Now.ToString() + ":" + msg);
                    if (m_eventRichBoxShow != null)
                        m_eventRichBoxShow(m_richTextBox, "Warn-" + DateTime.Now.ToString() + ":" + msg);
                    _logger.Warn(msg);
                }

          
                m_strOldMsg = msg;
            }
            public void Err(string msg)
            {
                ILog _logger = LogManager.GetLogger(GetType().Name);

                if (msg != m_strOldMsg)
                {
                    if (m_eventListBoxShow != null)
                        m_eventListBoxShow(m_listLog, "Err-" + DateTime.Now.ToString() + ":" + msg);
                    if (m_eventRichBoxShow != null)
                        m_eventRichBoxShow(m_richTextBox, "Err-" + DateTime.Now.ToString() + ":" + msg);
                    _logger.Error(msg);
                }


                m_strOldMsg = msg;
            }
        public void ShowLog(string strmsg)
        {
            Info(strmsg);
        }


    }

    public enum StationState
    {
        [Description("停止")]
        StationStateStop = 1,
        [Description("运行")]
        StationStateRun,
        [Description("暂停")]
        StationStatePause,
        [Description("急停")]
        StationStateEmg,
    }
    public enum AppMode
    {
        AirRun,
        Run,
    }
    public static class GlobalVariable
    {

        //工站状态
        public delegate void StationStateChangedHandler(StationState currState);
        public static event StationStateChangedHandler g_eventStationStateChanged = null;
        private static volatile  StationState g_oldStationState;
        private static object lockobj = new object();
        public static  StationState g_StationState
        {
            set
            {
                lock (lockobj)
                {
                    if (value != g_oldStationState)
                    {
                        if (g_eventStationStateChanged != null)
                            g_eventStationStateChanged?.Invoke(value);
                    }
                    g_oldStationState = value;
                }
            }
            get
            {
                lock (lockobj)
                {
                    return g_oldStationState;
                }
            }
        }
    }




}
