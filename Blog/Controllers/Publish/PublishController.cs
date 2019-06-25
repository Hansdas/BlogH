using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.AppSetting;
using Blog.Domain;
using Blog.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Controllers.Add
{
    public class PublishController : Controller
    {
        IOptions<ApiSettingModel> _settings;
        private readonly object _obj = new object();
        public PublishController(IOptions<ApiSettingModel> settings)
        {
            _settings = settings;
        }
        public IActionResult Publish()
        {
            return View();
        }
        public   string AddWhisper()
        {
            string content = Request.Form["content"];
            string[] srcArray = Request.Form["imgUrls"].ToString().Trim(',').Split(',');
            string uploadSavePathBase = _settings.Value.UploadSavePathBase;
            DateTime dateTime = DateTime.Now;
            UserModel userModel = Auth.GetLoginUser();
            string fileSavePath = string.Format("{0}/{1}/{2}/{3}/{4}", uploadSavePathBase,userModel.Account, dateTime.Year.ToString()
                , dateTime.Month.ToString(), dateTime.Day.ToString());
            IList<UploadFile> uploadFiles = new List<UploadFile>();
            try
            {
                Parallel.For(0, srcArray.Length,async s =>
                {
                    int index = srcArray[s].LastIndexOf("\\") + 1;
                    string fileName = srcArray[s].Substring(index);
                    long fileSize=await UploadHelper.Upload(srcArray[s],fileSavePath,fileName);
                    string guid = Guid.NewGuid().ToString();
                    UploadFile uploadFile = new UploadFile(userModel.Account, guid, fileSavePath,fileName,fileSize);
                    lock(_obj)
                    {
                        uploadFiles.Add(uploadFile);
                    }
                });
                Whisper whisper = new Whisper(content,uploadFiles);
                BlogContent blogContent = new BlogContent(userModel.Account,BlogType.微语, whisper);

            }
            catch (AggregateException)
            {
                //todo 有异常删除所有本次所传的附件
            }
            return "";
        }
    }
}