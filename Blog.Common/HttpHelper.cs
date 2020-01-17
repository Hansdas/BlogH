using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common
{
    public class HttpHelper
    {
        /// <summary>
        /// httpclient post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="RequestHeaders"></param>
        /// <param name="multipartFormDataContent"></param>
        /// <returns></returns>
        public static async Task<string> PostHttpClient(string url,
             HttpContent httpContent, NameValueCollection RequestHeaders = null)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            using (HttpClient client = new HttpClient(handler))
            {
                client.MaxResponseContentBufferSize = 256000;
                if (RequestHeaders == null)
                    RequestHeaders = new NameValueCollection();
                foreach (string key in RequestHeaders.AllKeys)
                {
                    client.DefaultRequestHeaders.Add(key, RequestHeaders[key]);
                }
                string result = "";
                try
                {
                    HttpResponseMessage httpResponseMessage = await client.PostAsync(url, httpContent);
                    httpResponseMessage.EnsureSuccessStatusCode();
                    result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    return result;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        /// <summary>
        /// 获取请求的ip信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetRequestIP(IHttpContextAccessor httpContext)
        {
            string ip = "";
            var connectionInfo = httpContext.HttpContext.Connection;
            ip = connectionInfo.LocalIpAddress.ToString();
            if (ip == "::1")
            {
                ip = "127.0.0.1";
                ip = string.Format("https://{0}:{1}", ip, connectionInfo.LocalPort);
            }
            else
                ip = string.Format("http://{0}:{1}", ip, connectionInfo.LocalPort);
            return ip;
        }
    }
}
