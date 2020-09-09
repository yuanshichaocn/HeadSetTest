using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseDll;
using UserCtrl;
using MotionIoLib;
using HalconDotNet;

namespace XYZDispensVision
{
     
    public partial class PointValSet : DispUserControl
    {
        public PointValSet()
        {
            InitializeComponent();
           
        }
     
        private void PointValSet_Load(object sender, EventArgs e)
        {
           
        }
        PointCoordinateElement PointCoordinate = null;
        bool bIsAllPointMachine = false;
       
       
        public void UpDataParam(bool bIsMachine, PointCoordinateElement point = null, VisionControl visionControl = null, bool bIsModify = false)
        {
            bIsModify = false;
            PointCoordinateElement pointparam = null;
            if (visionControl != null)
                vc = visionControl;
            if (point != null)
            {
                pointparam = point;
                PointCoordinate = point;
            }
               
            else
                pointparam = PointCoordinate;
            if (pointparam == null)
                return;
            
            

            dMz = point.dMz;
            if (bIsMachine )
            {
                dMx = point.dMx;
                dMy = point.dMy;
            }
            else
            {
                dVx = point.dVx;
                dVy = point.dVy;
            }
            bIsAllPointMachine = bIsMachine;
            bModify=  bIsModify;
        }

        public void SwitchCoordinat(bool bIsMachine)
        {
            if (PointCoordinate == null)
                return;
            dMz = PointCoordinate.dMz;
            if (bIsMachine)
            {
                dMx = PointCoordinate.dMx;
                dMy = PointCoordinate.dMy;
            }
            else
            {
                dVx = PointCoordinate.dVx;
                dVy = PointCoordinate.dVy;
            }
            bIsAllPointMachine = bIsMachine;
           
        }

        public double dMx
        {
            set
            {
                if(PointCoordinate!=null)
                    PointCoordinate.dMx = value;
                txtX.Text = value.ToString();
            }
            get
            {
                //if (PointCoordinate != null)
                //    PointCoordinate.dMx = txtX.Text.ToString().ToDouble();
                return txtX.Text.ToString().ToDouble();
            }
        }

        public double dMy
        {

            set
            {
                if (PointCoordinate != null)
                    PointCoordinate.dMy = value;
                txtY.Text = value.ToString();
            }
            get
            {
                //if (PointCoordinate != null)
                //    PointCoordinate.dMy = txtY.Text.ToString().ToDouble();
                return txtY.Text.ToString().ToDouble();
            }
        }

        public double dVx
        {
            set
            {
                if (PointCoordinate != null)
                    PointCoordinate.dVx = value;
                txtX.Text = value.ToString();
            }
            get
            {
                //if (PointCoordinate != null)
                //    PointCoordinate.dVx = txtX.Text.ToString().ToDouble();
                return txtX.Text.ToString().ToDouble();
            }
        }

        public double dVy
        {

            set
            {
                if (PointCoordinate != null)
                    PointCoordinate.dVy = value;
                txtY.Text = value.ToString();
            }
            get
            {
                //if (PointCoordinate != null)
                //    PointCoordinate.dVy= txtY.Text.ToString().ToDouble();
                return txtY.Text.ToString().ToDouble();
            }
        }
        public double dMz
        {

            set
            {
                if (PointCoordinate != null)
                    PointCoordinate.dMz = value;
                txtZ.Text = value.ToString();
            }
            get
            {
                return txtZ.Text.ToString().ToDouble();
            }
        }
        public void SaveParam()
        {
            dMx = dMx;
            dMy = dMy;
            dVx = dVx;
            dVy = dVy;
            dMz = dMz;
        }


        public int nAxisX;
        public int nAxisY;
        public int nAxisZ;
        private void BtnRecordZ_Click(object sender, EventArgs e)
        {
            dMz= MotionMgr.GetInstace().GetAxisPos(nAxisZ);
        }

        private void BtnSetXY_Click(object sender, EventArgs e)
        {
            if (!bIsAllPointMachine)
              if (vc == null || vc.Img == null || !vc.Img.IsInitialized())
                return;
            if (bIsAllPointMachine)
            {
                dMx = MotionMgr.GetInstace().GetAxisPos(nAxisX);
                dMy = MotionMgr.GetInstace().GetAxisPos(nAxisY);  
            }
            else
            {
                HTuple rowc, colc;
               if(!bModify)
                {
                    vc.DrawPoint(out rowc, out colc);
                }
               else
                {
                    vc.DrawPointMod(dVy, dVx, out rowc, out colc);
                }
                dVx = colc.D;
                dVy = rowc.D;
            }
        }
    }

   

}
