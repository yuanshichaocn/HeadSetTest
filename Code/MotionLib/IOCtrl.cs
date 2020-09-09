using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using gts;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using log4net;
namespace MotionIoLib
{
    public enum IoEdgeState
    {
        None,
        Rising,
        Falling,
    }
    public abstract class IoCtrl
    {
        protected ILog logger;
        /// <summary>
        ///卡在系统中的序号 
        /// </summary>
        public int m_nIndex;
        /// <summary>
        ///卡号 
        /// </summary>
        public ulong m_nCardNo;
        /// <summary>
        ///输入点状态缓存区 
        /// </summary>
        public int m_nInData;
        /// <summary>
        /// 输入缓冲区数据是否有变化
        /// </summary>
        public bool m_bDataChange;
        /// <summary>
        ///输出点状态缓存区 
        /// </summary>
        public int m_nOutData;
        /// <summary>
        ///卡的有无效状态 
        /// </summary>
        public bool m_bEnable = false;

        /// <summary>
        ///输入点名称向量 
        /// </summary>
        public string[] m_strArrayIn;
        /// <summary>
        ///输出点名称向量 
        /// </summary>
        public string[] m_strArrayOut;

        /// <summary>
        /// 卡名称
        /// </summary>
        public string m_strCardName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nCardNo"></param>
        public IoCtrl(int nIndex, ulong nCardNo)
        {
            m_nIndex = nIndex;//卡序号
            m_nCardNo = nCardNo;//卡号
            m_bEnable = true;
            m_strCardName = String.Format("{0}", this.GetType());
            logger = LogManager.GetLogger(this.m_strCardName);
        }


        /// <summary>
        /// 输入点总个数
        /// </summary>
        public int CountIoIn
        {
            get { return m_strArrayIn.Length; }
        }

        /// <summary>
        /// 输出点总个数 
        /// </summary>
        public int CountIoOut
        {
            get { return m_strArrayOut.Length; }
        }

        /// <summary>
        /// 卡是否启用成功
        /// </summary>
        /// <returns></returns>
        public bool IsEnable()
        {
            return m_bEnable;
        }

        /// <summary>
        /// 判断输入缓冲区是否有变化
        /// </summary>
        /// <returns></returns>
        public bool IsDataChange()
        {
            return m_bDataChange;
        }

        /// <summary>
        ///初始化 
        /// </summary>
        /// <returns></returns>
        public abstract bool Init();

        /// <summary>
        ///释放IO卡 5
        /// </summary>
        public abstract void DeInit();

        /// <summary>
        ///获取卡所有的输入信号 
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public abstract bool ReadIOIn(ref int nData);

        /// <summary>
        ///获取卡所有的输出信号 
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public abstract bool ReadIOOut(ref int nData);

        /// <summary>
        ///按位获取输入信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public abstract bool ReadIoInBit(int nIndex);

        /// <summary>
        ///按位获取输出信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public abstract bool ReadIoOutBit(int nIndex);


        /// <summary>
        /// 按位输出信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="bBit"></param>
        /// <returns></returns>
        public abstract bool WriteIoBit(int nIndex, bool bBit);
       

        /// <summary>
        /// 输出整个卡的信号 
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public abstract bool WriteIo(int nData);

        /// <summary>
        ///获取指定IO输入点的缓冲状态 
        /// </summary>
        /// <returns></returns>

        public bool GetIoInState(int nIndex)
        {
            return (m_nInData & (1 << (nIndex - 1))) != 0;
        }
        /// <summary>
        ///获取指定IO输出点的缓冲状态  
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool GetIoOutState(int nIndex)
        {
            return (m_nOutData & (1 << (nIndex - 1))) != 0;
        }
        public abstract bool InStopEnable(int nIndex);
        public abstract bool InStopDisenable(int nIndex);
        public virtual bool Latch(int nIndex)
        {
            return true;
        }
        public virtual bool InStopState(int nIndex, bool bState)
        {
            return true;
        }
       protected bool m_bOpen;
    }
    public class IOMgr
    {
        public struct IoDefine
        {
            public int _IoIndex;
            public int _AxisIndex;
            public int _CardIndex;
        };
        ILog _loggger = LogManager.GetLogger("IOMgr");

        private IOMgr()
            {}
          ~IOMgr()
        {
            m_bExit = true;
        }
       static IOMgr _IOObj = null;
       static  object _lock = new object();
        Thread thread = null;
        public static IOMgr GetInstace()
        {

            if (_IOObj == null)
            {
                lock (_lock)
                {
                    if (_IOObj == null)
                    {
                        _IOObj = new IOMgr();
                        return _IOObj;
                    }
                    else
                        return _IOObj;
                }
            }
            else
                return _IOObj;
        }

        List<IoCtrl> m_listCard = new List<IoCtrl>();
        public void AddCard(string strName, int nIndex, ulong nCardNo)
        {
            // listIoCard.Add(ioCtrl);
            Assembly assembly = Assembly.GetAssembly(typeof(IoCtrl));
            string name = "MotionIoLib.IoCtrl_" + strName;
            Type type = assembly.GetType(name);
            bool flag = type == null;
            if (flag)
            {
                MessageBox.Show("strName" + "卡配置出错","Err",MessageBoxButtons.OK,MessageBoxIcon.Error);
                throw new Exception(string.Format("IO卡{0}找不到可用的封装类，请确认motionio.dll是否正确或配置错误?" + strName, new object[0]));
            }
            object[] args = new object[]
            {
                nIndex,
                nCardNo
            };
            this.m_listCard.Add(Activator.CreateInstance(type, args) as IoCtrl);
        }
        public bool  initAllIoCard()
        {
            bool brtn = true;
            foreach(var temp in m_listCard)
            {
                brtn &= temp.Init();
            }
             
            m_bOldInputState = new bool[m_ioInput.Count];
            m_bOldOutputState = new bool[m_ioOutput.Count] ;

            for (int i = 0; i < m_ioOutput.Count; i++)
                m_bOldOutputState[i] = false;
            thread = new Thread(ThreadMonitor);
            thread.IsBackground = true;
            thread.Start();
            return brtn;
        }
        bool[] m_bOldInputState= null;
        bool[] m_bOldOutputState = null;
        Dictionary<string, IoDefine> m_ioInput = new Dictionary<string, IoDefine>();
        Dictionary<string ,IoDefine> m_ioOutput = new Dictionary<string ,IoDefine>();
        //上次IO状态
        Dictionary<string, bool> m_ioInputOldState = new Dictionary<string, bool>();
        Dictionary<string, bool> m_ioOutputOldState = new Dictionary<string, bool>();
        //IO边沿状态
        Dictionary<string, IoEdgeState> m_ioInputEdgeState = new Dictionary<string, IoEdgeState>();
        Dictionary<string, IoEdgeState> m_ioOutputEdgeState = new Dictionary<string, IoEdgeState>();
        public void  AddIoInput(string ioname,int nCardNo,int nAxisNo,int nIoIndex)
        {
            IoDefine ioDefine = new IoDefine();
            ioDefine._AxisIndex = nAxisNo;
            ioDefine._IoIndex = nIoIndex;
            ioDefine._CardIndex = nCardNo;
            if (ioname != "" && !m_ioInput.ContainsKey(ioname))
            {
                m_ioInput.Add(ioname, ioDefine);
                m_ioInputOldState.Add(ioname, false);
            }
            else if (ioname != "")
            {
                string str = "输入：" + ioname + ":配置出错，请检查";
                MessageBox.Show(str, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public void AddIoOutput(string ioname,  int nCardNo, int nAxisNo, int nIoIndex)
        {
            IoDefine ioDefine = new IoDefine();
            ioDefine._AxisIndex = nAxisNo;
            ioDefine._IoIndex = nIoIndex;
            ioDefine._CardIndex = nCardNo;
            if (ioname != "" && !m_ioOutput.ContainsKey(ioname))
            {
                m_ioOutput.Add(ioname, ioDefine);
                m_ioOutputOldState.Add(ioname, false);
            }
            else if (ioname != "")
            {
                string str = "输出：" + ioname + ":配置出错，请检查";
                MessageBox.Show(str, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public int  GetIoInputNum()
        {
            return m_ioInput.Count;
        }
        public Dictionary<string, IoDefine>  GetInputDic()
        {
            return m_ioInput;
        }
        public Dictionary<string, IoDefine> GetOutputDic()
        {
            return m_ioOutput;
        }

        public int GetIoOutputNum()
        {
            return m_ioOutput.Count;
        }
        public bool WriteIoBit(string ioname,bool bval)
        {
            IoDefine ioDefine = new IoDefine();
            if (!m_ioOutput.TryGetValue(ioname,out ioDefine))
            {
                //throw new Exception(string.Format("{0}不在配置文件中,请检查配置文件"));
                MessageBox.Show(string.Format("{0}不在配置文件中,请检查配置文件", ioname), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            bool bSafe = true;
            //输出 IO 安全检查
            if (m_eventIsSafeWhenOutIo != null)
            {
                Delegate[] delegates = m_eventIsSafeWhenOutIo.GetInvocationList();
                foreach(var tem in delegates)
                {
                   MethodInfo methodInfo = tem.GetMethodInfo();
                   bSafe &= (bool)methodInfo.Invoke(null, new object[] { ioname, bval });
                }
            }
            if (!bSafe)
                return false;
          //  _loggger.Info(string.Format("《{0}》 输出：{1}", ioname, bval));
                //if (!m_eventIsSafeWhenOutIo(ioname, bval))
                //    return false;
            int index = (ioDefine._AxisIndex<<8) | ioDefine._IoIndex;
            //io 写入IO卡 研华的前8位为板卡的轴号，后为Io的索引
            if (ioDefine._CardIndex >= m_listCard.Count)
            {
                MessageBox.Show("Io配置中，卡索引不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
          return    m_listCard[ioDefine._CardIndex].WriteIoBit(index, bval);
    
        }
        public  bool InStopEnable(string ioname)
        {
            IoDefine ioDefine = new IoDefine();
            if (!m_ioInput.TryGetValue(ioname, out ioDefine))
            {
                //throw new Exception(string.Format("{0}不在配置文件中,请检查配置文件"));
                MessageBox.Show(string.Format("{0}不在配置文件中,请检查配置文件", ioname), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
         
            int index = (ioDefine._AxisIndex << 8) | ioDefine._IoIndex;
            //io 写入IO卡 研华的前8位为板卡的轴号，后为Io的索引
            if (ioDefine._CardIndex >= m_listCard.Count)
            {
                MessageBox.Show("Io配置中，卡索引不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return m_listCard[ioDefine._CardIndex].InStopEnable(index);
        }
        public bool InStopState(string ioname,bool bState)
        {
            IoDefine ioDefine = new IoDefine();
            if (!m_ioInput.TryGetValue(ioname, out ioDefine))
            {
                //throw new Exception(string.Format("{0}不在配置文件中,请检查配置文件"));
                MessageBox.Show(string.Format("{0}不在配置文件中,请检查配置文件", ioname), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int index = (ioDefine._AxisIndex << 8) | ioDefine._IoIndex;
            //io 写入IO卡 研华的前8位为板卡的轴号，后为Io的索引
            if (ioDefine._CardIndex >= m_listCard.Count)
            {
                MessageBox.Show("Io配置中，卡索引不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return m_listCard[ioDefine._CardIndex].InStopState(index, bState);
        }
        public bool InStopDisenable(string ioname)
        {
            IoDefine ioDefine = new IoDefine();
            if (!m_ioInput.TryGetValue(ioname, out ioDefine))
            {
                //throw new Exception(string.Format("{0}不在配置文件中,请检查配置文件"));
                MessageBox.Show(string.Format("{0}不在配置文件中,请检查配置文件", ioname), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int index = (ioDefine._AxisIndex << 8) | ioDefine._IoIndex;
            //io 写入IO卡 研华的前8位为板卡的轴号，后为Io的索引
            if (ioDefine._CardIndex >= m_listCard.Count)
            {
                MessageBox.Show("Io配置中，卡索引不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return m_listCard[ioDefine._CardIndex].InStopDisenable(index);
        }
        public IoEdgeState ReadIoOutBitEdge(string ioname)
        {
            bool bval = ReadIoOutBit(ioname);
            if (m_ioOutputOldState[ioname] != bval && bval)
                m_ioOutputEdgeState[ioname] = IoEdgeState.Rising;
            else if (m_ioOutputOldState[ioname] != bval && !bval)
                m_ioOutputEdgeState[ioname] = IoEdgeState.Falling;
            else
                m_ioOutputEdgeState[ioname] = IoEdgeState.None;
            m_ioOutputOldState[ioname] = bval;
            return m_ioOutputEdgeState[ioname];
        }
        public bool ReadIoOutBit(string ioname )
        {
            IoDefine ioDefine = new IoDefine();
            if (!m_ioOutput.TryGetValue(ioname, out ioDefine))
            {
                //throw new Exception(string.Format("{0}不在配置文件中,请检查配置文件", ioname));
                MessageBox.Show(string.Format("{0}不在Io输出配置文件中,请检查配置文件", ioname), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            int index = (ioDefine._AxisIndex << 8) | ioDefine._IoIndex;
            //io 写入IO卡 研华的前8位为板卡的轴号，后为Io的索引
            if (ioDefine._CardIndex >= m_listCard.Count)
            {
                MessageBox.Show("Io配置中，卡索引不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            bool bval = m_listCard[ioDefine._CardIndex].ReadIoOutBit(index);
            
            return bval;
        }
        public bool ReadIoInBit(string ioname)
        {
            IoDefine ioDefine = new IoDefine();
            if (!m_ioInput.TryGetValue(ioname, out ioDefine))
            {
                // throw new Exception(string.Format("{0}不在配置文件中,请检查配置文件"));
                MessageBox.Show(string.Format("{0}不在Io输入配置文件中,请检查配置文件", ioname), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            int index = (ioDefine._AxisIndex << 8) | ioDefine._IoIndex;
            //io 写入IO卡 研华的前8位为板卡的轴号，后为Io的索引
            if (ioDefine._CardIndex >= m_listCard.Count)
            {
                MessageBox.Show("Io配置中，卡索引不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            bool bval = m_listCard[ioDefine._CardIndex].ReadIoInBit(index);
            
            return bval;
        }
        public IoEdgeState ReadIoInBitEdge(string ioname)
        {
            bool bval = ReadIoInBit(ioname);
            if (m_ioInputOldState[ioname] != bval && bval)
                m_ioInputEdgeState[ioname] = IoEdgeState.Rising;
            else if (m_ioInputOldState[ioname] != bval && !bval)
                m_ioInputEdgeState[ioname] = IoEdgeState.Falling;
            else
                m_ioInputEdgeState[ioname] = IoEdgeState.None;
            m_ioInputOldState[ioname] = bval;
            return m_ioInputEdgeState[ioname];
        }
        //输入状态更新
        public delegate void IoInputStateChangeHandler(int index, bool currentState);
        public event IoInputStateChangeHandler m_eventIoInputChanage;
        public delegate void IoInputStateChangedHandle(string ioName, bool currentState);
        public event IoInputStateChangedHandle m_eventIoInputChanageByName;


        //输出状态更新
        public delegate void IoOutputStateChangeHandler(int index, bool currentState);
        public event IoOutputStateChangeHandler m_eventIoOutputChanage;
        public delegate void IoOutputStateChangedHandle(string ioName, bool currentState);
        public event IoOutputStateChangedHandle m_eventIoOutputChanageByName;

        //系统Io
        public delegate void SystemSinglHandler(string IoName, bool bCurrentState);
        public  SystemSinglHandler m_deltgateSystemSingl = null;
        //
        public event SystemSinglHandler m_eventAllCheckSingl = null;

        //Io操作 安全检测函数，安全返回true  不安全返回false
        public delegate bool IsSafeWhenOutIoHandler(string ioName,bool bVal);
        public event IsSafeWhenOutIoHandler m_eventIsSafeWhenOutIo;
        bool m_bExit = false ;
        public void Close()
        {
            m_bExit = true;

            Thread.Sleep(500);
            thread.Join(500);
            try
            {
                if (thread.IsAlive)
                    thread.Abort();
            }
            catch (Exception E)
            {
               
            }
            foreach (var temp in m_listCard)
                temp.DeInit();
        }
        private void ThreadMonitor()
        {
            int indexIn = 0;
            int indexOut = 0;
            bool bIoState = false;
            while (!m_bExit)
            {
                Thread.Sleep(50);
                indexIn = 0;
                foreach (var tem in m_ioInput)
                {
                    bIoState = ReadIoInBit(tem.Key);
                    if (m_bOldInputState[indexIn] != bIoState)
                    {
                        if (m_eventIoInputChanage != null)
                            m_eventIoInputChanage(indexIn, bIoState);
                        if (m_eventIoInputChanageByName != null)
                            m_eventIoInputChanageByName(tem.Key, bIoState);
                       
                    }
                    if (m_deltgateSystemSingl != null /*&& (tem.Key == "急停" || tem.Key == "暂停" || tem.Key == "启动" || tem.Key == "复位" || tem.Key == "安全门" || tem.Key == "前门安全光栅" || tem.Key == "后门安全光栅" || tem.Key == "气源压力检测")*/)

                    {
                        m_deltgateSystemSingl(tem.Key, bIoState);
                    }
                    m_bOldInputState[indexIn] = bIoState;
                    indexIn++;
                    if (m_eventAllCheckSingl != null)
                        m_eventAllCheckSingl(tem.Key, bIoState);
                }
                indexOut = 0;
                foreach (var tem in m_ioOutput)
                {
                    bIoState = ReadIoOutBit(tem.Key);
                    if (m_bOldOutputState[indexOut] != bIoState)
                    {
                        if (m_eventIoOutputChanage != null)
                            m_eventIoOutputChanage(indexOut, bIoState);
                        if (m_eventIoOutputChanageByName != null)
                            m_eventIoOutputChanageByName(tem.Key, bIoState);
                    }
                    m_bOldOutputState[indexOut] = bIoState;
                    indexOut++;
                    if (m_eventAllCheckSingl != null)
                        m_eventAllCheckSingl(tem.Key, bIoState);
                }
                
            }
        }

    }
}