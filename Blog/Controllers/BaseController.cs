using Blog.Attr;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [GlobaExceptionFilter]
    public class BaseController : Controller
    {
      
    }
}