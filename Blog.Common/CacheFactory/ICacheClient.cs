using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.CacheFactory
{
   public interface ICacheClient
    {
        void Set(string key, string value, TimeSpan expiry);
        void Set<T>(string key, T t, TimeSpan? expiry = null);
        T Get<T>(string key);
        string Get(string key);
        void Remove(string key);
        void BatchRemove(string[] keys);
        void BatchRemovePattern(string keyPattern);
        string[] GetKeys(string keyPattern);
    }
}
