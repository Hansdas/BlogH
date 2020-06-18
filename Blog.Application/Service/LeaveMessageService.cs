using Blog.Application.IService;
using Blog.Application.ViewModel;
using Blog.Domain;
using Blog.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Service
{
    public class LeaveMessageService : ILeaveMessageService
    {
        private IEventBus _eventBus;
        public LeaveMessageService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public void Add(LeaveMessageModel model)
        {
            leaveMessage leaveMessage = new leaveMessage(model.Content,model.ContractEmail,false);
            CreateCommand createCommand = new CreateCommand(leaveMessage);
            _eventBus.Publish(createCommand);
        }
    }
}
