using System;
using System.Collections.Generic;
using System.Text;

namespace CacheFactory.Provider
{
   public enum CacheType
    {
        /// <summary>
        /// 本地缓存
        /// </summary>
        LOCALMEMORYCACHE = 0,

        /// <summary>
        /// MemcachedCache分布式缓存
        /// </summary>
        MEMCACHEDCACHE = 1,

        /// <summary>
        /// ServiceStack redis缓存
        /// </summary>
        Redis = 2,
    }
}
