using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.CacheFactory
{
    public class CacheClient : ICacheClient
    {
        private IDatabase database => CacheProvider.database;

        public void Set(string key, string value)
        {
            database.StringSet(key, value);
        }
        public void Set<T>(string key,T t)
        {
            string json = JsonHelper.Serialize(t);
            Set(key, json);
        }
        public string Get(string key)
        {
            return database.StringGet(key);
        }
        public T Get<T>(string key)
        {
            string value = Get(key);
            return JsonHelper.DeserializeObject<T>(value);
        }

        public void Remove(string key)
        {
            database.KeyDelete(key);
        }
    }
}
