using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.ServerManage;
using EFWCoreLib.WcfFrame.Utility.Upgrade;

namespace EFWCoreLib.WcfFrame.ServerManage
{
    /// <summary>
    /// 发布订阅处理
    /// </summary>
    public class PublishSubManager
    {
        public static string publishSubscibefile = System.Windows.Forms.Application.StartupPath + "\\Config\\PublishSubscibe.xml";
        public static List<PublishServiceObject> psoList;
        public static List<PublishSubService> psubserviceList;
        /// <summary>
        /// 开启客户端订阅
        /// </summary>
        public static void StartPublish()
        {
            psoList = new List<PublishServiceObject>();
            psubserviceList = new List<PublishSubService>();

            if (ServerManage.SuperClient.superclient != null)
                psoList = ServerManage.SuperClient.superclient.GetPublishServiceList();

            if (psoList != null)
            {
                Dictionary<string, bool> _subDic = LoadXML();
                foreach (var item in psoList)
                {
                    PublishSubService ps = new PublishSubService();
                    ps.whether = _subDic.ContainsKey(item.publishServiceName) ? _subDic[item.publishServiceName] : false;
                    ps.publishServiceName = item.publishServiceName;
                    ps.explain = item.explain;
                    ps.IsSub = false;
                    psubserviceList.Add(ps);
                }
            }

            //开始订阅
            foreach(var i in psubserviceList)
            {
                if (i.whether)
                {
                    ServerManage.SuperClient.superclient.Subscribe(i.publishServiceName);
                    i.IsSub = true;
                }
            }
        }

        /// <summary>
        /// 获取中心的发布服务
        /// </summary>
        /// <returns></returns>
        public static List<PublishSubService> GetCenterPublishService()
        {
            return psubserviceList;
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
                    List<CacheIdentify> ciList = DistributedCacheClient.GetCacheIdentifyList();
                    List<CacheObject> coList = _clientLink.GetDistributedCacheData(ciList);
                    if (coList.Count > 0)
                    {
                        DistributedCacheClient.SetCacheObjectList(coList);
                    }
                    break;
                case "RemotePlugin"://远程插件服务
                    LocalPlugin localPlugin = RemotePluginClient.GetLocalPlugin();
                    if (localPlugin.PluginDic.Count > 0)
                        _clientLink.RegisterRemotePlugin(WcfGlobal.Identify, localPlugin.PluginDic.Keys.ToArray());
                    break;
                case "UpgradeClient"://客户端升级
                    ClientUpgradeManager.DownLoadUpgrade();
                    break;
                case "UpgradeServer"://中间件升级
                    break;
                case "MongodbSync"://同步Mongodb数据
                    break;
                case "MiddlewareMonitor"://中间件集群监控服务
                    break;
                case "MiddlewareCmd"://中间件命令服务
                    break;
                default:
                    PublishServiceObject pso = psoList.Find(x => x.publishServiceName == publishServiceName);
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

        private static Dictionary<string, bool> LoadXML()
        {
            Dictionary<string, bool> _subDic = new Dictionary<string, bool>();
            try
            {
                XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load(publishSubscibefile);

                XmlNodeList subservicelist = xmlDoc.DocumentElement.SelectNodes("Subscibe/service");
                foreach (XmlNode xe in subservicelist)
                {
                    _subDic.Add(xe.Attributes["servicename"].Value, xe.Attributes["switch"].Value == "1" ? true : false);
                }
            }
            catch (Exception e)
            {
                MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, "加载定时任务配置文件错误！");
                MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, e.Message);
            }

            return _subDic;
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
        /// 是否启用
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
        /// <summary>
        /// 是否订阅
        /// </summary>
        public bool IsSub { get; set; }
        /// <summary>
        /// 订阅状态
        /// </summary>
        public string SubState
        {
            get
            {
                if (IsSub)
                    return "已订阅";
                else
                    return "未订阅";
            }
        }
    }
}
