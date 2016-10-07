/*
 *控制器的目的：
 *使界面对象与服务对象达到隔离和重用的目的 
 *所以控制器是把界面对象与服务对象组合一些业务功能、一些菜单。
 *如果一个界面有两个菜单那就分开建两个控制器对象。
 * 
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.EntLib;
using System.Windows.Forms;

namespace EFWCoreLib.WinformFrame.Controller
{
 
    /// <summary>
    /// Winform控制器基类
    /// 
    /// </summary>
    public class WinformController : AbstractController
    {
        //实例化此控制器的菜单ID
        //public int MenuId { get; set; }

        /// <summary>
        /// 获取页面子权限
        /// </summary>
        //public DataTable GetPageRight
        //{
        //    get
        //    {
        //        DataTable data = (DataTable)ExecuteFun.invoke(oleDb,"getPageRight", MenuId, LoginUserInfo.UserId);
        //        return data;
        //    }
        //}

        protected override SysLoginRight GetUserInfo()
        {
            if (EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser") != null)
            {
                return (SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser");
            }
            return base.GetUserInfo();
        }

        internal IBaseViewBusiness _defaultView;

        public IBaseViewBusiness DefaultView
        {
            get { return _defaultView; }
            set { _defaultView = value; }
        }

        private Dictionary<string, IBaseViewBusiness> _iBaseView;
        public Dictionary<string, IBaseViewBusiness> iBaseView
        {
            get { return _iBaseView; }
            set
            {
                _iBaseView = value;
                foreach (KeyValuePair<string, IBaseViewBusiness> val in _iBaseView)
                {
                    val.Value.InvokeController = new ControllerEventHandler(UI_ControllerEvent);
                }
            }
        }

        /// <summary>
        /// 创建WinformController的实例
        /// </summary>
        public WinformController()
        {
            
        }
        /// <summary>
        /// 界面控制事件
        /// </summary>
        /// <param name="eventname">事件名称</param>
        /// <param name="objs">参数数组</param>
        /// <returns></returns>
        public virtual object UI_ControllerEvent(string eventname, params object[] objs)
        {
            try
            {
                switch (eventname)
                {
                    case "Show":
                        if (objs.Length > 0)
                        {
                            Form form = null;
                            if (objs[0] is String)
                            {
                                form = iBaseView[objs[0].ToString()] as Form;
                            }
                            else
                            {
                                form = objs[0] as Form;
                            }

                            if (objs.Length == 1)
                            {
                                string tabName = form.Text;
                                string tabId = "view" + form.GetHashCode();
                                InvokeController("MainFrame.UI", "LoginController", "ShowForm", form, tabName, tabId);
                            }
                            else if (objs.Length == 2)
                            {
                                string tabName = objs[1].ToString();
                                string tabId = "view" + form.GetHashCode();
                                InvokeController("MainFrame.UI", "LoginController", "ShowForm", form, tabName, tabId);
                            }
                        }
                        return true;
                    case "ShowDialog":
                        if (objs.Length > 0)
                        {
                            Form form = null;
                            if (objs[0] is String)
                            {
                                form = iBaseView[objs[0].ToString()] as Form;
                            }
                            else
                            {
                                form = objs[0] as Form;
                            }
                            return form.ShowDialog();
                        }
                        return false;
                    case "Close":
                        if (objs[0] is Form)
                        {
                            string tabId = "view" + objs[0].GetHashCode();
                            InvokeController("MainFrame.UI", "LoginController", "CloseForm", tabId);
                        }
                        else
                        {
                            InvokeController("MainFrame.UI", "LoginController", "CloseForm", objs);
                        }
                        return true;
                    case "Exit":
                        WinformGlobal.Exit();
                        return null;
                    case "this":
                        return this;
                    case "AsynInitCompleted":
                        if (AsynInitCompletedFinish == false)
                        {
                            AsynInitCompletedFinish = true;
                            AsynInitCompleted();
                        }
                        return true;
                    case "MessageBoxShowSimple":
                        if (objs.Length > 0)
                        {
                            MessageBoxShowSimple(objs[0].ToString());
                        }
                        return true;
                }

                MethodInfo meth = ControllerHelper.CreateMethodInfo(_pluginName + "@" + this.GetType().Name, eventname);
                return meth.Invoke(this, objs);
            }
            catch (Exception err)
            {
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                if(err.InnerException!=null)
                    throw new Exception(err.InnerException.Message);
                throw new Exception(err.Message);
            }
        }

        public bool InitFinish = false;//是否完成初始化
        public bool AsynInitCompletedFinish = false;//是否完成异步初始化
        /// <summary>
        /// 初始化全局web服务参数对象
        /// </summary>
        public virtual void Init() { }

        public virtual void AsynInit()
        {

        }

        public virtual void AsynInitCompleted()
        {

        }

        public virtual IBaseViewBusiness GetView(string frmName)
        {
            return iBaseView[frmName];
        }

        /// <summary>
        /// 执行控制器
        /// </summary>
        /// <returns></returns>
        public Object InvokeController(string puginName, string controllerName, string methodName, params object[] objs)
        {
            try
            {
                WinformController icontroller = ControllerHelper.CreateController(puginName + "@" + controllerName);
                MethodInfo meth = ControllerHelper.CreateMethodInfo(puginName + "@" + controllerName, methodName);
                if (meth == null) throw new Exception("调用的方法名不存在");
                return meth.Invoke(icontroller, objs);
            }
            catch (Exception err)
            {
                //记录错误日志
                ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                throw new Exception(err.Message);
            }
        }

        public void MessageBoxShowSimple(string text)
        {
            InvokeController("MainFrame.UI", "wcfclientLoginController", "ShowBalloonMessage", "", text);
            //return MessageBoxEx.Show(text, "提示");
        }
        public DialogResult MessageBoxShowYesNo(string text)
        {
            return MessageBox.Show(text, "询问",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
        }
        public DialogResult MessageBoxShowError(string text)
        {
            return MessageBox.Show(text, "询问", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
        public DialogResult MessageBoxShow(string text, MessageBoxButtons msgBoxBtn, MessageBoxIcon msgBoxIcon, MessageBoxDefaultButton defaultBtn)
        {
            return MessageBox.Show(text, "提示", msgBoxBtn, msgBoxIcon, defaultBtn);
        }
    }
}
