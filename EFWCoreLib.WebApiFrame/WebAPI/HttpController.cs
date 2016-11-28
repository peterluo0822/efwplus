using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Init;

namespace EFWCoreLib.WebApiFrame.WebAPI
{
    /// <summary>
    /// Http服务器
    /// http://localhost:8021/httpep/index.html
    /// </summary>
    [efwplusApiController(PluginName = "coresys")]
    public class HttpController : ApiController
    {
        //下载文件
        [HttpGet]
        public HttpResponseMessage Html(string id)
        {
            try
            {
                string path = AppGlobal.AppRootPath + @"Http\";
                string filePath = path + id;
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists == false)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var stream = new FileStream(filePath, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                //{
                //    FileName = name
                //};
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }
    }
}
