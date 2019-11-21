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
        private readonly IUploadFileRepository _uploadFileRepository;
        private readonly ICommentRepository _commentRepository;
        public WhisperRepository(IUploadFileRepository uploadFileRepository, ICommentRepository commentRepository)
        {
            _uploadFileRepository = uploadFileRepository;
            _commentRepository = commentRepository;
        }
        private Whisper Map(dynamic d)
        {
            return new Whisper(
                d.whisper_id
                ,d.whisper_account
                ,d.whisper_content
                ,d.whisper_commentguids
                ,d.whisper_praisecount
                ,d.whisper_praiseaccount
                ,d.whisper_uploadfileguids
                ,d.whisper_createtime);
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
            throw new NotImplementedException();
        }

        public IEnumerable<Whisper> SelectByPage(int pageIndex, int pageSize,WhisperCondiiton condiiton=null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(condiiton, ref parameters);
            string sql = "SELECT * FROM Whisper WHERE "+where;
            IEnumerable<dynamic> dynamics = Select(sql, parameters);
            IList<Whisper> whispers = new List<Whisper>();
            List<string> uploadFileIds = new List<string>();
            List<string> commentGuids = new List<string>();
            foreach (var d in dynamics)
            {
                Whisper whisper = Map(d);
                whispers.Add(whisper);
                uploadFileIds.AddRange(Whisper.GetFileGuids(whisper));
                commentGuids.AddRange(Whisper.GetCommentGuidList(whisper));
            }
            IList<UploadFile> uploadFiles = _uploadFileRepository.SelectByIds(uploadFileIds);
            IList<Comment> comments = _commentRepository.SelectByIds(commentGuids);
            IList<Whisper> list = new List<Whisper>();
            foreach (var item in whispers)
            {
                Whisper whisper= new Whisper(
                item.Id
                , item.Account
                , item.Content
                , comments.Where(s=>item.CommentGuids.Contains(s.Guid)).ToList()
                , item.PraiseCount
                , item.PraiseAccount
                , uploadFiles.Where(s => item.CommentGuids.Contains(s.Guid)).ToList()
                ,item.CreateTime);
                list.Add(whisper);
            }
            return list;
        }

        public int SelectCount(WhisperCondiiton condiiton=null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(condiiton,ref parameters);
            string sql = "SELECT COUNT(*) FROM Whisper WHERE "+where;
            return DbConnection.ExecuteScalar<int>(sql,parameters);
        }
    }
}
