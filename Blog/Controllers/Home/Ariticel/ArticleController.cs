using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Attr;
using Blog.Common;
using Blog.Common.AppSetting;
using Blog.Domain;
using Blog.Domain.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Blog.Controllers.Home.Ariticel
{
    public class ArticleController : Controller
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
        public IActionResult Index()
        {
            return View();
        }
        [PermissionFilter]
        public IActionResult AddArticle()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult Publish()
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
            catch (AggregateException ex)
            {
                return Json(new ReturnResult() { Code = "500", Message ="服务器异常" });
                //todo 有异常删除所有本次所传的附件
            }
            return Json(new ReturnResult() { Code = "200", Message = "ok" });
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
        public int LoadTotal(string articleType)
        {
            ArticleCondition condition = new ArticleCondition();
            condition.ArticleType = articleType;
            int count = _articleRepository.SelectCount(condition);
            return count;
        }
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