using Blog.Common.AppSetting;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.CacheFactory
{
  public  class CacheProvider
    {
        private static ConcurrentDictionary<string, ConnectionMultiplexer> connectionDic;
        public static IDatabase database =null;
        public static IServer server = null;
        private int _defaultDB;
        public CacheProvider()
        {
            connectionDic = new ConcurrentDictionary<string, ConnectionMultiplexer>();
            database =GetConnection().GetDatabase(_defaultDB);
        }
        private ConnectionMultiplexer GetConnection()
        {
            RedisSettingModel model = ConfigurationProvider.GetSettingModel<RedisSettingModel>("Redis");
             string connStr = string.Format("{0}:{1}", model.Host, model.Port);
            _defaultDB = model.DefaultDB;
            ConfigurationOptions options = new ConfigurationOptions() {
                EndPoints = { { connStr } },
                DefaultDatabase = _defaultDB,
                ServiceName= connStr,
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
