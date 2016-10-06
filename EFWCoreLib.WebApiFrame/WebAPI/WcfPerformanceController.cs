using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame;
using EFWCoreLib.WcfFrame.DataSerialize;
using Newtonsoft.Json;

namespace EFWCoreLib.WebFrame.WebAPI
{
    /// <summary>
    /// 通过WebAPI测试中间件性能
    /// /efwplusApi/coresys/wcfperformance/request
    /// </summary>
    [efwplusApiController(PluginName = "coresys")]
    public class WcfPerformanceController: WebApiController
    {
        [HttpGet]
        public Object Request()
        {
            try
            {
                Action<ClientRequestData> requestAction = ((ClientRequestData request) =>
                {
                    request.Iscompressjson = false;
                    request.Isencryptionjson = false;
                    request.Serializetype = SerializeType.Newtonsoft;
                    request.SetJsonData("[]");
                });

                ServiceResponseData response = InvokeWcfService("Books.Service", "bookWcfController", "GetBooks", requestAction);
                //return JsonConvert.DeserializeObject(response.GetJsonData());
                return true;
            }
            catch (Exception err)
            {
                return "服务执行错误###" + err.Message;
            }
        }

        [HttpGet]
        public Object RequestAsync()
        {
            try
            {
                Action<ClientRequestData> requestAction = ((ClientRequestData request) =>
                {
                    request.Iscompressjson = false;
                    request.Isencryptionjson = false;
                    request.Serializetype = SerializeType.Newtonsoft;
                    request.SetJsonData("[]");
                });

                Action<ServiceResponseData> responseAction = ((ServiceResponseData response) =>
                {
                    
                });
                InvokeWcfServiceAsync("Books.Service", "bookWcfController", "GetBooks", requestAction, responseAction);
                //return JsonConvert.DeserializeObject(response.GetJsonData());
                return true;
            }
            catch (Exception err)
            {
                return "服务执行错误###" + err.Message;
            }
        }
    }
}
