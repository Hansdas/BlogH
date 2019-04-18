using Blog.Attr;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [PermissionFilter]
    public class BaseController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}