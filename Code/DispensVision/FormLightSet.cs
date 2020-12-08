using System;
using System.Windows.Forms;

namespace XYZDispensVision
{
    public partial class FormLightSet : Form
    {
        public FormLightSet()
        {
            InitializeComponent();
        }

        public int nCh = 1;
        public int nLightVal = 0;

        private void btnSure_Click(object sender, EventArgs e)
        {
            if (comboBoxSelCH.Text != null)
                nCh = Convert.ToInt32(comboBoxSelCH.Text);
            if (textBoxLightVal.Text != null)
                nLightVal = Convert.ToInt32(textBoxLightVal.Text);
            else
                nLightVal = -1;
            this.Close();
            this.Dispose();
            this.DialogResult = DialogResult.OK;
        }

        private void FormLightSet_Load(object sender, EventArgs e)
        {
            textBoxLightVal.Text = nLightVal.ToString();
            comboBoxSelCH.Text = nCh.ToString();
        }
    }
}