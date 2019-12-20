

using Blog;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Domain;
using Blog.Domain.Core;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [GlobaExceptionFilter]
    public class ArticleController : ControllerBase
    {
        private IArticleService _articleService;
        private IArticleRepository _articleRepository;
        private IHttpContextAccessor _httpContext;
        public ArticleController(IArticleService articleService,IArticleRepository articleRepository,IHttpContextAccessor httpContext)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
            _httpContext = httpContext;
        }
        [HttpPost]
        public IActionResult AddArticle([FromBody]ArticleModel articleModel)
        {
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            IEnumerable<string> pathValues =UploadHelper.Upload(articleModel.FilePaths).Select(s=>s.FilePath);
            articleModel.Content= RegexContent(articleModel.Content, pathValues);
            _articleService.AddOrUpdate(articleModel);            
            return new JsonResult(new ReturnResult("200"));
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

        [HttpGet]
        public int LoadTotal(string articleType)
        {
            ArticleCondition condition = new ArticleCondition();
            condition.ArticleType = articleType;
            int count = _articleRepository.SelectCount(condition);
            return count;
        }

        [HttpGet]
        public JsonResult LoadArticle(int pageIndex, int pageSize, string articleType)
        {
            PageResult pageResult = new PageResult();
            ArticleCondition condition = new ArticleCondition();
            condition.ArticleType = articleType;
            condition.IsDraft = false;
            try
            {
                IList<ArticleModel> articleModels = _articleService.SelectByPage(pageIndex, pageSize, condition);
                pageResult.Data = articleModels;
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
        [HttpGet("{id}")]
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
        [HttpGet("{id}/{articletype}")]
        public IActionResult NextUp(int id, string articletype)
        {
            ReturnResult returnResult = new ReturnResult();
            ArticleCondition condition = new ArticleCondition();
            if(string.IsNullOrEmpty(articletype))
                condition.ArticleType = Enum.Parse<ArticleType>(articletype).GetEnumValue().ToString();
            try
            {
                PageInfoMode result = _articleService.SelectNextUp(id, condition);
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

        [HttpDelete("{id}")]
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
        [HttpPost("{articleId}")]
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
    }
}