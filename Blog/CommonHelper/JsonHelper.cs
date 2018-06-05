using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonHelper
{
   public static class JsonHelper
    {
        public static string Serialize<T>(T t)
        {
            
            string json = JsonConvert.SerializeObject(t);
            return json;
        }
    }
}
