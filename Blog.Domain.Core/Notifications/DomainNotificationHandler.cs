using Blog.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain.Core.Notifications
{
    /// <summary>
    /// 领域通知处理
    /// </summary>
    public class DomainNotificationHandler : INoticficationHandler<DomainNotification>
    {
        /// <summary>
        /// 通知集合
        /// </summary>
        private IList<DomainNotification> domainNotificationList;
        public DomainNotificationHandler()
        {
             domainNotificationList = new List<DomainNotification>();
        }
        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="notification"></param>
        public void Handler(DomainNotification notification)
        {
            domainNotificationList.Add(notification);
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
