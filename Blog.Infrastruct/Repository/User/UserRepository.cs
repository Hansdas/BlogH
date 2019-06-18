using Blog.Domain;
using Blog.Infrastruct;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Blog.Domain.Core;

namespace Blog.Infrastruct
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        private static User Map(dynamic result)
        {
            return new User(result.Id, result.Username, result.Account, result.Password, (Sex)result.Sex, result.IsValid == 0 ? false : true
                , result.CreateTime, result.UpdateTime
                );

        }
        /// <summary>
        /// 根据账号查询
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public User SelectUserByAccount(string account)
        {

            string sql = "SELECT * FROM User where Account=@Account";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("Account", account);
            dynamic result = base.SelectSingle(sql, dynamicParameters);
            return Map(result);
        }
        /// <summary>
        /// 根据账号查询数量
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int SelectCountByAccount(string account)
        {

            string sql = "SELECT COUNT(*) FROM User where Account=@Account";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("Account", account);
            int count=CreateConnection(s => {
               return s.ExecuteScalar<int>(sql,dynamicParameters);
            });
            return count;
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        public void Insert(User user)
        {
            string sql = "INSERT INTO User(Id, Username, Account, Password, Sex, IsValid, CreateTime)" +
                  " VALUES (@Id, @Username, @Account, @Password, @Sex, @IsValid,NOW())";
            Insert(sql, user);
        }
    }
}
