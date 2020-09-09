namespace EpsonRobot
{
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double U { get; set; }

        public Coordinate Copy()
        {
            return new Coordinate()
            {
                X = this.X,
                Y = this.Y,
                Z = this.Z,
                U = this.U
            };
        }
    }
}
