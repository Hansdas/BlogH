using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Text;
using Blog.Common;
using System.Linq;
using Blog.Domain.Core;
using Blog.AOP.Cache;
using Blog.AOP.Transaction;
using Blog.AOP;

namespace Blog.Infrastruct
{
    public class WhisperRepository : Repository<Whisper, int>, IWhisperRepository, IInterceptorHandler
    {
        private readonly ICommentRepository _commentRepository;
        public WhisperRepository(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        private string Where(WhisperCondiiton condition, ref DynamicParameters dynamicParameters)
        {
            IList<string> sqlList = new List<string>();
            if (condition != null)
            {
                if (!string.IsNullOrEmpty(condition.Account))
                {
                    dynamicParameters.Add("Account", condition.Account);
                    sqlList.Add("whisper_account = @Account");
                }
            }
            sqlList.Add(" 1=1 ");
            string sql = string.Join(" AND ", sqlList);
            return sql;
        }
        public void Insert(Whisper whisper)
        {
            string sql = "INSERT INTO Whisper(whisper_account,whisper_content,whisper_createtime) VALUES(@Account,@Content,NOW())";
            DbConnection.Execute(sql, whisper);
        }

        public IEnumerable<Whisper> SelectByPage(int pageIndex, int pageSize, WhisperCondiiton condiiton = null)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pageId", pageId, DbType.Int32);
            parameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condiiton, ref parameters);
            string sql = "SELECT W.*,U.user_username " +
                        "FROM Whisper W INNER JOIN User U ON user_account=whisper_account WHERE " + where +
                        " AND  whisper_id <=(" +
                        "SELECT whisper_id FROM Whisper WHERE "
                        + where +
                        "ORDER BY whisper_id DESC " +
                        "LIMIT @pageId, 1) " +
                        "ORDER BY whisper_id DESC " +
                        "LIMIT @pageSize";
            IEnumerable<dynamic> dynamics = Select(sql, parameters);
            IList<Whisper> whispers = new List<Whisper>();
            List<string> commentGuids = new List<string>();
            foreach (var d in dynamics)
            {
                Whisper whisper = new Whisper(
                d.whisper_id
                , d.user_username
                , d.whisper_content
                , (string)d.whisper_commentguids
                , d.whisper_praisecount==null ? 0 : d.whisper_praisecount
                , d.whisper_praiseaccount
                , d.whisper_createtime);
                whispers.Add(whisper);
                commentGuids.AddRange(Whisper.GetCommentGuidList(whisper));
            }
            IList<Comment> comments = _commentRepository.SelectByIds(commentGuids);
            IList<Whisper> list = new List<Whisper>();
            foreach (var item in whispers)
            {
                Whisper whisper = new Whisper(
                item.Id
                , item.Account
                , item.Content
                , comments.Where(s => item.CommentGuids.Contains(s.Guid)).ToList()
                , item.PraiseCount
                , item.PraiseAccount
                , item.CreateTime);
                list.Add(whisper);
            }
            return list;
        }

        public int SelectCount(WhisperCondiiton condiiton = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(condiiton, ref parameters);
            string sql = "SELECT COUNT(*) FROM Whisper WHERE " + where;
            return DbConnection.ExecuteScalar<int>(sql, parameters);
        }
    }
}
