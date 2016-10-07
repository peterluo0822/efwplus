using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Business.Interface;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Plugin;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.Threading;
using EFWCoreLib.WcfFrame;
using EFWCoreLib.WcfFrame.DataSerialize;

namespace EFWCoreLib.WebFrame.WebAPI
{
    /// <summary>
    /// WebApi基类控制器
    /// </summary>
    public class WebApiController : ApiController, INewObject, INewDao
    {
        private AbstractDatabase _oleDb = null;
        private IUnityContainer _container = null;
        private ICacheManager _cache = null;
        private string _pluginName = null;
        //private HttpContext _context = HttpContext.Current;

        public AbstractDatabase oleDb
        {
            get
            {
                return _oleDb;
            }
        }
        public string PluginName
        {
            get { return _pluginName; }
            set { _pluginName = value; }
        }

       

        public WebApiController()
            : base()
        {
            efwplusApiControllerAttribute[] apiC = ((efwplusApiControllerAttribute[])this.GetType().GetCustomAttributes(typeof(efwplusApiControllerAttribute), true));
            if (apiC.Length > 0)
                _pluginName = apiC[0].PluginName;
        }

        #region INewObject 成员
        public T NewObject<T>()
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container, _cache, _pluginName, null);
            return t;
        }

        public T NewObject<T>(string unityname)
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container, _cache, _pluginName, unityname);
            return t;
        }

        #endregion

        #region INewDao 成员

        public T NewDao<T>()
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container, _cache, _pluginName, null);
            return t;
        }

        public T NewDao<T>(string unityname)
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container, _cache, _pluginName, unityname);
            return t;
        }

        #endregion

        protected void InitPlugin(string pluginname)
        {
            //Page_Load 进行初始化
            _pluginName = pluginname;
            if (pluginname == "coresys") return;
            ModulePlugin mp;
            mp = AppPluginManage.PluginDic[_pluginName];
            if (mp == null)
                throw new Exception("插件名：" + pluginname + "不存在！");
            _oleDb = mp.database;
            _container = mp.container;
            _cache = mp.cache;
        }

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            _pluginName = this.SetPluginName();
            if (_pluginName != null)
            {
                InitPlugin(_pluginName);
            }
        }

        protected virtual string SetPluginName()
        {
            return _pluginName;
        }


        #region CHDEP通讯，连接池方式
        private ClientLinkPool fromPoolGetClientLink(string wcfpluginname, out ClientLink clientlink, out int? index)
        {
            ClientLinkPool pool = ClientLinkPoolCache.GetClientPool("webapi");
            //获取的池子索引
            index = null;
            clientlink = null;
            //是否超时
            bool isouttime = false;
            //超时计时器
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                bool isReap = true;
                //先判断池子中是否有此空闲连接
                if (pool.GetFreePoolNums(wcfpluginname) > 0)
                {
                    isReap = false;
                    clientlink = pool.GetClientLink(wcfpluginname);
                    if (clientlink != null)
                    {
                        index = clientlink.Index;
                    }
                }
                //如果没有空闲连接判断是否池子是否已满，未满，则创建新连接并装入连接池
                //不要一下子将所有连接都在wcf服务创建，逐步创建连接
                if (clientlink == null && !pool.IsPoolFull && pool.GetOpeningNums(wcfpluginname) <= 100)
                {
                    //装入连接池
                    bool flag = pool.AddPool(wcfpluginname, out clientlink, out index);
                }

                //如果当前契约无空闲连接，并且队列已满，并且非当前契约有空闲，则踢掉一个非当前契约
                if (clientlink == null && pool.IsPoolFull && pool.GetFreePoolNums(wcfpluginname) == 0 && pool.GetUsedPoolNums(wcfpluginname) != 500)
                {
                    //创建新连接
                    pool.RemovePoolOneNotAt(wcfpluginname, out clientlink, out index);
                }

                if (clientlink != null)
                    break;

                //如果还未获取连接判断是否超时30秒，如果超时抛异常
                if (sw.Elapsed >= new TimeSpan(30 * 1000 * 10000))
                {
                    isouttime = true;
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            sw.Stop();
            sw = null;

            if (isouttime)
            {
                throw new Exception("获取连接池中的连接超时");
            }

            return pool;
        }

        public ServiceResponseData InvokeWcfService(string wcfpluginname, string wcfcontroller, string wcfmethod)
        {
            return InvokeWcfService(wcfpluginname, wcfcontroller, wcfmethod, null);
        }

        public ServiceResponseData InvokeWcfService(string wcfpluginname, string wcfcontroller, string wcfmethod, Action<ClientRequestData> requestAction)
        {

            //获取的池子索引
            int? index = null;
            ClientLink clientlink = null;
            ClientLinkPool pool=fromPoolGetClientLink(wcfpluginname, out clientlink, out index);
            ServiceResponseData retData = new ServiceResponseData();

            try
            {
                //Thread.Sleep(2000);
                //ClientLink wcfClientLink = ClientLinkManage.CreateConnection(wcfpluginname);
                //绑定LoginRight
                Action<ClientRequestData> _requestAction = ((ClientRequestData request) =>
                {
                    if (Request.Properties.ContainsKey("__user_token__"))
                        request.LoginRight = (SysLoginRight)Request.Properties["__user_token__"];
                    if (requestAction != null)
                        requestAction(request);
                });

                retData = clientlink.Request(wcfcontroller, wcfmethod, _requestAction);
            }
            catch (Exception ex){ throw ex; }
            finally
            {
                if (index != null)
                    pool.ReturnPool(wcfpluginname, (int)index);
            }
            return retData;
        }

        public IAsyncResult InvokeWcfServiceAsync(string wcfpluginname, string wcfcontroller, string wcfmethod, Action<ClientRequestData> requestAction, Action<ServiceResponseData> responseAction)
        {
            //获取的池子索引
            int? index = null;
            ClientLink clientlink = null;
            ClientLinkPool pool = fromPoolGetClientLink(wcfpluginname, out clientlink, out index);

            IAsyncResult result = null;
            try
            {
                //ClientLink wcfClientLink = ClientLinkManage.CreateConnection(wcfpluginname);
                //绑定LoginRight
                Action<ClientRequestData> _requestAction = ((ClientRequestData request) =>
                {
                    if (Request.Properties.ContainsKey("__user_token__"))
                        request.LoginRight = (SysLoginRight)Request.Properties["__user_token__"];
                    if (requestAction != null)
                        requestAction(request);
                });
                result = clientlink.RequestAsync(wcfcontroller, wcfmethod, _requestAction, responseAction);
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                if (index != null)
                    pool.ReturnPool(wcfpluginname, (int)index);
            }
            return result;
        }
        #endregion
    }
}