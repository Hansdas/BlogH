using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Blog.Common
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
  public  class ConfigurationProvider
    {
        public static IConfiguration configuration;
        static ConfigurationProvider()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("Configs/appsettings.json", false, true);
            builder.AddJsonFile("Configs/keyvalue.json", false, true);
            configuration = builder.Build();
        }
        /// <summary>
        /// 获取appsetting的配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSettingModel<T>(string key) where T:class,new()
        {
            var SettingModel = new ServiceCollection()
            .AddOptions()
            .Configure<T>(configuration.GetSection(key))
            .BuildServiceProvider()
            .GetService<IOptions<T>>()
            .Value;
            return SettingModel;
        }
    }
}
