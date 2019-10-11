using Blog.Common;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApi
{
    public class JWT
    {
        private readonly IHttpContextAccessor _context;
        public JWT(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }
        /// <summary>
        /// 生成jwtToken
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string CreateToken(IList<Claim> claims)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("abcdefg1234567890"));
            var expires = DateTime.Now.AddDays(7);
            var token = new JwtSecurityToken(
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: expires,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));
            //生成Token
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
        /// <summary>
        /// 解析jwtToken到json
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public  string ResolveToken()
        {
            bool IsAuthorized = _context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
            if (!IsAuthorized)
                throw new ValidationException("not login");
            string token = authStr.ToString().Substring("Bearer ".Length).Trim();
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            var json = decoder.Decode(token);
            return json;
        }

    }
}
