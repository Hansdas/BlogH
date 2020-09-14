using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [Route("api/qq")]
    [ApiController]
    public class QQLoginController : Controller
    {
        private IUserService _userService;
        private ICacheClient _cacheClient;
        public QQLoginController(IUserService userService,ICacheClient cacheClient)
        {
            _userService = userService;
            _cacheClient = cacheClient;
        }
        [Route("login/{code}")]
        [HttpGet]
        public async Task<ApiResult> Login(string code)
        {
            string accessToken =await QQClient.GetAccessToken(code);
            string openId = await QQClient.GetOpenId(accessToken);
            UserModel userModel = await QQClient.GetQQUser(accessToken, openId);
            _userService.Insert(userModel);
            IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", userModel.Account),
                    new Claim("username", userModel.Username),
                    new Claim("sex", userModel.Sex),
                    new Claim("birthDate",string.IsNullOrEmpty(userModel.BirthDate)?"":userModel.BirthDate),
                    new Claim("email", string.IsNullOrEmpty(userModel.Email)?"":userModel.Email),
                    new Claim("sign", string.IsNullOrEmpty(userModel.Sign)?"":userModel.Sign),
                    new Claim("phone",string.IsNullOrEmpty(userModel.Phone)?"":userModel.Phone),
                    new Claim("headPhoto", string.IsNullOrEmpty(userModel.HeadPhoto)?"":userModel.HeadPhoto)
                };
            string jwtToken = new JWT(_cacheClient).CreateToken(claims);
            return ApiResult.Success(jwtToken);
        }
    }
}
