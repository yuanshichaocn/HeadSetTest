using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools;
using MotionIoLib;
using Communicate;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using UserCtrl;
using EpsonRobot;
using BaseDll;
using UserData;
using VisionProcess;
using HalconDotNet;
using System.IO;

using CameraLib;
using OtherDevice;
using XYZDispensVision;
using MachineSafe;


namespace StationDemo
{

    public static class RoboteExt
    {
        static string RobotPauseIo = "";
        static string RobotResumeIo = "";
        public static WaranResult JumpInPos(this ScaraRobot scaraRobot, Coordinate coordinate, HandDirection direction, double limz, bool bCheckHandleSys = false, bool bmauanl = false, int nTimeout = 20000)
        {
            try
            {
                WaranResult waran = WaranResult.Failture;
                bool bcmd = ScaraRobot.GetInstance().Jump(coordinate, direction, limz);
                DoWhile doWhile = new DoWhile((nTimeed, doWhile2, bmanual, objs) =>
                {
                    bool bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < 0.03;
                    bool bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < 0.03;
                    bool bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < 0.03;
                    bool bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                    bool bInPos = bInPosX && bInPosY && bInPosZ && bInPosU;
                    if (!bmanual)
                    {
                        if (GlobalVariable.g_StationState == StationState.StationStatePause)
                        {
                            ScaraRobot.GetInstance().SetStopActionFlag();
                            return WaranResult.CheckAgain;
                        }
                        else if (GlobalVariable.g_StationState == StationState.StationStateRun)
                        {
                            ScaraRobot.GetInstance().ReasetStopActionFlag();
                            if (ScaraRobot.GetInstance().InPos && bInPos)
                                return WaranResult.Run;
                            else if (ScaraRobot.GetInstance().InPos && !bInPos)
                            {
                                ScaraRobot.GetInstance().Jump(coordinate, direction, limz);
                                return WaranResult.CheckAgain;
                            }

                        }
                    }

                    if (nTimeed > nTimeout)
                    {
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().ReasetStopActionFlag();
                        return WaranResult.TimeOut;
                    }


                    if (ScaraRobot.GetInstance().InPos && bInPos)
                        return WaranResult.Run;
                    else
                        return WaranResult.CheckAgain;
                }
                , 30000);
                return doWhile.doSomething2(null, doWhile, bmauanl, null);
            }
            catch (Exception ex)
            {
                ScaraRobot.GetInstance().SetStopActionFlag();
                ScaraRobot.GetInstance().SetStopActionFlag();
                ScaraRobot.GetInstance().ReasetStopActionFlag();
                throw ex;
            }


        }
        public static WaranResult GoInPos(this ScaraRobot scaraRobot, Coordinate coordinate, HandDirection direction, bool bCheckHandleSys = false, bool bmauanl = false, int nTimeout = 20000)
        {
            try
            {
                WaranResult waran = WaranResult.Failture;
                bool bcmd = ScaraRobot.GetInstance().Go(coordinate, direction);
                DoWhile doWhile = new DoWhile((nTimeed, doWhile2, bmanual, objs) =>
                {
                    bool bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < 0.03;
                    bool bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < 0.03;
                    bool bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < 0.03;
                    bool bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                    bool bInPos = bInPosX && bInPosY && bInPosZ && bInPosU;
                    if (!bmanual)
                    {
                        if (GlobalVariable.g_StationState == StationState.StationStatePause)
                        {
                            ScaraRobot.GetInstance().SetStopActionFlag();
                            return WaranResult.CheckAgain;
                        }
                        else if (GlobalVariable.g_StationState == StationState.StationStateRun)
                        {
                            ScaraRobot.GetInstance().ReasetStopActionFlag();
                            if (ScaraRobot.GetInstance().InPos && bInPos)
                                return WaranResult.Run;
                            else if (ScaraRobot.GetInstance().InPos && !bInPos)
                            {
                                ScaraRobot.GetInstance().Go(coordinate, direction);
                                return WaranResult.CheckAgain;
                            }

                        }
                    }

                    if (nTimeed > nTimeout)
                    {
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().ReasetStopActionFlag();
                        return WaranResult.TimeOut;
                    }


                    if (ScaraRobot.GetInstance().InPos && bInPos)
                        return WaranResult.Run;
                    else
                        return WaranResult.CheckAgain;
                }
                , 30000);
                return doWhile.doSomething2(null, doWhile, bmauanl, null);
            }
            catch (Exception ex)
            {
                ScaraRobot.GetInstance().SetStopActionFlag();
                ScaraRobot.GetInstance().SetStopActionFlag();
                ScaraRobot.GetInstance().ReasetStopActionFlag();
                throw ex;
            }
        }
        public static WaranResult GoInPosZ(this ScaraRobot scaraRobot, double Zpos, bool bCheckHandleSys = true, bool bmauanl = false, int nTimeout = 20000)
        {
            try
            {
                Coordinate coordinate = ScaraRobot.GetInstance().CurrentPosition;
                HandDirection direction = ScaraRobot.GetInstance().CurrentHandDirection;
                coordinate.Z = Zpos;
                WaranResult waran = WaranResult.Failture;
                bool bcmd = ScaraRobot.GetInstance().Go(coordinate, direction);
                DoWhile doWhile = new DoWhile((nTimeed, doWhile2, bmanual, objs) =>
                {
                    bool bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < 0.03;
                    bool bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < 0.03;
                    bool bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < 0.03;
                    bool bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                    bool bInPos = bInPosX && bInPosY && bInPosZ && bInPosU;
                    if (!bmanual)
                    {
                        if (GlobalVariable.g_StationState == StationState.StationStatePause)
                        {
                            ScaraRobot.GetInstance().SetStopActionFlag();
                            return WaranResult.CheckAgain;
                        }
                        else if (GlobalVariable.g_StationState == StationState.StationStateRun)
                        {
                            ScaraRobot.GetInstance().ReasetStopActionFlag();
                            if (bInPos)
                                return WaranResult.Run;
                            else if (ScaraRobot.GetInstance().InPos && !bInPos)
                            {
                                ScaraRobot.GetInstance().Go(coordinate, direction);
                                return WaranResult.CheckAgain;
                            }

                        }
                    }

                    if (nTimeed > nTimeout)
                    {
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().ReasetStopActionFlag();
                        return WaranResult.TimeOut;
                    }


                    if (ScaraRobot.GetInstance().InPos && bInPos)
                        return WaranResult.Run;
                    else
                        return WaranResult.CheckAgain;
                }
                , 30000);
                return doWhile.doSomething2(null, doWhile, bmauanl, null);
            }
            catch (Exception ex)
            {
                ScaraRobot.GetInstance().SetStopActionFlag();
                ScaraRobot.GetInstance().SetStopActionFlag();
                ScaraRobot.GetInstance().ReasetStopActionFlag();
                throw ex;
            }
        }


    }


}
