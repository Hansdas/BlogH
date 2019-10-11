using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Blog.Domain;
using Microsoft.AspNetCore.Http;
using Blog.Application.ViewModel;
using Blog.Common;

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
    }
}