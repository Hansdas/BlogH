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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Controllers.Add
{
    public class PublishController : Controller
    {
        IOptions<ApiSettingModel> _settings;
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
            string fileSavePath = string.Format("{0}/{1}/{2}/{3}", uploadSavePathBase, dateTime.Year.ToString(), dateTime.Month.ToString(), dateTime.Day.ToString());
            UserModel userModel = Auth.GetLoginUser();
            try
            {
                Parallel.For(0, srcArray.Length,async s =>
                {
                    long fileSize=await UploadHelper.Upload(srcArray[s]);
                    UploadFile uploadFile = new UploadFile(userModel.Account, fileSavePath,"",fileSize);
                });
            }
            catch (AggregateException)
            {
                //todo 有异常删除所有本次所传的附件
            }
            return "";
        }
    }
}