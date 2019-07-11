using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Common.CacheFactory
{
    public class CacheClient : ICacheClient
    {
        private IDatabase database => CacheProvider.database;
        private IServer server => CacheProvider.server;
        public void Set(string key, string value)
        {
            database.StringSet(key, value);
        }
        public void Set<T>(string key, T t)
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
        public void BatchRemove(string[] keys)
        {
            var tran = database.CreateTransaction();
            foreach (var item in keys)
                tran.KeyDeleteAsync(item);
            tran.Execute();
        }
        public void BatchRemovePattern(string keyPattern)
        {
            //string script = "loacl list=redis.call('keys',@keyPattern) for i,v in list do redis.call('delete',v) end";
            //LuaScript luaScript = LuaScript.Prepare(script);
            //database.ScriptEvaluate(luaScript, new { @keyPattern = keyPattern });
            string[] keys = LuaScriptGetKeys(keyPattern);
            var tran = database.CreateTransaction();
            foreach (var item in keys)
                tran.KeyDeleteAsync(item);
            tran.Execute();
        }

        public string[] GetKeys(string keyPattern)
        {
            var keys = server.Keys(database.Database, keyPattern).ToArray();
            return keys.Select(s => s.ToString()).ToArray();
        }
        public string[] LuaScriptGetKeys(string keyPattern)
        {
            //redis命令行执行  eval "return redis.call('keys','UserRepository*')" 0
            string script = "return redis.call('keys',@keyPattern)";
            LuaScript luaScript = LuaScript.Prepare(script);
            RedisResult redisResult = database.ScriptEvaluate(luaScript, new { @keyPattern = keyPattern });
            string[] keys = (string[])redisResult;
            return keys;
        }
    }
}
