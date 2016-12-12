using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.SDMessageHeader;
using EFWCoreLib.WcfFrame.ServerManage;

namespace EFWCoreLib.WcfFrame.WcfHandler
{
    /// <summary>
    /// 基础服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, ValidateMustUnderstand = false, IncludeExceptionDetailInFaults = false)]
    public class BaseService : IClientHandler, IHttpDataHandler
    {
        //string ns = "http://www.efwplus.cn/";
        public string CreateClient(string clientName)
        {
            //客户端回调
            IDataReply mCallBack = OperationContext.Current.GetCallbackChannel<IDataReply>();
            HeaderParameter para = HeaderOperater.GetHeaderValue(OperationContext.Current.RequestContext.RequestMessage);
            string ClientID = ClientManage.CreateClient(clientName, DateTime.Now, mCallBack, para.pluginname, para.replyidentify);
            return ClientID;
        }

        public bool Heartbeat(string clientId)
        {
            return ClientManage.Heartbeat(clientId);
        }

        public bool UnClient(string clientId)
        {
            return ClientManage.UnClient(clientId);
        }

        public string MiddlewareConfig()
        {
            return ClientManage.MiddlewareConfig();
        }

        public string GetAllPluginInfo()
        {
            return PluginInfoManage.GetAllPluginInfo();
        }

        public IAsyncResult BeginProcessRequest(string clientId, string plugin, string controller, string method, string jsondata, AsyncCallback callback, object asyncState)
        {
            HeaderParameter para = HeaderOperater.GetHeaderValue(OperationContext.Current.RequestContext.RequestMessage);
            return new CompletedAsyncResult<string>(DataManage.ProcessRequest(clientId, plugin, controller, method, jsondata, para));
        }

        public string EndProcessRequest(IAsyncResult result)
        {
            CompletedAsyncResult<string> ret = result as CompletedAsyncResult<string>;
            return ret.Data as string;
        }

        public string ProcessRequest(string clientId, string plugin, string controller, string method, string jsondata)
        {
            HeaderParameter para = HeaderOperater.GetHeaderValue(OperationContext.Current.RequestContext.RequestMessage);
            return DataManage.ProcessRequest(clientId, plugin, controller, method, jsondata, para);
        }

        public string ProcessHttpRequest(string token, string plugin, string controller, string method, string jsondata)
        {
            throw new NotImplementedException();
        }

        public List<CacheObject> GetDistributedCacheData(List<CacheIdentify> cacheIdList)
        {
            return DistributedCacheManage.GetCacheObjectList(cacheIdList);
        }

        public void RegisterRemotePlugin(string ServerIdentify, string[] plugin)
        {
            //客户端回调
            IDataReply callback = OperationContext.Current.GetCallbackChannel<IDataReply>();
            RemotePluginManage.RegisterRemotePlugin(callback, ServerIdentify, plugin);
        }

        public void Subscribe(string ServerIdentify, string clientId, string publishServiceName)
        {
            IDataReply callback = OperationContext.Current.GetCallbackChannel<IDataReply>();
            PublishServiceManage.Subscribe(ServerIdentify,clientId, publishServiceName, callback);
        }

        public void UnSubscribe(string clientId, string publishServiceName)
        {
            PublishServiceManage.UnSubscribe(clientId, publishServiceName);
        }

        public List<PublishServiceObject> GetPublishServiceList()
        {
            return PublishServiceManage.GetPublishServiceList();
        }
    }
}
