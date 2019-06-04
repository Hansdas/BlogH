using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public IActionResult AddWhisper()
        {
            string content = Request.Form["content"];
            string imgSrc = Request.Form["imgSrc"];
            using (HttpClient client = new HttpClient())
            {
                //client.PostAsync("");
            }
                return View();
        }
    }
}