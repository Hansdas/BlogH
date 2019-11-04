using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blog.Common;
using Blog.Domain.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers.Upload
{
    [Route("api/[controller]/[action]")]
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
        [HttpGet]
        public IActionResult InitPage()
        {
            Array array= Enum.GetValues(typeof(ArticleType));
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (var item in array)
            {
                pairs.Add(item.ToString(), Enum.GetName(typeof(ArticleType), item));
            }
            ReturnResult returnResult = new ReturnResult();
            returnResult.Code = "200";
            returnResult.Data = pairs;
            return Json(returnResult);
        }
        [HttpGet]
        public string GetIp()
        {
            return HttpHelper.GetRequestIP(_accessor);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [EnableCors("AllowSpecificOrigins")]
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
            string virtualPath = GetIp() + ConstantKey.STATIC_FILE + pathValue.DatePath + pathValue.FileName;
            return Json(new { Code = "200", Data = new { Src = virtualPath, Title = imgFile.FileName } });
        }
    
        public IActionResult DeleteFile(string imgpath)
        {
            int index = imgpath.IndexOf(ConstantKey.STATIC_FILE) + ConstantKey.STATIC_FILE.Length;
            string path = _webHostEnvironment.ContentRootPath + "/TempFile" + imgpath.Substring(index);
            path = path.Replace("/", @"\");
            DirectoryHelper.Delete(path);
            return Json(new ReturnResult("0"));
        }
    }
}