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
        public IList<Comment> SelectByIds(IList<string> guids)
        {
            IList<Comment> comments = new List<Comment>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Guids", guids);
            string sql = "SELECT comment_guid,comment_content,comment_postuser,user_username,comment_postdate " +
                "FROM Comment INNER JOIN User ON comment_postuser=user_account WHERE comment_guid in @Guids";
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
