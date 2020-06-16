using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
  public  class WhisperCommentCommand:Command
    {
        public WhisperCommentCommand(Comment comment,int whisperId)
        {
            Comment = comment;
            WhisperId = whisperId;
        }
        public Comment Comment { get; private set; }
        public int WhisperId { get; private set; }
    }
}
