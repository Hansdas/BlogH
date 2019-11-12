﻿using Blog.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Blog.Common
{
    /// <summary>
    /// 文件存储结果
    /// </summary>
    public struct PathValue
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 路径日期部分
        /// </summary>
        public string DatePath { get; set; }
    }
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
                throw new ServiceException("webapi请求错误:" + result.message);
            uploadSavePath = result.savepath;
            return fileSize;
        }
        /// <summary>
        /// 使用HttpClient上传附件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static PathValue Upload(string localFilePath, string fileName)
        {
            if (!File.Exists(localFilePath))
                throw new IOException("文件不存在");
            FileStream fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            HttpContent httpContent = new StreamContent(fileStream);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            string httpResult = "";
            using (MultipartFormDataContent mulContent = new MultipartFormDataContent("----WebKitFormBoundaryrXRBKlhEeCbfHIY"))
            {
                mulContent.Add(httpContent, "file", fileName);
                string url = GetIP() + controller;
                httpResult = HttpHelper.PostHttpClient(url, mulContent);
            }
            //上传成功后删除本地文件
            File.Delete(localFilePath);
            dynamic result = JsonHelper.DeserializeObject(httpResult);
            if (result.code == "500")
                throw new ServiceException("webapi请求错误:" + result.message);
            PathValue pathValue = new PathValue();
            pathValue.DatePath = result.datepath;
            pathValue.FilePath = GetIP()+"/"+pathValue.DatePath;
            pathValue.FileName = fileName;
            return pathValue;
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
        public static void DeleteFile(string path)
        {
            string ip = GetIP();
            int index = path.IndexOf(ip);
            string paramaters = path.Substring(index + ip.Length).Replace(".",@"/");
            string url = GetIP() + controller+ paramaters;
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse =httpClient.DeleteAsync(url).Result;
            httpResponse.EnsureSuccessStatusCode();
            string result = httpResponse.Content.ReadAsStringAsync().Result ;
        }

        /// <summary>
        /// 重新压缩图片高和宽，如果原图片小于期望值则不变
        /// </summary>
        /// <param name="imgStream">图片流</param>
        /// <param name="heigth">期望最大高度</param>
        /// <param name="width">期望最大宽度</param>
        /// <param name="isAbs">如果为true，则压缩为指定大小，否则相对原大小压缩</param>
        /// <returns></returns>
        private static Stream CompressImage(Stream imgStream,int maxHeigth,int maxWidth,bool isAbs)
        {
            using (Image image = Image.FromStream(imgStream))
            {
                int sourceWidth = image.Width;
                //获取图片高度
                int sourceHeight = image.Height;
                int height = image.Height, width = image.Width;
                if (isAbs)
                {
                    height = maxHeigth;
                    width = maxWidth;
                }
                else
                {
                    if (image.Height > maxHeigth)
                        height = maxHeigth;
                    if (image.Width > maxWidth)
                        width = maxWidth;
                }
                float nPercentW = 0;
                float nPercentH = 0;
                //计算宽度的缩放比例
                nPercentW = ((float)width/ (float)sourceWidth);
                //计算高度的缩放比例
                nPercentH = ((float)height / (float)sourceHeight);

                //期望的宽度
                int destWidth = (int)(sourceWidth * nPercentW);
                //期望的高度
                int destHeight = (int)(sourceHeight * nPercentH);
                using (Bitmap bitmap = new Bitmap(destWidth, destHeight))
                {

                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //绘制图像
                        graphics.DrawImage(image, 0, 0, destWidth, destHeight);
                        graphics.Dispose();
                        MemoryStream memoryStream = new MemoryStream();
                        if (image.RawFormat == ImageFormat.Jpeg)
                            bitmap.Save(memoryStream, ImageFormat.Jpeg);
                        else
                            bitmap.Save(memoryStream, ImageFormat.Png);
                        return memoryStream;
                    }
                }
            }
        }
        /// <summary>
        /// 重新压缩图片高和宽，如果原图片小于期望值则不变
        /// </summary>
        /// <param name="saveLoacalPath">本地存放路径</param>
        /// <param name="imgStream">图片流</param>
        /// <param name="heigth">期望最大高度</param>
        /// <param name="width">期望最大宽度</param>
        /// <param name="isAbs">如果为true，则压缩为指定大小，否则相对原大小压缩</param>
        /// <returns></returns>
        public static void CompressImage(string saveLoacalPath, Stream imgStream, int maxHeigth, int maxWidth, bool isAbs=false)
        {
            using (FileStream fs = File.Create(saveLoacalPath))
            {
                using (Stream stream = CompressImage(imgStream, maxHeigth, maxWidth, false))
                {
                    byte[] srcBuf = StreamToBytes(stream);

                    fs.Write(srcBuf, 0, srcBuf.Length);
                    fs.Flush();
                }
            }
        }
        /// <summary>
        /// 保存文件到本地，返回图片路径
        /// </summary>
        /// <param name="fileNameWithExtension"></param>
        /// <returns></returns>
        public static PathValue SaveFile(string fileNameWithExtension)
        {
            int index = fileNameWithExtension.LastIndexOf('.');
            string extension = fileNameWithExtension.Substring(index, fileNameWithExtension.Length - index);//获取后缀名
            string webpath = ConstantKey.WebRoot;//网站根路径
            string guid = Guid.NewGuid().ToString().Replace("-", "");//生成guid
            string newFileName = guid + extension;
            DateTime dateTime = DateTime.Now;
            string datePath = string.Format(@"\{0}\{1}\{2}\", dateTime.Year, dateTime.Month, dateTime.Day);//路径日期部分
            string fullPath = string.Format(@"{0}\TempFile{1}", webpath, datePath);//全路径
            DirectoryHelper.CreateDirectory(fullPath);//创建目录
            PathValue pathValue = new PathValue();
            pathValue.DatePath = datePath;
            pathValue.FileName = newFileName;
            pathValue.FilePath = fullPath + newFileName;
            return pathValue;

        }
        /// <summary>
        /// 将Stream放入byte组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

    }
}
