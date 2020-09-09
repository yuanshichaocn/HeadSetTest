
using System;
using System.Collections.Generic;
using System.Linq;
#region "以太网"
using Communicate;
#endregion
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Threading;
namespace MotionIoLib
{

    public enum ERemotCMD
    {
        /// <summary>
        ///登陆
        /// </summary>
        Login,
        /// <summary>
        /// 登出
        /// </summary>
        Logout,
        /// <summary>
        /// 执行指定编号的函数
        /// </summary>
        Start,
        /// <summary>
        /// 停止所有的任务和命令
        /// </summary>
        Stop,
        /// <summary>
        /// 暂停所有任务
        /// </summary>
        Pause,
        /// <summary>
        /// 继续暂停了的任务
        /// </summary>
        Continue,
        /// <summary>
        /// 清除紧急停止和错误
        /// </summary>
        Reset,
        /// <summary>
        /// 打开机器人电机
        /// </summary>
        SetMotorsOn,
        /// <summary>
        /// 关闭机器人电机
        /// </summary>
        SetMotorsOff,
        /// <summary>
        /// 选择机器人
        /// </summary>
        SetCurRobot,
        /// <summary>
        /// 过去当前的机器人编号
        /// </summary>
        GetCurRobot,
        /// <summary>
        /// 将机器人手臂移动到由用户定义的起始点位置上
        /// </summary>
        Home,
        /// <summary>
        /// 获取指定的I/O位
        /// </summary>
        GetIO,
        /// <summary>
        /// 设置I/O指定位
        /// </summary>
        SetIO,
        /// <summary>
        /// 获得指定的I/O端口（8位
        /// </summary>
        GetIOByte,
        /// <summary>
        /// 设置I/O指定端口（8位）
        /// </summary>
        SetIOByte,
        /// <summary>
        /// 获得指定的I/O字端口（16位）
        /// </summary>
        GetIOWord,
        /// <summary>
        /// 设置I/O指定字端口（8位）
        /// </summary>
        SetIOWord,
        /// <summary>
        /// 获取指定的内存I/O位
        /// </summary>
        GetMemIO,
        /// <summary>
        /// 设置指定的内存I/O位
        /// </summary>
        SetMemIO,
        /// <summary>
        /// 获取指定内存I/O端口
        /// </summary>
        GetMemIOByte,
        /// <summary>
        /// 设置指定的内存I/O端口（8位）
        /// </summary>
        SetMemIOByte,
        /// <summary>
        /// 获取指定的内存I/O字端口（16位）
        /// </summary>
        GetMemIOWord,
        /// <summary>
        /// 设置指定的内存I/O字端口（16位）
        /// </summary>
        SetMemIOWord,
        /// <summary>
        /// 获取备份（全局保留）参数的值
        /// </summary>
        GetVariable,
        /// <summary>
        /// 获取备份（全局保留）数组参数的值
        /// </summary>
        SetVariable,
        /// <summary>
        /// 获取控制器的状态
        /// </summary>
        GetStatus,
        /// <summary>
        /// 执行命令
        /// </summary>
        Execute,
        /// <summary>
        /// 中止命令的执行
        /// </summary>
        Abort,
        /// <summary>
        /// 空命令
        /// </summary>
        NULL,
    }
    public class ResponseStatus
    {
        /// <summary>
        ///已响应登陆命令
        /// </summary>
        public bool? Login { get; set; }
        /// <summary>
        /// 已响应登出命令
        /// </summary>
        public bool? Logout { get; set; }
        /// <summary>
        /// 已响应Start命令
        /// </summary>
        public bool? Start { get; set; }
        /// <summary>
        /// 已响应Stop命令
        /// </summary>
        public bool? Stop { get; set; }
        /// <summary>
        /// 已响应Paus命令
        /// </summary>
        public bool? Pause { get; set; }
        /// <summary>
        /// 已响应Continue命令
        /// </summary>
        public bool? Continue { get; set; }
        /// <summary>
        /// 已响应Reset命令
        /// </summary>
        public bool? Reset { get; set; }
        /// <summary>
        /// 已响应SetMotorsO命令
        /// </summary>
        public bool? SetMotorsOn { get; set; }
        /// <summary>
        /// 已响应SetMotorsOf命令
        /// </summary>
        public bool? SetMotorsOff { get; set; }
        /// <summary>
        /// 已响应SetCurRobo命令
        /// </summary>
        public bool? SetCurRobot { get; set; }
        /// <summary>
        /// 已响应GetCurRobot命令
        /// </summary>
        public bool? GetCurRobot { get; set; }
        /// <summary>
        /// 已响应Home命令
        /// </summary>
        public bool? Home { get; set; }
        /// <summary>
        /// 已响应GetIO命令
        /// </summary>
        public bool? GetIO { get; set; }
        /// <summary>
        /// 已响应SetIO命令
        /// </summary>
        public bool? SetIO { get; set; }
        /// <summary>
        /// 已响应GetIOByte命令
        /// </summary>
        public bool? GetIOByte { get; set; }
        /// <summary>
        ///  已响应SetIOByte命令
        /// </summary>
        public bool? SetIOByte { get; set; }
        /// <summary>
        /// 已响应GetIOWord命令
        /// </summary>
        public bool? GetIOWord { get; set; }
        /// <summary>
        ///  已响应SetIOWord命令
        /// </summary>
        public bool? SetIOWord { get; set; }
        /// <summary>
        /// 已响应GetMemIO命令
        /// </summary>
        public bool? GetMemIO { get; set; }
        /// <summary>
        /// 已响应SetMemIO命令
        /// </summary>
        public bool? SetMemIO { get; set; }
        /// <summary>
        /// 已响应GetMemIOByte命令
        /// </summary>
        public bool? GetMemIOByte { get; set; }
        /// <summary>
        /// 已响应SetMemIOByte命令
        /// </summary>
        public bool? SetMemIOByte { get; set; }
        /// <summary>
        /// 已响应GetMemIOWord命令
        /// </summary>
        public bool? GetMemIOWord { get; set; }
        /// <summary>
        /// 已响应SetMemIOWord命令
        /// </summary>
        public bool? SetMemIOWord { get; set; }
        /// <summary>
        /// 已响应GetVariable命令
        /// </summary>
        public bool? GetVariable { get; set; }
        /// <summary>
        ///已响应SetVariable命令
        /// </summary>
        public bool? SetVariable { get; set; }
        /// <summary>
        /// 已响应GetStatus命令
        /// </summary>
        public bool? GetStatus { get; set; }
        /// <summary>
        /// 已响应Execute命令
        /// </summary>
        public bool? Execute { get; set; }
        /// <summary>
        /// 已响应Abort命令
        /// </summary>
        public bool? Abort { get; set; }
    }

    public class ControlInfo
    {
        /// <summary>
        /// 测试
        /// </summary>
        public bool Test { get; set; }
        /// <summary>
        /// 示教
        /// </summary>
        public bool Teach { get; set; }
        /// <summary>
        /// 自动
        /// </summary>
        public bool Auto { get; set; }
        /// <summary>
        /// 报警
        /// </summary>
        public bool Waring { get; set; }
        /// <summary>
        /// 严重错误
        /// </summary>
        public bool SError { get; set; }
        /// <summary>
        /// 安全保护
        /// </summary>
        public bool Safeguard { get; set; }
        /// <summary>
        /// 急停
        /// </summary>
        public bool EStop { get; set; }
        /// <summary>
        /// 错误
        /// </summary>
        public bool Error { get; set; }
        /// <summary>
        /// 暂停
        /// </summary>
        public bool Paused { get; set; }
        /// <summary>
        /// 运行中
        /// </summary>
        public bool Running { get; set; }
        /// <summary>
        /// 准备好
        /// </summary>
        public bool Ready { get; set; }
        /// <summary>
        /// 获取状态时 错误警告编码
        /// </summary>
        public int ErrorCode { get { return erroeCode; } set { erroeCode = value; } }
        private int erroeCode = 0;
    }
    public class RobotInfo
    {
        /// <summary>
        /// 机器人电机使能 true=On,false=off
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 功率 true=high fales=low
        /// </summary>
        public bool Power { get; set; }
        /// <summary>
        /// Halt状态 true=Halt状态 false=非Halt状态
        /// </summary>
        public bool Halt { get; set; }
        /// <summary>
        /// 机器人处于原点位置
        /// </summary>
        public bool Home { get; set; }
        /// <summary>
        /// 运动中
        /// </summary>
        public bool Running { get; set; }

        public int nBaudRate;
        public int nDataBit;
        public string strPartiy;
        public string strStopBit;
        public string strFlowCtrl;
        public int nBufferSzie;
        public int InterNetIndex;
        public string InterNetName;
        public int timeout;
        public string RobotSuffic;
        public string IpAddress;
        public ushort Port;
        public string PassWard;
    }
    /// <summary>
    /// 机械手点位数据类型
    /// </summary>
    public class PositionInfo
    {
        /// <summary>
        /// 坐标X
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 坐标Y
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 坐标Z
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// 角度U(deg)
        /// </summary>
        public double U { get; set; }

        /// <summary>
        /// 角度V(deg)
        /// </summary>
        public double V { get; set; }

        /// <summary>
        /// 角度W(deg)
        /// </summary>
        public double W { get; set; }

        /// <summary>
        /// 手势 true=right false=left
        /// </summary>
        public bool Hand { get; set; }

        /// <summary>
        /// 本地坐标系编号
        /// </summary>
        public int LocalNo { get; set; }

        /// <summary>
        /// P1（脉冲）
        /// </summary>
        public int P1 { get; set; }

        /// <summary>
        /// J2（脉冲）
        /// </summary>
        public int P2 { get; set; }

        /// <summary>
        /// P3（脉冲）
        /// </summary>
        public int P3 { get; set; }

        /// <summary>
        /// P4（脉冲）
        /// </summary>
        public int P4 { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Tage { get; set; }

        /// <summary>
        /// 点ID
        /// </summary>
        public string PID { get; set; }
    }
    public class Status
    {
        /// <summary>
        /// 当前机器人编号
        /// </summary>
        public int CurrentRobot { get; set; }

        /// <summary>
        /// 控制器状态
        /// </summary>
        public ControlInfo ControlInfo { get { return controlInfo; } set { controlInfo = value; } }
        private ControlInfo controlInfo = new ControlInfo();

        /// <summary>
        /// 机器人状态
        /// </summary>
        public RobotInfo RobotInfo { get { return robotInfo; } set { robotInfo = value; } }
        private RobotInfo robotInfo = new RobotInfo();

        /// <summary>
        /// 当前位置信息
        /// </summary>
        public PositionInfo PosInfo { get { return posInfo; } set { posInfo = value; } }
        private PositionInfo posInfo = new PositionInfo();

        /// <summary>
        /// 点数据
        /// </summary>
        public List<PositionInfo> Points { get { return points; } set { points = value; } }
        private List<PositionInfo> points = new List<PositionInfo>();

        /// <summary>
        /// 起始点位置信息
        /// </summary>
        public int[] HomeSet { get; set; }

        /// <summary>
        /// 回起始位置 各轴动作次序
        /// </summary>
        public int[] Hordr { get; set; }
        /// <summary>
        /// IO状态 第0字节
        /// </summary>
        public Int16 IoIn0 { get; set; }
        /// <summary>
        /// IO状态 第1字节
        /// </summary>
        public Int16 IoIn1 { get; set; }
    }

    public class Spel
    {
        #region 基础设置
        /// <summary>
        /// 功率模式设为High
        /// </summary>
        public const string PowerHigh = "Power High";
        /// <summary>
        /// 功率模式设为Low
        /// </summary>
        public const string PowerLow = "Power Low";
        #endregion

        #region 读取状态
        /// <summary>
        /// 获取机器人状态
        /// </summary>
        public const string PrintRobotInfo_0 = "Print RobotInfo(0)";
        /// <summary>
        /// 获取当前位置
        /// </summary>
        public const string RealPos = "Print RealPos";
        /// <summary>
        /// 1关节的脉冲值
        /// </summary>
        public const string PrintPls_1 = "Print Pls(1)";
        /// <summary>
        /// 2关节的脉冲值
        /// </summary>
        public const string PrintPls_2 = "Print Pls(2)";
        /// <summary>
        /// 3关节的脉冲值
        /// </summary>
        public const string PrintPls_3 = "Print Pls(3)";
        /// <summary>
        /// 4关节的脉冲值
        /// </summary>
        public const string PrintPls_4 = "Print Pls(4)";
        #endregion

        #region 原点相关
        /// <summary>
        /// 让机器人回到起始点
        /// </summary>
        public const string Home = "Home";
        /// <summary>
        /// 设置起始点位置
        /// </summary>
        public const string HomeSet = "HomeSet";
        /// <summary>
        /// 设置起始次序
        /// </summary>
        public const string Hordr = "Hordr";
        /// <summary>
        /// 重置
        /// </summary>
        public const string Reset = "Reset";
        /// <summary>
        /// 获取机器人原点状态
        /// </summary>
        public const string PrintRobotInfo_2 = "Print RobotInfo(2)";
        #endregion

        #region 动作相关
        /// <summary>
        /// 点到点运动Go (PTP)
        /// </summary>
        public const string Go = "Go";
        /// <summary>
        ///  点到点运动TGo (PTP)
        /// </summary>
        public const string TGo = "TGo";
        /// <summary>
        ///  跳到某一点运动Jump (PTP)
        /// </summary>
        public const string Jump = "Jump";
        /// <summary>
        /// 直线运动Move
        /// </summary>
        public const string Move = "Move";
        /// <summary>
        /// 直线运动TMove
        /// </summary>
        public const string TMove = "TMove";
        /// <summary>
        /// 等待机器人进行减速停止
        /// </summary>
        public const string WaitPos = "WaitPos";
        /// <summary>
        /// 设置速度(CP)
        /// </summary>
        public const string Speeds = "Speeds";
        /// <summary>
        /// 设置速度(PTP)
        /// </summary>
        public const string Accel = "Accel";
        /// <summary>
        /// 设置速度(CP)
        /// </summary>
        public const string Accels = "Accels";
        /// <summary>
        /// 设置速度(PTP)
        /// </summary>
        public const string Speed = "Speed";
        #endregion

        #region 点位相关
        /// <summary>
        /// 保存点文件
        /// </summary>
        public const string SavePoints = "SavePoints";
        /// <summary>
        /// 读取点文件
        /// </summary>
        public const string LoadPoints = "LoadPoints";
        /// <summary>
        /// 导入至项
        /// </summary>
        public const string ImportPoints = "ImportPoints";
        /// <summary>
        /// 定义点
        /// </summary>
        public const string P = "P";
        /// <summary>
        /// 定义点标签
        /// </summary>
        public const string PLabel = "PLabel";
        /// <summary>
        /// 读取点数据集
        /// </summary>
        public const string Plist = "Plist";
        #endregion

        #region  阵列
        /// <summary>
        /// 构建阵列
        /// </summary>
        public const string Pallet = "Pallet";
        #endregion

    }
}