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
        private static List<RemotePlugin> RemotePluginDic = new List<RemotePlugin>();//远程注册插件

        public static LocalPlugin GetLocalPlugin()
        {
            LocalPlugin localPlugin = new LocalPlugin();
            localPlugin.ServerIdentify = WcfGlobal.Identify;
            localPlugin.PluginDic = CoreFrame.Init.AppPluginManage.PluginDic;

            return localPlugin;
        }

        public static List<RemotePlugin> GetRemotePlugin()
        {
            return RemotePluginDic;
        }

        //在订阅中调用
        public static void RegisterRemotePlugin(IDataReply callback, string ServerIdentify, string[] plugin)
        {
            
            //RemotePluginDic = new List<RemotePlugin>();
           
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
                //?
            }
        }
    }


    public class RemotePluginClient
    {
        public static LocalPlugin GetLocalPlugin()
        {
            LocalPlugin localPlugin = new LocalPlugin();
            localPlugin.ServerIdentify = WcfGlobal.Identify;
            localPlugin.PluginDic = CoreFrame.Init.AppPluginManage.PluginDic;

            return localPlugin;
        }
    }

    /// <summary>
    /// 本地插件
    /// </summary>
    public class LocalPlugin
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
