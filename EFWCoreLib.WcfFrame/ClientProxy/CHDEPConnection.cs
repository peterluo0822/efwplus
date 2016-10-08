using EFWCoreLib.WcfFrame.WcfService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame
{
    /// <summary>
    /// 平台连接对象
    /// </summary>
    public class CHDEPConnection
    {
        /// <summary>
        /// 业务数据服务
        /// </summary>
        public IWCFHandlerService WcfService { get; set; }
        /// <summary>
        /// 客户端回调服务
        /// </summary>
        public IClientService ClientService { get; set; }
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
