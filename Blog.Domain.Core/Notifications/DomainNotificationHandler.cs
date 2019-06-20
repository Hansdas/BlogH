using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain.Core
{
    /// <summary>
    /// 领域通知处理
    /// </summary>
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        /// <summary>
        /// 通知集合
        /// </summary>
        private IList<DomainNotification> domainNotificationList;
        public DomainNotificationHandler()
        {
            domainNotificationList = new List<DomainNotification>();
        }
        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            domainNotificationList.Add(notification);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 查询通知消息
        /// </summary>
        /// <returns></returns>
        public virtual IList<DomainNotification> GetDomainNotificationList()
        {
            return domainNotificationList;
        }
        /// <summary>
        /// 判断是否存在通知消息
        /// </summary>
        /// <returns></returns>
        public virtual bool AnyDomainNotification()
        {
            return GetDomainNotificationList().Any();
        }
        public void Dispose()
        {
            domainNotificationList = new List<DomainNotification>();
        }
    }
}
