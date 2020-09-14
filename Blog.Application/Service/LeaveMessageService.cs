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
        private ILeaveMessageRespository _leaveMessageRespository;
        public LeaveMessageService(IEventBus eventBus, ILeaveMessageRespository leaveMessageRespository)
        {
            _eventBus = eventBus;
            _leaveMessageRespository = leaveMessageRespository;
        }
        public void Add(LeaveMessageModel model)
        {
            leaveMessage leaveMessage = new leaveMessage(model.Content,model.ContractEmail,false);
            CreateCommand createCommand = new CreateCommand(leaveMessage);
            _eventBus.Publish(createCommand);
        }

        public IList<LeaveMessageModel> SelectByPage(int pageIndex, int pageSize)
        {
            IEnumerable<leaveMessage> leaveMessages = _leaveMessageRespository.SelectByPage(pageIndex, pageSize);
            IList<LeaveMessageModel> leaveMessageModels = new List<LeaveMessageModel>();
            foreach(var item in leaveMessages)
            {
                LeaveMessageModel leaveMessageModel = new LeaveMessageModel();
                leaveMessageModel.Content = item.Content;
                leaveMessageModel.CreateTime = item.CreateTime.HasValue ? item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm") : "";
                leaveMessageModels.Add(leaveMessageModel);
            }
            return leaveMessageModels;
        }

        public int SelectCount()
        {
            return _leaveMessageRespository.SelectCount();
        }
    }
}
