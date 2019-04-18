using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public  IActionResult LoginIn()
        {
            string message = string.Empty;
            try
            {
                string userName = Request.Form["Username"];
                string passWord = Request.Form["Password"];
                Domain.User user = _userServiceSvc.GetSingleUser(userName, passWord, out message);
                if (user == null)
                {
                    return new JsonResult(new ReturnResult() { Code = "200", Message = message });
                }
                IList<Claim> claims = new List<Claim>()
                {
                    new Claim("UserName", user.Account)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                Task.Run(() => {
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTimeOffset.Now.AddDays(30)
                    });
                });
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            if (!string.IsNullOrEmpty(message))
            {
                return new JsonResult(new ReturnResult() { Code = "500", Message = message });
            }
            return View("Home/Index");
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            string message = string.Empty;
            Domain.User user = _userServiceSvc.RegisterUser(username, password, out message);
            if (!string.IsNullOrEmpty(message))
                return Json(new ReturnResult() { Code = "500", Message = message });
            return Json(new ReturnResult() { Code = "200", Message = "注册成功" });
        }
    }
}