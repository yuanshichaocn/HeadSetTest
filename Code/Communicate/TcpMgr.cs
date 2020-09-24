using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;


namespace Communicate
{
   /// <summary>
   /// 网口类管理器
   /// </summary>
   public class TcpMgr
    {
        /// <summary>
        /// 网口描述定义
        /// </summary>
        public static readonly string[] m_strDescribe =  { "网口号", "网口定义", "对方IP地址", "端口号", "超时时间", "命令分隔" };
        /// <summary>
        /// 网络连接列表
        /// </summary>
        private List<TcpLink> m_listTcpLink = new List<TcpLink>();

        private Dictionary<string, TcpLink> m_dicTcpLink = new Dictionary<string, TcpLink>();
       public bool OpenAllEth()
        {
            bool brtn = true;
            foreach( var tem in m_dicTcpLink)
            {
                brtn &= tem.Value.Open();
            }
            return brtn;
        }
        public void  CloseAllEth()
        {
          
            foreach (var tem in m_dicTcpLink)
            {
                
               tem.Value.Close();

            }
            return ;
        }

        /// <summary>
        /// 返回对应索引号的对象
        /// </summary>
        /// <param name="index">网口索引号</param>
        /// <returns></returns>
        public TcpLink GetTcpLink(int index)
        {
            if (index < m_listTcpLink.Count())
            {
                return m_listTcpLink.ElementAt(index);
            }
            return null;
        }/// <summary>
        /// 通过名字获取网口的引用
        /// </summary>
        /// <param name="strEthName"></param>
        /// <returns></returns>
        public TcpLink GetTcpLink(string strEthName)
        {
           if(m_dicTcpLink.ContainsKey(strEthName))
            {
                return m_dicTcpLink[strEthName];
            }
            return null;
        }
        private TcpMgr()
        { }
        private static object objlock = new object();

        private static TcpMgr m_tcpMgr = null;
        public static TcpMgr GetInstance()
        {
            if (m_tcpMgr == null)
            {
                lock (objlock)
                {
                    if (m_tcpMgr == null)
                    {
                        m_tcpMgr = new TcpMgr();
                    }
                }
            }
            return m_tcpMgr;
        }
        /// <summary>
        /// 获取系统中网络连接总数
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get { return m_listTcpLink.Count; }
        }

        /// <summary>
        /// 从xml文件中读取定义的网口信息
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            if (doc == null)
                return;
            m_listTcpLink.Clear();
            XmlNodeList xnl = doc.SelectNodes("/SystemCfg/" + "Eth");
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;
                        string strName = "";
                        try
                        {
                            string strNo = xe.GetAttribute(m_strDescribe[0]).Trim();
                            strName = xe.GetAttribute(m_strDescribe[1]).Trim();
                            string strIP = xe.GetAttribute(m_strDescribe[2]).Trim();
                            string strPort = xe.GetAttribute(m_strDescribe[3]).Trim();
                            string strTime = xe.GetAttribute(m_strDescribe[4]).Trim();
                            string strLine = xe.GetAttribute(m_strDescribe[5]).Trim();

                            TcpLink tcpLink = new TcpLink(Convert.ToInt32(strNo), strName, strIP, Convert.ToInt32(strPort)
                                , Convert.ToInt32(strTime), strLine);
                            m_listTcpLink.Add(tcpLink);

                            m_dicTcpLink.Add(strName, tcpLink);
                        }
                        catch
                        {
                            string strMsg = "网口对象创建失败";
                            if (strName != null)
                                strMsg = string.Format("网口:{0}对象创建失败", strName);
                            MessageBox.Show(strMsg, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 跟新内存参数到表格数据
        /// </summary>
        /// <param name="grid">界面网口表格控件</param>
        public void UpdateGridFromParam(DataGridView grid)
        {
            grid.Rows.Clear();
            if (m_listTcpLink.Count > 0)
            {
                grid.Rows.AddCopies(0, m_listTcpLink.Count);

                int i = 0;
                foreach (TcpLink t in m_listTcpLink)
                {
                    int j = 0;
                    grid.Rows[i].Cells[j++].Value = t.m_nIndex.ToString();
                    grid.Rows[i].Cells[j++].Value = t.m_strName;
                    grid.Rows[i].Cells[j++].Value = t.m_strIP;
                    grid.Rows[i].Cells[j++].Value = t.m_nPort;
                    grid.Rows[i].Cells[j++].Value = t.m_nTime;
                    grid.Rows[i].Cells[j++].Value = t.m_strLineFlag;
                    i++;
                }
            }
        }

        /// <summary>
        /// 跟新表格数据到内存参数
        /// </summary>
        /// <param name="grid">界面网口表格控件</param>
        public void UpdateParamFromGrid(DataGridView grid)
        {
            int m = grid.RowCount;
            int n = grid.ColumnCount;

            m_listTcpLink.Clear();

            for (int i = 0; i < m; ++i)
            {
                if (grid.Rows[i].Cells[0].Value == null)
                    break;
                string strNo = grid.Rows[i].Cells[0].Value.ToString();
                string strName = grid.Rows[i].Cells[1].Value.ToString(); 
                string strIP = grid.Rows[i].Cells[2].Value.ToString(); 
                string strPort = grid.Rows[i].Cells[3].Value.ToString(); 
                string strTime = grid.Rows[i].Cells[4].Value.ToString();
                string strLine = grid.Rows[i].Cells[5].Value.ToString(); 

                m_listTcpLink.Add(new TcpLink(Convert.ToInt32(strNo), strName, strIP, Convert.ToInt32(strPort)
                    , Convert.ToInt32(strTime), strLine));

            }
        }

        /// <summary>
        /// 保存内存参数到xml文件
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void SaveCfgXML(XmlDocument doc)
        {
            XmlNode xnl = doc.SelectSingleNode("SystemCfg");
            XmlNode root = doc.CreateElement("Eth");
            xnl.AppendChild(root);

            foreach (TcpLink t in m_listTcpLink)
            {
                XmlElement xe = doc.CreateElement("Eth");

                int j = 0;
                xe.SetAttribute(m_strDescribe[j++], t.m_nIndex.ToString());
                xe.SetAttribute(m_strDescribe[j++], t.m_strName);
                xe.SetAttribute(m_strDescribe[j++], t.m_strIP);
                xe.SetAttribute(m_strDescribe[j++], t.m_nPort.ToString());
                xe.SetAttribute(m_strDescribe[j++], t.m_nTime.ToString());
                xe.SetAttribute(m_strDescribe[j++], t.m_strLineFlag);

                root.AppendChild(xe);
            }
        }
        public bool  WriteData(string TcpName, string strData)
        {
            bool brtn=  m_dicTcpLink.ContainsKey(TcpName);
            if (!brtn)
                return false;
             TcpLink tcpLink = m_dicTcpLink[TcpName];
             return    tcpLink.WriteString(strData);
        }

        public int  ReadLine(string TcpName,out string strRead)
        {
            strRead = "";
            bool brtn = m_dicTcpLink.ContainsKey(TcpName);
            if (!brtn)
            {
                strRead = "";
                return 0;
            }  
            TcpLink tcpLink = m_dicTcpLink[TcpName];
            return tcpLink.ReadLine( out strRead);
        }

    }
}
