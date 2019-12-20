using Blog.Application.ViewMode;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.IService
{
   public interface ITidingsService
    {
        int SelectCountByAccount(string account);

        IList<TidingsModel> SelectByPage(int pageIndex, int pageSize, TidingsCondition TidingsCondition = null);
    }
}
