//using CameraLib;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserCtrl;
using BaseDll;
using SerialDict;
using System.Reflection;
using System.IO;

namespace VisionProcess
{

    public class StepVisionInfo
    {
        public CamParam CamParam = new CamParam();
        public string VisionType;
        public int nLightVal = 100;
    }

    public class VisionMgr
    {
        private static VisionMgr visionMgr;
        private static object objlock = new object();
        private VisionMgr()
        {

        }
        public string CurrentVisionProcessDir
        {
            set;
            get;
        }
        public static VisionMgr GetInstance()
        {
            if(visionMgr==null)
            {
                lock(objlock)
                {
                    if(visionMgr == null)
                        visionMgr = new VisionMgr();
                }
            }
            return visionMgr;

        }
        public bool ProcessImage(string StepName,HObject img, VisionControl visionControl)
        {
            if (dicVision.ContainsKey(StepName))
            {
                return dicVision[StepName].Process_image(img, visionControl);
            }
            else
                return false;
        }
        public double? GetExpourseTime(string StepName)
        {
            if (dicVisionType.ContainsKey(StepName))
            {
                return dicVisionType[StepName].CamParam.m_dExposureTime;
            }
            else
                return null;
        }
        public double? GetGain(string StepName)
        {
            if (dicVisionType.ContainsKey(StepName))
            {
                return dicVisionType[StepName].CamParam.m_dGain;
            }
            else
                return null;
        }
        public string GetCamName(string StepName)
        {
            if (dicVisionType.ContainsKey(StepName))
            {
                return dicVisionType[StepName].CamParam.m_strCamName;
            }
            else
                return "";
        }
        public int GetLightVal(string StepName)
        {
            int nLightVal = 100;
          if(dicVisionType.ContainsKey(StepName))
            {
                nLightVal = dicVisionType[StepName].nLightVal;
            }
            return nLightVal;
        }
        public void  SetLightVal(string StepName ,int nLight)
        {
            int nLightVal = 100;
            if (dicVisionType.ContainsKey(StepName))
            {
                dicVisionType[StepName].nLightVal = nLight;
            }
         
        }
        public int nPrCount
        {
            get
            {
                return dicVision.Count;
            }

        }

        public SerialDictionary<string, VisionSetpBase> dicVision = new SerialDictionary<string, VisionSetpBase>();
        public SerialDictionary<string, StepVisionInfo> dicVisionType = new SerialDictionary<string, StepVisionInfo>();
        
        public void Save(string StepName)
        {
            if(dicVision.ContainsKey(StepName))
            {
                dicVision[StepName].Save();
            }
        }
        public void Read(string StepName)
        {
            if (dicVision.ContainsKey(StepName))
            {
                dicVision[StepName].Read();
            }
        }
        public delegate void PrItemChangedHandle(string itemname);
        public event PrItemChangedHandle PrItemChangedEvent;

        public void Add(string strName, VisionSetpBase visionSetpBase,StepVisionInfo stepVisionInfo)
        {
            if(dicVisionType.ContainsKey(strName) )
            {
                dicVisionType[strName] = stepVisionInfo;
            }
            else
            {
                dicVisionType.Add(strName, stepVisionInfo);
            }
            if (dicVision.ContainsKey(strName))
                dicVision[strName] = visionSetpBase;
            else
                dicVision.Add(strName, visionSetpBase);
            dicVision[strName].m_camparam = stepVisionInfo.CamParam;
            if (PrItemChangedEvent != null)
                PrItemChangedEvent(strName);
        }
        public void Add(string strName, VisionSetpBase visionSetpBase)
        {
       
            if (dicVision.ContainsKey(strName))
                dicVision[strName] = visionSetpBase;
            else
                dicVision.Add(strName, visionSetpBase);
            
        }
        public object GetResult(string visionStepName)
        {
            if (dicVision.ContainsKey(visionStepName))
                return dicVision[visionStepName].GetResult();
            else
                return null;
        }
        public void Clear()
        {
            dicVision.Clear();
            dicVisionType.Clear();
        }
        public  void Save()
        {
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "VisionMgr" + ".xml";
            dicVisionType.Clear();
           
            foreach ( var temp in  dicVision)
            {
                StepVisionInfo stepVisionInfo = new StepVisionInfo();
                stepVisionInfo.VisionType = temp.Value.GetType().ToString();
                stepVisionInfo.CamParam = temp.Value.m_camparam;
                dicVisionType.Add(temp.Key, stepVisionInfo);
                dicVision[temp.Key].Save();
                if (PrItemChangedEvent != null)
                    PrItemChangedEvent(temp.Key);
            }
            AccessXmlSerializer.ObjectToXml(strPath, dicVisionType);
           
        }
        public void DelItem( string strItem)
        {
            if(dicVisionType.ContainsKey(strItem))
            {
                dicVisionType.Remove(strItem);
            }
            if(dicVision.ContainsKey(strItem))
            {
                dicVision[strItem].Delete();
                dicVision.Remove(strItem);
            }
            if (PrItemChangedEvent != null)
                PrItemChangedEvent(strItem);
        }
  
        public void Read()
        {
            string strVisionConfigPath = VisionMgr.GetInstance().CurrentVisionProcessDir+ "VisionMgr" + ".xml";
            if (VisionMgr.GetInstance().CurrentVisionProcessDir == null &&
                ParamSetMgr.GetInstance().CurrentProductFile == null &&
                 ParamSetMgr.GetInstance().CurrentProductFile == ""&&
                 !Directory.Exists(VisionMgr.GetInstance().CurrentVisionProcessDir)   )
            {
                Directory.CreateDirectory(VisionMgr.GetInstance().CurrentVisionProcessDir);
            }
            dicVisionType.Clear();
            dicVisionType= (SerialDictionary<string, StepVisionInfo>)AccessXmlSerializer.XmlToObject(strVisionConfigPath, dicVisionType.GetType());

            dicVision.Clear();
            if (dicVisionType == null)
                dicVisionType = new SerialDictionary<string, StepVisionInfo>();
            foreach ( var temp  in dicVisionType)
            {
                Type type = AssemblyOperate.GetTypeFromAssembly(temp.Value.VisionType);
                object[] args = new object[]
               {
                 temp.Key
               };
                Add(temp.Key, Activator.CreateInstance(type, args) as VisionSetpBase);
                dicVision[temp.Key].Read();
                dicVision[temp.Key].m_camparam = temp.Value.CamParam;
            }

        }

        public Dictionary<string, StepVisionInfo> GetItemNamesAndTypes()
        {
            return dicVisionType;
        }
        public VisionSetpBase GetItem(string itemName)
        {
            if (dicVision.ContainsKey(itemName))
                return dicVision[itemName];
            return null;
        }
        public  void ClearResult(string itemName)
        {
            if(dicVision.ContainsKey(itemName))
                 dicVision[itemName].ClearResult();
        }


      
    }


}
