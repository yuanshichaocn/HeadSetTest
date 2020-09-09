using BaseDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionProcess;
using HalconDotNet;

namespace UserData
{
    public class CalibMgr
    {
        public CalibMgr()
        {
            xYMoveCalib = new XYMoveCalib();
        }
        private static object obj = new object();
        private static CalibMgr Mgr;
        XYMoveCalib xYMoveCalib;
        public static CalibMgr GetInstance()
        {
            if (Mgr == null)
            {
                lock (obj)
                {
                    if (Mgr == null)
                    {
                        Mgr = new CalibMgr();
                    }
                }
            }
            return Mgr;
        }
        ResultCalib resultCalib = new ResultCalib();
        public void Save()
        {
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "Calib" + "\\" + "offset" + ".xml";
            resultCalib.Save(strPath);

        }
        public object read()
        {
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "Calib" + "\\" + "offset" + ".xml";
           return resultCalib.Read(strPath);
        }
        public void init()
        {
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "Calib" + "\\" + "DownCCDRobot" + ".xml";
           

            resultCalib = (ResultCalib)resultCalib.Read(strPath);
            

            xYMoveCalib.CreateDownRobotCoor(resultCalib.VisionCol.ToArray(), resultCalib.VisionRow.ToArray(),
                resultCalib.MachineX.ToArray(), resultCalib.MachineY.ToArray());

            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "Calib" + "\\" + "DownCol.tup";
            HTuple hTupleDcol;
            HOperatorSet.ReadTuple(strPath, out hTupleDcol);

            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "Calib" + "\\" + "DownRow.tup";
            HTuple hTupleDrow;
            HOperatorSet.ReadTuple(strPath, out hTupleDrow);

            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "Calib" + "\\" + "UpCol.tup";
            HTuple hTupleUcol;
            HOperatorSet.ReadTuple(strPath, out hTupleUcol);

            strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "Calib" + "\\" + "UpRow.tup";
            HTuple hTupleUrow;
            HOperatorSet.ReadTuple(strPath, out hTupleUrow);
            // xYMoveCalib.CreateUpToDownCameraCoor(hTupleUcol.DArr, hTupleUrow.DArr, hTupleDcol.DArr, hTupleDrow.DArr);
            xYMoveCalib.CreateUpDownCameraCoor(hTupleUcol.DArr, hTupleUrow.DArr, hTupleDcol.DArr, hTupleDrow.DArr);
            string strPath1 = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + "Calib" + "\\" + "offset" + ".xml";
            resultCalib = (ResultCalib)resultCalib.Read(strPath1);
        }
        public XYUPoint GetPos(XYUPoint snapPos, double u0degModle, XYUPoint visionpoint)
        {
            return xYMoveCalib.GetPosWhenUpCameraSnap(snapPos, u0degModle, visionpoint);
        }
        public Point2d CalibPos
        {
            set
            {
                xYMoveCalib.CalibPos = value;
            }
            get
            {
                return xYMoveCalib.CalibPos;
            }
        }
        public XYUPoint NozzleCalibRobotCoor
        {
            set
            {
                xYMoveCalib.NozzleCalibRobotCoor = value;
            }
            get
            {
                return xYMoveCalib.NozzleCalibRobotCoor;
            }
        }

       public void GetNozzleInfo(out double offsetx, out double offsety, out double offsetu,
           out double offsetxR, out double offsetyR, out double offsetuR,
           out double pickHigh)
        {

            offsetx = resultCalib.offSetX;
            offsety = resultCalib.offSetY;
            offsetu = resultCalib.offSetU;
            offsetxR = 0;
            offsetyR = 0;
            offsetuR = 0;
            //offsetxR = resultCalib.offSetXR;
            //offsetyR = resultCalib.offSetYR;
            //offsetuR = resultCalib.offSetUR;
            pickHigh = resultCalib.highZ;
        }
        public void SetNozzleInfo( double offsetx,  double offsety,  double offsetu,
            double offsetxR, double offsetyR, double offsetuR, double pickHigh)
        {

            resultCalib.offSetX= offsetx ;
            resultCalib.offSetY= offsety  ;
            resultCalib.offSetU=offsetu  ;

            //resultCalib.offSetXR = offsetxR;
            //resultCalib.offSetYR = offsetyR;
            //resultCalib.offSetUR = offsetuR;
            resultCalib.highZ = pickHigh;
        }

    }
}
