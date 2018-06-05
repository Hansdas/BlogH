using BaseEntity;
using CacheFactory.Provider;
using CommonHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Domain.System
{
  public  class User: DomainBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string  Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }
        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string PhotoUrl { get; set; }
    }

}
