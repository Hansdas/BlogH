using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
        private ICommentRepository _commentRepository;
        private IUserRepository _userRepository;
        public ArticleService(IEventBus eventBus, IArticleRepository articleRepository, ICommentRepository commentRepository
            ,IUserRepository userRepository)
        {
            _eventBus = eventBus;
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }
        private ArticleCondition ConvertCondition(ArticleConditionModel articleConditionModel)
        {
            ArticleCondition articleCondition = new ArticleCondition();
            articleCondition.ArticleType = articleConditionModel.ArticleType;
            articleCondition.FullText = articleConditionModel.FullText;
            articleCondition.IsDraft = false;
            return articleCondition;
        }
        public void AddOrUpdate(ArticleModel model)
        {
            Article article = new Article(
                model.Id
                , model.Author
                , model.Title
                , model.TextSection
                , model.Content
                , Enum.Parse<ArticleType>(model.ArticleType)
                , Convert.ToBoolean(model.IsDraft));
            if (article.Id > 0)
            {
                UpdateArticleCommand updateArticleCommand = new UpdateArticleCommand(article);
                _eventBus.Publish(updateArticleCommand);
            }
            else
            {
                CreateArticleCommand createArticleCommand = new CreateArticleCommand(article);
                _eventBus.Publish(createArticleCommand);
            }
        }

        public IList<ArticleModel> SelectByPage(ArticleConditionModel articleConditionModel)
        {
            ArticleCondition condition=ConvertCondition(articleConditionModel);
            IEnumerable<Article> articles = _articleRepository.SelectByPage(articleConditionModel.PageSize, articleConditionModel.PageIndex, condition);
            IList<ArticleModel> articleModels = new List<ArticleModel>();
            foreach (var item in articles)
            {
                ArticleModel articleModel = new ArticleModel();
                articleModel.Id = item.Id;
                articleModel.ArticleType = item.ArticleType.GetEnumText();
                articleModel.TextSection = item.TextSection.Trim();
                articleModel.Title = item.Title;
                articleModel.Author = item.Author;
                articleModel.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd hh:mm");
                articleModel.IsDraft = item.IsDraft ? "是" : "否";
                articleModel.PraiseCount = item.PraiseCount;
                articleModel.BrowserCount = item.BrowserCount;
                articleModel.CommentCount = string.IsNullOrEmpty(item.CommentIds) ? 0 : item.CommentIds.Split(',').Length;
                articleModels.Add(articleModel);
            }
            return articleModels;
        }
        public int SelectCount(ArticleConditionModel articleConditionModel = null)
        {
            ArticleCondition articleCondition = null;
            if (articleConditionModel != null)
                articleCondition = ConvertCondition(articleConditionModel);
            return _articleRepository.SelectCount(articleCondition);
        }
        public ArticleModel Select(ArticleCondition articleCondition = null)
        {
            Article article = _articleRepository.Select(articleCondition);
            ArticleModel articleModel = new ArticleModel()
            {
                Id = article.Id,
                Title = article.Title,
                ArticleType = article.ArticleType.GetEnumText(),
                CreateTime = article.CreateTime.Value.ToString("yyyy/MM/dd"),
                Content = article.Content,
                Author = article.Author,
                IsDraft = article.IsDraft ? "是" : "否",
                Comments = GetCommentModels(article.Comments)
            };
            return articleModel;
        }
        private IList<CommentModel> GetCommentModels(IList<Comment> comments)
        {
            IList<CommentModel> commentModels = new List<CommentModel>();
           foreach (var item in comments)
            {
                CommentModel commentModel = new CommentModel();
                commentModel.Guid = item.Guid;
                commentModel.Content = item.Content;
                commentModel.PostUser = item.PostUser;
                commentModel.PostUsername = item.PostUsername;
                commentModel.PostDate = item.PostDate.ToString("yyyy-MM-dd hh:mm");
                commentModels.Add(commentModel);
            }
            return commentModels;
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

        public void Review(CommentModel commentModel, int id)
        {
            string guid = Guid.NewGuid().ToString();           
            Comment comment = new Comment(guid, commentModel.Content,commentModel.PostUser, commentModel.ReplyGuid);
            UpdateArticleCommand command = new UpdateArticleCommand(comment, id);
            _eventBus.Publish(command);
        }

        public void Praise(int articleId, string account,bool cancle)
        {
            PraiseArticleCommand praiseArticleCommand = new PraiseArticleCommand(articleId,account, cancle);
            _eventBus.Publish(praiseArticleCommand);
        }

        public IList<ArticleModel> SelectHotArticles()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("article_praisecount", "DESC");
            IEnumerable<Article> articles = _articleRepository.SelectTop(5, collection);
            IList<ArticleModel> articleModels = new List<ArticleModel>();
            foreach (var item in articles)
            {
                ArticleModel articleModel = new ArticleModel();
                articleModel.Id = item.Id;
                articleModel.ArticleType = item.ArticleType.GetEnumText();
                articleModel.Title = item.Title.Length>22?item.Title.Substring(0,20):item.Title;
                articleModels.Add(articleModel);
            }
            return articleModels;
        }
        public IList<ArticleModel> SelectByTypeMaxTime()
        {
            IEnumerable<Article> articles = _articleRepository.SelectByTypeMaxTime();
            Dictionary<string, string> users = _userRepository.SelectNameWithAccountDic(articles.Select(s => s.Author).Distinct());
            IList<ArticleModel> articleModels = new List<ArticleModel>();
            foreach(var item in articles)
            {
                ArticleModel articleModel = new ArticleModel();
                articleModel.ArticleType = item.ArticleType.GetEnumText();
                articleModel.Id = item.Id;
                articleModel.Title = item.Title;
                articleModel.Author = users[item.Author];
                articleModel.AuthorAccount = item.Author;
                articleModel.TextSection = item.TextSection.Trim();
                articleModel.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd hh:mm");
                articleModels.Add(articleModel);
            }
            return articleModels;
        }
    }
}
