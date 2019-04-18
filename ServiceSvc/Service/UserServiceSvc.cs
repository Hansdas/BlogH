using Dapper;
using Domain;
using IDapperFactory;
using ServiceSvc.IService;
using System;

namespace ServiceSvc.Service
{
    public class UserServiceSvc:IUserServiceSvc
    {
        protected IQuerySelect _querySelect;
        protected IQueryInsert _queryInsert;
        private static object _lock = new object();
        public UserServiceSvc(IQuerySelect querySelect, IQueryInsert queryInsert)
        {
            _querySelect = querySelect;
            _queryInsert = queryInsert;
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
            user = _querySelect.SelectSingle<User>(s=>s.Account==Account&&s.Password==Password);
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

        public User RegisterUser(string Account, string Password, out string message)
        {
           lock(_lock)
            {
                User user = new User
                {
                    Account = Account,
                    Password = Password,
                    CreateTime = DateTime.Now
                };
                try
                {
                    int count = _querySelect.SelectCount<User>(s=>s.Account==Account);
                    if (count > 0)
                    {
                        message = string.Format("'{0}'账号已存在", user.Account);
                        return user;
                    }
                    else
                    {
                        user = _queryInsert.InsertSingle<User>(user);
                        if (user!=null)
                        {
                            message = "";
                            return user;
                        }
                        else
                        {
                            message = string.Format("'{0}'账号注册失败", user.Account);
                            return user;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    message = e.Message;
                    return user;
                }

            }
        }
    }
}
