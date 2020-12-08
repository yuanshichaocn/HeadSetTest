using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UserCtrl
{
    public partial class ListLog : ListBox

    {
        private ConcurrentQueue<string> DataQueue = new ConcurrentQueue<string>();
        private object lockobj = new object();
        private const int m_nNum = 200;

        public ListLog()
        {
            InitializeComponent();
            this.DrawMode = DrawMode.OwnerDrawVariable;
            this.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawItem);
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
                        Items.Add(strMsgFromQueue);

                        // SelectedIndex = Items.Count - 1;
                        while (Items.Count > m_nNum)
                            Items.RemoveAt(0);
                    }
                }
            }
        }

        public ListLog(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private int iSave = 100;
        private Brush mybsh = Brushes.Black;

        private void OnDrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e != null && e.Index >= 0)
            {
                e.DrawBackground();

                string strMsg = "";
                ListBox it = sender as ListBox;
                if (it == null)
                    return;
                while (it.Items.Count > m_nNum)
                    it.Items.RemoveAt(0);
                int Index = e.Index;
                // if(it.Items.Count>0)
                //    it.SelectedIndex = it.Items.Count - 1;
                if (it.Items != null && it.Items.Count > Index && it.Items[Index].ToString().IndexOf("Info-") != -1)
                {
                    strMsg = it.Items[Index].ToString().Replace("Info-", "") + "\r\n";
                    mybsh = Brushes.Black;
                }
                else if (it.Items != null && it.Items.Count > Index && it.Items[Index].ToString().IndexOf("Warn-") != -1)
                {
                    strMsg = it.Items[Index].ToString().Replace("Warn-", "") + "\r\n";
                    mybsh = Brushes.Blue;
                }
                else if (it.Items != null && it.Items.Count > Index && it.Items[Index].ToString().IndexOf("Err-") != -1)
                {
                    strMsg = it.Items[Index].ToString().Replace("Err-", "") + "\r\n";
                    mybsh = Brushes.Red;
                }
                else
                    return;
                // e.DrawFocusRectangle();
                e.Graphics.DrawString(strMsg, e.Font, mybsh, e.Bounds, StringFormat.GenericTypographic);
            }
        }
    }
}