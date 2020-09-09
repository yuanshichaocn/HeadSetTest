using MotionIoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotionIoLib
{    
    public interface RobotInterface
    {
        /// <summary>
        /// 远程控制发送命令函数
        /// </summary>
        /// <param name="Command">命令字符串</param>
        /// <returns>命令回复值</returns>
        string RobotSendCmd(string Command);
        /// <summary>
        /// 登录EPSON机械手的远程以太网通讯
        /// </summary>
        /// <param name="LoginRobotPassword"></param>
        /// <returns></returns>
        bool Login(string LoginRobotPassword);
        /// <summary>
        /// 退出EPSON 机械手的远程以太网通讯
        /// </summary>
        /// <returns></returns>
        bool Logout();
        /// <summary>
        /// 启动EPSON机械手指定编号的任务
        /// </summary>
        /// <param name="NumberOfMission">任务编号 0~7</param>
        /// <returns></returns>
        bool StartMission(int NumberOfMission);
        /// <summary>
        /// 处理返回结果
        /// </summary>
        /// <param name="ResponseString">返回的指令</param>
        /// <returns></returns>
        string ProcessResponse(string ResponseString);
        /// <summary>
        /// 获取点坐标值
        /// </summary>
        /// <param name="PointName">点的编号 P0~P999</param>
        /// <param name="PointData"></param>
        /// <returns></returns>
        bool GetPointPos(string PointName, ref RobotPoint PointData);
        /// <summary>
        /// 设置某个点的坐标值
        /// </summary>
        /// <param name="PointName">点的编号  P0~P999</param>
        /// <param name="NewPointData">点的坐标值</param>
        /// <returns></returns>
        bool SetPointPos(string PointName, RobotPoint NewPointData);
        /// <summary>
        /// 执行JUMP命令道某一个点
        /// </summary>
        /// <param name="PointName">点的编号</param>
        /// <returns></returns>
        bool Jump(string PointName);
        /// <summary>
        /// 保存点位置
        /// </summary>
        /// <returns></returns>
        bool SavePointPos();
        /// <summary>
        /// 保存点位文件到指定文件夹
        /// </summary>
        /// <returns></returns>
        bool SavePointPosWithSaveDialog();
        /// <summary>
        /// 
        /// </summary>
        bool ConnectEPSON();
        /// <summary>
        /// 获取EPSON控制器中指定变量的值
        /// </summary>
        /// <param name="VariableName">变量名</param>
        /// <param name="Value">变量名的值</param>
        /// <returns></returns>
        bool GetVariable(ref string VariableName, ref string[] Value);
        /// <summary>
        /// 设置EPSON控制器的指定变量的值
        /// </summary>
        /// <param name="VariableName">变量名字</param>
        /// <param name="Value">变量值</param>
        /// <returns></returns>
        bool SetVariable(string VariableName, string Value, RobotVariable VariableType);

        /// <summary>
        /// 获取工具坐标系设置
        /// </summary>
        /// <param name="NumberOfTool">工具坐标编号</param>
        /// <returns></returns>
        bool GetToolSetting(int NumberOfTool);
        /// <summary>
        /// 设置工具坐标系
        /// </summary>
        /// <param name="NumberOfTool"></param>
        /// <param name="X">工具坐标系X值</param>
        /// <param name="Y">同上</param>
        /// <param name="Z">同上</param>
        /// <param name="U">同上</param>
        /// <returns></returns>
        bool SetTool(int NumberOfTool, double X, double Y, double Z, double U);
        /// <summary>
        /// 终止命令的执行
        /// </summary>
        /// <returns></returns>
        bool Abort();
        /// <summary>
        /// 设置功率模式
        /// </summary>
        /// <param name="PowerMode">功率的模式</param>
        /// <returns></returns>
        bool SetPowerMode(RobotPower PowerMode);
        /// <summary>
        /// 解除指定关节的SFREE，并且重新开启电机励磁使能
        /// </summary>
        /// <param name="TargetAxis">指定轴号 1-6</param>
        /// <returns></returns>
        bool SLock(int TargetAxis);
        /// <summary>
        /// 解除所有的指定关节的SFREE，并且重新开启电机的励磁使能
        /// </summary>
        /// <param name="AxisQty">轴数量  4-6</param>
        /// <returns></returns>
        bool SLockAll(int AxisQty);
        /// <summary>
        /// 关闭指定关节的励磁使能
        /// </summary>
        /// <param name="TargetAxis">轴号</param>
        /// <returns></returns>
        bool SFree(int TargetAxis);
        /// <summary>
        /// 关闭所有的关节的励磁使能
        /// </summary>
        /// <param name="AxisQty">所有轴的数量 4-6</param>
        /// <returns></returns>
        bool SFreeAll(int AxisQty);
        /// <summary>
        /// 获取机械手的状态
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        bool GetRobotStatus(ref string Status);
        /// <summary>
        /// 清楚紧急停止和错误
        /// </summary>
        /// <returns></returns>
        bool ResetRobot();
        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="TargetSpeed"></param>
        /// <returns></returns>
        bool SetSpeed(int TargetSpeed);
        /// <summary>
        /// 获取当前速度
        /// </summary>
        /// <returns></returns>
        int GetSpeed();
        /// <summary>
        /// 获取机械手返回状态的意义
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <returns></returns>
        RobotStatusBits NewProcessStatusCode(string StatusCode);
        /// <summary>
        /// 释放资源
        /// </summary>
        void Dispose();
        /// <summary>
        /// 选择机械手
        /// </summary>
        /// <param name="NumberOfRobot"></param>
        /// <returns></returns>
        bool SetCurrentRobot(int NumberOfRobot);
        /// <summary>
        /// 获取机械手编号
        /// </summary>
        /// <returns></returns>
        int GetCurrentRobot();

        bool SetACCELSpeed(ushort TargetAccelSpeed, ushort TargetDecelSpeed);
        /// <summary>
        /// 获取加速度
        /// </summary>
        /// <returns></returns>
        RobotAccel GetAccelSpeed();

        /// <summary>
        /// 获取当前功率模式
        /// </summary>
        /// <returns></returns>
        RobotPower GetPowerStatus();
        /// <summary>
        /// 设置原点坐标
        /// </summary>
        /// <param name="HomePoint"></param>
        /// <returns></returns>
        bool SetHome(RobotPoint HomePoint);
        /// <summary>
        /// 选择本地坐标系
        /// </summary>
        /// <returns></returns>
        bool Loacl(int LocalNo, RobotPoint LocalPoint);

        /// <summary>
        /// 定义基础坐标系
        /// </summary>
        /// <param name="BasePoint"></param>
        /// <returns></returns>
        bool Base(RobotPoint BasePoint);
        /// <summary>
        /// 松开刹车
        /// </summary>
        /// <param name="TargetAxis"></param>
        /// <returns></returns>
        bool StopAxisOff(int TargetAxis);
    }
}
