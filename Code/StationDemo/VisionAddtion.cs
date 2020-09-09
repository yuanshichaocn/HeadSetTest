using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using UserCtrl;
using BaseDll;
using CommonDlg;
using log4net;
using System.Threading.Tasks;
using VisionProcess;
using HalconDotNet;
using HalconLib;
using CommonTools;

namespace StationDemo
{

    public class VisionAddtion
    {
       

        public static bool FindNozzle(string name, HObject ho_Image, VisionControl vc, List<double> Vrow, List<double> Vcol)
        {

               HObject ho_Region=null, ho_RegionFillUp = null;
                HObject ho_ConnectedRegions = null, ho_SelectedRegions = null, ho_ImageReduced = null;
                HObject ho_Region1 = null, ho_RegionOpening = null;
            try
            {
               

                // Local control variables 

                HTuple hv_Area = null, hv_Row = null, hv_Column = null;
                // Initialize local and output iconic variables 

                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_Region1);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);

                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_Region, 0, 138);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                    70);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_SelectedRegions, out ho_ImageReduced);

                ho_Region1.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region1, 0, 155);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_Region1, out ho_RegionOpening, 3.5);
                HOperatorSet.AreaCenter(ho_RegionOpening, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.SmallestCircle(ho_RegionOpening, out HTuple circlerow, out HTuple circlecol, out HTuple radius);
                VisionFitCircircle visionFitCircircle = new VisionFitCircircle(name);
                visionFitCircircle.visionFitCircleParam.RadiusRoi = radius[0].D;
                visionFitCircircle.visionFitCircleParam.point2DRoixy.x = hv_Column[0].D;
                visionFitCircircle.visionFitCircleParam.point2DRoixy.y = hv_Row[0].D;
                visionFitCircircle.visionFitCircleParam.nLen1 = 30;
                visionFitCircircle.visionFitCircleParam.nLen2 = 3;
                visionFitCircircle.visionFitCircleParam.testDir = TestDir.从外到内;
                visionFitCircircle.visionFitCircleParam.testPolarity = TestPolarity.从明到暗;
                visionFitCircircle.visionFitCircleParam.ClearResult();
                bool brtn = visionFitCircircle.Process_image(ho_Image, vc);
                if (!brtn)
                    return false;

                DoWhile doWhile = new DoWhile((time, dowhile, bmanual, obj) =>
                {
                    if (time > 1500)
                        return WaranResult.Failture;
                    if (visionFitCircircle.visionFitCircleParam.GetResultNum() > 0)
                    {
                        return WaranResult.Run;
                    }
                    return WaranResult.CheckAgain;

                }, 3000);
                WaranResult waranResult = doWhile.doSomething(null, doWhile, true, null);
                if (waranResult != WaranResult.Run)
                    return false;

               
                HOperatorSet.DispCross(vc.GetHalconWindow(), visionFitCircircle.visionFitCircleParam.Resultpoint2D.y,
                    visionFitCircircle.visionFitCircleParam.Resultpoint2D.x, 30, 0);
                Vrow.Add(visionFitCircircle.visionFitCircleParam.Resultpoint2D.y);
                Vcol.Add(visionFitCircircle.visionFitCircleParam.Resultpoint2D.x);

                //  ho_Image.Dispose();

                return true;
            }
            catch( Exception e)
            {
                return false;
            }
            finally
            {
                ho_Region?.Dispose();
                ho_RegionFillUp?.Dispose();
                ho_ConnectedRegions?.Dispose();
                ho_SelectedRegions?.Dispose();
                ho_ImageReduced?.Dispose();
                ho_Region1?.Dispose();
                ho_RegionOpening?.Dispose();
            }
           
         
          


        }
        public static bool FindCircleCenter(HObject ho_Image, VisionControl vc, out HTuple hv_centerx, out HTuple hv_centery)
        {

            HObject ho_Region=null, ho_RegionFillUp = null, ho_ImageReduced = null;
            HObject ho_Region1 = null, ho_RegionOpening = null, ho_RegionBorder = null, ho_Contours = null;
            HObject ho_Contours1 = null, ho_Cross = null, ho_Contour = null, ho_Cross1 = null;
            HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_Row = null;
            HTuple hv_Column = null, hv_Phi = null, hv_Radius1 = null;
            HTuple hv_Radius2 = null, hv_StartPhi = null, hv_EndPhi = null;
            HTuple hv_PointOrder = null, hv_MetrologyHandle = null;
            HTuple hv_Index = null, hv_Row1 = null, hv_Column1 = null;
            HTuple hv_Parameter = null;
            hv_centerx = 0;
            hv_centery = 0;
            // Initialize local and output iconic variables 
            try
            {
              
                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_Region1);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                HOperatorSet.GenEmptyObj(out ho_RegionBorder);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_Contours);
                HOperatorSet.GenEmptyObj(out ho_Contours1);
                HOperatorSet.GenEmptyObj(out ho_Cross);
                HOperatorSet.GenEmptyObj(out ho_Contour);
                HOperatorSet.GenEmptyObj(out ho_Cross1);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_Region, 128, 255);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionFillUp, out ho_ImageReduced);

                ho_Region1.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region1, 0, 120);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_Region1, out ho_RegionOpening, 23.5);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                    70);
                HOperatorSet.GetImageSize(ho_ImageReduced, out hv_Width, out hv_Height);
                ho_RegionBorder.Dispose();
                HOperatorSet.Boundary(ho_SelectedRegions, out ho_RegionBorder, "inner");
                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_RegionBorder, out ho_Contours, "border");
                HOperatorSet.FitEllipseContourXld(ho_Contours, "fitzgibbon", -1, 0, 0, 200, 3,
                    2, out hv_Row, out hv_Column, out hv_Phi, out hv_Radius1, out hv_Radius2,
                    out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectEllipseMeasure(hv_MetrologyHandle, hv_Row, hv_Column,
                    hv_Phi, hv_Radius1, hv_Radius2, 20, 5, 1, 30, new HTuple(), new HTuple(),
                    out hv_Index);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "num_measures",
                    500);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "measure_transition",
                    "positive");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "measure_select",
                    "last");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "min_score",
                    0.5);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "measure_interpolation",
                    "bilinear");
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                ho_Contours1.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours1, hv_MetrologyHandle,
                    "all", "all", out hv_Row1, out hv_Column1);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 10, (new HTuple(45)).TupleRad()
                    );
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_Contour, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type",
                    "all_param", out hv_Parameter);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                ho_Cross1.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_Parameter.TupleSelect(0), hv_Parameter.TupleSelect(
                    1), 50, (new HTuple(45)).TupleRad());
                hv_centerx = hv_Parameter[1];
                hv_centery = hv_Parameter[0];
                if (vc != null && vc.GetHalconWindow() != null)
                {
                    HOperatorSet.DispObj(ho_Image, vc.GetHalconWindow());
                    HOperatorSet.DispObj(ho_Contour, vc.GetHalconWindow());
                    HOperatorSet.DispObj(ho_Cross1, vc.GetHalconWindow());
                    HalconLib.HalconExternFunExport.disp_message(vc.GetHalconWindow(), $"X:{hv_centerx},Y:{hv_centery}", "image", 0, 0, "green", "false");
                }
               

                if (hv_Parameter != null && hv_Parameter.Length > 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;

            }finally
            {
               
                ho_Region.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region1.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionBorder.Dispose();
                ho_Contours.Dispose();
                ho_Contours1.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_Cross1.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
            }
        }
        public static bool CalibUpDown(string name, double row, double col, double phi, double len1, double len2,
            int lowthreshold, int highthreshold, string sortedstyple, string sortedoride, TestPolarity testPolarity,
            HObject img, VisionControl vc, List<double> Vrow, List<double> Vcol)
        {
            HOperatorSet.SetColor(vc.GetHalconWindow(), "red");

            HObject ho_Rectangle, ho_ImageReduced;
            HObject ho_Region, ho_ConnectedRegions, ho_SortedRegions;
            HObject ho_SelectedRegions, ho_ObjectSelected = null;
            HObject ho_RegionOpening1, ho_SelectedRegions1;

            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);

            VisionFitCircircle visionFitCircircle = new VisionFitCircircle(name);
            try
            {
                ho_Rectangle.Dispose();
                HOperatorSet.SetDraw(vc.GetHalconWindow(), "margin");
                HOperatorSet.DispRectangle2(vc.GetHalconWindow(), row, col, phi, len1, len2);
                HOperatorSet.GenRectangle2(out ho_Rectangle, row, col, phi, len1, len2);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(img, ho_Rectangle, out ho_ImageReduced);

                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, lowthreshold, highthreshold);
                HOperatorSet.DispObj(ho_Region, vc.GetHalconWindow());
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions1, "circularity",
                    "and", 0.8, 1);

                HOperatorSet.OpeningCircle(ho_SelectedRegions1, out ho_RegionOpening1, 3.5);
                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShape(ho_RegionOpening1, out ho_SelectedRegions, "area",
                    "and", 1000, 99999);
                
                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character",
                    sortedoride, sortedstyple);
                HOperatorSet.DispObj(ho_SortedRegions, vc.GetHalconWindow());
                HTuple hv_tuplerow = new HTuple();
                HTuple hv_tuplecol = new HTuple();
                HTuple hv_i = 0;
                HOperatorSet.SetColor(vc.GetHalconWindow(), "green");
                HTuple num = 0;
                HOperatorSet.CountObj(ho_SortedRegions, out num);
                if (num != 24)
                    return false;
                for (hv_i = 1; (int)hv_i <= 24; hv_i = (int)hv_i + 1)
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected, hv_i);
                    HOperatorSet.SmallestCircle(ho_ObjectSelected, out HTuple hv_Row2, out HTuple hv_Column2,
                        out HTuple hv_Radius);
                    HTuple hv_colcenter = hv_Column2.Clone();
                    HTuple hv_rowcenter = hv_Row2.Clone();
                    visionFitCircircle.visionFitCircleParam.RadiusRoi = hv_Radius[0].D;
                    visionFitCircircle.visionFitCircleParam.point2DRoixy.x = hv_colcenter[0].D;
                    visionFitCircircle.visionFitCircleParam.point2DRoixy.y = hv_rowcenter[0].D;
                    HalconLib.HalconExternFunExport.disp_message(vc.GetHalconWindow(), hv_i.I.ToString(), "image", hv_rowcenter[0].D, hv_colcenter[0].D,"blue","false");
                    visionFitCircircle.visionFitCircleParam.nLen1 = 10;
                    visionFitCircircle.visionFitCircleParam.nLen2 = 3;
                    visionFitCircircle.visionFitCircleParam.testDir = TestDir.从外到内;
                    visionFitCircircle.visionFitCircleParam.testPolarity = testPolarity;
                    visionFitCircircle.visionFitCircleParam.ClearResult();
                    bool brtn = visionFitCircircle.Process_image(img, vc);
                    if (!brtn)
                        return false;

                    DoWhile doWhile = new DoWhile((time, dowhile, bmanual, obj) =>
                    {
                        if (time > 1500)
                            return WaranResult.Failture;
                        if (visionFitCircircle.visionFitCircleParam.GetResultNum() > 0)
                        {
                            return WaranResult.Run;
                        }
                        return WaranResult.CheckAgain;

                    }, 3000);
                    WaranResult waranResult = doWhile.doSomething(null, doWhile, true, null);
                    if (waranResult != WaranResult.Run)
                        return false;

                    Vrow.Add(visionFitCircircle.visionFitCircleParam.Resultpoint2D.y);
                    Vcol.Add(visionFitCircircle.visionFitCircleParam.Resultpoint2D.x);
                }

            }
            catch
            {
                 ho_Rectangle?.Dispose();
                ho_ImageReduced?.Dispose();
                 ho_Region?.Dispose();
                ho_ConnectedRegions?.Dispose();
                ho_SortedRegions?.Dispose();
                 ho_SelectedRegions?.Dispose();
                ho_ObjectSelected?.Dispose();
                 ho_RegionOpening1?.Dispose();
                ho_SelectedRegions1?.Dispose();
            }

            return true;
        }
    }

}