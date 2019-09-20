using Blog.Application.ViewModel;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application
{
   public interface IArticleService
    {
        void Publish(Article article);

        IList<ArticleModel> SelectByPage(int pageIndex, int pageSize,ArticleCondition condition=null);
        Task<IList<ArticleModel>> SelectByPageAsync(int pageIndex, int pageSize, ArticleCondition condition = null);

        ArticleModel Select(ArticleCondition articleCondition = null);
        PageInfoMode SelectNextUp(int id, ArticleCondition articleCondition = null);
    }
}
