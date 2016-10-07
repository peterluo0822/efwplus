using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Init;
using System.Xml;

namespace EFWCoreLib.CoreFrame.Plugin
{
    public class PluginSysManage
    {
        private static System.Xml.XmlDocument xmlDoc = null;
        public static string pluginsysFile = System.Windows.Forms.Application.StartupPath + "\\Config\\pluginsys.xml";

        private static void InitConfig()
        {
            xmlDoc = new System.Xml.XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(pluginsysFile, settings);
            xmlDoc.Load(reader);
            reader.Close();
        }

        public static List<string> GetWinformPlugin()
        {
            if (xmlDoc == null) InitConfig();
            List<string> plist = new List<string>();
            XmlNodeList nl = null;

            nl = xmlDoc.DocumentElement.SelectNodes("WinformModulePlugin/Plugin");
            foreach (XmlNode n in nl)
            {
                plist.Add(n.Attributes["path"].Value);
            }
            return plist;
        }

        /// <summary>
        /// 获取所有插件路径
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllPluginFile()
        {
            List<string> pflist = new List<string>();
            if (xmlDoc == null) InitConfig();
            XmlNodeList nl = null;
            string path = AppGlobal.AppRootPath;
            switch (AppGlobal.appType)
            {
                case AppType.Web:
                     nl = xmlDoc.DocumentElement.SelectNodes("WebModulePlugin/Plugin");
                    break;
                
                case AppType.Winform:
                    nl = xmlDoc.DocumentElement.SelectNodes("WinformModulePlugin/Plugin");
                    break;
                case AppType.WCF:
                //case AppType.WCFClient:
                    nl = xmlDoc.DocumentElement.SelectNodes("WcfModulePlugin/Plugin");
                    break;
            }
            foreach (XmlNode n in nl)
            {
                pflist.Add(n.Attributes["path"].Value);
            }
            return pflist;
        }

        /// <summary>
        /// 根据插件名获取插件路径
        /// </summary>
        /// <param name="pluginname"></param>
        /// <returns></returns>
        public static string GetPluginFile(string pluginname)
        {
            string pluginpath = null;
            if (xmlDoc == null) InitConfig();
            XmlNode xn = null;
            string path = AppGlobal.AppRootPath;
            switch (AppGlobal.appType)
            {
                case AppType.Web:
                    xn = xmlDoc.DocumentElement.SelectSingleNode("WebModulePlugin/Plugin[@name=" + pluginname + "]");
                    break;

                case AppType.Winform:
                    xn = xmlDoc.DocumentElement.SelectSingleNode("WinformModulePlugin/Plugin[@name=" + pluginname + "]");
                    break;
                case AppType.WCF:
                //case AppType.WCFClient:
                    xn = xmlDoc.DocumentElement.SelectSingleNode("WcfModulePlugin/Plugin[@name=" + pluginname + "]");
                    break;
            }
            if (xn != null)
            {
                pluginpath = xn.Attributes["path"].Value;
            }
            return pluginpath;
        }

        public static void GetWinformEntry(out string entryplugin, out string entrycontroller)
        {
            if (xmlDoc == null) InitConfig();

            entryplugin = xmlDoc.DocumentElement.SelectNodes("WinformModulePlugin")[0].Attributes["EntryPlugin"].Value.ToString();
            entrycontroller = xmlDoc.DocumentElement.SelectNodes("WinformModulePlugin")[0].Attributes["EntryController"].Value.ToString();
        }

        //public static void GetWcfClientEntry(out string entryplugin, out string entrycontroller)
        //{
        //    if (xmlDoc == null) InitConfig();

        //    entryplugin = xmlDoc.DocumentElement.SelectNodes("WcfModulePlugin")[0].Attributes["EntryPlugin"].Value.ToString();
        //    entrycontroller = xmlDoc.DocumentElement.SelectNodes("WcfModulePlugin")[0].Attributes["EntryController"].Value.ToString();
        //}
    }
}
