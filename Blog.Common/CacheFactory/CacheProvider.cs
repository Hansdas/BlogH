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
        private int _defaultDB;
        public CacheProvider()
        {
            connectionDic = new ConcurrentDictionary<string, ConnectionMultiplexer>();
            database = GetConnection().GetDatabase(_defaultDB);
        }
        private ConnectionMultiplexer GetConnection()
        {
            RedisSettingModel model = ConfigurationProvider.GetSettingModel<RedisSettingModel>("Redis");
            string connstr = string.Format("{0}:{1}", model.Connection, model.Port);
            _defaultDB = model.DefaultDB;
            ConfigurationOptions options = new ConfigurationOptions() {
                EndPoints = { { connstr} },
                DefaultDatabase = _defaultDB,
                ServiceName=connstr,
                Password=model.Password,
                ReconnectRetryPolicy = new ExponentialRetry(5000)
            };
            options.ClientName = model.InstanceName;
            //return connectionDic.GetOrAdd(connstr, s => ConnectionMultiplexer.Connect(options));
            return connectionDic.GetOrAdd(connstr, s => ConnectionMultiplexer.Connect("58.87.92.221:6379,allowAdmin=true,password=123456"));
        }
    }
}
