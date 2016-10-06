using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace EFWCoreLib.WebFrame.WebAPI
{
    public class ShowExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var message = context.Exception.Message;
            if (context.Exception.InnerException != null)
                message = context.Exception.InnerException.Message;

            WebApiSelfHosting.ShowHostMsg(Color.Red, DateTime.Now, "WebApi执行异常：[" + context.Request.RequestUri.LocalPath + "]" + message);

            //context.Response = new HttpResponseMessage() { Content = new StringContent(message) };
            base.OnException(context);
        }
    }
}
