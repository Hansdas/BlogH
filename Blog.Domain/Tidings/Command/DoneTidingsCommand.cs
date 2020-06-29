using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class DoneTidingsCommand:Command
    {
        public int Id { get; private set; }
        public DoneTidingsCommand(int id)
        {
            Id = id;
        }
    }
}
