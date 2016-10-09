using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Plugin;
using EFWCoreLib.WcfFrame.WcfHandler;

namespace EFWCoreLib.WcfFrame.ServerManage
{
    public class RemotePluginManage
    {
        public static ClientLink superclient;//连接上级中间件的连接
        public static LocalPlgin localPlugin;//本地插件
        public static List<RemotePlugin> RemotePluginDic;//远程注册插件

        public static void CreateSuperClient()
        {
            localPlugin = new LocalPlgin();
            localPlugin.ServerIdentify = WcfGlobal.Identify;
            localPlugin.PluginDic = CoreFrame.Init.AppPluginManage.PluginDic;

            //就算上级中间件重启了，下级中间件创建链接的时候会重新注册本地插件
            superclient = new ClientLink(WcfGlobal.HostName, (() =>
            {
                //注册本地插件到上级中间件
                superclient.RegisterRemotePlugin(WcfGlobal.Identify, localPlugin.PluginDic.Keys.ToArray());
                //同步缓存到上级中间件
                DistributedCacheManage.SyncAllCache();
            }));
            try
            {
                superclient.CreateConnection();
            }
            catch
            {
                MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "连接上级中间件失败！");
            }
        }

        public static void UnCreateSuperClient()
        {
            if (superclient != null)
                superclient.Dispose();
        }

        public static void RegisterRemotePlugin(IDataReply callback, string ServerIdentify, string[] plugin)
        {
            
            RemotePluginDic = new List<RemotePlugin>();
           
            //自己没必要注册自己
            if (ServerIdentify == WcfGlobal.Identify) return;
            bool isChanged = false;
            RemotePlugin rp = null;
            if (RemotePluginDic.FindIndex(x => x.ServerIdentify == ServerIdentify) > -1)
            {
                rp = RemotePluginDic.Find(x => x.ServerIdentify == ServerIdentify);
                //rp.clientService = callback;

                List<string> newplugin = rp.plugin.ToList();
                foreach (var p in plugin)
                {
                    //新注册的插件在原来插件中找不到，则新增
                    if (newplugin.ToList().FindIndex(x => x == p) == -1)
                    {
                        newplugin.Add(p);
                        isChanged = true;
                    }
                }
                foreach (var p in newplugin)
                {
                    //如果注册的插件在新注册插件中找不到，则移除
                    if (plugin.ToList().FindIndex(x => x == p) == -1)
                    {
                        newplugin.Remove(p);
                        isChanged = true;
                    }
                }
                rp.plugin = newplugin.ToArray();
            }
            else
            {
                rp = new RemotePlugin();
                rp.ServerIdentify = ServerIdentify;
                rp.callback = callback;
                rp.plugin = plugin;
                RemotePluginDic.Add(rp);
                isChanged = true;
            }

            if (isChanged == true)
            {
                //重新注册远程插件
                foreach (var p in RemotePluginDic)
                {
                    superclient.RegisterRemotePlugin(p.ServerIdentify, p.plugin);
                }
            }
        }
    }

    /// <summary>
    /// 本地插件
    /// </summary>
    public class LocalPlgin
    {
        public string ServerIdentify { get; set; }
        public Dictionary<string, ModulePlugin> PluginDic { get; set; }
    }

    /// <summary>
    /// 远程插件
    /// </summary>
    public class RemotePlugin
    {
        public string ServerIdentify { get; set; }
        public string[] plugin { get; set; }
        public IDataReply callback { get; set; }
    }
}
