
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Practices.Unity;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// 数据库访问对象不可能自己创建数据库操作对象
    /// </summary>
    [Serializable]
    public abstract class AbstractDao : AbstractBusines
    {
        public AbstractDatabase oleDb
        {
            get { return GetDb(); }
        }

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

        public int WorkId
        {
            get { return oleDb.WorkId; }
        }
    }
}
