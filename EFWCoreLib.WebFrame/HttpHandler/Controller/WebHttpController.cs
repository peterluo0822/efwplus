//2011.10.11 添加模板功能
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using EFWCoreLib.CoreFrame.Business;
using System.Text;
using Newtonsoft.Json;
using EFWCoreLib.WcfFrame;
using EFWCoreLib.WcfFrame.DataSerialize;
using System.Threading;
using System.Diagnostics;

namespace EFWCoreLib.WebFrame.HttpHandler.Controller
{
    /// <summary>
    /// WebHttpController控制器
    /// </summary>
    public class WebHttpController : AbstractController
    {
        public HttpContext context { get; set; }

        /// <summary>
        /// Ajax请求返回Json数据
        /// </summary>
        public string JsonResult { get; set; }
        /// <summary>
        /// URL请求界面
        /// </summary>
        public string ViewResult { get; set; }
        /// <summary>
        /// 界面数据
        /// </summary>
        public Dictionary<string, Object> ViewData { get; set; }

        protected override SysLoginRight GetUserInfo()
        {
            if (sessionData != null && sessionData.ContainsKey("RoleUser"))
            {
                return (SysLoginRight)sessionData["RoleUser"];
            }
            return base.GetUserInfo();
        }

        private System.Collections.Generic.Dictionary<string, Object> _sessionData;
        /// <summary>
        /// Session数据传入后台
        /// </summary>
        public System.Collections.Generic.Dictionary<string, Object> sessionData
        {
            get
            {
                return _sessionData;
            }
            set
            {
                _sessionData = value;
            }
        }

        private System.Collections.Generic.Dictionary<string, Object> _putOutData;
        /// <summary>
        /// 后台传出数据到Session数据
        /// </summary>
        public System.Collections.Generic.Dictionary<string, Object> PutOutData
        {
            get
            {
                return _putOutData;
            }
            set
            {
                _putOutData = value;
            }
        }

        private List<string> _clearKey;
        /// <summary>
        /// 清除Session的数据
        /// </summary>
        public List<string> ClearKey
        {
            get { return _clearKey; }
            set { _clearKey = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _paramsData;
        /// <summary>
        /// Url参数传递数据
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> ParamsData
        {
            get { return _paramsData; }
            set { _paramsData = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _formData;
        /// <summary>
        /// Form提交的数据
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> FormData
        {
            get { return _formData; }
            set { _formData = value; }
        }


        public string RetSuccess()
        {
            return RetSuccess(null, null);
        }

        public string RetSuccess(string info)
        {
            return RetSuccess(info, null);
        }

        public string RetSuccess(string info, string data)
        {
            info = info == null ? "" : info;
            data = data == null ? "{}" : data;
            return "{\"ret\":0,\"msg\":" + "\"" + info + "\"" + ",\"data\":" + data + "}";
        }

        public string RetError()
        {
            return RetError(null, null);
        }

        public string RetError(string info)
        {
            return RetError(info, null);
        }

        public string RetError(string info, string data)
        {
            info = info == null ? "" : info;
            data = data == null ? "{}" : data;
            return "{\"ret\":1,\"msg\":" + "\"" + info + "\"" + ",\"data\":" + data + "}";
        }

        public string ToUrl(string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\" type=\"text/javascript\">\n");
            sb.Append("window.location.href='" + url + "'\n");
            sb.Append("</script>\n");
            return sb.ToString();
        }

        public string ToJson2(object obj)
        {
            string value = JsonConvert.SerializeObject(obj);
            return value;
        }


        #region CHDEP通讯
        private ClientLinkPool fromPoolGetClientLink(string wcfpluginname, out ClientLink clientlink, out int? index)
        {
            ClientLinkPool pool = ClientLinkPoolCache.GetClientPool("efwplusweb");
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
                if (clientlink == null && !pool.IsPoolFull)
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
            ClientLinkPool pool = fromPoolGetClientLink(wcfpluginname, out clientlink, out index);
            ServiceResponseData retData = new ServiceResponseData();

            try
            {
                //Thread.Sleep(2000);
                //ClientLink wcfClientLink = ClientLinkManage.CreateConnection(wcfpluginname);
                retData = clientlink.Request(wcfcontroller, wcfmethod, requestAction);
            }
            catch (Exception ex) { throw ex; }
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
                result = clientlink.RequestAsync(wcfcontroller, wcfmethod, requestAction, responseAction);
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