using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EFWCoreLib.CoreFrame.EntLib;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Plugin;

namespace EFWCoreLib.WinformFrame
{
    /// <summary>
    /// Winform程序
    /// </summary>
    public class WinformGlobal
    {
        public static ILoading winfromMain;

        /// <summary>
        /// 启动程序
        /// </summary>
        public static void Main()
        {
            AppGlobal.AppRootPath = System.Windows.Forms.Application.StartupPath + "\\";
            AppGlobal.appType = AppType.Winform;
            AppGlobal.IsSaas= System.Configuration.ConfigurationManager.AppSettings["IsSaas"] == "true" ? true : false;
            FrmSplash frmSplash = new FrmSplash(Init);
            winfromMain = frmSplash as ILoading;
            System.Windows.Forms.Application.Run(frmSplash);
        }

        private static bool Init()
        {
            try
            {

                AppGlobal.AppStart();

                if (AppGlobal.missingDll.Count > 0)
                {
                    string msg = "缺失的程序集：\r";
                    for (int i = 0; i < AppGlobal.missingDll.Count; i++)
                    {
                        msg += AppGlobal.missingDll[i] + "\r";
                    }
                    MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                string entryplugin;
                string entrycontroller;

                PluginSysManage.GetWinformEntry(out entryplugin, out entrycontroller);
                EFWCoreLib.WinformFrame.Controller.WinformController controller = EFWCoreLib.WinformFrame.Controller.ControllerHelper.CreateController(entryplugin + "@" + entrycontroller);
                //controller.Init();
                if (controller == null)
                    throw new Exception("插件配置的启动项（插件名或控制器名称）不正确！");
                ((System.Windows.Forms.Form)controller.DefaultView).Show();
                winfromMain.MainForm = ((System.Windows.Forms.Form)controller.DefaultView);


                return true;

            }
            catch (Exception err)
            {
                //记录错误日志
                ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                //Application.Exit();
                //throw new Exception(err.Message + "\n\n请联系管理员！");
                MessageBox.Show(err.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //AppExit();
                return false;
            }
        }
        /// <summary>
        /// 退出程序
        /// </summary>
        public static void Exit()
        {
            (winfromMain as Form).Dispose();
        }

        public static void AppConfig()
        {
            FrmConfig config = new FrmConfig();
            config.ShowDialog();
        }
    }
}
