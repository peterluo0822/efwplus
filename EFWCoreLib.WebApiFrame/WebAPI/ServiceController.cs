using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EFWCoreLib.WcfFrame.DataSerialize;

namespace EFWCoreLib.WebFrame.WebAPI
{
    /// <summary>
    /// WepApi 文件传输
    /// /efwplusApi/coresys/service/
    /// </summary>
    [efwplusApiController(PluginName = "coresys")]
    public class ServiceController : WebApiController
    {
        //Accept: application/json
        //Content-Type: application/json
        //efwplusApi/coresys/service/get?wcfpluginname=Books.Service&wcfcontroller=bookWcfController&wcfmethod=GetBooks&jsondata=[]&token=111111
        public Object Get([FromUri]ServiceParam para)
        {
            try
            {
                if (para == null) return null;
                Action<ClientRequestData> requestAction = ((ClientRequestData request) =>
                {
                    request.Iscompressjson = false;
                    request.Isencryptionjson = false;
                    request.Serializetype = SerializeType.Newtonsoft;
                    request.SetJsonData(para.jsondata);
                });

                ServiceResponseData response = InvokeWcfService(para.wcfpluginname, para.wcfcontroller, para.wcfmethod, requestAction);
                return JsonConvert.DeserializeObject(response.GetJsonData());
            }
            catch (Exception err)
            {
                return "服务执行错误###" + err.Message;
            }
        }
        //Accept: application/json
        //Content-Type: application/json
        ///efwplusApi/coresys/service/post?token=111111
        ///{"wcfpluginname":"Books.Service","wcfcontroller":"bookWcfController","wcfmethod":"GetBooks","jsondata":"[]"}
        public Object Post([FromBody]ServiceParam data)
        {
            try {
                if (data == null) return null;
                Action<ClientRequestData> requestAction = ((ClientRequestData request) =>
                {
                    request.Iscompressjson = false;
                    request.Isencryptionjson = false;
                    request.Serializetype = SerializeType.Newtonsoft;
                    request.SetJsonData(data.jsondata);
                });
                ServiceResponseData response = InvokeWcfService(data.wcfpluginname, data.wcfcontroller, data.wcfmethod, requestAction);
                return JsonConvert.DeserializeObject(response.GetJsonData());
            }
            catch (Exception err)
            {
                return "服务执行错误###" + err.Message;
            }
        }
    }

    public class ServiceParam
    {

        public string wcfpluginname { get; set; }
        public string wcfcontroller { get; set; }
        public string wcfmethod { get; set; }
        public string jsondata { get; set; }
    }
}
