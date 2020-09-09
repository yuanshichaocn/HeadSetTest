using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserCtrl;
using HalconDotNet;

namespace XYZDispensVision
{

    public interface  InterfaceDispTraceElementModeOnDebugDlg
    {
        void FlushToDlg(DispTraceBaseElement dispTraceBaseElement, VisionControl visionControl,bool bIsModfiy);
        void SaveParm();

        void ShowObj();
    }
    public partial class TraceElemementArc : DispUserControl, InterfaceDispTraceElementModeOnDebugDlg
    {
        public TraceElemementArc()
        {
            InitializeComponent();
        }
        DispTraceBaseElementArc cDispTraceElementArc = null;
        public  void HideSomeBtns(bool bIsMachine)
        {
            bool bShow = false;
            if( bIsMachine)
            {
                btnDrawCircle.Visible = bShow;
                btnDrawEndPoint.Visible= bShow;
                btnDrawStartPoint.Visible = bShow;
            }
            else
            {
                bShow = true;
                btnDrawCircle.Visible = bShow;
                btnDrawEndPoint.Visible = bShow;
                btnDrawStartPoint.Visible = bShow;
            }
        }
        private void TraceElemementArc_Load(object sender, EventArgs e)
        {
            comPartSet1.ReUpdateCoordinateHandler += pointValSetArcCenterPoint.SwitchCoordinat;
            comPartSet1.ReUpdateCoordinateHandler += pointValSetArcStartPoint.SwitchCoordinat;
            comPartSet1.ReUpdateCoordinateHandler += pointValSetArcEndPoint.SwitchCoordinat;
            comPartSet1.ReUpdateCoordinateHandler += HideSomeBtns;
        }
       public bool bIsArcDir
        {
            set
            {
                checkSelDir.Checked = value;
                if(cDispTraceElementArc!=null)
                   cDispTraceElementArc.bIsArcDir = value;
            }
            get
            {
                return checkSelDir.Checked;
            }
        }
        public bool bIsArc
        {
            set
            {
                checkBoxIsArc.Checked = value;
                if (cDispTraceElementArc != null)
                    cDispTraceElementArc.bIsArc = value;
            }
            get
            {
                return checkBoxIsArc.Checked;
            }
        }

     

        public  void FlushToDlg(DispTraceBaseElement TraceElement, VisionControl visionControl,bool bIsModfiy)
        {
            if (visionControl != null)
                vc = visionControl;
            cDispTraceElementArc = TraceElement as DispTraceBaseElementArc;
            if(cDispTraceElementArc== null)
            {
                MessageBox.Show("点胶路径元素的类型不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comPartSet1.UpdataParam(cDispTraceElementArc, cDispTraceElementArc.TraceMoveParam);
            //点位设置  圆心 起点 终点
            pointValSetArcCenterPoint.UpDataParam(cDispTraceElementArc.bIsAllPointMachine, cDispTraceElementArc.cCenterPoint, visionControl,bIsModfiy);
            pointValSetArcStartPoint.UpDataParam(cDispTraceElementArc.bIsAllPointMachine, cDispTraceElementArc.cStartPoint, visionControl, bIsModfiy);
            pointValSetArcEndPoint.UpDataParam(cDispTraceElementArc.bIsAllPointMachine, cDispTraceElementArc.cEndPoint, visionControl, bIsModfiy);
            this.bIsArc = cDispTraceElementArc.bIsArc;
            this.bIsArcDir = cDispTraceElementArc.bIsArcDir;
            bModify = bIsModfiy;
        }

        public  void SaveParm()
        {
            comPartSet1.SaveParam();
            pointValSetArcCenterPoint.SaveParam();
            pointValSetArcStartPoint.SaveParam();
            pointValSetArcEndPoint.SaveParam();
            cDispTraceElementArc.bIsArc = this.bIsArc;
            cDispTraceElementArc.bIsArcDir = this.bIsArcDir;
        }

        private void btnDrawCircle_Click(object sender, EventArgs e)
        {
            if (!cDispTraceElementArc.bIsAllPointMachine)
                if(vc == null || vc.Img == null || !vc.Img.IsInitialized())
                  return;
            HTuple RowC = new HTuple(), ColC = new HTuple(), R = new HTuple();
            if (!cDispTraceElementArc.bIsAllPointMachine)
            {
               if( bModify)
                {
                    vc.DrawCircleMod(pointValSetArcCenterPoint.dVy, pointValSetArcCenterPoint.dVx, 1, out RowC, out ColC, out R);
                }
               else
                {
                    vc.DrawCircle( out RowC, out ColC, out R);
                }
                pointValSetArcCenterPoint.dVy = RowC.D;
                pointValSetArcCenterPoint.dVx = ColC.D;
            }

        } 
        private void btnDrawStartPoint_Click(object sender, EventArgs e)
        {
            if (!cDispTraceElementArc.bIsAllPointMachine)
                if (vc == null || vc.Img == null || !vc.Img.IsInitialized())
                    return;
            HTuple Row = new HTuple(), Col = new HTuple(), R = new HTuple();
            if (bModify)
            {
                vc.DrawPointMod(pointValSetArcStartPoint.dVy, pointValSetArcStartPoint.dVx,  out Row, out Col);
            }
            else
            {
                vc.DrawPoint(out Row, out Col);
            }
            pointValSetArcStartPoint.dVy = Row.D;
            pointValSetArcStartPoint.dVx = Col.D;
        }
        private void btnDrawEndPoint_Click(object sender, EventArgs e)
        {
            if (!cDispTraceElementArc.bIsAllPointMachine)
                if (vc == null || vc.Img == null || !vc.Img.IsInitialized())
                    return;

           
            HTuple Row = new HTuple(), Col = new HTuple(), R = new HTuple();
            if (bModify)
            {
                vc.DrawPointMod(pointValSetArcEndPoint.dVy, pointValSetArcEndPoint.dVx, out Row, out Col);
            }
            else
            {
                vc.DrawPoint(out Row, out Col);
            }
            pointValSetArcEndPoint.dVy = Row.D;
            pointValSetArcEndPoint.dVx = Col.D;
        }
    
        public void ShowObj()
        {
          
        }

    }
}
