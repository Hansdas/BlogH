using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain.Core.Event;
using Blog.Domain.Core.Notifications;

namespace Blog.Domain.Core.Bus
{
    public sealed class EventBus : IEventBus
    {
        private IServiceProvider _serviceProvider;
        public EventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventData"></param>
        public void Publish<TEventData>(TEventData eventData) where TEventData : EventData
        {
            IList<Type> types=typeof(TEventData).TryOrGetEventHandlerMapping();
            if (types == null || types.Count == 0)
                return;
            foreach(var type in types)
            {
                Type @interface = type.GetInterface("IEventHandler`1");
                object obj = _serviceProvider.GetService(@interface);
                if(obj.GetType()==type)
                {
                    IEventHandler<EventData> handler = obj as IEventHandler<EventData>;
                    if (handler != null)
                        handler.Handler(eventData);
                }
            }
        }
         /// <summary>
         /// 发送通知
         /// </summary>
         /// <typeparam name="TNotification"></typeparam>
         /// <param name="notification"></param>
        public void Send<TNotification>(TNotification notification) where TNotification : DomainNotification
        {
            IList<Type> types = typeof(TNotification).TryOrGetNotificationMapping();
            if (types == null || types.Count == 0)
                return;
            foreach (var type in types)
            {
                Type @interface = type.GetInterface("INoticficationHandler`1");
                object obj = _serviceProvider.GetService(@interface);
                if (obj.GetType() == type)
                {
                    INoticficationHandler<TNotification> handler = obj as INoticficationHandler<TNotification>;
                    if (handler != null)
                        handler.Handler(notification);
                }
            }
        }
    }
}
