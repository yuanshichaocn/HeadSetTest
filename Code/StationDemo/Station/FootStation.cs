using CommonTools;

namespace StationDemo.Station
{
    /// <summary>
    /// 行走工站
    /// </summary>
    public class FootStation : Stationbase
    {
        public FootStation(Stationbase p) : base(p)
        {
        }

        public FootStation(string strname, int[] axis, string[] axisname, params string[] CameraName) : base(strname, axis, axisname, CameraName)
        {
        }

        public override void DealException(string strExcptionName, WaranResult BitOpreatBtn, AlarmItem alarmItem)
        {
            base.DealException(strExcptionName, BitOpreatBtn, alarmItem);
        }

        public override void DoSomeThingBefore()
        {
            base.DoSomeThingBefore();
        }

        public override void EmgDeal()
        {
            base.EmgDeal();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override void ExctionDeal(string strmsg = "")
        {
            base.ExctionDeal(strmsg);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void PauseDeal()
        {
            base.PauseDeal();
        }

        public override void ResumeDeal()
        {
            base.ResumeDeal();
        }

        public override void StopDeal()
        {
            base.StopDeal();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override bool InitStation()
        {
            return base.InitStation();
        }

        protected override void StationWork(int step)
        {
            base.StationWork(step);
        }
    }
}