using BaseDll;
using System;
using System.IO;

namespace UserData
{
    public class CalcNum
    {
        private CalcNum()
        {
        }

        public static CalcNum GetInstance()
        {
            if (calcNum == null)
            {
                lock (objlock)
                {
                    if (calcNum == null)
                    {
                        calcNum = new CalcNum();
                    }
                }
            }
            return calcNum;
        }

        public double nTotalProductNum = 0;
        public double nTotalNgNum = 0;
        public double nTotalOKNum = 0;
        public double nVHBNGNum = 0;
        public double nBarCodeNum = 0;
        public double dRateOk = 1;
        private static CalcNum calcNum;
        private static object objlock = new object();
        private static object objlockSave = new object();

        public void Read()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "calcNum.xml";
            if (File.Exists(strPath))
                calcNum = (CalcNum)AccessXmlSerializer.XmlToObject(strPath, typeof(CalcNum));
            else
                Save();
        }

        public void Save()
        {
            lock (objlockSave)
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "calcNum.xml";
                if (calcNum != null)
                    AccessXmlSerializer.ObjectToXml(strPath, calcNum);
            }
        }
    }
}