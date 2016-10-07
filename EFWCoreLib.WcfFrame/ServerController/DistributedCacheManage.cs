using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace EFWCoreLib.WcfFrame.ServerController
{
    /// <summary>
    /// 分布式缓存管理
    /// 1.先下级中间件同步到上级中间件，然后上级中间件触发回调所有下级中间件
    /// 2.缓存同步的时候先判断标识是否不同，然后再同步标识不同的缓存数据
    /// </summary>
    public class DistributedCacheManage
    {
        public static HostWCFMsgHandler hostwcfMsg;
        //使用企业库中的缓存对象来进行分布式缓存管理
        private static ICacheManager _localCache
        {
            get { return AppGlobal.cache; }
        }
        private static List<string> _cacheNameList = new List<string>();

        /// <summary>
        /// 设置缓存,提供给服务控制器调用
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="key"></param>
        /// <param name="value">Json字符串</param>
        public static void SetCache(string cacheName, string key, string value)
        {
            lock (_localCache)
            {
                if (_localCache.Contains(cacheName))
                {
                    CacheObject co = _localCache.GetData(cacheName) as CacheObject;

                    co.identify = DateTimeToTimestamp(DateTime.Now);
                    if (co.cacheValue.FindIndex(x => x.key.Equals(key)) == -1)
                    {
                        CacheData cd = new CacheData();
                        cd.timestamp = DateTimeToTimestamp(DateTime.Now);
                        cd.key = key;
                        cd.value = value;
                        co.cacheValue.Add(cd);
                    }
                    else
                    {
                        CacheData _cd = co.cacheValue.Find(x => x.key.Equals(key));
                        co.cacheValue.Remove(_cd);
                        CacheData cd = new CacheData();
                        cd.timestamp = DateTimeToTimestamp(DateTime.Now);
                        cd.key = key;
                        cd.value = value;
                        co.cacheValue.Add(cd);
                    }
                }
                else
                {
                    CacheObject co = new CacheObject();
                    co.ServerIdentify = WcfServerManage.Identify;
                    co.cachename = cacheName;
                    co.identify = DateTimeToTimestamp(DateTime.Now);
                    co.cacheValue = new List<CacheData>();
                    CacheData cd = new CacheData();
                    cd.timestamp = DateTimeToTimestamp(DateTime.Now);
                    cd.key = key;
                    cd.value = value;
                    co.cacheValue.Add(cd);
                    _localCache.Add(cacheName, co);
                    if (_cacheNameList.Contains(cacheName) == false)
                        _cacheNameList.Add(cacheName);
                }
            }
        }
        /// <summary>
        /// 移除缓存，提供给服务控制器调用
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="key"></param>
        public static void RemoveCache(string cacheName, string key)
        {
            lock (_localCache)
            {
                if (_localCache.Contains(cacheName))
                {
                    CacheObject co = _localCache.GetData(cacheName) as CacheObject;
                    if (co.cacheValue.FindIndex(x => x.key == key) > -1)
                    {
                        CacheData cd = co.cacheValue.Find(x => x.key == key);
                        cd.deleteflag = true;//缓存移除
                        cd.timestamp = DateTimeToTimestamp(DateTime.Now);
                        co.identify = DateTimeToTimestamp(DateTime.Now);
                    }
                }
            }
        }

        /// <summary>
        /// 同步指定缓存给上级中间件
        /// </summary>
        /// <param name="cacheName"></param>
        public static void SyncCache(string cacheName)
        {
            if (WcfServerManage.superclient != null)
            {
                CacheIdentify cacheId = GetCacheIdentify(cacheName);
                if (cacheId.keytimestamps != null && cacheId.keytimestamps.Count > 0)
                {
                    cacheId = WcfServerManage.superclient.DistributedCacheSyncIdentify(cacheId);
                }
                CacheObject cache = GetStayLocalCache(cacheId);
                if (cache.cacheValue != null && cache.cacheValue.Count > 0)
                {
                    //缓存同步到上级中间件
                    WcfServerManage.superclient.DistributedCacheSync(cache);
                    //缓存还要同步到所有下级中间件
                    DistributedCacheManage.SyncCache(cache);
                }
            }
        }
        /// <summary>
        /// 同步所有缓存给上级中间件
        /// </summary>
        /// <param name="cacheName"></param>
        public static void SyncAllCache()
        {
            List<CacheObject> cachelist = DistributedCacheManage.GetAllCache();
            if (cachelist.Count > 0)
                WcfServerManage.superclient.DistributedAllCacheSync(cachelist);
        }


        /// <summary>
        /// 同步指定缓存给所有下级中间件
        /// </summary>
        /// <param name="cache"></param>
        public static void SyncCache(CacheObject cache)
        {
            //异步执行同步缓存
            List<WCFClientInfo> clist = WcfServerManage.wcfClientDic.Values.ToList().FindAll(x => (x.plugin == "SuperPlugin" && x.IsConnect == true));
            foreach (var client in clist)
            {
                //排除自己给自己同步缓存
                if (WcfServerManage.Identify == client.ServerIdentify)
                {
                    continue;
                }
                else
                {
                    //将上级中间件的缓存同步到下级中间件
                    client.callbackClient.DistributedCacheSync(cache);
                }
            }
        }

        /// <summary>
        /// 同步所有缓存给指定下级中间件
        /// </summary>
        /// <param name="cacheName"></param>
        public static void SyncAllCache(IClientService mCallBack)
        {
            List<CacheObject> cachelist = DistributedCacheManage.GetAllCache();
            if (cachelist.Count > 0)
                mCallBack.DistributedAllCacheSync(cachelist);
        }

       


        /// <summary>
        /// 获取本地缓存的标识
        /// </summary>
        public static CacheIdentify GetCacheIdentify(string cacheName)
        {
            CacheIdentify cacheId = new CacheIdentify();
            if (_localCache.Contains(cacheName))
            {
                CacheObject co = _localCache.GetData(cacheName) as CacheObject;
                cacheId.ServerIdentify = co.ServerIdentify;
                cacheId.cachename = co.cachename;
                cacheId.identify = co.identify;
                cacheId.keytimestamps = new Dictionary<string, double>();
                foreach (var cd in co.cacheValue)
                {
                    cacheId.keytimestamps.Add(cd.key, cd.timestamp);
                }
            }
            return cacheId;
        }
        /// <summary>
        /// 比较后不同的标识
        /// </summary>
        /// <returns></returns>
        public static CacheIdentify CompareCache(CacheIdentify _cacheId)
        {

            CacheIdentify cacheId = new CacheIdentify();
            cacheId.ServerIdentify = _cacheId.ServerIdentify;
            cacheId.cachename = _cacheId.cachename;
            cacheId.identify = _cacheId.identify;
            cacheId.keytimestamps = new Dictionary<string, double>();
            //自己跟自己比较返回空
            if (_cacheId.ServerIdentify == WcfServerManage.Identify) return cacheId;

            if (_localCache.Contains(_cacheId.cachename))
            {
                CacheObject co = _localCache.GetData(_cacheId.cachename) as CacheObject;
                if (_cacheId.identify != co.identify)
                {
                    //循环判断待同步的新增和修改，本地时间搓小于远程时间搓就修改
                    foreach (var t in _cacheId.keytimestamps)
                    {
                        //新增的
                        if (co.cacheValue.FindIndex(x => (x.key == t.Key)) == -1)
                        {
                            cacheId.keytimestamps.Add(t);
                        }
                        //修改的
                        if (co.cacheValue.FindIndex(x => (x.key == t.Key && t.Value > x.timestamp)) > -1)
                        {
                            cacheId.keytimestamps.Add(t);
                        }
                    }

                    //循环判断本地的删除，本地时间搓小于远程identify的就会删除
                    //删除是打删除标记，所以不存在移除，都进入修改列表
                }
                return cacheId;
            }
            else
            {
                return _cacheId;
            }
        }
        /// <summary>
        /// 获取待同步的缓存
        /// </summary>
        /// <returns></returns>
        public static CacheObject GetStayLocalCache(CacheIdentify identify)
        {
            CacheObject _co = new CacheObject();
            if (_localCache.Contains(identify.cachename))
            {
                CacheObject co = _localCache.GetData(identify.cachename) as CacheObject;
                _co.ServerIdentify = co.ServerIdentify;
                _co.cachename = co.cachename;
                _co.identify = co.identify;
                _co.cacheValue = new List<CacheData>();
                foreach (var kt in identify.keytimestamps)
                {
                    CacheData cd = co.cacheValue.Find(x => x.timestamp == kt.Value);
                    if (cd != null)
                        _co.cacheValue.Add(cd);
                }
            }
            return _co;
        }

        public static CacheObject GetLocalCache(string cacheName)
        {
            CacheObject _co = null;
            if (_localCache.Contains(cacheName))
            {
                _co = _localCache.GetData(cacheName) as CacheObject;
            }
            return _co;
        }

        /// <summary>
        /// 同步本地缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static bool SyncLocalCache(CacheObject cache)
        {
            //自己没必要同步到自己
            if (cache.ServerIdentify == WcfServerManage.Identify) return true;
            lock (_localCache)
            {
                bool isChanged = false;
                if (_localCache.Contains(cache.cachename))
                {
                    CacheObject co = _localCache.GetData(cache.cachename) as CacheObject;
                    if (cache.identify != co.identify)
                    {
                        //循环判断待同步的新增和修改，本地时间搓小于远程时间搓就修改
                        foreach (var t in cache.cacheValue)
                        {
                            //新增的
                            if (co.cacheValue.FindIndex(x => (x.key == t.key)) == -1)
                            {
                                co.cacheValue.Add(t);
                                isChanged = true;
                            }
                            //修改的
                            if (co.cacheValue.FindIndex(x => (x.key == t.key && t.timestamp > x.timestamp)) > -1)
                            {
                                CacheData cd = co.cacheValue.Find(x => x.key == t.key);
                                co.cacheValue.Remove(cd);
                                co.cacheValue.Add(t);
                                isChanged = true;
                            }
                        }
                    }
                }
                else
                {
                    cache.ServerIdentify = WcfServerManage.Identify;
                    _localCache.Add(cache.cachename, cache);
                    if (_cacheNameList.Contains(cache.cachename) == false)
                        _cacheNameList.Add(cache.cachename);
                    isChanged = true;
                }
               
                if (isChanged == true)//同步缓存到下级中间件
                {
                    if (WcfServerManage.IsDebug)
                        ShowHostMsg(Color.Black, DateTime.Now, String.Format("分布式缓存同步完成，缓存名称：【{0}】，缓存记录：【{1}】", cache.cachename, (_localCache.GetData(cache.cachename) as CacheObject).cacheValue.Count));

                    new Action<CacheObject>(delegate(CacheObject _cache)
                        {
                            SyncCache(_cache);
                        }).BeginInvoke(cache, null, null);
                }
            }
            return true;
        }

        /// <summary>
        /// 获取待同步的缓存
        /// </summary>
        /// <returns></returns>
        private static List<CacheObject> GetAllCache()
        {
            List<CacheObject> cachelist = new List<CacheObject>();

            foreach (string cn in _cacheNameList)
            {
                if (_localCache.Contains(cn))
                {
                    CacheObject co = _localCache.GetData(cn) as CacheObject;
                    cachelist.Add(co);
                }
            }
            return cachelist;
        }

        /// <summary>
        /// 日期转换成时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static double DateTimeToTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToDouble((dateTime - start).TotalMilliseconds);
        }

        /// <summary>
        /// 时间戳转换成日期
        /// </summary>
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns></returns>
        private static DateTime TimestampToDateTime(double timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return start.AddMilliseconds(timestamp);
        }

        private static void ShowHostMsg(Color clr, DateTime time, string text)
        {
            hostwcfMsg.BeginInvoke(clr, time, text, null, null);//异步方式不影响后台数据请求
            //hostwcfMsg(time, text);
        }
    }
    /// <summary>
    /// 缓存对象
    /// </summary>
    [DataContract]
    public class CacheObject
    {
        /// <summary>
        /// 中间件标识
        /// </summary>
        [DataMember]
        public string ServerIdentify { get; set; }
        /// <summary>
        /// 缓存名称
        /// </summary>
        [DataMember]
        public string cachename { get; set; }
        /// <summary>
        /// 唯一标识
        /// </summary>
        [DataMember]
        public double identify { get; set; }
        /// <summary>
        /// 缓存数据集合
        /// </summary>
        [DataMember]
        public List<CacheData> cacheValue { get; set; }
    }

    /// <summary>
    /// 缓存数据
    /// </summary>
    [DataContract]
    public class CacheData
    {

        [DataMember]
        public double timestamp { get; set; }
        [DataMember]
        public string key { get; set; }
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public bool deleteflag { get; set; }
    }
    /// <summary>
    /// 缓存标识
    /// </summary>
    [DataContract]
    public class CacheIdentify
    {
        [DataMember]
        public string ServerIdentify { get; set; }
        [DataMember]
        public string cachename { get; set; }
        [DataMember]
        public double identify { get; set; }
        [DataMember]
        public IDictionary<string,double> keytimestamps { get; set; }
    }
}
