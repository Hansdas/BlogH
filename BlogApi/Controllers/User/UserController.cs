using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Blog.Domain;
using Microsoft.AspNetCore.Http;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Domain.Core;

namespace BlogApi.Controllers.User
{

    [Route("blogh/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _context;
        public UserController(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }
        [HttpGet]
        public IActionResult UserInfo()
        {
            try
            {
                string json = new JWT(_context).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                return new JsonResult(new ReturnResult("200", userModel));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ReturnResult("500", ex.Message));
            }
        }
        [HttpPost]
        public IActionResult UpdateUser()
        {
            try
            {
                string account = Request.Form["account"];
                string username = Request.Form["username"];
                Sex sex = Enum.Parse<Sex>(Request.Form["sex"]);
                DateTime? birthDate;
                if (!string.IsNullOrEmpty(Request.Form["birthdate"]))
                    birthDate = Convert.ToDateTime(Request.Form["birthdate"]);
                string phone = Request.Form["phone"];
                string email = Request.Form["email"];
                string sign = Request.Form["sign"];
                string json = new JWT(_context).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                return new JsonResult(new ReturnResult("200", userModel));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ReturnResult("500", ex.Message));
            }
        }
    }
}