using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class WhisperController : BaseController
    {
        public IActionResult Index()
        {
              
            return View();
        }
        [HttpGet]
        //public IActionResult LoadWhisper()
        //{
        //    r
        //}
        public IActionResult AddWhisper()
        {
            return View();
        }
    }
}