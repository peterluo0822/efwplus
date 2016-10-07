using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml.Linq;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using EFWCoreLib.WcfFrame.ServerController;
using System.Drawing;
using EFWCoreLib.WcfFrame.SDMessageHeader;

namespace EFWCoreLib.WcfFrame.WcfService
{
    
    /// <summary>
    /// 路由服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, ValidateMustUnderstand = false)]
    public class RouterHandlerService :/* MarshalByRefObject,*/ IRouterService
    {
        public RouterHandlerService()
        {
            //WcfServerManage.hostwcfMsg(System.Drawing.Color.Blue, DateTime.Now, "New RouterHandlerService");
        }

        #region IRouterService Members

        /// <summary>
        /// 截获从Client端发送的消息转发到目标终结点并获得返回值给Client端
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public Message ProcessMessage(Message requestMessage)
        {
            try
            {
                begintime();
                IRouterService proxy = null;
                HeaderParameter para = HeaderOperater.GetHeaderValue(requestMessage);

                if (RouterServerManage.routerDic.ContainsKey(para.routerid))
                {
                    proxy = RouterServerManage.routerDic[para.routerid];
                    para.replyidentify = RouterServerManage.headParaDic[para.routerid].replyidentify;

                    
                }
                else
                {
                    //Binding binding = null;
                    EndpointAddress endpointAddress = null;
                    Uri touri = null;
                    para = RouterServerManage.AddClient(requestMessage, para, out endpointAddress, out touri);
                    requestMessage.Headers.To = touri;

                    IDuplexRouterCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexRouterCallback>();
                    NetTcpBinding tbinding = new NetTcpBinding("NetTcpBinding_RouterHandlerClient");
                    DuplexChannelFactory<IRouterService> factory = new DuplexChannelFactory<IRouterService>(new InstanceContext(new DuplexRouterCallback(callback)), tbinding, endpointAddress);
                    proxy = factory.CreateChannel();

                    //缓存会话
                    RouterServerManage.routerDic.Add(para.routerid, proxy);
                    RouterServerManage.headParaDic.Add(para.routerid, para);

                }

                Message responseMessage = null;
                try {
                    HeaderOperater.AddMessageHeader(requestMessage, para);//增加自定义消息头
                    responseMessage = proxy.ProcessMessage(requestMessage);
                }
                catch(CommunicationException e)
                {
                    RouterServerManage.RemoveClient(para);
                    throw e;
                }
                if (para.cmd == "Quit")
                {
                    //关闭连接释放缓存会话
                    RouterServerManage.RemoveClient(para);
                }

                double outtime = endtime();
                // 请求消息记录
                if (WcfServerManage.IsDebug)
                    RouterServerManage.hostwcfMsg(Color.Black, DateTime.Now, String.Format("路由请求消息发送(耗时[" + outtime + "])：  {0}", requestMessage.Headers.Action));


                return responseMessage;
            }
            catch (Exception e)
            {
                return Message.CreateMessage(requestMessage.Version, FaultCode.CreateReceiverFaultCode("error", RouterServerManage.ns), e.Message, requestMessage.Headers.Action);
            }
        }

        #endregion

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

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class DuplexRouterCallback : IDuplexRouterCallback
    {
        private IDuplexRouterCallback m_clientCallback;

        public DuplexRouterCallback(IDuplexRouterCallback clientCallback)
        {
            m_clientCallback = clientCallback;
        }

        public void ProcessMessage(Message requestMessage)
        {
            this.m_clientCallback.ProcessMessage(requestMessage);
        }
    }

    //UseSynchronizationContext 这个设置为false，必须异步执行
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, AddressFilterMode = AddressFilterMode.Any, UseSynchronizationContext = false, ValidateMustUnderstand = false)]
    public class FileRouterHandlerService : IFileRouterService
    {
        public FileRouterHandlerService()
        {
            //WcfServerManage.hostwcfMsg(System.Drawing.Color.Blue, DateTime.Now, "New FileRouterHandlerService");
        }

        #region IRouterService Members

        /// <summary>
        /// 截获从Client端发送的消息转发到目标终结点并获得返回值给Client端
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public Message ProcessMessage(Message requestMessage)
        {
            try
            {
                //Binding binding = null;
                EndpointAddress endpointAddress = null;
                Uri touri = null;


                RouterServerManage.GetServiceEndpointFile(requestMessage, out endpointAddress, out touri);
                requestMessage.Headers.To = touri;


                NetTcpBinding tbinding = new NetTcpBinding("NetTcpBinding_FileTransferHandlerService");
                using (ChannelFactory<IFileRouterService> factory = new ChannelFactory<IFileRouterService>(tbinding, endpointAddress))
                {

                    factory.Endpoint.Behaviors.Add(new MustUnderstandBehavior(false));
                    IFileRouterService proxy = factory.CreateChannel();

                    using (proxy as IDisposable)
                    {
                        // 请求消息记录
                        IClientChannel clientChannel = proxy as IClientChannel;
                        if (WcfServerManage.IsDebug)
                            RouterServerManage.hostwcfMsg(Color.Black, DateTime.Now, String.Format("路由请求消息发送：  {0}", clientChannel.RemoteAddress.Uri.AbsoluteUri));
                        // 调用绑定的终结点的服务方法
                        Message responseMessage = proxy.ProcessMessage(requestMessage);

                        return responseMessage;
                    }
                }

            }
            catch (Exception e)
            {
                return Message.CreateMessage(requestMessage.Version, FaultCode.CreateReceiverFaultCode("error", RouterServerManage.ns), e.Message, requestMessage.Headers.Action);
            }
        }


        #endregion
    }

}
