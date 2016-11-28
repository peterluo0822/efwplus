using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.WcfHandler;

namespace EFWCoreLib.WcfFrame.ClientProxy
{
    public class BaseServiceClient : ClientBase<IClientHandler>, IClientHandler
    {
        public BaseServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        { }

        public IAsyncResult BeginProcessRequest(string clientId, string plugin, string controller, string method, string jsondata, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginProcessRequest(clientId, plugin, controller, method, jsondata, callback, asyncState);
        }

        public string EndProcessRequest(IAsyncResult result)
        {
            return this.Channel.EndProcessRequest(result);
        }

        public string CreateClient(string clientName)
        {
            return this.Channel.CreateClient(clientName);
        }

        

        public string GetAllPluginInfo()
        {
            return this.Channel.GetAllPluginInfo();
        }

        public List<CacheObject> GetDistributedCacheData(List<CacheIdentify> cacheIdList)
        {
            return this.Channel.GetDistributedCacheData(cacheIdList);
        }

        public bool Heartbeat(string clientId)
        {
            return this.Channel.Heartbeat(clientId);
        }

        public string MiddlewareConfig()
        {
            return this.Channel.MiddlewareConfig();
        }

        public string ProcessRequest(string clientId, string plugin, string controller, string method, string jsondata)
        {
            return this.Channel.ProcessRequest(clientId, plugin, controller, method, jsondata);
        }

        public void RegisterRemotePlugin(string ServerIdentify, string[] plugin)
        {
            this.Channel.RegisterRemotePlugin(ServerIdentify, plugin);
        }

        public void Subscribe(string ServerIdentify, string clientId, string publishServiceName)
        {
            this.Channel.Subscribe(ServerIdentify, clientId, publishServiceName);
        }

        public bool UnClient(string clientId)
        {
            return this.Channel.UnClient(clientId);
        }

        public void UnSubscribe(string clientId, string publishServiceName)
        {
            this.Channel.UnSubscribe(clientId, publishServiceName);
        }
    }

    public class DuplexBaseServiceClient : DuplexClientBase<IClientHandler>, IClientHandler
    {
        public DuplexBaseServiceClient(object callbackInstance, string endpointConfigurationName) : base(callbackInstance, endpointConfigurationName)
        {

        }
        public IAsyncResult BeginProcessRequest(string clientId, string plugin, string controller, string method, string jsondata, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginProcessRequest(clientId, plugin, controller, method, jsondata, callback, asyncState);
        }

        public string EndProcessRequest(IAsyncResult result)
        {
            return this.Channel.EndProcessRequest(result);
        }

        public string CreateClient(string clientName)
        {
            return this.Channel.CreateClient(clientName);
        }



        public string GetAllPluginInfo()
        {
            return this.Channel.GetAllPluginInfo();
        }

        public List<CacheObject> GetDistributedCacheData(List<CacheIdentify> cacheIdList)
        {
            return this.Channel.GetDistributedCacheData(cacheIdList);
        }

        public bool Heartbeat(string clientId)
        {
            return this.Channel.Heartbeat(clientId);
        }

        public string MiddlewareConfig()
        {
            return this.Channel.MiddlewareConfig();
        }

        public string ProcessRequest(string clientId, string plugin, string controller, string method, string jsondata)
        {
            return this.Channel.ProcessRequest(clientId, plugin, controller, method, jsondata);
        }

        public void RegisterRemotePlugin(string ServerIdentify, string[] plugin)
        {
            this.Channel.RegisterRemotePlugin(ServerIdentify, plugin);
        }

        public void Subscribe(string ServerIdentify, string clientId, string publishServiceName)
        {
            this.Channel.Subscribe(ServerIdentify, clientId, publishServiceName);
        }

        public bool UnClient(string clientId)
        {
            return this.Channel.UnClient(clientId);
        }

        public void UnSubscribe(string clientId, string publishServiceName)
        {
            this.Channel.UnSubscribe(clientId, publishServiceName);
        }
    }
}
