using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        public static void Exit()
        {
            webapiHost.StopHost();
        }
    }
}
