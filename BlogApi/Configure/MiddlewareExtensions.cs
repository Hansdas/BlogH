using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Configure
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder app,Action<JwtAuthOption> action)
        {
            JwtAuthOption jwtAuthOption = new JwtAuthOption();
            action(jwtAuthOption);
            return app.UseMiddleware<AuthMiddleware>(jwtAuthOption._whiteList);
        }
    }
}
