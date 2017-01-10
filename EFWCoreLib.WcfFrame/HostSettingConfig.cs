using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame
{
    /// <summary>
    /// 宿主配置文件操作类
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
            if (xmlDoc.DocumentElement.SelectNodes(keyname).Count > 0)
                return xmlDoc.DocumentElement.SelectNodes(keyname)[0].Attributes["value"].Value.ToString();
            else
                return "0";
        }

        public static string GetValue(string keyname, string attrname)
        {
            if (xmlDoc == null) InitConfig();
            if (xmlDoc.DocumentElement.SelectNodes(keyname).Count > 0)
                return xmlDoc.DocumentElement.SelectNodes(keyname)[0].Attributes[attrname].Value.ToString();
            else
                return "0";
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
}
