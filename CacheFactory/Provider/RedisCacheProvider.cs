using CacheFactory.Provider;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheFactory
{
    public class RedisCacheProvider : ICacheProvider
    {
        private IDistributedCache _RedisCache { get; }
        public RedisCacheProvider(IDistributedCache RedisCache)
        {
            _RedisCache = RedisCache;
        }
        public void Set(string key, object value)
        {
            var stringJson = JsonConvert.SerializeObject(value);
            _RedisCache.Set(key, Encoding.UTF8.GetBytes(stringJson));
        }
     
        public void Set<T>(string key, T value)
        {
            var stringJson = JsonConvert.SerializeObject(value);
            _RedisCache.Set(key, Encoding.UTF8.GetBytes(stringJson));
        }     
    }
}
