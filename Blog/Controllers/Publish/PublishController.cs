using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blog.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.Add
{
    public class PublishController : Controller
    {
        public IActionResult Publish()
        {
            return View();
        }
        public async string AddWhisper()
        {
            string content = Request.Form["content"];
            string[] srcArray = Request.Form["imgUrls"].ToString().Trim(',').Split(',');
            //string imgSrc = @"D:\学习\博客\Blog\TemporaryFile\2019\6\20\af7a6d07ad444d2aab0781b90294c9d5.jpg";
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            string url = "https://localhost:5001/api/Upload";
            try
            {
                Parallel.For(0, srcArray.Length, async s =>
                {
                   await UploadHelper.Upload(srcArray[s]);
                    //using (HttpClient client = new HttpClient(handler))
                    //{
                    //    client.MaxResponseContentBufferSize = 256000;
                    //    client.DefaultRequestHeaders.Add("user-agent", "User-Agent    Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; MALNJS; rv:11.0) like Gecko");
                    //    using (MultipartFormDataContent mulContent = new MultipartFormDataContent("----WebKitFormBoundaryrXRBKlhEeCbfHIY"))
                    //    {
                    //        using (FileStream fileStream = new FileStream(srcArray[s], FileMode.Open, FileAccess.Read, FileShare.Read))
                    //        {
                    //            string filename = srcArray[s].Substring(srcArray[s].LastIndexOf("\\"));
                    //            HttpContent httpContent = new StreamContent(fileStream);
                    //            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    //            mulContent.Add(httpContent, "file", filename);
                    //            HttpResponseMessage responseMessage = await client.PostAsync(url, mulContent);
                    //            responseMessage.EnsureSuccessStatusCode();
                    //            string str = responseMessage.Content.ReadAsStringAsync().Result;
                    //        };

                    //    }
                    //}
                });
            }
            catch (AggregateException)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.Method = "DELETE";
                    webRequest.ContentType = "text/xml";
                    byte[] bufer = Encoding.GetEncoding("UTF-8").GetBytes(@"D:\学习\博客\Blog\TemporaryFile\2019\6\20\af7a6d07ad444d2aab0781b90294c9d5.jpg");
                    Stream stream = webRequest.GetRequestStream();
                    stream.Write(bufer, 0, bufer.Length);
                    stream.Close();
                    HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
                    string result = streamReader.ReadToEnd();
                    streamReader.Close();
                    httpResponse.Close();

                }
            }
        }
    }
}