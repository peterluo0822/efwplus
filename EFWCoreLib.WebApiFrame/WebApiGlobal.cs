using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WebFrame.WebAPI;

namespace EFWCoreLib.WebApiFrame
{
    /// <summary>
    /// WebApi服务启动程序
    /// </summary>
    public class WebApiGlobal
    {
        static WebApiSelfHosting webapiHost = null;
        public static void Main()
        {
            webapiHost = new WebApiSelfHosting(System.Configuration.ConfigurationSettings.AppSettings["WebApiUri"]);
            webapiHost.StartHost();

            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "WebAPI服务启动完成");
        }

        public static void Exit()
        {
            webapiHost.StopHost();
            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "WebAPI服务已关闭！");
        }
    }
}
