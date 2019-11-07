using Blog.Domain.Core.Notifications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core.Event
{
  public static class DependencyExtensions
    {
        private static ConcurrentDictionary<Type, IList<Type>> eventHandlerMapping = new ConcurrentDictionary<Type, IList<Type>>();
        private static ConcurrentDictionary<Type, IList<Type>> notificationMapping = new ConcurrentDictionary<Type, IList<Type>>();
        public static IList<Type> TryOrGetEventHandlerMapping(this Type eventType)
        {
           return eventHandlerMapping.GetOrAdd(eventType,(Type type)=>new List<Type>());
        }
        public static IList<Type> TryOrGetNotificationMapping(this Type eventType)
        {
            return notificationMapping.GetOrAdd(eventType, (Type type) => new List<Type>());
        }
        /// <summary>
        /// 注册事件总线
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceDescriptors"></param>
        public static void AddEventDependency<TImplementation, TService>(this IServiceCollection serviceDescriptors)
            where TImplementation:IEventHandler
            where TService:EventData
        {
            Type handler = typeof(TImplementation);
            Type @interface = handler.GetInterface("IEventHandler`1");
            serviceDescriptors.AddTransient(@interface, handler);
            TryOrGetEventHandlerMapping(typeof(TService)).Add(handler);
        }
        /// <summary>
        /// 注册领域通知
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceDescriptors"></param>
        public static void AddNoticficationDependency<TImplementation, TService>(this IServiceCollection serviceDescriptors)
             where TImplementation : INoticficationHandler
            where TService : DomainNotification
        {
            Type handler = typeof(TImplementation);
            Type @interface = handler.GetInterface("INoticficationHandler`1");
            serviceDescriptors.AddTransient(@interface, handler);
            TryOrGetEventHandlerMapping(typeof(TService)).Add(handler);
        }
    }
}
