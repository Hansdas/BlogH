//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Blog.Application;
//using Blog.Application.ViewModel;
//using Blog.Domain;
//using Microsoft.AspNetCore.Mvc;

//namespace Blog.Controllers
//{
//    public class WhisperController : BaseController
//    {
//        private readonly IBlogService _blogService;
//        private readonly IBlogRepository  _blogRepository;
//        public WhisperController(IBlogService  blogService, IBlogRepository blogRepository)
//        {
//            _blogService = blogService;
//            _blogRepository = blogRepository;
//        }
//        public IActionResult Index()
//        {
              
//            return View();
//        }
//        [HttpGet]
//        public IActionResult LoadWhisper(int pageIndex, int pageSize)
//        {
//            IList<BlogModel>  blogs = _blogService.GetBlogModels(pageIndex, pageSize);
//            PageResult pageResult = new PageResult();
//            pageResult.Data = blogs;
//            pageResult.Code = "200";
//            pageResult.Message = "ok";
//            return new JsonResult(pageResult);
//        }
//        public int LoadTotal()
//        {
//            int total = _blogRepository.SelectCount();
//            return total;
//        }
//        public IActionResult AddWhisper()
//        {
//            return View();
//        }
//    }
//}