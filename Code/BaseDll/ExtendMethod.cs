using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDll
{
    public static class ExtendMethod
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.ScrollToCaret();
        }

        public static int  ToInt(this string s)
        {
            if (int.TryParse(s, out int i))
                return i;
            else
                return 0;
        }

        public static byte ToByte(this string s)
        {
            if (byte.TryParse(s, out byte i))
                return i;
            else
                return 0;
        }

        public static ushort ToUshort(this string s)
        {
            if (ushort.TryParse(s, out ushort i))
                return i;
            else
                return 0;
        }

        public static double ToDouble(this string s)
        {
            if (double.TryParse(s, out double i))
                return i;
            else
                return 0;
        }

        public static string ToString(this byte[] bytes, int startIndex = 0)
        {
            return BitConverter.ToString(bytes, startIndex);
        }

        public static string ToUTF8String(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static void AppendTextColorful(this RichTextBox rtBox, string text, Color color, bool addNewLine = true)
        {
            string[] str;
            if (addNewLine)
                text += Environment.NewLine;
            rtBox.SelectionStart = rtBox.TextLength;
            rtBox.SelectionLength = 0;
            rtBox.SelectionColor = color;
            rtBox.AppendText(text);
            rtBox.SelectionColor = rtBox.ForeColor;

        }

    }
    public static class FileOpert
    {
      
        /// <summary>
        /// 删除文件夹及其内容
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                foreach (string content in Directory.GetFileSystemEntries(dirPath))
                {
                    if (Directory.Exists(content))
                    { Directory.Delete(content, true); }
                    else if (File.Exists(content))
                    { File.Delete(content); }
                }
                Directory.Delete(dirPath);
            }
        }

        private static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }

    }

    public static class UserConveter
    {

        public static IntPtr BytesToInptr(byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buff = Marshal.AllocHGlobal(size);
            try
            {

                Marshal.Copy(bytes, 0, buff, size);
                return buff;

            }
            finally
            {
                Marshal.FreeHGlobal(buff);
            }



        }
        public static object BytesToStruct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, type);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

        }
        public static byte[] StructToBytes(object stcObj)
        {

            int size = Marshal.SizeOf(stcObj);
            IntPtr buff = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(stcObj, buff, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buff, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buff);
            }
        }



    }

}
