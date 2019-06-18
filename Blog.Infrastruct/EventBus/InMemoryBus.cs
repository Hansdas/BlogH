using Blog.Domain.Core;
using Blog.Domain.Core.Bus;
using Blog.Domain.Core.Event;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastruct.EventBus
{
    public class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        public InMemoryBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task RaiseEvent<T>(T @event) where T : Event
        {
            return _mediator.Publish(@event);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task SendCommand<T>(T command) where T : Command
        {
           Task task=  _mediator.Send(command);
            task.ContinueWith(s => { throw new Exception(s.Exception.Message); }, TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }
    }
}
