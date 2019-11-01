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
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult UploadPhoto()
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile == null)
                return Json(new ReturnResult("1", "附件为null"));
            if (string.IsNullOrEmpty(imgFile.FileName))
                return Json(new ReturnResult("1", "附件名称为空"));
            var value = CombinePath(imgFile);
            using (FileStream fs = System.IO.File.Create(value.imgSrc))
            {
                imgFile.CopyTo(fs);
                fs.Flush();
            }
            return Json(new ReturnResult("0", "", value.imgSrc));
        }
        [HttpGet]
        public string GetIp()
        {
            string ip = "";
            var connectionInfo = _accessor.HttpContext.Connection;
            ip = connectionInfo.LocalIpAddress.ToString();
            if (ip == "::1")
            {
                ip = "127.0.0.1";
                ip = string.Format("https://{0}:{1}", ip, connectionInfo.LocalPort);
            }
            else
                ip = string.Format("http://{0}:{1}", ip, connectionInfo.LocalPort);
            return ip;


        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [EnableCors("AllowSpecificOrigins")]
        public IActionResult UploadImage()
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile == null)
                return Json(new ReturnResult("1", "附件为null"));
            if (string.IsNullOrEmpty(imgFile.FileName))
                return Json(new ReturnResult("1", "附件名称为空"));
            var value = CombinePath(imgFile);
            UploadHelper.CompressImage(value.imgSrc, imgFile.OpenReadStream(), 168, 168, false);
            //使用虚拟静态资源路径，否则无法读取到图片
            string virtualPath = GetIp() + ConstantKey.STATIC_FILE + value.datePath + value.newFileName;
            return Json(new { Code = "200", Data = new { Src = virtualPath, Title = imgFile.FileName } });
        }
        /// <summary>
        /// 组合图片保存路径
        /// </summary>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        private (string newFileName, string datePath, string imgSrc) CombinePath(IFormFile imgFile)
        {
            int index = imgFile.FileName.LastIndexOf('.');
            string extension = imgFile.FileName.Substring(index, imgFile.FileName.Length - index);//获取后缀名
            string webpath = _webHostEnvironment.ContentRootPath;//网站根路径
            string guid = Guid.NewGuid().ToString().Replace("-", "");//生成guid
            string newFileName = guid + extension;
            DateTime dateTime = DateTime.Now;
            string datePath = string.Format(@"\{0}\{1}\{2}\", dateTime.Year, dateTime.Month, dateTime.Day);//路径日期部分
            string fullPath = string.Format(@"{0}\TempFile{1}", webpath, datePath);//全路径
            string imgSrc = DirectoryHelper.CreateDirectory(fullPath) + newFileName;
            return (newFileName, datePath, imgSrc);
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