using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.Plugin;
using System.Reflection;

namespace EFWCoreLib.CoreFrame.Init
{
    /// <summary>
    /// 程序运行后的插件管理
    /// 插件实现热插拔，有两种方式：
    /// 1）用AppDomain，但这种方式在这里行不通，因为AppDomain之间传递的对象太复杂
    /// 2）用内存中读取dll的方式
    /// </summary>
    public class AppPluginManage
    {
        public static bool IsOpenDomain = false;//是否开启程序域来动态管理插件
        public static Dictionary<string, ModulePlugin> PluginDic;//本地插件
        //public static List<RemotePlugin> RemotePluginDic;//远程注册插件
        public static Dictionary<string, AppDomain> DomainDic;//程序域来动态管理插件
        /// <summary>
        /// 加载所有插件
        /// </summary>
        public static void LoadAllPlugin()
        {
            PluginDic = new Dictionary<string, ModulePlugin>();
            //RemotePluginDic = new List<RemotePlugin>();
            List<string> pflist = PluginSysManage.GetAllPluginFile();
            for (int i = 0; i < pflist.Count; i++)
            {
                AddPlugin(AppGlobal.AppRootPath + pflist[i]);
            }
        }

        /// <summary>
        /// 加载插件，创建AppDomain来动态加载或卸载dll
        /// </summary>
        /// <param name="plugfile"></param>
        public static void AddPlugin(string plugfile)
        {
            if (PluginDic == null)
                PluginDic = new Dictionary<string, ModulePlugin>();
            if (DomainDic == null)
                DomainDic = new Dictionary<string, AppDomain>();

            if (IsOpenDomain)//开启程序域
            {
                //AppDomainSetup setup = new AppDomainSetup();
                //setup.ApplicationName = "ApplicationLoader";
                //setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                //setup.PrivateBinPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private");
                //setup.CachePath = setup.ApplicationBase;
                //setup.ShadowCopyFiles = "true";
                //setup.ShadowCopyDirectories = setup.ApplicationBase;
                //AppDomain.CurrentDomain.SetShadowCopyFiles();
                //AppDomain appDomain = AppDomain.CreateDomain("ApplicationLoaderDomain", null, setup);
                //String name = Assembly.GetExecutingAssembly().GetName().FullName;
                //ModulePlugin mp = (ModulePlugin)appDomain.CreateInstanceAndUnwrap(name, typeof(ModulePlugin).FullName);
                //mp.appType = AppGlobal.appType;
                //mp.LoadPlugin(plugfile);

                //if (PluginDic.ContainsKey(mp.plugin.name) == false)
                //{
                //    PluginDic.Add(mp.plugin.name, mp);
                //    DomainDic.Add(mp.plugin.name, appDomain);
                //    mp.LoadAttribute(plugfile);
                //}
            }
            else
            {
                ModulePlugin mp = new ModulePlugin();
                mp.appType = AppGlobal.appType;
                mp.LoadPlugin(plugfile);

                if (PluginDic.ContainsKey(mp.plugin.name) == false)
                {
                    PluginDic.Add(mp.plugin.name, mp);
                    mp.LoadAttribute(plugfile);
                }
            }
        }
        /// <summary>
        /// 卸载插件
        /// </summary>
        /// <param name="plugname"></param>
        public static void RemovePlugin(string plugname)
        {
            if (PluginDic.ContainsKey(plugname) == true)
            {
                PluginDic[plugname].Remove();
                PluginDic.Remove(plugname);
            }
        }
        /// <summary>
        /// 卸载所有插件
        /// </summary>
        public static void RemoveAllPlugin()
        {
            foreach (var item in PluginDic)
            {
                RemovePlugin(item.Key);
            }
        }


        public static WebControllerAttributeInfo GetPluginWebControllerAttributeInfo(string pluginname, string name, out ModulePlugin mp)
        {
            mp = PluginDic[pluginname];
            if (mp != null)
            {
                List<WebControllerAttributeInfo> list = (List<WebControllerAttributeInfo>)WebControllerManager.GetAttributeInfo(mp.cache, mp.plugin.name);
                if (list.FindIndex(x => x.controllerName == name) > -1)
                {
                    return list.Find(x => x.controllerName == name);
                }
            }
            return null;
        }

        public static WinformControllerAttributeInfo GetPluginWinformControllerAttributeInfo(string pluginname, string name, out ModulePlugin mp)
        {
            if (PluginDic.ContainsKey(pluginname) == false)
                throw new Exception("["+pluginname+"]插件模块并没有在pluginsys.xml配置文件中配置好！");
            mp = PluginDic[pluginname];
            if (mp != null)
            {
                List<WinformControllerAttributeInfo> list = (List<WinformControllerAttributeInfo>)WinformControllerManager.GetAttributeInfo(mp.cache, mp.plugin.name);
                if (list!=null && list.FindIndex(x => x.controllerName == name) > -1)
                {
                    return list.Find(x => x.controllerName == name);
                }
            }
            return null;

            //foreach (KeyValuePair<string, ModulePlugin> val in PluginDic)
            //{
            //    List<WinformControllerAttributeInfo> list = (List<WinformControllerAttributeInfo>)WinformControllerManager.GetAttributeInfo(val.Value.cache, val.Value.plugin.name);
            //    if (list.FindIndex(x => x.controllerName == name) > -1)
            //    {
            //        mp = val.Value;
            //        return list.Find(x => x.controllerName == name);
            //    }
            //}
            //mp = null;
            //return null;
        }

        public static WcfControllerAttributeInfo GetPluginWcfControllerAttributeInfo(string pluginname, string name, out ModulePlugin mp)
        {
            mp = PluginDic[pluginname];
            if (mp != null)
            {
                List<WcfControllerAttributeInfo> list = (List<WcfControllerAttributeInfo>)WcfControllerManager.GetAttributeInfo(mp.cache, mp.plugin.name);
                if (list.FindIndex(x => x.controllerName == name) > -1)
                {
                    return list.Find(x => x.controllerName == name);
                }
            }
            return null;


            //foreach (KeyValuePair<string, ModulePlugin> val in PluginDic)
            //{
            //    List<WcfControllerAttributeInfo> list = (List<WcfControllerAttributeInfo>)WcfControllerManager.GetAttributeInfo(val.Value.cache, val.Value.plugin.name);
            //    if (list.FindIndex(x => x.controllerName == name) > -1)
            //    {
            //        mp = val.Value;
            //        return list.Find(x => x.controllerName == name);
            //    }
            //}
            //mp = null;
            //return null;
        }

        public static WebServicesAttributeInfo GetPluginWebServicesAttributeInfo(string name, out ModulePlugin mp)
        {
            foreach (KeyValuePair<string, ModulePlugin> val in PluginDic)
            {
                List<WebServicesAttributeInfo> list = (List<WebServicesAttributeInfo>)WebServicesManager.GetAttributeInfo(val.Value.cache, val.Value.plugin.name);
                if (list.FindIndex(x => x.ServiceName == name) > -1)
                {
                    mp = val.Value;
                    return list.Find(x => x.ServiceName == name);
                }
            }
            mp = null;
            return null;
        }

        public static string getbaseinfoDataValue(string _pluginName,string key)
        {
            if (AppPluginManage.PluginDic[_pluginName].plugin.baseinfoDataList.FindIndex(x => x.key == key) != -1)
            {
                return AppPluginManage.PluginDic[_pluginName].plugin.baseinfoDataList.Find(x => x.key == key).value;
            }
            return null;
        }
    }

    
}
