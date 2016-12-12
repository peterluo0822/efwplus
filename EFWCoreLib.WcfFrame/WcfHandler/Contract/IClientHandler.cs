using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.SDMessageHeader;

namespace EFWCoreLib.WcfFrame.WcfHandler
{

    [ServiceKnownType(typeof(DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "BaseService", SessionMode = SessionMode.Required, CallbackContract = typeof(IDataReply))]
    public interface IClientHandler
    {
        #region 客户端连接
        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <param name="clientName">客户端名称或IP地址</param>
        /// <returns>返回clientId</returns>
        [OperationContract(IsOneWay = false)]
        string CreateClient(string clientName);
        /// <summary>
        /// 卸载指定客户端
        /// </summary>
        /// <param name="clientId"></param>
        [OperationContract(IsOneWay = false)]
        bool UnClient(string clientId);

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        bool Heartbeat(string clientId);

        /// <summary>
        /// 返回服务端配置
        /// </summary>
        [OperationContract(IsOneWay = false)]
        string MiddlewareConfig();

        /// <summary>
        /// 返回所有插件服务配置信息，包括插件名称、控制器名称、方法名称
        /// </summary>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        string GetAllPluginInfo();
        #endregion

        #region 数据请求
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="plugin">插件名</param>
        /// <param name="controller">控制器</param>
        /// <param name="method">方法</param>
        /// <param name="jsondata">参数</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        string ProcessRequest(string clientId, string plugin, string controller, string method, string jsondata);
        /// <summary>
        /// 开始异步请求
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="plugin">插件名</param>
        /// <param name="controller">控制器</param>
        /// <param name="method">方法</param>
        /// <param name="jsondata">参数</param>
        /// <param name="callback">回调</param>
        /// <param name="asyncState">异步状态</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, AsyncPattern = true)]
        IAsyncResult BeginProcessRequest(string clientId, string plugin, string controller, string method, string jsondata, AsyncCallback callback, object asyncState);

        /// <summary>
        /// 结束异步请求
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        string EndProcessRequest(IAsyncResult result);
        #endregion

        #region 注册远程插件
        /// <summary>
        /// 注册远程插件
        /// </summary>
        [OperationContract]
        void RegisterRemotePlugin(string ServerIdentify, string[] plugin);
        #endregion

        #region 分布式缓存

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="cacheIdList"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        List<CacheObject> GetDistributedCacheData(List<CacheIdentify> cacheIdList);
        #endregion

        #region 发布订阅
        /// <summary>
        /// 获取发布服务列表
        /// </summary>
        [OperationContract(IsOneWay = false)]
        List<ServerManage.PublishServiceObject> GetPublishServiceList();
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="ServerIdentify">中间件标识，判断自己不能订阅自己</param>
        /// <param name="clientId"></param>
        /// <param name="publishServiceName">发布服务名称</param>
        [OperationContract(IsOneWay = true)]
        void Subscribe(string ServerIdentify, string clientId, string publishServiceName);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="publishServiceName">发布服务名称</param>
        [OperationContract(IsOneWay = true)]
        void UnSubscribe(string clientId, string publishServiceName);
        #endregion
    }

    [ServiceKnownType(typeof(System.DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "ReplyDataCallback", SessionMode = SessionMode.Required)]
    public interface IDataReply
    {
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
        string ReplyProcessRequest(HeaderParameter para, string plugin, string controller, string method, string jsondata);


        #region 订阅通知
        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="publishServiceName">服务名称</param>
        [OperationContract(IsOneWay = true)]
        void Notify(string publishServiceName);
        #endregion
    }
}
