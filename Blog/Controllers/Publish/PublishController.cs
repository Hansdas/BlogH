using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blog.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.Add
{
    public class PublishController : Controller
    {
        public IActionResult Publish()
        {
            return View();
        }
        public  string AddWhisper()
        {
            string content = Request.Form["content"];
            string[] srcArray = Request.Form["imgUrls"].ToString().Trim(',').Split(',');
            try
            {
                Parallel.For(0, srcArray.Length,s =>
                {
                    UploadHelper.Upload(srcArray[s]);
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