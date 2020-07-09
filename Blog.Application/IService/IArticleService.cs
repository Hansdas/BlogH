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
        IList<ArticleModel> SelectByPage(ArticleConditionModel articleConditionModel);
        int SelectCount(ArticleConditionModel articleConditionModel=null);
        ArticleModel Select(ArticleCondition articleCondition = null);
        /// <summary>
        /// 查询上一篇下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        PageInfoMode SelectContext(int id, ArticleCondition articleCondition = null);
        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="commentModel"></param>
        /// <param name="id"></param>
        void Review(CommentModel commentModel, int id);
        /// <summary>
        /// 点赞或取消
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="account"></param>
        /// <param name="cancle"></param>
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
        /// <summary>
        /// 根据文章id查询该作者的所有文章
        /// </summary>
        /// <returns></returns>
        IList<ArticleModel> SelectAllByArticle(int articleId);
        /// <summary>
        /// 查询文章归档
        /// </summary>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        IList<ArticleFileModel> SelectArticleFile(ArticleCondition articleCondition);

        /// <summary>
        /// 阅读排行前几
        /// </summary>
        /// <returns></returns>
        IList<ArticleModel> SelectByRead(int top);

    }
}
