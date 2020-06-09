using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class CreateNewsCommand:Command
    {
        public CreateNewsCommand(News news)
        {
            News = news;
        }
        public News News { get; private set; }
    }
}
