using System;
using System.Collections.Generic;
using System.Text;

namespace CacheFactory.Provider
{
    public class CacheTypeFactory
    {

        public static ICacheProvider GetCacheProvider(CacheType cacheType)
        {
            ICacheProvider cacheProvider = new MemoryCacheProvider();
            switch (cacheType)
            {
                case CacheType.LOCALMEMORYCACHE:
                    cacheProvider = new MemoryCacheProvider();
                    break;
            }
            return cacheProvider;
        }
    }
}
