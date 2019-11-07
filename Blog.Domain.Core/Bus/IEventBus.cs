using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core.Bus
{
  public  interface  IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventData"></param>
        void Publish<TEventData>(TEventData eventData) where TEventData : EventData;
        /// <summary>
        /// 发送领域通知
        /// </summary>
        /// <typeparam name="TNotification"></typeparam>
        /// <param name="notification"></param>
        void Send<TNotification>(TNotification notification) where TNotification : DomainNotification;
    }
}
