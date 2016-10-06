using EFWCoreLib.WcfFrame.ServerController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EFWCoreLib.CoreFrame.SSO
{
    /// <summary>
    /// 身份验证令牌管理
    /// </summary>
    public class TokenManager
    {
        private const int _TimerPeriod = 60000;//60秒
        private static Timer thTimer;
        /// <summary>
        /// 令牌集合
        /// </summary>
        //public static List<TokenInfo> tokenList = null;

        static TokenManager()
        {
            //tokenList = new List<TokenInfo>();
            //60秒失效
            //thTimer = new Timer(_ThreadTimerCallback, null, _TimerPeriod, _TimerPeriod);
        }

        public static bool AddToken(TokenInfo entity)
        {
            //tokenList.Add(entity);
            DistributedCacheManage.SetCache("tokenList", entity.UserId + (char)1 + entity.tokenId.ToString(), JsonConvert.SerializeObject(entity));
            return true;
        }

        //public static bool RemoveToken(Guid token)
        //{
        //    TokenInfo existToken = tokenList.SingleOrDefault(t => t.tokenId ==token);
        //    if (existToken != null)
        //    {
        //        tokenList.Remove(existToken);
        //        return true;
        //    }

        //    return false;
        //}

        public static bool RemoveToken(string userid)
        {
            TokenInfo tinfo = GetToken(userid);
            DistributedCacheManage.RemoveCache("tokenList", tinfo.UserId+(char)1+tinfo.tokenId.ToString());
            return true;
        }

        public static bool RemoveToken(Guid token)
        {
            TokenInfo tinfo = GetToken(token);
            DistributedCacheManage.RemoveCache("tokenList", tinfo.UserId + (char)1 + tinfo.tokenId.ToString());
            return true;
        }


        public static TokenInfo GetToken(Guid token)
        {
            TokenInfo existToken = null;
            try
            {
                CacheObject co = DistributedCacheManage.GetLocalCache("tokenList");
                if (co != null)
                {
                    CacheData cd = co.cacheValue.SingleOrDefault(x => (x.deleteflag == false && x.key.Split((char)1)[1] == token.ToString()));
                    if (cd != null)
                    {
                        existToken = JsonConvert.DeserializeObject<TokenInfo>(cd.value);
                        //更新时间
                        DistributedCacheManage.SetCache("tokenList", cd.key, JsonConvert.SerializeObject(existToken));
                    }
                }
            }
            catch { }
            return existToken;
        }

        public static TokenInfo GetToken(string userId)
        {
            TokenInfo existToken = null;
            try
            {
                CacheObject co = DistributedCacheManage.GetLocalCache("tokenList");
                if (co != null)
                {
                    CacheData cd = co.cacheValue.SingleOrDefault(x => (x.deleteflag == false && x.key.Split((char)1)[0] == userId));
                    if (cd != null)
                    {
                        existToken = JsonConvert.DeserializeObject<TokenInfo>(cd.value);
                        //更新时间
                        DistributedCacheManage.SetCache("tokenList", cd.key, JsonConvert.SerializeObject(existToken));
                    }
                }
            }
            catch { }
            return existToken;
        }

        private static void _ThreadTimerCallback(Object state)
        {
            //DateTime now = DateTime.Now;

            //Monitor.Enter(tokenList);
            //try
            //{
            //    // Searching for expired users
            //    foreach (TokenInfo t in tokenList)
            //    {
            //        if (((TimeSpan)(now - t.ActivityTime)).TotalMilliseconds > _TimerPeriod)
            //        {
            //            t.IsValid = false;//失效
            //        }
            //    }
            //}
            //finally
            //{
            //    Monitor.Exit(tokenList);
            //}
        }
    }
}
