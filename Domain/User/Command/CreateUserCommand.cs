using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class CreateUserCommand: Command
    {
        public CreateUserCommand(User user)
        {
            User = user;
        }
        public User User { get; private set; }
    }
}
