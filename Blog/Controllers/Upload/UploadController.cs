using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common;
using Blog.Domain.Core;
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
            return Upload(false);
        }
        public IActionResult UploadImage()
        {
            return Upload(true);
        }

        private JsonResult Upload(bool isArticle)
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile == null)
                return Json(new ReturnResult("1", "附件为null"));
            if (string.IsNullOrEmpty(imgFile.FileName))
                return Json(new ReturnResult("1", "附件名称为空"));
            int index = imgFile.FileName.LastIndexOf('.');
            //获取后缀名
            string extension = imgFile.FileName.Substring(index, imgFile.FileName.Length - index);
            string webpath = hostingEnvironment.ContentRootPath;
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            string newFileName = guid + extension;
            DateTime dateTime = DateTime.Now;
            string datePath = string.Format(@"\{0}\{1}\{2}\", dateTime.Year, dateTime.Month, dateTime.Day);
            string fullPath = string.Format(@"{0}\TempFile{1}", webpath, datePath);
            string imgSrc = DirectoryHelper.CreateDirectory(fullPath) + newFileName;
            using (FileStream fs = System.IO.File.Create(imgSrc))
            {

                imgFile.CopyTo(fs);
                MemoryStream memoryStream = CompressImage(fs);
                imgFile.CopyTo(memoryStream);
                fs.Flush();
            }
            string virtualPath = ConstantKey.STATIC_FILE + datePath + newFileName;
            //使用虚拟静态资源路径，否则无法读取到图片
            if (isArticle)
                return Json(new { Code = "0", Msg = "", Data = new { Src = virtualPath, Title = imgFile.FileName } });
            return Json(new ReturnResult("0", "", imgSrc));
        }
        private MemoryStream CompressImage(Stream stream)
        {
            //Image image = Image.FromStream(stream);
            using (Image image = Image.FromStream(stream))
            {
                Size size = new Size(200, 200);
                int sourceWidth = image.Width;
                //获取图片高度
                int sourceHeight = image.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;
                //计算宽度的缩放比例
                nPercentW = ((float)size.Width / (float)sourceWidth);
                //计算高度的缩放比例
                nPercentH = ((float)size.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;
                //期望的宽度
                int destWidth = (int)(sourceWidth * nPercent);
                //期望的高度
                int destHeight = (int)(sourceHeight * nPercent);
                using (Bitmap bitmap = new Bitmap(destWidth, destHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //绘制图像
                        graphics.DrawImage(image, 0, 0, 200, 200);
                        MemoryStream memoryStream = new MemoryStream();
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        graphics.Dispose();
                        return memoryStream;
                    }
                }
            }
        }
    }
}