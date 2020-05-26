using Blog.Common;
using Blog.Common.CacheFactory;
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
        /// <summary>
        /// 秘钥
        /// </summary>
        public const string SecurityKey = "BlogH123456789012";
        public const string issuer = "BlogAPI";
        public const string audience = "BlogUI";
        private readonly IHttpContextAccessor _context;
        private readonly ICacheClient _cacheClient;
        public class TokenModel
        {
            public string token { get; set; }
            public DateTime ExpireTime { get; set; }
        }
        public JWT(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }
        public JWT(ICacheClient cacheClient)
        {
            _cacheClient = cacheClient;
        }
        /// <summary>
        /// 生成jwtToken
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public  string CreateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecurityKey));
            var expires = DateTime.Now.AddMinutes(30);
            var token = new JwtSecurityToken(
                        issuer: issuer,
                        audience: audience,
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: expires,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            TokenModel tokenModel = new TokenModel();
            tokenModel.ExpireTime = DateTime.Now.AddDays(7);
            tokenModel.token = jwtToken;
            _cacheClient.StringSet(jwtToken, tokenModel, TimeSpan.FromDays(7));
            return jwtToken;
        }
        public  void RemoveToken(string token)
        {
            _cacheClient.Remove(token);
        }
        /// <summary>
        /// 更新token
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="isExpires">是否失效</param>
        public  void IfRefreshToken(string token,bool isExpires=false)
        {
            if(token==null)
                throw new AuthException("expires");
            TokenModel tokenMode = _cacheClient.StringGet<TokenModel>(token);
            if (tokenMode == null)
                throw new AuthException("expires");
            JwtSecurityToken jwtSecurityToken= new JwtSecurityTokenHandler().ReadJwtToken(token);
            if(isExpires)
                token = CreateToken(jwtSecurityToken.Claims);
            tokenMode.ExpireTime = tokenMode.ExpireTime.AddDays(7);
            if(tokenMode.ExpireTime.Day-DateTime.Now.Day==1)//实现最后一天如果访问了就自动续期
                _cacheClient.StringSet(token, tokenMode, TimeSpan.FromDays(7));

        }
        /// <summary>
        /// 解析jwtToken到json
        /// </summary>
        /// <returns></returns>
        public string ResolveToken()
        {
            bool IsAuthorized = _context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
            if (!IsAuthorized)
                throw new AuthException("not login");
            string token = authStr.ToString().Substring("Bearer ".Length).Trim();
            if(token=="null")
                throw new AuthException("not login");
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            var json = decoder.Decode(token);
            return json;
        }
        /// <summary>
        /// 解析jwtToken到json
        /// </summary>
        /// <param name="AuthorizationHeader">requestHeader的token值</param>
        /// <returns></returns>
        public string ResolveToken(string token)
        {
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
