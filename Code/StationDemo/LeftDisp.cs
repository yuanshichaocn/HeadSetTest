using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using CameraLib;
//using HalconLib;
using MotionIoLib;
using System.IO;
using CommonTools;
namespace StationDemo
{
    public class StationLeftDisp : CommonTools.Stationbase
    {
        public static object objlock = new object();
        public static int m_nPosNum = 6;
        public StationLeftDisp(string strStationName, int[] arrAxis, string[] axisname, params string[] CameraName)
            : base(strStationName, arrAxis, axisname, CameraName)
        {

        }
        public StationLeftDisp(CommonTools.Stationbase pb) : base(pb)
        {
            m_listIoInput.Add("光管升降左气缸上升到位");
            m_listIoInput.Add("光管升降左气缸下降到位");
            m_listIoInput.Add("光管升降右气缸上升到位");
            m_listIoInput.Add("光管升降右气缸下降到位");
            m_listIoInput.Add("UV固化灯上升到位");
            m_listIoInput.Add("UV固化灯下降到位");
            m_listIoInput.Add("胶水液位检测");

            m_listIoOutput.Add("点胶Z轴伺服刹车");
            m_listIoOutput.Add("左点胶阀打开");
            m_listIoOutput.Add("CCD环形光源");
            m_listIoOutput.Add("UV固化灯气缸升降");
            m_listIoOutput.Add("UV灯打开");
            m_listIoOutput.Add("左lens升降气缸");
            MotionMgr.GetInstace().AddAxisToGroup("LeftDisp", new int[] { AxisX,AxisY});
        }
        enum StationStep
        {
            StepInit = 100,//回原点
            [Description("XY回上料预备位置")]
            Step_GoXYReadyLoadPos,//XY回上料预备位置
            [Description("判断是否可以上料")]
            Step_JudgeCanLoad , //判断是否可以上料
            [Description("回上料位置")]
            Step_GoXYFeedPos,//XY 回上料位置
            [Description("等待6轴平台准备完成")]
            Step_Wait6AxisPlaformReady,//XY 等待6轴平台准备完成
            [Description("屏蔽安装光栅")]
            Step_ShieldSafetyGrating,
            [Description("开启安装光栅")]
            Step_OpenSafetyGrating,
            [Description("等待上料完成")]
            Step_WaitFeedFinish,//等待上料完成
            [Description("检查Hoder")]
            Step_CheckHoder,
            [Description("Z轴拍照准备位置")]
            Step_GoZSanpReadyPos,//Z轴拍照准备位置
            [Description("XY去拍照位置")]
            Step_GoSanpPos,//去拍照位置
            [Description("Z去拍照位置")]
            Step_GoZSanpPos,
            [Description("拍照")]
            Step_Sanp,//拍照
            [Description("拍照失败")]
            Step_SanpFailture,
            [Description("去清洗Hoder位置")]
            Step_GoHoderPlasumaPos,//去清洗Hoder位置
            [Description("Hoder清洗")]
            Step_HoderPlasuma,//Hoder清洗
            [Description("去清洗Lens位置")]
            Step_GoLensPlasumaPos,//去清洗Hoder位置
            [Description("Lens清洗")]
            Step_LensPlasuma,//Lens清洗
            [Description("Lens Hoder清洗完成")]
            Step_PlasumaFinish,//LensHoder清洗完成
            [Description("关闭Plasuma")]
            Step_ClosPlasuma,//关闭Plasuma
            [Description("去点胶高度探测位置")]
            Step_GoXYDispHighTestPos,//去点胶高度探测位置
            [Description("点胶高度探测")]
            Step_DispHighTest,//点胶高度探测
            [Description("记录点胶探测高度")]
            Step_RecordDispHigh,//记录点胶探测高度
            [Description("点胶Z轴抬起")]
            Step_ZAxisUp,//记录点胶探测高度

            [Description("Hoder 点胶")]
            Step_DispHoder,//Hoder 点胶
            [Description("Z轴回预备位置")]
            Step_GoZReadyPos,
            [Description("Z轴达到高度")]
            Step_ZarriveHeight,
            [Description("通知AA工站点胶清洗完成")]
            Step_NotifyAADispAndClearFinish,
           
            [Description("去预调焦点位置")]
            Step_GoPreFocusPos,

            [Description("等待AA工站装料完成")]
            Step_Waitloaded,
          
            [Description("通知AA预调焦")]
            Step_NotifyPreFocusing,

            [Description("预调焦完成")]
            Step_WaitPreFocusingFinish,

            [Description("判断下一步")]
            Step_JudgeNextStep,
            [Description("去预备位置")]
            Step_GoAAReadyPos,

            [Description("去AA位置")]
            Step_GoAAPos,

            [Description("平行光管下降")]
            Step_CollimatorDown,
            [Description("平行光管下降到位")]
            Step_CollimatorDownOn,
            [Description("平行光管上升")]
            Step_CollimatorUp,
            [Description("平行光管上升到位")]
            Step_CollimatorUpOn,

            [Description("AA 点亮光源")]
            Step_Light,

        
            [Description("通知AA工站AA")]
            Step_NotifyAA,
            [Description("AA 完成")]
            Step_WaitAAFinish,

           
            [Description("UV预备位置")]
            Step_GoUVReadyPos,
            [Description("UV灯上升")]
            Step_UVUp,
            [Description("UV灯上升到位")]
            Step_UVUpOn,
            [Description("UV灯下降")]
            Step_UVDown,
            [Description("UV灯下降到位")]
            Step_UVDownOn,
            [Description("UV固化位置")]
            Step_GoUVPos,
            [Description("UV灯固化")]
            Step_UVLight,
           
            [Description("通知Lens夹紧气缸张开")]
            Step_NotifyClampOpen,
            [Description("通知Lens夹紧气缸张开完成")]
            Step_WaitClampOpenFinish,
            [Description("通知AA-X回退到准备位")]
            Step_NotifyGoXAAReadyPos,
            [Description("等待AA-X回退到准备位")]
            Step_WaitInXAAReadyPos,
            [Description("lens升降气缸上升")]
            Step_LensLiftCylinderUp,
            [Description("X回预备收料位置")]
            Step_GoXPreFeedPos,
            //[Description("XY回收料位置")]
            //Step_GoXYFeedPos,




            [Description("出料")]
            Step_Out,
            [Description("清理")]
            Step_ClearZero,

            StepEnd,
        }
        protected override bool InitStation()
        {
            sys.GetInstance().SetIntParam("LeftPos", 0);
            PushStep((int)StationStep.StepInit);
            return true;
        }
        int nAACount = 0;
        int nSanpCount = 0;
        bool bSingalStart = false;
        int ZDispAxisPos = 0;
        protected override void StationWork(int step)
        {
            switch (m_iCurrentStep)
            {
                case (int)StationStep.StepInit:
                    Info(string.Format("{0}工站回原点",m_strStationName));
                 
                    PushMultStep((int)StationDefaultStep.HomeZ, (int)StationDefaultStep.WaitHomeZ,
                        (int)StationDefaultStep.HomeX, (int)StationDefaultStep.HomeY, (int)StationDefaultStep.HomeU,
                        (int)StationDefaultStep.WaitHomeX, (int)StationDefaultStep.WaitHomeY, (int)StationDefaultStep.WaitHomeU);
                    PushStep((int)StationStep.Step_GoXYReadyLoadPos);
                    DelCurrentStep();
                    sys.GetInstance().SetIntParam("LeftPos", 0);
                    break;
                case (int)StationStep.Step_GoXYReadyLoadPos:
                    Info(sys.GetEnumDescription(StationStep.Step_GoXYReadyLoadPos));
                    
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["准备上料位"].pointX,0);
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["准备上料位"].pointY, 0);
                    PushMultStep((int)StationDefaultStep.WaitX, (int)(int)StationDefaultStep.WaitY,
                       (int)StationStep.Step_JudgeCanLoad);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_JudgeCanLoad:
                    Info(("判断是否上料"));
                    lock(objlock)
                    {
                        int nLeftPos = sys.GetInstance().GetIntParam("LeftPos");
                        int nRightPos = sys.GetInstance().GetIntParam("RightPos");
                        if(nRightPos == 0 || (nLeftPos +1)% m_nPosNum != (nRightPos % m_nPosNum) )
                        {
                            nLeftPos = (nLeftPos + 1)% m_nPosNum;
                            sys.GetInstance().SetIntParam("LeftPos", nLeftPos);
                            PushMultStep(
                                (int)StationStep.Step_GoZSanpReadyPos,
                                (int)StationStep.Step_GoXYFeedPos,
                                (int)StationDefaultStep.WaitX,
                                (int)StationDefaultStep.WaitY,
                                (int)StationStep.Step_Wait6AxisPlaformReady,
                                (int)StationStep.Step_ShieldSafetyGrating,
                                (int)StationStep.Step_WaitFeedFinish
                                );
                            DelCurrentStep();
                        }  
                    }
                      break;
                case (int)StationStep.Step_GoZSanpReadyPos:
                    Info("去拍照预备位");
                    AbsMoveSingleAxis(AxisZ, GetStationPointDic()["上料位"].pointZ, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoXYFeedPos:
                    Info("去上料位");
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["上料位"].pointX, 0);
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["上料位"].pointY, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_Wait6AxisPlaformReady:
                    Info("等待6轴平台准备完成");
                    if (sys.GetInstance().GetBoolParam("左6轴平台准备完成"))
                    {
                        sys.GetInstance().SetBoolParam("左6轴平台准备完成", false);
                        Info("左6轴平台准备好，可以人工进料");
                        DelCurrentStep();
                    }
                    break;
                case (int)StationStep.Step_ShieldSafetyGrating:
                    Info("屏蔽安全光栅");
                    sys.GetInstance().SetBoolParam("启动安全光栅",false);
                    bSingalStart = IOMgr.GetInstace().ReadIoInBit("左启动按钮") & IOMgr.GetInstace().ReadIoInBit("右启动按钮");
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_OpenSafetyGrating:
                    sys.GetInstance().SetBoolParam("启动安全光栅", true);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_WaitFeedFinish:
                    Info("等待上料完成");
                    {
                        bool bSingnalStartNow = IOMgr.GetInstace().ReadIoInBit("左启动按钮") & IOMgr.GetInstace().ReadIoInBit("右启动按钮");
                        if (bSingalStart != bSingnalStartNow && bSingnalStartNow)
                        {
                            PushMultStep(
                               (int)StationStep.Step_CheckHoder);
                            DelCurrentStep();
                        }
                    }
                    break;
                case (int)StationStep.Step_CheckHoder:
                    Info("检查Hoder");
                   if( WaitSingleIO("左Socket工件检测", IOType.Type_Input, true))
                    {
                        PushMultStep(
                                (int)StationStep.Step_GoSanpPos,
                                (int)StationDefaultStep.WaitX,
                                (int)StationDefaultStep.WaitY,
                                (int)StationStep.Step_GoZSanpPos,
                                (int)StationDefaultStep.WaitZ,
                                (int)StationStep.Step_Sanp
                               );
                        DelCurrentStep();
                    }
                    break;
                case (int)StationStep.Step_GoSanpPos:
                    Info("XY去拍照位置");
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["拍照位"].pointX, 0);
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["拍照位"].pointY, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoZSanpPos:
                    Info("Z去拍照位置");
                    AbsMoveSingleAxis(AxisZ, GetStationPointDic()["拍照位"].pointZ, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_Sanp:
                    Info("拍照处理");
                    nSanpCount++;
                    //CameraMgr.GetInstance().
                    if (true)//
                    {
                        if(true)
                        {
                            //处理成功
                            PushMultStep(
                           (int)StationStep.Step_GoHoderPlasumaPos,
                           (int)StationStep.Step_HoderPlasuma,
                           (int)StationStep.Step_GoLensPlasumaPos,
                           (int)StationStep.Step_LensPlasuma,
                           (int)StationStep.Step_PlasumaFinish
                           );
                        }
                        else
                            PushMultStep((int)StationStep.Step_SanpFailture);
                    }
                    else
                    {
                        PushMultStep((int)StationStep.Step_SanpFailture);
                    }
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_SanpFailture:
                    if(nSanpCount< sys.GetInstance().GetIntParam("拍照失败次数"))
                    {
                        ClearAllStep();
                        PushMultStep((int)StationStep.Step_Sanp);
                        DelCurrentStep();
                    }
                    else
                    {
                        Info("拍照失败出料");
                        PushMultStep((int)StationStep.Step_Out,
                               
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY,
                           (int)StationStep.Step_ClearZero,
                           (int)StationStep.Step_GoXPreFeedPos,
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY);
                        DelCurrentStep();
                    }
                    break;
                case (int)StationStep.Step_GoHoderPlasumaPos:
                    Info("去Hoder清洗位置");
                   // AbsMoveSingleAxis(AxisX, GetStationPointDic()["清洗Hoder"].pointU, 2);
                   // MotionMgr.GetInstace().AddBufMove("LeftDisp", BufMotionType.buf_Line2dAbs, 0, 2, 8000, 2000, new double[] { }, null);
                   //  MotionMgr.GetInstace().AddBufIo("LeftDisp", "");
                   //   MotionMgr.GetInstace().AddBufMove("LeftDisp", BufMotionType.buf_Arc2dAbsCWAngle, 0, 2, 8000, 2000, new double[] { }, null);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_HoderPlasuma:
                    Info("Plasua清洗Hoder");
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoLensPlasumaPos:
                    Info("去Lens清洗位置");
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_LensPlasuma:
                    Info("Plasua清洗Lens");
                    MotionMgr.GetInstace().BufhMove("LeftDisp");
                    DelCurrentStep();
                    break;

                case (int)StationStep.Step_PlasumaFinish:
                    Info("Plasua清洗完成");
                    PushMultStep(
                         (int)StationDefaultStep.WaitX,
                         (int)StationDefaultStep.WaitY,
                         (int)StationDefaultStep.WaitZ,
                        (int)StationStep.Step_GoXYDispHighTestPos,
                        (int)StationDefaultStep.WaitX,
                        (int)StationDefaultStep.WaitY,
                        (int)StationStep.Step_DispHighTest,
                        (int)StationDefaultStep.WaitZ,
                        (int)StationStep.Step_RecordDispHigh,
                        (int)StationStep.Step_ZarriveHeight,
                        (int)StationDefaultStep.WaitZ,
                         (int)StationStep.Step_DispHoder,
                         (int)StationDefaultStep.WaitX,
                        (int)StationDefaultStep.WaitY,
                         (int)StationStep.Step_ZAxisUp,
                         (int)StationDefaultStep.WaitZ,
                         (int)StationStep.Step_NotifyAADispAndClearFinish

                         );
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoXYDispHighTestPos:
                    Info("去高度探测起点");
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["点胶下探位"].pointX, 0);
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["点胶下探位"].pointY, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_DispHighTest:
                    Info("高度探测");
                    IOMgr.GetInstace().InStopEnable("点胶针头下探检测");
                    AbsMoveSingleAxis(AxisZ, GetStationPointDic()["点胶下探位"].pointZ, 1);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_RecordDispHigh:
                    Info("点胶Z轴抬起");
                    ZDispAxisPos= MotionMgr.GetInstace().GetAxisPos(AxisZ);
                    IOMgr.GetInstace().InStopDisenable("点胶针头下探检测");
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_ZAxisUp:
                    Info("点胶Z轴抬起");
                    AbsMoveSingleAxis(AxisZ, ZDispAxisPos+900, 2);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_DispHoder:
                    Info("Hoder点胶");
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_ZarriveHeight:
                    Info("点胶Z达到高度");
                    Info("点胶Z轴抬起");
                    AbsMoveSingleAxis(AxisZ, ZDispAxisPos + 100, 2);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_NotifyAADispAndClearFinish:
                    Info("通知AA工站清洗和点胶完毕");
                    sys.GetInstance().SetBoolParam("通知左AA台装料", true);
                    PushMultStep((int)StationStep.Step_GoPreFocusPos,
                        (int)StationStep.Step_GoZReadyPos,
                       (int)StationDefaultStep.WaitX,
                       (int)StationDefaultStep.WaitY,
                        (int)StationStep.Step_Waitloaded
                         );
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoPreFocusPos: 
                    Info("去预调焦位置");
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["预调焦位"].pointX, 0);
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["预调焦位"].pointY, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_Waitloaded:
                    Info("等待AA装料完成，Z轴上升到位");
                   if( sys.GetInstance().GetBoolParam("左AA台装料完成"))
                    {
                        DelCurrentStep();
                        PushMultStep((int)StationStep.Step_NotifyPreFocusing,
                      (int)StationStep.Step_WaitPreFocusingFinish,
                      (int)StationStep.Step_JudgeNextStep,
                      (int)StationStep.Step_GoAAReadyPos,
                      (int)StationDefaultStep.WaitX,
                      (int)StationDefaultStep.WaitY,
                      (int)StationStep.Step_JudgeNextStep,
                      (int)StationStep.Step_GoAAPos,
                     (int)StationDefaultStep.WaitX,
                     (int)StationDefaultStep.WaitY,
                     (int)StationStep.Step_CollimatorDown,
                      (int)StationStep.Step_Light,
                      (int)StationStep.Step_CollimatorDownOn,
                     (int)StationStep.Step_NotifyAA,
                     (int)StationStep.Step_WaitAAFinish
                       );
                    }
                    break;
                case (int)StationStep.Step_WaitPreFocusingFinish:
                    Info("预调焦完成");
                    if(sys.GetInstance().GetBoolParam("左预调焦成功"))
                    {
                        sys.GetInstance().SetBoolParam("左预调焦开始", false);
                        sys.GetInstance().SetBoolParam("左预调焦成功", false);
                        sys.GetInstance().SetBoolParam("左预调焦失败", false);
                        DelCurrentStep();
                      
                    }
                    else if (sys.GetInstance().GetBoolParam("左预调焦失败"))
                    {
                        sys.GetInstance().SetBoolParam("左预调焦开始", false);
                        sys.GetInstance().SetBoolParam("左预调焦成功", false);
                        sys.GetInstance().SetBoolParam("左预调焦失败", false);
                        ClearAllStep();
                        PushMultStep((int)StationStep.Step_Out,

                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY,
                           (int)StationStep.Step_ClearZero,
                           (int)StationStep.Step_GoXPreFeedPos,
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY);

                        DelCurrentStep();
                    }

                     
                    break;
                case (int)StationStep.Step_NotifyPreFocusing:
                    Info("通知AA预调焦");
                
                    sys.GetInstance().SetBoolParam("左预调焦成功", false);
                    sys.GetInstance().SetBoolParam("左预调焦失败", false);
                    sys.GetInstance().SetBoolParam("左预调焦开始", true);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_JudgeNextStep:
                    Info("判断下一步");
                    lock (objlock)
                    {

                        int nLeftPos = sys.GetInstance().GetIntParam("LeftPos");
                        int nRightPos = sys.GetInstance().GetIntParam("RightPos");
                        if (nRightPos==0 || (nLeftPos + 1) % m_nPosNum != (nRightPos % m_nPosNum))
                        {
                            nLeftPos = (nLeftPos + 1) % m_nPosNum;
                            sys.GetInstance().SetIntParam("LeftPos", nLeftPos);
                            sys.GetInstance().SetBoolParam("左Y等待中...", false);
                            DelCurrentStep();
                        }
                        else
                        {
                            sys.GetInstance().SetBoolParam("左Y等待中...", true);
                        }
                    }
                    break;
                case (int)StationStep.Step_GoAAReadyPos:
                    Info("去AA预备位置");
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["AA准备位"].pointX, 0);
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["AA准备位"].pointY, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_GoAAPos:
                    Info("去AA预备位置");
                    AbsMoveSingleAxis(AxisX, GetStationPointDic()["AA位"].pointX, 0);
                    AbsMoveSingleAxis(AxisY, GetStationPointDic()["AA位"].pointY, 0);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_CollimatorDown:
                    Info("平行光管下降");
                    IOMgr.GetInstace().WriteIoBit("光管气缸升降", true);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_CollimatorDownOn:
                    Info("平行光管下降");
                   if(  WaitSingleIO("光管升降左气缸下降到位", IOType.Type_Input) &&
                        WaitSingleIO("光管升降右气缸下降到位", IOType.Type_Input) )
                    {
                        DelCurrentStep();
                    }
                    break;
                case (int)StationStep.Step_NotifyAA:
                    Info("AA工站开始AA");
                    sys.GetInstance().SetBoolParam("左AA工站AA成功", false);
                    sys.GetInstance().SetBoolParam("左AA工站AA失败", false);
                    sys.GetInstance().SetBoolParam("左AA工站AA开始",true);
                    DelCurrentStep();
                    break;
                case (int)StationStep.Step_WaitAAFinish:
                    Info("平行光管下降");
                    if(sys.GetInstance().GetBoolParam("左AA工站AA完成"))
                    {
                        DelCurrentStep();
                        sys.GetInstance().SetBoolParam("左AA工站AA完成", false);
                        sys.GetInstance().SetBoolParam("左AA工站AA失败", false);
                        ClearAllStep();
                        PushMultStep((int)StationStep.Step_CollimatorUp,
                           (int)StationStep.Step_CollimatorUpOn,
                           (int)StationStep.Step_JudgeNextStep,
                           (int)StationStep.Step_GoUVReadyPos,
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY,
                           (int)StationStep.Step_JudgeNextStep,
                           (int)StationStep.Step_UVUp,
                           (int)StationStep.Step_UVUpOn,
                           (int)StationStep.Step_GoUVPos,
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY,
                           (int)StationStep.Step_UVDown,
                           (int)StationStep.Step_UVDownOn,
                           (int)StationStep.Step_Out,
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY,
                           (int)StationStep.Step_ClearZero,
                           (int)StationStep.Step_GoXPreFeedPos,
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY


                           );
                    }
                    else if (sys.GetInstance().GetBoolParam("左AA工站AA失败"))
                    {
                        DelCurrentStep();
                        sys.GetInstance().SetBoolParam("左AA工站AA完成", false);
                        sys.GetInstance().SetBoolParam("左AA工站AA失败", false);
                        ClearAllStep();
                        PushMultStep((int)StationStep.Step_CollimatorUp,
                            (int)StationStep.Step_CollimatorUpOn,
                                 (int)StationStep.Step_Out,
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY,
                           (int)StationStep.Step_ClearZero,
                           (int)StationStep.Step_GoXPreFeedPos,
                           (int)StationDefaultStep.WaitX,
                           (int)StationDefaultStep.WaitY
                            );
                    }
                        break;
                case (int)StationStep.Step_GoUVReadyPos:
                    Info("去UV预备位置");
                   // MoveAxisAbs()
                   // PushMultStep(())
                    break;
               // case (int)StationStep.Step_JudgeNextStep:
               //     Info("去UV预备位置");
                    break;
            }

        }
         
        }
}