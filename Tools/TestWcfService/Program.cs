using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWcfService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            setprivatepath();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmTest());
        }

        static void setprivatepath()
        {
            //AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = @"Component;ModulePlugin\Books_Wcf\dll;ModulePlugin\WcfMainUIFrame\dll";
            string privatepath = @"Component";

            foreach (var p in PluginSysManage.GetAllPlugin())
            {
                privatepath += ";" + p.Replace("plugin.xml", "dll");
            }

            AppDomain.CurrentDomain.SetData("PRIVATE_BINPATH", privatepath);
            AppDomain.CurrentDomain.SetData("BINPATH_PROBE_ONLY", privatepath);
            var m = typeof(AppDomainSetup).GetMethod("UpdateContextProperty", BindingFlags.NonPublic | BindingFlags.Static);
            var funsion = typeof(AppDomain).GetMethod("GetFusionContext", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(null, new object[] { funsion.Invoke(AppDomain.CurrentDomain, null), "PRIVATE_BINPATH", privatepath });
        }
    }
}
