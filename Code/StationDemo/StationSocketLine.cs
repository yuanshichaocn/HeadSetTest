using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTools;
using BaseDll;
using MotionIoLib;
using UserData;
using System.Threading;
using System.Diagnostics;

namespace StationDemo
{
    public class StationSocketLine : Stationbase
    {
        const int dFineDistance = 10;//10个plus
        const int nSeparateCount = 10;
        const int nResolutionZ = 10000;
        const int nResolutionY = 10000;
        public StationSocketLine(Stationbase stationbase) : base(stationbase)
        {
            m_listIoInput.Add("X轴气缸1原位");
            m_listIoInput.Add("X轴气缸2原位");
            m_listIoInput.Add("X轴气缸1到位");
            m_listIoInput.Add("X轴气缸2到位");
            m_listIoInput.Add("Y轴气缸1原位");
            m_listIoInput.Add("Y轴气缸2原位");
            m_listIoInput.Add("Y轴气缸1到位");
            m_listIoInput.Add("Y轴气缸2到位");

            m_listIoInput.Add("上料位开SOCKET气缸原位");
            m_listIoInput.Add("上料位开SOCKET气缸到位");

            m_listIoInput.Add("下料位开SOCKET气缸原位");
            m_listIoInput.Add("下料位开SOCKET气缸到位");

            m_listIoInput.Add("保压上下气缸原位");
            m_listIoInput.Add("保压上下气缸到位");
            m_listIoInput.Add("上料位电机升降气缸原位");
            m_listIoInput.Add("上料位电机升降气缸到位");

            m_listIoInput.Add("上料吸嘴气缸原位");
            m_listIoInput.Add("上料吸嘴气缸到位");

            m_listIoInput.Add("下料位吸头气缸原位");
            m_listIoInput.Add("下料位吸头气缸到位");

          
            m_listIoOutput.Add("X轴气缸电磁阀");
            m_listIoOutput.Add("Y轴气缸电磁阀");
            m_listIoOutput.Add("上料开SOCKET电磁阀");
            m_listIoOutput.Add("下料位开SOCKET气缸电磁阀");
            

            m_listIoOutput.Add("上料夹紧SOCKET气缸电磁阀");
            m_listIoOutput.Add("保压气缸电磁阀");
            m_listIoOutput.Add("上料定位电机升降气缸电磁阀");
            m_listIoOutput.Add("上料吸头气缸伸出电磁阀");
            m_listIoOutput.Add("上料吸头气缸退回电磁阀");
            m_listIoOutput.Add("下料位吸头气缸伸出电磁阀");
            m_listIoOutput.Add("下料位吸头气缸退回电磁阀");


            m_listIoOutput.Add("上料仓正转（上）");
            m_listIoOutput.Add("上料仓反转（下）");

            m_listIoOutput.Add("下料仓正转（上）");
            m_listIoOutput.Add("下料仓反转（下）");

        }
        public enum StationStep
        {
            step_init,
            step_CheckAllFinish,
        }

        void DoSomethingWhenalarm()
        {

        }
        protected override bool InitStation()
        {
            ParamSetMgr.GetInstance().SetBoolParam("Socket流水线初始化完成", false);
            SocketMgr.GetInstance().ResetAllSocket();
            AlarmMgr.GetIntance().DoWhenAlarmEvent += DoSomethingWhenalarm;
            WaranResult waranResult;

            Info("Y轴气缸伸出");
           check_cyliderYout:
            IOMgr.GetInstace().WriteIoBit("Y轴气缸电磁阀", true);
            waranResult = CheckIobyName("Y轴气缸1到位", true, "Socket流水站：流水线自检  Y轴气缸移动伸出， 没到位可能卡住，，请拿开");
            if (waranResult == WaranResult.Retry)
                goto check_cyliderYout;
            waranResult = CheckIobyName("Y轴气缸2到位", true, "Socket流水站：流水线自检  Y轴气缸移动伸出， 没到位可能卡住，，请拿开");
            if (waranResult == WaranResult.Retry)
                goto check_cyliderYout;

            Info("Y轴气缸退回");
            check_cyliderYback:
            IOMgr.GetInstace().WriteIoBit("Y轴气缸电磁阀", false);

            waranResult = CheckIobyName("Y轴气缸1原位", true, "Socket流水站：流水线自检 Y轴气缸移动退回， 没到位可能卡住，，请拿开");
            if (waranResult == WaranResult.Retry)
                goto check_cyliderYback;
            waranResult = CheckIobyName("Y轴气缸2原位", true, "Socket流水站：流水线自检 Y轴气缸移动退回， 没到位可能卡住，，请拿开");
            if (waranResult == WaranResult.Retry)
                goto check_cyliderYback;
            Info("X轴气缸伸出");
            check_cyliderXout:
            IOMgr.GetInstace().WriteIoBit("X轴气缸电磁阀", true);
            waranResult = CheckIobyName("X轴气缸1到位", true, "Socket流水站： 流水线自检 X轴气缸伸出， 没到位可能卡住，，请拿开");
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXout;
            waranResult = CheckIobyName("X轴气缸2到位", true, "Socket流水站： 流水线自检 X轴气缸伸出， 没到位可能卡住，，请拿开");
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXout;

             Info("X轴气缸退回");
            check_cyliderXback:
            IOMgr.GetInstace().WriteIoBit("X轴气缸电磁阀", false);
            waranResult = CheckIobyName("Y轴气缸1原位", true, "Socket流水站： 流水线自检 X轴气缸退回， 没到位可能卡住，，请拿开");
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXback;
            waranResult = CheckIobyName("X轴气缸2原位", true, "Socket流水站：流水线自检 X轴气缸退回， 没到位可能卡住，，请拿开");
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXback;

        

            PushMultStep((int)StationStep.step_init);
            ParamSetMgr.GetInstance().SetBoolParam("Socket流水线初始化完成", true);
            return true;
        }
 
        public WaranResult CheckIobyName(string ioName, bool val = true, string excptionmsg = "", bool bmanual = false, int nTimeout = 3000)
        {
            DoWhile doWhile = new DoWhile((time, dowhile, bmanual2, obj) =>
            {
               
                if (IOMgr.GetInstace().ReadIoInBit(ioName) == val)
                {
                    return WaranResult.Run;
                }
                else if (time > nTimeout)
                {
                    WaranResult waranResult = AlarmMgr.GetIntance().WarnWithDlg(string.Format("《{0}》 信号异常:{1} ", ioName, excptionmsg), bmanual2?null: this, CommonDlg.DlgWaranType.WaranInorge_Stop_Pause_Retry, dowhile);
                    return waranResult;
                }
                else
                    return WaranResult.CheckAgain;

            }, 100000);
            return doWhile.doSomething(this, doWhile, bmanual, null);
        }
      
        public void MoveSocketLine( bool bmanual=false)
        {
            WaranResult waranResult;
            
            Info("各站都已经完成，开始移动");
            Info("XY轴气缸移动前 Socket 状态：");
            PrintSocketState();

            Info("X轴气缸退回");
           check_cyliderXback:
            IOMgr.GetInstace().WriteIoBit("X轴气缸电磁阀", false);
            waranResult = CheckIobyName("X轴气缸1原位", true, "Socket流水站： X轴气缸退回， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXback;
            waranResult = CheckIobyName("X轴气缸2原位", true, "Socket流水站： X轴气缸退回， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXback;
            Info("Y轴气缸伸出");
            check_cyliderYout:
            IOMgr.GetInstace().WriteIoBit("Y轴气缸电磁阀", true);
            waranResult = CheckIobyName("Y轴气缸1到位", true, "Socket流水站： Y轴气缸移动伸出， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderYout;
            waranResult = CheckIobyName("Y轴气缸2到位", true, "Socket流水站： Y轴气缸移动伸出， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderYout;

            Info("Y轴气缸退回");
            check_cyliderYback:
            IOMgr.GetInstace().WriteIoBit("Y轴气缸电磁阀", false);

            waranResult = CheckIobyName("Y轴气缸1原位", true, "Socket流水站： Y轴气缸移动退回， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderYback;
            waranResult = CheckIobyName("Y轴气缸2原位", true, "Socket流水站： Y轴气缸移动退回， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderYback;

            Info("X轴气缸伸出");
            check_cyliderXout:
            IOMgr.GetInstace().WriteIoBit("X轴气缸电磁阀", true);
            waranResult = CheckIobyName("X轴气缸1到位", true, "Socket流水站： X轴气缸伸出， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXout;
            waranResult = CheckIobyName("X轴气缸2到位", true, "Socket流水站： X轴气缸伸出， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXout;
            SocketMgr.GetInstance().MoveNext();
            Info("X轴气缸退回");
            check_cyliderXback2:
            IOMgr.GetInstace().WriteIoBit("X轴气缸电磁阀", false);
            waranResult = CheckIobyName("X轴气缸1原位", true, "Socket流水站： X轴气缸退回， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXback2;
            waranResult = CheckIobyName("X轴气缸2原位", true, "Socket流水站： X轴气缸退回， 没到位可能卡住，，请拿开", bmanual);
            if (waranResult == WaranResult.Retry)
                goto check_cyliderXback2;


            Info("XY轴气缸移动后 Socket 状态：");
            PrintSocketState();

        }
        public void PrintSocketState()
        {
            for (int i = 0; i < SocketMgr.GetInstance().socketArr.Length; i++)
                Info(i.ToString() + ":" + SocketMgr.GetInstance().socketArr[i].socketState.ToString());
        }
       public bool CheckLineIO()
        {
           bool b= true;
           b = b & IOMgr.GetInstace().ReadIoInBit("上料位开SOCKET气缸原位");
           b = b & IOMgr.GetInstace().ReadIoInBit("上料位夹紧SOCKET气缸原位");
           b = b & IOMgr.GetInstace().ReadIoInBit("上料位电机升降气缸原位");
           b = b & IOMgr.GetInstace().ReadIoInBit("下料位开SOCKET气缸原位");
           b = b & IOMgr.GetInstace().ReadIoInBit("保压上下气缸原位");
            b = b & IOMgr.GetInstace().ReadIoInBit("左蜂鸣器Z轴气缸1原位");
            b = b & IOMgr.GetInstace().ReadIoInBit("左蜂鸣器Z轴气缸2原位");
            b = b & IOMgr.GetInstace().ReadIoInBit("左蜂鸣器Z轴气缸3原位");
            b = b & IOMgr.GetInstace().ReadIoInBit("左蜂鸣器Z轴气缸4原位");
            b = b & IOMgr.GetInstace().ReadIoInBit("右蜂鸣器Z轴气缸1原位");
            b = b & IOMgr.GetInstace().ReadIoInBit("右蜂鸣器Z轴气缸2原位");
            b = b & IOMgr.GetInstace().ReadIoInBit("右蜂鸣器Z轴气缸3原位");
            b = b & IOMgr.GetInstace().ReadIoInBit("右蜂鸣器Z轴气缸4原位");
            return b;
        }
        int[] pressvals = new int[8];
        static  int[] press1 = new int[4];
        static int[] press2 = new int[4];
        DoWhile dowhileCheckA = new DoWhile(
            (time,dowhile2,bmanual,obj)=>
            {

                press1 = Weighing.GetInstance().ReadAData();
                if (press1 != null && press1.Length == 4)
                    return WaranResult.Run;
                else if (time > 2000)
                    return WaranResult.TimeOut;
                else
                    return WaranResult.CheckAgain;
            } );

        DoWhile dowhileCheckB = new DoWhile(
         (time, dowhile2, bmanual, obj) =>
         {

        press2 = Weighing.GetInstance().ReadBData();
        if (press2 != null && press2.Length == 4)
            return WaranResult.Run;
        else if (time > 2000)
            return WaranResult.TimeOut;
        else
            return WaranResult.CheckAgain;
         });

        public void KeepPressure(bool  bmanual=false)
        {
            WaranResult waranResult;
            if(SocketMgr.GetInstance().socketArr[(int)SocketType.press].socketState == SocketState.HaveOK || bmanual)
            {
                retry_press_up2:
                IOMgr.GetInstace().WriteIoBit("保压气缸电磁阀", false);
                waranResult = CheckIobyName("保压上下气缸原位", true, "流水线站:保压上下气缸原位 没有到位，请检查感应器，气缸，气压，线路等", bmanual);
                if (waranResult == WaranResult.Retry)
                    goto retry_press_up2;
                retry_messureA:
                waranResult= dowhileCheckA.doSomething(this, dowhileCheckA, bmanual, null);
                if( waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("保压时 ，A模块读不出数据 ", this, CommonDlg.DlgWaranType.Waran_Stop_Retry);
                    if (waranResult == WaranResult.Retry)
                        goto retry_messureA;
                }
                retry_messureB:
                waranResult = dowhileCheckB.doSomething(this, dowhileCheckA, bmanual, null);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("保压时 ，B模块读不出数据 ", this, CommonDlg.DlgWaranType.Waran_Stop_Retry);
                    if (waranResult == WaranResult.Retry)
                        goto retry_messureB;
                }

                for ( int  i=0; i< 4;i++)
                {
                    pressvals[i] = press1[i];
                    pressvals[4+i] = press2[i];
                }

                retry_press_down:
                IOMgr.GetInstace().WriteIoBit("保压气缸电磁阀", true);
                waranResult=CheckIobyName("保压上下气缸到位", true, "流水线站:保压上下气缸到位 没有到位，请检查感应器，气缸，气压，线路等", bmanual);
                if (waranResult == WaranResult.Retry)
                    goto retry_press_down;
               
                Thread.Sleep(ParamSetMgr.GetInstance().GetIntParam("保压时间"));

                retry_messureA2:
                waranResult = dowhileCheckA.doSomething(this, dowhileCheckA, bmanual, null);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("保压时 ，A模块读不出数据 ", this, CommonDlg.DlgWaranType.Waran_Stop_Retry);
                    if (waranResult == WaranResult.Retry)
                        goto retry_messureA2;
                }
                retry_messureB2:
                waranResult = dowhileCheckB.doSomething(this, dowhileCheckA, bmanual, null);
                if (waranResult != WaranResult.Run)
                {
                    waranResult = AlarmMgr.GetIntance().WarnWithDlg("保压时 ，B模块读不出数据 ", this, CommonDlg.DlgWaranType.Waran_Stop_Retry);
                    if (waranResult == WaranResult.Retry)
                        goto retry_messureB2;
                }
                for (int i = 0; i < 4; i++)
                {

                    pressvals[i] = Math.Abs(pressvals[i]- press1[i]);
                    pressvals[4 + i] = Math.Abs(pressvals[i+4] - press2[i]);
                }
                Thread.Sleep(500);
                Weighing.GetInstance().Update(new int[] { pressvals[0], pressvals[1], pressvals[2], pressvals[3] }, new int[] { pressvals[4], pressvals[5], pressvals[6], pressvals[7] });
                retry_press_up:
                IOMgr.GetInstace().WriteIoBit("保压气缸电磁阀", false );
                waranResult = CheckIobyName("保压上下气缸原位", true, "流水线站:保压上下气缸原位 没有到位，请检查感应器，气缸，气压，线路等", bmanual);
                if (waranResult == WaranResult.Retry)
                    goto retry_press_up;

            }
        }
        cUserTimer userTimerCT = new cUserTimer(long.MaxValue);
        long oldTime = 0;
        long CTmsCounts = 0;
        long CurrentTime = 0;


        protected override void StationWork(int step)
        {
          
            WaranResult waranResult;
            switch (step)
            {
                case (int)StationStep.step_init:

                    //retry_init:
                    //IOMgr.GetInstace().WriteIoBit("上料真空吸电磁阀", true);
                    //waranResult = CheckIobyName("上料吸嘴真空检测", false, "上料站： 上料吸嘴可能被堵住，请拿开");
                    //if (waranResult == WaranResult.Retry)
                    //    goto retry_init;
                    PushMultStep((int)StationStep.step_CheckAllFinish);
                    DelCurrentStep();
                    userTimerCT.ResetStartTimer();
                    oldTime = 0;
                    break;
                case (int)StationStep.step_CheckAllFinish:
                    if(    SocketMgr.GetInstance().socketArr[(int)SocketType.load].socketState == SocketState.Have 
                       && (SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.None || SocketMgr.GetInstance().socketArr[(int)SocketType.stick1].socketState == SocketState.HaveHaftOK)
                       && (SocketMgr.GetInstance().socketArr[(int)SocketType.stick2].socketState == SocketState.None || SocketMgr.GetInstance().socketArr[(int)SocketType.stick2].socketState == SocketState.HaveOK)
                       && (SocketMgr.GetInstance().socketArr[(int)SocketType.press].socketState == SocketState.None || SocketMgr.GetInstance().socketArr[(int)SocketType.press].socketState == SocketState.HaveOK)
                       && (SocketMgr.GetInstance().socketArr[(int)SocketType.unload].socketState == SocketState.None 
                        )
                       )
                    {
                        if(CheckLineIO())
                        {
                            Info("Socket流水线：各站准备完成开始流动");
                            MoveSocketLine();
                            if(ParamSetMgr.GetInstance().GetIntParam("保压屏蔽")!= 1)
                            {
                                KeepPressure();
                            }
                            CurrentTime = userTimerCT.NowTime;
                            CTmsCounts = CurrentTime - oldTime;
                            oldTime = CurrentTime;
                            ParamSetMgr.GetInstance().SetDoubleParam("CT", CTmsCounts / 1000.00);
                        }
                        
                    }
                    break;
            
            }
         


        }




    }
}
