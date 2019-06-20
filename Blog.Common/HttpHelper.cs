using System;
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
        public  static async Task<string>  PostHttpClient(string url, NameValueCollection RequestHeaders,
             MultipartFormDataContent multipartFormDataContent)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            using (HttpClient client = new HttpClient(handler))
            {
                client.MaxResponseContentBufferSize = 256000;
                client.DefaultRequestHeaders.Add(RequestHeaders.Keys[0],RequestHeaders[RequestHeaders.Keys[0]]);
                HttpResponseMessage httpResponseMessage = await client.PostAsync(url, multipartFormDataContent);
                httpResponseMessage.EnsureSuccessStatusCode();
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                return result;
            }
        }
    }
}
