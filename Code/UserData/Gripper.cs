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
    public enum GripperState
    {
        None,
        Have,
        HaveOK,
        HaveNG,
        HaveUnKnow,
    }
    public enum GripperType
    {
       A,
       B,
     
    }

   

    public class GripperData
    {
        public string strBarCode2d = "";
        public string strBarCode1d = "";
        private GripperState _gripperState = GripperState.None;
        public GripperState gripperState
        {
            set
            {
                if(value== GripperState.None)
                {
                    strBarCode2d = "";
                    strBarCode1d = "";
                    TrayIndexFrom = 0;
                    TrayCellIndexFrom = 0;
                }
                _gripperState = value;
            }
            get
            {
                return _gripperState;
            }
        }

        public int TrayIndexFrom =0;//Barrel 来自哪个盘
        public int TrayCellIndexFrom =0;//Barrel 来自这个盘的哪个位置
    }

    public class GripperMgr
    {
        private GripperMgr()
        {
           
        }
        private static object obj = new object();
        private static GripperMgr pGripperMgr;

        public static GripperMgr GetInstance()
        {
            if (pGripperMgr == null)
            {
                lock (obj)
                {
                    if (pGripperMgr == null)
                    {
                        pGripperMgr = new GripperMgr();
                    }
                }
            }
            return pGripperMgr;
        }
        public GripperData[] GripperArr = new GripperData[2] { new GripperData(), new GripperData()};
                                                            
        public void SetGripperState(int index, GripperState gripperState)
        {
            if (index <= GripperArr.Length && index >= 1)
                GripperArr[index - 1].gripperState = gripperState;
        }
        public GripperState GetGripperState(int index)
        {
            return GripperArr[index - 1].gripperState;
        }
        public void SetFrom( int nGripperIndex,int TrayIndex, int nCellIndex)
        {
            GripperArr[nGripperIndex - 1].TrayCellIndexFrom = nCellIndex;
            GripperArr[nGripperIndex - 1].TrayIndexFrom = TrayIndex;
        }
        public int GetFromTrayIndex(int nGripperIndex)
        {
            return GripperArr[nGripperIndex - 1].TrayIndexFrom;
        }
        public int GetFromTrayCellIndex(int nGripperIndex)
        {
            return GripperArr[nGripperIndex - 1].TrayCellIndexFrom;
        }
    }

}