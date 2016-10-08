using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame
{
    /// <summary>
    /// 连接池
    /// </summary>
    public class ClientLinkPool
    {
        /// <summary>
        /// 通讯实体列表
        /// </summary>
        private List<ClientLink> poollist = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="WcfMaxPoolSize">wcf池子最大数</param>
        /// <param name="WcfTimeOut">wcf获取连接超时时间，以秒为单位</param>
        public ClientLinkPool(int WcfMaxPoolSize, long WcfOutTime, long WcfFailureTime, int WcfPoolMonitorReapTime)
        {
            this.wcfMaxPoolSize = WcfMaxPoolSize;
            this.wcfOutTime = new TimeSpan(WcfOutTime * 1000 * 10000);
            this.wcfFailureTime = new TimeSpan(WcfFailureTime * 1000 * 10000);
            this.monitorTimeSpan = WcfPoolMonitorReapTime;
            poollist = new List<ClientLink>(this.wcfMaxPoolSize);
            usedNumsDic = new Dictionary<string, int>();
            countNumsDic = new Dictionary<string, int>();
            OpeningNumDic = new Dictionary<string, int>();
        }

        ~ClientLinkPool()
        {
            ClearPool();
        }

        /// <summary>
        /// Wcf连接池最大值，默认为100
        /// </summary>
        int wcfMaxPoolSize = 100;

        /// <summary>
        /// Wcf获取连接过期时间(默认一分钟)
        /// </summary>
        TimeSpan wcfOutTime = new TimeSpan((long)60 * 1000 * 10000);

        /// <summary>
        /// Wcf连接失效时间(默认一分钟)
        /// </summary>
        TimeSpan wcfFailureTime = new TimeSpan((long)60 * 1000 * 10000);

        /// <summary>
        /// 监控时间间隔（单位：秒）
        /// </summary>
        int monitorTimeSpan = 30;

        /// <summary>
        /// 监控逻辑
        /// </summary>
        public void MonitorExec()
        {
            while (true)
            {
                //write("F:\\1.txt", "monitor");
                Thread.Sleep(monitorTimeSpan * 1000);
                try
                {
                    ReapPool();
                }
                catch { }
            }
        }

        /// <summary>
        /// 清空连接池
        /// </summary>
        public void ClearPool()
        {
            if (this.poollist == null) return;
            lock (lockhelper)
            {
                foreach (ClientLink obj in this.poollist)
                {
                    try
                    {
                        //obj.Scope.Dispose();
                        obj.Dispose();
                    }
                    catch { }
                }
                poollist.Clear();
                poollist = null;
                countNumsDic.Clear();
                countNumsDic = null;
                usedNumsDic.Clear();
                usedNumsDic = null;
                index = 0;
            }
        }

        /// <summary>
        /// 当前正在使用的池子数量
        /// </summary>
        public int GetUsedPoolNums(string pluginname)
        {
            if (usedNumsDic.ContainsKey(pluginname))
            {
                return usedNumsDic[pluginname];
            }
            return 0;
        }

        /// <summary>
        /// 当前空闲池子数量
        /// </summary>
        /// <param name="pluginname"></param>
        /// <returns></returns>
        public int GetFreePoolNums(string pluginname)
        {
            return GetCountPoolNums(pluginname) - GetUsedPoolNums(pluginname);
        }

        /// <summary>
        /// 判断非当前契约是否有空闲池子
        /// </summary>
        /// <param name="pluginname"></param>
        /// <returns></returns>
        public bool GetFreePoolNumsNotCurrent(string pluginname)
        {
            bool flag = false;
            foreach (ClientLink obj in poollist)
            {
                if (!obj.IsUsed && obj.PluginName != pluginname)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        /// <summary>
        /// 获取池子总数
        /// </summary>
        /// <param name="pluginname"></param>
        /// <returns></returns>
        public int GetCountPoolNums(string pluginname)
        {
            if (countNumsDic.ContainsKey(pluginname))
            {
                return countNumsDic[pluginname];
            }
            return 0;
        }
        /// <summary>
        /// 获取连接池正在打开的连接数
        /// </summary>
        /// <param name="pluginname"></param>
        /// <returns></returns>
        public int GetOpeningNums(string pluginname)
        {
            if (OpeningNumDic.ContainsKey(pluginname))
            {
                return OpeningNumDic[pluginname];
            }
            return 0;
        }

        /// <summary>
        /// 当前池子连接总数
        /// </summary>
        public int CurrentPoolNums
        {
            get
            {
                return poollist.Count;
            }
        }

        /// <summary>
        /// 判断池子是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsPoolFull
        {
            get
            {
                return poollist.Count >= this.wcfMaxPoolSize;
            }
        }


        private object lockhelper = new object();
        /// <summary>
        /// 索引
        /// </summary>
        private int index = 0;
        /// <summary>
        /// 已经使用的数量
        /// </summary>
        private IDictionary<string, int> usedNumsDic = null;
        /// <summary>
        /// 池子里个类型连接总数
        /// </summary>
        private IDictionary<string, int> countNumsDic = null;

        /// <summary>
        /// 打开连接中的数量
        /// </summary>
        private IDictionary<string, int> OpeningNumDic = null;

        /// <summary>
        /// 处理连接池
        /// </summary>
        private void ReapPool()
        {
            lock (lockhelper)
            {
                //string content = "";
                for (int i = 0; i < poollist.Count; i++)
                {
                    ClientLink obj = poollist[i];
                    if ((!obj.IsUsed && DateTime.Now - obj.LastUsedTime > this.wcfFailureTime) || (obj.State != CommunicationState.Opened))
                    {
                        obj.Dispose();
                        poollist.Remove(obj);
                        if (countNumsDic.ContainsKey(obj.PluginName))
                            countNumsDic[obj.PluginName] = countNumsDic[obj.PluginName] == 0 ? 0 : countNumsDic[obj.PluginName] - 1;
                        if (usedNumsDic.ContainsKey(obj.PluginName))
                            usedNumsDic[obj.PluginName] = usedNumsDic[obj.PluginName] == 0 ? 0 : usedNumsDic[obj.PluginName] - 1;

                        i--;
                    }
                }
                //write("F:\\2.txt", content);
            }
        }

        /// <summary>
        /// 加入连接池
        /// </summary>
        /// <param name="clientlink">连接</param>
        /// <param name="Index">返回的连接池索引</param>
        /// <returns></returns>
        public bool AddPool(string pluginname, out ClientLink clientlink, out int? Index)
        {
            //做一次清理
            //if (isReap)
            //    ReapPool();

            bool flag = false;
            Index = null;
            clientlink = null;

            if (poollist.Count < this.wcfMaxPoolSize)
            {
 
                lock (lockhelper)
                {
                    OpeningNumDic[pluginname] = OpeningNumDic.ContainsKey(pluginname) ? OpeningNumDic[pluginname] + 1 : 0;

                    if (poollist.Count < this.wcfMaxPoolSize)
                    {
                        clientlink = new ClientLink(pluginname);
                        
                        index = index >= Int32.MaxValue ? 1 : index + 1;
                        Index = index;
                        clientlink.Index = index;
                        clientlink.UsedNums = 1;
                        clientlink.CreatedTime = DateTime.Now;
                        clientlink.LastUsedTime = DateTime.Now;
                        clientlink.IsUsed = true;
                        //obj.Scope = new OperationContextScope(((IClientChannel)channel));
                        poollist.Add(clientlink);
                        countNumsDic[clientlink.PluginName] = countNumsDic.ContainsKey(clientlink.PluginName) ? countNumsDic[clientlink.PluginName] + 1 : 1;
                        usedNumsDic[clientlink.PluginName] = usedNumsDic.ContainsKey(clientlink.PluginName) ? usedNumsDic[clientlink.PluginName] + 1 : 1;
                        flag = true;
                        clientlink.CreateConnection();
                    }

                    OpeningNumDic[pluginname] = OpeningNumDic.ContainsKey(pluginname) ? OpeningNumDic[pluginname] - 1 : 0;
                }
 
            }
            return flag;
        }

        /// <summary>
        /// 从连接池中获取一个连接
        /// </summary>
        /// <returns></returns>
        public ClientLink GetClientLink(string pluginname)
        {
            //先做一次清理
            //ReapPool();

            ClientLink clientlink = null;

            if (GetFreePoolNums(pluginname) > 0)
            {
                lock (lockhelper)
                {
                    if (GetFreePoolNums(pluginname) > 0)
                    {
                        for (int i = 0; i < poollist.Count; i++)
                        {
                            ClientLink obj = poollist[i];
                            if (!obj.IsUsed && DateTime.Now - obj.LastUsedTime < this.wcfFailureTime && pluginname == obj.PluginName)
                            {
                                if (obj.State == CommunicationState.Opened)
                                {
                                    obj.IsUsed = true;
                                    obj.UsedNums++;
                                    obj.LastUsedTime = DateTime.Now;
                                    usedNumsDic[obj.PluginName] = usedNumsDic.ContainsKey(obj.PluginName) ? usedNumsDic[obj.PluginName] + 1 : 1;

                                    clientlink = obj;
                                    break;
                                }
                                else//如果当前连接无效，则清理出连接池
                                {
                                    obj.Dispose();
                                    poollist.Remove(obj);
                                    if (countNumsDic.ContainsKey(obj.PluginName))
                                        countNumsDic[obj.PluginName] = countNumsDic[obj.PluginName] == 0 ? 0 : countNumsDic[obj.PluginName] - 1;
                                    if (usedNumsDic.ContainsKey(obj.PluginName))
                                        usedNumsDic[obj.PluginName] = usedNumsDic[obj.PluginName] == 0 ? 0 : usedNumsDic[obj.PluginName] - 1;
                                    i--;
                                }
                            }
                        }
                    }
                }
            }

            return clientlink;
        }

        /// <summary>
        /// 把连接放回池子
        /// </summary>
        public void ReturnPool(string pluginname, int Index)
        {
            lock (lockhelper)
            {
                foreach (ClientLink obj in poollist)
                {
                    if (Index == obj.Index && pluginname == obj.PluginName)
                    {
                        obj.IsUsed = false;
                        obj.LastUsedTime = DateTime.Now;
                        if (usedNumsDic.ContainsKey(obj.PluginName))
                            usedNumsDic[obj.PluginName] = usedNumsDic[obj.PluginName] == 0 ? 0 : usedNumsDic[obj.PluginName] - 1;
                        break;
                    }
                }
            }

            //做一次清理
            //ReapPool();
        }

        /// <summary>
        /// 移除索引的连接
        /// </summary>
        /// <param name="pluginname">插件名称</param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public bool RemovePoolAt(string pluginname, int Index)
        {
            bool flag = false;
            lock (lockhelper)
            {
                int len = poollist.Count;
                for (int i = 0; i < poollist.Count; i++)
                {
                    ClientLink obj = poollist[i];
                    if (Index == obj.Index && pluginname == obj.PluginName)
                    {

                        obj.Dispose();
                        poollist.Remove(obj);
                        if (countNumsDic.ContainsKey(obj.PluginName))
                            countNumsDic[obj.PluginName] = countNumsDic[obj.PluginName] == 0 ? 0 : countNumsDic[obj.PluginName] - 1;
                        if (usedNumsDic.ContainsKey(obj.PluginName))
                            usedNumsDic[obj.PluginName] = usedNumsDic[obj.PluginName] == 0 ? 0 : usedNumsDic[obj.PluginName] - 1;

                        flag = true;
                        i--;
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 踢掉一个非当前契约的空闲连接
        /// </summary>
        /// <returns></returns>
        public bool RemovePoolOneNotAt(string pluginname, out ClientLink clientlink, out int? Index)
        {
            bool flag = false;
            Index = null;
            clientlink = null;
            lock (lockhelper)
            {
                int len = poollist.Count;
                //如果池子满了，先踢出一个非当前创建契约的连接
                if (poollist.Count >= this.wcfMaxPoolSize)
                {
                    for (int i = 0; i < poollist.Count; i++)
                    {
                        ClientLink obj = poollist[i];
                        if (!obj.IsUsed && obj.PluginName != pluginname)
                        {
                            obj.Dispose();

                            poollist.Remove(obj);
                            if (countNumsDic.ContainsKey(obj.PluginName))
                                countNumsDic[obj.PluginName] = countNumsDic[obj.PluginName] == 0 ? 0 : countNumsDic[obj.PluginName] - 1;
                            if (usedNumsDic.ContainsKey(obj.PluginName))
                                usedNumsDic[obj.PluginName] = usedNumsDic[obj.PluginName] == 0 ? 0 : usedNumsDic[obj.PluginName] - 1;

                            flag = true;
                            i--;
                            break;
                        }
                    }
                }
                //增加一个连接到池子
                if (poollist.Count < this.wcfMaxPoolSize)
                {
                    clientlink = new ClientLink(pluginname);
                    clientlink.CreateConnection();
                    index = index >= Int32.MaxValue ? 1 : index + 1;
                    Index = index;
                    clientlink.Index = index;
                    clientlink.UsedNums = 1;
                    clientlink.CreatedTime = DateTime.Now;
                    clientlink.LastUsedTime = DateTime.Now;
                    clientlink.IsUsed = true;
                    //obj.Scope = new OperationContextScope(((IClientChannel)channel));
                    poollist.Add(clientlink);
                    countNumsDic[clientlink.PluginName] = countNumsDic.ContainsKey(clientlink.PluginName) ? countNumsDic[clientlink.PluginName] + 1 : 1;
                    usedNumsDic[clientlink.PluginName] = usedNumsDic.ContainsKey(clientlink.PluginName) ? usedNumsDic[clientlink.PluginName] + 1 : 1;
                    flag = true;
                }
            }

            return flag;
        }

    }
}
