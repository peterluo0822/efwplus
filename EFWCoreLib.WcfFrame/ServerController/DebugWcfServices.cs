using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using Newtonsoft.Json;

namespace EFWCoreLib.WcfFrame.ServerController
{
    /// <summary>
    /// 调试服务
    /// </summary>
    public class DebugWcfServices
    {
        /// <summary>
        /// 获取所有的服务信息
        /// </summary>
        /// <returns></returns>
        public static string getWcfServicesAllInfo()
        {
            List<dwPlugin> pluginlist=new List<dwPlugin>();
            foreach (var item in WcfServerManage.localPlugin.PluginDic)
            {
                dwPlugin p = new dwPlugin();
                p.pluginname = item.Key;
                p.controllerlist = new List<dwController>();
                List<WcfControllerAttributeInfo> cmdControllerList = (List<WcfControllerAttributeInfo>)item.Value.cache.GetData(item.Key + "@" + "wcfControllerAttributeList");
                foreach (var cmd in cmdControllerList)
                {
                    dwController c = new dwController();
                    c.controllername = cmd.controllerName;
                    c.methodlist = new List<string>();
                    foreach (var m in cmd.MethodList)
                    {
                        c.methodlist.Add(m.methodName);
                    }
                    p.controllerlist.Add(c);
                }
                pluginlist.Add(p);
            }

            return JsonConvert.SerializeObject(pluginlist);
        }
    }
    /// <summary>
    /// 服务插件对象
    /// </summary>
    public class dwPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string pluginname { get; set; }
        /// <summary>
        /// 插件内的控制器集合
        /// </summary>
        public List<dwController> controllerlist { get; set; }

    }
    /// <summary>
    /// 服务控制器对象
    /// </summary>
    public class dwController
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string controllername { get; set; }
        /// <summary>
        /// 控制器内的方法集合
        /// </summary>
        public List<string> methodlist { get; set; }
    }
}
