using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonTools;
using System.Xml;
using System.Diagnostics;
using BaseDll;
using UserData;
using log4net;

namespace StationDemo
{
    public partial class Form_ParamSet : Form,IUserRightSwitch
    {
        ILog logger = LogManager.GetLogger("Form_ParamSet");
        public Form_ParamSet()
        {
            InitializeComponent();
        }

        public UserRight userRight
        {
            get;
            set;
        }
        void UpdataParamDataGridView( User CurrentUser)
        {
         
            userRight = CurrentUser._userRight;
            dataGridView_Param.Rows.Clear();
            ParamSetMgr.GetInstance().ClearAllParamClassList();
            Dictionary<string, ParamSet> allparam = ParamSetMgr.GetInstance().GetAllParam();
            foreach (var temp in allparam)
            {
                if(temp.Value._ParamClass!=null)
                {
                    ParamSetMgr.GetInstance().AddParamClass(temp.Value._ParamClass);
                }
               
            }
            List<string> listParamList= ParamSetMgr.GetInstance().GetParamClassList();
            DataGridViewComboBoxColumn combox = (DataGridViewComboBoxColumn)dataGridView_Param.Columns[6];
            combox.Items.Clear();
            foreach (var temp in listParamList)
            {
                if(dataGridView_Param.Columns[6] is DataGridViewComboBoxColumn)
                {
                    combox.Items.Add(temp);
                }
            }

            int index = 0;
            foreach (var temp in allparam)
            {
                dataGridView_Param.Rows.Add(temp.Key, temp.Value._enuValType.ToString(),
                     temp.Value._strParamVal, temp.Value._strParamValMax, temp.Value._strParamValMin,
                     temp.Value._ParamRight.ToString(), temp.Value._ParamClass.ToString());
                if((int)CurrentUser._userRight >= (int)UserRight.软件工程师)
                {
                    dataGridView_Param.Rows[index].ReadOnly = false;
                }
                else if ((int)(UserRight)Enum.Parse(typeof(UserRight),dataGridView_Param.Rows[index].Cells[5].Value.ToString()) <= (int)CurrentUser._userRight)
                {
                    //dataGridView_Param.Rows[index].ReadOnly = false;
                    dataGridView_Param.Rows[index].Cells[0].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[1].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[2].ReadOnly = false;
                    dataGridView_Param.Rows[index].Cells[3].ReadOnly = false;
                    dataGridView_Param.Rows[index].Cells[4].ReadOnly = false;
                    dataGridView_Param.Rows[index].Cells[5].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[6].ReadOnly = true;

                }
                else
                {
                    dataGridView_Param.Rows[index].Cells[0].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[1].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[2].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[3].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[4].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[5].ReadOnly = true;
                    dataGridView_Param.Rows[index].Cells[6].ReadOnly = true;
                }
                
                index++;
            }
            if ((int)CurrentUser._userRight < (int)UserRight.软件工程师)
                dataGridView_Param.AllowUserToAddRows = false;
            else
                dataGridView_Param.AllowUserToAddRows = true;
       
        }
        public void ChangedUserRight(User CurrentUser)
        {

            if (userRight == CurrentUser._userRight)
                return;
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ChangedUserRight(CurrentUser)));
            }
            else
            {

                UpdataParamDataGridView(CurrentUser);
                //for (int i = 0; i < dataGridView_Param.Rows.Count; i++)
                //    heith+= dataGridView_Param.Height = dataGridView_Param.Rows[i].Height;
                //heith += dataGridView_Param.ColumnHeadersHeight;
                //dataGridView_Param.Height = heith;
            }
        }
        void LoadProductFile(string strFile)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => ChangedUserRight(sys.g_User)));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("start LoadProductFile\n");
              //  Console.WriteLine("start LoadProductFile\n");
                UpdataParamDataGridView(sys.g_User);
                label_CurrentFile.Text = "当前产品:" + strFile;
              //  Console.WriteLine("end LoadProductFile\n");
                System.Diagnostics.Debug.WriteLine("end LoadProductFilexx\n");
                BtnLoad.Enabled = true;
                System.Diagnostics.Debug.WriteLine("end LoadProductFile\n");
            }
        }
        private void  UpdateProductList()
        {
            treeView_ProdutFile.Nodes.Clear();
            treeView_ProdutFile.Nodes.Add("当前产品");
            treeView_ProdutFile.Nodes.Add("产品选择");
            if (ParamSetMgr.GetInstance().CurrentProductFile != null && ParamSetMgr.GetInstance().CurrentProductFile != "")
            {
                treeView_ProdutFile.Nodes[0].Nodes.Add(ParamSetMgr.GetInstance().CurrentProductFile);
                treeView_ProdutFile.Nodes[0].ImageIndex = 0;
            }
            DirectoryInfo[] dirInfo = ParamSetMgr.GetInstance().EnumProductFile();

            int pos;
            for (int i = 0; dirInfo != null && i < dirInfo.Length; i++)
            {
                pos = dirInfo[i].FullName.LastIndexOf("\\");
                string str = dirInfo[i].FullName.Substring(pos + 1, dirInfo[i].FullName.Length - pos - 1);
                if (str != ParamSetMgr.GetInstance().CurrentProductFile)
                    treeView_ProdutFile.Nodes[1].Nodes.Add(str);
            }
            treeView_ProdutFile.Nodes[0].Expand();
            if (treeView_ProdutFile.Nodes[0].FirstNode != null && treeView_ProdutFile.Nodes[0].FirstNode.Name != null && treeView_ProdutFile.Nodes[0].FirstNode.Text != "")
            {
                List<string> classList = ParamSetMgr.GetInstance().GetParamClassList();
                foreach (var temp in classList)
                    treeView_ProdutFile.Nodes[0].Nodes[0].Nodes.Add(temp);
            }
            treeView_ProdutFile.Nodes[0].ExpandAll();
            treeView_ProdutFile.Nodes[1].Expand();
        }
        private void Form_ParamSet_Load(object sender, EventArgs e)
        {
            sys.g_eventRightChanged += ChangedUserRight;
            ParamSetMgr.GetInstance().m_eventLoadProductFile += LoadProductFile;
            userRight = UserRight.User_None;
            int width = 0;
            for (int i = 0; i < dataGridView_Param.Columns.Count; i++)
                width += dataGridView_Param.Columns[i].Width;
            dataGridView_Param.Width = width + 35;
            richTextBox1.Location = new Point(dataGridView_Param.Location.X + dataGridView_Param.Width, dataGridView_Param.Location.Y);
            sys.g_User = sys.g_User;
            label_CurrentFile.Text = "当前产品:" + ParamSetMgr.GetInstance().CurrentProductFile;
            UpdateProductList();

            // m_path = ParamSetMgr.GetInstance().CurrentWorkFile;
            string path = $"{ParamSetMgr.GetInstance().CurrentWorkDir}\\{ParamSetMgr.GetInstance().CurrentProductFile}\\Description.xml";
            DescriptionClass.paramValue = AccessXmlSerializer.XmlToObject<List<ParamValue>>(path);
        }
     
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!Check())
            {
                return;
            }
            string strParamName = "";
            string currentProductFile = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + (".xml");
            if (!Directory.Exists(ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile))
            {
                MessageBox.Show("没有新建产品文件夹，请新建或另存", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // if (!File.Exists(currentProductFile))
            //{
            //    MessageBox.Show("没有载入产品文件，请载入", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return ;
            //}
            for ( int  i=0;i<  dataGridView_Param.RowCount;i++)
            {
                if (dataGridView_Param.Rows[i].Cells[0].Value!=null && dataGridView_Param.Rows[i].Cells[0].Value.ToString()!="")
                {
                    strParamName = dataGridView_Param.Rows[i].Cells[0].Value.ToString();
                    ParamSet paramSet = new ParamSet()
                    {
                        _enuValType = dataGridView_Param.Rows[i].Cells[0].Value == null ? ParamSetUnit.doubleUnit : (ParamSetUnit)Enum.Parse(typeof(ParamSetUnit), dataGridView_Param.Rows[i].Cells[1].Value.ToString()),
                        _strParamVal = dataGridView_Param.Rows[i].Cells[2].Value == null ? "0" : dataGridView_Param.Rows[i].Cells[2].Value.ToString(),
                        _strParamValMax = dataGridView_Param.Rows[i].Cells[3].Value == null ? "0" : dataGridView_Param.Rows[i].Cells[3].Value.ToString(),
                        _strParamValMin = dataGridView_Param.Rows[i].Cells[4].Value == null ? "0" : dataGridView_Param.Rows[i].Cells[4].Value.ToString(),
                        _ParamRight = dataGridView_Param.Rows[i].Cells[5].Value == null ? UserRight.客户操作员 : (UserRight)Enum.Parse(typeof(UserRight), dataGridView_Param.Rows[i].Cells[5].Value.ToString()),
                        _ParamClass = dataGridView_Param.Rows[i].Cells[6].Value == null ? "综合" : dataGridView_Param.Rows[i].Cells[6].Value.ToString(),
                     } ;

                    //if (ParamSetMgr.GetInstance().GetParam(strParamName) != paramSet._strParamVal)
                    //    logger.Info($"用户{sys.g_User._userName},修改参数：{strParamName} ，原来值：{ParamSetMgr.GetInstance().GetParam(strParamName)}，现在值：{ paramSet._strParamVal} ");

                    ParamSetMgr.GetInstance().SetParam(strParamName, paramSet );
                   
                    ParamSet ds = new ParamSet()
                    {
                        _enuValType = dataGridView_Param.Rows[i].Cells[0].Value == null ? ParamSetUnit.doubleUnit : (ParamSetUnit)Enum.Parse(typeof(ParamSetUnit), dataGridView_Param.Rows[i].Cells[1].Value.ToString()),
                        _strParamVal = dataGridView_Param.Rows[i].Cells[2].Value == null ? "0" : dataGridView_Param.Rows[i].Cells[2].Value.ToString(),
                        _strParamValMax = dataGridView_Param.Rows[i].Cells[3].Value == null ? "0" : dataGridView_Param.Rows[i].Cells[3].Value.ToString(),
                        _strParamValMin = dataGridView_Param.Rows[i].Cells[4].Value == null ? "0" : dataGridView_Param.Rows[i].Cells[4].Value.ToString(),
                        _ParamRight = dataGridView_Param.Rows[i].Cells[5].Value == null ? UserRight.客户操作员 : (UserRight)Enum.Parse(typeof(UserRight), dataGridView_Param.Rows[i].Cells[5].Value.ToString()),
                        _ParamClass = dataGridView_Param.Rows[i].Cells[6].Value == null ? "综合" : dataGridView_Param.Rows[i].Cells[6].Value.ToString(),
                    };
                }
            }
            ParamSetMgr.GetInstance().SaveParam(currentProductFile);
        }

        List<string> ItemName = new List<string>();
        private bool Check()
        {
            try
            {
                ItemName.Clear();
                for (int i = 0; i < dataGridView_Param.Rows.Count - 1; i++)
                {
                    string name = dataGridView_Param.Rows[i].Cells[0].Value.ToString().Trim();
                    string sort = dataGridView_Param.Rows[i].Cells[6].Value.ToString().Trim();
                    string type = dataGridView_Param.Rows[i].Cells[1].Value.ToString().Trim();
                    string value = dataGridView_Param.Rows[i].Cells[2].Value.ToString().Trim();
                    string Max = dataGridView_Param.Rows[i].Cells[3].Value.ToString().Trim();
                    string Min = dataGridView_Param.Rows[i].Cells[4].Value.ToString().Trim();
                    dynamic v; dynamic vMax; dynamic vMin;

                    if (type == "boolUnit")
                    {
                        if (value.ToLower() != "true" && value.ToLower() != "false")
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，value设置失败，请输入false或者true", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        if (Max.ToLower() != "true" && Max.ToLower() != "false")
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，Max设置失败，请输入false或者true", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        if (Min.ToLower() != "true" && Min.ToLower() != "false")
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，min设置失败，请输入false或者true", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                    }
                    else if (type == "intUnit")
                    {

                        if (value.Contains("."))
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，value设置失败，有小数点", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        if (Max.Contains("."))
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，Max设置失败，有小数点", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        if (Min.Contains("."))
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，min设置失败，有小数点", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        try
                        {
                            v = Convert.ToInt32(value);
                        }
                        catch
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，value设置失败，请输入int类型数据", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        try
                        {
                            vMax = Convert.ToInt32(Max);
                        }
                        catch
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，Max设置失败，请输入int类型数据", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        try
                        {
                            vMin = Convert.ToInt32(Min);
                        }
                        catch
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，min设置失败，请输入int类型数据", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        if (v > vMax || v < vMin)
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，value设置失败，不在规格内", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                    }
                    else if (type == "doubleUnit")
                    {
                        try
                        {
                            v = Convert.ToDouble(value);
                        }
                        catch
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，value设置失败，请输入int类型数据", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        try
                        {
                            vMax = Convert.ToDouble(Max);
                        }
                        catch
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，Max设置失败，请输入int类型数据", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        try
                        {
                            vMin = Convert.ToDouble(Min);
                        }
                        catch
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，min设置失败，请输入int类型数据", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }
                        if (v > vMax || v < vMin)
                        {
                            MessageBox.Show($"类别:{sort}名称:{name}，value设置失败，不在规格内", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return false;
                        }

                    }
                    else if (type == "stringUnit")
                    {

                    }
                    if (ItemName.Contains(name))
                    {
                        MessageBox.Show($"第{i}行存在重名参数{name},请检查", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        return false;
                    }
                    ItemName.Add(name);
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
           
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            bool bFind = Directory.Exists(ParamSetMgr.GetInstance().CurrentWorkDir);
            if (!bFind)
            {
                MessageBox.Show("没有产品文件，请设置", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
               
            Form_Input form_Input = new Form_Input("产品名称");
            if(DialogResult.OK ==form_Input.ShowDialog())
            {
                if (form_Input.InputText!="")
                {
                    string str = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + form_Input.InputText + ("\\")+ form_Input.InputText+(".xml");
                    Directory.CreateDirectory(ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + form_Input.InputText);
                    XmlDocument document = new XmlDocument();
                    XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", "no");
                    document.AppendChild(dec);
                    XmlElement root = document.CreateElement("ParamCfg");
                    document.AppendChild(root);
                    XmlElement item = document.CreateElement("ParamSet");
                    root.AppendChild(item);
                    document.Save(str);
                    treeView_ProdutFile.Nodes[1].Nodes.Add(form_Input.InputText);
                    document.RemoveAll();
                    document = null;

                }  
            }
            treeView_ProdutFile.Nodes[0].Expand();
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
        private void BtnOtherSave_Click(object sender, EventArgs e)
        {
            bool bFind = Directory.Exists(ParamSetMgr.GetInstance().CurrentWorkDir);
            if (!bFind)
            {
                MessageBox.Show("没有产品文件，请设置", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            Form_Input form_Input = new Form_Input("产品名称");
            if (DialogResult.OK == form_Input.ShowDialog())
            {
                if (form_Input.InputText == "" || form_Input.InputText.Trim() == "")
                {
                    MessageBox.Show("产品名称为空！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }
                string OldDir = $"{ParamSetMgr.GetInstance().CurrentWorkDir}\\{ParamSetMgr.GetInstance().CurrentProductFile}\\";
                string NewDir = $"{ParamSetMgr.GetInstance().CurrentWorkDir}\\{form_Input.InputText.Trim()}\\";
                if (Directory.Exists(NewDir))
                {
                    MessageBox.Show("该产品已存在！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }
                Directory.CreateDirectory($"{NewDir}");
                CopyDir(OldDir, NewDir, $"{ParamSetMgr.GetInstance().CurrentProductFile}.xml", $"{ form_Input.InputText.Trim()}");
                treeView_ProdutFile.Nodes[1].Nodes.Add(form_Input.InputText);
                treeView_ProdutFile.ExpandAll();
                //if (form_Input.InputText != "")
                //
                //    if (ParamSetMgr.GetInstance().CurrentWorkDir == "")
                //        return;
                //    string strDstFile = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + form_Input.InputText + ("\\") + form_Input.InputText + (".xml");
                //    Directory.CreateDirectory(ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + form_Input.InputText);
                //    string currentFilePath = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + ("\\") + ParamSetMgr.GetInstance().CurrentProductFile + (".xml");
                //    XmlDocument xmlDocument = new XmlDocument();
                //    if (File.Exists(currentFilePath))
                //    {
                //        xmlDocument.Load(currentFilePath);
                //        xmlDocument.Save(strDstFile);
                //        treeView_ProdutFile.Nodes[1].Nodes.Add(form_Input.InputText);
                //        treeView_ProdutFile.ExpandAll();
                //    }



                //}
            }
        }
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
            }
        }
        //public static void DeleteFolder(string dirPath)
        //{
        //    if (Directory.Exists(dirPath))
        //    {
        //        foreach (string content in Directory.GetFileSystemEntries(dirPath))
        //        {
        //            if (Directory.Exists(content))
        //            { Directory.Delete(content, true); }
        //            else if (File.Exists(content))
        //            { File.Delete(content); }
        //        }
        //    }
        //}

        private void BtnDel_Click(object sender, EventArgs e)
        {
        
            
            if (treeView_ProdutFile.SelectedNode == null)
            {
                 MessageBox.Show("请选择要添加子节点的节点！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(treeView_ProdutFile.SelectedNode== treeView_ProdutFile.Nodes[0])
            {
                MessageBox.Show("不能删除根结点！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ParamSetMgr.GetInstance().CurrentWorkDir == "")
            {
                MessageBox.Show("当前产品目录文件夹！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool bFind = Directory.Exists(ParamSetMgr.GetInstance().CurrentWorkDir);
            if (!bFind)
            {
                MessageBox.Show("当前产品目录文件夹！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(treeView_ProdutFile.SelectedNode== treeView_ProdutFile.Nodes[0].Nodes[0])
            {
                MessageBox.Show("当前产品文件不能删除，请先载入！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string strdirPath = ParamSetMgr.GetInstance().CurrentWorkDir + ("\\")+treeView_ProdutFile.SelectedNode.Text;
            if (MessageBox.Show($"是否确定删除{treeView_ProdutFile.SelectedNode.Text}", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;
            if (Directory.Exists(strdirPath))
            {
                // DeleteFolder(strdirPath);
                FileOpert.DeleteFolder(strdirPath);
             
            }
            treeView_ProdutFile.SelectedNode.Remove();
        }

        private void roundButtonLoad_Click(object sender, EventArgs e)
        {
            if (treeView_ProdutFile.SelectedNode == null)
            {
                MessageBox.Show("请选择要载入的文件！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (treeView_ProdutFile.SelectedNode == treeView_ProdutFile.Nodes[0])
            {
                MessageBox.Show("选择错误！","Err",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (ParamSetMgr.GetInstance().CurrentWorkDir == "")
            {
                MessageBox.Show("当前产品目录文件夹为空！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool bFind = Directory.Exists(ParamSetMgr.GetInstance().CurrentWorkDir);
            if (!bFind)
            {
                MessageBox.Show("当前产品目录文件夹不存在！", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ParamSetMgr.GetInstance().CurrentProductFile == treeView_ProdutFile.SelectedNode.Text)
                return;
             string strOldFile = ParamSetMgr.GetInstance().CurrentProductFile;
            if (!File.Exists(ParamSetMgr.GetInstance().CurrentWorkDir + ("\\") + treeView_ProdutFile.SelectedNode.Text + ("\\") + treeView_ProdutFile.SelectedNode.Text + (".xml")))
                 return;
            if (ParamSetMgr.GetInstance().CurrentProductFile != treeView_ProdutFile.SelectedNode.Text  )
            ParamSetMgr.GetInstance().CurrentProductFile = treeView_ProdutFile.SelectedNode.Text;
         
            ConfigToolMgr.GetInstance().SaveProductFile();

            //删除之前登陆产品文件名
            if (treeView_ProdutFile.Nodes.Count>0&& treeView_ProdutFile.Nodes[0].GetNodeCount(true)>0)
                treeView_ProdutFile.Nodes[0].Nodes[0].Remove();
            treeView_ProdutFile.Nodes[0].Nodes.Add(ParamSetMgr.GetInstance().CurrentProductFile);
           
            //删除当前登陆产品文件名
            string strNewFile = ParamSetMgr.GetInstance().CurrentProductFile;
            for (int i = 0; i < treeView_ProdutFile.Nodes[1].Nodes.Count; i++)
            {
                if (treeView_ProdutFile.Nodes[1].Nodes[i].Text == strNewFile)
                {
                    treeView_ProdutFile.Nodes[1].Nodes[i].Collapse();
                    treeView_ProdutFile.Nodes[1].Nodes.Remove(treeView_ProdutFile.Nodes[1].Nodes[i]);
                    //if (treeView_ProdutFile.Nodes[1].Nodes[i].Nodes.Count > 0)
                    //{
                    //    foreach (TreeNode node in (treeView_ProdutFile.Nodes[1].Nodes[i].Nodes))
                    //    {
                    //        DeleteTreeNode2(node);
                    //    }
                    //}
                }
            }
            treeView_ProdutFile.Update();
            if (treeView_ProdutFile.Nodes[0].FirstNode != null && treeView_ProdutFile.Nodes[0].FirstNode.Name != null && treeView_ProdutFile.Nodes[0].FirstNode.Text != "")
            {
                List<string> classList = ParamSetMgr.GetInstance().GetParamClassList();
                foreach (var temp in classList)
                    treeView_ProdutFile.Nodes[0].Nodes[0].Nodes.Add(temp);
            }
            treeView_ProdutFile.Nodes[0].ExpandAll();
            //增加之前登陆产品文件名
            treeView_ProdutFile.Nodes[1].Nodes.Add(strOldFile);
           
        }
        protected void DeleteTreeNode2(TreeNode node)
        {
            //后序遍历
            foreach (TreeNode child in node.Nodes)
            {
                DeleteTreeNode2(child);
            }
            node.Remove();
        }

        private void AddNewClass_Click(object sender, EventArgs e)
        {
            Form_Input form_Input = new Form_Input("产品名称");
            if (treeView_ProdutFile.Nodes[0].Name == null|| treeView_ProdutFile.Nodes[0].FirstNode==null || treeView_ProdutFile.Nodes[0].FirstNode.Text == "")
                return;
            if (DialogResult.OK == form_Input.ShowDialog())
            {
                if (form_Input.InputText != "")
                {
                    if (form_Input.InputText == "all")
                        return;
                    DataGridViewComboBoxColumn combox = (DataGridViewComboBoxColumn)dataGridView_Param.Columns[6];
                    if(!combox.Items.Contains(form_Input.InputText))
                    {
                        combox.Items.Add(form_Input.InputText);
                        treeView_ProdutFile.Nodes[0].Nodes[0].Nodes.Add(form_Input.InputText);
                    }
                    
                }
            }
        }

        private void treeView_ProdutFile_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView_ProdutFile.Nodes[0] == null || treeView_ProdutFile.Nodes[0].Nodes.Count==0 ||treeView_ProdutFile.Nodes[0].Nodes[0] == null)
                return;
            if (e.Node == null || e.Node.Parent == null)
                return;
            if( e.Node.Parent.Text== treeView_ProdutFile.Nodes[0].Nodes[0].Text)
            {
                //  e.Node.Text
                FlushParamToScreen(sys.g_User, e.Node.Text);
            }
            if(e.Node.Parent.Text == "当前产品")
            {
                FlushParamToScreen(sys.g_User, "all");
            }
        }
        public void FlushParamToScreen(User CurrentUser, string strParmClass)
        {
           
                System.Diagnostics.Debug.WriteLine("start updata\n");
                // Console.WriteLine("start updata\n");
                userRight = CurrentUser._userRight;
                dataGridView_Param.Rows.Clear();
                ParamSetMgr.GetInstance().ClearAllParamClassList();
                Dictionary<string, ParamSet> allparam = ParamSetMgr.GetInstance().GetAllParam();
              

                int index = 0;
                foreach (var temp in allparam)
                {
                  if(strParmClass!= "all")
                      if (temp.Value._ParamClass != strParmClass)
                         continue;
                    dataGridView_Param.Rows.Add(temp.Key, temp.Value._enuValType.ToString(),
                         temp.Value._strParamVal, temp.Value._strParamValMax, temp.Value._strParamValMin,
                         temp.Value._ParamRight.ToString(), temp.Value._ParamClass.ToString());
                    if ((int)CurrentUser._userRight >= (int)UserRight.软件工程师)
                    {
                        dataGridView_Param.Rows[index].ReadOnly = false;
                    }
                    else if ((int)(UserRight)Enum.Parse(typeof(UserRight), dataGridView_Param.Rows[index].Cells[5].Value.ToString()) <= (int)CurrentUser._userRight)
                    {
                        //dataGridView_Param.Rows[index].ReadOnly = false;
                        dataGridView_Param.Rows[index].Cells[0].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[1].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[2].ReadOnly = false;
                        dataGridView_Param.Rows[index].Cells[3].ReadOnly = false;
                        dataGridView_Param.Rows[index].Cells[4].ReadOnly = false;
                        dataGridView_Param.Rows[index].Cells[5].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[6].ReadOnly = true;

                    }
                    else
                    {
                        dataGridView_Param.Rows[index].Cells[0].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[1].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[2].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[3].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[4].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[5].ReadOnly = true;
                        dataGridView_Param.Rows[index].Cells[6].ReadOnly = true;
                    }

                    index++;
                }
                if ((int)CurrentUser._userRight < (int)UserRight.软件工程师)
                    dataGridView_Param.AllowUserToAddRows = false;
                else
                    dataGridView_Param.AllowUserToAddRows = true;
                System.Diagnostics.Debug.WriteLine("end updata\n");
                // Console.WriteLine("end updata\n");
            }

        
      
        private void dataGridView_Param_Click(object sender, EventArgs e)
        {
            try
            {
                int RowIndex = dataGridView_Param.CurrentCell.RowIndex;
                int ColumnIndex = dataGridView_Param.CurrentCell.ColumnIndex;
                if (ColumnIndex >= 0 && RowIndex >= 0)
                {
                    richTextBox1.Text = $"参数名:{ dataGridView_Param[0, RowIndex].Value}" + Environment.NewLine;
                    richTextBox1.Text += $"当前值:{ dataGridView_Param[2, RowIndex].Value}" + Environment.NewLine;
                    richTextBox1.Text += $"允许最大值:{ dataGridView_Param[3, RowIndex].Value}" + Environment.NewLine;
                    richTextBox1.Text += $"允许最小值:{ dataGridView_Param[4, RowIndex].Value}" + Environment.NewLine;
                    if (dataGridView_Param[0, RowIndex].Value == null)
                        return;
                    var de = DescriptionClass.paramValue.Find(p => p.ParamName == (dataGridView_Param[0, RowIndex].Value).ToString().Trim());
                    if (de == null)
                    {
                        richTextBox1.Text += $"参数描述:还未添加描述，请在Description.xml中添加" + Environment.NewLine;
                    }
                    else
                    {
                        richTextBox1.Text += $"参数描述:{ de.Description}" + Environment.NewLine;
                    }
                }
            }
            catch(Exception ex)
            {
                return;
            }
           
        }
        private static void CopyDir(string srcPath, string aimPath, string productName, string newName)
        {
            try
            {
                if (aimPath[aimPath.Length - 1] != System.IO.Path.AltDirectorySeparatorChar)
                {
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                }
                if (!System.IO.Directory.Exists(aimPath))
                {
                    Directory.CreateDirectory(aimPath);
                }
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                foreach (string file in fileList)
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        CopyDir(file, aimPath + Path.GetFileName(file), productName, newName);
                    }
                    else
                    {
                        try
                        {
                            string fileFullName = aimPath + Path.GetFileName(file);
                            if (productName == Path.GetFileName(file))
                            {
                                File.Copy(file, $"{aimPath}\\{newName}.xml", true);
                            }
                           
                            else
                            {
                                File.Copy(file, fileFullName, true);
                            }

                        }
                        catch { }
                    }
                }
            }
            catch
            {
            }
        }

    }
    //描述文件
    public class DescriptionClass
    {
        public static List<ParamValue> paramValue { get; set; } = new List<ParamValue>();
    }
    public class ParamValue
    {
        public string ParamName { get; set; }
        public string Description { get; set; }
    }
}
