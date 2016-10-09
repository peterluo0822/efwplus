using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.SDMessageHeader;
using EFWCoreLib.WcfFrame.ServerManage;

namespace EFWCoreLib.WcfFrame.WcfHandler
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, ValidateMustUnderstand = false, IncludeExceptionDetailInFaults = false)]
    public class RouterBaseService : IRouterBaseHandler
    {
        public Message ProcessMessage(Message requestMessage)
        {
            try
            {
                begintime();
                IRouterBaseHandler proxy = null;
                HeaderParameter para = HeaderOperater.GetHeaderValue(requestMessage);

                if (RouterManage.routerDic.ContainsKey(para.routerid))
                {
                    proxy = RouterManage.routerDic[para.routerid];
                    para.replyidentify = RouterManage.headParaDic[para.routerid].replyidentify;


                }
                else
                {
                    //Binding binding = null;
                    EndpointAddress endpointAddress = null;
                    Uri touri = null;
                    para = RouterManage.AddClient(requestMessage, para, out endpointAddress, out touri);
                    requestMessage.Headers.To = touri;

                    IRouterBaseReply callback = OperationContext.Current.GetCallbackChannel<IRouterBaseReply>();
                    NetTcpBinding tbinding = new NetTcpBinding("NetTcpBinding_RouterHandlerClient");
                    DuplexChannelFactory<IRouterBaseHandler> factory = new DuplexChannelFactory<IRouterBaseHandler>(new InstanceContext(new ReplyRouterBaseCallback(callback)), tbinding, endpointAddress);
                    proxy = factory.CreateChannel();

                    //缓存会话
                    RouterManage.routerDic.Add(para.routerid, proxy);
                    RouterManage.headParaDic.Add(para.routerid, para);

                }

                Message responseMessage = null;
                try
                {
                    HeaderOperater.AddMessageHeader(requestMessage, para);//增加自定义消息头
                    responseMessage = proxy.ProcessMessage(requestMessage);
                }
                catch (CommunicationException e)
                {
                    RouterManage.RemoveClient(para);
                    throw e;
                }
                if (para.cmd == "Quit")
                {
                    //关闭连接释放缓存会话
                    RouterManage.RemoveClient(para);
                }

                double outtime = endtime();
                // 请求消息记录
                if (WcfGlobal.IsDebug)
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Black, String.Format("路由请求消息发送(耗时[" + outtime + "])：  {0}", requestMessage.Headers.Action));


                return responseMessage;
            }
            catch (Exception e)
            {
                return Message.CreateMessage(requestMessage.Version, FaultCode.CreateReceiverFaultCode("error", RouterManage.ns), e.Message, requestMessage.Headers.Action);
            }
        }

        DateTime begindate;
        void begintime()
        {
            begindate = DateTime.Now;
        }
        //返回毫秒
        double endtime()
        {
            return DateTime.Now.Subtract(begindate).TotalMilliseconds;
        }
    }
}
