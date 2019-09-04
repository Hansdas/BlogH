using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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

    public class UploadController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [Consumes("multipart/form-data")]
        public IActionResult UploadPhoto()
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile == null)
                return Json(new ReturnResult("1", "附件为null"));
            if (string.IsNullOrEmpty(imgFile.FileName))
                return Json(new ReturnResult("1", "附件名称为空"));
            var value =CombinePath(imgFile);
            using (FileStream fs = System.IO.File.Create(value.imgSrc))
            {
                imgFile.CopyTo(fs);
                fs.Flush();
            }
            return Json(new ReturnResult("0", "", value.imgSrc));
        }

        [Consumes("multipart/form-data")]
        public IActionResult UploadImage()
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile == null)
                return Json(new ReturnResult("1", "附件为null"));
            if (string.IsNullOrEmpty(imgFile.FileName))
                return Json(new ReturnResult("1", "附件名称为空"));
            var value=  CombinePath(imgFile);
            using (FileStream fs = System.IO.File.Create(value.imgSrc))
            {
                using (Stream stream = CompressImage(imgFile.OpenReadStream()))
                {
                    byte[] srcBuf = StreamToBytes(stream);

                    fs.Write(srcBuf, 0, srcBuf.Length);
                    fs.Flush();
                }
            }
            //使用虚拟静态资源路径，否则无法读取到图片
            string virtualPath = ConstantKey.STATIC_FILE + value.datePath + value.newFileName;
            return Json(new { Code = "0", Msg = "", Data = new { Src = virtualPath, Title = imgFile.FileName } });
        }
         /// <summary>
         /// 组合图片保存路径
         /// </summary>
         /// <param name="imgFile"></param>
         /// <returns></returns>
        private (string newFileName,string datePath, string imgSrc) CombinePath(IFormFile imgFile)
        {
            int index = imgFile.FileName.LastIndexOf('.');
            //获取后缀名
            string extension = imgFile.FileName.Substring(index, imgFile.FileName.Length - index);
            string webpath = _webHostEnvironment.ContentRootPath;
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            string newFileName = guid + extension;
            DateTime dateTime = DateTime.Now;
            string datePath = string.Format(@"\{0}\{1}\{2}\", dateTime.Year, dateTime.Month, dateTime.Day);
            string fullPath = string.Format(@"{0}\TempFile{1}", webpath, datePath);
            string imgSrc = DirectoryHelper.CreateDirectory(fullPath) + newFileName;
            return (newFileName, datePath, imgSrc);
        }
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
        private MemoryStream CompressImage(Stream stream)
        {
            using (Image image = Image.FromStream(stream))
            {
                int sourceWidth = image.Width;
                //获取图片高度
                int sourceHeight = image.Height;
                Size size = default(Size);
                if (image.Width > 1200||image.Width>1200)
                    size = new Size(230, 200);
                else
                    size = new Size(sourceWidth, sourceHeight);
                float nPercentW = 0;
                float nPercentH = 0;
                //计算宽度的缩放比例
                nPercentW = ((float)size.Width / (float)sourceWidth);
                //计算高度的缩放比例
                nPercentH = ((float)size.Height / (float)sourceHeight);

                //期望的宽度
                int destWidth = (int)(sourceWidth * nPercentW);
                //期望的高度
                int destHeight = (int)(sourceHeight * nPercentH);
                using (Bitmap bitmap = new Bitmap(destWidth, destHeight))
                {

                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //绘制图像
                        graphics.DrawImage(image, 0, 0, destWidth, destHeight);
                        graphics.Dispose();
                        MemoryStream memoryStream = new MemoryStream();
                        if(image.RawFormat== ImageFormat.Jpeg)
                            bitmap.Save(memoryStream, ImageFormat.Jpeg);
                        else
                            bitmap.Save(memoryStream, ImageFormat.Png);
                        return memoryStream;
                    }
                }
            }
        }
        public IActionResult DeleteFile(string imgpath)
        {
            int index = imgpath.IndexOf(ConstantKey.STATIC_FILE)+ConstantKey.STATIC_FILE.Length;
            string path = _webHostEnvironment.ContentRootPath+ "/TempFile" + imgpath.Substring(index);
            path = path.Replace("/",@"\");
            DirectoryHelper.Delete(path);
            return Json(new ReturnResult("0"));
        }
    }
}