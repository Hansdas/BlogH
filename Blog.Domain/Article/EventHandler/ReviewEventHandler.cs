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
        private IArticleRepository _articleRepository;
        public ReviewEventHandler(ICommentRepository commentRepository, ITidingsRepository tidingsRepository, IArticleRepository articleRepository)
        {
            _commentRepository = commentRepository;
            _tidingsRepository = tidingsRepository;
            _articleRepository = articleRepository;
        }
        /// <summary>
        /// 触发评论事件
        /// </summary>
        /// <param name="notification"></param>
        public void Handler(ReviewEvent reviewEvent)
        {
            Tidings tidings = null;
            string url = "../article/detail?id=" + reviewEvent.ArticleId;
            if (string.IsNullOrEmpty(reviewEvent.Comment.ReplyGuid))//评论文章
            {
                Article article = _articleRepository.SelectById(reviewEvent.ArticleId);
                tidings = new Tidings(reviewEvent.Comment.Guid,reviewEvent.Comment.PostUser,reviewEvent.Comment.Content, article.Author, false, url, article.Title,DateTime.Now);
            }
            else//回复评论 
            {
                Comment comment = _commentRepository.SelectById(reviewEvent.Comment.ReplyGuid);//被评论的数据;
                tidings = new Tidings(reviewEvent.Comment.Guid, reviewEvent.Comment.PostUser, reviewEvent.Comment.Content.Substring(0, 200)
                    , comment.PostUser, false, url, comment.Content.Substring(0,200),DateTime.Now);
            }
           
            _tidingsRepository.Insert(tidings);
        }
    }
}
