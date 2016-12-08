using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.ServerManage;

namespace EFWCoreLib.WcfFrame.WcfHandler
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Reentrant, AddressFilterMode = AddressFilterMode.Any, UseSynchronizationContext = false, ValidateMustUnderstand = false)]
    public class RouterFileService : IRouterFileHandler
    {
        public Message ProcessMessage(Message requestMessage)
        {
            try
            {
                //Binding binding = null;
                EndpointAddress endpointAddress = null;
                Uri touri = null;


                RouterManage.GetServiceEndpointFile(requestMessage, out endpointAddress, out touri);
                requestMessage.Headers.To = touri;


                NetTcpBinding tbinding = new NetTcpBinding("NetTcpBinding_FileService");
                using (ChannelFactory<IRouterFileHandler> factory = new ChannelFactory<IRouterFileHandler>(tbinding, endpointAddress))
                {

                    factory.Endpoint.Behaviors.Add(new MustUnderstandBehavior(false));
                    IRouterFileHandler proxy = factory.CreateChannel();

                    using (proxy as IDisposable)
                    {
                        // 请求消息记录
                        IClientChannel clientChannel = proxy as IClientChannel;
                        if (WcfGlobal.IsDebug)
                            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Black, String.Format("路由请求消息发送：  {0}", clientChannel.RemoteAddress.Uri.AbsoluteUri));
                        // 调用绑定的终结点的服务方法
                        Message responseMessage = proxy.ProcessMessage(requestMessage);

                        return responseMessage;
                    }
                }

            }
            catch (Exception e)
            {
                return Message.CreateMessage(requestMessage.Version, FaultCode.CreateReceiverFaultCode("error", WcfGlobal.ns), e.Message, requestMessage.Headers.Action);
            }
        }
    }
}
