namespace StationDemo
{
    public static partial class UserConfig
    {
        /// <summary>
        /// 初始化产品数据参数 在Form_Auto上
        /// </summary>
        public static void BandEventOnAutoScreenLoad(Form_Auto formauto)
        {
            // public delegate void ShowSomeOnAutoScreenHander(string dealtype, params object[] osbjs);
            Form_Auto.ShowEventOnAutoScreen = null;
        }

        /// <summary>
        /// 初始化产品数据参数 在Form1上
        /// </summary>
        public static void BandEventOnForm1(Form1 form1)
        {
            // public delegate void ShowSomeOnMainFrameScreenHander(string dealtype, params object[] osbjs);
            //  Form1.ShowEventOnMainFrameScreen ;
        }
    }
}