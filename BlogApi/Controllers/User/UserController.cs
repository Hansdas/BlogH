﻿using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Blog.Domain;
using Microsoft.AspNetCore.Http;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Domain.Core;
using Blog.Application;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using System.Linq;
using Blog.Common.CacheFactory;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;

namespace BlogApi.Controllers.User
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private  IUserService _userService;
        private DomainNotificationHandler _domainNotificationHandler;
        private readonly ICacheClient _cacheClient;
        private IHttpContextAccessor _httpContext;
        public UserController( IUserService  userService, INotificationHandler<DomainNotification> notifications
            , ICacheClient cacheClient,IHttpContextAccessor httpContext)
        {
            _userService = userService;
            _domainNotificationHandler=  (DomainNotificationHandler)notifications;
            _cacheClient = cacheClient;
            _httpContext = httpContext;
        }
        [HttpGet]
        public IActionResult UserInfo()
        {
            try
            {
                string json = new JWT(_httpContext).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                return new JsonResult(new ReturnResult("200", userModel));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ReturnResult("500", ex.Message));
            }
        }
        [HttpPost]
        public IActionResult UpdateUser()
        {
            try
            {
                UserModel userModel = new UserModel();
                userModel.Account = Request.Form["account"];
                userModel.Username = Request.Form["username"];
                userModel.Sex = Request.Form["sex"];
                userModel.BirthDate = Request.Form["birthdate"];
                userModel .Phone= Request.Form["phone"];
                userModel.Email = Request.Form["email"];
                userModel.Sign = Request.Form["sign"];
                _userService.Update(userModel);
                string domainNotification =  _domainNotificationHandler.GetDomainNotificationList().Select(s => s.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(domainNotification))
                    return new JsonResult(new ReturnResult() { Code = "500", Data = domainNotification});
                IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", userModel.Account),
                    new Claim("username", userModel.Username),
                    new Claim("sex", userModel.Sex),
                    new Claim("birthDate", string.IsNullOrEmpty(userModel.BirthDate)?"":userModel.BirthDate),
                    new Claim("email", string.IsNullOrEmpty(userModel.Email)?"":userModel.Email),
                    new Claim("sign", string.IsNullOrEmpty(userModel.Sign)?"":userModel.Sign),
                    new Claim("phone",userModel.Phone)
                };
                string jwtToken = new JWT(_cacheClient).CreateToken(claims);
                return new JsonResult(new ReturnResult() { Code = "200", Data = jwtToken });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ReturnResult("500", ex.Message));
            }
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [EnableCors("AllowSpecificOrigins")]
        public IActionResult UploadPhoto()
        {
            var file = Request.Form.Files[0];
            PathValue pathValue = UploadHelper.SaveFile(file.FileName);
            UploadHelper.CompressImage(pathValue.FilePath,file.OpenReadStream(),168,168,true);
            //使用虚拟静态资源路径，否则无法读取到图片
            string virtualPath = HttpHelper.GetRequestIP(_httpContext) + ConstantKey.STATIC_FILE + pathValue.DatePath + pathValue.FileName;
            return Json(new { Code = "200", Data = new { Path=virtualPath } });
        }
        [HttpPost]
        public IActionResult UpdatePassword()
        {
             string password= Request.Form["password"];
            string OldPawword = Request.Form["oldpassword"];
            string json = new JWT(_httpContext).ResolveToken();
            UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
            _userService.UpdatePassword(userModel.Account,password, OldPawword);
            string domainNotification = _domainNotificationHandler.GetDomainNotificationList().Select(s => s.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(domainNotification))
                return new JsonResult(new ReturnResult() { Code = "500", Message = domainNotification });
            return new JsonResult(new ReturnResult() { Code = "200"});
        }
    }
}