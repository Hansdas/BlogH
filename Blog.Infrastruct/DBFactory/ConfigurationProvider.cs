using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Blog.Infrastruct
{
  public  class ConfigurationProvider
    {
        public static IConfiguration configuration;
        static ConfigurationProvider()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", false, true);
            configuration = builder.Build();
        }
    }
}
