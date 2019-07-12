using Blog.Domain;
using Blog.Infrastruct;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Blog.Domain.Core;
using Blog.AOP.Cache;

namespace Blog.Infrastruct
{
    public class UserRepository : Repository<User, int>, IUserRepository,ICache
    {
        private static User Map(dynamic result)
        {        
            return new User(result.user_id, result.user_username, result.user_account, result.user_password, (Sex)result.user_sex
                , result.IsValid == 0 ? false : true, result.user_email, result.user_phone, result.user_createtime, result.user_updatetime
                );

        }
        /// <summary>
        /// 根据账号查询
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [MapCache("User")]
        public User SelectUserByAccount(string account)
        {

            string sql = "SELECT * FROM User where user_account=@Account";
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

            string sql = "SELECT COUNT(*) FROM User where user_account=@Account";
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
            string sql = "INSERT INTO User(user_id, user_username, user_account, user_password, user_sex, user_phone, user_email, user_isvalid, user_createtime, user_updatetime)" +
                  " VALUES (@Id, @Username, @Account, @Password, @Sex,@Phone,@Email, @IsValid,NOW())";
            Insert(sql, user);
        }

        public Dictionary<string,string> SelectUserByAccounts(IList<string> accounts)
        {
            Dictionary<string, string> accountAndName = new Dictionary<string, string>();
            string sql = "SELECT  user_username, user_account FROM User WHERE user_account  IN @Useraccount";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Useraccount", accounts);
            IEnumerable<dynamic> dynamics = CreateConnection(s => {
                return Select(sql, parameters);
            });
            foreach(var d in dynamics)
            {
                accountAndName.Add(d.user_account, d.user_username);
            }
            return accountAndName;
        }
    }
}
