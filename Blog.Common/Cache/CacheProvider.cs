using Blog.Common.AppSetting;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.Cache
{
    public class CacheProvider
    {
        private static ConcurrentDictionary<string, ConnectionMultiplexer> connectionDic=new ConcurrentDictionary<string, ConnectionMultiplexer>();
        public static IDatabase database = null;
        public static IServer server = null;
        private int _defaultDB;
        private CacheProvider()
        {

        }
        public CacheProvider(string section)
        {
            if(database==null)
                database = GetConnection(section).GetDatabase(_defaultDB);
        }
        private ConnectionMultiplexer GetConnection(string section)
        {
            RedisSettingModel model = ConfigurationProvider.GetSettingModel<RedisSettingModel>(section);
             string connStr = string.Format("{0}:{1}", model.Host, model.Port);
            _defaultDB = model.DefaultDB;
            ConfigurationOptions options = new ConfigurationOptions() {
                EndPoints = { { connStr } },
                DefaultDatabase = _defaultDB,
                Password=model.Password,
                ReconnectRetryPolicy = new ExponentialRetry(5000)
            };
            options.ClientName = model.InstanceName;
            return connectionDic.GetOrAdd(connStr, s => ConnectionMultiplexer.Connect(options));
        }
        private IServer GetServer()
        {
            if (server == null)
            {
                RedisSettingModel model = ConfigurationProvider.GetSettingModel<RedisSettingModel>("Redis");
                string connStr = string.Format("{0}:{1}", model.Host, model.Port);
                server = connectionDic[connStr].GetServer(model.Host, model.Port);
            }
            return server;
        }
    }
}
