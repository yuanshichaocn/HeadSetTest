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
using UserData;
using System.Collections.Concurrent;

namespace StationDemo
{
    public enum LineIoIn
    {
        前壳段皮带进入感应,
        前壳段皮带离开感应,
        前壳段皮带阻挡气缸上升位,
        前壳段皮带阻挡气缸下降位,
        前壳段皮带顶升气缸上升位,
        前壳段皮带顶升气缸下降位,
        前壳段皮带线到位感应,
        sensor段皮带进入感应,
        sensor段皮带离开感应,
        sensor段皮带阻挡气缸上升位,
        sensor段皮带阻挡气缸下降位,
        sensor段皮带顶升气缸上升位,
        sensor段皮带顶升气缸下降位,
        sensor段皮带线到位感应,
        sensor锁付段皮带进入感应,
        sensor锁付段皮带离开感应,
        sensor锁付段皮带阻挡气缸上升位,
        sensor锁付段皮带阻挡气缸下降位,
        sensor锁付段皮带顶升气缸上升位,
        sensor锁付段皮带顶升气缸下降位,
        sensor锁付段皮带线到位感应,
        回流1段皮带进入感应,
        回流1段皮带离开感应,
        回流2段皮带进入感应,
        回流2段皮带离开感应,
        左边升降机皮带进入感应,
        左边升降机皮带到位感应,
        前壳NG料传送按钮,
        前壳NG料放入感应,
        前壳NG料满料感应,
        sensorNG料传送按钮,
        sensorNG料放入感应,
        sensorNG料满料感应,
        半成品NG料放入感应,
        半成品NG料满料感应,
    }
    public enum LineIoOut
    {
        /// <summary>
        /// 前壳段皮带
        /// </summary>
        前壳段皮带启动,
        前壳段皮带阻挡气缸上升,
        前壳段皮带阻挡气缸下降,
        前壳段皮带顶升气缸上升,
        前壳段皮带顶升气缸下降,

        /// <summary>
        /// sensor段皮带
        /// </summary>
        sensor段皮带启动,
        sensor段皮带方向,
        sensor段皮带阻挡气缸上升,
        sensor段皮带阻挡气缸下降,
        sensor段皮带顶升气缸上升,
        sensor段皮带顶升气缸下降,

        /// <summary>
        /// 锁付段皮带
        /// </summary>
        sensor锁付段皮带启动,
        sensor锁付段皮带方向,
        sensor锁付段皮带阻挡气缸上升,
        sensor锁付段皮带阻挡气缸下降,
        sensor锁付段皮带顶升气缸上升,
        sensor锁付段皮带顶升气缸下降,

        /// <summary>
        /// 左升降机皮带
        /// </summary>
        左升降机皮带启动,
        左升降机皮带方向,
        前壳NG料传送按钮,
        sensorNG料传送按钮,
        /// <summary>
        /// 回流皮带启动
        /// </summary>
        回流1段皮带启动,
        回流2段皮带启动,
        /// <summary>
        /// NG 皮带启动
        /// </summary>
        前壳NG料皮带线启动,
        sensorNG料皮带线启动,
        半成品NG料皮带线启动,
    }


    public partial class LineStation : CommonTools.Stationbase
    {
        public LineStation(CommonTools.Stationbase pb) : base(pb)
        {


            m_listIoInput.Add(LineIoIn.sensor段皮带进入感应.ToString());
            m_listIoInput.Add(LineIoIn.sensor段皮带离开感应.ToString());
            m_listIoInput.Add(LineIoIn.sensor段皮带阻挡气缸上升位.ToString());
            m_listIoInput.Add(LineIoIn.sensor段皮带阻挡气缸下降位.ToString());
            m_listIoInput.Add(LineIoIn.sensor段皮带顶升气缸上升位.ToString());
            m_listIoInput.Add(LineIoIn.sensor段皮带顶升气缸下降位.ToString());


            m_listIoInput.Add(LineIoIn.前壳段皮带进入感应.ToString());
            m_listIoInput.Add(LineIoIn.前壳段皮带离开感应.ToString());
            m_listIoInput.Add(LineIoIn.前壳段皮带阻挡气缸上升位.ToString());
            m_listIoInput.Add(LineIoIn.前壳段皮带阻挡气缸下降位.ToString());
            m_listIoInput.Add(LineIoIn.前壳段皮带顶升气缸上升位.ToString());
            m_listIoInput.Add(LineIoIn.前壳段皮带顶升气缸下降位.ToString());

            m_listIoInput.Add(LineIoIn.sensor锁付段皮带进入感应.ToString());
            m_listIoInput.Add(LineIoIn.sensor锁付段皮带离开感应.ToString());
            m_listIoInput.Add(LineIoIn.sensor锁付段皮带阻挡气缸上升位.ToString());
            m_listIoInput.Add(LineIoIn.sensor锁付段皮带阻挡气缸下降位.ToString());
            m_listIoInput.Add(LineIoIn.sensor锁付段皮带顶升气缸上升位.ToString());
            m_listIoInput.Add(LineIoIn.sensor锁付段皮带顶升气缸下降位.ToString());

            m_listIoInput.Add(LineIoIn.前壳段皮带线到位感应.ToString());
            m_listIoInput.Add(LineIoIn.sensor段皮带线到位感应.ToString());
            m_listIoInput.Add(LineIoIn.sensor锁付段皮带线到位感应.ToString());


            m_listIoOutput.Add(LineIoOut.前壳段皮带启动.ToString());
            m_listIoOutput.Add(LineIoOut.sensor段皮带启动.ToString());
            m_listIoOutput.Add(LineIoOut.sensor锁付段皮带启动.ToString());

            m_listIoOutput.Add(LineIoOut.sensor段皮带阻挡气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.sensor段皮带阻挡气缸下降.ToString());
            m_listIoOutput.Add(LineIoOut.sensor段皮带顶升气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.sensor段皮带顶升气缸下降.ToString());




            m_listIoOutput.Add(LineIoOut.前壳段皮带阻挡气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.前壳段皮带阻挡气缸下降.ToString());
            m_listIoOutput.Add(LineIoOut.前壳段皮带顶升气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.前壳段皮带顶升气缸下降.ToString());



            m_listIoOutput.Add(LineIoOut.sensor锁付段皮带阻挡气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.sensor锁付段皮带阻挡气缸下降.ToString());
            m_listIoOutput.Add(LineIoOut.sensor锁付段皮带顶升气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.sensor锁付段皮带顶升气缸下降.ToString());


            m_listIoOutput.Add(LineIoOut.前壳NG料传送按钮.ToString());
            m_listIoOutput.Add(LineIoOut.sensorNG料传送按钮.ToString());
            m_listIoOutput.Add(LineIoOut.前壳NG料皮带线启动.ToString());
            m_listIoOutput.Add(LineIoOut.sensorNG料皮带线启动.ToString());
            m_listIoOutput.Add(LineIoOut.半成品NG料皮带线启动.ToString());

        }
        enum StationStep
        {
            StepInit = 100,
            LineRun,
        }

        LineSegmentAction lineFront = new LineSegmentAction("前壳流水线");
        LineSegmentAction lineSenor = new LineSegmentAction("Sensor流水线");
        LineSegmentUpDownUseMotor lineUpDown = new LineSegmentUpDownUseMotor("升降机");
        LineSegmentAction lineLock = new LineSegmentAction("锁付流水线");
        LineNg lineNgFront = new LineNg("前壳NG皮带");
        LineNg lineNgSensor = new LineNg("SensorNG皮带");
        LineNg lineNgHalfProduct = new LineNg("半成品NG皮带");

        LineSegmentAction LineBack1 = new LineSegmentAction("回流1皮带");
        LineSegmentAction LineBack2 = new LineSegmentAction("回流2皮带");
        LineSegementState LineStateOfBackMachine
        {
            get
            {
                if (sys.g_AppMode == BaseDll.AppMode.AirRun)
                    return LineSegementState.None;
                else
                    //通过通讯获取数据
                    return LineSegementState.None;
            }
        }
     public   ConcurrentBag<LineSegmentAction> lineSegmentActions = new ConcurrentBag<LineSegmentAction>();
        public void Config()
        {
            lineFront.LineName = "前壳";
            lineFront.ForwardMotorIo = LineIoOut.前壳段皮带启动.ToString();
            lineFront.StopCylinderUpIo = LineIoOut.前壳段皮带阻挡气缸上升.ToString();
           // lineFront.StopCylinderDownIo = LineIoOut.前壳段皮带阻挡气缸下降.ToString();
            lineFront.JackUpCylinderUpIo = LineIoOut.前壳段皮带顶升气缸上升.ToString();
           // lineFront.JackUpCylinderDownIo = LineIoOut.前壳段皮带顶升气缸下降.ToString();

            lineFront.JackUpCylinderUpIoInPos = LineIoIn.前壳段皮带顶升气缸上升位.ToString();
            lineFront.JackUpCylinderDownIoInPos = LineIoIn.前壳段皮带顶升气缸下降位.ToString();
            lineFront.StopCylinderUpInPosIo = LineIoIn.前壳段皮带阻挡气缸上升位.ToString();
            lineFront.StopCylinderDwonInPosIo = LineIoIn.前壳段皮带阻挡气缸下降位.ToString();
            lineFront.InPosCheckIo = LineIoIn.前壳段皮带线到位感应.ToString();
  
            lineSenor.LineName = "sensor";
            lineSenor.ForwardMotorIo = LineIoOut.sensor段皮带启动.ToString();
            lineSenor.StopCylinderUpIo = LineIoOut.sensor段皮带阻挡气缸上升.ToString();
            lineSenor.IOMotionDir = LineIoOut.sensor段皮带方向.ToString();
            // lineSenor.StopCylinderDownIo = LineIoOut.sensor段皮带阻挡气缸下降.ToString();
            lineSenor.JackUpCylinderUpIo = LineIoOut.sensor段皮带顶升气缸上升.ToString();
           // lineSenor.JackUpCylinderDownIo = LineIoOut.sensor段皮带顶升气缸下降.ToString();

            lineSenor.JackUpCylinderUpIoInPos = LineIoIn.sensor段皮带顶升气缸上升位.ToString();
            lineSenor.JackUpCylinderDownIoInPos = LineIoIn.sensor段皮带顶升气缸下降位.ToString();
            lineSenor.StopCylinderUpInPosIo = LineIoIn.sensor段皮带阻挡气缸上升位.ToString();
            lineSenor.StopCylinderDwonInPosIo = LineIoIn.sensor段皮带阻挡气缸下降位.ToString();
            lineSenor.InPosCheckIo = LineIoIn.sensor段皮带线到位感应.ToString();

            lineLock.LineName = "sensor锁付";
            lineLock.ForwardMotorIo = LineIoOut.sensor锁付段皮带启动.ToString();
            lineLock.IOMotionDir = LineIoOut.sensor锁付段皮带方向.ToString();
            lineLock.StopCylinderUpIo = LineIoOut.sensor锁付段皮带阻挡气缸上升.ToString();
            // lineLock.StopCylinderDownIo = LineIoOut.sensor锁付段皮带阻挡气缸下降.ToString();
            lineLock.JackUpCylinderUpIo = LineIoOut.sensor锁付段皮带顶升气缸上升.ToString();
            // lineLock.JackUpCylinderDownIo = LineIoOut.sensor锁付段皮带顶升气缸下降.ToString();

            lineLock.JackUpCylinderUpIoInPos = LineIoIn.sensor锁付段皮带顶升气缸上升位.ToString();
            lineLock.JackUpCylinderDownIoInPos = LineIoIn.sensor锁付段皮带顶升气缸下降位.ToString();
            lineLock.StopCylinderUpInPosIo = LineIoIn.sensor锁付段皮带阻挡气缸上升位.ToString();
            lineLock.StopCylinderDwonInPosIo = LineIoIn.sensor锁付段皮带阻挡气缸下降位.ToString();
            lineLock.InPosCheckIo = LineIoIn.sensor锁付段皮带线到位感应.ToString();


            lineUpDown.LineName = "左边升降机";
            lineUpDown.ForwardMotorIo = LineIoOut.左升降机皮带启动.ToString();
            lineUpDown.IOMotionDir = LineIoOut.左升降机皮带方向.ToString();
            lineUpDown.nAxisNo = 11;
            lineUpDown.EnteryCheckIo = LineIoIn.左边升降机皮带进入感应.ToString();
            lineUpDown.InPosCheckIo = LineIoIn.左边升降机皮带到位感应.ToString();
            lineUpDown.bOutMotorRunDir = false;

            lineNgFront.FullMaterialCheckIO = LineIoIn.前壳NG料满料感应.ToString();
            lineNgFront.EnteryCheckIo = LineIoIn.前壳NG料放入感应.ToString();
            lineNgFront.ForwardMotorIo = LineIoOut.前壳NG料皮带线启动.ToString();

            lineNgSensor.FullMaterialCheckIO = LineIoIn.前壳NG料满料感应.ToString();
            lineNgSensor.EnteryCheckIo = LineIoIn.前壳NG料放入感应.ToString();
            lineNgSensor.ForwardMotorIo = LineIoOut.sensorNG料皮带线启动.ToString();


            lineNgHalfProduct.EnteryCheckIo = LineIoIn.半成品NG料放入感应.ToString();
            lineNgHalfProduct.FullMaterialCheckIO = LineIoIn.半成品NG料满料感应.ToString();
            lineNgHalfProduct.ForwardMotorIo = LineIoOut.半成品NG料皮带线启动.ToString();

      
           
            LineBack1.InPosCheckIo = LineIoIn.回流1段皮带离开感应.ToString();
            LineBack1.ForwardMotorIo = LineIoOut.回流1段皮带启动.ToString();
            LineBack1.nOutTimeout = 15000;
            LineBack1.nEnteryTimeout = 10000;

            //LineBack2.EnteryCheckIo = LineIoIn.回流2段皮带进入感应.ToString();
            LineBack2.InPosCheckIo = LineIoIn.回流2段皮带离开感应.ToString();
            LineBack2.ForwardMotorIo = LineIoOut.回流2段皮带启动.ToString();
            LineBack2.nOutTimeout = 15000;
            LineBack2.nEnteryTimeout = 10000;

            lineSegmentActions.Add(lineUpDown);
            lineSegmentActions.Add(lineFront);
            lineSegmentActions.Add(lineLock);
            lineSegmentActions.Add(lineSenor);
            // lineSegmentActions.Add(lineNgFront);
            // lineSegmentActions.Add(lineNgSensor);
            // lineSegmentActions.Add(lineNgHalfProduct);
            //  lineSegmentActions.Add(LineBack1);
            //   lineSegmentActions.Add(LineBack2);
        }
        protected override bool InitStation()
        {
            MotionMgr.GetInstace().ServoOn(11);
            ResetAllLineState();
            //Config();
            PushMultStep((int)StationStep.StepInit, (int)StationStep.LineRun);
            return true;
        }

        public override void PauseDeal()
        {
            foreach (var temp in lineSegmentActions)
                temp.IsPause = true; 
        }
        public override void ResumeDeal()
        {
            foreach (var temp in lineSegmentActions)
                temp.IsPause = false;
        }

        public void ResetAllLineState()
        {
            foreach (var temp in lineSegmentActions)
                temp.LineSegState = LineSegementState.UnKnow;
        }
        public bool Init(bool bmanual)
        {
            MotionMgr.GetInstace().ServoOn(11);
            bFirist = true;
            ResetAllLineState();
            Config();
            lineSenor.feedMode = FeedMode.前进料;
            lineLock.bOutMotorRunDir = true;
            WaranResult waranResult = HomeSigleAxisPosWaitInpos(lineUpDown.nAxisNo, this, 2000000, bmanual);
            return waranResult == WaranResult.Run;
        }

        bool bFirist = false;
        LineSegementState FiristLineState = LineSegementState.None;
        public void LineRun(bool bmanual)
        {
            if (bFirist)
            {
                //第一段流水线 启动整个流水线
               // LineBack1.ForwardRun(this, FiristLineState, LineBack2.LineSegState, bmanual);
                bFirist = false;
                LineBack2.LineSegState = LineSegementState.None;
                LineBack1.LineSegState = LineSegementState.None;
                lineUpDown.LineSegState = LineSegementState.None;
                lineFront.LineSegState = LineSegementState.None;
                lineSenor.LineSegState = LineSegementState.None;
                lineLock.LineSegState = LineSegementState.None;
            }
            else
            {
                if(LineBack2.LineSegState < LineSegementState.Entrying)
                {
                    lineUpDown.ForwardRun(this, LineSegementState.Outing, lineFront.LineSegState, bmanual);
                }
                else
                {
                    lineUpDown.ForwardRun(this, LineBack2.LineSegState, lineFront.LineSegState, bmanual);
                }

                LineBack1.ForwardRun(this, FiristLineState, LineBack2.LineSegState, bmanual);
                LineBack2.ForwardRun(this, LineBack1.LineSegState, lineUpDown.LineSegState, bmanual);
                lineFront.ForwardRun(this, lineUpDown.LineSegState, lineSenor.LineSegState, bmanual);
                lineSenor.LineRun(this, lineFront.LineSegState, lineLock.LineSegState, bmanual);
                lineLock.ForwardRun(this, lineSenor.LineSegState, LineSegementState.None, bmanual);
                lineLock.IsLastSegLine = true;

                if (LineBack1.LineSegState == LineSegementState.Have)
                {
                    LineBack1.LineSegState = LineSegementState.Finish;
                    this.Info($"{LineBack2.LineName} 状态:{LineSegementState.Have}->{LineSegementState.Finish}");
                }
                if (LineBack2.LineSegState == LineSegementState.Have)
                {
                    LineBack2.LineSegState = LineSegementState.Finish;
                    Info($"{LineBack2.LineName} 状态:{LineSegementState.Have}->{LineSegementState.Finish}");
                }
                if(!lineLock.bOutMotorRunDir)
                {
                    if (lineLock.LineSegState == LineSegementState.OutFinish)
                        lineLock.bOutMotorRunDir = true;

                }
                if(lineSenor.feedMode == FeedMode.后进料)
                {
                    if (lineSenor.LineSegState >=LineSegementState.BackEntryReady)
                    {
                        lineSenor.feedMode = FeedMode.前进料;
                    }
                }
            }

        }


        protected override void StationWork(int step)
        {
            bool bmaual = false;
            switch ((StationStep)step)
            {
                case StationStep.StepInit:
                    ResetAllLineState();
                    Init(false);
                    DelCurrentStep();
                    break;
                case StationStep.LineRun:
                    LineRun(bmaual);
                    break;

            }


        }

    }

}
