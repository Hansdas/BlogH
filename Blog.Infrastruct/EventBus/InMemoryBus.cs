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
    /// <summary>
    /// 密封类，防止重写
    /// </summary>
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        public InMemoryBus(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// 发送邻域命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task SendCommand<T>(T command) where T : Command
        {
            try
            {
              return _mediator.Send(command);
            }
            catch (AggregateException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 发布邻域事件通知
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task RaiseEvent<T>(T @event) where T : Event
        {
            try
            {
                return _mediator.Publish(@event);
            }
            catch (AggregateException ex)
            {
                throw ex;
            }
        }
    }
}
