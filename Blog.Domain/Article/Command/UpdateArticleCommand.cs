using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public class UpdateArticleCommand : Command
    {
        public UpdateArticleCommand(Article article)
        {
            Article = article;
        }
        public UpdateArticleCommand(IList<Comment> comments,int id)
        {
            Comments = comments;
            Id = id;
        }
        public IList<Comment> Comments;
        public int Id { get;private set; }
        public Article Article { get; private set; }

    }
}
