using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain.Core
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
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
        public virtual IList<DomainNotification> GetDomainNotificationList()
        {
            return domainNotificationList;
        }
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
