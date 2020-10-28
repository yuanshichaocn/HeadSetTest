
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;

namespace BaseDll
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public static class AccessXmlSerializer
    {
        public static readonly object LockObject = new object();
        private static Hashtable serializers = new Hashtable();
        private static XmlSerializer xs;
        public static LogView log = new LogView();
        /// <summary>
        /// 将对象反序列化为指定XML文件
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="sourceObj">对象</param>
        /// <returns>是否成功</returns>
        public static bool ObjectToXml(string filePath, object sourceObj)
        {
            lock (LockObject)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
                    {
                        string folderPath = filePath.Substring(0, filePath.LastIndexOf(@"\"));
                        if (Directory.Exists(folderPath) == false)
                        {

                            Directory.CreateDirectory(folderPath);
                        }
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(filePath))
                            {
                                CreateXMLSerial(sourceObj.GetType()).Serialize(writer, sourceObj);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Err("将对象反序列化为指定XML文件失败:" + ex.ToString());
                            return false;
                        }
                    }
                    else
                    {
                        log.Err("将对象反序列化为指定XML文件失败:路径或对象是否为空");
                        return false;
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("ObjectToXml 写入XML出错", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            return true;
        }

        /// <summary>
        /// 将指定XML反序列化为对象
        /// </summary>
        /// <param name="filePath">XML路径</param>
        /// <param name="type">类型</param>
        /// <returns>object</returns>
        public static object XmlToObject(string filePath, Type type)
        {

            object result = null;
            lock (LockObject)
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            
                            XmlSerializer xmlSerializer = new XmlSerializer(type, new XmlRootAttribute(type.Name));
                            result = xmlSerializer.Deserialize(reader);
                           // result = CreateXMLSerial(type).Deserialize(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Err("将指定XML反序列化为对象失败:" + ex.ToString());
                        return null;
                    }
                }
                else
                {
                    log.Warn("将指定XML反序列化为对象失败:" + filePath + "路径不存在");
                }
                return result;
            }
        }

        /// <summary>
        /// 将指定XML反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="filePath">路径</param>
        /// <returns>对象</returns>
        public static T XmlToObject<T>(string filePath)
        {
            T result = default(T);
            lock (LockObject)
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            result = (T)(CreateXMLSerial(typeof(T)).Deserialize(reader));
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Err("将指定XML反序列化为对象失败:" + ex.ToString());
                        return default(T);
                    }
                }
                else
                {
                    log.Err("将指定XML反序列化为对象失败:" + filePath + "路径不存在");
                }
            }
            return result;
        }

        private static XmlSerializer CreateXMLSerial(Type type)
        {
            object key = type.Name;
            xs = (XmlSerializer)serializers[key];
            if (xs == null)
            {
                xs = string.IsNullOrWhiteSpace(type.Name) ? new XmlSerializer(type) :
                            new XmlSerializer(type, new XmlRootAttribute(type.Name));
                serializers[key] = xs;
            }
            return xs;
        }
    }
    /// <summary>
    /// Json序列化
    /// </summary>
    public static class AccessJosnSerializer
    {
        public static LogView log = new LogView();
        public static bool ObjectToJson(string strFilePath, object obj)
        {
            string folderPath = strFilePath.Substring(0, strFilePath.LastIndexOf(@"\"));
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }
            try
            {
                string strRet = JsonConvert.SerializeObject(obj);
                using (StreamWriter sw = new StreamWriter(strFilePath, false))
                {
                    sw.WriteLine(strRet);
                }
                 return true;
            }
            catch (Exception exp)
            {
                log.Err(obj.GetType().ToString()+"序列化异常" + exp.ToString());
                return false;
            }
        }
        public static object  JsonToObject(string filePath,Type type)
        {
            string strData = "";
            using (StreamReader sr = new StreamReader(filePath))
            {
                strData = sr.ReadToEnd();
            }
            object obj = JsonConvert.DeserializeObject(strData, type);
           //object  obj = JsonConvert.DeserializeObject<type>(strData);
            return obj;
            
        }


    }


 
}
namespace SerialDict
{
    [Serializable]
    public class SerialDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        private string name;
        public SerialDictionary()
        {
            this.name = "SerialDictionary";
        }
        public SerialDictionary(string name)
        {
            this.name = name;
        }
        public void WriteXml(XmlWriter write)       // Serializer
        {
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));


            write.WriteAttributeString("name", name);

            write.WriteStartElement(name);
            foreach (KeyValuePair<TKey, TValue> kv in this)
            {
                write.WriteStartElement("element");
                write.WriteStartElement("key");
                KeySerializer.Serialize(write, kv.Key);
                write.WriteEndElement();
                write.WriteStartElement("value");
                ValueSerializer.Serialize(write, kv.Value);
                write.WriteEndElement();
                write.WriteEndElement();
            }
            write.WriteEndElement();
        }
        public void ReadXml(XmlReader reader)       // Deserializer
        {
            reader.Read();
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            name = reader.Name;
            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("element");
                reader.ReadStartElement("key");
                TKey tk = (TKey)KeySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                TValue vl = (TValue)ValueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadEndElement();

                this.Add(tk, vl);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
          //  reader.ReadEndElement();

        }
        public XmlSchema GetSchema()
        {
            return null;
        }
    }

}