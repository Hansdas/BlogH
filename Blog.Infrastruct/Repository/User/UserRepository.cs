using Blog.Domain;
using Blog.Infrastruct;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Blog.Domain.Core;
using Blog.AOP.Cache;
using Blog.AOP;

namespace Blog.Infrastruct
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        private static User Map(dynamic result)
        {
            return new User(result.user_id, result.user_username, result.user_account, result.user_password, (Sex)result.user_sex
                , result.IsValid == 0 ? false : true, result.user_email, result.user_phone,result.user_birthdate
                , result.user_sign, result.user_createtime, result.user_updatetime
                );

        }
        public User SelectUserByAccount(string account)
        {

            string sql = "SELECT * FROM User where user_account=@Account";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("Account", account);
            dynamic result = new User();
            result = base.SelectSingle(sql, dynamicParameters);
            if (result == null)
                return null;
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
            int count= DbConnection.ExecuteScalar<int>(sql, dynamicParameters);
            return count;
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        public void Insert(User user)
        {
            string sql = "INSERT INTO User(user_id, user_username, user_account, user_password, user_sex, user_phone, user_email,user_birthdate,user_sign, user_isvalid, user_createtime, user_updatetime)" +
                  " VALUES (@Id, @Username, @Account, @Password, @Sex,@Phone,@Email,@Birthdate,@Sign, @IsValid,NOW())";
            DbConnection.Execute(sql,user);
        }

        public Dictionary<string,string> SelectUserByAccounts(IList<string> accounts)
        {
            Dictionary<string, string> accountAndName = new Dictionary<string, string>();
            string sql = "SELECT  user_username, user_account FROM User WHERE user_account  IN @Useraccount";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Useraccount", accounts);
            IEnumerable<dynamic> dynamics = DbConnection.Query(sql, parameters);
            foreach (var d in dynamics)
            {
                accountAndName.Add(d.user_account, d.user_username);
            }
            return accountAndName;
        }
        public void UpdateUser(User user)
        {
            string sql= "UPDATE User SET user_username = @Username, user_account = @Account, user_password = @Password, user_sex = @Sex" +
                ", user_phone = @Phone, user_email = @Email, user_birthdate = @BirthDate, user_sign = @Sign, user_isvalid = @IsValid" +
                ", user_updatetime = @UpdateTime WHERE user_account = @Account";
            DbConnection.Execute(sql, user);
        }
    }
}
