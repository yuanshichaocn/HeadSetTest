using BaseDll;
using CommonTools;
using UserData;

namespace StationDemo
{
    public static partial class UserConfig
    {
        public static void UpdataTrayData(string strStationName, string posName)
        {
            if (("取料站" == strStationName || "Barrel站" == strStationName) && posName.Contains("料盘"))
            {
                if (StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic().ContainsKey(posName))
                {
                    int indexofstring = posName.IndexOf("_");
                    string sub = posName.Substring(indexofstring + 1);
                    indexofstring = sub.IndexOf("_");
                    sub = sub.Substring(0, indexofstring);
                    int index = sub.ToInt();
                    if (posName.Contains($"_M_SP"))
                    {
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].PlaceLeftTopcoordinate =
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].PickLeftTopcoordinate =
                           new Coordinate()
                           {
                               X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                               Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                               Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                           };
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init();
                    }
                    if (posName.Contains($"_M_EP"))
                    {
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].PlaceRightBottomcoordinate =
                            TrayMgr.GetInstance().trayDataLoadArr[index - 1].PickRightBottomcoordinate =
                            new Coordinate()
                            {
                                X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                                Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                                Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                            };
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init();
                    }
                    if (posName.Contains($"_V_SP"))
                    {
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].SnapLeftTopcoordinate =
                           new Coordinate()
                           {
                               X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                               Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                               Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                           };
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init();
                    }
                    if (posName.Contains($"_V_EP"))
                    {
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].SnapRightBottomcoordinate =
                            new Coordinate()
                            {
                                X = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointX,
                                Y = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointY,
                                Z = StationMgr.GetInstance().GetStation(strStationName).GetStationPointDic()[posName].pointZ,
                            };
                        TrayMgr.GetInstance().trayDataLoadArr[index - 1].Init();
                    }
                }
            }
        }

        public static bool UpdataTrayData()
        {
            return true;
        }
    }
}