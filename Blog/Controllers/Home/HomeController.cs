using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.Home
{
    public class HomeController : BaseController
    {
      public IActionResult Index()
        {           
            return View();
        }
    }
}