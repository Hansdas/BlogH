using DapperFactory;
using DBHelper;
using IDapperFactory;
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

            services.Add(new ServiceDescriptor(typeof(ConnectionProvider), new ConnectionProvider(configuration.GetConnectionString("MySqlConnection"))));

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
