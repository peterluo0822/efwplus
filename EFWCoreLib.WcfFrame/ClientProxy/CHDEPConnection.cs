
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.ClientProxy;
using EFWCoreLib.WcfFrame.WcfHandler;

namespace EFWCoreLib.WcfFrame
{
    /// <summary>
    /// 平台连接对象
    /// </summary>
    public class ClientObject
    {
        /// <summary>
        /// 业务数据服务
        /// </summary>
        public DuplexBaseServiceClient WcfService { get; set; }
        /// <summary>
        /// 客户端回调服务
        /// </summary>
        public IDataReply ReplyService { get; set; }
        /// <summary>
        /// 客户端ID，服务端生成
        /// </summary>
        public string ClientID { get; set; }//服务端返回
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 路由ID
        /// </summary>
        public string RouterID { get; set; }
        /// <summary>
        /// 服务插件名称
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        /// 客户端令牌
        /// </summary>
        public string Token { get; set; }
    }
}
