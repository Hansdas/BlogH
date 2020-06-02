﻿using Blog.Domain.Core.Notifications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blog.Domain.Core.Event
{
    /// <summary>
    /// 领域注册类
    /// </summary>
  public static class AddServices
    {
        private static ConcurrentDictionary<Type, IList<Type>> handlerMapping = new ConcurrentDictionary<Type, IList<Type>>();
        /// <summary>
        /// 临时存储类型数组
        /// </summary>
        private static Type[] serviceTypes = Assembly.Load("Blog.Domain").GetTypes();
        /// <summary>
        /// 加载完成时释放serviceTypes
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        public static void DisposeServiceTypes(this IServiceCollection serviceDescriptors)
        {
            serviceTypes = null;
        }
        public static IList<Type> GetOrAddHandlerMapping(this Type eventType)
        {
           return handlerMapping.GetOrAdd(eventType,(Type type)=>new List<Type>());
        }
        /// <summary>
        /// 注册事件总线
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceDescriptors"></param>
        public static void AddEventBus<TImplementation, TService>(this IServiceCollection serviceDescriptors)
        {
            Type handler = typeof(TImplementation);
            Type serviceType = serviceTypes.FirstOrDefault(s => handler.IsAssignableFrom(s));
            if (serviceType == null)
                throw new ArgumentNullException(string.Format("类型{0}未找到实现类", handler.FullName));
            serviceDescriptors.AddTransient(handler, serviceType);
            GetOrAddHandlerMapping(typeof(TService)).Add(handler);
        }
        /// <summary>
        /// 注册验证错误通知
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceDescriptors"></param>
        public static void AddNotifyValidation(this IServiceCollection serviceDescriptors)
        {
            Type serviceType = typeof(NotifyValidationHandler);
            Type handler = typeof(IEventHandler<NotifyValidation>);
            serviceDescriptors.AddScoped(handler, serviceType);
            GetOrAddHandlerMapping(typeof(NotifyValidation)).Add(handler);
        }
    }
}
