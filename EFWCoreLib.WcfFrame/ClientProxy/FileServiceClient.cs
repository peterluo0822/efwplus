using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.WcfHandler;

namespace EFWCoreLib.WcfFrame.ClientProxy
{
    public class FileServiceClient: ClientBase<IFileHandler>, IFileHandler
    {
        public FileServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        { }

        public DownFileResult DownLoadFile(DownFile downfile)
        {
            return this.Channel.DownLoadFile(downfile);
        }

        public UpFileResult UpLoadFile(UpFile filestream)
        {
            return this.Channel.UpLoadFile(filestream);
        }
    }
}
