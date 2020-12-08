using MotionIoLib;
using System.Collections.Generic;

namespace UserData
{
    public enum TableStationState
    {
        Wait,//等待到位
        Processing,//处理中
        Processed,//处理完成
    }

    public class TableData
    {
        private TableData()
        {
        }

        private static TableData pTableData = null;
        private static object lockobj = new object();

        public static TableData GetInstance()
        {
            if (pTableData == null)
            {
                lock (lockobj)
                {
                    pTableData = new TableData();
                }
            }
            return pTableData;
        }

        public string TableName = "Table";
        public int NumStaionsBroundTable = 2;
        public int AxisNo = 0;
        private TableStationState[] tableStationStates = new TableStationState[20];
        public Dictionary<double, int> dicTable = new Dictionary<double, int>();

        /// <summary>
        /// 添加夹具号和对位的位置
        /// </summary>
        /// <param name="currentpos"></param>
        /// <param name="SocketNo"></param>
        public void Add(double currentpos, int SocketNo)
        {
            dicTable.Add(currentpos, SocketNo);
        }

        public void Clear()
        {
            dicTable.Clear();
        }

        /// <summary>
        /// 获取当工站的夹具号 nCurrentNo当前工站号(站号从1开始）
        /// </summary>
        /// <param name="nCurrentNo"></param>
        /// <returns></returns>
        public int GetSocketNum(int nCurrentNo, double Fine = 0.1)
        {
            if (MotionMgr.GetInstace().GetHomeFinishFlag(AxisNo) != AxisHomeFinishFlag.Homed)
                return -1;
            double val = MotionMgr.GetInstace().GetAxisPos(AxisNo);
            foreach (var temp in dicTable)
            {
                if (val < temp.Key + Fine && val > temp.Key - Fine)
                {
                    int No = (temp.Value + nCurrentNo - 1) % NumStaionsBroundTable;
                    if (No == 0)
                        return NumStaionsBroundTable;
                    else
                        return No;
                }
            }
            return -1;
        }
    }
}