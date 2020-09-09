


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BaseDll;
using MotionIoLib;
using Newtonsoft.Json;
using SerialDict;

namespace XYZDispensVision
{
    /// <summary>
    /// 点胶轨迹的基本元素
    /// </summary>

    public class DispTraceBaseElement
    {
        public bool bIsAllPointMachine = false;//是否全是机械点
        public bool bIsDespense = true;//是否点胶
        public string ItemName = "DispTraceBaseElement";
      
        public string strType = "";
        public DispTraceBaseElement()
        {
            strType = this.GetType().ToString();
        }
    }


    public class DispEveryTraceMoveParam
    {
        public double SpeedHigh;
        public double SpeedLow;
        public double Acc;
        public double Dec;
    }
    public struct DispAllTraceMoveParam
    {
        public double SpeedHigh;
        public double SpeedLow;
        public double Acc;
        public double Dec;
        public bool bEnableBlending;
        public int nBlendingTime;
    }

    public class DispTraceBaseElementDelay : DispTraceBaseElement
    {
        public int delay = 0;
    }
    public class DispTraceBaseElementIo : DispTraceBaseElement
    {
        public int nCardIndex;
        public int nAxisNo;
        public int nIoIndex;
        private string strIoName = "";

        public string DispIoName
        {
            set
            {
                strIoName = value;
                Parse();
            }
            get
            {
                return strIoName;
            }
        }

        public bool bDispIoState = true;
        public void Parse()
        {
            if (strIoName != null && strIoName != "")
                if (IOMgr.GetInstace().GetOutputDic().ContainsKey(strIoName))
                {
                    nCardIndex = IOMgr.GetInstace().GetOutputDic()[strIoName]._CardIndex;
                    nAxisNo = IOMgr.GetInstace().GetOutputDic()[strIoName]._AxisIndex;
                    nIoIndex = IOMgr.GetInstace().GetOutputDic()[strIoName]._IoIndex;
                }
        }

    }
    public class DispTraceBaseElementEnd : DispTraceBaseElement
    {
    }
    public class PointCoordinateElement
    {

        public double dMx = 0;
        public double dMy = 0;
        public double dMz = 0;

        public double dVx = 0;
        public double dVy = 0;
    }

    public class DispTraceBaseElementPoint : DispTraceBaseElement
    {
        public PointCoordinateElement PointCoordinate = new PointCoordinateElement();
        public DispEveryTraceMoveParam TraceMoveParam = new DispEveryTraceMoveParam();

    }
  
    public class DispTraceBaseElementLine : DispTraceBaseElement
    {
        public PointCoordinateElement cStartPoint = new PointCoordinateElement();
        public PointCoordinateElement cEndPoint = new PointCoordinateElement();
        public DispEveryTraceMoveParam TraceMoveParam = new DispEveryTraceMoveParam();
    }
    /// <summary>
    ///  S代表圆弧的起点
    ///  C代表圆弧的中心
    ///  E代表圆弧的终点
    ///  R代表圆弧的半径
    ///  D代表圆弧的角度
    /// </summary>
    //public class DispTraceBaseElementArc_SCE : DispTraceBaseElement
    //{
    //    public DispTraceBaseElementPoint cCenterPoint = new DispTraceBaseElementPoint();
    //    public DispTraceBaseElementPoint cStartPoint = new DispTraceBaseElementPoint();
    //    public DispTraceBaseElementPoint cEndPoint = new DispTraceBaseElementPoint();
    //    public DispEveryTraceMoveParam TraceMoveParam = new DispEveryTraceMoveParam();  
    //}
    //public class DispTraceBaseElementArc_SER : DispTraceBaseElement
    //{
    //    public DispTraceBaseElementPoint cCenterPoint = new DispTraceBaseElementPoint();
    //    public DispTraceBaseElementPoint cStartPoint = new DispTraceBaseElementPoint();
    //    public DispTraceBaseElementPoint cEndPoint = new DispTraceBaseElementPoint();
    //    public DispEveryTraceMoveParam TraceMoveParam = new DispEveryTraceMoveParam();
    //    public double dR = 0;
    //}
    //public class DispTraceBaseElementArc_SED : DispTraceBaseElement
    //{
    //    public DispTraceBaseElementPoint cCenterPoint = new DispTraceBaseElementPoint();
    //    public DispTraceBaseElementPoint cStartPoint = new DispTraceBaseElementPoint();
    //    public DispTraceBaseElementPoint cEndPoint = new DispTraceBaseElementPoint();
    //    public DispEveryTraceMoveParam TraceMoveParam = new DispEveryTraceMoveParam();
    //    public double dStartDeg = 0;
    //    public double dEndDeg = 0;
    //    public double dR = 0;
    //}
    public class DispTraceBaseElementArc : DispTraceBaseElement
    {
        public PointCoordinateElement cCenterPoint = new PointCoordinateElement();
        public PointCoordinateElement cStartPoint = new PointCoordinateElement();
        public PointCoordinateElement cEndPoint = new PointCoordinateElement();
        public DispEveryTraceMoveParam TraceMoveParam = new DispEveryTraceMoveParam();
        public double dStartDeg = 0;
        public double dEndDeg = 0;
        public double dR = 0;
        public bool bIsArcDir = true; //逆时针为正
        public bool bIsArc = false;// 是否为圆弧
    }
    // <summary>
    /// 点胶轨迹的基本元素组合的形状集合
    /// </summary>
    public class DispTraceBaseShape : DispTraceBaseElement
    {


        public List<DispTraceBaseElement> dispTraceBaseElements = new List<DispTraceBaseElement>();
        /// <summary>
        /// 对形状进行解析
        /// </summary>
        public virtual void Parse()
        {

        }
        public List<DispTraceBaseElement> AddTo(List<DispTraceBaseElement> Dst)
        {
            if (Dst == null)
                return null;
            if (dispTraceBaseElements == null)
                return Dst;
            foreach (var tem in dispTraceBaseElements)
                Dst.Add(tem);
            return Dst;
        }
    }

    public class DispTraceRect : DispTraceBaseShape
    {

        public DispTraceBaseElementPoint cStart = new DispTraceBaseElementPoint();
        public DispTraceBaseElementPoint cEnd = new DispTraceBaseElementPoint();


        public DispTraceBaseElementPoint cVRow1 = new DispTraceBaseElementPoint();
        public DispTraceBaseElementPoint cVCol1 = new DispTraceBaseElementPoint();
        public DispTraceBaseElementPoint cVRow2 = new DispTraceBaseElementPoint();
        public DispTraceBaseElementPoint cVCol2 = new DispTraceBaseElementPoint();


        public DispTraceBaseElementPoint cMX1 = new DispTraceBaseElementPoint();
        public DispTraceBaseElementPoint cMY1 = new DispTraceBaseElementPoint();
        public DispTraceBaseElementPoint cMX2 = new DispTraceBaseElementPoint();
        public DispTraceBaseElementPoint cMY2 = new DispTraceBaseElementPoint();
        public override void Parse()
        {
            //foreach (System.Reflection.PropertyInfo info in this.GetType().GetProperties())
            //{
            //   if( this.GetType().GetProperty(info.Name).PropertyType == typeof(DispTraceBaseElementPoint))
            //    {
            //        this.GetType().GetProperty(info.Name).
            //    }
            //}
        }
    }


    public class DispTraceMgr
    {

        private DispTraceMgr() { }
        private static DispTraceMgr dispTraceElementsMgr = null;
        private static object objectlock = new object();
        public static DispTraceMgr GetInstance()
        {
            if (dispTraceElementsMgr == null)
            {
                lock (objectlock)
                {
                    if (dispTraceElementsMgr == null)
                    {
                        dispTraceElementsMgr = new DispTraceMgr();
                        dispTraceElementsMgr.dispTraces.Add("FiristTrace", new List<DispTraceBaseElement>());
                    }
                }
            }
            return dispTraceElementsMgr;
        }
        public delegate void UpdatDataGridViewHandler(string OperateName,object obj);
        public event UpdatDataGridViewHandler eventUpdataGridView;

        public XY_UR_Calib XYUR_Pin = null;
        public XY_UR_Calib XYUR_Laser = null;

        private Dictionary<string, List<DispTraceBaseElement>> dispTraces = new Dictionary<string, List<DispTraceBaseElement>>();

        public void Save(string Path)
        {
           // dispTraces.Add("SecondTrace", new List<DispTraceBaseElement>() { dispTraces["FiristTrace"][0], dispTraces["FiristTrace"][1] });
            AccessJosnSerializer.ObjectToJson("D:\\123.json", dispTraces);


        }
        List<string> ParseJsonString_List(string strobjs)
        {
            Stack<char> stk = new Stack<char>();
            List<string> JsonItems = new List<string>();

            int sum = 0;
            bool bhave = false;

            List<char> ItemCollect = new List<char>();
            char[] json = strobjs.ToCharArray();
            for (int i = 0; i < strobjs.Length; i++)
            {
                if (json[i] == '{')
                {
                    sum++;
                    bhave = true;
                }
                if (json[i] == '}')
                    sum--;
                stk.Push(json[i]);
                if (sum == 0 && bhave)
                {
                    ItemCollect.Clear();
                    while (stk.Peek() != '[')
                    {
                        ItemCollect.Add(stk.Pop());
                    }
                    bhave = false;
                    if (ItemCollect != null && ItemCollect.Count > 0)
                    {
                        Char[] itemchars = ItemCollect.ToArray();
                        int s1 = itemchars.Length - 1;
                        int s2 = 0;
                        char tempchar;

                        while ((itemchars.Length % 2 == 0 && s2 + 1 != s1) || (itemchars.Length % 2 == 1 && s1 != s2))
                        {
                            tempchar = itemchars[s1];
                            itemchars[s1] = itemchars[s2];
                            itemchars[s2] = tempchar;
                            s1--;
                            s2++;
                        }
                        string s = new string(itemchars);
                        int indexofdh = s.IndexOf(",");
                        if (indexofdh == 0)
                            s = s.Substring(1);
                        JsonItems.Add(s);
                    }

                }
            }
            return JsonItems;
        }


        //Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        void ParseJsonString_DicList(string strobjs, Dictionary<string, List<string>> dics)
        {

            int index = strobjs.IndexOf("]");
            if (index == -1)
                return;
            else
            {
                string strSub = strobjs.Substring(0, index);
                int indexfront = strSub.LastIndexOf("[");
                if (indexfront == 0)
                {
                    string itemstr = strobjs.Substring(indexfront, index - indexfront + 1);
                    ParseJsonString_List(itemstr);

                }
                else if (indexfront > 0 && strSub[indexfront - 1] == ':')
                {
                    int yhback = strSub.Substring(0, indexfront).LastIndexOf('"');
                    int yhfornt = strSub.Substring(0, indexfront).Substring(0, yhback).LastIndexOf('"');
                    string keystr = strSub.Substring(yhfornt, yhback - yhfornt + 1);
                    string itemstr = strobjs.Substring(indexfront, index - indexfront + 1);
                    dics.Add(keystr, ParseJsonString_List(itemstr));
                    ParseJsonString_DicList(strobjs.Substring(index + 1), dics);

                }
                else if (indexfront > 0 && strSub[indexfront - 1] == '{')
                {
                    string itemstr = strobjs.Substring(indexfront, index - indexfront + 1);
                    ParseJsonString_DicList(itemstr, dics);
                }
            }

        }
        void ParsejsonToTraces(string jsonString, Dictionary<string, List<DispTraceBaseElement>> DispTraces)
        {
            Dictionary<string, List<object>> ds = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(jsonString);
            //Dictionary<string, List<DispTraceBaseElement>> DispTraces = new Dictionary<string, List<DispTraceBaseElement>>();
            dispTraces.Clear();
            foreach (var tem in ds)
            {
                List<DispTraceBaseElement> TraceElements = new List<DispTraceBaseElement>();
                foreach (var tem1 in tem.Value)
                {
                    string strItemElement = tem1.ToString().Trim('\r', '\n');
                    string TypeElement = (JsonConvert.DeserializeObject<DispTraceBaseElement>(strItemElement) as DispTraceBaseElement).strType;
                    Assembly assembly = Assembly.GetAssembly(typeof(DispTraceBaseElement));
                    Type type = assembly.GetType(TypeElement);
                   
                    DispTraceBaseElement dispTraceBaseElement = JsonConvert.DeserializeObject(strItemElement, type) as DispTraceBaseElement;
                    // TraceElements.Add(Activator.CreateInstance(type, null) as DispTraceBaseElement);
                    TraceElements.Add(dispTraceBaseElement);


                }
                DispTraces.Add(tem.Key, TraceElements);
            }
        }

        public void Read(string Path)
        {

            string txt = File.ReadAllText(Path);
            ParsejsonToTraces(txt, dispTraces);
        }
        public void SetItem(int nIndex,DispTraceBaseElement dispTraceBaseElement, string DispTraceName = "FiristTrace")
        {
            if (dispTraces.ContainsKey(DispTraceName))
            {
                if (dispTraces[DispTraceName].Count > nIndex)
                dispTraces[DispTraceName][nIndex] = dispTraceBaseElement;
                
            }
            
        }
    
        public void AddItemToList(DispTraceBaseElement dispTraceBaseElement, string DispTraceName = "FiristTrace")
        {
            //  dispTraceBaseElements.Add(dispTraceBaseElement);
            if (dispTraces.ContainsKey(DispTraceName))
            {
                dispTraces[DispTraceName].Add(dispTraceBaseElement);
               if(eventUpdataGridView != null)
                   eventUpdataGridView("Add", dispTraceBaseElement);
            } 
            else
            {
                dispTraces.Add(DispTraceName, new List<DispTraceBaseElement>() { dispTraceBaseElement });
            }
        }
       public bool CheckReName(string ItemNmae, string DispTraceName = "FiristTrace")
        {
            if (dispTraces.ContainsKey(DispTraceName))
            {
                if (dispTraces[DispTraceName].FindIndex((t) => t.ItemName == ItemNmae) != -1)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

        public void DelItemFromList(string strItemName, string DispTraceName = "FiristTrace")
        {
            if (dispTraces.ContainsKey(DispTraceName))
            {
                int index = dispTraces[DispTraceName].FindIndex((t) => t.ItemName == strItemName);
                if (index != -1)
                {
                    dispTraces[DispTraceName].RemoveAt(index);
                    if(eventUpdataGridView!=null)
                        eventUpdataGridView("Del", index);
                }
                   
            }
        }
        public DispTraceBaseElement GetItem( int index, string DispTraceName = "FiristTrace")
        {
            DispTraceBaseElement dispTraceBaseElement = null;
            if (dispTraces.ContainsKey(DispTraceName))
            {
                if (dispTraces[DispTraceName].Count > index)
                {
                    return dispTraces[DispTraceName][index];
                }
            }
            return dispTraceBaseElement;
        }
        public DispTraceBaseElement GetItem(string  ItemName, string DispTraceName = "FiristTrace")
        {
            DispTraceBaseElement dispTraceBaseElement = null;
            if (dispTraces.ContainsKey(DispTraceName))
            {
                int index = dispTraces[DispTraceName].FindIndex((t) => t.ItemName == ItemName);
                if (index == -1)
                    return null;
                return dispTraces[DispTraceName][index];

            }
            return dispTraceBaseElement;
        }
        public void DelItemFromList(int  row, string DispTraceName = "FiristTrace")
        {
            if (dispTraces.ContainsKey(DispTraceName))
            {
                if (dispTraces[DispTraceName].Count > row)
                {
                    dispTraces[DispTraceName].RemoveAt(row);
                    if (eventUpdataGridView != null)
                        eventUpdataGridView("Del", row);
                }

            }
        }


        public void Clear(string DispTraceName = "FiristTrace")
        {
            if (dispTraces.ContainsKey(DispTraceName))
            {
                dispTraces[DispTraceName].Clear();
                if (eventUpdataGridView != null)
                    eventUpdataGridView("Clr", 0);
            }
        }
        public string GpName = "";

        public void Parse(string DispTraceName)
        {
            if (!dispTraces.ContainsKey(DispTraceName))
                return;
            List<DispTraceBaseElement> dispTraceBaseElements = dispTraces[DispTraceName];
            foreach (var tem in dispTraceBaseElements)
            {

                if (tem.GetType() == typeof(DispTraceBaseElementDelay))
                {
                    MotionMgr.GetInstace().AddBufDelay(GpName, ((DispTraceBaseElementDelay)tem).delay);
                }
                if (tem.GetType() == typeof(DispTraceBaseElementIo))
                {
                    MotionMgr.GetInstace().AddBufIo(GpName, ((DispTraceBaseElementIo)tem).DispIoName, ((DispTraceBaseElementIo)tem).bDispIoState);
                }
                if (tem.GetType() == typeof(DispTraceBaseElementLine))
                {
                    double vellow = ((DispTraceBaseElementLine)tem).TraceMoveParam.SpeedLow;
                    double velhigh = ((DispTraceBaseElementLine)tem).TraceMoveParam.SpeedHigh;
                    double[] StartPoint = new double[3];
                    StartPoint[0] = ((DispTraceBaseElementLine)tem).cStartPoint.dMx;
                    StartPoint[1] = ((DispTraceBaseElementLine)tem).cStartPoint.dMy;
                    StartPoint[2] = ((DispTraceBaseElementLine)tem).cStartPoint.dMz;
                    MotionMgr.GetInstace().AddBufMove(GpName, BufMotionType.buf_Line3dAbs, 0, 3, velhigh, vellow, null, StartPoint);
                    double[] EndPoint = new double[3];
                    EndPoint[0] = ((DispTraceBaseElementLine)tem).cEndPoint.dMx;
                    EndPoint[1] = ((DispTraceBaseElementLine)tem).cEndPoint.dMy;
                    EndPoint[2] = ((DispTraceBaseElementLine)tem).cEndPoint.dMz;
                    MotionMgr.GetInstace().AddBufMove(GpName, BufMotionType.buf_Line3dAbs, 0, 3, velhigh, vellow, null, EndPoint);
                }
                if (tem.GetType() == typeof(DispTraceBaseElementArc))
                {
                    double vellow = ((DispTraceBaseElementArc)tem).TraceMoveParam.SpeedLow;
                    double velhigh = ((DispTraceBaseElementArc)tem).TraceMoveParam.SpeedHigh;
                    double[] StartPoint = new double[3];
                    StartPoint[0] = ((DispTraceBaseElementArc)tem).cStartPoint.dMx;
                    StartPoint[1] = ((DispTraceBaseElementArc)tem).cStartPoint.dMy;
                    StartPoint[2] = ((DispTraceBaseElementArc)tem).cStartPoint.dMz;
                    MotionMgr.GetInstace().AddBufMove(GpName, BufMotionType.buf_Line3dAbs, 0, 3, velhigh, vellow, null, StartPoint);
                    double[] CenterPoint = new double[3];
                    CenterPoint[0] = ((DispTraceBaseElementArc)tem).cEndPoint.dMx;
                    CenterPoint[1] = ((DispTraceBaseElementArc)tem).cEndPoint.dMy;
                    double[] EndPoint = new double[3];
                    EndPoint[0] = ((DispTraceBaseElementArc)tem).cEndPoint.dMx;
                    EndPoint[1] = ((DispTraceBaseElementArc)tem).cEndPoint.dMy;
                    if (((DispTraceBaseElementArc)tem).bIsArcDir)//逆时针
                        MotionMgr.GetInstace().AddBufMove(GpName, BufMotionType.buf_Arc2dAbsCCW, 0, 2, velhigh, vellow, CenterPoint, EndPoint);
                    else
                        MotionMgr.GetInstace().AddBufMove(GpName, BufMotionType.buf_Arc2dAbsCW, 0, 2, velhigh, vellow, CenterPoint, EndPoint);

                }

            }

        }
        /// <summary>
        /// 仿射变化后的图像上的线段上各点 转换成机械点
        /// </summary>
        /// <param name="ItemName">轨迹中线段元素名称</param>
        /// <param name="SnapPoint">拍照位置的机械点</param>
        /// <param name="StatrtVPonit">仿射变化后的图像上的线段的起始点</param>
        /// <param name="StatrtEPonit">仿射变化后的图像上的线段的终止点</param>
        /// <param name="OffsetX">最后的统一补偿X</param>
        /// <param name="OffsetY">最后的统一补偿Y</param>
        /// <param name="OffsetZ">最后的统一补偿Z</param>
        /// <param name="DispTraceName">轨迹名称</param>
        /// <summary>
        public void TransLine(string ItemName, XYUPoint SnapPoint, XYUPoint StatrtVPonit, XYUPoint StatrtEPonit, double OffsetX, double OffsetY, double OffsetZ, string DispTraceName = "FiristTrace")
        {
            if (dispTraces.ContainsKey(DispTraceName))
            {
                int index = dispTraces[DispTraceName].FindIndex((t) => t.ItemName == ItemName);
                if (index == -1)
                {
                    return;
                }
                DispTraceBaseElement tem = null;
                tem = dispTraces[DispTraceName][index];

                if (tem.GetType() == typeof(DispTraceBaseElementDelay) || tem.GetType() == typeof(DispTraceBaseElementIo) || tem.GetType() == typeof(DispTraceBaseElementEnd))
                    return;
                ((DispTraceBaseElementLine)tem).cStartPoint.dMz = ((DispTraceBaseElementLine)tem).cStartPoint.dMz + OffsetZ;
                ((DispTraceBaseElementLine)tem).cEndPoint.dMz = ((DispTraceBaseElementLine)tem).cEndPoint.dMz + OffsetZ;
                if (!tem.bIsAllPointMachine)
                {
                    if (tem.GetType() == typeof(DispTraceBaseElementLine))
                    {
                        ((DispTraceBaseElementLine)tem).cStartPoint.dMx = XYUR_Pin.GetDstPonit(StatrtVPonit, SnapPoint).x + OffsetX;
                        ((DispTraceBaseElementLine)tem).cStartPoint.dMy = XYUR_Pin.GetDstPonit(StatrtVPonit, SnapPoint).y + OffsetY;

                        ((DispTraceBaseElementLine)tem).cEndPoint.dMx = XYUR_Pin.GetDstPonit(StatrtEPonit, SnapPoint).x + OffsetX;
                        ((DispTraceBaseElementLine)tem).cEndPoint.dMy = XYUR_Pin.GetDstPonit(StatrtEPonit, SnapPoint).y + OffsetY;

                    }

                }
            }

        }


        /// <summary>
        /// 仿射变化后的图像上的圆弧上各点 转换成机械点
        /// </summary>
        /// <param name="ItemName">轨迹中圆弧元素名称</param>
        /// <param name="SnapPoint">拍照位置的机械点</param>
        /// <param name="StatrtVPonit">仿射变化后的图像上的圆弧的起始点</param>
        /// <param name="CenterVPonit">仿射变化后的图像上的圆弧的圆心点</param>
        /// <param name="StatrtEPonit">仿射变化后的图像上的圆弧的终止点</param>
        /// <param name="OffsetX">最后的统一补偿X</param>
        /// <param name="OffsetY">最后的统一补偿Y</param>
        /// <param name="OffsetZ">最后的统一补偿Z</param>
        /// <param name="DispTraceName">轨迹名称</param>
        /// <summary>

        public void TransArc(string ItemName, XYUPoint SnapPoint, XYUPoint StatrtVPonit, XYUPoint CenterVPonit, XYUPoint StatrtEPonit, double OffsetX, double OffsetY, double OffsetZ, string DispTraceName = "FiristTrace")
        {
            if (dispTraces.ContainsKey(DispTraceName))
            {
                int index = dispTraces[DispTraceName].FindIndex((t) => t.ItemName == ItemName);
                if (index == -1)
                {
                    return;
                }
                DispTraceBaseElement tem = null;
                tem = dispTraces[DispTraceName][index];

                if (tem.GetType() == typeof(DispTraceBaseElementDelay) || tem.GetType() == typeof(DispTraceBaseElementIo) || tem.GetType() == typeof(DispTraceBaseElementEnd))
                    return;
                ((DispTraceBaseElementArc)tem).cStartPoint.dMz = ((DispTraceBaseElementArc)tem).cStartPoint.dMz + OffsetZ;
                ((DispTraceBaseElementArc)tem).cCenterPoint.dMz = ((DispTraceBaseElementArc)tem).cCenterPoint.dMz + OffsetZ;
                ((DispTraceBaseElementArc)tem).cEndPoint.dMz = ((DispTraceBaseElementArc)tem).cEndPoint.dMz + OffsetZ;
                if (!tem.bIsAllPointMachine)
                {
                    if (tem.GetType() == typeof(DispTraceBaseElementArc))
                    {
                        ((DispTraceBaseElementArc)tem).cStartPoint.dMx = XYUR_Pin.GetDstPonit(StatrtVPonit, SnapPoint).x + OffsetX;
                        ((DispTraceBaseElementArc)tem).cStartPoint.dMy = XYUR_Pin.GetDstPonit(StatrtVPonit, SnapPoint).y + OffsetY;
                        ((DispTraceBaseElementArc)tem).cCenterPoint.dMx = XYUR_Pin.GetDstPonit(CenterVPonit, SnapPoint).x + OffsetX;
                        ((DispTraceBaseElementArc)tem).cCenterPoint.dMy = XYUR_Pin.GetDstPonit(CenterVPonit, SnapPoint).y + OffsetY;
                        ((DispTraceBaseElementArc)tem).cEndPoint.dMx = XYUR_Pin.GetDstPonit(StatrtEPonit, SnapPoint).x + OffsetX;
                        ((DispTraceBaseElementArc)tem).cEndPoint.dMy = XYUR_Pin.GetDstPonit(StatrtEPonit, SnapPoint).y + OffsetY;
                    }
                }
            }

        }
        /// <summary>
        /// 仿射变化后的图像上的点 转换成机械点
        /// </summary>
        /// <param name="ItemName">轨迹中点元素名称</param>
        /// <param name="SnapPoint">拍照位置的机械点</param>
        /// <param name="visionPoint">仿射变化后的图像上的点</param>
        /// <param name="OffsetX">最后的统一补偿X</param>
        /// <param name="OffsetY">最后的统一补偿Y</param>
        /// <param name="OffsetZ">最后的统一补偿Z</param>
        /// <param name="DispTraceName">轨迹名称</param>
        public void TransPoint(string ItemName, XYUPoint SnapPoint, XYUPoint visionPoint, double OffsetX, double OffsetY, double OffsetZ, string DispTraceName = "FiristTrace")
        {
            if (dispTraces.ContainsKey(DispTraceName))
            {
                int index = dispTraces[DispTraceName].FindIndex((t) => t.ItemName == ItemName);
                if (index == -1)
                {
                    return;
                }
                DispTraceBaseElement tem = null;
                tem = dispTraces[DispTraceName][index];

                if (tem.GetType() == typeof(DispTraceBaseElementDelay) || tem.GetType() == typeof(DispTraceBaseElementIo) || tem.GetType() == typeof(DispTraceBaseElementEnd))
                    return;
                if (!tem.bIsAllPointMachine)
                {
                    if (tem.GetType() == typeof(DispTraceBaseElementPoint))
                    {
                        ((DispTraceBaseElementPoint)tem).PointCoordinate.dMx = XYUR_Pin.GetDstPonit(visionPoint, SnapPoint).x + OffsetX;
                        ((DispTraceBaseElementPoint)tem).PointCoordinate.dMy = XYUR_Pin.GetDstPonit(visionPoint, SnapPoint).y + OffsetY;
                    }
                }
            }
        }


    }



}
