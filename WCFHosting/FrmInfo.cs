using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.WcfFrame.ServerManage;

namespace WCFHosting
{
    public partial class FrmInfo : Form
    {
        public FrmInfo()
        {
            InitializeComponent();
        }

        private void FrmInfo_Load(object sender, EventArgs e)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine("[基本参数]");
            text.AppendLine("中间件名称：【" + WcfGlobal.HostName + "】");
            text.AppendLine("中间件唯一标识：【" + WcfGlobal.Identify + "】");
            text.AppendLine("调试信息：\t\t" + (WcfGlobal.IsDebug ? "开启" : "关闭"));

            text.AppendLine("基础服务：\t\t" + (HostSettingConfig.GetValue("wcfservice") == "1" ? "开启" : "关闭"));
            text.AppendLine("文件传输：\t\t" + (HostSettingConfig.GetValue("filetransfer") == "1" ? "开启" : "关闭"));
            text.AppendLine("路由服务：\t\t" + (HostSettingConfig.GetValue("router") == "1" ? "开启" : "关闭"));
            text.AppendLine("WebAPI服务：\t\t" + (HostSettingConfig.GetValue("webapi") == "1" ? "开启" : "关闭"));

            text.AppendLine("心跳检测：\t\t" + (ClientManage.IsHeartbeat ? "开启" : "关闭") + "\t" + "间隔时间(秒)：\t" + ClientManage.HeartbeatTime.ToString());
            text.AppendLine("消息发送：\t\t" + (ClientManage.IsMessage ? "开启" : "关闭") + "\t" + "间隔时间(秒)：\t" + ClientManage.MessageTime.ToString());
            text.AppendLine("耗时日志记录：\t\t" + (ClientManage.IsOverTime ? "开启" : "关闭") + "\t" + "超过时间(秒)：\t" + ClientManage.OverTime.ToString());

            text.AppendLine("数据压缩：\t\t" + (ClientManage.IsCompressJson ? "开启" : "关闭"));
            text.AppendLine("数据加密：\t\t" + (ClientManage.IsEncryptionJson ? "开启" : "关闭"));

            text.AppendLine();
            text.AppendLine("[发布服务地址]");
            if (HostSettingConfig.GetValue("wcfservice") == "1")
                text.AppendLine("基础数据服务：" + HostAddressConfig.GetWcfAddress());
            if (HostSettingConfig.GetValue("filetransfer") == "1")
                text.AppendLine("文件传输服务：" + HostAddressConfig.GetFileAddress());
            if (HostSettingConfig.GetValue("router") == "1")
            {
                text.AppendLine("路由基础服务：" + HostAddressConfig.GetRouterAddress());
                text.AppendLine("路由文件服务：" + HostAddressConfig.GetfileRouterAddress());
            }
            if (HostSettingConfig.GetValue("webapi") == "1")
            {
                text.AppendLine("WebAPI服务：" + HostAddressConfig.GetWebapiAddress());
            }

            text.AppendLine();
            text.AppendLine("[数据库连接]");
            text.AppendLine(HostDataBaseConfig.GetConnString());

            text.AppendLine();
            text.AppendLine("[通讯连接]");
            text.AppendLine("业务请求地址：" + HostAddressConfig.GetClientWcfAddress());
            text.AppendLine("文件传输地址：" + HostAddressConfig.GetClientFileAddress());

            text.AppendLine();
            text.AppendLine("[本地插件]");
            foreach (var p in AppPluginManage.PluginDic)
            {
                text.AppendLine(p.Key + "\t" + p.Value.plugin.title + "@" + p.Value.plugin.version);
            }

            text.AppendLine();
            text.AppendLine("[订阅者列表]");
            if (PublishServiceManage.subscriberList != null)
            {
                foreach (var p in PublishServiceManage.subscriberList)
                {
                    text.AppendLine(p.clientId + "\t" + p.publishServiceName);
                }
            }

            text.AppendLine();
            text.AppendLine("[远程插件]");
            if (RemotePluginManage.RemotePluginDic != null)
            {
                foreach (var p in RemotePluginManage.RemotePluginDic)
                {
                    text.AppendLine(p.ServerIdentify + "\t" + String.Join(",", p.plugin));
                }
            }

            text.AppendLine();
            text.AppendLine("[路由表]");
            text.AppendLine(HostRouterXml.GetXml());

            txtInfo.Text = text.ToString();
        }


    }
}
