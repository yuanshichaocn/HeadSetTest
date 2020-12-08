using BaseDll;
using System.Collections.Generic;
using System.IO;

namespace XYZDispensVision
{
    public struct PointDispense
    {
        public string strPointName;
        public XYUZPoint MachinePoint;
        public double vel;
        public double acc;
        public double dec;
        public double vellow;
    }

    public class DispConfig
    {
        public int AxisX;
        public int AxisY;
        public int AxisZ;
        public int AxisU = -1;
        public bool IsHaveLaset = false;//是否含有镭射
        public bool IsIoTriggerLight = false;// 是否使用IO触发光源
        public bool IsComTriggerLight = false;// 是否使用Com触发光源
        public string TriggerLightIoName;//触发光源的IO
        public string DispModleName = "Disp";// 点胶模块名
        public string TopCamName = "Top";
    }

    public class DispCalibParam
    {
        /// <summary>
        /// 物理坐标点标定用
        /// </summary>
        public List<PointDispense> pointDispenseCalibs = new List<PointDispense>();

        /// <summary>
        /// 点胶用的点（非轨迹）
        /// </summary>
        public List<PointDispense> OtherPointDispenses = new List<PointDispense>();

        /// <summary>
        /// 标定的偏移
        /// </summary>
        public double dXStep = 0;

        public double dYStep = 0;

        /// <summary>
        /// 针头的偏移
        /// </summary>
        public double dPinOffsetX = 0;

        public double dPinOffsetY = 0;

        /// <summary>
        /// 镭射的偏移
        /// </summary>
        public double dLaserOffsetX = 0;

        public double dLaserOffsetY = 0;

        /// <summary>
        /// 标定的增益曝光
        /// </summary>
        public double dCalibExposure = 0;

        public double dCalibGain = 0;

        /// <summary>
        /// 点胶的曝光增益
        /// </summary>
        public double dDstExposure = 0;

        public double dDstGain = 0;

        public double dDstVel = 0;
        public double dDstAcc = 0;
        public double dDstDec = 0;

        public XYUPoint PinPointAtModle;
        public XYUPoint LaserPointAtModle;

        /// <summary>针头锁存高度</summary>
        public double dNeedleZLatchHeight = 0;

        /// <summary>激光测高压力传感器读数</summary>
        public double dLaserHeightData = 0;

        /// <summary>针头激光Z向差值</summary>
        public double dNeedleLaserHeightOffset = 0;

        public string FileSavePath = "";

        public delegate void UpdatDataGridViewHandler(string OperateName, object obj);

        public event UpdatDataGridViewHandler eventUpdataGridViewForCalib = null;

        public event UpdatDataGridViewHandler eventUpdataGridViewForOtherDisp = null;

        public void SetDispenseCalibsPoint(PointDispense pointDispense)
        {
            int index = pointDispenseCalibs.FindIndex((t) => { return t.strPointName == pointDispense.strPointName; });
            if (index != -1)
            {
                pointDispenseCalibs[index] = pointDispense;
                if (eventUpdataGridViewForCalib != null)
                    eventUpdataGridViewForCalib("SetDispenseCalibsPoint", new object[] { index, pointDispense });
            }
            else
            {
                pointDispenseCalibs.Add(pointDispense);
                if (eventUpdataGridViewForCalib != null)
                    eventUpdataGridViewForCalib("AddDispenseCalibsPoint", new object[] { pointDispenseCalibs.Count - 1, pointDispense });
            }
        }

        public void SetDispenseOtherPoint(PointDispense pointDispense)
        {
            int index = OtherPointDispenses.FindIndex((t) => { return t.strPointName == pointDispense.strPointName; });
            if (index != -1)
            {
                OtherPointDispenses[index] = pointDispense;
                if (eventUpdataGridViewForOtherDisp != null)
                    eventUpdataGridViewForOtherDisp("SetDispenseCalibsPoint", new object[] { index, pointDispense });
            }
            else
            {
                OtherPointDispenses.Add(pointDispense);
                if (eventUpdataGridViewForOtherDisp != null)
                    eventUpdataGridViewForOtherDisp("AddDispenseCalibsPoint", new object[] { OtherPointDispenses.Count - 1, pointDispense });
            }
        }

        public void Save()
        {
            if (FileSavePath != null)
                AccessJosnSerializer.ObjectToJson(FileSavePath, this);
        }

        public void Save(string strFileSavePath)
        {
            FileSavePath = strFileSavePath;
            AccessJosnSerializer.ObjectToJson(FileSavePath, this);
        }

        public static DispCalibParam Read(string strFilePath)
        {
            DispCalibParam dispCalibParam = null;
            if (File.Exists(strFilePath))
                dispCalibParam = (DispCalibParam)AccessJosnSerializer.JsonToObject(strFilePath, typeof(DispCalibParam));
            else
                dispCalibParam = new DispCalibParam();
            return dispCalibParam;
        }
    }
}