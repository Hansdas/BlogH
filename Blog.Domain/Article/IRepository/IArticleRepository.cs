using Blog.Domain.Core.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public interface IArticleRepository : IRepository<Article, int>
    {
        void Insert(Article article);
        int SelectCount(ArticleCondition condition = null);
        IEnumerable<Article> SelectByPage(int pageSize, int pageIndex, ArticleCondition condition=null);
        Article Select(ArticleCondition articleCondition = null);
    }
}
