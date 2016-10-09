using EFWCoreLib.WcfFrame.SDMessageHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.WcfHandler
{

    /// <summary>
    /// 处理Http请求
    /// </summary>
    [ServiceKnownType(typeof(DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "BaseService", SessionMode = SessionMode.Required)]
    public interface IHttpDataHandler
    {
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="plugin">插件名</param>
        /// <param name="controller">控制器</param>
        /// <param name="method">方法</param>
        /// <param name="jsondata">参数</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        string ProcessHttpRequest(string token, string plugin, string controller, string method, string jsondata);
    }

}
