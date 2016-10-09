using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.WcfHandler
{

    [ServiceKnownType(typeof(System.DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "RouterBaseService", SessionMode = SessionMode.Required,CallbackContract = typeof(IRouterBaseReply))]
    public interface IRouterBaseHandler
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message ProcessMessage(Message requestMessage);
    }


    [ServiceKnownType(typeof(System.DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "ReplyRouterBaseCallback", SessionMode = SessionMode.Required)]
    public interface IRouterBaseReply
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void ProcessMessage(Message requestMessage);
    }

    [ServiceKnownType(typeof(System.DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", SessionMode = SessionMode.Allowed, Name = "RouterFileService")]
    public interface IRouterFileHandler
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message ProcessMessage(Message requestMessage);
    }
}
