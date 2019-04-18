using CommonHelper;
using DapperFactory;
using DBHelper;
using IDapperFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceSvc.IService;
using ServiceSvc.Service;

namespace ConfigureServiceHelper
{
    public static class ConfigureHelper
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IQuerySelect, QuerySelect>();
            services.AddTransient<IQueryInsert, QueryInsert>();
            services.AddTransient<IUserServiceSvc, UserServiceSvc>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Add(new ServiceDescriptor(typeof(ConnectionProvider), new ConnectionProvider(configuration.GetConnectionString("MySqlConnection"))));
            services.AddDistributedRedisCache(s => {
                s.Configuration = configuration.GetConnectionString("RedisConnection"); //多个redis服务器：s.Configuration="地址1:端口,地址2:端口"
                s.InstanceName = "Blog";
            });
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
          var httpContext=app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
          HttpHelper.Configure()
        }
        /// <summary>
        /// 服务集合
        /// </summary>
        /// <param name="services"></param>
        public static void APIConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IQuerySelect, QuerySelect>();
            services.AddTransient<IUserServiceSvc, UserServiceSvc>();
            services.Add(new ServiceDescriptor(typeof(ConnectionProvider), new ConnectionProvider(configuration.GetConnectionString("MySqlConnection"))));
        }
    }
}
