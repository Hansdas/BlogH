﻿using Autofac;
using Blog;
using Blog.AOP.Cache;
using Blog.Application;
using Blog.Common.AppSetting;
using Blog.Common.CacheFactory;
using Blog.Domain;
using Blog.Domain.Core;
using Blog.Domain.Core.Bus;
using Blog.Infrastruct;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Autofac.Extras.DynamicProxy;
using Blog.AOP.Transaction;
using Blog.AOP;
using Blog.Domain.Core.Event;
using Blog.Domain.Core.Notifications;

namespace BlogApi.Configure
{
    public static class Configure
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddEventBus<IEventHandler<CreateUserCommand>, CreateUserCommand>();
            services.AddEventBus<IEventHandler<UpdateUserCommand>, UpdateUserCommand>();

            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddEventBus<IEventHandler<CreateArticleCommand>, CreateArticleCommand>();


            services.AddTransient<ICommentRepository, CommentRepository>();

            services.AddTransient<ICacheClient, CacheClient>();

            services.AddTransient<ICacheInterceptor, CacheInterceptor>();
            services.AddTransient<ITransactionInterceptor, TransactionInterceptor>();
        }
        /// <summary>
        /// 基础配置
        /// </summary>
        /// <param name="services"></param>
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //注册全局过滤器
            services.AddMvc(s => s.Filters.Add<GlobaExceptionFilterAttribute>());
            //注册发布订阅中介处理
            services.AddTransient<IEventBus, EventBus>();
            //注册领域通知
            services.AddNoticfication<DomainNotificationHandler, DomainNotification>();
            //注册仓储接口
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            //注册Redis
            services.AddSingleton(new CacheProvider());

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<ApiSettingModel>(configuration.GetSection("webapi"));
            services.Configure<RedisSettingModel>(configuration.GetSection("Redis"));
            //注册AOP拦截器
            //return GetAutofacServiceProvider(services);
        }
        /// <summary>
        /// 3.0不支持返回IServiceProvider
        /// </summary>
        /// <param name="containerBuilder"></param>
        public static void GetAutofacServiceProvider(this ContainerBuilder containerBuilder)
        {
            var assembly = Assembly.Load("Blog.Infrastruct");
            containerBuilder.RegisterType<Interceptor>();
            containerBuilder.RegisterAssemblyTypes(assembly)
                         .Where(type => typeof(IInterceptorHandler).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                         .AsImplementedInterfaces()
                         .InstancePerLifetimeScope()
                         .EnableInterfaceInterceptors()
                         .InterceptedBy(typeof(Interceptor));
        }

    }
}
