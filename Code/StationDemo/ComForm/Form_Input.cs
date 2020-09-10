using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StationDemo
{
    public partial class Form_Input : Form
    {
        public Form_Input(string str)
        {
            InitializeComponent();
            label_InputInfo.Text = label_InputInfo.Text + str;
        }
        public string InputText;
        private void roundButton1_Click(object sender, EventArgs e)
        {
            InputText = textBox_Input.Text;
            DialogResult = DialogResult.OK;
            this.Close();

        }
    }
}
