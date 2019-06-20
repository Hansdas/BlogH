using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
   public static class JsonHelper
    {
        /// <summary>
        /// 将实体转为json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {           
            string json = JsonConvert.SerializeObject(t);
            return json;
        }
        /// <summary>
        /// 将json转为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string json)
        {
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
    }
}
