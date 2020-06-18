using Blog.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.IService
{
   public interface ILeaveMessageService
    {
        void Add(LeaveMessageModel model);
    }
}
