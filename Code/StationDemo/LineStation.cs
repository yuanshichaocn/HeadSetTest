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
        锁付段皮带进入感应,
        锁付段皮带离开感应,
        锁付段皮带阻挡气缸上升位,
        锁付段皮带阻挡气缸下降位,
        锁付段皮带顶升气缸上升位,
        锁付段皮带顶升气缸下降位,
        锁付段皮带线到位感应,
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
           前壳段皮带启动,
           前壳段皮带阻挡气缸上升,
           前壳段皮带阻挡气缸下降,
           前壳段皮带顶升气缸上升,
           前壳段皮带顶升气缸下降,

           sensor段皮带启动,
           sensor段皮带阻挡气缸上升,
           sensor段皮带阻挡气缸下降,
	       sensor段皮带顶升气缸上升,
           sensor段皮带顶升气缸下降,
         
           锁付段皮带阻挡气缸上升,
	       锁付段皮带阻挡气缸下降,
           锁付段皮带顶升气缸上升,
           锁付段皮带顶升气缸下降,
           锁付段皮带启动,
           左升降机皮带启动,
           左升降机皮带方向,
           前壳NG料传送按钮,
           sensorNG料传送按钮,
           前壳NG料皮带启动,
           sensorNG料皮带启动,
           回流1段皮带启动,
           回流2段皮带启动,
    }


    public class LineStation : CommonTools.Stationbase
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

            m_listIoInput.Add(LineIoIn.锁付段皮带进入感应.ToString());
            m_listIoInput.Add(LineIoIn.锁付段皮带离开感应.ToString());
            m_listIoInput.Add(LineIoIn.锁付段皮带阻挡气缸上升位.ToString());
            m_listIoInput.Add(LineIoIn.锁付段皮带阻挡气缸下降位.ToString());
            m_listIoInput.Add(LineIoIn.锁付段皮带顶升气缸上升位.ToString());
            m_listIoInput.Add(LineIoIn.锁付段皮带顶升气缸下降位.ToString());

            m_listIoInput.Add(LineIoIn.前壳段皮带线到位感应.ToString());
            m_listIoInput.Add(LineIoIn.sensor段皮带线到位感应.ToString());
            m_listIoInput.Add(LineIoIn.锁付段皮带线到位感应.ToString());

           
            m_listIoOutput.Add(LineIoOut.前壳段皮带启动.ToString());
            m_listIoOutput.Add(LineIoOut.sensor段皮带启动.ToString());
            m_listIoOutput.Add(LineIoOut.锁付段皮带启动.ToString());

            m_listIoOutput.Add(LineIoOut.sensor段皮带阻挡气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.sensor段皮带阻挡气缸下降.ToString());
            m_listIoOutput.Add(LineIoOut.sensor段皮带顶升气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.sensor段皮带顶升气缸下降.ToString());
          


            
            m_listIoOutput.Add(LineIoOut.前壳段皮带阻挡气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.前壳段皮带阻挡气缸下降.ToString());
            m_listIoOutput.Add(LineIoOut.前壳段皮带顶升气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.前壳段皮带顶升气缸下降.ToString());


           
            m_listIoOutput.Add(LineIoOut.锁付段皮带阻挡气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.锁付段皮带阻挡气缸下降.ToString());
            m_listIoOutput.Add(LineIoOut.锁付段皮带顶升气缸上升.ToString());
            m_listIoOutput.Add(LineIoOut.锁付段皮带顶升气缸下降.ToString());

      
            m_listIoOutput.Add(LineIoOut.前壳NG料传送按钮.ToString());
            m_listIoOutput.Add(LineIoOut.sensorNG料传送按钮.ToString());
            m_listIoOutput.Add(LineIoOut.前壳NG料皮带启动.ToString());
            m_listIoOutput.Add(LineIoOut.sensorNG料皮带启动.ToString());

        }
        enum StationStep
        {
            StepInit = 100,
          
        }

        LineSegmentAction lineFront = new LineSegmentAction("前壳流水线");
        LineSegmentAction lineSenor= new LineSegmentAction("Sensor流水线");
        LineSegmentUpDownUseMotor lineUpDown = new LineSegmentUpDownUseMotor("升降机");
        LineSegmentAction lineLock = new LineSegmentAction("锁付流水线");
        LineNg lineNgFront = new LineNg("前壳NG皮带");
        LineNg lineNgSensor = new LineNg("SensorNG皮带");

        LineNg lineNgHalfProduct = new LineNg("半成品NG皮带");

        LineNg lineNgLineBack1 = new LineNg("回流1皮带");
        LineNg lineNgLineBack2 = new LineNg("回流2皮带");
        LineSegementState LineStateOfBackMachine
        {
            get
            {
                if( sys.g_AppMode == BaseDll.AppMode.AirRun)
                    return LineSegementState.None;
                else
                    //通过通讯获取数据
                  return LineSegementState.None;
            }
        }
        public void Config()
        {
            lineFront.ForwardMotorIo = LineIoOut.前壳段皮带启动.ToString();
            lineFront.StopCylinderUpIo = LineIoOut.前壳段皮带阻挡气缸上升.ToString();
            lineFront.StopCylinderDownIo = LineIoOut.前壳段皮带阻挡气缸下降.ToString();
            lineFront.JackUpCylinderUpIo = LineIoOut.前壳段皮带顶升气缸上升.ToString();
            lineFront.JackUpCylinderDownIo = LineIoOut.前壳段皮带顶升气缸下降.ToString();

            lineFront.JackUpCylinderUpIoInPos = LineIoIn.前壳段皮带顶升气缸上升位.ToString();
            lineFront.JackUpCylinderDownIoInPos = LineIoIn.前壳段皮带顶升气缸下降位.ToString();
            lineFront.StopCylinderUpInPosIo = LineIoIn.前壳段皮带阻挡气缸上升位.ToString();
            lineFront.StopCylinderDwonInPosIo = LineIoIn.前壳段皮带阻挡气缸下降位.ToString();
            lineFront.InPosCheckIo = LineIoIn.前壳段皮带线到位感应.ToString();
            lineFront.EnteryCheckIo = LineIoIn.前壳段皮带进入感应.ToString();
            lineFront.LeaveCheckIo = LineIoIn.前壳段皮带离开感应.ToString();

            lineSenor.ForwardMotorIo = LineIoOut.sensor段皮带启动.ToString();
            lineSenor.StopCylinderUpIo = LineIoOut.sensor段皮带阻挡气缸上升.ToString();
            lineSenor.StopCylinderDownIo = LineIoOut.sensor段皮带阻挡气缸下降.ToString();
            lineSenor.JackUpCylinderUpIo = LineIoOut.sensor段皮带顶升气缸上升.ToString();
            lineSenor.JackUpCylinderDownIo = LineIoOut.sensor段皮带顶升气缸下降.ToString();

            lineSenor.JackUpCylinderUpIoInPos = LineIoIn.sensor段皮带顶升气缸上升位.ToString();
            lineSenor.JackUpCylinderDownIoInPos = LineIoIn.sensor段皮带顶升气缸下降位.ToString();
            lineSenor.StopCylinderUpInPosIo = LineIoIn.sensor段皮带阻挡气缸上升位.ToString();
            lineSenor.StopCylinderDwonInPosIo = LineIoIn.sensor段皮带阻挡气缸下降位.ToString();
            lineSenor.InPosCheckIo = LineIoIn.sensor段皮带线到位感应.ToString();
            lineSenor.EnteryCheckIo = LineIoIn.sensor段皮带进入感应.ToString();
            lineSenor.LeaveCheckIo = LineIoIn.sensor段皮带离开感应.ToString();


            lineLock.ForwardMotorIo = LineIoOut.锁付段皮带启动.ToString();
            lineLock.StopCylinderUpIo = LineIoOut.锁付段皮带阻挡气缸上升.ToString();
            lineLock.StopCylinderDownIo = LineIoOut.锁付段皮带阻挡气缸下降.ToString();
            lineLock.JackUpCylinderUpIo = LineIoOut.锁付段皮带顶升气缸上升.ToString();
            lineLock.JackUpCylinderDownIo = LineIoOut.锁付段皮带顶升气缸下降.ToString();

            lineLock.JackUpCylinderUpIoInPos = LineIoIn.锁付段皮带顶升气缸上升位.ToString();
            lineLock.JackUpCylinderDownIoInPos = LineIoIn.锁付段皮带顶升气缸下降位.ToString();
            lineLock.StopCylinderUpInPosIo = LineIoIn.锁付段皮带阻挡气缸上升位.ToString();
            lineLock.StopCylinderDwonInPosIo = LineIoIn.锁付段皮带阻挡气缸下降位.ToString();
            lineLock.InPosCheckIo = LineIoIn.锁付段皮带线到位感应.ToString();
            lineLock.EnteryCheckIo = LineIoIn.锁付段皮带进入感应.ToString();
            lineLock.LeaveCheckIo = LineIoIn.锁付段皮带离开感应.ToString();


            lineUpDown.EnteryCheckIo = LineIoIn.左边升降机皮带进入感应.ToString();
            lineUpDown.LeaveCheckIo = LineIoIn.左边升降机皮带进入感应.ToString();
            lineUpDown.InPosCheckIo = LineIoIn.左边升降机皮带到位感应.ToString();
            lineUpDown.nAxisNo = 3;

            lineNgFront.FullMaterialCheckIO = LineIoIn.前壳NG料满料感应.ToString();
            lineNgFront.EnteryCheckIo = LineIoIn.前壳NG料放入感应.ToString();
            lineNgFront.ForwardMotorIo = LineIoOut.前壳NG料皮带启动.ToString();

            lineNgSensor.FullMaterialCheckIO = LineIoIn.前壳NG料满料感应.ToString();
            lineNgSensor.EnteryCheckIo = LineIoIn.前壳NG料放入感应.ToString();
            lineNgSensor.ForwardMotorIo = LineIoOut.sensorNG料皮带启动.ToString();


            lineNgHalfProduct.FullMaterialCheckIO = LineIoIn.半成品NG料满料感应.ToString();
            lineNgHalfProduct.EnteryCheckIo = LineIoIn.半成品NG料放入感应.ToString();
            lineNgLineBack1.EnteryCheckIo = LineIoIn.回流1段皮带进入感应.ToString();
            lineNgLineBack1.LeaveCheckIo = LineIoIn.回流1段皮带离开感应.ToString();

            lineNgLineBack1.ForwardMotorIo = LineIoOut.回流1段皮带启动.ToString();


            lineNgLineBack2.EnteryCheckIo = LineIoIn.回流2段皮带进入感应.ToString();
            lineNgLineBack2.LeaveCheckIo = LineIoIn.回流2段皮带离开感应.ToString();
            lineNgLineBack2.ForwardMotorIo = LineIoOut.回流1段皮带启动.ToString();

        }
        protected override bool InitStation()
        {
            Config();
            return true;
        }

        public override void PauseDeal()
        {
            lineUpDown.Pause();
            lineFront.Pause();
            lineSenor.Pause();
            lineLock.Pause();
        }
        public override void ResumeDeal()
        {
            lineUpDown.Resume();
            lineFront.Resume();
            lineSenor.Resume();
            lineLock.Resume();
        }




        protected override void StationWork(int step)
        {
            bool bmaual = false;
            lineUpDown.ForwardRun(this, lineFront.LineSegState, bmaual);
            lineFront.ForwardRun(this, lineSenor.LineSegState, bmaual);
            lineSenor.ForwardRun(this, lineLock.LineSegState, bmaual);
            lineLock.ForwardRun(this, LineStateOfBackMachine, bmaual);
            lineNgFront.ForwardRun(this, LineSegementState.NoNextLine, bmaual);
            lineNgSensor.ForwardRun(this, LineSegementState.NoNextLine, bmaual);
            lineNgLineBack1.ForwardRun(this, lineUpDown.LineSegState, bmaual);
            lineNgLineBack2.ForwardRun(this, lineNgLineBack1.LineSegState, bmaual);
        }

    }

}
  