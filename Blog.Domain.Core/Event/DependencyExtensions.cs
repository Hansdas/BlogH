using Blog.Domain.Core.Notifications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blog.Domain.Core.Event
{
  public static class DependencyExtensions
    {
        private static ConcurrentDictionary<Type, IList<Type>> eventHandlerMapping = new ConcurrentDictionary<Type, IList<Type>>();
        private static ConcurrentDictionary<Type, IList<Type>> notificationMapping = new ConcurrentDictionary<Type, IList<Type>>();
        /// <summary>
        /// 临时存储类型数组
        /// </summary>
        private static Type[] serviceTypes = Assembly.Load("Blog.Domain").GetTypes();
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
        public static void AddEventBus<TImplementation, TService>(this IServiceCollection serviceDescriptors)
            where TImplementation:IEventHandler
            where TService:EventData
        {
            Type handler = typeof(TImplementation);
             handler.GetInterfaces();
            Type serviceType = serviceTypes.FirstOrDefault(s => handler.IsAssignableFrom(s));
            if (serviceType == null)
                throw new ArgumentNullException(string.Format("类型{0}未找到实现类", handler.FullName));
            serviceDescriptors.AddTransient(handler, serviceType);
            TryOrGetEventHandlerMapping(typeof(TService)).Add(handler);
        }
        /// <summary>
        /// 注册领域通知
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceDescriptors"></param>
        public static void AddNoticfication<TNoticficationHandler, TService>(this IServiceCollection serviceDescriptors)
             where TNoticficationHandler : INoticficationHandler
            where TService : DomainNotification
        {
            Type handler = typeof(TNoticficationHandler);
            Type @interface = handler.GetInterface("INoticficationHandler`1");
            serviceDescriptors.AddScoped(@interface, handler);
            TryOrGetEventHandlerMapping(typeof(TService)).Add(handler);
        }
    }
}
