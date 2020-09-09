using System;
using System.Runtime.InteropServices;

namespace GTN
{
    public class glink
    {
        public struct Program_Prm
        {
            public byte[] databuf;
            public UInt32 datalength;
        }

        [DllImport("gts.dll")]
        public static extern short GT_SetGLinkDo(short slaveno, ushort offset, ref byte pData, ushort bytelength);
        [DllImport("gts.dll")]
        public static extern short GT_GetGLinkDi(short slaveno, ushort offset, out byte pData, ushort bytelength);
        [DllImport("gts.dll")]
        public static extern short GT_SetGLinkDoBit(short slaveno, ushort offset, short diIndex, byte Value);
        [DllImport("gts.dll")]
        public static extern short GT_GetGLinkDiBit(short slaveno, ushort offset, short diIndex, out byte pValue);
        [DllImport("gts.dll")]
        public static extern short GT_SetGLinkAo(short slaveno, ushort channel, ref short data, ushort bytelength);
        [DllImport("gts.dll")]
        public static extern short GT_GetGLinkAi(short slaveno, ushort channel, out short data, ushort bytelength);
        [DllImport("gts.dll")]
        public static extern short GT_GetGLinkOnlineSlaveNum(out byte pSlavenum);
        [DllImport("gts.dll")]
        public static extern short GT_SetGLinkModuleConfig(ref string pFile);
        [DllImport("gts.dll")]
        public static extern short GT_GLinkInit(short cardNum);
    }
}