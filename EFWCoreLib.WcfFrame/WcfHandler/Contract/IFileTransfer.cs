using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.WcfFrame.DataSerialize;

namespace EFWCoreLib.WcfFrame.WcfService.Contract
{
    /// <summary>
    /// 文件传输服务
    /// </summary>
    [ServiceKnownType(typeof(DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/", Name = "FileTransferHandlerService", SessionMode = SessionMode.Allowed)]
    public interface IFileTransfer
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filestream">文件信息</param>
        /// <returns>返回上传结果</returns>
        [OperationContract]
        UpFileResult UpLoadFile(UpFile filestream);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="downfile">文件信息</param>
        /// <returns>返回下载结果</returns>
        [OperationContract]
        DownFileResult DownLoadFile(DownFile downfile);
    }

    
}
