using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace BaseDll
{
    public struct Point2d
    {
        public Point2d(double tx = 0, double ty = 0)
        {
            x = tx;
            y = ty;
        }

        public double x;
        public double y;

    }
    public struct XYUPoint
    {

        public XYUPoint(double tx = 0, double ty = 0, double tu = 0)
        {
            x = tx;
            y = ty;
            u = tu;
        }

        public double x;
        public double y;
        public double u;

    }
    public struct XYUZPoint
    {

        public XYUZPoint(double tx = 0, double ty = 0, double tu = 0, double tz = 0)
        {
            x = tx;
            y = ty;
            u = tu;
            z = tz;
        }

        public double x;
        public double y;
        public double u;
        public double z;

    }
    public struct XYUZTxyPoint
    {

        public XYUZTxyPoint(double xx = 0, double yy = 0, double tz = 0, double tu = 0, double ttx = 0, double tty = 0)
        {
            x = xx;
            y = yy;
            u = tu;
            z = tz;
            tx = ttx;
            ty = tty;
        }

        public double x;
        public double y;
        public double u;
        public double z;
        public double tx;
        public double ty;

    }

    public class ResultCalib
    {
        /// <summary>
        /// 视觉和机械手的 9点位置对应
        /// </summary>
        public List<double> VisionRow = new List<double>();
        public List<double> VisionCol = new List<double>();
        public List<double> MachineX = new List<double>();
        public List<double> MachineY = new List<double>();
        public double offSetX = 0;
        public double offSetY = 0;
        public double offSetU = 0;
        public double highZ = 0;
        public void clear()
        {
            VisionRow.Clear();
            VisionCol.Clear();
            MachineX.Clear();
            MachineY.Clear();
        }
        public object Read(string path)
        {
            return AccessXmlSerializer.XmlToObject(path, this.GetType());
        }
        public void Save(string path)
        {
            AccessXmlSerializer.ObjectToXml(path, this);
        }
    }
    public class calibMgr
    {
        Dictionary<string, calibbase> Calibs = new Dictionary<string, calibbase>();
        public void Add(string name, calibbase calibbase)
        {
            if (!Calibs.ContainsKey(name))
                Calibs.Add(name, calibbase);
            else
                Calibs[name] = calibbase;
        }
        public calibbase GetCalib(string name)
        {
            if (!Calibs.ContainsKey(name))
                return null;
            else
                return Calibs[name];
        }

    }
    public abstract class calibbase
    {
        public XYUPoint CalibPoint;
        /// <summary>
        /// 获取目标的点位
        /// </summary>
        /// <param name="dstVisoionpoint"> 目标视觉坐标</param>
        /// <param name="SanpPoint">目标的拍照机械位置</param>
        /// <returns>返回目标机械坐标</returns>
        public abstract XYUPoint GetDstPonit(XYUPoint dstVisoionpoint, XYUPoint SanpMachinePoint);
        /// <summary>
        /// 获取对象的点位
        /// </summary>
        /// <param name="dstVisoionpoint"> 对象视觉坐标</param>
        /// <param name="SanpPoint">对象的拍照机械位置</param>
        /// <returns>返回对象机械坐标</returns>
        public abstract XYUPoint GetObjPonit(XYUPoint objVisoionpoint, XYUPoint SanpMachinePoint);

    }

    public class XY_UR_Calib : calibbase
    {
        HTuple  m_Hom2Drtu, m_Hom2Dutr;
        public void CreateURCoor(double[] ux, double[] uy, double[] rx, double[] ry)
        {
            try
            {
                if (ux.Length != uy.Length || ux.Length != rx.Length || ux.Length != ry.Length || ux.Length == 0)
                    return;
                HTuple Hom2D;
                HOperatorSet.VectorToHomMat2d(ux, uy, rx, ry, out Hom2D);
                m_Hom2Dutr = Hom2D.Clone();
                HOperatorSet.VectorToHomMat2d(rx, ry, ux, uy, out Hom2D);
                m_Hom2Drtu = Hom2D.Clone();
                HTuple qx, qy;
                HTuple qx1, qy1;
                HOperatorSet.AffineTransPoint2d(m_Hom2Dutr, ux[0], uy[0], out qx, out qy);
                HOperatorSet.AffineTransPoint2d(m_Hom2Drtu, qx, qy, out qx1, out qy1);
            }
            catch(Exception e)
            {
                String S = e.Message;
            }  
        }

        public XYUPoint GetModleMachiePos(XYUPoint dstVisoionpoint, XYUPoint SanpMachinePoint)
        {
            return GetDstPonit( dstVisoionpoint,  SanpMachinePoint);
        }
#if false

        public XYUPoint GetDispOrLaserPoint(XYUPoint OldMachineModlePos,XYUPoint NowMachineModlePos, XYUPoint OldPinPos)
        {
            HOperatorSet.VectorAngleToRigid(OldMachineModlePos.x, OldMachineModlePos.y, 0, NowMachineModlePos.x, NowMachineModlePos.y, NowMachineModlePos.u, out HTuple hom2d);
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(hom2d, OldPinPos.x, OldPinPos.y, out qx, out qy);
            XYUPoint NowPoint = new XYUPoint(qx[0].D, qy[0].D, NowMachineModlePos.u);
            return NowPoint;
        }
        /// <summary>
        /// 获取对应Laser点和Pin的 现况下的位置
        /// </summary>
        /// <param name="OldVisoionModlePos">模板视觉坐标位置</param>
        /// <param name="NowVisionModlePos">现在匹配模板视觉坐标位置</param>
        /// <param name="OldSnapModlePos">模板拍照机械位置</param>
        /// <param name="OldPinMachinePos">开始设置的Pin机械位置</param>
        /// <param name=""></param>
        /// <returns></returns>

        public XYUPoint GetDispOrLaserPoint2(XYUPoint OldVisoionModlePos, XYUPoint NowVisionModlePos, XYUPoint OldSnapModlePos, XYUPoint OldPinMachinePos,XYUPoint NowSnapMchinePos)
        {
            HOperatorSet.VectorAngleToRigid(OldVisoionModlePos.x, OldVisoionModlePos.y, OldVisoionModlePos.u, NowVisionModlePos.x, NowVisionModlePos.y, NowVisionModlePos.u, out HTuple hom2d);

            HTuple vx, vy;
            XYUPoint machineXY = new XYUPoint(OldSnapModlePos.x- OldPinMachinePos.x, OldSnapModlePos.y- OldPinMachinePos.y,0);
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtu, machineXY.x, machineXY.y, out vx, out vy);

            HTuple qvx, qvy;
            HOperatorSet.AffineTransPixel(hom2d, vx, vy, out qvx, out qvy);

            XYUPoint NowPinMachinePoint = GetDstPonit(new XYUPoint(qvx, qvy,0), NowSnapMchinePos);

           
            return NowPinMachinePoint;
        }
#endif
        /// <summary>
        /// 针 或者洗头对准标定点位置
        /// </summary>
        public XYUZPoint CalibPoint
        {
            set;
            get;
        }
        public override XYUPoint GetDstPonit(XYUPoint dstVisoionpoint, XYUPoint SanpMachinePoint)
        {
            XYUPoint DstPoint = new XYUPoint();
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutr, dstVisoionpoint.x, dstVisoionpoint.y, out qx, out qy);
            DstPoint.x = SanpMachinePoint.x - qx[0].D;
            DstPoint.y = SanpMachinePoint.y - qy[0].D;
            return DstPoint;
        }

        public override XYUPoint GetObjPonit(XYUPoint objVisoionpoint, XYUPoint SanpMachinePoint)
        {
            XYUPoint ObjPoint = new XYUPoint();
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutr, objVisoionpoint.x, objVisoionpoint.y, out qx, out qy);
            ObjPoint.x = SanpMachinePoint.x - qx[0].D;
            ObjPoint.y = SanpMachinePoint.y - qy[0].D;
            return ObjPoint;
        }
       
    }


    /// <summary>
    /// 上相机移动 下相机固定 上相机拍目标 下相机拍对象 无角度旋转
    /// </summary>

    public class XY_DUR_Calib: calibbase
    {
        HTuple m_Hom2Ddtu, m_Hom2Dutd, m_Hom2Ddtr, m_Hom2Drtd;

         public void CreateUpDownCameraCoor(double[] ux, double[] uy, double[] dx, double[] dy)
        {
            try
            {
                if (ux.Length != uy.Length || ux.Length != dx.Length || ux.Length != dy.Length || ux.Length == 0)
                    return;
                HTuple Hom2D;
                HOperatorSet.VectorToHomMat2d(ux, uy, dx, dy, out Hom2D);
                m_Hom2Dutd = Hom2D.Clone();
                HOperatorSet.VectorToHomMat2d(dx, dy, ux, uy, out Hom2D);
                m_Hom2Ddtu = Hom2D.Clone();

                HTuple qx, qy;
                HTuple qx1, qy1;
                HOperatorSet.AffineTransPoint2d(m_Hom2Ddtu, dx[0], dy[0], out qx, out qy);
                HOperatorSet.AffineTransPoint2d(m_Hom2Dutd, qx, qy, out qx1, out qy1);
            }
            catch(Exception e)
            {
                String S = e.Message;
            }
            
            return;
        }
         public void CreateDownRobotCoor(double[] dx, double[] dy, double[] rx, double[] ry)
        {
            if (rx.Length != ry.Length || dx.Length != dy.Length || rx.Length != dx.Length || rx.Length == 0)
                return;
            HTuple Hom2D;
            HTuple Hom2D1;
            HOperatorSet.VectorToHomMat2d(dx, dy, rx, ry, out Hom2D);
            m_Hom2Ddtr = Hom2D.Clone();
            HOperatorSet.VectorToHomMat2d(rx, ry, dx, dy, out Hom2D1);
            m_Hom2Drtd = Hom2D1.Clone();
            HTuple qx, qy;
            HTuple qx1, qy1;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, dx[0], dy[0], out qx, out qy);
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, qx, qy, out qx1, out qy1);
            return;
        }
        public override XYUPoint GetDstPonit(XYUPoint dstVisoionpoint, XYUPoint SanpMachinePoint)
        {
            XYUPoint dstMachinePos = new XYUPoint();
            HTuple hTuplex, hTupley;
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutd, dstVisoionpoint.x, dstVisoionpoint.y,out hTuplex,out hTupley);
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, hTuplex, hTupley, out qx, out qy);
            XYUPoint vec = new XYUPoint();
            vec.x = SanpMachinePoint.x - CalibPoint.x;
            vec.y = SanpMachinePoint.y - CalibPoint.y;

            dstMachinePos.x = qx[0].D+vec.x;
            dstMachinePos.y = qy[0].D+vec.y;
            return dstMachinePos;
        }
        public override XYUPoint GetObjPonit(XYUPoint objVisoionpoint, XYUPoint SanpMachinePoint)
        {
            XYUPoint objMachinePos = new XYUPoint();
            HTuple hTuplex, hTupley;
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, objVisoionpoint.x, objVisoionpoint.y, out qx, out qy);
            objMachinePos.x = qx[0].D;
            objMachinePos.y = qy[0].D;
            return objMachinePos;
        }

    }

    /// <summary>
    /// 相机在 X 轴或者Y 轴上   XY 分离 ，相机无法直接拍到吸嘴， AidPoint 为 针尖对准一个Mark点 此mark 点供 上相机拍照
    /// 如果相机直接能看
    /// </summary>
    public class XY_C_Follow_Calib : calibbase
    {
        HTuple m_Hom2Drtu, m_Hom2Dutr;
        public void CreateURCoor(double[] ux, double[] uy, double[] rx, double[] ry)
        {
            try
            {
                if (ux.Length != uy.Length || ux.Length != rx.Length || ux.Length != ry.Length || ux.Length == 0)
                    return;
                HTuple Hom2D;
                HOperatorSet.VectorToHomMat2d(ux, uy, rx, ry, out Hom2D);
                m_Hom2Dutr = Hom2D.Clone();
                HOperatorSet.VectorToHomMat2d(rx, ry, ux, uy, out Hom2D);
                m_Hom2Drtu = Hom2D.Clone();
                HTuple qx, qy;
                HTuple qx1, qy1;
                HOperatorSet.AffineTransPoint2d(m_Hom2Dutr, ux[0], uy[0], out qx, out qy);
                HOperatorSet.AffineTransPoint2d(m_Hom2Drtu, qx, qy, out qx1, out qy1);
            }
            catch (Exception e)
            {
                String S = e.Message;
            }
        }
        /// <summary>
        /// 针和吸头对准的位置
        /// </summary>
        public XYUPoint AidPoint {
            set;
            get;
        }

        public override XYUPoint GetDstPonit(XYUPoint dstVisoionpoint, XYUPoint SanpMachinePoint)
        {
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutr, dstVisoionpoint.x, dstVisoionpoint.y, out qx, out qy);
            XYUPoint dstpoint = new XYUPoint();
            dstpoint.x = SanpMachinePoint.x - qx+ AidPoint.x;
            dstpoint.y = SanpMachinePoint.y - qy+ AidPoint.y;
            return dstpoint;
        }

        public override XYUPoint GetObjPonit(XYUPoint objVisoionpoint, XYUPoint SanpMachinePoint)
        {
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutr, objVisoionpoint.x, objVisoionpoint.y, out qx, out qy);
            XYUPoint dstpoint = new XYUPoint();
            dstpoint.x = SanpMachinePoint.x - qx + AidPoint.x;
            dstpoint.y = SanpMachinePoint.y - qy + AidPoint.y;
            return dstpoint;
        }
    }

     public class XYMoveCalib
    {

        HTuple m_Hom2Ddtu, m_Hom2Dutd, m_Hom2Ddtr, m_Hom2Drtd;
        public HTuple CreateUpToDownCameraCoor(double[] ux, double[] uy, double[] dx, double[] dy)
        {
            if (ux.Length != uy.Length || ux.Length != dx.Length || ux.Length != dy.Length || ux.Length == 0)
                return -1;
            HTuple Hom2D;
            HOperatorSet.VectorToHomMat2d(ux, uy, dx, dy, out Hom2D);
            m_Hom2Dutd = Hom2D;
            return Hom2D;
        }

        public void CreateUpDownCameraCoor(double[] ux, double[] uy, double[] dx, double[] dy)
        {
            if (ux.Length != uy.Length || ux.Length != dx.Length || ux.Length != dy.Length || ux.Length == 0)
                return;
            HTuple Hom2D;
            HOperatorSet.VectorToHomMat2d(ux, uy, dx, dy, out Hom2D);
            m_Hom2Dutd = Hom2D.Clone();
            HOperatorSet.VectorToHomMat2d(dx, dy, ux, uy, out Hom2D);
            m_Hom2Ddtu = Hom2D.Clone();

            HTuple qx, qy;
            HTuple qx1, qy1;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtu, dx[0], dy[0], out qx, out qy);
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutd, qx, qy, out qx1, out qy1);
            return;
        }
        public HTuple CreateDownToRobotCoor(double[] dx, double[] dy, double[] rx, double[] ry)
        {
            if (rx.Length != ry.Length || dx.Length != dy.Length || rx.Length != dx.Length || rx.Length == 0)
                return -1;
            HTuple Hom2Ddtr;
            HOperatorSet.VectorToHomMat2d(dx, dy, rx, ry, out Hom2Ddtr);
            m_Hom2Ddtr = Hom2Ddtr;
            return Hom2Ddtr;
        }
        public void CreateDownRobotCoor(double[] dx, double[] dy, double[] rx, double[] ry)
        {
            if (rx.Length != ry.Length || dx.Length != dy.Length || rx.Length != dx.Length || rx.Length == 0)
                return;
            HTuple Hom2D;
            HTuple Hom2D1;
            HOperatorSet.VectorToHomMat2d(dx, dy, rx, ry, out Hom2D);
            m_Hom2Ddtr = Hom2D.Clone();
            HOperatorSet.VectorToHomMat2d(rx, ry, dx, dy, out Hom2D1);
            m_Hom2Drtd = Hom2D1.Clone();
            HTuple qx, qy;
            HTuple qx1, qy1;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, dx[0], dy[0], out qx, out qy);
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, qx, qy, out qx1, out qy1);
            return;
        }
        //吸嘴标定下相机坐标
        public Point2d NozzleCalibVisionCoor { set; get; } = new Point2d(800, 640);
        //吸嘴标定Robot坐标
        public XYUPoint NozzleCalibRobotCoor { set; get; } = new XYUPoint(0, 0, 0);
        //上下相机标定Robot 位置
        public Point2d CalibPos { set; get; } = new Point2d(0, 0);
        /// <summary>
        /// 获取机器人U轴中心对准点
        /// </summary>
        /// <param name="snappos"></param>
        /// <param name="Hom2Ddtu"></param>
        /// <param name="Hom2Ddtr"></param>
        /// <param name="Point2dVisionCoor"></param>
        /// <returns></returns>
        public Point2d GetPosWhenUpCameraSnap(Point2d snappos, HTuple Hom2Ddtu, HTuple Hom2Ddtr, Point2d Point2dVisionCoor)
        {
            HTuple qx, qy;
            HTuple rqx, rqy;
            HOperatorSet.AffineTransPoint2d(Hom2Ddtu, Point2dVisionCoor.x, Point2dVisionCoor.y, out qx, out qy);
            HOperatorSet.AffineTransPoint2d(Hom2Ddtr, qx, qy, out rqx, out rqy);
            Point2d pos = new Point2d();
            pos.x = rqx + snappos.x - CalibPos.x;
            pos.y = rqy + snappos.y - CalibPos.y;
            return pos;
        }
        /// <summary>
        /// 获取 吸嘴中新对准点  上相机拍照  获取吸嘴对应的点视觉坐标 转换得出 机械坐标点（带旋转的机械手  适用 上相机 拍照拾取）
        /// </summary>带旋转的机械手  适用 上相机 拍照拾取
        /// <param name="snappos"> 拍照点 机械手坐标</param>
        /// <param name="u0degModle"> 0 度模板对应的 机械手 机械坐标</param>
        /// <param name="Point2dVisionCoor">视觉mark的视觉坐标</param>
        /// <returns></returns>
        public XYUPoint GetPosWhenUpCameraSnap(XYUPoint snappos, double u0degModle, XYUPoint Point2dVisionCoor)
        {
            // u0degModle 0 度模板（做模板） u的角度
            HTuple qx, qy;

            HTuple dsqx, dsqy;
            HTuple dcqx, dcqy;

            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, snappos.x, snappos.y, out dsqx, out dsqy);
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, CalibPos.x, CalibPos.y, out dcqx, out dcqy);

            HTuple calbx, calby;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, dcqx, dcqy, out calbx, out calby);
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutd, Point2dVisionCoor.x, Point2dVisionCoor.y, out qx, out qy);

            HTuple vecx, vecy;
            vecx = dsqx - dcqx;
            vecy = dsqy - dcqy;


            //mark 坐标
            HTuple dmarkx, dmarky;
            dmarkx = qx + vecx;
            dmarky = qy + vecy;
            HTuple rmarkx, rmarky;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, dmarkx, dmarky, out rmarkx, out rmarky);
            if (Point2dVisionCoor.u > Math.PI)
                Point2dVisionCoor.u = Point2dVisionCoor.u - 2 * Math.PI;
            else if (Point2dVisionCoor.u < -Math.PI)
                Point2dVisionCoor.u = Point2dVisionCoor.u + 2 * Math.PI;
            HTuple tupleAngle = u0degModle - 180.000 * Point2dVisionCoor.u / Math.PI;
            HTuple rqx, rqy;
            //HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, qx, qy, out rqx, out rqy);
            //XYUPoint post = new XYUPoint();
            //post.x = rqx + snappos.x - CalibPos.x;
            //post.y = rqy + snappos.y - CalibPos.y;

            HTuple vecNx, vecNy;
            HTuple dcnqx, dcnqy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, NozzleCalibRobotCoor.x, NozzleCalibRobotCoor.y, out dcnqx, out dcnqy);
            vecNx = NozzleCalibVisionCoor.x - dcnqx;
            vecNy = NozzleCalibVisionCoor.y - dcnqy;
            HTuple lens = Math.Sqrt(vecNx.D * vecNx.D + vecNy.D * vecNy.D);

            ////旋转后到的角度
            // HTuple tupleAngle = u0degModle+180.000* Point2dVisionCoor.u/Math.PI;
            //需要旋转的角度
            HTuple AngleRote = tupleAngle - snappos.u;

            HTuple tupleAnglecalib = 0;
            HOperatorSet.AngleLx(dcnqy, dcnqx, NozzleCalibVisionCoor.y, NozzleCalibVisionCoor.x, out tupleAnglecalib);
            //  HOperatorSet.AngleLl(0,0,0,100,dcnqy, dcnqx, NozzleCalibVisionCoor.y, NozzleCalibVisionCoor.x, out tupleAnglecalib);
            HTuple destangleandR = tupleAnglecalib - Math.PI * (tupleAngle - NozzleCalibRobotCoor.u) / 180.0000;
            HTuple Destnozzledx = dsqx + Math.Cos(destangleandR.D) * lens;
            HTuple Destnozzledy = dsqy - Math.Sin(destangleandR.D) * lens;
            HTuple rnozzlex, rnozzley;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, Destnozzledx, Destnozzledy, out rnozzlex, out rnozzley);


            HTuple offsetx = rmarkx - rnozzlex;
            HTuple offsety = rmarky - rnozzley;

            XYUPoint pos = new XYUPoint(snappos.x + offsetx.D, snappos.y + offsety.D, tupleAngle);

            return pos;
        }
        /// <summary>
        ///   上相机看目标 下相机看对象，机械手拿料移动组装
        /// </summary>
        /// <param name="snappos"></param>
        /// <param name="Point2dVisionCoorDst"></param>
        /// <param name="Point2dVisionCoorObj"></param>
        /// <returns></returns>
        public XYUPoint GetPosWhenUpDownSnap(XYUPoint snappos, XYUPoint Point2dVisionCoorDst, XYUPoint Point2dVisionCoorObj)
        {
            XYUPoint pos = new XYUPoint(snappos.x, snappos.y, 0);

            return pos;
        }

        ResultCalib resultCalib = new ResultCalib();



    }

    public struct NozzleSturt
    {
        public double offSetX;
        public double offSetY;
        public double offSetU;
        public double highZ;
        public Point2d NozzleCalibVisionCoor;
        //吸嘴标定Robot坐标
        public XYUPoint NozzleCalibRobotCoor;
        // 旋转轴 正向 逆时针 IsCounterclockwise_Rotation=ture 
        public bool IsCounterclockwise_Rotation;
        //0度图像模板时， 吸头的角度
        public double u0degModle;

        public void Init()
        {
            offSetX = 0;
            offSetY = 0;
            offSetU = 0;
            highZ = 0;
            NozzleCalibVisionCoor = new Point2d(0, 0);
            //吸嘴标定Robot坐标
            NozzleCalibRobotCoor = new XYUPoint(0, 0, 0);
            IsCounterclockwise_Rotation = true;
            u0degModle = 0;
        }
    }

    public class ResultCalibMulitNozzle
    {
        /// <summary>
        /// 视觉和机械手的 9点位置对应
        /// </summary>
        public List<double> VisionRow = new List<double>();
        public List<double> VisionCol = new List<double>();
        public List<double> MachineX = new List<double>();
        public List<double> MachineY = new List<double>();
        public List<NozzleSturt> nozzleSturts = new List<NozzleSturt>();

        public void clear()
        {
            VisionRow.Clear();
            VisionCol.Clear();
            MachineX.Clear();
            MachineY.Clear();
            nozzleSturts.Clear();
        }
        public object Read(string path)
        {
            return AccessXmlSerializer.XmlToObject(path, this.GetType());
        }
        public void Save(string path)
        {
            AccessXmlSerializer.ObjectToXml(path, this);
        }
    }

    public class XYMoveCalibMulitNozzles
    {
        ResultCalibMulitNozzle resultCalibMulitNozzle = new ResultCalibMulitNozzle();
        HTuple m_Hom2Ddtu, m_Hom2Dutd, m_Hom2Ddtr, m_Hom2Drtd;
        public HTuple CreateUpToDownCameraCoor(double[] ux, double[] uy, double[] dx, double[] dy)
        {
            if (ux.Length != uy.Length || ux.Length != dx.Length || ux.Length != dy.Length || ux.Length == 0)
                return -1;
            HTuple Hom2D;
            HOperatorSet.VectorToHomMat2d(ux, uy, dx, dy, out Hom2D);
            m_Hom2Dutd = Hom2D;
            return Hom2D;
        }

        public void CreateUpDownCameraCoor(double[] ux, double[] uy, double[] dx, double[] dy)
        {
            if (ux.Length != uy.Length || ux.Length != dx.Length || ux.Length != dy.Length || ux.Length == 0)
                return;
            HTuple Hom2D;
            HOperatorSet.VectorToHomMat2d(ux, uy, dx, dy, out Hom2D);
            m_Hom2Dutd = Hom2D.Clone();
            HOperatorSet.VectorToHomMat2d(dx, dy, ux, uy, out Hom2D);
            m_Hom2Ddtu = Hom2D.Clone();

            HTuple qx, qy;
            HTuple qx1, qy1;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtu, dx[0], dy[0], out qx, out qy);
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutd, qx, qy, out qx1, out qy1);
            return;
        }
        public HTuple CreateDownToRobotCoor(double[] dx, double[] dy, double[] rx, double[] ry)
        {
            if (rx.Length != ry.Length || dx.Length != dy.Length || rx.Length != dx.Length || rx.Length == 0)
                return -1;
            HTuple Hom2Ddtr;
            HOperatorSet.VectorToHomMat2d(dx, dy, rx, ry, out Hom2Ddtr);
            m_Hom2Ddtr = Hom2Ddtr;
            return Hom2Ddtr;
        }

        public void CreateDownRobotCoor(double[] dx, double[] dy, double[] rx, double[] ry)
        {
            if (rx.Length != ry.Length || dx.Length != dy.Length || rx.Length != dx.Length || rx.Length == 0)
                return;
            HTuple Hom2D;
            HTuple Hom2D1;
            HOperatorSet.VectorToHomMat2d(dx, dy, rx, ry, out Hom2D);
            m_Hom2Ddtr = Hom2D.Clone();
            HOperatorSet.VectorToHomMat2d(rx, ry, dx, dy, out Hom2D1);
            m_Hom2Drtd = Hom2D1.Clone();
            HTuple qx, qy;
            HTuple qx1, qy1;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, dx[0], dy[0], out qx, out qy);
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, qx, qy, out qx1, out qy1);
            return;
        }
        //吸嘴标定下相机坐标
        //  public Point2d NozzleCalibVisionCoor { set; get; } = new Point2d(1536, 1024);
        //吸嘴标定Robot坐标
        //     public XYUPoint NozzleCalibRobotCoor { set; get; } = new XYUPoint(0, 0, 0);
        //上下相机标定Robot 位置
        public XYUPoint CalibPos { set; get; } = new XYUPoint(0, 0, 0);
        /// <summary>
        /// 获取机器人U轴中心对准点
        /// </summary>
        /// <param name="snappos"></param>
        /// <param name="Hom2Ddtu"></param>
        /// <param name="Hom2Ddtr"></param>
        /// <param name="Point2dVisionCoor"></param>
        /// <returns></returns>
        public Point2d GetPosWhenUpCameraSnap(Point2d snappos, HTuple Hom2Ddtu, HTuple Hom2Ddtr, Point2d Point2dVisionCoor)
        {
            HTuple qx, qy;
            HTuple rqx, rqy;
            HOperatorSet.AffineTransPoint2d(Hom2Ddtu, Point2dVisionCoor.x, Point2dVisionCoor.y, out qx, out qy);
            HOperatorSet.AffineTransPoint2d(Hom2Ddtr, qx, qy, out rqx, out rqy);
            Point2d pos = new Point2d();
            pos.x = rqx + snappos.x - CalibPos.x;
            pos.y = rqy + snappos.y - CalibPos.y;
            return pos;
        }
        /// <summary>
        /// 获取 吸嘴中新对准点
        /// </summary>
        /// <param name="snappos"></param>
        /// <param name="u0degModle"></param>
        /// <param name="Point2dVisionCoor"></param>
        /// <returns></returns>
        public XYUPoint GetPosWhenUpCameraSnap(int nIndex, XYUPoint snappos, XYUPoint Point2dVisionCoor)
        {
            XYUPoint posull = new XYUPoint(double.MaxValue, double.MaxValue, double.MaxValue);
            if (nIndex > resultCalibMulitNozzle.nozzleSturts.Count || nIndex < 0)
                return posull;

            XYUPoint NozzleCalibRobotCoor = resultCalibMulitNozzle.nozzleSturts[nIndex].NozzleCalibRobotCoor;
            Point2d NozzleCalibVisionCoor = resultCalibMulitNozzle.nozzleSturts[nIndex].NozzleCalibVisionCoor;
            double u0degModle = resultCalibMulitNozzle.nozzleSturts[nIndex].u0degModle;
            bool IsCounterclockwise_Rotation = resultCalibMulitNozzle.nozzleSturts[nIndex].IsCounterclockwise_Rotation;
            // u0degModle 0 度模板（做模板） u的角度
            HTuple qx, qy;

            HTuple dsqx, dsqy;
            HTuple dcqx, dcqy;

            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, snappos.x, snappos.y, out dsqx, out dsqy);
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, CalibPos.x, CalibPos.y, out dcqx, out dcqy);

            HTuple calbx, calby;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, dcqx, dcqy, out calbx, out calby);
            HOperatorSet.AffineTransPoint2d(m_Hom2Dutd, Point2dVisionCoor.x, Point2dVisionCoor.y, out qx, out qy);

            HTuple vecx, vecy;
            vecx = dsqx - dcqx;
            vecy = dsqy - dcqy;


            //mark 坐标
            HTuple dmarkx, dmarky;
            dmarkx = qx + vecx;
            dmarky = qy + vecy;
            HTuple rmarkx, rmarky;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, dmarkx, dmarky, out rmarkx, out rmarky);
            if (Point2dVisionCoor.u > Math.PI)
                Point2dVisionCoor.u = Point2dVisionCoor.u - 2 * Math.PI;
            else if (Point2dVisionCoor.u < -Math.PI)
                Point2dVisionCoor.u = Point2dVisionCoor.u + 2 * Math.PI;
            //旋转轴的物理 角度
            HTuple tupleAngle = 0;
            if (!IsCounterclockwise_Rotation)// 旋转轴 正向 逆时针 IsCounterclockwise_Rotation=ture 
                tupleAngle = u0degModle - 180.000 * Point2dVisionCoor.u / Math.PI;
            else
                tupleAngle = u0degModle + 180.000 * Point2dVisionCoor.u / Math.PI;
            HTuple rqx, rqy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, qx, qy, out rqx, out rqy);
            XYUPoint post = new XYUPoint();
            post.x = rqx + snappos.x - CalibPos.x;
            post.y = rqy + snappos.y - CalibPos.y;

            HTuple vecNx, vecNy;
            HTuple dcnqx, dcnqy;
            HOperatorSet.AffineTransPoint2d(m_Hom2Drtd, NozzleCalibRobotCoor.x, NozzleCalibRobotCoor.y, out dcnqx, out dcnqy);
            vecNx = NozzleCalibVisionCoor.x - dcnqx;
            vecNy = NozzleCalibVisionCoor.y - dcnqy;
            HTuple lens = Math.Sqrt(vecNx.D * vecNx.D + vecNy.D * vecNy.D);

            //需要旋转的角度
            HTuple AngleRote = tupleAngle - snappos.u;

            HTuple tupleAnglecalib = 0;
            HOperatorSet.AngleLx(dcnqy, dcnqx, NozzleCalibVisionCoor.y, NozzleCalibVisionCoor.x, out tupleAnglecalib);
            //  HOperatorSet.AngleLl(0,0,0,100,dcnqy, dcnqx, NozzleCalibVisionCoor.y, NozzleCalibVisionCoor.x, out tupleAnglecalib);
            HTuple destangleandR = 0;
            if (!IsCounterclockwise_Rotation)
                destangleandR = tupleAnglecalib - Math.PI * (tupleAngle - NozzleCalibRobotCoor.u) / 180.0000;
            else
                destangleandR = tupleAnglecalib + Math.PI * (tupleAngle - NozzleCalibRobotCoor.u) / 180.0000;
            HTuple Destnozzledx = dsqx + Math.Cos(destangleandR.D) * lens;
            HTuple Destnozzledy = dsqy - Math.Sin(destangleandR.D) * lens;
            HTuple rnozzlex, rnozzley;
            HOperatorSet.AffineTransPoint2d(m_Hom2Ddtr, Destnozzledx, Destnozzledy, out rnozzlex, out rnozzley);


            HTuple offsetx = rmarkx - rnozzlex;
            HTuple offsety = rmarky - rnozzley;

            XYUPoint pos = new XYUPoint(snappos.x + offsetx.D, snappos.y + offsety.D, tupleAngle);

            return pos;
        }

    }

}
