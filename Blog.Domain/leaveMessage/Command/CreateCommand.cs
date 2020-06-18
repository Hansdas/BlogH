using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class CreateCommand:Command
    {
        public CreateCommand(leaveMessage leaveMessage)
        {
            LeaveMessage = leaveMessage;
        }
        public leaveMessage LeaveMessage { get; private set; }
    }
}
