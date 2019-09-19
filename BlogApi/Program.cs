using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using BlogApi;
using NLog.Web;

namespace Blog
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder
              .UseNLog()
              .UseStartup<Startup>()
              ;
          });
    }
}
