using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core.Notifications
{
    /// <summary>
    /// 标记接口
    /// </summary>
    public interface INoticficationHandler
    {

    }
    /// <summary>
    /// 标记领域通知
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    public interface INoticficationHandler<TNotification>:INoticficationHandler where TNotification:DomainNotification
    {
        void Handler(TNotification notification);
    }
}
