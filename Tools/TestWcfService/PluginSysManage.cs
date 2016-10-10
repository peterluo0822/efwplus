using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestWcfService
{
    /// <summary>
    /// pluginsys.xml 解析
    /// </summary>
    public class PluginSysManage
    {
        public static string localpath = System.Windows.Forms.Application.StartupPath;

        private static System.Xml.XmlDocument xmlDoc = null;
        private static string _pluginsysFile = System.Windows.Forms.Application.StartupPath + "\\Config\\pluginsys.xml";

        private static void InitConfig()
        {
            xmlDoc = new System.Xml.XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(_pluginsysFile, settings);
            xmlDoc.Load(reader);
            reader.Close();
        }

        public static List<string> GetAllPlugin()
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
    }
}
