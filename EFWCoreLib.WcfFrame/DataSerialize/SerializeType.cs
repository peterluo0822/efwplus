using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.DataSerialize
{
    [DataContract]
    public enum SerializeType
    {
        [EnumMember]
        Newtonsoft = 0,
        [EnumMember]
        protobuf = 1,
        [EnumMember]
        fastJSON =2
    }
}
