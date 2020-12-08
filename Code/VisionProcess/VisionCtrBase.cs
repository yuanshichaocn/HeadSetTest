﻿using System.Windows.Forms;
using UserCtrl;

namespace VisionProcess
{
    public partial class VisionBaseCtr : UserControl
    {
        public virtual void FlushToDlg(VisionSetpBase visionSetp, VisionControl visionControl, Control Farther = null)
        {
        }

        public virtual void SaveParm(VisionSetpBase visionSetp)
        {
        }

        protected VisionControl m_visionControl = null;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // VisionBaseCtr
            //
            this.Name = "VisionBaseCtr";
            this.Size = new System.Drawing.Size(411, 150);
            this.ResumeLayout(false);
        }
    }
}