using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface ITidingsRepository
    {
        Tidings SelectById(int id);
        void Insert(Tidings tidings);

        int SelectCountByAccount(string account);

        IList<Tidings> SelectByPage(int pageIndex, int pageSize, TidingsCondition TidingsCondition = null);

        void Done(int id);
    }
}
