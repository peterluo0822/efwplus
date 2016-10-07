using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using EFWCoreLib.WcfFrame.ServerController;
using System.Threading;
using System.ServiceModel.Channels;
using EFWCoreLib.WcfFrame.SDMessageHeader;

namespace EFWCoreLib.WcfFrame.WcfService
{
    //InstanceContextMode.PerSession  为每一个客户端创建服务实列,但是每次只能响应会话通道的一次请求，异步请求就没意义，客户端多线程没有意义
    //InstanceContextMode.PerCall  会话的每一次请求都会创建服务对象，异步请求也支持，但是影响请求性能
    //InstanceContextMode.Single 所有会话的请求都共用一个服务对象
    //ConcurrencyMode = ConcurrencyMode.Multiple  使用这个配置可能会出现不同步问题，WcfServerManage.wcfClientDic对象出现问题
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple,UseSynchronizationContext=false, IncludeExceptionDetailInFaults = true)]
    public class WCFHandlerService : MarshalByRefObject, IWCFHandlerService
    {
        string ns = "http://www.efwplus.cn/";

        public WCFHandlerService()
        {
            //WcfServerManage.hostwcfMsg(System.Drawing.Color.Blue, DateTime.Now, "New WCFHandlerService");
        }

        #region IapiWCFHandlerService 成员

        public string CreateDomain(string ipAddress)
        {
            //客户端回调
            IClientService mCallBack = OperationContext.Current.GetCallbackChannel<IClientService>();
            HeaderParameter para = HeaderOperater.GetHeaderValue(OperationContext.Current.RequestContext.RequestMessage);
            string ClientID = WcfServerManage.CreateClient(OperationContext.Current.SessionId, ipAddress, DateTime.Now, mCallBack, para.pluginname, para.replyidentify);
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

        //服务请求
        public string ProcessRequest(string clientId, string controller, string method, string jsondata)
        {
            string pluginname = null;
            string cname = null;
            string[] names = controller.Split(new char[] { '@' });
            if (names.Length == 2)
            {
                pluginname = names[0];
                cname = names[1];
            }
            HeaderParameter para = HeaderOperater.GetHeaderValue(OperationContext.Current.RequestContext.RequestMessage);
            return WcfServerManage.ProcessRequest(clientId, pluginname, cname, method, jsondata,para);
        }

        //异步请求
        public IAsyncResult BeginProcessRequest(string clientId, string controller, string method, string jsondata, AsyncCallback callback, object asyncState)
        {
            string pluginname = null;
            string cname = null;
            string[] names = controller.Split(new char[] { '@' });
            if (names.Length == 2)
            {
                pluginname = names[0];
                cname = names[1];
            }
            HeaderParameter para = HeaderOperater.GetHeaderValue(OperationContext.Current.RequestContext.RequestMessage);
            return new CompletedAsyncResult<string>(WcfServerManage.ProcessRequest(clientId, pluginname, cname, method, jsondata,para));
        }

        public string EndProcessRequest(IAsyncResult result)
        {
            CompletedAsyncResult<string> ret = result as CompletedAsyncResult<string>;
            return ret.Data as string;
        }

        public bool UnDomain(string clientId)
        {
            //OperationContext.Current.Channel.Close();
            return WcfServerManage.UnClient(clientId);
        }

        public bool Heartbeat(string clientId)
        {
            return WcfServerManage.Heartbeat(clientId);
        }

        public void SendBroadcast(string jsondata)
        {
            WcfServerManage.SendBroadcast(jsondata);
        }

        public string ServerConfig()
        {
            return WcfServerManage.ServerConfig();
        }

        public string WcfServicesAllInfo()
        {
            return DebugWcfServices.getWcfServicesAllInfo();
        }

        public void RegisterReplyPlugin(string ServerIdentify, string[] plugin)
        {
            //客户端回调
            IClientService mCallBack = OperationContext.Current.GetCallbackChannel<IClientService>();
            WcfServerManage.RegisterReplyPlugin(mCallBack, ServerIdentify, plugin);
        }

        #endregion

        #region 分布式缓存


        public CacheIdentify DistributedCacheSyncIdentify(CacheIdentify cacheId)
        {
            return DistributedCacheManage.CompareCache(cacheId);
        }

        public void DistributedCacheSync(CacheObject cache)
        {
            DistributedCacheManage.SyncLocalCache(cache);
            //客户端回调
            IClientService mCallBack = OperationContext.Current.GetCallbackChannel<IClientService>();
            //异步执行同步缓存
            new Action<CacheObject>(delegate(CacheObject _cache)
            {
                List<WCFClientInfo> clist = WcfServerManage.wcfClientDic.Values.ToList().FindAll(x => (x.plugin == "SuperPlugin" && x.IsConnect == true));
                foreach (var client in clist)
                {
                    //排除自己给自己同步缓存
                    if (mCallBack == client.callbackClient || WcfServerManage.Identify==client.ServerIdentify)
                    {
                        continue;
                    }
                    else
                    {
                        //将上级中间件的缓存同步到下级中间件
                        client.callbackClient.DistributedCacheSync(_cache);
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

            //客户端回调
            IClientService mCallBack = OperationContext.Current.GetCallbackChannel<IClientService>();
            //异步执行同步缓存
            new Action<List<CacheObject>>(delegate(List<CacheObject> _cachelist)
            {
                List<WCFClientInfo> clist = WcfServerManage.wcfClientDic.Values.ToList().FindAll(x => (x.plugin == "SuperPlugin" && x.IsConnect == true));
                foreach (var client in clist)
                {
                    //排除自己给自己同步缓存
                    if (mCallBack == client.callbackClient || WcfServerManage.Identify == client.ServerIdentify)
                    {
                        continue;
                    }
                    else
                    {
                        //将上级中间件的缓存同步到下级中间件
                        client.callbackClient.DistributedAllCacheSync(_cachelist);
                    }
                }
            }).BeginInvoke(cachelist, null, null);
        }

        #endregion
    }

    /// <summary>
    /// 异步结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompletedAsyncResult<T> : IAsyncResult
    {
        T data;

        public CompletedAsyncResult(T data)
        {
            this.data = data;
        }

        public T Data
        { get { return data; } }

        #region IAsyncResult Members
        public object AsyncState
        { get { return (object)data; } }

        public WaitHandle AsyncWaitHandle
        { get { throw new Exception("The method or operation is not implemented."); } }

        public bool CompletedSynchronously
        { get { return true; } }

        public bool IsCompleted
        { get { return true; } }
        #endregion
    }

}
