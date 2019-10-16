using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 更新User命令
    /// </summary>
  public  class UpdateUserCommand:Command
    {
        public UpdateUserCommand(User user)
        {
            User = user;
        }
        public UpdateUserCommand(User user,string oldPassowrd)
        {
            User = user;
            OldPassword = oldPassowrd;
        }
        public User User { get; private set; }
        public string OldPassword { get; private set; }
    }
}
