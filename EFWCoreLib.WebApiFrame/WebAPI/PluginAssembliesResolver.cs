using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using EFWCoreLib.CoreFrame.Init;

namespace EFWCoreLib.WebFrame.WebAPI
{
    public class PluginAssembliesResolver : DefaultAssembliesResolver
    {
        #region IAssembliesResolver 成员

        public override ICollection<System.Reflection.Assembly> GetAssemblies()
        {
            List<System.Reflection.Assembly> list = new List<System.Reflection.Assembly>();
            foreach (var p in AppPluginManage.PluginDic)
            {
                list.AddRange(p.Value.DllList);
            }
            list.Add(System.Reflection.Assembly.Load("EFWCoreLib.CoreFrame"));
            list.Add(System.Reflection.Assembly.Load("EFWCoreLib.WinformFrame"));
            list.Add(System.Reflection.Assembly.Load("EFWCoreLib.WcfFrame"));
            list.Add(System.Reflection.Assembly.Load("EFWCoreLib.WebApiFrame"));
            return list;
        }

        #endregion
    }
}
