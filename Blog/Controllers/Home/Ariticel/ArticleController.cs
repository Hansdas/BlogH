using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Attr;
using Blog.Common;
using Blog.Common.AppSetting;
using Blog.Domain;
using Blog.Domain.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Controllers.Home.Ariticel
{
    public class ArticleController : Controller
    {
        private readonly object _obj = new object();
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService, IWebHostEnvironment webHostEnvironment)
        {
            _articleService = articleService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [PermissionFilter]
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult AddArticle()
        {
            try
            {
                ArticleType articleType = Enum.Parse<ArticleType>(Request.Form["articletype"]);
                string title = Request.Form["title"];
                string content = Request.Form["content"];
                string imgSrc = Request.Form["imgSrc"];
                string textSection = Request.Form["text"];
                string[] srcArray = { };
                if (!string.IsNullOrEmpty(imgSrc))
                    srcArray = imgSrc.Trim(',').Split(',');
                UserModel userModel = Auth.GetLoginUser();
                IList<string> filePaths = new List<string>();
                try
                {
                    UploadFile(srcArray, userModel, filePaths);
                    Article article = new Article(title,textSection, content, articleType, true, filePaths);
                    //_articleService.(blog);
                }
                catch (AggregateException)
                {
                    //todo 有异常删除所有本次所传的附件
                }
                return Json(new ReturnResult() { Code = "200", Message = "ok" });
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        private void UploadFile(string[] srcArray, UserModel userModel, IList<string> filePaths)
        {
            Task[] tasks = new Task[srcArray.Length];
            for (int i = 0; i < srcArray.Length; i++)
            {
                int m = i;
                tasks[m] = Task.Run(() =>
                {
                    int index = srcArray[m].IndexOf(ConstantKey.STATIC_FILE) + ConstantKey.STATIC_FILE.Length;
                    string path = _webHostEnvironment.ContentRootPath + "/TempFile" + srcArray[m].Substring(index);
                    path = path.Replace("/", @"\");
                    string fileName = path.Substring(path.LastIndexOf(@"\") + 1);
                    string uploadSavePath;
                    long fileSzie = UploadHelper.Upload(path, fileName, userModel.Account, out uploadSavePath);
                    filePaths.Add(uploadSavePath);
                });
            }
            Task.WaitAll(tasks);
        }

        public int LoadTotal()
        {
            return 0;
        }
    }
}