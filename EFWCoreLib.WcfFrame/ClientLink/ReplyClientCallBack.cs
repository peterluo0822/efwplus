using EFWCoreLib.WcfFrame.SDMessageHeader;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame
{
    /// <summary>
    /// 客户端回调对象
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ReplyClientCallBack : IClientService
    {
        /// <summary>
        /// 回调委托
        /// </summary>
        public Action<string> ReplyClientAction
        {
            get;
            set;
        }

        /// <summary>
        /// 超级回调委托
        /// </summary>
        public Func<HeaderParameter, string, string, string, string, string> SuperReplyClientAction
        {
            get;
            set;
        }

        #region IClientService 成员
        /// <summary>
        /// 回调客户端
        /// </summary>
        /// <param name="jsondata"></param>
        public void ReplyClient(string jsondata)
        {
            if (ReplyClientAction != null)
            {
                ReplyClientAction(jsondata);
            }
        }
        /// <summary>
        /// 超级回调中间件
        /// </summary>
        /// <param name="para"></param>
        /// <param name="plugin"></param>
        /// <param name="controller"></param>
        /// <param name="method"></param>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public string SuperReplyClient(HeaderParameter para, string plugin, string controller, string method, string jsondata)
        {
            if (SuperReplyClientAction != null)
            {
                return SuperReplyClientAction(para, plugin, controller, method, jsondata);
            }
            return null;
        }

        #endregion

        #region 分布式缓存同步


        public ServerController.CacheIdentify DistributedCacheSyncIdentify(ServerController.CacheIdentify cacheId)
        {
            return EFWCoreLib.WcfFrame.ServerController.DistributedCacheManage.CompareCache(cacheId);
        }

        public void DistributedCacheSync(ServerController.CacheObject cache)
        {
            EFWCoreLib.WcfFrame.ServerController.DistributedCacheManage.SyncLocalCache(cache);
        }


        public void DistributedAllCacheSync(List<ServerController.CacheObject> cachelist)
        {
            foreach (var cache in cachelist)
            {
                EFWCoreLib.WcfFrame.ServerController.DistributedCacheManage.SyncLocalCache(cache);
            }
        }

        #endregion
    }
}
