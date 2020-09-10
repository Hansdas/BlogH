

using Blog;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Domain;
using Blog.Domain.Core;
using Blog.Domain.Core.Event;
using Blog.Domain.Core.Notifications;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlogApi
{
    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IArticleService _articleService;
        private IArticleRepository _articleRepository;
        private IHttpContextAccessor _httpContext;
        private IUserService _userService;
        private NotifyValidationHandler _notifyValidationHandler;
        public ArticleController(IArticleService articleService, IArticleRepository articleRepository, IHttpContextAccessor httpContext
            , IUserService userService, IEventHandler<NotifyValidation> notifyValidationHandler)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
            _httpContext = httpContext;
            _userService = userService;
            _notifyValidationHandler = (NotifyValidationHandler)notifyValidationHandler;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="articleModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public ApiResult AddArticle([FromBody]ArticleModel articleModel)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            articleModel.Author = userModel.Account;
            _articleService.AddOrUpdate(articleModel);
            return ApiResult.Success();

        }
        /// <summary>
        /// 将临时附件路径转换为服务器路径
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pathValues"></param>
        /// <returns></returns>
        private string RegexContent(string content, IEnumerable<string> pathValues)
        {
            string pattern = @"((http|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(content);
            for (int i = 0; i < matches.Count; i++)
            {
                string src = matches[i].Groups[0].Value;
                foreach (string path in pathValues)
                {
                    string fileName = path.Substring(path.LastIndexOf("/") + 1);
                    if (src.IndexOf(fileName) > 0)
                    {
                        content = content.Replace(src, path);
                    }
                }
            }
            return content;
        }
        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="articleType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("total")]
        public ApiResult LoadTotal(int articleType)
        {
            ArticleCondition condition = new ArticleCondition();
            condition.ArticleType = articleType;
            int count = _articleRepository.SelectCount(condition);
            return ApiResult.Success(count);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("page")]
        public ApiResult SelectArticle([FromBody]ArticleConditionModel condition)
        {
            if (condition.LoginUser)
                condition.Account = Auth.GetLoginUser(_httpContext).Account;
            IList<ArticleModel> articleModels = _articleService.SelectByPage(condition);
            int total = _articleService.SelectCount(condition);
            return ApiResult.Success(new { list = articleModels, total = total });
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public ApiResult Detail(int id)
        {
            ArticleCondition condition = new ArticleCondition();
            condition.Id = id;
            ArticleModel articleModel = _articleService.Select(condition);
            return ApiResult.Success(articleModel);
        }
        /// <summary>
        /// 上一篇下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="articletype"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("context/{id}")]
        public ApiResult SelectContext(int id)
        {
            ArticleCondition condition = new ArticleCondition();
            condition.IsDraft = false;
            PageInfoMode result = _articleService.SelectContext(id, condition);
            return ApiResult.Success(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public ApiResult Delete(int id)
        {
            _articleRepository.Delete(id);
            return ApiResult.Success();
        }
        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="commentModel"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("comment/add")]
        public ApiResult Review()
        {
            string content = Request.Form["content"];
            int articleId = Convert.ToInt32(Request.Form["articleId"]);
            string revicer = Request.Form["revicer"];
            string replyId = Request.Form["replyId"];
            int commentType = Convert.ToInt32(Request.Form["commentType"]);
            try
            {
                CommentModel commentModel = new CommentModel();
                commentModel.Content = content;
                commentModel.AdditionalData = replyId;
                commentModel.PostUser = Auth.GetLoginUser(_httpContext).Account;
                commentModel.Revicer = revicer;
                commentModel.CommentType = commentType;
                _articleService.Review(commentModel, articleId);
                return ApiResult.Success();
            }
            catch (AuthException)
            {
                return ApiResult.Error(HttpStatusCode.FORBIDDEN, "not login");
            }

        }
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("praise/{id}")]
        public ApiResult Praise(int id)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            _articleService.Praise(id, userModel.Account, false);
            string message = _notifyValidationHandler.GetErrorList().Select(s => s.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(message))
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, message);
            return ApiResult.Success();
        }
        /// <summary>
        /// 取消点赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("praiseout/{id}")]
        public ApiResult PraiseOut(int id)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            _articleService.Praise(id, userModel.Account, true);
            return ApiResult.Success();
        }
        /// <summary>
        /// 热门推荐
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("hot")]
        public ApiResult HotArticle()
        {
            IList<ArticleModel> articleModels = _articleService.SelectHotArticles();
            return ApiResult.Success(articleModels);
        }
        /// <summary>
        /// 查询每种文章最新发布
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("type/maxtime")]
        public ApiResult SelectArticleByTypeMaxTime()
        {
            IList<ArticleModel> articleModels = _articleService.SelectByTypeMaxTime();
            return ApiResult.Success(articleModels);

        }
        /// <summary>
        /// 获取文章类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("types")]
        public ApiResult ArticleTypes()
        {
            IList<KeyValueItem> articleTypes = EnumConvert<ArticleType>.AsKeyValueItem();
            return ApiResult.Success(articleTypes);
        }
        /// <summary>
        /// 根据文章获取作者的所有文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all/{articleId}")]
        public ApiResult SelectAllByArticle(int articleId)
        {
            IList<ArticleModel> articleModels = _articleService.SelectAllByArticle(articleId);
            return ApiResult.Success(articleModels);
        }
        /// <summary>
        /// 根据账号获取个人归档
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("archive/{account}")]
        public ApiResult SelectArticleFile(string account)
        {
            ArticleCondition articleCondition = new ArticleCondition();
            articleCondition.Account = account;
            IList<ArticleFileModel> fileModels = _articleService.SelectArticleFile(articleCondition);
            UserModel userModel= _userService.SelectUser(account);
            return ApiResult.Success(new { fileModels,userModel});

        }
        /// <summary>
        /// 阅读排行前几
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("read/rank/{top}")]
        public ApiResult ArticleReadRank(int top)
        {
            IList<ArticleModel> fileModels = _articleService.SelectByRead(top);
            return ApiResult.Success(fileModels);
        }
    }
}