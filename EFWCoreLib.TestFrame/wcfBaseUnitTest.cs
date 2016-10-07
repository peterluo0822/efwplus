using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame;
using EFWCoreLib.WcfFrame.ClientController;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EFWCoreLib.WcfFrame.DataSerialize;

namespace EFWCoreLib.UnitTestFrame
{
    public class wcfBaseUnitTest
    {
        private static ClientLink _wcfClientLink;
        public static ClientLink wcfClientLink
        {
            get
            {
                if (_wcfClientLink == null)
                {
                    ClientLink clientlink = new ClientLink("UnitTest", wcfpluginname);
                    clientlink.CreateConnection();
                    _wcfClientLink = clientlink;
                }
                return _wcfClientLink;
            }
        }
        public static string wcfpluginname;
        public static string wcfcontroller;


        #region CHDEP通讯
        public ServiceResponseData InvokeWcfService(string wcfmethod)
        {
            return InvokeWcfService(wcfmethod, null);
        }

        public ServiceResponseData InvokeWcfService(string wcfmethod, Action<ClientRequestData> requestAction)
        {
            ClientLink wcfClientLink = ClientLinkManage.CreateConnection(wcfpluginname);
            ServiceResponseData retData = wcfClientLink.Request(wcfcontroller, wcfmethod, requestAction);
            return retData;
        }

        public IAsyncResult InvokeWcfServiceAsync(string wcfmethod, Action<ClientRequestData> requestAction, Action<ServiceResponseData> responseAction)
        {
            ClientLink wcfClientLink = ClientLinkManage.CreateConnection(wcfpluginname);
            return wcfClientLink.RequestAsync(wcfcontroller, wcfmethod, requestAction, responseAction);
        }
        #endregion
    }
}
