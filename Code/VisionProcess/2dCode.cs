
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
    public class  VisionCode2dParam : IOperateParam

    {
        public string Code2dSystem= "QR Code";
        public string ContrastTolerance="";
      
        public string Mode2dcodeSearchPath="";
        public string RusultCode = "";
        public void ClearResult()
        {
            RusultCode = "";
        }
        public int GetResultNum()
        {
            if (RusultCode != null)
                return 1;
            else
                return 0;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
    [Description("2维码")]
    public class Vision2dCode : VisionSetpBase
    {
        
        public Vision2dCode(string strStepName) :
            base(strStepName)
        {
           
            // Gain = visionShapParam.m_dGain;
            //  ExposureTime = visionShapParam.m_dExposureTime;
            visionCtr = ctr;
        }
        static Vision2dCodeSetCtr ctr = new Vision2dCodeSetCtr();
        public override void Disopose()
        {
            if (Mode2dCode != null)
                HOperatorSet.ClearBarCodeModel(Mode2dCode);
            if (Mode2dCodeSearch != null && Mode2dCodeSearch.IsInitialized())
                Mode2dCodeSearch.Dispose();
        }
        HObject Mode2dCodeSearch = null;
        HTuple Mode2dCode = null;
        public VisionCode2dParam vision2dCodeParam = new VisionCode2dParam();
        public override void Save()
        {
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            //AccessXmlSerializer.ObjectToXml(strPath, vision2dCodeParam);
            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            AccessJosnSerializer.ObjectToJson(strPath, this);
        }
        public override void Save(string strPath)
        {

            //AccessXmlSerializer.ObjectToXml(strPath, vision2dCodeParam);
            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            AccessJosnSerializer.ObjectToJson(strPath, this);
        }
        public override Object Read()
        {
            VisionCode2dParam tempvision2dCodeParam = null;
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
             //tempvision2dCodeParam = (VisionCode2dParam)AccessXmlSerializer.XmlToObject(strPath, vision2dCodeParam.GetType());
            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            Vision2dCode vision2DCode = (Vision2dCode)AccessJosnSerializer.JsonToObject(strPath, this.GetType());
            if (vision2DCode == null || vision2DCode.vision2dCodeParam== null)
            {
                _logger.Warn(m_strStepName + ": 视觉处理项目加载失败，请检查");
                MessageBox.Show(m_strStepName + ": 视觉处理项目加载失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            vision2dCodeParam=tempvision2dCodeParam = vision2DCode.vision2dCodeParam;
            
            string  Mode2dcodePath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_data_code_model.dcm"; ;
            if (vision2dCodeParam!= null && Mode2dcodePath!=null && File.Exists(Mode2dcodePath))
            {
                if (Mode2dCode != null)
                    HOperatorSet.ClearDataCode2dModel(Mode2dCode);
                Mode2dCode = null;
                HOperatorSet.ReadDataCode2dModel(Mode2dcodePath, out Mode2dCode);
                if(Mode2dCode== null || Mode2dCode.Length<=0)
                {
                    _logger.Warn(m_strStepName + ": 2维码模板读取失败，请检查");
                    MessageBox.Show(m_strStepName + ": 2维码模板读取失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
               // MessageBox.Show(m_strStepName + ": 2维码模板读取失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
          
            if(vision2dCodeParam != null && File.Exists(vision2dCodeParam.Mode2dcodeSearchPath) )
            {
                if (Mode2dCodeSearch != null && Mode2dCodeSearch.IsInitialized())
                    Mode2dCodeSearch.Dispose();
                HOperatorSet.ReadRegion(out Mode2dCodeSearch, vision2dCodeParam.Mode2dcodeSearchPath);
                if(Mode2dCodeSearch ==null || Mode2dCodeSearch.IsInitialized())
                {
                    _logger.Warn(m_strStepName + ": 2维码搜索区域读取失败，请检查");
                }
                  //  MessageBox.Show(m_strStepName + ": 2维码搜索区域读取失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _logger.Warn(m_strStepName + ": 2维码搜索区域读取失败，请检查");
                //  MessageBox.Show(m_strStepName + ": 2维码搜索区域读取失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            GC.Collect();

            return vision2dCodeParam;
        }
        public override Object Read(string strPath)
        {
           // string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            VisionCode2dParam tempvision2dCodeParam = (VisionCode2dParam)AccessXmlSerializer.XmlToObject(strPath, vision2dCodeParam.GetType());
            if (tempvision2dCodeParam == null)
            {
                _logger.Warn(m_strStepName + ": 视觉处理项目加载失败，请检查");
                MessageBox.Show(m_strStepName + ": 视觉处理项目加载失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            vision2dCodeParam = tempvision2dCodeParam;
            string Mode2dcodePath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_data_code_model.dcm"; ;
            if (vision2dCodeParam != null && Mode2dcodePath != null && File.Exists(Mode2dcodePath))
            {
                if (Mode2dCode != null)
                    HOperatorSet.ClearDataCode2dModel(Mode2dCode);
                Mode2dCode = null;
                HOperatorSet.ReadDataCode2dModel(Mode2dcodePath, out Mode2dCode);
                if (Mode2dCode == null || Mode2dCode.Length <= 0)
                {
                    _logger.Warn(m_strStepName + ": 2维码模板读取失败，请检查");
                    MessageBox.Show(m_strStepName + ": 2维码模板读取失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // MessageBox.Show(m_strStepName + ": 2维码模板读取失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          

            if (vision2dCodeParam != null && File.Exists(vision2dCodeParam.Mode2dcodeSearchPath))
            {
                if (Mode2dCodeSearch != null && Mode2dCodeSearch.IsInitialized())
                    Mode2dCodeSearch.Dispose();
                HOperatorSet.ReadRegion(out Mode2dCodeSearch, vision2dCodeParam.Mode2dcodeSearchPath);
                if (Mode2dCodeSearch == null || Mode2dCodeSearch.IsInitialized())
                {
                    _logger.Warn(m_strStepName + ": 2维码搜索区域读取失败，请检查");
                }
                //  MessageBox.Show(m_strStepName + ": 2维码搜索区域读取失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _logger.Warn(m_strStepName + ": 2维码搜索区域读取失败，请检查");
                //  MessageBox.Show(m_strStepName + ": 2维码搜索区域读取失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            GC.Collect();

            return vision2dCodeParam;
        }
        public override bool Process_image(HObject ho_Image, VisionControl visionControl)
        {
            if(Mode2dCode==null)
            {
                MessageBox.Show(m_strStepName + "2CodeModle不存在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Warn(m_strStepName + "2CodeModle不存在");
                return false;
            }
            if(visionControl==null)
            {
                MessageBox.Show(m_strStepName + "显示窗体不存在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Warn(m_strStepName + "显示窗体不存在");
                return false;
            }
            if(ho_Image== null || !ho_Image.IsInitialized())
            {
                MessageBox.Show(m_strStepName + "图片不存在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Warn(m_strStepName + "图片不存在");
                return false;
            }
            HTuple hv_DataCodeHandleHigh = null;
            HTuple  hv_ResultHandles = new HTuple();
            HTuple hv_DecodedDataStrings = new HTuple();
            HObject ho_SymbolXLDs = null;
            HObject ReduceImg = null;
            try
            {
                if(visionControl!=null && visionControl.isOpen())
                    HOperatorSet.SetDraw(visionControl.GetHalconWindow(), "margin");
                if (Mode2dCodeSearch != null && Mode2dCodeSearch.IsInitialized())
                    HOperatorSet.ReduceDomain(ho_Image, Mode2dCodeSearch, out ReduceImg);
                if(ReduceImg!=null && ReduceImg.IsInitialized())
                    HOperatorSet.FindDataCode2d(ReduceImg, out ho_SymbolXLDs, Mode2dCode,
                        new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
                else
                    HOperatorSet.FindDataCode2d(ho_Image, out ho_SymbolXLDs, Mode2dCode,
                        new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
                if(hv_DecodedDataStrings!=null && hv_DecodedDataStrings.Length>0)
                {
                    if (visionControl != null && visionControl.isOpen())
                    {
                        HOperatorSet.SetColor(visionControl.GetHalconWindow(), "green");
                        HOperatorSet.DispObj(ho_SymbolXLDs, visionControl.GetHalconWindow());
                        HalconExternFunExport.disp_message(visionControl.GetHalconWindow(),hv_DecodedDataStrings, "window", 100, 200, "green", "false"); 
                        vision2dCodeParam.RusultCode = hv_DecodedDataStrings.S;
                        if (Mode2dCodeSearch != null && Mode2dCodeSearch.IsInitialized())
                            HOperatorSet.DispObj(Mode2dCodeSearch, visionControl.GetHalconWindow());
                    }
                }
                else
                {
                    if (visionControl != null && visionControl.isOpen())
                    {
                        HOperatorSet.SetColor(visionControl.GetHalconWindow(), "red");
                        if (Mode2dCodeSearch != null && Mode2dCodeSearch.IsInitialized())
                            HOperatorSet.DispObj(Mode2dCodeSearch, visionControl.GetHalconWindow());
                        HalconExternFunExport.disp_message(visionControl.GetHalconWindow(), "读码失败", "window", 100, 200, "red", "false");
                    }
                }
            }
            catch( HalconException e)
            {
               // MessageBox.Show(m_strStepName + "图片不存在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                ReduceImg?.Dispose();
                ho_SymbolXLDs?.Dispose();
               // GC.SuppressFinalize(ReduceImg);
            }
        
            return true;
        }
        public override bool GenObj(HObject image, VisionControl visionControl)
        {
            try
            {
                if (Mode2dCode != null)
                    HOperatorSet.ClearDataCode2dModel(Mode2dCode);
                HTuple hv_DataCodeHandleLow = null;
                HOperatorSet.CreateDataCode2dModel(vision2dCodeParam.Code2dSystem, "default_parameters",     "standard_recognition", out hv_DataCodeHandleLow);
          
              
                Mode2dCode = hv_DataCodeHandleLow;

                string Mode2dcodePath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_data_code_model.dcm";
                HOperatorSet.WriteDataCode2dModel(Mode2dCode, Mode2dcodePath);

                string imgpath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".bmp";
                if (image != null && image.IsInitialized())
                    HOperatorSet.WriteImage(image, "bmp", 0, imgpath);
            }
            catch (HalconException e)
            {
                MessageBox.Show(m_strStepName+"创建2CodeModle失败："+e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        
            return true;
        }
        public override object GetResult()
        {
            return vision2dCodeParam;
        }
        public override void ClearResult()
        {
            vision2dCodeParam.RusultCode = "";
        }
    }
}
