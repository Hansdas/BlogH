using Blog.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 评论触发事件处理
    /// </summary>
    public class ReviewEventHandler : INoticficationHandler<ReviewEvent>
    {  
        //public ReviewEvent()
        //{

        //}
        /// <summary>
        /// 触发评论事件
        /// </summary>
        /// <param name="notification"></param>
        public void Handler(ReviewEvent notification)
        {
            throw new NotImplementedException();
        }
    }
}
