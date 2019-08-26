using Blog.Common.AppSetting;
using Blog.Domain.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        /// 使用HttpClient上传附件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long Upload(string filePath, string fileName,string userAccount,out string uploadSavePath)
        {
            if (!File.Exists(filePath))
                throw new IOException("文件不存在");
            DateTime dateTime = DateTime.Now;
            ApiSettingModel apiSettingMode=ConfigurationProvider.GetSettingModel<ApiSettingModel>("webapi");
            string savePath  = string.Format("{0}{1}/{2}/{3}/{4}", apiSettingMode.UploadSavePathBase, userAccount, dateTime.Year.ToString()
               , dateTime.Month.ToString(), dateTime.Day.ToString());
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            long fileSize = fileStream.Length;
            HttpContent httpContent = new StreamContent(fileStream);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            using (MultipartFormDataContent mulContent = new MultipartFormDataContent("----WebKitFormBoundaryrXRBKlhEeCbfHIY"))
            {
                mulContent.Add(httpContent, "file", fileName);
                mulContent.Add(new StringContent(savePath), "savePath");
                mulContent.Add(new StringContent(fileName), "fileName");
                string url = GetIP() + controller;
                HttpHelper.PostHttpClient(url, mulContent);
            }
            //上传成功后删除本地文件
            File.Delete(filePath);
            uploadSavePath = savePath+"/"+fileName;
            return fileSize;
        }
        /// <summary>
        /// WebClinet下载附件
        /// </summary>
        /// <param name="httpUrl">原始文件地址</param>
        /// <param name="savePath">保存至本地地址</param>
        /// <returns></returns>
        public static string DownFile(string httpUrl, string saveLocalPath = "")
        {
            DateTime dateTime = DateTime.Now;
            IConfigurationSection section = GetConfigurationSection("webapi");
            if (string.IsNullOrEmpty(saveLocalPath))
            {
                saveLocalPath = string.Format(@"{0}\TempFile\{1}\{2}\", ConstantKey.WebRoot, dateTime.Year.ToString(), dateTime.Month.ToString());

            }
            string fileName = Path.GetFileName(httpUrl);
            if (!Directory.Exists(saveLocalPath))
                Directory.CreateDirectory(saveLocalPath);
            WebClient webClient = new WebClient();
            saveLocalPath = saveLocalPath + fileName;
            webClient.DownloadFile(httpUrl, saveLocalPath);
            int subIndex = saveLocalPath.IndexOf("Down") + 4;
            return string.Format("{0}{1}", ConstantKey.STATIC_FILE, saveLocalPath.Substring(subIndex).Replace(@"\", "/"));
        }
        /// <summary>
        /// WebClinet下载附件
        /// </summary>
        /// <param name="httpUrl">原始文件地址</param>
        /// <param name="savePath">保存至本地地址</param>
        /// <returns></returns>
        public static IList<string> DownFile(IList<string> savePaths)
        {
            DateTime dateTime = DateTime.Now;
            IConfigurationSection section = GetConfigurationSection("webapi");
            string saveLocalPath = saveLocalPath = string.Format(@"{0}\TempFile\{1}\{2}\", ConstantKey.WebRoot, dateTime.Year.ToString(), dateTime.Month.ToString()); ;
            IList<string> localSavePaths = new List<string>();
            string ip ="http://"+section.GetSection("HttpAddresss").Value;
            string UploadSavePathBase = ConfigurationProvider.configuration.GetSection("webapi:UploadSavePathBase").Value;
            Parallel.For(0, savePaths.Count, s =>
            {
                string str = DownFile(ip + "/" + savePaths[s].Replace(UploadSavePathBase, ""), saveLocalPath);
                lock (obj)
                {
                    localSavePaths.Add(str);
                }
            });
            return localSavePaths;
        }
    }
}
