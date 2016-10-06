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

namespace EFWCoreLib.WebFrame.WebAPI
{
    /// <summary>
    /// WepApi 文件传输
    /// /efwplusApi/coresys/file/
    /// </summary>
    [efwplusApiController(PluginName = "coresys")]
    public class FileController : ApiController
    {
        //返回上传后的文件名
        [HttpPost]
        public IEnumerable<string> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string path = AppGlobal.AppRootPath + @"filebuffer\";
            var provider = new CustomMultipartFormDataStreamProvider(path);

            try
            {
                List<string> files = new List<string>();

                var task = Request.Content.ReadAsMultipartAsync(provider);

                task.Wait();

                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    //sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                    files.Add(fileInfo.Name);
                }
                return files;
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        //下载文件
        [HttpGet]
        public HttpResponseMessage Download(string id)
        {
            try
            {
                string path = AppGlobal.AppRootPath + @"filebuffer\";
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

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path)
        { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            var sb = new StringBuilder((headers.ContentDisposition.FileName ?? DateTime.Now.Ticks.ToString()).Replace("\"", "").Trim().Replace(" ", "_"));
            Array.ForEach(Path.GetInvalidFileNameChars(), invalidChar => sb.Replace(invalidChar, '-'));
            return DateTime.Now.Ticks.ToString() + "_" + sb.ToString();
        }
    }
}
