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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Controllers.Home.Ariticel
{
    public class ArticleController : Controller
    {
        private readonly object _obj = new object();
        private readonly IBlogService _blogService;
        public ArticleController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [PermissionFilter]
        public IActionResult AddArticle()
        {
            try
            {
                ArticleType articleType = Enum.Parse<ArticleType>(Request.Form["articletype"]);
                string title = Request.Form["title"];
                string content = Request.Form["content"];
                string[] srcArray = Request.Form["imgUrls"].ToString().Trim(',').Split(',');
                UserModel userModel = Auth.GetLoginUser();
                IList<UploadFile> uploadFiles = new List<UploadFile>();
                try
                {
                    Task[] tasks = new Task[srcArray.Length];
                    for (int i = 0; i < srcArray.Length; i++)
                    {
                        int m = i;
                        tasks[m] = Task.Run(async () =>
                        {
                            int index = srcArray[m].LastIndexOf("\\") + 1;
                            string fileName = srcArray[m].Substring(index);
                            dynamic d = await UploadHelper.Upload(srcArray[m], fileName,userModel.Account);
                            string guid = Guid.NewGuid().ToString();
                            UploadFile uploadFile = new UploadFile(userModel.Account, guid, d.path, fileName, d.size);
                            lock (_obj)
                            {
                                uploadFiles.Add(uploadFile);
                            }
                        });
                    }
                    Task.WaitAll(tasks);
                    Article article = new Article(title,content, articleType, true, uploadFiles);
                    Domain.Blog blog = new Domain.Blog(userModel.Account, BlogType.文章, article);
                    _blogService.PublishBlog(blog);
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
    }
}