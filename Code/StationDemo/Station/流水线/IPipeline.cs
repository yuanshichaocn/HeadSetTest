using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserData;

namespace StationDemo.DemoByWHJ
{
    /// <summary>
    /// 流水线接口
    /// </summary>
    public interface IPipeline
    {
    
        /// <summary>
        /// 获得当前状态，建议使用枚举类型
        /// </summary>
        LineSegementState State(string name);
        /// <summary>
        /// 工站放好一个产品后，调用此函数以通知流水线
        /// </summary>
        bool WorkEnd(string name);
        /// <summary>
        /// 往前流一站
        /// </summary>
        bool ToBefore(string name);
        /// <summary>
        /// 是否可以抛料
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CanThrow(string name);
       
    }
}
