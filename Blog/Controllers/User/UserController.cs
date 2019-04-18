﻿using System;
using System.IO;
using Domain;
using FileHelper;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.User
{
    public class UserController : Controller
    {
        public UserController()
        {
            
        }
        public IActionResult UserInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update()
        {
            string userName = Request.Form["username"];
            string mobilephone = Request.Form["mobilephone"];
            string email = Request.Form["email"];
            string sex = Request.Form["sex"];
            string descript = Request.Form["descript"];
            var photoImage= Request.Form.Files["uploadImage"];
            string fileName=photoImage.FileName.Trim();
            try
            {                
                string path = DirectoryHelper.CreateDirectory() + fileName;
                using (Stream stream=System.IO.File.Create(path))
                {
                    photoImage.CopyToAsync(stream);                    
                    stream.Flush();
                }
                Domain.User user = new Domain.User();
            }
            catch (Exception e)
            {
                return Json(new ReturnResult() {Code="500", Message = e.Message });
            }
            return Json(new ReturnResult() { Code="200", Message = "更新成功" });
        }
    }
}