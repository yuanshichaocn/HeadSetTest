using BaseDll;
using log4net;
using SerialDict;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace OtherDevice
{
    public class SerialPortParam
    {
        #region 属性

        /// <summary>
        ///串口号
        /// </summary>
        public int m_nComNo = 7;

        /// <summary>
        ///串口定义名称
        /// </summary>
        public string m_strName = "";

        /// <summary>
        ///波特率
        /// </summary>
        public int m_nBaudRate = 19200;

        /// <summary>
        ///数据位
        /// </summary>
        public int m_nDataBit = 8;

        /// <summary>
        ///校验位
        /// </summary>
        public string m_strPartiy = "None";

        /// <summary>
        ///停止位
        /// </summary>
        public string m_strStopBit = "1";

        /// <summary>
        ///流控制
        /// </summary>
        public string m_strFlowCtrl = "None";

        /// <summary>
        ///超时时间
        /// </summary>
        public int m_nTime = 1000;

        /// <summary>
        ///缓冲区大小
        /// </summary>
        public int m_nBufferSzie = 4096;

        /// <summary>
        ///命令分隔符标志
        /// </summary>
        public string m_strLineFlag = "CRLF";

        /// <summary>
        ///命令分隔符
        /// </summary>
        private string m_strLine;

        #endregion 属性
    }

    public struct LightSet
    {
        public LightSet(int ch, int val)
        {
            nCh = ch;
            lightval = val;
        }

        public int nCh;
        public int lightval;
    }

    public class LightControler
    {
        private ILog logger = LogManager.GetLogger("LightControl");
        public SerialDictionary<string, LightSet> itemlightdic = new SerialDictionary<string, LightSet>();

        public string LightName = "";

        public object ReadParam()
        {
            object obj = null;
            object obj2 = null;
            try
            {
                obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"config\Light.xml", lightSerialPortParam.GetType());
                if (obj != null)
                    lightSerialPortParam = (SerialPortParam)obj;
            }
            catch
            {
                obj = null;
            }
            if (obj == null)
            {
                MessageBox.Show("lingt control 串口 数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return obj;
        }

        public object ReadParam2()
        {
            object obj2 = null;
            try
            {
                // obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"config\Light.xml", lightSerialPortParam.GetType());
                // if (obj != null)
                //      lightSerialPortParam = (LightSerialPortParam)obj;
                //obj2 = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"\config\Light" + ".xml", itemlightdic.GetType());
                obj2 = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"config\Light.xml", lightSerialPortParam.GetType());
                if (obj2 != null)
                {
                    itemlightdic = (SerialDictionary<string, LightSet>)obj2;
                    if (itemlightdic.Count == 0)
                    {
                        MessageBox.Show("lingt control  数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error("lingt control  数据读取出错");
                    }
                }
                else
                {
                    SaveItems();
                }
            }
            catch
            {
                obj2 = null;
            }
            if (obj2 == null)
            {
                MessageBox.Show("lingt control  数据读取出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return obj2;
        }

        private SerialPortParam lightSerialPortParam = new SerialPortParam();

        public void Save()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"\config\Light.xml";
            AccessXmlSerializer.ObjectToXml(strPath, lightSerialPortParam);
        }

        public void SetItem(string itemName, int nCh, int val)
        {
            logger.Info("光源控制器：" + "设置" + itemName + string.Format(" 通道{0}：，亮度值{1}", nCh, val));
            if (!itemlightdic.ContainsKey(itemName))
                itemlightdic.Add(itemName, new LightSet(nCh, val));
            else
                itemlightdic[itemName] = new LightSet(nCh, val);
        }

        public void SaveItems()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"config\LightVal_" + LightName + ".xml";
            if (File.Exists(strPath))
                File.Delete(strPath);
            AccessXmlSerializer.ObjectToXml(strPath, itemlightdic);
        }

        private SerialPort sp = new SerialPort();
        private bool CommConn; // 通訊是否連接

        public LightControler()
        {
            //ResetReadBuffer();
        }

        private SerialPort _serialPort;

        //初始化光源控制器串口
        public bool InitLightComport()
        {
            if (ReadParam() == null)
            {
                Save();
            }
            bool result = true;
            try
            {
                if (_serialPort == null)
                    _serialPort = new SerialPort("COM" + lightSerialPortParam.m_nComNo.ToString())
                    {
                        BaudRate = lightSerialPortParam.m_nBaudRate,
                        DataBits = lightSerialPortParam.m_nDataBit,
                        Parity = (Parity)Enum.Parse(typeof(Parity), lightSerialPortParam.m_strPartiy),
                        StopBits = (StopBits)Enum.Parse(typeof(StopBits), lightSerialPortParam.m_strStopBit)
                    };
                if (_serialPort.IsOpen)
                    _serialPort.Close();
                _serialPort.ReadTimeout = 300;
                _serialPort.WriteTimeout = 300;
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                result = _serialPort.IsOpen;

                logger.Info("光源控制器开打串口成功");
                return result;
            }
            catch (Exception exp)
            {
                logger.Warn("光源控制器开打串口失败：" + exp.Message);
                MessageBox.Show("光源控制器开打串口失败：" + exp.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                throw new Exception(exp.Message);
            }
        }

        #region Open / Close Procedures

        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            //Ensure port isn't already opened:
            if (!sp.IsOpen)
            {
                //Assign desired settings to the serial port:
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = databits;
                sp.Parity = parity;
                sp.StopBits = stopBits;
                //These timeouts are default and cannot be editted through the class at this point:
                sp.ReadTimeout = 1000;
                sp.WriteTimeout = 1000;
                sp.DtrEnable = true;

                try
                {
                    sp.Open();
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
                return true;
            }
            return false;
        }

        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Close();
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
                return true;
            }
            return false;
        }

        #endregion Open / Close Procedures

        //发送亮度指令
        public bool SerialSend(string command)
        {
            try
            {
                _serialPort.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送数据时发生错误！" + ex.Message);
            }
            return true;
        }

        public bool SerialSend(byte[] command)
        {
            if (command == null || command.Length <= 0)
                return false;
            try
            {
                _serialPort.Write(command, 0, command.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送数据时发生错误！" + ex.Message);
            }
            return true;
        }

        private object objlock = new object();

        /// <summary>
        /// 设置亮度值
        /// </summary>
        /// <param name="lightValue">亮度值</param>
        /// <param name="cHNum">通道号</param>
        public void SetLight(int nChnum, int nlightValue)
        {
            //参数合法性判断
            if (nChnum < 0 || nChnum >= 7 || nlightValue == null)
            {
                return;
            }

            byte[] senddata = new byte[8];
            senddata[0] = 0x40;
            senddata[1] = 0x05;
            senddata[2] = 0x01;
            senddata[3] = 0x00;
            senddata[4] = 0x1a;
            senddata[5] = (byte)nChnum;
            senddata[6] = (byte)nlightValue;
            int sum = 0;
            for (int i = 0; i < 7; i++)
            {
                sum += senddata[i];
            }
            senddata[7] = (byte)(sum & 0xff);
            lock (objlock)
            {
                SerialSend(senddata);
            }
        }

        public void Light(string ItemName)
        {
            if (itemlightdic.ContainsKey(ItemName))
            {
                // logger.Info("光源控制器：" + "点亮" +
                //     ItemName + string.Format(" 通道{0}：，亮度值{1}", itemlightdic[ItemName].nCh, itemlightdic[ItemName].lightval));
                Light(itemlightdic[ItemName].nCh, itemlightdic[ItemName].lightval);
            }
            else
            {
                // logger.Info("光源控制器：" + "点亮" +
                //    ItemName + string.Format(" 通道{0}：，亮度值{1}", itemlightdic[ItemName].nCh, itemlightdic[ItemName].lightval)+"没有此项目");
                MessageBox.Show(ItemName + " :没有此项目 ", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CloseLight(string ItemName)
        {
            if (itemlightdic.ContainsKey(ItemName))
            {
                //logger.Info("光源控制器：" + "熄灭" +
                //    ItemName + string.Format(" 通道{0}：，亮度值{1}", itemlightdic[ItemName].nCh, itemlightdic[ItemName].lightval));
                Light(itemlightdic[ItemName].nCh, 0);
            }
            else
            {
                // logger.Info("光源控制器：" + "熄灭" +
                //    ItemName + string.Format(" 通道{0}：，亮度值{1}", itemlightdic[ItemName].nCh, itemlightdic[ItemName].lightval) + "没有此项目");
                MessageBox.Show(ItemName + " :没有此项目 ", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CloseLight(int nChnum)
        {
            if (nChnum < 0 || nChnum >= 7)
            {
                return;
            }
            int nlightValue = 0;

            byte[] senddata = new byte[8];
            senddata[0] = 0x40;
            senddata[1] = 0x05;
            senddata[2] = 0x01;
            senddata[3] = 0x00;
            senddata[4] = 0x1a;
            senddata[5] = (byte)nChnum;
            senddata[6] = (byte)nlightValue;
            int sum = 0;
            for (int i = 0; i < 7; i++)
            {
                sum += senddata[i];
            }
            senddata[7] = (byte)(sum & 0xff);

            lock (objlock)
            {
                SerialSend(senddata);
            }
        }

        public void Light(int nChnum, int lightval)
        {
            if (nChnum < 0 || nChnum >= 7)
            {
                return;
            }
            int nlightValue = 0;

            byte[] senddata = new byte[8];
            senddata[0] = 0x40;
            senddata[1] = 0x05;
            senddata[2] = 0x01;
            senddata[3] = 0x00;
            senddata[4] = 0x1a;
            senddata[5] = (byte)nChnum;
            senddata[6] = (byte)lightval;
            int sum = 0;
            for (int i = 0; i < 7; i++)
            {
                sum += senddata[i];
            }
            senddata[7] = (byte)(sum & 0xff);

            lock (objlock)
            {
                SerialSend(senddata);
            }
        }
    }

    public class GripperTeachingPara : INotifyPropertyChanged
    {
        private int index;
        private GripperTeachingMode mode;
        private int movePosition;
        private int speed;
        private int porque;
        private int sign;
        private double positionLimitPositive;
        private double positionLimitNegative;
        private int delayTime;
        private int order;

        /// <summary>
        /// 程序序号,必须设置
        /// </summary>
        public int Index
        {
            get => index;
            set
            {
                index = value; NotifyPropertyChanged("Index");
            }
        }

        /// <summary>
        /// 模式选择
        /// </summary>
        public GripperTeachingMode Mode
        {
            get => mode;
            set
            {
                mode = value; NotifyPropertyChanged("Mode");
            }
        }

        /// <summary>
        /// 移动量
        /// </summary>
        public int MovePosition
        {
            get => movePosition;
            set
            {
                movePosition = value; NotifyPropertyChanged("MovePosition");
            }
        }

        /// <summary>
        /// 速度（0802h（%））
        /// </summary>
        public int Speed
        {
            get => speed;
            set
            {
                speed = value; NotifyPropertyChanged("Speed");
            }
        }

        /// <summary>
        /// 扭力（*0.1%）
        /// </summary>
        public int Torque
        {
            get => porque;
            set
            {
                porque = value; NotifyPropertyChanged("Torque");
            }
        }

        /// <summary>
        /// 信号探测模式
        /// </summary>
        public int Sign
        {
            get => sign;
            set
            {
                sign = value; NotifyPropertyChanged("Sign");
            }
        }

        /// <summary>
        /// 行程下限
        /// </summary>
        public double PositionLimitNegative
        {
            get => positionLimitNegative;
            set
            {
                positionLimitNegative = value; NotifyPropertyChanged("PositionLimitNegative");
            }
        }

        /// <summary>
        /// 行程上限
        /// </summary>
        public double PositionLimitPositive
        {
            get => positionLimitPositive;
            set
            {
                positionLimitPositive = value; NotifyPropertyChanged("PositionLimitPositive");
            }
        }

        /// <summary>
        /// 延迟时间（ms）
        /// </summary>
        public int DelayTime
        {
            get => delayTime;
            set
            {
                delayTime = value; NotifyPropertyChanged("DelayTime");
            }
        }

        /// <summary>
        /// 次序，默认-1表示不执行其他Index序号的示教点，其他数字表示执行Index序号
        /// </summary>
        public int Order
        {
            get => order;
            set
            {
                order = value; NotifyPropertyChanged("Order");
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// 程序设置模式枚举
    /// </summary>
    public enum GripperTeachingMode
    {
        /// <summary>
        /// 相對位置定位
        /// </summary>
        INC,

        /// <summary>
        /// 絕對位置定位
        /// </summary>
        ABS,

        /// <summary>
        /// 原點復歸
        /// </summary>
        ORG,

        /// <summary>
        /// + 向扭力極限探測
        /// </summary>
        TLSAdd,

        /// <summary>
        ///  - 向扭力極限探測 - 向扭力極限探測
        /// </summary>
        TLSSub,

        /// <summary>
        /// + 信號探測
        /// </summary>
        SIGAdd,

        /// <summary>
        /// - 信號探測
        /// </summary>
        SIGSub,

        /// <summary>
        ///  位置設置
        /// </summary>
        SET,

        /// <summary>
        /// CLR 清除偏差計數
        /// </summary>
        CLR,

        /// <summary>
        /// OUTI 通用輸出─即時
        /// </summary>
        OUTI,

        /// <summary>
        /// OUTB 通用輸出─座標比較(大 )
        /// </summary>
        OUTB,

        /// <summary>
        /// OUTS 通用輸出─座標比較(小 )
        /// </summary>
        OUTS,

        /// <summary>
        /// SETC 設定計次
        /// </summary>
        SETC,

        /// <summary>
        /// JNZ 將計次結果減 1 後，如為 0 則執行下一個步驟；如結果並非為 0，則會執行下一步驟中所指定編號之程式
        /// </summary>
        JNZ,
    }

    /// <summary>
    /// 运动模式，对应寄存器2015H中的值
    /// </summary>
    public enum MotionType
    {
        Clear = 0,//结束是寄存器状态复位
        Home = 1,//回零
        Open = 3,//打开抓手
        Close = 5,//关闭抓手
        End = 8,//走相对位移0，保证读取1026时不会在同一个状态（同一个状态不能多次回零）
    }

    public class SerialPortParamCommon
    {
        #region 属性

        /// <summary>
        ///串口号
        /// </summary>
        public int m_nComNo { get; set; }

        /// <summary>
        ///串口定义名称
        /// </summary>
        public string m_strName { get; set; }

        /// <summary>
        ///波特率
        /// </summary>
        public int m_nBaudRate { get; set; }

        /// <summary>
        ///数据位
        /// </summary>
        public int m_nDataBit { get; set; }

        /// <summary>
        ///校验位
        /// </summary>
        public int m_strPartiy { get; set; }

        /// <summary>
        ///停止位
        /// </summary>
        public int m_strStopBit { get; set; }

        /// <summary>
        ///流控制
        /// </summary>
        public string m_strFlowCtrl { get; set; }

        /// <summary>
        ///超时时间
        /// </summary>
        public int m_nTime { get; set; }

        /// <summary>
        ///缓冲区大小
        /// </summary>
        public int m_nBufferSzie { get; set; }

        /// <summary>
        ///命令分隔符标志
        /// </summary>
        public string m_strLineFlag { get; set; }

        /// <summary>
        ///命令分隔符
        /// </summary>
        private string m_strLine { get; set; }

        #endregion 属性
    }
}