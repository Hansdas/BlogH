using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface ILeaveMessageRespository:IRepository
    {
        void Insert(leaveMessage leaveMessage);
        IEnumerable<leaveMessage> SelectByPage(int currentPage,int pageSize);
        int SelectCount();
    }
}
