using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using log4net;
namespace StationDemo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Mutex.TryOpenExisting("StationDemo", out Mutex mutex) == false)
            {
                mutex = new Mutex(true, "StationDemo");
            }

            if (mutex.WaitOne(1) == false)
            {
                MessageBox.Show("请勿同时打开多个程序", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                mutex.Dispose();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            log4net.Config.XmlConfigurator.Configure();
            Application.Run(new Form1());
        }
    }
}
