using ProtoBuf;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// 系统登录后存在Session中用户的信息
    /// </summary>
    [Serializable]
    [ProtoContract]
    [DataContract]
    public class SysLoginRight
    {
        private int _userId;
        [XmlElement]
        [ProtoMember(1)]
        [DataMember]
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        private int _empId;
        [XmlElement]
        [ProtoMember(2)]
        [DataMember]
        public int EmpId
        {
            get { return _empId; }
            set { _empId = value; }
        }
        private string _empName;
        [XmlElement]
        [ProtoMember(3)]
        [DataMember]
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; }
        }
        private int _deptId;
        [XmlElement]
        [ProtoMember(4)]
        [DataMember]
        public int DeptId
        {
            get { return _deptId; }
            set { _deptId = value; }
        }
        private string _deptName;
        /// <summary>
        /// 当前登录科室
        /// </summary>
        [XmlElement]
        [ProtoMember(5)]
        [DataMember]
        public string DeptName
        {
            get { return _deptName; }
            set { _deptName = value; }
        }
        private int _workId;
        [XmlElement]
        [ProtoMember(6)]
        [DataMember]
        public int WorkId
        {
            get { return _workId; }
            set { _workId = value; }
        }

        private string _workName;
        [XmlElement]
        [ProtoMember(7)]
        [DataMember]
        public string WorkName
        {
            get { return _workName; }
            set { _workName = value; }
        }
        private int _isAdmin;
        /// <summary>
        /// 是否管理员 0普通用户 1机构管理员 2超级管理员
        /// </summary>
        [XmlElement]
        [ProtoMember(8)]
        [DataMember]
        public int IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }
        [XmlElement]
        [ProtoMember(8)]
        [DataMember]
        public Guid token { get; set; }
    }
}
