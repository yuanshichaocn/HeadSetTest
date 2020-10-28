using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UserCtrl;
using System.Timers;
using System.ComponentModel;
using System.Reflection;

namespace BaseDll
{

    public class AssemblyOperate
    {
        static Assembly[] assemblyArray = null;
        static object locksub = new object();
        static Dictionary<Type, List<Type>> dicBaseAndSub = new Dictionary<Type, List<Type>>();
        public static string GetDesciption(Enum enumValue)
        {
            string str = "";
            Attribute attinfo = Attribute.GetCustomAttribute((enumValue.GetType().GetField(enumValue.ToString())), typeof(DescriptionAttribute));
            Type ty = enumValue.GetType();
            FieldInfo fieldinfo = ty.GetField(enumValue.ToString());
            object[] custattri = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (custattri.Length > 0)
                str = ((DescriptionAttribute)custattri[0]).Description;
            return str;
        }
        public static string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)    //当描述属性没有时，返回名称
                return Enum.GetName(enumValue.GetType(), enumValue);
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        public static string GetDescription(Type type)
        {
            string strDefaultName = "NoDescription";
            string DescriptionString = strDefaultName;
            if (type != null)
            {
                var rep = type.GetCustomAttributes(typeof(DescriptionAttribute));
                var rep2 = rep.ToList<Attribute>();
                if (rep != null && rep2 != null)
                {
                    if (type != null)
                        strDefaultName = type.ToString();
                    DescriptionString = rep2.Count > 0 ? ((DescriptionAttribute)rep2[0]).Description : strDefaultName;
                }

            }
            return DescriptionString;
        }
        
        /// <summary>
        /// 获取 程序集中 指定继承基类（basetype）类型的 所有子类名称
        /// </summary>
        /// <param name="basetype"></param>
        /// <param name="assemblypath"></param>
        /// <returns></returns>
        public static List<string> GetAllSubClassName(Type basetype, string assemblypath)
        {
            List<string> SubClassTypeName = new List<string>();
            Assembly assembly = Assembly.LoadFile(assemblypath);
            var types = assembly.GetTypes();
            var baseType = basetype;
     
            foreach (var t in types)
            {
                var tmp = t.BaseType;
                int i = 0;
                while (tmp != null)
                {
                    if (tmp.Name == baseType.Name)
                    {
                        SubClassTypeName.Add(t.FullName);

                        break;
                    }
                    else
                    {
                        tmp = tmp.BaseType;
                    }
                }
            }
            if (SubClassTypeName.Count == 0)
                return null;
            else
                return SubClassTypeName;

        }

        /// <summary>
        /// 获取运行目录下所有 程序集中 指定继承基类（basetype）类型的 所有子类名称
        /// </summary>
        /// <param name="basetype"></param>
        /// <param name="assemblypath"></param>
        /// <returns></returns>
        public static List<string> GetAllSubClassNameOnRunDir(Type basetype)
        {
            List<string> SubClassTypeName = new List<string>();
            if (assemblyArray==null)
                assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            
            GetAllSubClassTypeOnRunDir(basetype);
            if (dicBaseAndSub.ContainsKey(basetype))
            {
                foreach (var temp in dicBaseAndSub[basetype])
                {
                    SubClassTypeName.Add(temp.ToString());
                }
                return SubClassTypeName;
            }
            return null;
  
        }
        /// <summary>
        /// 在运行目录下所有 程序集中  指定基类获取子类类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public static List<Type> GetAllSubClassTypeOnRunDir(Type basetype)
        {
            if (assemblyArray == null)
                  assemblyArray = AppDomain.CurrentDomain.GetAssemblies();

            if (dicBaseAndSub.ContainsKey(basetype))
                return dicBaseAndSub[basetype];
            
            List<Type> SubClassTypes = new List<Type>();
            for (int j = 0; j < assemblyArray.Length; j++)
            {
                Assembly assembly = assemblyArray[j];
                var types = assembly.GetTypes();
                var baseType = basetype;

                foreach (var t in types)
                {
                    var tmp = t.BaseType;
                    int i = 0;
                    while (tmp != null)
                    {
                        if (tmp.Name == baseType.Name)
                        {
                           
                                SubClassTypes.Add(t);
                            
                            break;
                        }
                        else
                        {
                            tmp = tmp.BaseType;
                        }
                    }
                }

            }
            if (SubClassTypes.Count == 0)
                return null;
            else
            {
                lock (locksub)
                {
                    dicBaseAndSub.Add(basetype, SubClassTypes);
                 }
                return SubClassTypes;
            }
               
        }
        /// <summary>
        /// 在运行目录下所有 程序集中  根据类型名字获取类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetTypeFromAssembly(string typeName)
        {

            Type type = null;
            if (assemblyArray == null)
                 assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
        /// <summary>
        /// 在运行目录下所有程序集中 根据描述找类型
        /// </summary>
        /// <param name="strDescription"></param>
        /// <returns></returns>
        public static Type GetTypeFromAssemblyByDescrition(string strDescription)
        {
            Type type = null;
            if (assemblyArray == null)
                 assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
               Assembly assembly = assemblyArray[i];
               var types = assembly.GetTypes();
               for( int j=0;j< types.Length;j++)
                {
                   if(strDescription == GetDescription(types[j]))
                    {
                        return types[j];
                    }
                }        
            }
            return null;
        }
        /// <summary>
        /// 在运行目录下所有程序集中 指定描述和基类 找类型
        /// </summary>
        /// <param name="strDescription"></param>
        /// <returns></returns>
        public static Type GetTypeFromAssemblyByDescrition(string strDescription  , Type typeBase)
        {
            Type type = null;
            if (assemblyArray == null)
                 assemblyArray = AppDomain.CurrentDomain.GetAssemblies();

            GetAllSubClassTypeOnRunDir(typeBase);
            if (dicBaseAndSub.ContainsKey(typeBase))
            {
                foreach (var temp in dicBaseAndSub[typeBase])
                {
                    if (strDescription == GetDescription(temp))
                    {
                        return temp;
                    }
                }
               
            }
            return null;
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                Assembly assembly = assemblyArray[i];
                var types = assembly.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (strDescription == GetDescription(types[j]))
                    {
                        var tmp = types[j].BaseType;
                        while (tmp != null)
                        {
                            if (tmp.Name == typeBase.Name)
                            {
                                return types[j];
                            }
                            else
                            {
                                tmp = tmp.BaseType;
                            }
                        }
                    }
                }
            }
            return null;
        }



    }
}