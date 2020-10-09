using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BaseDll;
using CameraLib;
using HalconDotNet;
using VisionProcess;
namespace StationDemo
{
    public partial class Form_VisionDebug : Form
    {
        public Form_VisionDebug()
        {
            InitializeComponent();
        }
        public void ChangedUserRight(User CurrentUser)
        {
            if (InvokeRequired)
                this.BeginInvoke(new Action(() => ChangedUserRight(CurrentUser)));
            else
            {

                switch ((int)CurrentUser._userRight)
                {
                    case (int)UserRight.客户操作员:
                        dataGridViewProcessItem.AllowUserToAddRows = false;
                        dataGridViewProcessItem.AllowUserToDeleteRows = false;
                        dataGridViewProcessItem.Columns[0].ReadOnly = true;
                        dataGridViewProcessItem.Columns[1].ReadOnly = true;
                        dataGridViewProcessItem.Columns[2].ReadOnly = true;
                        dataGridViewProcessItem.Columns[3].ReadOnly = true;
                        dataGridViewProcessItem.Columns[4].ReadOnly = true;
                        break;
                    case (int)UserRight.调试工程师:

                        dataGridViewProcessItem.AllowUserToAddRows = false;
                        dataGridViewProcessItem.AllowUserToDeleteRows = false;
                        dataGridViewProcessItem.Columns[0].ReadOnly = true;
                        dataGridViewProcessItem.Columns[1].ReadOnly = true;
                        dataGridViewProcessItem.Columns[2].ReadOnly = true;
                        dataGridViewProcessItem.Columns[3].ReadOnly = false;
                        dataGridViewProcessItem.Columns[4].ReadOnly = false;
                        break;
                    case (int)UserRight.软件工程师:
                        dataGridViewProcessItem.AllowUserToAddRows = false;
                        dataGridViewProcessItem.AllowUserToDeleteRows = false;
                        dataGridViewProcessItem.Columns[0].ReadOnly = true;
                        dataGridViewProcessItem.Columns[1].ReadOnly = true;
                        dataGridViewProcessItem.Columns[2].ReadOnly = true;
                        dataGridViewProcessItem.Columns[3].ReadOnly = false;
                        dataGridViewProcessItem.Columns[4].ReadOnly = false;
                        break;
                    case (int)UserRight.超级管理员:
                        dataGridViewProcessItem.AllowUserToAddRows = false;
                        dataGridViewProcessItem.AllowUserToDeleteRows = false;
                        dataGridViewProcessItem.Columns[0].ReadOnly = true;
                        dataGridViewProcessItem.Columns[1].ReadOnly = true;
                        dataGridViewProcessItem.Columns[2].ReadOnly = true;

                        dataGridViewProcessItem.Columns[3].ReadOnly = false;
                        dataGridViewProcessItem.Columns[4].ReadOnly = false;
                        break;
                }
                bool bEnable = true;
                if ((int)CurrentUser._userRight >= (int)UserRight.调试工程师)
                {
                    bEnable = true;

                }
                else
                {
                    bEnable = false;


                }

            }

        }
        List<Type> TypeLists = AssemblyOperate.GetAllSubClassTypeOnRunDir(typeof(VisionSetpBase));
        void UpdataVisionItems()
        {

            VisionSetpBase visionSetpBase = null;
            if (VisionMgr.GetInstance().GetItemNamesAndTypes() == null)
                return;
            dataGridViewProcessItem.Rows.Clear();
            foreach (var tem in VisionMgr.GetInstance().GetItemNamesAndTypes())
            {
                int index = tem.Value.VisionType.LastIndexOf(".");
                string strVisionProcessType = tem.Value.VisionType.Substring(index + 1);
                string strVisionTypeName = "";
                if (TypeLists != null)
                {
                  Type type=  TypeLists.Find((t) =>
                    {
                        return tem.Value.VisionType == t.ToString() ? true : false;
                    });
                    strVisionTypeName = AssemblyOperate.GetDescription(type);
                }
                  
                    
                    

               visionSetpBase = VisionMgr.GetInstance().GetItem(tem.Key);
                dataGridViewProcessItem.Rows.Add("False", tem.Key, strVisionTypeName, visionSetpBase.m_camparam.m_strCamName, visionSetpBase.m_camparam.m_dExposureTime.ToString(), visionSetpBase.m_camparam.m_dGain.ToString());
            }

        }

        private void Form_CameraDebug_Load(object sender, EventArgs e)
        {

            visionControl1.InitWindow();
            HOperatorSet.SetDraw(visionControl1.GetHalconWindow(), "margin");
      
            Thread.Sleep(10);
            List<string> camname = CameraMgr.GetInstance().GetCameraNameArr();
            foreach (var temp in camname)
            {
                this.Sel.Items.Add(temp.ToString());
                comboBox_SelCam.Items.Add(temp.ToString());
                CameraMgr.GetInstance().ClaerPr(temp.ToString());
            }
            if (camname.Count > 0)
                comboBox_SelCam.SelectedIndex = 0;
            DataGridViewComboBoxColumn sd =(DataGridViewComboBoxColumn)dataGridViewProcessItem.Columns[2];
            sd.Items.Clear();
           
            TypeLists = AssemblyOperate.GetAllSubClassTypeOnRunDir(typeof(VisionSetpBase));
            foreach (var temp in TypeLists)
            {
                string DescriptionName = AssemblyOperate.GetDescription(temp);
                if (DescriptionName != "NoDescription")
                {
                    sd.Items.Add(DescriptionName);
                } 
            }
            dataGridViewProcessItem.Columns[1].ReadOnly = true;
            dataGridViewProcessItem.Columns[2].ReadOnly = true;
            dataGridViewProcessItem.Columns[3].ReadOnly = true;
            textBox_exposureTimeVal.Text = CameraMgr.GetInstance().GetCamExposure(comboBox_SelCam.Text).ToString();
            textBox_GainVal.Text = CameraMgr.GetInstance().GetCamGain(comboBox_SelCam.Text).ToString();
            CameraMgr.GetInstance().BindWindow(comboBox_SelCam.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCam.Text);
            UpdataVisionItems();
            ParamSetMgr.GetInstance().m_eventLoadProductFileUpadata += UpdataVisionItems;
        }

        private void roundButton_ContinuousSnap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox_SelCam.Items.Count; i++)
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCam.Items[i].ToString());
               

            CameraMgr.GetInstance().BindWindow(comboBox_SelCam.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCam.Text);
            CameraMgr.GetInstance().GetCamera(comboBox_SelCam.Text).StartGrab();
            // textBox_exposureTimeVal.Text = CameraMgr.GetInstance().GetCamExposure(comboBox_SelCam.Text).ToString();
            //  textBox_GainVal.Text = CameraMgr.GetInstance().GetCamGain(comboBox_SelCam.Text).ToString();
            CameraMgr.GetInstance().SetCamExposure(comboBox_SelCam.Text, textBox_exposureTimeVal.Text.ToDouble());
            CameraMgr.GetInstance().SetCamGain(comboBox_SelCam.Text, textBox_GainVal.Text.ToDouble());
          
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox_SelCam.Items.Count; i++)
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCam.Items[i].ToString());
            CameraMgr.GetInstance().BindWindow(comboBox_SelCam.Text, visionControl1);
           
            CameraMgr.GetInstance().GetCamera(comboBox_SelCam.Text).StopGrap();
            CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCam.Text);
            // textBox_exposureTimeVal.Text = CameraMgr.GetInstance().GetCamExposure(comboBox_SelCam.Text).ToString();
            //  textBox_GainVal.Text = CameraMgr.GetInstance().GetCamGain(comboBox_SelCam.Text).ToString();
            CameraMgr.GetInstance().SetCamExposure(comboBox_SelCam.Text, textBox_exposureTimeVal.Text.ToDouble());
            CameraMgr.GetInstance().SetCamGain(comboBox_SelCam.Text, textBox_GainVal.Text.ToDouble());
            CameraMgr.GetInstance().GetCamera(comboBox_SelCam.Text).SetTriggerMode(CameraModeType.Software);

            CameraMgr.GetInstance().GetCamera(comboBox_SelCam.Text).StartGrab();
            CameraMgr.GetInstance().GetCamera(comboBox_SelCam.Text).GarbBySoftTrigger();
        }

        private void comboBox_SelCam_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox_SelCam.Items.Count; i++)
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCam.Items[i].ToString());
            textBox_exposureTimeVal.Text = CameraMgr.GetInstance().GetCamExposure(comboBox_SelCam.Text).ToString();
            textBox_GainVal.Text = CameraMgr.GetInstance().GetCamGain(comboBox_SelCam.Text).ToString();
            CameraMgr.GetInstance().BindWindow(comboBox_SelCam.Text, visionControl1);
            CameraMgr.GetInstance().SetAcquisitionMode(comboBox_SelCam.Text);
        }

        private void roundButton_DrawRect1_Click(object sender, EventArgs e)
        {
           
        }
        int indexSelVisionSel = -1;
        private void roundButton_DelItem_Click(object sender, EventArgs e)
        {
            int CellSelectedRow = dataGridViewProcessItem.SelectedCells[0].RowIndex;
            if (dataGridViewProcessItem.Rows[CellSelectedRow].Cells[0].Value == null)
                return;
            string strItemName = dataGridViewProcessItem.Rows[CellSelectedRow].Cells[1].Value.ToString();
            DataGridViewRow row = dataGridViewProcessItem.Rows[CellSelectedRow];
            dataGridViewProcessItem.Rows.Remove(row);
            VisionMgr.GetInstance().DelItem(strItemName);
            VisionMgr.GetInstance().Save();
            
            foreach (var temp in panel_VisionCtrls.Controls)
                ((Control)temp).Hide();
            if (indexSelVisionSel != -1 && indexSelVisionSel == CellSelectedRow)
                indexSelVisionSel = -1;
        }

        /// <summary>
        /// 判断视觉选项是否填写完整
        /// </summary>
        /// <returns></returns>
        public bool IsItemOK(int indexSel)
        {
            #region 界面判断提示
            if (indexSel < 0 || indexSel >= dataGridViewProcessItem.Rows.Count)
                return false;
            if (dataGridViewProcessItem.Rows[indexSel].Cells[2].Value == null
                 || dataGridViewProcessItem.Rows[indexSel].Cells[2].Value.ToString() == "")
            {
                MessageBox.Show("视觉类型为空，请选择", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //if (dataGridViewProcessItem.Rows[indexSel].Cells[3].Value == null
            //    || dataGridViewProcessItem.Rows[indexSel].Cells[3].Value.ToString() == "")
            //{
            //    MessageBox.Show("相机为空，请选择", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
            if (dataGridViewProcessItem.Rows[indexSel].Cells[4].Value == null
                  || dataGridViewProcessItem.Rows[indexSel].Cells[4].Value.ToString() == "")
            {
                MessageBox.Show("相机曝光为空，请填写", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (dataGridViewProcessItem.Rows[indexSel].Cells[5].Value == null
                 || dataGridViewProcessItem.Rows[indexSel].Cells[5].Value.ToString() == "")
            {
                MessageBox.Show("相机增益为空，请填写", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            #endregion 界面判断提示
            return true;
        }

        private void roundButton_Save_Click(object sender, EventArgs e)
        {
            //    VisionMgr.GetInstance().Save();
            if (indexSelVisionSel == -1)
                return;
            if (dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[1].Value == null ||
                dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[1].Value.ToString() == "")
                return;
            string strItem = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[1].Value.ToString();

            //判断视觉Item是否填写完整

            if (!IsItemOK(indexSelVisionSel))
            {
                return;
            }
            string strVisionType = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[2].Value.ToString();
            double dExposure = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[4].Value.ToString().ToDouble();
            double dGain = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[5].Value.ToString().ToDouble();
            string strCamName = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[3].Value.ToString();
            int nlight=   dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[3].Value.ToString().ToInt();
            VisionMgr.GetInstance().SetLightVal(strItem, nlight);
            VisionSetpBase visionSetpBase = VisionMgr.GetInstance().GetItem(strItem);

            visionSetpBase.m_camparam.m_dGain = dGain;
            visionSetpBase.m_camparam.m_dExposureTime = dExposure;
            visionSetpBase.m_camparam.m_strCamName = strCamName;
            visionSetpBase.SaveParm();
            visionSetpBase.Save();
            VisionMgr.GetInstance().Save();
        }
      
 
      
        private void dataGridViewProcessItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
            if (this.dataGridViewProcessItem.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType() != typeof(DataGridViewCheckBoxCell))
                return;
            if (this.dataGridViewProcessItem.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType() == typeof(DataGridViewCheckBoxCell))
            {
                for (int i = 0; i < this.dataGridViewProcessItem.RowCount; i++)
                {
                    this.dataGridViewProcessItem.Rows[i].Cells[e.ColumnIndex].Value = false;
                }
                //this.dataGridViewProcessItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                indexSelVisionSel = e.RowIndex;
               
            }
            DataGridViewCheckBoxCell checkCell =(DataGridViewCheckBoxCell) this.dataGridViewProcessItem.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if ((bool)checkCell.EditedFormattedValue==true)
                indexSelVisionSel = e.RowIndex;
            else
                indexSelVisionSel = -1;
            foreach (var temp in panel_VisionCtrls.Controls)
                ((Control)temp).Hide();
      
            if (indexSelVisionSel==-1)
            {
                dataGridViewProcessItem.Columns[2].ReadOnly = true;
                return;
            }
                
            dataGridViewProcessItem.Columns[2].ReadOnly = false;
            
            string ItemName = "";
            ItemName = dataGridViewProcessItem.Rows[e.RowIndex].Cells[1].Value.ToString();
            VisionSetpBase visionSetpBase = VisionMgr.GetInstance().GetItem(ItemName);
            //if(
            //    dataGridViewProcessItem.Rows[e.RowIndex].Cells[2].Value.ToString()!="" &&
            //    AssemblyOperate.GetDescription(visionSetpBase.GetType())!="NoDescrition"&&
            //    dataGridViewProcessItem.Rows[e.RowIndex].Cells[2].Value.ToString()!= AssemblyOperate.GetDescription(visionSetpBase.GetType()))
            //{
            //    string str = visionSetpBase.m_strStepName + "  当前的视觉类型为：" + AssemblyOperate.GetDescription(visionSetpBase.GetType()) + "是否切换到视觉类型:" + dataGridViewProcessItem.Rows[e.RowIndex].Cells[2].Value.ToString();
            //    DialogResult dialogResult= MessageBox.Show(str, "Err", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            //    if(dialogResult== DialogResult.Yes)
            //    {
            //        visionSetpBase.Disopose();
            //        string camname = visionSetpBase.m_camparam.m_strCamName;
            //        double exposure = visionSetpBase.m_camparam.m_dExposureTime;
            //        double dGain = visionSetpBase.m_camparam.m_dGain;
            //        VisionMgr.GetInstance().DelItem(ItemName);
            //        visionSetpBase = null;
            //        string strtype = dataGridViewProcessItem.Rows[e.RowIndex].Cells[2].Value.ToString();
            //        visionSetpBase= VisionMgr.GetInstance().GetItem(ItemName);
            //    }
            //    else
            //      return;
            //}
            visionSetpBase.FlushToDlg(visionControl1, panel_VisionCtrls);
            if(dataGridViewProcessItem.Rows[e.RowIndex].Cells[2].Value.ToString()== "模板匹配")
            {
                roundButton_ShapeROIAdd.Show();
                roundButton_ShapeROISub.Show();
            }
            else
            {
                roundButton_ShapeROIAdd.Hide();
                roundButton_ShapeROISub.Hide();
            }

        
            roundButton_SeachArea.Show();

            roundButton_Test.Show();
  

        }

        private void roundButton_AddItem_Click(object sender, EventArgs e)
        {
            ItemAdd itemAdd = null;
            if (CameraMgr.GetInstance().GetCameraNameArr()!=null && CameraMgr.GetInstance().GetCameraNameArr().Count>0)
                itemAdd = new ItemAdd(CameraMgr.GetInstance().GetCameraNameArr().ToArray());
            else
                itemAdd = new ItemAdd(null);
            DialogResult dialogResult = itemAdd.ShowDialog();
            if (dialogResult == DialogResult.Yes)
            {
                dataGridViewProcessItem.Rows.Add("False", itemAdd.ItemName,
                   itemAdd.VisionProcssName, itemAdd.CamName,
                   itemAdd.Exposure.ToString(), itemAdd.Gain.ToString(),itemAdd.nLightVal.ToString());
            }
        }

        private void roundButton_SnapSave_Click(object sender, EventArgs e)
        {
           // roundButton1_Click(null, null);
            CameraMgr.GetInstance().SaveImg(comboBox_SelCam.Text);

        }

        private void roundButton_ReadImg_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox_SelCam.Items.Count; i++)
                CameraMgr.GetInstance().SetTriggerSoftMode(comboBox_SelCam.Items[i].ToString());
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
               // openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;//注意这里写路径时要用c:\\而不是c:\
                openFileDialog.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "打开图片";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //string  fName = openFileDialog.FileName;
                    string strPath = openFileDialog.FileName;
                    HObject img=null;
                    HOperatorSet.ReadImage(out img, strPath);
                    visionControl1.DispImageFull(img);
                }
            }

        }

     

        private void roundButton_Test_Click(object sender, EventArgs e)
        {
            if (indexSelVisionSel == -1)
            {
                MessageBox.Show("请勾选视觉处理项目", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string str = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[1].Value.ToString();
            VisionSetpBase visionSetpBase = VisionMgr.GetInstance().GetItem(str);
            Action action = new Action(() => { visionSetpBase.Process_image(visionControl1.Img, visionControl1); });

            action.BeginInvoke((ar) => { },null);
        }

        private void roundButton_SeachArea_Click(object sender, EventArgs e)
        {
            if (indexSelVisionSel == -1)
            {
                MessageBox.Show("请勾选视觉处理项目", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HTuple row1, col1, row2, col2;
            HObject obj = visionControl1.DrawRectangle(out row1, out col1, out row2, out col2);
            string str = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[1].Value.ToString();
            VisionSetpBase visionSetpBase = VisionMgr.GetInstance().GetItem(str);
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + str + "\\" + str + "_SearchRect.hobj";
            switch (dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[2].Value.ToString())
            {
                case "模板匹配":
                    ((VisionShapMatch)visionSetpBase).visionShapParam.SeachRectRegionPath = strPath;
                    HOperatorSet.WriteRegion(obj, strPath);
                    visionSetpBase.Save();
                    ((VisionShapMatch)visionSetpBase).Read();
                    break;
                case "二维码":
                    ((Vision2dCode)visionSetpBase).vision2dCodeParam.Mode2dcodeSearchPath = strPath;
                    HOperatorSet.WriteRegion(obj, strPath);
                    visionSetpBase.Save();
                    ((Vision2dCode)visionSetpBase).Read();
                    break;
                case "一维码":
                    ((Vision1dCode)visionSetpBase).vision1dCodeParam.Mode1dcodeSearchPath = strPath;
                    HOperatorSet.WriteRegion(obj, strPath);
                    visionSetpBase.Save();
                    ((Vision1dCode)visionSetpBase).Read();
                    break;

            }
            obj.Dispose();
        }

        private void roundButton_ShapeROIAdd_Click(object sender, EventArgs e)
        {
            if (indexSelVisionSel == -1)
            {
                MessageBox.Show("请勾选视觉处理项目", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HTuple row1, col1, row2, col2;
            HObject obj = visionControl1.DrawShape();
            if (obj == null)
                return;
            HObject oldRegion = null;
            string str = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[1].Value.ToString();
            VisionSetpBase visionSetpBase = VisionMgr.GetInstance().GetItem(str);
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + str + "\\" + str + "_Roi.hobj";
            switch (dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[2].Value.ToString())
            {
                case "模板匹配":
                    ((VisionShapMatch)visionSetpBase).visionShapParam.RoiRegionPath = strPath;
                    visionSetpBase.Save();
                    visionSetpBase.Read();
                    try
                    {
                        if (File.Exists(strPath))
                        {
                            HOperatorSet.ReadRegion(out oldRegion, strPath);
                            if (oldRegion != null && oldRegion.IsInitialized())
                            {
                                HOperatorSet.Union2(oldRegion, obj, out obj);
                                oldRegion?.Dispose();
                            }
                        }
                        HOperatorSet.WriteRegion(obj, strPath);
                        ((VisionShapMatch)visionSetpBase).SetRoiRegion(obj);
                        HOperatorSet.DispObj(obj, visionControl1.GetHalconWindow());
                        visionSetpBase.Save();
                    }
                    catch (HalconException e1)
                    {
                        MessageBox.Show(visionSetpBase.m_strStepName + "画ROi失败" + e1.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        oldRegion?.Dispose();
                        obj?.Dispose();
                    }
                    
                    break;
                case "二维码":
                    break;
                case "一维码":
                    break;

            }
            obj.Dispose();
        }

        private void roundButton_ShapeROISub_Click(object sender, EventArgs e)
        {
            if (indexSelVisionSel == -1)
            {
                MessageBox.Show("请勾选视觉处理项目", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HTuple row1, col1, row2, col2;
            HObject obj = visionControl1.DrawShape();
            if (obj == null)
                return;
            HObject oldRegion = null;
            string str = dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[1].Value.ToString();
            VisionSetpBase visionSetpBase = VisionMgr.GetInstance().GetItem(str);
            string strPath = VisionMgr.GetInstance().CurrentVisionProcessDir + "\\" + str + "\\" + str + "_Roi" + ".hobj";
            switch (dataGridViewProcessItem.Rows[indexSelVisionSel].Cells[2].Value.ToString())
            {
                case "模板匹配":
                    ((VisionShapMatch)visionSetpBase).visionShapParam.RoiRegionPath = strPath;
                    visionSetpBase.Read();
                    try
                    {
                        if (File.Exists(strPath))
                        {
                            HOperatorSet.ReadRegion(out oldRegion, strPath);
                            if (oldRegion != null && oldRegion.IsInitialized())
                            {
                                HOperatorSet.Difference(oldRegion, obj, out obj);
                               
                            }
                        }
                        HOperatorSet.WriteRegion(obj, strPath);
                       
                        ((VisionShapMatch)visionSetpBase).SetRoiRegion(obj);
                        HOperatorSet.DispObj(obj, visionControl1.GetHalconWindow());
                        visionSetpBase.Save();
                    }
                    catch (HalconException e1)
                    {
                        MessageBox.Show(visionSetpBase.m_strStepName + "画ROi失败" + e1.Message, "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        oldRegion?.Dispose();
                        obj?.Dispose();
                    }
               
                    break;
                case "二维码":
                    break;
                case "一维码":
                    break;

            }
            obj.Dispose();
        }

        private void OnShowChanged(object sender, EventArgs e)
        {
            List<string> camname = CameraMgr.GetInstance().GetCameraNameArr();
            foreach(var tem in camname)
                CameraMgr.GetInstance().ClaerPr(tem);

        }
    }
}
