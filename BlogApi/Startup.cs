using Autofac;
using Blog.Domain.Core;
using BlogApi.Configure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace BlogApi
{
    public class Startup
    {

        public IConfiguration Configuration
        {
            get
            {
                return Blog.Common.ConfigurationProvider.configuration;
            }
        }
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.GetAutofacServiceProvider();
        }
        readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(s => {
            s.AddPolicy(AllowSpecificOrigins, build => {
                build.WithOrigins("http://127.0.0.1:8080", "https://127.0.0.1:5001");
            });
            });
            services.AddServices();
            services.AddInfrastructure(Configuration);
        //services.GetAutofacServiceProvider();

    }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("Configs/nlog.config");
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseCors(AllowSpecificOrigins);
            ConstantKey.WebRoot = env.ContentRootPath;
            app.UseAuthentication();
            //自定义使用资源目录
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath + "/TempFile")),
                RequestPath = ConstantKey.STATIC_FILE
            });
            app.UseSession();
            app.UseStaticHttpContext();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllers();
            });
        }
    }
}
