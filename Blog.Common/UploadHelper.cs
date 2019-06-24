using Blog.Common.AppSetting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common
{
    /// <summary>
    /// 上传帮助类文件
    /// </summary>
    public class UploadHelper
    {
        private static readonly string controller = "/api/Upload";
        /// <summary>
        /// 使用HttpClient上传附件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async  Task<long> Upload(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            HttpContent httpContent = new StreamContent(fileStream);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            string filename = filePath.Substring(filePath.LastIndexOf("\\") + 2);
            //NameValueCollection nameValueCollection = new NameValueCollection();
            //nameValueCollection.Add("user-agent", "User-Agent    Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; MALNJS; rv:11.0) like Gecko");
            using (MultipartFormDataContent mulContent = new MultipartFormDataContent("----WebKitFormBoundaryrXRBKlhEeCbfHIY"))
            {
                mulContent.Add(httpContent, "file", filename);
                string ip = ConfigurationProvider.configuration.GetSection("webapi:HttpAddresss").Value;
                string url = "http://"+ip + controller;
                await HttpHelper.PostHttpClient(url, mulContent);
            }
            return fileStream.Length;

        }
    }
}
