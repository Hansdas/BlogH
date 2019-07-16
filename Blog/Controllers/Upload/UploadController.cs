using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.Upload
{
    [Consumes("multipart/form-data")]
    public class UploadController : Controller
    {
        private IHostingEnvironment hostingEnvironment;
        public UploadController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }
        public IActionResult UploadPhoto()
        {
            var imgFile = Request.Form.Files[0];
            if(imgFile==null)
                return Json(new ReturnResult("500","附件为null"));
            if (string.IsNullOrEmpty(imgFile.FileName))
                return Json(new ReturnResult() { Code = "500", Message = "附件名称为空" });
            int index = imgFile.FileName.LastIndexOf('.');
            //获取后缀名
            string extension = imgFile.FileName.Substring(index, imgFile.FileName.Length- index);
            string webpath = hostingEnvironment.ContentRootPath;
            string guid = Guid.NewGuid().ToString().Replace("-","");
            string newFileName = guid + extension;
            DateTime dateTime = DateTime.Now;
            string path = string.Format(@"{0}\TemporaryFile\{1}\{2}\{3}", webpath, dateTime.Year.ToString(), dateTime.Month.ToString()
                , dateTime.Day.ToString());
           string imgSrc= DirectoryHelper.CreateDirectory(path) + @"\" + newFileName; ;
            using (FileStream fs = System.IO.File.Create(imgSrc))
            {
                imgFile.CopyTo(fs);
                fs.Flush();
            }
           return Json(new ReturnResult() { Code = "200", Message ="",Data=imgSrc });
        }
    }
}