using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.WcfHandler
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ReplyRouterBaseCallback : IRouterBaseReply
    {
        private IRouterBaseReply _callback;
        public ReplyRouterBaseCallback(IRouterBaseReply callback)
        {
            _callback = callback;
        }

        public void ProcessMessage(Message requestMessage)
        {
            _callback.ProcessMessage(requestMessage);
        }
    }
}
