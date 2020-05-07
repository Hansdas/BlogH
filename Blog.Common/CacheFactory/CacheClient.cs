using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Common.CacheFactory
{
    public class CacheClient : ICacheClient
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        private TimeSpan ExpireTime = new TimeSpan(24, 0, 0);
        private IDatabase database => CacheProvider.database;
        private IServer server => CacheProvider.server;
        #region String操作
        public void StringSet(string key, string value, TimeSpan expiry)
        {
            database.StringSet(key, value, expiry);
        }
        public void StringSet<T>(string key, T t, TimeSpan? expiry = null)
        {
            string json = JsonHelper.Serialize(t);
            StringSet(key, json, expiry ?? ExpireTime);
        }
        public string StringGet(string key)
        {
            return database.StringGet(key);
        }
        public T StringGet<T>(string key)
        {
            string value = StringGet(key);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return JsonHelper.DeserializeObject<T>(value);
        }
        #endregion

        #region 集合（Set）操作
        public void AddSet(string key, string value)
        {
            database.SetAdd(key, value);
        }
        public string[] GetMembers(string key)
        {
           string[] members= database.SetMembers(key).ToStringArray();
           return members;
        }

        public bool SetRemove(string key,string value)
        {
            return database.SetRemove(key,value);
        }

        #endregion
        public void Remove(string key)
        {
            database.KeyDelete(key);
        }
        public void BatchRemove(string[] keys)
        {
            var tran = database.CreateTransaction();
            foreach (var item in keys)
                tran.KeyDeleteAsync(item);
            tran.Execute();
        }
        public void BatchRemovePattern(string keyPattern)
        {
            string[] keys = GetKeys(keyPattern);
            BatchRemove(keys);
        }

        public string[] GetKeys(string keyPattern)
        {
            var keys = server.Keys(database.Database, keyPattern).ToArray();
            return keys.Select(s => s.ToString()).ToArray();
        }

    }
}
