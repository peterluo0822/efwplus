using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EFWCoreLib.CoreFrame.SSO
{

    public class AuthResult
    {
        public string token { get; set; }

        public UserInfo User { get; set; }

        public string ErrorMsg { get; set; }
    }
}
