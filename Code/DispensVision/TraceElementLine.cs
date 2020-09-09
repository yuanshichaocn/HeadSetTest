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
    public partial class TraceElementLine : DispUserControl, InterfaceDispTraceElementModeOnDebugDlg
    {
        public TraceElementLine()
        {
            InitializeComponent();
        }


        DispTraceBaseElementLine elementLine = null;
        //VisionControl vc;


        public void FlushToDlg(DispTraceBaseElement DispTraceElement, VisionControl visionControl, bool bIsModfiy)
        {
            if (visionControl != null)
                vc = visionControl;
            elementLine = DispTraceElement as DispTraceBaseElementLine;
            if (elementLine == null)
            {
                MessageBox.Show("点胶路径元素的类型不对", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comPartSet1.UpdataParam(elementLine, elementLine.TraceMoveParam);
            pointValSetStartPoint.UpDataParam(elementLine.bIsAllPointMachine, elementLine.cStartPoint, visionControl, bIsModfiy);
            pointValSetEndPoint.UpDataParam(elementLine.bIsAllPointMachine, elementLine.cEndPoint, visionControl, bIsModfiy);
            bModify = bIsModfiy;
        }

        public void SaveParm()
        {
            comPartSet1.SaveParam();
            pointValSetStartPoint.SaveParam();
            pointValSetEndPoint.SaveParam();
        }

        private void TraceElementLine_Load(object sender, EventArgs e)
        {
            comPartSet1.ReUpdateCoordinateHandler += pointValSetStartPoint.SwitchCoordinat;
            comPartSet1.ReUpdateCoordinateHandler += pointValSetEndPoint.SwitchCoordinat;
            comPartSet1.ReUpdateCoordinateHandler += HideSomeBtns;
        }

        private void BtnModifyLine_Click(object sender, EventArgs e)
        {
            if (vc == null || elementLine == null || elementLine.bIsAllPointMachine || vc.Img == null || !vc.Img.IsInitialized())
                return;
            HTuple StartRow = new HTuple(), StartCol = new HTuple(), EndRow = new HTuple(), EndCol = new HTuple();
            if (!bModify)
            {
                vc?.DrawLine(out StartRow, out StartCol, out EndRow, out EndCol);

            }
            else
            {
                vc?.DrawLineMod(pointValSetStartPoint.dVy, pointValSetStartPoint.dVx,
                    pointValSetEndPoint.dVy, pointValSetEndPoint.dVx,
                    out StartRow, out StartCol, out EndRow, out EndCol);
              
            }
            pointValSetStartPoint.dVx = StartCol.D;
            pointValSetStartPoint.dVy = StartRow.D;
            pointValSetEndPoint.dVx = EndCol.D;
            pointValSetEndPoint.dVy = EndRow.D;


        }
        public void HideSomeBtns(bool bIsMachine)
        {
            if (bIsMachine)
            {
                BtnModifyLine.Visible = false;
            }
            else
            {
                BtnModifyLine.Visible = true;
            }
        }
        public void ShowObj()
        {
            if (vc != null && elementLine != null && !elementLine.bIsAllPointMachine && vc.Img != null && vc.Img.IsInitialized())
            {
                HTuple StartRow = new HTuple(), StartCol = new HTuple(), EndRow = new HTuple(), EndCol = new HTuple();
                vc?.DrawLineMod(pointValSetStartPoint.dVy, pointValSetStartPoint.dVx,
                    pointValSetEndPoint.dVy, pointValSetEndPoint.dVx,
                    out StartRow, out StartCol, out EndRow, out EndCol);
                pointValSetStartPoint.dVx = StartCol.D;
                pointValSetStartPoint.dVy = StartRow.D;
                pointValSetEndPoint.dVx = EndCol.D;
                pointValSetEndPoint.dVy = EndRow.D;
            }
        }
    }

    public class DispUserControl : UserControl
    {
        public VisionControl vc = null;
        public bool bModify = false;
    }

}
