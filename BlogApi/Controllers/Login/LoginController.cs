using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Blog;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Domain.Core;
using Blog.Domain.Core.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [GlobaExceptionFilter]
    public class LoginController : Controller
    {
        protected IUserService _userService;
        protected ICacheClient _cacheClient;
        private readonly DomainNotificationHandler _domainNotificationHandler;
        public LoginController(IUserService userService, ICacheClient cacheClient, INoticficationHandler<DomainNotification> notifications)
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
                return new JsonResult(new ReturnResult() { Code = "500", Message = e.Message });
            }
            IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", user.Account),
                    new Claim("username", user.Username),
                    new Claim("sex", user.Sex.GetEnumText<Sex>()),
                    new Claim("birthDate", user.BirthDate.HasValue?user.BirthDate.Value.ToString("yyyy-MM-dd"):""),
                    new Claim("email", string.IsNullOrEmpty(user.Email)?"":user.Email),
                    new Claim("sign", string.IsNullOrEmpty(user.Sign)?"":user.Sign),
                    new Claim("phone",user.Phone),
                    new Claim("headPhoto", string.IsNullOrEmpty(user.HeadPhoto)?"":user.HeadPhoto)
                };
            string jwtToken = new JWT(_cacheClient).CreateToken(claims);
            return new JsonResult(new ReturnResult() { Code = "200", Data = jwtToken });
        }

        [HttpPost]
        public IActionResult Logon()
        {
            string message = string.Empty;
            string account = Request.Form["account"];
            string passWord = Request.Form["password"];
            string userName = Request.Form["username"];
            UserModel userModel = new UserModel();
            userModel.Username = userName;
            userModel.Account = account;
            userModel.Password = passWord;
            try
            {
                _userService.Insert(userModel);
                string domainNotification = _domainNotificationHandler.GetDomainNotificationList().Select(s => s.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(domainNotification))
                    message = domainNotification;
            }
            catch (ServiceException e)
            {
                message = e.Message;
            }

            if (!string.IsNullOrEmpty(message))
                return Json(new ReturnResult() { Code = "500", Message = message });
            return Json(new ReturnResult() { Code = "200", Message = "注册成功" });
        }
        public string LoginOut(string token)
        {
            _cacheClient.Remove(token);
            return "200";
        }

    }
}