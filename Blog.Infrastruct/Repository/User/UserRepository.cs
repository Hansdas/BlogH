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
    public class UserRepository : Repository, IUserRepository
    {
        private static User Map(dynamic result)
        {
            return new User(result.user_id, result.user_username, result.user_account, result.user_password, (Sex)result.user_sex
                , result.IsValid == 0 ? false : true, result.user_email, result.user_phone,result.user_birthdate
                , result.user_sign, result.user_headphoto, result.user_createtime, result.user_updatetime
                );

        }
        public User SelectUserByAccount(string account)
        {

            string sql = "SELECT * FROM T_User where user_account=@Account";
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

            string sql = "SELECT COUNT(*) FROM T_User where user_account=@Account";
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
            string sql = "INSERT INTO T_User(user_username, user_account, user_password, user_sex, user_phone, user_email,user_birthdate" +
                ",user_sign, user_isvalid,user_headphoto, user_createtime)" +
                  " VALUES (@Username, @Account, @Password, @Sex,@Phone,@Email,@Birthdate,@Sign, @IsValid,@HeadPhoto,NOW())";
            DbConnection.Execute(sql,user);
        }
        public string SelectPassword(string account)
        {
            string sql = "SELECT user_password FROM T_User where user_account=@Account";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("Account", account);
            string password = DbConnection.ExecuteScalar<string>(sql, dynamicParameters);
            return password;
        }
        public Dictionary<string,string> SelectNameWithAccountDic(IEnumerable<string> accounts)
        {
            Dictionary<string, string> accountAndName = new Dictionary<string, string>();
            string sql = "SELECT  user_username, user_account FROM T_User WHERE user_account  IN @Useraccount";
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
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Account", user.Account);
            parameters.Add("Username", user.Username);
            parameters.Add("Sex", user.Sex);
            parameters.Add("Phone", user.Phone);
            parameters.Add("Email", user.Email);
            parameters.Add("BirthDate", user.BirthDate);
            parameters.Add("Sign", user.Sign);
            parameters.Add("UpdateTime", DateTime.Now);
            parameters.Add("HeadPhoto", user.HeadPhoto);
            string sql= "UPDATE T_User SET user_username = @Username, user_sex = @Sex" +
                ", user_phone = @Phone, user_email = @Email, user_birthdate = @BirthDate, user_sign = @Sign" +
                ", user_updatetime = @UpdateTime, user_headphoto=@HeadPhoto WHERE user_account = @Account";
            DbConnection.Execute(sql, parameters);
        }

        public void UpdatePassword(string account, string password)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Password", password);
            parameters.Add("Account", account);
            string sql = "UPDATE T_User SET user_password = @Password WHERE user_account = @Account";
            DbConnection.Execute(sql, parameters);
        }
    }
}
