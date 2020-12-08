using BaseDll;
using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.IO;
using UserCtrl;

namespace VisionProcess
{
    public enum ShapeType
    {
        点,
        矩形,
        仿射矩形,
        圆形,
    }

    public class shapeparam
    {
        public string name;
        public ShapeType shapeType;
        public UserShape usrshape;

        public shapeparam Clone()
        {
            return new shapeparam()
            {
                name = this.name,
                shapeType = this.shapeType,
                usrshape = this.usrshape.Clone(),
            };
        }
    }

    public struct ShapeNameType
    {
        public string name;
        public ShapeType shapeType;
    }

    public class UserShape
    {
        [NonSerialized]
        [JsonIgnore]
        public string FilePath = "";

        [NonSerialized]
        [JsonIgnore]
        public VisionControl vc = null;

        public string UserTypeName = null;

        public UserShape()
        {
            UserTypeName = this.GetType().ToString();
        }

        public virtual object Read(string strpath)
        {
            return null;
        }

        public virtual bool Save(string strpath)
        {
            return true;
        }

        public virtual object Read()
        {
            return null;
        }

        public virtual bool Save()
        {
            return true;
        }

        public virtual bool bDraw(VisionControl visionControl)
        {
            return true;
        }

        public virtual bool bDraw()
        {
            return true;
        }

        public virtual UserShape Clone()
        {
            return null;
        }
    }

    [Serializable]
    public class UsrShapeCircle : UserShape
    {
        public double CircleCenterX = 0;
        public double CircleCenterY = 0;
        public double CircleRadius = 0;
        public double CircleStartAngle = 0;
        public double CircleEndAngle = 360;

        public override UserShape Clone()
        {
            return new UsrShapeCircle()
            {
                CircleCenterX = this.CircleCenterX,
                CircleCenterY = this.CircleCenterY,
                CircleRadius = this.CircleRadius,
                CircleStartAngle = this.CircleStartAngle,
                CircleEndAngle = this.CircleEndAngle,
            };
        }

        public override object Read()
        {
            if (FilePath != null && File.Exists(FilePath))
            {
                return Read(FilePath);
            }
            return null;
        }

        public override bool Save()
        {
            if (FilePath != null && FilePath != "")
            {
                return Save(FilePath);
            }
            return false;
        }

        public override object Read(string strpath)
        {
            object t = null;
            //t = AccessXmlSerializer.XmlToObject(strpath, GetType());
            t = AccessJosnSerializer.JsonToObject(strpath, GetType());
            if (t != null && t is UsrShapeCircle)
            {
                UsrShapeCircle pt = (UsrShapeCircle)t;
                CircleCenterX = pt.CircleCenterX;
                CircleCenterY = pt.CircleCenterY;
                CircleRadius = pt.CircleRadius;
                CircleRadius = pt.CircleRadius;
                CircleStartAngle = pt.CircleStartAngle;
                CircleEndAngle = pt.CircleEndAngle;
                FilePath = strpath;
            }
            return null;
        }

        public override bool Save(string strpath)
        {
            // return AccessXmlSerializer.ObjectToXml(strpath, this);
            if (AccessJosnSerializer.ObjectToJson(strpath, this))
            {
                FilePath = strpath;
                return true;
            }
            return false;
        }

        public override bool bDraw()
        {
            HTuple Row = 0; HTuple column = 0; HTuple Radiu = 0;
            vc?.DrawCircle(out Row, out column, out Radiu);

            if (vc != null && Row != null && Row.Length > 0)
            {
                return true;
            }

            return false;
        }

        public override bool bDraw(VisionControl visionControl)
        {
            HTuple Row = 0; HTuple column = 0; HTuple Radiu = 0;
            visionControl?.DrawCircle(out Row, out column, out Radiu);

            if (visionControl != null && Row != null && Row.Length > 0)
            {
                CircleCenterX = column[0].D;
                CircleCenterY = Row[0].D;
                CircleRadius = Radiu[0].D;
                return true;
            }

            return false;
        }
    }

    [Serializable]
    public class UsrShapeRect2 : UserShape
    {
        public double CenterX = 0;
        public double CenterY = 0;
        public double Len1 = 40;
        public double Len2 = 20;
        public double Phi = 0;

        public override UserShape Clone()
        {
            return new UsrShapeRect2()
            {
                CenterX = this.CenterX,
                CenterY = this.CenterY,
                Len1 = this.Len1,
                Len2 = this.Len2,
                Phi = this.Phi,
            };
        }

        public override object Read()
        {
            if (FilePath != null && File.Exists(FilePath))
            {
                return Read(FilePath);
            }
            return null;
        }

        public override bool Save()
        {
            if (FilePath != null && FilePath != "")
            {
                return Save(FilePath);
            }
            return false;
        }

        public override object Read(string strpath)
        {
            object t = null;
            //t = AccessXmlSerializer.XmlToObject(strpath, GetType());
            t = AccessJosnSerializer.JsonToObject(strpath, GetType());
            if (t != null && t is UsrShapeRect2)
            {
                UsrShapeRect2 pt = (UsrShapeRect2)t;
                CenterX = pt.CenterX;
                CenterY = pt.CenterY;
                Len1 = pt.Len1;
                Len2 = pt.Len2;
                Phi = pt.Phi;
                FilePath = strpath;
            }
            return null;
        }

        public override bool Save(string strpath)
        {
            // return AccessXmlSerializer.ObjectToXml(strpath, this);
            if (AccessJosnSerializer.ObjectToJson(strpath, this))
            {
                FilePath = strpath;
                return true;
            }
            return false;
        }

        public override bool bDraw()
        {
            HTuple Row = 0; HTuple column = 0; HTuple len1, len2, phi = 0;
            len1 = 100; len2 = 20;
            vc?.DrawRectangle2(out Row, out column, out phi, out len1, out len2);
            if (vc != null && Row != null && Row.Length > 0)
            {
                CenterX = column[0].D;
                CenterY = Row[0].D;
                Phi = phi[0].D;
                Len1 = len1[0].D;
                Len2 = len2[0].D;
                return true;
            }
            return false;
        }

        public override bool bDraw(VisionControl visionControl)
        {
            HTuple Row = 0; HTuple column = 0; HTuple len1, len2, phi = 0;
            len1 = 100; len2 = 20;
            visionControl?.DrawRectangle2(out Row, out column, out phi, out len1, out len2);
            if (visionControl != null && Row != null && Row.Length > 0)
            {
                CenterX = column[0].D;
                CenterY = Row[0].D;
                Phi = phi[0].D;
                Len1 = len1[0].D;
                Len2 = len2[0].D;
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class UsrShapeRect : UserShape
    {
        public double Y1 = 0;
        public double X1 = 0;
        public double Y2 = 40;
        public double X2 = 20;

        public override UserShape Clone()
        {
            return new UsrShapeRect()
            {
                Y1 = this.Y1,
                X1 = this.X1,
                Y2 = this.Y2,
                X2 = this.X2,
            };
        }

        public override object Read()
        {
            if (FilePath != null && File.Exists(FilePath))
            {
                return Read(FilePath);
            }
            return null;
        }

        public override bool Save()
        {
            if (FilePath != null && FilePath != "")
            {
                return Save(FilePath);
            }
            return false;
        }

        public override object Read(string strpath)
        {
            object t = null;
            t = AccessJosnSerializer.JsonToObject(strpath, GetType());
            if (t != null && t is UsrShapeRect)
            {
                UsrShapeRect pt = (UsrShapeRect)t;
                X1 = pt.X1;
                Y1 = pt.Y1;
                X2 = pt.X2;
                Y2 = pt.Y2;
                FilePath = strpath;
            }
            return null;
        }

        public override bool Save(string strpath)
        {
            if (AccessJosnSerializer.ObjectToJson(strpath, this))
            {
                FilePath = strpath;
                return true;
            }
            return false;
        }

        public override bool bDraw()
        {
            HTuple Row1 = 0; HTuple column1 = 0; HTuple Row2 = 0; HTuple column2 = 0;

            vc?.DrawRectangle(out Row1, out column1, out Row2, out column2);
            if (vc != null && Row1 != null && Row1.Length > 0)
            {
                X1 = column1[0].D;
                Y1 = Row1[0].D;
                X2 = column2[0].D;
                Y2 = Row2[0].D;

                return true;
            }
            return false;
        }

        public override bool bDraw(VisionControl visionControl)
        {
            HTuple Row1 = 0; HTuple column1 = 0; HTuple Row2 = 0; HTuple column2 = 0;
            visionControl?.DrawRectangle(out Row1, out column1, out Row2, out column2);
            if (visionControl != null && Row1 != null && Row1.Length > 0)
            {
                X1 = column1[0].D;
                Y1 = Row1[0].D;
                X2 = column2[0].D;
                Y2 = Row2[0].D;

                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class UsrShapePoint : UserShape
    {
        public double Y = 0;
        public double X = 0;

        public override UserShape Clone()
        {
            return new UsrShapePoint()
            {
                Y = this.Y,
                X = this.X,
            };
        }

        public override object Read()
        {
            if (FilePath != null && File.Exists(FilePath))
            {
                return Read(FilePath);
            }
            return null;
        }

        public override bool Save()
        {
            if (FilePath != null && FilePath != "")
            {
                return Save(FilePath);
            }
            return false;
        }

        public override object Read(string strpath)
        {
            object t = null;
            t = AccessJosnSerializer.JsonToObject(strpath, GetType());
            if (t != null && t is UsrShapePoint)
            {
                UsrShapePoint pt = (UsrShapePoint)t;
                X = pt.X;
                Y = pt.Y;

                FilePath = strpath;
            }
            return null;
        }

        public override bool Save(string strpath)
        {
            if (AccessJosnSerializer.ObjectToJson(strpath, this))
            {
                FilePath = strpath;
                return true;
            }
            return false;
        }

        public override bool bDraw()
        {
            HTuple Row1 = 0; HTuple column1 = 0; HTuple Row2 = 0; HTuple column2 = 0;

            vc?.DrawPoint(out Row1, out column1);
            if (vc != null && Row1 != null && Row1.Length > 0)
            {
                X = column1[0].D;
                Y = Row1[0].D;

                return true;
            }
            return false;
        }

        public override bool bDraw(VisionControl visionControl)
        {
            HTuple Row1 = 0; HTuple column1 = 0; HTuple Row2 = 0; HTuple column2 = 0;

            visionControl?.DrawPoint(out Row1, out column1);
            if (visionControl != null && Row1 != null && Row1.Length > 0)
            {
                X = column1[0].D;
                Y = Row1[0].D;

                return true;
            }
            return false;
        }
    }
}