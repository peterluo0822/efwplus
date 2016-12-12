using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.ServerManage;
using EFWCoreLib.WcfFrame.Utility.Upgrade;

namespace EFWCoreLib.WcfFrame.Utility
{
    /// <summary>
    /// 发布订阅处理
    /// </summary>
    public class PublishSubManager
    {

        public static List<PublishServiceObject> psoList;
        /// <summary>
        /// 开启客户端订阅
        /// </summary>
        public static void StartPublish()
        {
            psoList = ServerManage.SuperClient.superclient.GetPublishServiceList();
        }

        /// <summary>
        /// 获取中心的发布服务
        /// </summary>
        /// <returns></returns>
        public static List<PublishSubService> GetCenterPublishService()
        {
            List<PublishSubService> psService = new List<PublishSubService>();
            foreach(var item in psoList)
            {
                PublishSubService ps = new PublishSubService();
                ps.whether = true;
                ps.publishServiceName = item.publishServiceName;
                ps.explain = item.explain;
                psService.Add(ps);
            }
            return psService;
        }

        /// <summary>
        /// 客户端接收通知
        /// </summary>
        /// <param name="publishServiceName">订阅服务名称</param>
        /// <param name="_clientLink">客户端连接</param>
        public static void ReceiveNotify(string publishServiceName, ClientLink _clientLink)
        {
            ShowHostMsg(Color.Blue, DateTime.Now, "收到订阅的“" + publishServiceName + "”服务通知！");
            ProcessPublishService(publishServiceName, _clientLink);
        }
        /// <summary>
        /// 客户端执行订阅服务
        /// </summary>
        /// <param name="_clientLink"></param>
        public static void ProcessPublishService(string publishServiceName, ClientLink _clientLink)
        {
            switch (publishServiceName)
            {
                case "DistributedCache"://分布式缓存服务
                    List<CacheIdentify> ciList = DistributedCacheManage.GetCacheIdentifyList();
                    if (ciList.Count > 0)
                    {
                        List<CacheObject> coList = _clientLink.GetDistributedCacheData(ciList);
                        if (coList.Count > 0)
                        {
                            DistributedCacheManage.SetCacheObjectList(coList);
                        }
                    }
                    break;
                case "RemotePlugin"://远程插件服务
                    LocalPlugin localPlugin = RemotePluginManage.GetLocalPlugin();
                    if (localPlugin.PluginDic.Count > 0)
                        _clientLink.RegisterRemotePlugin(WcfGlobal.Identify, localPlugin.PluginDic.Keys.ToArray());
                    break;
                case "UpgradeClient"://客户端升级
                    ClientUpgradeManager.DownLoadUpgrade();
                    break;
                default:
                    PublishServiceObject pso= psoList.Find(x => x.publishServiceName == publishServiceName);
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, System.Drawing.Color.Blue, string.Format("正在执行服务{0}/{1}/{2}/{3}", pso.pluginname, pso.controller, pso.method, pso.argument));
                    ServiceResponseData retjson = InvokeWcfService(
                        pso.pluginname
                        , pso.controller
                        , pso.method
                        , (ClientRequestData request) =>
                        {
                            request.SetJsonData(pso.argument);
                        });
                    string txtResult = retjson.GetJsonData();
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, System.Drawing.Color.Blue, string.Format("服务执行完成，返回结果：{0}", txtResult));
                    break;
            }

            ShowHostMsg(Color.Blue, DateTime.Now, "执行“" + publishServiceName + "”订阅服务成功！");
        }


        public static void ProcessPublishService(string psname)
        {
            ProcessPublishService(psname, SuperClient.superclient);
        }

        private static void ShowHostMsg(Color clr, DateTime time, string text)
        {
            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, clr, text);
        }

        private static ServiceResponseData InvokeWcfService(string wcfpluginname, string wcfcontroller, string wcfmethod, Action<ClientRequestData> requestAction)
        {
            ClientLink wcfClientLink = ClientLinkManage.CreateConnection(wcfpluginname);
            //绑定LoginRight
            Action<ClientRequestData> _requestAction = ((ClientRequestData request) =>
            {
                request.LoginRight = new EFWCoreLib.CoreFrame.Business.SysLoginRight();
                if (requestAction != null)
                    requestAction(request);
            });
            ServiceResponseData retData = wcfClientLink.Request(wcfcontroller, wcfmethod, _requestAction);
            return retData;
        }
    }

    public class PublishSubService
    {
        /// <summary>
        /// 是否订阅
        /// </summary>
        public bool whether { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string publishServiceName { get; set; }
        /// <summary>
        /// 服务说明
        /// </summary>
        public string explain { get; set; }
    }
}
