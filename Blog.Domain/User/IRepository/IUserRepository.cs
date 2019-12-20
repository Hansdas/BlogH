
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
  public  interface IUserRepository : IRepository<User,int>
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
        /// <summary>
        /// 查询用户账号和名称集合
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Dictionary<string, string> SelectNameWithAccountDic(IEnumerable<string> accounts);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user);
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public void UpdatePassword(string account, string password);
        /// <summary>
        /// 查询密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string SelectPassword(string account);

    }
}
