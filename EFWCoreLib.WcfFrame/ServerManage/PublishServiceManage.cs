using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.ClientController;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.WcfFrame.Utility.Upgrade;
using EFWCoreLib.WcfFrame.WcfHandler;

namespace EFWCoreLib.WcfFrame.ServerManage
{
    /// <summary>
    /// 发布订阅服务管理
    /// </summary>
    public class PublishServiceManage
    {
        private static Object syncObj = new Object();//定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        public static List<Subscriber> subscriberList;//订阅者列表
        public static List<PublishServiceObject> serviceList;//服务列表
        /// <summary>
        /// 初始化订阅服务
        /// </summary>
        public static void InitPublishService()
        {
            subscriberList = new List<Subscriber>();//订阅者列表
            serviceList = new List<PublishServiceObject>();

            //考虑从订阅服务配置文件读取订阅服务
            PublishServiceObject serviceObj = new PublishServiceObject();
            serviceObj.publishServiceName = "DistributedCache";//分布式缓存服务
            serviceObj.explain = "分布式缓存服务";
            serviceList.Add(serviceObj);

            serviceObj = new PublishServiceObject();
            serviceObj.publishServiceName = "RemotePlugin";//远程插件服务
            serviceObj.explain = "远程插件服务";
            serviceList.Add( serviceObj);

            serviceObj = new PublishServiceObject();
            serviceObj.publishServiceName = "UpgradeClient";//文件升级服务
            serviceObj.explain = "客户端程序升级服务";
            serviceList.Add(serviceObj);

            serviceObj = new PublishServiceObject();
            serviceObj.publishServiceName = "MongodbSync";//Mongodb数据同步
            serviceObj.explain = "Mongodb数据同步服务";
            serviceList.Add(serviceObj);
        }
        /// <summary>
        /// 服务端发布订阅服务列表
        /// </summary>
        /// <returns></returns>
        public static List<PublishServiceObject> GetPublishServiceList()
        {
            return serviceList;
        }
        /// <summary>
        /// 客户端订阅
        /// </summary>
        /// <param name="ServerIdentify"></param>
        /// <param name="clientId"></param>
        /// <param name="publishServiceName"></param>
        /// <param name="callback"></param>
        public static void Subscribe(string ServerIdentify, string clientId, string publishServiceName, IDataReply callback)
        {
            if (ServerIdentify == WcfGlobal.Identify) return;//不能订阅自己

            if (subscriberList.FindIndex(x => x.clientId == clientId && x.publishServiceName == publishServiceName) == -1)
            {
                Subscriber sub;
                lock (syncObj)
                {
                    sub = new Subscriber();
                    sub.clientId = clientId;
                    sub.publishServiceName = publishServiceName;
                    sub.callback = callback;

                    subscriberList.Add(sub);
                }
                ShowHostMsg(Color.Blue, DateTime.Now, "客户端[" + clientId + "]订阅“" + publishServiceName + "”服务成功！");
                SendNotify(sub);
            }
        }
        /// <summary>
        /// 客户端取消订阅
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="publishServiceName"></param>
        public static void UnSubscribe(string clientId, string publishServiceName)
        {
            if (subscriberList.FindIndex(x => x.clientId == clientId && x.publishServiceName == publishServiceName) == -1)
            {
                lock (syncObj)
                {
                    Subscriber sub = subscriberList.Find(x => x.clientId == clientId && x.publishServiceName == publishServiceName);
                    subscriberList.Remove(sub);
                }
                ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + clientId + "]取消订阅“" + publishServiceName + "”服务成功！");
            }
        }
        /// <summary>
        /// 服务端发送通知
        /// </summary>
        public static void SendNotify(string publishServiceName)
        {
            List<Subscriber> list = subscriberList.FindAll(x => x.publishServiceName == publishServiceName);
            foreach (var item in list)
            {
                item.callback.Notify(publishServiceName);
            }
            ShowHostMsg(Color.Blue, DateTime.Now, "向已订阅的客户端发送了“" + publishServiceName + "”服务通知！");
        }
        /// <summary>
        /// 服务端给指定订阅者发布通知
        /// </summary>
        /// <param name="sub"></param>
        public static void SendNotify(Subscriber sub)
        {
            if (sub != null)
            {
                sub.callback.Notify(sub.publishServiceName);
                ShowHostMsg(Color.Blue, DateTime.Now, "向已订阅的客户端发送了“" + sub.publishServiceName + "”服务通知！");
            }
        }

        
        private static void ShowHostMsg(Color clr, DateTime time, string text)
        {
            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, clr, text);
        }
    }
    /// <summary>
    /// 订阅者
    /// </summary>
    public class Subscriber
    {
        /// <summary>
        /// 中间件标识
        /// </summary>
        public string ServerIdentify { get; set; }
        /// <summary>
        /// 客户端标识
        /// </summary>
        public string clientId { get; set; }
        /// <summary>
        /// 发布服务名称
        /// </summary>
        public string publishServiceName { get; set; }
        /// <summary>
        /// 回调通知客户端
        /// </summary>
        public IDataReply callback { get; set; }
    }
    /// <summary>
    /// 发布服务对象
    /// </summary>
    [DataContract]
    public class PublishServiceObject
    {
        /// <summary>
        /// 发布服务名称标识
        /// </summary>
        [DataMember]
        public string publishServiceName { get; set; }
        /// <summary>
        /// 服务中文说明
        /// </summary>
        [DataMember]
        public string explain { get; set; }
        [DataMember]
        public string pluginname { get; set; }
        [DataMember]
        public string controller { get; set; }
        [DataMember]
        public string method { get; set; }
        [DataMember]
        public string argument { get; set; }
    }

}
