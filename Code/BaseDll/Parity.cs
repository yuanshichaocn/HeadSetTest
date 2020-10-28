using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UserCtrl;
using System.Timers;
using System.ComponentModel;
using System.Reflection;

namespace BaseDll
{

    public static class ParityType
    {

        public static int CRC16(byte[] modbusdata, int length)
        {
            int num3 = 0xffff;
            for (int i = 0; i < length; i++)
            {
                num3 ^= modbusdata[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((num3 & 1) == 1)
                    {
                        num3 = (num3 >> 1) ^ 0xa001;
                    }
                    else
                    {
                        num3 = num3 >> 1;
                    }
                }
            }
            return num3;
        }
        public static int LRC(byte[] modbusdata,int len)
        {
            int LRC = 0;
            for (int i = 0; i < len; i++)
            {
                LRC = (LRC + modbusdata[i]);
            }

            LRC = (~LRC + 1);
            return LRC;
        }

    }

}