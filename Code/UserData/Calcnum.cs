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
        static CalcNum calcNum;
        static object objlock = new object();
        static object objlockSave = new object();
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