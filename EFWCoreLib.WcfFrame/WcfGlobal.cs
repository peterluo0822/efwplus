using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.WcfFrame.WcfService;

namespace EFWCoreLib.WcfFrame
{
    public class WcfGlobal
    {
        static ServiceHost mAppHost = null;
        static ServiceHost mFileHost = null;
        static ServiceHost mRouterHost = null;
        static ServiceHost mFileRouterHost = null;
        public static void Main(StartType type)
        {
            switch (type)
            {
                case StartType.BaseService:
                    mAppHost = new ServiceHost(typeof(WCFHandlerService));
                    //初始化连接池,默认10分钟清理连接
                    ClientLinkPoolCache.Init(true, 200, 30, 600, "wcfserver", 30);

                    AppGlobal.AppRootPath = System.Windows.Forms.Application.StartupPath + "\\";
                    AppGlobal.appType = AppType.WCF;
                    AppGlobal.IsSaas = System.Configuration.ConfigurationManager.AppSettings["IsSaas"] == "true" ? true : false;
                    AppGlobal.AppStart();


                    WcfServerManage.HostName = HostSettingConfig.GetValue("hostname");
                    WcfServerManage.IsDebug = HostSettingConfig.GetValue("debug") == "1" ? true : false;
                    WcfServerManage.IsHeartbeat = HostSettingConfig.GetValue("heartbeat") == "1" ? true : false;
                    WcfServerManage.HeartbeatTime = Convert.ToInt32(HostSettingConfig.GetValue("heartbeattime"));
                    WcfServerManage.IsMessage = HostSettingConfig.GetValue("message") == "1" ? true : false;
                    WcfServerManage.MessageTime = Convert.ToInt32(HostSettingConfig.GetValue("messagetime"));
                    WcfServerManage.IsCompressJson = HostSettingConfig.GetValue("compress") == "1" ? true : false;
                    WcfServerManage.IsEncryptionJson = HostSettingConfig.GetValue("encryption") == "1" ? true : false;
                    WcfServerManage.IsToken = HostSettingConfig.GetValue("token") == "1" ? true : false;
                    WcfServerManage.serializeType = (SerializeType)Convert.ToInt32(HostSettingConfig.GetValue("serializetype"));
                    WcfServerManage.IsOverTime = HostSettingConfig.GetValue("overtime") == "1" ? true : false;
                    WcfServerManage.OverTime = Convert.ToInt32(HostSettingConfig.GetValue("overtimetime"));

                    WcfServerManage.StartWCFHost();
                    mAppHost.Open();

                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "数据服务启动完成");
                    break;

                case StartType.FileService:
                    mFileHost = new ServiceHost(typeof(FileTransferHandlerService));
                    mFileHost.Open();

                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "文件服务启动完成");
                    break;
                case StartType.RouterBaseService:
                    mRouterHost = new ServiceHost(typeof(RouterHandlerService));
                    RouterServerManage.Start();
                    mRouterHost.Open();

                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "数据路由服务启动完成");
                    break;
                case StartType.RouterFileService:
                    mFileRouterHost = new ServiceHost(typeof(FileRouterHandlerService));
                    mFileRouterHost.Open();

                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "文件路由服务启动完成");
                    break;
                case StartType.SuperClient:
                    WcfServerManage.CreateSuperClient();
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "超级客户端启动完成");
                    break;
                case StartType.MiddlewareTask:
                    MiddlewareTask.StartTask();//开启定时任务
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "定时任务启动完成");
                    break;
            }

        }

        public static void Exit(StartType type)
        {
            switch (type)
            {
                case StartType.BaseService:
                    try {
                        if (mAppHost != null)
                        {
                            EFWCoreLib.WcfFrame.ClientLinkPoolCache.Dispose();
                            WcfServerManage.StopWCFHost();
                            mAppHost.Close();
                            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "数据服务已关闭！");
                        }
                    }
                    catch
                    {
                        if (mAppHost != null)
                            mAppHost.Abort();
                    }
                    break;

                case StartType.FileService:
                    try
                    {
                        if (mFileHost != null)
                        {
                            mFileHost.Close();
                            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "文件传输服务已关闭！");
                        }
                    }
                    catch
                    {
                        if (mFileHost != null)
                            mFileHost.Abort();
                    }
                    break;
                case StartType.RouterBaseService:
                    try
                    {
                        if (mRouterHost != null)
                        {
                            mRouterHost.Close();
                            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "数据路由服务已关闭！");
                        }
                    }
                    catch
                    {
                        if (mRouterHost != null)
                            mRouterHost.Abort();
                    }
                    break;
                case StartType.RouterFileService:
                    try
                    {
                        if (mFileRouterHost != null)
                        {
                            mFileRouterHost.Close();
                            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "文件路由服务已关闭！");
                        }
                    }
                    catch
                    {
                        if (mFileRouterHost != null)
                            mFileRouterHost.Abort();
                    }
                    break;
                case StartType.SuperClient:
                    WcfServerManage.UnCreateSuperClient();
                    MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, "超级客户端已关闭！");
                    break;
                case StartType.MiddlewareTask:
                    MiddlewareTask.StopTask();//停止任务
                    MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, "定时任务已停止！");
                    break;
            }
        }
    }

    public enum StartType
    {
        BaseService, FileService, RouterBaseService, RouterFileService, MiddlewareTask, SuperClient
    }
}
