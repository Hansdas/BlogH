
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
        public User()
        {

        }
        public User(string username, string account, string password)
        {
            Username = username;
            Account = account;
            Password = password;
        }
        public User(string username, string account, string password, Sex sex, bool isvalid):this(username,account,password)
        {
            Sex = sex;
            IsValid = isvalid;
        }
        public User(int id,string username, string account, string password, Sex sex, bool isvalid,string email
            ,string phone,DateTime? birthdayDate,string sign, DateTime createtime,DateTime? updatetime)
        :this(username, account, password,sex,isvalid)
        {
            Id = id;
            Username = username;
            Account = account;
            Password = password;
            Sex = sex;
            IsValid = isvalid;
            CreateTime = createtime;
            UpdateTime = updatetime;
            Email = email;
            Phone = phone;
            BirthdayDate = birthdayDate;
            Sign = sign;
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
        /// 邮箱
        /// </summary>
        public string Email { get; private set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get;private set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BirthdayDate { get; private set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Sign { get; private set; }
    }
}
