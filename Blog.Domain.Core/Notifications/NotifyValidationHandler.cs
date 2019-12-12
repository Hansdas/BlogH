using Blog.Domain.Core.Event;
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
    /// 错误处理
    /// </summary>
    public class NotifyValidationHandler : IEventHandler<NotifyValidation>
    {
        /// <summary>
        /// 错误集合
        /// </summary>
        private IList<NotifyValidation> validationErrors;
        public NotifyValidationHandler()
        {
            validationErrors = new List<NotifyValidation>();
        }
        /// <summary>
        /// 查询错误消息
        /// </summary>
        /// <returns></returns>
        public virtual IList<NotifyValidation> GetErrorList()
        {
            return validationErrors;
        }
        /// <summary>
        /// 判断是否存在错误消息
        /// </summary>
        /// <returns></returns>
        public virtual bool AnyDomainNotification()
        {
            return GetErrorList().Any();
        }
        public void Dispose()
        {
            validationErrors = new List<NotifyValidation>();
        }
        /// <summary>
        /// 添加错误消息
        /// </summary>
        /// <param name="notification"></param>
        public void Handler(NotifyValidation eventData)
        {
            validationErrors.Add(eventData);
        }
    }
}
