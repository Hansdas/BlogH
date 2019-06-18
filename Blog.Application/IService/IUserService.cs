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
        User SelectUserByAccount(string Account);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        void Insert(User user);
    }
}
