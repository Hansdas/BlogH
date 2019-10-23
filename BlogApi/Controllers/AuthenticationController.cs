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
    [Route("api/Auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IHttpContextAccessor _context;
        public AuthenticationController(IHttpContextAccessor httpContext)
        {
            _context = httpContext;
        }
        [HttpPost]
        [EnableCors("AllowSpecificOrigins")]
        public JsonResult IsLogin()
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                string json= new JWT(_context).ResolveToken();
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
    }
}