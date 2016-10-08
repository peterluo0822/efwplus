using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.WcfFrame.SDMessageHeader;
using EFWCoreLib.WcfFrame.DataSerialize;

namespace EFWCoreLib.WcfFrame.WcfService.Contract
{
    /// <summary>
    /// WCF处理服务
    /// </summary>
    [ServiceKnownType(typeof(DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/",Name = "WCFHandlerService", SessionMode = SessionMode.Required, CallbackContract = typeof(IClientService))]
    public interface IWCFHandlerService
    {
        /// <summary>
        /// 创建客户端运行环境
        /// </summary>
        /// <returns>返回clientId</returns>
        [OperationContract(IsOneWay = false)]
        string CreateDomain(string ipAddress);
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="controller">控制器</param>
        /// <param name="method">方法</param>
        /// <param name="jsondata">参数</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        string ProcessRequest(string clientId, string controller, string method, string jsondata);
        /// <summary>
        /// 开始异步请求
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="controller"></param>
        /// <param name="method"></param>
        /// <param name="jsondata"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false,AsyncPattern = true)]
        IAsyncResult BeginProcessRequest(string clientId, string controller, string method, string jsondata, AsyncCallback callback, object asyncState);

        /// <summary>
        /// 结束异步请求
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        //[OperationContract(IsOneWay = false)]
        string EndProcessRequest(IAsyncResult result);

        /// <summary>
        /// 卸载制定客户端环境
        /// </summary>
        /// <param name="clientId"></param>
        [OperationContract(IsOneWay = false)]
        bool UnDomain(string clientId);

        /// <summary>
        /// WCF心跳检测
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        bool Heartbeat(string clientId);
        /// <summary>
        /// 发送广播消息
        /// </summary>
        /// <param name="jsondata"></param>
        [OperationContract(IsOneWay = true)]
        void SendBroadcast(string jsondata);

        /// <summary>
        /// 返回服务端配置
        /// </summary>
        [OperationContract(IsOneWay = false)]
        string ServerConfig();

        /// <summary>
        /// 返回所有WCF服务的配置信息，包括插件名称、控制器名称、方法名称
        /// </summary>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        string WcfServicesAllInfo();

        /// <summary>
        /// 注册远程插件
        /// </summary>
        [OperationContract]
        void RegisterReplyPlugin(string ServerIdentify, string[] plugin);

        /// <summary>
        /// 分布式缓存同步标识
        /// </summary>
        [OperationContract(IsOneWay=false)]
        CacheIdentify DistributedCacheSyncIdentify(CacheIdentify cacheId);

        /// <summary>
        /// 分布式缓存同步
        /// </summary>
        /// <param name="data"></param>
        [OperationContract]
        void DistributedCacheSync(CacheObject cache);

        /// <summary>
        /// 分布式缓存同步
        /// </summary>
        [OperationContract]
        void DistributedAllCacheSync(List<CacheObject> cachelist);
    }

    /// <summary>
    /// 回调契约
    /// </summary>
    [ServiceKnownType(typeof(System.DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "ClientService", SessionMode = SessionMode.Required)]
    public interface IClientService
    {
        /// <summary>
        /// 回调客户端
        /// </summary>
        /// <param name="jsondata">回调数据</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        void ReplyClient(string jsondata);

        /// <summary>
        /// 超级回调中间件
        /// </summary>
        /// <param name="replyidentify">回调中间件唯一标识</param>
        /// <param name="plugin"></param>
        /// <param name="controller"></param>
        /// <param name="method"></param>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        string SuperReplyClient(HeaderParameter para, string plugin, string controller, string method, string jsondata);

        /// <summary>
        /// 分布式缓存同步标识
        /// </summary>
        [OperationContract(IsOneWay = false)]
        CacheIdentify DistributedCacheSyncIdentify(CacheIdentify cacheId);

        /// <summary>
        /// 分布式缓存同步
        /// </summary>
        /// <param name="cache"></param>
        [OperationContract]
        void DistributedCacheSync(CacheObject cache);

        /// <summary>
        /// 分布式缓存同步
        /// </summary>
        [OperationContract]
        void DistributedAllCacheSync(List<CacheObject> cachelist);
    }
}
