using BaseDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpsonRobot
{
    interface IScaraRobot
    {
        bool Jump(Coordinate coordinate, HandDirection direction, double limz, bool bCheckHandleSys);
        bool Go(Coordinate coordinate, HandDirection direction,bool  bCheckHandleSys);
        bool Move(Coordinate coordinate, HandDirection direction, bool bCheckHandleSys);
        void Stop();
        bool Home();
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
