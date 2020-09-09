using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StationDemo
{
    public partial class GetDataState : Form
    {
        public GetDataState()
        {
            InitializeComponent();
        }

        private void GetDataState_Load(object sender, EventArgs e)
        {

        }
        public void FlushNozzleState(string strstate)
        {
            txtNozzleStates.Text = strstate;
        }
        public void FlushStateState(string strstate)
        {
            txtSoketStates.Text = strstate;
        }
    }
}
