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
                if (condition.IsPassing.HasValue)
                {
                    dynamicParameters.Add("IsPassing", condition.IsPassing.Value,DbType.Boolean);
                    sqlList.Add("whisper_ispassing = @IsPassing");
                }
            }
            sqlList.Add(" 1=1 ");
            string sql = string.Join(" AND ", sqlList);
            return sql;
        }
        public int Insert(Whisper whisper)
        {
            string sql = "INSERT INTO T_Whisper(whisper_account,whisper_content,whisper_createtime) VALUES(@Account,@Content,NOW())" +
                ";SELECT LAST_INSERT_ID()";
             int id=Convert.ToInt32(DbConnection.ExecuteScalar(sql, whisper));
            return id;
        }

        public IEnumerable<Whisper> SelectByPage(int pageIndex, int pageSize, WhisperCondiiton condiiton = null)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pageId", pageId, DbType.Int32);
            parameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condiiton, ref parameters);
            string sql = "SELECT w.*,u.user_username " +
                        "FROM T_Whisper  w INNER JOIN T_User u on w.whisper_account=u.user_account  WHERE " + where +
                        " AND  whisper_id <=(" +
                        "SELECT whisper_id FROM T_Whisper WHERE "
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
                , d.whisper_account
                ,d.user_username
                , d.whisper_content
                , Convert.ToBoolean(d.whisper_ispassing)
                , (string)d.whisper_commentguids
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
                ,item.AccountName
                , item.Content
                , item.IsPassing
                , comments.Where(s => item.CommentGuids.Contains(s.Guid)).ToList()
                , item.CreateTime.Value);
                list.Add(whisper);
            }
            return list;
        }

        public int SelectCount(WhisperCondiiton condiiton = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(condiiton, ref parameters);
            string sql = "SELECT COUNT(*) FROM T_Whisper WHERE " + where;
            return DbConnection.ExecuteScalar<int>(sql, parameters);
        }
    }
}
