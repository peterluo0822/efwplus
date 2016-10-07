using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Init;

namespace EFWCoreLib.WcfFrame
{
    public class WcfGlobal
    {
        public static void Main()
        {
            AppGlobal.AppRootPath = System.Windows.Forms.Application.StartupPath + "\\";
            AppGlobal.appType = AppType.WCF;
            AppGlobal.IsSaas = System.Configuration.ConfigurationManager.AppSettings["IsSaas"] == "true" ? true : false;
            AppGlobal.AppStart();
        }

        public static void Exit()
        {

        }
    }
}
