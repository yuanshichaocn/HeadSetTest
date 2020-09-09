using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using System.Threading.Tasks;
using CameraLib;

namespace StationDemo
{
    public partial class Form_HalconWnd : Form
    {
        public Form_HalconWnd()
        {
            InitializeComponent();
        }
        public HTuple HalconWnd
        {
            get { return m_wnd; }
        }
        private HTuple m_wnd =new HTuple();
        private void Form_HalconWnd_Load(object sender, EventArgs e)
        {
           

            hWindowControl1.BackColor = Color.Blue;
            m_wnd = hWindowControl1.HalconID;
            //HOperatorSet.OpenWindow(0, 0,



            //    pictureBox_WndHalcon.Width,
            //    pictureBox_WndHalcon.Height,
            //    pictureBox_WndHalcon.Handle, "visible", "", out m_wnd);//pictureBox_WndHalcon.Handle
        }

     

        private void button1_Click_1(object sender, EventArgs e)
        {
            #region 读图
            //HObject img;
            //HOperatorSet.ReadImage(out img, "d:\\1.jpg");
            ////hWindowControl1.SetFullImagePart((HImage)(object)img);
            //HOperatorSet.DispObj(img, hWindowControl1.HalconID);
            #endregion
            #region 软触发
            //CameraMgr.GetInstance().GetCamera("CHP").SetTriggerMode("SoftWare");
            //uint nFrameNum =CameraMgr.GetInstance().GetCamera("CHP").FrameCount;
            //CameraMgr.GetInstance().GetCamera("CHP").GarbBySoftTrigger();
            //CameraMgr.GetInstance().GetCamera("CHP");
            //Task s = new Task((icount) =>
            //{
            //    uint frameCount =CameraMgr.GetInstance().GetCamera("CHP").FrameCount;
            //    while (frameCount <= nFrameNum)
            //    {
            //        frameCount = CameraMgr.GetInstance().GetCamera("CHP").FrameCount;
            //        Thread.Sleep(1);
            //    }
            //    HObject img = CameraMgr.GetInstance().GetCamera("CHP").GetImage();
            //    HOperatorSet.DispObj(img, (HTuple)CameraMgr.GetInstance().GetCamera("CHP").wnd);
            //},nFrameNum) ;
            //s.Start();
            #endregion
            string PagesItemName = this.Parent.Name;
            CameraMgr.GetInstance().GetCamera(PagesItemName).wnd = hWindowControl1.HalconID;
            CameraMgr.GetInstance().GetCamera(PagesItemName).StartGrab();
        }
    }
}
