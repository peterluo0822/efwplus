using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using EFWCoreLib.CoreFrame.Init;

namespace WCFHosting
{
    /// <summary>
    /// WCF主机配置文件操作类
    /// </summary>
    public class HostSettingConfig
    {
        private static System.Xml.XmlDocument xmlDoc = null;
        private static string configfile = System.Windows.Forms.Application.StartupPath + "\\Config\\SettingConfig.xml";

        private static void InitConfig()
        {
            xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(configfile);
        }

        public static string GetValue(string keyname)
        {
            if (xmlDoc == null) InitConfig();
            return xmlDoc.DocumentElement.SelectNodes(keyname)[0].Attributes["value"].Value.ToString();
        }

        public static string GetValue(string keyname,string attrname)
        {
            if (xmlDoc == null) InitConfig();
            return xmlDoc.DocumentElement.SelectNodes(keyname)[0].Attributes[attrname].Value.ToString();
        }

        public static void SetValue(string keyname, string value)
        {
            if (xmlDoc == null) InitConfig();
            xmlDoc.DocumentElement.SelectNodes(keyname)[0].Attributes["value"].Value = value;
        }

        public static void SetValue(string keyname, string attrname, string value)
        {
            if (xmlDoc == null) InitConfig();
            xmlDoc.DocumentElement.SelectNodes(keyname)[0].Attributes[attrname].Value = value;
        }

        public static void SaveConfig()
        {
            if (xmlDoc == null) InitConfig();
            xmlDoc.Save(configfile);
            InitConfig();
        }
    }

    public class HostAddressConfig
    {
        private static string appconfig = AppGlobal.AppRootPath + "efwplusServer.exe.config";
        private static XmlDocument xmldoc_app;

        private static void InitConfig()
        {
            appconfig = AppGlobal.AppRootPath + System.IO.Path.GetFileName(Application.ExecutablePath) + ".config";
            xmldoc_app = new XmlDocument();
            xmldoc_app.Load(appconfig);
        }

        public static string GetWcfAddress()
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node;
            node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.BaseService']/host/baseAddresses/add");
            if (node != null)
            {
                return node.Attributes["baseAddress"].Value;
            }
            return null;
        }

        public static void SetWcfAddress(string url)
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.BaseService']/host/baseAddresses/add");
            if (node != null)
            {
                node.Attributes["baseAddress"].Value = url;
            }
        }

        public static string GetFileAddress()
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.FileService']/host/baseAddresses/add");
            if (node != null)
            {
                return node.Attributes["baseAddress"].Value;
            }
            return null;
        }

        public static void SetFileAddress(string url)
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.FileService']/host/baseAddresses/add");
            if (node != null)
            {
                node.Attributes["baseAddress"].Value = url;
            }
        }

        public static string GetRouterAddress()
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.RouterBaseService']/host/baseAddresses/add");
            if (node != null)
            {
                return node.Attributes["baseAddress"].Value;
            }
            return null;
        }

        public static void SetRouterAddress(string url)
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.RouterBaseService']/host/baseAddresses/add");
            if (node != null)
            {
                node.Attributes["baseAddress"].Value = url;
            }
        }

        public static string GetfileRouterAddress()
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.RouterFileService']/host/baseAddresses/add");
            if (node != null)
            {
                return node.Attributes["baseAddress"].Value;
            }
            return null;
        }

        public static void SetfileRouterAddress(string url)
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/services/service[@name='EFWCoreLib.WcfFrame.WcfHandler.RouterFileService']/host/baseAddresses/add");
            if (node != null)
            {
                node.Attributes["baseAddress"].Value = url;
            }
        }

        public static string GetClientWcfAddress()
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/client/endpoint[@name='wcfendpoint']");
            if (node != null)
            {
                return node.Attributes["address"].Value;
            }
            return null;
        }

        public static void SetClientWcfAddress(string url)
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/client/endpoint[@name='wcfendpoint']");
            if (node != null)
            {
                node.Attributes["address"].Value = url;
            }
        }

        public static string GetClientFileAddress()
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/client/endpoint[@name='fileendpoint']");
            if (node != null)
            {
                return node.Attributes["address"].Value;
            }
            return null;
        }

        public static void SetClientFileAddress(string url)
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/client/endpoint[@name='fileendpoint']");
            if (node != null)
            {
                node.Attributes["address"].Value = url;
            }
        }

        public static string GetClientLocalAddress()
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/client/endpoint[@name='localendpoint']");
            if (node != null)
            {
                return node.Attributes["address"].Value;
            }
            return null;
        }

        public static void SetClientLocalAddress(string url)
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("system.serviceModel/client/endpoint[@name='localendpoint']");
            if (node != null)
            {
                node.Attributes["address"].Value = url;
            }
        }

        public static string GetWebapiAddress()
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("appSettings/add[@key='WebApiUri']");
            if (node != null)
            {
                return node.Attributes["value"].Value;
            }
            return null;
        }

        public static void SetWebapiAddress(string url)
        {
            if (xmldoc_app == null) InitConfig();
            XmlNode node = xmldoc_app.DocumentElement.SelectSingleNode("appSettings/add[@key='WebApiUri']");
            if (node != null)
            {
                node.Attributes["value"].Value = url;
            }
        }


        public static void SaveConfig()
        {
            if (xmldoc_app == null) InitConfig();
            xmldoc_app.Save(appconfig);
            InitConfig();
        }
    }

    public class HostDataBaseConfig
    {
        private static string entlibconfig = AppGlobal.AppRootPath + "Config\\EntLib.config";
        private static XmlDocument xmldoc_entlib;
        private static void InitConfig()
        {
            xmldoc_entlib = new XmlDocument();
            xmldoc_entlib.Load(entlibconfig);
        }

        public static string GetConnString()
        {
            if (xmldoc_entlib == null) InitConfig();
            XmlNode node = xmldoc_entlib.DocumentElement.SelectSingleNode("connectionStrings");
            if (node != null)
            {
                return node.InnerXml;
            }
            return null;
        }

        public static void SetConnString(string str)
        {
            if (xmldoc_entlib == null) InitConfig();
            XmlNode node = xmldoc_entlib.DocumentElement.SelectSingleNode("connectionStrings");
            if (node != null)
            {
                node.InnerXml = str;
            }
        }

        public static void SaveConfig()
        {
            if (xmldoc_entlib == null) InitConfig();
            xmldoc_entlib.Save(entlibconfig);
            InitConfig();
        }
    }

    public class HostRouterXml
    {
        private static string routerfile = System.Windows.Forms.Application.StartupPath + "\\Config\\RouterBill.xml";

        public static string GetXml()
        {
            FileInfo file = new FileInfo(routerfile);
            if (file.Exists)
            {
                return file.OpenText().ReadToEnd();
            }
            return null;
        }

        public static bool SaveXml(string xml)
        {
            FileInfo file = new FileInfo(routerfile);
            if (file.Exists)
            {
                file.Delete();
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(routerfile, true))
                {
                    sw.Write(xml);//直接追加文件末尾，不换行 
                }
                return true;
            }

            return false;
        }
    }
}
