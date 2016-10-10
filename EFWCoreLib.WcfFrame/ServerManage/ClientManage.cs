using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.WcfHandler;

namespace EFWCoreLib.WcfFrame.ServerManage
{
    public delegate void ClientInfoListHandler(List<ClientInfo> dic);
    /// <summary>
    /// 客户端管理
    /// </summary>
    public class ClientManage
    {
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

        public static ClientInfoListHandler clientinfoList;
        //客户端列表
        public static Dictionary<string, ClientInfo> ClientDic = new Dictionary<string, ClientInfo>();


        private static Object syncObj = new Object();//定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        /// <summary>
        /// 开始服务主机
        /// </summary>
        public static void StartHost()
        {
            //开启心跳监测
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
        public static void StopHost()
        {
            foreach (ClientInfo client in ClientDic.Values)
            {
                client.IsConnect = false;
            }
        }

        public static string CreateClient(string clientName, DateTime time, IDataReply dataReply, string plugin, string replyidentify)
        {
            string clientId = Guid.NewGuid().ToString();

            try
            {
                AddClient(clientId, clientName, time, dataReply, plugin, replyidentify);
                clientinfoList(ClientDic.Values.ToList());

                return clientId;
            }
            catch (Exception ex)
            {
                throw new System.ServiceModel.FaultException(ex.Source + "：创建客户端运行环境失败！");
            }
        }
        public static bool Heartbeat(string clientId)
        {
            bool b = UpdateHeartbeatClient(clientId);
            if (b == true)
            {
                ReConnectionClient(clientId);
                return true;
            }
            else
                return false;
        }

        public static bool UnClient(string clientId)
        {
            RemoveClient(clientId);
            clientinfoList(ClientDic.Values.ToList());
            return true;
        }

        public static string MiddlewareConfig()
        {
            string _IsHeartbeat = IsHeartbeat ? "1" : "0";
            string _HeartbeatTime = HeartbeatTime.ToString();
            string _IsMessage = IsMessage ? "1" : "0";
            string _MessageTime = MessageTime.ToString();
            string _IsCompressJson = IsCompressJson ? "1" : "0";
            string _IsEncryptionJson = IsEncryptionJson ? "1" : "0";
            string _serializetype = ((int)serializeType).ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append(_IsHeartbeat);
            sb.Append("#");
            sb.Append(_HeartbeatTime);
            sb.Append("#");
            sb.Append(_IsMessage);
            sb.Append("#");
            sb.Append(_MessageTime);
            sb.Append("#");
            sb.Append(_IsCompressJson);
            sb.Append("#");
            sb.Append(_IsEncryptionJson);
            sb.Append("#");
            sb.Append(_serializetype);
            return sb.ToString();
        }

        #region 界面显示

        //public static HostWCFMsgHandler hostwcfMsg;
        private static void AddClient(string clientId, string clientName, DateTime time, IDataReply dataReply, string plugin, string replyidentify)
        {
            lock (syncObj)
            {
                ClientInfo info = new ClientInfo();
                info.clientId = clientId;
                info.clientName = clientName;
                info.startTime = time;
                info.dataReply = dataReply;
                info.IsConnect = true;
                info.plugin = plugin;
                info.ServerIdentify = replyidentify;

                ClientDic.Add(clientId, info);
            }
            ShowHostMsg(Color.Blue, DateTime.Now, "客户端[" + clientName + "]已连接WCF服务主机");
        }
        public static bool UpdateRequestClient(string clientId, int rlen, int slen)
        {
            lock (syncObj)
            {
                if (ClientDic.ContainsKey(clientId))
                {

                    ClientDic[clientId].RequestCount += 1;
                    ClientDic[clientId].receiveData += rlen;
                    ClientDic[clientId].sendData += slen;
                }
            }
            return true;
        }
        private static bool UpdateHeartbeatClient(string clientId)
        {
            lock (syncObj)
            {
                if (ClientDic.ContainsKey(clientId))
                {

                    ClientDic[clientId].startTime = DateTime.Now;
                    ClientDic[clientId].HeartbeatCount += 1;
                    return true;
                }
                else
                    return false;
            }
        }
        private static void RemoveClient(string clientId)
        {
            lock (syncObj)
            {
                if (ClientDic.ContainsKey(clientId))
                {

                    ClientDic.Remove(clientId);
                    ShowHostMsg(Color.Blue, DateTime.Now, "客户端[" + clientId + "]已退出断开连接WCF服务主机");
                }
            }
        }
        private static void ReConnectionClient(string clientId)
        {
            lock (syncObj)
            {
                if (ClientDic.ContainsKey(clientId))
                {
                    if (ClientDic[clientId].IsConnect == false)
                    {
                        ShowHostMsg(Color.Blue, DateTime.Now, "客户端[" + clientId + "]已重新连接WCF服务主机");
                        ClientDic[clientId].IsConnect = true;
                    }
                }
            }
        }
        private static void DisConnectionClient(string clientId)
        {
            lock (syncObj)
            {
                if (ClientDic.ContainsKey(clientId))
                {
                    if (ClientDic[clientId].IsConnect == true)
                    {
                        ClientDic[clientId].IsConnect = false;
                        ShowHostMsg(Color.Blue, DateTime.Now, "客户端[" + clientId + "]已超时断开连接WCF服务主机");
                    }
                }
            }
        }
        
        #endregion

        #region 心跳显示
        //检测客户端是否在线，超时时间为10s
        static System.Timers.Timer timer;
        private static void StartListenClients()
        {
            timer = new System.Timers.Timer();
            timer.Interval = HeartbeatTime * 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (syncObj)
                {
                    foreach (ClientInfo client in ClientDic.Values)
                    {
                        if (client.startTime.AddSeconds(HeartbeatTime * 10) < DateTime.Now)//断开10秒就置为断开
                        {
                            DisConnectionClient(client.clientId);
                        }

                        if (client.startTime.AddSeconds(HeartbeatTime * 20) < DateTime.Now)//断开10分钟直接移除客户端
                        {
                            RemoveClient(client.clientId);
                        }
                    }
                    clientinfoList(ClientDic.Values.ToList());
                }
            }
            catch { }
        }
        #endregion

        private static void ShowHostMsg(Color clr, DateTime time, string text)
        {
            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, clr, text);
        }
    }

    /// <summary>
    /// 连接客户端信息
    /// </summary>
    public class ClientInfo : MarshalByRefObject, ICloneable
    {
        public string clientId { get; set; }
        public string clientName { get; set; }
        public DateTime startTime { get; set; }
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


        /// <summary>
        /// 数据回调
        /// </summary>
        public IDataReply dataReply { get; set; }

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    
}
