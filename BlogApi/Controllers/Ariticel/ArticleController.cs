﻿

using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Domain;
using Blog.Domain.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Blog.Controllers.Home.Ariticel
{
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly object _obj = new object();
        private IWebHostEnvironment _webHostEnvironment;
        private  IArticleService _articleService;
        private IArticleRepository _articleRepository;
        public ArticleController(IArticleService articleService, IWebHostEnvironment webHostEnvironment,IArticleRepository articleRepository)
        {
            _articleService = articleService;
            _webHostEnvironment = webHostEnvironment;
            _articleRepository = articleRepository;
        }
        [Route("/addarticle")]
        [HttpPost]
        public object AddArticle()
        {
            ArticleType articleType = Enum.Parse<ArticleType>(Request.Form["articletype"]);
            string title = Request.Form["title"];
            string content = Request.Form["content"];
            string imgSrc = Request.Form["imgSrc"];
            string textSection = Request.Form["textsection"];
            string[] srcArray = { };
            if (!string.IsNullOrEmpty(imgSrc))
                srcArray = imgSrc.Trim(',').Split(',');
            UserModel userModel = Auth.GetLoginUser();
            IList<string> filePaths = new List<string>();
            try
            {
                if(srcArray.Length>0)
                    filePaths=UploadHelper.Upload(srcArray, _webHostEnvironment.ContentRootPath);
                RegexContent(filePaths, content);
                Article article = new Article(userModel.Username, title, textSection, content, articleType, true, filePaths);
                _articleService.Publish(article);
            }
            catch (AggregateException)
            {
                return JsonHelper.Serialize(new ReturnResult() { Code = "500", Message ="服务器异常" });
                //todo 有异常删除所有本次所传的附件
            }
            return JsonHelper.Serialize(new ReturnResult() { Code = "500", Message = "服务器异常" }); ;
        }
        private void RegexContent(IList<string> savePaths,string input)
        {
            string pattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            Regex regex = new Regex(pattern);
            MatchCollection matches =regex.Matches(pattern);
            for (int i = 0; i < matches.Count; i++)
            {
                foreach (string item in savePaths)
                {
                    string fileName = item.Substring(item.LastIndexOf(@"\") + 1);
                    if (matches[i].Value.IndexOf(fileName) > 0)
                    {
                        string oldSrc = matches[i].Groups["imgsrc"].Value;
                        input = input.Replace(oldSrc, item);
                    }

                }
            }
        }
        [Route("{articleType}")]
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
        [HttpGet("{id}")]
        public IActionResult LoadArticleDetail(int id)
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
    }
}