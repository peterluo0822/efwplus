using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.SSO;

namespace EFWCoreLib.WebFrame.WebAPI
{
    public class UserTokenActionFilter : ActionFilterAttribute
    {
        private const string Key = "__user_token__";
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
             
            if (WebApiFrame.WebApiGlobal.IsToken == true)
            {
                //登陆之外的请求验证token
                if (actionContext.Request.RequestUri.AbsolutePath.ToLower().IndexOf("/efwplusApi/coresys/login/userlogin".ToLower()) == -1)
                {

                    string token = null;
                    string[] qs = actionContext.Request.RequestUri.Query.ToLower().Split(new char[] { '?', '&' });
                    foreach (var s in qs)
                    {
                        string[] kv = s.Split(new char[] { '=' });
                        if (kv.Length == 2 && kv[0] == "token")
                        {
                            token = kv[1];
                            break;
                        }
                    }

                    if (token == null)
                        throw new Exception("no token");

                    AuthResult result = SsoHelper.ValidateToken(token);
                    if (result.ErrorMsg != null)
                        throw new Exception(result.ErrorMsg);


                    SysLoginRight loginInfo = new SysLoginRight();
                    loginInfo.EmpId = result.User.EmpId;
                    //loginInfo.UserId =;
                    loginInfo.EmpName = result.User.UserName;
                    loginInfo.DeptId = result.User.DeptId;
                    loginInfo.DeptName = result.User.DeptName;
                    loginInfo.WorkId = result.User.WorkId;
                    loginInfo.WorkName = result.User.WorkName;
                    loginInfo.IsAdmin = result.User.IsAdmin;
                    loginInfo.token = Guid.Parse(result.token);

                    actionContext.Request.Properties[Key] = loginInfo;
                }
            } 
        }
    }
}
