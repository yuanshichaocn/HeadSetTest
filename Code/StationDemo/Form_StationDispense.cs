using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XYZDispensVision;
namespace StationDemo
{
    public partial class Form_StationDispense : Form
    {
        public Form_StationDispense()
        {
            InitializeComponent();
        }

        private void dispenseCtrl1_Load(object sender, EventArgs e)
        {
            dispenseCtrl1.AxisX = 0;
            dispenseCtrl1.AxisY = 1;
            dispenseCtrl1.AxisZ = 2;
            dispenseCtrl1.IsIoTriggerLight = true;
            dispenseCtrl1.TriggerLightIoName = "点胶光源点亮";
           
        }
        public void Read()
        {
            //dispenseCtrl1.Read();
        }
        public DispCalibParam GetDisipParam()
        {
            return dispenseCtrl1.dispCalibParam;
        }
    }
}
