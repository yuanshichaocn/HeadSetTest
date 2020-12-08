using HalconDotNet;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserCtrl
{
    /// <summary>
    /// 控件刷新接口,由当前占用该控件的类来负责刷新
    /// </summary>
    public interface IVisionControlUpdate
    {
        /// <summary>
        /// 界面刷新函数
        /// </summary>
        /// <param name="ctl"></param>
        void UpdateVisionControl(VisionControl ctl);

        //    bool ChangeValid();
    }

    /// <summary>
    /// 图像处理显示控件
    /// </summary>
    public partial class VisionControl : PictureBox
    {
        private HTuple m_windowHandle = null;       //图像显示控件的句柄
        private Point ptMouse;
        private object imgLock = new object();
        private IVisionControlUpdate m_IVisionControlUpdate = null;

        public int ImgWidth
        {
            set => m_nWidth = value;
            get => m_nWidth;
        }

        public int ImgHight
        {
            set => m_nHeight = value;
            get => m_nHeight;
        }

        private int m_nWidth = 2592;
        private int m_nHeight = 1944;

        /// <summary>
        /// 构造函数
        /// </summary>
        public VisionControl()
        {
            InitializeComponent();
            //this.BackColor = System.Drawing.Color.Yellow;
            //Pen blackPen = new Pen(Color.Red, 3);
            //Point p1 = new Point(this.Width/2, 0);
            //Point p2 = new Point(this.Width/2, this.Height);
            //Graphics g = this.CreateGraphics();
            //g.DrawLine(blackPen, p1, p2);
        }

        /// <summary>
        /// 初始化halcon窗口,分辨率为2592 * 1944
        /// </summary>
        ///
   //     [DllImport("user32.dll")]static extern IntPtr GetWindowDC(IntPtr hWnd);
        public void InitWindow()
        {
            try
            {
                //          HOperatorSet.NewExternWindow(this.Handle, 0, 0, this.Width, this.Height, out m_windowHandle);
                //          HOperatorSet.SetWindowDc(m_windowHandle, GetWindowDC(this.Handle));
                HOperatorSet.OpenWindow(0, 0, this.Width, this.Height, this.Handle, "", "", out m_windowHandle);
                HOperatorSet.SetWindowParam(m_windowHandle, "background_color", "#000040");
                HOperatorSet.ClearWindow(m_windowHandle);
                HOperatorSet.SetPart(m_windowHandle, 0, 0, m_nHeight, m_nWidth);
            }
            catch (HalconException HDevExpDefaultException1)
            {
                System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());
            }
        }

        /// <summary>
        /// 关闭halcon窗口
        /// </summary>
        public void DeinitWindow()
        {
            if (isOpen())
            {
                HOperatorSet.CloseWindow(m_windowHandle);
            }
        }

        /// <summary>
        /// 注册新的显示接口
        /// </summary>
        /// <param name="vsu"></param>
        public void RegisterUpdateInterface(IVisionControlUpdate vsu)
        {
            if (m_IVisionControlUpdate != vsu)
            {
                m_IVisionControlUpdate = vsu;
            }
        }

        /// <summary>
        /// 判断当前halcon窗口是否已经打开
        /// </summary>
        /// <returns></returns>
        public bool isOpen()
        {
            return m_windowHandle != null;
        }

        /// <summary>
        /// 获取当前的halcon句柄
        /// </summary>
        /// <returns></returns>
        public HTuple GetHalconWindow()
        {
            return m_windowHandle;
        }

        /// <summary>
        /// 锁定控件显示,其它线程不得进入
        /// </summary>
        public void LockDisplay()
        {
            System.Threading.Monitor.Enter(imgLock);
        }

        /// <summary>
        /// 解锁控件显示,其它线程可操作
        /// </summary>
        public void UnlockDisplay()
        {
            System.Threading.Monitor.Exit(imgLock);
        }

        private object lockObj = new object();

        public HObject Img
        {
            private set
            {
                m_hImg = value;
            }
            get
            {
                lock (lockObj)
                {
                    return m_hImg;
                }
            }
        }

        private HObject m_hImg;

        /// <summary>
        /// 全屏显示图像
        /// </summary>
        /// <param name="img"></param>
        public void DispImageFull(HObject img)
        {
            try
            {
                if (!isOpen())
                    InitWindow();
                if (img == null || !img.IsInitialized())
                    return;
                HTuple hv_Width = null, hv_Height = null;
                HOperatorSet.GetImageSize(img, out hv_Width, out hv_Height);
                if (hv_Width != m_nWidth || hv_Height != m_nHeight && hv_Width != null)
                {
                    m_nWidth = hv_Width;
                    m_nHeight = hv_Height;
                    double imgRatio = m_nWidth / (double)m_nHeight;
                    double winRatio = Width / (double)Height;
                    HTuple startX = new HTuple();
                    HTuple startY = new HTuple();
                    HTuple width = new HTuple();
                    HTuple height = new HTuple();
                    if (imgRatio >= winRatio)
                    {
                        width = m_nWidth;
                        height = m_nWidth / winRatio;
                        startX = 0;
                        startY = (height - m_nHeight) / 2;
                    }
                    else
                    {
                        width = m_nHeight * winRatio;
                        height = m_nHeight;
                        startX = (width - m_nWidth) / 2;
                        startY = 0;
                    }
                    HOperatorSet.SetPart(m_windowHandle, -startY, -startX, height - startY - 1, width - startX - 1);
                }
                HOperatorSet.DispImage(img, m_windowHandle);
                HObject ht_CrossLineV, ht_CrossLineH;
                HOperatorSet.SetColor(m_windowHandle, "red");
                HOperatorSet.GenRegionLine(out ht_CrossLineV, 0, m_nWidth / 2, m_nHeight, m_nWidth / 2);
                HOperatorSet.GenRegionLine(out ht_CrossLineH, m_nHeight / 2, 0, m_nHeight / 2, m_nWidth);
                HOperatorSet.DispObj(ht_CrossLineV, m_windowHandle);
                HOperatorSet.DispObj(ht_CrossLineH, m_windowHandle);
                lock (lockObj)
                {
                    if (m_hImg != null && m_hImg.IsInitialized())
                    {
                        m_hImg.Dispose();
                        m_hImg = null;
                    }
                    HOperatorSet.CopyImage(img, out m_hImg);
                }
            }
            catch (Exception ex)
            { }
        }

        public HObject DrawRectangle(out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2)
        {
            this.Focus();
            HTuple row0, col0/*, row1, col1, row2, col2*/;
            row1 = col1 = row2 = col2 = 0;
            HObject hObject;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                    HTuple width = (col1 - col0) / 2;
                    HTuple height = (row1 - row0) / 2;
                    HTuple RowC = (row0 + row1) / 2;
                    HTuple ColC = (col0 + col1) / 2;
                    HOperatorSet.DrawRectangle1Mod(m_windowHandle, RowC - height / 2, ColC - width / 2, RowC + height / 2, ColC + width / 2,
                       out row1, out col1, out row2, out col2);
                    //HOperatorSet.DrawRectangle1(m_windowHandle, out row1, out col1, out row2, out col2);
                    HOperatorSet.SetDraw(m_windowHandle, "margin");
                    HObject hobj = null;

                    HOperatorSet.GenRectangle1(out hobj, row1, col1, row2, col2);

                    if (m_hImg != null && m_hImg.IsInitialized())
                    {
                        HOperatorSet.DispObj(m_hImg, m_windowHandle);
                    }
                    HOperatorSet.SetPart(m_windowHandle, row0, col0, row1, col1);
                    HOperatorSet.DispRectangle1(m_windowHandle, row1, col1, row2, col2);
                    return hobj;
                }
                catch { }
                finally
                {
                    UnlockDisplay();
                }
            }
            return null;
        }

        public HObject DrawRectangle2(out HTuple rowC, out HTuple colC, out HTuple len1, out HTuple len2, out HTuple phi)
        {
            this.Focus();
            HTuple row0, col0, row1, col1/*, row1, col1, row2, col2*/;
            len2 = 50;
            len1 = 50;
            phi = 0;
            rowC = colC = 100;
            HTuple Nowlen1 = 50, Nowlen2 = 50, Nowphi = 0;
            HObject hObject;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                    HTuple width = (col1 - col0) / 2;
                    HTuple height = (row1 - row0) / 2;
                    HTuple RowC = (row0 + row1) / 2;
                    HTuple ColC = (col0 + col1) / 2;
                    Nowlen1 = width / 4;
                    Nowlen2 = height / 4;
                    Nowphi = 0;
                    HOperatorSet.DrawRectangle2Mod(m_windowHandle, RowC, ColC, Nowphi, Nowlen1, Nowlen2, out rowC, out colC, out len1, out len2, out phi);

                    //HOperatorSet.DrawRectangle1(m_windowHandle, out row1, out col1, out row2, out col2);
                    HOperatorSet.SetDraw(m_windowHandle, "margin");
                    HObject hobj = null;
                    HOperatorSet.GenRectangle2(out hobj, rowC, colC, len1, len2, phi);

                    if (m_hImg != null && m_hImg.IsInitialized())
                        HOperatorSet.DispObj(m_hImg, m_windowHandle);
                    HOperatorSet.SetPart(m_windowHandle, row0, col0, row1, col1);
                    HOperatorSet.DispRectangle2(m_windowHandle, rowC, colC, len1, len2, phi);
                    return hobj;
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    UnlockDisplay();
                }
            }
            return null;
        }

        public HObject DrawCircle(out HTuple rowCircle, out HTuple colCircle, out HTuple Rudiu)
        {
            this.Focus();
            HTuple row0, col0/*, row1, col1, row2, col2*/;
            HTuple row1; HTuple col1;
            HTuple row2; HTuple col2;
            row1 = col1 = row2 = col2 = 0;
            rowCircle = colCircle = Rudiu = 0;
            HObject hObject;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                    HTuple width = (col1 - col0) / 2;
                    HTuple height = (row1 - row0) / 2;
                    HTuple RowC = (row0 + row1) / 2;
                    HTuple ColC = (col0 + col1) / 2;
                    HTuple tupleRudiu = 0;
                    if (width >= height)
                        tupleRudiu = height / 2;
                    else
                        tupleRudiu = width / 2;
                    HOperatorSet.DrawCircleMod(m_windowHandle, RowC, ColC, tupleRudiu, out rowCircle, out colCircle, out Rudiu);

                    if (Rudiu == null || Rudiu.Length <= 0 || Rudiu.Length <= 0 || Rudiu[0].D <= 0)
                        return null;
                    HOperatorSet.SetDraw(m_windowHandle, "margin");
                    HObject hobj = null;
                    HOperatorSet.GenCircle(out hobj, rowCircle, colCircle, Rudiu);
                    //if (m_hImg != null && m_hImg.IsInitialized())
                    //    HOperatorSet.DispObj(m_hImg, m_windowHandle);
                    HOperatorSet.SetPart(m_windowHandle, row0, col0, row1, col1);
                    HOperatorSet.DispCircle(m_windowHandle, rowCircle, colCircle, Rudiu);
                    return hobj;
                }
                catch { }
                finally
                {
                    UnlockDisplay();
                }
            }
            return null;
        }

        public HObject DrawCircleMod(HTuple RowC, HTuple ColC, HTuple tupleRudiu, out HTuple rowCircle, out HTuple colCircle, out HTuple Rudiu)
        {
            this.Focus();
            HTuple row0, col0/*, row1, col1, row2, col2*/;
            HTuple row1; HTuple col1;
            HTuple row2; HTuple col2;
            row1 = col1 = row2 = col2 = 0;
            rowCircle = colCircle = Rudiu = 0;
            HObject hObject;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);

                    HOperatorSet.DrawCircleMod(m_windowHandle, RowC, ColC, tupleRudiu, out rowCircle, out colCircle, out Rudiu);

                    if (Rudiu == null || Rudiu.Length <= 0 || Rudiu.Length <= 0 || Rudiu[0].D <= 0)
                        return null;
                    HOperatorSet.SetDraw(m_windowHandle, "margin");
                    HObject hobj = null;
                    HOperatorSet.GenCircle(out hobj, rowCircle, colCircle, Rudiu);
                    //if (m_hImg != null && m_hImg.IsInitialized())
                    //    HOperatorSet.DispObj(m_hImg, m_windowHandle);
                    HOperatorSet.SetPart(m_windowHandle, row0, col0, row1, col1);
                    HOperatorSet.DispCircle(m_windowHandle, rowCircle, colCircle, Rudiu);
                    return hobj;
                }
                finally
                {
                    UnlockDisplay();
                }
            }
            return null;
        }

        public void DrawPoint(out HTuple rowC, out HTuple colC)
        {
            this.Focus();
            HTuple row0, col0;
            HTuple row1, col1;
            rowC = colC = 0;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                    HTuple RowC = (row0 + row1) / 2;
                    HTuple ColC = (col0 + col1) / 2;
                    HOperatorSet.SetColor(m_windowHandle, "red");
                    HOperatorSet.DrawPointMod(m_windowHandle, RowC, ColC, out rowC, out colC);
                    HOperatorSet.SetPart(m_windowHandle, row0, col0, row1, col1);
                    HOperatorSet.DispCross(m_windowHandle, rowC, colC, 80, 0);
                }
                finally
                {
                    UnlockDisplay();
                }
            }
        }

        public void DrawPointMod(HTuple RowIn, HTuple ColIn, out HTuple rowC, out HTuple colC)
        {
            this.Focus();
            HTuple row0, col0;
            HTuple row1, col1;
            rowC = colC = 0;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                    HTuple RowC = (row0 + row1) / 2;
                    HTuple ColC = (col0 + col1) / 2;
                    HOperatorSet.SetColor(m_windowHandle, "red");
                    HOperatorSet.DrawPointMod(m_windowHandle, RowIn, ColIn, out rowC, out colC);
                    HOperatorSet.SetPart(m_windowHandle, row0, col0, row1, col1);
                    HOperatorSet.DispCross(m_windowHandle, rowC, colC, 80, 0);
                }
                catch { }
                finally
                {
                    UnlockDisplay();
                }
            }
        }

        public void DrawLine(out HTuple StartRow, out HTuple StartCol, out HTuple EndRow, out HTuple EndCol)
        {
            this.Focus();
            HTuple row0, col0;
            HTuple row1, col1;
            StartRow = StartCol = EndRow = EndCol = 0; ;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                    HTuple RowC = (row0 + row1) / 2;
                    HTuple ColC = (col0 + col1) / 2;
                    HOperatorSet.SetColor(m_windowHandle, "red");
                    HOperatorSet.DrawLineMod(m_windowHandle, RowC, ColC / 2, RowC, ColC / 2, out StartRow, out StartCol, out EndRow, out EndCol);
                    HOperatorSet.SetPart(m_windowHandle, row0, col0, row1, col1);
                    HOperatorSet.DispLine(m_windowHandle, StartRow, StartCol, EndRow, EndCol);
                }
                catch { }
                finally
                {
                    UnlockDisplay();
                }
            }
        }

        public void DrawLineMod(HTuple RowIn1, HTuple ColIn1, HTuple RowIn2, HTuple ColIn2, out HTuple StartRow, out HTuple StartCol, out HTuple EndRow, out HTuple EndCol)
        {
            this.Focus();
            HTuple row0, col0;
            HTuple row1, col1;
            StartRow = StartCol = EndRow = EndCol = 0; ;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                    HTuple RowC = (row0 + row1) / 2;
                    HTuple ColC = (col0 + col1) / 2;
                    HOperatorSet.SetColor(m_windowHandle, "red");
                    HOperatorSet.DrawLineMod(m_windowHandle, RowIn1, ColIn1, RowIn2, ColIn2, out StartRow, out StartCol, out EndRow, out EndCol);
                    HOperatorSet.SetPart(m_windowHandle, row0, col0, row1, col1);
                    HOperatorSet.DispLine(m_windowHandle, StartRow, StartCol, EndRow, EndCol);
                }
                finally
                {
                    UnlockDisplay();
                }
            }
        }

        public HObject DrawShape()
        {
            this.Focus();
            HTuple row1; HTuple col1; HTuple row2; HTuple col2;
            HTuple row0, col0/*, row1, col1, row2, col2*/;
            row1 = col1 = row2 = col2 = 0;
            HObject hObject;
            if (isOpen())
            {
                try
                {
                    LockDisplay();
                    HOperatorSet.SetColor(m_windowHandle, "red");
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                    HTuple width = (col1 - col0) / 2;
                    HTuple height = (row1 - row0) / 2;
                    HTuple RowC = (row0 + row1) / 2;
                    HTuple ColC = (col0 + col1) / 2;

                    //HOperatorSet.DrawRectangle1(m_windowHandle, out row1, out col1, out row2, out col2);
                    HOperatorSet.SetDraw(m_windowHandle, "margin");
                    HObject hobj = null;
                    HOperatorSet.DrawRegion(out hobj, m_windowHandle);
                    HOperatorSet.DispObj(hobj, m_windowHandle);
                    //if (m_hImg != null && m_hImg.IsInitialized())
                    //    HOperatorSet.DispObj(m_hImg, m_windowHandle);
                    //HOperatorSet.DispRectangle1(m_windowHandle, row1, col1, row2, col2);
                    return hobj;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    UnlockDisplay();
                }
            }
            return null;
        }

        /// <summary>
        /// 鼠标滚动时缩放图片大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (isOpen())
            {
                HTuple row, col, button;
                HTuple row0, col0, row1, col1;
                bool bUpdate = false;

                LockDisplay();
                try
                {
                    HOperatorSet.GetMposition(m_windowHandle, out row, out col, out button);
                    //Action<object> action = (object obj) =>
                    //{
                    HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);

                    HTuple width = col1 - col0;
                    HTuple height = row1 - row0;

                    //col = ((double)e.X / this.Width) * Width;
                    //row = ((double)e.X / this.Width) * Width;

                    float k = (float)width / m_nWidth;
                    if ((k < 50 && e.Delta < 0) || (k > 0.02 && e.Delta > 0))
                    {
                        HTuple Zoom;
                        if (e.Delta > 0)
                        {
                            Zoom = 1.3;
                        }
                        else
                        {
                            Zoom = 1 / 1.3;
                        }

                        HTuple r1 = (row0 + ((1 - (1.0 / Zoom)) * (row - row0)));
                        HTuple c1 = (col0 + ((1 - (1.0 / Zoom)) * (col - col0)));
                        HTuple r2 = r1 + (height / Zoom);
                        HTuple c2 = c1 + (width / Zoom);

                        HOperatorSet.SetPart(m_windowHandle, r1, c1, r2, c2);
                        //if(e.Delta < 0)
                        HOperatorSet.ClearWindow(m_windowHandle);
                        if (m_hImg != null && m_hImg.IsInitialized())
                            HOperatorSet.DispObj(m_hImg, m_windowHandle);
                        bUpdate = true;
                    }
                    //};
                    //Task t1 = new Task(action, "");
                    //t1.Start();
                    //t1.Wait();
                }
                catch (HalconException HDevExpDefaultException1)
                {
                    System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());
                }
                catch (Exception exp)
                {
                    System.Diagnostics.Debug.WriteLine(exp.ToString());
                }
                finally
                {
                    UnlockDisplay();
                }
                if (bUpdate && m_IVisionControlUpdate != null)
                {
                    Action<object> action = (object obj) =>
                    {
                        m_IVisionControlUpdate.UpdateVisionControl(this);
                    };
                    Task t1 = new Task(action, "");
                    t1.Start();
                    t1.Wait();
                }
            }
        }

        /// <summary>
        /// 鼠标按下时切换光标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_MouseDown(object sender, MouseEventArgs e)
        {
            //if (this.Focused == false)
            //    this.Focus();
            //if(e.Button == MouseButtons.Left)
            //{
            //    this.Cursor = Cursors.Hand;
            //    ptMouse.X = e.X;
            //    ptMouse.Y = e.Y;
            //}

            if (m_IVisionControlUpdate != null)
            {
                m_IVisionControlUpdate.UpdateVisionControl(this);
            }
        }

        /// <summary>
        /// 平移图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isOpen())
            {
                if (this.Cursor == Cursors.Hand)
                {
                    //Action<object> action = (object obj) =>
                    //{
                    int x = e.X - ptMouse.X;
                    int y = e.Y - ptMouse.Y;

                    if (Math.Abs(x) > 2 || Math.Abs(y) > 2)
                    {
                        ptMouse.X = e.X;
                        ptMouse.Y = e.Y;
                        HTuple row0, col0, row1, col1;
                        bool bUpdate = false;
                        LockDisplay();
                        try
                        {
                            HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                            int zoom = (row1 - row0) / this.Height;
                            x *= zoom;
                            y *= zoom;

                            HOperatorSet.SetPart(m_windowHandle, row0 - y, col0 - x, row1 - y, col1 - x);
                            HOperatorSet.ClearWindow(m_windowHandle);
                            if (m_hImg != null && m_hImg.IsInitialized())
                                HOperatorSet.DispObj(m_hImg, m_windowHandle);
                            bUpdate = true;
                        }
                        catch (HalconException HDevExpDefaultException1)
                        {
                            System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());
                        }
                        catch (Exception exp)
                        {
                            System.Diagnostics.Debug.WriteLine(exp.ToString());
                        }
                        finally
                        {
                            UnlockDisplay();
                        }

                        if (bUpdate && m_IVisionControlUpdate != null)
                        {
                            m_IVisionControlUpdate.UpdateVisionControl(this);
                        }
                    }
                    //};
                    //Task t1 = new Task(action, "");
                    //t1.Start();
                    //t1.Wait();
                }
            }
        }

        /// <summary>
        /// 鼠标右键按下时返回全屏显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (isOpen())
                {
                    //return;
                    //Action<object> action = (object obj) =>
                    //{
                    bool bUpdate = false;
                    HTuple row0, col0, row1, col1;
                    LockDisplay();
                    try
                    {
                        HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);

                        if (row0 != 0 || col0 != 0 || col1 - col0 != m_nWidth || row1 - row0 != m_nHeight)
                        {
                            double imgRatio = m_nWidth / (double)m_nHeight;
                            double winRatio = Width / (double)Height;
                            HTuple startX = new HTuple();
                            HTuple startY = new HTuple();
                            HTuple width = new HTuple();
                            HTuple height = new HTuple();
                            if (imgRatio >= winRatio)
                            {
                                width = m_nWidth;
                                height = m_nWidth / winRatio;
                                startX = 0;
                                startY = (height - m_nHeight) / 2;
                            }
                            else
                            {
                                width = m_nHeight * winRatio;
                                height = m_nHeight;
                                startX = (width - m_nWidth) / 2;
                                startY = 0;
                            }
                            HOperatorSet.SetPart(m_windowHandle, -startY, -startX, height - startY - 1, width - startX - 1);
                            bUpdate = true;
                        }
                    }
                    catch (HalconException HDevExpDefaultException1)
                    {
                        System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());
                    }
                    catch (Exception exp)
                    {
                        System.Diagnostics.Debug.WriteLine(exp.ToString());
                    }
                    finally
                    {
                        UnlockDisplay();
                    }

                    if (bUpdate && m_IVisionControlUpdate != null)
                    {
                        m_IVisionControlUpdate.UpdateVisionControl(this);
                    }
                    //};
                    //Task t1 = new Task(action, "");
                    //t1.Start();
                    //t1.Wait();
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (this.Cursor == Cursors.Hand)
                    this.Cursor = Cursors.Arrow;
            }
        }

        private void VisionControl_MouseEnter(object sender, EventArgs e)
        {
            if (this.Focused == false)
                this.Focus();
        }

        private void VisionControl_MouseLeave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 控件缩放时自动调整halcon窗口的大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_SizeChanged(object sender, EventArgs e)
        {
            if (m_windowHandle != null)
            {
                //Action<object> action = (object obj) =>
                //{
                bool bUpdate = false;
                LockDisplay();
                try
                {
                    HOperatorSet.SetWindowExtents(m_windowHandle, this.ClientRectangle.Y, this.ClientRectangle.X, this.ClientRectangle.Width, this.ClientRectangle.Height);
                    bUpdate = true;
                }
                catch (HalconException HDevExpDefaultException1)
                {
                    System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());
                }
                catch (Exception exp)
                {
                    System.Diagnostics.Debug.WriteLine(exp.ToString());
                }
                finally { UnlockDisplay(); }
                if (bUpdate && m_IVisionControlUpdate != null)
                {
                    m_IVisionControlUpdate.UpdateVisionControl(this);
                }
                //};
                //Task t1 = new Task(action, "");
                //t1.Start();
                //t1.Wait();
            }
        }

        private void VisionControl_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics ht;
            ht = e.Graphics;
            Pen ok = new Pen(System.Drawing.Color.Red, 1);
            Point p1 = new Point(this.Width / 2, 0);
            Point p2 = new Point(this.Width / 2, this.Height);
            Point p3 = new Point(0, this.Height / 2);
            Point p4 = new Point(this.Width, this.Height / 2);
            ht.DrawLine(ok, p1, p2);
            ht.DrawLine(ok, p3, p4);
            Pen ok2 = new Pen(System.Drawing.Color.Yellow, 1);
            ht.DrawEllipse(ok2, this.Width / 2 - 5, this.Height / 2 - 5, 10, 10);
        }
    }
}