using System;
using System.Collections.Generic;
using System.Text;
using Blog.Common;
using Blog.Domain.Core.Event;
using Blog.Domain.Core.Notifications;

namespace Blog.Domain.Core.Bus
{
    /// <summary>
    /// 领域中间件
    /// </summary>
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
                throw new ServiceException("事件总线未注册：" + typeof(TEventData).Name);
            foreach (var type in types)
            {
                object obj = _serviceProvider.GetService(type);
                if(type.IsAssignableFrom(obj.GetType()))
                {
                    IEventHandler<TEventData> handler = obj as IEventHandler<TEventData>;
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
