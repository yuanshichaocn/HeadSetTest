using System;
using System.IO;
using System.Text;
using log4net;
using Newtonsoft.Json;

namespace EpsonRobot
{
    public class TcpParameter
    {
        private readonly ILog _logger = LogManager.GetLogger(nameof(TcpParameter));
        public string DefaultFilePath { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public int PortNum { get; set; }
        public string EndString { get => Encoding.ASCII.GetString(new byte[] { 0x0D, 0x0A }); }

        public int Timeout = 100_000;


        public static TcpParameter Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                TcpParameter entity = new TcpParameter();
                entity.DefaultFilePath = filePath;
                entity.Save(filePath);
            }
            string content = string.Empty;
            using (StreamReader sr = new StreamReader(filePath))
                content = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<TcpParameter>(content);
        }

        public bool Save(string filePath)
        {
            try
            {
                string content = JsonConvert.SerializeObject(this);
                File.WriteAllText(filePath, content);
            }
            catch (Exception ex)
            {
                _logger.Warn("配置文件保存失败：" + ex.Message, ex);
                return false;
            }
            return true;
        }

        public bool Save()
        {
            return Save(DefaultFilePath);
        }
    }
}
