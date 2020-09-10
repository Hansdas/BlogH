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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventBus _eventBus;
        /// <summary>
        /// 根据UserModel转实体
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private User TransferModel(UserModel userModel)
        {
            Sex sex = string.IsNullOrEmpty(userModel.Sex) ? Sex.男 : Enum.Parse<Sex>(userModel.Sex);
            DateTime? birthDate = null;
            if (!string.IsNullOrEmpty(userModel.BirthDate))
                birthDate = Convert.ToDateTime(userModel.BirthDate);
            User user = new User(userModel.Username
                , userModel.Account
                , userModel.Password
                , sex
                , userModel.IsValid
                , birthDate
                , userModel.Email
                , userModel.Sign
                ,userModel.Phone
                ,userModel.HeadPhoto);
            return user;
        }
        /// <summary>
        /// 根据实体转model
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private UserModel TransferUser(User user)
        {
            UserModel userModel = new UserModel();
            userModel.Account = user.Account;
            userModel.BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value.ToString("yyyy-MM-dd") : "";
            userModel.Email = string.IsNullOrEmpty(user.Email)?"":user.Email;
            userModel.HeadPhoto = user.HeadPhoto;
            userModel.Phone = user.Phone;
            userModel.Sex = user.Sex.GetEnumText();
            userModel.Sign = string.IsNullOrEmpty(user.Sign)?"":user.Sign;
            userModel.Username = user.Username;
            return userModel;
        }
        public UserService(IUserRepository userRepository, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _eventBus = eventBus;
        }
        public void Insert(UserModel userModel)
        {
            userModel.Password = EncrypUtil.MD5Encry(userModel.Password);
            var command = new CreateUserCommand(TransferModel(userModel));
            _eventBus.Publish(command);
        }

        public User SelectUser(string Account, string password)
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
            var command = new UpdateUserCommand(TransferModel(userModel));
            _eventBus.Publish(command);
        }
        public void UpdatePassword(string account, string password, string oldPassword)
        {
            UserModel userModel = new UserModel();
            userModel.Password = password;
            userModel.Account = account;
            var command = new UpdateUserCommand(TransferModel(userModel), EncrypUtil.MD5Encry(oldPassword));
            _eventBus.Publish(command);
        }

        public UserModel SelectUser(string Account)
        {
            User user = _userRepository.SelectUserByAccount(Account);
            return TransferUser(user);
        }
    }
}
