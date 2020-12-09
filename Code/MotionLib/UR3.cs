using MotionIoLib;
using System;

namespace MotionLib
{
    public class RobotUR3 : RobotBase
    {
        public RobotUR3(string strName, string strPwd, string strEth, int nAxisNoMin, int nAxisNoMax, RobotCommunicateType robotCommunicateType = RobotCommunicateType.Eth) : base(strName, strPwd, strEth, nAxisNoMin, nAxisNoMax, robotCommunicateType)
        {
        }

        public override bool Abort()
        {
            throw new NotImplementedException();
        }

        public override bool AbsMove(RobotPoint EpsonPoint)
        {
            throw new NotImplementedException();
        }

        public override bool AbsMove(string EpsonPoint)
        {
            throw new NotImplementedException();
        }

        public override bool Base(RobotPoint BasePoint)
        {
            throw new NotImplementedException();
        }

        public override bool Close()
        {
            throw new NotImplementedException();
        }

        public override bool ConnectEPSON()
        {
            throw new NotImplementedException();
        }

        public override RobotAccel GetAccelSpeed()
        {
            throw new NotImplementedException();
        }

        public override int GetAxisActPos(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override bool GetAxisActPos(ref PositionInfo EpsonPos)
        {
            throw new NotImplementedException();
        }

        public override int GetAxisCmdPos(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override int GetAxisPos(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override bool GetCurrentPos(ref RobotPoint PointData)
        {
            throw new NotImplementedException();
        }

        public override int GetCurrentRobot()
        {
            throw new NotImplementedException();
        }

        public override bool GetInputBit(int TargetInputBit, ref bool IsOn)
        {
            throw new NotImplementedException();
        }

        public override long GetMotionIoState(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override bool GetPointPos(string PointName, ref RobotPoint PointData)
        {
            throw new NotImplementedException();
        }

        public override RobotPower GetPowerStatus()
        {
            throw new NotImplementedException();
        }

        public override bool GetRobotStatus(ref string Status, bool bGetFromBuff = false)
        {
            throw new NotImplementedException();
        }

        public override bool GetServoState(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override int GetSpeed()
        {
            throw new NotImplementedException();
        }

        public override bool GetToolSetting(int NumberOfTool)
        {
            throw new NotImplementedException();
        }

        public override bool GetVariable(ref string VariableName, ref string[] Value)
        {
            throw new NotImplementedException();
        }

        public override bool Home(int nAxisNo, int nParam)
        {
            throw new NotImplementedException();
        }

        public override int IsAxisNormalStop(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override bool IsHomeNormalStop(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override bool JogMove(int nAxisNo, bool bPositive, int bStart, int nSpeed)
        {
            throw new NotImplementedException();
        }

        public override bool Jump(string PointName)
        {
            throw new NotImplementedException();
        }

        public override bool Loacl(int LocalNo, RobotPoint LocalPoint)
        {
            throw new NotImplementedException();
        }

        public override bool Login(string LoginRobotPassword)
        {
            throw new NotImplementedException();
        }

        public override bool Logout()
        {
            throw new NotImplementedException();
        }

        public override RobotStatusBits NewProcessStatusCode(string StatusCode)
        {
            throw new NotImplementedException();
        }

        public override bool Open()
        {
            throw new NotImplementedException();
        }

        public override string ProcessResponse(string ResponseString)
        {
            throw new NotImplementedException();
        }

        public override bool ReasetAxis(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override bool RelativeMove(int nAxisNo, double nPos, int nSpeed)
        {
            throw new NotImplementedException();
        }

        public override bool RelativeMove(int nAxisNo, double nPos, int nSpeed, CoordinateSys EpsonCoord)
        {
            throw new NotImplementedException();
        }

        public override bool ResetRobot()
        {
            throw new NotImplementedException();
        }

        public override string RobotSendCmd(string Command)
        {
            throw new NotImplementedException();
        }

        public override bool SavePointPos()
        {
            throw new NotImplementedException();
        }

        public override bool SavePointPosWithSaveDialog()
        {
            throw new NotImplementedException();
        }

        public override bool ServoOff()
        {
            throw new NotImplementedException();
        }

        public override bool ServoOn()
        {
            throw new NotImplementedException();
        }

        public override bool SetACCELSpeed(ushort TargetAccelSpeed, ushort TargetDecelSpeed)
        {
            throw new NotImplementedException();
        }

        public override bool SetCurrentRobot(int NumberOfRobot)
        {
            throw new NotImplementedException();
        }

        public override bool SetHome(RobotPoint HomePoint)
        {
            throw new NotImplementedException();
        }

        public override bool SetOutputBit(int TargetOutputBit, bool TurnOn)
        {
            throw new NotImplementedException();
        }

        public override bool SetPointPos(string PointName, string strLabel, PositionInfo NewPointData)
        {
            throw new NotImplementedException();
        }

        public override bool SetPowerMode(RobotPower PowerMode)
        {
            throw new NotImplementedException();
        }

        public override bool SetSpeed(int TargetSpeed)
        {
            throw new NotImplementedException();
        }

        public override bool SetTool(int NumberOfTool, double X, double Y, double Z, double U)
        {
            throw new NotImplementedException();
        }

        public override bool SetVariable(string VariableName, string Value, RobotVariable VariableType)
        {
            throw new NotImplementedException();
        }

        public override bool SFree(int TargetAxis)
        {
            throw new NotImplementedException();
        }

        public override bool SFreeAll(int AxisQty)
        {
            throw new NotImplementedException();
        }

        public override bool SLock(int TargetAxis)
        {
            throw new NotImplementedException();
        }

        public override bool SLockAll(int AxisQty)
        {
            throw new NotImplementedException();
        }

        public override bool StartMission(int NumberOfMission)
        {
            throw new NotImplementedException();
        }

        public override bool StopAxis(int nAxisNo)
        {
            throw new NotImplementedException();
        }

        public override bool StopAxisOff(int TargetAxis)
        {
            throw new NotImplementedException();
        }
    }
}