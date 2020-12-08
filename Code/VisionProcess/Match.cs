//using CameraLib;
using BaseDll;
using HalconDotNet;
using HalconLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using UserCtrl;

namespace VisionProcess
{
    public enum eModleType
    {
        形状,
        灰度
    }

    public class VisionShapParam : IOperateParam
    {
        public string SeachRectRegionPath = "";
        public double RectSeachRow1 = 0, RectSeachCol1 = 0, RectSeachRow2 = 0, RectSeachCol2 = 0;
        public string ModeShmPath;//模板路径
        public double dSorce = 0.5;
        public int nNum = 1;
        public double AngleStart = 0;
        public double AngleExtent = 360;
        public double MaxOverlap = 0.5;
        public List<double> ResultRow = new List<double>(), ResultCol = new List<double>(), ResultAngle = new List<double>();
        public string ModeImgPath = "";//模板图片路径
        public double MaxRowScale = 1.1, MaxColScale = 1.1, MinRowScale = 0.9, MinColScale = 0.9;
        public string RoiRegionPath = "";
        public double ContrastHigh = 30, ContrastLow = 20, MinSize = 10;
        public bool bSetOutPoint = false;
        public int CratePyramid = 0, MatchPyamidHigh = 1, MatchPyamidLow = 9;
        public string strPolaritySel = "use_polarity";
        public XYUPoint ModlePoint = new XYUPoint(0, 0, 0);
        public Point2d OutPointInModleImage = new Point2d(0, 0);
        public List<Point2d> OutPointInResultImg = new List<Point2d>();
        // public List<ShapeNameType> RegionNames = new List<ShapeNameType>();

        public eModleType ModeType = eModleType.形状;

        public VisionShapParam()
        {
            ResultRow.Clear();
            ResultCol.Clear();
            ResultAngle.Clear();
        }

        public void ClearResult()
        {
            ResultRow.Clear();
            ResultCol.Clear();
            ResultAngle.Clear();
        }

        public int GetResultNum()
        {
            if (ResultRow != null && ResultRow.Count > 0)
                return ResultRow.Count;
            else
                return 0;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [Description("模板匹配")]
    public class VisionShapMatch : VisionSetpBase
    {
        public VisionShapMatch(string strStepName) :
            base(strStepName)
        {
            visionCtr = ctr;
        }

        private static VisionMatchSetCtr ctr = new VisionMatchSetCtr();

        [JsonIgnore]
        private HTuple ModeID = null;

        [JsonIgnore]
        private HObject RegionSearch = null, RegionRoi = null;

        public VisionShapParam visionShapParam = new VisionShapParam();

        public override void Disopose()
        {
            if (RegionSearch != null && RegionSearch.IsInitialized())
                RegionSearch.Dispose();
            if (RegionRoi != null && RegionRoi.IsInitialized())
                RegionRoi.Dispose();
            if (ModeID != null)
                HOperatorSet.ClearShapeModel(ModeID);
        }

        public override VisionSetpBase Clone()
        {
            VisionShapMatch visionShapMatch = new VisionShapMatch(m_strStepName);
            visionShapMatch.Read();
            return visionShapMatch;
        }

        public void SetRoiRegion(HObject obj)
        {
            RegionRoi = obj;
        }

        public List<shapeparam> shapeslist = new List<shapeparam>();
        public List<shapeparam> shapeparamResults = new List<shapeparam>();

        public override void Save()
        {
            //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
            //if (strSavePath != "")
            //    strPath = strSavePath + "\\" + m_strStepName + ".xml";
            //AccessXmlSerializer.ObjectToXml(strPath, visionShapParam);

            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
            if (strSavePath != "")
                strPath = strSavePath + "\\" + m_strStepName + ".json";
            AccessJosnSerializer.ObjectToJson(strPath, this);
            // SaveRegions();
        }

        public override void Save(string strPath)
        {
            string regionsavepath = strPath;
            //strPath = strPath + "\\" + m_strStepName + ".xml";
            //AccessXmlSerializer.ObjectToXml(strPath, visionShapParam);
            AccessJosnSerializer.ObjectToJson(strPath, this);
            //SaveRegions(regionsavepath);
        }

        public override Object Read()
        {
            try
            {
                VisionShapParam tempvisionShapParam = null;
                string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
                if (strSavePath != "")
                    strPath = strSavePath + "\\" + m_strStepName + ".xml";

                strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
                if (strSavePath != "")
                    strPath = strSavePath + "\\" + m_strStepName + ".json";

                object visionShapMatch2 = AccessJosnSerializer.JsonToObject(strPath, typeof(object));

                //string json = visionShapMatch2.ToString();
                //var q = JsonConvert.DeserializeObject<dynamic>(json);
                //Newtonsoft.Json.Linq.JObject jobj = Newtonsoft.Json.Linq.JObject.Parse(json);
                //string jsonShapelist= jobj["shapeslist"].ToString();
                //List<shapeparam> ss =  JsonConvert.DeserializeObject<List<shapeparam>>(jsonShapelist);
                //var qq = JsonConvert.DeserializeObject<dynamic>(jsonShapelist);
                //Newtonsoft.Json.Linq.JObject jobjShape = Newtonsoft.Json.Linq.JObject.Parse(qq.ToString());
                //string jsonShapelists = jobjShape["usrshape"].ToString();

                VisionShapMatch visionShapMatch = (VisionShapMatch)AccessJosnSerializer.JsonToObject(strPath, this.GetType());
                string str = visionShapMatch2.ToString();
                int indexFindex = str.IndexOf("\"shapeslist\":");
                string sub1 = str.Substring(indexFindex);
                int indexFirist = sub1.IndexOf("[");
                string sub2 = sub1.Substring(indexFirist);
                int indexLast = sub2.IndexOf("]");
                int nLen = indexLast - indexFirist;
                if (indexLast != -1 && indexFirist != -1 && nLen > 0)
                {
                    string sub = sub2.Substring(0, indexLast + 1);
                    string strReg = "<Item>";

                    List<object> list = (List<object>)JsonConvert.DeserializeObject(sub, typeof(List<object>));
                    // List<shapeparam> list = (List<shapeparam>)AccessJosnSerializer.JsonToObject(strPath,typeof( shapeparam));

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ToString() != null)
                        {
                            int index = list[i].ToString().IndexOf("\"usrshape\": ");
                            if (index != -1)
                            {
                                string Itemstring = list[i].ToString();
                                string subusrshape = list[i].ToString().Substring(index);
                                int index2 = subusrshape.IndexOf("{");
                                int index3 = subusrshape.IndexOf("}");

                                if (index2 != -1 && index3 != -1)
                                {
                                    int nLenOfUserShape = index3 - index2;

                                    string subusrshapeObj = subusrshape.Substring(index2 - 1, nLenOfUserShape + 2);

                                    Type TY = AssemblyOperate.GetTypeFromAssembly(visionShapMatch.shapeslist[i].usrshape.UserTypeName);
                                    UserShape tem = (UserShape)JsonConvert.DeserializeObject(subusrshapeObj, TY);
                                    visionShapMatch.shapeslist[i].usrshape = tem;
                                }
                            }
                        }
                    }
                }

                if (visionShapMatch == null || visionShapMatch.visionShapParam == null)
                {
                    MessageBox.Show(m_strStepName + ": 视觉处理项目加载失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _logger.Warn(m_strStepName + ": 视觉处理项目加载失败，请检查");
                    return null;
                }
                visionShapParam = tempvisionShapParam = visionShapMatch.visionShapParam;
                this.shapeslist.Clear();
                int nIndex = 0;
                for (int s = 0; s < visionShapMatch.shapeslist.Count; s++)
                {
                    shapeslist.Add(new shapeparam()
                    {
                        name = visionShapMatch.shapeslist[s].name,
                        shapeType = visionShapMatch.shapeslist[s].shapeType,
                        usrshape = visionShapMatch.shapeslist[s].usrshape.Clone(),
                    });
                }
                string ModeShmPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_Mode.shm";
                if (visionShapParam != null && visionShapParam.ModeType == eModleType.形状 && File.Exists(ModeShmPath))
                {
                    if (ModeID != null)
                        HOperatorSet.ClearShapeModel(ModeID);
                    ModeID = null;
                    HOperatorSet.ReadShapeModel(ModeShmPath, out ModeID);
                    if (ModeID == null || ModeID.Length <= 0)
                    {
                        MessageBox.Show(m_strStepName + "读取项目：" + "模板读取失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _logger.Warn(m_strStepName + "读取项目：" + "模板读取失败");
                    }
                }
                else
                {
                    _logger.Warn(m_strStepName + "读取项目：" + "模板读取失败");
                    MessageBox.Show(m_strStepName + "读取项目：" + "模板读取失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                string SeachRectRegionPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_SearchRect.hobj";
                if (visionShapParam != null && File.Exists(SeachRectRegionPath))
                {
                    if (RegionSearch != null && RegionSearch.IsInitialized())
                        RegionSearch.Dispose();
                    HOperatorSet.ReadRegion(out RegionSearch, SeachRectRegionPath);
                    if (RegionSearch == null || RegionSearch.IsInitialized())
                    {
                        _logger.Warn(m_strStepName + "读取项目：" + "搜索区域读取失败");
                    }
                }
                else
                {
                    _logger.Warn(m_strStepName + "读取项目：" + "搜索区域读取失败");
                }
                string RoiRegionPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_Roi.hobj";
                if (visionShapParam != null && File.Exists(RoiRegionPath))
                {
                    if (RegionRoi != null && RegionRoi.IsInitialized())
                        RegionRoi.Dispose();
                    HOperatorSet.ReadRegion(out RegionRoi, RoiRegionPath);
                    if (RegionRoi == null || RegionRoi.IsInitialized())
                    {
                        _logger.Warn(m_strStepName + "读取项目：" + "roi读取失败");
                    }
                }
                else
                {
                    _logger.Warn(m_strStepName + "读取项目：" + "roi读取失败");
                }
            }
            catch (Exception e1)
            {
                _logger.Warn(m_strStepName + "读取项目：" + e1.Message);
                MessageBox.Show(m_strStepName + "读取项目：" + e1.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                _logger.Warn(m_strStepName + "读取项目失败");
                MessageBox.Show(m_strStepName + "读取项目失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            GC.Collect();

            return visionShapParam;
        }

        public override Object Read(string strPath)
        {
            string strSaveRegions = strPath;
            try
            {
                strPath = strPath + "\\" + m_strStepName + ".xml";
                //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
                VisionShapParam tempvisionShapParam = (VisionShapParam)AccessXmlSerializer.XmlToObject(strPath, visionShapParam.GetType());
                if (tempvisionShapParam == null)
                {
                    MessageBox.Show(m_strStepName + ": 视觉处理项目加载失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _logger.Warn(m_strStepName + ": 视觉处理项目加载失败，请检查");
                    return null;
                }
                visionShapParam = tempvisionShapParam;

                if (visionShapParam != null && File.Exists(visionShapParam.ModeShmPath))
                {
                    if (ModeID != null)
                        HOperatorSet.ClearShapeModel(ModeID);
                    ModeID = null;
                    HOperatorSet.ReadShapeModel(visionShapParam.ModeShmPath, out ModeID);
                    if (ModeID == null || ModeID.Length <= 0)
                    {
                        MessageBox.Show(m_strStepName + "读取项目：" + "模板读取失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _logger.Warn(m_strStepName + "读取项目：" + "模板读取失败");
                    }
                }
                else
                {
                    _logger.Warn(m_strStepName + "读取项目：" + "模板读取失败");
                    MessageBox.Show(m_strStepName + "读取项目：" + "模板读取失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (visionShapParam != null && File.Exists(visionShapParam.SeachRectRegionPath))
                {
                    if (RegionSearch != null && RegionSearch.IsInitialized())
                        RegionSearch.Dispose();
                    HOperatorSet.ReadRegion(out RegionSearch, visionShapParam.SeachRectRegionPath);
                    if (RegionSearch == null || RegionSearch.IsInitialized())
                    {
                        _logger.Warn(m_strStepName + "读取项目：" + "搜索区域读取失败");
                    }
                    //     MessageBox.Show(m_strStepName + "读取项目：" + "搜索区域读取失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _logger.Warn(m_strStepName + "读取项目：" + "搜索区域读取失败");
                    // MessageBox.Show(m_strStepName + "读取项目：" + "搜索区域读取失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (visionShapParam != null && File.Exists(visionShapParam.RoiRegionPath))
                {
                    if (RegionRoi != null && RegionRoi.IsInitialized())
                        RegionRoi.Dispose();
                    HOperatorSet.ReadRegion(out RegionRoi, visionShapParam.RoiRegionPath);
                    if (RegionRoi == null || RegionRoi.IsInitialized())
                    {
                        _logger.Warn(m_strStepName + "读取项目：" + "roi读取失败");
                    }
                    //     MessageBox.Show(m_strStepName + "读取项目：" + "roi读取失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _logger.Warn(m_strStepName + "读取项目：" + "roi读取失败");
                    // MessageBox.Show(m_strStepName + "读取项目：" + "roi读取失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e1)
            {
                _logger.Warn(m_strStepName + "读取项目：" + e1.Message);
                MessageBox.Show(m_strStepName + "读取项目：" + e1.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                _logger.Warn(m_strStepName + "读取项目失败");
                MessageBox.Show(m_strStepName + "读取项目失败", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            GC.Collect();
            //  ReadRegions(strSaveRegions);
            return visionShapParam;
        }

        public override bool Process_image(HObject ho_Image, VisionControl visionControl)
        {
            if (ModeID == null)
            {
                MessageBox.Show(m_strStepName + ":模板不存在，请登录模板", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Warn(m_strStepName + ":模板不存在，请登录模板");
                return false;
            }
            if (ho_Image == null || !ho_Image.IsInitialized())
            {
                _logger.Warn(m_strStepName + ":匹配图片不存在，请读取或采集图片");
                MessageBox.Show(m_strStepName + ":匹配图片不存在，请读取或采集图片", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            HObject ReduceImg = null;
            try
            {
                visionShapParam.ResultRow.Clear();
                visionShapParam.ResultCol.Clear();
                visionShapParam.ResultAngle.Clear();
                visionShapParam.OutPointInResultImg.Clear();
                HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
                HTuple hv_Column2 = null, hv_Row = null;
                HTuple hv_Column = null, hv_Angle = null, hv_ScaleR = null;
                HTuple hv_ScaleC = null, hv_Score = null;

                // HOperatorSet.SetSystem("border_shape_models", "true");
                if (RegionSearch != null && RegionSearch.IsInitialized())
                    HOperatorSet.ReduceDomain(ho_Image, RegionSearch, out ReduceImg);

                if (visionShapParam.ModeType.ToString() == "形状")
                {
                    if (ReduceImg != null && ReduceImg.IsInitialized())
                        HOperatorSet.FindScaledShapeModel(ReduceImg, ModeID,
                            (new HTuple(visionShapParam.AngleStart)).TupleRad(), (new HTuple(visionShapParam.AngleExtent)).TupleRad(),
                            visionShapParam.MinRowScale, visionShapParam.MaxRowScale, visionShapParam.dSorce, visionShapParam.nNum, visionShapParam.MaxOverlap, "least_squares", (new HTuple(visionShapParam.MatchPyamidLow)).TupleConcat(new HTuple(visionShapParam.MatchPyamidHigh)), 0.7,
                            out hv_Row, out hv_Column, out hv_Angle, out hv_ScaleR, out hv_Score);
                    else
                        HOperatorSet.FindScaledShapeModel(ho_Image, ModeID,
                       (new HTuple(visionShapParam.AngleStart)).TupleRad(), (new HTuple(visionShapParam.AngleExtent)).TupleRad(),
                       visionShapParam.MinRowScale, visionShapParam.MaxRowScale, visionShapParam.dSorce, visionShapParam.nNum, visionShapParam.MaxOverlap, "least_squares", (new HTuple(visionShapParam.MatchPyamidLow)).TupleConcat(new HTuple(visionShapParam.MatchPyamidHigh)), 0.9,
                       out hv_Row, out hv_Column, out hv_Angle, out hv_ScaleR, out hv_Score);
                }
                else
                {
                    if (ReduceImg != null && ReduceImg.IsInitialized())
                    {
                        HOperatorSet.FindNccModel(ReduceImg, ModeID, (new HTuple(visionShapParam.AngleStart)).TupleRad(), (new HTuple(visionShapParam.AngleExtent)).TupleRad(),
                              visionShapParam.dSorce, visionShapParam.nNum, visionShapParam.MaxOverlap, "true", 0, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                    }
                    else
                    {
                        HOperatorSet.FindNccModel(ho_Image, ModeID, (new HTuple(visionShapParam.AngleStart)).TupleRad(), (new HTuple(visionShapParam.AngleExtent)).TupleRad(),
                           visionShapParam.dSorce, visionShapParam.nNum, visionShapParam.MaxOverlap, "true", 0, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                    }
                }

                if (hv_Row.Length > 0)
                {
                    for (int i = 0; i < hv_Row.Length; i++)
                    {
                        visionShapParam.ResultRow.Add(hv_Row[i].D);
                        visionShapParam.ResultCol.Add(hv_Column[i].D);
                        visionShapParam.ResultAngle.Add(hv_Angle[i].D);
                        HOperatorSet.VectorAngleToRigid(visionShapParam.ModlePoint.y, visionShapParam.ModlePoint.x, visionShapParam.ModlePoint.u,
                                hv_Row[i].D, hv_Column[i].D, hv_Angle[i].D, out HTuple hom2d);
                        if (visionShapParam.bSetOutPoint)
                        {
                            HOperatorSet.VectorAngleToRigid(visionShapParam.ModlePoint.y, visionShapParam.ModlePoint.x, visionShapParam.ModlePoint.u,
                                hv_Row[i].D, hv_Column[i].D, hv_Angle[i].D, out hom2d);

                            HOperatorSet.AffineTransPixel(hom2d, visionShapParam.OutPointInModleImage.y, visionShapParam.OutPointInModleImage.x, out HTuple rowTrans, out HTuple colTrans);
                            if (visionControl != null && visionControl.isOpen())
                            {
                                HOperatorSet.SetColor(visionControl.GetHalconWindow(), "blue");
                                HOperatorSet.DispCross(visionControl.GetHalconWindow(), rowTrans, colTrans, 80, 0);
                            }

                            visionShapParam.OutPointInResultImg.Add(new Point2d(colTrans.D, rowTrans.D));
                        }
                        else
                        {
                            if (visionControl != null && visionControl.isOpen())
                                HOperatorSet.DispCross(visionControl.GetHalconWindow(), hv_Row[i].D, hv_Column[i].D, 20, 0);
                        }
                        int indexsel = 0;
                        shapeparamResults?.Clear();
                        HTuple rowTransUser = 0; HTuple colTransUser = 0;
                        shapeparam shapeparamInstance;
                        foreach (var temp in shapeslist)
                        {
                            shapeparamInstance = temp.Clone();

                            switch (temp.shapeType)
                            {
                                case ShapeType.点:

                                    HOperatorSet.AffineTransPixel(hom2d,
                                        ((UsrShapePoint)temp.usrshape).Y,
                                        ((UsrShapePoint)temp.usrshape).X,
                                        out rowTransUser, out colTransUser);
                                    ((UsrShapePoint)shapeparamInstance.usrshape).X = colTransUser[0].D;
                                    ((UsrShapePoint)shapeparamInstance.usrshape).Y = rowTransUser[0].D;

                                    shapeparamResults.Add(shapeparamInstance);
                                    break;

                                case ShapeType.圆形:

                                    ((UsrShapeCircle)shapeparamInstance.usrshape).CircleRadius = ((UsrShapeCircle)temp.usrshape).CircleRadius;
                                    HOperatorSet.AffineTransPixel(hom2d,
                                        ((UsrShapeCircle)temp.usrshape).CircleCenterY,
                                        ((UsrShapeCircle)temp.usrshape).CircleCenterX,
                                        out rowTransUser, out colTransUser);
                                    ((UsrShapeCircle)shapeparamInstance.usrshape).CircleCenterX = colTransUser[0].D;
                                    ((UsrShapeCircle)shapeparamInstance.usrshape).CircleCenterY = rowTransUser[0].D;
                                    ((UsrShapeCircle)shapeparamInstance.usrshape).CircleRadius = ((UsrShapeCircle)temp.usrshape).CircleRadius;
                                    shapeparamResults.Add(shapeparamInstance);
                                    try
                                    {
                                        HOperatorSet.DispCircle(visionControl.GetHalconWindow(), ((UsrShapeCircle)shapeparamInstance.usrshape).CircleCenterY,
                                            ((UsrShapeCircle)shapeparamInstance.usrshape).CircleCenterX, ((UsrShapeCircle)shapeparamInstance.usrshape).CircleRadius);

                                        //  HOperatorSet.DispObj(rectobj, visionControl.GetHalconWindow());
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                    break;

                                case ShapeType.仿射矩形:

                                    ((UsrShapeRect2)shapeparamInstance.usrshape).Len1 = ((UsrShapeRect2)temp.usrshape).Len1;
                                    ((UsrShapeRect2)shapeparamInstance.usrshape).Len2 = ((UsrShapeRect2)temp.usrshape).Len2;
                                    ((UsrShapeRect2)shapeparamInstance.usrshape).Phi = ((UsrShapeRect2)temp.usrshape).Phi + hv_Angle[i].D;
                                    HOperatorSet.AffineTransPixel(hom2d,
                                        ((UsrShapeRect2)temp.usrshape).CenterY,
                                        ((UsrShapeRect2)temp.usrshape).CenterX,
                                        out rowTransUser, out colTransUser);
                                    ((UsrShapeRect2)shapeparamInstance.usrshape).CenterX = colTransUser[0].D;
                                    ((UsrShapeRect2)shapeparamInstance.usrshape).CenterY = rowTransUser[0].D;
                                    shapeparamResults.Add(shapeparamInstance);
                                    try
                                    {
                                        HOperatorSet.DispRectangle2(visionControl.GetHalconWindow(), ((UsrShapeRect2)shapeparamInstance.usrshape).CenterY,
                                            ((UsrShapeRect2)shapeparamInstance.usrshape).CenterX, ((UsrShapeRect2)shapeparamInstance.usrshape).Phi,
                                             ((UsrShapeRect2)shapeparamInstance.usrshape).Len1,
                                             ((UsrShapeRect2)shapeparamInstance.usrshape).Len2
                                            );
                                        //  HOperatorSet.DispObj(rectobj, visionControl.GetHalconWindow());
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                    break;

                                case ShapeType.矩形:

                                    HOperatorSet.AffineTransPixel(hom2d,
                                        ((UsrShapeRect)temp.usrshape).Y1,
                                        ((UsrShapeRect)temp.usrshape).X1,
                                        out rowTransUser, out colTransUser);
                                    ((UsrShapeRect)shapeparamInstance.usrshape).X1 = colTransUser[0].D;
                                    ((UsrShapeRect)shapeparamInstance.usrshape).Y1 = rowTransUser[0].D;
                                    HOperatorSet.AffineTransPixel(hom2d,
                                       ((UsrShapeRect)temp.usrshape).Y2,
                                       ((UsrShapeRect)temp.usrshape).X2,
                                       out rowTransUser, out colTransUser);
                                    ((UsrShapeRect)shapeparamInstance.usrshape).X2 = colTransUser[0].D;
                                    ((UsrShapeRect)shapeparamInstance.usrshape).Y2 = rowTransUser[0].D;
                                    shapeparamResults.Add(shapeparamInstance);
                                    try
                                    {
                                        //   HOperatorSet.GenRectangle1(out HObject rectobj, ((UsrShapeRect)shapeparamInstance.usrshape).Y1,
                                        // ((UsrShapeRect)shapeparamInstance.usrshape).X1,

                                        //((UsrShapeRect)shapeparamInstance.usrshape).Y2,
                                        //       ((UsrShapeRect)shapeparamInstance.usrshape).X2);
                                        //   HOperatorSet.DispObj(rectobj, visionControl.GetHalconWindow());
                                        if (((UsrShapeRect)shapeparamInstance.usrshape).Y2 > ((UsrShapeRect)shapeparamInstance.usrshape).Y1)
                                        {
                                            HOperatorSet.DispRectangle1(visionControl.GetHalconWindow(),
                                                  ((UsrShapeRect)shapeparamInstance.usrshape).Y1,
                                           ((UsrShapeRect)shapeparamInstance.usrshape).X1, ((UsrShapeRect)shapeparamInstance.usrshape).Y2,
                                                        ((UsrShapeRect)shapeparamInstance.usrshape).X2
                                                       );
                                            // HOperatorSet.DispObj(rectobj, visionControl.GetHalconWindow());
                                        }
                                        else
                                        {
                                            HOperatorSet.DispRectangle1(visionControl.GetHalconWindow(), ((UsrShapeRect)shapeparamInstance.usrshape).Y2,
                                                      ((UsrShapeRect)shapeparamInstance.usrshape).X2,
                                                       ((UsrShapeRect)shapeparamInstance.usrshape).Y1,
                                         ((UsrShapeRect)shapeparamInstance.usrshape).X1);
                                            //  HOperatorSet.DispObj(rectobj, visionControl.GetHalconWindow());
                                        }
                                        //   HOperatorSet.DispRectangle1(visionControl.GetHalconWindow(),
                                        //((UsrShapeRect)shapeparamInstance.usrshape).Y1,
                                        // ((UsrShapeRect)shapeparamInstance.usrshape).X1,

                                        //((UsrShapeRect)shapeparamInstance.usrshape).Y2,
                                        //       ((UsrShapeRect)shapeparamInstance.usrshape).X2
                                        //);
                                    }
                                    catch (Exception e)
                                    {
                                    }

                                    break;
                            }
                        }
                    }
                    Save();
                    if (visionControl != null && visionControl.isOpen())
                        HOperatorSet.SetColor(visionControl.GetHalconWindow(), "green");
                    if (RegionSearch != null && RegionSearch.IsInitialized() && visionControl != null && visionControl.isOpen())
                        HOperatorSet.DispObj(RegionSearch, visionControl.GetHalconWindow());
                    if (visionControl != null && visionControl.isOpen())
                    {
                        HalconExternFunExport.dev_display_shape_matching_results(ModeID, visionControl.GetHalconWindow(), "green", hv_Row, hv_Column, hv_Angle, 1, 1, 0);
                        for (int i = 0; i < hv_Row.Length; i++)
                        {
                            HTuple hTuple = string.Format("x:{0},y:{1},u:{2},score{3}", hv_Column[i].D, hv_Row[i].D, hv_Angle[i].D / Math.PI * 180, hv_Score[i].D);
                            string strmsg = string.Format("x:{0},y:{1},u:{2},score{3},Numlevels", hv_Column[i].D.ToString("F2"), hv_Row[i].D.ToString("F2"), (hv_Angle[i].D / Math.PI * 180).ToString("F2"), hv_Score[i].D.ToString("F2"), visionShapParam.MatchPyamidHigh);
                            HalconExternFunExport.disp_message(visionControl.GetHalconWindow(), strmsg, "window", 0 + i * 20, 100, "green", "false");
                        }
                    }
                }
                else
                {
                    if (visionControl != null && visionControl.isOpen())
                        HOperatorSet.SetColor(visionControl.GetHalconWindow(), "red");
                    if (RegionSearch != null && RegionSearch.IsInitialized() && visionControl != null && visionControl.isOpen())
                        HOperatorSet.DispObj(RegionSearch, visionControl.GetHalconWindow());
                    if (visionControl != null && visionControl.isOpen())
                    {
                        HalconExternFunExport.disp_message(visionControl.GetHalconWindow(), "没有找到", "window", 100, 100, "red", "false");
                    }

                    return false;
                }
            }
            catch (HalconException e)
            {
                _logger.Warn(m_strStepName + ": 寻找模板失败：" + e.Message);
                MessageBox.Show(m_strStepName + ": 寻找模板失败：" + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                ReduceImg?.Dispose();
                GC.Collect();
            }

            return true;
        }

        public XYUPoint GetAffineTransPointAffterMatch(double VisionPointOnModleImgX, double VisionPointOnModleImgY, XYUPoint NowVisionDstModlePoint, VisionControl visionControl = null)
        {
            HOperatorSet.VectorAngleToRigid(visionShapParam.ModlePoint.y, visionShapParam.ModlePoint.x, visionShapParam.ModlePoint.u,
                                NowVisionDstModlePoint.y, NowVisionDstModlePoint.x, NowVisionDstModlePoint.u, out HTuple hom2d);

            HOperatorSet.AffineTransPixel(hom2d, VisionPointOnModleImgY, VisionPointOnModleImgX, out HTuple rowTrans, out HTuple colTrans);
            if (visionControl != null && visionControl.isOpen())
            {
                HOperatorSet.SetColor(visionControl.GetHalconWindow(), "blue");
                HOperatorSet.DispCross(visionControl.GetHalconWindow(), rowTrans, colTrans, 80, 0);
            }
            XYUPoint NowResultPoint = new XYUPoint(colTrans[0].D, rowTrans[0].D, 0);
            return NowResultPoint;
        }

        public override void ClearResult()
        {
            visionShapParam.ResultRow.Clear();
            visionShapParam.ResultCol.Clear();
            visionShapParam.ResultAngle.Clear();
        }

        public override bool GenObj(HObject image, VisionControl visionControl)
        {
            // HOperatorSet.CreateAnisoShapeModel()
            HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
            HTuple hv_Column2 = null, hv_ModelID = null, hv_Row = null;
            HTuple hv_Column = null, hv_Angle = null, hv_ScaleR = null;
            HTuple hv_ScaleC = null, hv_Score = null;
            string strPathRoi = visionShapParam.RoiRegionPath;
            if (!File.Exists(strPathRoi))
            {
                MessageBox.Show("Roi 不存在，请画ROI", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (image == null || !image.IsInitialized())
            {
                MessageBox.Show("图片不存在，请先读取图片", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            HObject ho_ImageReduced = null, ho_Rectangle = null;
            try
            {
                HOperatorSet.ReadRegion(out ho_Rectangle, strPathRoi);
                HOperatorSet.ReduceDomain(image, ho_Rectangle, out ho_ImageReduced);
                if (visionShapParam.ModeType.ToString() == "形状")
                {
                    HOperatorSet.EdgesSubPix(ho_ImageReduced, out HObject edges, "canny", 2, visionShapParam.ContrastLow, visionShapParam.ContrastHigh);
                    if (visionShapParam.CratePyramid == 0)
                    {
                        HOperatorSet.CreateShapeModelXld(edges, "auto", (new HTuple(visionShapParam.AngleStart)).TupleRad(), (new HTuple(visionShapParam.AngleExtent)).TupleRad(), new HTuple(0.05), "auto",
                                 new HTuple(visionShapParam.strPolaritySel), 5, out hv_ModelID);
                    }
                    else
                    {
                        HOperatorSet.CreateShapeModelXld(edges, visionShapParam.CratePyramid, (new HTuple(visionShapParam.AngleStart)).TupleRad()
                      , (new HTuple(visionShapParam.AngleExtent)).TupleRad(), new HTuple(0.05), "auto", new HTuple(visionShapParam.strPolaritySel), 5, out hv_ModelID);
                    }
                    HOperatorSet.GetShapeModelParams(hv_ModelID, out HTuple numlevels, out HTuple startangle,
                        out HTuple endextend, out HTuple anglestep, out HTuple scaleMin, out HTuple scaleMax, out HTuple scalestep, out HTuple metric, out HTuple minContrast);
                    visionShapParam.MatchPyamidHigh = numlevels.I;

                    string strPathMode = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_Mode" + ".shm";
                    if (strSavePath != "")
                        strPathMode = strSavePath + "\\" + m_strStepName + "_Mode" + ".shm";
                    visionShapParam.ModeShmPath = strPathMode;
                    HOperatorSet.WriteShapeModel(hv_ModelID, strPathMode);
                }
                else
                {
                    HOperatorSet.CreateNccModel(ho_ImageReduced, "auto", (new HTuple(visionShapParam.AngleStart)).TupleRad(), (new HTuple(visionShapParam.AngleExtent)).TupleRad(), "auto", new HTuple(visionShapParam.strPolaritySel),
                              out hv_ModelID);
                }

                string strPathModeImg = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_ModeImg" + ".bmp";
                if (strSavePath != "")
                    strPathModeImg = strSavePath + "\\" + m_strStepName + "_ModeImg" + ".bmp";
                visionShapParam.ModeImgPath = strPathModeImg;
                HOperatorSet.WriteImage(image, "bmp", 0, strPathModeImg);
                //if (ModeID != null)//清楚上次模板
                //{
                //    if (visionShapParam.ModeType == "形状")
                //    {
                //        HOperatorSet.ClearShapeModel(ModeID);
                //    }
                //    else
                //    {
                //        HOperatorSet.ClearNccModel(ModeID);
                //    }
                //}

                ModeID = hv_ModelID;

                Save();
                if (Process_image(image, visionControl) && visionShapParam.ResultCol.Count > 0)
                {
                    visionShapParam.ModlePoint.x = visionShapParam.ResultCol[0];
                    visionShapParam.ModlePoint.y = visionShapParam.ResultRow[0];
                    visionShapParam.ModlePoint.u = visionShapParam.ResultAngle[0];
                    if (visionControl != null && visionControl.isOpen())
                        HalconExternFunExport.disp_message(visionControl.GetHalconWindow(), "匹配成功", "window", 100, 100, "green", "false");
                    if (!visionShapParam.bSetOutPoint)
                    {
                        visionShapParam.OutPointInModleImage = new Point2d(visionShapParam.ModlePoint.x, visionShapParam.ModlePoint.y);
                    }
                }
                else
                {
                    MessageBox.Show(m_strStepName + ":创建模板失败-", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (HalconException e)
            {
                MessageBox.Show(m_strStepName + ":创建模板失败-" + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (GenFinished != null)
                    GenFinished();
                ho_Rectangle?.Dispose();
                ho_ImageReduced?.Dispose();
                GC.Collect();
            }

            return true;
        }

        public override bool GenObj(HObject image, string savePath, VisionControl visionControl)
        {
            // HOperatorSet.CreateAnisoShapeModel()
            HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
            HTuple hv_Column2 = null, hv_ModelID = null, hv_Row = null;
            HTuple hv_Column = null, hv_Angle = null, hv_ScaleR = null;
            HTuple hv_ScaleC = null, hv_Score = null;
            string strPathRoi = visionShapParam.RoiRegionPath;
            if (!File.Exists(strPathRoi))
            {
                MessageBox.Show("Roi 不存在，请画ROI", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (image == null || !image.IsInitialized())
            {
                MessageBox.Show("图片不存在，请先读取图片", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            HObject ho_ImageReduced = null, ho_Rectangle = null;
            try
            {
                string strPathMode = "";
                string strPathModeImg = "";
                if (visionShapParam.ModeType.ToString() == "形状")
                {
                    HOperatorSet.ReadRegion(out ho_Rectangle, strPathRoi);
                    HOperatorSet.ReduceDomain(image, ho_Rectangle, out ho_ImageReduced);
                    HOperatorSet.EdgesSubPix(ho_ImageReduced, out HObject edges, "canny", 2, visionShapParam.ContrastLow, visionShapParam.ContrastHigh);
                    if (visionShapParam.CratePyramid == 0)
                    {
                        HOperatorSet.CreateShapeModelXld(edges, "auto", (new HTuple(visionShapParam.AngleStart)).TupleRad(), (new HTuple(visionShapParam.AngleExtent)).TupleRad(), new HTuple(0.05), "auto",
                                 new HTuple(visionShapParam.strPolaritySel), 5, out hv_ModelID);
                    }
                    else
                    {
                        HOperatorSet.CreateShapeModelXld(edges, visionShapParam.CratePyramid, (new HTuple(visionShapParam.AngleStart)).TupleRad()
                      , (new HTuple(visionShapParam.AngleExtent)).TupleRad(), new HTuple(0.05), "auto", new HTuple(visionShapParam.strPolaritySel), 5, out hv_ModelID);
                    }
                    HOperatorSet.GetShapeModelParams(hv_ModelID, out HTuple numlevels, out HTuple startangle,
                        out HTuple endextend, out HTuple anglestep, out HTuple scaleMin, out HTuple scaleMax, out HTuple scalestep, out HTuple metric, out HTuple minContrast);
                    visionShapParam.MatchPyamidHigh = numlevels.I;

                    strPathMode = savePath + "\\" + m_strStepName + "_Mode.shm";
                    visionShapParam.ModeShmPath = strPathMode;
                    HOperatorSet.WriteShapeModel(hv_ModelID, strPathMode);
                }
                else
                {
                    HOperatorSet.CreateNccModel(ho_ImageReduced, "auto", (new HTuple(visionShapParam.AngleStart)).TupleRad(), (new HTuple(visionShapParam.AngleExtent)).TupleRad(), "auto", new HTuple(visionShapParam.strPolaritySel),
                              out hv_ModelID);
                }
                // strPathModeImg = savePath.Substring(0, index);
                strPathModeImg = savePath + "\\" + m_strStepName + "_ModeImg" + ".bmp";
                // string strPathModeImg = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + "_ModeImg" + ".bmp";
                visionShapParam.ModeImgPath = strPathModeImg;
                HOperatorSet.WriteImage(image, "bmp", 0, strPathModeImg);
                //if (ModeID != null)
                //    if (visionShapParam.ModeType == "形状")
                //    {
                //        HOperatorSet.ClearShapeModel(ModeID);
                //    }
                //    else
                //    {
                //        HOperatorSet.ClearNccModel(ModeID);
                //    }
                ModeID = hv_ModelID;
                Save();
                if (Process_image(image, visionControl) && visionShapParam.ResultCol.Count > 0)
                {
                    visionShapParam.ModlePoint.x = visionShapParam.ResultCol[0];
                    visionShapParam.ModlePoint.y = visionShapParam.ResultRow[0];
                    visionShapParam.ModlePoint.u = visionShapParam.ResultAngle[0];
                    if (visionControl != null && visionControl.isOpen())
                        HalconExternFunExport.disp_message(visionControl.GetHalconWindow(), "匹配成功", "window", 100, 100, "green", "false");
                    if (!visionShapParam.bSetOutPoint)
                    {
                        visionShapParam.OutPointInModleImage = new Point2d(visionShapParam.ModlePoint.x, visionShapParam.ModlePoint.y);
                    }
                }
                else
                {
                    MessageBox.Show(m_strStepName + ":创建模板失败-", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (HalconException e)
            {
                MessageBox.Show(m_strStepName + ":创建模板失败-" + e.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (GenFinished != null)
                    GenFinished();
                ho_Rectangle?.Dispose();
                ho_ImageReduced?.Dispose();
                GC.Collect();
            }

            return true;
        }

        public override object GetResult()
        {
            return visionShapParam;
        }

        public override List<shapeparam> GetRegionOuts()
        {
            return shapeparamResults;
        }
    }
}