using BaseDll;
using CommonTools;
using EpsonRobot;
using log4net;
using System;
using System.Threading;

namespace StationDemo
{
    public static class RoboteExt
    {
        private static ILog logger = LogManager.GetLogger("RoboteExt");
        private static string RobotPauseIo = "";
        private static string RobotResumeIo = "";
        private static double limitz = -10;

        private static double getLimit(Coordinate pos)
        {
            double limtZ = pos.Z > limitz ? pos.Z + 0.2 : limitz;
            limtZ = limtZ >= ScaraRobot.GetInstance().CurrentPosition.Z ? limtZ : ScaraRobot.GetInstance().CurrentPosition.Z + 0.2;
            return limtZ;
        }

        public static WaranResult JumpInPos(this ScaraRobot scaraRobot, Coordinate coordinate, HandDirection direction, double limz, bool bCheckHandleSys = false, bool bmauanl = false, int nTimeout = 20000)
        {
            logger.Info($"JumpInPos start pos{coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}");
            try
            {
                WaranResult waran = WaranResult.Failture;
                bool bcmd = ScaraRobot.GetInstance().Jump(coordinate, direction, limz);
                DoWhile doWhile = new DoWhile((nTimeed, doWhile2, bmanual, objs) =>
                {
                    double dfine = 0.05;
                    bool bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < dfine;
                    bool bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < dfine;
                    // bool bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < dfine;
                    bool bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                    bool bInPos = bInPosX && bInPosY && true && bInPosU;

                    if (!bmanual)
                    {
                        if (GlobalVariable.g_StationState == StationState.StationStatePause)
                        {
                            logger.Info($"JumpInPos  程序状态： {GlobalVariable.g_StationState }");
                            ScaraRobot.GetInstance().SetStopActionFlag();
                            return WaranResult.CheckAgain;
                        }
                        else if (GlobalVariable.g_StationState == StationState.StationStateRun)
                        {
                            Thread.Sleep(50);
                            bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < dfine;
                            bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < dfine;
                            //  bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < dfine;
                            bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                            bInPos = bInPosX && bInPosY && true && bInPosU;
                            if (!bInPos)
                            {
                                Coordinate coordinatetemp = ScaraRobot.GetInstance().GetCurrentImmediately();
                                bInPosX = Math.Abs(coordinatetemp.X - coordinate.X) < dfine;
                                bInPosY = Math.Abs(coordinatetemp.Y - coordinate.Y) < dfine;
                                // bInPosZ = Math.Abs(coordinatetemp.Z - coordinate.Z) < dfine;
                                bInPosU = Math.Abs(coordinatetemp.U - coordinate.U) < 0.1;
                                bInPos = bInPosX && bInPosY && true && bInPosU;
                            }
                            logger.Info($"JumpInPos  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos}, 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                              $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

                            if (bInPos)
                                return WaranResult.Run;
                            else if (ScaraRobot.GetInstance().InPos && !bInPos)
                            {
                                ScaraRobot.GetInstance().ReasetStopActionFlag();
                                ScaraRobot.GetInstance().Jump(coordinate, direction, limz);
                                return WaranResult.CheckAgain;
                            }
                        }
                    }

                    if (nTimeed > nTimeout)
                    {
                        logger.Info($"JumpInPos  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos}, 超时， 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                             $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().ReasetStopActionFlag();
                        logger.Info($"JumpInPos  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos}, 复位， 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                            $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

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
                logger.Info($"GoInPos start pos{coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}");
                WaranResult waran = WaranResult.Failture;
                bool bcmd = ScaraRobot.GetInstance().Go(coordinate, direction);
                DoWhile doWhile = new DoWhile((nTimeed, doWhile2, bmanual, objs) =>
                {
                    double dfine = 0.05;
                    bool bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < dfine;
                    bool bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < dfine;
                    bool bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < dfine;
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
                            Thread.Sleep(50);
                            bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < dfine;
                            bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < dfine;
                            bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < dfine;
                            bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                            bInPos = bInPosX && bInPosY && bInPosZ && bInPosU;
                            if (!bInPos)
                            {
                                Coordinate coordinatetemp = ScaraRobot.GetInstance().GetCurrentImmediately();
                                bInPosX = Math.Abs(coordinatetemp.X - coordinate.X) < dfine;
                                bInPosY = Math.Abs(coordinatetemp.Y - coordinate.Y) < dfine;
                                bInPosZ = Math.Abs(coordinatetemp.Z - coordinate.Z) < dfine;
                                bInPosU = Math.Abs(coordinatetemp.U - coordinate.U) < 0.1;
                                bInPos = bInPosX && bInPosY && bInPosZ && bInPosU;
                            }
                            logger.Info($"GoInPos  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos}, 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                              $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");
                            if (ScaraRobot.GetInstance().InPos && bInPos)
                                return WaranResult.Run;
                            else if (ScaraRobot.GetInstance().InPos && !bInPos)
                            {
                                ScaraRobot.GetInstance().ReasetStopActionFlag();
                                ScaraRobot.GetInstance().Go(coordinate, direction);
                                return WaranResult.CheckAgain;
                            }
                        }
                    }

                    if (nTimeed > nTimeout)
                    {
                        logger.Info($"GoInPos  程序状态： {GlobalVariable.g_StationState } {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U} 超时 ");
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().ReasetStopActionFlag();

                        logger.Info($"GoInPos  机器人停止 复位，{GlobalVariable.g_StationState}  {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U} 复位");

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
                logger.Info($"GoInPosZ start Zpos {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}");
                WaranResult waran = WaranResult.Failture;
                bool bcmd = ScaraRobot.GetInstance().Go(coordinate, direction);
                DoWhile doWhile = new DoWhile((nTimeed, doWhile2, bmanual, objs) =>
                {
                    double dfine = 0.05;
                    double ZPOS = ScaraRobot.GetInstance().CurrentPosition.Z;
                    bool bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < dfine;
                    bool bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < dfine;
                    bool bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < dfine;
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
                            Thread.Sleep(150);
                            bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < dfine;
                            bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < dfine;
                            bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < dfine;
                            bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                            bInPos = bInPosX && bInPosY && bInPosZ && bInPosU;
                            if (!bInPos)
                            {
                                Coordinate coordinatetemp = ScaraRobot.GetInstance().GetCurrentImmediately();
                                bInPosX = Math.Abs(coordinatetemp.X - coordinate.X) < dfine;
                                bInPosY = Math.Abs(coordinatetemp.Y - coordinate.Y) < dfine;
                                bInPosZ = Math.Abs(coordinatetemp.Z - coordinate.Z) < dfine;
                                bInPosU = Math.Abs(coordinatetemp.U - coordinate.U) < 0.1;
                                bInPos = bInPosX && bInPosY && bInPosZ && bInPosU;
                            }
                            logger.Info($"GoInPosZ  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos}, 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                                $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

                            if (bInPos)
                                return WaranResult.Run;
                            else if (ScaraRobot.GetInstance().InPos && !bInPos)
                            {
                                ScaraRobot.GetInstance().ReasetStopActionFlag();
                                ScaraRobot.GetInstance().Go(coordinate, direction);
                                return WaranResult.CheckAgain;
                            }
                        }
                    }

                    if (nTimeed > nTimeout)
                    {
                        logger.Info($"GoInPosZ  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos},超时， 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                               $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().ReasetStopActionFlag();
                        logger.Info($"GoInPosZ  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos},复位， 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                             $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

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

        public static WaranResult JumpInPos(this ScaraRobot scaraRobot, Coordinate coordinate, HandDirection direction, bool bCheckHandleSys = false, bool bmauanl = false, int nTimeout = 20000)
        {
            logger.Info($"JumpInPos start pos{coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}");
            try
            {
                WaranResult waran = WaranResult.Failture;
                bool bcmd = ScaraRobot.GetInstance().Jump(coordinate, direction, getLimit(coordinate));
                DoWhile doWhile = new DoWhile((nTimeed, doWhile2, bmanual, objs) =>
                {
                    double dfine = 0.05;
                    bool bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < dfine;
                    bool bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < dfine;
                    // bool bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < dfine;
                    bool bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                    bool bInPos = bInPosX && bInPosY && true && bInPosU;

                    if (!bmanual)
                    {
                        if (GlobalVariable.g_StationState == StationState.StationStatePause)
                        {
                            logger.Info($"JumpInPos  程序状态： {GlobalVariable.g_StationState }");
                            ScaraRobot.GetInstance().SetStopActionFlag();
                            return WaranResult.CheckAgain;
                        }
                        else if (GlobalVariable.g_StationState == StationState.StationStateRun)
                        {
                            Thread.Sleep(50);
                            bInPosX = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.X - coordinate.X) < dfine;
                            bInPosY = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Y - coordinate.Y) < dfine;
                            //  bInPosZ = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.Z - coordinate.Z) < dfine;
                            bInPosU = Math.Abs(ScaraRobot.GetInstance().CurrentPosition.U - coordinate.U) < 0.1;
                            bInPos = bInPosX && bInPosY && true && bInPosU;
                            if (!bInPos)
                            {
                                Coordinate coordinatetemp = ScaraRobot.GetInstance().GetCurrentImmediately();
                                bInPosX = Math.Abs(coordinatetemp.X - coordinate.X) < dfine;
                                bInPosY = Math.Abs(coordinatetemp.Y - coordinate.Y) < dfine;
                                // bInPosZ = Math.Abs(coordinatetemp.Z - coordinate.Z) < dfine;
                                bInPosU = Math.Abs(coordinatetemp.U - coordinate.U) < 0.1;
                                bInPos = bInPosX && bInPosY && true && bInPosU;
                            }
                            logger.Info($"JumpInPos  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos}, 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                              $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

                            if (bInPos)
                                return WaranResult.Run;
                            else if (ScaraRobot.GetInstance().InPos && !bInPos)
                            {
                                ScaraRobot.GetInstance().ReasetStopActionFlag();
                                ScaraRobot.GetInstance().Jump(coordinate, direction, getLimit(coordinate));
                                return WaranResult.CheckAgain;
                            }
                        }
                    }

                    if (nTimeed > nTimeout)
                    {
                        logger.Info($"JumpInPos  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos}, 超时， 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                             $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().SetStopActionFlag();
                        ScaraRobot.GetInstance().ReasetStopActionFlag();
                        logger.Info($"JumpInPos  程序状态：{GlobalVariable.g_StationState} bInPos {bInPos}, 复位， 目标位置 {coordinate.X}，{coordinate.Y}，{coordinate.Z}，{coordinate.U}" +
                            $"实际位置： {ScaraRobot.GetInstance().CurrentPosition.X}，{ScaraRobot.GetInstance().CurrentPosition.Y}，{ScaraRobot.GetInstance().CurrentPosition.Z}，{ScaraRobot.GetInstance().CurrentPosition.U}");

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