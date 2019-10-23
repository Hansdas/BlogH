
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
        /// 获取登录人基本信息
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static UserModel GetLoginUser(IHttpContextAccessor httpContextAccessor)
        {
            string json = new JWT(httpContextAccessor).ResolveToken();
            UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
            return userModel;
        }
    }
}
