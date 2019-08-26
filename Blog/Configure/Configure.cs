using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blog;
using Blog.AOP.Cache;
using Blog.Application;
using Blog.Common.AppSetting;
using Blog.Common.CacheFactory;
using Blog.Domain;
using Blog.Domain.Core;
using Blog.Domain.Core.Bus;
using Blog.Infrastruct;
using Blog.Infrastruct.EventBus;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Autofac.Extras.DynamicProxy;
using Blog.Dapper;
using Blog.AOP.Transaction;
using Blog.AOP;
using Castle.DynamicProxy;

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

            services.AddTransient<IWhisperRepository, BlogRepository>();
            services.AddTransient<IArticleService,BlogService>();
            services.AddScoped<IRequestHandler<CreateBlogCommand, Unit>, BlogCommandHandler>();

            //services.AddTransient<IUploadFileRepository, UploadFileRepository>();

            services.AddTransient<ICommentRepository, CommentRepository>();

            services.AddTransient<ICacheClient, CacheClient>();

            services.AddTransient<ICacheInterceptor, CacheInterceptor>();
            services.AddTransient<ITransactionInterceptor, TransactionInterceptor>();
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
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            //注册Redis
            services.AddSingleton(new CacheProvider());
            //注册AOP拦截器
            //return GetAutofacServiceProvider(services);
        }
        /// <summary>
        /// 配置集合
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigSettings(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(new ServiceDescriptor(typeof(DapperProvider), new DapperProvider(configuration.GetConnectionString("MySqlConnection"))));
            services.Configure<ApiSettingModel>(configuration.GetSection("webapi"));
            services.Configure<RedisSettingModel>(configuration.GetSection("Redis"));
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
        /// <summary>
        /// 3.0不支持返回IServiceProvider
        /// </summary>
        /// <param name="containerBuilder"></param>
        public static void GetAutofacServiceProvider(this ContainerBuilder containerBuilder)
        {
            //containerBuilder.Populate(services);
            var assembly = Assembly.Load("Blog.Infrastruct");
            //var assembly = typeof(Blog.AOP.).GetType().GetTypeInfo().Assembly;
            containerBuilder.RegisterType<Interceptor>();
            containerBuilder.RegisterAssemblyTypes(assembly)
                         .Where(type =>typeof(IInterceptorHandler).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                         .AsImplementedInterfaces()
                         .InstancePerLifetimeScope()
                         .EnableInterfaceInterceptors()
                         .InterceptedBy(typeof(Interceptor));
            //containerBuilder.Build();

            //return new AutofacServiceProvider(builder.Build());
        }
    }
}
