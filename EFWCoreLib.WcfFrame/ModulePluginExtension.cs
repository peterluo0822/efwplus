using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.DataSerialize;

namespace EFWCoreLib.CoreFrame.Plugin
{
    /// <summary>
    /// 插件扩展类
    /// </summary>
    public static class ModulePluginExtension
    {
        /// <summary>
        /// 执行WCF服务，返回结果
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="controllername"></param>
        /// <param name="methodname"></param>
        /// <param name="paramValue"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static Object WcfServerExecuteMethod(this ModulePlugin mp, string controllername, string methodname, object[] paramValue, ClientRequestData requestData)
        {
            if(mp.helper==null)
                mp.helper= new WcfFrame.ServerController.ControllerHelper();
            EFWCoreLib.WcfFrame.ServerController.WcfServerController wscontroller = mp.helper.CreateController(mp.plugin.name, controllername) as EFWCoreLib.WcfFrame.ServerController.WcfServerController;
            wscontroller.requestData = requestData;
            wscontroller.responseData = new ServiceResponseData(requestData.Iscompressjson, requestData.Isencryptionjson, requestData.Serializetype);
            wscontroller.BindLoginRight(requestData.LoginRight);
            MethodInfo methodinfo = mp.helper.CreateMethodInfo(mp.plugin.name, controllername, methodname, wscontroller);
            return methodinfo.Invoke(wscontroller, paramValue);
        }
    }
}
