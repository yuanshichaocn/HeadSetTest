using BaseDll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UserData
{
    public enum TrayCellState
    {
        None,//无料
        Pending,//有料
        Err,//错误
        NG,
        OK,
        //LocationStateOK = 0,
        //LocationStateFail = 1,
        //BarCodeStateOK = 2,
        //BarCodeStateFail = 3,
    }

    public class TrayCell
    {
        public string m_BarCode;
        public string m_1barCode;
        public double m_Vx;
        public double m_Vy;

        public Coordinate Snapcoordinate = new Coordinate();
        public Coordinate Pickcoordinate = new Coordinate();
        public Coordinate Placecoordinate = new Coordinate();

        public TrayCell()
        {
            m_BarCode = "";
            trayState = TrayCellState.None;
            m_Vx = 0;
            m_Vy = 0;
        }

        public void ReasetPending()
        {
            m_BarCode = "";
            trayState = TrayCellState.Pending;
            m_Vx = 0;
            m_Vy = 0;
        }

        public void ReasetNone()
        {
            m_BarCode = "";
            trayState = TrayCellState.None;
            m_Vx = 0;
            m_Vy = 0;
        }

        public TrayCellState trayState;
    }

    public class TrayData
    {
        public TrayData()
        {
        }

        public Coordinate SnapLeftTopcoordinate = new Coordinate();
        public Coordinate SnapRightTopcoordinate = new Coordinate();
        public Coordinate SnapRightBottomcoordinate = new Coordinate();

        public Coordinate PickLeftTopcoordinate = new Coordinate();
        public Coordinate PickRightTopcoordinate = new Coordinate();
        public Coordinate PickRightBottomcoordinate = new Coordinate();

        public Coordinate PlaceLeftTopcoordinate = new Coordinate();
        public Coordinate PlaceRightTopcoordinate = new Coordinate();
        public Coordinate PlaceRightBottomcoordinate = new Coordinate();
        public Sparepart eSparepart = Sparepart.None;

        public bool Init()
        {
            Coordinate coordinate = new Coordinate();

            trayCells.Clear();
            double dx, dy;
            coordinate = SnapLeftTopcoordinate;
            dy = RowCount > 1 ? (SnapRightBottomcoordinate.Y - SnapLeftTopcoordinate.Y) / (RowCount - 1) : 0;
            dx = ColCount > 1 ? (SnapRightBottomcoordinate.X - SnapLeftTopcoordinate.X) / (ColCount - 1) : 0;
            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    TrayCell trayCell = new TrayCell();
                    trayCells.Add(trayCell);
                    trayCells[i * RowCount + j].Snapcoordinate.Y = SnapLeftTopcoordinate.Y + j * dy;
                    trayCells[i * RowCount + j].Snapcoordinate.X = SnapLeftTopcoordinate.X + i * dx;
                    trayCells[i * RowCount + j].Snapcoordinate.U = coordinate.U;
                    trayCells[i * RowCount + j].Snapcoordinate.Z = coordinate.Z;
                    //trayCells[i * ColCount + j].Snapcoordinate = coordinate;
                }
            }

            coordinate = PickLeftTopcoordinate;
            dy = RowCount > 1 ? (PickRightBottomcoordinate.Y - PickLeftTopcoordinate.Y) / (RowCount - 1) : 0;
            dx = ColCount > 1 ? (PickRightBottomcoordinate.X - PickLeftTopcoordinate.X) / (ColCount - 1) : 0;
            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    trayCells[i * RowCount + j].Pickcoordinate.Y = PickLeftTopcoordinate.Y + j * dy;
                    trayCells[i * RowCount + j].Pickcoordinate.X = PickLeftTopcoordinate.X + i * dx;
                    trayCells[i * RowCount + j].Pickcoordinate.U = coordinate.U;
                    trayCells[i * RowCount + j].Pickcoordinate.Z = coordinate.Z;
                    //trayCells[i * ColCount + j].Pickcoordinate = coordinate;
                }
            }

            coordinate = PlaceLeftTopcoordinate;
            dy = RowCount > 1 ? (PlaceRightBottomcoordinate.Y - PlaceLeftTopcoordinate.Y) / (RowCount - 1) : 0;
            dx = ColCount > 1 ? (PlaceRightBottomcoordinate.X - PlaceLeftTopcoordinate.X) / (ColCount - 1) : 0;
            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    trayCells[i * RowCount + j].Placecoordinate.Y = PlaceLeftTopcoordinate.Y + j * dy;
                    trayCells[i * RowCount + j].Placecoordinate.X = PlaceLeftTopcoordinate.X + i * dx;
                    trayCells[i * RowCount + j].Placecoordinate.U = coordinate.U;
                    trayCells[i * RowCount + j].Placecoordinate.Z = coordinate.Z;
                }
            }

            return true;
        }

        public bool Init2()
        {
            Coordinate coordinate = new Coordinate();
            trayCells.Clear();
            double dx, dy;
            coordinate = SnapLeftTopcoordinate;
            dy = RowCount > 1 ? (SnapRightBottomcoordinate.Y - SnapLeftTopcoordinate.Y) / (RowCount - 1) : 0;
            dx = ColCount > 1 ? (SnapRightBottomcoordinate.X - SnapLeftTopcoordinate.X) / (ColCount - 1) : 0;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    TrayCell trayCell = new TrayCell();
                    trayCells.Add(trayCell);
                    trayCells[i * ColCount + j].Snapcoordinate.Y = SnapLeftTopcoordinate.Y + i * dy;
                    trayCells[i * ColCount + j].Snapcoordinate.X = SnapLeftTopcoordinate.X + j * dx;
                    trayCells[i * ColCount + j].Snapcoordinate.U = coordinate.U;
                    trayCells[i * ColCount + j].Snapcoordinate.Z = coordinate.Z;
                    //trayCells[i * ColCount + j].Pickcoordinate = coordinate;
                }
            }
            coordinate = PickLeftTopcoordinate;
            dy = RowCount > 1 ? (PickRightBottomcoordinate.Y - PickLeftTopcoordinate.Y) / (RowCount - 1) : 0;
            dx = ColCount > 1 ? (PickRightBottomcoordinate.X - PickLeftTopcoordinate.X) / (ColCount - 1) : 0;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    trayCells[i * ColCount + j].Pickcoordinate.Y = PickLeftTopcoordinate.Y + i * dy;
                    trayCells[i * ColCount + j].Pickcoordinate.X = PickLeftTopcoordinate.X + j * dx;
                    trayCells[i * ColCount + j].Pickcoordinate.U = coordinate.U;
                    trayCells[i * ColCount + j].Pickcoordinate.Z = coordinate.Z;
                    //trayCells[i * ColCount + j].Pickcoordinate = coordinate;
                }
            }
            coordinate = PlaceLeftTopcoordinate;
            dy = RowCount > 1 ? (PlaceRightBottomcoordinate.Y - PlaceLeftTopcoordinate.Y) / (RowCount - 1) : 0;
            dx = ColCount > 1 ? (PlaceRightBottomcoordinate.X - PlaceLeftTopcoordinate.X) / (ColCount - 1) : 0;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    trayCells[i * ColCount + j].Placecoordinate.Y = PlaceLeftTopcoordinate.Y + i * dy;
                    trayCells[i * ColCount + j].Placecoordinate.X = PlaceLeftTopcoordinate.X + j * dx;
                    trayCells[i * ColCount + j].Placecoordinate.U = coordinate.U;
                    trayCells[i * ColCount + j].Placecoordinate.Z = coordinate.Z;
                }
            }
            return true;
        }

        public List<TrayCell> trayCells = new List<TrayCell>();

        public void ResetAllCellsPending()
        {
            for (int i = 0; i < trayCells.Count; i++)
                trayCells[i].ReasetPending();
        }

        public void ResetAllCellsNone()
        {
            for (int i = 0; i < trayCells.Count; i++)
                trayCells[i].ReasetNone();
        }

        public void ResetOneCell(int index, bool bPending)
        {
            if (bPending)
                trayCells[index].ReasetPending();
            else
                trayCells[index].ReasetNone();
        }

        public void ReasetOneCellStatus(int index, TrayCellState tmptrayState)
        {
            trayCells[index].trayState = tmptrayState;
        }

        public int RowCount
        {
            set;
            get;
        }

        public int ColCount
        {
            set;
            get;
        }

        [JsonIgnore]
        public int TotalCount
        {
            get => ColCount * RowCount;
        }

        public TrayCell this[int nRow, int nCol]
        {
            get => trayCells[nRow * RowCount + nCol];
        }

        public TrayCell this[int index]
        {
            get => trayCells[index];
        }

        public void SetSnapPos(int index, double x, double y, double z, double u)
        {
            trayCells[index].Snapcoordinate.X = x;
            trayCells[index].Snapcoordinate.Y = y;
            trayCells[index].Snapcoordinate.Z = z;
            trayCells[index].Snapcoordinate.U = u;
        }

        public void SetPickPos(int index, double x, double y, double z, double u)
        {
            trayCells[index].Pickcoordinate.X = x;
            trayCells[index].Pickcoordinate.Y = y;
            trayCells[index].Pickcoordinate.Z = z;
            trayCells[index].Pickcoordinate.U = u;
        }

        public void GetRC(int index, ref int Row, ref int Col)
        {
            Row = index / RowCount;
            Col = index % RowCount;
        }

        public TrayCellState GetTrayCellState(int index)
        {
            if (index < 0 || index >= trayCells.Count)
                return TrayCellState.Err;
            return trayCells[index].trayState;
        }

        //public void SetTrayState(int index, TrayCellState trayCellState)
        //{
        //    if (index < 0 || index >= trayCells.Count)
        //        return ;
        //     trayCells[index].trayState = trayCellState;
        //}
        public int index = 0;

        public void Save()
        {
            AccessJosnSerializer.ObjectToJson(AppDomain.CurrentDomain.BaseDirectory + @"\config\traydata.xml", this);
        }

        public object Read()
        {
            object obj = null;
            obj = AccessJosnSerializer.JsonToObject(AppDomain.CurrentDomain.BaseDirectory + @"\config\traydata.xml", this.GetType());
            if (obj == null)
            {
                MessageBox.Show("Tray 数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return obj;
        }
    }

    public enum TrayType
    {
        Load,
        unLoad,
        feed,
        collect,
        LoadSnap,
        Stick1,
        Stick2,
    }

    public class TrayMgr
    {
        public TrayMgr()
        {
            trayDataLoadArr = new TrayData[30];
            for (int i = 0; i < trayDataLoadArr.Length; i++)
                trayDataLoadArr[i] = new TrayData();
        }

        private static object obj = new object();
        private static TrayMgr trayMgr;

        public static TrayMgr GetInstance()
        {
            if (trayMgr == null)
            {
                lock (obj)
                {
                    if (trayMgr == null)
                    {
                        trayMgr = new TrayMgr();
                    }
                }
            }
            return trayMgr;
        }

        //public TrayData trayDataLoad = new TrayData();
        public void Save()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"\config\traydata.xml";
            AccessXmlSerializer.ObjectToXml(strPath, this);
            // trayDataLoad.Save();
        }

        public object Read()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"\config\traydata.xml";
            Object obj = AccessXmlSerializer.XmlToObject(strPath, this.GetType());
            if (obj != null)
                trayMgr = (TrayMgr)obj;
            return trayMgr;
            // return  trayDataLoad.Read();
        }

        public TrayData[] trayDataLoadArr = null;
    }

    //public enum LineState
    //{
    //    None,
    //    Have,
    //    Finished,
    //    //Feeding,
    //    //Feeded,
    //    //loading,
    //    //Loaded,
    //    //UnLoading,
    //    //UnLoaded,
    //    //Collecting,
    //    //Collected,

    //}
    //public class LineMgr
    //{
    //    private LineMgr()
    //    { }
    //    private static LineMgr lineMgr;
    //    private static object lockobj = new object();
    //    public  static LineMgr GetInstance()
    //    {
    //        if(lineMgr==null)
    //        {
    //            lock(lockobj)
    //            {
    //                if(lineMgr==null)
    //                {
    //                    lineMgr = new LineMgr();
    //                }
    //            }
    //        }
    //        return lineMgr;
    //    }
    //    public static object feedlock = new object();
    //    public static object collectlock = new object();
    //    public static object loadlock = new object();
    //    public static object unloadlock = new object();
    //    public LineState FeedState
    //    {
    //        set {
    //            lock(feedlock)
    //            {
    //                lineState[0] = value;
    //            }
    //            }

    //        get => lineState[0] ;
    //    }
    //    public LineState CollectState
    //    {
    //        set
    //        {
    //            lock (collectlock)
    //            {
    //                lineState[3] = value;
    //            }
    //        }

    //        get => lineState[3];
    //    }
    //    public LineState LoadState
    //    {
    //        set
    //        {
    //            lock (loadlock)
    //            {
    //                lineState[1] = value;
    //            }
    //        }

    //        get => lineState[1];
    //    }
    //    public LineState UnLoadState
    //    {
    //        set
    //        {
    //            lock (unloadlock)
    //            {
    //                lineState[2] = value;
    //            }
    //        }

    //        get => lineState[2];
    //    }

    //    private  LineState[] lineState = new LineState[] { LineState.None, LineState.None, LineState.None, LineState.None };

    //}

    public class NDTDataMgr
    {
        private static object obj = new object();
        private static NDTDataMgr ndtMgr;

        public static NDTDataMgr GetInstance()
        {
            if (ndtMgr == null)
            {
                lock (obj)
                {
                    if (ndtMgr == null)
                    {
                        ndtMgr = new NDTDataMgr();
                    }
                }
            }
            return ndtMgr;
        }

        private object NDTlock = new object();
        private object NDTlock1 = new object();
        private object NDTlock2 = new object();

        //private object NDT2lock = new object();
        private List<Tuple<int, bool>> tuples = new List<Tuple<int, bool>>();

        private List<bool> resultNdt1 = new List<bool>();
        private List<bool> resultNdt2 = new List<bool>();

        public void AddResult(int indexNDT, bool bresult)
        {
            switch (indexNDT)
            {
                case 1:
                    lock (NDTlock1)
                    {
                        resultNdt1.Add(bresult);
                    }
                    break;

                case 2:
                    lock (NDTlock2)
                    {
                        resultNdt2.Add(bresult);
                    }
                    break;
            }
        }

        public void Clear(int indexNDT)
        {
            switch (indexNDT)
            {
                case 1:
                    lock (NDTlock1)
                    {
                        resultNdt1.Clear();
                    }
                    break;

                case 2:
                    lock (NDTlock2)
                    {
                        resultNdt2.Clear();
                    }
                    break;
            }
        }

        public void RemoveFirist(int indexNDT)
        {
            switch (indexNDT)
            {
                case 1:
                    lock (NDTlock1)
                    {
                        if (resultNdt1.Count > 0)
                            resultNdt1.RemoveAt(0);
                    }
                    break;

                case 2:
                    lock (NDTlock2)
                    {
                        if (resultNdt2.Count > 0)
                            resultNdt2.RemoveAt(0);
                    }
                    break;
            }
        }

        public int Count1
        {
            get => resultNdt1.Count;
        }

        public int Count2
        {
            get => resultNdt2.Count;
        }

        public bool Firist1
        {
            get => resultNdt1.Count > 0 ? resultNdt1[0] : false;
        }

        public bool Firist2
        {
            get => resultNdt2.Count > 0 ? resultNdt2[0] : false;
        }
    }
}