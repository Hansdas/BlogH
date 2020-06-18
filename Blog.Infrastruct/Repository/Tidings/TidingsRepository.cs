using Blog.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastruct
{
    public class TidingsRepository : Repository, ITidingsRepository
    {
        private Tidings Map(dynamic d)
        {
            return new Tidings(
                d.tidings_postuser,
                d.tidings_postcontent,
                d.tidings_reviceuser,
                Convert.ToBoolean(d.tidings_isread),
                d.tidings_url,
                d.tidings_additionaldata,
                d.tidings_senddate
                );
        }
        private IList<Tidings> Map(IEnumerable<dynamic> dynamics)
        {
            IList<Tidings> tidingList = new List<Tidings>();
            foreach (var item in dynamics)
            {
                Tidings tidings = Map(item);
                tidingList.Add(tidings);
            }
            return tidingList;
        }
        public void Insert(Tidings tidings)
        {
            string sql = "INSERT INTO T_Tidings(tidings_commentid,tidings_postuser,tidings_postcontent,tidings_reviceuser,tidings_isread,tidings_url,tidings_additionaldata,tidings_senddate) " +
                 "VALUES (@CommentId,@PostUser,@PostContent,@ReviceUser,@IsRead,@Url,@AdditionalData,@SendDate)";
            DbConnection.Execute(sql, tidings);
        }
        private string Where(TidingsCondition condition, ref DynamicParameters parameters)
        {
            IList<string> sqlList = new List<string>();
            if (!string.IsNullOrEmpty(condition.Account))
            {
                parameters.Add("account", condition.Account);
                sqlList.Add("tidings_reviceuser = @account");
            }
            sqlList.Add(" 1=1 ");
            string sql = string.Join(" AND ", sqlList);
            return sql;
        }
        public int SelectCountByAccount(string account)
        {
            string sql = "SELECT COUNT(*) FROM T_Tidings WHERE tidings_reviceuser=@reviceuser and tidings_isread=@isread";
            return SelectCount(sql, new { reviceuser = account, isread = 0 });
        }
        public IList<Tidings> SelectByPage(int pageIndex, int pageSize, TidingsCondition TidingsCondition = null)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pageId", pageId);
            parameters.Add("pageSize", pageSize);
            string where = Where(TidingsCondition, ref parameters);
            string sql = "SELECT tidings_postuser,tidings_postcontent,tidings_reviceuser,tidings_senddate,tidings_isread,tidings_additionaldata,tidings_url "
                         + " FROM T_Tidings  WHERE " + where + " AND tidings_id<=("
                            + "SELECT tidings_id FROM T_Tidings WHERE " + where + " ORDER BY tidings_id DESC LIMIT @pageId,1) ORDER BY tidings_id DESC LIMIT @pageSize";
            IEnumerable<dynamic> dynamics = Select(sql, parameters);
            return Map(dynamics);
        }
    }
}
