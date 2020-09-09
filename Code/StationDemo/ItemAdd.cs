
using BaseDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionProcess;

namespace StationDemo
{
   
    public partial class ItemAdd : Form
    {
        public ItemAdd(string[] camlist)
        {
            InitializeComponent();
            comboBox_camSel.Items.Clear();
            if(camlist!=null)
            comboBox_camSel.Items.AddRange(camlist);
            if (camlist?.Length > 0)
            {
                comboBox_camSel.Text = camlist[0];
            }
        }
        public double Gain { private set; get; }
        public  double Exposure { private set; get; }
        public  string ItemName { private set; get; }

        public int nLightVal;
        public string CamName { private set; get; }
        public string VisionProcssName { private set; get; }

        public VisionSetpBase visionSetpBase = null;


        private void roundButton_Add_Click(object sender, EventArgs e)
        {
            VisionProcssName = comboBox_SelVisionProcessType.Text;
            CamName = comboBox_camSel.Text;
            Exposure = textBox_ExposureTime.Text.ToDouble();
            Gain = textBox_Gain.Text.ToDouble();
            ItemName=textBox_ItemName.Text;
            nLightVal = txtLightVal.Text.ToInt();
            if (Gain ==0 || Exposure==0 || VisionProcssName=="" || ItemName=="")
            {
                MessageBox.Show("参数设置错误", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                Close();
                Dispose();
                if (VisionMgr.GetInstance().GetItemNamesAndTypes().ContainsKey(ItemName))
                {
                    MessageBox.Show($"{ItemName} 已经存在此项，重名了", "Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Type ty = AssemblyOperate.GetTypeFromAssemblyByDescrition(VisionProcssName, typeof(VisionSetpBase));
                visionSetpBase = Activator.CreateInstance(ty, ItemName) as VisionSetpBase;
                visionSetpBase.m_camparam.m_dExposureTime = Exposure;
                visionSetpBase.m_camparam.m_dGain = Gain;
                visionSetpBase.m_camparam.m_strCamName = CamName;
                StepVisionInfo stepVisionInfo = new StepVisionInfo();
                stepVisionInfo.CamParam = visionSetpBase.m_camparam;
                stepVisionInfo.VisionType = ty.ToString();
                stepVisionInfo.nLightVal = nLightVal;
                VisionMgr.GetInstance().Add(ItemName, visionSetpBase, stepVisionInfo);
                VisionMgr.GetInstance().Save();
                this.DialogResult = DialogResult.Yes;
            }
        }

        private void ItemAdd_Load(object sender, EventArgs e)
        {
            comboBox_SelVisionProcessType.Items.Clear();
            List<Type> TypeList = AssemblyOperate.GetAllSubClassTypeOnRunDir(typeof(VisionSetpBase));
            foreach (var temp  in TypeList)
            {
                string DescriptionName = AssemblyOperate.GetDescription(temp);
                if(DescriptionName != "NoDescription")
                {
                    comboBox_SelVisionProcessType.Items.Add(DescriptionName);
                    comboBox_SelVisionProcessType.SelectedIndex = 0;
                }
            }
     
        }
    }
}
