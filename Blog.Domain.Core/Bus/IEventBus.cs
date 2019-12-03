using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core.Bus
{
    /// <summary>
    /// 领域中间件
    /// </summary>
  public  interface  IEventBus
    {
        /// <summary>
        /// 发布领域命令
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventData"></param>
        void Publish<TCommand>(TCommand command) where TCommand : Command;
        /// <summary>
        /// 触发领域事件
        /// </summary>
        /// <typeparam name="TNotification"></typeparam>
        /// <param name="notification"></param>
        void RaiseEvent<TEventData>(TEventData eventData) where TEventData : EventData;
    }
}
