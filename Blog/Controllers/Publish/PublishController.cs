using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.AppSetting;
using Blog.Domain;
using Blog.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Controllers
{
    public class PublishController : Controller
    {
        private readonly  IOptions<ApiSettingModel> _settings;
        private readonly  IArticleService _blogService;

        private readonly object _obj = new object();
        public PublishController(IOptions<ApiSettingModel> settings, IArticleService blogService)
        {
            _settings = settings;
            _blogService = blogService;
        }
        public IActionResult Publish()
        {
            return View();
        }
        public IActionResult AddWhisper()
        {
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
                        string uploadSavePath;
                        long fileSize = UploadHelper.Upload(srcArray[m], fileName, userModel.Account,out uploadSavePath);
                        string guid = Guid.NewGuid().ToString();
                        UploadFile uploadFile = new UploadFile(userModel.Account, guid, uploadSavePath, fileName, fileSize);
                        lock (_obj)
                        {
                            uploadFiles.Add(uploadFile);
                        }
                    });
                }
                Task.WaitAll(tasks);
                Whisper whisper = new Whisper(content, uploadFiles);
                Domain.Blog blog = new Domain.Blog(userModel.Account, BlogType.微语, whisper);
                _blogService.PublishBlog(blog);
            }
            catch (AggregateException)
            {
                //todo 有异常删除所有本次所传的附件
            }
            return Json(new ReturnResult() { Code = "200", Message = "ok" });
        }

        //private void Upload(string[] srcArray, UserModel userModel, string fileSavePath, IList<UploadFile> uploadFiles)
        //{
        //    Parallel.For(0, srcArray.Length, async s =>
        //    {
        //        int index = srcArray[s].LastIndexOf("\\") + 1;
        //        string fileName = srcArray[s].Substring(index);
        //        long fileSize = await UploadHelper.Upload(srcArray[s], fileSavePath, fileName);
        //        string guid = Guid.NewGuid().ToString();
        //        UploadFile uploadFile = new UploadFile(userModel.Account, guid, fileSavePath, fileName, fileSize);
        //        lock (_obj)
        //        {
        //            uploadFiles.Add(uploadFile);
        //        }
        //    });
        //}
    }
}