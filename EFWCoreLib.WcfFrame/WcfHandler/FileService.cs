using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.ServerManage;

namespace EFWCoreLib.WcfFrame.WcfHandler
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = false)]
    public class FileService : IFileHandler
    {
        public DownFileResult DownLoadFile(DownFile downfile)
        {
            return FileManage.DownLoadFile(downfile);
        }

        public UpFileResult UpLoadFile(UpFile filestream)
        {
            return FileManage.UpLoadFile(filestream);
        }
    }
}
