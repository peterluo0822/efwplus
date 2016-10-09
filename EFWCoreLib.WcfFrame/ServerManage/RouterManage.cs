using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EFWCoreLib.WcfFrame.SDMessageHeader;
using EFWCoreLib.WcfFrame.WcfHandler;

namespace EFWCoreLib.WcfFrame.ServerManage
{
    public delegate void HostWCFRouterListHandler(List<RegistrationInfo> dic);

    public class RouterManage
    {
        public static HostWCFRouterListHandler hostwcfRouter;

        public static IDictionary<string, IRouterBaseHandler> routerDic = new Dictionary<string, IRouterBaseHandler>();
        public static IDictionary<string, HeaderParameter> headParaDic = new Dictionary<string, HeaderParameter>();

        public static IDictionary<int, RegistrationInfo> RegistrationList = new Dictionary<int, RegistrationInfo>();
        public static IDictionary<string, int> RoundRobinCount = new Dictionary<string, int>();
        
        public static string ns = "http://www.efwplus.cn/";
        public static string routerfile = System.Windows.Forms.Application.StartupPath + "\\Config\\RouterBill.xml";

        public static void Start()
        {
            //hostwcfMsg(Color.Blue, DateTime.Now, "RouterHandlerService服务正在初始化...");
            RegistrationInfo.LoadRouterBill();
            //hostwcfMsg(Color.Blue, DateTime.Now, "RouterHandlerService服务初始化完成");
            hostwcfRouter(RegistrationList.Values.ToList());
        }

        public static void Stop()
        {
            RegistrationList.Clear();
            hostwcfRouter(RegistrationList.Values.ToList());
        }


        public static void RemoveClient(HeaderParameter para)
        {
            if (routerDic.ContainsKey(para.routerid))
            {
                lock (routerDic)
                {
                    (routerDic[para.routerid] as IContextChannel).Abort();
                    routerDic.Remove(para.routerid);
                    headParaDic.Remove(para.routerid);
                }

            }
            if (RoundRobinCount.ContainsKey(para.routerid))
            {
                lock (RegistrationList)
                {
                    int key = RoundRobinCount[para.routerid];
                    RegistrationInfo regInfo = RegistrationList[key];
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

    
    public class RegistrationInfo
    {
        public string HostName { get; set; }

        
        public string ServiceType { get; set; }

        
        public string Address { get; set; }

        
        public string ContractName { get; set; }

        
        public string ContractNamespace { get; set; }

        
        public List<PluginInfo> pluginList { get; set; }

        
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

            XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(RouterManage.routerfile);

            XmlNodeList rlist = xmlDoc.DocumentElement.SelectNodes("record");

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

                if (!RouterManage.RegistrationList.ContainsKey(registrationInfo.GetHashCode()))
                {
                    RouterManage.RegistrationList.Add(registrationInfo.GetHashCode(), registrationInfo);
                }
            }

        }
    }

    public class PluginInfo
    {
        public string name { get; set; }

        public string title { get; set; }

        public string replyidentify { get; set; }
    }
}
