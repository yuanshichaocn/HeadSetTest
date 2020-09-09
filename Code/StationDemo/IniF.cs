using System.Runtime.InteropServices;

namespace EyesSafetyTest
{
    using System.Collections.Concurrent;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.IO;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    /*************读写.ini类*************/

    public class NiceIniWriteAndRead
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        //section: 要写入的段落名

        //key: 要写入的键，如果该key存在则覆盖写入

        //val: key所对应的值

        //filePath: INI文件的完整路径和文件名

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //section：要读取的段落名

        //key: 要读取的键

        //defVal: 读取异常的情况下的缺省值

        //retVal: key所对应的值，如果该key不存在则返回空值

        //size: 值允许的大小

        //filePath: INI文件的完整路径和文件名





        //------------【函数：将字符串写入ini】------------    



        //section: 要写入的段落名

        //key: 要写入的键，如果该key存在则覆盖写入

        //val: key所对应的值

        //filePath: INI文件的完整路径和文件名

        //------------------------------------------------------------------------

        public static bool WriteToIni(string section, string key, string val, string filePath)

        {
            WritePrivateProfileString(section, key, val, filePath);
            if(_iniDic!=null)
            {
                if(_iniDic.ContainsKey(section))
                {
                    if(_iniDic[section].ContainsKey(key))
                    {
                        _iniDic[section][key] = val;
                    }
                }
            }
            return true;

        }





        //------------【函数：从ini读取字符串】------------    
        //section：要读取的段落名

        //key: 要读取的键

        //defVal: 读取异常的情况下的缺省值
        //filePath: INI文件的完整路径和文件名
        //------------------------------------------------------------------------

        public static string ReadFromIni(string section, string key, string def, string filePath)

        {

            StringBuilder retVal = new StringBuilder();
            GetPrivateProfileString(section, key, def, retVal, 500, filePath);
            return retVal.ToString();

        }
        static ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _iniDic;
        public static  ConcurrentDictionary<string, ConcurrentDictionary<string,string>> ReadIniFileToDic(string strPath)
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, string>> dic = new ConcurrentDictionary<string, ConcurrentDictionary<string, string> >();
            string[] str=File.ReadAllLines(strPath);
            //Parallel.For(0, str.Length-1 ,index=>{
            //    str[index++].Split(==)
            //});
            string strkey = "";
            string strSection = "";
            string strParam = "";
            ConcurrentDictionary<string, string>temp = new ConcurrentDictionary<string, string>();
            for (int index=0;index< str.Length;index++)
            {
                if (str[index].Contains("[") && str[index].Contains("]"))
                {
                    strSection = str[index].Replace("[", "");
                    strSection = strSection.Replace("]", "");
                    temp = new ConcurrentDictionary<string, string>();
                }
               if( str[index].Contains("="))
                {
                   string[] strSplit= str[index].Split('=');
                    if (strSplit.Length >= 2)
                    {
                        strkey = strSplit[0];
                        strParam = strSplit[1];
                        temp.TryAdd(strkey, strParam);
                        dic.TryAdd(strSection, temp);
                        
                    }
                }
            }
            _iniDic = dic;
            return dic;
        }
    }




}
