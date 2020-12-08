using BaseDll;
using MotionIoLib;
using System;
using System.Windows.Forms;

namespace StationDemo
{
    public static class ExternIO
    {
        public static bool Set(this IOOut s, bool bval)
        {
            return IOMgr.GetInstace().WriteIoBit(s.ToString(), bval);
        }

        public static bool SetOn(this IOOut s)
        {
            return IOMgr.GetInstace().WriteIoBit(s.ToString(), true);
        }

        public static bool SetOff(this IOOut s)
        {
            return IOMgr.GetInstace().WriteIoBit(s.ToString(), false);
        }

        public static bool InVal(this IOIN s)
        {
            return IOMgr.GetInstace().ReadIoInBit(s.ToString());
        }

        public static bool OutVal(this IOOut s)
        {
            return IOMgr.GetInstace().ReadIoOutBit(s.ToString());
        }
    }

    public static class ExternParam
    {
        public static T GetPrm<T>(string name, bool isShowAlarm = true)
        {
            try
            {
                if (typeof(T) == typeof(bool))
                {
                    return (T)Convert.ChangeType(ParamSetMgr.GetInstance().GetBoolParam(name), typeof(bool));
                }
                else if (typeof(T) == typeof(int))
                {
                    return (T)Convert.ChangeType(ParamSetMgr.GetInstance().GetIntParam(name), typeof(int));
                }
                else if (typeof(T) == typeof(double))
                {
                    return (T)Convert.ChangeType(ParamSetMgr.GetInstance().GetDoubleParam(name), typeof(double));
                }
                else if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(ParamSetMgr.GetInstance().GetStringParam(name), typeof(string));
                }
            }
            catch
            {
                if (isShowAlarm)
                {
                    MessageBox.Show($"找不到 {typeof(T)} 类型参数名：{name}，现已使用默认值：{default(T)}，若是重要参数请手动添加", "参数加载错误");
                }
            }
            return default(T);
        }
    }
}