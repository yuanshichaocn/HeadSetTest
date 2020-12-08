namespace StationDemo
{
    public static partial class UserConfig
    {
        private static bool bIsHaveMes = false;

        public static bool InitSamllMesSever()
        {
            if (!bIsHaveMes)
                return true;
            SmallMes.Ins.SeverInit();
            return true;
        }
    }
}