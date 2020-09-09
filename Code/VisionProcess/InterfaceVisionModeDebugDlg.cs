using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCtrl;

namespace VisionProcess
{
 public interface InterfaceVisionModeDebugDlg
    {
         void FlushToDlg(VisionSetpBase visionSetp, VisionControl visionControl);
         void SaveParm(VisionSetpBase visionSetp);
       

    }
}
