using Domain;
using Domain.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IServiceSvc
{
   public interface IUserServiceSvc
    {
        /// <summary>
        /// 查询单个用户
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        User GetSingleUser(string Account, string Password,out string message);
        /// <summary>
        /// 插入用户
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        void RegisterUser(User User);
    }
}
