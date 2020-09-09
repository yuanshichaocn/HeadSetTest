using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpsonRobot
{
    interface IScaraRobot
    {
        void Jump(Coordinate coordinate, HandDirection direction, double limz);
        void Go(Coordinate coordinate, HandDirection direction);
        void Move(Coordinate coordinate);
        void Stop();
        void Home();
        void SetOutput(int index, bool value);
        bool ReadInput(int index);
    }

    public enum HandDirection
    {
        Default = 0,
        Lefty = 1,
        Right = 2
    }
}
