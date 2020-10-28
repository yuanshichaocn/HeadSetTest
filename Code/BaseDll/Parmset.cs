using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Xml;
using System.IO;
using System.Reflection;
namespace BaseDll
{
    public enum ParamSetUnit
    {
        boolUnit,
        intUnit,
        doubleUnit,
        stringUnit,
    }
    public struct ParamSet
    {
        public ParamSetUnit _enuValType;
        public string _strParamUnit;
        public dynamic _strParamValMax;
        public dynamic _strParamValMin;
        public dynamic _strParamVal;
        public UserRight _ParamRight;
        public string _ParamClass;
    }
    public class ParamSetMgr
    {
        private List<string> ParamClassList = new List<string>()
        {
            "综合",
        };

        public void AddParamClass(string strParamClass)
        {
            if (!ParamClassList.Contains(strParamClass))
                ParamClassList.Add(strParamClass);
        }
        public List<string> GetParamClassList()
        {
            return ParamClassList;
        }
        public void ClearAllParamClassList()
        {
            ParamClassList.Clear();
        }

        private ParamSetMgr() { }
        private static ParamSetMgr m_paramMgr = null;
        private static object lockobj = new object();
        public static ParamSetMgr GetInstance()
        {
            if (m_paramMgr == null)
            {
                lock (lockobj)
                {
                    if (m_paramMgr == null)
                        m_paramMgr = new ParamSetMgr();
                }
            }
            return m_paramMgr;

        }
        XmlDocument xmlDocument = new XmlDocument();
        Dictionary<string, ParamSet> m_param = new Dictionary<string, ParamSet>();
        public Dictionary<string, ParamSet> GetAllParam()
        {
            return m_param;
        }
        public void SetParam(string strParamName, ParamSet paramSet)
        {
            if (m_param.ContainsKey(strParamName))
            {
                m_param[strParamName] = paramSet;
            }
            else
                m_param.Add(strParamName, paramSet);
            switch (paramSet._enuValType)
            {
                case ParamSetUnit.boolUnit:
                    SetBoolParam(strParamName, Convert.ToBoolean(paramSet._strParamVal));
                    break;
                case ParamSetUnit.doubleUnit:
                    SetDoubleParam(strParamName, Convert.ToDouble(paramSet._strParamVal));
                    break;
                case ParamSetUnit.intUnit:
                    SetIntParam(strParamName, Convert.ToInt32(paramSet._strParamVal));
                    break;
                case ParamSetUnit.stringUnit:
                    SetStringParam(strParamName, paramSet._strParamVal.ToString());
                    break;
            }
            SetTypeDic(paramSet._ParamClass, strParamName, paramSet);
        }
        public bool GetParam(string strParamName, out ParamSet paramSet)
        {
            bool bRtn = false;
            bRtn = m_param.TryGetValue(strParamName, out paramSet);
            return bRtn;
        }

        public dynamic GetParam(string strParamName)
        {
            ParamSet paramSet;
            try
            {
                if (m_param.TryGetValue(strParamName, out paramSet))
                {
                    return m_param[strParamName]._strParamVal;
                }
                else
                    return null;

            }
            catch(Exception e)
            {
                throw new Exception($"{e.Message}+ 没有参数{strParamName}");
            }
            
        }
        public Dictionary<string,ParamSet> GetClassAllParams(string ParamClassName)
        {
            lock (locktype)
            {
                if (_dicTypeParamset.ContainsKey(ParamClassName))
                {
                    return _dicTypeParamset[ParamClassName];
                }
                else
                {
                    return null;
                }
            }
        }
        public void ClearAllParam()
        {
            m_param.Clear();
        }
        public bool ReadParamFromXml(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    System.Diagnostics.Debug.WriteLine("start ReadParamFromXml\n");
                    xmlDocument.Load(path);
                    XmlElement root = xmlDocument.DocumentElement;
                    if (root == null)
                        return false;
                    XmlNodeList clsMotionList = xmlDocument.SelectNodes("/ParamCfg/ParamSet");//(XmlElement)root.SelectSingleNode(" / SystemCfg / Motion");
                    if (clsMotionList == null || clsMotionList.Count == 0)
                        return false;

                    XmlNodeList xmlNodeList = clsMotionList.Item(0).ChildNodes;
                    string strType;
                    string strParamName = "";
                    string strParamVal = "";
                    string strParamRight = "";
                    ClearAllParam();
                    _dicTypeParamset.Clear();
                    foreach (XmlNode temp in xmlNodeList)
                    {
                        ParamSet paramSet = new ParamSet();
                        strParamName = temp.Attributes["ParamName"].Value.Trim();
                        strType = temp.Attributes["ParamType"].Value.Trim();
                        strParamVal = temp.Attributes["ParamVal"].Value.Trim();
                        paramSet._enuValType = (ParamSetUnit)Enum.Parse(typeof(ParamSetUnit), strType);
                        strParamRight = temp.Attributes["ParamRight"].Value.Trim();
                        paramSet._ParamRight = (UserRight)Enum.Parse(typeof(UserRight), strParamRight);
                        //paramSet._strParamVal = strParamVal;
                        //paramSet._strParamValMax = temp.Attributes["ParamValMax"].Value;
                        //paramSet._strParamValMin = temp.Attributes["ParamValMin"].Value;
                        paramSet._ParamClass = temp.Attributes["ParamValClass"].Value.Trim();
                        if (paramSet._enuValType == ParamSetUnit.boolUnit)
                        {
                            paramSet._strParamVal = Convert.ToBoolean(strParamVal);
                            paramSet._strParamValMax = Convert.ToBoolean(temp.Attributes["ParamValMax"].Value.Trim());
                            paramSet._strParamValMin = Convert.ToBoolean(temp.Attributes["ParamValMin"].Value.Trim());
                            SetBoolParam(strParamName, Convert.ToBoolean(strParamVal));
                        }
                        if (paramSet._enuValType == ParamSetUnit.intUnit)
                        {
                            paramSet._strParamVal = Convert.ToInt32(strParamVal);
                            paramSet._strParamValMax = Convert.ToInt32(temp.Attributes["ParamValMax"].Value.Trim());
                            paramSet._strParamValMin = Convert.ToInt32(temp.Attributes["ParamValMin"].Value.Trim());
                            SetIntParam(strParamName, Convert.ToInt32(strParamVal));
                        }
                        if (paramSet._enuValType == ParamSetUnit.doubleUnit)
                        {
                            paramSet._strParamVal = Convert.ToDouble(strParamVal);
                            paramSet._strParamValMax = Convert.ToDouble(temp.Attributes["ParamValMax"].Value.Trim());
                            paramSet._strParamValMin = Convert.ToDouble(temp.Attributes["ParamValMin"].Value.Trim());
                            SetDoubleParam(strParamName, Convert.ToDouble(strParamVal));
                        }
                        if (paramSet._enuValType == ParamSetUnit.stringUnit)
                        {
                            paramSet._strParamVal = Convert.ToString(strParamVal);
                            paramSet._strParamValMax = Convert.ToString(temp.Attributes["ParamValMax"].Value.Trim());
                            paramSet._strParamValMin = Convert.ToString(temp.Attributes["ParamValMin"].Value.Trim());
                            SetStringParam(strParamName, strParamVal);
                        }
                        SetParam(strParamName, paramSet);
                        SetTypeDic(paramSet._ParamClass, strParamName, paramSet);
                       
                    }
                    System.Diagnostics.Debug.WriteLine("end ReadParamFromXml\n");
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取：" + path + " 文件时：" + ex.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


        }
        private static object locktype = new object();
        public void SetTypeDic( string strClass, string strParamName, ParamSet paramSet)
        {
            lock(locktype)
            {
                if (_dicTypeParamset.ContainsKey(strClass))
                {
                    if (_dicTypeParamset[paramSet._ParamClass].ContainsKey(strParamName))
                        _dicTypeParamset[paramSet._ParamClass][strParamName] = paramSet;
                    else
                        _dicTypeParamset[paramSet._ParamClass].Add(strParamName, paramSet);
                }
                else
                {
                    _dicTypeParamset.TryAdd(paramSet._ParamClass, new Dictionary<string, ParamSet>());
                    _dicTypeParamset[paramSet._ParamClass].Add(strParamName, paramSet);
                }
                if (!ParamClassList.Contains(paramSet._ParamClass))
                {
                    ParamClassList.Add(paramSet._ParamClass);
                }
            }
            
        }

        public void SaveParam(string path)
        {
            try
            {
                if(xmlDocument==null)
                    xmlDocument = new XmlDocument();
                else if( !File.Exists(path))
                {
                    XmlDocument document = new XmlDocument();
                    XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", "no");
                    document.AppendChild(dec);
                    XmlElement roots = document.CreateElement("ParamCfg");
                    document.AppendChild(roots);
                    XmlElement item = document.CreateElement("ParamSet");
                    roots.AppendChild(item);
                    document.Save(path);
                }
                  

                xmlDocument.Load(path);
                XmlElement root = xmlDocument.DocumentElement;
                if (root == null)
                {
                    xmlDocument = new XmlDocument();
                    root = xmlDocument.CreateElement("ParamCfg");
                    xmlDocument.AppendChild(root);
                }
                XmlNodeList clsMotionList = xmlDocument.SelectNodes("/ParamCfg/ParamSet");//(XmlElement)root.SelectSingleNode(" / SystemCfg / Motion");
                if (clsMotionList == null || clsMotionList.Count == 0)
                {
                    XmlElement item = xmlDocument.CreateElement("ParamSet");
                    root.AppendChild(item);
                    clsMotionList = xmlDocument.SelectNodes("/ParamCfg/ParamSet");
                }

                XmlNode xmlNodeList = clsMotionList.Item(0);
                xmlNodeList.RemoveAll();

                bool bHave = false;
                
                foreach (var temp in m_param)
                {
                    for (int j = 0; xmlNodeList != null && j < xmlNodeList.ChildNodes.Count; j++)
                    {

                        if (temp.Key == xmlNodeList.ChildNodes[j].Attributes["ParamName"].Value)
                        {
                            try
                            {
                                XmlElement Item = (XmlElement)xmlNodeList.ChildNodes[j];
                                Item.SetAttribute("ParamName", temp.Key.ToString().Trim());
                                Item.SetAttribute("ParamType", temp.Value._enuValType.ToString().Trim());
                                Item.SetAttribute("ParamVal", temp.Value._strParamVal.ToString().Trim());
                                Item.SetAttribute("ParamValMax", temp.Value._strParamValMax.ToString().Trim());
                                Item.SetAttribute("ParamValMin", temp.Value._strParamValMin.ToString().Trim());
                                Item.SetAttribute("ParamRight", temp.Value._ParamRight.ToString().Trim());
                                Item.SetAttribute("ParamValClass", temp.Value._ParamClass.ToString().Trim());

                                bHave = true;
                                break;
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show($"参数{temp.Key}保存，有错误:{e.Message}");
                            }
                           
                        }
                        
                    }
                    if (!bHave)
                    {
                        try
                        {
                            XmlElement Item = xmlDocument.CreateElement("ParamSet");
                           Item.SetAttribute("ParamName", temp.Key.ToString().Trim());
                           Item.SetAttribute("ParamType", temp.Value._enuValType.ToString().Trim());
                           Item.SetAttribute("ParamVal", temp.Value._strParamVal.ToString().Trim());
                           Item.SetAttribute("ParamValMax", temp.Value._strParamValMax.ToString().Trim());
                           Item.SetAttribute("ParamValMin", temp.Value._strParamValMin.ToString().Trim());
                           Item.SetAttribute("ParamRight", temp.Value._ParamRight.ToString().Trim());
                           Item.SetAttribute("ParamValClass", temp.Value._ParamClass.ToString().Trim());
                           xmlNodeList.AppendChild(Item);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"参数保存{temp.Key}，有错误");
                        }
                    }
                    bHave = false;

                }
                if (path != "")
                    xmlDocument.Save(path);
            }
            catch( Exception e)
            {
                MessageBox.Show($"参数保存异常：{e.Message}，有错误");
            }
            
        }
        public delegate void LoadProductFileHandle(string strFile);
        public event LoadProductFileHandle m_eventLoadProductFile;
        public Action m_eventLoadProductFileUpadata;
        public string CurrentProductFile
        {
            set
            {
                if (value != "" && value != m_strOldCurrentProductFile)
                {
                    string strpath = CurrentWorkDir + ("\\") + value + ("\\") + value + (".xml");
                    if (ReadParamFromXml(strpath))
                    {
                        m_strOldCurrentProductFile = value;
                        if (m_eventLoadProductFile != null)
                            m_eventLoadProductFile(value);
                    }
                    m_strOldCurrentProductFile = value;
                }
                else
                    System.Diagnostics.Debug.WriteLine("!=");
            }
            get => m_strOldCurrentProductFile;
        }
        private string m_strOldCurrentProductFile;
        public string CurrentWorkDir
        {
            set;
            get;
        }
        public DirectoryInfo[] EnumProductFile()
        {
            string dir = CurrentWorkDir + ("\\");
            int pos = 0;
            if (Directory.Exists(dir))
            {
                DirectoryInfo theFolder = new DirectoryInfo(dir);
                DirectoryInfo[] dirInfo = theFolder.GetDirectories();

                return dirInfo;
            }
            return null;
        }

        public delegate void ChangedSysStringValHandler(string key, string val);
        public event ChangedSysStringValHandler m_eventChangedSysStrVal = null;

        public delegate void ChangedSysBoolValHandler(string key, bool val);
        public event ChangedSysBoolValHandler m_eventChangedBoolSysVal = null;

        public delegate void ChangedSysIntValHandler(string key, int val);
        public event ChangedSysIntValHandler m_eventChangedIntSysVal = null;

        public delegate void ChangedSysDoubleValHandler(string key, double val);
        public event ChangedSysDoubleValHandler m_eventChangedDoubleSysVal = null;

        private static object lockobjectint = new object();
        private static object lockobjectdouble = new object();
        private static object lockobjectstring = new object();
        private static object lockobjectbool = new object();
        private ConcurrentDictionary<string, string> _dicStrParam = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<string, int> _dicIntParam = new ConcurrentDictionary<string, int>();
        private ConcurrentDictionary<string, double> _dicDoubleParam = new ConcurrentDictionary<string, double>();
        private ConcurrentDictionary<string, bool> _dicBoolParam = new ConcurrentDictionary<string, bool>();
        private ConcurrentDictionary<string, Dictionary<string, ParamSet>> _dicTypeParamset = new ConcurrentDictionary<string, Dictionary<string, ParamSet>>();

        public bool GetBoolParam(string strParamName)
        {
            bool val;
            if (_dicBoolParam.TryGetValue(strParamName, out val))
                return val;
            else
                throw new Exception($"bool 类型没有参数{strParamName}") ;
        }
        public int GetIntParam(string strParamName)
        {
            int nval = 0;
            if (_dicIntParam.TryGetValue(strParamName, out nval))
                return nval;
            else
                throw new Exception($"int 类型没有参数{strParamName}");
        }
        public string GetStringParam(string strParamName)
        {
            string str = "NoFindKey";
            if (_dicStrParam.TryGetValue(strParamName, out str))
                return str;
            else
                throw new Exception($"string 类型没有参数{strParamName}");
        }
        public double GetDoubleParam(string strParamName)
        {
            double dval = 0.00;
            if (_dicDoubleParam.TryGetValue(strParamName, out dval))
                return dval;
            else
                throw new Exception($"double 类型没有参数{strParamName}");
        }
        /// <summary>
        /// 设置string参数
        /// </summary>
        /// <param name="strParamName"></param>
        /// <param name="strval"></param>
        public void SetStringParam(string strParamName, string strval)
        {
            string strOldVal;
            lock (lockobjectstring)
            {
                if (_dicStrParam.ContainsKey(strParamName))
                {
                    strOldVal = _dicStrParam[strParamName];
                    _dicStrParam[strParamName] = strval;

                    if (strOldVal != strParamName && m_eventChangedSysStrVal != null)
                        m_eventChangedSysStrVal(strParamName, strval);
                }
                else
                {
                    _dicStrParam.TryAdd(strParamName, strval);
                    if (m_eventChangedSysStrVal != null)
                        m_eventChangedSysStrVal(strParamName, strval);
                }
            }

        }
        /// <summary>
        /// 设置bool参数
        /// </summary>
        /// <param name="strParamName"></param>
        /// <param name="bval"></param>
        public void SetBoolParam(string strParamName, bool bval)
        {
            bool boldVal;
            lock (lockobjectbool)
            {
                if (_dicBoolParam.ContainsKey(strParamName))
                {
                    boldVal = _dicBoolParam[strParamName];
                    _dicBoolParam[strParamName] = bval;
                    if (bval != boldVal && m_eventChangedBoolSysVal != null)
                        m_eventChangedBoolSysVal(strParamName, bval);
                }
                else
                {
                    _dicBoolParam.TryAdd(strParamName, bval);
                    if (m_eventChangedBoolSysVal != null)
                        m_eventChangedBoolSysVal(strParamName, bval);
                }
            }
        }
        /// <summary>
        /// 设置double参数
        /// </summary>
        /// <param name="strParamName"></param>
        /// <param name="dval"></param>
        public void SetDoubleParam(string strParamName, double val)
        {
            double dOldVal = 0;
            lock (lockobjectdouble)
            {
                if (_dicDoubleParam.ContainsKey(strParamName))
                {
                    dOldVal = _dicDoubleParam[strParamName];
                    _dicDoubleParam[strParamName] = val;
                    if (dOldVal != val && m_eventChangedDoubleSysVal != null)
                        m_eventChangedDoubleSysVal(strParamName, val);

                }

                else
                {
                    _dicDoubleParam.TryAdd(strParamName, val);
                    if (m_eventChangedDoubleSysVal != null)
                        m_eventChangedDoubleSysVal(strParamName, val);
                }

            }
        }
        /// <summary>
        /// 设置int参数
        /// </summary>
        /// <param name="strParamName"></param>
        /// <param name="nval"></param>
        public void SetIntParam(string strParamName, int nval)
        {
            int nOldVal = 0;
            lock (lockobjectint)
            {
                if (_dicIntParam.ContainsKey(strParamName))
                {
                    nOldVal = _dicIntParam[strParamName];
                    _dicIntParam[strParamName] = nval;
                    if (nOldVal != nval && m_eventChangedIntSysVal != null)
                        m_eventChangedIntSysVal(strParamName, nval);
                }
                else
                {
                    _dicIntParam.TryAdd(strParamName, nval);
                    if (m_eventChangedIntSysVal != null)
                        m_eventChangedIntSysVal(strParamName, nval);

                }

            }

        }

    }
}