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
        public User User { get; private set; }
    }
}
