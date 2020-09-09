using BaseDll;
using SerialDict;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using log4net;
using System.IO;

namespace LightControler
{
#if false
    public class LightSerialPortParam
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

        public int[] LightValArr = new int[30];
#endregion
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
    public class LightControl
    {
        private ILog logger = LogManager.GetLogger("LightControl");
        public SerialDictionary<string, LightSet> itemlightdic = new SerialDictionary<string, LightSet>();
#region GetInstance()

        private static LightControl _instance;

        public static LightControl GetInstance()
        {
            return _instance ?? (_instance = new LightControl());
        }
#endregion
        public object ReadParam()
        {
            object obj = null;
            object obj2 = null;
            try
            {
                obj = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"config\Light.xml", lightSerialPortParam.GetType());
                if (obj != null)
                    lightSerialPortParam = (LightSerialPortParam)obj;

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
                obj2 = AccessXmlSerializer.XmlToObject(AppDomain.CurrentDomain.BaseDirectory + @"\config\LightVal.xml", itemlightdic.GetType());
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
                    SaveItem();
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
        LightSerialPortParam lightSerialPortParam = new LightSerialPortParam();

        public int GetLightSetVal(int index)
        {
            if (lightSerialPortParam == null)
                return 0;
            return lightSerialPortParam.LightValArr[index];
        }
        public LightSet GetItemLightSet(string name)
        {
            if (itemlightdic.ContainsKey(name))
                return itemlightdic[name];
            else
                return new LightSet(-1, -1);


        }
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
        public void SaveItem()
        {

            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"config\LightVal.xml";
            if (File.Exists(strPath))
                File.Delete(strPath);
            AccessXmlSerializer.ObjectToXml(strPath, itemlightdic);

        }

        SerialPort sp = new SerialPort();
        bool CommConn; // 通訊是否連接
        public LightControl()
        {
            //ResetReadBuffer();
        }
        SerialPort _serialPort;
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
#endregion

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

        object objlock = new object();
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
                lightSerialPortParam.LightValArr[nChnum] = nlightValue;
                Save();
            }

        }
        public void Light(int nChnum)
        {
            int nlightValue = 0;
            if (nChnum < 0 || nChnum >= 7 || nlightValue == null)
            {
                return;
            }
            nlightValue = lightSerialPortParam.LightValArr[nChnum];
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
#endif
}

