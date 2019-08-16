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
        private readonly IOptions<ApiSettingModel> _settings;
        private readonly object _obj = new object();
        private readonly IBlogService _blogService;
        public ArticleController(IOptions<ApiSettingModel> settings, IBlogService blogService)
        {
            _settings = settings;
            _blogService = blogService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[PermissionFilter]
        public IActionResult AddArticle()
        {
            try
            {
                //string articleType = Request.Form["type"];
                //string title = Request.Form["title"];
                //string content = Request.Form["content"];
                //string[] srcArray = Request.Form["imgUrls"].ToString().Trim(',').Split(',');
                string[] srcArray = {  };
                DateTime dateTime = DateTime.Now;
                UserModel userModel = Auth.GetLoginUser();
                string uploadSavePathBase = _settings.Value.UploadSavePathBase;
                string fileSavePath = string.Format("{0}{1}/{2}/{3}/{4}", uploadSavePathBase, "11", dateTime.Year.ToString()
                , dateTime.Month.ToString(), dateTime.Day.ToString());
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
                            long fileSize = await UploadHelper.Upload(srcArray[m], fileSavePath, fileName);
                            string guid = Guid.NewGuid().ToString();
                            UploadFile uploadFile = new UploadFile(userModel.Account, guid, fileSavePath, fileName, fileSize);
                            lock (_obj)
                            {
                                uploadFiles.Add(uploadFile);
                            }
                        });
                    }
                    Task.WaitAll(tasks);
                    Article article = new Article("111","111111",ArticleType.旅游杂记,true, uploadFiles);
                    Domain.Blog blog = new Domain.Blog("111", BlogType.文章, article);
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