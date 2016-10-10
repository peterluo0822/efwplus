using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WCFHosting
{
    public class ReadConfig
    {
        private static string appconfig = Application.ExecutablePath + ".config";
        private static XmlDocument init()
        {
            XmlDocument xmldoc_app = new XmlDocument();
            xmldoc_app.Load(appconfig);
            return xmldoc_app;
        }

        public static string GetWcfServerUrl()
        {
            XmlDocument xmldoc_app = init();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.BaseService']/host/baseAddresses/add");
            if (node != null)
            {
                return node.Attributes["baseAddress"].Value;
            }
            return null;
        }

        public static string GetRouterUrl()
        {
            XmlDocument xmldoc_app = init();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.RouterBaseService']/host/baseAddresses/add");
            if (node != null)
            {
                return node.Attributes["baseAddress"].Value;
            }
            return null;
        }


    }
}
