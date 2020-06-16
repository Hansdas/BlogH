using Blog.Common.CacheFactory;
using Blog.Domain.Core;
using Blog.Domain.Core.Bus;
using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain
{
    /// <summary>
    /// 文章命令处理程序
    /// </summary>
    public class ArticleCommandHandler : ICommandHandler<CreateArticleCommand>, ICommandHandler<UpdateArticleCommand>,ICommandHandler<PraiseArticleCommand>
    {
        private  IArticleRepository _articleRepository;
        private IEventBus _eventBus;
        private ICacheClient _cacheClient;
        public ArticleCommandHandler(IArticleRepository articleRepository, IEventBus eventBus,ICacheClient cacheClient)
        {
            _articleRepository = articleRepository;
            _eventBus = eventBus;
            _cacheClient = cacheClient;
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
                Comment comment = new Comment(command.Comment.Guid, 
                    command.Comment.Content,
                    command.Comment.CommentType,
                    command.Comment.PostUser,
                    command.Comment.RevicerUser, 
                    command.Comment.AdditionalData);

                IList<string> commentIds = _articleRepository.SelectCommentIds(command.Id);
                commentIds.Add(comment.Guid);
                _articleRepository.Review(commentIds,comment, command.Id);
                Task.Run(() => {
                    ReviewEvent reviewEvent = new ReviewEvent(comment, command.Id);
                    _eventBus.RaiseEvent(reviewEvent);
                });
                //消息
              
            }
            else
                _articleRepository.Update(command.Article);
        }

        public void Handler(PraiseArticleCommand command)
        {
            string key = "article_" + command.Id;
            if (!command.Cancle)
            {
                string[] members = _cacheClient.GetMembers(key);
                if (members.Contains(command.Account))
                {
                    _eventBus.RaiseEvent(new NotifyValidation("已点过赞，确定取消点赞？"));
                    return;
                }
                _cacheClient.AddSet(key, command.Account);
                _articleRepository.Praise(command.Id,false);
            }
            else
            {
                _cacheClient.SetRemove(key,command.Account);
                _articleRepository.Praise(command.Id, true);

            }

        }

    }
}
