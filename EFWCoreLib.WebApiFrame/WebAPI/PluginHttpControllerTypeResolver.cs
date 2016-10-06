using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace EFWCoreLib.WebFrame.WebAPI
{
    public class PluginHttpControllerTypeResolver : DefaultHttpControllerTypeResolver
    {
        public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            return base.GetControllerTypes(assembliesResolver);
        }
    }
}
