using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.DataSerialize
{
    /// <summary>
    /// 缓存对象
    /// </summary>
    [DataContract]
    public class CacheObject
    {
        /// <summary>
        /// 中间件标识
        /// </summary>
        [DataMember]
        public string ServerIdentify { get; set; }
        /// <summary>
        /// 缓存名称
        /// </summary>
        [DataMember]
        public string cachename { get; set; }
        /// <summary>
        /// 唯一标识
        /// </summary>
        [DataMember]
        public double identify { get; set; }
        /// <summary>
        /// 缓存数据集合
        /// </summary>
        [DataMember]
        public List<CacheData> cacheValue { get; set; }
    }

    /// <summary>
    /// 缓存数据
    /// </summary>
    [DataContract]
    public class CacheData
    {

        [DataMember]
        public double timestamp { get; set; }
        [DataMember]
        public string key { get; set; }
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public bool deleteflag { get; set; }
    }
    /// <summary>
    /// 缓存标识
    /// </summary>
    [DataContract]
    public class CacheIdentify
    {
        [DataMember]
        public string ServerIdentify { get; set; }
        [DataMember]
        public string cachename { get; set; }
        [DataMember]
        public double identify { get; set; }
        [DataMember]
        public IDictionary<string, double> keytimestamps { get; set; }
    }



}
