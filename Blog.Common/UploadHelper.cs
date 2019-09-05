using Blog.Common.AppSetting;
using Blog.Domain.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly static object obj = new object();
        private static IConfigurationSection GetConfigurationSection(string sctionKey)
        {
            IConfigurationSection section= ConfigurationProvider.configuration.GetSection(sctionKey);
            return section;
        }
        /// <summary>
        /// 通过配置文件回获取ip
        /// </summary>
        /// <returns></returns>
        private static string GetIP()
        {
            string ip= GetConfigurationSection("webapi").GetSection("HttpAddresss").Value;
            string http = "http://" + ip;
            return http;
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="localFilePath">本地图片相对路径</param>
        /// <param name="contentRootPath">程序路径</param>
        /// <returns></returns>
        public static IList<string>  Upload(string[] localFilePath,string contentRootPath)
        {
            IList<string> savePathList = new List<string>();
            Task[] tasks = new Task[localFilePath.Length];
            for (int i = 0; i < localFilePath.Length; i++)
            {
                int m = i;
                tasks[m] = Task.Run(() =>
                {
                    int index = localFilePath[m].IndexOf(ConstantKey.STATIC_FILE) + ConstantKey.STATIC_FILE.Length;
                    string path = contentRootPath + "/TempFile" + localFilePath[m].Substring(index);
                    path = path.Replace("/", @"\");
                    string fileName = path.Substring(path.LastIndexOf(@"\") + 1);
                    string uploadSavePath;
                    long fileSzie = Upload(path, fileName, out uploadSavePath);
                    lock (obj)
                    {
                        savePathList.Add(uploadSavePath);
                    }
                });
            }
            Task.WaitAll(tasks);
            return savePathList;
        }
        /// <summary>
        /// 使用HttpClient上传附件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long Upload(string localFilePath, string fileName,out string uploadSavePath)
        {
            if (!File.Exists(localFilePath))
                throw new IOException("文件不存在");
            FileStream fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            long fileSize = fileStream.Length;
            HttpContent httpContent = new StreamContent(fileStream);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            string httpResult = "";
            using (MultipartFormDataContent mulContent = new MultipartFormDataContent("----WebKitFormBoundaryrXRBKlhEeCbfHIY"))
            {
                mulContent.Add(httpContent, "file", fileName);
                //mulContent.Add(new StringContent("test"), "test");
                string url = GetIP() + controller;
                httpResult = HttpHelper.PostHttpClient(url, mulContent);
            }
            //上传成功后删除本地文件
            File.Delete(localFilePath);
            dynamic result = JsonHelper.DeserializeObject(httpResult);
            if (result.code == "500")
                throw new FrameworkException("webapi请求错误:" + result.message);
            uploadSavePath = result.savepath;
            return fileSize;
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="uploadPath">远程文件存放相对路径</param>
        /// <returns></returns>
        public static async Task<string> DownFileAsync(string uploadPath)
        {
            DateTime dateTime = DateTime.Now;
            IConfigurationSection section = GetConfigurationSection("webapi");
            string ip = "http://" + section.GetSection("HttpAddresss").Value;
            string loaclPath = string.Format(@"{0}/TempFile/Down/{1}/{2}/", ConstantKey.WebRoot, dateTime.Year.ToString(), dateTime.Month.ToString());
            string url = ip + uploadPath;
            string fileName = Path.GetFileName(url);
            if (!Directory.Exists(loaclPath))
                Directory.CreateDirectory(loaclPath);
            HttpClient httpClient = new HttpClient();
            loaclPath = loaclPath + fileName;
            HttpResponseMessage httpResponseMessage=await httpClient.GetAsync(url.Replace(@"\","/"));
            httpResponseMessage.EnsureSuccessStatusCode();
            Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            stream.Position = 0;
            Image img = Image.FromStream(stream);
            img.Save(loaclPath);
            int subIndex = loaclPath.IndexOf("Down") + 4;
            return  string.Format("{0}{1}", ConstantKey.STATIC_FILE, loaclPath.Substring(subIndex));
        }
        ///// <summary>
        ///// WebClinet下载附件
        ///// </summary>
        ///// <param name="httpUrl">原始文件地址</param>
        ///// <param name="savePath">保存至本地地址</param>
        ///// <returns></returns>
        //public static IList<string> DownFile(IList<string> savePaths)
        //{
        //    DateTime dateTime = DateTime.Now;
        //    IConfigurationSection section = GetConfigurationSection("webapi");
        //    string saveLocalPath = saveLocalPath = string.Format(@"{0}\TempFile\{1}\{2}\", ConstantKey.WebRoot, dateTime.Year.ToString(), dateTime.Month.ToString()); ;
        //    IList<string> localSavePaths = new List<string>();
        //    string ip ="http://"+section.GetSection("HttpAddresss").Value;
        //    string UploadSavePathBase = ConfigurationProvider.configuration.GetSection("webapi:UploadSavePathBase").Value;
        //    Parallel.For(0, savePaths.Count, s =>
        //    {
        //        string str = DownFile(ip + "/" + savePaths[s].Replace(UploadSavePathBase, ""), saveLocalPath);
        //        lock (obj)
        //        {
        //            localSavePaths.Add(str);
        //        }
        //    });
        //    return localSavePaths;
        //}
    }
}
