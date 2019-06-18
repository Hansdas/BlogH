using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Blog.Common;
using Blog.Domain;
using Blog.Domain.Core.Bus;

namespace Blog.Application
{
    public class UserService :IUserService
    {
       private readonly IUserRepository _userRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public UserService(IUserRepository userRepository, IMediatorHandler mediatorHandler)
        {
            _userRepository = userRepository;
            _mediatorHandler = mediatorHandler;
        }
        public void Insert(User user)
        {
            var command = new CreateUserCommand(user);
            try
            {
                Task task = _mediatorHandler.SendCommand(command);
                Task.WaitAll(task);
            }
            catch (AggregateException)
            {
                throw new FrameworkException("程序内部错误");
            }
        }

        public User SelectUserByAccount(string Account)
        {
            User user = _userRepository.SelectUserByAccount(Account);
            return user;
        }
    }
}
