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
        public UpdateArticleCommand(Comment comment,int id)
        {
            Comment = comment;
            Id = id;
        }
        public Comment Comment;
        public int Id { get;private set; }
        public Article Article { get; private set; }

    }
}
