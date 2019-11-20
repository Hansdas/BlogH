using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public class UpdateArticleCommand : Command
    {
        public Article Article { get; private set; }
        public UpdateArticleCommand(Article article)
        {
            Article = article;
        }
    }
}
