﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Plugin;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using System.Reflection;
using EFWCoreLib.CoreFrame.Business;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using EFWCoreLib.CoreFrame.Common;
using System.Windows.Forms;

namespace EFWCoreLib.WinformFrame.Controller
{
    public class ControllerHelper
    {
        public static WinformController CreateController(string controllername)
        {
            try
            {
                string[] names = controllername.Split(new char[] { '@' });
                if (names.Length != 2) throw new Exception("控制器名称错误!");
                string pluginname = names[0];
                string cname = names[1];

                ModulePlugin mp;
                WinformControllerAttributeInfo wattr = AppPluginManage.GetPluginWinformControllerAttributeInfo(pluginname, cname, out mp);
                if (wattr != null)
                {
                    WinformController iController = wattr.winformController as WinformController;
                    if (iController.InitFinish == false)
                    {
                        iController.BindDb(mp.database, mp.container, mp.cache, mp.plugin.name);


                        //IBaseView deview = (IBaseView)System.Activator.CreateInstance(wattr.ViewList.Find(x => x.IsDefaultView).ViewType);
                        //iController._defaultView = deview;

                        Dictionary<string, IBaseViewBusiness> viewDic = new Dictionary<string, IBaseViewBusiness>();
                        for (int i = 0; i < wattr.ViewList.Count; i++)
                        {
                            IBaseViewBusiness view = System.Activator.CreateInstance(wattr.ViewList[i].ViewType) as IBaseViewBusiness;
                            //IBaseViewBusiness view = (IBaseViewBusiness)(CreateInstance(wattr.ViewList[i].ViewType)());
                            view.frmName = wattr.ViewList[i].Name;
                            viewDic.Add(wattr.ViewList[i].Name, view);

                            if (wattr.ViewList[i].IsDefaultView)
                                iController._defaultView = view;
                        }
                        iController.iBaseView = viewDic;


                        iController.Init();
                        List<IntPtr> ptrlist = new List<IntPtr>();
                        foreach (var frm in iController.iBaseView)
                        {
                            ptrlist.Add((frm.Value as Form).Handle);
                        }
                        //异步执行数据初始化
                        var asyn = new Func<IntPtr[]>(delegate ()
                         {
                             iController.AsynInit();
                             return ptrlist.ToArray();
                         });
                        IAsyncResult asynresult = asyn.BeginInvoke(new System.AsyncCallback(CallbackHandler), null);
                        iController.InitFinish = true;
                    }

                    return iController;
                }
                else
                    return null;
            }
            catch (Exception err)
            {
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                throw new Exception(err.Message);
            }
        }
        static void CallbackHandler(IAsyncResult iar)
        {
            AsyncResult ar = (AsyncResult)iar;
            // 获取原委托对象。
            Func<IntPtr[]> operation = (Func<IntPtr[]>)ar.AsyncDelegate;
            // 结束委托调用。
            IntPtr[] intptrs = operation.EndInvoke(iar);
            //iController.AsynInitCompleted();
            foreach (var intp in intptrs)
            {
                WindowsAPI.SendMessage(intp, WindowsAPI.WM_ASYN_INPUT, 0, 0);
            }
        }
        static Func<object> CreateInstance(Type type)
        {

            NewExpression newExp = Expression.New(type);
            Expression<Func<object>> lambdaExp = Expression.Lambda<Func<object>>(newExp, null);
            Func<object> func = lambdaExp.Compile();
            return func;
        }

        public static MethodInfo CreateMethodInfo(string controllername, string methodname)
        {
            try
            {
                string[] names = controllername.Split(new char[] { '@' });
                if (names.Length != 2) throw new Exception("控制器名称错误!");
                string pluginname = names[0];
                string cname = names[1];

                ModulePlugin mp;
                WinformControllerAttributeInfo cattr = AppPluginManage.GetPluginWinformControllerAttributeInfo(pluginname, cname, out mp);

                WinformMethodAttributeInfo mattr = cattr.MethodList.Find(x => x.methodName == methodname);
                if (mattr == null) throw new Exception("控制器中没有此方法名");

                if (mattr.dbkeys != null && mattr.dbkeys.Count > 0)
                {
                    cattr.winformController.BindMoreDb(mp.database, "default");
                    foreach (string dbkey in mattr.dbkeys)
                    {
                        EFWCoreLib.CoreFrame.DbProvider.AbstractDatabase _Rdb = EFWCoreLib.CoreFrame.DbProvider.FactoryDatabase.GetDatabase(dbkey);
                        _Rdb.WorkId = cattr.winformController.LoginUserInfo.WorkId;
                        //创建数据库连接
                        cattr.winformController.BindMoreDb(_Rdb, dbkey);
                    }
                }

                return mattr.methodInfo;
            }
            catch (Exception err)
            {
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                throw new Exception(err.Message);
            }
        }
    }
}
