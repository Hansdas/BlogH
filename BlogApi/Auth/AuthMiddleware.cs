﻿using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi
{
    public class AuthMiddleware
    {
        private RequestDelegate _next;
        private IHttpContextAccessor _httpContext;
        private ICacheClient _cacheClient;
        private IList<string> _requestPaths;
        public AuthMiddleware(IHttpContextAccessor httpContext, RequestDelegate next, ICacheClient cacheClient,IList<string> requestPaths)
        {
            _httpContext = httpContext;
            _next = next;
            _cacheClient = cacheClient;
            _requestPaths = requestPaths;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                bool isExpires = context.Request.Headers.TryGetValue("isExpires", out StringValues expires);
                bool IsAuthorized = context.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
                string token = authStr.ToString().Substring("Bearer ".Length).Trim();
                //每次访问都更新token有效期
                new JWT(_cacheClient).RefreshToken(token, isExpires);
                context.Response.Headers.Add("refreshTokne", token);
            }
            catch (ValidationException)
            {
                if (_requestPaths.Contains(context.Request.Path.Value))
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("not login");
                }
            }
              await _next(context);
        }
    }
    public class JwtAuthOption
    {
        public IList<string> _requstPaths;
        public void RequestPaths(IList<string> requestPaths)
        {
            _requstPaths = requestPaths;
        }

    }
}
