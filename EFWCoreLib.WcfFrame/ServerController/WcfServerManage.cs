using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using System.Reflection;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Common;
using System.Drawing;
using EFWCoreLib.WcfFrame.ClientController;
using EFWCoreLib.CoreFrame.Plugin;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using EFWCoreLib.CoreFrame.SSO;
using EFWCoreLib.WcfFrame.SDMessageHeader;
using EFWCoreLib.WcfFrame.DataSerialize;

namespace EFWCoreLib.WcfFrame.ServerController
{
    /// <summary>
    /// WCF通讯服务端管理
    /// </summary>
    public class WcfServerManage
    {
        public static ClientLink superclient;//连接上级中间件的连接
        public static LocalPlgin localPlugin;//本地插件
        public static List<RemotePlugin> RemotePluginDic;//远程注册插件

        //客户端列表
        public static Dictionary<string, WCFClientInfo> wcfClientDic = new Dictionary<string, WCFClientInfo>();
        public static string Identify = "";//中间件唯一标识
        public static string HostName = "";//中间件显示名称
        public static bool IsDebug = false;//开始调试
        public static bool IsHeartbeat = false;//开启心跳
        public static int HeartbeatTime = 1;//默认间隔1秒,客户端5倍
        public static bool IsMessage = false;//开启业务消息
        public static int MessageTime = 1;//默认间隔1秒
        public static bool IsCompressJson = false;//是否压缩Json数据
        public static bool IsEncryptionJson = false;//是否加密Json数据
        public static bool IsToken = false;//是否开启身份验证
        public static SerializeType serializeType = SerializeType.Newtonsoft;//序列化方式
        public static bool IsOverTime = false;//开启超时记录
        public static int OverTime = 1;//超时记录日志

        /// <summary>
        /// 开始服务主机
        /// </summary>
        public static void StartWCFHost()
        {
            hostwcfMsg(Color.Blue, DateTime.Now, "WCFHandlerService服务正在初始化...");

            WcfGlobal.Main();

            //初始化连接池,默认10分钟清理连接
            ClientLinkPoolCache.Init(true, 200, 30, 600, "wcfserver", 30);

            //初始化插件
            localPlugin = new LocalPlgin();
            RemotePluginDic = new List<RemotePlugin>();

            localPlugin.ServerIdentify = WcfServerManage.Identify;
            localPlugin.PluginDic = AppPluginManage.PluginDic;

            hostwcfMsg(Color.Blue, DateTime.Now, "WCFHandlerService服务初始化完成");

            if (IsHeartbeat == true)
            {
                if (timer == null)
                    StartListenClients();
                else
                    timer.Start();
            }
            else
            {
                if (timer != null)
                    timer.Stop();
            }
        }
        /// <summary>
        /// 停止服务主机
        /// </summary>
        public static void StopWCFHost()
        {
            foreach (WCFClientInfo client in wcfClientDic.Values)
            {
                client.IsConnect = false;
            }
        }

        #region 服务操作
        public static string CreateClient(string sessionId, string ipaddress, DateTime time, IClientService clientService, string plugin, string replyidentify)
        {
            string clientId = Guid.NewGuid().ToString();

            try
            {
                AddClient(sessionId, clientId, ipaddress, time, clientService, plugin, replyidentify);
                hostwcfclientinfoList(wcfClientDic.Values.ToList());
                
                return clientId;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Source + "：创建客户端运行环境失败！");
            }
        }
        public static bool Heartbeat(string clientId)
        {   
            bool b= UpdateHeartbeatClient(clientId);
            if (b == true)
            {
                ReConnectionClient(clientId);
                return true;
            }
            else
                return false;
        }
        public static string ProcessRequest(string clientId, string plugin, string controller, string method, string jsondata, HeaderParameter para)
        {
            string retJson = null;
            //WCFClientInfo ClientInfo = null;
            try
            {
                if (plugin == null || controller == null)
                    throw new Exception("插件名称或控制器名称不能为空!");

                if (wcfClientDic.ContainsKey(clientId) == false)
                    throw new Exception("客户端不存在，正在创建新的连接！");

                if (WcfServerManage.IsToken == true)//非调试模式下才验证
                {
                    //验证身份，创建连接的时候验证，请求不验证
                    IsAuth(plugin, controller, method, para.token);
                }

                //显示调试信息
                if (WcfServerManage.IsDebug == true)
                    ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + clientId + "]正在执行：" + controller + "." + method + "(" + jsondata + ")");


                begintime();

                #region 执行插件控制器的核心算法
                object[] paramValue = null;//jsondata?
                ServiceResponseData retObj = null;
                if (string.IsNullOrEmpty(para.replyidentify) || localPlugin.ServerIdentify == para.replyidentify)
                {
                    if (localPlugin.PluginDic.ContainsKey(plugin) == true)
                    {
                        //先解密再解压
                        string _jsondata = jsondata;
                        //解密参数
                        if (para.isencryptionjson)
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = _jsondata;
                            des.DesDecrypt();
                            _jsondata = des.OutString;
                        }
                        //解压参数
                        if (para.iscompressjson)
                        {
                            _jsondata = ZipComporessor.Decompress(_jsondata);
                        }

                        ClientRequestData requestData = new ClientRequestData(para.iscompressjson, para.isencryptionjson, para.serializetype);
                        requestData.SetJsonData(_jsondata);
                        requestData.LoginRight = para.LoginRight;

                        EFWCoreLib.CoreFrame.Plugin.ModulePlugin moduleplugin = localPlugin.PluginDic[plugin];
                        retObj = (ServiceResponseData)moduleplugin.WcfServerExecuteMethod(controller, method, paramValue, requestData);

                        if (retObj != null)
                        {
                            retJson = retObj.GetJsonData();
                        }
                        else
                        {
                            retObj = new ServiceResponseData();
                            retObj.Iscompressjson = para.iscompressjson;
                            retObj.Isencryptionjson = para.isencryptionjson;
                            retObj.Serializetype = para.serializetype;

                            retJson = retObj.GetJsonData();
                        }

                        retJson = "{\"flag\":0,\"msg\":" + "\"\"" + ",\"data\":" + retJson + "}";
                        //先压缩再加密
                        //压缩结果
                        if (para.iscompressjson)
                        {
                            retJson = ZipComporessor.Compress(retJson);
                        }
                        //加密结果
                        if (para.isencryptionjson)
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = retJson;
                            des.DesEncrypt();
                            retJson = des.OutString;
                        }
                    }
                    else
                        throw new Exception("本地插件找不到指定的插件");
                }
                else//本地插件找不到，就执行远程插件
                {
                    if (RemotePluginDic.FindIndex(x => x.ServerIdentify == para.replyidentify) > -1)
                    {
                        RemotePlugin rp = RemotePluginDic.Find(x => x.ServerIdentify == para.replyidentify);
                        string[] ps = rp.plugin;

                        if (ps.ToList().FindIndex(x => x == plugin) > -1)
                        {
                            retJson = rp.clientService.SuperReplyClient(para, plugin, controller, method, jsondata);
                        }
                        else
                            throw new Exception("远程插件找不到指定的插件");
                    }
                    else
                        throw new Exception("远程插件找不到指定的回调中间件");
                }
                #endregion

                double outtime = endtime();
                //记录超时的方法
                if (WcfServerManage.IsOverTime == true)
                {
                    if (outtime > Convert.ToDouble(WcfServerManage.OverTime * 1000))
                    {
                        WriterOverTimeLog(outtime, controller + "." + method + "(" + jsondata + ")");
                    }
                }
                //显示调试信息
                if (WcfServerManage.IsDebug == true)
                    ShowHostMsg(Color.Green, DateTime.Now, "客户端[" + clientId + "]收到结果(耗时[" + outtime + "])：" + retJson);

                //更新客户端信息
                UpdateRequestClient(clientId, jsondata == null ? 0 : jsondata.Length, retJson == null ? 0 : retJson.Length);


                if (retJson == null)
                    throw new Exception("插件执行未返回有效数据");

                return retJson;
            }
            catch (Exception err)
            {
                //记录错误日志
                if (err.InnerException == null)
                {
                    retJson = "{\"flag\":1,\"msg\":" + "\"" + err.Message + "\"" + "}";
                    if (para.iscompressjson)
                    {
                        retJson = ZipComporessor.Compress(retJson);
                    }
                    ShowHostMsg(Color.Red, DateTime.Now, "客户端[" + clientId + "]执行失败：" + err.Message);
                    return retJson;
                }
                else
                {
                    retJson = "{\"flag\":1,\"msg\":" + "\"" + err.InnerException.Message + "\"" + "}";
                    if (para.iscompressjson)
                    {
                        retJson = ZipComporessor.Compress(retJson);
                    }
                    ShowHostMsg(Color.Red, DateTime.Now, "客户端[" + clientId + "]执行失败：" + err.InnerException.Message);
                    return retJson;
                }
            }
        }
        public static bool UnClient(string clientId)
        {
            RemoveClient(clientId);
            hostwcfclientinfoList(wcfClientDic.Values.ToList());
            return true;
        }
        public static void SendBroadcast(string jsondata)
        {
            lock (wcfClientDic)
            {
                foreach (WCFClientInfo client in wcfClientDic.Values)
                {
                    IClientService mCallBack = client.callbackClient;
                    //?
                    mCallBack.ReplyClient(jsondata);
                }
            }
        }
        public static string ServerConfig()
        {
            string IsHeartbeat = WcfServerManage.IsHeartbeat ? "1" : "0";
            string HeartbeatTime = WcfServerManage.HeartbeatTime.ToString();
            string IsMessage = WcfServerManage.IsMessage ? "1" : "0";
            string MessageTime = WcfServerManage.MessageTime.ToString();
            string IsCompressJson = WcfServerManage.IsCompressJson ? "1" : "0";
            string IsEncryptionJson = WcfServerManage.IsEncryptionJson ? "1" : "0";
            string serializetype = ((int)WcfServerManage.serializeType).ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append(IsHeartbeat);
            sb.Append("#");
            sb.Append(HeartbeatTime);
            sb.Append("#");
            sb.Append(IsMessage);
            sb.Append("#");
            sb.Append(MessageTime);
            sb.Append("#");
            sb.Append(IsCompressJson);
            sb.Append("#");
            sb.Append(IsEncryptionJson);
            sb.Append("#");
            sb.Append(serializetype);
            return sb.ToString();
        }

        //每次请求的身份验证，分布式情况下验证麻烦
        static bool IsAuth(string pname, string cname, string methodname, string token)
        {
            ModulePlugin mp;
            WcfControllerAttributeInfo cattr = AppPluginManage.GetPluginWcfControllerAttributeInfo(pname, cname, out mp);
            if (cattr == null) throw new Exception("插件中没有此控制器名");
            WcfMethodAttributeInfo mattr = cattr.MethodList.Find(x => x.methodName == methodname);
            if (mattr == null) throw new Exception("控制器中没有此方法名");

            if (mattr.IsAuthentication)
            {
                if (token == null)
                    throw new Exception("no token");

                AuthResult result = SsoHelper.ValidateToken(token);
                if (result.ErrorMsg != null)
                    throw new Exception(result.ErrorMsg);

                SysLoginRight loginInfo = new SysLoginRight();
                loginInfo.UserId = Convert.ToInt32(result.User.UserId);
                loginInfo.EmpName = result.User.UserName;

                //clientinfo.LoginRight = loginInfo;
            }

            return true;
        }
        #endregion

        #region 界面显示
        public static HostWCFClientInfoListHandler hostwcfclientinfoList;
        public static HostWCFMsgHandler hostwcfMsg;
        private static void AddClient(string sessionId, string clientId, string ipaddress, DateTime time, IClientService clientService, string plugin, string replyidentify)
        {
            WCFClientInfo info = new WCFClientInfo();
            info.clientId = clientId;
            info.ipAddress = ipaddress;
            info.startTime = time;
            info.callbackClient = clientService;
            info.IsConnect = true;
            info.plugin = plugin;
            info.ServerIdentify = replyidentify;
            lock (wcfClientDic)
            {
                wcfClientDic.Add(clientId, info);
            }
            ShowHostMsg(Color.Blue, DateTime.Now, "客户端[" + ipaddress + "]已连接WCF服务主机");
        }
        private static bool UpdateRequestClient(string clientId, int rlen, int slen)
        {
            lock (wcfClientDic)
            {
                if (wcfClientDic.ContainsKey(clientId))
                {

                    wcfClientDic[clientId].RequestCount += 1;
                    wcfClientDic[clientId].receiveData += rlen;
                    wcfClientDic[clientId].sendData += slen;
                }
            }
            return true;
        }
        private static bool UpdateHeartbeatClient(string clientId)
        {
            lock (wcfClientDic)
            {
                if (wcfClientDic.ContainsKey(clientId))
                {

                    wcfClientDic[clientId].startTime = DateTime.Now;
                    wcfClientDic[clientId].HeartbeatCount += 1;
                    return true;
                }
                else
                    return false;
            }
        }
        private static void RemoveClient(string clientId)
        {
            lock (wcfClientDic)
            {
                if (wcfClientDic.ContainsKey(clientId))
                {

                    wcfClientDic.Remove(clientId);
                    ShowHostMsg(Color.Blue, DateTime.Now, "客户端[" + clientId + "]已退出断开连接WCF服务主机");
                }
            }
        }
        private static void ReConnectionClient(string clientId)
        {
            lock (wcfClientDic)
            {
                if (wcfClientDic.ContainsKey(clientId))
                {
                    if (wcfClientDic[clientId].IsConnect == false)
                    {
                        ShowHostMsg(Color.Blue,DateTime.Now, "客户端[" + clientId + "]已重新连接WCF服务主机");
                        wcfClientDic[clientId].IsConnect = true;
                    }
                }
            }
        }
        private static void DisConnectionClient(string clientId)
        {
            lock (wcfClientDic)
            {
                if (wcfClientDic.ContainsKey(clientId))
                {

                    if (wcfClientDic[clientId].IsConnect == true)
                    {
                        wcfClientDic[clientId].IsConnect = false;
                        ShowHostMsg(Color.Blue,DateTime.Now, "客户端[" + clientId + "]已超时断开连接WCF服务主机");
                    }
                }
            }
        }
        private static void ShowHostMsg(Color clr, DateTime time, string text)
        {
            //hostwcfMsg.BeginInvoke(clr, time, text, null, null);//异步方式不影响后台数据请求
            hostwcfMsg(clr, time, text);
        }
        #endregion

        #region 定时刷新界面
        //检测客户端是否在线，超时时间为10s
        static System.Timers.Timer timer;
        private static void StartListenClients()
        {
            timer = new System.Timers.Timer();
            timer.Interval = WcfServerManage.HeartbeatTime*1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        static Object syncObj = new Object();////定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (syncObj)
                {
                    foreach (WCFClientInfo client in wcfClientDic.Values)
                    {
                        if (client.startTime.AddSeconds(WcfServerManage.HeartbeatTime * 10) < DateTime.Now)//断开10秒就置为断开
                        {
                            DisConnectionClient(client.clientId);
                        }

                        if (client.startTime.AddSeconds(WcfServerManage.HeartbeatTime * 20) < DateTime.Now)//断开10分钟直接移除客户端
                        {
                            RemoveClient(client.clientId);
                        }
                    }
                    hostwcfclientinfoList(wcfClientDic.Values.ToList());
                    WriterOverTimeFile();
                }
            }
            catch { }
        }
        #endregion

        #region 超时服务请求记录
        static DateTime begindate;
        static void begintime()
        {
            begindate = DateTime.Now;
        }
        //返回毫秒
        static double endtime()
        {
            return DateTime.Now.Subtract(begindate).TotalMilliseconds;
        }
        static StringBuilder overtimesb = new StringBuilder();
        static void WriterOverTimeLog(double overtime, string text)
        {
            string info = "时间：" + DateTime.Now.ToString() + "\t\t" + "耗时：" + overtime + "\t\t" + "方法：" + text + "\r\n";
            lock (overtimesb)
            {
                overtimesb.AppendLine(info);
            }
        }
        static void WriterOverTimeFile()
        {
            string info = null;
            lock (overtimesb)
            {
                info = overtimesb.ToString();
                if (info != null)
                    overtimesb.Clear();
            }
            if (string.IsNullOrEmpty(info) == false)
            {
                string filepath = AppGlobal.AppRootPath + "OverTimeLog\\" + DateTime.Now.ToString("yyyyMM") + ".txt";
                if (System.IO.Directory.Exists(AppGlobal.AppRootPath + "OverTimeLog") == false)
                {
                    System.IO.Directory.CreateDirectory(AppGlobal.AppRootPath + "OverTimeLog");
                }
                System.IO.File.AppendAllText(filepath, info);
            }
        }
        #endregion

        #region 创建中间件与中间件之间的超级连接,通过EFWCore名称来转发
        public static void CreateSuperClient()
        {
            //就算上级中间件重启了，下级中间件创建链接的时候会重新注册本地插件
            superclient = new ClientLink(WcfServerManage.HostName, (() =>
            {
                //注册本地插件到上级中间件
                superclient.RegisterReplyPlugin(WcfServerManage.Identify, localPlugin.PluginDic.Keys.ToArray());
                (superclient.mConn.ClientService as ReplyClientCallBack).SuperReplyClientAction = ((HeaderParameter para, string plugin, string controller, string method, string jsondata) =>
                {
                    return ProcessReply(para, plugin, controller, method, jsondata);
                });
                //同步缓存到上级中间件
                DistributedCacheManage.SyncAllCache();
            }));
            try
            {
                superclient.CreateConnection();
            }
            catch
            {
                ShowHostMsg(Color.Red, DateTime.Now, "连接上级中间件失败！");
            }
        }

        public static void UnCreateSuperClient()
        {
            if (superclient != null)
                superclient.Dispose();
        }

        public static void RegisterReplyPlugin(IClientService callback, string ServerIdentify, string[] plugin)
        {
            //自己没必要注册自己
            if (ServerIdentify == WcfServerManage.Identify) return;
            bool isChanged = false;
            RemotePlugin rp = null;
            if (WcfServerManage.RemotePluginDic.FindIndex(x => x.ServerIdentify == ServerIdentify) > -1)
            {
                rp = WcfServerManage.RemotePluginDic.Find(x => x.ServerIdentify == ServerIdentify);
                //rp.clientService = callback;

                List<string> newplugin = rp.plugin.ToList();
                foreach (var p in plugin)
                {
                    //新注册的插件在原来插件中找不到，则新增
                    if (newplugin.ToList().FindIndex(x => x == p) == -1)
                    {
                        newplugin.Add(p);
                        isChanged = true;
                    }
                }
                foreach (var p in newplugin)
                {
                    //如果注册的插件在新注册插件中找不到，则移除
                    if (plugin.ToList().FindIndex(x => x == p) == -1)
                    {
                        newplugin.Remove(p);
                        isChanged = true;
                    }
                }
                rp.plugin = newplugin.ToArray();
            }
            else
            {
                rp = new RemotePlugin();
                rp.ServerIdentify = ServerIdentify;
                rp.clientService = callback;
                rp.plugin = plugin;
                WcfServerManage.RemotePluginDic.Add(rp);
                isChanged = true;
            }

            if (isChanged==true)
            {
                //重新注册远程插件
                foreach (var p in WcfServerManage.RemotePluginDic)
                {
                    superclient.RegisterReplyPlugin(p.ServerIdentify, p.plugin);
                }
            }
        }

        private static string ProcessReply(HeaderParameter para, string plugin, string controller, string method, string jsondata)
        {
            string retJson = null;
            
            try
            {

                //显示调试信息
                if (WcfServerManage.IsDebug == true)
                    ShowHostMsg(Color.Black, DateTime.Now, "客户端[本地回调]正在执行：" + controller + "." + method + "(" + jsondata + ")");

                begintime();

                #region 执行插件控制器的核心算法
                object[] paramValue = null;//jsondata?
                ServiceResponseData retObj = null;
                if (string.IsNullOrEmpty(para.replyidentify) || localPlugin.ServerIdentify == para.replyidentify)
                {
                    if (localPlugin.PluginDic.ContainsKey(plugin) == true)
                    {
                        //先解密再解压
                        string _jsondata = jsondata;
                        //解密参数
                        if (para.isencryptionjson)
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = _jsondata;
                            des.DesDecrypt();
                            _jsondata = des.OutString;
                        }
                        //解压参数
                        if (para.iscompressjson)
                        {
                            _jsondata = ZipComporessor.Decompress(_jsondata);
                        }

                        ClientRequestData requestData = new ClientRequestData(para.iscompressjson, para.isencryptionjson, para.serializetype);
                        requestData.SetJsonData(_jsondata);
                        requestData.LoginRight = para.LoginRight;

                        EFWCoreLib.CoreFrame.Plugin.ModulePlugin moduleplugin = localPlugin.PluginDic[plugin];
                        retObj = (ServiceResponseData)moduleplugin.WcfServerExecuteMethod(controller, method, paramValue, requestData);

                        if (retObj != null)
                        {
                            retJson = retObj.GetJsonData();
                        }
                        else
                        {
                            retObj = new ServiceResponseData();
                            retObj.Iscompressjson = para.iscompressjson;
                            retObj.Isencryptionjson = para.isencryptionjson;
                            retObj.Serializetype = para.serializetype;

                            retJson = retObj.GetJsonData();
                        }

                        retJson = "{\"flag\":0,\"msg\":" + "\"\"" + ",\"data\":" + retJson + "}";
                        //先压缩再加密
                        //压缩结果
                        if (para.iscompressjson)
                        {
                            retJson = ZipComporessor.Compress(retJson);
                        }
                        //加密结果
                        if (para.isencryptionjson)
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = retJson;
                            des.DesEncrypt();
                            retJson = des.OutString;
                        }
                    }
                    else
                        throw new Exception("本地插件找不到指定的插件");
                }
                else//本地插件找不到，就执行远程插件
                {
                    if (RemotePluginDic.FindIndex(x => x.ServerIdentify == para.replyidentify) > -1)
                    {
                        RemotePlugin rp = RemotePluginDic.Find(x => x.ServerIdentify == para.replyidentify);
                        string[] ps = rp.plugin;

                        if (ps.ToList().FindIndex(x => x == plugin) > -1)
                        {
                            retJson = rp.clientService.SuperReplyClient(para, plugin, controller, method, jsondata);
                        }
                        else
                            throw new Exception("远程插件找不到指定的插件");
                    }
                    else
                        throw new Exception("远程插件找不到指定的回调中间件");
                }
                #endregion

                //System.Threading.Thread.Sleep(20000);//测试并发问题，此处也没有问题
                double outtime = endtime();
                //记录超时的方法
                if (WcfServerManage.IsOverTime == true)
                {
                    if (outtime > Convert.ToDouble(WcfServerManage.OverTime * 1000))
                    {
                        WriterOverTimeLog(outtime, controller + "." + method + "(" + jsondata + ")");
                    }
                }
                //显示调试信息
                if (WcfServerManage.IsDebug == true)
                    ShowHostMsg(Color.Green, DateTime.Now, "客户端[本地回调]收到结果(耗时[" + outtime + "])：" + retJson);

                if (retJson == null)
                    throw new Exception("请求的插件未获取到有效数据");
               
                return retJson;
            }
            catch (Exception err)
            {
                //记录错误日志
                //EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");

                if (err.InnerException == null)
                {
                    retJson = "{\"flag\":1,\"msg\":" + "\"" + err.Message + "\"" + "}";
                    if (para.iscompressjson)
                    {
                        retJson = ZipComporessor.Compress(retJson);
                    }
                    ShowHostMsg(Color.Red, DateTime.Now, "客户端[本地回调]执行失败：" + err.Message);
                    return retJson;
                }
                else
                {
                    retJson = "{\"flag\":1,\"msg\":" + "\"" + err.InnerException.Message + "\"" + "}";
                    if (para.iscompressjson)
                    {
                        retJson = ZipComporessor.Compress(retJson);
                    }
                    ShowHostMsg(Color.Red, DateTime.Now, "客户端[本地回调]执行失败：" + err.InnerException.Message);
                    return retJson;
                }

            }
        }
        #endregion

    }

    /// <summary>
    /// 连接客户端信息
    /// </summary>
    public class WCFClientInfo : MarshalByRefObject,ICloneable
    {
        public string clientId { get; set; }
        public string ipAddress { get; set; }
        public DateTime startTime { get; set; }
        public IClientService callbackClient { get; set; }
        //public SysLoginRight LoginRight { get; set; }
        //这个不能放这里,重新创建连接和分布式调用都会丢失LoginRight
        public int HeartbeatCount { get; set; }
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnect { get; set; }
        /// <summary>
        /// 请求次数
        /// </summary>
        public int RequestCount { get; set; }
        /// <summary>
        /// 接收数据
        /// </summary>
        public long receiveData { get; set; }
        /// <summary>
        /// 发送数据
        /// </summary>
        public long sendData { get; set; }
        /// <summary>
        /// 插件名称
        /// </summary>
        public string plugin { get; set; }
        /// <summary>
        /// 中间件标识，只有超级客户端才有值
        /// </summary>
        public string ServerIdentify { get; set; }


        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    public delegate void HostWCFClientInfoListHandler(List<WCFClientInfo> dic);
    public delegate void HostWCFMsgHandler(System.Drawing.Color clr, DateTime time, string text);

    /// <summary>
    /// 本地插件
    /// </summary>
    public class LocalPlgin
    {
        public string ServerIdentify { get; set; }
        public Dictionary<string, ModulePlugin> PluginDic { get; set; }
    }

    /// <summary>
    /// 远程插件
    /// </summary>
    public class RemotePlugin
    {
        public string ServerIdentify { get; set; }
        public string[] plugin { get; set; }
        public EFWCoreLib.WcfFrame.WcfService.Contract.IClientService clientService { get; set; }
    }
    
}
