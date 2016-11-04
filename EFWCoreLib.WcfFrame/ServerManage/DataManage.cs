using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using EFWCoreLib.CoreFrame.Plugin;
using EFWCoreLib.CoreFrame.SSO;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.SDMessageHeader;

namespace EFWCoreLib.WcfFrame.ServerManage
{
    public class DataManage
    {
        public static string ProcessRequest(string clientId, string plugin, string controller, string method, string jsondata, HeaderParameter para)
        {
            string retJson = null;
            try
            {
                if (plugin == null || controller == null)
                    throw new Exception("插件名称或控制器名称不能为空!");

                if (ClientManage.ClientDic.ContainsKey(clientId) == false)
                    throw new Exception("客户端不存在，正在创建新的连接！");

                if (ClientManage.IsToken == true)//非调试模式下才验证
                {
                    //验证身份，创建连接的时候验证，请求不验证
                    IsAuth(plugin, controller, method, para.token);
                }

                //显示调试信息
                if (WcfGlobal.IsDebug == true)
                    ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + clientId + "]正在执行：" + controller + "." + method + "(" + jsondata + ")");


                begintime();

                #region 执行插件控制器的核心算法
                object[] paramValue = null;//jsondata?
                ServiceResponseData retObj = null;
                LocalPlugin localPlugin = RemotePluginManage.GetLocalPlugin();
                if (string.IsNullOrEmpty(para.replyidentify) || localPlugin.ServerIdentify == para.replyidentify)
                {
                    if (localPlugin.PluginDic.ContainsKey(plugin) == true)
                    {
                        //先解密再解压
                        string _jsondata = jsondata;
                        //解密参数
                        if (para.isencryptionjson)
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = _jsondata;
                            des.DesDecrypt();
                            _jsondata = des.OutString;
                        }
                        //解压参数
                        if (para.iscompressjson)
                        {
                            _jsondata = ZipComporessor.Decompress(_jsondata);
                        }

                        ClientRequestData requestData = new ClientRequestData(para.iscompressjson, para.isencryptionjson, para.serializetype);
                        requestData.SetJsonData(_jsondata);
                        requestData.LoginRight = para.LoginRight;

                        EFWCoreLib.CoreFrame.Plugin.ModulePlugin moduleplugin = localPlugin.PluginDic[plugin];
                        retObj = (ServiceResponseData)moduleplugin.WcfServerExecuteMethod(controller, method, paramValue, requestData);

                        if (retObj != null)
                        {
                            retJson = retObj.GetJsonData();
                        }
                        else
                        {
                            retObj = new ServiceResponseData();
                            retObj.Iscompressjson = para.iscompressjson;
                            retObj.Isencryptionjson = para.isencryptionjson;
                            retObj.Serializetype = para.serializetype;

                            retJson = retObj.GetJsonData();
                        }

                        retJson = "{\"flag\":0,\"msg\":" + "\"\"" + ",\"data\":" + retJson + "}";
                        //先压缩再加密
                        //压缩结果
                        if (para.iscompressjson)
                        {
                            retJson = ZipComporessor.Compress(retJson);
                        }
                        //加密结果
                        if (para.isencryptionjson)
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = retJson;
                            des.DesEncrypt();
                            retJson = des.OutString;
                        }
                    }
                    else
                        throw new Exception("本地插件找不到指定的插件");
                }
                else//本地插件找不到，就执行远程插件
                {
                    if (RemotePluginManage.RemotePluginDic.FindIndex(x => x.ServerIdentify == para.replyidentify) > -1)
                    {
                        RemotePlugin rp = RemotePluginManage.RemotePluginDic.Find(x => x.ServerIdentify == para.replyidentify);
                        string[] ps = rp.plugin;

                        if (ps.ToList().FindIndex(x => x == plugin) > -1)
                        {
                            retJson = rp.callback.ReplyProcessRequest(para, plugin, controller, method, jsondata);
                        }
                        else
                            throw new Exception("远程插件找不到指定的插件");
                    }
                    else
                        throw new Exception("远程插件找不到指定的回调中间件");
                }
                #endregion

                double outtime = endtime();
                //记录超时的方法
                if (ClientManage.IsOverTime == true)
                {
                    if (outtime > Convert.ToDouble(ClientManage.OverTime * 1000))
                    {
                        WriterOverTimeLog(outtime, controller + "." + method + "(" + jsondata + ")");
                    }
                }
                //显示调试信息
                if (WcfGlobal.IsDebug == true)
                    ShowHostMsg(Color.Green, DateTime.Now, "客户端[" + clientId + "]收到结果(耗时[" + outtime + "])：" + retJson);

                //更新客户端信息
                ClientManage.UpdateRequestClient(clientId, jsondata == null ? 0 : jsondata.Length, retJson == null ? 0 : retJson.Length);


                if (retJson == null)
                    throw new Exception("插件执行未返回有效数据");

                return retJson;
            }
            catch (Exception err)
            {
                //记录错误日志
                if (err.InnerException == null)
                {
                    retJson = "{\"flag\":1,\"msg\":" + "\"" + err.Message + "\"" + "}";
                    if (para.iscompressjson)
                    {
                        retJson = ZipComporessor.Compress(retJson);
                    }
                    ShowHostMsg(Color.Red, DateTime.Now, "客户端[" + clientId + "]执行失败：" + err.Message);
                    return retJson;
                }
                else
                {
                    retJson = "{\"flag\":1,\"msg\":" + "\"" + err.InnerException.Message + "\"" + "}";
                    if (para.iscompressjson)
                    {
                        retJson = ZipComporessor.Compress(retJson);
                    }
                    ShowHostMsg(Color.Red, DateTime.Now, "客户端[" + clientId + "]执行失败：" + err.InnerException.Message);
                    return retJson;
                }
            }
        }

        public static string ReplyProcessRequest(string plugin, string controller, string method, string jsondata, HeaderParameter para)
        {
            string retJson = null;

            try
            {

                //显示调试信息
                if (WcfGlobal.IsDebug == true)
                    ShowHostMsg(Color.Black, DateTime.Now, "客户端[本地回调]正在执行：" + controller + "." + method + "(" + jsondata + ")");

                begintime();

                #region 执行插件控制器的核心算法
                object[] paramValue = null;//jsondata?
                ServiceResponseData retObj = null;
                LocalPlugin localPlugin = RemotePluginManage.GetLocalPlugin();
                if (string.IsNullOrEmpty(para.replyidentify) || localPlugin.ServerIdentify == para.replyidentify)
                {
                    if (localPlugin.PluginDic.ContainsKey(plugin) == true)
                    {
                        //先解密再解压
                        string _jsondata = jsondata;
                        //解密参数
                        if (para.isencryptionjson)
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = _jsondata;
                            des.DesDecrypt();
                            _jsondata = des.OutString;
                        }
                        //解压参数
                        if (para.iscompressjson)
                        {
                            _jsondata = ZipComporessor.Decompress(_jsondata);
                        }

                        ClientRequestData requestData = new ClientRequestData(para.iscompressjson, para.isencryptionjson, para.serializetype);
                        requestData.SetJsonData(_jsondata);
                        requestData.LoginRight = para.LoginRight;

                        EFWCoreLib.CoreFrame.Plugin.ModulePlugin moduleplugin = localPlugin.PluginDic[plugin];
                        retObj = (ServiceResponseData)moduleplugin.WcfServerExecuteMethod(controller, method, paramValue, requestData);

                        if (retObj != null)
                        {
                            retJson = retObj.GetJsonData();
                        }
                        else
                        {
                            retObj = new ServiceResponseData();
                            retObj.Iscompressjson = para.iscompressjson;
                            retObj.Isencryptionjson = para.isencryptionjson;
                            retObj.Serializetype = para.serializetype;

                            retJson = retObj.GetJsonData();
                        }

                        retJson = "{\"flag\":0,\"msg\":" + "\"\"" + ",\"data\":" + retJson + "}";
                        //先压缩再加密
                        //压缩结果
                        if (para.iscompressjson)
                        {
                            retJson = ZipComporessor.Compress(retJson);
                        }
                        //加密结果
                        if (para.isencryptionjson)
                        {
                            DESEncryptor des = new DESEncryptor();
                            des.InputString = retJson;
                            des.DesEncrypt();
                            retJson = des.OutString;
                        }
                    }
                    else
                        throw new Exception("本地插件找不到指定的插件");
                }
                else//本地插件找不到，就执行远程插件
                {
                    if (RemotePluginManage.RemotePluginDic.FindIndex(x => x.ServerIdentify == para.replyidentify) > -1)
                    {
                        RemotePlugin rp = RemotePluginManage.RemotePluginDic.Find(x => x.ServerIdentify == para.replyidentify);
                        string[] ps = rp.plugin;

                        if (ps.ToList().FindIndex(x => x == plugin) > -1)
                        {
                            retJson = rp.callback.ReplyProcessRequest(para, plugin, controller, method, jsondata);
                        }
                        else
                            throw new Exception("远程插件找不到指定的插件");
                    }
                    else
                        throw new Exception("远程插件找不到指定的回调中间件");
                }
                #endregion

                //System.Threading.Thread.Sleep(20000);//测试并发问题，此处也没有问题
                double outtime = endtime();
                //记录超时的方法
                if (ClientManage.IsOverTime == true)
                {
                    if (outtime > Convert.ToDouble(ClientManage.OverTime * 1000))
                    {
                        WriterOverTimeLog(outtime, controller + "." + method + "(" + jsondata + ")");
                    }
                }
                //显示调试信息
                if (WcfGlobal.IsDebug == true)
                    ShowHostMsg(Color.Green, DateTime.Now, "客户端[本地回调]收到结果(耗时[" + outtime + "])：" + retJson);

                if (retJson == null)
                    throw new Exception("请求的插件未获取到有效数据");

                return retJson;
            }
            catch (Exception err)
            {
                //记录错误日志
                //EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");

                if (err.InnerException == null)
                {
                    retJson = "{\"flag\":1,\"msg\":" + "\"" + err.Message + "\"" + "}";
                    if (para.iscompressjson)
                    {
                        retJson = ZipComporessor.Compress(retJson);
                    }
                    ShowHostMsg(Color.Red, DateTime.Now, "客户端[本地回调]执行失败：" + err.Message);
                    return retJson;
                }
                else
                {
                    retJson = "{\"flag\":1,\"msg\":" + "\"" + err.InnerException.Message + "\"" + "}";
                    if (para.iscompressjson)
                    {
                        retJson = ZipComporessor.Compress(retJson);
                    }
                    ShowHostMsg(Color.Red, DateTime.Now, "客户端[本地回调]执行失败：" + err.InnerException.Message);
                    return retJson;
                }

            }
        }

        private static void ShowHostMsg(Color clr, DateTime time, string text)
        {
            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, clr, text);
        }
        static DateTime begindate;
        static void begintime()
        {
            begindate = DateTime.Now;
        }
        //返回毫秒
        static double endtime()
        {
            return DateTime.Now.Subtract(begindate).TotalMilliseconds;
        }
        private static void WriterOverTimeLog(double overtime, string text)
        {
            string info = "耗时：" + overtime + "\t\t" + "方法：" + text + "\r\n";
            MiddlewareLogHelper.WriterLog(LogType.OverTimeLog, true, Color.Red, info);
        }

        //每次请求的身份验证，分布式情况下验证麻烦
        private static bool IsAuth(string pname, string cname, string methodname, string token)
        {
            ModulePlugin mp;
            WcfControllerAttributeInfo cattr = AppPluginManage.GetPluginWcfControllerAttributeInfo(pname, cname, out mp);
            if (cattr == null) throw new Exception("插件中没有此控制器名");
            WcfMethodAttributeInfo mattr = cattr.MethodList.Find(x => x.methodName == methodname);
            if (mattr == null) throw new Exception("控制器中没有此方法名");

            if (mattr.IsAuthentication)
            {
                if (token == null)
                    throw new Exception("no token");

                AuthResult result = SsoHelper.ValidateToken(token);
                if (result.ErrorMsg != null)
                    throw new Exception(result.ErrorMsg);

                SysLoginRight loginInfo = new SysLoginRight();
                loginInfo.UserId = Convert.ToInt32(result.User.UserId);
                loginInfo.EmpName = result.User.UserName;

                //clientinfo.LoginRight = loginInfo;
            }

            return true;
        }
    }

    /// <summary>
    /// 异步结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompletedAsyncResult<T> : IAsyncResult
    {
        T data;

        public CompletedAsyncResult(T data)
        {
            this.data = data;
        }

        public T Data
        { get { return data; } }

        #region IAsyncResult Members
        public object AsyncState
        { get { return (object)data; } }

        public WaitHandle AsyncWaitHandle
        { get { throw new Exception("The method or operation is not implemented."); } }

        public bool CompletedSynchronously
        { get { return true; } }

        public bool IsCompleted
        { get { return true; } }
        #endregion
    }
}
