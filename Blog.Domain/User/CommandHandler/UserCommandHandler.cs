using Blog.Common;
using Blog.Domain.Core;
using Blog.Domain.Core.Bus;
using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain
{
    /// <summary>
    /// 用户命令处理程序
    /// </summary>
    public class UserCommandHandler : ICommandHandler<CreateUserCommand>, ICommandHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventBus _eventBus;
        public UserCommandHandler(IUserRepository userRepository, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _eventBus = eventBus;
        }

        public void Handler(CreateUserCommand command)
        {
            int count = _userRepository.SelectCountByAccount(command.User.Account);
            if (count > 0)
            {
                _eventBus.RaiseEvent(new DomainNotification("该账号已存在"));
                return;
            }
            _userRepository.Insert(command.User);
        }

        public void Handler(UpdateUserCommand command)
        {
            if (!string.IsNullOrEmpty(command.OldPassword))
            {
                string password = _userRepository.SelectPassword(command.User.Account);
                if (password != command.OldPassword)
                {
                    _eventBus.RaiseEvent(new DomainNotification("原始密码错误"));
                    return;
                }
                _userRepository.UpdatePassword(command.User.Account, command.User.Password);
            }
            else
            {
                User user = _userRepository.SelectUserByAccount(command.User.Account);
                if (user == null)
                {
                    _eventBus.RaiseEvent(new DomainNotification("不存在用户账号：" + command.User.Account));
                    return;
                }
                _userRepository.UpdateUser(command.User);
            }
        }
    }
}
