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
        private IUserService _userService;
        public ArticleService(IEventBus eventBus, IArticleRepository articleRepository, ICommentRepository commentRepository
            ,IUserRepository userRepository, IUserService userService)
        {
            _eventBus = eventBus;
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _userService = userService;
        }
        private ArticleCondition ConvertCondition(ArticleConditionModel articleConditionModel)
        {
            ArticleCondition articleCondition = new ArticleCondition();
            articleCondition.ArticleType = articleConditionModel.ArticleType;
            articleCondition.FullText = articleConditionModel.FullText;
            articleCondition.IsDraft =Convert.ToBoolean(articleConditionModel.IsDraft);
            articleCondition.TitleContain = articleConditionModel.TitleContain;
            if (!string.IsNullOrEmpty(articleConditionModel.Account))
                articleCondition.Account = articleConditionModel.Account;
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
            Dictionary<string, string> userNames = _userRepository.SelectNameWithAccountDic(articles.Select(s=>s.Author));
            IList<ArticleModel> articleModels = new List<ArticleModel>();
            foreach (var item in articles)
            {
                ArticleModel articleModel = new ArticleModel();
                articleModel.Id = item.Id;
                articleModel.ArticleType = item.ArticleType.GetEnumText();
                articleModel.TextSection = item.TextSection.Trim();
                articleModel.Title = item.Title;
                articleModel.AuthorAccount = item.Author;
                articleModel.Author = userNames[item.Author];
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
            UserModel userModel= _userService.SelectUser(article.Author);
            ArticleModel articleModel = new ArticleModel()
            {
                Id = article.Id,
                Title = article.Title,
                ArticleType = article.ArticleType.GetEnumText(),
                CreateTime = article.CreateTime.Value.ToString("yyyy/MM/dd"),
                Content = article.Content,
                Author = userModel.Username,
                AuthorAccount=userModel.Account,
                IsDraft = article.IsDraft ? "是" : "否",
                Comments = CommentModel.ConvertToCommentModels(article.Comments)
            };
            return articleModel;
        }

        public PageInfoMode SelectContext(int id, ArticleCondition articleCondition = null)
        {
            IEnumerable<dynamic> dynamics = _articleRepository.SelectContext(id, articleCondition);
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
            Comment comment = new Comment(guid, commentModel.Content, Enum.Parse<CommentType>(commentModel.CommentType.ToString()),commentModel.PostUser,commentModel.Revicer, commentModel.AdditionalData);
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

        public IList<ArticleModel> SelectAllByArticle(int articleId)
        {
            IEnumerable<Article> articles = _articleRepository.SelectAllByArticle(articleId);
            IList<ArticleModel> articleModels = new List<ArticleModel>();
            foreach (var item in articles)
            {
                ArticleModel articleModel = new ArticleModel();
                articleModel.ArticleType = item.ArticleType.GetEnumText();
                articleModel.Id = item.Id;
                articleModel.Title = item.Title;
                articleModel.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd");
                articleModels.Add(articleModel);
            }
            return articleModels;
        }
        public IList<ArticleFileModel> SelectArticleFile(ArticleCondition articleCondition)
        {
            IEnumerable<dynamic> resultList = _articleRepository.SelectArticleFile(articleCondition);
            IList<ArticleFileModel> fileModels = new List<ArticleFileModel>();
            foreach(var item in resultList)
            {
                ArticleFileModel model = new ArticleFileModel();
                model.ArticleType = item.article_articletype;
                model.Total = (int)item.count;
                model.Account = item.article_author;
                fileModels.Add(model);
            }
            return fileModels;
        }
    }
}
