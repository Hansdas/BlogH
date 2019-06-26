using Blog.Domain;
using Blog.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public BlogService(IBlogRepository blogRepository,IMediatorHandler mediatorHandler)
        {
            _blogRepository = blogRepository;
            _mediatorHandler = mediatorHandler;
        }
        public void PublishBlog(Domain.Blog blog)
        {
            try
            {
                var command = new CreateBlogCommand(blog);
                _mediatorHandler.SendCommand(command);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
