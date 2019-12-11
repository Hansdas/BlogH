using Blog.Domain.Core.Event;
using Blog.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 评论触发事件处理
    /// </summary>
    public class ReviewEventHandler : IEventHandler<ReviewEvent>
    {
        private ICommentRepository _commentRepository;
        private ITidingsRepository  _tidingsRepository;
        public ReviewEventHandler(ICommentRepository commentRepository, ITidingsRepository tidingsRepository)
        {
            _commentRepository = commentRepository;
            _tidingsRepository = tidingsRepository;
        }
        /// <summary>
        /// 触发评论事件
        /// </summary>
        /// <param name="notification"></param>
        public void Handler(ReviewEvent reviewEvent)
        {
            string reviceUser = _commentRepository.SelectById(reviewEvent.Comment.ReplyGuid).PostUser;
            string url = "../article/detail?id=" + reviewEvent.ArticleId;
            Tidings tidings = new Tidings(reviewEvent.Comment.Guid, reviceUser, false, url, "");
            _tidingsRepository.Insert(tidings);
        }
    }
}
