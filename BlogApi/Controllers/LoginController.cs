﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blog;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Common.EnumExtensions;
using Blog.Domain.Core;
using Blog.Domain.Core.Event;
using Blog.Domain.Core.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : Controller
    {
        protected IUserService _userService;
        protected ICacheClient _cacheClient;
        private NotifyValidationHandler _notifyValidationHandler;
        public LoginController(IUserService userService, ICacheClient cacheClient, IEventHandler<NotifyValidation> notifyValidationHandler)
        {
            _userService = userService;
            _notifyValidationHandler = (NotifyValidationHandler)notifyValidationHandler;
            _cacheClient = cacheClient;
        }
        [HttpPost]
        public ApiResult Login()
        {
            string account = Request.Form["Account"];
            string passWord = Request.Form["Password"];
            Blog.Domain.User user = new Blog.Domain.User();
            try
            {
                user = _userService.SelectUser(account, passWord);
                IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", user.Account),
                    new Claim("username", user.Username),
                    new Claim("sex", user.Sex.GetEnumText<Sex>()),
                    new Claim("birthDate", user.BirthDate.HasValue?user.BirthDate.Value.ToString("yyyy-MM-dd"):""),
                    new Claim("email", string.IsNullOrEmpty(user.Email)?"":user.Email),
                    new Claim("sign", string.IsNullOrEmpty(user.Sign)?"":user.Sign),
                    new Claim("phone",string.IsNullOrEmpty(user.Phone)?"":user.Phone),
                    new Claim("headPhoto", string.IsNullOrEmpty(user.HeadPhoto)?"":user.HeadPhoto)
                };
                string jwtToken = new JWT(_cacheClient).CreateToken(claims);
                return ApiResult.Success(jwtToken);
            }
            catch (ValidationException e)
            {
                return ApiResult.AuthError(e.Message);
            }
           
        }
        [HttpPost]
        [Route("logon")]
        public ApiResult Logon([FromBody]UserModel userModel)
        {
            string message = string.Empty;
            try
            {
                userModel.LoginType = LoginType.SITE;
                _userService.Insert(userModel);
                string domainNotification = _notifyValidationHandler.GetErrorList().Select(s => s.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(domainNotification))
                    message = domainNotification;
            }
            catch (ServiceException e)
            {
                message = e.Message;
            }
            IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", userModel.Account),
                    new Claim("username", userModel.Username),
                };
            string jwtToken = new JWT(_cacheClient).CreateToken(claims);
            if (!string.IsNullOrEmpty(message))
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, message);
            return ApiResult.Success(jwtToken);
        }
        [HttpPost]
        [Route("out")]
        public void LoginOut()
        {
            string token = Request.Form["token"].ToString();
            _cacheClient.Remove(token);
        }

    }
}