﻿using Blog.Common;
using Blog.Domain.Core;
using CommonHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;

namespace Blog
{
    public class Startup
    {
     
        public IConfiguration Configuration
        {
            get
            {
                return Common.ConfigurationProvider.configuration;
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.ConfigFrame();
            services.ConfigServices();
            services.ConfigSettings(Configuration);
            return services.GetAutofacServiceProvider();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            ConstantKey.WebRoot = env.ContentRootPath;
            app.UseStaticFiles();
            //自定义使用资源目录
            app.UseStaticFiles(new StaticFileOptions()
            {              
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath + Configuration.GetSection("webapi:DownSavePathBase").Value)),
                RequestPath = ConstantKey.STATIC_FILE
            }); 
            app.UseAuthentication();
            app.UseSession();
            app.UseStaticHttpContext();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
