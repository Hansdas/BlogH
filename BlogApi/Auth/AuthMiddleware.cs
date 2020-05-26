using Blog.Application.ViewModel;
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
        private ICacheClient _cacheClient;
        private string[] _requestPaths = { "/api/auth/islogin", "/api/auth/getloginuser" };
        private IList<string> _whiteList;
        public AuthMiddleware(RequestDelegate next, ICacheClient cacheClient, IList<string> whiteList)
        {
            _next = next;
            _cacheClient = cacheClient;
            _whiteList = whiteList;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!_whiteList.Contains(context.Request.Path.Value))
                {
                    bool isExpires = context.Request.Headers.TryGetValue("isExpires", out StringValues expires);
                    bool IsAuthorized = context.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
                    string token = authStr.ToString().Substring("Bearer ".Length).Trim();
                    new JWT(_cacheClient).IfRefreshToken(token, isExpires);
                    context.Response.Headers.Add("refreshToken", token);
                    context.Response.Headers.Add("Access-Control-Expose-Headers", "refreshToken");
                }

            }
            catch (AuthException)
            {
                if (_requestPaths.Contains(context.Request.Path.Value.ToLower()))
                {
                    context.Response.Headers.Add("auth", "false");
                }
            }
            await _next(context);

        }
    }
    public class JwtAuthOption
    {
        /// <summary>
        /// 白名单
        /// </summary>
        public IList<string> _whiteList;
        public void SetWhiteList(IList<string> whiteList)
        {
            _whiteList = whiteList;
        }

    }
}
