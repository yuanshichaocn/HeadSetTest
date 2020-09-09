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
    public partial class TraceElementPoint : DispUserControl, InterfaceDispTraceElementModeOnDebugDlg
    {
        public TraceElementPoint()
        {
            InitializeComponent();
        }
        DispTraceBaseElementPoint PonitElement = null;
        
        public void FlushToDlg(DispTraceBaseElement dispTraceBaseElement, VisionControl visionControl, bool bIsModfiy)
        {
            if (visionControl != null)
                vc = visionControl;
            PonitElement = dispTraceBaseElement as DispTraceBaseElementPoint;
            comPartSet1.UpdataParam(PonitElement, PonitElement.TraceMoveParam);
            pointValSet.UpDataParam(PonitElement.bIsAllPointMachine, PonitElement.PointCoordinate, visionControl, bIsModfiy);
            bModify = bIsModfiy;
        }

        public void SaveParm()
        {
            comPartSet1.SaveParam();
            pointValSet.SaveParam();
        }

        private void TraceElementPoint_Load(object sender, EventArgs e)
        {
            comPartSet1.ReUpdateCoordinateHandler += pointValSet.SwitchCoordinat;
        }
        public void ShowObj()
        {
            if (vc != null && PonitElement != null && !PonitElement.bIsAllPointMachine && vc.Img != null && vc.Img.IsInitialized())
            {
                HTuple RowOut= new HTuple(), ColOut = new HTuple();
                vc.DrawPointMod(PonitElement.PointCoordinate.dVy,
                    PonitElement.PointCoordinate.dVx, out RowOut, out ColOut );

                pointValSet.dVx = ColOut.D;
                pointValSet.dVy = RowOut.D;

            }
        }
    }
}
