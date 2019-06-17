using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using Blog.Domain;

namespace Blog.Application
{
    public class UserService : IUserService
    {
       private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User Insert(User user)
        {
            user = _userRepository.Insert(user);
            return user; 
        }

        public User SelectSingle(Expression<Func<User, bool>> condition)
        {
            User user = _userRepository.SelectSingle(condition);
            return user;
        }
    }
}
