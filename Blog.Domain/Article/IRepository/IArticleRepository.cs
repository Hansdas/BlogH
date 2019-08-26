using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public interface IArticleRepository : IRepository<Article, int>
    {
        void Insert(Article article);
    }
}
