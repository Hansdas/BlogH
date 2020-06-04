using System;
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
using System.Linq;
using Blog.Common.CacheFactory;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Blog.Domain.Core.Notifications;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Blog.Domain.Core.Event;
using Blog.Application.IService;
using Blog.Application.ViewMode;

namespace BlogApi.Controllers.User
{

    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserService _userService;
        private NotifyValidationHandler _notifyValidationHandler;
        private readonly ICacheClient _cacheClient;
        private IHttpContextAccessor _httpContext;
        private ITidingsService _tidingsService;
        public UserController(IUserService userService, IEventHandler<NotifyValidation> notifyValidationHandler
            , ICacheClient cacheClient, IHttpContextAccessor httpContext, ITidingsService tidingsService)
        {
            _userService = userService;
            _notifyValidationHandler = (NotifyValidationHandler)notifyValidationHandler;
            _cacheClient = cacheClient;
            _httpContext = httpContext;
            _tidingsService = tidingsService;
        }
        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult loginUser()
        {
            try
            {
                string json = new JWT(_httpContext).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                return new JsonResult(new ReturnResult("0", userModel));
            }
            catch(AuthException)
            {
                return new JsonResult(new ReturnResult("1", "not login"));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ReturnResult("1", ex.Message));
            }
        }
        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{account}")]
        public IActionResult SelectUser(string account)
        {
            try
            {
                UserModel userModel = _userService.SelectUser(account);
                return new JsonResult(new ReturnResult("0", userModel));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ReturnResult("1", ex.Message));
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateUser([FromBody]UserModel userModel)
        {
            try
            {
                _userService.Update(userModel);
                string domainNotification = _notifyValidationHandler.GetErrorList().Select(s => s.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(domainNotification))
                    return new JsonResult(new ReturnResult() { Code = "500", Data = domainNotification });
                IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", userModel.Account),
                    new Claim("username", userModel.Username),
                    new Claim("sex", userModel.Sex),
                    new Claim("birthDate", string.IsNullOrEmpty(userModel.BirthDate)?"":userModel.BirthDate),
                    new Claim("email", string.IsNullOrEmpty(userModel.Email)?"":userModel.Email),
                    new Claim("sign", string.IsNullOrEmpty(userModel.Sign)?"":userModel.Sign),
                    new Claim("phone",userModel.Phone),
                    new Claim("headPhoto",JsonHelper.DeserializeObject<UserModel>(new JWT(_httpContext).ResolveToken()).HeadPhoto)
                };
                string jwtToken = new JWT(_cacheClient).CreateToken(claims);
                if (Response.Headers.ContainsKey("refreshToken"))
                    Response.Headers.Remove("refreshToken");
                Response.Headers.Add("refreshToken", jwtToken);
                return new JsonResult(new ReturnResult() { Code = "0"});
            }
            catch (Exception ex)
            {
                return new JsonResult(new ReturnResult("1", ex.Message));
            }
        }
        /// <summary>
        /// 更改头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("update/photo")]
        public JsonResult UploadPhoto()
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            string oldPath = userModel.HeadPhoto;
            if (!string.IsNullOrEmpty(oldPath))
                UploadHelper.DeleteFile(oldPath);
            var file = Request.Form.Files[0];
            PathValue pathValue = UploadHelper.SaveFile(file.FileName);
            UploadHelper.CompressImage(pathValue.FilePath, file.OpenReadStream(), 168, 168, true);
            pathValue = UploadHelper.Upload(pathValue.FilePath, file.FileName).GetAwaiter().GetResult();
            userModel.HeadPhoto = pathValue.FilePath;
            _userService.Update(userModel);
            IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", userModel.Account),
                    new Claim("username", userModel.Username),
                    new Claim("sex", userModel.Sex),
                    new Claim("birthDate", string.IsNullOrEmpty(userModel.BirthDate)?"":userModel.BirthDate),
                    new Claim("email", string.IsNullOrEmpty(userModel.Email)?"":userModel.Email),
                    new Claim("sign", string.IsNullOrEmpty(userModel.Sign)?"":userModel.Sign),
                    new Claim("phone",userModel.Phone),
                    new Claim("headPhoto",userModel.HeadPhoto)
                };
            string jwtToken = new JWT(_cacheClient).CreateToken(claims);
            return new JsonResult(new ReturnResult() { Code = "200", Data = new { Path = pathValue.FilePath, token = jwtToken } });
        }
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update/password")]
        public IActionResult UpdatePassword()
        {
            string password = Request.Form["password"];
            string OldPawword = Request.Form["oldpassword"];
            string json = new JWT(_httpContext).ResolveToken();
            UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
            _userService.UpdatePassword(userModel.Account, password, OldPawword);
            string domainNotification = _notifyValidationHandler.GetErrorList().Select(s => s.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(domainNotification))
                return new JsonResult(new ReturnResult() { Code = "500", Message = domainNotification });
            return new JsonResult(new ReturnResult() { Code = "200" });
        }
        /// <summary>
        /// 获取头像
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("photo")]
        public string GetPhoto()
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            return userModel.HeadPhoto;
        }
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("tidings")]
        public JsonResult GetTidings(int pageIndex, int pageSize)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            TidingsCondition tidingsCondition = new TidingsCondition();
            tidingsCondition.Account = userModel.Account;
            try
            {
                IList<TidingsModel> tidingsModels = _tidingsService.SelectByPage(pageIndex, pageSize, tidingsCondition);
                return new JsonResult(new ReturnResult() { Code = "0", Data=tidingsModels });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ReturnResult() { Code = "1", Message=ex.Message });
            }

        }
    }
}