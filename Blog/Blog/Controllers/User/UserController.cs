using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResultStatu;

namespace Blog.Controllers.User
{
    public class UserController : Controller
    {
        public IActionResult UserInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update()
        {
            return Json(new ReturnResult() { IsSuccess = true, Message = "更新成功" });
        }
    }
}