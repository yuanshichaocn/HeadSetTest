using BaseDll;
using CommonDlg;
using CommonTools;
using System;
using System.Windows.Forms;

namespace StationDemo
{
    public partial class UserManger : Form, IUserRightSwitch
    {
        public UserManger()
        {
            InitializeComponent();
        }

        public UserRight userRight
        {
            get;
            set;
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
                userRight = CurrentUser._userRight;
                comboBox_SelRight.Items.Clear();
                for (int i = 0; i <= (int)CurrentUser._userRight; i++)
                {
                    comboBox_SelRight.Items.Add(((UserRight)i).ToString());
                    //if (i==(int)CommonTools.UserRight.User_Costmomer)
                    //    comboBox_SelRight.Items.Add(CommonTools.UserRight.User_Costmomer.ToString());
                    //if (i == (int)CommonTools.UserRight.User_Debuger)
                    //    comboBox_SelRight.Items.Add(CommonTools.UserRight.User_Debuger.ToString());
                    //if (i == (int)CommonTools.UserRight.User_Enginerr)
                    //    comboBox_SelRight.Items.Add(CommonTools.UserRight.User_Enginerr.ToString());
                    //if (i == (int)CommonTools.UserRight.User_Admin)
                    //    comboBox_SelRight.Items.Add(CommonTools.UserRight.User_Admin.ToString());
                }
                comboBox_SelRight.SelectedIndex = 0;
                dataGridView_UserList.Rows.Clear();
                foreach (var temp in sys.g_listUser)
                {
                    if ((int)temp._userRight <= (int)userRight)
                    {
                        dataGridView_UserList.Rows.Add(temp._userName,
                          temp._userRight.ToString());
                    }
                }

                if ((int)CurrentUser._userRight < (int)UserRight.软件工程师)
                {
                    for (int i = 0; i < groupBox_SystemSet.Controls.Count; i++)
                        groupBox_SystemSet.Controls[i].Enabled = false;
                    groupBox_SystemSet.Enabled = false;
                }
                else
                {
                    for (int i = 0; i < groupBox_SystemSet.Controls.Count; i++)
                        groupBox_SystemSet.Controls[i].Enabled = true;
                    groupBox_SystemSet.Enabled = true;
                }

                //int heith = 0;
                //for (int i = 0; i < dataGridView_UserList.Rows.Count; i++)
                //    heith +=  dataGridView_UserList.Rows[i].Height;
                //heith += dataGridView_UserList.ColumnHeadersHeight;
                //dataGridView_UserList.Height = heith+30;
            }
        }

        private void button_AddUser_Click(object sender, EventArgs e)
        {
            if (textBox_UserName.Text == "" || textBox_PassWord.Text == "")
            {
                AlarmMgr.GetIntance().Warn("用户名和密码不能为空 ");
                return;
            }
            string str = ((UserRight)comboBox_SelRight.SelectedIndex).ToString();
            int userIndex = sys.g_listUser.FindIndex(t => t._userName == textBox_UserName.Text);
            if (userIndex != -1)
            {
                AlarmMgr.GetIntance().Warn("用户名已存在 ");
                return;
            }

            dataGridView_UserList.Rows.Add(textBox_UserName.Text, str);
            sys.g_listUser.Add(new User()
            {
                _userName = textBox_UserName.Text,
                _userPassWord = textBox_PassWord.Text,
                _userRight = (UserRight)comboBox_SelRight.SelectedIndex
            });
            ConfigToolMgr.GetInstance().SaveUserConfig();
        }

        private void UserManger_Load(object sender, EventArgs e)
        {
            int width = 0;
            for (int i = 0; i < dataGridView_UserList.Columns.Count; i++)
                width += dataGridView_UserList.Columns[i].Width;
            dataGridView_UserList.Width = width + 30;
            textBox_ProductDir.Text = ParamSetMgr.GetInstance().CurrentWorkDir;
            userRight = UserRight.User_None;
            sys.g_eventRightChanged += ChangedUserRight;
            sys.g_User = sys.g_User;
            checkBox_UseSafeDoor.Checked = true;
            checkBox_UseSafeGate.Checked = true;
        }

        private void button_DelUser_Click(object sender, EventArgs e)
        {
            if (dataGridView_UserList.SelectedRows.Count > 0)
            {
                /*    DataGridViewSelectedRowCollection r0 =*/
                bool bDel = false;
                string str = dataGridView_UserList.SelectedRows[0].Cells[0].Value.ToString();
                User delobj = new User();
                foreach (var temp in sys.g_listUser)
                {
                    if (temp._userName == str)
                    {
                        delobj = temp;
                        bDel = true;
                    }
                }
                DataGridViewRow row = dataGridView_UserList.SelectedRows[0];
                dataGridView_UserList.Rows.Remove(row);
                if (bDel)
                {
                    sys.g_listUser.Remove(delobj);
                    ConfigToolMgr.GetInstance().SaveUserConfig();
                }
            }
        }

        private void BtnScarnProductDir_Click(object sender, EventArgs e)
        {
            FolderDialog openFolder = new FolderDialog();
            if (openFolder.DisplayDialog() == DialogResult.OK)
            {
                textBox_ProductDir.Text = openFolder.Path.ToString();
                ParamSetMgr.GetInstance().CurrentWorkDir = textBox_ProductDir.Text;
                ConfigToolMgr.GetInstance().SaveProductDir();
            }
            else
                textBox_ProductDir.Text = "你没有选择目录";
        }

        private void checkBox_UseSafeDoor_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_UseSafeDoor.Checked)
                ParamSetMgr.GetInstance().SetBoolParam("启用安全门", true);
            else
                ParamSetMgr.GetInstance().SetBoolParam("启用安全门", false);
        }

        private void checkBox_UseSafeGate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_UseSafeGate.Checked)
                ParamSetMgr.GetInstance().SetBoolParam("启用安全光栅", true);
            else
                ParamSetMgr.GetInstance().SetBoolParam("启用安全光栅", false);
        }

        private void checkBox_ModeAirRun_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ModeAirRun.Checked)
                checkBox_ModeRun.Checked = false;
            sys.g_AppMode = checkBox_ModeAirRun.Checked ? AppMode.AirRun : AppMode.Run;
            ParamSetMgr.GetInstance().SetBoolParam("系统空跑", sys.g_AppMode == AppMode.AirRun);
        }

        private void checkBox_ModeRun_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ModeRun.Checked)
                checkBox_ModeAirRun.Checked = false;
            sys.g_AppMode = checkBox_ModeAirRun.Checked ? AppMode.AirRun : AppMode.Run;
        }

        private void check_CloseSafeProtect_CheckedChanged(object sender, EventArgs e)
        {
            if (sys.g_User._userRight <= UserRight.调试工程师)
            {
                MessageBox.Show($"当前用户:{sys.g_User._userName}  权限不够，不能屏蔽安全防护");
            }

            if (check_CloseSafeProtect.Checked)
            {
            }
            else
            {
                //if(!MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenXYMoveAss))
                //    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenXYMoveAss;
                //if (!MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenXYMoveDisp))
                //    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenXYMoveDisp;
                //if (!MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenAssXMove))
                //    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssXMove;
                //if (!MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenAssYMove))
                //    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenAssYMove;
                //if (!MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenDispXMove))
                //    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenDispXMove;
                //if (!MotionMgr.GetInstace().IsSafeFunRegister(Safe.IsSafeWhenTrayYMove))
                //    MotionMgr.GetInstace().m_eventIsSafeWhenAxisMove += Safe.IsSafeWhenTrayYMove;
            }
        }
    }
}