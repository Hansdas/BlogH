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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _context;
        public AuthController(IHttpContextAccessor httpContext)
        {
            _context = httpContext;
        }
        [HttpPost]
        [EnableCors("AllowSpecificOrigins")]
        public JsonResult GetLoginUser()
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                bool noLogin = Request.Headers.TryGetValue("auth", out StringValues value);
                if (noLogin)
                    throw new ValidationException();
                string json = new JWT(_context).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                returnResult.Code = "200";
                returnResult.Data = userModel;
            }
            catch (ValidationException)
            {
                returnResult.Code = "500";
                returnResult.Message = "checkfail";
            }
            return new JsonResult(returnResult);
        }
        /// <summary>
        /// 是否登录，通过中间件处理
        /// </summary>
        [HttpPost]
        public string IsLogin()
        {
            return "200";
        }
    }
}