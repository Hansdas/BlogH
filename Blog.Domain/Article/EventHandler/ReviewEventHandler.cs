using Blog.Common.Socket;
using Blog.Domain.Core;
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
        private ISingalrContent _singalrContent;
        public ReviewEventHandler(ICommentRepository commentRepository, ITidingsRepository tidingsRepository, IArticleRepository articleRepository
            , ISingalrContent singalrContent)
        {
            _commentRepository = commentRepository;
            _tidingsRepository = tidingsRepository;
            _articleRepository = articleRepository;
            _singalrContent = singalrContent;
        }
        /// <summary>
        /// 触发评论事件
        /// </summary>
        /// <param name="notification"></param>
        public void Handler(ReviewEvent reviewEvent)
        {
            Tidings tidings = null;
            string url = "../article/detail.html?id=" + reviewEvent.ArticleId;
            if (reviewEvent.Comment.CommentType==CommentType.文章)//评论文章
            {
                Article article = _articleRepository.SelectById(reviewEvent.ArticleId);
                tidings = new Tidings(reviewEvent.Comment.Guid,reviewEvent.Comment.PostUser,reviewEvent.Comment.Content, article.Author, false, url, article.Title,DateTime.Now);
            }
            else//回复评论 
            {
                Comment comment = _commentRepository.SelectById(reviewEvent.Comment.AdditionalData);//被评论的数据;
                tidings = new Tidings(reviewEvent.Comment.Guid, reviewEvent.Comment.PostUser, reviewEvent.Comment.Content
                    , comment.PostUser, false, url, comment.Content,DateTime.Now);
            }           
            _tidingsRepository.Insert(tidings);
            int count = _tidingsRepository.SelectCountByAccount(reviewEvent.Comment.RevicerUser);
            Message message = new Message();
            message.Data = count;
            _singalrContent.SendClientMessage(reviewEvent.Comment.RevicerUser, message);
        }
    }
}
