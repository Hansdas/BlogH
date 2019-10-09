using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application.ViewModel;
using Blog.Common;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Controllers
{
    [Route("blogh/Auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IHttpContextAccessor _context;
        public AuthenticationController(IHttpContextAccessor httpContext)
        {
            _context = httpContext;
        }
        [HttpGet]
        [EnableCors("AllowSpecificOrigins")]
        public JsonResult Authenticate()
        {
            ReturnResult returnResult = new ReturnResult();
            bool IsAuthorized = _context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
            if (!IsAuthorized)
            {
                returnResult.Code = "500";
                returnResult.Message = "checkfail";
            }
            else
            {
                string token = authStr.ToString().Substring("Bearer ".Length).Trim();
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var json = decoder.Decode(token);
                UserModel userModel= JsonHelper.DeserializeObject<UserModel>(json);
                returnResult.Code = "200";
                returnResult.Data = userModel;
            }
            return new JsonResult(returnResult);
        }
    }
}