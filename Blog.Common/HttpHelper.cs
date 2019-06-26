using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
                HttpResponseMessage httpResponseMessage = await client.PostAsync(url, httpContent);
                httpResponseMessage.EnsureSuccessStatusCode();
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                return result;

            }
        }
    }
}
