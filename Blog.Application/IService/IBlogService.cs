using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application
{
   public interface IBlogService
    {
        void PublishBlog(Domain.Blog blog);
    }
}
