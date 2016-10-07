using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml.Linq;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using EFWCoreLib.WcfFrame.ServerController;
using System.Drawing;
using EFWCoreLib.WcfFrame.WcfService;
using System.Xml;
using EFWCoreLib.WcfFrame.SDMessageHeader;

namespace EFWCoreLib.WcfFrame.ServerController
{
    public class RouterServerManage
    {
        public static IDictionary<string, IRouterService> routerDic = new Dictionary<string, IRouterService>();
        public static IDictionary<string, HeaderParameter> headParaDic = new Dictionary<string, HeaderParameter>();

        public static IDictionary<int, RegistrationInfo> RegistrationList = new Dictionary<int, RegistrationInfo>();
        public static IDictionary<string, int> RoundRobinCount = new Dictionary<string, int>();
        public static HostWCFMsgHandler hostwcfMsg;
        public static HostWCFRouterListHandler hostwcfRouter;
        public static string ns = "http://www.efwplus.cn/";
        public static string routerfile = System.Windows.Forms.Application.StartupPath + "\\Config\\RouterBill.xml";

        public static void Start()
        {
            hostwcfMsg(Color.Blue, DateTime.Now, "RouterHandlerService服务正在初始化...");
            RegistrationInfo.LoadRouterBill();
            hostwcfMsg(Color.Blue, DateTime.Now, "RouterHandlerService服务初始化完成");
            hostwcfRouter(RegistrationList.Values.ToList());
        }

        public static void Stop()
        {
            RegistrationList.Clear();
            hostwcfRouter(RegistrationList.Values.ToList());
        }


        public static void RemoveClient(HeaderParameter para)
        {
            if (RouterServerManage.routerDic.ContainsKey(para.routerid))
            {
                lock (RouterServerManage.routerDic)
                {
                    (RouterServerManage.routerDic[para.routerid] as IContextChannel).Abort();
                    RouterServerManage.routerDic.Remove(para.routerid);
                    RouterServerManage.headParaDic.Remove(para.routerid);
                }

            }
            if (RouterServerManage.RoundRobinCount.ContainsKey(para.routerid))
            {
                lock (RouterServerManage.RegistrationList)
                {
                    int key = RouterServerManage.RoundRobinCount[para.routerid];
                    RegistrationInfo regInfo = RouterServerManage.RegistrationList[key];
                    regInfo.ClientNum -= 1;
                }
            }

            //界面显示
            hostwcfRouter(RegistrationList.Values.ToList());
        }

        /// <summary>
        /// 从注册表容器中根据Message的Action找到匹配的 binding和 endpointaddress
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="binding"></param>
        /// <param name="endpointAddress"></param>
        public static HeaderParameter AddClient(Message requestMessage, HeaderParameter para, out EndpointAddress endpointAddress, out Uri touri)
        {
            string contractNamespace = requestMessage.Headers.Action.Substring(0, requestMessage.Headers.Action.LastIndexOf("/"));

            RegistrationInfo regInfo = null;

            lock (RegistrationList)
            {
                List<KeyValuePair<int, RegistrationInfo>> krlist = RegistrationList.OrderBy(x => x.Value.ClientNum).ToList().FindAll(x => x.Value.ContractNamespace.Contains(contractNamespace));
                if (krlist.Count > 0)
                {
                    foreach (var r in krlist)
                    {
                        if (r.Value.pluginList.FindIndex(x => x.name == para.pluginname) > -1)
                        {
                            RoundRobinCount[para.routerid] = r.Key;
                            r.Value.ClientNum += 1;
                            regInfo = r.Value;
                            break;
                        }
                    }
                }
            }
            if (regInfo == null)
                throw new Exception("找不到对应的路由地址");

            Uri addressUri = new Uri(regInfo.Address);

            //binding = CustomBindConfig.GetRouterBinding(addressUri.Scheme);
            endpointAddress = new EndpointAddress(regInfo.Address);
            //重设Message的目标终结点
            touri = new Uri(regInfo.Address);

            PluginInfo pinfo = regInfo.pluginList.Find(x => x.name == para.pluginname);
            if (pinfo != null && !string.IsNullOrEmpty(pinfo.replyidentify))
                para.replyidentify = pinfo.replyidentify;
            //界面显示
            hostwcfRouter(RegistrationList.Values.ToList());

            return para;
        }

        public static void GetServiceEndpointFile(Message requestMessage, out EndpointAddress endpointAddress, out Uri touri)
        {
            string contractNamespace = requestMessage.Headers.Action.Substring(0, requestMessage.Headers.Action.LastIndexOf("/"));

            RegistrationInfo regInfo = null;

            lock (RegistrationList)
            {
                List<KeyValuePair<int, RegistrationInfo>> krlist = RegistrationList.OrderBy(x => x.Value.ClientNum).ToList().FindAll(x => x.Value.ContractNamespace.Contains(contractNamespace));
                if (krlist.Count > 0)
                {
                    regInfo = krlist.First().Value;
                    regInfo.ClientNum += 1;
                }
            }


            if (regInfo == null)
                throw new Exception("找不到对应的路由地址");

            Uri addressUri = new Uri(regInfo.Address);

            //binding = CustomBindConfig.GetRouterBinding(addressUri.Scheme);
            endpointAddress = new EndpointAddress(regInfo.Address);
            //重设Message的目标终结点
            touri = new Uri(regInfo.Address);

            //界面显示
            hostwcfRouter(RegistrationList.Values.ToList());
        }

        
    }

    public delegate void HostWCFRouterListHandler(List<RegistrationInfo> dic);

    [DataContract]
    public class RegistrationInfo
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string HostName { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string ServiceType { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public string Address { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public string ContractName { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public string ContractNamespace { get; set; }

        [DataMember(IsRequired = true, Order = 6)]
        public List<PluginInfo> pluginList { get; set; }

        [DataMember(IsRequired = true, Order = 7)]
        public int ClientNum { get; set; }

        public override int GetHashCode()
        {
            return this.Address.GetHashCode() + this.ContractName.GetHashCode() + this.ContractNamespace.GetHashCode();
        }

        /// <summary>
        /// 加载路由器的路由表
        /// </summary>
        public static void LoadRouterBill()
        {
            string _hostname = null;
            string _servicetype = null;
            string _address = null;
            string _contractname = null;
            string _contractnamespace = null;

            XmlDocument xmlDoc =new System.Xml.XmlDocument();
            xmlDoc.Load(RouterServerManage.routerfile);

            XmlNodeList rlist= xmlDoc.DocumentElement.SelectNodes("record");

            foreach (XmlNode xe in rlist)
            {
                _hostname = xe.SelectSingleNode("hostname").InnerText;
                _servicetype = xe.SelectSingleNode("servicetype").InnerText;
                _address = xe.SelectSingleNode("address").InnerText;
                _contractname = xe.SelectSingleNode("ContractName").InnerText;
                _contractnamespace = xe.SelectSingleNode("ContractNamespace").InnerText;

                RegistrationInfo registrationInfo = new RegistrationInfo { HostName = _hostname, ServiceType = _servicetype, Address = _address, ContractName = _contractname, ContractNamespace = _contractnamespace };
                registrationInfo.pluginList = new List<PluginInfo>();
                XmlNodeList plist = xe.SelectNodes("plugins/plugin");
                foreach (XmlNode ps in plist)
                {
                    string name = ps.Attributes["name"].Value;
                    string title = ps.Attributes["title"].Value;
                    string replyidentify = ps.Attributes["replyidentify"].Value;
                    PluginInfo plugin = new PluginInfo();
                    plugin.name = name;
                    plugin.title = title;
                    plugin.replyidentify = replyidentify;
                    registrationInfo.pluginList.Add(plugin);
                }

                if (!RouterServerManage.RegistrationList.ContainsKey(registrationInfo.GetHashCode()))
                {
                    RouterServerManage.RegistrationList.Add(registrationInfo.GetHashCode(), registrationInfo);
                }
            }

        }
    }

    [DataContract]
    public class PluginInfo
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string name { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string title { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public string replyidentify { get; set; }
    }

}
