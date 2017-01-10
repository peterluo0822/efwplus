using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Plugin;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using System.Reflection;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.DbProvider;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.Business.Interface;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace EFWCoreLib.WcfFrame.ServerController
{
    public class ControllerHelper : AbstractControllerHelper
    {

        public override AbstractController CreateController(string pluginname, string controllername)
        {
            string pname = pluginname;
            string cname = controllername;
            ModulePlugin mp;
            WcfControllerAttributeInfo wattr = AppPluginManage.GetPluginWcfControllerAttributeInfo(pname, cname, out mp);

            WcfServerController iController = (WcfServerController)EFWCoreLib.CoreFrame.Business.FactoryModel.GetObject(wattr.wcfControllerType, mp.database, mp.container, mp.cache, mp.plugin.name, null);

            //每次创建控制器的时候,重新创建db，防止并发操作数据
            AbstractDatabase db= FactoryDatabase.GetDatabase(mp.plugin.defaultdbkey);
            db.PluginName = mp.plugin.name;
            iController.BindDb(db, mp.container, mp.cache,mp.plugin.name);
            iController.requestData = null;
            iController.responseData = null;
           
            return iController;
        }

        public override MethodInfo CreateMethodInfo(string pluginname, string controllername, string methodname, AbstractController controller)
        {
            string pname = pluginname;
            string cname = controllername;

            ModulePlugin mp;
            WcfControllerAttributeInfo cattr = AppPluginManage.GetPluginWcfControllerAttributeInfo(pname, cname, out mp);
            if (cattr == null) throw new Exception("插件中没有此控制器名");
            WcfMethodAttributeInfo mattr = cattr.MethodList.Find(x => x.methodName == methodname);
            if (mattr == null) throw new Exception("控制器中没有此方法名");

            if (mattr.dbkeys != null && mattr.dbkeys.Count > 0)
            {
                controller.BindMoreDb(controller.oleDb, "default");
                foreach (string dbkey in mattr.dbkeys)
                {
                    EFWCoreLib.CoreFrame.DbProvider.AbstractDatabase _Rdb = EFWCoreLib.CoreFrame.DbProvider.FactoryDatabase.GetDatabase(dbkey);
                    _Rdb.WorkId = controller.LoginUserInfo.WorkId;
                    //创建数据库连接
                    controller.BindMoreDb(_Rdb, dbkey);
                }
            }

            return mattr.methodInfo;
        }

    }
}
