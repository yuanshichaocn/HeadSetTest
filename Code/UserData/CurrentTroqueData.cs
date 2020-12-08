using BaseDll;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace UserData
{
    public class RecordData
    {
        public Dictionary<double, double> dicCurrentTroque = new Dictionary<double, double>();
        public Dictionary<double, double> dicTroqueCurrent = new Dictionary<double, double>();
        public Dictionary<double, double> dicCurrentTroque2 = new Dictionary<double, double>();
        public Dictionary<double, double> dicTroqueCurrent2 = new Dictionary<double, double>();
    }

    public class CurrentTroqueData
    {
        private CurrentTroqueData()
        {
        }

        [JsonIgnore]
        private static CurrentTroqueData pData = null;

        [JsonIgnore]
        private static object lockobj = new object();

        public static CurrentTroqueData GetInstance()
        {
            if (pData == null)
            {
                lock (lockobj)
                {
                    pData = new CurrentTroqueData();
                }
            }
            return pData;
        }

        private RecordData recordData = new RecordData();

        public void Add(double dCurrent, double dTroque)
        {
            recordData.dicCurrentTroque.Add(dCurrent, dTroque);
            recordData.dicTroqueCurrent.Add(dTroque, dCurrent);
        }

        public void Clear()
        {
            recordData.dicCurrentTroque.Clear();
            recordData.dicTroqueCurrent.Clear();
        }

        public void trans()
        {
            List<double> listList = new List<double>();
            listList.Clear();
            bool bFind = false;
            foreach (var tem in recordData.dicCurrentTroque)
            {
                if (listList.Count == 0)
                {
                    listList.Add(tem.Key);
                    continue;
                }
                bFind = false;
                for (int i = 0; i < listList.Count; i++)
                {
                    if (tem.Key < listList[i])
                    {
                        listList.Insert(i, tem.Key);
                        bFind = true;
                        break;
                    }
                }
                if (!bFind)
                    listList.Add(tem.Key);
            }
            recordData.dicCurrentTroque2.Clear();
            for (int i = 0; i < listList.Count; i++)
                recordData.dicCurrentTroque2.Add(listList[i], recordData.dicCurrentTroque[listList[i]]);

            listList.Clear();
            foreach (var tem in recordData.dicTroqueCurrent)
            {
                if (listList.Count == 0)
                {
                    listList.Add(tem.Key);
                    continue;
                }
                bFind = false;
                for (int i = 0; i < listList.Count; i++)
                {
                    if (tem.Key < listList[i])
                    {
                        listList.Insert(i, tem.Key);
                        bFind = true;
                        break;
                    }
                }
                if (!bFind)
                    listList.Add(tem.Key);
            }
            recordData.dicTroqueCurrent2.Clear();
            for (int i = 0; i < listList.Count; i++)
                recordData.dicTroqueCurrent2.Add(listList[i], recordData.dicTroqueCurrent[listList[i]]);
        }

        public void Save(string strpath)
        {
            AccessJosnSerializer.ObjectToJson(@"E:\TorqueTest.json", recordData);
        }

        public object ReadObj()
        {
            object obj = AccessJosnSerializer.JsonToObject(@"E:\TorqueTest.json", typeof(RecordData));
            if (obj != null)
                recordData = (RecordData)obj;
            return obj;
        }

        public double GetTroqueByCurrent(double dCurrent)
        {
            ;
            int index = 0;
            foreach (var tem in recordData.dicCurrentTroque2)
            {
                if (tem.Key == dCurrent)
                    return tem.Value;
                if (tem.Key < dCurrent)
                    index++;
                if (tem.Key > dCurrent)
                {
                    if (index == 0)
                        return -1;
                    double frontval = recordData.dicCurrentTroque2.ElementAt(index - 1).Value;
                    double backval = recordData.dicCurrentTroque2.ElementAt(index).Value;
                    double frontkey = recordData.dicCurrentTroque2.ElementAt(index - 1).Key;
                    double backkey = recordData.dicCurrentTroque2.ElementAt(index).Key;
                    double dmidval = frontval + (dCurrent - frontkey) * (backval - frontval) / (backkey - frontkey);
                    return dmidval;
                }
            }

            return -1;
        }

        public double GetCurrentByTroque(double dTroque)
        {
            int index = 0;
            foreach (var tem in recordData.dicTroqueCurrent2)
            {
                if (tem.Key == dTroque)
                    return tem.Value;
                if (tem.Key < dTroque)
                    index++;
                if (tem.Key > dTroque)
                {
                    if (index == 0)
                        return -1;
                    double frontval = recordData.dicTroqueCurrent2.ElementAt(index - 1).Value;
                    double backval = recordData.dicTroqueCurrent2.ElementAt(index).Value;
                    double frontkey = recordData.dicTroqueCurrent2.ElementAt(index - 1).Key;
                    double backkey = recordData.dicTroqueCurrent2.ElementAt(index).Key;
                    double dmidval = frontval + (dTroque - frontkey) * (backval - frontval) / (backkey - frontkey);
                    return dmidval;
                }
            }

            //IEnumerable<KeyValuePair<double,double>>lis= recordData.dicCurrentTroque2.((t) => { if( t.Value< dTroque)
            //        return t; });
            return -1;
        }
    }
}