using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application;
using Blog.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class WhisperController : BaseController
    {
        private readonly IBlogRepository _blogRepository;
        public WhisperController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public IActionResult Index()
        {
              
            return View();
        }
        [HttpGet]
        public void LoadWhisper(int pageIndex, int pageSize)
        {
            int count;
            IEnumerable<Domain.Blog>  blogs = _blogRepository.SelectByPage(pageIndex, pageSize,out count);
            PageResult pageResult = new PageResult();
            pageResult.Total = count;
            IList<dynamic> returnList = new List<dynamic>();
            foreach(var item in blogs)
            {
                //dynamic d=;
                //d.Author = item.Account;
            }
        }
        public IActionResult AddWhisper()
        {
            return View();
        }
    }
}