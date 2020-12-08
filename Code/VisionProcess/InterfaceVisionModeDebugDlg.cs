using UserCtrl;

namespace VisionProcess
{
    public interface InterfaceVisionModeDebugDlg
    {
        void FlushToDlg(VisionSetpBase visionSetp, VisionControl visionControl);

        void SaveParm(VisionSetpBase visionSetp);
    }
}