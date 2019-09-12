using Blog.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
   public static class Http
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static HttpContext httpContext => _httpContextAccessor.HttpContext;
        internal static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 将字符串存入Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSession(string key,string value)
        {
            httpContext.Session.SetString(key, value);
        }
        /// <summary>
        /// 从Session得到字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSession(string key)
        {
          string value=httpContext.Session.GetString(key);
          return value;
        }
        /// <summary>
        /// 将实体存入Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void SetSession<T>(string key,T t)
        {
            string jsonValue = JsonHelper.Serialize<T>(t);
            SetSession(key,jsonValue);
        }
        /// <summary>
        /// 从Session得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSession<T>(string key)
        {   
            string jsonValue = GetSession(key);
            if (jsonValue == null)
                return default(T);
            T t = JsonHelper.DeserializeObject<T>(jsonValue);
            return t;
        }
        /// <summary>
        /// 清楚指定session
        /// </summary>
        public static void ClearSession(string key)
        {
            httpContext.Session.Remove(key);
        }
    }
}
