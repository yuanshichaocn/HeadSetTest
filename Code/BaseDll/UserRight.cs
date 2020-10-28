using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Reflection;
namespace BaseDll
{
    public enum UserRight
    {
        User_None = -1, //未知
        客户操作员,//
        调试工程师,  //售后工程师
        软件工程师, //软件工程师
        超级管理员,    //超级管理员
    };
    public struct User
    {
        public string _userName;
        public string _userPassWord;
        public UserRight _userRight;
    }
    public interface IUserRightSwitch
    {
        UserRight userRight
        {
            set;
            get;
        }
        void ChangedUserRight(User CurrentUser);
    }
}