using Blog.Application.ViewModel;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application
{
   public interface IArticleService
    {
        void AddOrUpdate(ArticleModel model);
        IList<ArticleModel> SelectByPage(int pageIndex, int pageSize,ArticleCondition condition=null);
        ArticleModel Select(ArticleCondition articleCondition = null);
        PageInfoMode SelectNextUp(int id, ArticleCondition articleCondition = null);
        void Review(CommentModel commentModel, int id);
        void Praise(int articleId, string account, bool cancle);
        /// <summary>
        /// 查询热门推荐
        /// </summary>
        /// <returns></returns>
        IList<ArticleModel> SelectHotArticles();
        /// <summary>
        /// 查询每种文章发布最新的
        /// </summary>
        /// <returns></returns>
        IList<ArticleModel> SelectByTypeMaxTime();

    }
}
