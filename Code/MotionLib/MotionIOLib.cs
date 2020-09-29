using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using gts;
using System.Diagnostics;
using System.Reflection;
using Communicate;
using log4net;
using System.Collections.Concurrent;

namespace MotionIoLib
{
    public enum AxisState
    {
        Homeing=-2,
        Moving = -1,
        NormalStop = 0,
        DriveAlarm = 2,
        LimtPStop = 4,
        LimtNStop = 5,
        ErrAlarm = 6,
    }
    public struct THomePrm
    {
        public bool _bHomeDir;
        public double _iSeachDistancePluse;
        public double _iSeachOffstPluse;
        public bool _bHomeDown;//true 上升沿触发，false 下降沿触发
        public double VelH;
        public double AccH;
        public double DccH;
        public double VelL;
        public double AccL;
        public double DccL;
        public int _nHomeMode;

    }
    public enum MoveType
    {
        Home_P,
        Home_N,
        PTP_P,
        PTP_N,
        BUF_P,
        BUF_N,
        JOG_P,
        JOG_N,
        Line2AxisAbs,
    }
    public enum GpState
    {
        GpDisable,//群组没有使能
        GpReady,//群组准备完成
        GpStoping,//群组停止
        GpErrStop,//群组错误停止
        GpMotion,//群组运行中
        Gp_AX_Motion,//群组不支持
        GpBuff,//群组缓存运动中
        GpPause,//群组暂停

    }
    public enum AxisSignlNo
    {
        伺服,
        报警,
        正极限,
        负极限,
        急停,
        实际位置,
        命令位置,
    }


    public struct TMovePrm
    {
        public double VelH;
        public double AccH;
        public double DccH;
        public double VelM;
        public double AccM;
        public double DccM;
        public double VelL;
        public double AccL;
        public double DccL;
        public double PlusePerRote;
        public double AxisLeadRange;
    }
    public struct ProPrietarySignal
    {
        public int oriSignal;
        public int positiveLimtSignal;
        public int NegitiveSignal;
    }
    /// <summary>
    /// 电机类型，如要添加新类型  伺服添加到SEVER 上面，步进 是在STEP
    /// </summary>
    public enum MotorType
    {
       

        /// <summary>
        /// 普通步进
        /// </summary>
        STEP,
        /// <summary>
        /// Ecat 轴模块步进
        /// </summary>
        EcatAxisModleStep,

        /// <summary>
        /// 普通伺服
        /// </summary>
        SEVER,
        /// <summary>
        /// 闭环步进 上电伺服
        /// </summary>
        CLStepSVOn,
        /// <summary>
        /// 闭环步进 掉电伺服
        /// </summary>
        CLStepSVOFF,

        /// <summary>
        /// Ecate轴
        /// </summary>
        EcatAxis,
        /// <summary>
        /// Ecat 轴模块伺服
        /// </summary>
        EcatAxisModleSevro,
      
  
        
    }
    /// <summary>
    /// 轴当前模式
    /// </summary>
    public enum MotorMode
    {
        HomeIng=6,
        CyclicSyncPosition = 8,
        CyclicSyncVelocity = 9,
        CyclicSyncTorque =10,
    }

    public struct AxisIOState
    {
        public bool _bSeverOn;
        public bool _bAlarm;
        public bool _bLimtP;
        public bool _bLimtN;
        public bool _bOrg;
        public bool _bEmg;
    };
    public struct AxisPos
    {
        public double _lCmdPos;
        public double _lActPos;
    };
    public struct AxisConfig
    {
        public MotorType motorType;
        public bool _bEnableAxis;
    };
    public enum BufMotionType
    {
        buf_IO,
        buf_Delay,
        buf_end,
        buf_Line2dAbs,
        buf_Line3dAbs,
        buf_Arc2dAbsCCW,
        buf_Arc2dAbsCW,
        buf_Arc2dAbsAngleCCW,
        buf_Arc2dAbsAngleCW,
    };
    public struct BufMotionParam
    {
        public BufMotionType _Type;
        public int _mode;//模式
        public int _nAxisNum;//轴个数
        public double _VelHigh;
        public double _VelLow;
        public double[] _point1;
        public double[] _point2;

        public string _strIoName;//Io名称
        public int _nCardNo;//Io所在卡号
        public int _nIoNo;//Io索引
        public int _nAxisNo;//Io轴号
        public bool _IoVal;//Io状态
        public int _nDelayMs;//毫秒延时
    };
    public enum CommuncateItf
    {
        Serial,
        Internet,
        UnKonw
    };
    //public enum SpeedHML
    //{
    //    speedH = 0,
    //    speedM = 1,
    //    speedL = 3,
    //}
    public enum AxisHomeFinishFlag
    {
        NoHome = -1,
        Homeing=0,
        Homed = 1,
        
    }
   

    #region "机械手参数类型定义"
    /// <summary>
    /// 机械手的状态
    /// </summary>
    public struct RobotStatusBits   //添加Robot机械手状态
    {
        //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 
        //1 为开/0 为关
        /// <summary>
        /// 测试
        /// </summary>
        public bool Test;
        /// <summary>
        /// 示教
        /// </summary>
        public bool Teach;
        /// <summary>
        /// 自动
        /// </summary>
        public bool Auto;
        /// <summary>
        /// 报警
        /// </summary>
        public bool Warning;
        /// <summary>
        /// 严重错误
        /// </summary>
        public bool SError;
        /// <summary>
        /// 安全保护
        /// </summary>
        public bool Safeguard;
        /// <summary>
        /// 急停
        /// </summary>
        public bool EStop;
        /// <summary>
        /// 错误
        /// </summary>
        public bool Error;
        /// <summary>
        /// 暂停
        /// </summary>
        public bool Paused;
        /// <summary>
        /// 运行中
        /// </summary>
        public bool Running;
        /// <summary>
        /// 准备好
        /// </summary>
        public bool Ready;
        /// <summary>
        /// 登录
        /// </summary>
        public bool Login;
        /// <summary>
        ///上电
        /// </summary>
        public bool Power;
    };
    /// <summary>
    /// 左右手势
    /// </summary>
    public enum RobotHand
    {
        /// <summary>
        /// 左手势
        /// </summary>
        LeftHand,
        /// <summary>
        /// 右手势
        /// </summary>
        RightHand,
        /// <summary>
        /// 未知
        /// </summary>
        Unknow
    };
    public enum RobotPower
    {
        /// <summary>
        /// 高功率模式
        /// </summary>
        HIGHPOWER,
        /// <summary>
        /// 低功率模式
        /// </summary>
        LOWPOWER,
        /// <summary>
        /// 未知
        /// </summary>
        UnKonw
    };
    public struct RobotPoint
    {
        /// <summary>
        /// 轴X坐标值
        /// </summary>
        public double X;
        /// <summary>
        /// 轴Y坐标值
        /// </summary>
        public double Y;
        /// <summary>
        /// 轴Z坐标值
        /// </summary>
        public double Z;
        /// <summary>
        /// 轴U坐标值
        /// </summary>
        public double U;
        /// <summary>
        /// 轴V坐标值
        /// </summary>
        public double V;
        /// <summary>
        /// 轴W坐标值
        /// </summary>
        public double W;
        /// <summary>
        /// 手势
        /// </summary>
        public RobotHand HandStyle;
    };
    /// <summary>
    /// 机械手的变量返回类型枚举
    /// </summary>
    public enum RobotVariable
    {
        RCBoolean,
        RCByte,
        RCDouble,
        RCInteger,
        RCLong,
        RCReal,
        RCString,
        RCShort,
        RCUByte,
        RCUShort,
        RCInt32,
        RCUInt32,
        RCInt64,
        RCUInt64
    };
    public struct MotionErrorMess
    {
        public string ErrorCmd;
        public string DeviceType;
        public string ErrorMess;
    }
    //public struct RobotInfo
    //{
    //    public int nBaudRate;
    //    public int nDataBit;
    //    public string strPartiy;
    //    public string strStopBit;
    //    public string strFlowCtrl;
    //    public int nBufferSzie;
    //    public int InterNetIndex;
    //    public string InterNetName;
    //    public int timeout;
    //    public string RobotSuffic;
    //    public string IpAddress;
    //    public ushort Port;
    //    public string PassWard;
    //}
    public struct RobotAccel
    {
        /// <summary>
        /// 加速度
        /// </summary>
        public int AccelSpeed;

        /// <summary>
        /// 减速度
        /// </summary>
        public int DecelSpeed;
    }

    public enum CoordinateSys
    {
        Base,
        Local,
        Tool
    }
    #endregion
    public abstract class MotionCardBase
    {
        protected InfoOnly infoOnly = null;
        protected ILog logger = null;
        protected double[] m_nAccs;
        protected double[] m_nDecs;
        protected uint[] m_nSmoothTimes;
        protected THomePrm[] m_HomePrm;
        protected TMovePrm[] m_MovePrm;
        protected MotorType[] m_MotorType;
        protected string[] m_strAxisName;
        protected ProPrietarySignal[] m_ProPrietarySignal;
        protected ManualResetEvent[] m_ManualEventHomeingStop;
        protected AxisState[] m_AxisStates;
        protected int m_nMinAxisNo;
        protected int m_nMaxAxisNo;
        protected ulong m_nCardIndex;
        protected AxisHomeFinishFlag[] m_nHomeFinishFlag;
        protected string m_strCardName = "";
      

        public void ReasetHomeFinishFlag( int nAxisNo)
        {
            m_nHomeFinishFlag[nAxisNo] = AxisHomeFinishFlag.NoHome;
        }
        public void SetHomeingFlag(int nAxisNo)
        {
            m_nHomeFinishFlag[nAxisNo] = AxisHomeFinishFlag.Homeing;
        }
        public void SetHomeFinishFlag(int nAxisNo)
        {
            m_nHomeFinishFlag[nAxisNo] = AxisHomeFinishFlag.Homed;
        }
        public AxisHomeFinishFlag GetHomeFinishFlag(int nAxisNo)
        {
          return (AxisHomeFinishFlag)Enum.Parse(typeof(AxisHomeFinishFlag), m_nHomeFinishFlag[nAxisNo].ToString());

        }
        public MotionCardBase(ulong indexCard, string strName, int nMinAxisNo, int nMaxAxisNo)
        {
            logger = LogManager.GetLogger(strName);
            infoOnly = new InfoOnly(logger);
            m_nCardIndex = indexCard;   //卡的标号
            m_strCardName = strName;    //卡的名字
            m_nMinAxisNo = nMinAxisNo;  //最小轴号
            m_nMaxAxisNo = nMaxAxisNo;  //最大轴号
            m_nAccs = new double[nMaxAxisNo - nMinAxisNo + 1];  //加速度
            m_nDecs = new double[nMaxAxisNo - nMinAxisNo + 1];  //减速度
            m_nSmoothTimes = new uint[nMaxAxisNo - nMinAxisNo + 1]; //平滑时间
            m_HomePrm = new THomePrm[nMaxAxisNo - nMinAxisNo + 1];  //原点参数
            m_MovePrm = new TMovePrm[nMaxAxisNo - nMinAxisNo + 1];  //移动参数
            m_nHomeFinishFlag = new AxisHomeFinishFlag[nMaxAxisNo - nMinAxisNo + 1]; //回归原点结束标志
            m_MotorType = new MotorType[nMaxAxisNo - nMinAxisNo + 1]; //电机类型 --步进 --伺服  --机械手
            m_strAxisName = new string[nMaxAxisNo - nMinAxisNo + 1];   //轴名字
            m_ProPrietarySignal = new ProPrietarySignal[nMaxAxisNo - nMinAxisNo + 1]; //信号标志
            m_ManualEventHomeingStop = new ManualResetEvent[nMaxAxisNo - nMinAxisNo + 1];
            m_AxisStates = new AxisState[nMaxAxisNo - nMinAxisNo + 1];
            for (int i = 0; i < nMaxAxisNo - nMinAxisNo + 1; i++)
            {
                m_nHomeFinishFlag[i] = AxisHomeFinishFlag.NoHome;
                m_ProPrietarySignal[i].oriSignal = 0;
                m_ProPrietarySignal[i].NegitiveSignal = 0;
                m_ProPrietarySignal[i].positiveLimtSignal = 0;
                m_nAccs[i] = 10;
                m_nDecs[i] = 10;
                m_nSmoothTimes[i] = 20;
                m_HomePrm[i]._bHomeDir = true;
                m_HomePrm[i]._bHomeDown = false;
                m_HomePrm[i].VelH = 20;
                m_HomePrm[i].AccH = 5;
                m_HomePrm[i].DccH = 5;
                m_HomePrm[i].VelL = 3;
                m_HomePrm[i].AccL = 1;
                m_HomePrm[i].DccL = 1;
                m_HomePrm[i]._iSeachDistancePluse = 10000;
                m_HomePrm[i]._iSeachOffstPluse = 1000;
                m_HomePrm[i]._nHomeMode = 0;
                m_strAxisName[i] = i.ToString();
                m_MovePrm[i].VelH = 20;
                m_MovePrm[i].VelM = 20;
                m_MovePrm[i].VelL = 20;
                m_MovePrm[i].AccH = 20;
                m_MovePrm[i].AccM = 20;
                m_MovePrm[i].AccL = 20;
                m_MovePrm[i].DccH = 20;
                m_MovePrm[i].DccM = 20;
                m_MovePrm[i].DccL = 20;
                m_MovePrm[i].PlusePerRote = 10000;
                m_MovePrm[i].AxisLeadRange = 10;
                if (1 == i)
                {
                    m_HomePrm[i]._bHomeDir = false;
                }
                if (0 == i)
                {
                    m_HomePrm[i]._bHomeDir = false;
                }
                if (3 == i || 2 == i)
                    m_ProPrietarySignal[i].oriSignal = 1;
                m_MotorType[i] = MotorType.SEVER;
                m_ManualEventHomeingStop[i] = new ManualResetEvent(false);
                m_AxisStates[i] = AxisState.NormalStop;
            }

        }
        public string CardName
        {
            get { return m_strCardName; }
        }

        public  double GetAxisPulsesPerMM(int nAxisNo)
        {
            return  (m_MovePrm[nAxisNo].PlusePerRote / m_MovePrm[nAxisNo].AxisLeadRange);
        }
        public double GetAxisSpeed(int nAxisNo, double type)
        {
            if (type == 0)
            {
                return m_MovePrm[nAxisNo].VelH;
            }
            else if (type == 1)
            {
                return m_MovePrm[nAxisNo].VelM;
            }
            else if (type == 2)
            {
                return m_MovePrm[nAxisNo].VelL;
            }
            else
            {
                return type;
            }
        }
        public  short GetSystemAxisNo( int AxisNo)
        {
            return (short)(AxisNo + GetMinAxisNo());
        }

        public string GetCardName() { return m_strCardName; }
        public int GetMinAxisNo() { return m_nMinAxisNo; }
        public int GetMaxAxisNo() { return m_nMaxAxisNo; }
        //public abstract int GetAxisNo(int IndexAxis)
        public int GetAxisNo(int IndexAxis)
        {
            return AxisInRang(IndexAxis) ? (IndexAxis - GetMinAxisNo()) : int.MaxValue;
        }
        public bool AxisInRang(int nAxisNo)
        {
            return nAxisNo >= m_nMinAxisNo && nAxisNo <= m_nMaxAxisNo;
        }
        public ulong GetCardIndex()
        {
            return m_nCardIndex;
        }
        public abstract bool Open();
        protected bool m_bOpen = false;
        public abstract bool IsOpen();
        public abstract bool Close();

        public abstract bool ServoOn(short nAxisNo);
        public abstract bool ServoOff(short nAxisNo);

        public abstract bool AbsMove(int nAxisNo, double nPos, double nSpeed);// 0 高速 1 中速 2 低速
        public abstract bool RelativeMove(int nAxisNo, double nPos, double nSpeed);
        //mm 转 脉冲
        public abstract bool TranMMToPluse(int nAxisNo,  ref double dSpeed, ref double acc ,ref double dec);

        public abstract bool TransMMToPluseForHomeParam(int nAxisNo, ref double dVelH, ref double dVelL, ref double dAccH, ref double dAccL, ref double dDecH, ref double dDecL);

        //public abstract bool AbsMove(int nAxisNo, double nPos, double dSpeed, double acc, double dec);// 0 高速 1 中速 2 低速
        //public abstract bool RelativeMove(int nAxisNo, double nPos, double nSpeed, double acc, double dec);
        public abstract bool JogMove(int nAxisNo, bool bPositive, int bStart, double nSpeed);
        public abstract bool StopAxis(int nAxisNo);

        public abstract bool StopEmg(int nAxisNo);
        public abstract bool ReasetAxis(int nAxisNo);

        public virtual bool ResetAllAxis()
        {
            return true;
        }

        public abstract long GetMotionIoState(int nAxisNo);
        public abstract bool GetServoState(int nAxisNo);
        public abstract AxisState IsAxisNormalStop(int nAxisNo);
        public abstract bool Home(int nAxisNo, int nParam);
        public abstract bool SetActutalPos(int nAxisNo, double pos);

        public abstract bool SetCmdPos(int nAxisNo, double Pos);
        public abstract AxisState IsHomeNormalStop(int nAxisNo);
        public abstract int GetAxisPos(int nAxisNo);
        public abstract int GetAxisActPos(int nAxisNo);
        public abstract int GetAxisCmdPos(int nAxisNo);
        public abstract bool isOrgTrig(int nAxisNo);

        public abstract bool IsInpos(int nAxisNo);

        public void SetAxisMovePrm(int nAxisNo, TMovePrm prm)
        {
            if (nAxisNo >= m_MovePrm.Length || nAxisNo < 0)
                return;
            m_MovePrm[nAxisNo] = prm;
        }


        public TMovePrm GetAxisMovePrm(int nAxisNo)
        {
            return m_MovePrm[nAxisNo];
        }
        public void SetAxisHomePrm(int nAxisNo, THomePrm homePrm)
        {
            if (nAxisNo >= m_HomePrm.Length || nAxisNo < 0)
                return;
            m_HomePrm[nAxisNo] = homePrm;
        }

        public THomePrm GetHomePrm(int nAxisNo)
        {
            return m_HomePrm[nAxisNo];
        }
        public string GetAxisName(int nAxisNo)
        {
            return m_strAxisName[nAxisNo];
        }
        public void SetAxisName(int nAxisNo, string strName)
        {
            if (nAxisNo >= m_strAxisName.Length || nAxisNo < 0)
                return;
            m_strAxisName[nAxisNo] = strName;
        }
        public int  FindAxisNoByName(string strName)
        {
            for(int i=0;i< m_strAxisName.Length;i++  )
            {
                if (m_strAxisName[i] == strName)
                    return GetMinAxisNo() + i;
            }

            return -1;
        }
        public MotorType GetMotorType(int nAxisNo)
        {
            return m_MotorType[nAxisNo];
        }
        public void SetMotorType(int nAxisNo, MotorType motorType)
        {
            if (nAxisNo >= m_MotorType.Length || nAxisNo < 0)
                return;
            m_MotorType[nAxisNo] = motorType;
        }
        public bool IsSever(int nAxisNo)
        {
            if (nAxisNo >= m_MotorType.Length || nAxisNo < 0)
                return false;
            return m_MotorType[nAxisNo] >= MotorType.SEVER;
        }
     
       public abstract bool AddAxisToGroup(int[] nAxisarr, ref object groupId);
       public virtual bool CloseAxisGroup(int[] nAxisArr, ref object group)
        {
            return true;
        }
        public virtual GpState GetGpState(object group)
        {
            return GpState.GpDisable;
        }
        public virtual int GetGpLeftSpace(object group)
        {
            return 4096;
        }
        public virtual int GpRunAlready(object group)
        {
            return 0;
        }
        public virtual  bool StopGp(object group)
        {
            return true;
        }
        public virtual bool Line2Axisabs(IntPtr group, int xAxis, int yAxis, double xpos, double ypos, double acc, double dec, double velrun, double velori = 0)
        {

            return true;
        }
      
        public virtual bool ResetGpErr(object group)
        {
            return true;
        }
        public abstract bool AddBufMove(object objGroup, BufMotionType type, int mode, int nAxisNum, double velHigh, double velLow, double[] Point1, double[] Point2);
        public abstract bool AddBufIo(object objGroup, string strIoName, bool bVal, int nAxisIndexInGroup = 0);

        public abstract bool AddBufDelay(object objGroup, int nTime);
        public abstract bool ClearBufMove(object objGroup);

        public abstract bool StartBufMove(object objGroup);
        public virtual bool SetBufMoveParam(object objGroup, double velhigh, double vellow, double acc, double dec)
        {
            return true;
        }
    }
   
 
    public struct MoveGroup
    {
        public dynamic _pGroup;//群组的索引或引用
        public int[] _nAxisArr;//群组的轴号（非系统号）
        public int[] _nSysAxisArr;//群组的轴号（系统号）
        public double _VelHighGp;//群组的高速
        public double _VelLowGp;//群组的低速
        public double _AccGp;//群组的加速度
        public double _DecGp;//群组的减速度
        public MotionCardBase _pCard; //群组所在卡的引用
        public List<BufMotionParam> _listBufMotionParams;//buf运动的集合
    }
    public class MotionMgr
    {

        private MotionMgr()
        {
        }
        ~MotionMgr()
        {
            m_bExit = true;
        }
        ILog _logger = LogManager.GetLogger("MotionMgr");
        private static MotionMgr _MotionObj = null;
        private static object _lock = new object();
        public static MotionMgr GetInstace()
        {

            if (_MotionObj == null)
            {
                lock (_lock)
                {
                    if (_MotionObj == null)
                    {
                        _MotionObj = new MotionMgr();
                        return _MotionObj;
                    }
                    else
                        return _MotionObj;
                }
            }
            else
                return _MotionObj;
        }

        public void AddCompensation( int nAixs, int nRelatedAxis, Dictionary<double ,double > RelatedAxisPosCompensation)
        {
            if(RelatedCompensation.ContainsKey(nAixs))
            {
                Dictionary<int, Dictionary<double, double>> val =new  Dictionary<int, Dictionary<double, double>>();
                if(RelatedCompensation[nAixs].ContainsKey(nRelatedAxis))
                    RelatedCompensation[nAixs][nRelatedAxis] = RelatedAxisPosCompensation;
                else
                    RelatedCompensation[nAixs].Add(nRelatedAxis,RelatedAxisPosCompensation);
            }
            else
            {
                Dictionary<int, Dictionary<double, double>> val = new Dictionary<int, Dictionary<double, double>>();
                val.Add(nRelatedAxis, RelatedAxisPosCompensation);
                RelatedCompensation.Add(nAixs, val);
            }
        }
        // 
        private Dictionary<int, Dictionary<int, Dictionary<double, double>>> RelatedCompensation = new Dictionary<int, Dictionary<int, Dictionary<double, double>>>();

        /// <summary>
        /// 获取轴的的补偿量
        /// </summary>
        /// <param name="nAxis"></param>
        /// <returns></returns>
        public double GetCompensationPos( int nAxis)
        {
            double dCompensationVal = 0;
            if (RelatedCompensation.ContainsKey(nAxis))
            {
                foreach ( var temp in RelatedCompensation[nAxis])
                {
                    double currntpos = 0;
                    if( MotionMgr.GetInstace().GetMotorType(nAxis) == MotorType.SEVER ||
                        MotionMgr.GetInstace().GetMotorType(nAxis) == MotorType.EcatAxis||
                          //MotionMgr.GetInstace().GetMotorType(nAxis) == MotorType.ClosedLoopStepping||
                           MotionMgr.GetInstace().GetMotorType(nAxis) == MotorType.EcatAxisModleSevro
                        )
                        currntpos = MotionMgr.GetInstace().GetAxisActPos(temp.Key);
                    else
                        currntpos = MotionMgr.GetInstace().GetAxisCmdPos(temp.Key);
                    
             
                    double dSmallPos = double.MinValue;
                    double dLargerPos = double.MaxValue;
                    double dSmallVal = double.MinValue;
                    double dLargerVal = double.MaxValue;
                    foreach( var s in temp.Value)
                    {
                        if (s.Value == currntpos)
                            dCompensationVal+= s.Key;
                        if (s.Value < currntpos)
                        {
                            dSmallVal = s.Value;
                            dSmallPos = s.Key;
                            continue;
                        }
                        if( s.Value > currntpos)
                        {
                            dLargerVal = s.Value;
                            dLargerPos = s.Key;
                            break;
                        }
                    }
                   if ( dSmallVal != double.MinValue  &&
                     dLargerVal != double.MinValue)
                        dCompensationVal+= dSmallVal + (dLargerVal - dSmallVal) / (dLargerPos - dSmallPos) * (currntpos - dSmallPos);
                }
            }
            return dCompensationVal;
        }
        /// <summary>
        /// 理论坐标（干涉仪） 坐标， 获取实际卡的位置
        /// </summary>
        /// <param name="nAxis"> 需要运动的轴</param>
        /// <param name="RelatedAxisAnDstPos">关联轴的目标位置</param>
        /// <returns></returns>
        public double GetMovePosFromTheoryPos(int nAxis,double dDstPos,   Dictionary< int, double> RelatedAxisAnDstPos)
        {
            if (RelatedCompensation.ContainsKey(nAxis))
            {
                double dVal = 0;
                foreach (var temp in RelatedCompensation[nAxis])
                {
                    //获取关联轴的当前位置
                    double dRelatedAxisDstPos = 0;
                    if (MotionMgr.GetInstace().GetMotorType(temp.Key) == MotorType.SEVER ||
                      MotionMgr.GetInstace().GetMotorType(temp.Key) == MotorType.EcatAxis ||
                        MotionMgr.GetInstace().GetMotorType(temp.Key) == MotorType.CLStepSVOn ||
                        MotionMgr.GetInstace().GetMotorType(temp.Key) == MotorType.CLStepSVOFF ||
                         MotionMgr.GetInstace().GetMotorType(temp.Key) == MotorType.EcatAxisModleSevro
                      )
                        dRelatedAxisDstPos = MotionMgr.GetInstace().GetAxisActPos(temp.Key);
                    else
                        dRelatedAxisDstPos = MotionMgr.GetInstace().GetAxisCmdPos(temp.Key);

                    if (RelatedAxisAnDstPos.ContainsKey(temp.Key))
                        dRelatedAxisDstPos = RelatedAxisAnDstPos[temp.Key];

                    double dSmallPos = double.MinValue;
                    double dLargerPos = double.MaxValue;
                    double dSmallVal = double.MinValue;
                    double dLargerVal = double.MaxValue;
                    foreach (var s in temp.Value)
                    {
                        if (s.Value == dRelatedAxisDstPos)
                            dVal+= s.Key;
                        if (s.Value < dRelatedAxisDstPos)
                        {
                            dSmallVal = s.Value;
                            dSmallPos = s.Key;
                            continue;
                        }
                        if (s.Value > dRelatedAxisDstPos)
                        {
                            dLargerVal = s.Value;
                            dLargerPos = s.Key;
                            break;
                        }
                    }
                    if (dSmallVal != double.MinValue &&dLargerVal != double.MinValue)
                        dVal += dSmallVal + (dLargerVal - dSmallVal) / (dLargerPos - dSmallPos) * (dDstPos - dSmallPos); 
                }
                return dDstPos - dVal;
            }
            return dDstPos;
        }

        public int[]  GetAxisRelatedSysAxiss( int nAxis)
        {
            if (RelatedCompensation.ContainsKey(nAxis))
            {
               return RelatedCompensation[nAxis].Keys.ToArray();

            }
            return null;

        }
        public bool IsSafeFunRegister(IsSafeWhenAxisMoveHandler isSafeWhenAxisMoveHandler)
        {
            if (MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove != null)
            {
                Delegate[] delegates = MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove.GetInvocationList();
                if (delegates != null && delegates.Length > 0 )
                {
                    for ( int i=0; i< delegates.Length;i++)
                    {
                        delegates[i] = isSafeWhenAxisMoveHandler;
                        return true;
                    }
                }
            }
            return false;
        }
      

        public List<MotionCardBase> GetCardList()
        {
            return m_lisCard;
        }
        private List<MotionCardBase> m_lisCard = new List<MotionCardBase>();
        public void AddCard(string strName, ulong nCardNo, int nAxisMin, int nAxisMax)
        {
            // m_lisCard.Add(card);
            Assembly assembly = Assembly.GetAssembly(typeof(MotionCardBase));
            string name = "MotionIoLib.Motion_" + strName;
            //Motion_Advantech
            Type type = assembly.GetType(name);
            bool flag = type == null;
            if (flag)
            {
                throw new Exception(string.Format($"卡类型：{strName} 卡号：{nCardNo}+运动控制卡{0}找不到可用的封装类，请确认motionio.dll是否正确或配置错误?" ));
            }
            object[] args = new object[]
            {
              nCardNo,
              strName,
              nAxisMin,
              nAxisMax
            };
            this.m_lisCard.Add(Activator.CreateInstance(type, args) as MotionCardBase);
        }
        public bool OpenAllCard()
        {
            bool bOpenFlag = true;
            foreach (var temp in m_lisCard)
            {
                // bOpenFlag=temp.Value.Close();
                  bOpenFlag = bOpenFlag & temp.Open();
            }
            Thread thread = new Thread(ThreadMonitor);
            thread.IsBackground = true;
            thread.Start();
            return bOpenFlag;
        }
        public void SetAxisHomeParam(int nAxisNo, THomePrm prm)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                tempCardBase.SetAxisHomePrm(nAxis, prm);
            }
        }
        public THomePrm GetAxisHomePrm(int nAxisNo)
        {
            THomePrm prm = new THomePrm();
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.GetHomePrm(nAxis);
            }
            else
                return prm;
        }
        public void SetAxisMoveParam(int nAxisNo, TMovePrm prm)
        {
            _logger.Info(string.Format("修改{0}的参数", nAxisNo));
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                tempCardBase.SetAxisMovePrm(nAxis, prm);
            }
        }
        public TMovePrm GetAxisMovePrm(int nAxisNo)
        {
            TMovePrm prm = new TMovePrm();
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.GetAxisMovePrm(nAxis);
            }
            else
                return prm;
        }
        public void SetMotorType(int nAxisNo, MotorType motorType)
        {

            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            AxisConfig axisConfig = new AxisConfig();
            if (tempCardBase != null)
            {
                GetAxisConfig(nAxisNo, ref axisConfig);
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                if (tempCardBase.GetMotorType(nAxis) != motorType)
                    if (m_eventChangeMotionConfig != null)
                        m_eventChangeMotionConfig(nAxisNo, axisConfig);
                axisConfig.motorType = motorType;
                SetAxisConfig(nAxisNo, axisConfig);
                _logger.Info(string.Format("修改{0}的电机类型", nAxisNo));
                tempCardBase.SetMotorType(nAxis, motorType);
            }

        }
        public MotorType GetMotorType(int nAxisNo)
        {
            MotorType motorType = new MotorType();
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.GetMotorType(nAxis);
            }
            else
                return motorType;
        }
        public void SetAxisHomeFinishFlag(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                _logger.Info(string.Format("{0}轴：{1}：置位 回原点完成标志", nAxisNo, GetAxisName(nAxisNo)));
                tempCardBase.SetHomeFinishFlag(nAxis);
              
            }
        }
        public void SetAxisHomeingFlag(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                _logger.Info(string.Format("{0}轴：{1}：置位 回原点进行中标志", nAxisNo, GetAxisName(nAxisNo)));
                tempCardBase.SetHomeingFlag(nAxis);

            }
        }
        
        public void ReasetAxisHomeFinishFlag(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                _logger.Info(string.Format("{0}轴：{1}：复位 回原点完成标志", nAxisNo, GetAxisName(nAxisNo)));
                tempCardBase.ReasetHomeFinishFlag(nAxis);
                
            }
        }
         public AxisHomeFinishFlag GetHomeFinishFlag(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                AxisHomeFinishFlag axisHomeFinishFlag = tempCardBase.GetHomeFinishFlag(nAxis);
                //_logger.Info(string.Format("获取{0}轴：{1}： 回原点完成标志{2}", nAxisNo, GetAxisName(nAxisNo), axisHomeFinishFlag));

                return axisHomeFinishFlag;
            }
            else
                return AxisHomeFinishFlag.NoHome;
        }

        public string GetAxisName(int nAxisNo)
        {
            string strAxisName = "";
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.GetAxisName(nAxis);
            }
            else
                return strAxisName;
        }
        public void SetAxisName(int nAxisNo, string strName)
        {

            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                tempCardBase.SetAxisName(nAxis, strName);
            }
        }

        public bool Close()
        {
            bool bCloseFlag = true;
            foreach (var temp in m_lisCard)
            {
                bCloseFlag = bCloseFlag & temp.Close();
            }
            return bCloseFlag;
        }
        public MotionCardBase GetCardByIndexAxis(int nAxisNo)
        {
            MotionCardBase tempCardBase = null;
            foreach (var temp in m_lisCard)
            {
                if (temp.AxisInRang(nAxisNo))
                    tempCardBase = temp;
            }
            return tempCardBase;
        }
        public int GetAxisNoByName(string strAxisName)
        {
            int nAxisNo = 0;
            foreach(var temp in m_lisCard)
            {
                nAxisNo = temp.FindAxisNoByName(strAxisName);
                if (nAxisNo != -1)
                    return nAxisNo;

            }
            return -1;
        }
        public bool ServoOn(short nAxisNo)
        {

            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                //if (tempCardBase.IsSever(nAxis))
                //    return tempCardBase.ServoOn((short)nAxis);
                //else
                //    return false;

               if( MotionMgr.GetInstace().GetMotorType(nAxisNo) == MotorType.CLStepSVOFF)
                    return tempCardBase.ServoOff((short)nAxis);
                return tempCardBase.ServoOn((short)nAxis);
            }
            else
                return false;
        }
        public bool ServoOff(short nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                //if (tempCardBase.IsSever(nAxis))
                //    return tempCardBase.ServoOff((short)nAxis);
                //else
                //    return false;

                if (MotionMgr.GetInstace().GetMotorType(nAxisNo) == MotorType.CLStepSVOFF)
                    return tempCardBase.ServoOn((short)nAxis);
                return tempCardBase.ServoOff((short)nAxis);
            }
            else
                return false;
        }
        public bool AbsMove(int nAxisNo, double nPos, double nSpeed)
        {
            
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                double currentpos = long.MaxValue;
                double destpos = nPos;
                currentpos = GetAxisPos(nAxisNo);

                //轴运动安全检查
                bool bSave = true;
                if (m_eventIsSafeWhenAxisMove != null)
                {
                    Delegate[] delegates = m_eventIsSafeWhenAxisMove.GetInvocationList();
                    foreach( var tem in  delegates)
                    {
                        MethodInfo methodInfo = tem.GetMethodInfo();
                        bSave = bSave & (bool)methodInfo.Invoke(null, new object[] { nAxisNo, currentpos, destpos, destpos > currentpos ? MoveType.PTP_P:MoveType.PTP_N});
                    }
                }
                if (!bSave)
                {
                    _logger.Info(string.Format("{0}轴AbsMove 运动到{1}开始 安全检查失败", nAxisNo, destpos));
                   
                    return false;
                }
               // _logger.Info(string.Format("{0}轴AbsMove 运动到{1}开始", nAxisNo, destpos));
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                double rate = tempCardBase.GetAxisPulsesPerMM(nAxis);
                return tempCardBase.AbsMove(nAxis, nPos* rate, nSpeed);
            }
            else
                return false;
        }
        public double GetAxisPulsesPerMM(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int  nAxis = tempCardBase.GetAxisNo(nAxisNo);
            double rate = tempCardBase.GetAxisPulsesPerMM(nAxis);
            return rate;

        }
        public double GetAxisSpeed(int nAxisNo, double tpye)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = tempCardBase.GetAxisNo(nAxisNo);
            double rate = tempCardBase.GetAxisSpeed(nAxis, tpye);
            return rate;

        }
        public bool RelativeMove(int nAxisNo, double nPos, double nSpeed)
        {
           
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                double currentpos = long.MaxValue;
                double destpos = nPos;
                currentpos = GetAxisPos(nAxisNo);
                destpos = nPos+ currentpos;
                //轴运动安全检查
                bool bSave = true;
                if (m_eventIsSafeWhenAxisMove != null)
                {
                    Delegate[] delegates = m_eventIsSafeWhenAxisMove.GetInvocationList();
                    foreach (var tem in delegates)
                    {
                        MethodInfo methodInfo = tem.GetMethodInfo();
                        bSave = bSave & (bool)methodInfo.Invoke(null, new object[] { nAxisNo, currentpos, destpos, destpos > currentpos ? MoveType.PTP_P : MoveType.PTP_N });
                    }
                }
                if (!bSave)
                {
                    _logger.Info(string.Format("{0}轴RelativeMove 运动到{1}失败 安全检查不通过", nAxisNo,destpos));
                    return false;
                }
                   
                //_logger.Info(string.Format("{0}轴RelativeMove 运动到{1}开始", nAxisNo, destpos));
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                double rate = tempCardBase.GetAxisPulsesPerMM(nAxis);
                return tempCardBase.RelativeMove(nAxis, nPos* rate, nSpeed);
            }
            else
                return false;

        }
        public bool JogMove(int nAxisNo, bool bPositive, int bStart, double nSpeed)
        {
            
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                double currentpos = long.MaxValue;
                double destpos = long.MaxValue; 
                currentpos = GetAxisPos(nAxisNo);
                if(bPositive)
                     destpos = currentpos + 100;
                else
                    destpos = currentpos - 100;
                //轴运动安全检查
                bool bSave = true;
                if (m_eventIsSafeWhenAxisMove != null)
                {
                    Delegate[] delegates = m_eventIsSafeWhenAxisMove.GetInvocationList();
                    foreach (var tem in delegates)
                    {
                        MethodInfo methodInfo = tem.GetMethodInfo();
                        bSave = bSave & (bool)methodInfo.Invoke(null, new object[] { nAxisNo, currentpos, destpos, bPositive ? MoveType.JOG_P : MoveType.JOG_N });
                    }
                }
                if (!bSave)
                {
                    _logger.Info(string.Format("{0}轴JogMove 运动失败 安全检查不通过", nAxisNo));
                    return false;
                }
                   
             //   _logger.Info(string.Format("{0}轴JogMove 运动开始", nAxisNo));
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.JogMove(nAxis, bPositive, bStart, nSpeed);
            }
            else
                return false;
        }
        public bool StopAxis(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                _logger.Info(string.Format("{0}轴StopAxis 运动停止", nAxisNo));
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.StopAxis(nAxis);
            }
            else
                return false;
        }
        public long GetMotionIoState(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.GetMotionIoState(nAxis);
            }
            else
                return 0;
        }
        public bool GetServoState(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.GetServoState(nAxis);
            }
            else
                return false;
        }
        public AxisState IsAxisNormalStop(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.IsAxisNormalStop(nAxis);
            }
            else
                return 0;
        }
        public bool Home(int nAxisNo, int nParam)
        {
           
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                double currentpos = long.MaxValue;
                double destpos = long.MaxValue;
                currentpos = GetAxisPos(nAxisNo);
                destpos = currentpos;
                //轴运动安全检查
                bool bSave = true;
                if (m_eventIsSafeWhenAxisMove != null)
                {
                    Delegate[] delegates = m_eventIsSafeWhenAxisMove.GetInvocationList();
                    foreach (var tem in delegates)
                    {
                        MethodInfo methodInfo = tem.GetMethodInfo();
                        bSave = bSave & (bool)methodInfo.Invoke(null, new object[] { nAxisNo, currentpos, destpos,GetAxisHomePrm(nAxisNo)._bHomeDir ? MoveType.Home_P : MoveType.Home_N });
                    }
                }
                if (!bSave)
                {
                    _logger.Info(string.Format("{0}轴Home 运动停止", nAxisNo));
                    return false;
                }
                 
                _logger.Info(string.Format("{0}轴Home Home运动开始", nAxisNo));
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.Home(nAxis, 0);
            }
            else
                return false;
        }
        public bool isOrgTrig(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                bool bsignal = tempCardBase.isOrgTrig(nAxis); ;
                return bsignal;
            }
            else
                return false;
        }
        public bool IsHomeNormalStop(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                AxisState axisState = tempCardBase.IsHomeNormalStop(nAxis); ;
                return axisState== AxisState.NormalStop;
            }
            else
                return false;
        }
       
        public bool SetAxisActualPos(int nAxisNo, double pos)
        {
            if (nAxisNo == -1)
                return true;
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.SetActutalPos(nAxis, pos);
            }
            else
                return false;
        }

        public bool SetAxisCmdPos(int nAxisNo, double pos)
        {
            if (nAxisNo == -1)
                return true;
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.SetCmdPos(nAxis, pos);
            }
            else
                return false;
        }
        public double GetAxisPos(int nAxisNo)
        {
            if(nAxisNo==-1)
                return int.MaxValue;
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                double rate = tempCardBase.GetAxisPulsesPerMM(nAxis);
                if (rate == 0)
                    rate = 1;
                double dCompensationPos =  GetCompensationPos(nAxisNo);
                return tempCardBase.GetAxisPos(nAxis)/ rate+ dCompensationPos;
            }
            else
                return int.MaxValue;
        }
        public double GetAxisActPos(int nAxisNo)
        {
            if (nAxisNo == -1)
                return int.MaxValue;
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                double rate = tempCardBase.GetAxisPulsesPerMM(nAxis);
                if (rate == 0)
                    rate = 1;

                return tempCardBase.GetAxisActPos(nAxis) / rate;
            }
            else
                return int.MaxValue;
        }
        public double GetAxisCmdPos(int nAxisNo)
        {
            if (nAxisNo == -1)
                return int.MaxValue;
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                double rate = tempCardBase.GetAxisPulsesPerMM(nAxis);
                if (rate == 0)
                    rate = 1;

                return tempCardBase.GetAxisCmdPos(nAxis) / rate;
            }
            else
                return int.MaxValue;
        }
        public  bool  IsInPos(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.IsInpos(nAxis);
            }
            else
                return false; 
        }
        public bool StopEmg(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.StopEmg(nAxis);
            }
            else
                return false; 
        }
        public bool StopEmg()
        {
            int nAxis = 0;
            bool result = true;
            foreach (var temp in m_lisCard)
            {
                for (int i = temp.GetMinAxisNo(); i <= temp.GetMinAxisNo(); i++)
                {
                    nAxis = temp.GetAxisNo(i);
                    result = result & temp.StopEmg(nAxis);
                }

            }
            return result;
        }
        public bool ResetAxis()
        {
            int nAxis = 0;
            bool result = true;
            foreach (var temp in m_lisCard)
            {
                for (int i = temp.GetMinAxisNo(); i <= temp.GetMinAxisNo(); i++)
                {
                    nAxis = temp.GetAxisNo(i);
                    result = result & temp.ReasetAxis(nAxis);
                }

            }
            return result;
        }
        public bool ResetAxis(int nAxisNo)
        {
            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisNo);
            int nAxis = 0;
            if (tempCardBase != null)
            {
                nAxis = tempCardBase.GetAxisNo(nAxisNo);
                return tempCardBase.ReasetAxis(nAxis);
            }
            else
                return false;
        }

        AxisIOState[] m_arrAxisIOState = new AxisIOState[100];
        AxisPos[] m_arrAxisPos = new AxisPos[100];
        AxisConfig[] m_arrAxisConfig = new AxisConfig[100];
        object m_lockReadAxisIO = new object();
        public delegate void ChangeMotionIoOrPosHandler(int nAxisNo, bool[] bChangeBitArr, AxisIOState state, AxisPos axisPos);
        public event ChangeMotionIoOrPosHandler m_eventChangeMotionIoOrPos;
        public delegate bool IsSafeWhenAxisMoveHandler(int nAxisNo,double  currentpos, double dsstpos, MoveType moveType);
        public event IsSafeWhenAxisMoveHandler m_eventIsSafeWhenAxisMove = null;

        //轴信号处理
        public delegate void AxismSinglHandler(string IoName, bool bCurrentState);
        public event AxismSinglHandler m_eventAxisSingl = null;




        public delegate void ChangeMotionConfigHandler(int nAxis, AxisConfig axisConfig);
        public event ChangeMotionConfigHandler m_eventChangeMotionConfig = null;
        private bool m_bExit = false;
        public void ThreadMonitor()
        {
            //驱动器报警存在第1位
            //正限位报警存在第2位
            //负限位报警存在第3位
            //急停触发存在第5位
            //电机到位标志存在第7位
            //电机使能标志存在第8位
            long val;
            AxisIOState axisIOState = new AxisIOState();
            AxisPos axisPos = new AxisPos();
            bool[] bChangeBitArr = new bool[8] { false, false, false, false, false, false, false, false };
            int indexbit = 0;
            int nMinAxisNo = 0;
            double rate = 1;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (!m_bExit)
            {
                
                foreach (var temp in m_lisCard)
                {
                    nMinAxisNo = temp.GetMinAxisNo();
                    for (int index = nMinAxisNo; index <= temp.GetMaxAxisNo(); index++)
                    {
                        if (!temp.IsOpen())
                        {
                            Thread.Sleep(100);
                            continue;
                        }
                     
                        val = temp.GetMotionIoState(index - nMinAxisNo);
                        rate = temp.GetAxisPulsesPerMM(index - nMinAxisNo);
                     //   if(stopwatch.ElapsedMilliseconds%500==0)
                     //       temp.ResetAllAxis();
                        if (rate == 0)
                            rate = 1;
                        axisPos._lActPos = temp.GetAxisActPos(index - nMinAxisNo)/ rate;
                        axisPos._lCmdPos = temp.GetAxisCmdPos(index - nMinAxisNo)/ rate;
                        lock (m_lockReadAxisIO)
                        {
                            axisIOState._bAlarm = (val & (0x01 << 0)) > 0;
                            axisIOState._bLimtP = (val & (0x01 << 1)) > 0;
                            axisIOState._bLimtN = (val & (0x01 << 2)) > 0;
                            axisIOState._bOrg = (val & (0x01 << 3)) > 0;
                            axisIOState._bEmg = (val & (0x01 << 4)) > 0;
                            axisIOState._bSeverOn = (val & (0x01 << 7)) > 0;
                            if (m_arrAxisIOState[index]._bOrg != axisIOState._bOrg ||
                                m_arrAxisIOState[index]._bAlarm != axisIOState._bAlarm ||
                                m_arrAxisIOState[index]._bLimtN != axisIOState._bLimtN ||
                                m_arrAxisIOState[index]._bLimtP != axisIOState._bLimtP ||
                              /*  m_arrAxisIOState[index]._bEmg != */axisIOState._bEmg ||
                                m_arrAxisIOState[index]._bEmg != axisIOState._bEmg ||
                                m_arrAxisIOState[index]._bSeverOn != axisIOState._bSeverOn ||
                                axisPos._lActPos != m_arrAxisPos[index]._lActPos ||
                                axisPos._lCmdPos != m_arrAxisPos[index]._lCmdPos
                                )
                            {
                                indexbit = 0;
                                if (m_arrAxisIOState[index]._bSeverOn != axisIOState._bSeverOn)
                                    bChangeBitArr[0] = true;
                                if (m_arrAxisIOState[index]._bAlarm != axisIOState._bAlarm)
                                {
                                    if (m_eventAxisSingl != null && axisIOState._bLimtP)
                                        m_eventAxisSingl("报警", axisIOState._bLimtP);
                                    bChangeBitArr[1] = true;
                                }
                                if (m_arrAxisIOState[index]._bLimtP != axisIOState._bLimtP)
                                {
                                    if (m_eventAxisSingl != null && axisIOState._bLimtP)
                                        m_eventAxisSingl("正极限", axisIOState._bLimtP);
                                    bChangeBitArr[2] = true;
                                }
                                
                                if (m_arrAxisIOState[index]._bLimtN != axisIOState._bLimtN)
                                {
                                    if (m_eventAxisSingl != null && axisIOState._bLimtN)
                                        m_eventAxisSingl("负极限", axisIOState._bLimtN);
                                    bChangeBitArr[3] = true;
                                }
                                if (m_arrAxisIOState[index]._bOrg != axisIOState._bOrg)
                                {

                                    bChangeBitArr[4] = true;

                                }
                                if (/*m_arrAxisIOState[index]._bEmg != */axisIOState._bEmg)
                                {
                                    if (m_eventAxisSingl != null && axisIOState._bEmg)
                                        m_eventAxisSingl("急停", axisIOState._bEmg);
                                    bChangeBitArr[5] = true;
                                }
                                if (m_arrAxisIOState[index]._bEmg != axisIOState._bEmg)
                                {
                                    if (m_eventAxisSingl != null && axisIOState._bEmg)
                                        m_eventAxisSingl("急停", axisIOState._bEmg);
                                    bChangeBitArr[5] = true;
                                }
                                if (axisPos._lActPos != m_arrAxisPos[index]._lActPos)
                                    bChangeBitArr[6] = true;
                                if (axisPos._lCmdPos != m_arrAxisPos[index]._lCmdPos)
                                    bChangeBitArr[7] = true;
                                m_arrAxisIOState[index] = axisIOState;
                                m_arrAxisPos[index] = axisPos;
                                if (m_eventChangeMotionIoOrPos != null)
                                    m_eventChangeMotionIoOrPos(index, bChangeBitArr, axisIOState, axisPos);

                            }

                        }
                    }
                    for (int i = 0; i < bChangeBitArr.Length; i++)
                        bChangeBitArr[i] = false;
                }
                Thread.Sleep(100);
            }
        }
        public bool GetAxisIOState(int nAxis, ref AxisIOState axisIOState)
        {
            if (nAxis >= m_arrAxisIOState.Length || nAxis < 0)
                return false;
            else
            {
                lock (m_lockReadAxisIO)
                {
                    axisIOState = m_arrAxisIOState[nAxis];
                }
                return true;
            }
        }
        public bool GetAxisPos(int nAxis, ref AxisPos axispos)
        {
            if (nAxis >= m_arrAxisPos.Length || nAxis < 0)
                return false;
            else
            {
                lock (m_lockReadAxisIO)
                {
                    axispos = m_arrAxisPos[nAxis];
                }
                return true;
            }
        }
        public bool GetAxisConfig(int nAxis, ref AxisConfig axisConfig)
        {
            if (nAxis >= m_arrAxisConfig.Length || nAxis < 0)
                return false;
            else
            {
                axisConfig = m_arrAxisConfig[nAxis];
                return true;
            }
        }
        public void SetAxisConfig(int nAxis, AxisConfig axisConfig)
        {
            if (nAxis >= m_arrAxisConfig.Length || nAxis < 0)
                return;
            else
            {
                m_arrAxisConfig[nAxis]= axisConfig ;
                return;
            }
        }

        ConcurrentDictionary<string, MoveGroup> m_dicGroup = new ConcurrentDictionary<string, MoveGroup>();
        public MoveGroup GetMoveGrop(string strGpName)
        {
            MoveGroup moveGroup=new MoveGroup();
            if (m_dicGroup.ContainsKey(strGpName))
            {
                moveGroup = m_dicGroup[strGpName];
                return moveGroup;
            }
            return moveGroup;

        }
        public void SetMoveGrop(string strGpName,MoveGroup moveGroup)
        {
            if (m_dicGroup.ContainsKey(strGpName))
            {
               m_dicGroup[strGpName]= moveGroup;
            }
        }
        /// <summary>
        /// 添加一个群组
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="nAxisArr"></param>
        public bool AddAxisToGroup(string GroupName, int[] nAxisArr)
        {
            if (nAxisArr== null ||nAxisArr.Length <= 0)
                return false;
            MoveGroup moveGroup = new MoveGroup();
            moveGroup._nAxisArr = new int[nAxisArr.Length];
            moveGroup._nSysAxisArr = new int[nAxisArr.Length];
            for (int i = 0; i < nAxisArr.Length; i++)
            {
                moveGroup._nAxisArr[i] = GetCardByIndexAxis(nAxisArr[i]).GetAxisNo(nAxisArr[i]);
                moveGroup._nSysAxisArr[i] = nAxisArr[i];
            }
               

            MotionCardBase tempCardBase = GetCardByIndexAxis(nAxisArr[0]);
            moveGroup._pCard = tempCardBase;

        //    IntPtr intPtr=IntPtr.Zero;
      
            object intPtr = 1;
            if (!m_dicGroup.ContainsKey(GroupName))
            {
                if (m_dicGroup.Count == 0)
                    intPtr = 1;
                else
                    intPtr = 2;

            }
            else
            {
                intPtr = m_dicGroup[GroupName]._pGroup;
            }
           try
            {
          
                if ((bool)tempCardBase?.AddAxisToGroup(moveGroup._nAxisArr, ref intPtr))
                {
                    moveGroup._pGroup = intPtr;
                    moveGroup._listBufMotionParams = new List<BufMotionParam>();
                    moveGroup._listBufMotionParams.Clear();
                    if(!m_dicGroup.ContainsKey(GroupName))
                    {
                        m_dicGroup.TryAdd(GroupName, moveGroup);
                    }
                    else
                    {
                        m_dicGroup[GroupName]= moveGroup;
                    }
                  
                    _logger.Info(string.Format("{0} 群组创建成功", GroupName));
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch(Exception e)
            {
                _logger.Info(string.Format("{0} 群组创建异常：{1}", GroupName,e.Message));
            }

           
            return false;
        }
       
        public void SetGpParam(string strGpName, double acc, double dec, double velrun, double velori = 0)
        {
            if (m_dicGroup.ContainsKey(strGpName))
            {
                MoveGroup moveGroup = m_dicGroup[strGpName];
                moveGroup._AccGp = acc;
                moveGroup._DecGp = dec;
                moveGroup._VelHighGp = velrun;
                moveGroup._VelLowGp = velori;
                m_dicGroup[strGpName] = moveGroup;

            }
        }

        public GpState GetGpState(string strGpName)
        {
            if (m_dicGroup.ContainsKey(strGpName))
            {
                MoveGroup moveGroup = m_dicGroup[strGpName];
                if (moveGroup._pCard == null)
                {
                    _logger.Info("GetGpState  卡的句柄为空");
                    return GpState.GpDisable;
                }

                if (moveGroup._nAxisArr == null || moveGroup._nAxisArr.Length <=1)
                {
                    _logger.Info("GetGpState  群组的轴数小于等于1");
                    return GpState.GpDisable;
                }
                if (moveGroup._pGroup == null )
                {
                    _logger.Info("GetGpState  群组的句柄的引用为空");
                    return GpState.GpDisable;
                }
                
                return moveGroup._pCard.GetGpState(moveGroup._pGroup);

            }
            return GpState.GpDisable;
        }
        public bool  StopGp(string strGpName)
        {
            if (m_dicGroup.ContainsKey(strGpName))
            {
                MoveGroup moveGroup = m_dicGroup[strGpName];
                if (moveGroup._pCard == null)
                {
                    _logger.Info("GetGpState  卡的句柄为空");
                    return false;
                }

                if (moveGroup._nAxisArr == null || moveGroup._nAxisArr.Length <= 1)
                {
                    _logger.Info("GetGpState  群组的轴数小于等于1");
                    return false;
                }
                if (moveGroup._pGroup == null)
                {
                    _logger.Info("GetGpState  群组的句柄的引用为空");
                    return false;
                }
                IntPtr intPtrgroup = (IntPtr)moveGroup._pGroup;
                return moveGroup._pCard.StopGp(intPtrgroup);

            }
            return false;
        }
        public  int GetGpLeftSpace(string strGpName)
        {
            if (m_dicGroup.ContainsKey(strGpName))
            {
                MoveGroup moveGroup = m_dicGroup[strGpName];
                if (moveGroup._pCard == null)
                {
                    _logger.Info("GetGpState  卡的句柄为空");
                    return -1;
                }

                if (moveGroup._nAxisArr == null || moveGroup._nAxisArr.Length <= 1)
                {
                    _logger.Info("GetGpState  群组的轴数小于等于1");
                    return -1;
                }
                if (moveGroup._pGroup == null)
                {
                    _logger.Info("GetGpState  群组的句柄的引用为空");
                    return -1;
                }
             
                return moveGroup._pCard.GetGpLeftSpace(moveGroup._pGroup);

            }
            return -1;
        }
        public  int GpRunAlready(string strGpName)
        {
            if(m_dicGroup.ContainsKey(strGpName))
            {
                MoveGroup moveGroup = m_dicGroup[strGpName];
                if (moveGroup._pCard == null)
                {
                    _logger.Info("GetGpState  卡的句柄为空");
                    return 0;
                }

                if (moveGroup._nAxisArr == null || moveGroup._nAxisArr.Length <= 1)
                {
                    _logger.Info("GetGpState  群组的轴数小于等于1");
                    return 0;
                }
                if (moveGroup._pGroup == null)
                {
                    _logger.Info("GetGpState  群组的句柄的引用为空");
                    return 0;
                }

                return moveGroup._pCard.GpRunAlready(moveGroup._pGroup);

            }
            return 0;
        }
        public bool RestGpErr(string strGpName)
        {
            if (m_dicGroup.ContainsKey(strGpName))
            {
                MoveGroup moveGroup = m_dicGroup[strGpName];
                if (moveGroup._pCard == null)
                {
                    _logger.Info("GetGpState  卡的句柄为空");
                    return false;
                }

                if (moveGroup._nAxisArr == null || moveGroup._nAxisArr.Length <= 1)
                {
                    _logger.Info("GetGpState  群组的轴数小于等于1");
                    return false;
                }
                if (moveGroup._pGroup == null)
                {
                    _logger.Info("GetGpState  群组的句柄的引用为空");
                    return false;
                }
               // IntPtr intPtrgroup = (IntPtr)moveGroup._pGroup;
                return moveGroup._pCard.ResetGpErr(moveGroup._pGroup);

            }
            return false;
        }
        public bool Line2Axisabs( string strGpName, double xAxisPos, double yAxisPos)
        {
            double[] posDst = new double[] { xAxisPos, yAxisPos };
            if (m_dicGroup.ContainsKey(strGpName))
            {
                MoveGroup moveGroup = m_dicGroup[strGpName];
                if (moveGroup._pCard == null)
                {
                    _logger.Info("Line2Axisabs 运动时 卡的句柄为空");
                    return false;
                }
                  
                if (moveGroup._nAxisArr == null || moveGroup._nAxisArr.Length!=2)
                {
                    _logger.Info("Line2Axisabs 运动时 群组的轴数字为空或者不等于2");
                    return false;
                }
                   
                //轴运动安全检查
                bool bSave = true;
                if (m_eventIsSafeWhenAxisMove != null)
                {
                    Delegate[] delegates = m_eventIsSafeWhenAxisMove.GetInvocationList();
                    double currentpos=0;
                    double destpos = 0;
                    foreach (var tem in delegates)
                    {
                        MethodInfo methodInfo = tem.GetMethodInfo();
                        for( int nAxisNo = 0; nAxisNo < 2; nAxisNo++)
                        {
                            destpos = posDst[nAxisNo];
                            currentpos = MotionMgr.GetInstace().GetAxisPos(moveGroup._nSysAxisArr[nAxisNo]);
                            bSave = bSave & (bool)methodInfo.Invoke(null, new object[] { moveGroup._nSysAxisArr[nAxisNo], currentpos, destpos,MoveType.Line2AxisAbs});
                        }

                    }
                }
                if (!bSave)
                {
                    _logger.Info(string.Format("{0}群组Line2AxisAbs运动停止", strGpName));
                    return false;
                }
                double ratex = moveGroup._pCard.GetAxisPulsesPerMM(moveGroup._nAxisArr[0]);
                double ratey = moveGroup._pCard.GetAxisPulsesPerMM(moveGroup._nAxisArr[1]);
                bool? brtn =  moveGroup._pCard?.Line2Axisabs((IntPtr)moveGroup._pGroup, moveGroup._nAxisArr[0], moveGroup._nAxisArr[1],
                    xAxisPos* ratex, yAxisPos* ratey,
                    moveGroup._AccGp,moveGroup._DecGp,
                    moveGroup._VelHighGp, moveGroup._VelLowGp);
                if (brtn == null || brtn == false)
                    return false;
                return true;
            }
            return false;
        }
        
        public void CloseAxisToGroup(int[] nAxisArr, string GroupName)
        {
           if( m_dicGroup.ContainsKey(GroupName))
            {
                int[] _nAxisArr = new int[nAxisArr.Length];
                for (int i = 0; i < nAxisArr.Length; i++)
                {
                    _nAxisArr[i] = GetCardByIndexAxis(nAxisArr[i]).GetAxisNo(nAxisArr[i]);
                }
                MotionCardBase tempCardBase = m_dicGroup[GroupName]._pCard;
                object obj =(object)m_dicGroup[GroupName]._pGroup;
                tempCardBase.CloseAxisGroup(_nAxisArr, ref obj);
            }
        }
        ///群组 添加运动
        public bool AddBufMove(string GroupName, BufMotionType type, int mode, int nAxisNum, double velHigh, double velLow, double[] Point1, double[] Point2)
        {
            MoveGroup moveGroup = new MoveGroup();
            if (m_dicGroup.TryGetValue(GroupName, out moveGroup))
            {
                if (moveGroup._listBufMotionParams == null)
                    return false;
                if (type == BufMotionType.buf_IO || type == BufMotionType.buf_Delay || type == BufMotionType.buf_end)
                    return false;
                moveGroup._listBufMotionParams.Add(new BufMotionParam
                {
                    _Type = type,
                    _mode = mode,
                    _nAxisNum = nAxisNum,
                    _VelHigh = velHigh,
                    _VelLow = velLow,
                    _point1 = Point1,
                    _point2 = Point2
                });
                return true;
            }
            else
                return false;
        }

        ///群组 添加IO
        public bool AddBufIo(string GroupName, string IoName, bool bIoState)
        {

            MoveGroup moveGroup = new MoveGroup();
            if (m_dicGroup.TryGetValue(GroupName, out moveGroup))
            {
                if (moveGroup._listBufMotionParams == null)
                    return false;
                if (!IOMgr.GetInstace().GetOutputDic().ContainsKey(IoName))
                    return false;
                int nNum = moveGroup._nAxisArr == null ? 0 : moveGroup._nAxisArr.Length;
                moveGroup._listBufMotionParams.Add(new BufMotionParam
                {
                    _Type = BufMotionType.buf_IO,
                    _nAxisNo = IOMgr.GetInstace().GetOutputDic()[IoName]._AxisIndex,
                    _nCardNo = IOMgr.GetInstace().GetOutputDic()[IoName]._CardIndex,
                    _nIoNo = IOMgr.GetInstace().GetOutputDic()[IoName]._IoIndex,
                    _strIoName = IoName,
                    _nAxisNum = nNum,
                    _IoVal= bIoState,
                });
                return true;
            }
            else
                return false;
        }
        ///群组 添加Delay
        public bool AddBufDelay(string GroupName, int Time)
        {
            MoveGroup moveGroup = new MoveGroup();
            if (m_dicGroup.TryGetValue(GroupName, out moveGroup))
            {
                if (moveGroup._listBufMotionParams == null)
                    return false;
                moveGroup._listBufMotionParams.Add(new BufMotionParam
                {
                    _Type = BufMotionType.buf_Delay,
                    _nDelayMs = Time,
                });
                return true;
            }
            else
                return false;
        }
        public bool AddBufEnd(string GroupName)
        {
            MoveGroup moveGroup = new MoveGroup();
            if (m_dicGroup.TryGetValue(GroupName, out moveGroup))
            {
                if (moveGroup._listBufMotionParams == null)
                    return false;
                int nNum = moveGroup._nAxisArr == null ? 0 : moveGroup._nAxisArr.Length;
                moveGroup._listBufMotionParams.Add(new BufMotionParam
                {
                    _Type = BufMotionType.buf_end,
                    _nAxisNum = nNum,
                });
                return true;
            }
            else
                return false;
        }

        public bool ClearBufMove(string strGroupName)
        {
            MoveGroup moveGroup = new MoveGroup();
            if (m_dicGroup.TryGetValue(strGroupName, out moveGroup))
            {
                moveGroup._listBufMotionParams.Clear();
                return moveGroup._pCard.ClearBufMove(moveGroup._pGroup);
            }
            return false;
        }

        public bool BufTrans(string strGroupName)
        {
            bool bRtn = true;
            MoveGroup moveGroup = new MoveGroup();
            if (m_dicGroup.TryGetValue(strGroupName, out moveGroup))
            {
                if (moveGroup._listBufMotionParams == null)
                    return false;
                moveGroup._pCard.ClearBufMove(moveGroup._pGroup);
                int nAxisIndexInGroup = -1;
                foreach (var tem in moveGroup._listBufMotionParams)
                {
                    switch (tem._Type)
                    {
                        case BufMotionType.buf_IO:
                            for (int i = 0; i < moveGroup._nAxisArr.Length; i++)
                            {
                                if (moveGroup._nAxisArr[i] == tem._nAxisNo)
                                {
                                    nAxisIndexInGroup = i;
                                    break;
                                }
                            }
                            bRtn = bRtn & moveGroup._pCard.AddBufIo(moveGroup._pGroup, tem._strIoName, tem._IoVal, nAxisIndexInGroup);
                            break;
                        case BufMotionType.buf_Delay:
                            bRtn = bRtn & moveGroup._pCard.AddBufDelay(moveGroup._pGroup, tem._nDelayMs);
                            break;
                        case BufMotionType.buf_Arc2dAbsAngleCCW:
                            double[] point1pluseAngel1 = null;
                            double acc1 = 0;
                            if (tem._point1 != null)
                            {
                                point1pluseAngel1 = new double[tem._point1.Length];
                                for (int i = 0; i < tem._point1.Length; i++)
                                {
                                    point1pluseAngel1[i] = GetAxisPulsesPerMM(moveGroup._nSysAxisArr[i]) * tem._point1[i];
                                    acc1 = GetAxisPulsesPerMM(moveGroup._nSysAxisArr[i]) * GetAxisSpeed(moveGroup._nSysAxisArr[i], tem._VelHigh);
                                }

                            }
                         
                            bRtn = bRtn & moveGroup._pCard.AddBufMove(moveGroup._pGroup, tem._Type, tem._mode, tem._nAxisNum, acc1, acc1, point1pluseAngel1, tem._point2);


                            break;
                        case BufMotionType.buf_Arc2dAbsAngleCW:
                            double rateAngel = 0;
                            double acc2 = 0;
                            double[] point1pluseAngel2 = null;
                            if (tem._point1 != null)
                            {
                                point1pluseAngel2 = new double[tem._point1.Length];
                                for (int i = 0; i < tem._point1.Length; i++)
                                {
                                    point1pluseAngel2[i] = GetAxisPulsesPerMM(moveGroup._nSysAxisArr[i]) * tem._point1[i];
                                    acc2 = GetAxisPulsesPerMM(moveGroup._nSysAxisArr[i])* GetAxisSpeed(moveGroup._nSysAxisArr[i], tem._VelHigh);
                                }

                            }
                            bRtn = bRtn & moveGroup._pCard.AddBufMove(moveGroup._pGroup, tem._Type, tem._mode,
                                tem._nAxisNum, acc2, acc2,
                                point1pluseAngel2, tem._point2);
                            break;
                        default:
                            double rate = 0;
                            double[] point1pluse = null;
                            double[] point2pluse = null;
                            double acc3 = 0;
                            if (tem._point1 != null)
                            {
                                point1pluse = new double[tem._point1.Length];
                                for ( int i=0;i< tem._point1.Length;i++)
                                {
                                    point1pluse[i] = GetAxisPulsesPerMM(moveGroup._nSysAxisArr[i])* tem._point1[i];
                                    acc3 = GetAxisPulsesPerMM(moveGroup._nSysAxisArr[i])*GetAxisSpeed(moveGroup._nSysAxisArr[i], tem._VelHigh);
                                }

                            }
                            if (tem._point2 != null)
                            {
                                 point2pluse = new double[tem._point2.Length];
                                for (int i = 0; i < tem._point2.Length; i++)
                                {
                                    point2pluse[i] = GetAxisPulsesPerMM(moveGroup._nSysAxisArr[i]) * tem._point2[i];
                                }

                            }
                            bRtn = bRtn & moveGroup._pCard.AddBufMove(moveGroup._pGroup, tem._Type, tem._mode,
                                tem._nAxisNum, acc3, acc3,
                                point1pluse, point2pluse);
                            break;
                    }

                }
                //bRtn = bRtn & moveGroup._pCard.StartBufMove(moveGroup._pGroup);
                return bRtn;
            }
            else
                return false;


        }

        public bool BufStart(string strGroupName)
        {
            bool bRtn = true;
            MoveGroup moveGroup = new MoveGroup();
            if (m_dicGroup.TryGetValue(strGroupName, out moveGroup))
            {
                return moveGroup._pCard.StartBufMove(moveGroup._pGroup);
            }
            return false;

        }
        public bool SetBufMoveParam(string strGroupName, double velhigh, double vellow, double acc, double dec)
        {
            bool bRtn = true;
            MoveGroup moveGroup = new MoveGroup();
            if (m_dicGroup.TryGetValue(strGroupName, out moveGroup))
            {
                return moveGroup._pCard.SetBufMoveParam(moveGroup._pGroup,  velhigh,  vellow,  acc,  dec);
            }
            return false;

        }
       

    }

}
