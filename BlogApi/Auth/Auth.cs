
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Application.ViewModel;
using Blog.Common;

namespace BlogApi
{
    public static class Auth
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        private const string SESSION_LOGIN_ACCOUNT = "Account";
        /// <summary>
        /// 从Session获取登录信息
        /// </summary>
        /// <returns></returns>
        public static UserModel GetLoginUser()
        {
            string userName = "";
           var v=  Http.httpContext.User.FindFirst("account");
            if (Http.httpContext.User.Identity.IsAuthenticated)
            {
                userName = Http.httpContext.User.Claims.First().Value;
            }
            UserModel userModel = Http.GetSession<UserModel>(userName);
            return userModel;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        public  static void Login(this HttpContext httpContext, User user)
        {
            Http.SetSession(user.Account, user);
        }
         /// <summary>
        /// 退出操作
        /// </summary>
        public static void LoginOut()
        {
             string userName = "";
            if(Http.httpContext.User.Identity.IsAuthenticated)
            {
                userName = Http.httpContext.User.Claims.First().Value;
            }
            Http.ClearSession(userName);
        }
    }
}
