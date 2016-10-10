using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.SDMessageHeader;
using EFWCoreLib.WcfFrame.WcfHandler;
using Newtonsoft.Json;

namespace EFWCoreLib.WcfFrame
{
    /// <summary>
    /// 客户端连接对象，一个对象一个会话通道
    /// </summary>
    public class ClientLink : IDisposable
    {
        /// <summary>
        /// 服务插件名称
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        /// 平台连接对象
        /// </summary>
        public CHDEPConnection mConn { get; set; }

        /// <summary>
        /// 文件下载存放路径
        /// </summary>
        private string filebufferpath = System.Windows.Forms.Application.StartupPath + "\\filebuffer\\";
        private readonly string myNamespace = "http://www.efwplus.cn/";
        private DuplexChannelFactory<IClientHandler> mChannelFactory;
        private ChannelFactory<IFileHandler> mfileChannelFactory = null;
        private Action<bool, int> backConfig = null;//参数配置回调
        private Action createconnAction = null;//创建连接后回调
        private string wcfendpoint = "wcfendpoint";//服务地址
        private string fileendpoint = "fileendpoint";//文件服务地址

        /// <summary>
        /// 初始化通讯连接
        /// </summary>
        /// <param name="pluginname">插件名称</param>
        public ClientLink(string pluginname)
        {
            InitComm(null, pluginname);
        }
        /// <summary>
        /// 初始化通讯连接
        /// </summary>
        /// <param name="clientname">客户端名称</param>
        /// <param name="pluginname">插件名称</param>
        public ClientLink(string clientname,string pluginname)
        {
            InitComm(clientname,pluginname);
        }
        /// <summary>
        /// 初始化通讯连接
        /// </summary>
        /// <param name="clientname">客户端名称</param>
        /// <param name="pluginname">插件名称</param>
        /// <param name="actionConfig">获取消息配置</param>
        public ClientLink(string clientname, string pluginname, Action<bool, int> actionConfig)
        {
            backConfig = actionConfig;
            InitComm(clientname, pluginname);
        }

        public ClientLink(string clientname, string pluginname, Action<bool, int> actionConfig,string _wcfendpoint)
        {
            backConfig = actionConfig;
            wcfendpoint = _wcfendpoint;
            InitComm(clientname, pluginname);
        }

        /// <summary>
        /// 初始化通讯连接
        /// </summary>
        /// <param name="clientname">客户端名称</param>
        /// <param name="_createconnAction">创建连接回调</param>
        public ClientLink(string clientname, Action _createconnAction)
        {
            createconnAction = _createconnAction;
            InitComm(clientname, null);
        }


        private void InitComm(string clientname, string pluginname)
        {
            if (string.IsNullOrEmpty(clientname))
                clientname = getLocalIPAddress();
            if (string.IsNullOrEmpty(pluginname))
                pluginname = "SuperPlugin";

            PluginName = pluginname;

            mConn = new CHDEPConnection();
            mConn.ClientName = clientname;
            mConn.RouterID = Guid.NewGuid().ToString();
            mConn.PluginName = pluginname;
            mConn.ClientService = new ReplyDataCallback();

            if (mChannelFactory == null)
                mChannelFactory = new DuplexChannelFactory<IClientHandler>(mConn.ClientService, wcfendpoint);
            if (mfileChannelFactory == null)
                mfileChannelFactory = new ChannelFactory<IFileHandler>(fileendpoint);
        }

        #region IDisposable 成员
        /// <summary>
        /// 释放连接
        /// </summary>
        ~ClientLink()
        {
            Dispose();
        } 
        /// <summary>
        /// 释放连接
        /// </summary>
        public void Dispose()
        {
            UnConnection();

            try
            {
                if (mChannelFactory != null)
                    mChannelFactory.Close();
                if (mfileChannelFactory != null)
                    mfileChannelFactory.Close();
            }
            catch
            {
                if (mChannelFactory != null)
                    mChannelFactory.Abort();
                if (mfileChannelFactory != null)
                    mfileChannelFactory.Abort();
            }
        }

        #endregion

        #region 连接池属性
        private int index;
        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        bool isUsed = false;
        /// <summary>
        /// 是否在使用
        /// </summary>
        public bool IsUsed
        {
            get { return isUsed; }
            set { isUsed = value; }
        }

        DateTime createdTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        DateTime lastUsedTime;
        /// <summary>
        /// 最后使用时间
        /// </summary>
        public DateTime LastUsedTime
        {
            get { return lastUsedTime; }
            set { lastUsedTime = value; }
        }

        int usedNums;
        /// <summary>
        /// 使用次数
        /// </summary>
        public int UsedNums
        {
            get { return usedNums; }
            set { usedNums = value; }
        }

        public CommunicationState State {
            get { return mChannelFactory.State; }
        }
        #endregion

        #region 数据交互

        private bool IsHeartbeat = false;
        private int HeartbeatTime = 1;//默认间隔1秒,客户端5倍
        private bool IsMessage = false;
        private int MessageTime = 1;//默认间隔1秒
        private bool IsCompressJson = false;//是否压缩Json数据
        private bool IsEncryptionJson = false;//是否加密Json数据
        private SerializeType serializeType = SerializeType.Newtonsoft;//序列化方式

        private bool ServerConfigRequestState = false;//获取服务端配置读取状态

        /// <summary>
        /// 创建连接
        /// </summary>
        public void CreateConnection()
        {
            IClientHandler wcfHandlerService = mChannelFactory.CreateChannel();
            mConn.WcfService = wcfHandlerService;
            string serverConfig = null;

            AddMessageHeader(wcfHandlerService as IContextChannel, "", (() =>
            {

                mConn.ClientID = wcfHandlerService.CreateClient(mConn.ClientName);//创建连接获取ClientID
                if (ServerConfigRequestState == false)
                {
                    //重新获取服务端配置，如：是否压缩Json、是否加密Json
                    serverConfig = wcfHandlerService.MiddlewareConfig();
                    ServerConfigRequestState = true;
                }
            }));

            if (!string.IsNullOrEmpty(serverConfig))
            {
                IsHeartbeat = serverConfig.Split(new char[] { '#' })[0] == "1" ? true : false;
                HeartbeatTime = Convert.ToInt32(serverConfig.Split(new char[] { '#' })[1]);
                IsMessage = serverConfig.Split(new char[] { '#' })[2] == "1" ? true : false;
                MessageTime = Convert.ToInt32(serverConfig.Split(new char[] { '#' })[3]);
                IsCompressJson = serverConfig.Split(new char[] { '#' })[4] == "1" ? true : false;
                IsEncryptionJson = serverConfig.Split(new char[] { '#' })[5] == "1" ? true : false;
                serializeType = (SerializeType)Convert.ToInt32(serverConfig.Split(new char[] { '#' })[6]);
                if (IsHeartbeat)
                {
                    //开启发送心跳
                    if (timer == null)
                        StartTimer();
                    else
                        timer.Start();
                }
                else
                {
                    if (timer != null)
                        timer.Stop();
                }
            }

            if (backConfig != null)
                backConfig(IsMessage, MessageTime);

            //创建连接成功后回调
            if (createconnAction != null)
                createconnAction.BeginInvoke(null,null);
        }

        /// <summary>
        /// 向服务发送请求
        /// </summary>
        /// <param name="controller">控制器名称</param>
        /// <param name="method">方法名称</param>
        /// <param name="requestAction">数据</param>
        /// <returns>返回Json数据</returns>
        public ServiceResponseData Request(string controller, string method, Action<ClientRequestData> requestAction)
        {
            if (mConn == null) throw new Exception("还没有创建连接！");
            while (mConn.ClientID == null)//解决并发问题
            {
                Thread.Sleep(100);
            }
            try
            {
                ClientRequestData requestData = new ClientRequestData(IsCompressJson, IsEncryptionJson, serializeType);
                if (requestAction != null)
                    requestAction(requestData);

                string jsondata = requestData.GetJsonData();//获取序列化的请求数据

                if (requestData.Iscompressjson)//开启压缩
                {
                    jsondata = ZipComporessor.Compress(jsondata);//压缩传入参数
                }
                if (requestData.Isencryptionjson)//开启加密
                {
                    DESEncryptor des = new DESEncryptor();
                    des.InputString = jsondata;
                    des.DesEncrypt();
                    jsondata = des.OutString;
                }

                IClientHandler _wcfService = mConn.WcfService;
                string retJson = "";

                AddMessageHeader(_wcfService as IContextChannel, "", requestData.Iscompressjson, requestData.Isencryptionjson, requestData.Serializetype, requestData.LoginRight, (() =>
                   {
                       retJson = _wcfService.ProcessRequest(mConn.ClientID, mConn.PluginName, controller, method, jsondata);
                   }));

                if (requestData.Isencryptionjson)//解密结果
                {
                    DESEncryptor des = new DESEncryptor();
                    des.InputString = retJson;
                    des.DesDecrypt();
                    retJson = des.OutString;
                }
                if (requestData.Iscompressjson)//解压结果
                {
                    retJson = ZipComporessor.Decompress(retJson);
                }

                new Action(delegate ()
                {
                    if (IsHeartbeat == false)//如果没有启动心跳，则请求发送心跳
                    {
                        ServerConfigRequestState = false;
                        Heartbeat();
                    }
                }).BeginInvoke(null, null);//异步执行

                string retData = "";
                object Result = JsonConvert.DeserializeObject(retJson);
                int ret = Convert.ToInt32((((Newtonsoft.Json.Linq.JObject)Result)["flag"]).ToString());
                string msg = (((Newtonsoft.Json.Linq.JObject)Result)["msg"]).ToString();
                if (ret == 1)
                {
                    throw new Exception(msg);
                }
                else
                {
                    retData = ((Newtonsoft.Json.Linq.JObject)(Result))["data"].ToString();
                }

                ServiceResponseData responsedata = new ServiceResponseData();
                responsedata.Iscompressjson = requestData.Iscompressjson;
                responsedata.Isencryptionjson = requestData.Isencryptionjson;
                responsedata.Serializetype = requestData.Serializetype;
                responsedata.SetJsonData(retData);

                return responsedata;
            }
            catch (Exception e)
            {
                ServerConfigRequestState = false;
                ReConnection(true);//连接服务主机失败，重连
                //throw new Exception(e.Message + "\n连接服务主机失败，请联系管理员！");
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// 向服务发送异步请求
        /// 客户端建议不要用多线程，都采用异步请求方式
        /// </summary>
        /// <param name="controller">插件名@控制器名称</param>
        /// <param name="method">方法名称</param>
        /// <param name="jsondata">数据</param>
        /// <returns>返回Json数据</returns>
        public IAsyncResult RequestAsync(string controller, string method, Action<ClientRequestData> requestAction, Action<ServiceResponseData> action)
        {
            if (mConn == null) throw new Exception("还没有创建连接！");
            try
            {
                ClientRequestData requestData = new ClientRequestData(IsCompressJson, IsEncryptionJson, serializeType);
                if (requestAction != null)
                    requestAction(requestData);

                string jsondata = requestData.GetJsonData();//获取序列化的请求数据

                if (requestData.Iscompressjson)//开启压缩
                {
                    jsondata = ZipComporessor.Compress(jsondata);//压缩传入参数
                }
                if (requestData.Isencryptionjson)//开启加密
                {
                    DESEncryptor des = new DESEncryptor();
                    des.InputString = jsondata;
                    des.DesEncrypt();
                    jsondata = des.OutString;
                }

                IClientHandler _wcfService = mConn.WcfService;
                IAsyncResult result = null;

                AddMessageHeader(_wcfService as IContextChannel, "", requestData.Iscompressjson, requestData.Isencryptionjson, requestData.Serializetype, requestData.LoginRight,  (() =>
                {
                    AsyncCallback callback = delegate(IAsyncResult r)
                    {
                        string retJson = _wcfService.EndProcessRequest(r);

                        if (requestData.Isencryptionjson)//解密结果
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = retJson;
                            des.DesDecrypt();
                            retJson = des.OutString;
                        }
                        if (requestData.Iscompressjson)//解压结果
                        {
                            retJson = ZipComporessor.Decompress(retJson);
                        }

                        string retData = "";
                        object Result = JsonConvert.DeserializeObject(retJson);
                        int ret = Convert.ToInt32((((Newtonsoft.Json.Linq.JObject)Result)["flag"]).ToString());
                        string msg = (((Newtonsoft.Json.Linq.JObject)Result)["msg"]).ToString();
                        if (ret == 1)
                        {
                            throw new Exception(msg);
                        }
                        else
                        {
                            retData = ((Newtonsoft.Json.Linq.JObject)(Result))["data"].ToString();
                        }

                        ServiceResponseData responsedata = new ServiceResponseData();
                        responsedata.Iscompressjson = requestData.Iscompressjson;
                        responsedata.Isencryptionjson = requestData.Isencryptionjson;
                        responsedata.Serializetype = requestData.Serializetype;
                        responsedata.SetJsonData(retData);

                        action(responsedata);
                    };
                    result = _wcfService.BeginProcessRequest(mConn.ClientID, mConn.PluginName , controller, method, jsondata, callback, null);
                }));

                new Action(delegate ()
                {
                    if (IsHeartbeat == false)//如果没有启动心跳，则请求发送心跳
                    {
                        ServerConfigRequestState = false;
                        Heartbeat();
                    }
                }).BeginInvoke(null, null);//异步执行

                return result;
            }
            catch (Exception e)
            {
                ServerConfigRequestState = false;
                ReConnection(true);//连接服务主机失败，重连
                throw new Exception(e.Message + "\n连接服务主机失败，请联系管理员！");
            }
        }

       
        /// <summary>
        /// 卸载连接
        /// </summary>
        public void UnConnection()
        {
            if (mConn == null) return;
            string mClientID = mConn.ClientID;
            IClientHandler mWcfService = mConn.WcfService;
            if (mClientID != null)
            {
                try
                {
                    AddMessageHeader(mWcfService as IContextChannel, "Quit", (() =>
                       {
                           mWcfService.UnClient(mClientID);
                       }));


                    //mChannelFactory.Close();//关闭通道
                    (mWcfService as IContextChannel).Close();

                    if (timer != null)//关闭连接必须停止心跳
                        timer.Stop();
                }
                catch
                {
                    if ((mWcfService as IContextChannel) != null)
                        (mWcfService as IContextChannel).Abort();
                }

                mConn = null;
            }
        }

        

        /// <summary>
        /// 重新连接wcf服务，服务端存在ClientID
        /// </summary>
        /// <param name="isRequest">是否请求调用</param>
        private void ReConnection(bool isRequest)
        {
            try
            {
                IClientHandler wcfHandlerService = mChannelFactory.CreateChannel();
                mConn.WcfService = wcfHandlerService;
                if (isRequest == true)//避免死循环
                    Heartbeat();//重连之后必须再次调用心跳
            }
            catch
            {
                //throw new Exception(err.Message);
            }
        }

        /// <summary>
        /// 发送心跳
        /// </summary>
        /// <returns></returns>
        private bool Heartbeat()
        {
            IClientHandler _wcfService = mConn.WcfService;
            try
            {
                bool ret = false;
                string serverConfig = null;
                AddMessageHeader(_wcfService as IContextChannel, "",  (() =>
                {
                    ret = _wcfService.Heartbeat(mConn.ClientID);
                    if (ServerConfigRequestState == false)
                    {
                        //重新获取服务端配置，如：是否压缩Json、是否加密Json
                        serverConfig = _wcfService.MiddlewareConfig();
                        ServerConfigRequestState = true;
                    }
                }));

                if (!string.IsNullOrEmpty(serverConfig))
                {
                    IsHeartbeat = serverConfig.Split(new char[] { '#' })[0] == "1" ? true : false;
                    HeartbeatTime = Convert.ToInt32(serverConfig.Split(new char[] { '#' })[1]);
                    IsMessage = serverConfig.Split(new char[] { '#' })[2] == "1" ? true : false;
                    MessageTime = Convert.ToInt32(serverConfig.Split(new char[] { '#' })[3]);
                    IsCompressJson = serverConfig.Split(new char[] { '#' })[4] == "1" ? true : false;
                    IsEncryptionJson = serverConfig.Split(new char[] { '#' })[5] == "1" ? true : false;
                    serializeType = (SerializeType)Convert.ToInt32(serverConfig.Split(new char[] { '#' })[6]);

                    if (backConfig != null)
                        backConfig(IsMessage, MessageTime);

                    if (IsHeartbeat)
                    {
                        //开启发送心跳
                        if (timer == null)
                            StartTimer();
                        else
                            timer.Start();
                    }
                    else
                    {
                        if (timer != null)
                            timer.Stop();
                    }
                }

                if (ret == false)//表示服务主机关闭过，丢失了clientId，必须重新创建连接
                {
                    //mChannelFactory.Abort();//关闭原来通道
                    (_wcfService as IContextChannel).Abort();
                    CreateConnection();
                }
                return ret;
            }
            catch
            {
                ServerConfigRequestState = false;
                ReConnection(false);//连接服务主机失败，重连
                return false;
            }
        }

        private string getLocalIPAddress()
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            string myip = "";
            foreach (IPAddress ip in IpEntry.AddressList)
            {
                if (Regex.IsMatch(ip.ToString(), @"\d{0,3}\.\d{0,3}\.\d{0,3}\.\d{0,3}"))
                {
                    myip = ip.ToString();
                    break;
                }
            }
            return myip;
        }

        private void AddMessageHeader(IContextChannel channel, string cmd, Action callback)
        {
            AddMessageHeader(channel, cmd, IsCompressJson, IsEncryptionJson, serializeType, new SysLoginRight(), callback);
        }
        private void AddMessageHeader(IContextChannel channel, string cmd, bool iscompressjson, bool isencryptionjson, SerializeType serializetype, SysLoginRight loginright, Action callback)
        {
            using (var scope = new OperationContextScope(channel as IContextChannel))
            {
                if (string.IsNullOrEmpty(cmd)) cmd = "";

                HeaderParameter para = new HeaderParameter();
                para.cmd = cmd;
                para.routerid = mConn.RouterID;
                para.pluginname = mConn.PluginName;
                //ReplyIdentify如果客户端创建连接为空，如果中间件连接上级中间件那就是本地中间件标识
                para.replyidentify = WcfGlobal.Identify;
                para.token = mConn.Token;
                para.iscompressjson = iscompressjson;
                para.isencryptionjson = isencryptionjson;
                para.serializetype = serializetype;
                para.LoginRight = loginright;
                HeaderOperater.AddMessageHeader(OperationContext.Current.OutgoingMessageHeaders, para);
                callback();
            }
        }
        //向服务端发送心跳，间隔时间为5s
        System.Timers.Timer timer;
        void StartTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = HeartbeatTime * 5 * 1000;//客户端比服务端心跳间隔多5倍
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        Object syncObj = new Object();////定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (syncObj)
            {
                try
                {
                    Heartbeat();
                }
                catch
                {
                    //throw new Exception(err.Message);
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取所有服务插件的控制器和方法
        /// </summary>
        /// <returns></returns>
        public List<ServerManage.dwPlugin> GetWcfServicesAllInfo()
        {
            IClientHandler _wcfService = mConn.WcfService;
            List<ServerManage.dwPlugin> list = new List<ServerManage.dwPlugin>();
            AddMessageHeader(_wcfService as IContextChannel, "", (() =>
            {
                string ret = _wcfService.GetAllPluginInfo();
                list = JsonConvert.DeserializeObject<List<ServerManage.dwPlugin>>(ret);
            }));

            return list;
        }

        #region 上传下载文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filepath">文件本地路径</param>
        /// <returns>上传成功后返回的文件名</returns>
        public string UpLoadFile(string filepath)
        {
            return UpLoadFile(filepath, null);
        }
        /// <summary>
        /// 上传文件，有进度显示
        /// </summary>
        /// <param name="filepath">文件本地路径</param>
        /// <param name="action">进度0-100</param>
        /// <returns>上传成功后返回的文件名</returns>
        public string UpLoadFile(string filepath, Action<int> action)
        {

            IFileHandler fileHandlerService = null;
            try
            {
                FileInfo finfo = new FileInfo(filepath);
                if (finfo.Exists == false)
                    throw new Exception("文件不存在！");


                fileHandlerService = mfileChannelFactory.CreateChannel();

                UpFile uf = new UpFile();
                uf.clientId = mConn == null ? "" : mConn.ClientID;
                uf.UpKey = Guid.NewGuid().ToString();
                uf.FileExt = finfo.Extension;
                uf.FileName = finfo.Name;
                uf.FileSize = finfo.Length;
                uf.FileStream = finfo.OpenRead();

                if (action != null)
                    getupdownprogress(uf.FileStream, uf.FileSize, action);//获取进度条

                UpFileResult result = new UpFileResult();
                result = fileHandlerService.UpLoadFile(uf);

                if (result.IsSuccess)
                    return result.Message;
                else
                    throw new Exception("上传文件失败！");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n上传文件失败！");
            }
            finally
            {
                if (fileHandlerService != null)
                {
                    (fileHandlerService as IContextChannel).Abort();
                }
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filename">下载文件名</param>
        /// <returns>下载成功后返回存储在本地文件路径</returns>
        public string DownLoadFile(string filename)
        {
            return DownLoadFile(filename, null);
        }
        /// <summary>
        /// 下载文件，有进度显示
        /// </summary>
        /// <param name="filename">下载文件名</param>
        /// <param name="action">进度0-100</param>
        /// <returns>下载成功后返回存储在本地文件路径</returns>
        public string DownLoadFile(string filename, Action<int> action)
        {
            IFileHandler fileHandlerService = null;
            try
            {
                if (string.IsNullOrEmpty(filename))
                    throw new Exception("文件名不为空！");

                //string filebufferpath = AppRootPath + @"filebuffer\";
                if (!Directory.Exists(filebufferpath))
                {
                    Directory.CreateDirectory(filebufferpath);
                }

                fileHandlerService = mfileChannelFactory.CreateChannel();
                DownFile df = new DownFile();
                df.clientId = mConn == null ? "" : mConn.ClientID;
                df.DownKey = Guid.NewGuid().ToString();
                df.FileName = filename;


                DownFileResult result = new DownFileResult();

                result = fileHandlerService.DownLoadFile(df);

                if (result.IsSuccess)
                {
                    string filepath = filebufferpath + filename;
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }

                    FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);

                    int bufferlen = 4096;
                    int count = 0;
                    byte[] buffer = new byte[bufferlen];

                    if (action != null)
                        getupdownprogress(result.FileStream, result.FileSize, action);//获取进度条


                    while ((count = result.FileStream.Read(buffer, 0, bufferlen)) > 0)
                    {
                        fs.Write(buffer, 0, count);
                    }
                    //清空缓冲区
                    fs.Flush();
                    //关闭流
                    fs.Close();
                    return filepath;
                }
                else
                    throw new Exception("下载文件失败！");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n下载文件失败！");
            }
            finally
            {
                if (fileHandlerService != null)
                    (fileHandlerService as IContextChannel).Abort();
            }
        }

        private void getprogress(long filesize, long readnum, ref int progressnum)
        {
            //decimal percent = Convert.ToDecimal(100 / Convert.ToDecimal(filesize / bufferlen));
            //progressnum = progressnum + percent > 100 ? 100 : progressnum + percent;
            decimal percent = Convert.ToDecimal(readnum) / Convert.ToDecimal(filesize) * 100;
            progressnum = Convert.ToInt32(Math.Ceiling(percent));
        }
        private void getupdownprogress(Stream file, long flength, Action<int> action)
        {
            new Action<Stream, long, Action<int>>(delegate(Stream _file, long _flength, Action<int> _action)
            {
                try
                {
                    int oldnum = 0;
                    int num = 0;

                    while (num != 100)
                    {
                        getprogress(_flength - 1, _file.Position, ref num);
                        if (oldnum < num)
                        {
                            oldnum = num;
                            _action.BeginInvoke(num, null, null);
                        }
                        System.Threading.Thread.Sleep(100);
                    }
                    //_action(100);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + "\n获取文件进度失败！");
                }

            }).BeginInvoke(file, flength, action, null, null);
        }
        
        #endregion

        #region 超级回调
        /// <summary>
        /// 注册远程插件
        /// </summary>
        /// <param name="ServerIdentify"></param>
        /// <param name="plugin"></param>
        public void RegisterRemotePlugin(string ServerIdentify, string[] plugin)
        {
            if (mConn == null) throw new Exception("还没有创建连接！");
            try
            {
                IClientHandler _wcfService = mConn.WcfService;
                _wcfService.RegisterRemotePlugin(ServerIdentify, plugin);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n连接服务主机失败，请联系管理员！");
            }
        }
        #endregion 

        #region 分布式缓存

        public CacheIdentify DistributedCacheSyncIdentify(CacheIdentify cacheId)
        {
            if (mConn == null) throw new Exception("还没有创建连接！");
            try
            {
                IClientHandler _wcfService = mConn.WcfService;
                return _wcfService.DistributedCacheSyncIdentify(cacheId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n连接服务主机失败，请联系管理员！");
            }
        }

        public void DistributedCacheSync(CacheObject cache)
        {
            if (mConn == null) throw new Exception("还没有创建连接！");
            try
            {
                IClientHandler _wcfService = mConn.WcfService;
                _wcfService.DistributedCacheSync(cache);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n连接服务主机失败，请联系管理员！");
            }
        }

        public void DistributedAllCacheSync(List<CacheObject> cachelist)
        {
            if (mConn == null) throw new Exception("还没有创建连接！");
            try
            {
                IClientHandler _wcfService = mConn.WcfService;
                _wcfService.DistributedAllCacheSync(cachelist);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n连接服务主机失败，请联系管理员！");
            }
        }
        #endregion
    }
    

}
