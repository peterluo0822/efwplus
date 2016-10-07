using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace EFWCoreLib.WcfFrame.WcfService.Contract
{
    /// <summary>
    /// WCF路由服务
    /// </summary>
    [ServiceKnownType(typeof(System.DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "RouterHandlerService", SessionMode = SessionMode.Required, CallbackContract = typeof(IDuplexRouterCallback))]
    public interface IRouterService
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message ProcessMessage(Message requestMessage);
    }

    [ServiceKnownType(typeof(System.DBNull))]
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IDuplexRouterCallback
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void ProcessMessage(Message requestMessage);
    }

    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "FileRouterHandlerService", SessionMode = SessionMode.Allowed)]
    public interface IFileRouterService
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message ProcessMessage(Message requestMessage);
        //[OperationContract(AsyncPattern = true, Action = "*", ReplyAction = "*")]
        //IAsyncResult BeginProcessMessage(Message requestMessage, AsyncCallback callback, object state);
        //Message EndProcessMessage(IAsyncResult result);
    }
}
