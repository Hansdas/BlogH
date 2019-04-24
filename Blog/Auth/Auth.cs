using CommonHelper;
using Domain;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
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
        public static User GetLoginUser()
        {
            string userName = "";
            if(Http.httpContext.User.Identity.IsAuthenticated)
            {
                userName = Http.httpContext.User.Claims.First().Value;
            }
            User user = Http.GetSession<User>(userName);
            return user;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        public  static void Login(User user)
        {
           Http.SetSession(user.Account, user);
        }
    }
}
