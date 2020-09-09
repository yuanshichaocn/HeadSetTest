using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate
{
    /// <summary>
    /// 通讯事件基类
    /// </summary>
    public class Notify
    {
        /// <summary>
        /// 发送委托函数
        /// </summary>
        /// <param name="strLog"></param>
        public delegate void SendData(string strLog);
        /// <summary>
        /// 发送事件
        /// </summary>
        public event SendData DoSend;
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="strLog"></param>
        public void NotifyData(string strLog)
        {
       //     if (DoSend != null)
            {
                DoSend(strLog);
            }
        }
        /// <summary>
        /// 返回类引用
        /// </summary>
        /// <returns></returns>
        public Notify GetObject()
        {
            return this;
        }
    }
}
