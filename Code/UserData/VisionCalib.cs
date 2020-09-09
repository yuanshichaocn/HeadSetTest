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

using BaseDll;


using System.Threading.Tasks;
using VisionProcess;
using HalconDotNet;


namespace UserData
{
    public class VisionCalib
    {

     

        public static XY_DUR_Calib XYFrontCalib = new XY_DUR_Calib();
        public static XY_DUR_Calib XYBackCalib = new XY_DUR_Calib();

        public static XY_C_Follow_Calib XYLeftCalib = new XY_C_Follow_Calib();
        public static XY_C_Follow_Calib XYRightCalib = new XY_C_Follow_Calib();

    }
}