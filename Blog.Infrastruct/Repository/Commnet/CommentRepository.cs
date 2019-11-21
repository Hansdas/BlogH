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
            return new Comment(d.comment_content, d.comment_guid,d.comment_account,d.comment_commentdate);
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
            string sql = "SELECT * FROM Comment WHERE comment_guid in @Guids";
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
