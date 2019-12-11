using Blog.Domain.Core.Bus;
using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain
{
    /// <summary>
    /// 文章命令处理程序
    /// </summary>
    public class ArticleCommandHandler : ICommandHandler<CreateArticleCommand>, ICommandHandler<UpdateArticleCommand>
    {
        private  IArticleRepository _articleRepository;
        private IEventBus _eventBus;
        public ArticleCommandHandler(IArticleRepository articleRepository, IEventBus eventBus)
        {
            _articleRepository = articleRepository;
            _eventBus = eventBus;
        }

        /// <summary>
        /// 创建文章事件
        /// </summary>
        /// <param name="command"></param>
        public void Handler(CreateArticleCommand command)
        {
            _articleRepository.Insert(command.Article);
        }
        /// <summary>
        /// 更新文章事件
        /// </summary>
        /// <param name="command"></param>

        public void Handler(UpdateArticleCommand command)
        {
            if (command.Comment != null)
            {
                Comment comment = new Comment(command.Comment.Guid, command.Comment.Content, command.Comment.PostUser, command.Comment.ReplyGuid);

                IList<string> commentIds = _articleRepository.SelectCommentIds(command.Id);
                commentIds.Add(comment.Guid);
                _articleRepository.Review(commentIds,comment, command.Id);

                //消息
                ReviewEvent reviewEvent = new ReviewEvent(comment,command.Id);
                _eventBus.RaiseEvent(reviewEvent);
              
            }
            else
                _articleRepository.Update(command.Article);
        }
    }
}
