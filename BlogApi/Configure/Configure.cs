using Autofac;
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
            services.AddTransient<IRequestHandler<CreateUserCommand, Unit>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateUserCommand, Unit>, UserCommandHandler>();

            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IRequestHandler<CreateArticleCommand, Unit>, ArticleCommandHandler>();

            //services.AddTransient<IUploadFileRepository, UploadFileRepository>();

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
            //注册中介工具 https://github.com/jbogard/MediatR/wiki
            services.AddMediatR(typeof(Startup));
            //注册发布订阅中介处理
            services.AddTransient<IMediatorHandler, InMemoryBus>();
            //注册领域通知
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
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
            //containerBuilder.Populate(services);
            var assembly = Assembly.Load("Blog.Infrastruct");
            containerBuilder.RegisterType<Interceptor>();
            containerBuilder.RegisterAssemblyTypes(assembly)
                         .Where(type => typeof(IInterceptorHandler).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                         .AsImplementedInterfaces()
                         .InstancePerLifetimeScope()
                         .EnableInterfaceInterceptors()
                         .InterceptedBy(typeof(Interceptor));
            //containerBuilder.Build();

            //return new AutofacServiceProvider(builder.Build());
        }

    }
}
