using Blog.Domain;
using Blog.Infrastruct;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastruct
{
   public class UserRepository: Repository<User,int>,IUserRepository
    {
    }
}
