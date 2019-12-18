using Blog.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastruct
{
    public class TidingsRepository : Repository<Tidings, int>, ITidingsRepository
    {
        public void Insert(Tidings tidings)
        {
            string sql = "INSERT INTO T_Tidings(tidings_commentid,tidings_reviceuser,tidings_isread,tidings_url,tidings_additionaldata,tidings_senddate) " +
                 "VALUES (@CommentId,@ReviceUser,@IsRead,@Url,@AdditionalData,NOW())";
            DbConnection.Execute(sql,tidings);
        }

        public int SelectCountByAccount(string account)
        {
            string sql = "SELECT COUNT(*) FROM T_Tidings WHERE tidings_reviceuser=@reviceuser and tidings_isread=@isread";
            return SelectCount(sql, new { reviceuser = account, isread =0});
        }
    }
}
