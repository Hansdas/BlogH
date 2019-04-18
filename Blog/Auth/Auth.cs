using CommonHelper;
using Domain;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Auth
{
    public static class Auth
    {
        /// <summary>
        /// 登录Id
        /// </summary>
        private const string SESSION_LOGIN_ID = "loginId";
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
    }
}
