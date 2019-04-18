using System;
using System.Collections.Generic;
using System.Text;

namespace CacheFactory.Provider
{
    public class CacheTypeFactory
    {

        public static ICacheProvider GetCacheProvider(CacheType cacheType)
        {
            ICacheProvider cacheProvider = new RedisCacheProvider();
            switch (cacheType)
            {
            //    case CacheType.LOCALMEMORYCACHE:
            //        cacheProvider = new RedisCacheProvider();
            //        break;
                case CacheType.Redis:
                    cacheProvider = new RedisCacheProvider();
                    break;
            }
            return cacheProvider;
        }
    }
}
