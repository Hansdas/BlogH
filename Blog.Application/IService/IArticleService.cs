﻿using Blog.Application.ViewModel;
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
        PageInfoMode SelectContext(int id, ArticleCondition articleCondition = null);
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
        /// <summary>
        /// 根据文章id查询该作者的所有文章
        /// </summary>
        /// <returns></returns>
        IList<ArticleModel> SelectAllByArticle(int articleId);

    }
}
