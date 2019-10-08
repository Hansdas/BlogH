using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
          var v= _context.HttpContext.User.FindFirst("account");
            UserModel userModel = Auth.GetLoginUser();
            ReturnResult returnResult = new ReturnResult();
            if (userModel == null)
            {
                returnResult.Code = "200";
                returnResult.Message = "checkfail";
            }
            else
            {
                returnResult.Code = "200";
                returnResult.Message = "ok";
                returnResult.Data = userModel;
            }
            return new JsonResult(returnResult);
        }
    }
}