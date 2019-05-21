using Dapper;
using DapperFactory;
using Domain;
using IServiceSvc;
using System;

namespace ServiceSvc
{
    public class UserServiceSvc : IUserServiceSvc
    {
        protected IQueryDapper _queryDapper;
        private static object _lock = new object();
        public UserServiceSvc(IQueryDapper queryDapper)
        {
            _queryDapper = queryDapper;
        }
        /// <summary>
        /// 查询单个用户
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public User GetSingleUser(string Account, string Password, out string message)
        {
            User user = new User();
            user = _queryDapper.SelectSingle<User>(s => s.Account == Account && s.Password == Password);
            if (user == null)
            {
                message = "账号不存在";
                return user;
            }
            else
            {
                if (user.IsValid)
                {
                    message = "账后已冻结";
                    return user;
                }
                if (user.Password != Password)
                {
                    message = "用户名或密码错误";
                    return user;
                }
            }
            message = "";
            return user;
        }

        public void RegisterUser(User user)
        {
            string message = null;
            int count = _queryDapper.SelectCount<User>(s => s.Account == user.Account);
            if (count > 0)
            {
                message = string.Format("'{0}'账号已存在", user.Account);
                throw new ValidationException(message);
            }
            user = _queryDapper.InsertSingle<User>(user);
        }
    }
}
