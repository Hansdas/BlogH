using Blog.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.IService
{
   public interface ILeaveMessageService
    {
        void Add(LeaveMessageModel model);
        IList<LeaveMessageModel> SelectByPage(int pageIndex, int pageSize);
        int SelectCount();
    }
}
