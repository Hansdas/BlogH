using Domain.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ServiceSvc.IService
{
   public interface IUserServiceSvc
    {
        User GetSingleUser(string Account, string Password,out string message);
        User RegisterUser(string Account,string Password, out string message);
    }
}
