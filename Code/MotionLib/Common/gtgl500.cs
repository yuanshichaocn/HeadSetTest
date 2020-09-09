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

        [DllImport("gen.dll")]
        public static extern short GT_SetGLinkDo(short slaveno, ushort offset, ref byte pData, ushort bytelength);
        [DllImport("gen.dll")]
        public static extern short GT_GetGLinkDo(short slaveno, ushort offset, ref byte pData, ushort bytelength);
        [DllImport("gen.dll")]
        public static extern short GT_GetGLinkDi(short slaveno, ushort offset, out byte pData, ushort bytelength);
        [DllImport("gen.dll")]
        public static extern short GT_SetGLinkDoBit(short slaveno, short doIndex, byte value);
        [DllImport("gen.dll")]
        public static extern short GT_GetGLinkDiBit(short slaveno, short diIndex, out byte pValue);
        [DllImport("gen.dll")]
        public static extern short GT_SetGLinkAo(short slaveno, ushort channel, ref short data, ushort count);
        [DllImport("gen.dll")]
        public static extern short GT_GetGLinkAo(short slaveno, ushort channel, ref short data, ushort count);
        [DllImport("gen.dll")]
        public static extern short GT_GetGLinkAi(short slaveno, ushort channel, out short data, ushort count);
        [DllImport("gen.dll")]
        public static extern short GT_GetGLinkOnlineSlaveNum(out byte pSlavenum);
        [DllImport("gen.dll")]
        public static extern short GT_SetGLinkModuleConfig(ref string pFile);
        [DllImport("gen.dll")]
        public static extern short GT_GLinkInit(short cardNum);
        [DllImport("gen.dll")]
        public static extern short GT_GetGLinkDiEx(short slaveno, out byte pData, ushort count);
    }
}