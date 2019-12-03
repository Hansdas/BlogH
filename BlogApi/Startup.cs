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
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blog.Common;

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
            services.AddControllers();
            services.AddCors(s =>
            {
                s.AddPolicy("cores", build =>
                {
                    build.WithOrigins("http://127.0.0.1:8080", "http://127.0.0.1:5000").WithHeaders("Authorization", "content-type")
                    .AllowAnyMethod();
                });

            });
            services.AddServices();
            services.AddInfrastructure(Configuration);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";

            }).AddJwtBearer("JwtBearer",
            (jwtBearerOptions) =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(JWT.SecurityKey)),//秘钥
                    ValidateIssuer = true,
                    ValidIssuer = JWT.issuer,
                    ValidateAudience = true,
                    ValidAudience = JWT.audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("isExpires", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("Configs/nlog.config");
            app.UseCors("cores");
            ConstantKey.WebRoot = env.ContentRootPath;
            //自定义使用资源目录
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath + "/TempFile")),
                RequestPath = ConstantKey.STATIC_FILE
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthMiddleware();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
            });
        }
    }
}
