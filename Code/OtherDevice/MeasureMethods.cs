using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace OtherDevice
{

    public class KeyenceHigh
    {
        public KeyenceHigh()
         {
            OpenEthernetCommunication();
        }
         ~KeyenceHigh()
        {
            CloseEthernetCommunication();
        }
        public double mesureResult;
        public double GetMeasureDate( int times, int measureDelayTime)
        {
            int sum = 0;
            for (int i = 0; i < times; i++)
            {
                sum += GetMeasurementData();
                Thread.Sleep(measureDelayTime);
            }
            mesureResult = (double)sum / (double)times;
            return mesureResult/10000;   //测量值被扩大了10000倍
        }
        public void OpenEthernetCommunication()
        {
            CL3IF_ETHERNET_SETTING ethernetSetting = new CL3IF_ETHERNET_SETTING();
            ethernetSetting.ipAddress = new byte[4];
            ethernetSetting.ipAddress[0] = 192;
            ethernetSetting.ipAddress[1] = 168;
            ethernetSetting.ipAddress[2] = 0;
            ethernetSetting.ipAddress[3] = 1;
            ethernetSetting.portNo = 24685;
            ethernetSetting.reserved = new byte[2];
            ethernetSetting.reserved[0] = 0x00;
            ethernetSetting.reserved[1] = 0x00;

            int returnCode = KeyenceHigh.CL3IF_OpenEthernetCommunication(CurrentDeviceId, ref ethernetSetting, 10000);

            SetDeviceStatement(returnCode, DeviceStatus.Ethernet);
            _deviceData[CurrentDeviceId].EthernetSetting = ethernetSetting;
        }
        private int GetMeasurementData()
        {
            byte[] buffer = new byte[MaxRequestDataLength];
            using (PinnedObject pin = new PinnedObject(buffer))
            {
                measurementData.outMeasurementData = new CL3IF_OUTMEASUREMENT_DATA[1];
                int returnCode = KeyenceHigh.CL3IF_GetMeasurementData(0, pin.Pointer);

                if (returnCode != 0)
                {
                    return -999999999;
                }

                measurementData.addInfo = (CL3IF_ADD_INFO)Marshal.PtrToStructure(pin.Pointer, typeof(CL3IF_ADD_INFO));  //实现测量值获取
                int readPosition = Marshal.SizeOf(typeof(CL3IF_ADD_INFO));
                measurementData.outMeasurementData[0] = (CL3IF_OUTMEASUREMENT_DATA)Marshal.PtrToStructure(pin.Pointer +2* readPosition, typeof(CL3IF_OUTMEASUREMENT_DATA));
                return measurementData.outMeasurementData[0].measurementValue;
            }
        }
        public void CloseEthernetCommunication()
        {
            CL3IF_CloseCommunication(CurrentDeviceId);
            _deviceData[CurrentDeviceId].Status = DeviceStatus.NoConnection;
        }
        public void AutoZeroSingle(bool onOff)
        {
            KeyenceHigh.CL3IF_AutoZeroSingle(CurrentDeviceId, 0, onOff);     //CL3IF_AutoZeroSingle(设备,  指定控制的OUT 编号(0至7）, 开/关)
        }
        public void ResetSingle()
        {
            KeyenceHigh.CL3IF_ResetSingle(CurrentDeviceId, 0);
        }

        private readonly DeviceData[] _deviceData = new DeviceData[3] { new DeviceData(), new DeviceData(), new DeviceData() };
        public const int CL3IF_MAX_OUT_COUNT = 8;
        private const int MaxRequestDataLength = 512000;
        CL3IF_MEASUREMENT_DATA measurementData = new CL3IF_MEASUREMENT_DATA();

        public int CurrentDeviceId
        {
            get { return 0; }
        }
        private void SetDeviceStatement(int returnCode, DeviceStatus status)
        {
            if (returnCode == 0)
            {
                _deviceData[CurrentDeviceId].Status = status;
            }
            else
            {
                _deviceData[CurrentDeviceId].Status = DeviceStatus.NoConnection;
            }
        }
        public bool bIsConnected
        {
            get
            {
                return _deviceData[CurrentDeviceId].Status != DeviceStatus.NoConnection;
            }
        }



        [DllImport("CL3_IF.dll")]
        internal static extern int CL3IF_OpenEthernetCommunication(int deviceId, ref CL3IF_ETHERNET_SETTING ethernetSetting, uint timeout); //通讯连接
        [DllImport("CL3_IF.dll")]
        internal static extern int CL3IF_GetMeasurementData(int deviceId, IntPtr measurementData);  //获取测量值
        [DllImport("CL3_IF.dll")]
        internal static extern int CL3IF_CloseCommunication(int deviceId); //断开连接
        [DllImport("CL3_IF.dll")]
        internal static extern int CL3IF_AutoZeroSingle(int deviceId, byte outNo, bool onOff); //自动归零(单个)
        [DllImport("CL3_IF.dll")]
        internal static extern int CL3IF_ResetSingle(int deviceId, byte outNo);//复位（单个）
    }
    public struct CL3IF_VERSION_INFO
    {
        public int majorNumber;
        public int minorNumber;
        public int revisionNumber;
        public int buildNumber;
    };
    public struct CL3IF_ETHERNET_SETTING
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] ipAddress;
        public ushort portNo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved;
    };
    public enum CL3IF_DEVICETYPE
    {
        CL3IF_DEVICETYPE_INVALID = 0x0000,
        CL3IF_DEVICETYPE_CONTROLLER = 0x0001,
        CL3IF_DEVICETYPE_OPTICALUNIT1 = 0x0011,
        CL3IF_DEVICETYPE_OPTICALUNIT2 = 0x0012,
        CL3IF_DEVICETYPE_OPTICALUNIT3 = 0x0013,
        CL3IF_DEVICETYPE_OPTICALUNIT4 = 0x0014,
        CL3IF_DEVICETYPE_OPTICALUNIT5 = 0x0015,
        CL3IF_DEVICETYPE_OPTICALUNIT6 = 0x0016,
        CL3IF_DEVICETYPE_EXUNIT1 = 0x0041,     // Extensional Unit 1
        CL3IF_DEVICETYPE_EXUNIT2 = 0x0042      // Extensional Unit 2
    };
    public struct CL3IF_ADD_INFO
    {
        public uint triggerCount;
        public int pulseCount;
    }
    public struct CL3IF_OUTMEASUREMENT_DATA
    {
        public int measurementValue;
        //public byte valueInfo;
        //public byte judgeResult;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved;

    };
    public enum DeviceStatus     //设备状态
    {
        NoConnection = 0,
        Usb,
        Ethernet,
    };
    public class DeviceData       //设备数据
    {
        private DeviceStatus _status = DeviceStatus.NoConnection;

        public DeviceStatus Status
        {
            get { return _status; }
            set
            {
                EthernetSetting = new CL3IF_ETHERNET_SETTING();
                _status = value;
            }
        }
        public CL3IF_ETHERNET_SETTING EthernetSetting { get; set; }

        public DeviceData()
        {
            EthernetSetting = new CL3IF_ETHERNET_SETTING();
        }

        public string GetStatusString()
        {
            string status = _status == DeviceStatus.NoConnection ? "No connection" : _status.ToString();
            const string SegmentBranch = ".";
            switch (_status)
            {
                case DeviceStatus.Ethernet:
                    status += string.Format("---{0}", EthernetSetting.ipAddress[0]
                                                      + SegmentBranch + EthernetSetting.ipAddress[1]
                                                      + SegmentBranch + EthernetSetting.ipAddress[2]
                                                      + SegmentBranch + EthernetSetting.ipAddress[3]);
                    break;
                default:
                    break;
            }
            return status;
        }
    }
    public struct CL3IF_MEASUREMENT_DATA
    {
        public CL3IF_ADD_INFO addInfo;
        public CL3IF_OUTMEASUREMENT_DATA[] outMeasurementData;
    };
    public enum CL3IF_VALUE_INFO
    {
        CL3IF_VALUE_INFO_VALID,
        CL3IF_VALUE_INFO_JUDGMENTSTANDBY,
        CL3IF_VALUE_INFO_INVALID,
        CL3IF_VALUE_INFO_OVERDISPRANGE_P,
        CL3IF_VALUE_INFO_OVERDISPRANGE_N
    }
    public enum CL3IF_OUTNO
    {
        CL3IF_OUTNO_01 = 0x0001, // OUT1
        CL3IF_OUTNO_02 = 0x0002, // OUT2
        CL3IF_OUTNO_03 = 0x0004, // OUT3
        CL3IF_OUTNO_04 = 0x0008, // OUT4
        CL3IF_OUTNO_05 = 0x0010, // OUT5
        CL3IF_OUTNO_06 = 0x0020, // OUT6
        CL3IF_OUTNO_07 = 0x0040, // OUT7
        CL3IF_OUTNO_08 = 0x0080, // OUT8
        CL3IF_OUTNO_ALL = 0x00FF // ALL
    }
    public sealed class PinnedObject : IDisposable
    {
        private GCHandle _Handle;

        public IntPtr Pointer
        {
            get { return _Handle.AddrOfPinnedObject(); }
        }

        public PinnedObject(object target)
        {
            _Handle = GCHandle.Alloc(target, GCHandleType.Pinned);
        }

        public void Dispose()
        {
            _Handle.Free();
            _Handle = new GCHandle();
        }
    }


}


//{
//    double sum = 0;
//    Method method = new Method();
//    method.OpenEthernetCommunication();
//    Thread.Sleep(1000);
//    for (int i = 0; i < times; i++)
//    {
//        sum += method.MeasurementData();
//        Thread.Sleep(measureDelayTime);
//    }


//internal int times;
//internal int measureDelayTime;
//public double mesureResult;
//private void button1_Click(object sender, EventArgs e)//保存
//{
//    if (!int.TryParse(textBox1.Text, out times))
//    {
//        MessageBox.Show(this, "请输入有效值");
//        return;
//    }
//    else if (times <= 0)
//    {
//        MessageBox.Show(this, "请输入有效值");
//        return;
//    }


//    if (!int.TryParse(textBox2.Text, out measureDelayTime))
//    {
//        MessageBox.Show(this, "请输入有效值");
//        return;
//    }
//    else if (measureDelayTime <= 0)
//    {
//        MessageBox.Show(this, "请输入有效值");
//        return;
//    }
//    MessageBox.Show(this, "保存成功");
//}
//internal double GetMeasureDate()
//{
//    double sum = 0;
//    Method method = new Method();
//    method.OpenEthernetCommunication();
//    Thread.Sleep(1000);
//    for (int i = 0; i < times; i++)
//    {
//        sum += method.MeasurementData();
//        Thread.Sleep(measureDelayTime);
//    }
//    return sum / times;
//}

//private void button2_Click(object sender, EventArgs e)
//{

//    mesureResult = GetMeasureDate();
//    textBox4.Text = Convert.ToString(mesureResult);
//}


//static class Program
//{
//    static void Main()
//    {
//        Method communicate = new Method();
//        communicate.OpenEthernetCommunication();//通讯连接
//        ////communicate.CloseEthernetCommunication();//断开连接
//        //communicate.ResetSingle();//单个复位    感觉没用
//        //int i = communicate.GetMeasurementData();//获得测量值，使用时需除以10000
//        Console.WriteLine(communicate.GetMeasureDate(communicate, 5, 100));
//        //communicate.AutoZeroSingle(false);//归零       等同于卖菜去皮功能
//        Console.ReadKey();
//    }
//}