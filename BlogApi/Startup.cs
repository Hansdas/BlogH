using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Blog.Domain.Core;
using Blog.Common.AppSetting;
using BlogApi.Configure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using NLog;
using System.IO;
using NLog.Extensions.Logging;
using NLog.Web;

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
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(s=>s.EnableEndpointRouting=false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddServices();
            services.AddInfrastructure();
            services.AddSettings(Configuration);
            //services.GetAutofacServiceProvider();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("Configs/nlog.config");
            //if(env)
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseCors();
            ConstantKey.WebRoot = env.ContentRootPath;
            app.UseRouting();
            app.UseAuthentication();
            //自定义使用资源目录
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath + "/TempFile")),
                RequestPath = ConstantKey.STATIC_FILE
            });
            app.UseSession();
            app.UseStaticHttpContext();
            app.UseHttpsRedirection();
            app.UseMvc();
            //app.UseMvc();
        }
    }
}
