//using CameraLib;
using BaseDll;
using HalconDotNet;
using HalconLib;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using UserCtrl;

namespace VisionProcess
{
    public interface IOperateParam
    {
        void ClearResult();

        int GetResultNum();

        object Clone();
    }

    public class Code1dParam : IOperateParam
    {
        public string Code1dSystem = "EAN-8";
        public string Mode1dcodeSearchPath = "";
        public string RusultCode = "";

        public void ClearResult()
        {
            RusultCode = "";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int GetResultNum()
        {
            if (RusultCode != null && RusultCode != "")
                return 1;
            else
                return 0;
        }
    }

    [Description("1维码")]
    public class Vision1dCode : VisionSetpBase
    {
        public Vision1dCode(string strStepName) :
            base(strStepName)
        {
            visionCtr = ctr;
        }

        [JsonIgnore]
        private static Vision1BarCodeSetCtr ctr = new Vision1BarCodeSetCtr();

        public override void Disopose()
        {
            if (Mode1dCode != null)
                HOperatorSet.ClearBarCodeModel(Mode1dCode);
            if (Mode1dCodeSearch != null && Mode1dCodeSearch.IsInitialized())
                Mode1dCodeSearch.Dispose();
        }

        [JsonIgnore]
        private HObject Mode1dCodeSearch = null;

        [JsonIgnore]
        private HTuple Mode1dCode = null;

        public Code1dParam vision1dCodeParam = new Code1dParam();

        public override void Save()
        {
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            // AccessXmlSerializer.ObjectToXml(strPath, vision1dCodeParam);
            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            AccessJosnSerializer.ObjectToJson(strPath, this);
        }

        public override void Save(string strPath)
        {
            //AccessXmlSerializer.ObjectToXml(strPath, vision1dCodeParam);
            AccessJosnSerializer.ObjectToJson(strPath, this);
        }

        public override Object Read()
        {
            Code1dParam tempvision1dCodeParam = null;
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            // tempvision1dCodeParam = (Code1dParam)AccessXmlSerializer.XmlToObject(strPath, vision1dCodeParam.GetType());
            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            Vision1dCode vision1DCode = (Vision1dCode)AccessJosnSerializer.JsonToObject(strPath, this.GetType());
            if (vision1DCode == null || vision1DCode.vision1dCodeParam == null)
            {
                _logger.Warn(m_strStepName + ": 视觉处理项目加载失败，请检查");
                MessageBox.Show(m_strStepName + ": 视觉处理项目加载失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            vision1dCodeParam = tempvision1dCodeParam = vision1DCode.vision1dCodeParam;
            string Mode1dcodePath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_bar_code_model.bcm";

            if (Mode1dcodePath != null && File.Exists(Mode1dcodePath))
            {
                if (Mode1dCode != null)
                    HOperatorSet.ClearBarCodeModel(Mode1dCode);
                HOperatorSet.ReadBarCodeModel(Mode1dcodePath, out Mode1dCode);
            }

            if (File.Exists(vision1dCodeParam.Mode1dcodeSearchPath))
            {
                if (Mode1dCodeSearch != null && Mode1dCodeSearch.IsInitialized())
                    Mode1dCodeSearch.Dispose();
                HOperatorSet.ReadRegion(out Mode1dCodeSearch, vision1dCodeParam.Mode1dcodeSearchPath);
            }
            GC.Collect();

            return vision1dCodeParam;
        }

        public override Object Read(string strPath)
        {
            Code1dParam tempvision1dCodeParam = (Code1dParam)AccessXmlSerializer.XmlToObject(strPath, vision1dCodeParam.GetType());
            if (tempvision1dCodeParam == null)
            {
                _logger.Warn(m_strStepName + ": 视觉处理项目加载失败，请检查");
                MessageBox.Show(m_strStepName + ": 视觉处理项目加载失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            vision1dCodeParam = tempvision1dCodeParam;
            string Mode1dcodePath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_bar_code_model.bcm"; ;
            if (Mode1dcodePath != null && File.Exists(Mode1dcodePath))
            {
                if (Mode1dCode != null)
                    HOperatorSet.ClearBarCodeModel(Mode1dCode);
                HOperatorSet.ReadBarCodeModel(Mode1dcodePath, out Mode1dCode);
            }
            if (File.Exists(vision1dCodeParam.Mode1dcodeSearchPath))
            {
                if (Mode1dCodeSearch != null && Mode1dCodeSearch.IsInitialized())
                    Mode1dCodeSearch.Dispose();
                HOperatorSet.ReadRegion(out Mode1dCodeSearch, vision1dCodeParam.Mode1dcodeSearchPath);
            }
            GC.Collect();

            return vision1dCodeParam;
        }

        public override bool Process_image(HObject ho_Image, VisionControl visionControl)
        {
            if (Mode1dCode == null)
            {
                MessageBox.Show(m_strStepName + "1CodeModle不存在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (visionControl == null)
            {
                MessageBox.Show(m_strStepName + "显示窗体不存在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (ho_Image == null || !ho_Image.IsInitialized())
            {
                MessageBox.Show(m_strStepName + "图片不存在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            HTuple hv_DataCodeHandleHigh = null;
            HTuple hv_ResultHandles = new HTuple();
            HTuple hv_DecodedDataStrings = new HTuple();
            HObject ho_SymbolRegions = null;
            HObject ReduceImg = null;
            try
            {
                if (visionControl != null && visionControl.isOpen())
                    HOperatorSet.SetDraw(visionControl.GetHalconWindow(), "margin");
                if (Mode1dCodeSearch != null && Mode1dCodeSearch.IsInitialized())
                    HOperatorSet.ReduceDomain(ho_Image, Mode1dCodeSearch, out ReduceImg);
                //HOperatorSet.SetBarCodeParam(Mode1dCode, "meas_thresh_abs", 0.0);
                // HOperatorSet.SetBarCodeParam(Mode1dCode, "element_size_min", 1.0);
                // HOperatorSet.SetBarCodeParam(Mode1dCode, "meas_param_estimation", "true");
                if (ReduceImg != null && ReduceImg.IsInitialized())
                    HOperatorSet.FindBarCode(ReduceImg, out ho_SymbolRegions, Mode1dCode,
                                "auto", out hv_DecodedDataStrings);
                else
                    HOperatorSet.FindBarCode(ho_Image, out ho_SymbolRegions, Mode1dCode,
                                  "auto", out hv_DecodedDataStrings);
                if (hv_DecodedDataStrings != null && hv_DecodedDataStrings.Length > 0)
                {
                    if (visionControl != null && visionControl.isOpen())
                    {
                        HOperatorSet.SetColor(visionControl.GetHalconWindow(), "green");
                        HOperatorSet.DispObj(ho_SymbolRegions, visionControl.GetHalconWindow());
                        HalconExternFunExport.disp_message(visionControl.GetHalconWindow(), hv_DecodedDataStrings, "window", 100, 200, "green", "false");
                        vision1dCodeParam.RusultCode = hv_DecodedDataStrings.S;
                        if (Mode1dCodeSearch != null && Mode1dCodeSearch.IsInitialized())
                            HOperatorSet.DispObj(Mode1dCodeSearch, visionControl.GetHalconWindow());
                    }
                }
                else
                {
                    if (visionControl != null && visionControl.isOpen())
                    {
                        HOperatorSet.SetColor(visionControl.GetHalconWindow(), "red");
                        if (Mode1dCodeSearch != null && Mode1dCodeSearch.IsInitialized())
                            HOperatorSet.DispObj(Mode1dCodeSearch, visionControl.GetHalconWindow());
                        HalconExternFunExport.disp_message(visionControl.GetHalconWindow(), "读码失败", "window", 100, 200, "red", "false");
                    }
                }
            }
            catch (HalconException e)
            {
                // MessageBox.Show(m_strStepName + "图片不存在", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                ReduceImg?.Dispose();
                ho_SymbolRegions?.Dispose();
                // GC.SuppressFinalize(ReduceImg);
            }

            return true;
        }

        public override bool GenObj(HObject image, VisionControl visionControl)
        {
            try
            {
                HTuple hv_BarCodeModel = null;
                if (Mode1dCode != null)
                    HOperatorSet.ClearBarCodeModel(Mode1dCode);

                HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out hv_BarCodeModel);
                Mode1dCode = hv_BarCodeModel;

                string Mode1dcodePath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_bar_code_model.bcm";
                HOperatorSet.WriteBarCodeModel(Mode1dCode, Mode1dcodePath);

                string imgpath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".bmp";
                if (image != null && image.IsInitialized())
                    HOperatorSet.WriteImage(image, "bmp", 0, imgpath);
            }
            catch (HalconException e)
            {
                MessageBox.Show(m_strStepName + "创建1CodeModle失败：" + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public override object GetResult()
        {
            return vision1dCodeParam;
        }

        public override void ClearResult()
        {
            vision1dCodeParam.RusultCode = "";
        }
    }
}