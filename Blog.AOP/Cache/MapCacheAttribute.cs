using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.AOP.Cache
{
    /// <summary>
    /// AOP缓存，如果使用了该特性，相关的修改操作需使用RomveCacheAttribute删除缓存
    /// </summary>
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
