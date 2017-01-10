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
using EFWCoreLib.WcfFrame.ServerManage;
using EFWCoreLib.WcfFrame.Utility;
using EFWCoreLib.WcfFrame.Utility.Mongodb;
using EFWCoreLib.WcfFrame.Utility.Nginx;
using EFWCoreLib.WcfFrame.WcfHandler;

namespace EFWCoreLib.WcfFrame
{
    public class WcfGlobal
    {
        /// <summary>
        /// 调试模式
        /// </summary>
        public static bool IsDebug = false;
        public static string Identify = "";//中间件唯一标识
        public static string HostName = "";//中间件显示名称
        public static bool IsToken = false;
        public static string ns = "http://www.efwplus.cn/";
        public static string MongoConnStr = "";//mongo连接字符串

        static ServiceHost mAppHost = null;
        static ServiceHost mFileHost = null;
        static ServiceHost mRouterHost = null;
        static ServiceHost mFileRouterHost = null;

        public static void Main()
        {
            IsDebug = HostSettingConfig.GetValue("debug") == "1" ? true : false;
            HostName = HostSettingConfig.GetValue("hostname");
            IsToken = HostSettingConfig.GetValue("token") == "1" ? true : false;
            MongoConnStr = HostSettingConfig.GetValue("mongodb_conn");

            WcfGlobal.Run(StartType.KillAllProcess);
            if (Convert.ToInt32(HostSettingConfig.GetValue("wcfservice")) == 1)
            {
                WcfGlobal.Run(StartType.BaseService);
            }
            if (Convert.ToInt32(HostSettingConfig.GetValue("filetransfer")) == 1)
            {
                WcfGlobal.Run(StartType.FileService);
            }

            if (Convert.ToInt32(HostSettingConfig.GetValue("router")) == 1)
            {
                WcfGlobal.Run(StartType.RouterBaseService);
                WcfGlobal.Run(StartType.RouterFileService);
            }

            WcfGlobal.Run(StartType.SuperClient);

            if (Convert.ToInt32(HostSettingConfig.GetValue("mongodb")) == 1)
            {
                WcfGlobal.Run(StartType.MongoDB);
            }

            if (Convert.ToInt32(HostSettingConfig.GetValue("timingtask")) == 1)
            {
                WcfGlobal.Run(StartType.MiddlewareTask);
            }
            
            WcfGlobal.Run(StartType.PublishService);
            if (Convert.ToInt32(HostSettingConfig.GetValue("nginx")) == 1)
            {
                WcfGlobal.Run(StartType.Nginx);
            }
        }

        public static void Exit()
        {
            WcfGlobal.Quit(StartType.PublishService);
            WcfGlobal.Quit(StartType.MiddlewareTask);
            WcfGlobal.Quit(StartType.SuperClient);
            WcfGlobal.Quit(StartType.BaseService);
            WcfGlobal.Quit(StartType.FileService);
            WcfGlobal.Quit(StartType.RouterBaseService);
            WcfGlobal.Quit(StartType.RouterFileService);
            WcfGlobal.Quit(StartType.MongoDB);
            WcfGlobal.Quit(StartType.Nginx);
        }

        public static void Run(StartType type)
        {
            
            switch (type)
            {
                case StartType.BaseService:
                    mAppHost = new ServiceHost(typeof(BaseService));
                    //初始化连接池,默认10分钟清理连接
                    ClientLinkPoolCache.Init(true, 200, 30, 600, "wcfserver", 30);

                    AppGlobal.AppRootPath = System.Windows.Forms.Application.StartupPath + "\\";
                    AppGlobal.appType = AppType.WCF;
                    AppGlobal.IsSaas = System.Configuration.ConfigurationManager.AppSettings["IsSaas"] == "true" ? true : false;
                    AppGlobal.AppStart();


                    ClientManage.IsHeartbeat = HostSettingConfig.GetValue("heartbeat") == "1" ? true : false;
                    ClientManage.HeartbeatTime = Convert.ToInt32(HostSettingConfig.GetValue("heartbeattime"));
                    ClientManage.IsMessage = HostSettingConfig.GetValue("message") == "1" ? true : false;
                    ClientManage.MessageTime = Convert.ToInt32(HostSettingConfig.GetValue("messagetime"));
                    ClientManage.IsCompressJson = HostSettingConfig.GetValue("compress") == "1" ? true : false;
                    ClientManage.IsEncryptionJson = HostSettingConfig.GetValue("encryption") == "1" ? true : false;
                    ClientManage.IsToken = HostSettingConfig.GetValue("token") == "1" ? true : false;
                    ClientManage.serializeType = (SerializeType)Convert.ToInt32(HostSettingConfig.GetValue("serializetype"));
                    ClientManage.IsOverTime = HostSettingConfig.GetValue("overtime") == "1" ? true : false;
                    ClientManage.OverTime = Convert.ToInt32(HostSettingConfig.GetValue("overtimetime"));

                    ClientManage.StartHost();
                    mAppHost.Open();

                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "数据服务启动完成");
                    break;

                case StartType.FileService:
                    AppGlobal.AppRootPath = System.Windows.Forms.Application.StartupPath + "\\";

                    mFileHost = new ServiceHost(typeof(FileService));
                    mFileHost.Open();

                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "文件服务启动完成");
                    break;
                case StartType.RouterBaseService:
                    mRouterHost = new ServiceHost(typeof(RouterBaseService));
                    RouterManage.Start();
                    mRouterHost.Open();

                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "数据路由服务启动完成");
                    break;
                case StartType.RouterFileService:
                    mFileRouterHost = new ServiceHost(typeof(RouterFileService));
                    mFileRouterHost.Open();

                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "文件路由服务启动完成");
                    break;
                case StartType.SuperClient:
                    SuperClient.CreateSuperClient();
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "超级客户端启动完成");
                    break;
                case StartType.MiddlewareTask:
                    MiddlewareTask.StartTask();//开启定时任务
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "定时任务启动完成");
                    break;
                case StartType.PublishService://订阅
                    PublishServiceManage.InitPublishService();
                    PublishSubManager.StartPublish();
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "发布订阅服务完成");
                    break;
                case StartType.MongoDB:
                    MongodbManager.StartDB();//开启MongoDB
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "MongoDB启动完成");
                    break;

                case StartType.Nginx:
                    NginxManager.StartWeb();//开启Nginx
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Blue, "Nginx启动完成");
                    break;
                case StartType.KillAllProcess:
                    MongodbManager.StopDB();//停止MongoDB  清理掉所有子进程，因为主进程关闭子进程不关闭的话，占用的端口号一样不会释放
                    NginxManager.StopWeb();
                    break;
            }

        }

        public static void Quit(StartType type)
        {
            ClientLinkManage.UnAllConnection();//关闭所有连接
            switch (type)
            {
                case StartType.BaseService:
                    try
                    {
                        if (mAppHost != null)
                        {
                            EFWCoreLib.WcfFrame.ClientLinkPoolCache.Dispose();
                            ClientManage.StopHost();
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
                    SuperClient.UnCreateSuperClient();
                    MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, "超级客户端已关闭！");
                    break;
                case StartType.MiddlewareTask:
                    MiddlewareTask.StopTask();//停止任务
                    MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, "定时任务已停止！");
                    break;
                case StartType.PublishService://订阅
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "订阅服务已停止");
                    break;
                case StartType.MongoDB:
                    MongodbManager.StopDB();//停止MongoDB
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "MongoDB已停止");
                    break;
                case StartType.Nginx:
                    NginxManager.StopWeb();
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "Nginx已停止");
                    break;
            }
        }
    }

    public enum StartType
    {
        BaseService,
        FileService,
        RouterBaseService,
        RouterFileService,
        MiddlewareTask,
        SuperClient,
        PublishService,
        MongoDB,
        Nginx,
        KillAllProcess
    }
}
