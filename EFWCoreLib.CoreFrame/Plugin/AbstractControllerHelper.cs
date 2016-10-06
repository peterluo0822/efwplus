using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Business;

namespace EFWCoreLib.CoreFrame.Plugin
{
    public abstract class AbstractControllerHelper
    {
        public abstract AbstractController CreateController(string pluginname,string controllername);

        public abstract MethodInfo CreateMethodInfo(string pluginname, string controllername, string methodname, AbstractController controller);
    }
}
