
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace CommonHelper
{
   public  class WebHelper
    {
        public static readonly string httpAddress = "http://58.87.92.221:22";
        public static readonly string basePath = "/usr/upload/";
        public static string UploadFile(string localFilePath, NameValueCollection collection,CookieContainer cookies)
        {
            DateTime dateTime = DateTime.Now;
            string path = string.Format("{0}/{1}/{2}/{3}", basePath, dateTime.Year.ToString(), dateTime.Month.ToString(), dateTime.Day.ToString());
            string postData = "?";
            foreach(string key in collection.Keys)
            {
                postData += key + "=" + collection.Get(key)+"&";
            }
            Uri uri = new Uri(httpAddress+ path + postData);
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
            webrequest.CookieContainer = cookies;
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";

            // Build up the post message header  
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(localFilePath));
            sb.Append("\"");
            sb.Append("");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("");
            sb.Append("");
            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("--" + boundary + "");
            FileStream fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + boundaryBytes.Length + fileStream.Length;
            webrequest.ContentLength = length;
            Stream requestStream = webrequest.GetRequestStream();
            requestStream.Write(postHeaderBytes,0,boundaryBytes.Length);
            byte[] buffer = new Byte[checked((uint)Math.Min(4096,(int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // Write out the trailing boundary  
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            return path;
        }
    }
}
