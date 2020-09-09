using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserCtrl
{
    public partial class UserButton : UserControl
    {
        public UserButton()
        {
            InitializeComponent();
        }
        [Category("General"), Description("Control's Text"), Browsable(true)]

        public override string Text
        {
            set { label_Name.Text = value;
                Name = value;
            }
            get { return label_Name.Text; }
        }
        public string Name
        {
            set { label_Name.Text = value; }
            get { return label_Name.Text; }
        }
        bool m_bOldState = false;
        public bool State
        {
            set
            {
                if (m_bOldState != value ||( button_Operate.Text== "ON"&& !value)
                    || (button_Operate.Text == "OFF" && value))
                {
                    button_Operate.Text = value ? "ON" : "OFF";
                    button_Operate.BackColor = value ? Color.LightGreen : Color.LightBlue;
                    Update();
                }
                m_bOldState = value;
            }
            get
            {
                return m_bOldState;
            }
        }
        public delegate void ClickFunHandler(string name);
        public event ClickFunHandler m_eventClick;

        public delegate void ClickDownFunHandler(string name);
        public event ClickDownFunHandler m_eventClickDown;

        public delegate void ClickUpFunHandler(string name);
        public event ClickUpFunHandler m_eventClickUp;
        private void UserButton_Load(object sender, EventArgs e)
        {
            button_Operate.Text = "OFF";
            button_Operate.BackColor = Color.LightBlue;
            Update();
        }

        private void button_Operate_Click(object sender, EventArgs e)
        {
            if(m_eventClick!=null)
                 m_eventClick(this.Text);
        }

        private void button_Operate_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_eventClickDown != null)
                m_eventClickDown(this.Text);

        }

        private void button_Operate_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_eventClickUp != null)
                m_eventClickUp(this.Text);
        }
    }
}
