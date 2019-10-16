﻿using Blog.Common;
using Blog.Domain.Core;
using Blog.Domain.Core.Bus;
using MediatR;
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
    public class UserCommandHandler : IRequestHandler<CreateUserCommand, Unit>,IRequestHandler<UpdateUserCommand,Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public UserCommandHandler(IUserRepository userRepository, IMediatorHandler mediatorHandler)
        {
            _userRepository = userRepository;
            _mediatorHandler = mediatorHandler;
        }
        public Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            int count=_userRepository.SelectCountByAccount(command.User.Account);
            if (count > 0)
            {
                _mediatorHandler.RaiseEvent(new DomainNotification(ConstantKey.CHECK_KEY, "该账号已存在"));
                return Task.FromResult(new Unit());
            }
            _userRepository.Insert(command.User);
            return Task.FromResult(new Unit());
        }

        public Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(command.OldPassword))
            {
                string password = _userRepository.SelectPassword(command.User.Account);
                if (password != command.OldPassword)
                {
                    _mediatorHandler.RaiseEvent(new DomainNotification(ConstantKey.CHECK_KEY, "原始密码错误"));
                    return Task.FromResult(new Unit());
                }
            }
            else
            {
                User user = _userRepository.SelectUserByAccount(command.User.Account);
                if (user == null)
                {
                    _mediatorHandler.RaiseEvent(new DomainNotification(ConstantKey.CHECK_KEY, "不存在用户账号：" + command.User.Account));
                    return Task.FromResult(new Unit());
                }
            }
            _userRepository.UpdateUser(command.User);
            return Task.FromResult(new Unit());
        }
    }
}
