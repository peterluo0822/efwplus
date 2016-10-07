using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.WcfFrame.DataSerialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.SDMessageHeader
{

    /// <summary>
    /// 自定义消息头参数
    /// </summary>
    [DataContract]
    public class HeaderParameter
    {
        [DataMember]
        public string cmd { get; set; }
        [DataMember]
        public string routerid { get; set; }
        [DataMember]
        public string pluginname { get; set; }
        [DataMember]
        public string replyidentify { get; set; }
        [DataMember]
        public string token { get; set; }
        /// <summary>
        /// 压缩Json字符
        /// </summary>
        [DataMember]
        public bool iscompressjson { get; set; }
        /// <summary>
        /// 加密Json字符
        /// </summary>
        [DataMember]
        public bool isencryptionjson { get; set; }
        /// <summary>
        /// 序列化类型
        /// </summary>
        [DataMember]
        public SerializeType serializetype { get; set; }

        /// <summary>
        /// 登录信息
        /// </summary>
        public SysLoginRight LoginRight { get; set; }
    }
    
}
