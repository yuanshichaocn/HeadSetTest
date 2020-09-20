﻿using StationDemo.DemoByWHJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserData;

using SmallMesClients;
using System.Windows.Forms;

namespace StationDemo
{
    public partial class LineStation : IPipeline
    {
        public bool CanThrow(string name)
        {

            foreach (var temp in lineSegmentActions)
            {
                if (temp.LineName == name)
                    return temp.IsCanPut(true);
            }

            return true;



        }

        public LineSegementState State(string name)
        {

            foreach (var temp in lineSegmentActions)
            {
                if (temp.LineName == name)
                    return temp.GetState();
            }
            return LineSegementState.UnKnow;
        }


        public bool ToBefore(string name)
        {

            foreach (var temp in lineSegmentActions)
            {
                if (temp.LineName == name)
                {
                    if(name  == "sensor锁付")
                    {
                       LineSegmentAction lineobj = lineSegmentActions.First((t) => { return t.LineName == "sensor"; });
                        lineobj.feedMode = FeedMode.后进料;
                        temp.bOutMotorRunDir = false;
                        temp.LineSegState = LineSegementState.Finish;
                    }
                    else
                    {
                        MessageBox.Show("非 sensor锁付不能调用tobefore");
                    }
                }
                    
            }
            return true;
        }

        public bool WorkEnd(string name)
        {

            foreach (var temp in lineSegmentActions)
            {
                if (temp.LineName == name)
                    temp.LineSegState = LineSegementState.Finish;
            }
            return false;
        }
    }
    public enum LineName
    {
        前壳流水线,
        Sensor流水线,
        锁付流水线,
        前壳NG流水线,
        SensorNG流水线,
        锁付NG流水线,
    }
}
