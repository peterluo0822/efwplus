using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.EntLib;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using System.Reflection;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using EFWCoreLib.CoreFrame.Business;
using System.IO;

namespace EFWCoreLib.CoreFrame.Plugin
{
    /// <summary>
    /// 模块插件
    /// </summary>
    public class ModulePlugin : MarshalByRefObject
    {
        /// <summary>
        /// 插件配置
        /// </summary>
        public PluginConfig plugin { get; set; }
        /// <summary>
        /// 数据库对象
        /// </summary>
        public AbstractDatabase database{get;set;}
        /// <summary>
        /// Unity对象容器
        /// </summary>
        public IUnityContainer container{get;set;}
        /// <summary>
        /// 企业库缓存
        /// </summary>
        public ICacheManager cache{get;set;}

        /// <summary>
        /// 执行控制器
        /// </summary>
        public AbstractControllerHelper helper { get; set; }

        public AppType appType { get; set; }
        /// <summary>
        /// 插件程序集的路径
        /// </summary>
        public string assemblyPath { get; set; }

        private List<Assembly> dllList=null;
        /// <summary>
        /// 插件所有程序集
        /// </summary>
        public List<Assembly> DllList
        {
            get { return dllList; }
        }

        public ModulePlugin()
        {
           
            
        }

        private void LoadDllList(string plugfile)
        {
            //string dllpath = new System.IO.FileInfo(plugfile).DirectoryName + "\\dll";
            dllList = new List<Assembly>();
            foreach (businessinfoDll dll in plugin.businessinfoDllList)
            {
                switch (appType)
                {
                    case AppType.Web:
                        dllList.Add(Assembly.Load(dll.name.Replace(".dll","")));
                        break;
                    default:
                        //方式一:直接读取文件，这种方式不支持热插拔
                        //dllList.Add(Assembly.LoadFrom(dllpath + "\\" + dll.name));
                        dllList.Add(Assembly.Load(dll.name.Replace(".dll","")));
                        //方式二：把dll读到内存再加载,再次加载内存会不断变大
                        //FileStream fs = new FileStream(dllpath + "\\" + dll.name, FileMode.Open, FileAccess.Read);
                        //BinaryReader br = new BinaryReader(fs);
                        //byte[] bFile = br.ReadBytes((int)fs.Length);
                        //br.Close();
                        //fs.Close();
                        //dllList.Add(Assembly.Load(bFile));
                        break;
                }
            }
        }

        /// <summary>
        /// 导入插件配置文件
        /// </summary>
        /// <param name="plugfile">插件配置文件路径</param>
        public void LoadPlugin(string plugfile)
        {
            

            container = ZhyContainer.CreateUnity();
            plugin = new PluginConfig();

            //switch (appType)
            //{
            //    case AppType.WCF:
            //        helper = new WcfFrame.ServerController.ControllerHelper();
            //        break;
            //}

            assemblyPath = new FileInfo(plugfile).Directory.FullName + "\\dll";

            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = plugfile };
            System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var plugininfo = (PluginSectionHandler)configuration.GetSection("plugin");
            if (plugininfo != null)
                plugin.Load(plugininfo, plugfile);

            LoadDllList(plugfile);//加载程序集

            var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");
            if (unitySection != null)
                container.LoadConfiguration(unitySection);//判断EntLib的路径对不对

            

            if (plugin.defaultdbkey != "")
                database = FactoryDatabase.GetDatabase(plugin.defaultdbkey);
            else
                database = FactoryDatabase.GetDatabase();

            database.PluginName = plugin.name;

            if (plugin.defaultcachekey != "")
                cache = ZhyContainer.CreateCache(plugin.defaultcachekey);
            else
                cache = ZhyContainer.CreateCache();
        }

        public void LoadAttribute(string plugfile)
        {
            
            if (dllList.Count > 0)
            {
                switch (appType)
                {
                    case AppType.Web:
                        EntityManager.LoadAttribute(dllList, cache, plugin.name);
                        WebControllerManager.LoadAttribute(dllList, this);
                        WebServicesManager.LoadAttribute(dllList, cache, plugin.name);
                        break;
                    case AppType.Winform:
                    //case AppType.WCFClient:
                        EntityManager.LoadAttribute(dllList, cache, plugin.name);
                        WinformControllerManager.LoadAttribute(dllList, this);
                        break;
                    case AppType.WCF:
                        EntityManager.LoadAttribute(dllList, cache, plugin.name);
                        WcfControllerManager.LoadAttribute(dllList, this);
                        break;
                }
            }
        }

        public void Remove()
        {
            ICacheManager _cache = cache;

            switch (AppGlobal.appType)
            {
                case AppType.Web:
                    EntityManager.ClearAttributeData(_cache, plugin.name);
                    WebControllerManager.ClearAttributeData(_cache, plugin.name);
                    WebServicesManager.ClearAttributeData(_cache, plugin.name);
                    break;
                case AppType.Winform:
                //case AppType.WCFClient:
                    EntityManager.ClearAttributeData(_cache, plugin.name);
                    WinformControllerManager.ClearAttributeData(_cache, plugin.name);
                    break;
                case AppType.WCF:
                    EntityManager.ClearAttributeData(_cache, plugin.name);
                    WcfControllerManager.ClearAttributeData(_cache, plugin.name);
                    break;
            }
        }

        //public Object WcfServerExecuteMethod(string controllername, string methodname, object[] paramValue, ClientRequestData requestData)
        //{
        //    EFWCoreLib.WcfFrame.ServerController.WcfServerController wscontroller = helper.CreateController(plugin.name, controllername) as EFWCoreLib.WcfFrame.ServerController.WcfServerController;
        //    wscontroller.requestData = requestData;
        //    wscontroller.responseData = new ServiceResponseData(requestData.Iscompressjson,requestData.Isencryptionjson,requestData.Serializetype);
        //    wscontroller.BindLoginRight(requestData.LoginRight);
        //    MethodInfo methodinfo = helper.CreateMethodInfo(plugin.name, controllername, methodname, wscontroller);
        //    return methodinfo.Invoke(wscontroller, paramValue);
        //}

        //public Object WcfClientExecuteMethod()
        //{
        //    return null;
        //}
    }
}
