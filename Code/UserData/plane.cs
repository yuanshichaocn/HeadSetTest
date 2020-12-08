using BaseDll;
using System;
using System.Collections.Generic;

namespace UserData
{
    public enum PlaneState
    {
        None,
        Have,
        HaveOK,
        HaveNG,
        HaveUnKnow,
    }

    public enum UseBarrelClampMode
    {
        不使用,
        使用,
    }

    public enum UseNozzleMode
    {
        不使用,
        只用中间,
        只用外边,
        中间外边,
    }

    public enum PlaneType
    {
        A,
        B,
    }

    public struct MeasureData
    {
        public Sparepart eSparepart;
        public string strCmdName;
        public double dMeasureValLaser;
        public double dMeasureValEnc;
        public double dAngle;
        public double dX;
        public double dY;
        public double dUpZpos;
        public double dLeadVcmPos;
        public double dEnteryVcmPos;
        public double dPressVcmPos;
        public int TrayIndexFrom;//Barrel 来自那个盘
        public int TrayCellIndexFrom;//Barrel 来自盘的哪个位置
    }

    public struct LaserRelativedData
    {
        public double dUpZVal;
        public double dDownZVal;
        public double dLaserMeasure;
    }

    public class PlaneData
    {
        public int Index = 0;
        public string strBarCode2d;
        public string strBarCode1d;
        private PlaneState _planeState = PlaneState.None;

        public PlaneState planeState
        {
            set
            {
                if (value == PlaneState.None)
                {
                    Index = 0;
                    strBarCode2d = "";
                    strBarCode1d = "";

                    dDataMeasureVal.Clear();
                }
                _planeState = value;
            }
            get
            {
                return _planeState;
            }
        }

        public UseNozzleMode eUseNozzleMode;
        public UseBarrelClampMode eUseBarrelClampMode;

        public double dCurrentAngle = 0;
        public int TrayIndexFrom = 0;//Barrel 来自那个盘
        public int TrayCellIndexFrom = 0;//Barrel 来自盘的哪个位置

        public void ClearData()
        {
            Index = 0;
            strBarCode2d = "";
            strBarCode1d = "";
            planeState = PlaneState.None;
            dDataMeasureVal.Clear();
        }

        public LaserRelativedData dBottomHigh = new LaserRelativedData();

        public void Save(string strPath)
        {
            AccessXmlSerializer.ObjectToXml(@"E:\VCMPlaneBottomHigh.xml", dBottomHigh);
        }

        public void Read()
        {
            dBottomHigh = (LaserRelativedData)AccessXmlSerializer.XmlToObject(@"E:\VCMPlaneBottomHigh.xml", dBottomHigh.GetType());
        }

        public Dictionary<int, MeasureData> dDataMeasureVal = new Dictionary<int, MeasureData>();

        public PlaneData()
        {
            strBarCode2d = "";
            strBarCode1d = "";
            planeState = PlaneState.None;
            eUseNozzleMode = UseNozzleMode.不使用;
            eUseBarrelClampMode = UseBarrelClampMode.不使用;
            ClearData();
        }

        public void SetLaserData(double dLaserVal)
        {
            if (!dDataMeasureVal.ContainsKey(Index))
            {
                dDataMeasureVal.Add(Index, new MeasureData()
                {
                    dMeasureValLaser = dLaserVal
                });
            }
            else
            {
                MeasureData md = dDataMeasureVal[Index];
                md.dMeasureValLaser = dLaserVal;
                dDataMeasureVal[Index] = md;
            }
        }

        public void SetEncData(double dEncVal)
        {
            if (!dDataMeasureVal.ContainsKey(Index))
            {
                dDataMeasureVal.Add(Index, new MeasureData()
                {
                    dMeasureValEnc = dEncVal
                });
            }
            else
            {
                MeasureData md = dDataMeasureVal[Index];
                md.dMeasureValEnc = dEncVal;
                dDataMeasureVal[Index] = md;
            }
        }

        public void SetAngle(double dAngle)
        {
            if (!dDataMeasureVal.ContainsKey(Index))
            {
                dDataMeasureVal.Add(Index, new MeasureData()
                {
                    dAngle = dAngle
                });
            }
            else
            {
                MeasureData md = dDataMeasureVal[Index];
                md.dAngle = dAngle;
                dDataMeasureVal[Index] = md;
            }
        }

        public void SetSparepart(Sparepart sparepart)
        {
            if (!dDataMeasureVal.ContainsKey(Index))
            {
                dDataMeasureVal.Add(Index, new MeasureData()
                {
                    eSparepart = sparepart
                });
            }
            else
            {
                MeasureData md = dDataMeasureVal[Index];
                md.eSparepart = sparepart;
                dDataMeasureVal[Index] = md;
            }
        }

        public int GetFromTrayIndex(int nIndex)
        {
            if (dDataMeasureVal.ContainsKey(nIndex))
                return dDataMeasureVal[nIndex].TrayIndexFrom;
            return -1;
        }

        public int GetFromTrayCellIndex(int nIndex)
        {
            if (dDataMeasureVal.ContainsKey(nIndex))
                return dDataMeasureVal[nIndex].TrayCellIndexFrom;
            return -1;
        }

        public void SetFrom(int nIndex, int nTrayIndex, int nCellIndex)
        {
            if (!dDataMeasureVal.ContainsKey(Index))
            {
                dDataMeasureVal.Add(Index, new MeasureData()
                {
                    TrayCellIndexFrom = nCellIndex,
                    TrayIndexFrom = nTrayIndex,
                });
            }
            else
            {
                MeasureData md = dDataMeasureVal[Index];
                md.TrayCellIndexFrom = nCellIndex;
                md.TrayIndexFrom = nTrayIndex;
                dDataMeasureVal[Index] = md;
            }
        }
    }

    public class PlaneMgr
    {
        public delegate void UpdataPlaneData(int index, string ModifyType, params object[] objs);

        public event UpdataPlaneData eventUpdataPlaneData = null;

        private PlaneMgr()
        {
        }

        private static object obj = new object();
        private static PlaneMgr planeMgr;

        public static PlaneMgr GetInstance()
        {
            if (planeMgr == null)
            {
                lock (obj)
                {
                    if (planeMgr == null)
                    {
                        planeMgr = new PlaneMgr();
                    }
                }
            }
            return planeMgr;
        }

        private PlaneData[] PlaneArr = new PlaneData[2] { new PlaneData(), new PlaneData() };

        public void SetPlaneBottomHigh(double dHigh, double dUpZ, double dDownZ)
        {
            PlaneArr[0].dBottomHigh = new LaserRelativedData()
            {
                dDownZVal = dDownZ,
                dUpZVal = dUpZ,
                dLaserMeasure = dHigh,
            };

            PlaneArr[0].Save("");
        }

        public LaserRelativedData GetPlaneBottomHigh(double dHigh)
        {
            PlaneArr[0].Read();
            return PlaneArr[0].dBottomHigh;
        }

        public void SetPlaneState(PlaneState planeState)
        {
            PlaneArr[0].planeState = planeState;
            if (eventUpdataPlaneData != null)
                eventUpdataPlaneData(PlaneArr[0].Index, "SetPlaneState", planeState);
        }

        public PlaneState GetPlaneState()
        {
            return PlaneArr[0].planeState;
        }

        public void SetLayerXYAngle(String cmdName, double dx, double dy, double dAngle, double dVcmLeadPos = 0, double dVcmEntryPos = 0, double dVcmPressPos = 0)
        {
            PlaneArr[0].SetAngle(dAngle);
            if (eventUpdataPlaneData != null)
                eventUpdataPlaneData(PlaneArr[0].Index, "SetLayerXYAngle", dx, dy, dAngle);
        }

        public void SetLayerSparepart(Sparepart eSparepart)
        {
            PlaneArr[0].SetSparepart(eSparepart);
            if (eventUpdataPlaneData != null)
                eventUpdataPlaneData(PlaneArr[0].Index, "SetLayerSparepart", eSparepart);
        }

        public void SetLayerCmd(string cmdname)
        {
            if (eventUpdataPlaneData != null)
                eventUpdataPlaneData(PlaneArr[0].Index, "SetLayerCmd", cmdname);
        }

        public double GetTopLayerAngle()
        {
            int index = PlaneArr[0].Index;
            return PlaneArr[0].dDataMeasureVal[index].dAngle;
        }

        public void SetLayerLaserData(double dLaserData)
        {
            PlaneArr[0].SetLaserData(dLaserData);
            if (eventUpdataPlaneData != null)
                eventUpdataPlaneData(PlaneArr[0].Index, "SetLayerLaserData", dLaserData);
        }

        public double GetLayerLaserData()
        {
            int index = PlaneArr[0].Index;
            return PlaneArr[0].dDataMeasureVal[index].dMeasureValLaser;
        }

        public void SetLayerEncData(double dEncData)
        {
            PlaneArr[0].SetEncData(dEncData);
            if (eventUpdataPlaneData != null)
                eventUpdataPlaneData(PlaneArr[0].Index, "SetLayerEncData", dEncData);
        }

        public double GetLayerEncData()
        {
            int index = PlaneArr[0].Index;
            return PlaneArr[0].dDataMeasureVal[index].dMeasureValEnc;
        }

        public void ResetPlane()
        {
            PlaneArr[0].ClearData();
            if (eventUpdataPlaneData != null)
                eventUpdataPlaneData(PlaneArr[0].Index, "ResetPlaneData");
        }

        public void SetFrom(int nTrayIndex, int nCellIndex)
        {
            PlaneArr[0].SetFrom(PlaneArr[0].Index, nTrayIndex, nCellIndex);
        }

        public void SetFrom(int nIndex, int nTrayIndex, int nCellIndex)
        {
            PlaneArr[0].SetFrom(nIndex, nTrayIndex, nCellIndex);
        }

        public int GetFromTrayIndex(int nIndex = 0)
        {
            return PlaneArr[0].GetFromTrayIndex(nIndex);
        }

        public int GetFromTrayCellIndex(int nIndex = 0)
        {
            return PlaneArr[0].GetFromTrayCellIndex(nIndex);
        }

        public int Index
        {
            set => PlaneArr[0].Index = value;
            get => PlaneArr[0].Index;
        }
    }
}