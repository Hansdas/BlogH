using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class CreateBlogCommand:Command
    {
        public Blog Blog { get; private set; }
        public CreateBlogCommand(Blog blog)
        {
            Blog = blog;
        }
    }
}
