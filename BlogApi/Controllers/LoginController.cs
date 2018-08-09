using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApi.Controllers.attr;
using CommonHelper;
using Domain.System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ResultStatu;
using ServiceSvc.IService;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("any")]
    public class LoginController : Controller
    {
        protected  IUserServiceSvc _UserServiceSvc;
        public LoginController(IUserServiceSvc UserServiceSvc)
        {
            _UserServiceSvc = UserServiceSvc;
        }
        [Login]
        [HttpGet]
        public string GetResult(string Account,string Password)
        {
            LoginResult loginResult = new LoginResult();
            string message = string.Empty ;
            User user = _UserServiceSvc.GetSingleUser(Account, Password, out message);
            if (!string.IsNullOrEmpty(message))
                return JsonHelper.Serialize(new LoginResult() { IsSuccess = false, Message = message });
            return JsonHelper.Serialize(new LoginResult() { IsSuccess = true, Message ="登录成功" });
        }
    }
}