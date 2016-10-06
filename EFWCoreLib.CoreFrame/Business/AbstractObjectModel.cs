using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.Unity;

namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// 抽象对象模型类
    /// </summary>
    public abstract class AbstractObjectModel : AbstractBusines
    {
        public IUnityContainer _container
        {
            get { return GetUnityContainer(); }
        }

        public ICacheManager _cache
        {
            get { return GetCache(); }
        }

        public string _pluginName
        {
            get { return GetPluginName(); }
        }
    }
}
