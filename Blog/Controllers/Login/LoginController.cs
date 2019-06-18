using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Application;
using Blog.Common;
using Blog.Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Blog.Controllers
{
    public class LoginController : Controller
    {
        protected IUserService _userService;
        private readonly DomainNotificationHandler _domainNotificationHandler;
        public LoginController(IUserService userService, INotificationHandler<DomainNotification> notifications)
        {
            _userService = userService;
            _domainNotificationHandler = (DomainNotificationHandler)notifications;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginIn()
        {
            string account = Request.Form["Account"];
            string passWord = Request.Form["Password"];
            try
            {
                Domain.User user = _userService.SelectUserByAccount(account);
                if (user == null)
                {
                    return new JsonResult(new ReturnResult() { Code = "200", Message = "用户名或密码错我" });
                }
                HttpContext.Login(user);
                IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", user.Account)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                Task.Run(() =>
                {
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTimeOffset.Now.AddDays(30)
                    });
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new ReturnResult() { Code = "200", Message = e.Message });
            }
            return new JsonResult(new ReturnResult() { Code = "200", Message = "OK" });
        }

        [HttpPost]
        public IActionResult Logon()
        {
            string message = string.Empty;
            string account = Request.Form["account"];
            string passWord = Request.Form["password"];
            string userName = Request.Form["username"];
            Domain.User user = new Domain.User(userName, account, passWord);
            try
            {
                _userService.Insert(user);
                IEnumerable<string> domainNotifications= _domainNotificationHandler.GetDomainNotificationList().Select(s=>s.Value);
                foreach(var value in domainNotifications)
                {
                    message = value;
                    break;
                }
            }
            catch (FrameworkException e)
            {
                message = e.Message;
            }

            if (!string.IsNullOrEmpty(message))
                return Json(new ReturnResult() { Code = "500", Message = message });
            return Json(new ReturnResult() { Code = "200", Message = "注册成功" });
        }
        public IActionResult LoginOut()
        {
            Auth.LoginOut();
            return RedirectToAction("login", "login");
        }
    }
}