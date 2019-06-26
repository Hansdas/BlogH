using Blog;
using Blog.Application;
using Blog.Common.AppSetting;
using Blog.Domain;
using Blog.Domain.Core;
using Blog.Domain.Core.Bus;
using Blog.Infrastruct;
using Blog.Infrastruct.EventBus;
using Chloe;
using Chloe.MySql;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommonHelper
{
    public static class Configure
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigServices(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<IRequestHandler<CreateUserCommand, Unit>, UserCommandHandler>();

            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<IBlogService,BlogService>();
            services.AddScoped<IRequestHandler<CreateBlogCommand, Unit>, BlogCommandHandler>();
        }
        /// <summary>
        /// 基础框架
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigFrame(this IServiceCollection services)
        {
            //注册全局过滤器
            services.AddMvc(s => s.Filters.Add<GlobaExceptionFilterAttribute>());
            //注册中介工具 https://github.com/jbogard/MediatR/wiki
            services.AddMediatR(typeof(Startup));
            //注册发布订阅中介处理
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            //注册领域通知
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            //注册仓储接口
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        }
        /// <summary>
        /// 配置集合
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigSettings(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(new ServiceDescriptor(typeof(ConnectionProvider), new ConnectionProvider(configuration.GetConnectionString("MySqlConnection"))));
            services.Configure<ApiSettingModel>(configuration.GetSection("webapi"));
            services.AddSession(s => {
                s.IdleTimeout = TimeSpan.FromDays(30);
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(s => {
                s.LoginPath = "/Login/Login";
                s.ExpireTimeSpan = TimeSpan.FromDays(30);
                s.SlidingExpiration = false;
            });
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
