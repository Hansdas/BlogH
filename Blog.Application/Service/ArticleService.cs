using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
        public ArticleService(IMediatorHandler mediatorHandler,IArticleRepository articleRepository)
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
            IEnumerable<Article> articles = _articleRepository.SelectByPage(pageSize,pageIndex, condition);
            IList<ArticleModel> articleModels = new List<ArticleModel>();
            foreach(var item in articles)
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
            ArticleModel articleModel = new ArticleModel() {
                Id = article.Id
                , Title = article.Title
                , ArticleType = article.ArticleType.GetEnumText<ArticleType>()
                , CreateTime = article.CreateTime.ToString("yyyy/MM/dd")
                , Content = RegexContent(article.Content)
            };
            return articleModel;
        }
        private string RegexContent(string input)
        {
            string pattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgsrc>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(input);
            for (int i = 0; i < matches.Count; i++)
            {
                string src = matches[i].Groups["imgsrc"].Value;
                string path = src.Substring(src.IndexOf(ConstantKey.STATIC_FILE)+ConstantKey.STATIC_FILE.Length);
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
