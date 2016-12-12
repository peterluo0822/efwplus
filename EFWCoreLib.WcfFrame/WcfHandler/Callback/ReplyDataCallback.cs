using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.SDMessageHeader;
using EFWCoreLib.WcfFrame.ServerManage;
using EFWCoreLib.WcfFrame.Utility;

namespace EFWCoreLib.WcfFrame.WcfHandler
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ReplyDataCallback : IDataReply
    {
        private ClientLink clientLink;
        public ReplyDataCallback(ClientLink _clientLink)
        {
            clientLink = _clientLink;
        }

        public string ReplyProcessRequest(HeaderParameter para, string plugin, string controller, string method, string jsondata)
        {
            return DataManage.ReplyProcessRequest(plugin, controller, method, jsondata, para);
        }

        public void Notify(string publishServiceName)
        {
            PublishSubManager.ReceiveNotify(publishServiceName, clientLink);
        }
    }
}
