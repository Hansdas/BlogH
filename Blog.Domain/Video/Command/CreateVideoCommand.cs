using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class CreateVideoCommand:Command
    {
        public CreateVideoCommand(Video video)
        {
            Video = video;
        }
        public Video Video { get; private set; }
    }
}
