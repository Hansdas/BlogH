using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.IService
{
   public interface ITidingsService
    {
        int SelectCountByAccount(string account);
    }
}
