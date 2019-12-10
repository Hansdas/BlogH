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
        private readonly IArticleRepository _articleRepository;
        public ArticleCommandHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
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
            }
            else
                _articleRepository.Update(command.Article);
        }
    }
}
