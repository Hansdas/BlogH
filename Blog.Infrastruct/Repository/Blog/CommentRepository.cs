using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastruct
{
    public class CommentRepository : ICommentRepository
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
    }
}
