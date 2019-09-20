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
        private readonly IMediatorHandler _mediatorHandler;
        public ArticleService(IMediatorHandler mediatorHandler, IArticleRepository articleRepository)
        {
            _mediatorHandler = mediatorHandler;
            _articleRepository = articleRepository;
        }
        public void Publish(Article article)
        {
            try
            {
                var command = new CreateArticleCommand(article);
                _mediatorHandler.SendCommand(command);
            }
            catch (Exception)
            {

                throw;
            }
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
                Content = RegexContent(article.Content)
            };
            return articleModel;
        }
        public PageInfoMode SelectNextUp(int id, ArticleCondition articleCondition = null)
        {
            IEnumerable<dynamic> dynamics = _articleRepository.SelectNextUp(id,articleCondition);
            PageInfoMode pageInfoMode = new PageInfoMode();
            int i = 0;
            foreach (dynamic d in dynamics)
            {
                if (i == 0)
                {
                    pageInfoMode.BeforeId = d.article_id;
                    pageInfoMode.BeforeTitle = d.article_title;
                }
                else
                {
                    pageInfoMode.NextId = d.article_id;
                    pageInfoMode.NextTitle = d.article_title;
                }
            }
            return pageInfoMode;
        }
        private string RegexContent(string input)
        {
            string pattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgsrc>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(input);
            for (int i = 0; i < matches.Count; i++)
            {
                string src = matches[i].Groups["imgsrc"].Value;
                string path = src.Substring(src.IndexOf(ConstantKey.STATIC_FILE) + ConstantKey.STATIC_FILE.Length);
                try
                {
                    string loaclPath = UploadHelper.DownFileAsync(path).Result;
                    input = input.Replace(src, loaclPath);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return input;
        }
    }
}
