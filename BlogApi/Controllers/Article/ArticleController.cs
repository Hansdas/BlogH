

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
        private NotifyValidationHandler _notifyValidationHandler;
        public ArticleController(IArticleService articleService,IArticleRepository articleRepository,IHttpContextAccessor httpContext
            , IEventHandler<NotifyValidation> notifyValidationHandler)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
            _httpContext = httpContext;
            _notifyValidationHandler = (NotifyValidationHandler)notifyValidationHandler;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="articleModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public IActionResult AddArticle([FromBody]ArticleModel articleModel)
        {
            try
            {
                UserModel userModel = Auth.GetLoginUser(_httpContext);
                IEnumerable<string> pathValues = UploadHelper.Upload(articleModel.FilePaths).Select(s => s.FilePath);
                articleModel.Content = RegexContent(articleModel.Content, pathValues);
                articleModel.Author = userModel.Account;
                _articleService.AddOrUpdate(articleModel);
                return new JsonResult(new ReturnResult("0"));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ReturnResult("1",ex.Message));
            }

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
                        content=content.Replace(src, path);
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
        public int LoadTotal(string articleType)
        {
            ArticleCondition condition = new ArticleCondition();
            condition.ArticleType = articleType;
            int count = _articleRepository.SelectCount(condition);
            return count;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("page")]
        public JsonResult SelectArticle([FromBody]ArticleConditionModel condition)
        {
            PageResult pageResult = new PageResult();
            try
            {
                IList<ArticleModel> articleModels = _articleService.SelectByPage(condition);
                pageResult.Data = articleModels;
                pageResult.Total = _articleService.SelectCount(condition);
                pageResult.Code = "0";
            }
            catch (Exception e)
            {
                pageResult.Data = null;
                pageResult.Code = "1";
                pageResult.Message = e.Message;
            }
            return new JsonResult(pageResult);
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Detail(int id)
        {
            PageResult pageResult = new PageResult();
            ArticleCondition condition = new ArticleCondition();
            condition.Id = id;
            try
            {
                ArticleModel articleModel = _articleService.Select(condition);
                pageResult.Data = articleModel;
                pageResult.Code = "0";
                pageResult.Message = "ok";
            }
            catch (Exception e)
            {
                pageResult.Code = "1";
                pageResult.Message = e.Message;
            }
            return new JsonResult(pageResult);
        }
        /// <summary>
        /// 上一篇下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="articletype"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("context/{id}/{articletype}")]
        public IActionResult SelectContext(int id, string articletype)
        {
            ReturnResult returnResult = new ReturnResult();
            ArticleCondition condition = new ArticleCondition();
            condition.IsDraft = false;
            if (string.IsNullOrEmpty(articletype))
                condition.ArticleType = Enum.Parse<ArticleType>(articletype).GetEnumValue().ToString();
            try
            {
                PageInfoMode result = _articleService.SelectContext(id, condition);
                returnResult.Data = result;
                returnResult.Code = "0";
            }
            catch (Exception e)
            {
                returnResult.Data = null;
                returnResult.Code = "1";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                _articleRepository.Delete(id);
                result.Code = "200";
            }
            catch (Exception e)
            {
                result.Code = "500";
                result.Message = e.Message;
            }
            return new JsonResult(result);
        }
        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="commentModel"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("review/{articleId}")]
        public  JsonResult Review([FromBody]CommentModel commentModel,int articleId)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            commentModel.PostUser = userModel.Account;
            _articleService.Review(commentModel, articleId);
            commentModel.PostUsername = userModel.Username;
            commentModel.PostDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
            commentModel.Guid = "";
            return new JsonResult(new ReturnResult("0", commentModel));
        }
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("praise/{id}")]
        public JsonResult Praise(int id)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            _articleService.Praise(id, userModel.Account,false);
            string message= _notifyValidationHandler.GetErrorList().Select(s=>s.Value).FirstOrDefault();
            if(!string.IsNullOrEmpty(message))
                return new JsonResult(new ReturnResult("0",message));
            return new JsonResult(new ReturnResult("0","0"));
        }
        /// <summary>
        /// 取消点赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("praiseout/{id}")]
        public JsonResult PraiseOut(int id)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            _articleService.Praise(id, userModel.Account,true);
            return new JsonResult(new ReturnResult("0"));
        }
        /// <summary>
        /// 热门推荐
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("hot")]
        public JsonResult HotArticle()
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                IList<ArticleModel> articleModels = _articleService.SelectHotArticles();
                returnResult.Data = articleModels;
                returnResult.Code = "0";
            }
            catch (Exception e)
            {
                returnResult.Data = null;
                returnResult.Code = "1";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }
        /// <summary>
        /// 查询每种文章最新发布
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("type/maxtime")]
        public JsonResult SelectArticleByTypeMaxTime()
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                IList<ArticleModel> articleModels = _articleService.SelectByTypeMaxTime();
                returnResult.Data = articleModels;
                returnResult.Code = "0";
            }
            catch (Exception e)
            {
                returnResult.Data = null;
                returnResult.Code = "1";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }
        /// <summary>
        /// 获取文章类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("types")]
        public JsonResult ArticleTypes()
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                IList<KeyValueItem> articleTypes = EnumConvert<ArticleType>.AsKeyValueItem();
                returnResult.Data = articleTypes;
                returnResult.Code = "0";
            }
            catch (Exception e)
            {
                returnResult.Data = null;
                returnResult.Code = "1";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }
        /// <summary>
        /// 根据文章获取改作者的所有
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all/{articleId}")]
        public JsonResult SelectAllByArticle(int articleId)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                IList<ArticleModel> articleModels = _articleService.SelectAllByArticle(articleId);
                returnResult.Data = articleModels;
                returnResult.Code = "0";
            }
            catch (Exception e)
            {
                returnResult.Data = null;
                returnResult.Code = "1";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }
    }
}