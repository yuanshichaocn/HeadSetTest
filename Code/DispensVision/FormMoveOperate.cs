
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XYZDispensVision
{
    public partial class FormMoveOperate : Form
    {
        
        public FormMoveOperate()
        {
            InitializeComponent();
        }

        private void fourAxisesCtrl1_Load(object sender, EventArgs e)
        {

        }

        private void FormMoveOperate_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            
        }

        private void FormMoveOperate_Load(object sender, EventArgs e)
        {
           // fourAxisesCtrl1.Init(Device.GetDevice().FRobot);
        }
    }
}
