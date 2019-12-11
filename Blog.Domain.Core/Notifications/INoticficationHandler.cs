using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core.Notifications
{
    /// <summary>
    /// 标记领域通知
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    public interface INoticficationHandler<TNotification>
    {
        void Handler(TNotification notification);
    }
}
