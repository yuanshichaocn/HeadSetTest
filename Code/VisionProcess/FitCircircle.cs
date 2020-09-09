using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseDll;
using HalconDotNet;
using HalconLib;
using UserCtrl;

namespace VisionProcess 
{
    public enum TestPos
    {
       前端,
       后端,
       最强,
    }
    public enum TestPolarity
    {
        从明到暗,
        从暗到明,
        双向,
    }
    public enum TestDir
    {
      从内到外,
      从外到内,

    }

    public class VisionFitCircleParam : IOperateParam
    {

     
        public TestPos testPos = TestPos.前端;
        public TestPolarity testPolarity = TestPolarity.从明到暗;
        public TestDir testDir = TestDir.从外到内;
        public double thresoldVal = 20;
        public double startAngledeg = 0;
        public double endAngleDeg = 360;
        public int    nTestNum = 100;
        public int nLen1 = 20;
        public int nLen2 = 4;
        public Point2d Resultpoint2D = new Point2d(0, 0);
        public double ResultRadius = 0;
        public Point2d point2DRoixy = new Point2d(0, 0);
        public double  RadiusRoi = 0;
        public double Sigma = 1;
        public void ClearResult()
        {
            ResultRadius = 0;
            Resultpoint2D = new Point2d(0, 0);
        }

        public int GetResultNum()
        {
            if (ResultRadius > 0)
                return 1;
            else
                return 0;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    [Description("找圆")]
    public class VisionFitCircircle : VisionSetpBase
    {
        
        public VisionFitCircircle(string strItemName):  base(strItemName)
        {
            visionCtr = ctr;
        }

        static VisionFindCircleCtr ctr = new VisionFindCircleCtr();
        public override VisionSetpBase Clone()
        {
            VisionFitCircircle visionFitCircircle = new VisionFitCircircle(m_strStepName);
            visionFitCircircle.Read();
            return visionFitCircircle;
        }
        public VisionFitCircleParam visionFitCircleParam = new VisionFitCircleParam();
        public override object Read()
        {
            try
            {
                VisionFitCircleParam tempvisionFitCircleParam = null;
                string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
                //tempvisionFitCircleParam = (VisionFitCircleParam)AccessXmlSerializer.XmlToObject(strPath, visionFitCircleParam.GetType());
                strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
                VisionFitCircircle  fitCircircle = (VisionFitCircircle)AccessJosnSerializer.JsonToObject(strPath, this.GetType());
                if(fitCircircle!=null  && fitCircircle.visionFitCircleParam!=null)
                    visionFitCircleParam = tempvisionFitCircleParam= fitCircircle.visionFitCircleParam;
                else
                {
                    _logger.Warn(m_strStepName + ": 视觉处理项目加载失败，请检查");
                    MessageBox.Show(m_strStepName + ": 视觉处理项目加载失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                return visionFitCircleParam;

                //object obj = new object();
                //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".json";
                //AccessJosnSerializer.ObjectToJson(strPath,obj);
            }
            catch(Exception e )
            {
                _logger.Warn(m_strStepName + "读取失败:"+ e.Message);
            }
            return null;
        }
        public override object Read(string strPath)
        {
            try
            {
              //  string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
                VisionFitCircleParam tempvisionFitCircleParam = (VisionFitCircleParam)AccessXmlSerializer.XmlToObject(strPath, visionFitCircleParam.GetType());
                if (tempvisionFitCircleParam != null)
                    visionFitCircleParam = tempvisionFitCircleParam;
                else
                {
                    _logger.Warn(m_strStepName + ": 视觉处理项目加载失败，请检查");
                    MessageBox.Show(m_strStepName + ": 视觉处理项目加载失败，请检查", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                return visionFitCircleParam;

            }
            catch (Exception e)
            {
                _logger.Warn(m_strStepName + "读取失败:" + e.Message);
            }
            return null;
        }
        public override void Save()
        {
            try
            {
                //string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + m_strStepName + "\\" + m_strStepName + ".xml";
                //if (visionFitCircleParam != null)
                //    AccessXmlSerializer.ObjectToXml(strPath, visionFitCircleParam);
                string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir  + m_strStepName + "\\" + m_strStepName + ".json";
                AccessJosnSerializer.ObjectToJson(strPath, this);
            }
            catch (Exception e)
            {
                _logger.Warn(m_strStepName + "保存失败:" + e.Message);
            }
           

        }
        public override void Save(string strPath)
        {
            try
            {

                //if (visionFitCircleParam != null)
                //    AccessXmlSerializer.ObjectToXml(strPath, visionFitCircleParam);

                AccessJosnSerializer.ObjectToJson(strPath, this);
            }
            catch (Exception e)
            {
                _logger.Warn(m_strStepName + "保存失败:" + e.Message);
            }


        }

        // Local procedures 
        //public void FitCirCle(HObject ho_image, HTuple hv_startangle, HTuple hv_endangle,
        //    HTuple hv_bw, HTuple hv_num, HTuple hv_Radius, HTuple hv_colcenter, HTuple hv_rowcenter,
        //    HTuple hv_len1, HTuple hv_len2, HTuple hv_sigma, HTuple hv_thresholdval, HTuple hv_transition,
        //    HTuple hv_select, HTuple hv_windowhandle, HTuple hv_showrow, HTuple hv_showcol,
        //    out HTuple hv_RowCircleCenterFit, out HTuple hv_ColumnCircleCenterFit, out HTuple hv_RadiusCircleCenterFit,
        //    out HTuple hv_StartPhiCircleCenterFit, out HTuple hv_EndPhiCircleCenterFit,
        //    out HTuple hv_PointOrderCircleCenterFit)
        //{




        //    // Local iconic variables 

        //    HObject ho_Rectangle = null, ho_RectangleSM = null;
        //    HObject ho_ImageReduced = null, ho_Contour = null;

        //    // Local control variables 

        //    HTuple hv_t1 = null, hv_rowedgearr = null;
        //    HTuple hv_coledgearr = null, hv_step = null, hv_angle = null;
        //    HTuple hv_x = new HTuple(), hv_y = new HTuple(), hv_anglerad = new HTuple();
        //    HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
        //    HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
        //    HTuple hv_width = new HTuple(), hv_high = new HTuple();
        //    HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
        //    HTuple hv_MeasureHandle = new HTuple(), hv_RowEdge = new HTuple();
        //    HTuple hv_ColumnEdge = new HTuple(), hv_Amplitude = new HTuple();
        //    HTuple hv_Distance = new HTuple(), hv_t2 = new HTuple();
        //    HTuple hv_t = new HTuple(), hv_msg = new HTuple();
        //    // Initialize local and output iconic variables 
        //    HOperatorSet.GenEmptyObj(out ho_Rectangle);
        //    HOperatorSet.GenEmptyObj(out ho_RectangleSM);
        //    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
        //    HOperatorSet.GenEmptyObj(out ho_Contour);
        //    hv_RowCircleCenterFit = new HTuple();
        //    hv_ColumnCircleCenterFit = new HTuple();
        //    hv_RadiusCircleCenterFit = new HTuple();
        //    hv_StartPhiCircleCenterFit = new HTuple();
        //    hv_EndPhiCircleCenterFit = new HTuple();
        //    hv_PointOrderCircleCenterFit = new HTuple();
        //    try
        //    {
        //        HOperatorSet.CountSeconds(out hv_t1);
        //        hv_rowedgearr = new HTuple();
        //        hv_coledgearr = new HTuple();
        //        hv_step = (((hv_startangle - hv_endangle)).TupleAbs()) / hv_num;
        //        HTuple end_val4 = hv_endangle;
        //        HTuple step_val4 = hv_step;
        //        for (hv_angle = hv_startangle; hv_angle.Continue(end_val4, step_val4); hv_angle = hv_angle.TupleAdd(step_val4))
        //        {
        //            HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
        //            hv_x = hv_colcenter + (hv_Radius * (((hv_angle.TupleRad())).TupleCos()));
        //            hv_y = hv_rowcenter - (hv_Radius * (((hv_angle.TupleRad())).TupleSin()));
        //            if ((int)(new HTuple(hv_bw.TupleEqual(1))) != 0)
        //            {
        //                hv_anglerad = ((hv_angle + 180)).TupleRad();
        //            }
        //            else
        //            {
        //                hv_anglerad = hv_angle.TupleRad();
        //            }

        //            ho_Rectangle.Dispose();
        //            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_y, hv_x, hv_anglerad, hv_len1,
        //                hv_len2);
        //            HOperatorSet.SmallestRectangle1(ho_Rectangle, out hv_Row1, out hv_Column1,
        //                out hv_Row2, out hv_Column2);
        //            ho_RectangleSM.Dispose();
        //            HOperatorSet.GenRectangle1(out ho_RectangleSM, hv_Row1 - 1, hv_Column1 - 1, hv_Row2 + 1,
        //                hv_Column2 + 1);
        //            hv_width = (((hv_Column2 - hv_Column1)).TupleAbs()) + 2;
        //            hv_high = (((hv_Row2 - hv_Row1)).TupleAbs()) + 2;
        //            ho_ImageReduced.Dispose();
        //            HOperatorSet.ReduceDomain(ho_image, ho_RectangleSM, out ho_ImageReduced);
        //            HOperatorSet.GetImageSize(ho_ImageReduced, out hv_Width, out hv_Height);

        //            HOperatorSet.GenMeasureRectangle2(hv_y, hv_x, hv_anglerad, hv_len1, hv_len2,
        //                hv_Width, hv_Height, "nearest_neighbor", out hv_MeasureHandle);
        //            HOperatorSet.MeasurePos(ho_image, hv_MeasureHandle, hv_sigma, hv_thresholdval,
        //                hv_transition, hv_select, out hv_RowEdge, out hv_ColumnEdge, out hv_Amplitude,
        //                out hv_Distance);
        //            HOperatorSet.CloseMeasure(hv_MeasureHandle);
        //            HOperatorSet.DispRectangle2(hv_ExpDefaultWinHandle, hv_y, hv_x, hv_anglerad,
        //                hv_len1, hv_len2);
        //            if ((int)(new HTuple((new HTuple(hv_RowEdge.TupleLength())).TupleGreater(
        //                0))) != 0)
        //            {
        //                HOperatorSet.DispCross(hv_ExpDefaultWinHandle, hv_RowEdge, hv_ColumnEdge,
        //                    36, 0);
        //                hv_rowedgearr = hv_rowedgearr.TupleConcat(hv_RowEdge);
        //                hv_coledgearr = hv_coledgearr.TupleConcat(hv_ColumnEdge);
        //            }

        //        }
        //        if ((int)(new HTuple((new HTuple(hv_rowedgearr.TupleLength())).TupleGreater(
        //            3))) != 0)
        //        {
        //            ho_Contour.Dispose();
        //            HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_rowedgearr, hv_coledgearr);
        //            HOperatorSet.FitCircleContourXld(ho_Contour, "atukey", -1, 0, 0, 3, 2, out hv_RowCircleCenterFit,
        //                out hv_ColumnCircleCenterFit, out hv_RadiusCircleCenterFit, out hv_StartPhiCircleCenterFit,
        //                out hv_EndPhiCircleCenterFit, out hv_PointOrderCircleCenterFit);
        //            HOperatorSet.DispCircle(hv_ExpDefaultWinHandle, hv_RowCircleCenterFit, hv_ColumnCircleCenterFit,
        //                hv_RadiusCircleCenterFit);

        //            HOperatorSet.CountSeconds(out hv_t2);
        //            hv_t = hv_t2 - hv_t1;
        //            hv_msg = (((hv_t + ",x:") + hv_ColumnCircleCenterFit) + ",y:") + hv_RowCircleCenterFit;
        //            disp_message(hv_ExpDefaultWinHandle, hv_msg, "window", hv_showrow, hv_showcol,
        //                "black", "true");
        //        }
        //        ho_Rectangle.Dispose();
        //        ho_RectangleSM.Dispose();
        //        ho_ImageReduced.Dispose();
        //        ho_Contour.Dispose();

        //        return;
        //    }
        //    catch (HalconException HDevExpDefaultException)
        //    {
        //        ho_Rectangle.Dispose();
        //        ho_RectangleSM.Dispose();
        //        ho_ImageReduced.Dispose();
        //        ho_Contour.Dispose();

        //        throw HDevExpDefaultException;
        //    }
        //}

        public bool FitCirCle(HObject ho_image, HTuple hv_startangle, HTuple hv_endangle,
      HTuple hv_bw, HTuple hv_num, HTuple hv_Radius, HTuple hv_colcenter, HTuple hv_rowcenter,
      HTuple hv_len1, HTuple hv_len2, HTuple hv_sigma, HTuple hv_thresholdval, HTuple hv_transition,
      HTuple hv_select, HTuple hv_windowhandle, HTuple hv_showrow, HTuple hv_showcol,
      out HTuple hv_RowCircleCenterFit, out HTuple hv_ColumnCircleCenterFit, out HTuple hv_RadiusCircleCenterFit,
      out HTuple hv_StartPhiCircleCenterFit, out HTuple hv_EndPhiCircleCenterFit,
      out HTuple hv_PointOrderCircleCenterFit)
        {
            
            // Local iconic variables 
            HObject ho_Rectangle = null, ho_RectangleSM = null;
            HObject ho_ImageReduced = null, ho_Contour = null;

            // Local control variables 

            HTuple hv_t1 = null, hv_rowedgearr = null;
            HTuple hv_coledgearr = null, hv_step = null, hv_angle = null;
            HTuple hv_anglerad = new HTuple(), hv_x = new HTuple();
            HTuple hv_y = new HTuple(), hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_width = new HTuple(), hv_high = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MeasureHandle = new HTuple(), hv_RowEdge = new HTuple();
            HTuple hv_ColumnEdge = new HTuple(), hv_Amplitude = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_t2 = new HTuple();
            HTuple hv_t = new HTuple(), hv_msg = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RectangleSM);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            hv_RowCircleCenterFit = new HTuple();
            hv_ColumnCircleCenterFit = new HTuple();
            hv_RadiusCircleCenterFit = new HTuple();
            hv_StartPhiCircleCenterFit = new HTuple();
            hv_EndPhiCircleCenterFit = new HTuple();
            hv_PointOrderCircleCenterFit = new HTuple();
            hv_RowCircleCenterFit = 0;
            hv_ColumnCircleCenterFit = 0;
            hv_RadiusCircleCenterFit = 0;
            try
            {
                if (ho_image == null || !ho_image.IsInitialized())
                    return false;
                HOperatorSet.CountSeconds(out hv_t1);
                hv_rowedgearr = new HTuple();
                hv_coledgearr = new HTuple();
                hv_step = (((hv_startangle - hv_endangle)).TupleAbs()) / hv_num;
                HTuple end_val4 = hv_endangle;
                HTuple step_val4 = hv_step;
                for (hv_angle = hv_startangle; hv_angle.Continue(end_val4, step_val4); hv_angle = hv_angle.TupleAdd(step_val4))
                {
                    HOperatorSet.SetDraw(hv_windowhandle, "margin");
                    try
                    {
                        hv_x = hv_colcenter + (hv_Radius * (((hv_angle.TupleRad())).TupleCos()));
                        hv_y = hv_rowcenter - (hv_Radius * (((hv_angle.TupleRad())).TupleSin()));
                        if ((int)(new HTuple(hv_bw.TupleEqual(1))) != 0)
                        {
                            hv_anglerad = ((hv_angle + 180)).TupleRad();
                        }
                        else
                        {
                            hv_anglerad = hv_angle.TupleRad();
                        }
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2(out ho_Rectangle, hv_y, hv_x, hv_anglerad, hv_len1,
                            hv_len2);
                        HOperatorSet.SmallestRectangle1(ho_Rectangle, out hv_Row1, out hv_Column1,
                            out hv_Row2, out hv_Column2);
                        ho_RectangleSM.Dispose();
                        HOperatorSet.GenRectangle1(out ho_RectangleSM, hv_Row1 - 1, hv_Column1 - 1, hv_Row2 + 1,
                            hv_Column2 + 1);
                        hv_width = (((hv_Column2 - hv_Column1)).TupleAbs()) + 2;
                        hv_high = (((hv_Row2 - hv_Row1)).TupleAbs()) + 2;
                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(ho_image, ho_RectangleSM, out ho_ImageReduced);
                        HOperatorSet.GetImageSize(ho_ImageReduced, out hv_Width, out hv_Height);

                        HOperatorSet.GenMeasureRectangle2(hv_y, hv_x, hv_anglerad, hv_len1, hv_len2,
                            hv_Width, hv_Height, "nearest_neighbor", out hv_MeasureHandle);
                        HOperatorSet.MeasurePos(ho_ImageReduced, hv_MeasureHandle, hv_sigma, hv_thresholdval,
                            hv_transition, hv_select, out hv_RowEdge, out hv_ColumnEdge, out hv_Amplitude,
                            out hv_Distance);
                        HOperatorSet.CloseMeasure(hv_MeasureHandle);
                        //disp_rectangle2 (windowhandle, y, x, anglerad, len1, len2)
                        if ((int)(new HTuple((new HTuple(hv_RowEdge.TupleLength())).TupleGreater(
                            0))) != 0)
                        {
                            //disp_cross (windowhandle, RowEdge, ColumnEdge, 36, 0)
                            hv_rowedgearr = hv_rowedgearr.TupleConcat(hv_RowEdge);
                            hv_coledgearr = hv_coledgearr.TupleConcat(hv_ColumnEdge);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                    

                }
                bool brtn = true;
                if ((int)(new HTuple((new HTuple(hv_rowedgearr.TupleLength())).TupleGreater(
                    3))) != 0)
                {
                    if(hv_rowedgearr.Length*0.7> 3)
                    {
                        ho_Contour.Dispose();

                        HOperatorSet.SetColor(hv_windowhandle, "green");
                        HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_rowedgearr, hv_coledgearr);
                        HOperatorSet.FitCircleContourXld(ho_Contour, "atukey", -1, 0, 0, 15, 6, out hv_RowCircleCenterFit,
                            out hv_ColumnCircleCenterFit, out hv_RadiusCircleCenterFit, out hv_StartPhiCircleCenterFit,
                            out hv_EndPhiCircleCenterFit, out hv_PointOrderCircleCenterFit);
                        HOperatorSet.DispCircle(hv_windowhandle, hv_RowCircleCenterFit, hv_ColumnCircleCenterFit,
                            hv_RadiusCircleCenterFit);

                        HOperatorSet.CountSeconds(out hv_t2);
                        hv_t = hv_t2 - hv_t1;
                        hv_msg = (((hv_t + ",x:") + hv_ColumnCircleCenterFit) + ",y:") + hv_RowCircleCenterFit;
                        //disp_message(hv_ExpDefaultWinHandle, hv_msg, "window", hv_showrow, hv_showcol,
                        //    "black", "true");
                        HalconExternFunExport.disp_message(hv_windowhandle, hv_msg, "window", 100, 200, "green", "false");
                        _logger.Warn(m_strStepName + "找圆成功:" + hv_msg.S);
                        brtn = true;
                    }
                    else
                    {
                        brtn = false;
                    }

                }
                else
                {
                    brtn = false;
                }
                ho_Rectangle.Dispose();
                ho_RectangleSM.Dispose();
                ho_ImageReduced.Dispose();
                ho_Contour.Dispose();

                return brtn;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_RectangleSM.Dispose();
                ho_ImageReduced.Dispose();
                ho_Contour.Dispose();
                _logger.Warn(m_strStepName + "找圆失败:" + HDevExpDefaultException.Message);
                // throw HDevExpDefaultException;
                return false;
            }
        }
        public override bool Process_image(HObject obj, VisionControl visionControl)
        {
            if(visionFitCircleParam!=null)
            {
                int  bw;
                if (visionFitCircleParam.testDir == TestDir.从外到内) bw = 1;
                else bw = 0;
                string strTrans = "";
                if (visionFitCircleParam.testPolarity == TestPolarity.从明到暗)
                    strTrans = "negative";
                else if (visionFitCircleParam.testPolarity == TestPolarity.从暗到明)
                    strTrans = "positive";
                else
                    strTrans = "all";
                string strSelect = "";
                if (visionFitCircleParam.testPos == TestPos.前端)
                    strSelect = "first";
                else if (visionFitCircleParam.testPos == TestPos.后端)
                    strSelect = "last";
                else if (visionFitCircleParam.testPos == TestPos.最强)
                    strSelect = "all";
                try
                {
                    bool btrn = FitCirCle(obj, visionFitCircleParam.startAngledeg, visionFitCircleParam.endAngleDeg,
                        bw, visionFitCircleParam.nTestNum,
                        visionFitCircleParam.RadiusRoi, visionFitCircleParam.point2DRoixy.x, visionFitCircleParam.point2DRoixy.y,
                        visionFitCircleParam.nLen1, visionFitCircleParam.nLen2, visionFitCircleParam.Sigma, visionFitCircleParam.thresoldVal,
                        strTrans, strSelect, visionControl.GetHalconWindow(), 0, 0, out HTuple hv_RowCircleCenterFit, out HTuple hv_ColumnCircleCenterFit, out HTuple hv_RadiusCircleCenterFit,
                        out HTuple hv_StartPhiCircleCenterFit, out HTuple hv_EndPhiCircleCenterFit, out HTuple hv_PointOrderCircleCenterFit);

                    if (btrn)
                    {
                        visionFitCircleParam.Resultpoint2D.x = hv_ColumnCircleCenterFit[0].D;
                        visionFitCircleParam.Resultpoint2D.y = hv_RowCircleCenterFit[0].D;
                        visionFitCircleParam.ResultRadius = hv_RadiusCircleCenterFit[0].D;
                        return true;
                    }
                    else
                    {
                        visionFitCircleParam.Resultpoint2D.x = 0;
                        visionFitCircleParam.Resultpoint2D.y = 0;
                        visionFitCircleParam.ResultRadius = 0;
                        return false;
                    }
                    
                }
                catch( Exception e)
                {
                    visionFitCircleParam.Resultpoint2D.x = 0;
                    visionFitCircleParam.Resultpoint2D.y = 0;
                    visionFitCircleParam.ResultRadius = 0;
                    _logger.Warn(m_strStepName + "参数为空,找园失败"+ e.Message);
                    return false;
                }
            }
            else
            {
                _logger.Warn(m_strStepName + "参数为空,找园失败");
                return false;
            }
               

            
           
        }
        public override object GetResult()
        {
            return visionFitCircleParam;
        }
        public override void ClearResult()
        {
            visionFitCircleParam?.ClearResult();
        }
    }
}
