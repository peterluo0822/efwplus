using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.CoreFrame.Business.AttributeInfo
{
    /// <summary>
    /// Webform自定义标签
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class WebformControllerAttribute : Attribute
    {
        string _memo;
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
    }
}
