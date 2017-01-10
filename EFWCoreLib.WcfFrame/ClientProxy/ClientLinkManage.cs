using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame
{
    /// <summary>
    /// 客户端程序连接管理类
    /// </summary>
    public class ClientLinkManage
    {
        /// <summary>
        /// 是否开启业务消息
        /// </summary>
        public static bool IsMessage = false;
        /// <summary>
        /// 业务消息触发时间间隔，单位秒
        /// </summary>
        public static int MessageTime = 1;//默认间隔1秒
        /// <summary>
        /// 登陆后缓存令牌
        /// </summary>
        public static string Token = null;//

        /// <summary>
        /// 缓存的客户连接
        /// </summary>
        private static Dictionary<string, ClientLink> ClientLinkDic = new Dictionary<string, ClientLink>();
        /// <summary>
        /// 创建wcf服务连接,此方式一个服务只有一个连接
        /// </summary>
        public static ClientLink CreateConnection(string pluginname)
        {
            try
            {
                ClientLink link;
                lock (ClientLinkDic)
                {
                    if (ClientLinkDic.ContainsKey(pluginname))
                    {
                        return ClientLinkDic[pluginname];
                    }

                    link = new ClientLink(null, pluginname);
                    ClientLinkDic.Add(pluginname, link);
                }
                link.CreateConnection(null, ((ism, met) =>
                {
                    IsMessage = ism;
                    MessageTime = met;
                }));
                link.clientObj.Token = Token;//赋值令牌
                return link;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public static ClientLink CreateConnection(string wcfendpoint, string pluginname)
        {
            try
            {
                ClientLink link;
                lock (ClientLinkDic)
                {
                    if (ClientLinkDic.ContainsKey(pluginname))
                    {
                        return ClientLinkDic[pluginname];
                    }

                    link = new ClientLink(null, pluginname, wcfendpoint);
                    ClientLinkDic.Add(pluginname, link);
                }
                link.CreateConnection(null, ((ism, met) =>
                 {
                     IsMessage = ism;
                     MessageTime = met;
                 }));
                link.clientObj.Token = Token;//赋值令牌
                return link;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        /// <summary>
        /// 卸载连接
        /// </summary>
        public static void UnConnection(string pluginname)
        {
            if (ClientLinkDic.Count == 0) return;
            if (ClientLinkDic.ContainsKey(pluginname) == false) return;
            ClientLinkDic[pluginname].Dispose();
            ClientLinkDic.Remove(pluginname);
        }

        /// <summary>
        /// 关闭所有连接
        /// </summary>
        public static void UnAllConnection()
        {
            if (ClientLinkDic.Count == 0) return;
            foreach (var c in ClientLinkDic)
            {
                c.Value.Dispose();
            }
            ClientLinkDic.Clear();
        }
    }
}
