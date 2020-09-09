using BaseDll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserData
{
    public enum Sparepart
    {
        None,
        Barrel,
        Lens,
        Soma,
        Spacer,
    }
    public enum NozzleState
    {
        None,
        Have,
        HaveOthers,
        HaveBarrel,
        SnapedOK,
        SnapedNG,
        HaveOK,
        HaveNg,
    }

    public class Nozzle
    {
        public Nozzle()
        {
            nozzleState = NozzleState.None;
            VacuumTime = 100;
            BreakVacuumTime = 100;

        }
        public void Reset()
        {
            strBarCode = "";
            nozzleState = NozzleState.None;
            indexPickFromSocket = -1;
        }
        public NozzleState nozzleState
        {
            set
            {
                _nozzleState = value;
                if(_nozzleState== NozzleState.None)
                {
                    indexPickFromSocket = -1;
                    strBarCode = "";
                    eSparepart = Sparepart.None;
                }
            }
            get => _nozzleState;
        }
        public Sparepart eSparepart = Sparepart.None;
        public XYUZPoint DstMachinePos
        {
            set;
            get;
        }
        public XYUPoint ObjMachinePos
        {
            set;
            get;
        }
        public XYUPoint ObjSnapMachinePos
        {
            set;
            get;

        }
        public XYUPoint DstSnapMachinePos
        {
            set;
            get;
        }
        public double dAssmebleZ0
        {
            set;
            get;
        }

        public int TrayIndexFrom = 0;//Barrel 来自那个盘
        public int TrayCellIndexFrom = 0;//Barrel 来自盘的哪个位置
    

        public XYUPoint xYUOffset = new XYUPoint(0, 0, 0);
        

       
        
        public NozzleState nozzleState2
        {
            set
            {
                _nozzleState2 = value;
                if (_nozzleState2 == NozzleState.None)
                {
                    indexPickFromSocket = -1;
                    strBarCode = "";
                }
            }
            get => _nozzleState2;
        }
        NozzleState _nozzleState;
        NozzleState _nozzleState2;
        public string NozzleVacuumIoName
        {
            set;
            get;
        }
        public string NozzleBreakVacuumIoName
        {
            set;
            get;
        }
        public string NozzleVacuumCheckIoName
        {
            set;
            get;
        }
        public int BreakVacuumTime
        {
            set;
            get;
        }
        public int VacuumTime
        {
            set;
            get;
        }
        public string strNozzleName
        {
            set;
            get;
        }
        public string strBarCode
        {
            set; get;
        }
        public string strBarCode1d
        {
            set;
            get;
        }

        public int indexPickFromSocket
        {
            set; get;
        }
     



    }
    public enum NozzleType
    {
        BuzzerNozzle_L1=0,
        BuzzerNozzle_L2,
        BuzzerNozzle_L3,
        BuzzerNozzle_L4,

        BuzzerNozzle_R1=4,
        BuzzerNozzle_R2,
        BuzzerNozzle_R3,
        BuzzerNozzle_R4,
        LoadNozzle=8,
        UnLoadNozzle,
        LeftStripNozzle,
        RightStripNozzle,
    
    }
    public delegate void UpdataNozzleDataHandler(int nIndex);
    public class NozzleMgr
    {
        public delegate void UpdataNozzle(int nNozzleNo, string ModifyType, params object[] objs);
        public event UpdataNozzle eventUpdatNozzleData = null;
        public NozzleMgr()
        {

        }
     
        private static object obj = new object();
        private static NozzleMgr nozzleMgr;
        public static NozzleMgr GetInstance()
        {
            if (nozzleMgr == null)
            {
                lock (obj)
                {
                    if (nozzleMgr == null)
                    {
                        nozzleMgr = new NozzleMgr();
                    }
                }
            }
            return nozzleMgr;
        }
        private Nozzle[] nozzleArr = new Nozzle[18] {
            new Nozzle(), new Nozzle(), new Nozzle(), new Nozzle(),
            new Nozzle(), new Nozzle(), new Nozzle(), new Nozzle(),
            new Nozzle(), new Nozzle(), new Nozzle(), new Nozzle(),
             new Nozzle(), new Nozzle(), new Nozzle(), new Nozzle(),
              new Nozzle(), new Nozzle()
        };
        public void Save()
        {

            string currentNozzleFile = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + "nozzleArr" + (".xml");
            AccessXmlSerializer.ObjectToXml(currentNozzleFile, nozzleArr);
        }
    
       public void  Read()
        {
            string currentNozzleFile = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + "nozzleArr" + (".xml");
            if( !File.Exists(currentNozzleFile))
            {
                Save();
            }
            Object obs=  AccessXmlSerializer.XmlToObject(currentNozzleFile, typeof(Nozzle[]));
            if(obs!=null)
            {
                nozzleArr =(Nozzle[])obs;
            }
        }
        public void SetNozzleState(int  nozzleType, NozzleState nozzleState)
        {
            
            nozzleArr[(int)nozzleType-1].nozzleState = nozzleState;
            if (eventUpdatNozzleData != null)
                eventUpdatNozzleData(nozzleType, "SetNozzleState", nozzleState);
        }
        public void SetNozzleAssBottomPos(int nNozzleIndex, double pos)
        {
            if (eventUpdatNozzleData != null)
                eventUpdatNozzleData(nNozzleIndex, "SetNozzleAssPos", pos);
            nozzleArr[(int)nNozzleIndex - 1].dAssmebleZ0 = pos;
        }
        public double GetNozzleAssBottomPos(int nNozzleIndex)
        {
            return    nozzleArr[(int)nNozzleIndex - 1].dAssmebleZ0 ;
        }

        public void SetFrom( int nNozzleIndex,int nTrayIndex, int nCellIndex)
        {
            if (eventUpdatNozzleData != null)
                eventUpdatNozzleData(nNozzleIndex, "SetBarrelFrom", nTrayIndex,  nCellIndex);
            nozzleArr[(int)nNozzleIndex - 1].TrayIndexFrom = nTrayIndex;
            nozzleArr[(int)nNozzleIndex - 1].TrayCellIndexFrom = nCellIndex;
        }
        public int GetFromTrayIndex(int nNozzleIndex)
        {
            return nozzleArr[(int)nNozzleIndex - 1].TrayIndexFrom;
        }
        public int GetFromTrayCellIndex(int nNozzleIndex)
        {
            return nozzleArr[(int)nNozzleIndex - 1].TrayCellIndexFrom;
        }
        public NozzleState GetNozzleState(int nozzleType)
        {

            return nozzleArr[(int)nozzleType-1].nozzleState;
        }
        public  void SetSparepartTypeOnNozzle(int nNozzleIndex, Sparepart eSparepart)
        {
            if(eventUpdatNozzleData != null)
                eventUpdatNozzleData(nNozzleIndex, "SetSparepartTypeOnNozzle", eSparepart);
            nozzleArr[(int)nNozzleIndex-1].eSparepart = eSparepart;
        }
        public Sparepart GetSparepartTypeOnNozzle(int nNozzleIndex)
        {
            return nozzleArr[(int)nNozzleIndex-1].eSparepart;
        }
     
        public void SetObjSnapPos(int nNozzleIndex, double dObjx, double dObjy, double dObju)
        {
            if (eventUpdatNozzleData != null)
                eventUpdatNozzleData(nNozzleIndex, "SetObjMachinePos", dObjx, dObjy, dObju);
            NozzleMgr.GetInstance().nozzleArr[nNozzleIndex - 1].ObjSnapMachinePos = new XYUPoint()
            {
                x = dObjx,
                y = dObjy,
                u = dObju,
            };
        }
        public XYUPoint GetObjSnapPos(int nNozzleIndex)
        {
            return NozzleMgr.GetInstance().nozzleArr[nNozzleIndex - 1].ObjSnapMachinePos;
        }

        public void SetObjMachinePos(int nNozzleIndex, double dObjx, double dObjy, double dObju)
        {
            if (eventUpdatNozzleData != null)
                eventUpdatNozzleData(nNozzleIndex, "SetObjMachinePos", dObjx, dObjy, dObju);
            NozzleMgr.GetInstance().nozzleArr[nNozzleIndex - 1].ObjMachinePos = new XYUPoint()
            {
                x = dObjx,
                y = dObjy,
                u = dObju,
            };
        }
        public XYUPoint GetObjMachinePos(int nNozzleIndex)
        {
            if (nNozzleIndex > NozzleMgr.GetInstance().nozzleArr.Length || nNozzleIndex < 0)
                return new XYUPoint();

             return   NozzleMgr.GetInstance().nozzleArr[nNozzleIndex - 1].ObjMachinePos;
            
        }
        public void SetDstMachinePos(int nNozzleIndex, double dDstx, double dDsty, double dDstu,double Z=0)
        {
            if (eventUpdatNozzleData != null)
                eventUpdatNozzleData(nNozzleIndex, "SetDstMachinePos", dDstx, dDsty, dDstu);
            NozzleMgr.GetInstance().nozzleArr[nNozzleIndex - 1].DstMachinePos = new XYUZPoint()
            {
                x = dDstx,
                y = dDsty,
                u = dDstu,
                z = Z,
            };
        }
        public XYUZPoint GetDstMachinePos(int nNozzleIndex)
        {
            if (nNozzleIndex > NozzleMgr.GetInstance().nozzleArr.Length || nNozzleIndex < 0)
                return new XYUZPoint();

            return NozzleMgr.GetInstance().nozzleArr[nNozzleIndex - 1].DstMachinePos;

        }

        public void ResetAllNozzleNone()
        {
            int i = 0;
           for ( int  index=1;index<= nozzleArr.Length;index++)
            {
                SetNozzleState(index, NozzleState.None);
            }
            for (int index = 1; index <= nozzleArr.Length; index++)
            {
                SetSparepartTypeOnNozzle(index, Sparepart.None);
            }
            for (int index = 1; index <= nozzleArr.Length; index++)
            {
                SetObjMachinePos(index,0,0,0);
            }
            for (int index = 1; index <= nozzleArr.Length; index++)
            {
               SetDstMachinePos(index, 0, 0, 0);
            }
        }
        public void ResetNozzleNone( int nNozzleIndex)
        {
            SetNozzleState(nNozzleIndex, NozzleState.None);
            SetSparepartTypeOnNozzle(nNozzleIndex, Sparepart.None);
            SetObjMachinePos(nNozzleIndex, 0, 0, 0);
            SetDstMachinePos(nNozzleIndex, 0, 0, 0);
        }
    }
}
