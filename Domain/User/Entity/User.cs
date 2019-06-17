
using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
   public class User:Entity<int>
    {
        public User(string username, string account, string password, Sex sex, bool isvalid, DateTime createtime)
        {
            Username = username;
            Account = account;
            Password = password;
            Sex = sex;
            IsValid = isvalid;
            CreateTime = createtime;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; private set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; private set; }
        /// <summary>
        /// 是否失效
        /// </summary>
        public bool IsValid { get; private set; }
    }
}
