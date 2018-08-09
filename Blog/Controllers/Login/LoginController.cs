using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonHelper;
using Domain.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ResultStatu;
using ServiceSvc.IService;

namespace Blog.Controllers
{
    public class LoginController : Controller
    {
        protected IUserServiceSvc _userServiceSvc;
        public LoginController(IUserServiceSvc userServiceSvc)
        {
            _userServiceSvc = userServiceSvc;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public  IActionResult Login(string username,string password)
        {
            string message = string.Empty;
            try
            {
                Domain.User user = _userServiceSvc.GetSingleUser(username, password, out message);
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            if (!string.IsNullOrEmpty(message))
                return Json(new ReturnResult() { IsSuccess = false, Message = message });
            return Json(new ReturnResult() { IsSuccess = true, Message = "登录成功" });
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            string message = string.Empty;
            Domain.User user = _userServiceSvc.RegisterUser(username, password, out message);
            if (!string.IsNullOrEmpty(message))
                return Json(new ReturnResult() { IsSuccess = false, Message = message });
            return Json(new ReturnResult() { IsSuccess = true, Message = "注册成功" });
        }
    }
}