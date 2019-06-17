using Blog;
using Blog.Application;
using Blog.Infrastruct;
using Chloe;
using Chloe.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonHelper
{
    public static class ConfigureHelper
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {            
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IDbContext>(s =>
            {
                return new MySqlContext(new ConnectionProvider(configuration.GetConnectionString("MySqlConnection")));
            });
            //services.AddSingleton(new ServiceDescriptor(typeof(ConnectionProvider), new ConnectionProvider(configuration.GetConnectionString("MySqlConnection"))));
            //services.AddDistributedRedisCache(s => {
            //    s.Configuration = configuration.GetConnectionString("RedisConnection"); //多个redis服务器：s.Configuration="地址1:端口,地址2:端口"
            //    s.InstanceName = "RedisDistributedCache";
            //});
        }
        /// <summary>
        /// 添加对HttpContext的扩展支持
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            Http.Configure(httpContextAccessor);
            return app;
        }
    }
}
