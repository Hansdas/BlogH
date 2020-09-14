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
        private IArticleService _articleService;
        public UserController(IUserService userService, IEventHandler<NotifyValidation> notifyValidationHandler
            , ICacheClient cacheClient, IHttpContextAccessor httpContext, ITidingsService tidingsService
            , IArticleService articleService)
        {
            _userService = userService;
            _notifyValidationHandler = (NotifyValidationHandler)notifyValidationHandler;
            _cacheClient = cacheClient;
            _httpContext = httpContext;
            _tidingsService = tidingsService;
            _articleService = articleService;
        }
        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult loginUser()
        {
            try
            {
                string json = new JWT(_httpContext).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                if (userModel.HeadPhoto.Contains(ConstantKey.NGINX_FILE_ROUTE_OLD))
                    userModel.HeadPhoto = userModel.HeadPhoto.Replace(ConstantKey.NGINX_FILE_ROUTE_OLD, ConstantKey.NGINX_FILE_ROUTE);
                return ApiResult.Success(userModel);
            }
            catch (AuthException)
            {
                return ApiResult.Error(HttpStatusCode.FORBIDDEN, "not login");
            }
        }
        /// <summary>
        /// 根据账号查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{account}")]
        public ApiResult SelectUser(string account)
        {
            UserModel userModel = _userService.SelectUser(account);
            if (userModel.HeadPhoto.Contains(ConstantKey.NGINX_FILE_ROUTE_OLD))
                userModel.HeadPhoto = userModel.HeadPhoto.Replace(ConstantKey.NGINX_FILE_ROUTE_OLD, ConstantKey.NGINX_FILE_ROUTE);
            return ApiResult.Success(userModel);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult UpdateUser([FromBody]UserModel userModel)
        {
            _userService.Update(userModel);
            string error = _notifyValidationHandler.GetErrorList().Select(s => s.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(error))
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, error);
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
            return ApiResult.Success();
        }
        /// <summary>
        /// 更改头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("update/photo")]
        public ApiResult UploadPhoto()
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
            return ApiResult.Success(new { Path = pathValue.FilePath, token = jwtToken });
        }
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update/password")]
        public ApiResult UpdatePassword()
        {
            string password = Request.Form["password"];
            string OldPawword = Request.Form["oldpassword"];
            string json = new JWT(_httpContext).ResolveToken();
            UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
            _userService.UpdatePassword(userModel.Account, password, OldPawword);
            string error = _notifyValidationHandler.GetErrorList().Select(s => s.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(error))
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, error);
            return ApiResult.Success();
        }
        /// <summary>
        /// 获取头像
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("photo")]
        public ApiResult GetPhoto()
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            return ApiResult.Success(userModel.HeadPhoto);
        }
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("tidings")]
        public ApiResult GetTidings(int pageIndex, int pageSize)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            TidingsCondition tidingsCondition = new TidingsCondition();
            tidingsCondition.Account = userModel.Account;
            tidingsCondition.IsRead = false;
                IList<TidingsModel> tidingsModels = _tidingsService.SelectByPage(pageIndex, pageSize, tidingsCondition);
            return ApiResult.Success(tidingsModels);

        }
        /// <summary>
        /// 获取个人归档
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("article/archive")]
        public ApiResult SelectArchive()
        {
            try
            {
                string json = new JWT(_httpContext).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                ArticleCondition articleCondition = new ArticleCondition();
                articleCondition.Account = userModel.Account;
                IList<ArticleFileModel> fileModels = _articleService.SelectArticleFile(articleCondition);
                return ApiResult.Success(fileModels);
            }
            catch (AuthException)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST,"not login");
            }
        }
    }
}