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
    [DefaultProperty("Text")]
    public partial class UserLabel: UserControl
    {
        public UserLabel()
        {
            InitializeComponent();
          
            label_State.Invalidate();
        }
        public string Name
        {
            set { label_Name.Text = value; }
            get { return label_Name.Text; }
        }
        bool m_bOldState=false;
        [Description("TEXT"),Category("Label_Text")]
        public string Text
        {
            set { label_Name.Text = value; }
            get { return label_Name.Text; }
        }
        public bool State
        {
            set {
                if(m_bOldState!= value  )
                {
                    label_State.Text = value? "ON":"OFF";
                    label_State.BackColor = value ? Color.LightGreen : Color.LightBlue;
                    Update();
                }
                if(!value && label_State.BackColor != Color.LightBlue)
                {
                    label_State.BackColor = value ? Color.LightGreen : Color.LightBlue;
                    Update();
                }
                if (value && label_State.BackColor != Color.LightGreen)
                {
                    label_State.BackColor = value ? Color.LightGreen : Color.LightBlue;
                    Update();
                }
                m_bOldState = value;
                Update();
            }
            get
            {
                return m_bOldState;
            }


        }
     


        private void UserLabel_Load(object sender, EventArgs e)
        {

            //State = false;
            label_State.Text = "OFF";
            label_State.BackColor = Color.LightBlue;
            Update();
        }

        private void UserLabel_SizeChanged(object sender, System.EventArgs e)
        {
            //label_State.Location = new Point(label_Name.Location.X + label_Name.Width + 2, label_Name.Location.Y);
            label_State.Height = this.Height;
            label_Name.Height = this.Height;
        }
    }
}
