using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog;
using Blog.Application;
using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Controllers
{
    [Route("blogh/[controller]/[action]")]
    [ApiController]
    public class LoginController : Controller
    {
        protected IUserService _userService;
        protected ICacheClient _cacheClient;
        private readonly DomainNotificationHandler _domainNotificationHandler;
        public LoginController(IUserService userService, ICacheClient cacheClient, INotificationHandler<DomainNotification> notifications)
        {
            _userService = userService;
            _domainNotificationHandler = (DomainNotificationHandler)notifications;
            _cacheClient = cacheClient;
        }
        [HttpPost]
        public ActionResult Login()
        {
            string account = Request.Form["Account"];
            string passWord = Request.Form["Password"];
            Blog.Domain.User user = new Blog.Domain.User();
            try
            {
                user = _userService.SelectUser(account, passWord);
            }
            catch (ValidationException e)
            {
                return new JsonResult(new ReturnResult() { Code = "200", Message = e.Message });
            }
            IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", user.Account),
                    new Claim("username", user.Username)
                };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("abcdefg1234567890"));


            var expires = DateTime.Now.AddDays(7);//
            var token = new JwtSecurityToken(
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: expires,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));

            //生成Token
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new JsonResult(new ReturnResult() { Code = "200", Message = jwtToken});
        }

        [HttpPost]
        public IActionResult Logon()
        {
            string message = string.Empty;
            string account = Request.Form["account"];
            string passWord = Request.Form["password"];
            string userName = Request.Form["username"];
            Blog.Domain.User user = new Blog.Domain.User(userName, account, passWord);
            try
            {
                _userService.Insert(user);
                string domainNotification = _domainNotificationHandler.GetDomainNotificationList().Select(s => s.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(domainNotification))
                    message = domainNotification;
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