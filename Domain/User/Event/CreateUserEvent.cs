using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class CreateUserEvent:Event
    {
        public CreateUserEvent(User user)
        {
            User = user;
        }
        public User User { get; private set; }
    }
}
