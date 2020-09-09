using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace Communicate
{
    //
    // 摘要:
    //     日志显示到列表框的基类
  
    /// <summary>
    /// 串口系统管理类
    /// </summary>
    public class ComMgr 
    {
        /// <summary>
        /// 串口定义描述
        /// </summary>
        public static readonly string[] m_strDescribe = {  "串口号", "串口定义", "波特率", "数据位", "校验位",
                                            "停止位", "流控制", "超时时间", "缓冲区大小","命令分隔" };
        /// <summary>
        /// 串口定义列表
        /// </summary>
        private List<ComLink> m_listComLink = new List<ComLink>();
        private Dictionary<string, ComLink> m_dicComLink = new Dictionary<string, ComLink>();

        /// <summary>
        /// 从xml文件中读取定义的串口信息
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            if (doc == null)
                return;
            m_listComLink.Clear();
            XmlNodeList xnl = doc.SelectNodes("/SystemCfg/" + "Com");
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;
                        string strName="";
                        try
                        {
                        string strNo = xe.GetAttribute(m_strDescribe[0]).Trim();
                        strName = xe.GetAttribute(m_strDescribe[1]).Trim();
                        string strBaudRate = xe.GetAttribute(m_strDescribe[2]).Trim();
                        string strDataBit = xe.GetAttribute(m_strDescribe[3]).Trim();
                        string strPartiy = xe.GetAttribute(m_strDescribe[4]).Trim();
                        string strStopBit = xe.GetAttribute(m_strDescribe[5]).Trim();
                        string strFlowCtrl = xe.GetAttribute(m_strDescribe[6]).Trim(); 
                        string strTime = xe.GetAttribute(m_strDescribe[7]).Trim();
                        string strBufferSize = xe.GetAttribute(m_strDescribe[8]).Trim();
                        string strLine = xe.GetAttribute(m_strDescribe[9]).Trim();

                            ComLink comLink = new ComLink(Convert.ToInt32(strNo), strName, Convert.ToInt32(strBaudRate),
                                 Convert.ToInt32(strDataBit), strPartiy, strStopBit, strFlowCtrl,
                                 Convert.ToInt32(strTime), Convert.ToInt32(strBufferSize), strLine);
                            m_listComLink.Add(comLink);
                            m_dicComLink.Add(strName, comLink);
                        }
                        catch
                        {
                            string strMsg = "串口对象创建失败";
                            if (strName!=null)
                             strMsg = string.Format("串口:{0}对象创建失败" ,strName);
                            MessageBox.Show(strMsg, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                     
                      
                    }
                }
            }
        }

        private  ComMgr()
        { }
        private static object objlock = new object();

        private static  ComMgr m_comMgr= null;
        public static ComMgr GetInstance()
        {
            if(m_comMgr==null)
            {
                lock(objlock)
                {
                    if (m_comMgr == null)
                    {
                        m_comMgr = new ComMgr();
                    }
                }
            }
            return m_comMgr;
        }
        /// <summary>
        /// 跟新内存参数到表格数据
        /// </summary>
        /// <param name="grid">界面串口表格控件</param>
        public void UpdateGridFromParam(DataGridView grid)
        {
            grid.Rows.Clear();
            if (m_listComLink.Count > 0)
            {
                
                grid.Rows.AddCopies(0, m_listComLink.Count);

                int i = 0;
                foreach (ComLink t in m_listComLink)
                {
                    int j = 0;
                    grid.Rows[i].Cells[j++].Value = t.m_nComNo.ToString();
                    grid.Rows[i].Cells[j++].Value = t.m_strName;
                    grid.Rows[i].Cells[j++].Value = t.m_nBaudRate.ToString();
                    grid.Rows[i].Cells[j++].Value = t.m_nDataBit.ToString();
                    grid.Rows[i].Cells[j++].Value = t.m_strPartiy;
                    grid.Rows[i].Cells[j++].Value = t.m_strStopBit;
                    grid.Rows[i].Cells[j++].Value = t.m_strFlowCtrl;
                    grid.Rows[i].Cells[j++].Value = t.m_nTime.ToString();
                    grid.Rows[i].Cells[j++].Value = t.m_nBufferSzie.ToString();
                    grid.Rows[i].Cells[j++].Value = t.m_strLineFlag;

                    i++;
                }
            }
        }

        /// <summary>
        /// 更新表格数据到内存参数
        /// </summary>
        /// <param name="grid">界面串口表格控件</param>
        public void UpdateParamFromGrid(DataGridView grid)
        {
            int m = grid.RowCount;
            int n = grid.ColumnCount;

            m_listComLink.Clear();

            for (int i = 0; i < m; ++i)
            {
                if (grid.Rows[i].Cells[0].Value == null)
                    break;
                string strNo = grid.Rows[i].Cells[0].Value.ToString();
                string strName = grid.Rows[i].Cells[1].Value.ToString();
                string strBaudRate = grid.Rows[i].Cells[2].Value.ToString();
                string strDataBit = grid.Rows[i].Cells[3].Value.ToString();
                string strPartiy = grid.Rows[i].Cells[4].Value.ToString();
                string strStopBit = grid.Rows[i].Cells[5].Value.ToString();
                string strFlowCtrl = grid.Rows[i].Cells[6].Value.ToString();
                string strTime = grid.Rows[i].Cells[7].Value.ToString();
                string strBufferSize = grid.Rows[i].Cells[8].Value.ToString();
                string strLine = grid.Rows[i].Cells[9].Value.ToString();

                m_listComLink.Add(new ComLink(Convert.ToInt32(strNo), strName, Convert.ToInt32(strBaudRate),
                        Convert.ToInt32(strDataBit), strPartiy, strStopBit, strFlowCtrl,
                         Convert.ToInt32(strTime), Convert.ToInt32(strBufferSize), strLine));

            }
        }

        /// <summary>
        /// 保存内存参数到xml文件
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void SaveCfgXML(XmlDocument doc)
        {
            XmlNode xnl = doc.SelectSingleNode("SystemCfg");
            XmlNode root = doc.CreateElement("Com");
            xnl.AppendChild(root);

            foreach (ComLink t in m_listComLink)
            {
                XmlElement xe = doc.CreateElement("Com");

                int j = 0;
                xe.SetAttribute(m_strDescribe[j++], t.m_nComNo.ToString());
                xe.SetAttribute(m_strDescribe[j++], t.m_strName);
                xe.SetAttribute(m_strDescribe[j++], t.m_nBaudRate.ToString());
                xe.SetAttribute(m_strDescribe[j++], t.m_nDataBit.ToString());
                xe.SetAttribute(m_strDescribe[j++], t.m_strPartiy);
                xe.SetAttribute(m_strDescribe[j++], t.m_strStopBit);
                xe.SetAttribute(m_strDescribe[j++], t.m_strFlowCtrl);
                xe.SetAttribute(m_strDescribe[j++], t.m_nTime.ToString());
                xe.SetAttribute(m_strDescribe[j++], t.m_nBufferSzie.ToString());
                xe.SetAttribute(m_strDescribe[j++], t.m_strLineFlag);
                root.AppendChild(xe);
            }
        }

        /// <summary>
        /// 返回对应索引的对象
        /// </summary>
        /// <param name="nIndex">索引号</param>
        /// <returns></returns>
        public ComLink GetComLink(int nIndex)
        {
            if (nIndex < m_listComLink.Count())
            {
                return m_listComLink.ElementAt(nIndex);
            }
            return null;
        }

        /// <summary>
        /// 获取系统串口总数
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get{ return m_listComLink.Count; }
        }

    }
}
