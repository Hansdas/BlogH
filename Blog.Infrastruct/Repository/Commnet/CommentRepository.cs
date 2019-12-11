using Blog.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastruct
{
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        public Comment Map(dynamic d)
        {
            return new Comment(d.comment_guid,d.comment_content, d.comment_postuser, d.user_username,d.comment_postdate);
        }

        public IList<Comment> Map(IEnumerable<dynamic> dynamics)
        {
            IList<Comment> comments = new List<Comment>();
            foreach (var item in dynamics)
            {
                Comment comment = Map(item);
                comments.Add(comment);
            }
            return comments;
        }

        public Comment SelectById(string guid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("guid", guid);
            string sql = "SELECT comment_guid,comment_content,comment_postuser,user_username,comment_postdate " +
                "FROM T_Comment INNER JOIN T_User ON comment_postuser=user_account WHERE comment_guid =@guid";
            dynamic d = SelectSingle(sql, parameters);
            return Map(d);
        }

        public IList<Comment> SelectByIds(IList<string> guids)
        {
            IList<Comment> comments = new List<Comment>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Guids", guids);
            string sql = "SELECT comment_guid,comment_content,comment_postuser,user_username,comment_postdate " +
                "FROM T_Comment INNER JOIN T_User ON comment_postuser=user_account WHERE comment_guid in @Guids ORDER BY comment_postdate DESC";
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
