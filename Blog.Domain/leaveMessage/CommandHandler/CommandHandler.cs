using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public class CommandHandler : ICommandHandler<CreateCommand>
    {
        private ILeaveMessageRespository _leaveMessageRespository;
        public CommandHandler(ILeaveMessageRespository leaveMessageRespository)
        {
            _leaveMessageRespository = leaveMessageRespository;
        }
        public void Handler(CreateCommand command)
        {
            _leaveMessageRespository.Insert(command.LeaveMessage);
        }
    }
}
