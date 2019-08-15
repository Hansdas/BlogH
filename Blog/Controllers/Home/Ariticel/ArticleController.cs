using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Attr;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.Home.Ariticel
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //[PermissionFilter]
        public IActionResult AddArticle()
        {
            try
            {
                string account = Request.Form["type"];
            }
            catch (Exception)
            {

            }
            return View();
        }
    }
}