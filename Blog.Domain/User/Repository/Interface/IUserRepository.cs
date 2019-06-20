
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
  public  interface IUserRepository:IRepository<User,int>
    {
        /// <summary>
        /// 根据账号查询
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        User SelectUserByAccount(string account);
        /// <summary>
        /// 根据账号查询数量
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        int SelectCountByAccount(string account);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        void Insert(User user);
    }
}
