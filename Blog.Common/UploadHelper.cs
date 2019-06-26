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
        public static async  Task<long> Upload(string filePath,string savePath,string fileName)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            long fileSize = fileStream.Length;
            HttpContent httpContent = new StreamContent(fileStream);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    using (MultipartFormDataContent mulContent = new MultipartFormDataContent("----WebKitFormBoundaryrXRBKlhEeCbfHIY"))
            {
                mulContent.Add(httpContent, "file", fileName);
                mulContent.Add(new StringContent(savePath),"savePath");
                mulContent.Add(new StringContent(fileName), "fileName");
                string ip = ConfigurationProvider.configuration.GetSection("webapi:HttpAddresss").Value;
                string url = "http://"+ip + controller;
                await HttpHelper.PostHttpClient(url, mulContent);
            }
            return fileSize;
        }
    }
}
