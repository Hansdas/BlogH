using Blog.Common.Socket;
using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public class TidingsEventHandler : IEventHandler<DoneEvent>
    {
        private ITidingsRepository _tidingsRepository;
        private ISingalrContent _singalrContent;
        public TidingsEventHandler( ITidingsRepository tidingsRepository,ISingalrContent singalrContent)
        {
            _tidingsRepository = tidingsRepository;
            _singalrContent = singalrContent;
        }
        public void Handler(DoneEvent eventData)
        {
            TidingsCondition condition = new TidingsCondition();
            condition.Account = eventData.Account;
            Message message = new Message();
            message.Data = _tidingsRepository.SelectCountByAccount(eventData.Account);
            _singalrContent.SendClientMessage(eventData.Account,message);
        }
    }
}
