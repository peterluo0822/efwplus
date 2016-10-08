using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.DataSerialize
{
    /// <summary>
    /// 上传文件结构
    /// </summary>
    [MessageContract]
    public class UpFile
    {
        [MessageHeader]
        public string clientId { get; set; }
        [MessageHeader]
        public string UpKey { get; set; }
        [MessageHeader]
        public long FileSize { get; set; }
        [MessageHeader]
        public string FileName { get; set; }
        [MessageHeader]
        public string FileExt { get; set; }//带.
        [MessageBodyMember]
        public Stream FileStream { get; set; }
    }
    /// <summary>
    /// 上传文件后返回结果数据
    /// </summary>
    [MessageContract]
    public class UpFileResult
    {
        [MessageHeader]
        public bool IsSuccess { get; set; }
        [MessageHeader]
        public string Message { get; set; }
    }
    /// <summary>
    /// 下载文件结构
    /// </summary>
    [MessageContract]
    public class DownFile
    {
        [MessageHeader]
        public string clientId { get; set; }
        [MessageHeader]
        public string DownKey { get; set; }
        [MessageHeader]
        public string FileName { get; set; }
    }
    /// <summary>
    /// 下载文件后返回结果数据
    /// </summary>
    [MessageContract]
    public class DownFileResult
    {
        [MessageHeader]
        public long FileSize { get; set; }
        [MessageHeader]
        public bool IsSuccess { get; set; }
        [MessageHeader]
        public string Message { get; set; }
        [MessageBodyMember]
        public Stream FileStream { get; set; }
    }
}
