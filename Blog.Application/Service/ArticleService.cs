using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Domain;
using Blog.Domain.Core;
using Blog.Domain.Core.Bus;

namespace Blog.Application
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository _articleRepository;
        private IEventBus _eventBus;
        public ArticleService(IEventBus eventBus, IArticleRepository articleRepository)
        {
            _eventBus = eventBus;
            _articleRepository = articleRepository;
        }
        public void AddArticle(Article article)
        {
            var command = new CreateArticleCommand(article);
            _eventBus.Publish(command);
        }

        public IList<ArticleModel> SelectByPage(int pageIndex, int pageSize, ArticleCondition condition = null)
        {
            IEnumerable<Article> articles = _articleRepository.SelectByPage(pageSize, pageIndex, condition);
            IList<ArticleModel> articleModels = new List<ArticleModel>();
            foreach (var item in articles)
            {
                ArticleModel articleModel = new ArticleModel();
                articleModel.Id = item.Id;
                articleModel.ArticleType = item.ArticleType.GetEnumText<ArticleType>();
                articleModel.TextSection = item.TextSection;
                articleModel.Title = item.Title;
                articleModel.Author = item.Author;
                articleModel.CreateTime = item.CreateTime.ToString("yyyy-MM-dd hh:mm");
                articleModel.IsDraft = item.IsDraft ? "是" : "否";
                articleModels.Add(articleModel);
            }
            return articleModels;
        }
        public async Task<IList<ArticleModel>> SelectByPageAsync(int pageIndex, int pageSize, ArticleCondition condition = null)
        {
            IEnumerable<Article> articles = await _articleRepository.SelectByPageAsync(pageSize, pageIndex, condition);
            IList<ArticleModel> articleModels = new List<ArticleModel>();
            foreach (var item in articles)
            {
                ArticleModel articleModel = new ArticleModel();
                articleModel.Id = item.Id;
                articleModel.ArticleType = item.ArticleType.GetEnumText<ArticleType>();
                articleModel.TextSection = item.TextSection;
                articleModel.Title = item.Title;
                articleModels.Add(articleModel);
            }
            return articleModels;
        }
        public ArticleModel Select(ArticleCondition articleCondition = null)
        {
            Article article = _articleRepository.Select(articleCondition);
            ArticleModel articleModel = new ArticleModel()
            {
                Id = article.Id
                ,
                Title = article.Title
                ,
                ArticleType = article.ArticleType.GetEnumText<ArticleType>()
                ,
                CreateTime = article.CreateTime.ToString("yyyy/MM/dd")
                ,
                Content = article.Content
                ,
                Author=article.Author
            };
            return articleModel;
        }
        public PageInfoMode SelectNextUp(int id, ArticleCondition articleCondition = null)
        {
            IEnumerable<dynamic> dynamics = _articleRepository.SelectNextUp(id, articleCondition);
            PageInfoMode pageInfoMode = new PageInfoMode();
            foreach (dynamic d in dynamics)
            {
                if (d.article_id > id)
                {
                    pageInfoMode.NextId = d.article_id;
                    pageInfoMode.NextTitle = d.article_title;
                }
                else
                {
                    pageInfoMode.BeforeId = d.article_id;
                    pageInfoMode.BeforeTitle = d.article_title;
                }
            }
            return pageInfoMode;
        }
    }
}
