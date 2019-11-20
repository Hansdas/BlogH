﻿

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
    public class ArticleController : ControllerBase
    {
        private readonly object _obj = new object();
        private IWebHostEnvironment _webHostEnvironment;
        private IArticleService _articleService;
        private IArticleRepository _articleRepository;
        private ICacheClient _cacheClient;
        private IHttpContextAccessor _httpContext;
        public ArticleController(IArticleService articleService, IWebHostEnvironment webHostEnvironment
            , IArticleRepository articleRepository, ICacheClient cacheClient, IHttpContextAccessor httpContext)
        {
            _articleService = articleService;
            _webHostEnvironment = webHostEnvironment;
            _articleRepository = articleRepository;
            _cacheClient = cacheClient;
            _httpContext = httpContext;
        }
        [HttpPost]
        public IActionResult AddArticle()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            ArticleType articleType = Enum.Parse<ArticleType>(Request.Form["articletype"]);
            string title = Request.Form["title"];
            string content = Request.Form["content"];
            string imgSrc = Request.Form["imgSrc"];
            string textSection = Request.Form["textsection"];
            bool isDraft = Convert.ToBoolean(Request.Form["isDraft"]);
            string[] srcArray = { };
            if (!string.IsNullOrEmpty(imgSrc))
                srcArray = imgSrc.Trim(',').Split(',');
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            IList<string> filePaths = new List<string>();
            UploadHelper.Upload(srcArray);
            Article article = new Article(id,userModel.Account, title, textSection, content, articleType, isDraft);
            _articleService.AddArticle(article);
            return new JsonResult(new ReturnResult("200"));
        }
        [EnableCors("AllowSpecificOrigins")]
        [HttpGet]
        public int LoadTotal(string articleType)
        {
            ArticleCondition condition = new ArticleCondition();
            condition.ArticleType = articleType;
            int count = _articleRepository.SelectCount(condition);
            return count;
        }
        [EnableCors("AllowSpecificOrigins")]
        [HttpGet]
        public JsonResult LoadArticle(int pageIndex, int pageSize, string articleType)
        {
            PageResult pageResult = new PageResult();
            ArticleCondition condition = new ArticleCondition();
            condition.ArticleType = articleType;
            try
            {
                IList<ArticleModel> articleModels = _articleService.SelectByPage(pageIndex, pageSize, condition);
                pageResult.Data = articleModels;
                pageResult.Code = "200";
                pageResult.Message = "ok";
            }
            catch (Exception e)
            {
                pageResult.Data = null;
                pageResult.Code = "500";
                pageResult.Message = e.Message;
            }
            return new JsonResult(pageResult);
        }

        [EnableCors("AllowSpecificOrigins")]
        [HttpGet]
        public JsonResult SelectArticle(int page, int limit, string title, bool isDraft)
        {
            PageResult pageResult = new PageResult();
            ArticleCondition condition = new ArticleCondition();
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            condition.TitleContain = title;
            condition.IsDraft = isDraft;
            condition.Account = userModel.Account;
            try
            {
                IList<ArticleModel> articleModels = _articleService.SelectByPage(page, limit, condition);
                pageResult.Total = _articleRepository.SelectCount(condition);
                pageResult.Data = articleModels;
                pageResult.Code = "0";
                pageResult.Message = "";
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
                pageResult.Code = "200";
                pageResult.Message = "ok";
            }
            catch (Exception e)
            {
                pageResult.Data = null;
                pageResult.Code = "500";
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
                returnResult.Code = "200";
                returnResult.Message = "ok";
            }
            catch (Exception e)
            {
                returnResult.Data = null;
                returnResult.Code = "500";
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
    }
}