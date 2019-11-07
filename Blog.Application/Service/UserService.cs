using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Domain;
using Blog.Domain.Core;
using Blog.Domain.Core.Bus;

namespace Blog.Application
{
    public class UserService :IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventBus _eventBus;
        public UserService(IUserRepository userRepository, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _eventBus = eventBus;
        }
        public void Insert(User user)
        {
            var command = new CreateUserCommand(user);
            _eventBus.Publish(command);
        }

        public User SelectUser(string Account,string password)
        {
            User user = _userRepository.SelectUserByAccount(Account);
            if (user == null)
                throw new ValidationException("用户名不存在或密码错误");
            if (user.Password != EncrypUtil.MD5Encry(password))
                throw new ValidationException("用户名不存在或密码错误");
            return user;
        }

        public void Update(UserModel userModel)
        {
            DateTime? birthDate = null;
            if (!string.IsNullOrEmpty(userModel.BirthDate))
                birthDate = Convert.ToDateTime(userModel.BirthDate);
            User newUser = new User(userModel.Username, userModel.Account, "", Enum.Parse<Sex>(userModel.Sex),false, userModel.Email
              , userModel.Phone, birthDate, userModel.Sign, DateTime.Now);
            var command = new UpdateUserCommand(newUser);
            _eventBus.Publish(command);
        }
        public void UpdatePassword(string account, string password,string oldPassword)
        {
            User user = new User(account, EncrypUtil.MD5Encry(password));
            var command = new UpdateUserCommand(user, EncrypUtil.MD5Encry(oldPassword));
            _eventBus.Publish(command);
        }
    }
}
