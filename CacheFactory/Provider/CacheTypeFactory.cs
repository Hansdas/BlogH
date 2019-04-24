using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheFactory.Provider
{
    public class CacheTypeFactory
    {

        public static ICacheProvider GetCacheProvider(CacheType cacheType, IDistributedCache distributedCache)
        {
            ICacheProvider cacheProvider = new RedisCacheProvider(distributedCache);
            switch (cacheType)
            {
            //    case CacheType.LOCALMEMORYCACHE:
            //        cacheProvider = new RedisCacheProvider();
            //        break;
                case CacheType.Redis:
                    cacheProvider = new RedisCacheProvider(distributedCache);
                    break;
            }
            return cacheProvider;
        }
    }
}
