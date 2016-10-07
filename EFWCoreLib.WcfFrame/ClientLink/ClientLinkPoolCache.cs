using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame
{
    /// <summary>
    /// 连接池缓存
    /// </summary>
    public class ClientLinkPoolCache
    {
        /// <summary>
        /// Wcf连接池
        /// </summary>
        volatile static IDictionary<string, ClientLinkPool> poolDic = new Dictionary<string, ClientLinkPool>();
        volatile static object lockpool = new object();
        /// <summary>
        /// 监控线程
        /// </summary>
        volatile static IDictionary<string, Thread> thDic = new Dictionary<string, Thread>();
        volatile static object lockth = new object();

        /// <summary>
        /// 初始化连接池
        /// </summary>
        /// <param name="isUseWcfPool">是否使用连接池</param>
        /// <param name="wcfMaxPoolSize">池子最大值</param>
        /// <param name="wcfOutTime">获取连接超时时间</param>
        /// <param name="WcfFailureTime">连接池回收时间</param>
        /// <param name="server_name">服务器名</param>
        public static void Init(bool isUseWcfPool, int wcfMaxPoolSize, long wcfOutTime, long WcfFailureTime, string server_name, int WcfPoolMonitorReapTime)
        {
            //装在连接池
            if (isUseWcfPool && !poolDic.ContainsKey(server_name))
            {
                lock (lockpool)
                {
                    if (isUseWcfPool && !poolDic.ContainsKey(server_name))
                    {
                        ClientLinkPool pool = new ClientLinkPool(wcfMaxPoolSize, wcfOutTime, WcfFailureTime, WcfPoolMonitorReapTime);
                        poolDic.Add(server_name, pool);
                    }
                }
            }
            //开启监控线程
            if (isUseWcfPool && !thDic.ContainsKey(server_name))
            {
                lock (lockth)
                {
                    if (!thDic.ContainsKey(server_name))
                    {
                        Thread poolMonitorTh = new Thread(poolDic[server_name].MonitorExec);
                        poolMonitorTh.Start();
                        thDic.Add(server_name, poolMonitorTh);
                    }
                }
            }
        }

        /// <summary>
        /// 获取连接池
        /// </summary>
        /// <param name="server_name"></param>
        /// <returns></returns>
        public static ClientLinkPool GetClientPool(string server_name)
        {
            if (poolDic.ContainsKey(server_name) == false)
                return null;
            return poolDic[server_name];
        }

        public static void Dispose()
        {
            foreach(ClientLinkPool p in poolDic.Values)
            {
                p.ClearPool();
            }

            foreach(Thread t in thDic.Values)
            {
                t.Abort();
            }
        }

    }
}
