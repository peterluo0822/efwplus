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
    /// 客户端升级服务器
    /// http://localhost:8021/upgrade/update.xml
    /// </summary>
    [efwplusApiController(PluginName = "coresys")]
    public class ClientUpgradeController : ApiController
    {
        //下载文件
        [HttpGet]
        public HttpResponseMessage Upgrade(string id)
        {
            try
            {
                string path = AppGlobal.AppRootPath + @"FileStore\ClientUpgrade\";
                string filePath = path + id;
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists == false)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var stream = new FileStream(filePath, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = id
                };
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }
    }
}
