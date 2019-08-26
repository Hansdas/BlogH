using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public class CreateArticleCommand : Command
    {
        public Article Article { get; private set; }
        public CreateArticleCommand(Article article)
        {
            Article = article;
        }
    }
}
