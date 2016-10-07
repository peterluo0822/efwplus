using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using EFWCoreLib.CoreFrame.Init;

namespace EFWCoreLib.WebFrame.HttpHandler
{
    /// <summary>
    /// web系统启动调用此对象(已移植到Global中调用)
    /// </summary>
    public class WebGlobal : IHttpModule
    {

        #region IHttpModule 成员

        
        private HttpApplication _context;
        public void Init(HttpApplication context)
        {
            _context = context;
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            AppGlobal.AppRootPath = _context.Server.MapPath("~/");
            AppGlobal.appType = AppType.Web;
            AppGlobal.IsSaas = System.Configuration.ConfigurationManager.AppSettings["IsSaas"] == "true" ? true : false;
            AppGlobal.AppStart();
        }

        public void Dispose()
        {
            //AppGlobal.AppEnd();
        }

        #endregion
    }
}
