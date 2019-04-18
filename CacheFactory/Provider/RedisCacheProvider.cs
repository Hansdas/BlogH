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
        private IDistributedCache _cache;
        //public RedisCacheProvider()
        //{

        //}
        //public RedisCacheProvider(IDistributedCache cache)
        //{
        //    _cache = cache;
        //}
        public void Set(string key, object value)
        {
            var stringJson = JsonConvert.SerializeObject(value);
            _cache.Set(key, Encoding.UTF8.GetBytes(stringJson));
        }

        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Add<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, object value, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, object value, DateTime absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public void FlushAll()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }
        public void Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value, DateTime absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            throw new NotImplementedException();
        }
    }
}
