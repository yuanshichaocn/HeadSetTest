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

namespace XYZDispensVision
{
    public partial class ComPartSet : UserControl
    {
        public ComPartSet()
        {
            InitializeComponent();
        }
        public event UpdateCoordinate ReUpdateCoordinateHandler;
        private void ComPartSet_Load(object sender, EventArgs e)
        {

        }

        private DispTraceBaseElement TraceElement = null;
        private DispEveryTraceMoveParam MoveParam = null;
        public void UpdataParam(DispTraceBaseElement  element, DispEveryTraceMoveParam dispmoveparam)
        {
            TraceElement = element;
            MoveParam = dispmoveparam;
        }
        public string ItemName
        {

            set
            {
                txtName.Text = value;
                if(TraceElement!=null)
                     TraceElement.ItemName = value;
            }
            get
            {
                if (TraceElement != null)
                    TraceElement.ItemName= txtName.Text;
                return txtName.Text;
            }

        }
        public bool bIsAllPointMachine
        {
            set
            {
                if(ReUpdateCoordinateHandler!=null && checkBoxSelMachie.Checked != value)
                {
                    ReUpdateCoordinateHandler(value);
                }
                checkBoxSelMachie.Checked = value;
                if (TraceElement != null)
                    TraceElement.bIsAllPointMachine = value;
            }
            get
            {
                if (TraceElement != null)
                    TraceElement.bIsAllPointMachine = checkBoxSelMachie.Checked;
                return checkBoxSelMachie.Checked;
            }
        }
        public double VelHigh
        {
            set
            {
                if (MoveParam != null)
                    MoveParam.SpeedHigh= value;
                txtVelHigh.Text = value.ToString();
            }

            get
            {
                if(MoveParam != null)
                    MoveParam.SpeedHigh = txtVelHigh.Text.ToString().ToDouble();
                return txtVelHigh.Text.ToString().ToDouble();
            }
        }
        public double VelLow
        {
            set
            {
                if (MoveParam != null)
                    MoveParam.SpeedLow = value;
                txtVelLow.Text = value.ToString();
            }

            get
            {
                if (MoveParam != null)
                    MoveParam.SpeedLow = txtVelLow.Text.ToString().ToDouble();
                return txtVelLow.Text.ToString().ToDouble();
            }
        }
        public double Acc
        {
            set
            {
                if (MoveParam != null)
                    MoveParam.Dec= MoveParam.Acc = value;
                txtAcc.Text = value.ToString();
            }

            get
            {
                if (MoveParam != null)
                    MoveParam.Dec = MoveParam.Acc = txtAcc.Text.ToString().ToDouble();
                return txtAcc.Text.ToString().ToDouble();
            }
        }

        public void SaveParam()
        {
            Acc = Acc;
            ItemName = ItemName;
            VelHigh = VelHigh;
            VelLow = VelLow;
            if (TraceElement != null)
                TraceElement.bIsAllPointMachine = bIsAllPointMachine;
        }
    }
    public delegate void UpdateCoordinate(bool bIsMahcinePoint);
}
