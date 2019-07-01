using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class WhisperController : BaseController
    {
        private readonly IBlogService _blogService;
        public WhisperController(IBlogService  blogService)
        {
            _blogService = blogService;
        }
        public IActionResult Index()
        {
              
            return View();
        }
        [HttpGet]
        public IActionResult LoadWhisper(int pageIndex, int pageSize)
        {
            int count;
            IList<BlogModel>  blogs = _blogService.GetBlogModels(pageIndex, pageSize,out count);
            PageResult pageResult = new PageResult();
            pageResult.Total = count;
            pageResult.Data = blogs;
            pageResult.Code = "200";
            pageResult.Message = "ok";
            return new JsonResult(pageResult);
        }
        public IActionResult AddWhisper()
        {
            return View();
        }
    }
}