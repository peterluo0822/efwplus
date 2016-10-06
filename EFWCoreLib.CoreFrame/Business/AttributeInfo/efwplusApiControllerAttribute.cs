using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.CoreFrame.Business.AttributeInfo
{
    /// <summary>
    /// WebAPI自定义标签
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class efwplusApiControllerAttribute : Attribute
    {
        string _memo;
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }

        private string _pluginname;
        public string PluginName
        {
            get { return _pluginname; }
            set { _pluginname = value; }
        }
    }
}
