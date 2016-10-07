using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EFWCoreLib.CoreFrame.SSO
{
    [DataContract]
    public class TokenInfo
    {
        [DataMember]
        public Guid tokenId { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public DateTime ActivityTime { get; set; }
        [DataMember]
        public string RemoteIp { get; set; }
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public UserInfo userinfo { get; set; }
    }
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public int EmpId { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int DeptId { get; set; }
        [DataMember]
        public string DeptName { get; set; }
        [DataMember]
        public int WorkId { get; set; }
        [DataMember]
        public string WorkName { get; set; }
        [DataMember]
        public int IsAdmin { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
    }
}
