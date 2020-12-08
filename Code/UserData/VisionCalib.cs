using BaseDll;

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