using Blog.Domain.Core.Bus;
using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text; 

namespace Blog.Domain
{
    public class TidingsCommandHandler : ICommandHandler<DoneTidingsCommand>
    {
        private ITidingsRepository _tidingsRepository;
        private IEventBus _eventBus;
        public TidingsCommandHandler(ITidingsRepository tidingsRepository, IEventBus eventBus)
        {
            _tidingsRepository = tidingsRepository;
            _eventBus = eventBus;
        }
        public void Handler(DoneTidingsCommand command)
        {
            _tidingsRepository.Done(command.Id);
            Tidings tidings = _tidingsRepository.SelectById(command.Id);
            _eventBus.RaiseEventAsync(new DoneEvent(tidings.ReviceUser));
        }
    }
}
