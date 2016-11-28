using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;

namespace EFWCoreLib.WcfFrame.ServerManage
{
    /// <summary>
    /// 超级客户端
    /// </summary>
    public class SuperClient
    {
        public static ClientLink superclient;//连接上级中间件超级客户端

        public static void CreateSuperClient()
        {
            //就算上级中间件重启了，下级中间件创建链接的时候会重新注册本地插件
            superclient = new ClientLink(WcfGlobal.HostName);
            try
            {
                superclient.CreateConnection((() =>
                {
                    //以后可以做成配置方式
                    foreach (var item in PublishServiceManage.serviceDic.Keys)
                    {
                        superclient.Subscribe(item);//订阅服务?
                    }
                }), null);
            }
            catch (Exception e)
            {
                MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "连接上级中间件失败！" + e.Message);
            }
        }

        public static void UnCreateSuperClient()
        {
            if (superclient != null)
                superclient.Dispose();
        }
    }
}
