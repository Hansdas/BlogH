
using Blog.Domain.Core;
using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
   public class User:Entity<int>, IAggregateRoot
    {
        public User(string username, string account, string password)
        {
            Username = username;
            Account = account;
            Password = password;
        }
        public User(string username, string account, string password, Sex sex, bool isvalid)
        {
            Username = username;
            Account = account;
            Password = password;
            Sex = sex;
            IsValid = false;
        }
        public User(int id,string username, string account, string password, Sex sex, bool isvalid, DateTime createtime,DateTime? updatetime)
        {
            Id = id;
            Username = username;
            Account = account;
            Password = password;
            Sex = sex;
            IsValid = isvalid;
            CreateTime = createtime;
            UpdateTime = updatetime;
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
        /// <summary>
        /// 结果转换
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
    }
}
