using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blog;
using Blog.Common;
using Blog.Domain.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers.Upload
{
    [Route("api")]
    [ApiController]
    public class UploadController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IHttpContextAccessor _accessor;
        public UploadController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor accessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _accessor = accessor;
        }
        private string GetIp()
        {
            return HttpHelper.GetRequestIP(_accessor);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("upload/image")]
        public IActionResult UploadImage()
        {   
            int height =Convert.ToInt32(Request.Form["height"]);
            int width = Convert.ToInt32(Request.Form["height"]);
            bool b = Convert.ToBoolean(Request.Form["isAbs"]);
            var imgFile = Request.Form.Files[0];
            if (imgFile == null)
                return Json(new ReturnResult("500", "附件为null"));
            if (string.IsNullOrEmpty(imgFile.FileName))
                return Json(new ReturnResult("500", "附件名称为空"));
            PathValue pathValue =UploadHelper.SaveFile(imgFile.FileName);
            UploadHelper.CompressImage(pathValue.FilePath, imgFile.OpenReadStream(), height, width, b);
            //使用虚拟静态资源路径，否则无法读取到图片
            var configuration = ConfigurationProvider.configuration.GetSection("loaclweb");
            string ip = configuration.GetSection("httpAddresss").Value;
            string port = configuration.GetSection("port").Value;
            string virtualPath = string.Format("http://{0}:{1}{2}", ip,port, ConstantKey.STATIC_FILE + pathValue.DatePath + pathValue.FileName);
            //string virtualPath = GetIp() + ConstantKey.STATIC_FILE + pathValue.DatePath + pathValue.FileName;
            return Json(new { Code = "0", Data = new { Src = virtualPath, Title = imgFile.FileName } });
        }
         /// <summary>
         /// 删除图片
         /// </summary>
         /// <returns></returns>
        [HttpDelete]
        [Route("upload/image/delete")]
        public IActionResult DeleteFile()
        {
            string imgpath = Request.Form["imgpath"];
            int index = imgpath.IndexOf(ConstantKey.STATIC_FILE) + ConstantKey.STATIC_FILE.Length;
            string path = _webHostEnvironment.ContentRootPath + "/TempFile" + imgpath.Substring(index);
            DirectoryHelper.Delete(path);
            return Json(new ReturnResult("0"));
        }
    }
}