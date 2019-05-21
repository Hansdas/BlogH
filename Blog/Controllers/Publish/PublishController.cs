using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.Add
{
    public class PublishController : Controller
    {
        public IActionResult Publish()
        {
            return View();
        }
    }
}