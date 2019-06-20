using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 创建用户领域命令
    /// </summary>
    public class CreateUserCommand: Command
    {
        public CreateUserCommand(User user)
        {
            User = user;
        }
        public User User { get; private set; }
    }
}
