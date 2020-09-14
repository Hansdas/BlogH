using Autofac;
using BlogApi.Configure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blog.Common;
using ConfigProvider = Blog.Common.ConfigurationProvider;
using Blog.Quartz;

namespace BlogApi
{
    public class Startup
    {

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
                    IConfigurationSection section = ConfigProvider.configuration.GetSection("Policy");
                    string[] origins= section.GetSection("Origins").Value.Split(',');
                    string[] headers = section.GetSection("Headers").Value.Split(',');
                    build.WithOrigins(origins)
                    .WithHeaders(headers)
                    .AllowAnyMethod()
                    .AllowCredentials();
                    
                });

            });
            services.AddServices();
            services.AddInfrastructure();
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
                    },
                };
            });
            //new QuartzApplication().StartJob().GetAwaiter().GetResult();

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
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath , "TempFile")),
                RequestPath = ConstantKey.STATIC_FILE
            });
            bool enableSwagger = Convert.ToBoolean(ConfigProvider.configuration.GetSection("EnableSwagger").Value);
            if (enableSwagger)//本地开发使用
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogApi");
                });
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthMiddleware(s=> {
                s.SetWhiteList(new List<string>() {
                    "/api/login/login",
                    "/chatHub/negotiate",//SingalR通讯请求的地址
                    "/chatHub", //SingalR通讯请求的地址
                    "/api/login/logon",
                    "/api/qq/login"
                });
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SingalrClient>("/chatHub");
                endpoints.MapControllers();
            });
        }
    }
}
