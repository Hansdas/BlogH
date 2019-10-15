using Blog.Application.ViewModel;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Application
{
  public  interface IUserService
    {
        /// <summary>
        /// 查询单个用户
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        User SelectUser(string Account, string password);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        void Insert(User userModel);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userModel"></param>
        void Update(UserModel userModel);
    }
}
