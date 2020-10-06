using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log
{
    public partial class LogHelper : RichTextBox
    {
        public delegate void ShowMsgDelegate(string msg = "");
        public static ShowMsgDelegate EvenShowMsgDelegate;
        public LogHelper()
        {
            EvenShowMsgDelegate += show;
            InitializeComponent();
        }

        public LogHelper(IContainer container)
        {
            container.Add(this);
          
            InitializeComponent();
        }
        private static object obj = new object();
        public static string LogPath = "D:\\LOGddd.txt";
        public static StreamWriter LogFile = null;
        public async static void Write(string Msg, LogType type)
        {
            await Task.Run(() =>
            {
                lock (obj)
                {
                    try
                    {
                        LogFile = new StreamWriter(LogPath, true);//文件保存位置
                        string strlogInfo = null;
                        switch (type)
                        {
                            case LogType.Err:
                                {
                                    strlogInfo = $"[{DateTime.Now.ToString("yyyy-MM-dd ")} {DateTime.Now.ToString("HHmmss:fff")}] Err:{Msg}";
                                }
                                break;
                            case LogType.Info:
                                {
                                    strlogInfo = $"[{DateTime.Now.ToString("yyyy-MM-dd ")} {DateTime.Now.ToString("HHmmss:fff")}] Info:{Msg}";
                                }
                                break;
                            case LogType.Warn:
                                {
                                    strlogInfo = $"[{DateTime.Now.ToString("yyyy-MM-dd ")} {DateTime.Now.ToString("HHmmss:fff")}] Warn:{Msg}";
                                }
                                break;
                        }
                        LogFile.WriteLine(strlogInfo);
                        LogFile.Close();
                        EvenShowMsgDelegate(strlogInfo);
                    }

                    catch (Exception ee)
                    {

                    }
                    finally
                    {

                        LogFile?.Close();

                    }




                }
            });
        }
        private void show(string Msg)
        {
            LogHelper.ShowText.Text += Msg;
        }
    }
    public enum LogType
    {
        Info,
        Err,
        Warn,
    }
}
