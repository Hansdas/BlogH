using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class DeleteWhisperCommand:Command
    {
        public int Id { get; private set; }
        public DeleteWhisperCommand(int id)
        {
            Id = id;
        }
    }
}
