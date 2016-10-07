using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.WcfFrame.DataSerialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.SDMessageHeader
{
    /// <summary>
    /// wcf消息头处理
    /// </summary>
    public class HeaderOperater
    {
        public static string ns = "http://www.efwplus.cn/";
        public static void AddMessageHeader(Message requestMessage, HeaderParameter para)
        {
            requestMessage.Headers.RemoveAll("CMD", ns);
            requestMessage.Headers.RemoveAll("RouterID", ns);
            requestMessage.Headers.RemoveAll("Plugin", ns);
            requestMessage.Headers.RemoveAll("ReplyIdentify", ns);
            requestMessage.Headers.RemoveAll("Token", ns);
            requestMessage.Headers.RemoveAll("IsCompressJson", ns);
            requestMessage.Headers.RemoveAll("IsEncryptionJson", ns);
            requestMessage.Headers.RemoveAll("SerializeType", ns);
            requestMessage.Headers.RemoveAll("LoginRight", ns);

            var CMD = System.ServiceModel.Channels.MessageHeader.CreateHeader("CMD", ns, para.cmd);
            requestMessage.Headers.Add(CMD);
            var router = System.ServiceModel.Channels.MessageHeader.CreateHeader("RouterID", ns, para.routerid);
            requestMessage.Headers.Add(router);
            var plugin = System.ServiceModel.Channels.MessageHeader.CreateHeader("Plugin", ns, para.pluginname);
            requestMessage.Headers.Add(plugin);
            var ReplyHN = System.ServiceModel.Channels.MessageHeader.CreateHeader("ReplyIdentify", ns, para.replyidentify);
            requestMessage.Headers.Add(ReplyHN);
            var token = System.ServiceModel.Channels.MessageHeader.CreateHeader("Token", ns, para.token);
            requestMessage.Headers.Add(token);
            var IsCompressJson = System.ServiceModel.Channels.MessageHeader.CreateHeader("IsCompressJson", ns, Convert.ToString(para.iscompressjson ? 1 : 0));
            requestMessage.Headers.Add(IsCompressJson);
            var IsEncryptionJson = System.ServiceModel.Channels.MessageHeader.CreateHeader("IsEncryptionJson", ns, Convert.ToString(para.isencryptionjson ? 1 : 0));
            requestMessage.Headers.Add(IsEncryptionJson);
            var SerializeType = System.ServiceModel.Channels.MessageHeader.CreateHeader("SerializeType", ns, Convert.ToString((int)para.serializetype));
            requestMessage.Headers.Add(SerializeType);
            var LoginRight = System.ServiceModel.Channels.MessageHeader.CreateHeader("LoginRight", ns, para.LoginRight);
            requestMessage.Headers.Add(LoginRight);
        }

        public static void AddMessageHeader(MessageHeaders headers, HeaderParameter para)
        {
            headers.RemoveAll("CMD", ns);
            headers.RemoveAll("RouterID", ns);
            headers.RemoveAll("Plugin", ns);
            headers.RemoveAll("ReplyIdentify", ns);
            headers.RemoveAll("Token", ns);
            headers.RemoveAll("IsCompressJson", ns);
            headers.RemoveAll("IsEncryptionJson", ns);
            headers.RemoveAll("SerializeType", ns);
            headers.RemoveAll("LoginRight", ns);

            var CMD = System.ServiceModel.Channels.MessageHeader.CreateHeader("CMD", ns, para.cmd);
            headers.Add(CMD);
            var router = System.ServiceModel.Channels.MessageHeader.CreateHeader("RouterID", ns, para.routerid);
            headers.Add(router);
            var plugin = System.ServiceModel.Channels.MessageHeader.CreateHeader("Plugin", ns, para.pluginname);
            headers.Add(plugin);
            var ReplyHN = System.ServiceModel.Channels.MessageHeader.CreateHeader("ReplyIdentify", ns, para.replyidentify);
            headers.Add(ReplyHN);
            var token = System.ServiceModel.Channels.MessageHeader.CreateHeader("Token", ns, para.token);
            headers.Add(token);
            var IsCompressJson = System.ServiceModel.Channels.MessageHeader.CreateHeader("IsCompressJson", ns, Convert.ToString(para.iscompressjson ? 1 : 0));
            headers.Add(IsCompressJson);
            var IsEncryptionJson = System.ServiceModel.Channels.MessageHeader.CreateHeader("IsEncryptionJson", ns, Convert.ToString(para.isencryptionjson ? 1 : 0));
            headers.Add(IsEncryptionJson);
            var SerializeType = System.ServiceModel.Channels.MessageHeader.CreateHeader("SerializeType", ns, Convert.ToString((int)para.serializetype));
            headers.Add(SerializeType);
            var LoginRight = System.ServiceModel.Channels.MessageHeader.CreateHeader("LoginRight", ns, para.LoginRight);
            headers.Add(LoginRight);
        }

        public static HeaderParameter GetHeaderValue(Message requestMessage)
        {
            HeaderParameter para = new HeaderParameter();

            var headers = requestMessage.Headers;

            var index = headers.FindHeader("CMD", ns);
            if (index > -1)
                para.cmd = headers.GetHeader<string>(index);
            index = headers.FindHeader("RouterID", ns);
            if (index > -1)
                para.routerid = headers.GetHeader<string>(index);
            index = headers.FindHeader("Plugin", ns);
            if (index > -1)
                para.pluginname = headers.GetHeader<string>(index);
            index = headers.FindHeader("ReplyIdentify", ns);
            if (index > -1)
                para.replyidentify = headers.GetHeader<string>(index);
            index = headers.FindHeader("Token", ns);
            if (index > -1)
                para.token = headers.GetHeader<string>(index);
            index = headers.FindHeader("IsCompressJson", ns);
            if (index > -1)
                para.iscompressjson = headers.GetHeader<string>(index).Trim() == "1" ? true : false;
            index = headers.FindHeader("IsEncryptionJson", ns);
            if (index > -1)
                para.isencryptionjson = headers.GetHeader<string>(index).Trim() == "1" ? true : false;
            index = headers.FindHeader("SerializeType", ns);
            if (index > -1)
                para.serializetype = (SerializeType)Convert.ToInt32(headers.GetHeader<string>(index).Trim());
            index = headers.FindHeader("LoginRight", ns);
            if (index > -1)
                para.LoginRight = headers.GetHeader<SysLoginRight>(index);
            return para;
        }
    }
}
