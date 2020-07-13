using Blog.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Blog.Domain.Core;

namespace Blog.Infrastruct
{
    public class CommentRepository : Repository, ICommentRepository
    {
        private IUserRepository _userRepository;
        public CommentRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        private Comment Map(dynamic d, Dictionary<string, string> accountAndName=null)
        {
            if(accountAndName == null)
            {
                List<string> accounts = new List<string>();
                accounts.Add(d.comment_postuser);
                accounts.Add(d.comment_revicer);
                accountAndName = _userRepository.SelectNameWithAccountDic(accounts.Distinct());
            }
            string postUsername = accountAndName[d.comment_postuser];
            string revicerUsername = accountAndName[d.comment_revicer];
            return new Comment(
                d.comment_guid,
                d.comment_content,
                (CommentType)d.comment_type,
                d.comment_postuser,
                postUsername, 
                d.comment_revicer,
                revicerUsername,
                d.comment_additional,
                d.usingcontent,
                d.comment_postdate);
        }

        public IList<Comment> Map(IEnumerable<dynamic> dynamics)
        {
            IList<Comment> comments = new List<Comment>();
            List<string> accounts = new List<string>();
            foreach (var item in dynamics)
            {
                accounts.Add(item.comment_postuser);
                accounts.Add(item.comment_revicer);
            }
           Dictionary<string,string> pairs =_userRepository.SelectNameWithAccountDic(accounts.Distinct());
            foreach (var item in dynamics)
            {
                Comment comment = Map(item, pairs);
                comments.Add(comment);
            }
            return comments;
        }

        public void Insert(Comment comment)
        {
            string insert = "INSERT INTO T_Comment(comment_guid,comment_content,comment_type,comment_postuser,comment_revicer,comment_additional,comment_postdate)" +
               " VALUES (@Guid,@Content,@CommentType,@PostUser,@RevicerUser,@AdditionalData,NOW())";
            DbConnection.Execute(insert, comment);
        }
        public Comment SelectById(string guid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("guid", guid);
            string sql = "SELECT a.*,b.comment_content as usingcontent FROM T_Comment a LEFT JOIN T_Comment b on a.comment_additional=b.comment_guid " +
                " WHERE a.comment_guid =@guid";
            dynamic d = SelectSingle(sql, parameters);
            return Map(d);
        }

        public IList<Comment> SelectByIds(IList<string> guids)
        {
            IList<Comment> comments = new List<Comment>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Guids", guids);
            string sql = "SELECT a.*,b.comment_content as usingcontent FROM T_Comment a LEFT JOIN T_Comment b on a.comment_additional=b.comment_guid " +
                " WHERE a.comment_guid in @Guids ORDER BY comment_postdate DESC";
            IEnumerable<dynamic> dynamics = Select(sql, parameters);
            foreach (var d in dynamics)
            {
                Comment comment = Map(d);
                comments.Add(comment);
            }
            return comments;
        }
    }
}
