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
            if (para.pluginname == "SuperPlugin")
            {
                //异步执行同步缓存
                new Action(delegate ()
                {
                    //创建连接时候会将上级中间件的缓存同步到下级中间件
                    DistributedCacheManage.SyncAllCache(mCallBack);

                }).BeginInvoke(null, null);
            }
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

        public CacheIdentify DistributedCacheSyncIdentify(CacheIdentify cacheId)
        {
            return DistributedCacheManage.CompareCache(cacheId);
        }

        public void DistributedCacheSync(CacheObject cache)
        {
            DistributedCacheManage.SyncLocalCache(cache);
            //异步执行同步缓存
            new Action<CacheObject>(delegate (CacheObject _cache)
            {
                List<ClientInfo> clist = ClientManage.ClientDic.Values.ToList().FindAll(x => (x.plugin == "SuperPlugin" && x.IsConnect == true));
                foreach (var client in clist)
                {
                    //排除自己给自己同步缓存
                    if (WcfGlobal.Identify == client.ServerIdentify)
                    {
                        continue;
                    }
                    else
                    {
                        //将上级中间件的缓存同步到下级中间件
                        client.dataReply.DistributedCacheSync(_cache);
                    }
                }
            }).BeginInvoke(cache, null, null);
        }

        public void DistributedAllCacheSync(List<CacheObject> cachelist)
        {
            foreach (var cache in cachelist)
            {
                DistributedCacheManage.SyncLocalCache(cache);
            }

            //异步执行同步缓存
            new Action<List<CacheObject>>(delegate (List<CacheObject> _cachelist)
            {
                List<ClientInfo> clist = ClientManage.ClientDic.Values.ToList().FindAll(x => (x.plugin == "SuperPlugin" && x.IsConnect == true));
                foreach (var client in clist)
                {
                    //排除自己给自己同步缓存
                    if (WcfGlobal.Identify == client.ServerIdentify)
                    {
                        continue;
                    }
                    else
                    {
                        //将上级中间件的缓存同步到下级中间件
                        client.dataReply.DistributedAllCacheSync(_cachelist);
                    }
                }
            }).BeginInvoke(cachelist, null, null);
        }

        public void RegisterRemotePlugin(string ServerIdentify, string[] plugin)
        {
            //客户端回调
            IDataReply callback = OperationContext.Current.GetCallbackChannel<IDataReply>();
            RemotePluginManage.RegisterRemotePlugin(callback, ServerIdentify, plugin);
        }

       
    }
}
