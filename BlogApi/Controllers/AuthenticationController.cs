using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("blogh/Auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet]
        public JsonResult Authenticate()
        {
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