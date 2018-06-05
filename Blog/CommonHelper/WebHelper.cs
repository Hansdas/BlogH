
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonHelper
{
   public static class WebHelper
    {
        public static string GetClientIP(HttpRequest Requset)
        {
            if (Requset == null)
                return null;
            string clientIp = string.Empty;
            var ip = Requset.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(ip))
                ip = Requset.HttpContext.Connection.RemoteIpAddress.ToString();
            return ip;
        }

    }
}
