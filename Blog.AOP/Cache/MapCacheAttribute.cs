using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.AOP.Cache
{
   [AttributeUsage(AttributeTargets.Method)]
   public  class MapCacheAttribute:Attribute
    {
        public MapCacheAttribute(params string[] dbNames)
        {
            dbNameList = dbNames;
        }
        public int ExpirationTime { get; set; }
        public IList<string> dbNameList { get; set; }
    }
}
