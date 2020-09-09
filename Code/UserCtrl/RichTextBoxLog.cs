
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserCtrl
{
    public partial class RichTxtBoxLog : RichTextBox
    {


        ConcurrentQueue<string> DataQueue = new ConcurrentQueue<string>();
        object lockobj = new object();
        const int m_nNum = 200;
        public RichTxtBoxLog()
        {



        }
        public void AppendTextColorful(string text, Color color, bool addNewLine = true)
        {
            string[] str;
            if (addNewLine)
                text += Environment.NewLine;
            SelectionStart = TextLength;
            SelectionLength = 0;
            SelectionColor = color;
            AppendText(text);
            SelectionColor = ForeColor;

        }

        public void AddMsg(string strMsg)
        {
            DataQueue.Enqueue(strMsg);
            if (DataQueue.Count > 0)
            {
                string strMsgFromQueue = "";
                DataQueue.TryDequeue(out strMsgFromQueue);
                lock (lockobj)
                {
                    if (strMsgFromQueue != null && strMsgFromQueue != "")
                    {
                        if (strMsgFromQueue.IndexOf("Info-") != -1)
                        {
                            strMsg = strMsgFromQueue.Replace("Info-", "");
                            AppendTextColorful(strMsg, Color.Black);

                        }
                        if (strMsgFromQueue.IndexOf("Warn-") != -1)
                        {
                            strMsg = strMsgFromQueue.Replace("Warn-", "");
                            AppendTextColorful(strMsg, Color.Blue);

                        }
                        if (strMsgFromQueue.IndexOf("Err-") != -1)
                        {
                            strMsg = strMsgFromQueue.Replace("Err-", "");
                            AppendTextColorful(strMsg, Color.Blue);

                        }

                    }
                }

            }
        }



    }
}