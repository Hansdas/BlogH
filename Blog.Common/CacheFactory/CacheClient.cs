using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.CacheFactory
{
    public class CacheClient : ICacheClient
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        private TimeSpan ExpireTime = new TimeSpan(24, 0, 0);
        private IServer server => CacheProvider.server;
        #region String操作
        public void StringSet(string key, string value, TimeSpan expiry)
        {
            CacheProvider.database.StringSet(key, value, expiry);
        }
        public void StringSet<T>(string key, T t, TimeSpan? expiry = null)
        {
            string json = JsonHelper.Serialize(t);
            StringSet(key, json, expiry ?? ExpireTime);
        }
        public string StringGet(string key)
        {
            return CacheProvider.database.StringGet(key);
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
            CacheProvider.database.SetAdd(key, value);
        }
        public string[] GetMembers(string key)
        {
           string[] members= CacheProvider.database.SetMembers(key).ToStringArray();
           return members;
        }

        public bool SetRemove(string key,string value)
        {
            return CacheProvider.database.SetRemove(key,value);
        }

        #endregion
        public void Remove(string key)
        {
            CacheProvider.database.KeyDelete(key);
        }
        public void BatchRemove(string[] keys)
        {
            var tran = CacheProvider.database.CreateTransaction();
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
            var keys = server.Keys(CacheProvider.database.Database, keyPattern).ToArray();
            return keys.Select(s => s.ToString()).ToArray();
        }
        #region list操作
        public async  Task<long> AddListTop<T>(string key, T t)
        {
            if (t == null)
                throw new ServiceException("对象t为null");
            string value = JsonHelper.Serialize(t);
            return await AddListTop(key, value);
        }

        public async Task<long> AddListTop(string key, string value)
        {          
           return  await CacheProvider.database.ListLeftPushAsync(key, value);
        }
        public async Task<long> AddListTail<T>(string key, T t)
        {
            if (t == null)
                throw new ArgumentException("对象t为null");
            string value = JsonHelper.Serialize(t);
            return await AddListTail(key, value);
        }

        public async Task<long> AddListTail(string key, string value)
        {
            return await CacheProvider.database.ListRightPushAsync(key, value);
        }
        public async Task ListInsert<T>(string key, int index, T t)
        {
            if (t == null)
                throw new ArgumentException("对象t为null");
            string value = JsonHelper.Serialize(t);
             await  CacheProvider.database.ListSetByIndexAsync(key, index, value);
        }
        public async Task<long> ListLenght(string key)
        {
          return await CacheProvider.database.ListLengthAsync(key);
        }

        public async Task listPop(string key)
        {
            await CacheProvider.database.ListRightPopAsync(key);
        }

        public async Task<List<T>> ListRange<T>(string key, int startindex, int endIndex, JsonSerializerSettings settings=null)
        {
            var rediusValue = await CacheProvider.database.ListRangeAsync(key, startindex, endIndex);
            List<T> list = new List<T>();
            foreach (var item in rediusValue)
            {
                list.Add(JsonHelper.DeserializeObject<T>(item, settings));
            }
            return list;
        }

        #endregion
    }
}
