
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
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using HalconLib;
using System.ComponentModel;

namespace VisionProcess
{
    public struct areaCenter 
    {
        public double area;
        public double row;
        public double col;
    };
   
    public class  BlobParam
     {
     
        public double WidthMax = 10000;
        public double WidthMin = 0;
        public List<double> ResultWidth = new List<double>();

        public double LenMax = 10000;
        public double LenMin = 0;
        public List<double> ResultLen = new List<double>();

        public double CircleMax = 1;
        public double CircleMin = 0;
        public List<double> ResultCircle = new List<double>();

        public double threshold = 120;
        public double thresholdMax = 255;
        public double thresholdMin = 0;

        public List<areaCenter> RuslutAreaCenter = new List<areaCenter>();

    }
    [Description("Blob")]
    public class VisionBlob : VisionSetpBase
    {
        public VisionBlob(string strStepName) :
            base(strStepName)
        {
            
            // Gain = visionShapParam.m_dGain;
            //  ExposureTime = visionShapParam.m_dExposureTime;
        }
        HObject Mode2dCodeSearch = null;
        HTuple Mode2dCode = null;
        public BlobParam blobParam = new BlobParam();
        public override void Save()
        {
            //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            //AccessXmlSerializer.ObjectToXml(strPath, blobParam);

            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            AccessJosnSerializer.ObjectToJson(strPath, this);
        }
        public override void Save(string strPath)
        {
            //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            // AccessXmlSerializer.ObjectToXml(strPath, blobParam);
            AccessJosnSerializer.ObjectToJson(strPath, this);
        }
        public override Object Read()
        {
            //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            //blobParam = (BlobParam)AccessXmlSerializer.XmlToObject(strPath, blobParam.GetType());

            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            VisionBlob  obj= (VisionBlob) AccessJosnSerializer.JsonToObject(strPath, this.GetType());
           
            GC.Collect();

            return blobParam;
        }
        public override Object Read(string strPath)
        {
           // string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            blobParam = (BlobParam)AccessXmlSerializer.XmlToObject(strPath, blobParam.GetType());

            GC.Collect();

            return blobParam;
        }

        public override bool Process_image(HObject ho_Image, VisionControl visionControl)
        {
            
            try
            {
              
            }
            catch( HalconException e)
            {
               
                return false;
            }
            finally
            {
                
               
            }
        
            return true;
        }
        public override bool GenObj(HObject image, VisionControl visionControl)
        {
            try
            {
           
            }
            catch (HalconException e)
            {
                MessageBox.Show(m_strStepName+"创建2CodeModle失败："+e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        
            return true;
        }
    }
}
