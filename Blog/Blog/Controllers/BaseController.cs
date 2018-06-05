using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}