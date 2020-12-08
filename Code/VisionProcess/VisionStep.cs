//using CameraLib;
using BaseDll;
using HalconDotNet;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using UserCtrl;

namespace VisionProcess
{
    public class CamParam
    {
        public double m_dExposureTime = 10000;
        public double m_dGain = 1;
        public string m_strCamName = "";
    }

    public class VisionSetpBase
    {
        public VisionSetpBase(string strStepName)
        {
            m_strStepName = strStepName;
            strClassType = this.GetType().ToString();
            _logger = LogManager.GetLogger(this.GetType().Name);
        }

        protected ILog _logger = null;

        public void BindWindow(VisionControl vc)
        {
            m_Wnd = vc;
        }

        //[XmlElement("StepName")]
        public string m_strStepName = "";

        public string strClassType;

        public CamParam m_camparam = new CamParam();

        // [XmlIgnore]
        // protected CameraBase m_Cam= null;
        [XmlIgnore]
        [JsonIgnore]
        protected VisionControl m_Wnd = null;

        [XmlIgnore]
        [JsonIgnore]
        protected HObject m_image = null;

        [XmlIgnore]
        [JsonIgnore]
        protected HObject RectSeach = null;

        [XmlIgnore]
        [JsonIgnore]
        public Action GenFinished = null;

        public string strSavePath = "";

        [XmlIgnore]
        [JsonIgnore]
        public VisionBaseCtr visionCtr = null;

        public virtual void Disopose()
        {
        }

        public virtual VisionSetpBase Clone()
        {
            return null;
        }

        public void FlushToDlg(VisionControl visionControl, Control Father = null)
        {
            if (Father != null && Father.Controls != null && !Father.Controls.Contains(visionCtr))
                Father.Controls.Add(visionCtr);
            if (visionCtr != null)
            {
                visionCtr?.FlushToDlg(this, visionControl, Father);
                visionCtr.Location = new System.Drawing.Point(1, -1);
                visionCtr?.Show();
            }
        }

        public void SaveParm()
        {
            if (visionCtr != null)
            {
                visionCtr.SaveParm(this);
            }
        }

        public virtual void Save()
        {
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            AccessXmlSerializer.ObjectToXml(strPath, this);
            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            AccessJosnSerializer.ObjectToJson(strPath, this);
        }

        public virtual void Save(string strPath)
        {
            //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            AccessXmlSerializer.ObjectToXml(strPath, this);
            // AccessJosnSerializer.ObjectToJson(strPath, this);
        }

        public virtual Object Read()
        {
            string strPath = "";
            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            AccessJosnSerializer.ObjectToJson(strPath, this);
            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            return AccessXmlSerializer.XmlToObject(strPath, this.GetType());
        }

        public virtual Object Read(string strPath)
        {
            //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            return AccessXmlSerializer.XmlToObject(strPath, this.GetType());
        }

        public void Delete()
        {
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + m_strStepName;
            if (Directory.Exists(strPath))
            {
                FileOpert.DeleteFolder(strPath);
            }
        }

        public void Disp(HObject image)
        {
            if (null == m_Wnd)
                return;
            m_Wnd.DispImageFull(image);
        }

        public virtual bool GenObj(HObject image, VisionControl visionControl)
        {
            return true;
        }

        public virtual bool GenObj(HObject image, string path, VisionControl visionControl)
        {
            return true;
        }

        public virtual bool Process_image(HObject obj, VisionControl visionControl)
        {
            return true;
        }

        public virtual object GetResult()
        {
            return null;
        }

        public virtual void ClearResult()
        {
        }

        public virtual List<shapeparam> GetRegionOuts()
        {
            return null;
        }
    }
}